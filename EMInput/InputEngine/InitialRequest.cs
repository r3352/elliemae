// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InitialRequest
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class InitialRequest : UserControl
  {
    private LoanData loanData;
    private DocTrackingType docTrackingType;
    private bool isFormEnabled;
    private string prefix;
    private IContainer components;
    private Label label1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnAddRequest;
    private Button btnAddReceipts;
    private GradientPanel gradientPanel1;
    private Label lblPurchaseDt;
    private Label lblNextFollowupDt;
    private DatePicker dpNextFollowupDate;
    private GradientPanel gradientPanel2;
    private Label label4;
    private Label label5;
    private Label label6;
    private DatePicker dpLastRequested;
    private Label label7;
    private TextBox txtRequestedBy;
    private TextBox txtDaysOutstanding;
    private Label label8;
    private TextBox txtOrganization;
    private StandardIconButton btnOrganization;
    private Label label9;
    private TextBox txtContact;
    private Label label10;
    private TextBox txtEmail;
    private Label label11;
    private TextBox txtPhone;
    private Label lblDocumentNum;
    private TextBox txtDocumentNumber;
    private Label lblBookNum;
    private TextBox txtBookNumber;
    private Label lblPageNum;
    private TextBox txtPageNumber;
    private Label lblRidersRec;
    private RadioButton rbYes;
    private RadioButton rbNa;
    private RadioButton rbNo;
    private GradientPanel gradientPanel3;
    private DatePicker dpReceivedDate;
    private TextBox txtPurchaseDt;

    public InitialRequest(DocTrackingType docTrackingType)
    {
      this.InitializeComponent();
      this.loanData = DocTrackingUtils.Session.LoanData;
      this.docTrackingType = docTrackingType;
      this.prefix = DocTrackingUtils.GetPrefix(docTrackingType, DocTrackingRequestType.InitialRequest);
      if (this.docTrackingType != DocTrackingType.FTP)
        return;
      this.lblDocumentNum.Visible = false;
      this.lblBookNum.Visible = false;
      this.lblPageNum.Visible = false;
      this.lblRidersRec.Visible = false;
      this.txtDocumentNumber.Visible = false;
      this.txtBookNumber.Visible = false;
      this.txtPageNumber.Visible = false;
      this.rbYes.Visible = false;
      this.rbNo.Visible = false;
      this.rbNa.Visible = false;
    }

    public void EnableDisableInitialRequest(bool isEnable)
    {
      this.isFormEnabled = isEnable;
      this.btnAddRequest.Enabled = isEnable;
      this.btnAddReceipts.Enabled = isEnable;
      this.dpNextFollowupDate.Enabled = false;
      this.dpReceivedDate.Enabled = false;
      this.dpLastRequested.Enabled = false;
      this.txtRequestedBy.Enabled = false;
      this.txtOrganization.Enabled = false;
      this.txtContact.Enabled = false;
      this.txtEmail.Enabled = false;
      this.txtPhone.Enabled = false;
      this.txtDocumentNumber.Enabled = false;
      this.txtBookNumber.Enabled = false;
      this.txtPageNumber.Enabled = false;
      this.rbYes.Enabled = false;
      this.rbNo.Enabled = false;
      this.rbNa.Enabled = false;
      this.btnOrganization.Enabled = false;
      this.btnOrganization.Hide();
    }

    public void InitData()
    {
      this.CalculatePurchaseDt();
      this.dpReceivedDate.Value = Utils.ParseDate((object) this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate)));
      this.CalculateDaysOutStanding();
      this.CalculateNextFollowupDate();
      this.dpLastRequested.Value = Utils.ParseDate((object) this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpLastRequested)));
      this.txtRequestedBy.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtRequestedBy));
      this.txtOrganization.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization));
      this.txtContact.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtContact));
      this.txtEmail.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtEmail));
      this.txtPhone.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPhone));
      if (this.docTrackingType != DocTrackingType.DOT)
        return;
      this.txtDocumentNumber.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtDocumentNumber));
      this.txtBookNumber.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtBookNumber));
      this.txtPageNumber.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPageNumber));
      this.CheckRidersRecvd();
    }

    public void RefreshContent()
    {
      this.txtPurchaseDt.Text = this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.PurchaseDate") == string.Empty || this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.PurchaseDate") == "//" ? "" : this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.PurchaseDate");
      this.dpNextFollowupDate.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpNextFollowupDate));
      this.dpReceivedDate.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate));
      this.txtDaysOutstanding.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtDaysOutstanding));
      this.dpLastRequested.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpLastRequested));
      this.txtRequestedBy.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtRequestedBy));
      this.txtOrganization.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization));
      this.txtContact.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtContact));
      this.txtEmail.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtEmail));
      this.txtPhone.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPhone));
      if (this.docTrackingType != DocTrackingType.DOT)
        return;
      this.txtDocumentNumber.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtDocumentNumber));
      this.txtBookNumber.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtBookNumber));
      this.txtPageNumber.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPageNumber));
      this.CheckRidersRecvd();
    }

    private void CheckRidersRecvd()
    {
      if (this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.RiderReceived") == "Yes")
        this.rbYes.Checked = true;
      else if (this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.RiderReceived") == "No")
        this.rbNo.Checked = true;
      else if (this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.RiderReceived") == "N/A")
      {
        this.rbNa.Checked = true;
      }
      else
      {
        this.rbYes.Checked = false;
        this.rbNo.Checked = false;
        this.rbNa.Checked = false;
      }
    }

    private void CalculatePurchaseDt()
    {
      DateTime minValue = DateTime.MinValue;
      DateTime date = Utils.ParseDate((object) this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.PurchaseDate"));
      DateTime dateTime = !this.isFormEnabled ? date : DocTrackingUtils.GetNotePurchaseDt();
      this.txtPurchaseDt.Text = dateTime != DateTime.MinValue ? dateTime.ToString("MM/dd/yyyy") : "";
      this.SetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.PurchaseDate", this.txtPurchaseDt.Text);
    }

    private void CalculateNextFollowupDate()
    {
      DateTime date = Utils.ParseDate((object) this.txtPurchaseDt.Text);
      DateTime dateTime1 = this.dpReceivedDate.Value;
      DateTime dateTime2 = dateTime1 != DateTime.MinValue || date == DateTime.MinValue ? DateTime.MinValue : Utils.ParseDate((object) this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpNextFollowupDate)));
      if (DocTrackingUtils.IsCalculate && dateTime1 == DateTime.MinValue && date != DateTime.MinValue)
      {
        int num = Utils.ParseInt(DocTrackingUtils.DocTrackingSettings[(object) (this.docTrackingType.ToString() + "InitReqTriggerDays")]);
        dateTime2 = DocTrackingUtils.GetPreviousClosestBusinessDay(date.AddDays((double) num));
      }
      this.dpNextFollowupDate.Value = dateTime2;
      this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpNextFollowupDate), this.dpNextFollowupDate.Text);
    }

    private void CalculateDaysOutStanding()
    {
      DateTime date = Utils.ParseDate((object) this.txtPurchaseDt.Text);
      DateTime dateTime = this.dpReceivedDate.Value;
      int num = 0;
      if (date != DateTime.MinValue)
      {
        if (dateTime != DateTime.MinValue)
          num = (dateTime.Date - date.Date).Days;
        else if (date != DateTime.MinValue)
          num = (DateTime.Now.Date - date.Date).Days;
      }
      this.txtDaysOutstanding.Text = num != 0 ? num.ToString() : "";
      this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtDaysOutstanding), this.txtDaysOutstanding.Text);
    }

    private void dpReceivedDate_ValueChanged(object sender, EventArgs e)
    {
      this.freeEntryDate_ValueChanged(sender, e);
      this.CalculateDaysOutStanding();
      this.CalculateNextFollowupDate();
    }

    private void freeEntryDate_ValueChanged(object sender, EventArgs e)
    {
      DatePicker datePicker = (DatePicker) sender;
      string fieldId = DocTrackingUtils.GetFieldId(this.prefix, (Control) datePicker);
      try
      {
        if (datePicker.Value != DateTime.MinValue)
          this.SetField(fieldId, datePicker.Value.ToString("MM/dd/yyyy"));
        else
          this.SetField(fieldId ?? "", "");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.SetField(fieldId ?? "", "");
        datePicker.Text = "";
      }
    }

    private void freeEntry_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      string fieldId = DocTrackingUtils.GetFieldId(this.prefix, (Control) textBox);
      string str = textBox.Text.Trim();
      try
      {
        this.SetField(fieldId, str);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.SetField(fieldId ?? "", "");
        textBox.Text = "";
      }
    }

    private void rbYes_CheckedChanged(object sender, EventArgs e)
    {
      RadioButton radioButton = (RadioButton) sender;
      string fieldId = DocTrackingUtils.GetFieldId(this.prefix, (Control) radioButton);
      try
      {
        this.SetField(fieldId, radioButton.Text);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.SetField(fieldId ?? "", "");
        radioButton.Text = "";
      }
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
          DocTrackingUtils.Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
          this.txtOrganization.Text = rxContactRecord[RolodexField.Company];
          this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtOrganization), this.txtOrganization.Text);
          this.txtContact.Text = rxContactRecord[RolodexField.Name];
          this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtContact), this.txtContact.Text);
          this.txtPhone.Text = rxContactRecord[RolodexField.Phone];
          this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtPhone), this.txtPhone.Text);
          this.txtEmail.Text = rxContactRecord[RolodexField.Email];
          this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.txtEmail), this.txtEmail.Text);
        }
      }
    }

    private void SetField(string id, string value)
    {
      if (!this.isFormEnabled)
        return;
      this.loanData.SetField(id, value);
    }

    private void txtEmail_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (DocTrackingUtils.ValidateEmail((TextBox) sender, (Control) this))
        this.freeEntry_Leave(sender, e);
      else
        textBox.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) textBox));
    }

    private void txtPhone_Leave(object sender, EventArgs e) => this.freeEntry_Leave(sender, e);

    private void txtPhone_TextChanged(object sender, EventArgs e)
    {
      DocTrackingUtils.ValidatePhone((TextBox) sender, (Control) this);
    }

    private void dpNextFollowupDate_ValueChanged(object sender, EventArgs e)
    {
      if (this.dpReceivedDate.Value != DateTime.MinValue)
      {
        if (Utils.Dialog((IWin32Window) this, DocTrackingUtils.InitialRequest_NextFollowupDate, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
          this.dpReceivedDate.Value = DateTime.MinValue;
          this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate), this.dpReceivedDate.Text);
          this.CalculateDaysOutStanding();
        }
        else
          this.dpNextFollowupDate.Value = DateTime.MinValue;
      }
      this.freeEntryDate_ValueChanged(sender, e);
    }

    private void btnAddRequest_Click(object sender, EventArgs e)
    {
      if (!this.ValidateReceivdDt(true, DocTrackingUtils.InitialRequest_AddRequest))
        return;
      this.ShowPopup((Form) new AddInitialRequest(this.docTrackingType));
    }

    private void btnAddReceipts_Click(object sender, EventArgs e)
    {
      if (!this.ValidateReceivdDt(false, DocTrackingUtils.InitialRequest_AddReceipt))
        return;
      this.ShowPopup((Form) new AddInitialReceipt(this.docTrackingType));
    }

    private void ShowPopup(Form popup)
    {
      int num = (int) popup.ShowDialog();
      if (popup.DialogResult != DialogResult.OK)
        return;
      DocTrackingUtils.DocTrackingManagementForm.RefreshContent();
    }

    private bool ValidateReceivdDt(bool clearReceiptDt, string message)
    {
      bool flag = true;
      if (this.dpReceivedDate.Value != DateTime.MinValue)
      {
        if (Utils.Dialog((IWin32Window) this, message, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
          if (clearReceiptDt)
          {
            this.dpReceivedDate.Value = DateTime.MinValue;
            this.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpReceivedDate), this.dpReceivedDate.Text);
          }
        }
        else
          flag = false;
      }
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gradientPanel3 = new GradientPanel();
      this.rbNo = new RadioButton();
      this.rbNa = new RadioButton();
      this.rbYes = new RadioButton();
      this.lblRidersRec = new Label();
      this.txtPageNumber = new TextBox();
      this.lblPageNum = new Label();
      this.txtBookNumber = new TextBox();
      this.lblBookNum = new Label();
      this.txtDocumentNumber = new TextBox();
      this.lblDocumentNum = new Label();
      this.txtPhone = new TextBox();
      this.label11 = new Label();
      this.txtEmail = new TextBox();
      this.label10 = new Label();
      this.txtContact = new TextBox();
      this.label9 = new Label();
      this.btnOrganization = new StandardIconButton();
      this.txtOrganization = new TextBox();
      this.label8 = new Label();
      this.txtDaysOutstanding = new TextBox();
      this.txtRequestedBy = new TextBox();
      this.label7 = new Label();
      this.dpLastRequested = new DatePicker();
      this.label6 = new Label();
      this.label5 = new Label();
      this.dpReceivedDate = new DatePicker();
      this.label4 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.txtPurchaseDt = new TextBox();
      this.dpNextFollowupDate = new DatePicker();
      this.lblNextFollowupDt = new Label();
      this.lblPurchaseDt = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnAddRequest = new Button();
      this.btnAddReceipts = new Button();
      this.label1 = new Label();
      this.gradientPanel3.SuspendLayout();
      ((ISupportInitialize) this.btnOrganization).BeginInit();
      this.gradientPanel2.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.gradientPanel3.BackColor = Color.Transparent;
      this.gradientPanel3.Controls.Add((Control) this.rbNo);
      this.gradientPanel3.Controls.Add((Control) this.rbNa);
      this.gradientPanel3.Controls.Add((Control) this.rbYes);
      this.gradientPanel3.Controls.Add((Control) this.lblRidersRec);
      this.gradientPanel3.Controls.Add((Control) this.txtPageNumber);
      this.gradientPanel3.Controls.Add((Control) this.lblPageNum);
      this.gradientPanel3.Controls.Add((Control) this.txtBookNumber);
      this.gradientPanel3.Controls.Add((Control) this.lblBookNum);
      this.gradientPanel3.Controls.Add((Control) this.txtDocumentNumber);
      this.gradientPanel3.Controls.Add((Control) this.lblDocumentNum);
      this.gradientPanel3.Controls.Add((Control) this.txtPhone);
      this.gradientPanel3.Controls.Add((Control) this.label11);
      this.gradientPanel3.Controls.Add((Control) this.txtEmail);
      this.gradientPanel3.Controls.Add((Control) this.label10);
      this.gradientPanel3.Controls.Add((Control) this.txtContact);
      this.gradientPanel3.Controls.Add((Control) this.label9);
      this.gradientPanel3.Controls.Add((Control) this.btnOrganization);
      this.gradientPanel3.Controls.Add((Control) this.txtOrganization);
      this.gradientPanel3.Controls.Add((Control) this.label8);
      this.gradientPanel3.Controls.Add((Control) this.txtDaysOutstanding);
      this.gradientPanel3.Controls.Add((Control) this.txtRequestedBy);
      this.gradientPanel3.Controls.Add((Control) this.label7);
      this.gradientPanel3.Controls.Add((Control) this.dpLastRequested);
      this.gradientPanel3.Controls.Add((Control) this.label6);
      this.gradientPanel3.Controls.Add((Control) this.label5);
      this.gradientPanel3.Controls.Add((Control) this.dpReceivedDate);
      this.gradientPanel3.Controls.Add((Control) this.label4);
      this.gradientPanel3.Dock = DockStyle.Bottom;
      this.gradientPanel3.GradientColor1 = Color.White;
      this.gradientPanel3.GradientColor2 = Color.White;
      this.gradientPanel3.Location = new Point(0, 87);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(953, 200);
      this.gradientPanel3.TabIndex = 5;
      this.rbNo.AutoSize = true;
      this.rbNo.BackColor = Color.Transparent;
      this.rbNo.Location = new Point(638, 93);
      this.rbNo.Name = "rbNo";
      this.rbNo.Size = new Size(39, 17);
      this.rbNo.TabIndex = 29;
      this.rbNo.TabStop = true;
      this.rbNo.Tag = (object) "RiderReceived";
      this.rbNo.Text = "No";
      this.rbNo.UseVisualStyleBackColor = false;
      this.rbNo.CheckedChanged += new EventHandler(this.rbYes_CheckedChanged);
      this.rbNa.AutoSize = true;
      this.rbNa.BackColor = Color.Transparent;
      this.rbNa.Location = new Point(681, 93);
      this.rbNa.Name = "rbNa";
      this.rbNa.Size = new Size(45, 17);
      this.rbNa.TabIndex = 28;
      this.rbNa.TabStop = true;
      this.rbNa.Tag = (object) "RiderReceived";
      this.rbNa.Text = "N/A";
      this.rbNa.UseVisualStyleBackColor = false;
      this.rbNa.CheckedChanged += new EventHandler(this.rbYes_CheckedChanged);
      this.rbYes.AutoSize = true;
      this.rbYes.BackColor = Color.Transparent;
      this.rbYes.Location = new Point(591, 92);
      this.rbYes.Name = "rbYes";
      this.rbYes.Size = new Size(43, 17);
      this.rbYes.TabIndex = 27;
      this.rbYes.TabStop = true;
      this.rbYes.Tag = (object) "RiderReceived";
      this.rbYes.Text = "Yes";
      this.rbYes.UseVisualStyleBackColor = false;
      this.rbYes.CheckedChanged += new EventHandler(this.rbYes_CheckedChanged);
      this.lblRidersRec.AutoSize = true;
      this.lblRidersRec.BackColor = Color.Transparent;
      this.lblRidersRec.Location = new Point(500, 94);
      this.lblRidersRec.Name = "lblRidersRec";
      this.lblRidersRec.Size = new Size(74, 13);
      this.lblRidersRec.TabIndex = 26;
      this.lblRidersRec.Text = "Riders Rec'vd";
      this.txtPageNumber.Location = new Point(591, 64);
      this.txtPageNumber.Name = "txtPageNumber";
      this.txtPageNumber.Size = new Size(210, 20);
      this.txtPageNumber.TabIndex = 25;
      this.txtPageNumber.Tag = (object) "PageNumber";
      this.txtPageNumber.Leave += new EventHandler(this.freeEntry_Leave);
      this.lblPageNum.AutoSize = true;
      this.lblPageNum.BackColor = Color.Transparent;
      this.lblPageNum.Location = new Point(500, 67);
      this.lblPageNum.Name = "lblPageNum";
      this.lblPageNum.Size = new Size(39, 13);
      this.lblPageNum.TabIndex = 24;
      this.lblPageNum.Text = "Page #";
      this.txtBookNumber.Location = new Point(591, 38);
      this.txtBookNumber.Name = "txtBookNumber";
      this.txtBookNumber.Size = new Size(210, 20);
      this.txtBookNumber.TabIndex = 23;
      this.txtBookNumber.Tag = (object) "BookNumber";
      this.txtBookNumber.Leave += new EventHandler(this.freeEntry_Leave);
      this.lblBookNum.AutoSize = true;
      this.lblBookNum.BackColor = Color.Transparent;
      this.lblBookNum.Location = new Point(500, 41);
      this.lblBookNum.Name = "lblBookNum";
      this.lblBookNum.Size = new Size(39, 13);
      this.lblBookNum.TabIndex = 22;
      this.lblBookNum.Text = "Book #";
      this.txtDocumentNumber.Location = new Point(591, 12);
      this.txtDocumentNumber.Name = "txtDocumentNumber";
      this.txtDocumentNumber.Size = new Size(210, 20);
      this.txtDocumentNumber.TabIndex = 21;
      this.txtDocumentNumber.Tag = (object) "DocumentNumber";
      this.txtDocumentNumber.Leave += new EventHandler(this.freeEntry_Leave);
      this.lblDocumentNum.AutoSize = true;
      this.lblDocumentNum.BackColor = Color.Transparent;
      this.lblDocumentNum.Location = new Point(500, 15);
      this.lblDocumentNum.Name = "lblDocumentNum";
      this.lblDocumentNum.Size = new Size(63, 13);
      this.lblDocumentNum.TabIndex = 20;
      this.lblDocumentNum.Text = "Document #";
      this.txtPhone.Location = new Point(104, 170);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(210, 20);
      this.txtPhone.TabIndex = 19;
      this.txtPhone.Tag = (object) "Phone";
      this.txtPhone.TextChanged += new EventHandler(this.txtPhone_TextChanged);
      this.txtPhone.Leave += new EventHandler(this.txtPhone_Leave);
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Location = new Point(13, 173);
      this.label11.Name = "label11";
      this.label11.Size = new Size(38, 13);
      this.label11.TabIndex = 18;
      this.label11.Text = "Phone";
      this.txtEmail.Location = new Point(104, 144);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(210, 20);
      this.txtEmail.TabIndex = 17;
      this.txtEmail.Tag = (object) "Email";
      this.txtEmail.Leave += new EventHandler(this.txtEmail_Leave);
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Location = new Point(13, 147);
      this.label10.Name = "label10";
      this.label10.Size = new Size(32, 13);
      this.label10.TabIndex = 16;
      this.label10.Text = "Email";
      this.txtContact.Location = new Point(104, 118);
      this.txtContact.Name = "txtContact";
      this.txtContact.Size = new Size(210, 20);
      this.txtContact.TabIndex = 15;
      this.txtContact.Tag = (object) "Contact";
      this.txtContact.Leave += new EventHandler(this.freeEntry_Leave);
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(13, 121);
      this.label9.Name = "label9";
      this.label9.Size = new Size(44, 13);
      this.label9.TabIndex = 14;
      this.label9.Text = "Contact";
      this.btnOrganization.BackColor = Color.Transparent;
      this.btnOrganization.Location = new Point(320, 94);
      this.btnOrganization.MouseDownImage = (Image) null;
      this.btnOrganization.Name = "btnOrganization";
      this.btnOrganization.Size = new Size(16, 16);
      this.btnOrganization.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnOrganization.TabIndex = 13;
      this.btnOrganization.TabStop = false;
      this.btnOrganization.Click += new EventHandler(this.btnOrganization_Click);
      this.txtOrganization.Location = new Point(104, 92);
      this.txtOrganization.Name = "txtOrganization";
      this.txtOrganization.Size = new Size(210, 20);
      this.txtOrganization.TabIndex = 12;
      this.txtOrganization.Tag = (object) "Organization";
      this.txtOrganization.Leave += new EventHandler(this.freeEntry_Leave);
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Location = new Point(13, 94);
      this.label8.Name = "label8";
      this.label8.Size = new Size(66, 13);
      this.label8.TabIndex = 11;
      this.label8.Text = "Organization";
      this.txtDaysOutstanding.Enabled = false;
      this.txtDaysOutstanding.Location = new Point(342, 12);
      this.txtDaysOutstanding.Name = "txtDaysOutstanding";
      this.txtDaysOutstanding.Size = new Size(57, 20);
      this.txtDaysOutstanding.TabIndex = 10;
      this.txtDaysOutstanding.Tag = (object) "DaysOutstanding";
      this.txtRequestedBy.Location = new Point(104, 66);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.Size = new Size(210, 20);
      this.txtRequestedBy.TabIndex = 9;
      this.txtRequestedBy.Tag = (object) "RequestedBy";
      this.txtRequestedBy.Leave += new EventHandler(this.freeEntry_Leave);
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(13, 68);
      this.label7.Name = "label7";
      this.label7.Size = new Size(73, 13);
      this.label7.TabIndex = 8;
      this.label7.Text = "Requested by";
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
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(13, 42);
      this.label6.Name = "label6";
      this.label6.Size = new Size(82, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Last Requested";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(245, 15);
      this.label5.Name = "label5";
      this.label5.Size = new Size(91, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Days Outstanding";
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
      this.dpReceivedDate.ValueChanged += new EventHandler(this.dpReceivedDate_ValueChanged);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(13, 15);
      this.label4.Name = "label4";
      this.label4.Size = new Size(53, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Received";
      this.gradientPanel2.Controls.Add((Control) this.txtPurchaseDt);
      this.gradientPanel2.Controls.Add((Control) this.dpNextFollowupDate);
      this.gradientPanel2.Controls.Add((Control) this.lblNextFollowupDt);
      this.gradientPanel2.Controls.Add((Control) this.lblPurchaseDt);
      this.gradientPanel2.Dock = DockStyle.Fill;
      this.gradientPanel2.GradientColor1 = Color.White;
      this.gradientPanel2.GradientColor2 = Color.White;
      this.gradientPanel2.Location = new Point(0, 40);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(953, 247);
      this.gradientPanel2.TabIndex = 4;
      this.txtPurchaseDt.Enabled = false;
      this.txtPurchaseDt.Location = new Point(135, 13);
      this.txtPurchaseDt.Name = "txtPurchaseDt";
      this.txtPurchaseDt.Size = new Size(122, 20);
      this.txtPurchaseDt.TabIndex = 30;
      this.txtPurchaseDt.Tag = (object) "PurchaseDate";
      this.dpNextFollowupDate.BackColor = SystemColors.Window;
      this.dpNextFollowupDate.Location = new Point(384, 12);
      this.dpNextFollowupDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpNextFollowupDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpNextFollowupDate.Name = "dpNextFollowupDate";
      this.dpNextFollowupDate.Size = new Size(113, 21);
      this.dpNextFollowupDate.TabIndex = 3;
      this.dpNextFollowupDate.Tag = (object) "NextFollowupDate";
      this.dpNextFollowupDate.ToolTip = "";
      this.dpNextFollowupDate.Value = new DateTime(0L);
      this.dpNextFollowupDate.ValueChanged += new EventHandler(this.dpNextFollowupDate_ValueChanged);
      this.lblNextFollowupDt.AutoSize = true;
      this.lblNextFollowupDt.BackColor = Color.Transparent;
      this.lblNextFollowupDt.Location = new Point(277, 15);
      this.lblNextFollowupDt.Name = "lblNextFollowupDt";
      this.lblNextFollowupDt.Size = new Size(101, 13);
      this.lblNextFollowupDt.TabIndex = 2;
      this.lblNextFollowupDt.Text = "Next Follow-up date";
      this.lblPurchaseDt.AutoSize = true;
      this.lblPurchaseDt.BackColor = Color.Transparent;
      this.lblPurchaseDt.Location = new Point(13, 15);
      this.lblPurchaseDt.Name = "lblPurchaseDt";
      this.lblPurchaseDt.Size = new Size(116, 13);
      this.lblPurchaseDt.TabIndex = 0;
      this.lblPurchaseDt.Text = "Note or Purchase Date";
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.White;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(953, 40);
      this.gradientPanel1.TabIndex = 3;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddRequest);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddReceipts);
      this.flowLayoutPanel1.Location = new Point(770, 7);
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
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(13, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(673, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Initial Requests and Receipt status shown below. Click Add Request or Add Receipt to record requests or receipt of document(s) from source.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gradientPanel3);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Name = nameof (InitialRequest);
      this.Size = new Size(953, 287);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      ((ISupportInitialize) this.btnOrganization).EndInit();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
