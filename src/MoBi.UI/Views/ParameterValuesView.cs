﻿using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.UI.Binders;

namespace MoBi.UI.Views
{
   public partial class ParameterValuesView : BasePathAndValueEntityView<ParameterValueDTO, ParameterValue>, IParameterValuesView
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly UxRepositoryItemComboBox _dimensionComboBoxRepository;

      public ParameterValuesView(IDimensionFactory dimensionFactory, ValueOriginBinder<ParameterValueDTO> valueOriginBinder):base(valueOriginBinder)
      {
         InitializeComponent();
         
         _dimensionFactory = dimensionFactory;
         _dimensionComboBoxRepository = new UxRepositoryItemComboBox(gridView);
      }

      public void AttachPresenter(IParameterValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      protected override void DoInitializeBinding()
      {
         base.DoInitializeBinding();

         _unitControl.ParameterUnitSet += setParameterUnit;

         _dimensionComboBoxRepository.FillComboBoxRepositoryWith(_dimensionFactory.DimensionsSortedByName);

         BindValueColumn(dto => dto.Value)
            .WithCaption(AppConstants.Captions.ParameterValue)
            .WithFormat(dto => dto.ParameterValueFormatter())
            .WithOnValueUpdating(onParameterValueSet);

         InitializeValueOriginBinding();

         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => parameterValuesPresenter.SetFormula(o, e.NewValue.Formula));

         _gridViewBinder.Bind(x => x.Dimension).WithRepository(x => _dimensionComboBoxRepository)
            .WithOnValueUpdating((o,e) => OnEvent(() => onDimensionSet(o,e)));
      }

      public override string NameColumnCaption => AppConstants.Captions.ParameterName;

      private void onDimensionSet(ParameterValueDTO parameterValueDTO, PropertyValueSetEventArgs<IDimension> propertyValueSetEventArgs)
      {
         parameterValuesPresenter.UpdateDimension(parameterValueDTO, propertyValueSetEventArgs.NewValue);
      }

      private void onParameterValueSet(ParameterValueDTO psv, PropertyValueSetEventArgs<double?> e)
      {
         OnEvent(() => parameterValuesPresenter.SetValue(psv, e.NewValue));
      }

      private void setParameterUnit(ParameterValueDTO parameterValue, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            parameterValuesPresenter.SetUnit(parameterValue, unit);
         });
      }

      private IParameterValuesPresenter parameterValuesPresenter => _presenter.DowncastTo<IParameterValuesPresenter>();
   }
}