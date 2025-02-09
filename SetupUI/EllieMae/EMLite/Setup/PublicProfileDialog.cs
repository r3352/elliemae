// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PublicProfileDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PublicProfileDialog : Form, IHelp
  {
    private const string className = "PublicProfileDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private TextBox lnameTxt;
    private TextBox fNameTxt;
    private ComboBox phone1ComboBox;
    private bool intermidiateData;
    private bool deleteBackKey;
    private UserInfo currentUser;
    private UserProfileInfo userprofile;
    private WorkflowManager workflowMgr;
    private IContainer components;
    private string userid;
    private Label lnameLabel;
    private Label fnameLabel;
    private string nmlsOriginatorID = string.Empty;
    private bool editUserProfile;
    private ToolTip toolTip1;
    private Panel panelTop;
    private Label label15;
    private TextBox suffixTxt;
    private Label label14;
    private TextBox middleNameTxt;
    private GradientPanel gradientPanel1;
    private CheckBox enableCheckBox;
    private Label label16;
    private Label label10;
    private Panel panel1;
    private Label label3;
    private TextBox link1Txt;
    private Label label2;
    private TextBox nmlsIdTxt;
    private FieldLockButton lBtnEmail;
    private Label label1;
    private TextBox emailTxt;
    private TextBox phone2Txt;
    private ComboBox phone2ComboBox;
    private FieldLockButton lBtnPhone1;
    private TextBox phone1Txt;
    private Label label8;
    private TextBox jobTitleTxt;
    private FieldLockButton lBtnSuffix;
    private FieldLockButton lBtnLastName;
    private FieldLockButton lBtnMiddleName;
    private FieldLockButton lBtnFirstName;
    private Panel panel2;
    private Label label4;
    private TextBox profileDescTxt;
    private TextBox link3Txt;
    private TextBox link2Txt;
    private Button cancelBtn;
    private Button okBtn;
    private Label label6;
    private FieldLockButton lBtnPhone2;
    private GroupContainer profile_photo_gc;
    private StandardIconButton btnAddPhoto;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnRemovePic;
    private StandardIconButton btnEditPhoto;
    private PictureBox profilePhoto;
    private Label label5;
    private string imageURL = "";
    private Image defaultImage;

    public string Userid => this.userid;

    public string NMLSOriginatorID => this.nmlsOriginatorID;

    public PublicProfileDialog(string userid)
    {
      this.workflowMgr = (WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.InitializeComponent();
      this.userid = userid;
      this.currentUser = (UserInfo) null;
      this.currentUser = Session.OrganizationManager.GetUser(userid);
      this.userprofile = (UserProfileInfo) null;
      this.userprofile = Session.OrganizationManager.GetUserProfile(userid);
      this.nmlsIdTxt.Text = this.currentUser.NMLSOriginatorID;
      this.phone2ComboBox.SelectedIndex = 0;
      if (this.userprofile == null)
      {
        this.editUserProfile = false;
        this.populateUserDetails();
      }
      else
      {
        this.populateProfileDetails();
        this.editUserProfile = true;
      }
      if (this.currentUser.Status == UserInfo.UserStatusEnum.Disabled)
      {
        this.enableCheckBox.Checked = this.enableCheckBox.Enabled = false;
      }
      else
      {
        this.enableCheckBox.Enabled = UserInfo.IsSuperAdministrator(Session.UserID, this.currentUser.UserPersonas);
        this.enableCheckBox.Checked = this.userprofile != null && this.userprofile.Enable_Profile;
      }
      if (this.currentUser.ProfileURL.Length > 0)
      {
        this.defaultImage = (Image) FileImageHelper.RetrieveImage(this.currentUser.ProfileURL);
        this.profilePhoto.Image = FileImageHelper.FixedSize(this.defaultImage, 80, 80);
        this.btnAddPhoto.Visible = false;
        this.btnEditPhoto.Enabled = true;
        this.btnRemovePic.Enabled = true;
      }
      else
      {
        this.btnAddPhoto.Visible = true;
        this.btnEditPhoto.Enabled = false;
        this.btnRemovePic.Enabled = false;
      }
    }

    private void populateProfileDetails()
    {
      this.fNameTxt.Text = this.userprofile.FirstName_IsDefault ? this.currentUser.FirstName : this.userprofile.FirstName;
      this.lBtnFirstName.Locked = !this.userprofile.FirstName_IsDefault;
      this.fNameTxt.ReadOnly = this.userprofile.FirstName_IsDefault;
      this.lnameTxt.Text = this.userprofile.LastName_IsDefault ? this.currentUser.LastName : this.userprofile.LastName;
      this.lBtnLastName.Locked = !this.userprofile.LastName_IsDefault;
      this.lnameTxt.ReadOnly = this.userprofile.LastName_IsDefault;
      this.middleNameTxt.Text = this.userprofile.MiddleName_IsDefault ? this.currentUser.MiddleName : this.userprofile.MiddleName;
      this.lBtnMiddleName.Locked = !this.userprofile.MiddleName_IsDefault;
      this.middleNameTxt.ReadOnly = this.userprofile.MiddleName_IsDefault;
      this.suffixTxt.Text = this.userprofile.SuffixName_IsDefault ? this.currentUser.SuffixName : this.userprofile.SuffixName;
      this.lBtnSuffix.Locked = this.suffixTxt.ReadOnly = !this.userprofile.SuffixName_IsDefault;
      this.suffixTxt.ReadOnly = this.userprofile.SuffixName_IsDefault;
      this.jobTitleTxt.Text = this.userprofile.JobTitle;
      if (this.userprofile.Phone1_IsDefault)
      {
        this.phone1ComboBox.SelectedIndex = this.userprofile.Phone1Type;
        if (this.phone1ComboBox.SelectedIndex == 0)
          this.phone1Txt.Text = this.currentUser.Phone;
        else if (this.phone1ComboBox.SelectedIndex == 1)
          this.phone1Txt.Text = this.currentUser.CellPhone;
        else if (this.phone1ComboBox.SelectedIndex == 2)
          this.phone1Txt.Text = this.currentUser.Fax;
        this.lBtnPhone1.Locked = false;
        this.phone1Txt.ReadOnly = true;
      }
      else
      {
        this.phone1ComboBox.SelectedIndex = this.userprofile.Phone1Type;
        this.phone1Txt.Text = this.userprofile.Phone1;
        this.lBtnPhone1.Locked = true;
        this.phone1Txt.ReadOnly = false;
      }
      if (this.userprofile.Phone2_IsDefault)
      {
        this.phone2ComboBox.SelectedIndex = this.userprofile.Phone2Type;
        if (this.phone2ComboBox.SelectedIndex == 1)
          this.phone2Txt.Text = this.currentUser.Phone;
        else if (this.phone2ComboBox.SelectedIndex == 2)
          this.phone2Txt.Text = this.currentUser.CellPhone;
        else if (this.phone2ComboBox.SelectedIndex == 3)
          this.phone2Txt.Text = this.currentUser.Fax;
        this.lBtnPhone2.Locked = false;
        this.phone2Txt.ReadOnly = true;
      }
      else
      {
        this.phone2ComboBox.SelectedIndex = this.userprofile.Phone2Type;
        this.phone2Txt.Text = this.userprofile.Phone2;
        this.lBtnPhone2.Locked = true;
        this.phone2Txt.ReadOnly = false;
      }
      this.emailTxt.Text = this.userprofile.Email_IsDefault ? this.currentUser.Email : this.userprofile.Email;
      this.lBtnEmail.Locked = !this.userprofile.Email_IsDefault;
      this.emailTxt.ReadOnly = this.userprofile.Email_IsDefault;
      this.nmlsIdTxt.Text = this.currentUser.NMLSOriginatorID;
      this.link1Txt.Text = this.userprofile.Link1;
      this.link2Txt.Text = this.userprofile.Link2;
      this.link3Txt.Text = this.userprofile.Link3;
      this.profileDescTxt.Text = this.userprofile.ProfileDesc;
    }

    private void populateUserDetails()
    {
      this.fNameTxt.Text = this.currentUser.FirstName;
      this.lnameTxt.Text = this.currentUser.LastName;
      this.middleNameTxt.Text = this.currentUser.MiddleName;
      this.suffixTxt.Text = this.currentUser.SuffixName;
      this.phone1ComboBox.SelectedIndex = 0;
      this.phone1Txt.Text = this.currentUser.Phone;
      this.emailTxt.Text = this.currentUser.Email;
      this.fNameTxt.ReadOnly = this.lnameTxt.ReadOnly = this.middleNameTxt.ReadOnly = this.suffixTxt.ReadOnly = this.phone1Txt.ReadOnly = this.emailTxt.ReadOnly = true;
      this.lBtnFirstName.Locked = this.lBtnLastName.Locked = this.lBtnMiddleName.Locked = this.lBtnSuffix.Locked = this.lBtnPhone1.Locked = this.lBtnEmail.Locked = this.lBtnPhone2.Locked = false;
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
      this.toolTip1 = new ToolTip(this.components);
      this.btnAddPhoto = new StandardIconButton();
      this.btnRemovePic = new StandardIconButton();
      this.panelTop = new Panel();
      this.panel2 = new Panel();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.panel1 = new Panel();
      this.profile_photo_gc = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnEditPhoto = new StandardIconButton();
      this.profilePhoto = new PictureBox();
      this.lBtnPhone2 = new FieldLockButton();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.profileDescTxt = new TextBox();
      this.link3Txt = new TextBox();
      this.link2Txt = new TextBox();
      this.label3 = new Label();
      this.link1Txt = new TextBox();
      this.label2 = new Label();
      this.nmlsIdTxt = new TextBox();
      this.lBtnEmail = new FieldLockButton();
      this.label1 = new Label();
      this.emailTxt = new TextBox();
      this.phone2Txt = new TextBox();
      this.phone2ComboBox = new ComboBox();
      this.lBtnPhone1 = new FieldLockButton();
      this.phone1Txt = new TextBox();
      this.label8 = new Label();
      this.jobTitleTxt = new TextBox();
      this.lBtnSuffix = new FieldLockButton();
      this.lBtnLastName = new FieldLockButton();
      this.lBtnMiddleName = new FieldLockButton();
      this.label15 = new Label();
      this.lBtnFirstName = new FieldLockButton();
      this.suffixTxt = new TextBox();
      this.fnameLabel = new Label();
      this.label14 = new Label();
      this.lnameTxt = new TextBox();
      this.middleNameTxt = new TextBox();
      this.fNameTxt = new TextBox();
      this.lnameLabel = new Label();
      this.phone1ComboBox = new ComboBox();
      this.gradientPanel1 = new GradientPanel();
      this.enableCheckBox = new CheckBox();
      this.label16 = new Label();
      this.label10 = new Label();
      ((ISupportInitialize) this.btnAddPhoto).BeginInit();
      ((ISupportInitialize) this.btnRemovePic).BeginInit();
      this.panelTop.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.profile_photo_gc.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnEditPhoto).BeginInit();
      ((ISupportInitialize) this.profilePhoto).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.btnAddPhoto.BackColor = Color.Transparent;
      this.btnAddPhoto.Location = new Point(67, 68);
      this.btnAddPhoto.Margin = new Padding(2, 3, 3, 3);
      this.btnAddPhoto.MouseDownImage = (Image) null;
      this.btnAddPhoto.Name = "btnAddPhoto";
      this.btnAddPhoto.Size = new Size(16, 16);
      this.btnAddPhoto.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPhoto.TabIndex = 81;
      this.btnAddPhoto.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddPhoto, "Add Photo");
      this.btnAddPhoto.Click += new EventHandler(this.btnAddPhoto_Click);
      this.btnRemovePic.BackColor = Color.Transparent;
      this.btnRemovePic.Enabled = false;
      this.btnRemovePic.Location = new Point(24, 3);
      this.btnRemovePic.Margin = new Padding(2, 3, 3, 3);
      this.btnRemovePic.MouseDownImage = (Image) null;
      this.btnRemovePic.Name = "btnRemovePic";
      this.btnRemovePic.Size = new Size(16, 16);
      this.btnRemovePic.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemovePic.TabIndex = 82;
      this.btnRemovePic.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemovePic, "Delete Photo");
      this.btnRemovePic.Click += new EventHandler(this.btnRemove_Click);
      this.panelTop.Controls.Add((Control) this.panel2);
      this.panelTop.Controls.Add((Control) this.panel1);
      this.panelTop.Controls.Add((Control) this.gradientPanel1);
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(514, 503);
      this.panelTop.TabIndex = 70;
      this.panel2.Controls.Add((Control) this.cancelBtn);
      this.panel2.Controls.Add((Control) this.okBtn);
      this.panel2.Dock = DockStyle.Bottom;
      this.panel2.Location = new Point(0, 469);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(514, 34);
      this.panel2.TabIndex = 4;
      this.cancelBtn.BackColor = SystemColors.Control;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(431, 7);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(72, 22);
      this.cancelBtn.TabIndex = 80;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Location = new Point(353, 7);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(72, 22);
      this.okBtn.TabIndex = 75;
      this.okBtn.Text = "&Save";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.panel1.Controls.Add((Control) this.profile_photo_gc);
      this.panel1.Controls.Add((Control) this.lBtnPhone2);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.profileDescTxt);
      this.panel1.Controls.Add((Control) this.link3Txt);
      this.panel1.Controls.Add((Control) this.link2Txt);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.link1Txt);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.nmlsIdTxt);
      this.panel1.Controls.Add((Control) this.lBtnEmail);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.emailTxt);
      this.panel1.Controls.Add((Control) this.phone2Txt);
      this.panel1.Controls.Add((Control) this.phone2ComboBox);
      this.panel1.Controls.Add((Control) this.lBtnPhone1);
      this.panel1.Controls.Add((Control) this.phone1Txt);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.jobTitleTxt);
      this.panel1.Controls.Add((Control) this.lBtnSuffix);
      this.panel1.Controls.Add((Control) this.lBtnLastName);
      this.panel1.Controls.Add((Control) this.lBtnMiddleName);
      this.panel1.Controls.Add((Control) this.label15);
      this.panel1.Controls.Add((Control) this.lBtnFirstName);
      this.panel1.Controls.Add((Control) this.suffixTxt);
      this.panel1.Controls.Add((Control) this.fnameLabel);
      this.panel1.Controls.Add((Control) this.label14);
      this.panel1.Controls.Add((Control) this.lnameTxt);
      this.panel1.Controls.Add((Control) this.middleNameTxt);
      this.panel1.Controls.Add((Control) this.fNameTxt);
      this.panel1.Controls.Add((Control) this.lnameLabel);
      this.panel1.Controls.Add((Control) this.phone1ComboBox);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 69);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(514, 401);
      this.panel1.TabIndex = 3;
      this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);
      this.profile_photo_gc.Controls.Add((Control) this.btnAddPhoto);
      this.profile_photo_gc.Controls.Add((Control) this.flowLayoutPanel1);
      this.profile_photo_gc.Controls.Add((Control) this.profilePhoto);
      this.profile_photo_gc.HeaderForeColor = SystemColors.ControlText;
      this.profile_photo_gc.Location = new Point(378, 13);
      this.profile_photo_gc.Name = "profile_photo_gc";
      this.profile_photo_gc.Size = new Size(128, 134);
      this.profile_photo_gc.TabIndex = 150;
      this.profile_photo_gc.Text = "Profile Photo";
      this.profile_photo_gc.Visible = false;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEditPhoto);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemovePic);
      this.flowLayoutPanel1.Location = new Point(64, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(48, 24);
      this.flowLayoutPanel1.TabIndex = 82;
      this.btnEditPhoto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditPhoto.BackColor = Color.Transparent;
      this.btnEditPhoto.Location = new Point(3, 3);
      this.btnEditPhoto.MouseDownImage = (Image) null;
      this.btnEditPhoto.Name = "btnEditPhoto";
      this.btnEditPhoto.Size = new Size(16, 16);
      this.btnEditPhoto.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditPhoto.TabIndex = 83;
      this.btnEditPhoto.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditPhoto, "Edit Photo");
      this.btnEditPhoto.Click += new EventHandler(this.btnPhotoEdit_Click);
      this.profilePhoto.BackColor = Color.DarkGray;
      this.profilePhoto.Location = new Point(32, 38);
      this.profilePhoto.Name = "profilePhoto";
      this.profilePhoto.Size = new Size(80, 80);
      this.profilePhoto.TabIndex = 83;
      this.profilePhoto.TabStop = false;
      this.lBtnPhone2.Location = new Point(233, 155);
      this.lBtnPhone2.LockedStateToolTip = "Use Default Value";
      this.lBtnPhone2.MaximumSize = new Size(16, 17);
      this.lBtnPhone2.MinimumSize = new Size(16, 17);
      this.lBtnPhone2.Name = "lBtnPhone2";
      this.lBtnPhone2.Size = new Size(16, 17);
      this.lBtnPhone2.TabIndex = 149;
      this.lBtnPhone2.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnPhone2.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnPhone2.Click += new EventHandler(this.lBtnPhone2_Click);
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(9, 155);
      this.label6.Name = "label6";
      this.label6.Size = new Size(46, 14);
      this.label6.TabIndex = 148;
      this.label6.Text = "Phone 2";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(9, 131);
      this.label5.Name = "label5";
      this.label5.Size = new Size(46, 14);
      this.label5.TabIndex = 147;
      this.label5.Text = "Phone 1";
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(9, 299);
      this.label4.Name = "label4";
      this.label4.Size = new Size(94, 14);
      this.label4.TabIndex = 146;
      this.label4.Text = "Profile Description";
      this.profileDescTxt.Location = new Point(152, 296);
      this.profileDescTxt.MaxLength = 1000;
      this.profileDescTxt.Multiline = true;
      this.profileDescTxt.Name = "profileDescTxt";
      this.profileDescTxt.ScrollBars = ScrollBars.Vertical;
      this.profileDescTxt.Size = new Size(220, 96);
      this.profileDescTxt.TabIndex = 70;
      this.link3Txt.Location = new Point(152, 272);
      this.link3Txt.MaxLength = 64;
      this.link3Txt.Name = "link3Txt";
      this.link3Txt.Size = new Size(220, 20);
      this.link3Txt.TabIndex = 65;
      this.link2Txt.Location = new Point(152, 248);
      this.link2Txt.MaxLength = 64;
      this.link2Txt.Name = "link2Txt";
      this.link2Txt.Size = new Size(220, 20);
      this.link2Txt.TabIndex = 60;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(9, 227);
      this.label3.Name = "label3";
      this.label3.Size = new Size(32, 14);
      this.label3.TabIndex = 142;
      this.label3.Text = "Links";
      this.link1Txt.Location = new Point(152, 224);
      this.link1Txt.MaxLength = 64;
      this.link1Txt.Name = "link1Txt";
      this.link1Txt.Size = new Size(220, 20);
      this.link1Txt.TabIndex = 55;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(9, 203);
      this.label2.Name = "label2";
      this.label2.Size = new Size(124, 14);
      this.label2.TabIndex = 140;
      this.label2.Text = "NMLS Loan Originator ID";
      this.nmlsIdTxt.Location = new Point(152, 200);
      this.nmlsIdTxt.MaxLength = 64;
      this.nmlsIdTxt.Name = "nmlsIdTxt";
      this.nmlsIdTxt.ReadOnly = true;
      this.nmlsIdTxt.Size = new Size(220, 20);
      this.nmlsIdTxt.TabIndex = 139;
      this.lBtnEmail.Location = new Point(130, 178);
      this.lBtnEmail.LockedStateToolTip = "Use Default Value";
      this.lBtnEmail.MaximumSize = new Size(16, 17);
      this.lBtnEmail.MinimumSize = new Size(16, 17);
      this.lBtnEmail.Name = "lBtnEmail";
      this.lBtnEmail.Size = new Size(16, 17);
      this.lBtnEmail.TabIndex = 138;
      this.lBtnEmail.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnEmail.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnEmail.Click += new EventHandler(this.lBtnEmail_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(9, 179);
      this.label1.Name = "label1";
      this.label1.Size = new Size(31, 14);
      this.label1.TabIndex = 137;
      this.label1.Text = "Email";
      this.emailTxt.Location = new Point(152, 176);
      this.emailTxt.MaxLength = 64;
      this.emailTxt.Name = "emailTxt";
      this.emailTxt.Size = new Size(220, 20);
      this.emailTxt.TabIndex = 50;
      this.phone2Txt.Enabled = false;
      this.phone2Txt.Location = new Point((int) byte.MaxValue, 152);
      this.phone2Txt.MaxLength = 64;
      this.phone2Txt.Name = "phone2Txt";
      this.phone2Txt.Size = new Size(117, 20);
      this.phone2Txt.TabIndex = 45;
      this.phone2Txt.TextChanged += new EventHandler(this.phoneTxt_TextChanged);
      this.phone2Txt.KeyDown += new KeyEventHandler(this.phoneTxt_KeyDown);
      this.phone2ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.phone2ComboBox.Items.AddRange(new object[4]
      {
        (object) "None",
        (object) "Work",
        (object) "Cell",
        (object) "Fax"
      });
      this.phone2ComboBox.Location = new Point(152, 152);
      this.phone2ComboBox.Name = "phone2ComboBox";
      this.phone2ComboBox.Size = new Size(62, 22);
      this.phone2ComboBox.TabIndex = 40;
      this.phone2ComboBox.SelectedIndexChanged += new EventHandler(this.phone2ComboBox_SelectedIndexChanged);
      this.lBtnPhone1.Location = new Point(233, 130);
      this.lBtnPhone1.LockedStateToolTip = "Use Default Value";
      this.lBtnPhone1.MaximumSize = new Size(16, 17);
      this.lBtnPhone1.MinimumSize = new Size(16, 17);
      this.lBtnPhone1.Name = "lBtnPhone1";
      this.lBtnPhone1.Size = new Size(16, 17);
      this.lBtnPhone1.TabIndex = 133;
      this.lBtnPhone1.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnPhone1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnPhone1.Click += new EventHandler(this.lBtnPhone1_Click);
      this.phone1Txt.Location = new Point((int) byte.MaxValue, 128);
      this.phone1Txt.MaxLength = 64;
      this.phone1Txt.Name = "phone1Txt";
      this.phone1Txt.Size = new Size(117, 20);
      this.phone1Txt.TabIndex = 35;
      this.phone1Txt.TextChanged += new EventHandler(this.phoneTxt_TextChanged);
      this.phone1Txt.KeyDown += new KeyEventHandler(this.phoneTxt_KeyDown);
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Location = new Point(9, 107);
      this.label8.Name = "label8";
      this.label8.Size = new Size(46, 14);
      this.label8.TabIndex = 131;
      this.label8.Text = "Job Title";
      this.jobTitleTxt.Location = new Point(152, 104);
      this.jobTitleTxt.MaxLength = 64;
      this.jobTitleTxt.Name = "jobTitleTxt";
      this.jobTitleTxt.Size = new Size(220, 20);
      this.jobTitleTxt.TabIndex = 25;
      this.lBtnSuffix.Location = new Point(130, 82);
      this.lBtnSuffix.LockedStateToolTip = "Use Default Value";
      this.lBtnSuffix.MaximumSize = new Size(16, 17);
      this.lBtnSuffix.MinimumSize = new Size(16, 17);
      this.lBtnSuffix.Name = "lBtnSuffix";
      this.lBtnSuffix.Size = new Size(16, 17);
      this.lBtnSuffix.TabIndex = 129;
      this.lBtnSuffix.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnSuffix.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnSuffix.Click += new EventHandler(this.lBtnSuffix_Click);
      this.lBtnLastName.Location = new Point(130, 57);
      this.lBtnLastName.LockedStateToolTip = "Use Default Value";
      this.lBtnLastName.MaximumSize = new Size(16, 17);
      this.lBtnLastName.MinimumSize = new Size(16, 17);
      this.lBtnLastName.Name = "lBtnLastName";
      this.lBtnLastName.Size = new Size(16, 17);
      this.lBtnLastName.TabIndex = 128;
      this.lBtnLastName.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnLastName.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnLastName.Click += new EventHandler(this.lBtnLastName_Click);
      this.lBtnMiddleName.Location = new Point(130, 33);
      this.lBtnMiddleName.LockedStateToolTip = "Use Default Value";
      this.lBtnMiddleName.MaximumSize = new Size(16, 17);
      this.lBtnMiddleName.MinimumSize = new Size(16, 17);
      this.lBtnMiddleName.Name = "lBtnMiddleName";
      this.lBtnMiddleName.Size = new Size(16, 17);
      this.lBtnMiddleName.TabIndex = (int) sbyte.MaxValue;
      this.lBtnMiddleName.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnMiddleName.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnMiddleName.Click += new EventHandler(this.lBtnMiddleName_Click);
      this.label15.AutoSize = true;
      this.label15.BackColor = Color.Transparent;
      this.label15.Location = new Point(9, 83);
      this.label15.Name = "label15";
      this.label15.Size = new Size(39, 14);
      this.label15.TabIndex = 126;
      this.label15.Text = "Suffix ";
      this.lBtnFirstName.Location = new Point(130, 11);
      this.lBtnFirstName.LockedStateToolTip = "Use Default Value";
      this.lBtnFirstName.MaximumSize = new Size(16, 17);
      this.lBtnFirstName.MinimumSize = new Size(16, 17);
      this.lBtnFirstName.Name = "lBtnFirstName";
      this.lBtnFirstName.Size = new Size(16, 17);
      this.lBtnFirstName.TabIndex = 43;
      this.lBtnFirstName.Tag = (object) "LOCKBUTTON_3142";
      this.lBtnFirstName.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnFirstName.Click += new EventHandler(this.lBtnFirstName_Click);
      this.suffixTxt.Location = new Point(152, 80);
      this.suffixTxt.MaxLength = 64;
      this.suffixTxt.Name = "suffixTxt";
      this.suffixTxt.Size = new Size(220, 20);
      this.suffixTxt.TabIndex = 20;
      this.fnameLabel.AutoSize = true;
      this.fnameLabel.BackColor = Color.Transparent;
      this.fnameLabel.Location = new Point(9, 11);
      this.fnameLabel.Name = "fnameLabel";
      this.fnameLabel.Size = new Size(58, 14);
      this.fnameLabel.TabIndex = 8;
      this.fnameLabel.Text = "First Name";
      this.label14.AutoSize = true;
      this.label14.BackColor = Color.Transparent;
      this.label14.Location = new Point(9, 35);
      this.label14.Name = "label14";
      this.label14.Size = new Size(67, 14);
      this.label14.TabIndex = 124;
      this.label14.Text = "Middle Name";
      this.lnameTxt.Location = new Point(152, 56);
      this.lnameTxt.MaxLength = 64;
      this.lnameTxt.Name = "lnameTxt";
      this.lnameTxt.Size = new Size(220, 20);
      this.lnameTxt.TabIndex = 15;
      this.middleNameTxt.Location = new Point(152, 32);
      this.middleNameTxt.MaxLength = 64;
      this.middleNameTxt.Name = "middleNameTxt";
      this.middleNameTxt.Size = new Size(220, 20);
      this.middleNameTxt.TabIndex = 10;
      this.fNameTxt.Location = new Point(152, 8);
      this.fNameTxt.MaxLength = 64;
      this.fNameTxt.Name = "fNameTxt";
      this.fNameTxt.Size = new Size(220, 20);
      this.fNameTxt.TabIndex = 5;
      this.lnameLabel.AutoSize = true;
      this.lnameLabel.BackColor = Color.Transparent;
      this.lnameLabel.Location = new Point(9, 59);
      this.lnameLabel.Name = "lnameLabel";
      this.lnameLabel.Size = new Size(58, 14);
      this.lnameLabel.TabIndex = 10;
      this.lnameLabel.Text = "Last Name";
      this.phone1ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.phone1ComboBox.Items.AddRange(new object[3]
      {
        (object) "Work",
        (object) "Cell",
        (object) "Fax"
      });
      this.phone1ComboBox.Location = new Point(152, 128);
      this.phone1ComboBox.Name = "phone1ComboBox";
      this.phone1ComboBox.Size = new Size(62, 22);
      this.phone1ComboBox.TabIndex = 30;
      this.phone1ComboBox.SelectedIndexChanged += new EventHandler(this.phone1ComboBox_SelectedIndexChanged);
      this.gradientPanel1.BackColor = SystemColors.Control;
      this.gradientPanel1.Controls.Add((Control) this.enableCheckBox);
      this.gradientPanel1.Controls.Add((Control) this.label16);
      this.gradientPanel1.Controls.Add((Control) this.label10);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(514, 69);
      this.gradientPanel1.TabIndex = 2;
      this.enableCheckBox.BackColor = Color.Transparent;
      this.enableCheckBox.Enabled = false;
      this.enableCheckBox.Location = new Point(12, 42);
      this.enableCheckBox.Name = "enableCheckBox";
      this.enableCheckBox.Size = new Size(184, 18);
      this.enableCheckBox.TabIndex = 1;
      this.enableCheckBox.Text = "Enable Public Profile";
      this.enableCheckBox.UseVisualStyleBackColor = false;
      this.label16.AutoSize = true;
      this.label16.BackColor = Color.Transparent;
      this.label16.Location = new Point(9, 25);
      this.label16.Name = "label16";
      this.label16.Size = new Size(305, 14);
      this.label16.TabIndex = 128;
      this.label16.Text = "profile information is shared for the Connect portals from here";
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Location = new Point(9, 9);
      this.label10.Name = "label10";
      this.label10.Size = new Size(497, 14);
      this.label10.TabIndex = (int) sbyte.MaxValue;
      this.label10.Text = "Enter/Modify information for the users's public profile. You may also enable/disable whether the public";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(514, 503);
      this.Controls.Add((Control) this.panelTop);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PublicProfileDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Public Profile Information";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      ((ISupportInitialize) this.btnAddPhoto).EndInit();
      ((ISupportInitialize) this.btnRemovePic).EndInit();
      this.panelTop.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.profile_photo_gc.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnEditPhoto).EndInit();
      ((ISupportInitialize) this.profilePhoto).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (!this.saveUserProfile())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private bool verifyInput()
    {
      if (this.lnameTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user last name and first name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.lnameTxt.Focus();
        return false;
      }
      if (this.fNameTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user last name and first name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.fNameTxt.Focus();
        return false;
      }
      if (this.emailTxt.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Many Encompass features require an email address. Please enter one at this time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.emailTxt.Focus();
        return false;
      }
      if (!Utils.ValidateEmail(this.emailTxt.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.emailTxt.Focus();
        return false;
      }
      if (!string.IsNullOrEmpty(this.link1Txt.Text.Trim()) && !Utils.ValidateUrl(this.link1Txt.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The link1 format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.link1Txt.Focus();
        return false;
      }
      if (!string.IsNullOrEmpty(this.link2Txt.Text.Trim()) && !Utils.ValidateUrl(this.link2Txt.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The link2 format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.link2Txt.Focus();
        return false;
      }
      if (string.IsNullOrEmpty(this.link3Txt.Text.Trim()) || Utils.ValidateUrl(this.link3Txt.Text.Trim()))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The link3 format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.link3Txt.Focus();
      return false;
    }

    private void phoneTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private bool saveUserProfile()
    {
      try
      {
        if (!this.verifyInput())
          return false;
        Session.OrganizationManager.UpdateUserProfile(this.getUserProfileInfo(), this.editUserProfile);
        this.editUserProfile = true;
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public UserProfileInfo getUserProfileInfo()
    {
      return new UserProfileInfo(this.currentUser.Userid, this.lnameTxt.Text.Trim(), this.suffixTxt.Text.Trim(), this.fNameTxt.Text.Trim(), this.middleNameTxt.Text.Trim(), this.jobTitleTxt.Text.Trim(), this.phone1ComboBox.SelectedIndex, this.phone1Txt.Text.Trim(), this.phone2ComboBox.SelectedIndex, this.phone2Txt.Text.Trim(), this.emailTxt.Text.Trim(), this.link1Txt.Text.Trim(), this.link2Txt.Text.Trim(), this.link3Txt.Text.Trim(), this.profileDescTxt.Text.Trim(), !this.lBtnFirstName.Locked, !this.lBtnLastName.Locked, !this.lBtnMiddleName.Locked, !this.lBtnSuffix.Locked, !this.lBtnPhone1.Locked, !this.lBtnEmail.Locked, !this.lBtnPhone2.Locked, this.enableCheckBox.Checked);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (PublicProfileDialog));
    }

    private void lBtnFirstName_Click(object sender, EventArgs e)
    {
      this.lBtnFirstName.Locked = !this.lBtnFirstName.Locked;
      this.fNameTxt.ReadOnly = !this.lBtnFirstName.Locked;
      if (this.lBtnFirstName.Locked)
        return;
      this.fNameTxt.Text = this.currentUser.FirstName;
    }

    private void lBtnMiddleName_Click(object sender, EventArgs e)
    {
      this.lBtnMiddleName.Locked = !this.lBtnMiddleName.Locked;
      this.middleNameTxt.ReadOnly = !this.lBtnMiddleName.Locked;
      if (this.lBtnMiddleName.Locked)
        return;
      this.middleNameTxt.Text = this.currentUser.MiddleName;
    }

    private void lBtnLastName_Click(object sender, EventArgs e)
    {
      this.lBtnLastName.Locked = !this.lBtnLastName.Locked;
      this.lnameTxt.ReadOnly = !this.lBtnLastName.Locked;
      if (this.lBtnLastName.Locked)
        return;
      this.lnameTxt.Text = this.currentUser.LastName;
    }

    private void lBtnSuffix_Click(object sender, EventArgs e)
    {
      this.lBtnSuffix.Locked = !this.lBtnSuffix.Locked;
      this.suffixTxt.ReadOnly = !this.lBtnSuffix.Locked;
      if (this.lBtnSuffix.Locked)
        return;
      this.suffixTxt.Text = this.currentUser.SuffixName;
    }

    private void lBtnPhone1_Click(object sender, EventArgs e)
    {
      this.lBtnPhone1.Locked = !this.lBtnPhone1.Locked;
      this.phone1Txt.ReadOnly = !this.lBtnPhone1.Locked;
      if (this.lBtnPhone1.Locked)
        return;
      if (this.phone1ComboBox.SelectedIndex == 0)
        this.phone1Txt.Text = this.currentUser.Phone;
      else if (this.phone1ComboBox.SelectedIndex == 1)
        this.phone1Txt.Text = this.currentUser.CellPhone;
      else if (this.phone1ComboBox.SelectedIndex == 2)
        this.phone1Txt.Text = this.currentUser.Fax;
      else
        this.phone1Txt.Text = string.Empty;
    }

    private void lBtnEmail_Click(object sender, EventArgs e)
    {
      this.lBtnEmail.Locked = !this.lBtnEmail.Locked;
      this.emailTxt.ReadOnly = !this.lBtnEmail.Locked;
      if (this.lBtnEmail.Locked)
        return;
      this.emailTxt.Text = this.currentUser.Email;
    }

    private void phoneTxt_TextChanged(object sender, EventArgs e)
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

    private void phoneTxt_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void phone2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.phone2ComboBox.SelectedIndex == 0)
      {
        this.phone2Txt.Text = string.Empty;
        this.phone2Txt.Enabled = false;
        this.lBtnPhone2.Enabled = false;
      }
      else
      {
        this.lBtnPhone2.Enabled = true;
        this.phone2Txt.Enabled = true;
        this.lBtnPhone2.Locked = false;
        this.phone2Txt.ReadOnly = true;
        if (this.phone2ComboBox.SelectedIndex == 1)
          this.phone2Txt.Text = this.currentUser.Phone;
        else if (this.phone2ComboBox.SelectedIndex == 2)
        {
          this.phone2Txt.Text = this.currentUser.CellPhone;
        }
        else
        {
          if (this.phone2ComboBox.SelectedIndex != 3)
            return;
          this.phone2Txt.Text = this.currentUser.Fax;
        }
      }
    }

    private void lBtnPhone2_Click(object sender, EventArgs e)
    {
      this.lBtnPhone2.Locked = !this.lBtnPhone2.Locked;
      this.phone2Txt.ReadOnly = !this.lBtnPhone2.Locked;
      if (this.lBtnPhone2.Locked)
        return;
      if (this.phone2ComboBox.SelectedIndex == 1)
        this.phone2Txt.Text = this.currentUser.Phone;
      else if (this.phone2ComboBox.SelectedIndex == 2)
        this.phone2Txt.Text = this.currentUser.CellPhone;
      else if (this.phone2ComboBox.SelectedIndex == 3)
        this.phone2Txt.Text = this.currentUser.Fax;
      else
        this.phone2Txt.Text = string.Empty;
    }

    private void phone1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.lBtnPhone1.Locked = false;
      this.phone1Txt.ReadOnly = true;
      if (this.phone1ComboBox.SelectedIndex == 0)
        this.phone1Txt.Text = this.currentUser.Phone;
      else if (this.phone1ComboBox.SelectedIndex == 1)
        this.phone1Txt.Text = this.currentUser.CellPhone;
      else if (this.phone1ComboBox.SelectedIndex == 2)
        this.phone1Txt.Text = this.currentUser.Fax;
      else
        this.phone1Txt.Text = string.Empty;
    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void btnAddPhoto_Click(object sender, EventArgs e)
    {
      Image img = (Image) null;
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = "Upload Photo";
        openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          if (FileImageHelper.ValidPhoto(openFileDialog.FileName))
          {
            img = (Image) new Bitmap(openFileDialog.FileName);
            if (!FileImageHelper.ValidDimensions(img.Width, img.Height))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Profile picture size does not meet the minimum size requirements of 300 pixels length and 300 pixels width.  We recommend that you upload a picture that is larger in size.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
          else
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "Profile photo must be between 3kb and 2MB.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      if (img == null)
        return;
      using (ProfilePictureEdit profilePictureEdit = new ProfilePictureEdit(img))
      {
        if (profilePictureEdit.ShowDialog() != DialogResult.OK || profilePictureEdit.newImage == null)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.imageURL = FileImageHelper.UploadImage(profilePictureEdit.newImage);
        if (this.currentUser == (UserInfo) null)
          this.currentUser = Session.OrganizationManager.GetUser(this.userid);
        this.currentUser.ProfileURL = this.imageURL;
        Session.OrganizationManager.UpdateUser(this.currentUser);
        this.defaultImage = (Image) FileImageHelper.RetrieveImage(this.currentUser.ProfileURL);
        this.profilePhoto.Image = FileImageHelper.FixedSize(this.defaultImage, 80, 80);
        this.btnAddPhoto.Visible = false;
        this.btnRemovePic.Enabled = true;
        this.btnEditPhoto.Enabled = true;
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnPhotoEdit_Click(object sender, EventArgs e)
    {
      using (ProfilePictureEdit profilePictureEdit = new ProfilePictureEdit(this.defaultImage))
      {
        if (profilePictureEdit.ShowDialog() != DialogResult.OK || profilePictureEdit.newImage == null)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.imageURL = FileImageHelper.UploadImage(profilePictureEdit.newImage);
        if (this.currentUser == (UserInfo) null)
          this.currentUser = Session.OrganizationManager.GetUser(this.userid);
        this.currentUser.ProfileURL = this.imageURL;
        Session.OrganizationManager.UpdateUser(this.currentUser);
        this.defaultImage = (Image) FileImageHelper.RetrieveImage(this.currentUser.ProfileURL);
        this.profilePhoto.Image = FileImageHelper.FixedSize(this.defaultImage, 80, 80);
        this.btnAddPhoto.Visible = false;
        this.btnRemovePic.Enabled = true;
        this.btnEditPhoto.Enabled = true;
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the profile picture.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      if (this.currentUser == (UserInfo) null)
        this.currentUser = Session.OrganizationManager.GetUser(this.userid);
      this.currentUser.ProfileURL = "";
      Session.OrganizationManager.UpdateUser(this.currentUser);
      this.profilePhoto.Image = (Image) null;
      this.defaultImage = (Image) null;
      this.btnAddPhoto.Visible = true;
      this.btnEditPhoto.Enabled = false;
    }
  }
}
