// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddInitialRequest
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
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
namespace EllieMae.EMLite.InputEngine
{
  public class AddInitialRequest : Form
  {
    private DocTrackingType docTrackingType;
    private Sessions.Session session;
    private LoanData loanData;
    private Hashtable settings;
    private Hashtable docTypeClearReceivedDt;
    private string prefix;
    private IContainer components;
    private Button btnOk;
    private Button btnCancel;
    private Label label3;
    private CheckBox ckPhone;
    private CheckBox ckEmail;
    private Label label2;
    private TextBox txtPhone;
    private TextBox txtEmail;
    private TextBox txtContact;
    private Label label9;
    private StandardIconButton btnOrganization;
    private TextBox txtOrganization;
    private Label label8;
    private TextBox txtRequestedBy;
    private Label label7;
    private Label label4;
    private Label label1;
    private RichTextBox rtComments;
    private DatePickerCustom dpRequestedDt;
    private Panel pnlDocType;
    private CheckBox ckFTP;
    private CheckBox ckDot;

    public AddInitialRequest(DocTrackingType docTrackingType)
    {
      this.InitializeComponent();
      this.docTrackingType = docTrackingType;
      this.session = DocTrackingUtils.Session;
      this.loanData = this.session.LoanData;
      this.settings = DocTrackingUtils.DocTrackingSettings;
      this.prefix = DocTrackingUtils.GetPrefix(docTrackingType, DocTrackingRequestType.InitialRequest);
      this.InitData();
    }

    public AddInitialRequest(DocumentTrackingLog dtl)
    {
      this.InitializeComponent();
      this.btnOk.Visible = false;
      this.btnCancel.Text = "Close";
      this.InitDataFromLog(dtl);
      this.EnableDisableAddInitialRequest(false);
    }

    private void InitData()
    {
      this.docTypeClearReceivedDt = new Hashtable();
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        string key = control.Tag.ToString();
        if (key != this.docTrackingType.ToString())
        {
          this.docTypeClearReceivedDt[(object) key] = (object) false;
          control.Enabled = Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) ("Enable" + key)]);
        }
        else
        {
          control.Checked = true;
          control.Enabled = false;
        }
      }
      this.dpRequestedDt.Value = DateTime.Now;
      this.txtRequestedBy.Text = this.session.UserInfo.FullName;
      if (!string.IsNullOrEmpty(this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization))))
      {
        this.txtOrganization.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization));
        this.txtContact.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtContact));
        this.txtEmail.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtEmail));
        this.txtPhone.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPhone));
      }
      else if (this.loanData.GetField("2626") == LoanChannel.Correspondent.ToString())
      {
        this.txtOrganization.Text = this.loanData.GetField("TPO.X14");
        this.txtContact.Text = this.loanData.GetField("TPO.X28");
        this.txtEmail.Text = this.loanData.GetField("TPO.X29");
        this.txtPhone.Text = this.loanData.GetField("TPO.X22");
      }
      else if (this.docTrackingType == DocTrackingType.DOT)
      {
        this.txtOrganization.Text = this.loanData.GetField("610");
        this.txtContact.Text = this.loanData.GetField("611");
        this.txtEmail.Text = this.loanData.GetField("87");
        this.txtPhone.Text = this.loanData.GetField("615");
      }
      else
      {
        if (this.docTrackingType != DocTrackingType.FTP)
          return;
        this.txtOrganization.Text = this.loanData.GetField("411");
        this.txtContact.Text = this.loanData.GetField("416");
        this.txtEmail.Text = this.loanData.GetField("88");
        this.txtPhone.Text = this.loanData.GetField("417");
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, DocTrackingUtils.Validate_Error_message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        foreach (string key in (IEnumerable) this.docTypeClearReceivedDt.Keys)
        {
          if (Utils.ParseBoolean(this.docTypeClearReceivedDt[(object) key]))
            this.loanData.SetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.InitialRequest.Received", (object) key), "");
        }
        foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
        {
          if (control.Checked)
          {
            string str = control.Tag.ToString();
            string prefix = DocTrackingUtils.GetPrefix((DocTrackingType) Enum.Parse(typeof (DocTrackingType), str, true), DocTrackingRequestType.InitialRequest);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.dpRequestedDt), this.dpRequestedDt.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtRequestedBy), this.txtRequestedBy.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtOrganization), this.txtOrganization.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtContact), this.txtContact.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtEmail), this.txtEmail.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtPhone), this.txtPhone.Text);
            DateTime date = this.dpRequestedDt.Value.AddDays((double) Utils.ParseInt(this.settings[(object) string.Format("{0}InitReqDaysBtwnFollowUp", (object) str)]));
            date = DocTrackingUtils.GetPreviousClosestBusinessDay(date);
            this.loanData.SetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.InitialRequest.NextFollowUpDate", (object) str), date.ToShortDateString());
          }
        }
        DocumentTrackingLog rec = new DocumentTrackingLog(DateTime.Now)
        {
          ActionCd = DocTrackingActionCd.ActionInitialRequested,
          Action = DocTrackingUtils.Action_Initial_Requested,
          LogDate = this.dpRequestedDt.Text,
          LogBy = this.txtRequestedBy.Text,
          Dot = this.ckDot.Checked,
          Ftp = this.ckFTP.Checked,
          Organization = this.txtOrganization.Text,
          Contact = this.txtContact.Text,
          Email = this.ckEmail.Checked,
          Phone = this.ckPhone.Checked
        };
        Hashtable hashtable = new Hashtable();
        hashtable.Add((object) "Email", (object) this.txtEmail.Text);
        hashtable.Add((object) "Phone", (object) this.txtPhone.Text);
        hashtable.Add((object) "Comments", (object) this.rtComments.Text);
        if (!string.IsNullOrEmpty(this.rtComments.Text))
          DocTrackingUtils.SaveComments(new DocumentTrackingComment()
          {
            UserName = this.session.UserInfo.FullName,
            LogDate = DateTime.Now,
            CommentText = this.rtComments.Text
          });
        rec.DocTrackingSnapshot = hashtable;
        this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool validateData()
    {
      bool flag = true;
      if (!this.ckDot.Checked && !this.ckFTP.Checked)
      {
        DocTrackingUtils.HilightFields((Control) this.ckDot, true);
        DocTrackingUtils.HilightFields((Control) this.ckFTP, true);
        flag = false;
      }
      else
      {
        DocTrackingUtils.HilightFields((Control) this.ckDot, false);
        DocTrackingUtils.HilightFields((Control) this.ckFTP, false);
      }
      if (string.IsNullOrEmpty(this.dpRequestedDt.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.dpRequestedDt, true);
        this.dpRequestedDt.Refresh();
        flag = false;
      }
      else
      {
        DocTrackingUtils.HilightFields((Control) this.dpRequestedDt, false);
        this.dpRequestedDt.Refresh();
      }
      if (string.IsNullOrEmpty(this.txtRequestedBy.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtRequestedBy, true);
        flag = false;
      }
      else
        DocTrackingUtils.HilightFields((Control) this.txtRequestedBy, false);
      if (string.IsNullOrEmpty(this.txtOrganization.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtOrganization, true);
        flag = false;
      }
      else
        DocTrackingUtils.HilightFields((Control) this.txtOrganization, false);
      if (string.IsNullOrEmpty(this.txtContact.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtContact, true);
        flag = false;
      }
      else
        DocTrackingUtils.HilightFields((Control) this.txtContact, false);
      if (!this.ckEmail.Checked && !this.ckPhone.Checked)
      {
        DocTrackingUtils.HilightFields((Control) this.ckEmail, true);
        DocTrackingUtils.HilightFields((Control) this.ckPhone, true);
        flag = false;
      }
      else
      {
        DocTrackingUtils.HilightFields((Control) this.ckEmail, false);
        DocTrackingUtils.HilightFields((Control) this.ckPhone, false);
      }
      if (!string.IsNullOrEmpty(this.txtEmail.Text.Trim()) && !Utils.ValidateEmail(this.txtEmail.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtEmail, true);
        flag = false;
      }
      else
        DocTrackingUtils.HilightFields((Control) this.txtEmail, false);
      return flag;
    }

    private void btnOrganization_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact.CompanyName = this.txtOrganization.Text.Trim();
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", rxContact.CompanyName, "", rxContact, true, true, CRMRoleType.NotFound, true, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
        {
          this.session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
          this.txtOrganization.Text = rxContactRecord[RolodexField.Company];
          this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization), this.txtOrganization.Text);
          this.txtContact.Text = rxContactRecord[RolodexField.Name];
          this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtContact), this.txtContact.Text);
          this.txtPhone.Text = rxContactRecord[RolodexField.Phone];
          this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPhone), this.txtPhone.Text);
          this.txtEmail.Text = rxContactRecord[RolodexField.Email];
          this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtEmail), this.txtEmail.Text);
        }
      }
    }

    private void ckDocType_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      string key = checkBox.Tag.ToString();
      if (this.loanData == null || !checkBox.Checked || Utils.ParseBoolean(this.docTypeClearReceivedDt[(object) key]) || !(Utils.ParseDate((object) this.loanData.GetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.InitialRequest.Received", (object) key.ToString()))) != DateTime.MinValue))
        return;
      if (Utils.Dialog((IWin32Window) this, DocTrackingUtils.InitialRequest_AddRequest, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        this.docTypeClearReceivedDt[(object) key] = (object) true;
      else
        checkBox.Checked = false;
    }

    private void txtPhone_TextChanged(object sender, EventArgs e)
    {
      DocTrackingUtils.ValidatePhone((TextBox) sender, (Control) this);
    }

    private void InitDataFromLog(DocumentTrackingLog dtl)
    {
      this.ckDot.Checked = dtl.Dot;
      this.ckFTP.Checked = dtl.Ftp;
      this.dpRequestedDt.Value = Utils.ParseDate((object) dtl.LogDate);
      this.txtRequestedBy.Text = dtl.LogBy;
      this.txtOrganization.Text = dtl.Organization;
      this.txtContact.Text = dtl.Contact;
      this.ckEmail.Checked = dtl.Email;
      this.ckPhone.Checked = dtl.Phone;
      Hashtable trackingSnapshot = dtl.DocTrackingSnapshot;
      if (trackingSnapshot == null)
        return;
      this.txtEmail.Text = (string) trackingSnapshot[(object) "Email"];
      this.txtPhone.Text = (string) trackingSnapshot[(object) "Phone"];
      this.rtComments.Text = (string) trackingSnapshot[(object) "Comments"];
    }

    private void EnableDisableAddInitialRequest(bool isEnable)
    {
      this.ckDot.Enabled = isEnable;
      this.ckFTP.Enabled = isEnable;
      this.dpRequestedDt.Enabled = isEnable;
      this.txtRequestedBy.Enabled = isEnable;
      this.txtOrganization.Enabled = isEnable;
      this.txtContact.Enabled = isEnable;
      this.ckEmail.Enabled = isEnable;
      this.ckPhone.Enabled = isEnable;
      this.txtEmail.Enabled = isEnable;
      this.txtPhone.Enabled = isEnable;
      this.rtComments.Enabled = isEnable;
      this.btnOrganization.Enabled = isEnable;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.label3 = new Label();
      this.ckPhone = new CheckBox();
      this.ckEmail = new CheckBox();
      this.label2 = new Label();
      this.txtPhone = new TextBox();
      this.txtEmail = new TextBox();
      this.txtContact = new TextBox();
      this.label9 = new Label();
      this.btnOrganization = new StandardIconButton();
      this.txtOrganization = new TextBox();
      this.label8 = new Label();
      this.txtRequestedBy = new TextBox();
      this.label7 = new Label();
      this.label4 = new Label();
      this.label1 = new Label();
      this.rtComments = new RichTextBox();
      this.pnlDocType = new Panel();
      this.ckFTP = new CheckBox();
      this.ckDot = new CheckBox();
      this.dpRequestedDt = new DatePickerCustom();
      ((ISupportInitialize) this.btnOrganization).BeginInit();
      this.pnlDocType.SuspendLayout();
      this.SuspendLayout();
      this.btnOk.Location = new Point(189, 445);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Location = new Point(270, 445);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(29, 297);
      this.label3.Name = "label3";
      this.label3.Size = new Size(56, 13);
      this.label3.TabIndex = 52;
      this.label3.Text = "Comments";
      this.ckPhone.AutoSize = true;
      this.ckPhone.Location = new Point(32, 261);
      this.ckPhone.Name = "ckPhone";
      this.ckPhone.Size = new Size(57, 17);
      this.ckPhone.TabIndex = 51;
      this.ckPhone.Text = "Phone";
      this.ckPhone.UseVisualStyleBackColor = true;
      this.ckEmail.AutoSize = true;
      this.ckEmail.Location = new Point(32, 235);
      this.ckEmail.Name = "ckEmail";
      this.ckEmail.Size = new Size(51, 17);
      this.ckEmail.TabIndex = 50;
      this.ckEmail.Text = "Email";
      this.ckEmail.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(29, 212);
      this.label2.Name = "label2";
      this.label2.Size = new Size(69, 13);
      this.label2.TabIndex = 49;
      this.label2.Text = "Method used";
      this.txtPhone.Location = new Point(120, 259);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(210, 20);
      this.txtPhone.TabIndex = 48;
      this.txtPhone.Tag = (object) "Phone";
      this.txtPhone.TextChanged += new EventHandler(this.txtPhone_TextChanged);
      this.txtEmail.Location = new Point(120, 233);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(210, 20);
      this.txtEmail.TabIndex = 47;
      this.txtEmail.Tag = (object) "Email";
      this.txtContact.Location = new Point(120, 177);
      this.txtContact.Name = "txtContact";
      this.txtContact.Size = new Size(210, 20);
      this.txtContact.TabIndex = 46;
      this.txtContact.Tag = (object) "Contact";
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(29, 180);
      this.label9.Name = "label9";
      this.label9.Size = new Size(44, 13);
      this.label9.TabIndex = 45;
      this.label9.Text = "Contact";
      this.btnOrganization.BackColor = Color.Transparent;
      this.btnOrganization.Location = new Point(336, 153);
      this.btnOrganization.MouseDownImage = (Image) null;
      this.btnOrganization.Name = "btnOrganization";
      this.btnOrganization.Size = new Size(16, 16);
      this.btnOrganization.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnOrganization.TabIndex = 44;
      this.btnOrganization.TabStop = false;
      this.btnOrganization.Click += new EventHandler(this.btnOrganization_Click);
      this.txtOrganization.Location = new Point(120, 151);
      this.txtOrganization.Name = "txtOrganization";
      this.txtOrganization.Size = new Size(210, 20);
      this.txtOrganization.TabIndex = 43;
      this.txtOrganization.Tag = (object) "Organization";
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Location = new Point(29, 153);
      this.label8.Name = "label8";
      this.label8.Size = new Size(66, 13);
      this.label8.TabIndex = 42;
      this.label8.Text = "Organization";
      this.txtRequestedBy.Location = new Point(120, 125);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.Size = new Size(210, 20);
      this.txtRequestedBy.TabIndex = 41;
      this.txtRequestedBy.Tag = (object) "RequestedBy";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(29, (int) sbyte.MaxValue);
      this.label7.Name = "label7";
      this.label7.Size = new Size(73, 13);
      this.label7.TabIndex = 40;
      this.label7.Text = "Requested by";
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(29, 101);
      this.label4.Name = "label4";
      this.label4.Size = new Size(85, 13);
      this.label4.TabIndex = 38;
      this.label4.Text = "Requested Date";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(29, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(152, 13);
      this.label1.TabIndex = 34;
      this.label1.Text = "Select the items you requested";
      this.rtComments.Location = new Point(32, 314);
      this.rtComments.MaxLength = 500;
      this.rtComments.Name = "rtComments";
      this.rtComments.Size = new Size(298, 95);
      this.rtComments.TabIndex = 53;
      this.rtComments.Text = "";
      this.pnlDocType.Controls.Add((Control) this.ckFTP);
      this.pnlDocType.Controls.Add((Control) this.ckDot);
      this.pnlDocType.Location = new Point(32, 37);
      this.pnlDocType.Name = "pnlDocType";
      this.pnlDocType.Size = new Size(200, 56);
      this.pnlDocType.TabIndex = 55;
      this.ckFTP.AutoSize = true;
      this.ckFTP.Location = new Point(3, 26);
      this.ckFTP.Name = "ckFTP";
      this.ckFTP.Size = new Size(102, 17);
      this.ckFTP.TabIndex = 38;
      this.ckFTP.Tag = (object) "FTP";
      this.ckFTP.Text = "Final Title Policy";
      this.ckFTP.UseVisualStyleBackColor = true;
      this.ckFTP.CheckedChanged += new EventHandler(this.ckDocType_CheckedChanged);
      this.ckDot.AutoSize = true;
      this.ckDot.Location = new Point(3, 3);
      this.ckDot.Name = "ckDot";
      this.ckDot.Size = new Size(99, 17);
      this.ckDot.TabIndex = 37;
      this.ckDot.Tag = (object) "DOT";
      this.ckDot.Text = "DOT/Mortgage";
      this.ckDot.UseVisualStyleBackColor = true;
      this.ckDot.CheckedChanged += new EventHandler(this.ckDocType_CheckedChanged);
      this.dpRequestedDt.Location = new Point(120, 99);
      this.dpRequestedDt.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpRequestedDt.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpRequestedDt.Name = "dpRequestedDt";
      this.dpRequestedDt.Size = new Size(113, 21);
      this.dpRequestedDt.TabIndex = 54;
      this.dpRequestedDt.Tag = (object) "LastRequested";
      this.dpRequestedDt.ToolTip = "";
      this.dpRequestedDt.Value = new DateTime(0L);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(375, 485);
      this.Controls.Add((Control) this.pnlDocType);
      this.Controls.Add((Control) this.dpRequestedDt);
      this.Controls.Add((Control) this.rtComments);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.ckPhone);
      this.Controls.Add((Control) this.ckEmail);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtPhone);
      this.Controls.Add((Control) this.txtEmail);
      this.Controls.Add((Control) this.txtContact);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.btnOrganization);
      this.Controls.Add((Control) this.txtOrganization);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.txtRequestedBy);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddInitialRequest);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Initial Request";
      ((ISupportInitialize) this.btnOrganization).EndInit();
      this.pnlDocType.ResumeLayout(false);
      this.pnlDocType.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
