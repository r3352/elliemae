// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.SendEmailDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class SendEmailDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private string packageGUID;
    private StatusOnlineTrigger statusTrigger;
    private const string CURRENT_USER = "Current User";
    private const string FILE_STARTER = "File Starter";
    private const string LOAN_OFFICER = "Loan Officer";
    private const string OWNER = "Other";
    private IContainer components;
    private Label lblFrom;
    private ComboBox cboFrom;
    private Label lblTo;
    private TextBox txtTo;
    private Label lblSubject;
    private ComboBox cboSubject;
    private Button btnSend;
    private Button btnSkip;
    private StandardIconButton btnTo;
    private HtmlEmailControl htmlEmailControl;

    public SendEmailDialog(
      LoanDataMgr loanDataMgr,
      string packageGUID,
      StatusOnlineTrigger statusTrigger)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.packageGUID = packageGUID;
      this.statusTrigger = statusTrigger;
      this.initContents();
    }

    private void initContents()
    {
      this.initHtmlControl();
      this.initFrom();
      this.initTo();
      this.initSelectedEmailTemplate();
    }

    private void initHtmlControl()
    {
      this.htmlEmailControl.LoadText(string.Empty, false);
      this.htmlEmailControl.ShowFieldButton = false;
      this.htmlEmailControl.AllowPersonalImages = Session.UserInfo.PersonalStatusOnline;
    }

    private void initFrom()
    {
      List<string> stringList = new List<string>();
      string userid1 = string.Empty;
      string email1 = string.Empty;
      string name1 = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, TriggerEmailFromType.CurrentUser, this.statusTrigger.OwnerID, out userid1, out email1, out name1);
      if (!string.IsNullOrEmpty(userid1) && !string.IsNullOrEmpty(email1) && !stringList.Contains(userid1.ToLower().Trim()))
      {
        stringList.Add(userid1.ToLower().Trim());
        this.cboFrom.Items.Add((object) new FieldOption(email1 + " (Current User)", "Current User", userid1));
      }
      string userid2 = string.Empty;
      string email2 = string.Empty;
      string name2 = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, TriggerEmailFromType.FileStarter, this.statusTrigger.OwnerID, out userid2, out email2, out name2);
      if (!string.IsNullOrEmpty(userid2) && !string.IsNullOrEmpty(email2) && !stringList.Contains(userid2.ToLower().Trim()))
      {
        stringList.Add(userid2.ToLower().Trim());
        this.cboFrom.Items.Add((object) new FieldOption(email2 + " (File Starter)", "File Starter", userid2));
      }
      string userid3 = string.Empty;
      string email3 = string.Empty;
      string name3 = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, TriggerEmailFromType.LoanOfficer, this.statusTrigger.OwnerID, out userid3, out email3, out name3);
      if (!string.IsNullOrEmpty(userid3) && !string.IsNullOrEmpty(email3) && !stringList.Contains(userid3.ToLower().Trim()))
      {
        stringList.Add(userid3.ToLower().Trim());
        this.cboFrom.Items.Add((object) new FieldOption(email3 + " (Loan Officer)", "Loan Officer", userid3));
      }
      string userid4 = string.Empty;
      string email4 = string.Empty;
      string name4 = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, TriggerEmailFromType.Owner, this.statusTrigger.OwnerID, out userid4, out email4, out name4);
      if (!string.IsNullOrEmpty(userid4) && !string.IsNullOrEmpty(email4) && !stringList.Contains(userid4.ToLower().Trim()))
      {
        stringList.Add(userid4.ToLower().Trim());
        this.cboFrom.Items.Add((object) new FieldOption(email4 + " (Other)", "Other", userid4));
      }
      this.cboFrom.SelectedIndex = this.getFromSelection();
    }

    private int getFromSelection()
    {
      if (this.cboFrom.Items.Count == 0)
        return -1;
      for (int index = 0; index < this.cboFrom.Items.Count; ++index)
      {
        FieldOption fieldOption = (FieldOption) this.cboFrom.Items[index];
        string str = string.Empty;
        switch (this.statusTrigger.EmailFromType)
        {
          case TriggerEmailFromType.CurrentUser:
            str = "Current User";
            break;
          case TriggerEmailFromType.LoanOfficer:
            str = "Loan Officer";
            break;
          case TriggerEmailFromType.FileStarter:
            str = "File Starter";
            break;
          case TriggerEmailFromType.Owner:
            str = "Other";
            break;
        }
        if (fieldOption.Value == str)
          return index;
      }
      return 0;
    }

    private void initTo()
    {
      this.txtTo.Text = StatusOnlineManager.GetTriggerRecipients(this.loanDataMgr, this.statusTrigger.EmailRecipients);
    }

    private void initSelectedEmailTemplate()
    {
      string emailTemplate = this.statusTrigger.EmailTemplate;
      if (string.IsNullOrEmpty(emailTemplate))
        return;
      HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate(this.statusTrigger.EmailTemplateOwner, emailTemplate);
      if (!(htmlEmailTemplate != (HtmlEmailTemplate) null) || this.cboSubject.Items.Contains((object) htmlEmailTemplate))
        return;
      this.cboSubject.SelectedIndex = this.cboSubject.Items.Add((object) htmlEmailTemplate);
      this.populateHtmlEmail();
    }

    private void populateHtmlEmail()
    {
      UserInfo userInfo = (UserInfo) null;
      if ((object) (this.cboSubject.SelectedItem as HtmlEmailTemplate) == null)
        return;
      HtmlEmailTemplate selectedItem1 = (HtmlEmailTemplate) this.cboSubject.SelectedItem;
      if (this.cboFrom.SelectedItem != null)
      {
        FieldOption selectedItem2 = (FieldOption) this.cboFrom.SelectedItem;
        if (selectedItem2 != null)
          userInfo = !(selectedItem2.Value == "Current User") ? Session.OrganizationManager.GetUser(selectedItem2.ReportingDatabaseValue) : Session.UserInfo;
      }
      string html = new HtmlFieldMerge(selectedItem1.Html).MergeContent(this.loanDataMgr.LoanData, userInfo);
      if (string.IsNullOrEmpty(html))
        return;
      this.htmlEmailControl.LoadHtml(html, false);
    }

    private void btnTo_Click(object sender, EventArgs e)
    {
      string str1 = (string) null;
      using (EmailListDialog emailListDialog = new EmailListDialog(this.loanDataMgr.LoanData, true))
      {
        if (emailListDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        str1 = emailListDialog.EmailList;
      }
      string str2 = this.txtTo.Text.Trim();
      this.txtTo.Text = !(str2 == string.Empty) ? str2 + "; " + str1 : str1;
      this.txtTo.Focus();
    }

    private void cboSubject_DropDown(object sender, EventArgs e)
    {
      foreach (HtmlEmailTemplate htmlEmailTemplate in Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.StatusOnline))
      {
        if (!this.cboSubject.Items.Contains((object) htmlEmailTemplate))
          this.cboSubject.Items.Add((object) htmlEmailTemplate);
      }
      FieldOption selectedItem = (FieldOption) this.cboFrom.SelectedItem;
      if (selectedItem != null)
      {
        string reportingDatabaseValue = selectedItem.ReportingDatabaseValue;
        if (!string.IsNullOrEmpty(reportingDatabaseValue))
        {
          foreach (HtmlEmailTemplate htmlEmailTemplate in Session.ConfigurationManager.GetHtmlEmailTemplates(reportingDatabaseValue, HtmlEmailTemplateType.StatusOnline))
          {
            if (!this.cboSubject.Items.Contains((object) htmlEmailTemplate))
              this.cboSubject.Items.Add((object) htmlEmailTemplate);
          }
        }
      }
      this.cboSubject.DropDown -= new EventHandler(this.cboSubject_DropDown);
    }

    private void cboSubject_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.populateHtmlEmail();
    }

    private void cboFrom_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.populateHtmlEmail();
      this.cboSubject.DropDown -= new EventHandler(this.cboSubject_DropDown);
    }

    private void btnSkip_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnSend_Click(object sender, EventArgs e)
    {
      char[] chArray = new char[1]{ ' ' };
      string[] strArray = this.cboFrom.Text.Trim().Split(chArray);
      string fromName = string.Empty;
      FieldOption selectedItem = (FieldOption) this.cboFrom.SelectedItem;
      if (selectedItem != null)
      {
        UserInfo userInfo = !(selectedItem.Value == "Current User") ? Session.OrganizationManager.GetUser(selectedItem.ReportingDatabaseValue) : Session.UserInfo;
        if (userInfo != (UserInfo) null)
          fromName = userInfo.FullName;
      }
      StatusOnlineClient statusOnlineClient = new StatusOnlineClient(this.loanDataMgr);
      bool isTPOConnectLoan = StatusOnlineManager.IsTPOConnectLoan(this.loanDataMgr);
      RecipientInfo recipientInfo = statusOnlineClient.CreateRecipientInfo(this.packageGUID, this.statusTrigger.Guid, strArray[0], fromName, this.txtTo.Text.Trim(), this.cboSubject.Text.Trim(), this.htmlEmailControl.Html, true, isTPOConnectLoan);
      string str = statusOnlineClient.ValidateRecipientInfo(recipientInfo);
      if (str != string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must address the following issues before you can send the email:" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (statusOnlineClient.SendRecipientEmail(recipientInfo) == string.Empty)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "An email message has been sent to " + recipientInfo.to, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblFrom = new Label();
      this.cboFrom = new ComboBox();
      this.lblTo = new Label();
      this.txtTo = new TextBox();
      this.lblSubject = new Label();
      this.cboSubject = new ComboBox();
      this.btnSend = new Button();
      this.btnSkip = new Button();
      this.btnTo = new StandardIconButton();
      this.htmlEmailControl = new HtmlEmailControl();
      ((ISupportInitialize) this.btnTo).BeginInit();
      this.SuspendLayout();
      this.lblFrom.AutoSize = true;
      this.lblFrom.Location = new Point(15, 27);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new Size(31, 14);
      this.lblFrom.TabIndex = 0;
      this.lblFrom.Text = "From";
      this.cboFrom.FormattingEnabled = true;
      this.cboFrom.Location = new Point(63, 24);
      this.cboFrom.Name = "cboFrom";
      this.cboFrom.Size = new Size(507, 22);
      this.cboFrom.TabIndex = 1;
      this.cboFrom.SelectionChangeCommitted += new EventHandler(this.cboFrom_SelectionChangeCommitted);
      this.lblTo.AutoSize = true;
      this.lblTo.Location = new Point(15, 55);
      this.lblTo.Name = "lblTo";
      this.lblTo.Size = new Size(19, 14);
      this.lblTo.TabIndex = 2;
      this.lblTo.Text = "To";
      this.txtTo.Location = new Point(63, 54);
      this.txtTo.Name = "txtTo";
      this.txtTo.Size = new Size(482, 20);
      this.txtTo.TabIndex = 3;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(13, 87);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(43, 14);
      this.lblSubject.TabIndex = 4;
      this.lblSubject.Text = "Subject";
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(64, 83);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(506, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 5;
      this.cboSubject.SelectionChangeCommitted += new EventHandler(this.cboSubject_SelectionChangeCommitted);
      this.cboSubject.DropDown += new EventHandler(this.cboSubject_DropDown);
      this.btnSend.Location = new Point(393, 520);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 23);
      this.btnSend.TabIndex = 8;
      this.btnSend.Text = "Send";
      this.btnSend.UseVisualStyleBackColor = true;
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.btnSkip.Location = new Point(480, 520);
      this.btnSkip.Name = "btnSkip";
      this.btnSkip.Size = new Size(89, 23);
      this.btnSkip.TabIndex = 9;
      this.btnSkip.Text = "Skip This Email";
      this.btnSkip.UseVisualStyleBackColor = true;
      this.btnSkip.Click += new EventHandler(this.btnSkip_Click);
      this.btnTo.BackColor = Color.Transparent;
      this.btnTo.Location = new Point(551, 57);
      this.btnTo.MouseDownImage = (Image) null;
      this.btnTo.Name = "btnTo";
      this.btnTo.Size = new Size(16, 16);
      this.btnTo.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnTo.TabIndex = 10;
      this.btnTo.TabStop = false;
      this.btnTo.Click += new EventHandler(this.btnTo_Click);
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(18, 126);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(554, 373);
      this.htmlEmailControl.TabIndex = 11;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(593, 568);
      this.Controls.Add((Control) this.htmlEmailControl);
      this.Controls.Add((Control) this.btnTo);
      this.Controls.Add((Control) this.btnSkip);
      this.Controls.Add((Control) this.btnSend);
      this.Controls.Add((Control) this.cboSubject);
      this.Controls.Add((Control) this.lblSubject);
      this.Controls.Add((Control) this.txtTo);
      this.Controls.Add((Control) this.lblTo);
      this.Controls.Add((Control) this.cboFrom);
      this.Controls.Add((Control) this.lblFrom);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SendEmailDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Send Notification Email";
      ((ISupportInitialize) this.btnTo).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
