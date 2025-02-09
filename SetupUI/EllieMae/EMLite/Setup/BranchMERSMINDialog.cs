// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BranchMERSMINDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BranchMERSMINDialog : Form
  {
    private BranchMERSMINNumberingInfo[] branch;
    private IContainer components;
    private Label label1;
    private Label label2;
    private GroupContainer groupContainer1;
    private GridView gvMERS;
    private StandardIconButton sibtnEdit;
    private Button btnClose;
    private VerticalSeparator verticalSeparator1;

    public BranchMERSMINDialog()
    {
      this.InitializeComponent();
      this.branch = Session.ConfigurationManager.GetAllBranchMERSNumberingInfo(true);
      this.InitializeBranchList();
    }

    private void InitializeBranchList()
    {
      this.gvMERS.Items.Clear();
      for (int index = 0; index < this.branch.Length; ++index)
        this.gvMERS.Items.Add(new GVItem(this.branch[index].MERSMINCode)
        {
          SubItems = {
            (object) this.branch[index].NextNumber,
            this.branch[index].Enabled ? (object) "Enabled" : (object) "Disabled"
          },
          Tag = (object) this.branch[index]
        });
    }

    private void sibtnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvMERS.SelectedItems.Count == 0)
        return;
      GVItem selectedItem = this.gvMERS.SelectedItems[0];
      if (selectedItem == null)
        return;
      BranchMERSNoEditDialog mersNoEditDialog = new BranchMERSNoEditDialog(selectedItem.SubItems[0].Text, selectedItem.SubItems[1].Text, selectedItem.SubItems[2].Text == "Enabled");
      if (mersNoEditDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      BranchMERSMINNumberingInfo tag = (BranchMERSMINNumberingInfo) selectedItem.Tag;
      tag.Enabled = mersNoEditDialog.OrgUseMERSNo;
      tag.NextNumber = mersNoEditDialog.OrgNextNo;
      try
      {
        Session.ConfigurationManager.SaveBranchMERSNumberingInfo(tag);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The MERS MIN numbering for " + tag.MERSMINCode + " can't be changed. Error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      selectedItem.SubItems[1].Text = mersNoEditDialog.OrgNextNo;
      selectedItem.SubItems[2].Text = mersNoEditDialog.OrgUseMERSNo ? "Enabled" : "Disabled";
    }

    private void gvMERS_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.sibtnEdit_Click((object) null, (EventArgs) null);
    }

    private void gvMERS_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvMERS.SelectedItems.Count > 0)
        this.sibtnEdit.Enabled = true;
      else
        this.sibtnEdit.Enabled = false;
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.label1 = new Label();
      this.label2 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.sibtnEdit = new StandardIconButton();
      this.gvMERS = new GridView();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnClose = new Button();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.sibtnEdit).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(38, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Notes:";
      this.label2.Location = new Point(49, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(461, 27);
      this.label2.TabIndex = 1;
      this.label2.Text = "Changes to the MERS MIN numbering can result in duplicate numbers.  Care should be taken to review the impact of changes before making them.";
      this.groupContainer1.Controls.Add((Control) this.btnClose);
      this.groupContainer1.Controls.Add((Control) this.verticalSeparator1);
      this.groupContainer1.Controls.Add((Control) this.sibtnEdit);
      this.groupContainer1.Controls.Add((Control) this.gvMERS);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(8, 35);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(502, 309);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "MERS MIN Numbering";
      this.sibtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.sibtnEdit.BackColor = Color.Transparent;
      this.sibtnEdit.Enabled = false;
      this.sibtnEdit.Location = new Point(397, 4);
      this.sibtnEdit.Name = "sibtnEdit";
      this.sibtnEdit.Size = new Size(16, 16);
      this.sibtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.sibtnEdit.TabIndex = 1;
      this.sibtnEdit.TabStop = false;
      this.sibtnEdit.Click += new EventHandler(this.sibtnEdit_Click);
      this.gvMERS.AllowMultiselect = false;
      this.gvMERS.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Org. ID";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Next Number";
      gvColumn2.Width = 250;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Status";
      gvColumn3.Width = 100;
      this.gvMERS.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvMERS.Dock = DockStyle.Fill;
      this.gvMERS.Location = new Point(1, 26);
      this.gvMERS.Name = "gvMERS";
      this.gvMERS.Size = new Size(500, 282);
      this.gvMERS.TabIndex = 0;
      this.gvMERS.SelectedIndexChanged += new EventHandler(this.gvMERS_SelectedIndexChanged);
      this.gvMERS.ItemDoubleClick += new GVItemEventHandler(this.gvMERS_ItemDoubleClick);
      this.verticalSeparator1.Location = new Point(418, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 2;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnClose.Location = new Point(423, 2);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(523, 362);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BranchMERSMINDialog);
      this.Text = "MERS MIN Numbering";
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.sibtnEdit).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
