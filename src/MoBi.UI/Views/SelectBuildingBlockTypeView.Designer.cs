﻿using System.Windows.Forms;

namespace MoBi.UI.Views
{
   partial class SelectBuildingBlockTypeView
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
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.clarificationLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.buildingBlockSelectionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.descriptionLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.buildingBlockSelectionlayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.buildingBlockSelectionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buildingBlockSelectionlayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 501);
         this.tablePanel.Size = new System.Drawing.Size(1431, 109);
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.clarificationLabelControl);
         this.layoutControl1.Controls.Add(this.buildingBlockSelectionComboBoxEdit);
         this.layoutControl1.Controls.Add(this.descriptionLabelControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(1431, 501);
         this.layoutControl1.TabIndex = 39;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // clarificationLabelControl
         // 
         this.clarificationLabelControl.Location = new System.Drawing.Point(12, 456);
         this.clarificationLabelControl.Name = "clarificationLabelControl";
         this.clarificationLabelControl.Size = new System.Drawing.Size(282, 33);
         this.clarificationLabelControl.StyleController = this.layoutControl1;
         this.clarificationLabelControl.TabIndex = 6;
         this.clarificationLabelControl.Text = "clarificationLabelControl";
         // 
         // buildingBlockSelectionComboBoxEdit
         // 
         this.buildingBlockSelectionComboBoxEdit.Location = new System.Drawing.Point(499, 83);
         this.buildingBlockSelectionComboBoxEdit.Name = "buildingBlockSelectionComboBoxEdit";
         this.buildingBlockSelectionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.buildingBlockSelectionComboBoxEdit.Size = new System.Drawing.Size(920, 48);
         this.buildingBlockSelectionComboBoxEdit.StyleController = this.layoutControl1;
         this.buildingBlockSelectionComboBoxEdit.TabIndex = 5;
         // 
         // descriptionLabelControl
         // 
         this.descriptionLabelControl.Location = new System.Drawing.Point(12, 12);
         this.descriptionLabelControl.Name = "descriptionLabelControl";
         this.descriptionLabelControl.Size = new System.Drawing.Size(277, 33);
         this.descriptionLabelControl.StyleController = this.layoutControl1;
         this.descriptionLabelControl.TabIndex = 4;
         this.descriptionLabelControl.Text = "descriptionLabelControl";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.buildingBlockSelectionlayoutControlItem,
            this.layoutControlItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1431, 501);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.descriptionLabelControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1411, 37);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // buildingBlockSelectionlayoutControlItem
         // 
         this.buildingBlockSelectionlayoutControlItem.Control = this.buildingBlockSelectionComboBoxEdit;
         this.buildingBlockSelectionlayoutControlItem.Location = new System.Drawing.Point(0, 71);
         this.buildingBlockSelectionlayoutControlItem.Name = "buildingBlockSelectionlayoutControlItem";
         this.buildingBlockSelectionlayoutControlItem.Size = new System.Drawing.Size(1411, 52);
         this.buildingBlockSelectionlayoutControlItem.TextSize = new System.Drawing.Size(475, 33);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.clarificationLabelControl;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 444);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1411, 37);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 37);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(1411, 34);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 123);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(1411, 321);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // SelectBuildingBlockTypeView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SelectBuildingBlockTypeView";
         this.ClientSize = new System.Drawing.Size(1431, 610);
         this.Controls.Add(this.layoutControl1);
         this.Name = "SelectBuildingBlockTypeView";
         this.Text = "SelectBuildingBlockTypeView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.buildingBlockSelectionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buildingBlockSelectionlayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.ComboBoxEdit buildingBlockSelectionComboBoxEdit;
      private DevExpress.XtraEditors.LabelControl descriptionLabelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem buildingBlockSelectionlayoutControlItem;
      private DevExpress.XtraEditors.LabelControl clarificationLabelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
   }
}