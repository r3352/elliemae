// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ShippingStatus
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ShippingStatus : UserControl
  {
    private LoanData loanData;
    private string prefix;
    private DocTrackingType docTrackingType;
    private bool intermidiateData;
    private bool deleteBackKey;
    private bool displayingImportShippingDetails;
    private IContainer components;
    private GradientPanel gradientPanel1;
    private Button btnAddShipment;
    private Label label11;
    private GradientPanel gradientPanel2;
    private StandardIconButton btnOrganization;
    private TextBox tbPhone;
    private TextBox tbEmail;
    private TextBox tbAddr;
    private TextBox tbContact;
    private TextBox tbOrg;
    private TextBox tbTrackingNum;
    private ComboBox cboxCarrier;
    private TextBox tbShippedBy;
    private Label label10;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private DatePicker dpShippingDt;
    private Button btnImportShippingDetails;

    public ShippingStatus(DocTrackingType docTrackingType)
    {
      this.InitializeComponent();
      this.docTrackingType = docTrackingType;
      this.loanData = DocTrackingUtils.Session.LoanData;
      this.btnImportShippingDetails.Visible = docTrackingType == DocTrackingType.EN;
      this.btnImportShippingDetails.Enabled = Session.LoanDataMgr.Writable;
      this.prefix = DocTrackingUtils.FieldPrefix + (object) docTrackingType + ".ShippingStatus.";
      this.initCarrierList();
      this.InitData();
    }

    public void InitData()
    {
      this.dpShippingDt.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpShippingDt));
      this.cboxCarrier.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.cboxCarrier));
      this.tbShippedBy.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbShippedBy));
      this.tbTrackingNum.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbTrackingNum));
      this.tbOrg.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbOrg));
      this.tbContact.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbContact));
      this.tbAddr.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbAddr));
      this.tbEmail.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbEmail));
      this.tbPhone.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbPhone));
    }

    public void EnableDisableShippingStatus(bool isEnable)
    {
      this.dpShippingDt.Enabled = false;
      this.cboxCarrier.Enabled = false;
      this.tbShippedBy.Enabled = false;
      this.tbTrackingNum.Enabled = false;
      this.tbOrg.Enabled = false;
      this.tbContact.Enabled = false;
      this.tbAddr.Enabled = false;
      this.tbEmail.Enabled = false;
      this.tbPhone.Enabled = false;
      this.btnOrganization.Enabled = false;
      this.btnOrganization.Hide();
      this.btnAddShipment.Enabled = isEnable;
      this.btnImportShippingDetails.Enabled = isEnable && Session.LoanDataMgr.Writable;
    }

    private void initCarrierList()
    {
      string[] strArray = new string[0];
      this.cboxCarrier.Items.AddRange((object[]) Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.PreferredCarrier).ToArray(typeof (string)));
    }

    private void btnOrganization_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact.CompanyName = this.tbOrg.Text;
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", rxContact.CompanyName, this.tbContact.Text, rxContact, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.tbOrg.Text = rxBusinessContact.RxContactRecord[RolodexField.Company];
        this.tbContact.Text = rxBusinessContact.RxContactRecord[RolodexField.Name];
        this.tbAddr.Text = rxBusinessContact.RxContactRecord.BizAddress1 + (string.IsNullOrEmpty(rxBusinessContact.RxContactRecord.BizAddress2) ? ", " : " " + rxBusinessContact.RxContactRecord.BizAddress2 + ", ") + rxBusinessContact.RxContactRecord.BizCity + ", " + rxBusinessContact.RxContactRecord.BizState + ", " + rxBusinessContact.RxContactRecord.BizZip;
        this.tbEmail.Text = rxBusinessContact.RxContactRecord.BizEmail;
        this.tbPhone.Text = rxBusinessContact.RxContactRecord.WorkPhone;
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbOrg), this.tbOrg.Text);
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbContact), this.tbContact.Text);
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbAddr), this.tbAddr.Text);
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbEmail), this.tbEmail.Text);
        this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbPhone), this.tbPhone.Text);
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

    private void control_ValueChanged(object sender, EventArgs e)
    {
      Control control = (Control) sender;
      this.loanData.SetField(DocTrackingUtils.GetFieldId(this.prefix, control), control.Text);
    }

    public void btnAddShipment_Click(object sender, EventArgs e)
    {
      string field = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.dpShippingDt));
      if (!string.IsNullOrEmpty(field) && field != "//" && Utils.Dialog((IWin32Window) this, DocTrackingUtils.ShippingStatus_Override, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      using (AddShipment addShipment = new AddShipment(this.docTrackingType, DocTrackingUtils.Session))
      {
        int num = (int) addShipment.ShowDialog();
        if (addShipment.DialogResult != DialogResult.OK)
          return;
        this.InitData();
        DocTrackingUtils.DocTrackingManagementForm.RefreshContent();
      }
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

    private void btnImportShippingDetails_Click(object sender, EventArgs e)
    {
      if (this.displayingImportShippingDetails)
        return;
      try
      {
        this.displayingImportShippingDetails = true;
        using (Form shippingStatusForm = ImportShippingStatusFactory.GetImportShippingStatusForm(DocTrackingUtils.Session.LoanDataMgr))
        {
          if (shippingStatusForm == null)
            return;
          shippingStatusForm.ShowDialog((IWin32Window) Form.ActiveForm);
          if (!ImportShippingStatusFactory.Success)
            return;
          this.loanData = DocTrackingUtils.Session.LoanData;
          DocTrackingUtils.DocTrackingManagementForm.RefreshContent();
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        this.displayingImportShippingDetails = false;
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
      this.gradientPanel2 = new GradientPanel();
      this.dpShippingDt = new DatePicker();
      this.btnOrganization = new StandardIconButton();
      this.tbPhone = new TextBox();
      this.tbEmail = new TextBox();
      this.tbAddr = new TextBox();
      this.tbContact = new TextBox();
      this.tbOrg = new TextBox();
      this.tbTrackingNum = new TextBox();
      this.cboxCarrier = new ComboBox();
      this.tbShippedBy = new TextBox();
      this.label10 = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.btnImportShippingDetails = new Button();
      this.btnAddShipment = new Button();
      this.label11 = new Label();
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnOrganization).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.gradientPanel2.Controls.Add((Control) this.dpShippingDt);
      this.gradientPanel2.Controls.Add((Control) this.btnOrganization);
      this.gradientPanel2.Controls.Add((Control) this.tbPhone);
      this.gradientPanel2.Controls.Add((Control) this.tbEmail);
      this.gradientPanel2.Controls.Add((Control) this.tbAddr);
      this.gradientPanel2.Controls.Add((Control) this.tbContact);
      this.gradientPanel2.Controls.Add((Control) this.tbOrg);
      this.gradientPanel2.Controls.Add((Control) this.tbTrackingNum);
      this.gradientPanel2.Controls.Add((Control) this.cboxCarrier);
      this.gradientPanel2.Controls.Add((Control) this.tbShippedBy);
      this.gradientPanel2.Controls.Add((Control) this.label10);
      this.gradientPanel2.Controls.Add((Control) this.label9);
      this.gradientPanel2.Controls.Add((Control) this.label8);
      this.gradientPanel2.Controls.Add((Control) this.label7);
      this.gradientPanel2.Controls.Add((Control) this.label6);
      this.gradientPanel2.Controls.Add((Control) this.label5);
      this.gradientPanel2.Controls.Add((Control) this.label4);
      this.gradientPanel2.Controls.Add((Control) this.label3);
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.GradientColor1 = Color.White;
      this.gradientPanel2.GradientColor2 = Color.White;
      this.gradientPanel2.Location = new Point(0, 40);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(953, 247);
      this.gradientPanel2.TabIndex = 43;
      this.dpShippingDt.BackColor = SystemColors.Window;
      this.dpShippingDt.Location = new Point(95, 32);
      this.dpShippingDt.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpShippingDt.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpShippingDt.Name = "dpShippingDt";
      this.dpShippingDt.Size = new Size(164, 21);
      this.dpShippingDt.TabIndex = 61;
      this.dpShippingDt.Tag = (object) "ShippingDate";
      this.dpShippingDt.ToolTip = "";
      this.dpShippingDt.Value = new DateTime(0L);
      this.dpShippingDt.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.btnOrganization.BackColor = Color.Transparent;
      this.btnOrganization.Location = new Point(723, 33);
      this.btnOrganization.MouseDownImage = (Image) null;
      this.btnOrganization.Name = "btnOrganization";
      this.btnOrganization.Size = new Size(16, 16);
      this.btnOrganization.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnOrganization.TabIndex = 60;
      this.btnOrganization.TabStop = false;
      this.btnOrganization.Click += new EventHandler(this.btnOrganization_Click);
      this.tbPhone.Location = new Point(478, 147);
      this.tbPhone.Name = "tbPhone";
      this.tbPhone.Size = new Size(239, 20);
      this.tbPhone.TabIndex = 59;
      this.tbPhone.Tag = (object) "Phone";
      this.tbPhone.TextChanged += new EventHandler(this.phone_TextChanged);
      this.tbPhone.KeyDown += new KeyEventHandler(this.phone_KeyDown);
      this.tbPhone.LostFocus += new EventHandler(this.phone_Leave);
      this.tbEmail.Location = new Point(478, 119);
      this.tbEmail.Name = "tbEmail";
      this.tbEmail.Size = new Size(239, 20);
      this.tbEmail.TabIndex = 58;
      this.tbEmail.Tag = (object) "Email";
      this.tbEmail.LostFocus += new EventHandler(this.email_Leave);
      this.tbAddr.Location = new Point(478, 90);
      this.tbAddr.Name = "tbAddr";
      this.tbAddr.Size = new Size(239, 20);
      this.tbAddr.TabIndex = 57;
      this.tbAddr.Tag = (object) "Address";
      this.tbAddr.TextChanged += new EventHandler(this.control_ValueChanged);
      this.tbContact.Location = new Point(478, 61);
      this.tbContact.Name = "tbContact";
      this.tbContact.Size = new Size(239, 20);
      this.tbContact.TabIndex = 56;
      this.tbContact.Tag = (object) "Contact";
      this.tbContact.TextChanged += new EventHandler(this.control_ValueChanged);
      this.tbOrg.Location = new Point(478, 32);
      this.tbOrg.Name = "tbOrg";
      this.tbOrg.Size = new Size(239, 20);
      this.tbOrg.TabIndex = 55;
      this.tbOrg.Tag = (object) "Organization";
      this.tbOrg.TextChanged += new EventHandler(this.control_ValueChanged);
      this.tbTrackingNum.Location = new Point(95, 119);
      this.tbTrackingNum.Name = "tbTrackingNum";
      this.tbTrackingNum.Size = new Size(239, 20);
      this.tbTrackingNum.TabIndex = 54;
      this.tbTrackingNum.Tag = (object) "TrackingNumber";
      this.tbTrackingNum.TextChanged += new EventHandler(this.control_ValueChanged);
      this.cboxCarrier.FormattingEnabled = true;
      this.cboxCarrier.Location = new Point(95, 90);
      this.cboxCarrier.Name = "cboxCarrier";
      this.cboxCarrier.Size = new Size(164, 21);
      this.cboxCarrier.TabIndex = 53;
      this.cboxCarrier.Tag = (object) "Carrier";
      this.cboxCarrier.SelectedIndexChanged += new EventHandler(this.control_ValueChanged);
      this.cboxCarrier.TextChanged += new EventHandler(this.control_ValueChanged);
      this.tbShippedBy.BackColor = SystemColors.Window;
      this.tbShippedBy.Location = new Point(95, 61);
      this.tbShippedBy.Name = "tbShippedBy";
      this.tbShippedBy.Size = new Size(164, 20);
      this.tbShippedBy.TabIndex = 52;
      this.tbShippedBy.Tag = (object) "ShippedBy";
      this.tbShippedBy.TextChanged += new EventHandler(this.control_ValueChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(406, 151);
      this.label10.Name = "label10";
      this.label10.Size = new Size(38, 13);
      this.label10.TabIndex = 50;
      this.label10.Text = "Phone";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(406, 122);
      this.label9.Name = "label9";
      this.label9.Size = new Size(32, 13);
      this.label9.TabIndex = 49;
      this.label9.Text = "Email";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(406, 93);
      this.label8.Name = "label8";
      this.label8.Size = new Size(45, 13);
      this.label8.TabIndex = 48;
      this.label8.Text = "Address";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(406, 64);
      this.label7.Name = "label7";
      this.label7.Size = new Size(44, 13);
      this.label7.TabIndex = 47;
      this.label7.Text = "Contact";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(406, 35);
      this.label6.Name = "label6";
      this.label6.Size = new Size(66, 13);
      this.label6.TabIndex = 46;
      this.label6.Text = "Organization";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(15, 122);
      this.label5.Name = "label5";
      this.label5.Size = new Size(59, 13);
      this.label5.TabIndex = 45;
      this.label5.Text = "Tracking #";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(15, 93);
      this.label4.Name = "label4";
      this.label4.Size = new Size(37, 13);
      this.label4.TabIndex = 44;
      this.label4.Text = "Carrier";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(15, 64);
      this.label3.Name = "label3";
      this.label3.Size = new Size(61, 13);
      this.label3.TabIndex = 43;
      this.label3.Text = "Shipped By";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(15, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 13);
      this.label2.TabIndex = 42;
      this.label2.Text = "Shipping Date";
      this.gradientPanel1.Controls.Add((Control) this.btnImportShippingDetails);
      this.gradientPanel1.Controls.Add((Control) this.btnAddShipment);
      this.gradientPanel1.Controls.Add((Control) this.label11);
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.White;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(953, 40);
      this.gradientPanel1.TabIndex = 42;
      this.btnImportShippingDetails.Location = new Point(693, 8);
      this.btnImportShippingDetails.Name = "btnImportShippingDetails";
      this.btnImportShippingDetails.Size = new Size(130, 23);
      this.btnImportShippingDetails.TabIndex = 2;
      this.btnImportShippingDetails.Text = "Import Shipping Details";
      this.btnImportShippingDetails.UseVisualStyleBackColor = true;
      this.btnImportShippingDetails.Click += new EventHandler(this.btnImportShippingDetails_Click);
      this.btnAddShipment.Location = new Point(829, 8);
      this.btnAddShipment.Name = "btnAddShipment";
      this.btnAddShipment.Size = new Size(98, 23);
      this.btnAddShipment.TabIndex = 2;
      this.btnAddShipment.Text = "Add Shipment";
      this.btnAddShipment.UseVisualStyleBackColor = true;
      this.btnAddShipment.Click += new EventHandler(this.btnAddShipment_Click);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(12, 13);
      this.label11.Name = "label11";
      this.label11.Size = new Size(627, 13);
      this.label11.TabIndex = 1;
      this.label11.Text = "Document Shipping status shown below. Click Add Shipment to record shipment of document(s) to Investor or Document Custodian.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Name = nameof (ShippingStatus);
      this.Size = new Size(953, 287);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      ((ISupportInitialize) this.btnOrganization).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
