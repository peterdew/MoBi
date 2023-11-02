﻿using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using static OSPSuite.Assets.Captions;

namespace MoBi.UI.Views
{
   public partial class SelectSpatialStructureAndMoleculesView : BaseModalView, ISelectSpatialStructureAndMoleculesView
   {
      private ISelectSpatialStructureAndMoleculesPresenter _presenter;
      private ScreenBinder<SelectSpatialStructureAndMoleculesDTO> _screenBinder;
      private GridViewBinder<MoleculeSelectionDTO> _gridViewBinder;
      private readonly IImageListRetriever _imageListRetriever;
      private readonly string _selectedColumnName;

      public SelectSpatialStructureAndMoleculesView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _imageListRetriever = imageListRetriever;
         _selectedColumnName = nameof(MoleculeSelectionDTO.Selected);
         descriptionLabel.AsDescription();
         descriptionLabel.Text = AppConstants.Captions.ExtendDescription;
         _gridViewBinder = new GridViewBinder<MoleculeSelectionDTO>(gridView);

         // Disable "selected" appearance for rows. UxRepositoryItemImageComboBox do not have a "selected" appearance.
         Load += (o, e) => OnEvent(formLoad);
         gridView.RowStyle += (o, e) => OnEvent(() => gridViewRowStyle(e));
         gridView.SelectionChanged += (o, e) => OnEvent(gridViewSelectionChanged);
      }

      private void gridViewSelectionChanged()
      {
         // The select/unselect of a molecule will affect the validation of another molecule
         // with the same name. If both were selected, and one is unselected, then the other becomes
         // valid.
         gridView.RefreshData();
         SetOkButtonEnable();
      }

      private void formLoad()
      {
         gridControl.ForceInitialize();
         gridView.Appearance.SelectedRow.Assign(gridView.PaintAppearance.Row);
      }

      public void AttachPresenter(ISelectSpatialStructureAndMoleculesPresenter presenter)
      {
         _presenter = presenter;
      }

      private void gridViewRowStyle(RowStyleEventArgs e)
      {
         if (e.Appearance == gridView.PaintAppearance.SelectedRow)
            e.Appearance.Assign(gridView.PaintAppearance.Row);
         e.HighPriority = true;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder = new ScreenBinder<SelectSpatialStructureAndMoleculesDTO>();
         _screenBinder.Bind(dto => dto.SpatialStructure).To(cmbSpatialStructure).WithValues(dto => _presenter.AllSpatialStructures);

         _gridViewBinder = new GridViewBinder<MoleculeSelectionDTO>(gridView);

         var colBuildingBlock = _gridViewBinder.Bind(dto => dto.BuildingBlock).AsReadOnly();
         colBuildingBlock.XtraColumn.GroupIndex = 0;

         _gridViewBinder.
            AutoBind(dto => dto.MoleculeName).
            WithRepository(configureMoleculeRepository).
            WithCaption(Molecule).
            AsReadOnly();
         RegisterValidationFor(_screenBinder);
      }

      private RepositoryItem configureMoleculeRepository(MoleculeSelectionDTO molecule)
      {
         var repository = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         return repository.AddItem(molecule.MoleculeName, molecule.Icon);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemSpatialStructure.Text = ObjectTypes.SpatialStructure.FormatForLabel();
         layoutControlItemMolecules.Text = AppConstants.Captions.Molecules.FormatForLabel();
         Text = AppConstants.Captions.NewWindow(ObjectTypes.MoleculeBuildingBlock);

         configureGridForCheckBoxSelect();

         layoutControlItemMolecules.TextLocation = Locations.Top;
         layoutControlItemMolecules.TextVisible = true;
         configureGridGrouping();
      }

      private void configureGridGrouping()
      {
         // When grouping, the default is to show the column name and the value. So we would see
         // "BuildingBlock: Molecules" instead of just "Molecules". We just want to see the value in this case
         gridView.GroupFormat = "[#image]{1}";
         gridView.EndGrouping += (o, e) => gridView.ExpandAllGroups();
      }

      private void configureGridForCheckBoxSelect()
      {
         gridView.MultiSelect = true;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
         gridView.OptionsSelection.CheckBoxSelectorField = _selectedColumnName;
         gridView.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DefaultBoolean.True;
      }

      public void Show(SelectSpatialStructureAndMoleculesDTO dto)
      {
         _screenBinder.BindToSource(dto);
         _gridViewBinder.BindToSource(dto.Molecules);
         gridView.BestFitColumns();
      }

      public override bool HasError => base.HasError || _gridViewBinder.HasError || _screenBinder.HasError;

      private void disposeBinders()
      {
         _screenBinder.Dispose();
         _gridViewBinder.Dispose();
      }
   }
}