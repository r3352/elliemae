// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorrowerInfo2Form
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorrowerInfo2Form : Form, IBindingForm
  {
    private bool changed;
    private BorrowerStatus borStatus;
    private bool intermidiateData;
    private bool deleteBackKey;
    private bool initialLoad = true;
    private System.ComponentModel.Container components;
    private TextBox txtBoxSpouseName;
    private CheckBox chkBoxMarried;
    private Label lblAnniversary;
    private Label lblBirthday;
    private TextBox txtBoxAnniversary;
    private Label lblSpouse;
    private BorrowerInfo contactInfo;
    private Label lblBorrowerType;
    private Label lblBorrowerStatus;
    private Label lblReferral;
    private TextBox txtBoxOwner;
    private Label lblOwner;
    private ComboBox cmbBoxContactType;
    private ComboBox cmbBoxStatus;
    private CheckBox chkBoxAccess;
    private CheckBox chkBoxAccessPublic;
    private NoContextMenuTextBox txtBoxReferral;
    private ToolTip statusToolTip;
    private NoContextMenuTextBox txtBoxLeadTransID;
    private Label label2;
    private GroupContainer gcContactProperty;
    private GroupContainer gcCampaign;
    private Label label3;
    private Label label7;
    private Label label5;
    private Label label4;
    private CheckBox chkBoxNoFax;
    private CheckBox chkBoxNoSpam;
    private CheckBox chkBoxNoCall;
    private Panel pnlMiddle;
    private Label label8;
    private CheckBox chkPrimary;
    private ContactAppointmentsPage apptPage;
    private ContactGroupsPage groupPage;
    private Panel pnlAppointment;
    private Panel pnlGroups;
    private StandardIconButton siBtnSearchReferral;
    private DatePicker dpBirthday;
    public bool IsReadOnly;

    public event BorrowerSummaryChangedEventHandler SummaryChanged;

    public event EventHandler RequireContactRefresh;

    private void setChkBoxAccessVisibility(bool visible) => this.chkBoxAccess.Visible = visible;

    private void setChkBoxAccessPublicVisibility(bool visible)
    {
      this.chkBoxAccessPublic.Checked = true;
      this.chkBoxAccessPublic.Visible = visible;
    }

    public BorrowerInfo2Form()
    {
      this.InitializeComponent();
      this.statusToolTip = new ToolTip();
      this.statusToolTip.AutoPopDelay = 3000;
      this.statusToolTip.InitialDelay = 500;
      this.statusToolTip.ReshowDelay = 100;
      this.statusToolTip.SetToolTip((Control) this.cmbBoxStatus, "To setup values for contact status, go to Settings\\Contact Setup\\Borrower Contact Status");
      this.chkBoxAccessPublic.Size = this.chkBoxAccess.Size;
      this.chkBoxAccessPublic.Location = this.chkBoxAccess.Location;
      this.cmbBoxContactType.Items.AddRange(BorrowerTypeEnumUtil.GetDisplayNames());
      BorrowerStatus borrowerStatus = Session.ContactManager.GetBorrowerStatus();
      this.cmbBoxStatus.Items.Clear();
      this.cmbBoxStatus.Items.Add((object) new BorrowerStatusItem("", -1));
      this.cmbBoxStatus.Items.AddRange((object[]) borrowerStatus.Items);
      this.apptPage = new ContactAppointmentsPage();
      this.apptPage.TopLevel = false;
      this.apptPage.Dock = DockStyle.Fill;
      this.apptPage.Visible = true;
      this.apptPage.AppointmentModified += new EventHandler(this.apptPage_AppointmentModified);
      this.pnlAppointment.Controls.Add((Control) this.apptPage);
      this.groupPage = new ContactGroupsPage(ContactType.Borrower);
      this.groupPage.TopLevel = false;
      this.groupPage.Dock = DockStyle.Fill;
      this.groupPage.Visible = true;
      this.groupPage.GroupsModified += new EventHandler(this.groupPage_GroupsModified);
      this.pnlGroups.Controls.Add((Control) this.groupPage);
      this.CurrentContactID = -1;
    }

    private void groupPage_GroupsModified(object sender, EventArgs e)
    {
      if (this.RequireContactRefresh == null)
        return;
      this.RequireContactRefresh((object) null, (EventArgs) null);
    }

    private void apptPage_AppointmentModified(object sender, EventArgs e)
    {
      if (this.RequireContactRefresh == null)
        return;
      this.RequireContactRefresh((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtBoxAnniversary = new TextBox();
      this.txtBoxSpouseName = new TextBox();
      this.lblSpouse = new Label();
      this.chkBoxMarried = new CheckBox();
      this.lblAnniversary = new Label();
      this.lblBirthday = new Label();
      this.txtBoxLeadTransID = new NoContextMenuTextBox();
      this.label2 = new Label();
      this.txtBoxReferral = new NoContextMenuTextBox();
      this.chkBoxAccess = new CheckBox();
      this.chkBoxAccessPublic = new CheckBox();
      this.lblReferral = new Label();
      this.txtBoxOwner = new TextBox();
      this.lblOwner = new Label();
      this.lblBorrowerType = new Label();
      this.cmbBoxContactType = new ComboBox();
      this.lblBorrowerStatus = new Label();
      this.cmbBoxStatus = new ComboBox();
      this.gcContactProperty = new GroupContainer();
      this.siBtnSearchReferral = new StandardIconButton();
      this.label8 = new Label();
      this.chkPrimary = new CheckBox();
      this.gcCampaign = new GroupContainer();
      this.dpBirthday = new DatePicker();
      this.label7 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.chkBoxNoFax = new CheckBox();
      this.chkBoxNoSpam = new CheckBox();
      this.chkBoxNoCall = new CheckBox();
      this.label3 = new Label();
      this.pnlMiddle = new Panel();
      this.pnlAppointment = new Panel();
      this.pnlGroups = new Panel();
      this.gcContactProperty.SuspendLayout();
      ((ISupportInitialize) this.siBtnSearchReferral).BeginInit();
      this.gcCampaign.SuspendLayout();
      this.pnlMiddle.SuspendLayout();
      this.SuspendLayout();
      this.txtBoxAnniversary.Location = new Point(87, 99);
      this.txtBoxAnniversary.MaxLength = 10;
      this.txtBoxAnniversary.Name = "txtBoxAnniversary";
      this.txtBoxAnniversary.Size = new Size(153, 20);
      this.txtBoxAnniversary.TabIndex = 14;
      this.txtBoxAnniversary.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxAnniversary.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxAnniversary.Leave += new EventHandler(this.txtBoxAnniversary_Leave);
      this.txtBoxSpouseName.Location = new Point(87, 77);
      this.txtBoxSpouseName.MaxLength = 50;
      this.txtBoxSpouseName.Name = "txtBoxSpouseName";
      this.txtBoxSpouseName.Size = new Size(153, 20);
      this.txtBoxSpouseName.TabIndex = 12;
      this.txtBoxSpouseName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblSpouse.Location = new Point(10, 81);
      this.lblSpouse.Name = "lblSpouse";
      this.lblSpouse.Size = new Size(44, 14);
      this.lblSpouse.TabIndex = 11;
      this.lblSpouse.Text = "Spouse";
      this.chkBoxMarried.BackColor = Color.Transparent;
      this.chkBoxMarried.Location = new Point(87, 60);
      this.chkBoxMarried.Name = "chkBoxMarried";
      this.chkBoxMarried.Size = new Size(19, 16);
      this.chkBoxMarried.TabIndex = 10;
      this.chkBoxMarried.UseVisualStyleBackColor = false;
      this.chkBoxMarried.Click += new EventHandler(this.summaryFieldChanged);
      this.chkBoxMarried.CheckedChanged += new EventHandler(this.chkBoxMarried_CheckedChanged);
      this.lblAnniversary.Location = new Point(10, 102);
      this.lblAnniversary.Name = "lblAnniversary";
      this.lblAnniversary.Size = new Size(67, 14);
      this.lblAnniversary.TabIndex = 13;
      this.lblAnniversary.Text = "Anniversary";
      this.lblBirthday.Location = new Point(10, 39);
      this.lblBirthday.Name = "lblBirthday";
      this.lblBirthday.Size = new Size(53, 14);
      this.lblBirthday.TabIndex = 1;
      this.lblBirthday.Text = "Birthday";
      this.txtBoxLeadTransID.Location = new Point(103, 106);
      this.txtBoxLeadTransID.Name = "txtBoxLeadTransID";
      this.txtBoxLeadTransID.ReadOnly = true;
      this.txtBoxLeadTransID.Size = new Size(133, 20);
      this.txtBoxLeadTransID.TabIndex = 21;
      this.txtBoxLeadTransID.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.label2.Location = new Point(10, 109);
      this.label2.Name = "label2";
      this.label2.Size = new Size(76, 14);
      this.label2.TabIndex = 20;
      this.label2.Text = "Lead Trans #";
      this.txtBoxReferral.Location = new Point(103, 84);
      this.txtBoxReferral.Name = "txtBoxReferral";
      this.txtBoxReferral.Size = new Size(112, 20);
      this.txtBoxReferral.TabIndex = 5;
      this.txtBoxReferral.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxReferral.MouseUp += new MouseEventHandler(this.txtBoxReferral_MouseUp);
      this.chkBoxAccess.Location = new Point(103, 168);
      this.chkBoxAccess.Name = "chkBoxAccess";
      this.chkBoxAccess.Size = new Size(76, 16);
      this.chkBoxAccess.TabIndex = 16;
      this.chkBoxAccess.Text = "Public";
      this.chkBoxAccess.Click += new EventHandler(this.summaryFieldChanged);
      this.chkBoxAccessPublic.Checked = true;
      this.chkBoxAccessPublic.CheckState = CheckState.Checked;
      this.chkBoxAccessPublic.Enabled = false;
      this.chkBoxAccessPublic.Location = new Point(103, 183);
      this.chkBoxAccessPublic.Name = "chkBoxAccessPublic";
      this.chkBoxAccessPublic.Size = new Size(76, 16);
      this.chkBoxAccessPublic.TabIndex = 17;
      this.chkBoxAccessPublic.Text = "Public";
      this.lblReferral.Location = new Point(10, 87);
      this.lblReferral.Name = "lblReferral";
      this.lblReferral.Size = new Size(64, 14);
      this.lblReferral.TabIndex = 3;
      this.lblReferral.Text = "Referral";
      this.txtBoxOwner.Location = new Point(103, 145);
      this.txtBoxOwner.MaxLength = 50;
      this.txtBoxOwner.Name = "txtBoxOwner";
      this.txtBoxOwner.ReadOnly = true;
      this.txtBoxOwner.Size = new Size(133, 20);
      this.txtBoxOwner.TabIndex = 19;
      this.txtBoxOwner.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblOwner.Location = new Point(10, 149);
      this.lblOwner.Name = "lblOwner";
      this.lblOwner.Size = new Size(83, 14);
      this.lblOwner.TabIndex = 18;
      this.lblOwner.Text = "Contact Owner";
      this.lblBorrowerType.Location = new Point(10, 39);
      this.lblBorrowerType.Name = "lblBorrowerType";
      this.lblBorrowerType.Size = new Size(76, 14);
      this.lblBorrowerType.TabIndex = 0;
      this.lblBorrowerType.Text = "Contact Type";
      this.cmbBoxContactType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxContactType.Location = new Point(103, 36);
      this.cmbBoxContactType.Name = "cmbBoxContactType";
      this.cmbBoxContactType.Size = new Size(133, 22);
      this.cmbBoxContactType.TabIndex = 1;
      this.cmbBoxContactType.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
      this.lblBorrowerStatus.Location = new Point(10, 64);
      this.lblBorrowerStatus.Name = "lblBorrowerStatus";
      this.lblBorrowerStatus.Size = new Size(43, 14);
      this.lblBorrowerStatus.TabIndex = 2;
      this.lblBorrowerStatus.Text = "Status";
      this.cmbBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxStatus.Location = new Point(103, 60);
      this.cmbBoxStatus.Name = "cmbBoxStatus";
      this.cmbBoxStatus.Size = new Size(133, 22);
      this.cmbBoxStatus.TabIndex = 3;
      this.cmbBoxStatus.MouseHover += new EventHandler(this.cmbBoxStatus_MouseHover);
      this.cmbBoxStatus.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
      this.gcContactProperty.Controls.Add((Control) this.siBtnSearchReferral);
      this.gcContactProperty.Controls.Add((Control) this.label8);
      this.gcContactProperty.Controls.Add((Control) this.chkPrimary);
      this.gcContactProperty.Controls.Add((Control) this.txtBoxLeadTransID);
      this.gcContactProperty.Controls.Add((Control) this.chkBoxAccess);
      this.gcContactProperty.Controls.Add((Control) this.chkBoxAccessPublic);
      this.gcContactProperty.Controls.Add((Control) this.lblBorrowerType);
      this.gcContactProperty.Controls.Add((Control) this.label2);
      this.gcContactProperty.Controls.Add((Control) this.txtBoxReferral);
      this.gcContactProperty.Controls.Add((Control) this.cmbBoxContactType);
      this.gcContactProperty.Controls.Add((Control) this.txtBoxOwner);
      this.gcContactProperty.Controls.Add((Control) this.lblOwner);
      this.gcContactProperty.Controls.Add((Control) this.cmbBoxStatus);
      this.gcContactProperty.Controls.Add((Control) this.lblBorrowerStatus);
      this.gcContactProperty.Controls.Add((Control) this.lblReferral);
      this.gcContactProperty.Dock = DockStyle.Left;
      this.gcContactProperty.HeaderForeColor = SystemColors.ControlText;
      this.gcContactProperty.Location = new Point(1, 1);
      this.gcContactProperty.Name = "gcContactProperty";
      this.gcContactProperty.Padding = new Padding(0, 0, 4, 0);
      this.gcContactProperty.Size = new Size(249, 266);
      this.gcContactProperty.TabIndex = 3;
      this.gcContactProperty.Text = "Contact Properties";
      this.siBtnSearchReferral.BackColor = Color.Transparent;
      this.siBtnSearchReferral.Location = new Point(220, 87);
      this.siBtnSearchReferral.Name = "siBtnSearchReferral";
      this.siBtnSearchReferral.Size = new Size(16, 16);
      this.siBtnSearchReferral.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.siBtnSearchReferral.TabIndex = 24;
      this.siBtnSearchReferral.TabStop = false;
      this.siBtnSearchReferral.Click += new EventHandler(this.siBtnSearchReferral_Click);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 129);
      this.label8.Name = "label8";
      this.label8.Size = new Size(83, 14);
      this.label8.TabIndex = 23;
      this.label8.Text = "Primary Contact";
      this.chkPrimary.AutoSize = true;
      this.chkPrimary.Location = new Point(103, 129);
      this.chkPrimary.Name = "chkPrimary";
      this.chkPrimary.Size = new Size(15, 14);
      this.chkPrimary.TabIndex = 22;
      this.chkPrimary.UseVisualStyleBackColor = true;
      this.chkPrimary.Click += new EventHandler(this.summaryFieldChanged);
      this.gcCampaign.Controls.Add((Control) this.dpBirthday);
      this.gcCampaign.Controls.Add((Control) this.label7);
      this.gcCampaign.Controls.Add((Control) this.label5);
      this.gcCampaign.Controls.Add((Control) this.label4);
      this.gcCampaign.Controls.Add((Control) this.chkBoxNoFax);
      this.gcCampaign.Controls.Add((Control) this.chkBoxNoSpam);
      this.gcCampaign.Controls.Add((Control) this.chkBoxNoCall);
      this.gcCampaign.Controls.Add((Control) this.label3);
      this.gcCampaign.Controls.Add((Control) this.lblBirthday);
      this.gcCampaign.Controls.Add((Control) this.lblAnniversary);
      this.gcCampaign.Controls.Add((Control) this.chkBoxMarried);
      this.gcCampaign.Controls.Add((Control) this.txtBoxAnniversary);
      this.gcCampaign.Controls.Add((Control) this.lblSpouse);
      this.gcCampaign.Controls.Add((Control) this.txtBoxSpouseName);
      this.gcCampaign.Dock = DockStyle.Left;
      this.gcCampaign.HeaderForeColor = SystemColors.ControlText;
      this.gcCampaign.Location = new Point(4, 0);
      this.gcCampaign.Margin = new Padding(4, 3, 3, 3);
      this.gcCampaign.Name = "gcCampaign";
      this.gcCampaign.Size = new Size(249, 266);
      this.gcCampaign.TabIndex = 4;
      this.gcCampaign.Text = "Campaign Information";
      this.dpBirthday.BackColor = SystemColors.Window;
      this.dpBirthday.Location = new Point(87, 36);
      this.dpBirthday.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpBirthday.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpBirthday.Name = "dpBirthday";
      this.dpBirthday.Size = new Size(153, 22);
      this.dpBirthday.TabIndex = 31;
      this.dpBirthday.Value = new DateTime(0L);
      this.dpBirthday.ValueChanged += new EventHandler(this.summaryFieldChanged);
      this.label7.Location = new Point(10, 158);
      this.label7.Name = "label7";
      this.label7.Size = new Size(64, 14);
      this.label7.TabIndex = 30;
      this.label7.Text = "Do not Fax";
      this.label5.Location = new Point(10, 140);
      this.label5.Name = "label5";
      this.label5.Size = new Size(66, 14);
      this.label5.TabIndex = 29;
      this.label5.Text = "Do not Email";
      this.label4.Location = new Point(10, 122);
      this.label4.Name = "label4";
      this.label4.Size = new Size(64, 14);
      this.label4.TabIndex = 28;
      this.label4.Text = "Do not Call";
      this.chkBoxNoFax.Location = new Point(87, 158);
      this.chkBoxNoFax.Name = "chkBoxNoFax";
      this.chkBoxNoFax.Size = new Size(19, 14);
      this.chkBoxNoFax.TabIndex = 27;
      this.chkBoxNoFax.Click += new EventHandler(this.summaryFieldChanged);
      this.chkBoxNoSpam.Location = new Point(87, 140);
      this.chkBoxNoSpam.Name = "chkBoxNoSpam";
      this.chkBoxNoSpam.Size = new Size(19, 14);
      this.chkBoxNoSpam.TabIndex = 26;
      this.chkBoxNoSpam.Click += new EventHandler(this.summaryFieldChanged);
      this.chkBoxNoCall.Location = new Point(87, 122);
      this.chkBoxNoCall.Name = "chkBoxNoCall";
      this.chkBoxNoCall.Size = new Size(19, 14);
      this.chkBoxNoCall.TabIndex = 25;
      this.chkBoxNoCall.Click += new EventHandler(this.summaryFieldChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 61);
      this.label3.Name = "label3";
      this.label3.Size = new Size(43, 14);
      this.label3.TabIndex = 11;
      this.label3.Text = "Married";
      this.pnlMiddle.Controls.Add((Control) this.pnlAppointment);
      this.pnlMiddle.Controls.Add((Control) this.gcCampaign);
      this.pnlMiddle.Dock = DockStyle.Fill;
      this.pnlMiddle.Location = new Point(250, 1);
      this.pnlMiddle.Name = "pnlMiddle";
      this.pnlMiddle.Padding = new Padding(4, 0, 4, 0);
      this.pnlMiddle.Size = new Size(510, 266);
      this.pnlMiddle.TabIndex = 6;
      this.pnlAppointment.Dock = DockStyle.Fill;
      this.pnlAppointment.Location = new Point(253, 0);
      this.pnlAppointment.Name = "pnlAppointment";
      this.pnlAppointment.Padding = new Padding(4, 0, 0, 0);
      this.pnlAppointment.Size = new Size(253, 266);
      this.pnlAppointment.TabIndex = 6;
      this.pnlGroups.Dock = DockStyle.Right;
      this.pnlGroups.Location = new Point(760, 1);
      this.pnlGroups.Name = "pnlGroups";
      this.pnlGroups.Size = new Size(249, 266);
      this.pnlGroups.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Window;
      this.ClientSize = new Size(1010, 268);
      this.Controls.Add((Control) this.pnlMiddle);
      this.Controls.Add((Control) this.pnlGroups);
      this.Controls.Add((Control) this.gcContactProperty);
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorrowerInfo2Form);
      this.Padding = new Padding(1);
      this.Text = "BorrowerInfoForm";
      this.SizeChanged += new EventHandler(this.BorrowerInfoForm_SizeChanged);
      this.gcContactProperty.ResumeLayout(false);
      this.gcContactProperty.PerformLayout();
      ((ISupportInitialize) this.siBtnSearchReferral).EndInit();
      this.gcCampaign.ResumeLayout(false);
      this.gcCampaign.PerformLayout();
      this.pnlMiddle.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public bool isDirty() => this.changed;

    public void AddAppointment() => this.apptPage.AddAppointment();

    public int CurrentContactID
    {
      get => this.contactInfo != null ? this.contactInfo.ContactID : -1;
      set
      {
        if (this.CurrentContactID == value)
          return;
        this.contactInfo = (BorrowerInfo) null;
        this.apptPage.CurrentContact = (ContactInfo) null;
        this.groupPage.CurrentContact = value;
        this.disableControls();
        if (value < 0)
          return;
        this.contactInfo = Session.ContactManager.GetBorrower(value);
        if (this.contactInfo == null)
          throw new ObjectNotFoundException("Unable to retrieve borrower contact.", ObjectType.Contact, (object) value);
        this.apptPage.CurrentContact = new ContactInfo(this.contactInfo.FirstName + " " + this.contactInfo.LastName, this.contactInfo.ContactID.ToString(), CategoryType.Borrower);
        this.groupPage.CurrentContact = this.contactInfo.ContactID;
        this.loadForm();
      }
    }

    public object CurrentContact
    {
      get => (object) this.contactInfo;
      set
      {
        if (this.CurrentContact == value)
          return;
        this.contactInfo = (BorrowerInfo) null;
        this.apptPage.CurrentContact = (ContactInfo) null;
        this.groupPage.CurrentContact = -1;
        this.disableControls();
        if (value == null)
          return;
        this.contactInfo = (BorrowerInfo) value;
        this.apptPage.CurrentContact = new ContactInfo(this.contactInfo.FirstName + " " + this.contactInfo.LastName, this.contactInfo.ContactID.ToString(), CategoryType.Borrower);
        this.groupPage.CurrentContact = this.contactInfo.ContactID;
        this.loadForm();
      }
    }

    public bool SaveChanges()
    {
      if (!this.changed || this.contactInfo == null)
        return false;
      this.contactInfo = Session.ContactManager.GetBorrower(this.CurrentContactID);
      this.contactInfo.ContactType = BorrowerTypeEnumUtil.NameToValue(this.cmbBoxContactType.Text);
      this.contactInfo.Status = this.cmbBoxStatus.Text;
      this.contactInfo.Referral = this.txtBoxReferral.Text;
      this.contactInfo.AccessLevel = this.chkBoxAccess.Checked ? ContactAccess.Public : ContactAccess.Private;
      this.contactInfo.PrimaryContact = this.chkPrimary.Checked;
      this.contactInfo.Birthdate = this.dpBirthday.Text == "" || this.dpBirthday.Text == "//" ? DateTime.MinValue : this.dpBirthday.Value;
      this.contactInfo.Married = this.chkBoxMarried.Checked;
      this.contactInfo.SpouseName = this.txtBoxSpouseName.Text;
      this.contactInfo.Anniversary = this.txtBoxAnniversary.Text == "" || this.txtBoxAnniversary.Text == "/" ? DateTime.MinValue : Utils.ParseDate((object) (this.txtBoxAnniversary.Text + "/2000"));
      this.contactInfo.NoSpam = this.chkBoxNoSpam.Checked;
      this.contactInfo.NoCall = this.chkBoxNoCall.Checked;
      this.contactInfo.NoFax = this.chkBoxNoFax.Checked;
      try
      {
        Session.ContactManager.UpdateBorrower(this.contactInfo);
      }
      catch (Exception ex)
      {
        throw new ObjectNotFoundException("Unable to update borrower contact '" + this.contactInfo.LastName + ", " + this.contactInfo.FirstName + "' (ContactID:" + (object) this.contactInfo.ContactID + ").", ObjectType.Contact, (object) this.contactInfo.ContactID);
      }
      this.changed = false;
      return true;
    }

    public void RefreshData() => this.borStatus = Session.ContactManager.GetBorrowerStatus();

    private void AppointmentModEvent(object sender, EventArgs e)
    {
    }

    private void reloadStatusBox()
    {
      if (this.borStatus == null)
        this.RefreshData();
      this.cmbBoxStatus.Items.Clear();
      this.cmbBoxStatus.Items.Add((object) new BorrowerStatusItem("", -1));
      this.cmbBoxStatus.Items.AddRange((object[]) this.borStatus.Items);
    }

    public void disableForm() => this.disableControlsOnly();

    private void disableControlsOnly()
    {
      this.cmbBoxContactType.Enabled = false;
      this.cmbBoxStatus.Enabled = false;
      this.txtBoxReferral.ReadOnly = true;
      this.chkBoxAccess.Enabled = false;
      this.chkPrimary.Enabled = false;
      this.chkBoxMarried.Enabled = false;
      this.txtBoxAnniversary.ReadOnly = true;
      this.dpBirthday.ReadOnly = true;
      this.txtBoxSpouseName.ReadOnly = true;
      this.chkBoxNoCall.Enabled = false;
      this.chkBoxNoFax.Enabled = false;
      this.chkBoxNoSpam.Enabled = false;
      this.apptPage.IsReadOnly = true;
      this.groupPage.IsReadOnly = true;
      this.siBtnSearchReferral.Enabled = false;
    }

    private void disableControls()
    {
      this.clearForm();
      this.disableControlsOnly();
    }

    private void enableControls()
    {
      this.cmbBoxContactType.Enabled = true;
      this.cmbBoxStatus.Enabled = true;
      this.txtBoxReferral.ReadOnly = false;
      this.chkBoxAccess.Enabled = true;
      this.chkPrimary.Enabled = true;
      this.chkBoxMarried.Enabled = true;
      this.dpBirthday.ReadOnly = false;
      this.txtBoxSpouseName.ReadOnly = false;
      this.txtBoxAnniversary.ReadOnly = false;
      this.chkBoxNoCall.Enabled = true;
      this.chkBoxNoFax.Enabled = true;
      this.chkBoxNoSpam.Enabled = true;
      this.apptPage.IsReadOnly = false;
      this.groupPage.IsReadOnly = false;
      this.siBtnSearchReferral.Enabled = true;
    }

    private void loadForm()
    {
      this.initialLoad = true;
      if (!this.IsReadOnly)
        this.enableControls();
      this.cmbBoxContactType.Text = BorrowerTypeEnumUtil.ValueToName(this.contactInfo.ContactType);
      this.reloadStatusBox();
      bool flag = false;
      foreach (BorrowerStatusItem borrowerStatusItem in this.cmbBoxStatus.Items)
      {
        if (borrowerStatusItem.name == this.contactInfo.Status)
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        this.cmbBoxStatus.Text = this.contactInfo.Status;
      }
      else
      {
        this.cmbBoxStatus.Items.Add((object) new BorrowerStatusItem(this.contactInfo.Status, -1));
        this.cmbBoxStatus.Text = this.contactInfo.Status;
      }
      this.txtBoxLeadTransID.Text = this.contactInfo.LeadTxnID;
      this.chkBoxAccess.Checked = this.contactInfo.AccessLevel == ContactAccess.Public;
      ContactAccess contactAccess = ContactAccess.Private;
      AclGroup[] groupsOfUser = Session.AclGroupManager.GetGroupsOfUser(Session.UserID);
      if (groupsOfUser != null && groupsOfUser.Length != 0)
      {
        foreach (AclGroup aclGroup in groupsOfUser)
        {
          if (aclGroup.ViewSubordinatesContacts)
          {
            contactAccess = ContactAccess.Public;
            break;
          }
        }
      }
      if (contactAccess == ContactAccess.Public)
      {
        this.setChkBoxAccessVisibility(false);
        this.setChkBoxAccessPublicVisibility(true);
      }
      else
      {
        this.setChkBoxAccessVisibility(true);
        this.setChkBoxAccessPublicVisibility(false);
      }
      this.txtBoxReferral.Text = this.contactInfo.Referral;
      string str = "No Owner";
      UserInfo user = Session.OrganizationManager.GetUser(this.contactInfo.OwnerID);
      if (user != (UserInfo) null)
        str = user.FullName;
      this.txtBoxOwner.Text = str;
      this.chkPrimary.Checked = this.contactInfo.PrimaryContact;
      this.chkBoxMarried.Checked = this.contactInfo.Married;
      if (this.contactInfo.Married)
      {
        this.txtBoxAnniversary.Enabled = true;
        this.txtBoxSpouseName.Enabled = true;
      }
      else
      {
        this.txtBoxAnniversary.Enabled = false;
        this.txtBoxSpouseName.Enabled = false;
      }
      if (this.contactInfo.Anniversary == DateTime.MinValue)
        this.txtBoxAnniversary.Text = "/";
      else
        this.txtBoxAnniversary.Text = this.contactInfo.Anniversary.ToString("MM/dd");
      if (this.contactInfo.Birthdate == DateTime.MinValue)
        this.dpBirthday.Text = "";
      else
        this.dpBirthday.Text = this.contactInfo.Birthdate.ToString("MM/dd/yyyy");
      this.txtBoxSpouseName.Text = this.contactInfo.SpouseName;
      this.chkBoxNoCall.Checked = this.contactInfo.NoCall;
      this.chkBoxNoFax.Checked = this.contactInfo.NoFax;
      this.chkBoxNoSpam.Checked = this.contactInfo.NoSpam;
      this.changed = !flag;
      this.initialLoad = false;
    }

    private void clearForm()
    {
      this.txtBoxOwner.Text = "";
      this.txtBoxReferral.Text = "";
      this.cmbBoxStatus.Text = "";
      this.cmbBoxContactType.Text = "";
      this.txtBoxLeadTransID.Text = "";
      this.txtBoxOwner.Text = "";
      this.chkBoxAccess.Checked = false;
      this.chkBoxAccessPublic.Checked = false;
      this.chkPrimary.Checked = false;
      this.txtBoxAnniversary.Text = "/";
      this.dpBirthday.Text = "";
      this.txtBoxSpouseName.Text = "";
      this.chkBoxMarried.Checked = false;
      this.chkBoxNoCall.Checked = false;
      this.chkBoxNoFax.Checked = false;
      this.chkBoxNoSpam.Checked = false;
      this.changed = false;
    }

    private void BorrowerInfoForm_SizeChanged(object sender, EventArgs e)
    {
      int width = 249;
      if (this.Width > 1010)
        width = 249 + (this.Width - 1010) / 4;
      this.gcContactProperty.Size = this.gcCampaign.Size = this.pnlGroups.Size = new Size(width, this.Height);
    }

    private void txtBoxAnniversary_Leave(object sender, EventArgs e)
    {
      if (this.txtBoxAnniversary.Text == "" || this.txtBoxAnniversary.Text == "/")
        return;
      bool flag = false;
      string s = this.txtBoxAnniversary.Text;
      int length = s.LastIndexOf('/');
      if (length != -1 && length != s.IndexOf('/'))
      {
        s = s.Substring(0, length);
        flag = true;
      }
      try
      {
        string str = DateTime.Parse(s).ToString("MM/dd");
        if (!flag && !(str != s))
          return;
        this.txtBoxAnniversary.Text = str;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Anniversay format. Please enter a month and day in MM/DD.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        if (!this.Focused)
          this.txtBoxAnniversary.Text = string.Empty;
        try
        {
          this.txtBoxAnniversary.Focus();
        }
        catch
        {
        }
      }
    }

    private void chkBoxMarried_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkBoxMarried.Checked && !this.IsReadOnly)
      {
        this.txtBoxSpouseName.Enabled = true;
        this.txtBoxAnniversary.Enabled = true;
      }
      else
      {
        this.txtBoxSpouseName.Enabled = false;
        this.txtBoxAnniversary.Enabled = false;
        this.txtBoxSpouseName.Text = string.Empty;
        this.txtBoxAnniversary.Text = "/";
      }
      this.changed = true;
    }

    private void chkBoxPrimary_CheckedChanged(object sender, EventArgs e) => this.changed = true;

    public void summaryFieldChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
      if (this.intermidiateData)
        return;
      this.changed = true;
      if (this.SummaryChanged == null)
        return;
      this.SummaryChanged();
    }

    public void formatText(object sender, EventArgs e)
    {
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        if (sender != this.txtBoxAnniversary)
          return;
        FieldFormat dataFormat = FieldFormat.MONTHDAY;
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        int newCursorPos = 0;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate, textBox.SelectionStart, ref newCursorPos);
        if (!(str != textBox.Text))
          return;
        this.intermidiateData = true;
        textBox.Text = str;
        textBox.SelectionStart = newCursorPos;
      }
    }

    private void controlChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
      this.changed = true;
    }

    private void txtBoxState_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      textBox.Text = textBox.Text.ToUpper();
    }

    private void txtBoxEmail_Leave(object sender, EventArgs e)
    {
      string text = ((Control) sender).Text;
      if (text == "" || SystemUtil.IsEmailAddress(text))
        return;
      int num = (int) MessageBox.Show("Invalid email address.", "Email Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void txtBoxKeyDown(object sender, KeyEventArgs e)
    {
      if (this.initialLoad || e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void txtBoxReferral_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right || !this.txtBoxReferral.Enabled || this.txtBoxReferral.ReadOnly)
        return;
      this.siBtnSearchReferral_Click((object) null, (EventArgs) null);
    }

    private void cmbBoxStatus_MouseHover(object sender, EventArgs e)
    {
      if (this.cmbBoxStatus.Items.Count <= 1)
        this.statusToolTip.Active = true;
      else
        this.statusToolTip.Active = false;
    }

    private void siBtnSearchReferral_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      string[] firstLastName = ContactUtil.GetFirstLastName(this.txtBoxReferral.Text);
      rxContact.FirstName = firstLastName[0];
      rxContact.LastName = firstLastName[1];
      RxBusinessContact rxBusinessContact = new RxBusinessContact("", "", rxContact.LastName, rxContact, false, CRMRoleType.NotFound, true, "");
      if (rxBusinessContact.ShowDialog() != DialogResult.OK)
        return;
      if (rxBusinessContact.GoToContact)
      {
        Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
      }
      else
      {
        if (!(rxBusinessContact.RxContactRecord != (RxContactInfo) null))
          return;
        this.txtBoxReferral.Text = rxBusinessContact.RxContactRecord.FirstName + " " + rxBusinessContact.RxContactRecord.LastName;
      }
    }
  }
}
