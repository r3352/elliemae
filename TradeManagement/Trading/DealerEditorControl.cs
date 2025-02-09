// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.DealerEditorControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
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
namespace EllieMae.EMLite.Trading
{
  public class DealerEditorControl : UserControl
  {
    private ContactInformation contactInfo;
    private bool modified;
    private bool readOnly;
    private IContainer components;
    private ToolTip toolTips;
    private TextBox txtEntity;
    private StandardIconButton btnRolodex;
    private Label label7;
    private SizableTextBox txtZip;
    private Label label10;
    private TextBox txtCity;
    private Label label4;
    private Label label3;
    private TextBox txtStreet;
    private Label label2;
    private ComboBox cboState;
    private TextBox txtPhone;
    private Label label6;
    private TextBox txtFax;
    private Label label1;
    private TextBox txtEmail;
    private Label label8;
    private TextBox txtWebSite;
    private Label label9;
    private TextBox txtContact;
    private Label label5;
    private Panel panel2;

    public DealerEditorControl()
      : this((ContactInformation) null)
    {
    }

    public DealerEditorControl(ContactInformation dealer)
    {
      this.InitializeComponent();
      this.contactInfo = dealer;
      this.loadStateDropdown();
      PhoneNumberFormatter phoneNumberFormatter1 = new PhoneNumberFormatter(this.txtPhone);
      PhoneNumberFormatter phoneNumberFormatter2 = new PhoneNumberFormatter(this.txtFax);
      ZipCodeCityStateFormatter cityStateFormatter = new ZipCodeCityStateFormatter(this.txtZip.TextBox, this.txtCity, (Control) this.cboState);
    }

    public bool DataModified => !this.readOnly && this.modified;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (value == this.readOnly)
          return;
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    public ContactInformation Dealer
    {
      get
      {
        if (this.contactInfo == null)
          this.contactInfo = new ContactInformation();
        return this.contactInfo;
      }
    }

    public void Init(ContactInformation dealer)
    {
      this.contactInfo = dealer;
      this.RefreshContents();
    }

    public void RefreshContents()
    {
      this.Clear();
      if (this.contactInfo == null)
        return;
      this.loadDealerData();
    }

    public void Clear()
    {
      this.txtEntity.Text = "";
      this.txtStreet.Text = "";
      this.txtCity.Text = "";
      this.txtZip.Text = "";
      this.txtContact.Text = "";
      this.txtPhone.Text = "";
      this.txtFax.Text = "";
      this.txtEmail.Text = "";
      this.txtWebSite.Text = "";
      this.cboState.SelectedIndex = 0;
    }

    public bool isEmpty()
    {
      return this.txtEntity.Text.Trim() == "" && this.txtStreet.Text.Trim() == "" && this.txtCity.Text.Trim() == "" && this.txtZip.Text.Trim() == "" && this.txtContact.Text.Trim() == "" && this.txtPhone.Text.Trim() == "" && this.txtFax.Text.Trim() == "" && this.txtEmail.Text.Trim() == "" && this.txtWebSite.Text.Trim() == "" && this.cboState.Text.Trim() == "";
    }

    private void loadDealerData()
    {
      this.txtEntity.Text = this.contactInfo.EntityName;
      this.txtStreet.Text = this.contactInfo.Address.Street1;
      this.txtCity.Text = this.contactInfo.Address.City;
      this.txtZip.Text = this.contactInfo.Address.Zip;
      this.txtContact.Text = this.contactInfo.ContactName;
      this.txtPhone.Text = this.contactInfo.PhoneNumber;
      this.txtFax.Text = this.contactInfo.FaxNumber;
      this.txtEmail.Text = this.contactInfo.EmailAddress;
      this.txtWebSite.Text = this.contactInfo.WebSite;
      USPS.State state = USPS.State.FromAbbreviation(this.contactInfo.Address.State);
      if (state == null)
        this.cboState.SelectedIndex = 0;
      else
        this.cboState.SelectedItem = (object) state;
      this.modified = false;
    }

    private void loadStateDropdown()
    {
      this.cboState.Items.Clear();
      this.cboState.Items.Add((object) "");
      foreach (object state in USPS.States)
        this.cboState.Items.Add(state);
    }

    private void populateRolodexInfo(RxContactInfo info)
    {
      this.txtEntity.Text = info.CompanyName;
      this.txtStreet.Text = info.BizAddress1;
      if (info.BizAddress2 != "")
      {
        TextBox txtStreet = this.txtStreet;
        txtStreet.Text = txtStreet.Text + ", " + info.BizAddress2;
      }
      this.txtCity.Text = info.BizCity;
      this.txtZip.Text = info.BizZip;
      this.txtContact.Text = Utils.JoinName(info.FirstName, info.LastName);
      this.txtPhone.Text = info.WorkPhone;
      this.txtFax.Text = info.FaxNumber;
      this.txtEmail.Text = info.BizEmail;
      this.txtWebSite.Text = info.WebSite;
      USPS.State state = USPS.State.FromAbbreviation(info.BizState);
      if (state == null)
        this.cboState.SelectedIndex = 0;
      else
        this.cboState.SelectedItem = (object) state;
    }

    private void setReadOnly()
    {
      this.txtEntity.ReadOnly = this.readOnly;
      this.txtStreet.ReadOnly = this.readOnly;
      this.txtCity.ReadOnly = this.readOnly;
      this.txtZip.ReadOnly = this.readOnly;
      this.txtContact.ReadOnly = this.readOnly;
      this.txtPhone.ReadOnly = this.readOnly;
      this.txtFax.ReadOnly = this.readOnly;
      this.txtEmail.ReadOnly = this.readOnly;
      this.txtWebSite.ReadOnly = this.readOnly;
      this.cboState.Enabled = !this.readOnly;
      this.cboState.Text = string.Concat(this.cboState.SelectedItem);
      this.cboState.Visible = true;
      this.btnRolodex.Visible = !this.readOnly;
      this.txtEntity.Width = this.readOnly ? this.txtStreet.Width : this.txtStreet.Width - this.btnRolodex.Width - 5;
    }

    public void CommitChanges()
    {
      if (this.contactInfo == null)
        this.contactInfo = new ContactInformation();
      this.contactInfo.EntityName = this.txtEntity.Text;
      this.contactInfo.Address.Street1 = this.txtStreet.Text;
      this.contactInfo.Address.City = this.txtCity.Text;
      this.contactInfo.Address.Zip = this.txtZip.Text;
      this.contactInfo.PhoneNumber = this.txtPhone.Text;
      this.contactInfo.FaxNumber = this.txtFax.Text;
      this.contactInfo.EmailAddress = this.txtEmail.Text;
      this.contactInfo.WebSite = this.txtWebSite.Text;
      this.contactInfo.ContactName = this.txtContact.Text;
      this.contactInfo.Address.State = this.cboState.SelectedIndex > 0 ? ((USPS.State) this.cboState.SelectedItem).Abbrev : "";
      this.modified = false;
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.modified = true;

    private void btnDealerRolodex_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContactInfo = this.createRxContactInfo();
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Dealer", rxContactInfo.CompanyName, rxContactInfo.LastName, rxContactInfo, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        else
          this.populateRolodexInfo(rxBusinessContact.RxContactRecord);
      }
    }

    private RxContactInfo createRxContactInfo()
    {
      RxContactInfo rxContactInfo = new RxContactInfo();
      rxContactInfo.CompanyName = this.txtEntity.Text;
      rxContactInfo.BizAddress1 = this.txtStreet.Text;
      rxContactInfo.BizCity = this.txtCity.Text;
      rxContactInfo.BizZip = this.txtZip.Text;
      rxContactInfo.WorkPhone = this.txtPhone.Text;
      rxContactInfo.FaxNumber = this.txtFax.Text;
      rxContactInfo.BizEmail = this.txtEmail.Text;
      rxContactInfo.WebSite = this.txtWebSite.Text;
      string[] strArray = Utils.SplitName(this.txtContact.Text);
      rxContactInfo.FirstName = strArray[0];
      rxContactInfo.LastName = strArray[1];
      rxContactInfo.BizState = this.cboState.SelectedIndex > 0 ? ((USPS.State) this.cboState.SelectedItem).Abbrev : "";
      return rxContactInfo;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.toolTips = new ToolTip(this.components);
      this.txtContact = new TextBox();
      this.label5 = new Label();
      this.txtWebSite = new TextBox();
      this.label9 = new Label();
      this.txtEmail = new TextBox();
      this.label8 = new Label();
      this.txtFax = new TextBox();
      this.label1 = new Label();
      this.txtPhone = new TextBox();
      this.label6 = new Label();
      this.txtZip = new SizableTextBox();
      this.label10 = new Label();
      this.txtCity = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.txtStreet = new TextBox();
      this.label2 = new Label();
      this.cboState = new ComboBox();
      this.txtEntity = new TextBox();
      this.btnRolodex = new StandardIconButton();
      this.label7 = new Label();
      this.panel2 = new Panel();
      ((ISupportInitialize) this.btnRolodex).BeginInit();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.txtContact.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContact.Location = new Point(64, 196);
      this.txtContact.MaxLength = 60;
      this.txtContact.Name = "txtContact";
      this.txtContact.Size = new Size(223, 20);
      this.txtContact.TabIndex = 123;
      this.txtContact.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(2, 199);
      this.label5.Name = "label5";
      this.label5.Size = new Size(44, 14);
      this.label5.TabIndex = 122;
      this.label5.Text = "Contact";
      this.txtWebSite.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWebSite.Location = new Point(64, 173);
      this.txtWebSite.MaxLength = 100;
      this.txtWebSite.Name = "txtWebSite";
      this.txtWebSite.Size = new Size(223, 20);
      this.txtWebSite.TabIndex = 121;
      this.txtWebSite.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(2, 176);
      this.label9.Name = "label9";
      this.label9.Size = new Size(50, 14);
      this.label9.TabIndex = 120;
      this.label9.Text = "Web Site";
      this.txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEmail.Location = new Point(64, 149);
      this.txtEmail.MaxLength = 100;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(223, 20);
      this.txtEmail.TabIndex = 119;
      this.txtEmail.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(2, 152);
      this.label8.Name = "label8";
      this.label8.Size = new Size(31, 14);
      this.label8.TabIndex = 118;
      this.label8.Text = "Email";
      this.txtFax.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFax.Location = new Point(64, 126);
      this.txtFax.MaxLength = 30;
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size(144, 20);
      this.txtFax.TabIndex = 117;
      this.txtFax.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 129);
      this.label1.Name = "label1";
      this.label1.Size = new Size(25, 14);
      this.label1.TabIndex = 116;
      this.label1.Text = "Fax";
      this.txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPhone.Location = new Point(64, 102);
      this.txtPhone.MaxLength = 30;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(144, 20);
      this.txtPhone.TabIndex = 115;
      this.txtPhone.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 105);
      this.label6.Name = "label6";
      this.label6.Size = new Size(37, 14);
      this.label6.TabIndex = 114;
      this.label6.Text = "Phone";
      this.txtZip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtZip.BackColor = SystemColors.Window;
      this.txtZip.BorderColor = Color.Black;
      this.txtZip.Location = new Point(172, 78);
      this.txtZip.MaxLength = 10;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(115, 22);
      this.txtZip.TabIndex = 112;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(145, 79);
      this.label10.Name = "label10";
      this.label10.Size = new Size(22, 14);
      this.label10.TabIndex = 113;
      this.label10.Text = "Zip";
      this.txtCity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCity.Location = new Point(64, 53);
      this.txtCity.MaxLength = 60;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(223, 20);
      this.txtCity.TabIndex = 110;
      this.txtCity.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(2, 79);
      this.label4.Name = "label4";
      this.label4.Size = new Size(32, 14);
      this.label4.TabIndex = 108;
      this.label4.Text = "State";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(2, 57);
      this.label3.Name = "label3";
      this.label3.Size = new Size(25, 14);
      this.label3.TabIndex = 106;
      this.label3.Text = "City";
      this.txtStreet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStreet.Location = new Point(64, 30);
      this.txtStreet.MaxLength = 100;
      this.txtStreet.Name = "txtStreet";
      this.txtStreet.Size = new Size(223, 20);
      this.txtStreet.TabIndex = 109;
      this.txtStreet.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(2, 34);
      this.label2.Name = "label2";
      this.label2.Size = new Size(49, 14);
      this.label2.TabIndex = 107;
      this.label2.Text = "Address";
      this.cboState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboState.FormattingEnabled = true;
      this.cboState.Location = new Point(64, 76);
      this.cboState.Name = "cboState";
      this.cboState.Size = new Size(64, 22);
      this.cboState.TabIndex = 111;
      this.cboState.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.txtEntity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEntity.Location = new Point(64, 8);
      this.txtEntity.MaxLength = 64;
      this.txtEntity.Name = "txtEntity";
      this.txtEntity.Size = new Size(202, 20);
      this.txtEntity.TabIndex = 104;
      this.txtEntity.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.btnRolodex.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRolodex.BackColor = Color.Transparent;
      this.btnRolodex.Location = new Point(270, 9);
      this.btnRolodex.MouseDownImage = (Image) null;
      this.btnRolodex.Name = "btnRolodex";
      this.btnRolodex.Size = new Size(16, 16);
      this.btnRolodex.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnRolodex.TabIndex = 105;
      this.btnRolodex.TabStop = false;
      this.btnRolodex.Click += new EventHandler(this.btnDealerRolodex_Click);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3, 9);
      this.label7.Name = "label7";
      this.label7.Size = new Size(34, 14);
      this.label7.TabIndex = 0;
      this.label7.Text = "Name";
      this.panel2.AutoScroll = true;
      this.panel2.Controls.Add((Control) this.txtContact);
      this.panel2.Controls.Add((Control) this.label7);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.btnRolodex);
      this.panel2.Controls.Add((Control) this.txtWebSite);
      this.panel2.Controls.Add((Control) this.txtEntity);
      this.panel2.Controls.Add((Control) this.label9);
      this.panel2.Controls.Add((Control) this.cboState);
      this.panel2.Controls.Add((Control) this.txtEmail);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.label8);
      this.panel2.Controls.Add((Control) this.txtStreet);
      this.panel2.Controls.Add((Control) this.txtFax);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.txtPhone);
      this.panel2.Controls.Add((Control) this.txtCity);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.txtZip);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(290, 228);
      this.panel2.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.panel2);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DealerEditorControl);
      this.Size = new Size(290, 228);
      ((ISupportInitialize) this.btnRolodex).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
