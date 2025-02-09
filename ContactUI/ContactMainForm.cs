// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactMainForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.CampaignUI;
using EllieMae.EMLite.ContactUI.Properties;
using EllieMae.EMLite.ContactUI.TaskList;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Synchronization.UI;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactMainForm : Form, IOnlineHelpTarget, IPrint
  {
    private const string className = "ContactMainForm";
    private static readonly string sw = Tracing.SwContact;
    private BorrowerListForm borrowerListForm;
    private BizPartnerListForm bizPartnerListForm;
    private CampaignDashboardForm frmCampaignDashboard;
    private frmCalendar CalendarListForm;
    private frmCalendar frmAuxilliaryCalendar;
    private TaskListForm taskListForm;
    private string currentCalendarUserID;
    private CSMessage.AccessLevel currentCalendarUserAccessLevel = CSMessage.AccessLevel.Full;
    private IContainer components;
    private FeaturesAclManager aclMgr;
    private Panel panel3;
    private Panel panelRight;
    private Color backColor = Color.FromArgb(247, 235, 206);
    private Color btnColor = Color.FromArgb(221, 221, 221);
    private Color contactTabColor = Color.FromArgb(239, 239, 239);
    private Color unselectedButtonColor = Color.FromArgb(0, 0, 0);
    private Color selectedButtonColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    private IconButton btnBorrower;
    private IconButton btnBizPartner;
    private IconButton btnCalendar;
    private IconButton btnTasks;
    private IconButton btnCampaigns;
    private FlowLayoutPanel flowLayoutPanel1;
    private Panel pnlBorrower;
    private Panel pnlBizPartner;
    private Panel pnlCalendar;
    private Panel pnlTask;
    private Panel pnlCampaign;
    private Panel panel1;
    private Sessions.Session session;
    private ContactMainForm.FormState formState;

    public event ContactMainForm.ContactTabChanged OnContactTabChange;

    public bool IsPrintEnabled
    {
      get
      {
        FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
        return ContactMainForm.FormState.Borrower == this._FormState ? aclManager.GetUserApplicationRight(AclFeature.Cnt_Borrower_Print) : (ContactMainForm.FormState.BizContact == this._FormState ? aclManager.GetUserApplicationRight(AclFeature.Cnt_Biz_Print) : ContactMainForm.FormState.Calendar == this._FormState);
      }
    }

    public ContactMainForm(
      Sessions.Session session,
      ContactMainForm.ContactTabChanged onSubTabChange)
    {
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "Instantiating ContactMainForm...");
      this.session = session;
      this.InitializeComponent();
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "ContactMainForm content initialization completed...");
      PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Contacts UI Initialization", 226, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\ContactMainForm.cs");
      this.currentCalendarUserID = this.session.UserID;
      this.OnContactTabChange += new ContactMainForm.ContactTabChanged(onSubTabChange.Invoke);
      this.Init();
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "ContactMainForm constructor completed...");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnBorrower = new IconButton();
      this.btnBizPartner = new IconButton();
      this.btnCalendar = new IconButton();
      this.btnCampaigns = new IconButton();
      this.btnTasks = new IconButton();
      this.panelRight = new Panel();
      this.panel3 = new Panel();
      this.panel1 = new Panel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.pnlBorrower = new Panel();
      this.pnlBizPartner = new Panel();
      this.pnlCalendar = new Panel();
      this.pnlTask = new Panel();
      this.pnlCampaign = new Panel();
      ((ISupportInitialize) this.btnBorrower).BeginInit();
      ((ISupportInitialize) this.btnBizPartner).BeginInit();
      ((ISupportInitialize) this.btnCalendar).BeginInit();
      ((ISupportInitialize) this.btnCampaigns).BeginInit();
      ((ISupportInitialize) this.btnTasks).BeginInit();
      this.panel3.SuspendLayout();
      this.panel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.pnlBorrower.SuspendLayout();
      this.pnlBizPartner.SuspendLayout();
      this.pnlCalendar.SuspendLayout();
      this.pnlTask.SuspendLayout();
      this.pnlCampaign.SuspendLayout();
      this.SuspendLayout();
      this.btnBorrower.DisabledImage = (Image) null;
      this.btnBorrower.Image = (Image) Resources.btn_borrower_contacts;
      this.btnBorrower.Location = new Point(0, 0);
      this.btnBorrower.Margin = new Padding(0);
      this.btnBorrower.MouseDownImage = (Image) null;
      this.btnBorrower.MouseOverImage = (Image) Resources.btn_borrower_contacts_over;
      this.btnBorrower.Name = "btnBorrower";
      this.btnBorrower.Size = new Size(138, 31);
      this.btnBorrower.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnBorrower.TabIndex = 5;
      this.btnBorrower.TabStop = false;
      this.btnBorrower.Click += new EventHandler(this.btnBorrower_Click);
      this.btnBizPartner.DisabledImage = (Image) null;
      this.btnBizPartner.Image = (Image) Resources.btn_business_contacts;
      this.btnBizPartner.Location = new Point(0, 0);
      this.btnBizPartner.Margin = new Padding(0);
      this.btnBizPartner.MouseDownImage = (Image) null;
      this.btnBizPartner.MouseOverImage = (Image) Resources.btn_business_contacts_over;
      this.btnBizPartner.Name = "btnBizPartner";
      this.btnBizPartner.Size = new Size(141, 31);
      this.btnBizPartner.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnBizPartner.TabIndex = 6;
      this.btnBizPartner.TabStop = false;
      this.btnBizPartner.Click += new EventHandler(this.btnBizPartner_Click);
      this.btnCalendar.DisabledImage = (Image) null;
      this.btnCalendar.Image = (Image) Resources.btn_calendar;
      this.btnCalendar.Location = new Point(0, 0);
      this.btnCalendar.Margin = new Padding(0);
      this.btnCalendar.MouseDownImage = (Image) null;
      this.btnCalendar.MouseOverImage = (Image) Resources.btn_calendar_over;
      this.btnCalendar.Name = "btnCalendar";
      this.btnCalendar.Size = new Size(93, 31);
      this.btnCalendar.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnCalendar.TabIndex = 7;
      this.btnCalendar.TabStop = false;
      this.btnCalendar.Click += new EventHandler(this.btnCalendar_Click);
      this.btnCampaigns.DisabledImage = (Image) null;
      this.btnCampaigns.Image = (Image) Resources.btn_campaigns;
      this.btnCampaigns.Location = new Point(-1, 0);
      this.btnCampaigns.Margin = new Padding(0);
      this.btnCampaigns.MouseDownImage = (Image) null;
      this.btnCampaigns.MouseOverImage = (Image) Resources.btn_campaigns_over;
      this.btnCampaigns.Name = "btnCampaigns";
      this.btnCampaigns.Size = new Size(106, 31);
      this.btnCampaigns.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnCampaigns.TabIndex = 9;
      this.btnCampaigns.TabStop = false;
      this.btnCampaigns.Click += new EventHandler(this.btnCampaigns_Click);
      this.btnTasks.DisabledImage = (Image) null;
      this.btnTasks.Image = (Image) Resources.btn_tasks;
      this.btnTasks.Location = new Point(0, 0);
      this.btnTasks.Margin = new Padding(0);
      this.btnTasks.MouseDownImage = (Image) null;
      this.btnTasks.MouseOverImage = (Image) Resources.btn_tasks_over;
      this.btnTasks.Name = "btnTasks";
      this.btnTasks.Size = new Size(73, 31);
      this.btnTasks.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnTasks.TabIndex = 8;
      this.btnTasks.TabStop = false;
      this.btnTasks.Click += new EventHandler(this.btnTasks_Click);
      this.panelRight.AutoScroll = true;
      this.panelRight.Dock = DockStyle.Fill;
      this.panelRight.Location = new Point(0, 29);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new Size(794, 476);
      this.panelRight.TabIndex = 0;
      this.panel3.BackColor = Color.Transparent;
      this.panel3.Controls.Add((Control) this.panel1);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(794, 505);
      this.panel3.TabIndex = 9;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.panelRight);
      this.panel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(794, 505);
      this.panel1.TabIndex = 3;
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlBorrower);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlBizPartner);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlCalendar);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlTask);
      this.flowLayoutPanel1.Controls.Add((Control) this.pnlCampaign);
      this.flowLayoutPanel1.Dock = DockStyle.Top;
      this.flowLayoutPanel1.Location = new Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(794, 29);
      this.flowLayoutPanel1.TabIndex = 10;
      this.pnlBorrower.Controls.Add((Control) this.btnBorrower);
      this.pnlBorrower.Location = new Point(0, 0);
      this.pnlBorrower.Margin = new Padding(0);
      this.pnlBorrower.Name = "pnlBorrower";
      this.pnlBorrower.Size = new Size(137, 29);
      this.pnlBorrower.TabIndex = 10;
      this.pnlBizPartner.Controls.Add((Control) this.btnBizPartner);
      this.pnlBizPartner.Location = new Point(137, 0);
      this.pnlBizPartner.Margin = new Padding(0);
      this.pnlBizPartner.Name = "pnlBizPartner";
      this.pnlBizPartner.Size = new Size(140, 29);
      this.pnlBizPartner.TabIndex = 11;
      this.pnlCalendar.Controls.Add((Control) this.btnCalendar);
      this.pnlCalendar.Location = new Point(277, 0);
      this.pnlCalendar.Margin = new Padding(0);
      this.pnlCalendar.Name = "pnlCalendar";
      this.pnlCalendar.Size = new Size(92, 31);
      this.pnlCalendar.TabIndex = 12;
      this.pnlTask.Controls.Add((Control) this.btnTasks);
      this.pnlTask.Location = new Point(369, 0);
      this.pnlTask.Margin = new Padding(0);
      this.pnlTask.Name = "pnlTask";
      this.pnlTask.Size = new Size(73, 29);
      this.pnlTask.TabIndex = 13;
      this.pnlCampaign.Controls.Add((Control) this.btnCampaigns);
      this.pnlCampaign.Location = new Point(442, 0);
      this.pnlCampaign.Margin = new Padding(0);
      this.pnlCampaign.Name = "pnlCampaign";
      this.pnlCampaign.Size = new Size(106, 29);
      this.pnlCampaign.TabIndex = 14;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(794, 505);
      this.Controls.Add((Control) this.panel3);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ContactMainForm);
      this.Text = nameof (ContactMainForm);
      this.Closed += new EventHandler(this.ContactMainForm_Closed);
      this.SizeChanged += new EventHandler(this.ContactMainForm_SizeChanged);
      ((ISupportInitialize) this.btnBorrower).EndInit();
      ((ISupportInitialize) this.btnBizPartner).EndInit();
      ((ISupportInitialize) this.btnCalendar).EndInit();
      ((ISupportInitialize) this.btnCampaigns).EndInit();
      ((ISupportInitialize) this.btnTasks).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.pnlBorrower.ResumeLayout(false);
      this.pnlBorrower.PerformLayout();
      this.pnlBizPartner.ResumeLayout(false);
      this.pnlBizPartner.PerformLayout();
      this.pnlCalendar.ResumeLayout(false);
      this.pnlCalendar.PerformLayout();
      this.pnlTask.ResumeLayout(false);
      this.pnlTask.PerformLayout();
      this.pnlCampaign.ResumeLayout(false);
      this.pnlCampaign.PerformLayout();
      this.ResumeLayout(false);
    }

    public void ShowContent()
    {
      if (this._FormState != ContactMainForm.FormState.None)
        return;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      if (aclManager.GetUserApplicationRight(AclFeature.Cnt_Borrower_Access))
        this.btnBorrower_Click((object) null, (EventArgs) null);
      else if (aclManager.GetUserApplicationRight(AclFeature.Cnt_Biz_Access))
        this.btnBizPartner_Click((object) null, (EventArgs) null);
      else
        this.btnCalendar_Click((object) null, (EventArgs) null);
    }

    public void ShowContent(ContactMainForm.ContactsContentEnum content)
    {
      switch (content)
      {
        case ContactMainForm.ContactsContentEnum.BorrowerContacts:
          this.btnBorrower_Click((object) this, (EventArgs) null);
          break;
        case ContactMainForm.ContactsContentEnum.BusinessContacts:
          this.btnBizPartner_Click((object) this, (EventArgs) null);
          break;
        case ContactMainForm.ContactsContentEnum.Calendar:
          this.btnCalendar_Click((object) this, (EventArgs) null);
          break;
        case ContactMainForm.ContactsContentEnum.Tasks:
          this.btnTasks_Click((object) this, (EventArgs) null);
          break;
        case ContactMainForm.ContactsContentEnum.Campaigns:
          this.btnCampaigns_Click((object) this, (EventArgs) null);
          break;
      }
    }

    public void PrintPreview()
    {
    }

    private void Init()
    {
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "Retrieving user's feature access rights...");
      UserInfo userInfo = this.session.UserInfo;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.pnlBorrower.Visible = aclManager.GetUserApplicationRight(AclFeature.Cnt_Borrower_Access);
      this.pnlBizPartner.Visible = aclManager.GetUserApplicationRight(AclFeature.Cnt_Biz_Access);
      this.pnlCampaign.Visible = aclManager.GetUserApplicationRight(AclFeature.Cnt_Campaign_Access);
      PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Contacts UI Visibility", 543, nameof (Init), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\ContactMainForm.cs");
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "Creating calendar form...");
      this.CalendarListForm = frmCalendar.GetUserCalendar(this.session.UserInfo.Userid, CSMessage.AccessLevel.Full);
      this.CalendarListForm.TopLevel = false;
      this.CalendarListForm.Visible = true;
      this.CalendarListForm.Dock = DockStyle.Fill;
      this.CalendarListForm.CalendarSearchEvent += new EllieMae.EMLite.Calendar.CalendarSearchControl.CalendarSearchEventHandler(this.CalendarListForm_CalendarSearchEvent);
      PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Contacts Calendar For Alerts", 553, nameof (Init), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\ContactMainForm.cs");
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "Loading calendar state...");
      this.LoadCalendarState(false);
      PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Contacts Calendar State", 558, nameof (Init), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\ContactMainForm.cs");
      this.Dock = DockStyle.Fill;
    }

    public void GotoContact(ContactInfo contact)
    {
      this.Calendar_GotoContact((object) contact, (EventArgs) null);
    }

    private void Calendar_GotoContact(object sender, EventArgs e)
    {
      ContactInfo contactInfo = (ContactInfo) null;
      if (sender is ContactInfo)
        contactInfo = (ContactInfo) sender;
      if (contactInfo == null)
        return;
      if (contactInfo.ContactType == CategoryType.Borrower)
      {
        if (!this.pnlBorrower.Visible)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have access to borrower contact.");
        }
        else
        {
          this.btnBorrower_Click((object) null, (EventArgs) null);
          this.borrowerListForm.GoToContact(int.Parse(contactInfo.ContactID));
        }
      }
      else
      {
        if (contactInfo.ContactType != CategoryType.BizPartner)
          return;
        if (!this.pnlBizPartner.Visible)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You do not have access to business contact.");
        }
        else
        {
          this.btnBizPartner_Click((object) null, (EventArgs) null);
          this.bizPartnerListForm.GoToContact(int.Parse(contactInfo.ContactID));
        }
      }
    }

    public void AddBorrowerContactToList(int contactID)
    {
      if (!this.pnlBorrower.Visible)
        return;
      if (this.borrowerListForm == null)
        this.LoadBorrowerScreen();
      this.borrowerListForm.InsertContactIntoList(contactID);
    }

    private void CalendarListForm_CalendarSearchEvent(
      object sender,
      EllieMae.EMLite.Calendar.CalendarSearchControl.CalendarSearchEventArgs e)
    {
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "Committing changes to current calendar...");
      this.CalendarListForm.CommitChanges();
      if (this.frmAuxilliaryCalendar != null)
        this.frmAuxilliaryCalendar.GotoContact -= new EventHandler(this.Calendar_GotoContact);
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "Creating Calendar control for user '" + e.UserInfo.Userid + "' with access '" + (object) e.AccessLevel + "'...");
      this.frmAuxilliaryCalendar = frmCalendar.GetUserCalendar(e.UserInfo.Userid, e.AccessLevel);
      Tracing.Log(ContactMainForm.sw, nameof (ContactMainForm), TraceLevel.Info, "Attaching calendar form to UI...");
      this.frmAuxilliaryCalendar.TopLevel = false;
      this.frmAuxilliaryCalendar.Visible = true;
      this.frmAuxilliaryCalendar.Dock = DockStyle.Fill;
      this.frmAuxilliaryCalendar.CalendarSearchEvent += new EllieMae.EMLite.Calendar.CalendarSearchControl.CalendarSearchEventHandler(this.CalendarListForm_CalendarSearchEvent);
      this.frmAuxilliaryCalendar.GotoContact += new EventHandler(this.Calendar_GotoContact);
      this.panelRight.Controls.Clear();
      this.panelRight.Controls.Add((Control) this.frmAuxilliaryCalendar);
    }

    public void LoadBorrowerScreen()
    {
      if (this.borrowerListForm != null || !this.session.ACL.IsAuthorizedForFeature(AclFeature.Cnt_Borrower_Access))
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.borrowerListForm = new BorrowerListForm(this.session, this);
      this.borrowerListForm.TopLevel = false;
      this.borrowerListForm.Visible = true;
      this.borrowerListForm.Dock = DockStyle.Fill;
      Cursor.Current = Cursors.Default;
    }

    public void LoadBizPartnerScreen()
    {
      if (this.bizPartnerListForm != null || !this.session.ACL.IsAuthorizedForFeature(AclFeature.Cnt_Biz_Access))
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.bizPartnerListForm = new BizPartnerListForm(this);
      this.bizPartnerListForm.TopLevel = false;
      this.bizPartnerListForm.Visible = true;
      this.bizPartnerListForm.Dock = DockStyle.Fill;
      Cursor.Current = Cursors.Default;
    }

    public void LoadCampaignScreen()
    {
      if (this.frmCampaignDashboard != null || !this.session.ACL.IsAuthorizedForFeature(AclFeature.Cnt_Campaign_Access))
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.frmCampaignDashboard = new CampaignDashboardForm(this);
      this.frmCampaignDashboard.TopLevel = false;
      this.frmCampaignDashboard.Visible = true;
      this.frmCampaignDashboard.Dock = DockStyle.Fill;
      this.frmCampaignDashboard.LoadCampaignList();
      Cursor.Current = Cursors.Default;
    }

    public void LoadTasksScreen()
    {
      if (this.taskListForm != null)
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.taskListForm = new TaskListForm();
      this.taskListForm.TopLevel = false;
      this.taskListForm.Visible = true;
      this.taskListForm.Dock = DockStyle.Fill;
      Cursor.Current = Cursors.Default;
    }

    public void SaveContactChanges()
    {
      if (this.borrowerListForm != null)
        this.borrowerListForm.SaveChanges();
      if (this.bizPartnerListForm == null)
        return;
      this.bizPartnerListForm.SaveChanges();
    }

    public void RefreshContactData()
    {
      Cursor.Current = Cursors.WaitCursor;
      bool flag1 = false;
      if (this.pnlBorrower.Visible && this.borrowerListForm != null)
      {
        this.borrowerListForm.RefreshDataSet();
        flag1 = true;
      }
      else if (this.pnlBorrower.Visible && this.borrowerListForm == null && !flag1)
      {
        this.btnBorrower_Click((object) null, (EventArgs) null);
        flag1 = true;
      }
      bool flag2;
      if (this.pnlBizPartner.Visible && this.bizPartnerListForm != null)
      {
        this.bizPartnerListForm.RefreshDataSetWOSave();
        flag2 = true;
      }
      else if (this.pnlBizPartner.Visible && this.bizPartnerListForm == null && !flag1)
      {
        this.btnBizPartner_Click((object) null, (EventArgs) null);
        flag2 = true;
      }
      this.SetCampaignsButtonText(this.session.CampaignManager.GetTasksDueForUser(this.session.UserID));
      if (ContactMainForm.FormState.Campaign == this._FormState && this.frmCampaignDashboard != null)
        this.frmCampaignDashboard.RefreshCampaignData();
      Cursor.Current = Cursors.Default;
    }

    internal void SetCampaignsButtonText(int tasksDue)
    {
    }

    private void LoadBorrowerState()
    {
      this.LoadBorrowerScreen();
      this.borrowerListForm.RefreshHelpText();
      this.panelRight.Controls.Clear();
      this.panelRight.Controls.Add((Control) this.borrowerListForm);
    }

    private void LoadBizPartnerState()
    {
      this.LoadBizPartnerScreen();
      this.bizPartnerListForm.RefreshHelpText();
      this.panelRight.Controls.Clear();
      this.panelRight.Controls.Add((Control) this.bizPartnerListForm);
    }

    private void LoadCampaignDashboardState()
    {
      this.panelRight.Controls.Clear();
      if (this.frmCampaignDashboard == null)
        this.LoadCampaignScreen();
      else
        this.frmCampaignDashboard.RefreshCampaignData();
      this.panelRight.Controls.Add((Control) this.frmCampaignDashboard);
    }

    private void LoadCalendarState(bool ensureContactsLoaded)
    {
      UserInfo userInfo = Session.UserInfo;
      if (this.currentCalendarUserID != userInfo.Userid)
        userInfo = this.session.OrganizationManager.GetUser(this.currentCalendarUserID);
      this.CalendarListForm_CalendarSearchEvent((object) null, new EllieMae.EMLite.Calendar.CalendarSearchControl.CalendarSearchEventArgs(userInfo, this.currentCalendarUserAccessLevel));
      if (!ensureContactsLoaded || this.frmAuxilliaryCalendar == null)
        return;
      this.frmAuxilliaryCalendar.EnsureCalendarLoaded();
    }

    private void LoadTaskListState()
    {
      this.LoadTasksScreen();
      this.panelRight.Controls.Clear();
      this.panelRight.Controls.Add((Control) this.taskListForm);
      this.taskListForm.LoadTasks();
    }

    public void RefreshTaskData()
    {
      Cursor.Current = Cursors.WaitCursor;
      this.LoadTasksScreen();
      this.taskListForm.LoadTasks();
      Cursor.Current = Cursors.Default;
    }

    public string GetCurrentScreen()
    {
      switch (this._FormState)
      {
        case ContactMainForm.FormState.Borrower:
          return "BorrowerContacts";
        case ContactMainForm.FormState.BizContact:
          return "BusinessContacts";
        case ContactMainForm.FormState.Campaign:
          return "Campaign";
        case ContactMainForm.FormState.Calendar:
          return "Calendar";
        case ContactMainForm.FormState.Tasks:
          return "Tasks";
        case ContactMainForm.FormState.LeadCenter:
          return "LeadCenter";
        default:
          return (string) null;
      }
    }

    public bool SetCurrentScreen(string screenName)
    {
      if (!this.session.ACL.IsAuthorizedForFeature(AclFeature.GlobalTab_Contacts))
        return false;
      switch (screenName.ToLower())
      {
        case "borrowercontacts":
          if (!this.session.ACL.IsAuthorizedForFeature(AclFeature.Cnt_Borrower_Access))
            return false;
          this.ShowBorrowerContacts(true);
          break;
        case "businesscontacts":
          if (!this.session.ACL.IsAuthorizedForFeature(AclFeature.Cnt_Biz_Access))
            return false;
          this.ShowBizPartnerContacts(true);
          break;
        case "calendar":
          this.showCalendar();
          break;
        case "tasks":
          this.ShowTaskList();
          break;
        case "campaigns":
          if (!this.session.ACL.IsAuthorizedForFeature(AclFeature.Cnt_Campaign_Access))
            return false;
          this.ShowCampaignDashboard();
          break;
        default:
          return false;
      }
      return true;
    }

    private void ContactMainForm_Closed(object sender, EventArgs e)
    {
      if (this.CalendarListForm != null)
        this.CalendarListForm.Close();
      if (this.borrowerListForm != null)
        this.borrowerListForm.Close();
      if (this.bizPartnerListForm != null)
        this.bizPartnerListForm.Close();
      if (this.frmCampaignDashboard == null)
        return;
      this.frmCampaignDashboard.Close();
    }

    public string GetHelpTargetName()
    {
      if (this._FormState == ContactMainForm.FormState.Calendar)
        return "Calendar";
      if (this._FormState == ContactMainForm.FormState.LeadCenter)
        return ContactMainForm.FormState.LeadCenter.ToString();
      if (this._FormState == ContactMainForm.FormState.Tasks)
        return ContactMainForm.FormState.Tasks.ToString();
      return this._FormState == ContactMainForm.FormState.Campaign ? ContactMainForm.FormState.Campaign.ToString() : this.Name;
    }

    private void btnBorrower_Click(object sender, EventArgs e)
    {
      this.ShowBorrowerContacts(false);
      if (this.OnContactTabChange == null)
        return;
      this.OnContactTabChange(ContactMainForm.ContactsContentEnum.BorrowerContacts);
    }

    private void btnBizPartner_Click(object sender, EventArgs e)
    {
      this.ShowBizPartnerContacts(false);
      if (this.OnContactTabChange == null)
        return;
      this.OnContactTabChange(ContactMainForm.ContactsContentEnum.BusinessContacts);
    }

    private void btnCalendar_Click(object sender, EventArgs e)
    {
      this.showCalendar();
      if (this.OnContactTabChange == null)
        return;
      this.OnContactTabChange(ContactMainForm.ContactsContentEnum.Calendar);
    }

    private void btnCampaigns_Click(object sender, EventArgs e)
    {
      if (!((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Campaign_Access))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access to this feature. Contact your system administrator for additional information on Campaign Management.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.ShowCampaignDashboard();
        if (this.OnContactTabChange == null)
          return;
        this.OnContactTabChange(ContactMainForm.ContactsContentEnum.Campaigns);
      }
    }

    private void setButtonSelected(IconButton selectedButton, Image selectedImage)
    {
      this.btnBorrower.Image = (Image) Resources.btn_borrower_contacts;
      this.btnBizPartner.Image = (Image) Resources.btn_business_contacts;
      this.btnCampaigns.Image = (Image) Resources.btn_campaigns;
      this.btnCalendar.Image = (Image) Resources.btn_calendar;
      this.btnTasks.Image = (Image) Resources.btn_tasks;
      selectedButton.Image = selectedImage;
    }

    public void ShowBizPartnerContacts(bool refresh)
    {
      MetricsFactory.IncrementCounter("ContactsBusinessUsageCounter", (SFxTag) new SFxUiTag());
      this.setButtonSelected(this.btnBizPartner, (Image) Resources.btn_business_contacts_selected);
      this._FormState = ContactMainForm.FormState.BizContact;
      if (!refresh)
        return;
      this.bizPartnerListForm.RefreshDataSet();
    }

    public void ShowBorrowerContacts(bool refresh)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerUsageCounter", (SFxTag) new SFxUiTag());
      this.setButtonSelected(this.btnBorrower, (Image) Resources.btn_borrower_contacts_selected);
      this._FormState = ContactMainForm.FormState.Borrower;
      if (!refresh)
        return;
      this.borrowerListForm.RefreshDataSet();
    }

    public ContactMainForm.FormState CurrentFormState => this._FormState;

    private void showCalendar()
    {
      this.ShowCalendar((IWin32Window) this, this.session.UserID, CSMessage.AccessLevel.Full, false);
    }

    public void ShowCalendar(
      IWin32Window owner,
      string userID,
      CSMessage.AccessLevel accessLevel,
      bool accessUpdate)
    {
      MetricsFactory.IncrementCounter("ContactsCalendarUsageCounter", (SFxTag) new SFxUiTag());
      this.setButtonSelected(this.btnCalendar, (Image) Resources.btn_calendar_selected);
      if (this.currentCalendarUserID == userID & accessUpdate)
      {
        string text = "";
        switch (accessLevel)
        {
          case CSMessage.AccessLevel.ReadOnly:
            text = "This calendar’s owner has modified your access level. You now have ReadOnly permission.";
            break;
          case CSMessage.AccessLevel.Partial:
            text = "This calendar’s owner has modified your access level. You now have Partial permission.";
            break;
          case CSMessage.AccessLevel.Full:
            text = "This calendar’s owner has modified your access level. You now have Full permission.";
            break;
          case CSMessage.AccessLevel.Pending:
            text = "Your access to this calendar has been removed. Your calendar will be loaded.";
            break;
        }
        int num = (int) Utils.Dialog(owner, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        if (accessLevel == CSMessage.AccessLevel.Pending)
        {
          userID = this.session.UserID;
          accessLevel = CSMessage.AccessLevel.Full;
        }
        this.frmAuxilliaryCalendar.AccessLevel = accessLevel;
      }
      this.currentCalendarUserID = userID;
      this.currentCalendarUserAccessLevel = accessLevel;
      this._FormState = ContactMainForm.FormState.Calendar;
      this.session.Application.GetService<IStatusDisplay>().DisplayHelpText("Right click on an appointment for more options. Press F1 for further help.");
    }

    public bool IsCurrentCalendarOwner(string userID)
    {
      bool flag = false;
      if (this.currentCalendarUserID == userID)
        flag = true;
      return flag;
    }

    public void ShowCampaignDashboard()
    {
      MetricsFactory.IncrementCounter("ContactsCampaignsUsageCounter", (SFxTag) new SFxUiTag());
      this.session.Application.GetService<IStatusDisplay>().DisplayHelpText("Press F1 for Help");
      this.setButtonSelected(this.btnCampaigns, (Image) Resources.btn_campaigns_selected);
      this._FormState = ContactMainForm.FormState.Campaign;
    }

    private void btnTasks_Click(object sender, EventArgs e)
    {
      this.ShowTaskList();
      if (this.OnContactTabChange == null)
        return;
      this.OnContactTabChange(ContactMainForm.ContactsContentEnum.Tasks);
    }

    public void ShowTaskList()
    {
      MetricsFactory.IncrementCounter("ContactsTasksUsageCounter", (SFxTag) new SFxUiTag());
      this.setButtonSelected(this.btnTasks, (Image) Resources.btn_tasks_selected);
      this._FormState = ContactMainForm.FormState.Tasks;
      this.session.Application.GetService<IStatusDisplay>().DisplayHelpText("Right click on a task for more options. Press F1 for further help.");
    }

    private ContactMainForm.FormState _FormState
    {
      get => this.formState;
      set
      {
        if (this.formState == ContactMainForm.FormState.Borrower && this.borrowerListForm != null)
          this.borrowerListForm.SaveChanges();
        else if (this.formState == ContactMainForm.FormState.BizContact && this.bizPartnerListForm != null)
          this.bizPartnerListForm.SaveChanges();
        switch (value)
        {
          case ContactMainForm.FormState.Borrower:
            this.LoadBorrowerState();
            break;
          case ContactMainForm.FormState.BizContact:
            this.LoadBizPartnerState();
            break;
          case ContactMainForm.FormState.Campaign:
            this.LoadCampaignDashboardState();
            break;
          case ContactMainForm.FormState.Calendar:
            this.LoadCalendarState(true);
            break;
          case ContactMainForm.FormState.Tasks:
            this.LoadTaskListState();
            break;
        }
        this.formState = value;
      }
    }

    public void ContactMenu_Click(string item)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(item))
      {
        case 366520832:
          if (!(item == "Borrower Contacts"))
            break;
          this.btnBorrower_Click((object) null, (EventArgs) null);
          break;
        case 662021555:
          if (!(item == "Import Business Contacts"))
            break;
          this.LoadBizPartnerScreen();
          this.bizPartnerListForm.ImportContacts();
          break;
        case 966695021:
          if (!(item == "Tasks"))
            break;
          this.btnTasks_Click((object) null, (EventArgs) null);
          break;
        case 1495791840:
          if (!(item == "Campaigns"))
            break;
          this.btnCampaigns_Click((object) null, (EventArgs) null);
          break;
        case 2656199999:
          if (!(item == "Calendar"))
            break;
          this.btnCalendar_Click((object) null, (EventArgs) null);
          break;
        case 3274649836:
          if (!(item == "Business Contacts"))
            break;
          this.btnBizPartner_Click((object) null, (EventArgs) null);
          break;
        case 3657510877:
          if (!(item == "Synchronize"))
            break;
          if (!((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Synchronization))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary rights to use Contact Synchronization. Contact your system administrator for additional information.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          this.OpenSynchronization();
          break;
        case 4261748011:
          if (!(item == "Import Borrower Contacts"))
            break;
          this.borrowerListForm.ImportContacts();
          break;
      }
    }

    public bool IsMenuItemEnabled(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          if (this.borrowerListForm != null && this._FormState == ContactMainForm.FormState.Borrower)
          {
            flag = this.borrowerListForm.IsMenuItemEnabled(ContactMainForm.ContactsActionEnum.Synchronization);
            break;
          }
          if (this.bizPartnerListForm != null && this._FormState == ContactMainForm.FormState.BizContact)
          {
            flag = this.bizPartnerListForm.IsMenuItemEnabled(ContactMainForm.ContactsActionEnum.Synchronization);
            break;
          }
          if (this.CalendarListForm != null && this._FormState == ContactMainForm.FormState.Calendar)
          {
            flag = true;
            break;
          }
          if (this.taskListForm != null && this._FormState == ContactMainForm.FormState.Tasks)
          {
            flag = true;
            break;
          }
          break;
        case ContactMainForm.ContactsActionEnum.HomePoints:
        case ContactMainForm.ContactsActionEnum.Borrower_NewContact:
        case ContactMainForm.ContactsActionEnum.Borrower_DuplicateContact:
        case ContactMainForm.ContactsActionEnum.Borrower_DeleteContact:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintAllDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMerge:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeAll:
        case ContactMainForm.ContactsActionEnum.Borrower_AddToGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_RemoveFromGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_EditGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_OriginateLoan:
        case ContactMainForm.ContactsActionEnum.Borrower_OrderCredit:
        case ContactMainForm.ContactsActionEnum.Borrower_ProductPricing:
        case ContactMainForm.ContactsActionEnum.Borrower_BuyLead:
        case ContactMainForm.ContactsActionEnum.Borrower_ImportLead:
        case ContactMainForm.ContactsActionEnum.Borrower_ImportContact:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExport:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportSelected:
        case ContactMainForm.ContactsActionEnum.Borrower_Reassign:
        case ContactMainForm.ContactsActionEnum.Borrower_CustomizeColumns:
        case ContactMainForm.ContactsActionEnum.Borrower_SaveView:
        case ContactMainForm.ContactsActionEnum.Borrower_ResetView:
        case ContactMainForm.ContactsActionEnum.Borrower_ManageView:
          flag = this.borrowerListForm.IsMenuItemEnabled(action);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_NewContact:
        case ContactMainForm.ContactsActionEnum.Biz_DuplicateContact:
        case ContactMainForm.ContactsActionEnum.Biz_DeleteContact:
        case ContactMainForm.ContactsActionEnum.Biz_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Biz_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Biz_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Biz_PrintDetails:
        case ContactMainForm.ContactsActionEnum.Biz_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Biz_PrintAllDetails:
        case ContactMainForm.ContactsActionEnum.Biz_MailMerge:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeAll:
        case ContactMainForm.ContactsActionEnum.Biz_AddToGroup:
        case ContactMainForm.ContactsActionEnum.Biz_RemoveFromGroup:
        case ContactMainForm.ContactsActionEnum.Biz_EditGroup:
        case ContactMainForm.ContactsActionEnum.Biz_ImportContact:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExport:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportSelected:
        case ContactMainForm.ContactsActionEnum.Biz_CustomizeColumns:
        case ContactMainForm.ContactsActionEnum.Biz_SaveView:
        case ContactMainForm.ContactsActionEnum.Biz_ResetView:
        case ContactMainForm.ContactsActionEnum.Biz_ManageView:
          flag = this.bizPartnerListForm.IsMenuItemEnabled(action);
          break;
        case ContactMainForm.ContactsActionEnum.Cal_NewAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_EditAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_DeleteAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_Print:
        case ContactMainForm.ContactsActionEnum.Cal_View:
        case ContactMainForm.ContactsActionEnum.Cal_Today:
        case ContactMainForm.ContactsActionEnum.Cal_1Day:
        case ContactMainForm.ContactsActionEnum.Cal_WorkDay:
        case ContactMainForm.ContactsActionEnum.Cal_Week:
        case ContactMainForm.ContactsActionEnum.Cal_Month:
          flag = this.CalendarListForm.IsMenuItemEnabled(this.translateToCalendarAction(action));
          break;
        case ContactMainForm.ContactsActionEnum.Task_NewTask:
        case ContactMainForm.ContactsActionEnum.Task_EditTask:
        case ContactMainForm.ContactsActionEnum.Task_DeleteTask:
        case ContactMainForm.ContactsActionEnum.Task_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Task_Status:
        case ContactMainForm.ContactsActionEnum.Task_Status_NotStarted:
        case ContactMainForm.ContactsActionEnum.Task_Status_InProgress:
        case ContactMainForm.ContactsActionEnum.Task_Status_Completed:
        case ContactMainForm.ContactsActionEnum.Task_Status_WaitOnSomeoneElse:
        case ContactMainForm.ContactsActionEnum.Task_Status_Deferred:
          flag = this.taskListForm.IsMenuItemEnabled(action);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          flag = this.frmCampaignDashboard.IsMenuItemEnabled(action);
          break;
      }
      return flag;
    }

    public bool IsMenuItemVisible(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          if (this.borrowerListForm != null && this._FormState == ContactMainForm.FormState.Borrower)
          {
            flag = this.borrowerListForm.IsMenuItemVisible(ContactMainForm.ContactsActionEnum.Synchronization);
            break;
          }
          if (this.bizPartnerListForm != null && this._FormState == ContactMainForm.FormState.BizContact)
          {
            flag = this.bizPartnerListForm.IsMenuItemVisible(ContactMainForm.ContactsActionEnum.Synchronization);
            break;
          }
          if (this.CalendarListForm != null && this._FormState == ContactMainForm.FormState.Calendar)
          {
            this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
            flag = this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Synchronization);
            break;
          }
          if (this.taskListForm != null && this._FormState == ContactMainForm.FormState.Tasks)
          {
            this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
            flag = this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Synchronization);
            break;
          }
          break;
        case ContactMainForm.ContactsActionEnum.HomePoints:
        case ContactMainForm.ContactsActionEnum.Borrower_NewContact:
        case ContactMainForm.ContactsActionEnum.Borrower_DuplicateContact:
        case ContactMainForm.ContactsActionEnum.Borrower_DeleteContact:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintAllDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMerge:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeAll:
        case ContactMainForm.ContactsActionEnum.Borrower_AddToGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_RemoveFromGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_EditGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_OriginateLoan:
        case ContactMainForm.ContactsActionEnum.Borrower_OrderCredit:
        case ContactMainForm.ContactsActionEnum.Borrower_ProductPricing:
        case ContactMainForm.ContactsActionEnum.Borrower_BuyLead:
        case ContactMainForm.ContactsActionEnum.Borrower_ImportLead:
        case ContactMainForm.ContactsActionEnum.Borrower_ImportContact:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExport:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportSelected:
        case ContactMainForm.ContactsActionEnum.Borrower_Reassign:
        case ContactMainForm.ContactsActionEnum.Borrower_CustomizeColumns:
        case ContactMainForm.ContactsActionEnum.Borrower_SaveView:
        case ContactMainForm.ContactsActionEnum.Borrower_ResetView:
        case ContactMainForm.ContactsActionEnum.Borrower_ManageView:
          flag = this.borrowerListForm.IsMenuItemVisible(action);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_Access:
          flag = this.pnlBorrower.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_Access:
          flag = this.pnlBizPartner.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_NewContact:
        case ContactMainForm.ContactsActionEnum.Biz_DuplicateContact:
        case ContactMainForm.ContactsActionEnum.Biz_DeleteContact:
        case ContactMainForm.ContactsActionEnum.Biz_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Biz_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Biz_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Biz_PrintDetails:
        case ContactMainForm.ContactsActionEnum.Biz_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Biz_PrintAllDetails:
        case ContactMainForm.ContactsActionEnum.Biz_MailMerge:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeAll:
        case ContactMainForm.ContactsActionEnum.Biz_AddToGroup:
        case ContactMainForm.ContactsActionEnum.Biz_RemoveFromGroup:
        case ContactMainForm.ContactsActionEnum.Biz_EditGroup:
        case ContactMainForm.ContactsActionEnum.Biz_ImportContact:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExport:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportSelected:
        case ContactMainForm.ContactsActionEnum.Biz_CustomizeColumns:
        case ContactMainForm.ContactsActionEnum.Biz_SaveView:
        case ContactMainForm.ContactsActionEnum.Biz_ResetView:
        case ContactMainForm.ContactsActionEnum.Biz_ManageView:
          flag = this.bizPartnerListForm.IsMenuItemVisible(action);
          break;
        case ContactMainForm.ContactsActionEnum.Cal_NewAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_EditAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_DeleteAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_Print:
        case ContactMainForm.ContactsActionEnum.Cal_View:
        case ContactMainForm.ContactsActionEnum.Cal_Today:
        case ContactMainForm.ContactsActionEnum.Cal_1Day:
        case ContactMainForm.ContactsActionEnum.Cal_WorkDay:
        case ContactMainForm.ContactsActionEnum.Cal_Week:
        case ContactMainForm.ContactsActionEnum.Cal_Month:
          flag = this.CalendarListForm.IsMenuItemVisible(this.translateToCalendarAction(action));
          break;
        case ContactMainForm.ContactsActionEnum.Task_NewTask:
        case ContactMainForm.ContactsActionEnum.Task_EditTask:
        case ContactMainForm.ContactsActionEnum.Task_DeleteTask:
        case ContactMainForm.ContactsActionEnum.Task_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Task_Status:
        case ContactMainForm.ContactsActionEnum.Task_Status_NotStarted:
        case ContactMainForm.ContactsActionEnum.Task_Status_InProgress:
        case ContactMainForm.ContactsActionEnum.Task_Status_Completed:
        case ContactMainForm.ContactsActionEnum.Task_Status_WaitOnSomeoneElse:
        case ContactMainForm.ContactsActionEnum.Task_Status_Deferred:
          flag = this.taskListForm.IsMenuItemVisible(action);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_Access:
          flag = this.pnlCampaign.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          flag = this.frmCampaignDashboard.IsMenuItemVisible(action);
          break;
      }
      return flag;
    }

    private CalendarActionEnum translateToCalendarAction(ContactMainForm.ContactsActionEnum action)
    {
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Cal_NewAppointment:
          return CalendarActionEnum.Cal_NewAppointment;
        case ContactMainForm.ContactsActionEnum.Cal_EditAppointment:
          return CalendarActionEnum.Cal_EditAppointment;
        case ContactMainForm.ContactsActionEnum.Cal_DeleteAppointment:
          return CalendarActionEnum.Cal_DeleteAppointment;
        case ContactMainForm.ContactsActionEnum.Cal_Print:
          return CalendarActionEnum.Cal_Print;
        case ContactMainForm.ContactsActionEnum.Cal_View:
          return CalendarActionEnum.Cal_View;
        case ContactMainForm.ContactsActionEnum.Cal_Today:
          return CalendarActionEnum.Cal_Today;
        case ContactMainForm.ContactsActionEnum.Cal_1Day:
          return CalendarActionEnum.Cal_1Day;
        case ContactMainForm.ContactsActionEnum.Cal_WorkDay:
          return CalendarActionEnum.Cal_WorkDay;
        case ContactMainForm.ContactsActionEnum.Cal_Week:
          return CalendarActionEnum.Cal_Week;
        case ContactMainForm.ContactsActionEnum.Cal_Month:
          return CalendarActionEnum.Cal_Month;
        default:
          throw new Exception("");
      }
    }

    public void ContactMenu_Click(ContactMainForm.ContactsActionEnum action)
    {
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          if (!((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Synchronization))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary rights to use Contact Synchronization. Contact your system administrator for additional information.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          this.OpenSynchronization();
          break;
        case ContactMainForm.ContactsActionEnum.HomePoints:
          this.borrowerListForm.NavigateCustomerLoyalty((string) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_NewContact:
        case ContactMainForm.ContactsActionEnum.Borrower_DuplicateContact:
        case ContactMainForm.ContactsActionEnum.Borrower_DeleteContact:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintAllDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeAll:
        case ContactMainForm.ContactsActionEnum.Borrower_AddToGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_RemoveFromGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_EditGroup:
        case ContactMainForm.ContactsActionEnum.Borrower_OriginateLoan:
        case ContactMainForm.ContactsActionEnum.Borrower_OrderCredit:
        case ContactMainForm.ContactsActionEnum.Borrower_ProductPricing:
        case ContactMainForm.ContactsActionEnum.Borrower_BuyLead:
        case ContactMainForm.ContactsActionEnum.Borrower_ImportLead:
        case ContactMainForm.ContactsActionEnum.Borrower_ImportContact:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportSelected:
        case ContactMainForm.ContactsActionEnum.Borrower_Reassign:
        case ContactMainForm.ContactsActionEnum.Borrower_CustomizeColumns:
        case ContactMainForm.ContactsActionEnum.Borrower_SaveView:
        case ContactMainForm.ContactsActionEnum.Borrower_ResetView:
        case ContactMainForm.ContactsActionEnum.Borrower_ManageView:
          this.borrowerListForm.TriggerContactAction(action);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_NewContact:
        case ContactMainForm.ContactsActionEnum.Biz_DuplicateContact:
        case ContactMainForm.ContactsActionEnum.Biz_DeleteContact:
        case ContactMainForm.ContactsActionEnum.Biz_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Biz_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Biz_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Biz_PrintAllDetails:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeAll:
        case ContactMainForm.ContactsActionEnum.Biz_AddToGroup:
        case ContactMainForm.ContactsActionEnum.Biz_RemoveFromGroup:
        case ContactMainForm.ContactsActionEnum.Biz_EditGroup:
        case ContactMainForm.ContactsActionEnum.Biz_ImportContact:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportSelected:
        case ContactMainForm.ContactsActionEnum.Biz_CustomizeColumns:
        case ContactMainForm.ContactsActionEnum.Biz_SaveView:
        case ContactMainForm.ContactsActionEnum.Biz_ResetView:
        case ContactMainForm.ContactsActionEnum.Biz_ManageView:
          this.bizPartnerListForm.TriggerContactAction(action);
          break;
        case ContactMainForm.ContactsActionEnum.Cal_NewAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_EditAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_DeleteAppointment:
        case ContactMainForm.ContactsActionEnum.Cal_Print:
        case ContactMainForm.ContactsActionEnum.Cal_View:
        case ContactMainForm.ContactsActionEnum.Cal_Today:
        case ContactMainForm.ContactsActionEnum.Cal_1Day:
        case ContactMainForm.ContactsActionEnum.Cal_WorkDay:
        case ContactMainForm.ContactsActionEnum.Cal_Week:
        case ContactMainForm.ContactsActionEnum.Cal_Month:
          this.CalendarListForm.TriggerContactAction(this.translateToCalendarAction(action));
          break;
        case ContactMainForm.ContactsActionEnum.Task_NewTask:
        case ContactMainForm.ContactsActionEnum.Task_EditTask:
        case ContactMainForm.ContactsActionEnum.Task_DeleteTask:
        case ContactMainForm.ContactsActionEnum.Task_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Task_Status:
        case ContactMainForm.ContactsActionEnum.Task_Status_NotStarted:
        case ContactMainForm.ContactsActionEnum.Task_Status_InProgress:
        case ContactMainForm.ContactsActionEnum.Task_Status_Completed:
        case ContactMainForm.ContactsActionEnum.Task_Status_WaitOnSomeoneElse:
        case ContactMainForm.ContactsActionEnum.Task_Status_Deferred:
          this.taskListForm.TriggerContactAction(action);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          this.frmCampaignDashboard.TriggerContactAction(action);
          break;
        default:
          int num1 = (int) Utils.Dialog((IWin32Window) this, "No matching contact action can be found.");
          break;
      }
    }

    private void OpenSynchronization()
    {
      this.SaveContactChanges();
      this.CalendarListForm.CommitChanges();
      using (SyncWizard syncWizard = new SyncWizard())
      {
        if (syncWizard.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.RefreshContactData();
        this.RefreshTaskData();
      }
    }

    public void Print()
    {
      switch (this._FormState)
      {
        case ContactMainForm.FormState.Borrower:
          this.borrowerListForm.PrintContactBorrower(false);
          break;
        case ContactMainForm.FormState.BizContact:
          this.bizPartnerListForm.PrintContactBusiness(false);
          break;
        case ContactMainForm.FormState.Calendar:
          this.CalendarListForm.PrintCalendar();
          break;
      }
    }

    private void ContactMainForm_SizeChanged(object sender, EventArgs e)
    {
    }

    protected override void OnResize(EventArgs e)
    {
      IntPtr handle = this.Handle;
      this.BeginInvoke((Delegate) new MethodInvoker(this.ChangeSize));
      base.OnResize(e);
    }

    protected void ChangeSize()
    {
      foreach (Control control in (ArrangedElementCollection) this.Controls)
        control.Size = new Size(control.Width - 1, control.Height - 1);
    }

    public delegate void ContactTabChanged(ContactMainForm.ContactsContentEnum tab);

    public enum ContactsContentEnum
    {
      BorrowerContacts,
      BusinessContacts,
      Calendar,
      Tasks,
      Campaigns,
    }

    public enum ContactsActionEnum
    {
      Synchronization,
      HomePoints,
      Borrower_Access,
      Borrower_NewContact,
      Borrower_DuplicateContact,
      Borrower_DeleteContact,
      Borrower_ExportExcel,
      Borrower_ExportSelectedExcel,
      Borrower_ExportAllExcel,
      Borrower_PrintDetails,
      Borrower_PrintSelectedDetails,
      Borrower_PrintAllDetails,
      Borrower_MailMerge,
      Borrower_MailMergeSelected,
      Borrower_MailMergeAll,
      Borrower_AddToGroup,
      Borrower_RemoveFromGroup,
      Borrower_EditGroup,
      Borrower_OriginateLoan,
      Borrower_OrderCredit,
      Borrower_ProductPricing,
      Borrower_BuyLead,
      Borrower_ImportLead,
      Borrower_ImportContact,
      Borrower_CSVExport,
      Borrower_CSVExportAll,
      Borrower_CSVExportSelected,
      Borrower_Reassign,
      Borrower_CustomizeColumns,
      Borrower_SaveView,
      Borrower_ResetView,
      Borrower_ManageView,
      Biz_Access,
      Biz_NewContact,
      Biz_DuplicateContact,
      Biz_DeleteContact,
      Biz_ExportExcel,
      Biz_ExportSelectedExcel,
      Biz_ExportAllExcel,
      Biz_PrintDetails,
      Biz_PrintSelectedDetails,
      Biz_PrintAllDetails,
      Biz_MailMerge,
      Biz_MailMergeSelected,
      Biz_MailMergeAll,
      Biz_AddToGroup,
      Biz_RemoveFromGroup,
      Biz_EditGroup,
      Biz_ImportContact,
      Biz_CSVExport,
      Biz_CSVExportAll,
      Biz_CSVExportSelected,
      Biz_CustomizeColumns,
      Biz_SaveView,
      Biz_ResetView,
      Biz_ManageView,
      Cal_NewAppointment,
      Cal_EditAppointment,
      Cal_DeleteAppointment,
      Cal_Print,
      Cal_View,
      Cal_Today,
      Cal_1Day,
      Cal_WorkDay,
      Cal_Week,
      Cal_Month,
      Task_NewTask,
      Task_EditTask,
      Task_DeleteTask,
      Task_ExportExcel,
      Task_Status,
      Task_Status_NotStarted,
      Task_Status_InProgress,
      Task_Status_Completed,
      Task_Status_WaitOnSomeoneElse,
      Task_Status_Deferred,
      Campaign_Access,
      Campaign_NewCampaign,
      Campaign_OpenCampaign,
      Campaign_DuplicateCampaign,
      Campaign_DeleteCampaign,
      Campaign_StartCampaign,
      Campaign_StopCampaign,
      Campaign_ManageTemplate,
    }

    public enum FormState
    {
      None,
      Borrower,
      BizContact,
      Campaign,
      Calendar,
      Tasks,
      LeadCenter,
    }
  }
}
