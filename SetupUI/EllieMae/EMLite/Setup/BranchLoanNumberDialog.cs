// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BranchLoanNumberDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BranchLoanNumberDialog : Form
  {
    private BranchLoanNumberingInfo[] branch;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ListView branchListview;
    private Button exitBtn;
    private Button editBtn;
    private Label label1;
    private Label label2;
    private EMHelpLink emHelpLink1;
    private System.ComponentModel.Container components;

    public BranchLoanNumberDialog()
    {
      this.InitializeComponent();
      this.branch = Session.ConfigurationManager.GetAllBranchLoanNumberingInfo(true);
      this.InitializeBranchList();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.exitBtn = new Button();
      this.editBtn = new Button();
      this.branchListview = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.label1 = new Label();
      this.label2 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.SuspendLayout();
      this.exitBtn.DialogResult = DialogResult.Cancel;
      this.exitBtn.Location = new Point(440, 100);
      this.exitBtn.Name = "exitBtn";
      this.exitBtn.Size = new Size(75, 24);
      this.exitBtn.TabIndex = 18;
      this.exitBtn.Text = "&Close";
      this.editBtn.Location = new Point(440, 68);
      this.editBtn.Name = "editBtn";
      this.editBtn.Size = new Size(75, 24);
      this.editBtn.TabIndex = 17;
      this.editBtn.Text = "&Edit";
      this.editBtn.Click += new EventHandler(this.editBtn_Click);
      this.branchListview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.branchListview.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.branchListview.FullRowSelect = true;
      this.branchListview.GridLines = true;
      this.branchListview.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.branchListview.HideSelection = false;
      this.branchListview.Location = new Point(12, 68);
      this.branchListview.MultiSelect = false;
      this.branchListview.Name = "branchListview";
      this.branchListview.Size = new Size(416, 256);
      this.branchListview.Sorting = SortOrder.Ascending;
      this.branchListview.TabIndex = 37;
      this.branchListview.UseCompatibleStateImageBehavior = false;
      this.branchListview.View = View.Details;
      this.branchListview.DoubleClick += new EventHandler(this.branchListview_DoubleClick);
      this.columnHeader1.Text = "Org. Code";
      this.columnHeader1.Width = 100;
      this.columnHeader2.Text = "Next Loan Number";
      this.columnHeader2.Width = 200;
      this.columnHeader3.Text = "Status";
      this.columnHeader3.TextAlign = HorizontalAlignment.Center;
      this.columnHeader3.Width = 100;
      this.label1.Location = new Point(48, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(416, 39);
      this.label1.TabIndex = 38;
      this.label1.Text = "Changes to these loan numbering settings can result in duplicate loan numbers.  Care should be taken to review the impact of changes before making them.";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(33, 13);
      this.label2.TabIndex = 39;
      this.label2.Text = "Note:";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Organization Loan Numbering";
      this.emHelpLink1.Location = new Point(12, 336);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 40;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(530, 364);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.branchListview);
      this.Controls.Add((Control) this.exitBtn);
      this.Controls.Add((Control) this.editBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BranchLoanNumberDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Organization Auto Loan Numbering";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void InitializeBranchList()
    {
      this.branchListview.Items.Clear();
      for (int index = 0; index < this.branch.Length; ++index)
        this.branchListview.Items.Add(new ListViewItem(this.branch[index].OrgCode)
        {
          SubItems = {
            this.branch[index].NextNumber,
            this.branch[index].Enabled ? "Enabled" : "Disabled"
          },
          Tag = (object) this.branch[index]
        });
    }

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (this.branchListview.SelectedItems.Count == 0)
        return;
      ListViewItem selectedItem = this.branchListview.SelectedItems[0];
      if (selectedItem == null)
        return;
      BranchLoanNoEditDialog loanNoEditDialog = new BranchLoanNoEditDialog(selectedItem.SubItems[0].Text, selectedItem.SubItems[1].Text, selectedItem.SubItems[2].Text == "Enabled");
      if (loanNoEditDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      BranchLoanNumberingInfo tag = (BranchLoanNumberingInfo) selectedItem.Tag;
      tag.Enabled = loanNoEditDialog.OrgUseLoanNo;
      tag.NextNumber = loanNoEditDialog.OrgNextNo;
      try
      {
        Session.ConfigurationManager.SaveBranchLoanNumberingInfo(tag);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The loan numbering for " + tag.OrgCode + " can't be changed. Error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      selectedItem.SubItems[1].Text = loanNoEditDialog.OrgNextNo;
      selectedItem.SubItems[2].Text = loanNoEditDialog.OrgUseLoanNo ? "Enabled" : "Disabled";
    }

    private void branchListview_DoubleClick(object sender, EventArgs e)
    {
      this.editBtn_Click((object) null, (EventArgs) null);
    }
  }
}
