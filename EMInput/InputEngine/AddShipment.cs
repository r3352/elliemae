// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddShipment
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddShipment : Form
  {
    private Sessions.Session session;
    private DocTrackingType docTrackingType;
    private LoanData loanData;
    private bool intermidiateData;
    private bool deleteBackKey;
    private string prefix;
    private Hashtable docTypeOvrdAsked;
    private string[] carrierList = new string[0];
    private IContainer components;
    private StandardIconButton btnOrg;
    private ComboBox cboxCarrier;
    private TextBox tbPhone;
    private TextBox tbEmail;
    private TextBox tbAddr;
    private TextBox tbContact;
    private TextBox tbOrg;
    private TextBox tbTrackNum;
    private TextBox tbShippedBy;
    private RichTextBox tboxComments;
    private Label label10;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private CheckBox cbFtp;
    private CheckBox cbDot;
    private Button btnOk;
    private Button btnCancel;
    private DatePickerCustom dpShippingDt;
    private Panel pnlDocType;
    private CheckBox cbEn;

    public AddShipment(DocTrackingType docTrackingType, Sessions.Session session)
    {
      this.InitializeComponent();
      this.docTrackingType = docTrackingType;
      this.session = session;
      this.loanData = session.LoanData;
      this.prefix = DocTrackingUtils.FieldPrefix + (object) docTrackingType + ".ShippingStatus.";
      this.InitCarrierList();
      this.InitData();
    }

    public AddShipment(DocumentTrackingLog dtl)
    {
      this.InitializeComponent();
      this.EnableDisableAddShipment(false);
      this.btnOk.Visible = false;
      this.btnCancel.Text = "Close";
      this.InitDataFromLog(dtl);
    }

    private void InitData()
    {
      if (this.docTrackingType != DocTrackingType.EN && !((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_ImportShippingDetails))
        this.cbEn.Hide();
      this.docTypeOvrdAsked = new Hashtable();
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        string key = control.Tag.ToString();
        if (key != this.docTrackingType.ToString())
        {
          this.docTypeOvrdAsked[(object) key] = (object) false;
          control.Checked = false;
          control.Enabled = key == DocTrackingType.EN.ToString() || Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) ("Enable" + key)]);
        }
        else
        {
          this.docTypeOvrdAsked[(object) key] = (object) true;
          control.Checked = true;
          control.Enabled = false;
        }
      }
      this.dpShippingDt.Text = DateTime.Now.ToString("MM/dd/yyy");
      this.tbShippedBy.Text = this.session.UserInfo.FullName;
      if (!string.IsNullOrEmpty(this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbOrg))))
      {
        this.tbOrg.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbOrg));
        this.tbContact.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbContact));
        this.tbAddr.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbAddr));
        this.tbEmail.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbEmail));
        this.tbPhone.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(this.prefix, (Control) this.tbPhone));
      }
      else if (this.docTrackingType == DocTrackingType.EN && !string.IsNullOrEmpty(this.loanData.GetField("VEND.X549")))
      {
        this.tbOrg.Text = this.loanData.GetField("VEND.X549");
        this.tbContact.Text = this.loanData.GetField("VEND.X555");
        this.tbAddr.Text = DocTrackingUtils.BuildAddress(new string[4]
        {
          this.loanData.GetField("VEND.X550"),
          this.loanData.GetField("VEND.X552"),
          this.loanData.GetField("VEND.X553"),
          this.loanData.GetField("VEND.X554")
        });
        this.tbEmail.Text = this.loanData.GetField("VEND.X557");
        this.tbPhone.Text = this.loanData.GetField("VEND.X556");
      }
      else if (!string.IsNullOrEmpty(this.loanData.GetField("VEND.X387")))
      {
        this.tbOrg.Text = this.loanData.GetField("VEND.X387");
        this.tbContact.Text = this.loanData.GetField("VEND.X392");
        this.tbAddr.Text = DocTrackingUtils.BuildAddress(new string[4]
        {
          this.loanData.GetField("VEND.X388"),
          this.loanData.GetField("VEND.X389"),
          this.loanData.GetField("VEND.X390"),
          this.loanData.GetField("VEND.X391")
        });
        this.tbEmail.Text = this.loanData.GetField("VEND.X394");
        this.tbPhone.Text = this.loanData.GetField("VEND.X393");
      }
      else if (!string.IsNullOrEmpty(this.loanData.GetField("VEND.X263")))
      {
        this.tbOrg.Text = this.loanData.GetField("VEND.X263");
        this.tbContact.Text = this.loanData.GetField("VEND.X271");
        this.tbAddr.Text = DocTrackingUtils.BuildAddress(new string[4]
        {
          this.loanData.GetField("VEND.X264"),
          this.loanData.GetField("VEND.X265"),
          this.loanData.GetField("VEND.X266"),
          this.loanData.GetField("VEND.X267")
        });
        this.tbEmail.Text = this.loanData.GetField("VEND.X273");
        this.tbPhone.Text = this.loanData.GetField("VEND.X272");
      }
      else
      {
        this.tbOrg.Text = "";
        this.tbContact.Text = "";
        this.tbAddr.Text = "";
        this.tbEmail.Text = "";
        this.tbPhone.Text = "";
      }
    }

    private void InitCarrierList()
    {
      this.carrierList = (string[]) Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.PreferredCarrier).ToArray(typeof (string));
      this.cboxCarrier.Items.AddRange((object[]) this.carrierList);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, DocTrackingUtils.Validate_Error_message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
        {
          if (control.Checked)
          {
            string prefix = DocTrackingUtils.GetPrefix((DocTrackingType) Enum.Parse(typeof (DocTrackingType), control.Tag.ToString(), true), DocTrackingRequestType.ShippingStatus);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.dpShippingDt), this.dpShippingDt.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.tbShippedBy), this.tbShippedBy.Text);
            if (string.IsNullOrEmpty(this.cboxCarrier.Text))
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.cboxCarrier), this.carrierList[0]);
            else
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.cboxCarrier), this.cboxCarrier.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.tbTrackNum), this.tbTrackNum.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.tbOrg), this.tbOrg.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.tbContact), this.tbContact.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.tbAddr), this.tbAddr.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.tbEmail), this.tbEmail.Text);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.tbPhone), this.tbPhone.Text);
          }
        }
        this.DialogResult = DialogResult.OK;
        DocumentTrackingLog rec = new DocumentTrackingLog(DateTime.Now);
        rec.ActionCd = DocTrackingActionCd.ActionShipped;
        rec.Action = DocTrackingUtils.Action_Shipped;
        rec.LogDate = this.dpShippingDt.Text.Trim();
        rec.LogBy = this.tbShippedBy.Text.Trim();
        rec.Dot = this.cbDot.Checked;
        rec.Ftp = this.cbFtp.Checked;
        rec.En = this.cbEn.Checked;
        rec.Organization = this.tbOrg.Text.Trim();
        rec.Contact = this.tbContact.Text.Trim();
        rec.Email = false;
        rec.Phone = false;
        Hashtable hashtable = new Hashtable();
        if (!string.IsNullOrEmpty(this.cboxCarrier.Text))
          hashtable.Add((object) "Carrier", (object) this.cboxCarrier.Text);
        else
          hashtable.Add((object) "Carrier", (object) this.carrierList[0]);
        hashtable.Add((object) "TrackingNumber", (object) this.tbTrackNum.Text);
        hashtable.Add((object) "Address", (object) this.tbAddr.Text);
        hashtable.Add((object) "Email", (object) this.tbEmail.Text);
        hashtable.Add((object) "Phone", (object) this.tbPhone.Text);
        hashtable.Add((object) "Comments", (object) this.tboxComments.Text);
        if (!string.IsNullOrEmpty(this.tboxComments.Text))
          DocTrackingUtils.SaveComments(new DocumentTrackingComment()
          {
            UserName = this.session.UserInfo.FullName,
            LogDate = DateTime.Now,
            CommentText = this.tboxComments.Text
          });
        rec.DocTrackingSnapshot = hashtable;
        this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
        this.Close();
      }
    }

    private bool validateData()
    {
      bool flag1 = true;
      bool flag2 = false;
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        if (control.Checked)
        {
          flag2 = true;
          break;
        }
      }
      if (!flag2)
      {
        foreach (Control control in (ArrangedElementCollection) this.pnlDocType.Controls)
          DocTrackingUtils.HilightFields(control, true);
        flag1 = false;
      }
      else
      {
        foreach (Control control in (ArrangedElementCollection) this.pnlDocType.Controls)
          DocTrackingUtils.HilightFields(control, false);
      }
      DocTrackingUtils.HilightFields((Control) this.dpShippingDt, false);
      if (string.IsNullOrEmpty(this.dpShippingDt.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.dpShippingDt, true);
        flag1 = false;
      }
      this.dpShippingDt.Refresh();
      DocTrackingUtils.HilightFields((Control) this.tbShippedBy, false);
      if (string.IsNullOrEmpty(this.tbShippedBy.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.tbShippedBy, true);
        flag1 = false;
      }
      DocTrackingUtils.HilightFields((Control) this.tbTrackNum, false);
      if (string.IsNullOrEmpty(this.tbTrackNum.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.tbTrackNum, true);
        flag1 = false;
      }
      DocTrackingUtils.HilightFields((Control) this.tbOrg, false);
      if (string.IsNullOrEmpty(this.tbOrg.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.tbOrg, true);
        flag1 = false;
      }
      DocTrackingUtils.HilightFields((Control) this.tbContact, false);
      if (string.IsNullOrEmpty(this.tbContact.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.tbContact, true);
        flag1 = false;
      }
      DocTrackingUtils.HilightFields((Control) this.tbAddr, false);
      if (string.IsNullOrEmpty(this.tbAddr.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.tbAddr, true);
        flag1 = false;
      }
      DocTrackingUtils.HilightFields((Control) this.tbEmail, false);
      if (!string.IsNullOrEmpty(this.tbEmail.Text.Trim()) && !Utils.ValidateEmail(this.tbEmail.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.tbEmail, true);
        flag1 = false;
      }
      return flag1;
    }

    private void btnOrg_Click(object sender, EventArgs e)
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
      }
    }

    private void setLoanDataField(Control ctrl, string value)
    {
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        if (control.Checked)
          this.loanData.SetField(DocTrackingUtils.GetFieldId(DocTrackingUtils.FieldPrefix + control.Tag + ".ShippingStatus.", ctrl), value);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void checkBox_Click(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      if (!checkBox.Checked)
        return;
      string key = checkBox.Tag.ToString();
      string field = this.loanData.GetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.ShippingStatus.ShippingDate", (object) key.ToString()));
      if (key.Equals((object) this.docTrackingType) || !checkBox.Checked || Utils.ParseBoolean(this.docTypeOvrdAsked[(object) key]) || field.Equals("//"))
        return;
      if (Utils.Dialog((IWin32Window) this, DocTrackingUtils.ShippingStatus_Override, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
      {
        checkBox.Checked = true;
        this.docTypeOvrdAsked[(object) key] = (object) true;
      }
      else
        checkBox.Checked = false;
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

    public void EnableDisableAddShipment(bool isEnable)
    {
      this.cbDot.Enabled = isEnable;
      this.cbFtp.Enabled = isEnable;
      this.cbEn.Enabled = isEnable;
      this.dpShippingDt.Enabled = isEnable;
      this.cboxCarrier.Enabled = isEnable;
      this.tbShippedBy.Enabled = isEnable;
      this.tbTrackNum.Enabled = isEnable;
      this.tbOrg.Enabled = isEnable;
      this.tbContact.Enabled = isEnable;
      this.tbAddr.Enabled = isEnable;
      this.tbEmail.Enabled = isEnable;
      this.tbPhone.Enabled = isEnable;
      this.btnOrg.Enabled = isEnable;
      this.tboxComments.Enabled = isEnable;
    }

    public void InitDataFromLog(DocumentTrackingLog dtl)
    {
      this.cbDot.Checked = dtl.Dot;
      this.cbFtp.Checked = dtl.Ftp;
      this.cbEn.Checked = dtl.En;
      this.dpShippingDt.Text = dtl.LogDate;
      this.cboxCarrier.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Carrier"];
      this.tbShippedBy.Text = dtl.LogBy;
      this.tbTrackNum.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "TrackingNumber"];
      this.tbOrg.Text = dtl.Organization;
      this.tbContact.Text = dtl.Contact;
      this.tbAddr.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Address"];
      this.tbEmail.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Email"];
      this.tbPhone.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Phone"];
      this.tboxComments.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Comments"];
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboxCarrier = new ComboBox();
      this.tbPhone = new TextBox();
      this.tbEmail = new TextBox();
      this.tbAddr = new TextBox();
      this.tbContact = new TextBox();
      this.tbOrg = new TextBox();
      this.tbTrackNum = new TextBox();
      this.tbShippedBy = new TextBox();
      this.tboxComments = new RichTextBox();
      this.label10 = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.cbFtp = new CheckBox();
      this.cbDot = new CheckBox();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.btnOrg = new StandardIconButton();
      this.pnlDocType = new Panel();
      this.cbEn = new CheckBox();
      this.dpShippingDt = new DatePickerCustom();
      ((ISupportInitialize) this.btnOrg).BeginInit();
      this.pnlDocType.SuspendLayout();
      this.SuspendLayout();
      this.cboxCarrier.BackColor = Color.White;
      this.cboxCarrier.FormattingEnabled = true;
      this.cboxCarrier.Location = new Point(116, 134);
      this.cboxCarrier.Name = "cboxCarrier";
      this.cboxCarrier.Size = new Size(121, 21);
      this.cboxCarrier.TabIndex = 45;
      this.cboxCarrier.Tag = (object) "Carrier";
      this.tbPhone.Location = new Point(116, 284);
      this.tbPhone.Name = "tbPhone";
      this.tbPhone.Size = new Size(270, 20);
      this.tbPhone.TabIndex = 44;
      this.tbPhone.Tag = (object) "Phone";
      this.tbPhone.TextChanged += new EventHandler(this.phone_TextChanged);
      this.tbPhone.KeyDown += new KeyEventHandler(this.phone_KeyDown);
      this.tbEmail.Location = new Point(116, 259);
      this.tbEmail.Name = "tbEmail";
      this.tbEmail.Size = new Size(270, 20);
      this.tbEmail.TabIndex = 43;
      this.tbEmail.Tag = (object) "Email";
      this.tbAddr.Location = new Point(116, 234);
      this.tbAddr.Name = "tbAddr";
      this.tbAddr.Size = new Size(270, 20);
      this.tbAddr.TabIndex = 42;
      this.tbAddr.Tag = (object) "Address";
      this.tbContact.Location = new Point(116, 209);
      this.tbContact.Name = "tbContact";
      this.tbContact.Size = new Size(270, 20);
      this.tbContact.TabIndex = 41;
      this.tbContact.Tag = (object) "Contact";
      this.tbOrg.Location = new Point(116, 184);
      this.tbOrg.Name = "tbOrg";
      this.tbOrg.Size = new Size(270, 20);
      this.tbOrg.TabIndex = 40;
      this.tbOrg.Tag = (object) "Organization";
      this.tbTrackNum.Location = new Point(116, 159);
      this.tbTrackNum.Name = "tbTrackNum";
      this.tbTrackNum.Size = new Size(270, 20);
      this.tbTrackNum.TabIndex = 39;
      this.tbTrackNum.Tag = (object) "TrackingNumber";
      this.tbShippedBy.BackColor = SystemColors.Window;
      this.tbShippedBy.Location = new Point(116, 109);
      this.tbShippedBy.Name = "tbShippedBy";
      this.tbShippedBy.Size = new Size(270, 20);
      this.tbShippedBy.TabIndex = 38;
      this.tbShippedBy.Tag = (object) "ShippedBy";
      this.tboxComments.Location = new Point(32, 331);
      this.tboxComments.MaxLength = 500;
      this.tboxComments.Name = "tboxComments";
      this.tboxComments.Size = new Size(354, 96);
      this.tboxComments.TabIndex = 37;
      this.tboxComments.Text = "";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(26, 309);
      this.label10.Name = "label10";
      this.label10.Size = new Size(56, 13);
      this.label10.TabIndex = 36;
      this.label10.Text = "Comments";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(26, 284);
      this.label9.Name = "label9";
      this.label9.Size = new Size(38, 13);
      this.label9.TabIndex = 35;
      this.label9.Text = "Phone";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(26, 259);
      this.label8.Name = "label8";
      this.label8.Size = new Size(32, 13);
      this.label8.TabIndex = 34;
      this.label8.Text = "Email";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(26, 234);
      this.label7.Name = "label7";
      this.label7.Size = new Size(45, 13);
      this.label7.TabIndex = 33;
      this.label7.Text = "Address";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(26, 209);
      this.label6.Name = "label6";
      this.label6.Size = new Size(44, 13);
      this.label6.TabIndex = 32;
      this.label6.Text = "Contact";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(26, 184);
      this.label5.Name = "label5";
      this.label5.Size = new Size(66, 13);
      this.label5.TabIndex = 31;
      this.label5.Text = "Organization";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(26, 159);
      this.label4.Name = "label4";
      this.label4.Size = new Size(59, 13);
      this.label4.TabIndex = 30;
      this.label4.Text = "Tracking #";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(26, 134);
      this.label3.Name = "label3";
      this.label3.Size = new Size(37, 13);
      this.label3.TabIndex = 29;
      this.label3.Text = "Carrier";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(26, 109);
      this.label2.Name = "label2";
      this.label2.Size = new Size(61, 13);
      this.label2.TabIndex = 28;
      this.label2.Text = "Shipped By";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(26, 84);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 13);
      this.label1.TabIndex = 27;
      this.label1.Text = "Shipping Date";
      this.cbFtp.AutoSize = true;
      this.cbFtp.BackColor = SystemColors.Control;
      this.cbFtp.Location = new Point(3, 26);
      this.cbFtp.Name = "cbFtp";
      this.cbFtp.Size = new Size(102, 17);
      this.cbFtp.TabIndex = 26;
      this.cbFtp.Tag = (object) "FTP";
      this.cbFtp.Text = "Final Title Policy";
      this.cbFtp.UseVisualStyleBackColor = false;
      this.cbFtp.Click += new EventHandler(this.checkBox_Click);
      this.cbDot.AutoSize = true;
      this.cbDot.BackColor = SystemColors.Control;
      this.cbDot.Location = new Point(3, 3);
      this.cbDot.Name = "cbDot";
      this.cbDot.Size = new Size(99, 17);
      this.cbDot.TabIndex = 25;
      this.cbDot.Tag = (object) "DOT";
      this.cbDot.Text = "DOT/Mortgage";
      this.cbDot.UseVisualStyleBackColor = false;
      this.cbDot.Click += new EventHandler(this.checkBox_Click);
      this.btnOk.Location = new Point(205, 445);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 48;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Location = new Point(296, 445);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 49;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOrg.BackColor = Color.Transparent;
      this.btnOrg.Location = new Point(393, 187);
      this.btnOrg.MouseDownImage = (Image) null;
      this.btnOrg.Name = "btnOrg";
      this.btnOrg.Size = new Size(16, 16);
      this.btnOrg.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnOrg.TabIndex = 46;
      this.btnOrg.TabStop = false;
      this.btnOrg.Click += new EventHandler(this.btnOrg_Click);
      this.pnlDocType.Controls.Add((Control) this.cbDot);
      this.pnlDocType.Controls.Add((Control) this.cbFtp);
      this.pnlDocType.Controls.Add((Control) this.cbEn);
      this.pnlDocType.Location = new Point(29, 9);
      this.pnlDocType.Name = "pnlDocType";
      this.pnlDocType.Size = new Size(169, 85);
      this.pnlDocType.TabIndex = 52;
      this.cbEn.AutoSize = true;
      this.cbEn.BackColor = SystemColors.Control;
      this.cbEn.Location = new Point(3, 47);
      this.cbEn.Name = "cbEn";
      this.cbEn.Size = new Size(97, 17);
      this.cbEn.TabIndex = 26;
      this.cbEn.Tag = (object) "EN";
      this.cbEn.Text = "Executed Note";
      this.cbEn.UseVisualStyleBackColor = false;
      this.cbEn.Click += new EventHandler(this.checkBox_Click);
      this.dpShippingDt.Location = new Point(116, 82);
      this.dpShippingDt.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpShippingDt.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpShippingDt.Name = "dpShippingDt";
      this.dpShippingDt.Size = new Size(121, 21);
      this.dpShippingDt.TabIndex = 51;
      this.dpShippingDt.Tag = (object) "ShippingDate";
      this.dpShippingDt.ToolTip = "";
      this.dpShippingDt.Value = new DateTime(0L);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.ClientSize = new Size(429, 489);
      this.Controls.Add((Control) this.dpShippingDt);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.btnOrg);
      this.Controls.Add((Control) this.cboxCarrier);
      this.Controls.Add((Control) this.tbPhone);
      this.Controls.Add((Control) this.tbEmail);
      this.Controls.Add((Control) this.tbAddr);
      this.Controls.Add((Control) this.tbContact);
      this.Controls.Add((Control) this.tbOrg);
      this.Controls.Add((Control) this.tbTrackNum);
      this.Controls.Add((Control) this.tbShippedBy);
      this.Controls.Add((Control) this.tboxComments);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pnlDocType);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddShipment);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Shipment";
      ((ISupportInitialize) this.btnOrg).EndInit();
      this.pnlDocType.ResumeLayout(false);
      this.pnlDocType.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
