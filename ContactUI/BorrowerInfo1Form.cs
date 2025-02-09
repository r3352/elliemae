// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorrowerInfo1Form
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine.MilestoneManagement;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Xml;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorrowerInfo1Form : Form, IBindingForm
  {
    private static string sw = Tracing.SwContact;
    private bool changed;
    private bool intermidiateData;
    private bool deleteBackKey;
    private bool initialLoad = true;
    private TextBox txtBoxLastName;
    private Label lblLastName;
    private Label lblFirstName;
    private TextBox txtBoxFirstName;
    private TextBox txtBoxHomeState;
    private TextBox txtBoxHomeZip;
    private Label lblHomeZip;
    private Label lblHomeState;
    private TextBox txtBoxHomeCity;
    private TextBox txtBoxHomeAddress2;
    private Label lblHomeCity;
    private Label lblHomeAddress2;
    private Label lblHomeAddress1;
    private TextBox txtBoxHomeAddress1;
    private IContainer components;
    private TextBox txtBoxHomePhone;
    private Panel pnlContactInfo;
    private BorrowerInfo contactInfo;
    private Label lblHomePhone;
    private Label lblWorkPhone;
    private Label lblHomeEmail;
    private TextBox txtBoxWorkPhone;
    private Label lblCellPhone;
    private TextBox txtBoxFaxNumber;
    private Label lblFaxNumber;
    private Label lblWorkEmail;
    private bool isReadOnly;
    private TextBox txtBoxPersonalEmail;
    private TextBox txtBoxMobilePhone;
    private TextBox txtBoxBizEmail;
    private TextBox txtBoxSalutation;
    private Label label1;
    private BorrowerTabForm tabForm;
    private Label lblSSN;
    private TextBox txtBoxSSN;
    private PictureBox picBoxEmailOver;
    private ToolTip toolTip;
    private Label label2;
    private TextBox txtSuffixName;
    private Label label3;
    private TextBox txtMiddleName;
    private GroupContainer gcPersonalInfo;
    private GroupContainer gcBusinessInfo;
    private GroupContainer groupContainer1;
    private TextBox txtBoxBizState;
    private TextBox txtBoxBizAddress2;
    private TextBox txtBoxBizAddress1;
    private Label lblBizZip;
    private Label lblBizCity;
    private Label lblBizState;
    private Label lblBizAddress1;
    private TextBox txtBoxBizZip;
    private Label lblBizAddress2;
    private TextBox txtBoxBizCity;
    private Label lblCompany;
    private TextBox txtBoxEmployerName;
    private Label lblTitle;
    private TextBox txtBoxJobTitle;
    private TextBox txtBoxBizWebUrl;
    private Label lblWebUrl;
    private StandardIconButton siBtnCellPhone;
    private StandardIconButton siBtnWorkPhone;
    private StandardIconButton siBtnHomePhone;
    private StandardIconButton siBtnFaxNumber;
    private StandardIconButton siBtnWorkEmail;
    private StandardIconButton siBtnHomeEmail;
    private LoanAccessBpmManager loanRuleManager;

    public event BorrowerSummaryChangedEventHandler SummaryChanged;

    public BorrowerInfo1Form(
      BorrowerTabForm tabForm,
      ContactGroupListController groupListController)
    {
      this.InitializeComponent();
      this.loanRuleManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess);
      this.tabForm = tabForm;
      this.txtBoxHomeState.CharacterCasing = CharacterCasing.Upper;
      this.CurrentContactID = -1;
      string caption1 = "Open the Contact Notes and send an email";
      this.toolTip.SetToolTip((Control) this.siBtnHomeEmail, caption1);
      this.toolTip.SetToolTip((Control) this.siBtnWorkEmail, caption1);
      string caption2 = "Open the Contact Notes";
      this.toolTip.SetToolTip((Control) this.siBtnHomePhone, caption2);
      this.toolTip.SetToolTip((Control) this.siBtnWorkPhone, caption2);
      this.toolTip.SetToolTip((Control) this.siBtnCellPhone, caption2);
      this.toolTip.SetToolTip((Control) this.siBtnFaxNumber, caption2);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BorrowerInfo1Form));
      this.pnlContactInfo = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.siBtnWorkEmail = new StandardIconButton();
      this.siBtnHomeEmail = new StandardIconButton();
      this.siBtnFaxNumber = new StandardIconButton();
      this.siBtnCellPhone = new StandardIconButton();
      this.siBtnWorkPhone = new StandardIconButton();
      this.siBtnHomePhone = new StandardIconButton();
      this.lblHomePhone = new Label();
      this.txtBoxHomePhone = new TextBox();
      this.txtBoxPersonalEmail = new TextBox();
      this.lblHomeEmail = new Label();
      this.lblWorkPhone = new Label();
      this.txtBoxWorkPhone = new TextBox();
      this.lblCellPhone = new Label();
      this.txtBoxMobilePhone = new TextBox();
      this.lblFaxNumber = new Label();
      this.lblWorkEmail = new Label();
      this.txtBoxFaxNumber = new TextBox();
      this.txtBoxBizEmail = new TextBox();
      this.picBoxEmailOver = new PictureBox();
      this.toolTip = new ToolTip(this.components);
      this.gcBusinessInfo = new GroupContainer();
      this.txtBoxBizState = new TextBox();
      this.txtBoxBizAddress2 = new TextBox();
      this.txtBoxBizAddress1 = new TextBox();
      this.lblBizZip = new Label();
      this.lblBizCity = new Label();
      this.lblBizState = new Label();
      this.lblBizAddress1 = new Label();
      this.txtBoxBizZip = new TextBox();
      this.lblBizAddress2 = new Label();
      this.txtBoxBizCity = new TextBox();
      this.lblCompany = new Label();
      this.txtBoxEmployerName = new TextBox();
      this.lblTitle = new Label();
      this.txtBoxJobTitle = new TextBox();
      this.txtBoxBizWebUrl = new TextBox();
      this.lblWebUrl = new Label();
      this.gcPersonalInfo = new GroupContainer();
      this.lblHomeZip = new Label();
      this.txtSuffixName = new TextBox();
      this.txtBoxHomeState = new TextBox();
      this.label3 = new Label();
      this.lblFirstName = new Label();
      this.txtBoxHomeAddress2 = new TextBox();
      this.txtBoxFirstName = new TextBox();
      this.txtBoxHomeAddress1 = new TextBox();
      this.txtMiddleName = new TextBox();
      this.lblLastName = new Label();
      this.lblHomeCity = new Label();
      this.lblSSN = new Label();
      this.lblHomeState = new Label();
      this.txtBoxSSN = new TextBox();
      this.lblHomeAddress1 = new Label();
      this.txtBoxHomeZip = new TextBox();
      this.label2 = new Label();
      this.lblHomeAddress2 = new Label();
      this.txtBoxLastName = new TextBox();
      this.txtBoxHomeCity = new TextBox();
      this.txtBoxSalutation = new TextBox();
      this.label1 = new Label();
      this.pnlContactInfo.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.siBtnWorkEmail).BeginInit();
      ((ISupportInitialize) this.siBtnHomeEmail).BeginInit();
      ((ISupportInitialize) this.siBtnFaxNumber).BeginInit();
      ((ISupportInitialize) this.siBtnCellPhone).BeginInit();
      ((ISupportInitialize) this.siBtnWorkPhone).BeginInit();
      ((ISupportInitialize) this.siBtnHomePhone).BeginInit();
      ((ISupportInitialize) this.picBoxEmailOver).BeginInit();
      this.gcBusinessInfo.SuspendLayout();
      this.gcPersonalInfo.SuspendLayout();
      this.SuspendLayout();
      this.pnlContactInfo.BackColor = SystemColors.Window;
      this.pnlContactInfo.Controls.Add((Control) this.groupContainer1);
      this.pnlContactInfo.Controls.Add((Control) this.picBoxEmailOver);
      this.pnlContactInfo.Dock = DockStyle.Fill;
      this.pnlContactInfo.Location = new Point(335, 1);
      this.pnlContactInfo.Name = "pnlContactInfo";
      this.pnlContactInfo.Size = new Size(340, 248);
      this.pnlContactInfo.TabIndex = 0;
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.siBtnWorkEmail);
      this.groupContainer1.Controls.Add((Control) this.siBtnHomeEmail);
      this.groupContainer1.Controls.Add((Control) this.siBtnFaxNumber);
      this.groupContainer1.Controls.Add((Control) this.siBtnCellPhone);
      this.groupContainer1.Controls.Add((Control) this.siBtnWorkPhone);
      this.groupContainer1.Controls.Add((Control) this.siBtnHomePhone);
      this.groupContainer1.Controls.Add((Control) this.lblHomePhone);
      this.groupContainer1.Controls.Add((Control) this.txtBoxHomePhone);
      this.groupContainer1.Controls.Add((Control) this.txtBoxPersonalEmail);
      this.groupContainer1.Controls.Add((Control) this.lblHomeEmail);
      this.groupContainer1.Controls.Add((Control) this.lblWorkPhone);
      this.groupContainer1.Controls.Add((Control) this.txtBoxWorkPhone);
      this.groupContainer1.Controls.Add((Control) this.lblCellPhone);
      this.groupContainer1.Controls.Add((Control) this.txtBoxMobilePhone);
      this.groupContainer1.Controls.Add((Control) this.lblFaxNumber);
      this.groupContainer1.Controls.Add((Control) this.lblWorkEmail);
      this.groupContainer1.Controls.Add((Control) this.txtBoxFaxNumber);
      this.groupContainer1.Controls.Add((Control) this.txtBoxBizEmail);
      this.groupContainer1.Location = new Point(3, 0);
      this.groupContainer1.Margin = new Padding(0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(334, 248);
      this.groupContainer1.TabIndex = 31;
      this.groupContainer1.Text = "Contact Information";
      this.siBtnWorkEmail.BackColor = Color.Transparent;
      this.siBtnWorkEmail.Enabled = false;
      this.siBtnWorkEmail.Location = new Point(250, 146);
      this.siBtnWorkEmail.Name = "siBtnWorkEmail";
      this.siBtnWorkEmail.Size = new Size(16, 16);
      this.siBtnWorkEmail.StandardButtonType = StandardIconButton.ButtonType.EmailButton;
      this.siBtnWorkEmail.TabIndex = 35;
      this.siBtnWorkEmail.TabStop = false;
      this.siBtnWorkEmail.Click += new EventHandler(this.picBoxWorkEmail_Click);
      this.siBtnHomeEmail.BackColor = Color.Transparent;
      this.siBtnHomeEmail.Enabled = false;
      this.siBtnHomeEmail.Location = new Point(250, 126);
      this.siBtnHomeEmail.Name = "siBtnHomeEmail";
      this.siBtnHomeEmail.Size = new Size(16, 16);
      this.siBtnHomeEmail.StandardButtonType = StandardIconButton.ButtonType.EmailButton;
      this.siBtnHomeEmail.TabIndex = 34;
      this.siBtnHomeEmail.TabStop = false;
      this.siBtnHomeEmail.Click += new EventHandler(this.picBoxHomeEmail_Click);
      this.siBtnFaxNumber.BackColor = Color.Transparent;
      this.siBtnFaxNumber.Enabled = false;
      this.siBtnFaxNumber.Location = new Point(250, 104);
      this.siBtnFaxNumber.Name = "siBtnFaxNumber";
      this.siBtnFaxNumber.Size = new Size(16, 16);
      this.siBtnFaxNumber.StandardButtonType = StandardIconButton.ButtonType.FaxPhoneButton;
      this.siBtnFaxNumber.TabIndex = 33;
      this.siBtnFaxNumber.TabStop = false;
      this.siBtnFaxNumber.Click += new EventHandler(this.picBoxFaxNumber_Click);
      this.siBtnCellPhone.BackColor = Color.Transparent;
      this.siBtnCellPhone.Enabled = false;
      this.siBtnCellPhone.Location = new Point(250, 81);
      this.siBtnCellPhone.Name = "siBtnCellPhone";
      this.siBtnCellPhone.Size = new Size(16, 16);
      this.siBtnCellPhone.StandardButtonType = StandardIconButton.ButtonType.CellPhoneButton;
      this.siBtnCellPhone.TabIndex = 32;
      this.siBtnCellPhone.TabStop = false;
      this.siBtnCellPhone.Click += new EventHandler(this.picBoxCellPhone_Click);
      this.siBtnWorkPhone.BackColor = Color.Transparent;
      this.siBtnWorkPhone.Enabled = false;
      this.siBtnWorkPhone.Location = new Point(250, 59);
      this.siBtnWorkPhone.Name = "siBtnWorkPhone";
      this.siBtnWorkPhone.Size = new Size(16, 16);
      this.siBtnWorkPhone.StandardButtonType = StandardIconButton.ButtonType.PhoneButton;
      this.siBtnWorkPhone.TabIndex = 31;
      this.siBtnWorkPhone.TabStop = false;
      this.siBtnWorkPhone.Click += new EventHandler(this.picBoxWorkPhone_Click);
      this.siBtnHomePhone.BackColor = Color.Transparent;
      this.siBtnHomePhone.Enabled = false;
      this.siBtnHomePhone.Location = new Point(250, 37);
      this.siBtnHomePhone.Name = "siBtnHomePhone";
      this.siBtnHomePhone.Size = new Size(16, 16);
      this.siBtnHomePhone.StandardButtonType = StandardIconButton.ButtonType.PhoneButton;
      this.siBtnHomePhone.TabIndex = 30;
      this.siBtnHomePhone.TabStop = false;
      this.siBtnHomePhone.Click += new EventHandler(this.picBoxHomePhone_Click);
      this.lblHomePhone.Location = new Point(10, 39);
      this.lblHomePhone.Name = "lblHomePhone";
      this.lblHomePhone.Size = new Size(70, 13);
      this.lblHomePhone.TabIndex = 10;
      this.lblHomePhone.Text = "Home Phone";
      this.txtBoxHomePhone.Location = new Point(90, 36);
      this.txtBoxHomePhone.MaxLength = 30;
      this.txtBoxHomePhone.Name = "txtBoxHomePhone";
      this.txtBoxHomePhone.Size = new Size(154, 20);
      this.txtBoxHomePhone.TabIndex = 12;
      this.txtBoxHomePhone.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxHomePhone.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxPersonalEmail.Location = new Point(90, 124);
      this.txtBoxPersonalEmail.MaxLength = 50;
      this.txtBoxPersonalEmail.Name = "txtBoxPersonalEmail";
      this.txtBoxPersonalEmail.Size = new Size(154, 20);
      this.txtBoxPersonalEmail.TabIndex = 16;
      this.txtBoxPersonalEmail.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblHomeEmail.Location = new Point(10, (int) sbyte.MaxValue);
      this.lblHomeEmail.Name = "lblHomeEmail";
      this.lblHomeEmail.Size = new Size(65, 15);
      this.lblHomeEmail.TabIndex = 18;
      this.lblHomeEmail.Text = "Home Email";
      this.lblWorkPhone.Location = new Point(10, 61);
      this.lblWorkPhone.Name = "lblWorkPhone";
      this.lblWorkPhone.Size = new Size(76, 15);
      this.lblWorkPhone.TabIndex = 12;
      this.lblWorkPhone.Text = "Work Phone";
      this.txtBoxWorkPhone.Location = new Point(90, 58);
      this.txtBoxWorkPhone.MaxLength = 30;
      this.txtBoxWorkPhone.Name = "txtBoxWorkPhone";
      this.txtBoxWorkPhone.Size = new Size(154, 20);
      this.txtBoxWorkPhone.TabIndex = 13;
      this.txtBoxWorkPhone.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblCellPhone.Location = new Point(10, 83);
      this.lblCellPhone.Name = "lblCellPhone";
      this.lblCellPhone.Size = new Size(76, 15);
      this.lblCellPhone.TabIndex = 14;
      this.lblCellPhone.Text = "Cell Phone";
      this.txtBoxMobilePhone.Location = new Point(90, 80);
      this.txtBoxMobilePhone.MaxLength = 30;
      this.txtBoxMobilePhone.Name = "txtBoxMobilePhone";
      this.txtBoxMobilePhone.Size = new Size(154, 20);
      this.txtBoxMobilePhone.TabIndex = 14;
      this.txtBoxMobilePhone.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblFaxNumber.Location = new Point(10, 105);
      this.lblFaxNumber.Name = "lblFaxNumber";
      this.lblFaxNumber.Size = new Size(76, 15);
      this.lblFaxNumber.TabIndex = 16;
      this.lblFaxNumber.Text = "Fax Number";
      this.lblWorkEmail.Location = new Point(10, 149);
      this.lblWorkEmail.Name = "lblWorkEmail";
      this.lblWorkEmail.Size = new Size(65, 15);
      this.lblWorkEmail.TabIndex = 20;
      this.lblWorkEmail.Text = "Work Email";
      this.txtBoxFaxNumber.Location = new Point(90, 102);
      this.txtBoxFaxNumber.MaxLength = 30;
      this.txtBoxFaxNumber.Name = "txtBoxFaxNumber";
      this.txtBoxFaxNumber.Size = new Size(154, 20);
      this.txtBoxFaxNumber.TabIndex = 15;
      this.txtBoxFaxNumber.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizEmail.Location = new Point(90, 146);
      this.txtBoxBizEmail.MaxLength = 50;
      this.txtBoxBizEmail.Name = "txtBoxBizEmail";
      this.txtBoxBizEmail.Size = new Size(154, 20);
      this.txtBoxBizEmail.TabIndex = 17;
      this.txtBoxBizEmail.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.picBoxEmailOver.Image = (Image) componentResourceManager.GetObject("picBoxEmailOver.Image");
      this.picBoxEmailOver.Location = new Point(352, 4);
      this.picBoxEmailOver.Name = "picBoxEmailOver";
      this.picBoxEmailOver.Size = new Size(16, 16);
      this.picBoxEmailOver.TabIndex = 30;
      this.picBoxEmailOver.TabStop = false;
      this.picBoxEmailOver.Visible = false;
      this.toolTip.AutomaticDelay = 0;
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizState);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizAddress2);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizAddress1);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizZip);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizCity);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizState);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizAddress1);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizZip);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizAddress2);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizCity);
      this.gcBusinessInfo.Controls.Add((Control) this.lblCompany);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxEmployerName);
      this.gcBusinessInfo.Controls.Add((Control) this.lblTitle);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxJobTitle);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizWebUrl);
      this.gcBusinessInfo.Controls.Add((Control) this.lblWebUrl);
      this.gcBusinessInfo.Dock = DockStyle.Right;
      this.gcBusinessInfo.Location = new Point(675, 1);
      this.gcBusinessInfo.Name = "gcBusinessInfo";
      this.gcBusinessInfo.Size = new Size(334, 248);
      this.gcBusinessInfo.TabIndex = 3;
      this.gcBusinessInfo.Text = "Business Information";
      this.txtBoxBizState.Location = new Point(80, 124);
      this.txtBoxBizState.MaxLength = 2;
      this.txtBoxBizState.Name = "txtBoxBizState";
      this.txtBoxBizState.Size = new Size(28, 20);
      this.txtBoxBizState.TabIndex = 22;
      this.txtBoxBizState.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizAddress2.Location = new Point(80, 80);
      this.txtBoxBizAddress2.MaxLength = 50;
      this.txtBoxBizAddress2.Name = "txtBoxBizAddress2";
      this.txtBoxBizAddress2.Size = new Size(242, 20);
      this.txtBoxBizAddress2.TabIndex = 20;
      this.txtBoxBizAddress2.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizAddress1.Location = new Point(80, 58);
      this.txtBoxBizAddress1.MaxLength = (int) byte.MaxValue;
      this.txtBoxBizAddress1.Name = "txtBoxBizAddress1";
      this.txtBoxBizAddress1.Size = new Size(242, 20);
      this.txtBoxBizAddress1.TabIndex = 19;
      this.txtBoxBizAddress1.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblBizZip.Location = new Point(132, 125);
      this.lblBizZip.Name = "lblBizZip";
      this.lblBizZip.RightToLeft = RightToLeft.No;
      this.lblBizZip.Size = new Size(25, 16);
      this.lblBizZip.TabIndex = 28;
      this.lblBizZip.Text = "Zip";
      this.lblBizCity.Location = new Point(10, 107);
      this.lblBizCity.Name = "lblBizCity";
      this.lblBizCity.RightToLeft = RightToLeft.No;
      this.lblBizCity.Size = new Size(33, 15);
      this.lblBizCity.TabIndex = 24;
      this.lblBizCity.Text = "City";
      this.lblBizState.Location = new Point(10, (int) sbyte.MaxValue);
      this.lblBizState.Name = "lblBizState";
      this.lblBizState.RightToLeft = RightToLeft.No;
      this.lblBizState.Size = new Size(33, 15);
      this.lblBizState.TabIndex = 26;
      this.lblBizState.Text = "State";
      this.lblBizAddress1.Location = new Point(10, 61);
      this.lblBizAddress1.Name = "lblBizAddress1";
      this.lblBizAddress1.Size = new Size(60, 18);
      this.lblBizAddress1.TabIndex = 20;
      this.lblBizAddress1.Text = "Address 1";
      this.txtBoxBizZip.Location = new Point(163, 124);
      this.txtBoxBizZip.MaxLength = 20;
      this.txtBoxBizZip.Name = "txtBoxBizZip";
      this.txtBoxBizZip.Size = new Size(72, 20);
      this.txtBoxBizZip.TabIndex = 23;
      this.txtBoxBizZip.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizZip.KeyDown += new KeyEventHandler(this.txtBoxBizZip_KeyDown);
      this.txtBoxBizZip.Leave += new EventHandler(this.txtBoxBizZip_Leave);
      this.lblBizAddress2.Location = new Point(10, 83);
      this.lblBizAddress2.Name = "lblBizAddress2";
      this.lblBizAddress2.Size = new Size(60, 19);
      this.lblBizAddress2.TabIndex = 22;
      this.lblBizAddress2.Text = "Address 2";
      this.txtBoxBizCity.Location = new Point(80, 102);
      this.txtBoxBizCity.MaxLength = 50;
      this.txtBoxBizCity.Name = "txtBoxBizCity";
      this.txtBoxBizCity.Size = new Size(242, 20);
      this.txtBoxBizCity.TabIndex = 21;
      this.txtBoxBizCity.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblCompany.Location = new Point(10, 39);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(55, 16);
      this.lblCompany.TabIndex = 16;
      this.lblCompany.Text = "Company";
      this.txtBoxEmployerName.Location = new Point(80, 36);
      this.txtBoxEmployerName.MaxLength = 50;
      this.txtBoxEmployerName.Name = "txtBoxEmployerName";
      this.txtBoxEmployerName.Size = new Size(242, 20);
      this.txtBoxEmployerName.TabIndex = 18;
      this.txtBoxEmployerName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblTitle.Location = new Point(9, 171);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(55, 16);
      this.lblTitle.TabIndex = 18;
      this.lblTitle.Text = "Job Title";
      this.txtBoxJobTitle.Location = new Point(80, 168);
      this.txtBoxJobTitle.MaxLength = 50;
      this.txtBoxJobTitle.Name = "txtBoxJobTitle";
      this.txtBoxJobTitle.Size = new Size(242, 20);
      this.txtBoxJobTitle.TabIndex = 25;
      this.txtBoxJobTitle.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizWebUrl.Location = new Point(80, 146);
      this.txtBoxBizWebUrl.MaxLength = 50;
      this.txtBoxBizWebUrl.Name = "txtBoxBizWebUrl";
      this.txtBoxBizWebUrl.Size = new Size(242, 20);
      this.txtBoxBizWebUrl.TabIndex = 24;
      this.txtBoxBizWebUrl.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizWebUrl.Leave += new EventHandler(this.txtBoxBizWebUrl_Leave);
      this.lblWebUrl.Location = new Point(10, 149);
      this.lblWebUrl.Name = "lblWebUrl";
      this.lblWebUrl.Size = new Size(55, 16);
      this.lblWebUrl.TabIndex = 30;
      this.lblWebUrl.Text = "Web URL";
      this.gcPersonalInfo.Controls.Add((Control) this.lblHomeZip);
      this.gcPersonalInfo.Controls.Add((Control) this.txtSuffixName);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxHomeState);
      this.gcPersonalInfo.Controls.Add((Control) this.label3);
      this.gcPersonalInfo.Controls.Add((Control) this.lblFirstName);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxHomeAddress2);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxFirstName);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxHomeAddress1);
      this.gcPersonalInfo.Controls.Add((Control) this.txtMiddleName);
      this.gcPersonalInfo.Controls.Add((Control) this.lblLastName);
      this.gcPersonalInfo.Controls.Add((Control) this.lblHomeCity);
      this.gcPersonalInfo.Controls.Add((Control) this.lblSSN);
      this.gcPersonalInfo.Controls.Add((Control) this.lblHomeState);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxSSN);
      this.gcPersonalInfo.Controls.Add((Control) this.lblHomeAddress1);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxHomeZip);
      this.gcPersonalInfo.Controls.Add((Control) this.label2);
      this.gcPersonalInfo.Controls.Add((Control) this.lblHomeAddress2);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxLastName);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxHomeCity);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxSalutation);
      this.gcPersonalInfo.Controls.Add((Control) this.label1);
      this.gcPersonalInfo.Dock = DockStyle.Left;
      this.gcPersonalInfo.Location = new Point(1, 1);
      this.gcPersonalInfo.Name = "gcPersonalInfo";
      this.gcPersonalInfo.Size = new Size(334, 248);
      this.gcPersonalInfo.TabIndex = 2;
      this.gcPersonalInfo.Text = "Personal Information";
      this.lblHomeZip.Location = new Point(138, 214);
      this.lblHomeZip.Name = "lblHomeZip";
      this.lblHomeZip.RightToLeft = RightToLeft.No;
      this.lblHomeZip.Size = new Size(24, 17);
      this.lblHomeZip.TabIndex = 8;
      this.lblHomeZip.Text = "Zip";
      this.txtSuffixName.Location = new Point(228, 80);
      this.txtSuffixName.MaxLength = 50;
      this.txtSuffixName.Name = "txtSuffixName";
      this.txtSuffixName.Size = new Size(88, 20);
      this.txtSuffixName.TabIndex = 4;
      this.txtSuffixName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxHomeState.Location = new Point(80, 212);
      this.txtBoxHomeState.MaxLength = 2;
      this.txtBoxHomeState.Name = "txtBoxHomeState";
      this.txtBoxHomeState.Size = new Size(28, 20);
      this.txtBoxHomeState.TabIndex = 10;
      this.txtBoxHomeState.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxHomeState.Leave += new EventHandler(this.txtBoxState_Leave);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(190, 83);
      this.label3.Name = "label3";
      this.label3.Size = new Size(36, 14);
      this.label3.TabIndex = 34;
      this.label3.Text = "Suffix";
      this.lblFirstName.Location = new Point(10, 38);
      this.lblFirstName.Name = "lblFirstName";
      this.lblFirstName.Size = new Size(60, 15);
      this.lblFirstName.TabIndex = 2;
      this.lblFirstName.Text = "First Name";
      this.txtBoxHomeAddress2.Location = new Point(80, 168);
      this.txtBoxHomeAddress2.MaxLength = 50;
      this.txtBoxHomeAddress2.Name = "txtBoxHomeAddress2";
      this.txtBoxHomeAddress2.Size = new Size(236, 20);
      this.txtBoxHomeAddress2.TabIndex = 8;
      this.txtBoxHomeAddress2.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxFirstName.Location = new Point(80, 36);
      this.txtBoxFirstName.MaxLength = 50;
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.Size = new Size(236, 20);
      this.txtBoxFirstName.TabIndex = 1;
      this.txtBoxFirstName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxHomeAddress1.Location = new Point(80, 146);
      this.txtBoxHomeAddress1.MaxLength = (int) byte.MaxValue;
      this.txtBoxHomeAddress1.Name = "txtBoxHomeAddress1";
      this.txtBoxHomeAddress1.Size = new Size(236, 20);
      this.txtBoxHomeAddress1.TabIndex = 7;
      this.txtBoxHomeAddress1.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtMiddleName.Location = new Point(80, 58);
      this.txtMiddleName.MaxLength = 50;
      this.txtMiddleName.Name = "txtMiddleName";
      this.txtMiddleName.Size = new Size(236, 20);
      this.txtMiddleName.TabIndex = 2;
      this.txtMiddleName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblLastName.Location = new Point(10, 82);
      this.lblLastName.Name = "lblLastName";
      this.lblLastName.Size = new Size(60, 15);
      this.lblLastName.TabIndex = 4;
      this.lblLastName.Text = "Last Name";
      this.lblHomeCity.Location = new Point(10, 193);
      this.lblHomeCity.Name = "lblHomeCity";
      this.lblHomeCity.RightToLeft = RightToLeft.No;
      this.lblHomeCity.Size = new Size(33, 19);
      this.lblHomeCity.TabIndex = 4;
      this.lblHomeCity.Text = "City";
      this.lblSSN.Location = new Point(10, (int) sbyte.MaxValue);
      this.lblSSN.Name = "lblSSN";
      this.lblSSN.Size = new Size(29, 16);
      this.lblSSN.TabIndex = 8;
      this.lblSSN.Text = "SSN";
      this.lblHomeState.Location = new Point(10, 216);
      this.lblHomeState.Name = "lblHomeState";
      this.lblHomeState.RightToLeft = RightToLeft.No;
      this.lblHomeState.Size = new Size(33, 15);
      this.lblHomeState.TabIndex = 6;
      this.lblHomeState.Text = "State";
      this.txtBoxSSN.Location = new Point(80, 124);
      this.txtBoxSSN.MaxLength = 12;
      this.txtBoxSSN.Name = "txtBoxSSN";
      this.txtBoxSSN.Size = new Size(106, 20);
      this.txtBoxSSN.TabIndex = 6;
      this.txtBoxSSN.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblHomeAddress1.Location = new Point(10, 149);
      this.lblHomeAddress1.Name = "lblHomeAddress1";
      this.lblHomeAddress1.Size = new Size(60, 18);
      this.lblHomeAddress1.TabIndex = 0;
      this.lblHomeAddress1.Text = "Address 1";
      this.txtBoxHomeZip.Location = new Point(164, 212);
      this.txtBoxHomeZip.MaxLength = 20;
      this.txtBoxHomeZip.Name = "txtBoxHomeZip";
      this.txtBoxHomeZip.Size = new Size(105, 20);
      this.txtBoxHomeZip.TabIndex = 11;
      this.txtBoxHomeZip.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxHomeZip.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxHomeZip.Leave += new EventHandler(this.txtBoxHomeZip_Leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 61);
      this.label2.Name = "label2";
      this.label2.Size = new Size(37, 14);
      this.label2.TabIndex = 32;
      this.label2.Text = "Middle";
      this.lblHomeAddress2.Location = new Point(10, 171);
      this.lblHomeAddress2.Name = "lblHomeAddress2";
      this.lblHomeAddress2.Size = new Size(60, 19);
      this.lblHomeAddress2.TabIndex = 2;
      this.lblHomeAddress2.Text = "Address 2";
      this.txtBoxLastName.Location = new Point(80, 80);
      this.txtBoxLastName.MaxLength = 50;
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.Size = new Size(106, 20);
      this.txtBoxLastName.TabIndex = 3;
      this.txtBoxLastName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxHomeCity.Location = new Point(80, 190);
      this.txtBoxHomeCity.MaxLength = 50;
      this.txtBoxHomeCity.Name = "txtBoxHomeCity";
      this.txtBoxHomeCity.Size = new Size(236, 20);
      this.txtBoxHomeCity.TabIndex = 9;
      this.txtBoxHomeCity.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxSalutation.Location = new Point(80, 102);
      this.txtBoxSalutation.MaxLength = 84;
      this.txtBoxSalutation.Name = "txtBoxSalutation";
      this.txtBoxSalutation.Size = new Size(236, 20);
      this.txtBoxSalutation.TabIndex = 5;
      this.txtBoxSalutation.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.label1.Location = new Point(10, 104);
      this.label1.Name = "label1";
      this.label1.Size = new Size(56, 16);
      this.label1.TabIndex = 6;
      this.label1.Text = "Salutation";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = Color.White;
      this.ClientSize = new Size(1010, 250);
      this.Controls.Add((Control) this.pnlContactInfo);
      this.Controls.Add((Control) this.gcBusinessInfo);
      this.Controls.Add((Control) this.gcPersonalInfo);
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorrowerInfo1Form);
      this.Padding = new Padding(1);
      this.Text = "BorrowerInfoForm";
      this.SizeChanged += new EventHandler(this.BorrowerInfoForm_SizeChanged);
      this.pnlContactInfo.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.siBtnWorkEmail).EndInit();
      ((ISupportInitialize) this.siBtnHomeEmail).EndInit();
      ((ISupportInitialize) this.siBtnFaxNumber).EndInit();
      ((ISupportInitialize) this.siBtnCellPhone).EndInit();
      ((ISupportInitialize) this.siBtnWorkPhone).EndInit();
      ((ISupportInitialize) this.siBtnHomePhone).EndInit();
      ((ISupportInitialize) this.picBoxEmailOver).EndInit();
      this.gcBusinessInfo.ResumeLayout(false);
      this.gcBusinessInfo.PerformLayout();
      this.gcPersonalInfo.ResumeLayout(false);
      this.gcPersonalInfo.PerformLayout();
      this.ResumeLayout(false);
    }

    public bool IsReadOnly
    {
      get => this.isReadOnly;
      set => this.isReadOnly = value;
    }

    public bool isDirty() => this.changed;

    public int CurrentContactID
    {
      get => this.contactInfo != null ? this.contactInfo.ContactID : -1;
      set
      {
        if (this.CurrentContactID == value)
          return;
        this.contactInfo = (BorrowerInfo) null;
        this.disableControls();
        if (value < 0)
          return;
        this.contactInfo = Session.ContactManager.GetBorrower(value);
        if (this.contactInfo == null)
          throw new ObjectNotFoundException("Unable to retrieve borrower contact.", ObjectType.Contact, (object) value);
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
        this.disableControls();
        if (value == null)
          return;
        this.contactInfo = (BorrowerInfo) value;
        this.loadForm();
      }
    }

    public bool SaveChanges()
    {
      if (!this.changed || this.contactInfo == null)
        return false;
      if (this.txtBoxPersonalEmail.Text.Trim() != string.Empty && !Utils.ValidateEmail(this.txtBoxPersonalEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid email address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxPersonalEmail.Focus();
        return false;
      }
      if (this.txtBoxBizEmail.Text.Trim() != string.Empty && !Utils.ValidateEmail(this.txtBoxBizEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid email address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxBizEmail.Focus();
        return false;
      }
      this.contactInfo = Session.ContactManager.GetBorrower(this.CurrentContactID);
      this.contactInfo.FirstName = this.txtBoxFirstName.Text;
      this.contactInfo.MiddleName = this.txtMiddleName.Text;
      this.contactInfo.LastName = this.txtBoxLastName.Text;
      this.contactInfo.SuffixName = this.txtSuffixName.Text;
      this.contactInfo.Salutation = this.txtBoxSalutation.Text;
      this.contactInfo.SSN = this.txtBoxSSN.Text;
      this.contactInfo.HomeAddress.State = this.txtBoxHomeState.Text;
      this.contactInfo.HomeAddress.Street2 = this.txtBoxHomeAddress2.Text;
      this.contactInfo.HomeAddress.Street1 = this.txtBoxHomeAddress1.Text;
      this.contactInfo.HomeAddress.Zip = this.txtBoxHomeZip.Text;
      this.contactInfo.HomeAddress.City = this.txtBoxHomeCity.Text;
      this.contactInfo.HomePhone = this.txtBoxHomePhone.Text;
      this.contactInfo.WorkPhone = this.txtBoxWorkPhone.Text;
      this.contactInfo.MobilePhone = this.txtBoxMobilePhone.Text;
      this.contactInfo.FaxNumber = this.txtBoxFaxNumber.Text;
      this.contactInfo.PersonalEmail = this.txtBoxPersonalEmail.Text;
      this.contactInfo.BizEmail = this.txtBoxBizEmail.Text;
      this.contactInfo.EmployerName = this.txtBoxEmployerName.Text;
      this.contactInfo.BizAddress.Street2 = this.txtBoxBizAddress2.Text;
      this.contactInfo.BizAddress.Street1 = this.txtBoxBizAddress1.Text;
      this.contactInfo.BizAddress.State = this.txtBoxBizState.Text;
      this.contactInfo.BizAddress.Zip = this.txtBoxBizZip.Text;
      this.contactInfo.BizAddress.City = this.txtBoxBizCity.Text;
      this.contactInfo.BizWebUrl = this.txtBoxBizWebUrl.Text;
      this.contactInfo.JobTitle = this.txtBoxJobTitle.Text;
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

    public void SetFocusOnDefaultField() => this.txtBoxFirstName.Focus();

    public void disableForm() => this.disableControlsOnly();

    private void disableControlsOnly()
    {
      this.txtBoxFirstName.ReadOnly = true;
      this.txtBoxLastName.ReadOnly = true;
      this.txtMiddleName.ReadOnly = true;
      this.txtSuffixName.ReadOnly = true;
      this.txtBoxSalutation.ReadOnly = true;
      this.txtBoxSSN.ReadOnly = true;
      this.txtBoxHomeState.ReadOnly = true;
      this.txtBoxHomeAddress2.ReadOnly = true;
      this.txtBoxHomeAddress1.ReadOnly = true;
      this.txtBoxHomeZip.ReadOnly = true;
      this.txtBoxHomeCity.ReadOnly = true;
      this.txtBoxFaxNumber.ReadOnly = true;
      this.siBtnFaxNumber.Enabled = false;
      this.txtBoxHomePhone.ReadOnly = true;
      this.siBtnHomePhone.Enabled = false;
      this.txtBoxMobilePhone.ReadOnly = true;
      this.siBtnCellPhone.Enabled = false;
      this.txtBoxPersonalEmail.ReadOnly = true;
      this.siBtnHomeEmail.Enabled = false;
      this.txtBoxBizEmail.ReadOnly = true;
      this.siBtnWorkEmail.Enabled = false;
      this.txtBoxWorkPhone.ReadOnly = true;
      this.siBtnWorkPhone.Enabled = false;
      this.txtBoxEmployerName.ReadOnly = true;
      this.txtBoxBizAddress2.ReadOnly = true;
      this.txtBoxBizAddress1.ReadOnly = true;
      this.txtBoxBizState.ReadOnly = true;
      this.txtBoxBizZip.ReadOnly = true;
      this.txtBoxBizCity.ReadOnly = true;
      this.txtBoxBizWebUrl.ReadOnly = true;
      this.txtBoxJobTitle.ReadOnly = true;
    }

    private void disableControls()
    {
      this.clearForm();
      this.disableControlsOnly();
    }

    private void enableControls()
    {
      this.txtBoxSSN.ReadOnly = false;
      this.txtBoxLastName.ReadOnly = false;
      this.txtBoxSalutation.ReadOnly = false;
      this.txtBoxFirstName.ReadOnly = false;
      this.txtMiddleName.ReadOnly = false;
      this.txtSuffixName.ReadOnly = false;
      this.txtBoxHomeState.ReadOnly = false;
      this.txtBoxHomeAddress2.ReadOnly = false;
      this.txtBoxHomeAddress1.ReadOnly = false;
      this.txtBoxHomeZip.ReadOnly = false;
      this.txtBoxHomeCity.ReadOnly = false;
      this.txtBoxFaxNumber.ReadOnly = false;
      this.txtBoxHomePhone.ReadOnly = false;
      this.txtBoxMobilePhone.ReadOnly = false;
      this.txtBoxPersonalEmail.ReadOnly = false;
      this.txtBoxBizEmail.ReadOnly = false;
      this.txtBoxWorkPhone.ReadOnly = false;
      this.txtBoxEmployerName.ReadOnly = false;
      this.txtBoxBizAddress2.ReadOnly = false;
      this.txtBoxBizAddress1.ReadOnly = false;
      this.txtBoxBizState.ReadOnly = false;
      this.txtBoxBizZip.ReadOnly = false;
      this.txtBoxBizCity.ReadOnly = false;
      this.txtBoxBizWebUrl.ReadOnly = false;
      this.txtBoxJobTitle.ReadOnly = false;
    }

    private void loadForm()
    {
      this.initialLoad = true;
      if (!this.IsReadOnly)
        this.enableControls();
      this.txtMiddleName.Text = this.contactInfo.MiddleName;
      this.txtSuffixName.Text = this.contactInfo.SuffixName;
      this.txtBoxSSN.Text = this.contactInfo.SSN;
      this.txtBoxLastName.Text = this.contactInfo.LastName;
      this.txtBoxSalutation.Text = this.contactInfo.Salutation;
      this.txtBoxFirstName.Text = this.contactInfo.FirstName;
      this.txtBoxHomeState.Text = this.contactInfo.HomeAddress.State;
      this.txtBoxHomeAddress2.Text = this.contactInfo.HomeAddress.Street2;
      this.txtBoxHomeAddress1.Text = this.contactInfo.HomeAddress.Street1;
      this.txtBoxHomeZip.Text = this.contactInfo.HomeAddress.Zip;
      this.txtBoxHomeCity.Text = this.contactInfo.HomeAddress.City;
      this.txtBoxFaxNumber.Text = this.contactInfo.FaxNumber;
      this.txtBoxHomePhone.Text = this.contactInfo.HomePhone;
      this.txtBoxMobilePhone.Text = this.contactInfo.MobilePhone;
      this.txtBoxPersonalEmail.Text = this.contactInfo.PersonalEmail;
      this.txtBoxBizEmail.Text = this.contactInfo.BizEmail;
      this.txtBoxWorkPhone.Text = this.contactInfo.WorkPhone;
      this.txtBoxEmployerName.Text = this.contactInfo.EmployerName;
      this.txtBoxBizAddress2.Text = this.contactInfo.BizAddress.Street2;
      this.txtBoxBizAddress1.Text = this.contactInfo.BizAddress.Street1;
      this.txtBoxBizState.Text = this.contactInfo.BizAddress.State;
      this.txtBoxBizZip.Text = this.contactInfo.BizAddress.Zip;
      this.txtBoxBizCity.Text = this.contactInfo.BizAddress.City;
      this.txtBoxBizWebUrl.Text = this.contactInfo.BizWebUrl;
      this.txtBoxJobTitle.Text = this.contactInfo.JobTitle;
      this.siBtnFaxNumber.Enabled = true;
      this.siBtnHomePhone.Enabled = true;
      this.siBtnCellPhone.Enabled = true;
      this.siBtnHomeEmail.Enabled = true;
      this.siBtnWorkEmail.Enabled = true;
      this.siBtnWorkPhone.Enabled = true;
      this.changed = false;
      this.initialLoad = false;
    }

    private void clearForm()
    {
      this.txtBoxSSN.Text = "";
      this.txtBoxLastName.Text = "";
      this.txtBoxSalutation.Text = "";
      this.txtBoxFirstName.Text = "";
      this.txtMiddleName.Text = "";
      this.txtSuffixName.Text = "";
      this.txtBoxHomeState.Text = "";
      this.txtBoxHomeAddress2.Text = "";
      this.txtBoxHomeAddress1.Text = "";
      this.txtBoxHomeZip.Text = "";
      this.txtBoxHomeCity.Text = "";
      this.txtBoxFaxNumber.Text = "";
      this.txtBoxHomePhone.Text = "";
      this.txtBoxMobilePhone.Text = "";
      this.txtBoxPersonalEmail.Text = "";
      this.txtBoxBizEmail.Text = "";
      this.txtBoxWorkPhone.Text = "";
      this.txtBoxEmployerName.Text = "";
      this.txtBoxBizAddress2.Text = "";
      this.txtBoxBizAddress1.Text = "";
      this.txtBoxBizState.Text = "";
      this.txtBoxBizZip.Text = "";
      this.txtBoxBizCity.Text = "";
      this.txtBoxBizWebUrl.Text = "";
      this.txtBoxJobTitle.Text = "";
      this.changed = false;
    }

    private void BorrowerInfoForm_SizeChanged(object sender, EventArgs e)
    {
      this.gcPersonalInfo.Size = this.gcBusinessInfo.Size = this.Width <= 1010 ? new Size(334, this.Height) : new Size((this.Width - 8) / 3, this.Height);
    }

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
        FieldFormat dataFormat;
        if (sender == this.txtBoxHomePhone || sender == this.txtBoxFaxNumber || sender == this.txtBoxWorkPhone || sender == this.txtBoxMobilePhone)
          dataFormat = FieldFormat.PHONE;
        else if (sender == this.txtBoxHomeZip || sender == this.txtBoxBizZip)
          dataFormat = FieldFormat.ZIPCODE;
        else if (sender == this.txtBoxSSN)
        {
          dataFormat = FieldFormat.SSN;
        }
        else
        {
          if (sender != this.txtBoxHomeState && sender != this.txtBoxBizState)
            return;
          dataFormat = FieldFormat.STATE;
        }
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        int newCursorPos = 0;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate, textBox.SelectionStart, ref newCursorPos);
        if (!needsUpdate)
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

    private void txtBoxHomeZip_Leave(object sender, EventArgs e)
    {
      if (this.txtBoxHomeZip.Text == "")
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(this.txtBoxHomeZip.Text, ZipCodeUtils.GetMultipleZipInfoAt(this.txtBoxHomeZip.Text));
      if (zipCodeInfo == null)
        return;
      this.txtBoxHomeCity.Text = zipCodeInfo.City;
      this.txtBoxHomeState.Text = zipCodeInfo.State;
    }

    private void txtBoxState_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      textBox.Text = textBox.Text.ToUpper();
    }

    private void txtBoxKeyDown(object sender, KeyEventArgs e)
    {
      if (this.initialLoad || e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void picBoxHomeEmail_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      SystemUtil.ShellExecute("mailto:" + this.contactInfo.PersonalEmail);
      ContactUtils.addEmailHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.Borrower, newNote);
    }

    private void picBoxWorkEmail_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      SystemUtil.ShellExecute("mailto:" + this.contactInfo.BizEmail);
      ContactUtils.addEmailHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.Borrower, newNote);
    }

    private void picBoxHomePhone_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addCallHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.Borrower, newNote);
    }

    private void picBoxWorkPhone_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addCallHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.Borrower, newNote);
    }

    private void picBoxCellPhone_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addCallHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.Borrower, newNote);
    }

    private void picBoxFaxNumber_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addFaxHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.Borrower, newNote);
    }

    private void preServiceCheck(string serviceArea)
    {
      if (this.tabForm.isDirty() && MessageBox.Show("Your changes to the selected Contact will be saved before proceeding.", serviceArea, MessageBoxButtons.OK, MessageBoxIcon.Asterisk) == DialogResult.OK)
        this.tabForm.ForceSave(false);
      if (this.CurrentContactID != -1)
        return;
      this.CurrentContactID = this.tabForm.CurrentContactID;
    }

    public void btnOriginateLoan_Click(object sender, EventArgs e)
    {
      this.preServiceCheck("Originate Loan");
      BorrowerInfo borrower = Session.ContactManager.GetBorrower(this.CurrentContactID);
      if (borrower == null)
      {
        int num = (int) MessageBox.Show("The selected Contact no longer exists.", "Originate Loan from Contact");
      }
      else
      {
        if (!this.validateLOLicenseForOpportunity(borrower) || !this.promptToSaveCurrentLoan())
          return;
        LoanIdentity loanId = (LoanIdentity) null;
        if (!this.findMatchingLoan(borrower, ref loanId))
          return;
        if (loanId != (LoanIdentity) null)
        {
          Session.Application.GetService<ILoanConsole>().OpenLoan(loanId.Guid);
        }
        else
        {
          LoanDataMgr newLoanForContact = this.createNewLoanForContact(borrower, "Originate Loan");
          if (newLoanForContact == null)
            return;
          newLoanForContact.LoanData.UnsetCreatedWithoutLoanNumber();
          this.openLoanInEditor(newLoanForContact, true);
        }
      }
    }

    private bool findMatchingLoan(BorrowerInfo contact, ref LoanIdentity loanId)
    {
      string str = contact.LastName + " " + contact.SuffixName;
      if (str.Trim() == "")
        return true;
      QueryCriterion filter = new StringValueCriterion("Loan.BorrowerLastName", str.Trim()).Or((QueryCriterion) new StringValueCriterion("borrower.ssn", contact.SSN, StringMatchType.Exact));
      QueryCriterion[] queryCriterionArray = new QueryCriterion[1]
      {
        filter
      };
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(LoanInfo.Right.Access, MatchedLoansForm.RequiredFields, PipelineData.Fields, (SortField[]) null, filter, false);
      if (pipeline.Length != 0)
      {
        MatchedLoansForm matchedLoansForm = new MatchedLoansForm(pipeline);
        switch (matchedLoansForm.ShowDialog((IWin32Window) this))
        {
          case DialogResult.Cancel:
            return false;
          case DialogResult.Yes:
            loanId = matchedLoansForm.LoanID;
            break;
        }
      }
      return true;
    }

    public void btnOrderCredit_Click(object sender, EventArgs e)
    {
      this.preServiceCheck("Order Credit");
      BorrowerInfo borrower = Session.ContactManager.GetBorrower(this.CurrentContactID);
      if (borrower == null)
      {
        int num1 = (int) MessageBox.Show("The selected Contact no longer exists.", "Order Credit for Contact");
      }
      else
      {
        if (!this.checkRequiredFieldsForOrderCredit(borrower) || !this.promptToSaveCurrentLoan())
          return;
        BorrowerInfo coborrower = (BorrowerInfo) null;
        LoanDataMgr newLoanForContact = this.createNewLoanForContact(borrower, out coborrower, "Order Credit");
        if (newLoanForContact == null)
          return;
        this.openLoanInEditor(newLoanForContact, false);
        Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;Credit+Report");
        if (Session.LoanData.GetLogList().GetAllePASSDocuments().Length == 1)
        {
          if (Session.LoanData.LoanNumber == "" && Session.LoanManager.IsTimeToSetLoanNumber(Session.LoanData))
            Session.LoanData.LoanNumber = Session.LoanManager.GetNextLoanNumber();
          Session.LoanData.MersNumber = Session.LoanDataMgr.GetNextMersNumber();
          Session.LoanData.UnsetCreatedWithoutLoanNumber();
          if (!Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) new StandardMilestoneTemplateApply(Session.DefaultInstance, false, true, ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_DisplayMilestoneChangeScreen)), false))
            return;
          IMainScreen service = Session.Application.GetService<IMainScreen>();
          if (service != null)
            service.NavigateToContact(CategoryType.Borrower);
          else
            Session.MainScreen.NavigateToContact(CategoryType.Borrower);
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Encompass generated a loan file to hold the credit report you just ordered. The loan file is currently open for your convenience to view the credit report.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          try
          {
            ContactCreditScores[] creditReport = new CreditReportParser().parseCreditReport();
            ContactUtils.addCreditOrderingHistory(borrower.ContactID, creditReport);
            if (coborrower == null)
              return;
            ContactUtils.addCreditOrderingHistory(coborrower.ContactID, creditReport);
          }
          catch
          {
          }
        }
        else
        {
          string guid = Session.LoanData.GUID;
          this.closeLoanEditor(false);
          Session.LoanManager.DeleteLoan(guid, false, true);
          Session.Application.GetService<IEncompassApplication>().SetCurrentActivity(EncompassActivity.BorrowerContacts);
        }
      }
    }

    private bool promptToSaveCurrentLoan()
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (!service.HasOpenLoan)
        return true;
      using (CloseLoanQueryForm closeLoanQueryForm = new CloseLoanQueryForm())
        return closeLoanQueryForm.ShowDialog() != DialogResult.Cancel && service.CloseLoanWithoutPrompts(closeLoanQueryForm.BoolSaveLoan);
    }

    private LoanDataMgr createNewLoanForContact(BorrowerInfo contact, string purpose)
    {
      BorrowerInfo coborrower = (BorrowerInfo) null;
      return this.createNewLoanForContact(contact, out coborrower, purpose);
    }

    private static void addLoanOriginationHistory(BorrowerInfo contact, BorrowerInfo coborrower)
    {
      Opportunity opportunityByBorrowerId = Session.ContactManager.GetOpportunityByBorrowerId(contact.ContactID);
      ContactLoanInfo info = new ContactLoanInfo();
      info.BorrowerID = contact.ContactID;
      if (opportunityByBorrowerId != null)
      {
        info.AppraisedValue = opportunityByBorrowerId.PropertyValue;
        info.LoanAmount = opportunityByBorrowerId.LoanAmount;
        info.InterestRate = opportunityByBorrowerId.MortgageRate;
        info.Term = opportunityByBorrowerId.Term;
        info.Purpose = opportunityByBorrowerId.Purpose;
        info.DownPayment = opportunityByBorrowerId.DownPayment;
        info.Amortization = opportunityByBorrowerId.Amortization;
      }
      else
      {
        info.AppraisedValue = 0M;
        info.LoanAmount = 0M;
        info.InterestRate = 0M;
        info.Term = 0;
        info.Purpose = EllieMae.EMLite.Common.Contact.LoanPurpose.Blank;
        info.DownPayment = 0M;
        info.Amortization = AmortizationType.Blank;
      }
      info.LTV = 0M;
      info.LoanType = LoanTypeEnum.Blank;
      info.LienPosition = LienEnum.Blank;
      info.LoanStatus = "Started";
      info.DateCompleted = DateTime.Now;
      int loanId = Session.ContactManager.AddContactLoan(info);
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem(-1, "Loan Origination", info.DateCompleted, loanId, string.Empty, Session.UserID, string.Empty);
      Session.ContactManager.AddHistoryItemForContact(contact.ContactID, ContactType.Borrower, contactHistoryItem);
      if (coborrower == null)
        return;
      Session.ContactManager.AddHistoryItemForContact(coborrower.ContactID, ContactType.Borrower, contactHistoryItem);
    }

    private LoanDataMgr createNewLoanForContact(
      BorrowerInfo contact,
      out BorrowerInfo coborrower,
      string purpose)
    {
      LoanTemplateSelection loanTemplate = (LoanTemplateSelection) null;
      string loanFolder = (string) null;
      coborrower = (BorrowerInfo) null;
      using (LoanOriginationDlg loanOriginationDlg = new LoanOriginationDlg(contact.ContactID, contact.FirstName + " " + contact.LastName, purpose))
      {
        if (DialogResult.Cancel == loanOriginationDlg.ShowDialog((IWin32Window) this))
          return (LoanDataMgr) null;
        loanTemplate = loanOriginationDlg.SelectedTemplate;
        loanFolder = loanOriginationDlg.LoanFolder;
        coborrower = loanOriginationDlg.CoBorrower;
      }
      LoanDataMgr loan = this.createLoan(loanFolder, loanTemplate);
      if (!new ContactUtil(Session.SessionObjects).SetContactFieldsInLoan(loan.LoanData, contact, coborrower))
        return (LoanDataMgr) null;
      BorrowerPair borrowerPair = loan.LoanData.GetBorrowerPairs()[0];
      string id = borrowerPair.Borrower.Id;
      string str = "";
      if (borrowerPair.CoBorrower != null)
        str = borrowerPair.CoBorrower.Id;
      LogList logList1 = loan.LoanData.GetLogList();
      string mappingID1 = id;
      Guid contactGuid1 = contact.ContactGuid;
      string contactGuid2 = contactGuid1.ToString();
      logList1.AddCRMMapping(mappingID1, CRMLogType.BorrowerContact, contactGuid2, CRMRoleType.Borrower);
      if (coborrower != null)
      {
        LogList logList2 = loan.LoanData.GetLogList();
        string mappingID2 = str;
        contactGuid1 = coborrower.ContactGuid;
        string contactGuid3 = contactGuid1.ToString();
        logList2.AddCRMMapping(mappingID2, CRMLogType.BorrowerContact, contactGuid3, CRMRoleType.Coborrower);
      }
      BorrowerInfo1Form.addLoanOriginationHistory(contact, coborrower);
      return loan;
    }

    private LoanDataMgr createLoan(string loanFolder, LoanTemplateSelection loanTemplate)
    {
      Cursor.Current = Cursors.WaitCursor;
      LoanDataMgr loan = LoanDataMgr.NewLoan(Session.SessionObjects, loanTemplate, loanFolder, (string) null);
      loan.VerifyAssignFirstMilestoneRole();
      loan.LoanData.SetCreatedWithoutLoanNumber();
      Cursor.Current = Cursors.Default;
      return loan;
    }

    private bool checkStatePermission(string stateName)
    {
      if (stateName == null || stateName.Length != 2)
        return true;
      bool flag = false;
      LOLicenseInfo loLicense = Session.OrganizationManager.GetLOLicense(Session.UserID, stateName);
      if (loLicense != null && loLicense.Enabled)
      {
        DateTime dateTime = DateTime.Today;
        dateTime = dateTime.Date;
        if (dateTime.CompareTo(loLicense.ExpirationDate) <= 0)
          flag = true;
      }
      return flag;
    }

    private bool validateLOLicenseForOpportunity(BorrowerInfo contact)
    {
      Opportunity opportunityByBorrowerId = Session.ContactManager.GetOpportunityByBorrowerId(contact.ContactID);
      if (opportunityByBorrowerId == null || this.checkStatePermission(opportunityByBorrowerId.PropertyAddress.State))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) null, "The property is in a state in which you are not licensed to originate loans or the license expiration is expired. Contact your system administrator for more details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private bool checkRequiredFieldsForOrderCredit(BorrowerInfo contact)
    {
      StringBuilder stringBuilder = new StringBuilder("Please fill in the following fields required for ordering credit report on the borrower contact and try again.\n\n");
      bool flag = false;
      if (contact.FirstName.Trim() == string.Empty)
      {
        stringBuilder.Append("Contact First Name\n");
        flag = true;
      }
      if (contact.LastName.Trim() == string.Empty)
      {
        stringBuilder.Append("Contact Last Name\n");
        flag = true;
      }
      if (contact.SSN.Trim() == string.Empty)
      {
        stringBuilder.Append("Contact Social Security Number\n");
        flag = true;
      }
      if (contact.HomeAddress.Street1.Trim() == string.Empty)
      {
        stringBuilder.Append("Contact Address Address1\n");
        flag = true;
      }
      if (contact.HomeAddress.City.Trim() == string.Empty)
      {
        stringBuilder.Append("Contact Address City\n");
        flag = true;
      }
      if (contact.HomeAddress.State.Trim() == string.Empty)
      {
        stringBuilder.Append("Contact Address State\n");
        flag = true;
      }
      if (contact.HomeAddress.Zip.Trim() == string.Empty)
      {
        stringBuilder.Append("Contact Address Zipcode\n");
        flag = true;
      }
      if (!flag)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private FileSystemEntry getCurrentDefaultTemplate()
    {
      string privateProfileString = Session.GetPrivateProfileString("LoanTemplate", "Default");
      try
      {
        FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(privateProfileString);
        return !((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Personal_LoanTemplateSets, Session.UserInfo) && fileSystemEntry.ToDisplayString().IndexOf("Personal:") > -1 ? (FileSystemEntry) null : fileSystemEntry;
      }
      catch
      {
        return (FileSystemEntry) null;
      }
    }

    private void horzSplitter_Paint(object sender, PaintEventArgs e)
    {
      Session.Application.GetService<IEncompassApplication>().GetTipControl().Refresh();
    }

    private void picBoxEmail_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BorrowerInfo1Form)).GetObject("picBoxEmailOver.Image");
    }

    private void picBoxEmail_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BorrowerInfo1Form)).GetObject("picBoxHomeEmail.Image");
    }

    private void picBoxPhone_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BorrowerInfo1Form)).GetObject("picBoxPhoneOver.Image");
    }

    private void picBoxPhone_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BorrowerInfo1Form)).GetObject("picBoxHomePhone.Image");
    }

    public void btnPricing_Click(object sender, EventArgs e)
    {
      this.preServiceCheck("Product and Pricing");
      BorrowerInfo borrower = Session.ContactManager.GetBorrower(this.CurrentContactID);
      if (borrower == null)
      {
        int num1 = (int) MessageBox.Show("The selected Contact no longer exists.", "Product and Pricing for Contact");
      }
      else
      {
        if (!this.promptToSaveCurrentLoan())
          return;
        LoanDataMgr newLoanForContact = this.createNewLoanForContact(borrower, "Product and Pricing");
        if (newLoanForContact == null)
          return;
        this.openLoanInEditor(newLoanForContact, false);
        this.insertProductPrincingHiddenField();
        Session.LoanData.SetCreatedWithoutLoanNumber();
        Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;Product+and+Pricing");
        bool flag = false;
        DocumentLog[] allePassDocuments = Session.LoanData.GetLogList().GetAllePASSDocuments();
        if (allePassDocuments != null && allePassDocuments.Length != 0)
          flag = true;
        if (flag)
        {
          if (Session.LoanData.LoanNumber == "" && Session.LoanManager.IsTimeToSetLoanNumber(Session.LoanData))
          {
            Session.LoanData.LoanNumber = Session.LoanManager.GetNextLoanNumber();
            Session.LoanData.MersNumber = Session.LoanDataMgr.GetNextMersNumber();
          }
          Session.LoanData.UnsetCreatedWithoutLoanNumber();
          if (!Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) new StandardMilestoneTemplateApply(Session.DefaultInstance, false, true, ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_DisplayMilestoneChangeScreen)), false))
            return;
          IMainScreen service = Session.Application.GetService<IMainScreen>();
          if (service != null)
            service.NavigateToContact(CategoryType.Borrower);
          else
            Session.MainScreen.NavigateToContact(CategoryType.Borrower);
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Encompass generated a loan file to hold the product and pricing you just ordered. The loan file is currently open for your convenience to view the product and pricing.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          try
          {
            ContactUtils.addProductAndPricingHistory(borrower.ContactID);
          }
          catch
          {
          }
        }
        else
        {
          string guid = Session.LoanData.GUID;
          this.closeLoanEditor(false);
          Session.LoanManager.DeleteLoan(guid, false, true);
          Session.Application.GetService<IEncompassApplication>().SetCurrentActivity(EncompassActivity.BorrowerContacts);
        }
      }
    }

    private void closeLoanEditor(bool allowSave)
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (allowSave)
        service.CloseLoan(false);
      else
        service.CloseLoanWithoutPrompts(false);
    }

    private void openLoanInEditor(LoanDataMgr dataMgr, bool displayEditor)
    {
      Session.Application.GetService<ILoanConsole>().OpenLoan(dataMgr, displayEditor);
    }

    private void insertProductPrincingHiddenField()
    {
      Session.LoanData.SetField("OPTIMAL.REQUEST", "");
      Session.LoanData.SetField("OPTIMAL.RESPONSE", "");
      Session.LoanData.SetField("OPTIMAL.REQUEST", this.buildRequestData());
    }

    private string buildRequestData()
    {
      ValuePairXmlWriter valuePairXmlWriter = new ValuePairXmlWriter("FieldID", "FieldValue");
      valuePairXmlWriter.Write("ReturnPage", "BorrowerContact");
      string xml = valuePairXmlWriter.ToXML();
      Tracing.Log(BorrowerInfo1Form.sw, TraceLevel.Verbose, nameof (BorrowerInfo1Form), "Optimal Blue Request String = " + xml);
      return xml;
    }

    private void txtBoxBizZip_Leave(object sender, EventArgs e)
    {
      if (this.txtBoxBizZip.Text == "")
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(this.txtBoxBizZip.Text, ZipCodeUtils.GetMultipleZipInfoAt(this.txtBoxBizZip.Text));
      if (zipCodeInfo == null)
        return;
      this.txtBoxBizCity.Text = zipCodeInfo.City;
      this.txtBoxBizState.Text = zipCodeInfo.State;
    }

    private void txtBoxBizZip_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.initialLoad || e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void txtBoxBizWebUrl_Leave(object sender, EventArgs e)
    {
      string text = ((Control) sender).Text;
      if (text == "" || SystemUtil.IsValidURL(text))
        return;
      int num = (int) MessageBox.Show("Invalid Web URL.", "Web URL", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
  }
}
