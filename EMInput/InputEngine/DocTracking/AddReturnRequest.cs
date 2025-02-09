// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DocTracking.AddReturnRequest
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine.DocTracking
{
  public class AddReturnRequest : Form
  {
    private Sessions.Session session;
    private DocTrackingType _docTrackingType;
    private ContainerControl parentWindow;
    private string fieldsPrefix;
    private Hashtable docTrackingSettings;
    private IContainer components;
    private Panel panel_Main;
    private TextBox textBox_Phone;
    private TextBox textBox_Email;
    private TextBox textBox_Contact;
    private TextBox textBox_Organization;
    private TextBox textBox_RequestedBy;
    private Label label_CommentsLimit;
    private RichTextBox richTextBox_Comment;
    private Label label_Comments;
    private CheckBox checkBox_Phone;
    private CheckBox checkBox_Email;
    private Label label_MethodUsed;
    private Label label_Contact;
    private Label label_Organization;
    private Label label_RequestedBy;
    private Label label_RequestedDate;
    private CheckBox checkBox_FTP;
    private CheckBox checkBox_DOTMortgage;
    private Label label_SelectCheckBoxItem;
    private Button button_OK;
    private Button button_Cancel;
    private Label label_ErrorMessage;
    private StandardIconButton btnOrganization;
    private DatePickerCustom dpReceivedDate;

    public AddReturnRequest(
      Sessions.Session session,
      DocTrackingType docTrackingType,
      ContainerControl callForm,
      Hashtable settings)
    {
      this.InitializeComponent();
      this.session = session;
      this.docTrackingSettings = settings;
      this._docTrackingType = docTrackingType;
      this.parentWindow = callForm;
      this.fieldsPrefix = string.Format(DocTrackingUtils.FieldPrefix + "{0}.ReturnRequest.", (object) docTrackingType.ToString());
      this.InitialDefaultVlaues();
    }

    public AddReturnRequest(DocumentTrackingLog dtl)
    {
      this.InitializeComponent();
      this.EnableDisableAddRtnRqst(false);
      this.button_OK.Visible = false;
      this.button_Cancel.Text = "Close";
      this.InitDataFromLog(dtl);
    }

    private void button_OK_Click(object sender, EventArgs e)
    {
      if (this.ValidatePage())
      {
        this.Save();
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ErrorMessage.AddRetrunRequest_Validate_FillRequiredField, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private bool ValidatePage()
    {
      this.ClearValidation();
      bool flag = true;
      if (!this.checkBox_DOTMortgage.Checked && !this.checkBox_FTP.Checked)
      {
        this.checkBox_FTP.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        this.checkBox_DOTMortgage.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      if (string.IsNullOrWhiteSpace(this.textBox_RequestedBy.Text))
      {
        this.textBox_RequestedBy.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      if (string.IsNullOrEmpty(this.dpReceivedDate.Text))
      {
        this.dpReceivedDate.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        this.dpReceivedDate.Refresh();
        flag = false;
      }
      if (string.IsNullOrWhiteSpace(this.textBox_Organization.Text))
      {
        this.textBox_Organization.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      if (string.IsNullOrWhiteSpace(this.textBox_Contact.Text))
      {
        this.textBox_Contact.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      if (!this.checkBox_Email.Checked && !this.checkBox_Phone.Checked)
      {
        this.checkBox_Email.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        this.checkBox_Phone.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      return flag;
    }

    private void ClearValidation()
    {
      foreach (Control control in (ArrangedElementCollection) this.panel_Main.Controls)
      {
        if (control is TextBox || control is DatePicker)
          control.BackColor = SystemColors.Window;
        if (control is CheckBox)
          control.BackColor = SystemColors.Control;
      }
      this.dpReceivedDate.BackColor = SystemColors.Window;
      this.dpReceivedDate.Refresh();
    }

    private void InitialDefaultVlaues()
    {
      this.checkBox_DOTMortgage.Enabled = Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) "EnableDOT"]);
      this.checkBox_FTP.Enabled = Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) "EnableFTP"]);
      if (this._docTrackingType == DocTrackingType.DOT)
      {
        this.checkBox_DOTMortgage.Checked = true;
        this.checkBox_DOTMortgage.Enabled = false;
      }
      else
      {
        this.checkBox_FTP.Checked = true;
        this.checkBox_FTP.Enabled = false;
      }
      this.textBox_RequestedBy.Text = this.session.UserInfo.FullName;
      this.dpReceivedDate.Value = DateTime.Today;
      if (!string.IsNullOrWhiteSpace(this.session.LoanData.GetField(string.Format("{0}{1}", (object) this.fieldsPrefix, this.textBox_Organization.Tag))))
      {
        this.textBox_Organization.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) this.fieldsPrefix, this.textBox_Organization.Tag));
        this.textBox_Contact.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) this.fieldsPrefix, this.textBox_Contact.Tag));
        this.textBox_Email.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) this.fieldsPrefix, this.textBox_Email.Tag));
        this.textBox_Phone.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) this.fieldsPrefix, this.textBox_Phone.Tag));
      }
      else
      {
        string str = this._docTrackingType != DocTrackingType.DOT ? DocTrackingUtils.FTPShippingStatusPrefix : DocTrackingUtils.DOTShippingStatusPrefix;
        if (!string.IsNullOrWhiteSpace(this.session.LoanData.GetField(string.Format("{0}{1}", (object) str, this.textBox_Organization.Tag))))
        {
          this.textBox_Organization.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) str, this.textBox_Organization.Tag));
          this.textBox_Contact.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) str, this.textBox_Contact.Tag));
          this.textBox_Email.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) str, this.textBox_Email.Tag));
          this.textBox_Phone.Text = this.session.LoanData.GetField(string.Format("{0}{1}", (object) str, this.textBox_Phone.Tag));
        }
        else if (!string.IsNullOrWhiteSpace(this.session.LoanData.GetField("VEND.X387")))
        {
          this.textBox_Organization.Text = this.session.LoanData.GetField("VEND.X387");
          this.textBox_Contact.Text = this.session.LoanData.GetField("VEND.X392");
          this.textBox_Email.Text = this.session.LoanData.GetField("VEND.X394");
          this.textBox_Phone.Text = this.session.LoanData.GetField("VEND.X393");
        }
        else
        {
          if (string.IsNullOrWhiteSpace(this.session.LoanData.GetField("VEND.X263")))
            return;
          this.textBox_Organization.Text = this.session.LoanData.GetField("VEND.X263");
          this.textBox_Contact.Text = this.session.LoanData.GetField("VEND.X271");
          this.textBox_Email.Text = this.session.LoanData.GetField("VEND.X273");
          this.textBox_Phone.Text = this.session.LoanData.GetField("VEND.X272");
        }
      }
    }

    private void richTextBox_Comment_TextChanged(object sender, EventArgs e)
    {
      if (this.richTextBox_Comment.TextLength >= 500)
        this.richTextBox_Comment.Text = this.richTextBox_Comment.Text.Substring(0, 500);
      this.label_CommentsLimit.Text = string.Format("{0}{1}", (object) (500 - this.richTextBox_Comment.Text.Length).ToString(), (object) " chars left");
    }

    private void Save()
    {
      if (this.checkBox_DOTMortgage.Checked)
        this.SaveValuesByDocType(DocTrackingUtils.FieldPrefix + "DOT.ReturnRequest.");
      if (this.checkBox_FTP.Checked)
        this.SaveValuesByDocType(DocTrackingUtils.FieldPrefix + "FTP.ReturnRequest.");
      if (!string.IsNullOrWhiteSpace(this.richTextBox_Comment.Text))
        DocTrackingUtils.SaveComments(new DocumentTrackingComment()
        {
          UserName = this.session.UserInfo.FullName,
          LogDate = DateTime.Now,
          CommentText = this.richTextBox_Comment.Text
        });
      Session.LoanData.GetLogList().AddRecord((LogRecordBase) new DocumentTrackingLog(DateTime.Now)
      {
        ActionCd = DocTrackingActionCd.ActionReturnRequested,
        Action = DocTrackingUtils.Action_Return_Requested,
        LogDate = this.dpReceivedDate.Text.Trim(),
        LogBy = this.textBox_RequestedBy.Text.Trim(),
        Dot = this.checkBox_DOTMortgage.Checked,
        Ftp = this.checkBox_FTP.Checked,
        Organization = this.textBox_Organization.Text.Trim(),
        Contact = this.textBox_Contact.Text.Trim(),
        Email = this.checkBox_Email.Checked,
        Phone = this.checkBox_Phone.Checked,
        DocTrackingSnapshot = new Hashtable()
        {
          {
            (object) "Email",
            (object) this.textBox_Email.Text
          },
          {
            (object) "Phone",
            (object) this.textBox_Phone.Text
          },
          {
            (object) "Comments",
            (object) this.richTextBox_Comment.Text
          }
        }
      });
    }

    private void SaveValuesByDocType(string fieldPrefix)
    {
      this.session.LoanData.SetField(string.Format("{0}{1}", (object) fieldPrefix, (object) "RequestedBy"), this.textBox_RequestedBy.Text);
      if (this.dpReceivedDate.Value > DateTime.MinValue)
      {
        this.session.LoanData.SetField(string.Format("{0}{1}", (object) fieldPrefix, (object) "LastRequested"), this.dpReceivedDate.Value.ToShortDateString());
        DateTime followDate;
        if (fieldPrefix == DocTrackingUtils.FieldPrefix + "DOT.ReturnRequest.")
        {
          LoanData loanData = this.session.LoanData;
          string id = string.Format("{0}{1}", (object) fieldPrefix, (object) "NextFollowUpDate");
          followDate = this.GetFollowDate("DOT");
          string shortDateString = followDate.ToShortDateString();
          loanData.SetField(id, shortDateString);
        }
        if (fieldPrefix == DocTrackingUtils.FieldPrefix + "FTP.ReturnRequest.")
        {
          LoanData loanData = this.session.LoanData;
          string id = string.Format("{0}{1}", (object) fieldPrefix, (object) "NextFollowUpDate");
          followDate = this.GetFollowDate("FTP");
          string shortDateString = followDate.ToShortDateString();
          loanData.SetField(id, shortDateString);
        }
      }
      this.session.LoanData.SetField(string.Format("{0}{1}", (object) fieldPrefix, (object) "Organization"), this.textBox_Organization.Text);
      this.session.LoanData.SetField(string.Format("{0}{1}", (object) fieldPrefix, (object) "Contact"), this.textBox_Contact.Text);
      this.session.LoanData.SetField(string.Format("{0}{1}", (object) fieldPrefix, (object) "Email"), this.textBox_Email.Text);
      this.session.LoanData.SetField(string.Format("{0}{1}", (object) fieldPrefix, (object) "Phone"), this.textBox_Phone.Text);
    }

    private DateTime GetFollowDate(string type)
    {
      return DocTrackingUtils.GetPreviousClosestBusinessDay(this.dpReceivedDate.Value.AddDays((double) Utils.ParseInt(this.docTrackingSettings[(object) (type + "RtrnDaysBtwnFollowUp")])));
    }

    public DialogResult OverwriteMessage()
    {
      return MessageBox.Show(ErrorMessage.AddRetrunRequest_Validate_OverWrite, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
    }

    private void checkBox_DOTMortgage_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.checkBox_DOTMortgage.Checked || this._docTrackingType != DocTrackingType.FTP || !(this.session.LoanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.ReturnRequest.Received") != "//") || string.IsNullOrWhiteSpace(this.session.LoanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.ReturnRequest.Received")))
        return;
      if (this.OverwriteMessage() != DialogResult.OK)
      {
        this.checkBox_DOTMortgage.Checked = false;
      }
      else
      {
        if (!(this.parentWindow is DocumentTrackingManagement))
          return;
        ((DocumentTrackingManagement) this.parentWindow).GetDOTControls().GetReturnReqestControls().ClearReceiveDate();
      }
    }

    private void checkBox_FTP_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.checkBox_FTP.Checked || this._docTrackingType != DocTrackingType.DOT || !(this.session.LoanData.GetField(DocTrackingUtils.FieldPrefix + "FTP.ReturnRequest.Received") != "//") || string.IsNullOrWhiteSpace(this.session.LoanData.GetField(DocTrackingUtils.FieldPrefix + "FTP.ReturnRequest.Received")))
        return;
      if (this.OverwriteMessage() != DialogResult.OK)
      {
        this.checkBox_FTP.Checked = false;
      }
      else
      {
        if (!(this.parentWindow is DocumentTrackingManagement))
          return;
        ((DocumentTrackingManagement) this.parentWindow).GetFTPControls().GetReturnReqestControls().ClearReceiveDate();
      }
    }

    private void button_Cancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnOrganization_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact.CompanyName = this.textBox_Organization.Text;
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", rxContact.CompanyName, this.textBox_Contact.Text, rxContact, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.textBox_Organization.Text = rxBusinessContact.RxContactRecord.CompanyName;
        this.textBox_Contact.Text = rxBusinessContact.RxContactRecord.FirstName + " " + rxBusinessContact.RxContactRecord.LastName;
        this.textBox_Email.Text = rxBusinessContact.RxContactRecord.BizEmail;
        this.textBox_Phone.Text = rxBusinessContact.RxContactRecord.WorkPhone;
      }
    }

    private void EnableDisableAddRtnRqst(bool isEnable)
    {
      this.checkBox_DOTMortgage.Enabled = isEnable;
      this.checkBox_FTP.Enabled = isEnable;
      this.dpReceivedDate.Enabled = isEnable;
      this.textBox_RequestedBy.Enabled = isEnable;
      this.textBox_Organization.Enabled = isEnable;
      this.textBox_Contact.Enabled = isEnable;
      this.textBox_Email.Enabled = isEnable;
      this.textBox_Phone.Enabled = isEnable;
      this.checkBox_Email.Enabled = isEnable;
      this.checkBox_Phone.Enabled = isEnable;
      this.richTextBox_Comment.Enabled = isEnable;
      this.btnOrganization.Enabled = isEnable;
    }

    private void InitDataFromLog(DocumentTrackingLog dtl)
    {
      this.checkBox_DOTMortgage.Checked = dtl.Dot;
      this.checkBox_FTP.Checked = dtl.Ftp;
      this.dpReceivedDate.Text = dtl.LogDate;
      this.textBox_RequestedBy.Text = dtl.LogBy;
      this.textBox_Organization.Text = dtl.Organization;
      this.textBox_Contact.Text = dtl.Contact;
      this.textBox_Email.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Email"];
      this.textBox_Phone.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Phone"];
      this.checkBox_Email.Checked = dtl.Email;
      this.checkBox_Phone.Checked = dtl.Phone;
      this.richTextBox_Comment.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Comments"];
    }

    private void textBox_Phone_TextChanged(object sender, EventArgs e)
    {
      DocTrackingUtils.ValidatePhone((TextBox) sender, (Control) this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel_Main = new Panel();
      this.dpReceivedDate = new DatePickerCustom();
      this.btnOrganization = new StandardIconButton();
      this.textBox_Phone = new TextBox();
      this.textBox_Email = new TextBox();
      this.textBox_Contact = new TextBox();
      this.textBox_Organization = new TextBox();
      this.textBox_RequestedBy = new TextBox();
      this.label_CommentsLimit = new Label();
      this.richTextBox_Comment = new RichTextBox();
      this.label_Comments = new Label();
      this.checkBox_Phone = new CheckBox();
      this.checkBox_Email = new CheckBox();
      this.label_MethodUsed = new Label();
      this.label_Contact = new Label();
      this.label_Organization = new Label();
      this.label_RequestedBy = new Label();
      this.label_RequestedDate = new Label();
      this.checkBox_FTP = new CheckBox();
      this.checkBox_DOTMortgage = new CheckBox();
      this.label_SelectCheckBoxItem = new Label();
      this.button_OK = new Button();
      this.button_Cancel = new Button();
      this.label_ErrorMessage = new Label();
      this.panel_Main.SuspendLayout();
      ((ISupportInitialize) this.btnOrganization).BeginInit();
      this.SuspendLayout();
      this.panel_Main.BackColor = SystemColors.Control;
      this.panel_Main.Controls.Add((Control) this.dpReceivedDate);
      this.panel_Main.Controls.Add((Control) this.btnOrganization);
      this.panel_Main.Controls.Add((Control) this.textBox_Phone);
      this.panel_Main.Controls.Add((Control) this.textBox_Email);
      this.panel_Main.Controls.Add((Control) this.textBox_Contact);
      this.panel_Main.Controls.Add((Control) this.textBox_Organization);
      this.panel_Main.Controls.Add((Control) this.textBox_RequestedBy);
      this.panel_Main.Controls.Add((Control) this.label_CommentsLimit);
      this.panel_Main.Controls.Add((Control) this.richTextBox_Comment);
      this.panel_Main.Controls.Add((Control) this.label_Comments);
      this.panel_Main.Controls.Add((Control) this.checkBox_Phone);
      this.panel_Main.Controls.Add((Control) this.checkBox_Email);
      this.panel_Main.Controls.Add((Control) this.label_MethodUsed);
      this.panel_Main.Controls.Add((Control) this.label_Contact);
      this.panel_Main.Controls.Add((Control) this.label_Organization);
      this.panel_Main.Controls.Add((Control) this.label_RequestedBy);
      this.panel_Main.Controls.Add((Control) this.label_RequestedDate);
      this.panel_Main.Controls.Add((Control) this.checkBox_FTP);
      this.panel_Main.Controls.Add((Control) this.checkBox_DOTMortgage);
      this.panel_Main.Controls.Add((Control) this.label_SelectCheckBoxItem);
      this.panel_Main.Location = new Point(12, 12);
      this.panel_Main.Name = "panel_Main";
      this.panel_Main.Size = new Size(370, 462);
      this.panel_Main.TabIndex = 2;
      this.dpReceivedDate.Location = new Point(122, 112);
      this.dpReceivedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpReceivedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpReceivedDate.Name = "dpReceivedDate";
      this.dpReceivedDate.Size = new Size(113, 21);
      this.dpReceivedDate.TabIndex = 40;
      this.dpReceivedDate.Tag = (object) "Received";
      this.dpReceivedDate.ToolTip = "";
      this.dpReceivedDate.Value = new DateTime(0L);
      this.btnOrganization.BackColor = Color.Transparent;
      this.btnOrganization.Location = new Point(347, 160);
      this.btnOrganization.MouseDownImage = (Image) null;
      this.btnOrganization.Name = "btnOrganization";
      this.btnOrganization.Size = new Size(16, 16);
      this.btnOrganization.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnOrganization.TabIndex = 23;
      this.btnOrganization.TabStop = false;
      this.btnOrganization.Click += new EventHandler(this.btnOrganization_Click);
      this.textBox_Phone.Location = new Point(122, 265);
      this.textBox_Phone.Name = "textBox_Phone";
      this.textBox_Phone.Size = new Size(219, 20);
      this.textBox_Phone.TabIndex = 22;
      this.textBox_Phone.Tag = (object) "Phone";
      this.textBox_Phone.TextChanged += new EventHandler(this.textBox_Phone_TextChanged);
      this.textBox_Email.Location = new Point(122, 239);
      this.textBox_Email.Name = "textBox_Email";
      this.textBox_Email.Size = new Size(219, 20);
      this.textBox_Email.TabIndex = 21;
      this.textBox_Email.Tag = (object) "Email";
      this.textBox_Contact.Location = new Point(122, 178);
      this.textBox_Contact.Name = "textBox_Contact";
      this.textBox_Contact.Size = new Size(219, 20);
      this.textBox_Contact.TabIndex = 19;
      this.textBox_Contact.Tag = (object) "Contact";
      this.textBox_Organization.Location = new Point(122, 156);
      this.textBox_Organization.Name = "textBox_Organization";
      this.textBox_Organization.Size = new Size(219, 20);
      this.textBox_Organization.TabIndex = 18;
      this.textBox_Organization.Tag = (object) "Organization";
      this.textBox_RequestedBy.BackColor = SystemColors.Window;
      this.textBox_RequestedBy.Location = new Point(122, 134);
      this.textBox_RequestedBy.Name = "textBox_RequestedBy";
      this.textBox_RequestedBy.Size = new Size(219, 20);
      this.textBox_RequestedBy.TabIndex = 17;
      this.textBox_RequestedBy.Tag = (object) "RequestedBy";
      this.label_CommentsLimit.AutoSize = true;
      this.label_CommentsLimit.ForeColor = SystemColors.ActiveCaptionText;
      this.label_CommentsLimit.Location = new Point(270, 442);
      this.label_CommentsLimit.Name = "label_CommentsLimit";
      this.label_CommentsLimit.Size = new Size(71, 13);
      this.label_CommentsLimit.TabIndex = 15;
      this.label_CommentsLimit.Text = "500 chars left";
      this.label_CommentsLimit.Visible = false;
      this.richTextBox_Comment.Location = new Point(15, 311);
      this.richTextBox_Comment.MaxLength = 500;
      this.richTextBox_Comment.Name = "richTextBox_Comment";
      this.richTextBox_Comment.Size = new Size(326, 128);
      this.richTextBox_Comment.TabIndex = 14;
      this.richTextBox_Comment.Text = "";
      this.richTextBox_Comment.TextChanged += new EventHandler(this.richTextBox_Comment_TextChanged);
      this.label_Comments.AutoSize = true;
      this.label_Comments.Location = new Point(16, 295);
      this.label_Comments.Name = "label_Comments";
      this.label_Comments.Size = new Size(56, 13);
      this.label_Comments.TabIndex = 13;
      this.label_Comments.Text = "Comments";
      this.checkBox_Phone.AutoSize = true;
      this.checkBox_Phone.Location = new Point(15, 265);
      this.checkBox_Phone.Name = "checkBox_Phone";
      this.checkBox_Phone.Size = new Size(57, 17);
      this.checkBox_Phone.TabIndex = 11;
      this.checkBox_Phone.Text = "Phone";
      this.checkBox_Phone.UseVisualStyleBackColor = true;
      this.checkBox_Email.AutoSize = true;
      this.checkBox_Email.Location = new Point(15, 241);
      this.checkBox_Email.Name = "checkBox_Email";
      this.checkBox_Email.Size = new Size(51, 17);
      this.checkBox_Email.TabIndex = 10;
      this.checkBox_Email.Text = "Email";
      this.checkBox_Email.UseVisualStyleBackColor = true;
      this.label_MethodUsed.AutoSize = true;
      this.label_MethodUsed.Location = new Point(12, 224);
      this.label_MethodUsed.Name = "label_MethodUsed";
      this.label_MethodUsed.Size = new Size(71, 13);
      this.label_MethodUsed.TabIndex = 9;
      this.label_MethodUsed.Text = "Method Used";
      this.label_Contact.AutoSize = true;
      this.label_Contact.Location = new Point(12, 178);
      this.label_Contact.Name = "label_Contact";
      this.label_Contact.Size = new Size(44, 13);
      this.label_Contact.TabIndex = 8;
      this.label_Contact.Text = "Contact";
      this.label_Organization.AutoSize = true;
      this.label_Organization.Location = new Point(12, 156);
      this.label_Organization.Name = "label_Organization";
      this.label_Organization.Size = new Size(66, 13);
      this.label_Organization.TabIndex = 7;
      this.label_Organization.Text = "Organization";
      this.label_RequestedBy.AutoSize = true;
      this.label_RequestedBy.Location = new Point(12, 134);
      this.label_RequestedBy.Name = "label_RequestedBy";
      this.label_RequestedBy.Size = new Size(74, 13);
      this.label_RequestedBy.TabIndex = 6;
      this.label_RequestedBy.Text = "Requested By";
      this.label_RequestedDate.AutoSize = true;
      this.label_RequestedDate.Location = new Point(12, 112);
      this.label_RequestedDate.Name = "label_RequestedDate";
      this.label_RequestedDate.Size = new Size(85, 13);
      this.label_RequestedDate.TabIndex = 5;
      this.label_RequestedDate.Text = "Requested Date";
      this.checkBox_FTP.AutoSize = true;
      this.checkBox_FTP.Location = new Point(15, 63);
      this.checkBox_FTP.Name = "checkBox_FTP";
      this.checkBox_FTP.Size = new Size(102, 17);
      this.checkBox_FTP.TabIndex = 3;
      this.checkBox_FTP.Text = "Final Title Policy";
      this.checkBox_FTP.UseVisualStyleBackColor = true;
      this.checkBox_FTP.Click += new EventHandler(this.checkBox_FTP_CheckedChanged);
      this.checkBox_DOTMortgage.AutoSize = true;
      this.checkBox_DOTMortgage.Location = new Point(15, 40);
      this.checkBox_DOTMortgage.Name = "checkBox_DOTMortgage";
      this.checkBox_DOTMortgage.Size = new Size(99, 17);
      this.checkBox_DOTMortgage.TabIndex = 2;
      this.checkBox_DOTMortgage.Text = "DOT\\Mortgage";
      this.checkBox_DOTMortgage.UseVisualStyleBackColor = true;
      this.checkBox_DOTMortgage.Click += new EventHandler(this.checkBox_DOTMortgage_CheckedChanged);
      this.label_SelectCheckBoxItem.AutoSize = true;
      this.label_SelectCheckBoxItem.Location = new Point(12, 11);
      this.label_SelectCheckBoxItem.Name = "label_SelectCheckBoxItem";
      this.label_SelectCheckBoxItem.Size = new Size(152, 13);
      this.label_SelectCheckBoxItem.TabIndex = 1;
      this.label_SelectCheckBoxItem.Text = "Select the items you requested";
      this.button_OK.Location = new Point(214, 496);
      this.button_OK.Name = "button_OK";
      this.button_OK.Size = new Size(75, 23);
      this.button_OK.TabIndex = 3;
      this.button_OK.Text = "OK";
      this.button_OK.UseVisualStyleBackColor = true;
      this.button_OK.Click += new EventHandler(this.button_OK_Click);
      this.button_Cancel.Location = new Point(306, 496);
      this.button_Cancel.Name = "button_Cancel";
      this.button_Cancel.Size = new Size(75, 23);
      this.button_Cancel.TabIndex = 4;
      this.button_Cancel.Text = "Cancel";
      this.button_Cancel.UseVisualStyleBackColor = true;
      this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
      this.label_ErrorMessage.AutoSize = true;
      this.label_ErrorMessage.ForeColor = Color.FromArgb(192, 0, 0);
      this.label_ErrorMessage.Location = new Point(13, 472);
      this.label_ErrorMessage.Name = "label_ErrorMessage";
      this.label_ErrorMessage.Size = new Size(0, 13);
      this.label_ErrorMessage.TabIndex = 5;
      this.label_ErrorMessage.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(393, 531);
      this.Controls.Add((Control) this.label_ErrorMessage);
      this.Controls.Add((Control) this.button_Cancel);
      this.Controls.Add((Control) this.button_OK);
      this.Controls.Add((Control) this.panel_Main);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddReturnRequest);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Return Request";
      this.panel_Main.ResumeLayout(false);
      this.panel_Main.PerformLayout();
      ((ISupportInitialize) this.btnOrganization).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
