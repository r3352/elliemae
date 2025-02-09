// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MatchedLoanDuplicate
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MatchedLoanDuplicate : Form
  {
    private Sessions.Session session;
    private IContainer components;
    private Button btnCancel;
    private Button btnSelect;
    private Label label1;
    private GridView gvLoans;
    private CheckBox chkNotShowAgain;

    public MatchedLoanDuplicate(Sessions.Session session, List<LoanDuplicateChecker> loanDuplicates)
    {
      this.session = session;
      this.InitializeComponent();
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(LoanInfo.Right.Access, new string[5]
      {
        "Loan.Guid",
        "Loan.BorrowerFirstName",
        "Loan.BorrowerLastName",
        "Loan.DateFileOpened",
        "Loan.LoanFolder"
      }, PipelineData.Fields, (SortField[]) null, (QueryCriterion) new ListValueCriterion("Loan.Guid", (Array) loanDuplicates.Select<LoanDuplicateChecker, string>((Func<LoanDuplicateChecker, string>) (a => a.GUID.ToString("B"))).ToArray<string>()), false);
      this.gvLoans.Items.Clear();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (PipelineInfo pipelineInfo in pipeline)
        stringSet.Add(pipelineInfo.GUID);
      foreach (LoanDuplicateChecker loanDuplicate in loanDuplicates)
      {
        UserInfo user1 = loanDuplicate.FileStarter == string.Empty ? (UserInfo) null : session.OrganizationManager.GetUser(loanDuplicate.FileStarter);
        UserInfo user2 = loanDuplicate.LoanOfficer == string.Empty ? (UserInfo) null : session.OrganizationManager.GetUser(loanDuplicate.LoanOfficer);
        GVItem gvItem = this.createGVItem(loanDuplicate, user1, user2);
        gvItem.ForeColor = Color.Gray;
        if (stringSet.Contains(loanDuplicate.GUID.ToString("B")))
          gvItem.ForeColor = Color.Black;
        this.gvLoans.Items.Add(gvItem);
      }
      this.chkNotShowAgain.Checked = !session.LoanDataMgr.SystemConfiguration.IsDuplicateLoanCheckLoanOnly;
    }

    private GVItem createGVItem(
      LoanDuplicateChecker loan,
      UserInfo fileStarter,
      UserInfo loanOfficer)
    {
      GVItem gvItem = new GVItem();
      Panel panel1 = fileStarter != (UserInfo) null ? this.createUserElement(fileStarter) : new Panel();
      Panel panel2 = loanOfficer != (UserInfo) null ? this.createUserElement(loanOfficer) : new Panel();
      if (loan.FileStarter == "")
        panel1.Tag = (object) "zzzzzzzzzzzzz";
      if (loan.LoanOfficer == "")
        panel2.Tag = (object) "zzzzzzzzzzzzz";
      gvItem.SubItems[0].Value = (object) loan.DateOpened.ToString("d");
      gvItem.SubItems[1].Value = (object) (loan.LastName + ", " + loan.FirstName);
      gvItem.SubItems[2].Value = (object) loan.SubjectProperty.ToString().Trim(',');
      gvItem.SubItems[3].Value = loan.LoanAmount <= 0L ? (object) "" : (object) loan.LoanAmount.ToString("#,##0");
      gvItem.SubItems[4].Value = (object) loan.Loantype.ToString();
      gvItem.SubItems[5].Value = (object) loan.LoanStatus.ToString();
      gvItem.SubItems[6].Value = (object) panel2;
      gvItem.SubItems[6].SortValue = panel2.Tag;
      gvItem.SubItems[7].Value = (object) panel1;
      gvItem.SubItems[7].SortValue = panel1.Tag;
      gvItem.SubItems[8].Value = (object) loan.LoanFolder.ToString();
      gvItem.Tag = (object) loan;
      return gvItem;
    }

    private Panel createUserElement(UserInfo user)
    {
      Label label1 = new Label();
      Panel userElement = new Panel();
      ElementControl elementControl = new ElementControl();
      elementControl.Element = (object) new UserLink((Control) this, user.Userid, user.FirstName);
      userElement.Size = new Size(100, 16);
      label1.Size = new Size(80, 16);
      userElement.Controls.Add((Control) elementControl);
      userElement.Controls.Add((Control) label1);
      elementControl.Size = new Size(16, 16);
      Label label2 = label1;
      Point location = elementControl.Location;
      int x = location.X + 20;
      location = elementControl.Location;
      int y = location.Y;
      Point point = new Point(x, y);
      label2.Location = point;
      label1.Text = user.FullName;
      userElement.Tag = (object) user.FullName.Trim();
      return userElement;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
        return;
      LoanDuplicateChecker tag = (LoanDuplicateChecker) this.gvLoans.SelectedItems[0].Tag;
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      using (SaveOnClose saveOnClose = new SaveOnClose())
      {
        if (saveOnClose.ShowDialog() == DialogResult.Cancel)
          return;
        if (service.CloseLoanWithoutPrompts(saveOnClose.BoolSaveLoan))
          service.OpenLoan("{" + tag.GUID.ToString() + "}");
        this.DialogResult = DialogResult.OK;
      }
    }

    private void gvLoans_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
        return;
      if (this.gvLoans.SelectedItems[0].ForeColor == Color.Gray)
      {
        this.btnSelect.Enabled = false;
      }
      else
      {
        if (!(this.gvLoans.SelectedItems[0].ForeColor == Color.Black))
          return;
        this.btnSelect.Enabled = true;
      }
    }

    private void chkNotShowAgain_CheckedChanged(object sender, EventArgs e)
    {
      this.session.ConfigurationManager.SaveDuplicateScreenSetting(this.chkNotShowAgain.Checked ? new DuplicateScreenSetting(false) : new DuplicateScreenSetting(true), this.session.LoanDataMgr.LoanFolder, this.session.LoanDataMgr.LoanName);
      this.session.LoanDataMgr.SystemConfiguration.IsDuplicateLoanCheckLoanOnly = !this.chkNotShowAgain.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MatchedLoanDuplicate));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.label1 = new Label();
      this.chkNotShowAgain = new CheckBox();
      this.gvLoans = new GridView();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(961, 245);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(86, 24);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Exit";
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(869, 245);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(86, 24);
      this.btnSelect.TabIndex = 7;
      this.btnSelect.Text = "Review Loan";
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.label1.Location = new Point(1, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(1035, 32);
      this.label1.TabIndex = 6;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.chkNotShowAgain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkNotShowAgain.AutoSize = true;
      this.chkNotShowAgain.Location = new Point(12, 250);
      this.chkNotShowAgain.Name = "chkNotShowAgain";
      this.chkNotShowAgain.Size = new Size(172, 17);
      this.chkNotShowAgain.TabIndex = 10;
      this.chkNotShowAgain.Text = "Do not show again for this loan";
      this.chkNotShowAgain.UseVisualStyleBackColor = true;
      this.chkNotShowAgain.CheckedChanged += new EventHandler(this.chkNotShowAgain_CheckedChanged);
      this.gvLoans.AllowColumnReorder = true;
      this.gvLoans.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvLoans.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Date;
      gvColumn1.Text = "Date Created";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Borrower Name";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Property Address";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Loan Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Loan Type";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Loan Status";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Loan Officer";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.SortMethod = GVSortMethod.Custom;
      gvColumn8.Text = "Started By";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Text = "Loan Folder";
      gvColumn9.Width = 100;
      this.gvLoans.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvLoans.Location = new Point(3, 45);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(1051, 193);
      this.gvLoans.TabIndex = 9;
      this.gvLoans.SelectedIndexChanged += new EventHandler(this.gvLoans_SelectedIndexChanged);
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1055, 276);
      this.Controls.Add((Control) this.chkNotShowAgain);
      this.Controls.Add((Control) this.gvLoans);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.label1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (MatchedLoanDuplicate);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Duplicate Loan Check";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
