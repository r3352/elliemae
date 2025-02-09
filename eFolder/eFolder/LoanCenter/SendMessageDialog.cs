// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.SendMessageDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class SendMessageDialog : Form
  {
    private LoanCenterMessage msg;
    private string packageID;
    private List<string> useridList = new List<string>();
    private IContainer components;
    private CheckBox chkReadReceipt;
    private Button btnCancel;
    private Button btnSend;
    private Label lblSubject;
    private TextBox txtTo;
    private HtmlEmailControl htmlEmailControl;
    private StandardIconButton btnToList;
    private ComboBox cboSubject;
    private Label lblFrom;
    private Label lblTo;
    private ComboBox cboFrom;
    private Button btnNotifyUsers;
    private Label lblNotifyUserCount;

    public SendMessageDialog(LoanCenterMessage msg)
    {
      this.InitializeComponent();
      this.msg = msg;
      this.initContents();
    }

    public string PackageID => this.packageID;

    private void initContents()
    {
      this.initHtmlControl();
      this.initFrom();
      this.initSelectedEmailTemplate();
      this.initNotifyCount();
    }

    private void initHtmlControl()
    {
      this.htmlEmailControl.LoadText(string.Empty, false);
      this.htmlEmailControl.ShowFieldButton = false;
      this.htmlEmailControl.AllowPersonalImages = true;
    }

    private void initFrom()
    {
      this.setFromUser(EmailFromType.CurrentUser);
      this.setFromUser(EmailFromType.FileStarter);
      this.setFromUser(EmailFromType.LoanOfficer);
      if (this.cboFrom.Items.Count <= 0)
        return;
      this.cboFrom.SelectedIndex = 0;
    }

    private void initNotifyCount()
    {
      this.lblNotifyUserCount.Text = string.Format("({0} Users selected)", (object) new EmailNotificationClient().ActiveEmailCount(new List<Guid>()
      {
        new Guid(this.msg.LoanDataMgr.LoanData.GUID)
      }.ToArray()));
    }

    private void setFromUser(EmailFromType fromUser)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string str = string.Empty;
      switch (fromUser)
      {
        case EmailFromType.CurrentUser:
          str = "Current User";
          break;
        case EmailFromType.FileStarter:
          str = "File Starter";
          break;
        case EmailFromType.LoanOfficer:
          str = "Loan Officer";
          break;
      }
      EmailFrom.GetFromUser(this.msg.LoanDataMgr, fromUser, ref empty1, ref empty2, ref empty3);
      if (string.IsNullOrEmpty(empty1) || string.IsNullOrEmpty(empty2) || this.useridList.Contains(empty1.ToLower().Trim()))
        return;
      this.useridList.Add(empty1.ToLower().Trim());
      this.cboFrom.Items.Add((object) new FieldOption(empty2 + " (" + str + ")", str, empty1));
    }

    private void initSelectedEmailTemplate()
    {
      string privateProfileString = Session.GetPrivateProfileString("EmailTemplates", "Send");
      if (!string.IsNullOrEmpty(privateProfileString))
      {
        this.setSelectedEmailTemplate(privateProfileString);
      }
      else
      {
        string companySetting = Session.ConfigurationManager.GetCompanySetting("DefaultEmailTemplates", "Send");
        if (string.IsNullOrEmpty(companySetting))
          return;
        this.setSelectedEmailTemplate(companySetting);
      }
    }

    private void setSelectedEmailTemplate(string emailTemplateGUID)
    {
      HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate((string) null, emailTemplateGUID);
      if (!(htmlEmailTemplate != (HtmlEmailTemplate) null) || this.cboSubject.Items.Contains((object) htmlEmailTemplate))
        return;
      this.cboSubject.SelectedIndex = this.cboSubject.Items.Add((object) htmlEmailTemplate);
      this.populateHtmlEmail();
    }

    private bool checkSecondaryUse()
    {
      if (this.msg.MsgType != LoanCenterMessageType.SendDocuments || !LoanServiceManager.ShowEmailPrompt)
        return true;
      bool flag1 = false;
      foreach (SDTAttachment attachment in this.msg.Attachments)
      {
        foreach (DocumentLog document in attachment.Documents)
        {
          if (document.Title == "Credit Report")
            flag1 = true;
        }
      }
      if (!flag1)
        return true;
      LoanData loanData = this.msg.LoanDataMgr.LoanData;
      BorrowerPair currentBorrowerPair = loanData.CurrentBorrowerPair;
      bool flag2 = false;
      foreach (BorrowerPair borrowerPair in loanData.GetBorrowerPairs())
      {
        loanData.SetBorrowerPair(borrowerPair);
        string field1 = loanData.GetField("1240");
        string field2 = loanData.GetField("1268");
        string to = this.msg.To;
        char[] chArray = new char[1]{ ';' };
        foreach (string str in to.Split(chArray))
        {
          if (string.Compare(str.Trim(), field1.Trim(), true) != 0 && string.Compare(str.Trim(), field2.Trim(), true) != 0)
            flag2 = true;
        }
      }
      loanData.SetBorrowerPair(currentBorrowerPair);
      if (!flag2)
        return true;
      DialogResult dialogResult = DialogResult.None;
      using (SecondaryUseDialog secondaryUseDialog = new SecondaryUseDialog())
        dialogResult = secondaryUseDialog.ShowDialog((IWin32Window) this);
      return dialogResult.Equals((object) DialogResult.Yes);
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      char[] chArray = new char[1]{ ' ' };
      this.msg.ReplyTo = this.cboFrom.Text.Trim().Split(chArray)[0];
      this.msg.To = this.txtTo.Text.Trim();
      this.msg.Subject = this.cboSubject.Text.Trim();
      if ((object) (this.cboSubject.SelectedItem as HtmlEmailTemplate) != null)
        Session.WritePrivateProfileString("EmailTemplates", "Send", (this.cboSubject.SelectedItem as HtmlEmailTemplate).Guid);
      this.msg.Body = this.htmlEmailControl.Html;
      this.msg.ReadReceipt = this.chkReadReceipt.Checked;
      string str = string.Empty;
      if (this.msg.ReplyTo == string.Empty)
        str += "From: email address";
      if (this.msg.To == string.Empty)
        str += "To: email address";
      if (this.msg.Subject == string.Empty)
        str += "Subject";
      if (str != string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The following information is required:\n\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!Utils.ValidateEmail(this.msg.ReplyTo))
          str = str + "From: email address." + Environment.NewLine;
        if (!Utils.ValidateEmail(this.msg.To))
          str = str + "Mail To: email address." + Environment.NewLine;
        if (str != string.Empty)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The following information is invalid:\n\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          using (UpdateEmailAddress updateEmailAddress = new UpdateEmailAddress(this.msg.LoanDataMgr.LoanData))
            updateEmailAddress.CheckEmailRecipients(this.msg.To, string.Empty);
          if (!this.checkSecondaryUse())
            return;
          using (LoanCenterClient loanCenterClient = new LoanCenterClient())
          {
            this.packageID = loanCenterClient.SendMessage(this.msg);
            if (this.packageID == null)
              return;
          }
          this.msg.CreateLogEntry();
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void btnToList_Click(object sender, EventArgs e)
    {
      string str1 = (string) null;
      using (EmailListDialog emailListDialog = new EmailListDialog(this.msg.LoanDataMgr.LoanData, true))
      {
        if (emailListDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        str1 = emailListDialog.EmailList;
      }
      string str2 = this.txtTo.Text.Trim();
      this.txtTo.Text = !(str2 == string.Empty) ? str2 + "; " + str1 : str1;
    }

    private void cboSubject_SelectionChangeCommitted(object sender, EventArgs e)
    {
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
      string html = new HtmlFieldMerge(selectedItem1.Html).MergeContent(this.msg.LoanDataMgr.LoanData, userInfo);
      if (string.IsNullOrEmpty(html))
        return;
      this.htmlEmailControl.LoadHtml(html, false);
    }

    private void cboSubject_DropDown(object sender, EventArgs e)
    {
      foreach (HtmlEmailTemplate htmlEmailTemplate in Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.SendDocuments))
      {
        if (!this.cboSubject.Items.Contains((object) htmlEmailTemplate))
          this.cboSubject.Items.Add((object) htmlEmailTemplate);
      }
      this.cboSubject.DropDown -= new EventHandler(this.cboSubject_DropDown);
    }

    private void btnNotifyUsers_Click(object sender, EventArgs e)
    {
      LoanDisplayInfo loanDisplayInfo = new LoanDisplayInfo()
      {
        LoanGuid = new Guid(this.msg.LoanDataMgr.LoanData.GUID)
      };
      using (NotifyUsersDialog notifyUsersDialog = new NotifyUsersDialog(new List<LoanDisplayInfo>()
      {
        loanDisplayInfo
      }))
      {
        int num = (int) notifyUsersDialog.ShowDialog((IWin32Window) this);
        if (!notifyUsersDialog.IsModified)
          return;
        this.initNotifyCount();
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
      this.chkReadReceipt = new CheckBox();
      this.btnCancel = new Button();
      this.btnSend = new Button();
      this.lblSubject = new Label();
      this.txtTo = new TextBox();
      this.htmlEmailControl = new HtmlEmailControl();
      this.btnToList = new StandardIconButton();
      this.cboSubject = new ComboBox();
      this.lblFrom = new Label();
      this.lblTo = new Label();
      this.cboFrom = new ComboBox();
      this.btnNotifyUsers = new Button();
      this.lblNotifyUserCount = new Label();
      ((ISupportInitialize) this.btnToList).BeginInit();
      this.SuspendLayout();
      this.chkReadReceipt.Location = new Point(17, 448);
      this.chkReadReceipt.Name = "chkReadReceipt";
      this.chkReadReceipt.Size = new Size(322, 22);
      this.chkReadReceipt.TabIndex = 7;
      this.chkReadReceipt.Text = "Notify me when the recipient has received the message";
      this.chkReadReceipt.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(556, 453);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnSend.Location = new Point(474, 453);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 22);
      this.btnSend.TabIndex = 8;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.lblSubject.Location = new Point(17, 65);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(44, 17);
      this.lblSubject.TabIndex = 4;
      this.lblSubject.Text = "Subject";
      this.lblSubject.TextAlign = ContentAlignment.MiddleCenter;
      this.txtTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTo.Location = new Point(73, 36);
      this.txtTo.Name = "txtTo";
      this.txtTo.Size = new Size(534, 20);
      this.txtTo.TabIndex = 3;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(17, 100);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(615, 342);
      this.htmlEmailControl.TabIndex = 6;
      this.btnToList.BackColor = Color.Transparent;
      this.btnToList.Location = new Point(613, 39);
      this.btnToList.MouseDownImage = (Image) null;
      this.btnToList.Name = "btnToList";
      this.btnToList.Size = new Size(16, 16);
      this.btnToList.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnToList.TabIndex = 13;
      this.btnToList.TabStop = false;
      this.btnToList.Click += new EventHandler(this.btnToList_Click);
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(73, 62);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(556, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 5;
      this.cboSubject.DropDown += new EventHandler(this.cboSubject_DropDown);
      this.cboSubject.SelectionChangeCommitted += new EventHandler(this.cboSubject_SelectionChangeCommitted);
      this.lblFrom.AutoSize = true;
      this.lblFrom.Location = new Point(17, 10);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new Size(31, 14);
      this.lblFrom.TabIndex = 0;
      this.lblFrom.Text = "From";
      this.lblTo.AutoSize = true;
      this.lblTo.Location = new Point(17, 39);
      this.lblTo.Name = "lblTo";
      this.lblTo.Size = new Size(18, 14);
      this.lblTo.TabIndex = 2;
      this.lblTo.Text = "To";
      this.cboFrom.FormattingEnabled = true;
      this.cboFrom.Location = new Point(73, 7);
      this.cboFrom.Name = "cboFrom";
      this.cboFrom.Size = new Size(556, 22);
      this.cboFrom.TabIndex = 1;
      this.btnNotifyUsers.Location = new Point(14, 469);
      this.btnNotifyUsers.Name = "btnNotifyUsers";
      this.btnNotifyUsers.Size = new Size(129, 23);
      this.btnNotifyUsers.TabIndex = 15;
      this.btnNotifyUsers.Text = "Notify Additional Users";
      this.btnNotifyUsers.UseVisualStyleBackColor = true;
      this.btnNotifyUsers.Click += new EventHandler(this.btnNotifyUsers_Click);
      this.lblNotifyUserCount.AutoSize = true;
      this.lblNotifyUserCount.Location = new Point(145, 474);
      this.lblNotifyUserCount.Name = "lblNotifyUserCount";
      this.lblNotifyUserCount.Size = new Size(105, 14);
      this.lblNotifyUserCount.TabIndex = 14;
      this.lblNotifyUserCount.Text = "({0} Users selected)";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(642, 495);
      this.Controls.Add((Control) this.btnNotifyUsers);
      this.Controls.Add((Control) this.lblNotifyUserCount);
      this.Controls.Add((Control) this.cboFrom);
      this.Controls.Add((Control) this.lblTo);
      this.Controls.Add((Control) this.lblFrom);
      this.Controls.Add((Control) this.cboSubject);
      this.Controls.Add((Control) this.btnToList);
      this.Controls.Add((Control) this.htmlEmailControl);
      this.Controls.Add((Control) this.chkReadReceipt);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSend);
      this.Controls.Add((Control) this.lblSubject);
      this.Controls.Add((Control) this.txtTo);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SendMessageDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Send";
      ((ISupportInitialize) this.btnToList).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
