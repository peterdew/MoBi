using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IBuildingBlockWithInitialConditionsPresenter : IExtendablePathAndValueBuildingBlockPresenter<InitialConditionDTO>
   {
      /// <summary>
      ///    Sets the Scale Divisor of the start value to the new value
      /// </summary>
      /// <param name="dto">The dto and start value being modified</param>
      /// <param name="newScaleDivisor">The new value of scale divisor</param>
      void SetScaleDivisor(InitialConditionDTO dto, double newScaleDivisor);

      /// <summary>
      ///    Sets the IsPresent property of the start value <paramref name="dto" /> to <paramref name="isPresent" />
      /// </summary>
      void SetIsPresent(InitialConditionDTO dto, bool isPresent);

      /// <summary>
      ///    Sets the NegativeValuesAllowed property of the start value <paramref name="dto" /> to
      ///    <paramref name="negativeValuesAllowed" />
      /// </summary>
      void SetNegativeValuesAllowed(InitialConditionDTO dto, bool negativeValuesAllowed);
   }

   public interface IBuildingBlockWithInitialConditionsPresenter<TBuildingBlock> : IBuildingBlockWithInitialConditionsPresenter, IEditPresenter<TBuildingBlock>
   {
   }

   public abstract class BuildingBlockWithInitialConditionsPresenter<TView, TPresenter, TBuildingBlock> : ExtendablePathAndValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, InitialConditionDTO, InitialCondition> where TView : IInitialConditionsView, IPathAndValueEntitiesView<InitialConditionDTO>, IView<TPresenter> where TPresenter : IPresenter where TBuildingBlock : class, IBuildingBlock<InitialCondition>
   {
      private readonly IInitialConditionsTask<TBuildingBlock> _initialConditionsTask;

      protected BuildingBlockWithInitialConditionsPresenter(TView view,
         IInitialConditionToInitialConditionDTOMapper dtoMapper,
         IInitialConditionsTask<TBuildingBlock> initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IDeletePathAndValueEntityPresenter deletePathAndValueEntityPresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IRefreshInitialConditionsPresenter refreshInitialConditionsPresenter,
         IMoleculeIsPresentSelectionPresenter isPresentSelectionPresenter,
         IMoleculeNegativeValuesAllowedSelectionPresenter negativeStartValuesAllowedSelectionPresenter, 
         IDistributedPathAndValueEntityPresenter<InitialConditionDTO, TBuildingBlock> distributedParameterPresenter) : 
         base(view, dtoMapper, initialConditionsTask, msvCreator, context, deletePathAndValueEntityPresenter, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _initialConditionsTask = initialConditionsTask;
         refreshInitialConditionsPresenter.ApplySelectionAction = performRefreshAction;
         _view.AddIsPresentSelectionView(isPresentSelectionPresenter.BaseView);
         _view.AddNegativeValuesAllowedSelectionView(negativeStartValuesAllowedSelectionPresenter.BaseView);
         _view.AddRefreshSelectionView(refreshInitialConditionsPresenter.BaseView);

         AddSubPresenters(refreshInitialConditionsPresenter);
         isPresentSelectionPresenter.ApplySelectionAction = performIsPresentAction;
         negativeStartValuesAllowedSelectionPresenter.ApplySelectionAction = performNegativeValuesAllowedAction;
      }

      private void performRefreshAction(SelectOption option)
      {
         if (option == SelectOption.RefreshSelected)
         {
            refreshInitialConditions(SelectedStartValues);
         }
         else if (option == SelectOption.RefreshAll)
         {
            refreshInitialConditions(VisibleStartValues);
         }
      }

      private void refreshInitialConditions(IEnumerable<InitialCondition> initialConditions)
      {
         AddCommand(
            _initialConditionsTask.RefreshInitialConditionsFromBuildingBlocks(
               _buildingBlock,
               initialConditions.ToList()));
      }

      protected override string RemoveCommandDescription() => AppConstants.Commands.RemoveMultipleInitialConditions;

      public void SetScaleDivisor(InitialConditionDTO dto, double newScaleDivisor) => AddCommand(() => _initialConditionsTask.UpdateInitialConditionScaleDivisor(_buildingBlock, dto.InitialCondition, newScaleDivisor, dto.InitialCondition.ScaleDivisor));

      public void SetIsPresent(InitialConditionDTO dto, bool isPresent) => setIsPresent(new[] { dto.InitialCondition }, isPresent);

      private void setIsPresent(IEnumerable<InitialCondition> startValuesToUpdate, bool isPresent)
      {
         AddCommand(() => _initialConditionsTask.SetIsPresent(_buildingBlock, startValuesToUpdate, isPresent));
         _view.RefreshData();
      }

      public void SetNegativeValuesAllowed(InitialConditionDTO dto, bool negativeValuesAllowed) => setNegativeValuesAllowed(new[] { dto.InitialCondition }, negativeValuesAllowed);

      protected void DisablePathColumns() => _view.DisablePathColumns();

      public void HideIsPresentColumn() => _view.HideIsPresentColumn();

      private void setNegativeValuesAllowed(IEnumerable<InitialCondition> startValuesToUpdate, bool negativeValuesAllowed)
      {
         AddCommand(() => _initialConditionsTask.SetNegativeValuesAllowed(_buildingBlock, startValuesToUpdate, negativeValuesAllowed));
         _view.RefreshData();
      }

      private void performIsPresentAction(SelectOption option) => performSetFlagValueAction(setIsPresent, option);

      private void performNegativeValuesAllowedAction(SelectOption option) => performSetFlagValueAction(setNegativeValuesAllowed, option);

      private void performSetFlagValueAction(Action<IEnumerable<InitialCondition>, bool> selectionAction, SelectOption option)
      {
         if (option.IsOneOf(SelectOption.AllPresent, SelectOption.AllNegativeValuesAllowed))
            selectionAction(VisibleStartValues, true);

         else if (option.IsOneOf(SelectOption.AllNotPresent, SelectOption.AllNegativeValuesNotAllowed))
            selectionAction(VisibleStartValues, false);

         else if (option.IsOneOf(SelectOption.SelectedPresent, SelectOption.SelectedNegativeValuesAllowed))
            selectionAction(SelectedStartValues, true);

         else if (option.IsOneOf(SelectOption.SelectedNotPresent, SelectOption.SelectedNegativeValuesNotAllowed))
            selectionAction(SelectedStartValues, false);
      }
   }
}