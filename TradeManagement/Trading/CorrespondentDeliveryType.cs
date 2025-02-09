// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentDeliveryType
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentDeliveryType : Form
  {
    private CorrespondentMasterDeliveryType[] types;
    private MasterCommitmentDeliveryInfo deliveryInfo;
    private DateTime sRange;
    private DateTime eRange;
    private bool isEdit;
    private bool readOnly;
    private bool InitCompleted;
    private IContainer components;
    private GroupContainer groupContainer1;
    private DatePicker dtExpireDate;
    private DatePicker dtStartDate;
    private ComboBox cmbExpMinute;
    private ComboBox cmbExpHour;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label12;
    private ComboBox cmbEffectiveMinute;
    private ComboBox cmbEffectiveHour;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtTolerance;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox txtDays;
    private ComboBox cmbDeliveryMethodType;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnSave;

    public CorrespondentDeliveryType() => this.InitializeComponent();

    public CorrespondentDeliveryType(
      CorrespondentMasterDeliveryType[] deliveryTypes,
      MasterCommitmentDeliveryInfo deliveryInfo,
      bool isEdit = false,
      DateTime startRange = default (DateTime),
      DateTime endRange = default (DateTime))
    {
      this.InitializeComponent();
      this.types = deliveryTypes;
      this.deliveryInfo = deliveryInfo;
      this.isEdit = isEdit;
      this.sRange = startRange;
      this.eRange = endRange;
      this.Init();
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    private void setReadOnly()
    {
      this.cmbDeliveryMethodType.Enabled = !this.readOnly;
      this.txtDays.ReadOnly = this.readOnly;
      this.txtTolerance.ReadOnly = this.readOnly;
      this.txtDays.Enabled = !this.readOnly;
      this.txtTolerance.Enabled = !this.readOnly;
      this.dtStartDate.ReadOnly = this.readOnly;
      this.dtExpireDate.ReadOnly = this.readOnly;
      this.cmbEffectiveHour.Enabled = !this.readOnly;
      this.cmbExpHour.Enabled = !this.readOnly;
      this.cmbExpMinute.Enabled = !this.readOnly;
      this.cmbEffectiveMinute.Enabled = !this.readOnly;
      this.btnSave.Enabled = !this.readOnly;
    }

    private void Init()
    {
      TextBoxFormatter.Attach(this.txtTolerance, TextBoxContentRule.NonNegativeDecimal, "0.####;;\"\"");
      TextBoxFormatter.Attach(this.txtDays, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      foreach (CorrespondentMasterDeliveryType type in this.types)
        this.cmbDeliveryMethodType.Items.Add((object) new System.Web.UI.WebControls.ListItem(type.ToDescription(), ((int) type).ToString()));
      if (!this.isEdit)
      {
        this.cmbEffectiveHour.SelectedIndex = 0;
        this.cmbEffectiveMinute.SelectedIndex = 0;
        this.cmbExpHour.SelectedIndex = 0;
        this.cmbExpMinute.SelectedIndex = 0;
        if (this.cmbDeliveryMethodType.Items.Count > 0)
          this.cmbDeliveryMethodType.SelectedIndex = 0;
      }
      else
      {
        this.txtDays.Text = this.deliveryInfo.DeliveryDays.ToString();
        this.txtTolerance.Text = this.deliveryInfo.Tolerance.ToString();
        if (this.cmbDeliveryMethodType.Items.Count > 0)
          this.cmbDeliveryMethodType.SelectedIndex = 0;
        this.dtStartDate.Value = this.deliveryInfo.EffectiveDateTime;
        this.cmbEffectiveHour.SelectedIndex = Utils.ParseInt((object) this.deliveryInfo.EffectiveDateTime.Hour, 0);
        this.cmbEffectiveMinute.SelectedIndex = Utils.ParseInt((object) this.deliveryInfo.EffectiveDateTime.Minute, 0);
        this.dtExpireDate.Value = this.deliveryInfo.ExpireDateTime;
        this.cmbExpHour.SelectedIndex = Utils.ParseInt((object) this.deliveryInfo.ExpireDateTime.Hour, 0);
        this.cmbExpMinute.SelectedIndex = Utils.ParseInt((object) this.deliveryInfo.ExpireDateTime.Minute, 0);
      }
      this.InitCompleted = true;
    }

    private bool ValidateForm()
    {
      DateTime dateTime1 = new DateTime();
      bool flag = true;
      if (this.txtDays.Text.Trim() == "")
        flag = false;
      if (this.txtTolerance.Text.Trim() == "")
        flag = false;
      DateTime dateTime2 = this.dtExpireDate.Value;
      if (dateTime2.Equals(dateTime1))
        flag = false;
      dateTime2 = this.dtStartDate.Value;
      if (dateTime2.Equals(dateTime1))
        flag = false;
      if (!flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter all required fields.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      return this.ValidateExiprationDateRange((object) null) && this.ValidateStartDateRange((object) null);
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.ValidateForm())
      {
        this.DialogResult = DialogResult.None;
      }
      else
      {
        this.deliveryInfo.Type = (CorrespondentMasterDeliveryType) Utils.ParseInt((object) ((System.Web.UI.WebControls.ListItem) this.cmbDeliveryMethodType.SelectedItem).Value.ToString());
        this.deliveryInfo.DeliveryDays = Utils.ParseInt((object) this.txtDays.Text, 0);
        this.deliveryInfo.Tolerance = Utils.ParseDecimal((object) this.txtTolerance.Text, 0M);
        this.deliveryInfo.EffectiveDateTime = this.DeliveryEffectiveDateTime();
        this.deliveryInfo.ExpireDateTime = this.DeliveryExpirationDateTime();
      }
    }

    public MasterCommitmentDeliveryInfo SelectedMasterCommitmentDeliveryInfo => this.deliveryInfo;

    private DateTime DeliveryEffectiveDateTime()
    {
      DateTime dateTime = this.dtStartDate.Value;
      dateTime = dateTime.AddHours(Utils.ParseDouble((object) this.cmbEffectiveHour.SelectedIndex));
      return dateTime.AddMinutes(Utils.ParseDouble((object) this.cmbEffectiveMinute.Text));
    }

    private DateTime DeliveryExpirationDateTime()
    {
      DateTime dateTime = this.dtExpireDate.Value;
      dateTime = dateTime.AddHours(Utils.ParseDouble((object) this.cmbExpHour.SelectedIndex));
      return dateTime.AddMinutes(Utils.ParseDouble((object) this.cmbExpMinute.Text));
    }

    private void OnFieldValueChangeEffectiveValidation(object sender, EventArgs e)
    {
      if (this.btnCancel.Focused || !this.InitCompleted)
        return;
      this.ValidateStartDateRange(sender);
    }

    private void OnFieldValueChangeExpirationValidation(object sender, EventArgs e)
    {
      if (this.btnCancel.Focused || !this.InitCompleted)
        return;
      this.ValidateExiprationDateRange(sender);
    }

    private bool ValidateStartDateRange(object sender)
    {
      DateTime dateTime1 = this.DeliveryEffectiveDateTime();
      dateTime1 = dateTime1.Date;
      if (!dateTime1.Equals(DateTime.MinValue))
      {
        DateTime dateTime2 = this.DeliveryEffectiveDateTime();
        dateTime2 = dateTime2.Date;
        if (!dateTime2.Equals(DateTime.MinValue))
        {
          DateTime dateTime3 = this.DeliveryExpirationDateTime();
          dateTime3 = dateTime3.Date;
          if (!dateTime3.Equals(DateTime.MinValue) && this.DeliveryEffectiveDateTime() > this.DeliveryExpirationDateTime())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Effective Date can not be greater than Expiration Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (sender != null && !((Control) sender).Focused && sender.GetType() != this.dtStartDate.GetType())
              ((Control) sender).Focus();
            return false;
          }
        }
        if (this.DeliveryEffectiveDateTime() < this.sRange)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "This Delivery Type Effective/Expire date must occur within the Master Effective/Expire Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          if (sender != null && !((Control) sender).Focused && sender.GetType() != this.dtStartDate.GetType())
            ((Control) sender).Focus();
          return false;
        }
      }
      return true;
    }

    private bool ValidateExiprationDateRange(object sender)
    {
      DateTime dateTime1 = this.DeliveryExpirationDateTime();
      dateTime1 = dateTime1.Date;
      if (!dateTime1.Equals(DateTime.MinValue))
      {
        DateTime dateTime2 = this.DeliveryEffectiveDateTime();
        dateTime2 = dateTime2.Date;
        if (!dateTime2.Equals(DateTime.MinValue))
        {
          DateTime dateTime3 = this.DeliveryExpirationDateTime();
          dateTime3 = dateTime3.Date;
          if (!dateTime3.Equals(DateTime.MinValue) && this.DeliveryEffectiveDateTime() > this.DeliveryExpirationDateTime())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Effective Date can not be greater than Expiration Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (sender != null && !((Control) sender).Focused && sender.GetType() != this.dtExpireDate.GetType())
              ((Control) sender).Focus();
            return false;
          }
        }
        if (this.DeliveryExpirationDateTime() > this.eRange)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "This Delivery Type Effective/Expire date must occur within the Master Effective/Expire Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          if (sender != null && !((Control) sender).Focused && sender.GetType() != this.dtExpireDate.GetType())
            ((Control) sender).Focus();
          return false;
        }
      }
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.cmbDeliveryMethodType = new ComboBox();
      this.dtExpireDate = new DatePicker();
      this.dtStartDate = new DatePicker();
      this.cmbExpMinute = new ComboBox();
      this.cmbExpHour = new ComboBox();
      this.label14 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.cmbEffectiveMinute = new ComboBox();
      this.cmbEffectiveHour = new ComboBox();
      this.label10 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtTolerance = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.txtDays = new System.Windows.Forms.TextBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.cmbDeliveryMethodType);
      this.groupContainer1.Controls.Add((Control) this.dtExpireDate);
      this.groupContainer1.Controls.Add((Control) this.dtStartDate);
      this.groupContainer1.Controls.Add((Control) this.cmbExpMinute);
      this.groupContainer1.Controls.Add((Control) this.cmbExpHour);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.label12);
      this.groupContainer1.Controls.Add((Control) this.cmbEffectiveMinute);
      this.groupContainer1.Controls.Add((Control) this.cmbEffectiveHour);
      this.groupContainer1.Controls.Add((Control) this.label10);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.txtTolerance);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.txtDays);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(415, 159);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Delivery Method Type";
      this.cmbDeliveryMethodType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDeliveryMethodType.FormattingEnabled = true;
      this.cmbDeliveryMethodType.Location = new Point(145, 29);
      this.cmbDeliveryMethodType.Name = "cmbDeliveryMethodType";
      this.cmbDeliveryMethodType.Size = new Size(188, 21);
      this.cmbDeliveryMethodType.TabIndex = 77;
      this.dtExpireDate.BackColor = SystemColors.Window;
      this.dtExpireDate.Location = new Point(145, 123);
      this.dtExpireDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtExpireDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtExpireDate.Name = "dtExpireDate";
      this.dtExpireDate.Size = new Size(145, 21);
      this.dtExpireDate.TabIndex = 76;
      this.dtExpireDate.Tag = (object) "DateRangeExpiration";
      this.dtExpireDate.ToolTip = "";
      this.dtExpireDate.Value = new DateTime(0L);
      this.dtExpireDate.ValueChanged += new EventHandler(this.OnFieldValueChangeExpirationValidation);
      this.dtExpireDate.Leave += new EventHandler(this.OnFieldValueChangeExpirationValidation);
      this.dtStartDate.BackColor = SystemColors.Window;
      this.dtStartDate.Location = new Point(145, 99);
      this.dtStartDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtStartDate.Name = "dtStartDate";
      this.dtStartDate.Size = new Size(145, 21);
      this.dtStartDate.TabIndex = 75;
      this.dtStartDate.Tag = (object) "DateRangeEffective";
      this.dtStartDate.ToolTip = "";
      this.dtStartDate.Value = new DateTime(0L);
      this.dtStartDate.ValueChanged += new EventHandler(this.OnFieldValueChangeEffectiveValidation);
      this.dtStartDate.Leave += new EventHandler(this.OnFieldValueChangeEffectiveValidation);
      this.cmbExpMinute.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbExpMinute.FormattingEnabled = true;
      this.cmbExpMinute.ImeMode = ImeMode.NoControl;
      this.cmbExpMinute.Items.AddRange(new object[60]
      {
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29 ",
        (object) "30 ",
        (object) "31 ",
        (object) "32 ",
        (object) "33 ",
        (object) "34 ",
        (object) "35 ",
        (object) "36 ",
        (object) "37 ",
        (object) "38 ",
        (object) "39 ",
        (object) "40 ",
        (object) "41 ",
        (object) "42 ",
        (object) "43 ",
        (object) "44 ",
        (object) "45 ",
        (object) "46 ",
        (object) "47 ",
        (object) "48 ",
        (object) "49 ",
        (object) "50 ",
        (object) "51 ",
        (object) "52 ",
        (object) "53 ",
        (object) "54 ",
        (object) "55 ",
        (object) "56 ",
        (object) "57 ",
        (object) "58 ",
        (object) "59 "
      });
      this.cmbExpMinute.Location = new Point(364, 123);
      this.cmbExpMinute.Name = "cmbExpMinute";
      this.cmbExpMinute.Size = new Size(36, 21);
      this.cmbExpMinute.TabIndex = 74;
      this.cmbExpMinute.Tag = (object) "DateRangeExpiration";
      this.cmbExpMinute.SelectedIndexChanged += new EventHandler(this.OnFieldValueChangeExpirationValidation);
      this.cmbExpMinute.Leave += new EventHandler(this.OnFieldValueChangeExpirationValidation);
      this.cmbExpHour.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbExpHour.FormattingEnabled = true;
      this.cmbExpHour.ImeMode = ImeMode.NoControl;
      this.cmbExpHour.Items.AddRange(new object[24]
      {
        (object) "12 AM",
        (object) "01 AM",
        (object) "02 AM",
        (object) "03 AM",
        (object) "04 AM",
        (object) "05 AM",
        (object) "06 AM",
        (object) "07 AM",
        (object) "08 AM",
        (object) "09 AM",
        (object) "10 AM",
        (object) "11 AM",
        (object) "12 PM",
        (object) "01 PM",
        (object) "02 PM",
        (object) "03 PM",
        (object) "04 PM",
        (object) "05 PM",
        (object) "06 PM",
        (object) "07 PM",
        (object) "08 PM",
        (object) "09 PM",
        (object) "10 PM",
        (object) "11 PM"
      });
      this.cmbExpHour.Location = new Point(297, 123);
      this.cmbExpHour.Name = "cmbExpHour";
      this.cmbExpHour.Size = new Size(61, 21);
      this.cmbExpHour.TabIndex = 73;
      this.cmbExpHour.Tag = (object) "DateRangeExpiration";
      this.cmbExpHour.SelectedIndexChanged += new EventHandler(this.OnFieldValueChangeExpirationValidation);
      this.cmbExpHour.Leave += new EventHandler(this.OnFieldValueChangeExpirationValidation);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(370, 85);
      this.label14.Name = "label14";
      this.label14.Size = new Size(25, 13);
      this.label14.TabIndex = 72;
      this.label14.Text = "MM";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(315, 85);
      this.label12.Name = "label12";
      this.label12.Size = new Size(23, 13);
      this.label12.TabIndex = 71;
      this.label12.Text = "HH";
      this.cmbEffectiveMinute.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEffectiveMinute.FormattingEnabled = true;
      this.cmbEffectiveMinute.ImeMode = ImeMode.NoControl;
      this.cmbEffectiveMinute.Items.AddRange(new object[60]
      {
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29 ",
        (object) "30 ",
        (object) "31 ",
        (object) "32 ",
        (object) "33 ",
        (object) "34 ",
        (object) "35 ",
        (object) "36 ",
        (object) "37 ",
        (object) "38 ",
        (object) "39 ",
        (object) "40 ",
        (object) "41 ",
        (object) "42 ",
        (object) "43 ",
        (object) "44 ",
        (object) "45 ",
        (object) "46 ",
        (object) "47 ",
        (object) "48 ",
        (object) "49 ",
        (object) "50 ",
        (object) "51 ",
        (object) "52 ",
        (object) "53 ",
        (object) "54 ",
        (object) "55 ",
        (object) "56 ",
        (object) "57 ",
        (object) "58 ",
        (object) "59 "
      });
      this.cmbEffectiveMinute.Location = new Point(364, 99);
      this.cmbEffectiveMinute.Name = "cmbEffectiveMinute";
      this.cmbEffectiveMinute.Size = new Size(36, 21);
      this.cmbEffectiveMinute.TabIndex = 69;
      this.cmbEffectiveMinute.Tag = (object) "DateRangeEffective";
      this.cmbEffectiveMinute.SelectedIndexChanged += new EventHandler(this.OnFieldValueChangeEffectiveValidation);
      this.cmbEffectiveMinute.Leave += new EventHandler(this.OnFieldValueChangeEffectiveValidation);
      this.cmbEffectiveHour.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEffectiveHour.FormattingEnabled = true;
      this.cmbEffectiveHour.ImeMode = ImeMode.NoControl;
      this.cmbEffectiveHour.Items.AddRange(new object[24]
      {
        (object) "12 AM",
        (object) "01 AM",
        (object) "02 AM",
        (object) "03 AM",
        (object) "04 AM",
        (object) "05 AM",
        (object) "06 AM",
        (object) "07 AM",
        (object) "08 AM",
        (object) "09 AM",
        (object) "10 AM",
        (object) "11 AM",
        (object) "12 PM",
        (object) "01 PM",
        (object) "02 PM",
        (object) "03 PM",
        (object) "04 PM",
        (object) "05 PM",
        (object) "06 PM",
        (object) "07 PM",
        (object) "08 PM",
        (object) "09 PM",
        (object) "10 PM",
        (object) "11 PM"
      });
      this.cmbEffectiveHour.Location = new Point(297, 99);
      this.cmbEffectiveHour.Name = "cmbEffectiveHour";
      this.cmbEffectiveHour.Size = new Size(61, 21);
      this.cmbEffectiveHour.TabIndex = 68;
      this.cmbEffectiveHour.Tag = (object) "DateRangeEffective";
      this.cmbEffectiveHour.SelectedIndexChanged += new EventHandler(this.OnFieldValueChangeEffectiveValidation);
      this.cmbEffectiveHour.Leave += new EventHandler(this.OnFieldValueChangeEffectiveValidation);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(7, 126);
      this.label10.Name = "label10";
      this.label10.Size = new Size(97, 13);
      this.label10.TabIndex = 67;
      this.label10.Text = "Expire Date && Time";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(7, 103);
      this.label9.Name = "label9";
      this.label9.Size = new Size(110, 13);
      this.label9.TabIndex = 65;
      this.label9.Text = "Effective Date && Time";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 82);
      this.label2.Name = "label2";
      this.label2.Size = new Size(66, 13);
      this.label2.TabIndex = 48;
      this.label2.Text = "Tolerance %";
      this.txtTolerance.BackColor = SystemColors.Window;
      this.txtTolerance.Location = new Point(145, 76);
      this.txtTolerance.MaxLength = 6;
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.Size = new Size(146, 20);
      this.txtTolerance.TabIndex = 47;
      this.txtTolerance.TextAlign = HorizontalAlignment.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(72, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Delivery Type";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 60);
      this.label11.Name = "label11";
      this.label11.Size = new Size(72, 13);
      this.label11.TabIndex = 39;
      this.label11.Text = "Delivery Days";
      this.txtDays.BackColor = SystemColors.Window;
      this.txtDays.Location = new Point(145, 54);
      this.txtDays.MaxLength = 3;
      this.txtDays.Name = "txtDays";
      this.txtDays.Size = new Size(146, 20);
      this.txtDays.TabIndex = 26;
      this.txtDays.TextAlign = HorizontalAlignment.Right;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(328, 172);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 22;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(247, 172);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 23;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(415, 206);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CorrespondentDeliveryType);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Correspondent Delivery Type";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
