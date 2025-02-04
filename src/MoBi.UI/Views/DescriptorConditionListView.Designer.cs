﻿namespace MoBi.UI.Views
{
   partial class DescriptorConditionListView
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
         _gridViewBinder.Dispose();
         _screenBinder.Dispose();
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
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.lblCriteriaDescription = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.cbOperator = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutItemOperator = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOperator.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOperator)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.Location = new System.Drawing.Point(2, 26);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(330, 292);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.cbOperator);
         this.layoutControl.Controls.Add(this.lblCriteriaDescription);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(334, 337);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // lblCriteriaDescription
         // 
         this.lblCriteriaDescription.Location = new System.Drawing.Point(2, 322);
         this.lblCriteriaDescription.Name = "lblCriteriaDescription";
         this.lblCriteriaDescription.Size = new System.Drawing.Size(98, 13);
         this.lblCriteriaDescription.StyleController = this.layoutControl;
         this.lblCriteriaDescription.TabIndex = 4;
         this.lblCriteriaDescription.Text = "lblCriteriaDescription";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutItemOperator});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(334, 337);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.gridControl;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(334, 296);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblCriteriaDescription;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 320);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(334, 17);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // cbOperator
         // 
         this.cbOperator.Location = new System.Drawing.Point(110, 2);
         this.cbOperator.Name = "cbOperator";
         this.cbOperator.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbOperator.Size = new System.Drawing.Size(222, 20);
         this.cbOperator.StyleController = this.layoutControl;
         this.cbOperator.TabIndex = 6;
         // 
         // layoutItemOperator
         // 
         this.layoutItemOperator.Control = this.cbOperator;
         this.layoutItemOperator.Location = new System.Drawing.Point(0, 0);
         this.layoutItemOperator.Name = "layoutItemOperator";
         this.layoutItemOperator.Size = new System.Drawing.Size(334, 24);
         this.layoutItemOperator.TextSize = new System.Drawing.Size(96, 13);
         // 
         // DescriptorConditionListView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "DescriptorConditionListView";
         this.Size = new System.Drawing.Size(334, 337);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOperator.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOperator)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private MoBi.UI.Views.UxGridView gridView;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraEditors.LabelControl lblCriteriaDescription;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.ComboBoxEdit cbOperator;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOperator;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
   }
}
