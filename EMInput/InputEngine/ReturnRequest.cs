// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ReturnRequest
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine.DocTracking;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ReturnRequest : UserControl
  {
    private LoanData loanData;
    private string prefix;
    private DocTrackingType docTrackingType;
    private bool intermidiateData;
    private bool deleteBackKey;
    private IContainer components;
    private Label lblRetrunRequest1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnAddRequest;
    private Button btnAddReceipts;
    private GradientPanel gradientPanel1;
    private Label lblNextFollowupDt;
    private DatePicker dpNextFollowupDate;
    private GradientPanel gradientPanel2;
    private Label lblReturnRequest2;
    private Label label4;
    private DatePicker dpReceivedDate;
    private Label label6;
    private DatePicker dpLastRequested;
    private Label label7;
    private TextBox txtRequestedBy;
    private Label label8;
    private TextBox txtOrganization;
    private StandardIconButton btnOrganization;
    private Label label9;
    private TextBox txtContact;
    private Label label10;
    private TextBox txtEmail;
    private Label label11;
    private TextBox txtPhone;
    private GradientPanel gradientPanel3;

    public ReturnRequest(DocTrackingType docTrackingType)
    {
      this.InitializeComponent();
      this.loanData = DocTrackingUtils.Session.LoanData;
      this.docTrackingType = docTrackingType;
      this.prefix = DocTrackingUtils.FieldPrefix + (object) docTrackingType + ".ReturnRequest.";
      this.InitData();
    }

    public void InitData()
    {
      this.dpNextFollowupDate.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpNextFollowupDate));
      this.dpReceivedDate.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate));
      this.dpLastRequested.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpLastRequested));
      this.txtRequestedBy.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtRequestedBy));
      this.txtOrganization.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization));
      this.txtContact.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtContact));
      this.txtEmail.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtEmail));
      this.txtPhone.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPhone));
    }

    public void EnableDisableReturnRequst(bool isEnable)
    {
      this.dpNextFollowupDate.Enabled = false;
      this.dpReceivedDate.Enabled = false;
      this.dpLastRequested.Enabled = false;
      this.txtRequestedBy.Enabled = false;
      this.txtOrganization.Enabled = false;
      this.txtContact.Enabled = false;
      this.txtEmail.Enabled = false;
      this.txtPhone.Enabled = false;
      this.btnAddReceipts.Enabled = isEnable;
      this.btnAddRequest.Enabled = isEnable;
      this.btnOrganization.Enabled = false;
      this.btnOrganization.Hide();
    }

    private void btnOrganization_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact.CompanyName = this.txtOrganization.Text;
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", rxContact.CompanyName, this.txtContact.Text, rxContact, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.txtOrganization.Text = rxBusinessContact.RxContactRecord.CompanyName;
        this.txtContact.Text = rxBusinessContact.RxContactRecord.FirstName + " " + rxBusinessContact.RxContactRecord.LastName;
        this.txtEmail.Text = rxBusinessContact.RxContactRecord.BizEmail;
        this.txtPhone.Text = rxBusinessContact.RxContactRecord.WorkPhone;
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization), this.txtOrganization.Text);
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtContact), this.txtContact.Text);
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtEmail), this.txtEmail.Text);
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPhone), this.txtPhone.Text);
      }
    }

    private void freeEntryDate_ValueChanged(object sender, EventArgs e)
    {
      DatePicker datePicker = (DatePicker) sender;
      try
      {
        if (datePicker.Value != DateTime.MinValue)
          this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) datePicker), datePicker.Value.ToString("MM/dd/yyyy"));
        else
          this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) datePicker), "");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.loanData.SetField(string.Concat(datePicker.Tag), "");
        datePicker.Text = "";
      }
    }

    private void btnAddRequest_Click(object sender, EventArgs e)
    {
      if (this.dpReceivedDate.Value > DateTime.MinValue && Utils.Dialog((IWin32Window) this, ErrorMessage.AddRetrunRequest_Validate_OverWrite, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      this.dpReceivedDate.Text = string.Empty;
      this.loanData.SetField(string.Format("{0}{1}", (object) this.prefix, (object) "Received"), string.Empty);
      AddReturnRequest addReturnRequest = new AddReturnRequest(DocTrackingUtils.Session, this.docTrackingType, (ContainerControl) DocTrackingUtils.DocTrackingManagementForm, DocTrackingUtils.DocTrackingSettings);
      int num = (int) addReturnRequest.ShowDialog();
      if (addReturnRequest.DialogResult != DialogResult.OK)
        return;
      DocTrackingUtils.DocTrackingManagementForm.RefreshContent();
    }

    private void control_ValueChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) textBox), textBox.Text);
    }

    private void phone_TextChanged(object sender, EventArgs e)
    {
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        FieldFormat fieldFormat;
        FieldFormat dataFormat = fieldFormat = FieldFormat.PHONE;
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
        if (!needsUpdate)
          return;
        this.intermidiateData = true;
        textBox.Text = str;
        textBox.SelectionStart = str.Length;
      }
    }

    private void phone_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void phone_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) textBox), textBox.Text.Trim());
    }

    private void email_Leave(object sender, EventArgs e)
    {
      TextBox control = (TextBox) sender;
      if (DocTrackingUtils.ValidateEmail(control, (Control) this))
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) control), control.Text.Trim());
      else
        control.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) control));
    }

    private void nxtFollowupDate_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.dpReceivedDate.Text.Trim()) || string.IsNullOrEmpty(this.dpNextFollowupDate.Text.Trim()))
        return;
      if (Utils.Dialog((IWin32Window) this, DocTrackingUtils.ReturnRequest_Override, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
      {
        this.dpReceivedDate.Text = "";
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate), "");
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpNextFollowupDate), this.dpNextFollowupDate.Text);
      }
      else
      {
        this.dpNextFollowupDate.Text = "";
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpNextFollowupDate), "");
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate), this.dpReceivedDate.Text);
      }
    }

    public DateTime GetReceiveDate() => this.dpReceivedDate.Value;

    public void ClearReceiveDate()
    {
      this.dpReceivedDate.Text = string.Empty;
      this.loanData.SetField(string.Format("{0}{1}", (object) this.prefix, (object) "Received"), "");
    }

    private void btnAddReceipts_Click(object sender, EventArgs e)
    {
      string field = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate));
      if (!string.IsNullOrEmpty(field) && field != "//" && Utils.Dialog((IWin32Window) this, DocTrackingUtils.ReturnReceipt_Override, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      using (AddReturnReceipt addReturnReceipt = new AddReturnReceipt(this.docTrackingType, DocTrackingUtils.Session))
      {
        int num = (int) addReturnReceipt.ShowDialog();
        if (addReturnReceipt.DialogResult != DialogResult.OK)
          return;
        this.InitData();
        DocTrackingUtils.DocTrackingManagementForm.RefreshContent();
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
      this.lblRetrunRequest1 = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnAddRequest = new Button();
      this.btnAddReceipts = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.lblReturnRequest2 = new Label();
      this.lblNextFollowupDt = new Label();
      this.dpNextFollowupDate = new DatePicker();
      this.gradientPanel2 = new GradientPanel();
      this.label4 = new Label();
      this.dpReceivedDate = new DatePicker();
      this.label6 = new Label();
      this.dpLastRequested = new DatePicker();
      this.label7 = new Label();
      this.txtRequestedBy = new TextBox();
      this.label8 = new Label();
      this.txtOrganization = new TextBox();
      this.btnOrganization = new StandardIconButton();
      this.label9 = new Label();
      this.txtContact = new TextBox();
      this.label10 = new Label();
      this.txtEmail = new TextBox();
      this.label11 = new Label();
      this.txtPhone = new TextBox();
      this.gradientPanel3 = new GradientPanel();
      this.flowLayoutPanel1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnOrganization).BeginInit();
      this.gradientPanel3.SuspendLayout();
      this.SuspendLayout();
      this.lblRetrunRequest1.AutoSize = true;
      this.lblRetrunRequest1.BackColor = Color.Transparent;
      this.lblRetrunRequest1.Location = new Point(13, 10);
      this.lblRetrunRequest1.Name = "lblRetrunRequest1";
      this.lblRetrunRequest1.Size = new Size(643, 13);
      this.lblRetrunRequest1.TabIndex = 0;
      this.lblRetrunRequest1.Text = "Return Requests and Receipt status shown below. Click Add Request or Add Receipt to record requests or receipt of document(s) from";
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddRequest);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddReceipts);
      this.flowLayoutPanel1.Location = new Point(770, 18);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(174, 30);
      this.flowLayoutPanel1.TabIndex = 3;
      this.btnAddRequest.Location = new Point(3, 3);
      this.btnAddRequest.Name = "btnAddRequest";
      this.btnAddRequest.Size = new Size(78, 23);
      this.btnAddRequest.TabIndex = 4;
      this.btnAddRequest.Text = "Add Request";
      this.btnAddRequest.UseVisualStyleBackColor = true;
      this.btnAddRequest.Click += new EventHandler(this.btnAddRequest_Click);
      this.btnAddReceipts.Location = new Point(87, 3);
      this.btnAddReceipts.Name = "btnAddReceipts";
      this.btnAddReceipts.Size = new Size(75, 23);
      this.btnAddReceipts.TabIndex = 4;
      this.btnAddReceipts.Text = "Add Receipt";
      this.btnAddReceipts.UseVisualStyleBackColor = true;
      this.btnAddReceipts.Click += new EventHandler(this.btnAddReceipts_Click);
      this.gradientPanel1.Controls.Add((Control) this.lblReturnRequest2);
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Controls.Add((Control) this.lblRetrunRequest1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.White;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(953, 52);
      this.gradientPanel1.TabIndex = 3;
      this.lblReturnRequest2.AutoSize = true;
      this.lblReturnRequest2.BackColor = Color.Transparent;
      this.lblReturnRequest2.Location = new Point(13, 27);
      this.lblReturnRequest2.Name = "lblReturnRequest2";
      this.lblReturnRequest2.Size = new Size(174, 13);
      this.lblReturnRequest2.TabIndex = 4;
      this.lblReturnRequest2.Text = "Document Custodian or other party.";
      this.lblNextFollowupDt.AutoSize = true;
      this.lblNextFollowupDt.BackColor = Color.Transparent;
      this.lblNextFollowupDt.Location = new Point(13, 15);
      this.lblNextFollowupDt.Name = "lblNextFollowupDt";
      this.lblNextFollowupDt.Size = new Size(101, 13);
      this.lblNextFollowupDt.TabIndex = 2;
      this.lblNextFollowupDt.Text = "Next Follow-up date";
      this.dpNextFollowupDate.BackColor = SystemColors.Window;
      this.dpNextFollowupDate.Location = new Point(135, 12);
      this.dpNextFollowupDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpNextFollowupDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpNextFollowupDate.Name = "dpNextFollowupDate";
      this.dpNextFollowupDate.Size = new Size(113, 21);
      this.dpNextFollowupDate.TabIndex = 3;
      this.dpNextFollowupDate.Tag = (object) "NextFollowUpDate";
      this.dpNextFollowupDate.ToolTip = "";
      this.dpNextFollowupDate.Value = new DateTime(0L);
      this.dpNextFollowupDate.ValueChanged += new EventHandler(this.nxtFollowupDate_TextChanged);
      this.gradientPanel2.Controls.Add((Control) this.dpNextFollowupDate);
      this.gradientPanel2.Controls.Add((Control) this.lblNextFollowupDt);
      this.gradientPanel2.Dock = DockStyle.Fill;
      this.gradientPanel2.GradientColor1 = Color.White;
      this.gradientPanel2.GradientColor2 = Color.White;
      this.gradientPanel2.Location = new Point(0, 52);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(953, 241);
      this.gradientPanel2.TabIndex = 4;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(13, 15);
      this.label4.Name = "label4";
      this.label4.Size = new Size(53, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Received";
      this.dpReceivedDate.BackColor = SystemColors.Window;
      this.dpReceivedDate.Location = new Point(104, 12);
      this.dpReceivedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpReceivedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpReceivedDate.Name = "dpReceivedDate";
      this.dpReceivedDate.Size = new Size(113, 21);
      this.dpReceivedDate.TabIndex = 3;
      this.dpReceivedDate.Tag = (object) "Received";
      this.dpReceivedDate.ToolTip = "";
      this.dpReceivedDate.Value = new DateTime(0L);
      this.dpReceivedDate.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(13, 42);
      this.label6.Name = "label6";
      this.label6.Size = new Size(82, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Last Requested";
      this.dpLastRequested.BackColor = SystemColors.Window;
      this.dpLastRequested.Location = new Point(104, 39);
      this.dpLastRequested.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpLastRequested.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpLastRequested.Name = "dpLastRequested";
      this.dpLastRequested.Size = new Size(113, 21);
      this.dpLastRequested.TabIndex = 7;
      this.dpLastRequested.Tag = (object) "LastRequested";
      this.dpLastRequested.ToolTip = "";
      this.dpLastRequested.Value = new DateTime(0L);
      this.dpLastRequested.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(13, 68);
      this.label7.Name = "label7";
      this.label7.Size = new Size(73, 13);
      this.label7.TabIndex = 8;
      this.label7.Text = "Requested by";
      this.txtRequestedBy.Location = new Point(104, 66);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.Size = new Size(210, 20);
      this.txtRequestedBy.TabIndex = 9;
      this.txtRequestedBy.Tag = (object) "RequestedBy";
      this.txtRequestedBy.TextChanged += new EventHandler(this.control_ValueChanged);
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Location = new Point(13, 94);
      this.label8.Name = "label8";
      this.label8.Size = new Size(66, 13);
      this.label8.TabIndex = 11;
      this.label8.Text = "Organization";
      this.txtOrganization.Location = new Point(104, 92);
      this.txtOrganization.Name = "txtOrganization";
      this.txtOrganization.Size = new Size(210, 20);
      this.txtOrganization.TabIndex = 12;
      this.txtOrganization.Tag = (object) "Organization";
      this.txtOrganization.TextChanged += new EventHandler(this.control_ValueChanged);
      this.btnOrganization.BackColor = Color.Transparent;
      this.btnOrganization.Location = new Point(320, 94);
      this.btnOrganization.MouseDownImage = (Image) null;
      this.btnOrganization.Name = "btnOrganization";
      this.btnOrganization.Size = new Size(16, 16);
      this.btnOrganization.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnOrganization.TabIndex = 13;
      this.btnOrganization.TabStop = false;
      this.btnOrganization.Click += new EventHandler(this.btnOrganization_Click);
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(13, 121);
      this.label9.Name = "label9";
      this.label9.Size = new Size(44, 13);
      this.label9.TabIndex = 14;
      this.label9.Text = "Contact";
      this.txtContact.Location = new Point(104, 118);
      this.txtContact.Name = "txtContact";
      this.txtContact.Size = new Size(210, 20);
      this.txtContact.TabIndex = 15;
      this.txtContact.Tag = (object) "Contact";
      this.txtContact.TextChanged += new EventHandler(this.control_ValueChanged);
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Location = new Point(13, 147);
      this.label10.Name = "label10";
      this.label10.Size = new Size(32, 13);
      this.label10.TabIndex = 16;
      this.label10.Text = "Email";
      this.txtEmail.Location = new Point(104, 144);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(210, 20);
      this.txtEmail.TabIndex = 17;
      this.txtEmail.Tag = (object) "Email";
      this.txtEmail.LostFocus += new EventHandler(this.email_Leave);
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Location = new Point(13, 173);
      this.label11.Name = "label11";
      this.label11.Size = new Size(38, 13);
      this.label11.TabIndex = 18;
      this.label11.Text = "Phone";
      this.txtPhone.Location = new Point(104, 170);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(210, 20);
      this.txtPhone.TabIndex = 19;
      this.txtPhone.Tag = (object) "Phone";
      this.txtPhone.TextChanged += new EventHandler(this.phone_TextChanged);
      this.txtPhone.KeyDown += new KeyEventHandler(this.phone_KeyDown);
      this.txtPhone.LostFocus += new EventHandler(this.phone_Leave);
      this.gradientPanel3.BackColor = Color.Transparent;
      this.gradientPanel3.Controls.Add((Control) this.txtPhone);
      this.gradientPanel3.Controls.Add((Control) this.label11);
      this.gradientPanel3.Controls.Add((Control) this.txtEmail);
      this.gradientPanel3.Controls.Add((Control) this.label10);
      this.gradientPanel3.Controls.Add((Control) this.txtContact);
      this.gradientPanel3.Controls.Add((Control) this.label9);
      this.gradientPanel3.Controls.Add((Control) this.btnOrganization);
      this.gradientPanel3.Controls.Add((Control) this.txtOrganization);
      this.gradientPanel3.Controls.Add((Control) this.label8);
      this.gradientPanel3.Controls.Add((Control) this.txtRequestedBy);
      this.gradientPanel3.Controls.Add((Control) this.label7);
      this.gradientPanel3.Controls.Add((Control) this.dpLastRequested);
      this.gradientPanel3.Controls.Add((Control) this.label6);
      this.gradientPanel3.Controls.Add((Control) this.dpReceivedDate);
      this.gradientPanel3.Controls.Add((Control) this.label4);
      this.gradientPanel3.Dock = DockStyle.Bottom;
      this.gradientPanel3.GradientColor1 = Color.White;
      this.gradientPanel3.GradientColor2 = Color.White;
      this.gradientPanel3.Location = new Point(0, 93);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(953, 200);
      this.gradientPanel3.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gradientPanel3);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Name = nameof (ReturnRequest);
      this.Size = new Size(953, 293);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      ((ISupportInitialize) this.btnOrganization).EndInit();
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
