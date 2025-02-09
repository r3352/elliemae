// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeGroupsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FeeGroupsForm : UserControl
  {
    private IContainer components;
    private GroupContainer gcBaseRate;
    private StandardIconButton stdBtnMainSave;
    private GridView listViewOptions;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip;

    public FeeGroupsForm() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.gcBaseRate = new GroupContainer();
      this.stdBtnMainSave = new StandardIconButton();
      this.listViewOptions = new GridView();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip = new ToolTip(this.components);
      this.gcBaseRate.SuspendLayout();
      ((ISupportInitialize) this.stdBtnMainSave).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.gcBaseRate.Controls.Add((Control) this.stdBtnMainSave);
      this.gcBaseRate.Controls.Add((Control) this.listViewOptions);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnNew);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcBaseRate.Dock = DockStyle.Fill;
      this.gcBaseRate.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseRate.Location = new Point(0, 0);
      this.gcBaseRate.Name = "gcBaseRate";
      this.gcBaseRate.Size = new Size(702, 517);
      this.gcBaseRate.TabIndex = 3;
      this.gcBaseRate.Text = "Fee Group";
      this.stdBtnMainSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdBtnMainSave.BackColor = Color.Transparent;
      this.stdBtnMainSave.Location = new Point(678, 5);
      this.stdBtnMainSave.MouseDownImage = (Image) null;
      this.stdBtnMainSave.Name = "stdBtnMainSave";
      this.stdBtnMainSave.Size = new Size(16, 16);
      this.stdBtnMainSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdBtnMainSave.TabIndex = 82;
      this.stdBtnMainSave.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdBtnMainSave, "Save");
      this.listViewOptions.AutoHeight = true;
      this.listViewOptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Fee Group Name";
      gvColumn1.Width = 180;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Fee Lines";
      gvColumn2.Width = 240;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Last Modified By";
      gvColumn3.Width = 140;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Last Modified Date";
      gvColumn4.Width = 140;
      this.listViewOptions.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listViewOptions.Dock = DockStyle.Fill;
      this.listViewOptions.HeaderHeight = 22;
      this.listViewOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewOptions.Location = new Point(1, 26);
      this.listViewOptions.Name = "listViewOptions";
      this.listViewOptions.Size = new Size(700, 490);
      this.listViewOptions.TabIndex = 0;
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(612, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 79;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(634, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 77;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(656, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 74;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcBaseRate);
      this.Name = nameof (FeeGroupsForm);
      this.Size = new Size(702, 517);
      this.gcBaseRate.ResumeLayout(false);
      ((ISupportInitialize) this.stdBtnMainSave).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
