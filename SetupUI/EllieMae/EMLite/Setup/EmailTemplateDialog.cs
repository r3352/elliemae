// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EmailTemplateDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EmailTemplateDialog : Form
  {
    private const string LOANLEVELCONSENT = "Loan Level Consent";
    private const string REQUEST = "Document Requests";
    private const string DISCLOSURE = "eDisclosures";
    private const string PRECLOSING = "Pre-Closing Documents";
    private const string SEND = "Sending Files";
    private const string STATUSONLINE = "Status Online";
    private const string CCLOANLEVELCONSENT = "Consumer Connect Loan Level Consent";
    private const string CCREQUEST = "Consumer Connect Document Requests";
    private const string CCDISCLOSURE = "Consumer Connect eDisclosures";
    private const string CCPRECLOSING = "Consumer Connect Pre-Closing Documents";
    private const string CCSEND = "Consumer Connect Sending Files";
    private const string CCSTATUSONLINE = "Consumer Connect Status Online";
    private const string NONCC = "Non-Consumer Connect";
    private const string CC = "Consumer Connect";
    private Sessions.Session session;
    private HtmlEmailTemplate template;
    private bool readOnly;
    private bool isDirty;
    private IContainer components;
    private TextBox txtSubject;
    private Label lblSubject;
    private HtmlEmailControl htmlEmailContent;
    private Button btnSave;
    private Button btnCancel;
    private EMHelpLink helpLink;
    private Label lblType;
    private ComboBox cboType;
    private Label lblLoan;
    private ComboBox cboLoan;

    public string Type2Name(HtmlEmailTemplateType type)
    {
      string str = (string) null;
      switch (type)
      {
        case HtmlEmailTemplateType.StatusOnline:
          str = "Status Online";
          this.template.Loan = HtmlEmailLoanSourceType.NonConsumerConnect;
          break;
        case HtmlEmailTemplateType.RequestDocuments:
          str = "Document Requests";
          this.template.Loan = HtmlEmailLoanSourceType.NonConsumerConnect;
          break;
        case HtmlEmailTemplateType.SendDocuments:
          str = "Sending Files";
          this.template.Loan = HtmlEmailLoanSourceType.NonConsumerConnect;
          break;
        case HtmlEmailTemplateType.InitialDisclosures:
          str = "eDisclosures";
          this.template.Loan = HtmlEmailLoanSourceType.NonConsumerConnect;
          break;
        case HtmlEmailTemplateType.LoanLevelConsent:
          str = "Loan Level Consent";
          this.template.Loan = HtmlEmailLoanSourceType.NonConsumerConnect;
          break;
        case HtmlEmailTemplateType.PreClosing:
          str = "Pre-Closing Documents";
          this.template.Loan = HtmlEmailLoanSourceType.NonConsumerConnect;
          break;
        case HtmlEmailTemplateType.ConsumerConnectPreClosing:
          str = "Pre-Closing Documents";
          this.template.Loan = HtmlEmailLoanSourceType.ConsumerConnect;
          break;
        case HtmlEmailTemplateType.ConsumerConnectRequestDocuments:
          str = "Document Requests";
          this.template.Loan = HtmlEmailLoanSourceType.ConsumerConnect;
          break;
        case HtmlEmailTemplateType.ConsumerConnectSendDocuments:
          str = "Sending Files";
          this.template.Loan = HtmlEmailLoanSourceType.ConsumerConnect;
          break;
        case HtmlEmailTemplateType.ConsumerConnectInitialDisclosures:
          str = "eDisclosures";
          this.template.Loan = HtmlEmailLoanSourceType.ConsumerConnect;
          break;
        case HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent:
          str = "Loan Level Consent";
          this.template.Loan = HtmlEmailLoanSourceType.ConsumerConnect;
          break;
        case HtmlEmailTemplateType.ConsumerConnectStatusOnline:
          str = "Status Online";
          this.template.Loan = HtmlEmailLoanSourceType.ConsumerConnect;
          break;
      }
      return str;
    }

    public static HtmlEmailTemplateType Name2Type(string name, bool isCCLoanSource = false)
    {
      if (isCCLoanSource)
      {
        if (string.Compare(name, "Consumer Connect Loan Level Consent", StringComparison.CurrentCultureIgnoreCase) == 0)
          return HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent;
        if (string.Compare(name, "Consumer Connect eDisclosures", StringComparison.CurrentCultureIgnoreCase) == 0)
          return HtmlEmailTemplateType.ConsumerConnectInitialDisclosures;
        if (string.Compare(name, "Consumer Connect Pre-Closing Documents", StringComparison.CurrentCultureIgnoreCase) == 0)
          return HtmlEmailTemplateType.ConsumerConnectPreClosing;
        if (string.Compare(name, "Consumer Connect Document Requests", StringComparison.CurrentCultureIgnoreCase) == 0)
          return HtmlEmailTemplateType.ConsumerConnectRequestDocuments;
        if (string.Compare(name, "Consumer Connect Sending Files", StringComparison.CurrentCultureIgnoreCase) == 0)
          return HtmlEmailTemplateType.ConsumerConnectSendDocuments;
        if (string.Compare(name, "Consumer Connect Status Online", StringComparison.CurrentCultureIgnoreCase) == 0)
          return HtmlEmailTemplateType.ConsumerConnectStatusOnline;
      }
      if (string.Compare(name, "Loan Level Consent", StringComparison.CurrentCultureIgnoreCase) == 0)
        return HtmlEmailTemplateType.LoanLevelConsent;
      if (string.Compare(name, "eDisclosures", StringComparison.CurrentCultureIgnoreCase) == 0)
        return HtmlEmailTemplateType.InitialDisclosures;
      if (string.Compare(name, "Pre-Closing Documents", StringComparison.CurrentCultureIgnoreCase) == 0)
        return HtmlEmailTemplateType.PreClosing;
      if (string.Compare(name, "Document Requests", StringComparison.CurrentCultureIgnoreCase) == 0)
        return HtmlEmailTemplateType.RequestDocuments;
      if (string.Compare(name, "Sending Files", StringComparison.CurrentCultureIgnoreCase) == 0)
        return HtmlEmailTemplateType.SendDocuments;
      return string.Compare(name, "Status Online", StringComparison.CurrentCultureIgnoreCase) == 0 ? HtmlEmailTemplateType.StatusOnline : HtmlEmailTemplateType.Unknown;
    }

    public static string Loan2Name(HtmlEmailLoanSourceType loan)
    {
      string str = (string) null;
      switch (loan)
      {
        case HtmlEmailLoanSourceType.NonConsumerConnect:
          str = "Non-Consumer Connect";
          break;
        case HtmlEmailLoanSourceType.ConsumerConnect:
          str = "Consumer Connect";
          break;
      }
      return str;
    }

    public static HtmlEmailLoanSourceType Name2Loan(string name)
    {
      if (string.Compare(name, "Non-Consumer Connect", StringComparison.CurrentCultureIgnoreCase) == 0)
        return HtmlEmailLoanSourceType.NonConsumerConnect;
      return string.Compare(name, "Consumer Connect", StringComparison.CurrentCultureIgnoreCase) == 0 ? HtmlEmailLoanSourceType.ConsumerConnect : HtmlEmailLoanSourceType.Unknown;
    }

    public EmailTemplateDialog(Sessions.Session session, HtmlEmailTemplate template, bool readOnly)
    {
      this.htmlEmailContent = new HtmlEmailControl(session);
      this.InitializeComponent();
      this.helpLink.AssignSession(session);
      this.setWindowSize();
      this.session = session;
      this.template = template;
      this.readOnly = readOnly;
      this.helpLink.AssignSession(session);
      if (template.Type == (HtmlEmailTemplateType.StatusOnline | HtmlEmailTemplateType.ConsumerConnectStatusOnline))
      {
        this.helpLink.HelpTag = "Personal Status Online";
        if (string.IsNullOrEmpty(template.OwnerID))
          this.helpLink.HelpTag = "Company Status Online";
      }
      else
        this.helpLink.HelpTag = "HTML Email Templates";
      this.initTemplateTypes();
      this.initLoanTypes();
      this.loadTemplateData();
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.95);
        this.Height = Convert.ToInt32((double) form.Height * 0.95);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    private void initTemplateTypes()
    {
      if (this.cboType.Items.Count == 0)
      {
        foreach (HtmlEmailTemplateType type in this.template.Type == HtmlEmailTemplateType.StatusOnline || this.template.Type == HtmlEmailTemplateType.ConsumerConnectStatusOnline ? HtmlEmailTemplate.GetValidStatusOnlineTypes() : HtmlEmailTemplate.GetValidEmailTemplateTypes())
        {
          string text = this.Type2Name(type);
          if (text != null)
            this.cboType.Items.Add((object) new FieldOption(text, text));
        }
      }
      this.cboType.SelectedIndex = this.getTemplateTypeSelection();
      if (this.template.Type == HtmlEmailTemplateType.Unknown)
        return;
      this.cboType.Enabled = false;
    }

    private int getTemplateTypeSelection()
    {
      if (this.cboType.Items.Count == 0)
        return -1;
      string str = this.Type2Name(this.template.Type) ?? string.Empty;
      for (int index = 0; index < this.cboType.Items.Count; ++index)
      {
        if (((FieldOption) this.cboType.Items[index]).Value == str)
          return index;
      }
      return 0;
    }

    private void initLoanTypes()
    {
      if (this.cboLoan.Items.Count == 0)
      {
        foreach (HtmlEmailLoanSourceType validLoanSourceType in HtmlEmailTemplate.GetValidLoanSourceTypes())
        {
          string text = EmailTemplateDialog.Loan2Name(validLoanSourceType);
          if (text != null)
            this.cboLoan.Items.Add((object) new FieldOption(text, text));
        }
      }
      this.cboLoan.SelectedIndex = this.getTemplateLoanSelection();
      if (this.template.Subject.Trim() != string.Empty)
        this.cboLoan.Enabled = false;
      else
        this.template.Loan = HtmlEmailLoanSourceType.Unknown;
    }

    private int getTemplateLoanSelection()
    {
      if (this.cboLoan.Items.Count == 0)
        return -1;
      string str = EmailTemplateDialog.Loan2Name(this.template.Loan) ?? string.Empty;
      for (int index = 0; index < this.cboLoan.Items.Count; ++index)
      {
        if (((FieldOption) this.cboLoan.Items[index]).Value == str)
          return index;
      }
      return 0;
    }

    private void loadTemplateData()
    {
      this.txtSubject.Text = this.template.Subject;
      this.htmlEmailContent.LoadHtml(this.template.Html, this.readOnly);
      if (this.readOnly)
      {
        this.txtSubject.ReadOnly = true;
        this.htmlEmailContent.ShowToolbar = false;
        this.helpLink.Visible = false;
        this.btnSave.Visible = false;
        this.btnCancel.Text = "Close";
      }
      this.htmlEmailContent.AllowPersonalImages = this.template.Type != (HtmlEmailTemplateType.StatusOnline | HtmlEmailTemplateType.ConsumerConnectStatusOnline) || !string.IsNullOrEmpty(this.template.OwnerID) && this.session.UserInfo.PersonalStatusOnline;
      this.htmlEmailContent.ContentChanged += new EventHandler(this.HtmlEmailControl_ContentChanged);
      this.isDirty = false;
    }

    private bool validateTemplateData()
    {
      List<string> stringList = new List<string>();
      if (this.txtSubject.Text.Trim() == string.Empty)
        stringList.Add("You must specify a subject for this email template.");
      if (this.htmlEmailContent.HtmlBodyText != null && this.htmlEmailContent.HtmlBodyText.Trim().Contains("Recipient Full Name") && !this.htmlEmailContent.IsConsumerConnect)
        stringList.Add("'Recipient Full Name' can only be used for Consumer Connect email templates.");
      if (stringList.Count == 0)
        return true;
      string text = "You must fix the following items in order to save:";
      foreach (string str in stringList)
        text = text + "\n\n" + str;
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private void saveTemplateData()
    {
      this.template.Subject = this.txtSubject.Text.Trim();
      this.template.Html = this.htmlEmailContent.Html;
      if (this.template.Type == HtmlEmailTemplateType.StatusOnline && this.template.Loan == HtmlEmailLoanSourceType.Unknown)
      {
        HtmlEmailLoanSourceType emailLoanSourceType = EmailTemplateDialog.Name2Loan(((FieldOption) this.cboLoan.SelectedItem).Value);
        if (emailLoanSourceType != HtmlEmailLoanSourceType.Unknown)
          this.template.Loan = emailLoanSourceType;
        if (this.template.Loan == HtmlEmailLoanSourceType.ConsumerConnect)
          this.template.Type = HtmlEmailTemplateType.ConsumerConnectStatusOnline;
      }
      if (this.template.Type == HtmlEmailTemplateType.Unknown)
      {
        FieldOption selectedItem = (FieldOption) this.cboType.SelectedItem;
        HtmlEmailTemplateType emailTemplateType = EmailTemplateDialog.Name2Type(selectedItem.Value);
        if (emailTemplateType != HtmlEmailTemplateType.Unknown)
        {
          if (this.template.Loan == HtmlEmailLoanSourceType.Unknown)
          {
            HtmlEmailLoanSourceType emailLoanSourceType = EmailTemplateDialog.Name2Loan(((FieldOption) this.cboLoan.SelectedItem).Value);
            if (emailLoanSourceType != HtmlEmailLoanSourceType.Unknown)
              this.template.Loan = emailLoanSourceType;
          }
          if (this.template.Loan == HtmlEmailLoanSourceType.ConsumerConnect)
            emailTemplateType = EmailTemplateDialog.Name2Type("Consumer Connect " + selectedItem.Value, true);
          this.template.Type = emailTemplateType;
        }
      }
      this.session.ConfigurationManager.SaveHtmlEmailTemplate(this.template);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.validateTemplateData())
        return;
      this.saveTemplateData();
      this.isDirty = false;
      this.DialogResult = DialogResult.OK;
    }

    private void EmailTemplateDetailsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void HtmlEmailControl_ContentChanged(object sender, EventArgs e) => this.isDirty = true;

    private void txtSubject_TextChanged(object sender, EventArgs e) => this.isDirty = true;

    private void EmailTemplateDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.isDirty || DialogResult.Yes != Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        return;
      if (!this.validateTemplateData())
      {
        e.Cancel = true;
      }
      else
      {
        this.saveTemplateData();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void cboLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboLoan.SelectedItem.ToString() == "Non-Consumer Connect")
      {
        this.htmlEmailContent.IsConsumerConnect = false;
        if (this.htmlEmailContent.HtmlBodyText == null || !this.htmlEmailContent.HtmlBodyText.Trim().Contains("Recipient Full Name"))
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "Warning: \n\n 'Recipient Full Name' can only be used for Consumer Connect email templates.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.htmlEmailContent.IsConsumerConnect = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtSubject = new TextBox();
      this.lblSubject = new Label();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.lblType = new Label();
      this.cboType = new ComboBox();
      this.lblLoan = new Label();
      this.cboLoan = new ComboBox();
      this.helpLink = new EMHelpLink();
      this.SuspendLayout();
      this.txtSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSubject.Location = new Point(65, 67);
      this.txtSubject.Name = "txtSubject";
      this.txtSubject.Size = new Size(519, 20);
      this.txtSubject.TabIndex = 3;
      this.txtSubject.TextChanged += new EventHandler(this.txtSubject_TextChanged);
      this.lblSubject.AutoSize = true;
      this.lblSubject.BackColor = Color.Transparent;
      this.lblSubject.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSubject.Location = new Point(11, 70);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(48, 14);
      this.lblSubject.TabIndex = 2;
      this.lblSubject.Text = "Subject";
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(430, 439);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(510, 439);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblType.AutoSize = true;
      this.lblType.BackColor = Color.Transparent;
      this.lblType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblType.Location = new Point(13, 40);
      this.lblType.Name = "lblType";
      this.lblType.Size = new Size(87, 14);
      this.lblType.TabIndex = 0;
      this.lblType.Text = "Template Type";
      this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboType.FormattingEnabled = true;
      this.cboType.Location = new Point(121, 37);
      this.cboType.Name = "cboType";
      this.cboType.Size = new Size(159, 22);
      this.cboType.TabIndex = 1;
      this.lblLoan.AutoSize = true;
      this.lblLoan.BackColor = Color.Transparent;
      this.lblLoan.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLoan.Location = new Point(13, 10);
      this.lblLoan.Name = "lblLoan";
      this.lblLoan.Size = new Size(87, 14);
      this.lblLoan.TabIndex = 0;
      this.lblLoan.Text = "Loan Source";
      this.cboLoan.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLoan.FormattingEnabled = true;
      this.cboLoan.Location = new Point(121, 7);
      this.cboLoan.Name = "cboLoan";
      this.cboLoan.Size = new Size(159, 22);
      this.cboLoan.TabIndex = 1;
      this.cboLoan.SelectedIndexChanged += new EventHandler(this.cboLoan_SelectedIndexChanged);
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.Location = new Point(12, 441);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 5;
      this.htmlEmailContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.htmlEmailContent.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailContent.Location = new Point(12, 100);
      this.htmlEmailContent.Name = "htmlEmailContent";
      this.htmlEmailContent.Size = new Size(572, 331);
      this.htmlEmailContent.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(597, 472);
      this.Controls.Add((Control) this.cboType);
      this.Controls.Add((Control) this.lblType);
      this.Controls.Add((Control) this.cboLoan);
      this.Controls.Add((Control) this.lblLoan);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.htmlEmailContent);
      this.Controls.Add((Control) this.txtSubject);
      this.Controls.Add((Control) this.lblSubject);
      this.Font = new Font("Arial", 8.25f);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (EmailTemplateDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Email Template Details";
      this.FormClosing += new FormClosingEventHandler(this.EmailTemplateDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.EmailTemplateDetailsDialog_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
