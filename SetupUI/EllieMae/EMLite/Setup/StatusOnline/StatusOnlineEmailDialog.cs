// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StatusOnline.StatusOnlineEmailDialog
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
namespace EllieMae.EMLite.Setup.StatusOnline
{
  public class StatusOnlineEmailDialog : Form
  {
    private const string LOANLEVELCONSENT = "Loan Level Consent";
    private const string REQUEST = "Document Requests";
    private const string DISCLOSURE = "eDisclosures";
    private const string PRECLOSING = "Pre-Closing Documents";
    private const string SEND = "Sending Files";
    private const string STATUSONLINE = "Status Online";
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

    public StatusOnlineEmailDialog(
      Sessions.Session session,
      HtmlEmailTemplate template,
      bool readOnly)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.session = session;
      this.template = template;
      this.readOnly = readOnly;
      if (template.Type == HtmlEmailTemplateType.StatusOnline)
      {
        this.helpLink.HelpTag = "Personal Status Online";
        if (string.IsNullOrEmpty(template.OwnerID))
          this.helpLink.HelpTag = "Company Status Online";
      }
      this.initTemplateTypes();
      this.initTemplateLoanTypes();
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
        if (this.template.Type != HtmlEmailTemplateType.StatusOnline)
        {
          this.cboType.Items.Add((object) new FieldOption("Loan Level Consent", "Loan Level Consent"));
          this.cboType.Items.Add((object) new FieldOption("Document Requests", "Document Requests"));
          this.cboType.Items.Add((object) new FieldOption("eDisclosures", "eDisclosures"));
          this.cboType.Items.Add((object) new FieldOption("Pre-Closing Documents", "Pre-Closing Documents"));
          this.cboType.Items.Add((object) new FieldOption("Sending Files", "Sending Files"));
        }
        else
          this.cboType.Items.Add((object) new FieldOption("Status Online", "Status Online"));
      }
      this.cboType.SelectedIndex = this.getTemplateTypeSelection();
      if (this.template.Type == HtmlEmailTemplateType.Unknown)
        return;
      this.cboType.Enabled = false;
    }

    private void initTemplateLoanTypes()
    {
      if (this.cboLoan.Items.Count == 0)
      {
        this.cboLoan.Items.Add((object) new FieldOption("Non-Consumer Connect", "Non-Consumer Connect"));
        this.cboLoan.Items.Add((object) new FieldOption("Consumer Connect", "Consumer Connect"));
      }
      this.cboLoan.SelectedIndex = this.getTemplateLoanSelection();
      if (this.template.Loan == HtmlEmailLoanSourceType.Unknown)
        return;
      this.cboType.Enabled = false;
    }

    private int getTemplateTypeSelection()
    {
      if (this.cboType.Items.Count == 0)
        return -1;
      string str = string.Empty;
      switch (this.template.Type)
      {
        case HtmlEmailTemplateType.StatusOnline:
          str = "Status Online";
          break;
        case HtmlEmailTemplateType.RequestDocuments:
          str = "Document Requests";
          break;
        case HtmlEmailTemplateType.SendDocuments:
          str = "Sending Files";
          break;
        case HtmlEmailTemplateType.InitialDisclosures:
          str = "eDisclosures";
          break;
        case HtmlEmailTemplateType.LoanLevelConsent:
          str = "Loan Level Consent";
          break;
        case HtmlEmailTemplateType.PreClosing:
          str = "Pre-Closing Documents";
          break;
      }
      for (int index = 0; index < this.cboType.Items.Count; ++index)
      {
        if (((FieldOption) this.cboType.Items[index]).Value == str)
          return index;
      }
      return 0;
    }

    private int getTemplateLoanSelection()
    {
      if (this.cboLoan.Items.Count == 0)
        return -1;
      string str = string.Empty;
      switch (this.template.Loan)
      {
        case HtmlEmailLoanSourceType.NonConsumerConnect:
          str = "Non-Consumer Connect";
          break;
        case HtmlEmailLoanSourceType.ConsumerConnect:
          str = "Consumer Connect";
          break;
      }
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
      this.htmlEmailContent.AllowPersonalImages = this.template.Type != HtmlEmailTemplateType.StatusOnline || !string.IsNullOrEmpty(this.template.OwnerID) && this.session.UserInfo.PersonalStatusOnline;
      this.htmlEmailContent.ContentChanged += new EventHandler(this.HtmlEmailControl_ContentChanged);
      this.isDirty = false;
    }

    private bool validateTemplateData()
    {
      List<string> stringList = new List<string>();
      if (this.txtSubject.Text.Trim() == string.Empty)
        stringList.Add("You must specify a subject for this email template.");
      if (this.htmlEmailContent.HtmlBodyText.Trim() != string.Empty && this.htmlEmailContent.HtmlBodyText.Trim().Contains("<<Recipient Full Name >>") && !this.htmlEmailContent.IsConsumerConnect)
        stringList.Add("'Recipent Full Name' can only be used for Consumer Connect email templates.");
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
      if (this.template.Type == HtmlEmailTemplateType.Unknown)
      {
        FieldOption selectedItem = (FieldOption) this.cboType.SelectedItem;
        if (this.template.Loan == HtmlEmailLoanSourceType.Unknown)
        {
          switch (((FieldOption) this.cboLoan.SelectedItem).Value)
          {
            case "Non-Consumer Connect":
              this.template.Loan = HtmlEmailLoanSourceType.NonConsumerConnect;
              break;
            case "Consumer Connect":
              this.template.Loan = HtmlEmailLoanSourceType.ConsumerConnect;
              break;
          }
        }
        if (this.template.Loan == HtmlEmailLoanSourceType.ConsumerConnect)
        {
          switch (selectedItem.Value)
          {
            case "Loan Level Consent":
              this.template.Type = HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent;
              break;
            case "Document Requests":
              this.template.Type = HtmlEmailTemplateType.ConsumerConnectRequestDocuments;
              break;
            case "eDisclosures":
              this.template.Type = HtmlEmailTemplateType.ConsumerConnectInitialDisclosures;
              break;
            case "Pre-Closing Documents":
              this.template.Type = HtmlEmailTemplateType.ConsumerConnectPreClosing;
              break;
            case "Sending Files":
              this.template.Type = HtmlEmailTemplateType.ConsumerConnectSendDocuments;
              break;
          }
        }
        switch (selectedItem.Value)
        {
          case "Loan Level Consent":
            this.template.Type = HtmlEmailTemplateType.LoanLevelConsent;
            break;
          case "Document Requests":
            this.template.Type = HtmlEmailTemplateType.RequestDocuments;
            break;
          case "eDisclosures":
            this.template.Type = HtmlEmailTemplateType.InitialDisclosures;
            break;
          case "Pre-Closing Documents":
            this.template.Type = HtmlEmailTemplateType.PreClosing;
            break;
          case "Sending Files":
            this.template.Type = HtmlEmailTemplateType.SendDocuments;
            break;
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

    private void StatusOnlineEmailDialog_FormClosing(object sender, FormClosingEventArgs e)
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
        if (this.htmlEmailContent.HtmlBodyText == null || !this.htmlEmailContent.HtmlBodyText.Trim().Contains("<<Recipient Full Name >>"))
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "Warning: \n\n 'Recipent Full Name' can only be used for Consumer Connect email templates.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
      this.htmlEmailContent = new HtmlEmailControl();
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
      this.lblLoan.Text = "Template Type";
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
      this.htmlEmailContent.Size = new Size(572, 334);
      this.htmlEmailContent.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(597, 475);
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
      this.Name = nameof (StatusOnlineEmailDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Email Template Details";
      this.FormClosing += new FormClosingEventHandler(this.StatusOnlineEmailDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.EmailTemplateDetailsDialog_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
