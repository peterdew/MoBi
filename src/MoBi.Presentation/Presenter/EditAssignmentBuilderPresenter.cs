using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditAssignmentBuilderPresenter : IEditPresenter<EventAssignmentBuilder>,
      IPresenter<IEditEventAssignmentBuilderView>,
      IPresenterWithFormulaCache,
      ICanEditPropertiesPresenter,
      ICreatePresenter<EventAssignmentBuilder>
   {
      void SelectPath();
      IReadOnlyList<IDimension> AllDimensions();
      void SetDimension(IDimension dimension);
      void SetEventAssignmentPath(string newPath);
   }

   public class EditAssignmentBuilderPresenter : AbstractEntityEditPresenter<IEditEventAssignmentBuilderView, IEditAssignmentBuilderPresenter, EventAssignmentBuilder>, IEditAssignmentBuilderPresenter
   {
      private EventAssignmentBuilder _eventAssignmentBuilder;
      private readonly IEventAssignmentBuilderToEventAssignmentDTOMapper _eventAssignmentToDTOAssignmentMapper;
      private readonly IEditTaskFor<EventAssignmentBuilder> _editTasksForAssignment;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private readonly IEditFormulaPresenter _editFormulaPresenter;
      private readonly IMoBiContext _context;
      private readonly ISelectReferenceAtEventAssignmentPresenter _selectReferencePresenter;
      private readonly IContextSpecificReferencesRetriever _contextSpecificReferencesRetriever;
      private readonly IMoBiApplicationController _applicationController;
      private EventAssignmentBuilderDTO _eventAssignmentBuilderDTO;
      public IBuildingBlock BuildingBlock { get; set; }

      public EditAssignmentBuilderPresenter(
         IEditEventAssignmentBuilderView view,
         IEventAssignmentBuilderToEventAssignmentDTOMapper eventAssignmentToDTOAssignmentMapper,
         IEditTaskFor<EventAssignmentBuilder> editTasksForAssignment,
         IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper,
         IEditFormulaPresenter editFormulaPresenter,
         IMoBiContext context,
         ISelectReferenceAtEventAssignmentPresenter selectReferencePresenter,
         IContextSpecificReferencesRetriever contextSpecificReferencesRetriever,
         IMoBiApplicationController applicationController) : base(view)
      {
         _contextSpecificReferencesRetriever = contextSpecificReferencesRetriever;
         _applicationController = applicationController;
         _selectReferencePresenter = selectReferencePresenter;
         _context = context;
         _editFormulaPresenter = editFormulaPresenter;
         _editFormulaPresenter.ReferencePresenter = _selectReferencePresenter;
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _editTasksForAssignment = editTasksForAssignment;
         _view.SetFormulaView(_editFormulaPresenter.BaseView);
         _eventAssignmentToDTOAssignmentMapper = eventAssignmentToDTOAssignmentMapper;
         AddSubPresenters(_editFormulaPresenter, selectReferencePresenter);
      }

      public override void Edit(EventAssignmentBuilder eventAssignmentBuilder, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _eventAssignmentBuilder = eventAssignmentBuilder;
         _selectReferencePresenter.Init(eventAssignmentBuilder, _contextSpecificReferencesRetriever.RetrieveFor(_eventAssignmentBuilder), eventAssignmentBuilder);
         _eventAssignmentBuilderDTO = _eventAssignmentToDTOAssignmentMapper.MapFrom(_eventAssignmentBuilder);
         _eventAssignmentBuilderDTO.AddUsedNames(_editTasksForAssignment.GetForbiddenNamesWithoutSelf(eventAssignmentBuilder, existingObjectsInParent));
         bindToView();
      }

      private void bindToView()
      {
         _view.Show(_eventAssignmentBuilderDTO);
         bindToFormula();
      }

      private void bindToFormula()
      {
         _editFormulaPresenter.Init(_eventAssignmentBuilder, BuildingBlock);
      }

      public override object Subject => _eventAssignmentBuilder;

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache.MapAllUsing(_formulaToDTOFormulaMapper);
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _eventAssignmentBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasksForAssignment.Rename(_eventAssignmentBuilder, BuildingBlock);
      }

      public void SelectPath()
      {
         FormulaUsablePath objectPath;
         using (var presenter = _applicationController.Start<ISelectEventAssignmentTargetPresenter>())
         {
            presenter.Init(_eventAssignmentBuilder.RootContainer);
            objectPath = presenter.Select();
         }

         if (objectPath == null)
            return;

         setObjectPath(objectPath);
      }

      private void setObjectPath(FormulaUsablePath objectPath)
      {
         AddCommand(new SetEventAssignmentObjectPathCommand(_eventAssignmentBuilder, objectPath, BuildingBlock).Run(_context));
         _eventAssignmentBuilderDTO.ChangedEntityPath = _eventAssignmentBuilder.ObjectPath.ToString();
         bindToFormula();
      }

      private void setObjectPath(string path, IDimension dimension)
      {
         var objectPath = new FormulaUsablePath(path.ToPathArray()) {Dimension = dimension};
         setObjectPath(objectPath);
      }

      public void SetDimension(IDimension dimension) => setObjectPath(_eventAssignmentBuilder.ObjectPath ?? string.Empty, dimension);

      public void SetEventAssignmentPath(string newPath) => setObjectPath(newPath, _eventAssignmentBuilder.Dimension);

      public IReadOnlyList<IDimension> AllDimensions()
      {
         return _context.DimensionFactory.DimensionsSortedByName;
      }
   }
}