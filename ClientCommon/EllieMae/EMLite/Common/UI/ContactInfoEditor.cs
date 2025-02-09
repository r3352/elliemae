// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ContactInfoEditor
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ContactInfoEditor : UserControl
  {
    private ContactInformation contactInfo;
    public string rolodexCategory = "";
    private bool displayEntity = true;
    private bool displayAddress = true;
    private bool displayContact = true;
    private bool displayPhone = true;
    private bool displayFax = true;
    private bool displayEmail = true;
    private bool displayWebSite = true;
    private bool modified;
    private bool readOnly;
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private Panel pnlEntity;
    private TextBox txtEntity;
    private Panel pnlAddress;
    private TextBox txtStreet;
    private Label label2;
    private TextBox txtCity;
    private Label label3;
    private SizableTextBox txtZip;
    private ComboBox cboState;
    private Label label4;
    private TextBox txtContact;
    private Label label5;
    private Panel pnlPhone;
    private TextBox txtPhone;
    private Label label6;
    private Panel pnlFax;
    private TextBox txtFax;
    private Label label7;
    private Panel pnlEmail;
    private TextBox txtEmail;
    private Label label8;
    private Panel pnlWebSite;
    private TextBox txtWebSite;
    private Label label9;
    private SizableTextBox txtState;
    private StandardIconButton btnRolodex;
    private Label label10;
    private ToolTip toolTip1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Panel pnlContact;

    public event EventHandler DataChange;

    public event ContactInfoEditor.EntityNameTextChangeEventHandler EntityNameTextChanged;

    public ContactInfoEditor()
      : this(Session.DefaultInstance)
    {
    }

    public ContactInfoEditor(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.loadStateDropdown();
      PhoneNumberFormatter phoneNumberFormatter1 = new PhoneNumberFormatter(this.txtPhone);
      PhoneNumberFormatter phoneNumberFormatter2 = new PhoneNumberFormatter(this.txtFax);
      ZipCodeCityStateFormatter cityStateFormatter = new ZipCodeCityStateFormatter(this.txtZip.TextBox, this.txtCity, (Control) this.cboState);
    }

    public bool DisplayEntityName
    {
      get => this.displayEntity;
      set
      {
        this.displayEntity = value;
        this.pnlEntity.Visible = value;
      }
    }

    public bool DisplayAddress
    {
      get => this.displayAddress;
      set
      {
        this.displayAddress = value;
        this.pnlAddress.Visible = value;
      }
    }

    public bool DisplayContactName
    {
      get => this.displayContact;
      set
      {
        this.displayContact = value;
        this.pnlContact.Visible = value;
      }
    }

    public bool DisplayPhoneNumber
    {
      get => this.displayPhone;
      set
      {
        this.displayPhone = value;
        this.pnlPhone.Visible = value;
      }
    }

    public bool DisplayFaxNumber
    {
      get => this.displayFax;
      set
      {
        this.displayFax = value;
        this.pnlFax.Visible = value;
      }
    }

    public bool DisplayEmailAddress
    {
      get => this.displayEmail;
      set
      {
        this.displayEmail = value;
        this.pnlEmail.Visible = value;
      }
    }

    public bool DisplayWebSite
    {
      get => this.displayWebSite;
      set
      {
        this.displayWebSite = value;
        this.pnlWebSite.Visible = value;
      }
    }

    public bool DataModified => this.modified;

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

    public string RolodexCategory
    {
      get => this.rolodexCategory;
      set
      {
        this.rolodexCategory = value;
        if ((this.rolodexCategory ?? "") == "")
        {
          this.btnRolodex.Visible = false;
          this.txtEntity.Width = this.txtStreet.Width;
        }
        else
        {
          this.btnRolodex.Visible = true;
          this.txtEntity.Width = this.txtStreet.Width - this.btnRolodex.Width - 5;
        }
      }
    }

    public ContactInformation ContactInformation
    {
      get => this.contactInfo;
      set
      {
        this.contactInfo = value;
        if (value != null)
          this.loadContactInfo();
        else
          this.contactInfo = new ContactInformation();
      }
    }

    public void SetTextBoxValues(ContactInformation contactInfo)
    {
      this.txtEntity.Text = contactInfo.EntityName;
      this.txtStreet.Text = contactInfo.Address.Street1;
      this.txtCity.Text = contactInfo.Address.City;
      this.txtZip.Text = contactInfo.Address.Zip;
      this.txtContact.Text = contactInfo.ContactName;
      this.txtPhone.Text = contactInfo.PhoneNumber;
      this.txtFax.Text = contactInfo.FaxNumber;
      this.txtEmail.Text = contactInfo.EmailAddress;
      this.txtWebSite.Text = contactInfo.WebSite;
      this.txtState.Text = contactInfo.Address.State;
      USPS.State state = USPS.State.FromAbbreviation(contactInfo.Address.State);
      if (state == null)
        this.cboState.SelectedIndex = 0;
      else
        this.cboState.SelectedItem = (object) state;
      this.modified = true;
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
      this.txtState.Text = "";
      this.cboState.SelectedIndex = -1;
    }

    private void loadStateDropdown()
    {
      this.cboState.Items.Clear();
      this.cboState.Items.Add((object) "");
      foreach (object state in USPS.States)
        this.cboState.Items.Add(state);
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
      this.cboState.Visible = !this.readOnly;
      this.txtState.Text = string.Concat(this.cboState.SelectedItem);
      this.txtState.Visible = this.readOnly;
      this.btnRolodex.Visible = !this.readOnly;
      this.txtEntity.Width = this.readOnly ? this.txtStreet.Width : this.txtStreet.Width - this.btnRolodex.Width - 5;
    }

    private void loadContactInfo()
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
      this.txtState.Text = this.contactInfo.Address.State;
      USPS.State state = USPS.State.FromAbbreviation(this.contactInfo.Address.State);
      if (state == null)
        this.cboState.SelectedIndex = 0;
      else
        this.cboState.SelectedItem = (object) state;
      this.modified = false;
    }

    public void CommitChanges()
    {
      if (this.contactInfo != null)
      {
        this.contactInfo.EntityName = this.txtEntity.Text;
        this.contactInfo.Address.Street1 = this.txtStreet.Text;
        this.contactInfo.Address.City = this.txtCity.Text;
        this.contactInfo.Address.Zip = this.txtZip.Text;
        this.contactInfo.ContactName = this.txtContact.Text;
        this.contactInfo.PhoneNumber = this.txtPhone.Text;
        this.contactInfo.FaxNumber = this.txtFax.Text;
        this.contactInfo.EmailAddress = this.txtEmail.Text;
        this.contactInfo.WebSite = this.txtWebSite.Text;
        this.contactInfo.Address.State = this.cboState.SelectedIndex > 0 ? ((USPS.State) this.cboState.SelectedItem).Abbrev : "";
      }
      this.modified = false;
    }

    private void txtEntity_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right || !((this.rolodexCategory ?? "") != "") || this.readOnly)
        return;
      this.showRolodex();
    }

    private void showRolodex()
    {
      RxContactInfo rxContactInfo = this.createRxContactInfo();
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact(this.rolodexCategory, rxContactInfo.CompanyName, rxContactInfo.LastName, rxContactInfo, CRMRoleType.NotFound, false, "", this.session))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
          this.session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        else
          this.populateRolodexInfo(rxBusinessContact.RxContactRecord);
      }
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

    private void onControlValueModified(object sender, EventArgs e)
    {
      this.modified = true;
      if (this.DataChange != null)
        this.DataChange((object) this, EventArgs.Empty);
      if (!(sender is TextBox textBox) || textBox != this.txtEntity || this.EntityNameTextChanged == null)
        return;
      this.EntityNameTextChanged((object) this, this.txtEntity.Text);
    }

    private void txtEmail_Validated(object sender, EventArgs e)
    {
      if (!(this.txtEmail.Text != "") || SystemUtil.IsEmailAddress(this.txtEmail.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The specified email address does not have a valid format.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void btnRolodex_Click(object sender, EventArgs e) => this.showRolodex();

    public string EntityNameTextBoxText
    {
      get => this.txtEntity.Text;
      set => this.txtEntity.Text = value;
    }

    public void ResizeToLandscape()
    {
      this.Size = new Size(650, (int) sbyte.MaxValue);
      this.pnlEntity.Margin = new Padding(3, 0, 3, 0);
      this.pnlAddress.Location = new Point(3, 28);
      this.pnlAddress.Margin = new Padding(3, 3, 3, 0);
      this.pnlContact.Location = new Point(3, 99);
      this.pnlContact.Margin = new Padding(3, 3, 3, 0);
      this.pnlPhone.Location = new Point(326, 3);
      this.pnlPhone.Margin = new Padding(3, 3, 3, 0);
      this.pnlFax.Location = new Point(326, 28);
      this.pnlFax.Margin = new Padding(3, 3, 3, 0);
      this.pnlEmail.Location = new Point(326, 53);
      this.pnlEmail.Margin = new Padding(3, 3, 3, 0);
      this.pnlWebSite.Location = new Point(326, 78);
      this.pnlWebSite.Margin = new Padding(3, 3, 3, 0);
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
      this.label1 = new Label();
      this.pnlEntity = new Panel();
      this.btnRolodex = new StandardIconButton();
      this.txtEntity = new TextBox();
      this.pnlAddress = new Panel();
      this.txtZip = new SizableTextBox();
      this.label10 = new Label();
      this.txtCity = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.txtStreet = new TextBox();
      this.label2 = new Label();
      this.cboState = new ComboBox();
      this.txtState = new SizableTextBox();
      this.pnlContact = new Panel();
      this.txtContact = new TextBox();
      this.label5 = new Label();
      this.pnlPhone = new Panel();
      this.txtPhone = new TextBox();
      this.label6 = new Label();
      this.pnlFax = new Panel();
      this.txtFax = new TextBox();
      this.label7 = new Label();
      this.pnlEmail = new Panel();
      this.txtEmail = new TextBox();
      this.label8 = new Label();
      this.pnlWebSite = new Panel();
      this.txtWebSite = new TextBox();
      this.label9 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.pnlEntity.SuspendLayout();
      ((ISupportInitialize) this.btnRolodex).BeginInit();
      this.pnlAddress.SuspendLayout();
      this.pnlContact.SuspendLayout();
      this.pnlPhone.SuspendLayout();
      this.pnlFax.SuspendLayout();
      this.pnlEmail.SuspendLayout();
      this.pnlWebSite.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Investor";
      this.pnlEntity.Controls.Add((Control) this.btnRolodex);
      this.pnlEntity.Controls.Add((Control) this.txtEntity);
      this.pnlEntity.Controls.Add((Control) this.label1);
      this.pnlEntity.Location = new Point(0, 3);
      this.pnlEntity.Margin = new Padding(0, 3, 0, 0);
      this.pnlEntity.Name = "pnlEntity";
      this.pnlEntity.Size = new Size(317, 22);
      this.pnlEntity.TabIndex = 1;
      this.btnRolodex.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRolodex.BackColor = Color.Transparent;
      this.btnRolodex.Location = new Point(290, 3);
      this.btnRolodex.MouseDownImage = (Image) null;
      this.btnRolodex.Name = "btnRolodex";
      this.btnRolodex.Size = new Size(16, 16);
      this.btnRolodex.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnRolodex.TabIndex = 2;
      this.btnRolodex.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRolodex, "Select a Business Contact");
      this.btnRolodex.Click += new EventHandler(this.btnRolodex_Click);
      this.txtEntity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEntity.Location = new Point(70, 1);
      this.txtEntity.MaxLength = 60;
      this.txtEntity.Name = "txtEntity";
      this.txtEntity.Size = new Size(216, 20);
      this.txtEntity.TabIndex = 1;
      this.txtEntity.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtEntity.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.pnlAddress.Controls.Add((Control) this.txtZip);
      this.pnlAddress.Controls.Add((Control) this.label10);
      this.pnlAddress.Controls.Add((Control) this.txtCity);
      this.pnlAddress.Controls.Add((Control) this.label4);
      this.pnlAddress.Controls.Add((Control) this.label3);
      this.pnlAddress.Controls.Add((Control) this.txtStreet);
      this.pnlAddress.Controls.Add((Control) this.label2);
      this.pnlAddress.Controls.Add((Control) this.cboState);
      this.pnlAddress.Controls.Add((Control) this.txtState);
      this.pnlAddress.Location = new Point(0, 25);
      this.pnlAddress.Margin = new Padding(0);
      this.pnlAddress.Name = "pnlAddress";
      this.pnlAddress.Size = new Size(317, 68);
      this.pnlAddress.TabIndex = 2;
      this.txtZip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtZip.BackColor = SystemColors.Window;
      this.txtZip.BorderColor = Color.Black;
      this.txtZip.Location = new Point(178, 45);
      this.txtZip.MaxLength = 10;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(128, 22);
      this.txtZip.TabIndex = 4;
      this.txtZip.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtZip.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(151, 49);
      this.label10.Name = "label10";
      this.label10.Size = new Size(22, 14);
      this.label10.TabIndex = 6;
      this.label10.Text = "Zip";
      this.txtCity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCity.Location = new Point(70, 23);
      this.txtCity.MaxLength = 60;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(236, 20);
      this.txtCity.TabIndex = 2;
      this.txtCity.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtCity.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 49);
      this.label4.Name = "label4";
      this.label4.Size = new Size(32, 14);
      this.label4.TabIndex = 0;
      this.label4.Text = "State";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 27);
      this.label3.Name = "label3";
      this.label3.Size = new Size(25, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "City";
      this.txtStreet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStreet.Location = new Point(70, 1);
      this.txtStreet.Margin = new Padding(3, 0, 3, 0);
      this.txtStreet.MaxLength = 100;
      this.txtStreet.Name = "txtStreet";
      this.txtStreet.Size = new Size(236, 20);
      this.txtStreet.TabIndex = 1;
      this.txtStreet.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtStreet.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(49, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Address";
      this.cboState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboState.FormattingEnabled = true;
      this.cboState.Location = new Point(70, 45);
      this.cboState.Name = "cboState";
      this.cboState.Size = new Size(64, 22);
      this.cboState.TabIndex = 3;
      this.cboState.SelectedIndexChanged += new EventHandler(this.onControlValueModified);
      this.cboState.MouseClick += new MouseEventHandler(this.txtEntity_MouseClick);
      this.txtState.BackColor = SystemColors.Window;
      this.txtState.BorderColor = Color.Black;
      this.txtState.Location = new Point(70, 45);
      this.txtState.MaxLength = 10;
      this.txtState.Name = "txtState";
      this.txtState.ReadOnly = true;
      this.txtState.Size = new Size(64, 22);
      this.txtState.TabIndex = 5;
      this.txtState.Visible = false;
      this.pnlContact.Controls.Add((Control) this.txtContact);
      this.pnlContact.Controls.Add((Control) this.label5);
      this.pnlContact.Location = new Point(0, 93);
      this.pnlContact.Margin = new Padding(0);
      this.pnlContact.Name = "pnlContact";
      this.pnlContact.Size = new Size(317, 22);
      this.pnlContact.TabIndex = 3;
      this.txtContact.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContact.Location = new Point(70, 1);
      this.txtContact.MaxLength = 60;
      this.txtContact.Name = "txtContact";
      this.txtContact.Size = new Size(236, 20);
      this.txtContact.TabIndex = 1;
      this.txtContact.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtContact.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 4);
      this.label5.Name = "label5";
      this.label5.Size = new Size(44, 14);
      this.label5.TabIndex = 0;
      this.label5.Text = "Contact";
      this.pnlPhone.Controls.Add((Control) this.txtPhone);
      this.pnlPhone.Controls.Add((Control) this.label6);
      this.pnlPhone.Location = new Point(0, 115);
      this.pnlPhone.Margin = new Padding(0);
      this.pnlPhone.Name = "pnlPhone";
      this.pnlPhone.Size = new Size(317, 22);
      this.pnlPhone.TabIndex = 4;
      this.txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPhone.Location = new Point(70, 1);
      this.txtPhone.MaxLength = 30;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(156, 20);
      this.txtPhone.TabIndex = 1;
      this.txtPhone.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtPhone.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 4);
      this.label6.Name = "label6";
      this.label6.Size = new Size(37, 14);
      this.label6.TabIndex = 0;
      this.label6.Text = "Phone";
      this.pnlFax.Controls.Add((Control) this.txtFax);
      this.pnlFax.Controls.Add((Control) this.label7);
      this.pnlFax.Location = new Point(0, 137);
      this.pnlFax.Margin = new Padding(0);
      this.pnlFax.Name = "pnlFax";
      this.pnlFax.Size = new Size(317, 22);
      this.pnlFax.TabIndex = 5;
      this.txtFax.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFax.Location = new Point(70, 1);
      this.txtFax.MaxLength = 30;
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size(156, 20);
      this.txtFax.TabIndex = 1;
      this.txtFax.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtFax.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 4);
      this.label7.Name = "label7";
      this.label7.Size = new Size(25, 14);
      this.label7.TabIndex = 0;
      this.label7.Text = "Fax";
      this.pnlEmail.Controls.Add((Control) this.txtEmail);
      this.pnlEmail.Controls.Add((Control) this.label8);
      this.pnlEmail.Location = new Point(0, 159);
      this.pnlEmail.Margin = new Padding(0);
      this.pnlEmail.Name = "pnlEmail";
      this.pnlEmail.Size = new Size(317, 22);
      this.pnlEmail.TabIndex = 6;
      this.txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEmail.Location = new Point(70, 1);
      this.txtEmail.MaxLength = 100;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(236, 20);
      this.txtEmail.TabIndex = 1;
      this.txtEmail.TextChanged += new EventHandler(this.onControlValueModified);
      this.txtEmail.MouseDown += new MouseEventHandler(this.txtEntity_MouseClick);
      this.txtEmail.Validated += new EventHandler(this.txtEmail_Validated);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 4);
      this.label8.Name = "label8";
      this.label8.Size = new Size(31, 14);
      this.label8.TabIndex = 0;
      this.label8.Text = "Email";
      this.pnlWebSite.Controls.Add((Control) this.txtWebSite);
      this.pnlWebSite.Controls.Add((Control) this.label9);
      this.pnlWebSite.Location = new Point(0, 181);
      this.pnlWebSite.Margin = new Padding(0);
      this.pnlWebSite.Name = "pnlWebSite";
      this.pnlWebSite.Size = new Size(317, 22);
      this.pnlWebSite.TabIndex = 7;
      this.txtWebSite.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWebSite.Location = new Point(70, 1);
      this.txtWebSite.MaxLength = 100;
      this.txtWebSite.Name = "txtWebSite";
      this.txtWebSite.Size = new Size(236, 20);
      this.txtWebSite.TabIndex = 1;
      this.txtWebSite.TextChanged += new EventHandler(this.onControlValueModified);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 4);
      this.label9.Name = "label9";
      this.label9.Size = new Size(50, 14);
      this.label9.TabIndex = 0;
      this.label9.Text = "Web Site";
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlEntity);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlAddress);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlContact);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlPhone);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlFax);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlEmail);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlWebSite);
      this.flowLayoutPanel1.Dock = DockStyle.Fill;
      this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
      this.flowLayoutPanel1.Location = new Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(317, 233);
      this.flowLayoutPanel1.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ContactInfoEditor);
      this.Size = new Size(317, 233);
      this.pnlEntity.ResumeLayout(false);
      this.pnlEntity.PerformLayout();
      ((ISupportInitialize) this.btnRolodex).EndInit();
      this.pnlAddress.ResumeLayout(false);
      this.pnlAddress.PerformLayout();
      this.pnlContact.ResumeLayout(false);
      this.pnlContact.PerformLayout();
      this.pnlPhone.ResumeLayout(false);
      this.pnlPhone.PerformLayout();
      this.pnlFax.ResumeLayout(false);
      this.pnlFax.PerformLayout();
      this.pnlEmail.ResumeLayout(false);
      this.pnlEmail.PerformLayout();
      this.pnlWebSite.ResumeLayout(false);
      this.pnlWebSite.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void EntityNameTextChangeEventHandler(object sender, string newName);
  }
}
