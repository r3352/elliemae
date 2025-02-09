// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TrusteeDetailsEditor
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
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
  public class TrusteeDetailsEditor : Form
  {
    private static readonly string[] orgType = new string[15]
    {
      "A Trust",
      "A Corporation",
      "A General Partnership",
      "A Sole Proprietorship",
      "A Limited Partnership",
      "A Partnership",
      "A Federal Savings Association",
      "A Federal Savings Bank",
      "A Federal Bank",
      "A Federal Credit Union",
      "A National Association",
      "A National Bank",
      "A National Banking Association",
      "A Limited Liability Company",
      "An Inter Vivos Trust"
    };
    private static readonly string[] states = new string[52]
    {
      "",
      "Alabama",
      "Alaska",
      "Arizona",
      "Arkansas",
      "California",
      "Colorado",
      "Connecticut",
      "Delaware",
      "Dist. of Col.",
      "Florida",
      "Georgia",
      "Hawaii",
      "Idaho",
      "Illinois",
      "Indiana",
      "Iowa",
      "Kansas",
      "Kentucky",
      "Louisiana",
      "Maine",
      "Maryland",
      "Massachusetts",
      "Michigan",
      "Minnesota",
      "Mississippi",
      "Missouri",
      "Montana",
      "Nebraska",
      "Nevada",
      "New Hampshire",
      "New Jersey",
      "New Mexico",
      "New York",
      "North Carolina",
      "North Dakota",
      "Ohio",
      "Oklahoma",
      "Oregon",
      "Pennsylvania",
      "Rhode Island",
      "South Carolina",
      "South Dakota",
      "Tennessee",
      "Texas",
      "Utah",
      "Vermont",
      "Virginia",
      "Washington",
      "West Virginia",
      "Wisconsin",
      "Wyoming"
    };
    private int trusteeRecordID = -1;
    private IContainer components;
    private Label label8;
    private Label label6;
    private Label label1;
    private ComboBox cboType;
    private TextBox txtName;
    private ComboBox cboState;
    private Label label2;
    private TextBox txtAddress;
    private Label label5;
    private Label label3;
    private TextBox txtZip;
    private TextBox txtCity;
    private TextBox txtState;
    private Label label4;
    private Button btnSave;
    private Button btnCancel;
    private Label label7;
    private DatePicker datePickerDate;
    private TextBox txtPhone;
    private Label label9;
    private StandardIconButton btnRolodex;
    private Label label10;
    private TextBox txtCounty;

    public TrusteeRecord TrusteeRecord
    {
      get
      {
        return new TrusteeRecord(this.txtName.Text, this.txtAddress.Text, this.txtCity.Text, this.txtState.Text, this.txtZip.Text, this.txtCounty.Text, this.txtPhone.Text, Utils.ParseDate((object) this.datePickerDate.Text), this.cboState.Text, this.cboType.Text)
        {
          Id = this.trusteeRecordID
        };
      }
    }

    public TrusteeDetailsEditor(TrusteeRecord trusteeRecord)
    {
      this.InitializeComponent();
      this.cboState.Items.AddRange((object[]) TrusteeDetailsEditor.states);
      if (trusteeRecord == null)
      {
        this.trusteeRecordID = -1;
      }
      else
      {
        this.trusteeRecordID = trusteeRecord.Id;
        this.txtName.Text = trusteeRecord.ContactName;
        this.txtAddress.Text = trusteeRecord.Address;
        this.txtCity.Text = trusteeRecord.City;
        this.txtState.Text = trusteeRecord.State;
        this.txtZip.Text = trusteeRecord.ZipCode;
        if (string.IsNullOrEmpty(trusteeRecord.County))
          this.populateCounty(this.txtZip.Text, true);
        else
          this.txtCounty.Text = trusteeRecord.County;
        this.txtPhone.Text = trusteeRecord.Phone;
        if (trusteeRecord.TrustDate == DateTime.MinValue)
          this.datePickerDate.Text = "";
        else
          this.datePickerDate.Text = trusteeRecord.TrustDate.ToString("MM/dd/yyyy");
        this.cboState.Text = trusteeRecord.OrgState;
        this.cboType.Text = trusteeRecord.OrgType;
        this.cboState_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void formatField_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox.ReadOnly)
        return;
      FieldFormat dataFormat = FieldFormat.ZIPCODE;
      if (textBox == this.txtState)
        dataFormat = FieldFormat.STATE;
      else if (textBox == this.txtPhone)
        dataFormat = FieldFormat.PHONE;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void txtZip_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox.ReadOnly || textBox.Text.Trim().Length < 5)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(textBox.Text.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(textBox.Text.Trim().Substring(0, 5)));
      if (zipCodeInfo == null)
        return;
      this.txtCity.Text = Utils.CapsConvert(zipCodeInfo.City, false);
      this.txtState.Text = zipCodeInfo.State;
      this.txtCounty.Text = Utils.CapsConvert(zipCodeInfo.County, false);
    }

    private void cboState_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.cboState.Text;
      string str = "A ";
      if (text != "")
      {
        string lower = text.Substring(0, 1).ToLower();
        if (lower == "a" || lower == nameof (e) || lower == "i" || lower == "o")
          str = "An ";
      }
      this.cboType.Items.Clear();
      this.cboType.BeginUpdate();
      if (text != "")
      {
        this.cboType.Items.Add((object) "");
        this.cboType.Items.Add((object) (str + text + " Trust"));
        this.cboType.Items.Add((object) (str + text + " Corporation"));
        this.cboType.Items.Add((object) (str + text + " Banking Corporation"));
        this.cboType.Items.Add((object) (str + text + " General Partnership"));
        this.cboType.Items.Add((object) (str + text + " Limited Partnership"));
        this.cboType.Items.Add((object) (str + text + " Limited Liability Company"));
      }
      this.cboType.Items.AddRange((object[]) TrusteeDetailsEditor.orgType);
      this.cboType.EndUpdate();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!(this.txtName.Text.Trim() == ""))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Trustee name cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.DialogResult = DialogResult.None;
    }

    private void btnRolodex_Click(object sender, EventArgs e)
    {
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", this.txtName.Text.Trim(), "", new RxContactInfo()
      {
        [RolodexField.Company] = this.txtName.Text.Trim()
      }, true, true, CRMRoleType.NotFound, true, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
        {
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
          this.txtName.Text = rxContactRecord[RolodexField.Company];
          this.txtAddress.Text = rxContactRecord[RolodexField.FullAddress];
          this.txtCity.Text = rxContactRecord[RolodexField.City];
          this.txtState.Text = rxContactRecord[RolodexField.State];
          this.txtZip.Text = rxContactRecord[RolodexField.ZipCode];
          this.txtPhone.Text = rxContactRecord[RolodexField.Phone];
          this.populateCounty(this.txtZip.Text, false);
        }
      }
    }

    private void populateCounty(string zipcode, bool getFirstMatched)
    {
      if (!string.IsNullOrEmpty(zipcode.Trim()))
      {
        ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(zipcode.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(zipcode.Trim().Substring(0, 5)), getFirstMatched);
        if (zipCodeInfo != null)
          this.txtCounty.Text = Utils.CapsConvert(zipCodeInfo.County, false);
        else
          this.txtCounty.Text = string.Empty;
      }
      else
        this.txtCounty.Text = string.Empty;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label8 = new Label();
      this.label6 = new Label();
      this.label1 = new Label();
      this.cboType = new ComboBox();
      this.txtName = new TextBox();
      this.cboState = new ComboBox();
      this.label2 = new Label();
      this.txtAddress = new TextBox();
      this.label5 = new Label();
      this.label3 = new Label();
      this.txtZip = new TextBox();
      this.txtCity = new TextBox();
      this.txtState = new TextBox();
      this.label4 = new Label();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.label7 = new Label();
      this.datePickerDate = new DatePicker();
      this.txtPhone = new TextBox();
      this.label9 = new Label();
      this.btnRolodex = new StandardIconButton();
      this.label10 = new Label();
      this.txtCounty = new TextBox();
      ((ISupportInitialize) this.btnRolodex).BeginInit();
      this.SuspendLayout();
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 145);
      this.label8.Name = "label8";
      this.label8.Size = new Size(57, 13);
      this.label8.TabIndex = 27;
      this.label8.Text = "Trust Date";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 190);
      this.label6.Name = "label6";
      this.label6.Size = new Size(54, 13);
      this.label6.TabIndex = 26;
      this.label6.Text = "Org. Type";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 13);
      this.label1.TabIndex = 13;
      this.label1.Text = "Trustee Name";
      this.cboType.FormattingEnabled = true;
      this.cboType.Location = new Point(92, 187);
      this.cboType.Name = "cboType";
      this.cboType.Size = new Size(210, 21);
      this.cboType.TabIndex = 9;
      this.txtName.Location = new Point(92, 10);
      this.txtName.MaxLength = 256;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(210, 20);
      this.txtName.TabIndex = 0;
      this.cboState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboState.FormattingEnabled = true;
      this.cboState.Location = new Point(92, 164);
      this.cboState.Name = "cboState";
      this.cboState.Size = new Size(210, 21);
      this.cboState.TabIndex = 8;
      this.cboState.SelectedIndexChanged += new EventHandler(this.cboState_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(45, 13);
      this.label2.TabIndex = 16;
      this.label2.Text = "Address";
      this.txtAddress.Location = new Point(92, 32);
      this.txtAddress.MaxLength = 256;
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(210, 20);
      this.txtAddress.TabIndex = 1;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 167);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 13);
      this.label5.TabIndex = 25;
      this.label5.Text = "Org. State";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 57);
      this.label3.Name = "label3";
      this.label3.Size = new Size(24, 13);
      this.label3.TabIndex = 19;
      this.label3.Text = "City";
      this.txtZip.Location = new Point(202, 76);
      this.txtZip.MaxLength = 10;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(100, 20);
      this.txtZip.TabIndex = 4;
      this.txtZip.KeyUp += new KeyEventHandler(this.formatField_KeyUp);
      this.txtZip.Leave += new EventHandler(this.txtZip_Leave);
      this.txtCity.Location = new Point(92, 54);
      this.txtCity.MaxLength = 60;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(210, 20);
      this.txtCity.TabIndex = 2;
      this.txtState.Location = new Point(92, 76);
      this.txtState.MaxLength = 2;
      this.txtState.Name = "txtState";
      this.txtState.Size = new Size(44, 20);
      this.txtState.TabIndex = 3;
      this.txtState.KeyUp += new KeyEventHandler(this.formatField_KeyUp);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 79);
      this.label4.Name = "label4";
      this.label4.Size = new Size(32, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "State";
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(148, 221);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 10;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(227, 221);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(178, 79);
      this.label7.Name = "label7";
      this.label7.Size = new Size(22, 13);
      this.label7.TabIndex = 30;
      this.label7.Text = "Zip";
      this.datePickerDate.BackColor = SystemColors.Window;
      this.datePickerDate.Location = new Point(92, 142);
      this.datePickerDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.datePickerDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datePickerDate.Name = "datePickerDate";
      this.datePickerDate.Size = new Size(116, 21);
      this.datePickerDate.TabIndex = 7;
      this.datePickerDate.ToolTip = "";
      this.datePickerDate.Value = new DateTime(0L);
      this.txtPhone.Location = new Point(92, 120);
      this.txtPhone.MaxLength = 60;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(210, 20);
      this.txtPhone.TabIndex = 6;
      this.txtPhone.KeyUp += new KeyEventHandler(this.formatField_KeyUp);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(10, 123);
      this.label9.Name = "label9";
      this.label9.Size = new Size(78, 13);
      this.label9.TabIndex = 32;
      this.label9.Text = "Phone Number";
      this.btnRolodex.BackColor = Color.Transparent;
      this.btnRolodex.Location = new Point(305, 12);
      this.btnRolodex.MouseDownImage = (Image) null;
      this.btnRolodex.Name = "btnRolodex";
      this.btnRolodex.Size = new Size(16, 16);
      this.btnRolodex.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnRolodex.TabIndex = 33;
      this.btnRolodex.TabStop = false;
      this.btnRolodex.Click += new EventHandler(this.btnRolodex_Click);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(10, 101);
      this.label10.Name = "label10";
      this.label10.Size = new Size(40, 13);
      this.label10.TabIndex = 34;
      this.label10.Text = "County";
      this.txtCounty.Location = new Point(92, 98);
      this.txtCounty.MaxLength = 256;
      this.txtCounty.Name = "txtCounty";
      this.txtCounty.Size = new Size(210, 20);
      this.txtCounty.TabIndex = 5;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(330, 256);
      this.Controls.Add((Control) this.txtCounty);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.btnRolodex);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.txtPhone);
      this.Controls.Add((Control) this.datePickerDate);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboType);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.cboState);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtAddress);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtZip);
      this.Controls.Add((Control) this.txtCity);
      this.Controls.Add((Control) this.txtState);
      this.Controls.Add((Control) this.label4);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TrusteeDetailsEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Trustee Details";
      ((ISupportInitialize) this.btnRolodex).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
