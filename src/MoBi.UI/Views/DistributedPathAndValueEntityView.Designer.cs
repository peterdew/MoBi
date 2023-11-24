﻿using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class DistributedPathAndValueEntityView<TPresenter, TPathAndValueEntity, TPathAndValueDTO, TBuildingBlock> 
      where TPresenter : IDistributedPathAndValueEntityPresenter<TPathAndValueDTO, TBuildingBlock>
      where TPathAndValueDTO : PathAndValueEntityDTO<TPathAndValueEntity, TPathAndValueDTO> where TPathAndValueEntity : PathAndValueEntity
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         disposeBinders();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.lblDistributionType = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(2, 19);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(429, 206);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = true;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsSelection.MultiSelect = true;
         this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.lblDistributionType);
         this.uxLayoutControl.Controls.Add(this.gridControl);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.Root = this.Root;
         this.uxLayoutControl.Size = new System.Drawing.Size(433, 227);
         this.uxLayoutControl.TabIndex = 1;
         this.uxLayoutControl.Text = "uxLayoutControl";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(433, 227);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.gridControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(433, 210);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // lblDistributionType
         // 
         this.lblDistributionType.Location = new System.Drawing.Point(2, 2);
         this.lblDistributionType.Name = "lblDistributionType";
         this.lblDistributionType.Size = new System.Drawing.Size(88, 13);
         this.lblDistributionType.StyleController = this.uxLayoutControl;
         this.lblDistributionType.TabIndex = 4;
         this.lblDistributionType.Text = "lblDistributionType";
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblDistributionType;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(433, 17);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // DistributedPathAndValueEntityView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Name = "DistributedPathAndValueEntityView";
         this.Size = new System.Drawing.Size(433, 227);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private OSPSuite.UI.Controls.UxGridView gridView;
      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraEditors.LabelControl lblDistributionType;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}
