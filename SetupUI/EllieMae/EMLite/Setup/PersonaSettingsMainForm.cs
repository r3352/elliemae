// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSettingsMainForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.HomePage;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.Setup.BrokerPersona;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonaSettingsMainForm : Form
  {
    private IWin32Window owner;
    private EventHandler dirtyFlagChangedEventHandler;
    private TabControl tabControl1;
    private TabPage tabPageHome;
    private TabPage tabPagePipeline;
    private TabPage tabPageLoans;
    private TabPage tabPageFormsTools;
    private TabPage tabPageContacts;
    private TabPage tabPageSettings;
    private EllieMae.EMLite.Setup.HomePage homePage;
    private PipelineConfiguration pipelinePage;
    private LoansPage loansPage;
    private FormsToolsConfig formsToolsPage;
    private LOConnectConfig loConnectPage;
    private ContactsPage contactsPage;
    private SettingsPage settingsPage;
    private ExtSettingsPage externalSettingsPage;
    private TPOAdminPage tpoAdminPage;
    private AccessPage brokerAccessPage;
    private AccessConfig accessConfigPage;
    private eFolderPage eFolderPage;
    private EnhancedConditionsPage enhancedConditionsPage;
    private CCAdminPage ccAdminPage;
    private AIQPersonaSettingPage AIQPage;
    private EVaultPage eVaultPage;
    private DeveloperConnectPage developerConnectPage;
    private IPersonaSecurityPage[] subpages;
    private int currentPersonaId = -1;
    private string userId = string.Empty;
    private bool skipChecking;
    private Persona[] personas;
    private CheckBox cxbModify;
    private bool internalChange;
    private UserInfo currentUser;
    private TabPage tabPageExtSettings;
    private TabPage tabPageTPOAdministration;
    private TabPage tabPageConsumerConnect;
    private TabPage tabPageEnhancedConditions;
    private TabPage tabPageLOConnect;
    private TabPage tabPageAIQ;
    private TabPage tabPageEVault;
    private TabPage tabPageDeveloperConnect;
    private bool personal;
    private string personaName = "";
    private GroupContainer gcPersona;
    private StandardIconButton stdIconBtnSave;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnReset;
    private PanelEx pnlExBottom;
    private Button btnClose;
    protected Label lblDisconnectedFromPersona;
    protected Label lblLinkedWithPersona;
    private IContainer components;
    private bool suspendEvent;
    private TabPage tabPageBrokerAccess;
    private bool requirePipelineLoanReadOnly;
    private TabPage tabPageeFolder;
    private FeaturesAclManager aclMgr;
    private Sessions.Session session;
    private TabPage tabPageAccess;
    private bool showTabPageHome = true;

    private bool isBanker => this.session.EncompassEdition == EncompassEdition.Banker;

    public int CurrentPersonaID
    {
      get => this.currentPersonaId;
      set
      {
        if (this.currentPersonaId == value && !this.IsDirty())
          return;
        this.suspendEvent = true;
        this.currentPersonaId = value;
        if (this.subpages == null)
          return;
        if (this.isBanker)
        {
          if (this.subpages[2] != null)
            this.subpages[2].SetPersona(value);
          if (this.session.StartupInfo.EnhancedConditionSettings)
          {
            if (this.subpages[7] != null)
              this.subpages[7].SetPersona(value);
          }
          else if (this.subpages[6] != null)
            this.subpages[6].SetPersona(value);
          if (!this.refreshTabPage(this.tabPageAccess, 0, value))
            this.accessConfigPage = (AccessConfig) null;
          if (!this.refreshTabPage(this.tabPageHome, 1, value))
            this.homePage = (EllieMae.EMLite.Setup.HomePage) null;
          if (!this.refreshTabPage(this.tabPageLoans, 3, value))
            this.loansPage = (LoansPage) null;
          if (!this.refreshTabPage(this.tabPageFormsTools, 4, value))
            this.formsToolsPage = (FormsToolsConfig) null;
          if (!this.refreshTabPage(this.tabPageeFolder, 5, value))
            this.eFolderPage = (eFolderPage) null;
          if (this.session.StartupInfo.EnhancedConditionSettings)
          {
            if (!this.refreshTabPage(this.tabPageEnhancedConditions, 6, value))
              this.enhancedConditionsPage = (EnhancedConditionsPage) null;
            if (!this.refreshTabPage(this.tabPageSettings, 8, value))
              this.settingsPage = (SettingsPage) null;
            if (!this.refreshTabPage(this.tabPageExtSettings, 9, value))
              this.externalSettingsPage = (ExtSettingsPage) null;
            if (!this.refreshTabPage(this.tabPageTPOAdministration, 10, value))
              this.tpoAdminPage = (TPOAdminPage) null;
            if (!this.refreshTabPage(this.tabPageConsumerConnect, 11, value))
              this.ccAdminPage = (CCAdminPage) null;
            if (!this.refreshTabPage(this.tabPageEVault, 12, value))
              this.eVaultPage = (EVaultPage) null;
            if (!this.refreshTabPage(this.tabPageLOConnect, 13, value))
              this.loConnectPage = (LOConnectConfig) null;
            if (!this.refreshTabPage(this.tabPageAIQ, 14, value))
              this.AIQPage = (AIQPersonaSettingPage) null;
            if (!this.refreshTabPage(this.tabPageDeveloperConnect, 15, value))
              this.developerConnectPage = (DeveloperConnectPage) null;
          }
          else
          {
            if (!this.refreshTabPage(this.tabPageSettings, 7, value))
              this.settingsPage = (SettingsPage) null;
            if (!this.refreshTabPage(this.tabPageExtSettings, 8, value))
              this.externalSettingsPage = (ExtSettingsPage) null;
            if (!this.refreshTabPage(this.tabPageTPOAdministration, 9, value))
              this.tpoAdminPage = (TPOAdminPage) null;
            if (!this.refreshTabPage(this.tabPageConsumerConnect, 10, value))
              this.ccAdminPage = (CCAdminPage) null;
            if (!this.refreshTabPage(this.tabPageEVault, 11, value))
              this.eVaultPage = (EVaultPage) null;
            if (!this.refreshTabPage(this.tabPageLOConnect, 12, value))
              this.loConnectPage = (LOConnectConfig) null;
            if (!this.refreshTabPage(this.tabPageAIQ, 13, value))
              this.AIQPage = (AIQPersonaSettingPage) null;
            if (!this.refreshTabPage(this.tabPageDeveloperConnect, 14, value))
              this.developerConnectPage = (DeveloperConnectPage) null;
          }
        }
        else
        {
          if (this.showTabPageHome)
          {
            if (this.tabControl1.SelectedTab == this.tabPageHome)
            {
              if (this.subpages[this.tabControl1.SelectedIndex] == null)
                this.subpages[this.tabControl1.SelectedIndex] = (IPersonaSecurityPage) this.homePage;
              this.subpages[this.tabControl1.SelectedIndex].SetPersona(value);
            }
            else if (!this.refreshTabPage(this.tabPageHome, 0, value))
              this.homePage = (EllieMae.EMLite.Setup.HomePage) null;
          }
          if (!this.refreshTabPage(this.tabPageBrokerAccess, 1, value))
            this.brokerAccessPage = (AccessPage) null;
          if (!this.refreshTabPage(this.tabPageConsumerConnect, 2, value))
            this.ccAdminPage = (CCAdminPage) null;
        }
        this.suspendEvent = false;
      }
    }

    private bool refreshTabPage(TabPage checkPage, int index, int personaId)
    {
      bool flag = false;
      if (this.tabControl1.SelectedTab == checkPage)
      {
        if (this.subpages[index] != null)
          this.subpages[index].SetPersona(personaId);
        flag = true;
      }
      else
      {
        checkPage.Controls.Clear();
        this.subpages[index] = (IPersonaSecurityPage) null;
      }
      return flag;
    }

    public string CurrentPersonaName
    {
      set => this.personaName = value;
    }

    public bool SkipChecking
    {
      get => this.skipChecking;
      set => this.skipChecking = value;
    }

    public PersonaSettingsMainForm(
      IWin32Window owner,
      Sessions.Session session,
      string userId,
      Persona[] personas)
    {
      this.InitializeComponent();
      this.showTabPageHome = Session.DefaultInstance != null;
      if (!this.showTabPageHome)
        this.tabControl1.TabPages.Remove(this.tabPageHome);
      this.session = session;
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.init(owner);
      this.applyEncompassEdition();
      this.personal = true;
      this.userId = userId;
      this.personas = personas;
      this.loadTabPagesForUser();
      bool flag = false;
      this.cxbModify.Visible = true;
      foreach (Persona persona in personas)
      {
        if (persona.ID == 1)
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        this.cxbModify.Enabled = false;
        this.forcedReadOnly();
      }
      else
        this.loadModificationSetting();
      this.pnlExBottom.BackColor = EncompassColors.Neutral3;
      if (this.isBanker)
      {
        this.contactsPage = new ContactsPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler);
        this.contactsPage.CreateLoanStatusChanged += new ContactsPage.OriginateLoanFeatureStatusChanged(this.contactsPage_CreateLoanStatusChanged);
        this.contactsPage.HasPipelineLoanTabAccessEvent += new ContactsPage.PersonaAccess(this.contactsPage_HasPipelineLoanTabAccessEvent);
        this.contactsPage.TopLevel = false;
        this.contactsPage.Visible = true;
        this.contactsPage.Dock = DockStyle.Fill;
        this.contactsPage.BackColor = this.BackColor;
        this.tabPageContacts.Controls.Add((Control) this.contactsPage);
        if (this.session.StartupInfo.EnhancedConditionSettings)
          this.subpages[7] = (IPersonaSecurityPage) this.contactsPage;
        else
          this.subpages[6] = (IPersonaSecurityPage) this.contactsPage;
      }
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonasEdit) && !this.session.UserInfo.IsSuperAdministrator())
      {
        this.stdIconBtnSave.Enabled = false;
        this.stdIconBtnReset.Enabled = false;
        this.cxbModify.Enabled = false;
      }
      this.displayTPOAdministrationTab(session);
      this.tabControl1.SelectedTab = this.tabControl1.TabPages.Contains(this.tabPageAccess) ? (!this.tabControl1.TabPages.Contains(this.tabPageAccess) ? this.tabPageBrokerAccess : this.tabPageAccess) : (!this.tabControl1.TabPages.Contains(this.tabPagePipeline) ? this.tabPageBrokerAccess : this.tabPagePipeline);
      this.tabControl1_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void displayTPOAdministrationTab(Sessions.Session session)
    {
      IBam bam = (IBam) new Bam((HomePageControl) null, this.session);
      if (session.ConfigurationManager.CheckIfAnyTPOSiteExists())
        return;
      this.tabControl1.TabPages.Remove(this.tabPageTPOAdministration);
    }

    private void applyEncompassEdition()
    {
      if (this.isBanker)
      {
        this.tabControl1.TabPages.Remove(this.tabPageBrokerAccess);
      }
      else
      {
        this.tabControl1.TabPages.Clear();
        if (this.showTabPageHome)
          this.tabControl1.TabPages.Add(this.tabPageHome);
        this.tabControl1.TabPages.Add(this.tabPageBrokerAccess);
        this.tabControl1.TabPages.Add(this.tabPageConsumerConnect);
      }
    }

    private bool contactsPage_HasPipelineLoanTabAccessEvent()
    {
      return this.pipelinePage.HasPipelineLoanTabAccess();
    }

    public PersonaSettingsMainForm(IWin32Window owner, Sessions.Session session, int personaId)
    {
      this.session = session;
      this.InitializeComponent();
      this.showTabPageHome = Session.DefaultInstance != null;
      if (!this.showTabPageHome)
        this.tabControl1.TabPages.Remove(this.tabPageHome);
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.init(owner);
      this.applyEncompassEdition();
      this.currentPersonaId = personaId;
      this.loadTabPagesForPersona();
      this.cxbModify.Visible = false;
      this.pnlExBottom.Visible = false;
      if (this.isBanker)
      {
        this.contactsPage = new ContactsPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
        this.contactsPage.CreateLoanStatusChanged += new ContactsPage.OriginateLoanFeatureStatusChanged(this.contactsPage_CreateLoanStatusChanged);
        this.contactsPage.HasPipelineLoanTabAccessEvent += new ContactsPage.PersonaAccess(this.contactsPage_HasPipelineLoanTabAccessEvent);
        this.contactsPage.TopLevel = false;
        this.contactsPage.Visible = true;
        this.contactsPage.Dock = DockStyle.Fill;
        this.contactsPage.BackColor = this.BackColor;
        this.tabPageContacts.Controls.Add((Control) this.contactsPage);
        if (this.session.StartupInfo.EnhancedConditionSettings)
          this.subpages[7] = (IPersonaSecurityPage) this.contactsPage;
        else
          this.subpages[6] = (IPersonaSecurityPage) this.contactsPage;
      }
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonasEdit) && !this.session.UserInfo.IsSuperAdministrator())
      {
        this.stdIconBtnSave.Enabled = false;
        this.stdIconBtnReset.Enabled = false;
        this.cxbModify.Enabled = false;
      }
      this.displayTPOAdministrationTab(session);
      this.tabControl1.SelectedTab = this.tabControl1.TabPages.Contains(this.tabPageAccess) ? (!this.tabControl1.TabPages.Contains(this.tabPageAccess) ? this.tabPageBrokerAccess : this.tabPageAccess) : (!this.tabControl1.TabPages.Contains(this.tabPagePipeline) ? this.tabPageBrokerAccess : this.tabPagePipeline);
      this.tabControl1_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void init(IWin32Window owner)
    {
      this.owner = owner;
      if (this.dirtyFlagChangedEventHandler == null)
        this.dirtyFlagChangedEventHandler = new EventHandler(this.onDirtyFlagChange);
      this.onDirtyFlagChange((object) this, (EventArgs) null);
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
      this.stdIconBtnSave = new StandardIconButton();
      this.stdIconBtnReset = new StandardIconButton();
      this.gcPersona = new GroupContainer();
      this.tabControl1 = new TabControl();
      this.tabPageAccess = new TabPage();
      this.tabPageHome = new TabPage();
      this.tabPagePipeline = new TabPage();
      this.tabPageLoans = new TabPage();
      this.tabPageFormsTools = new TabPage();
      this.tabPageeFolder = new TabPage();
      this.tabPageEnhancedConditions = new TabPage();
      this.tabPageContacts = new TabPage();
      this.tabPageSettings = new TabPage();
      this.tabPageExtSettings = new TabPage();
      this.tabPageBrokerAccess = new TabPage();
      this.tabPageTPOAdministration = new TabPage();
      this.tabPageConsumerConnect = new TabPage();
      this.tabPageLOConnect = new TabPage();
      this.tabPageAIQ = new TabPage();
      this.tabPageDeveloperConnect = new TabPage();
      this.cxbModify = new CheckBox();
      this.pnlExBottom = new PanelEx();
      this.lblDisconnectedFromPersona = new Label();
      this.lblLinkedWithPersona = new Label();
      this.btnClose = new Button();
      this.tabPageEVault = new TabPage();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      this.gcPersona.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.pnlExBottom.SuspendLayout();
      this.SuspendLayout();
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(1293, 7);
      this.stdIconBtnSave.MouseDownImage = (Image) null;
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(26, 24);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 3;
      this.stdIconBtnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnSave, "Save");
      this.stdIconBtnSave.Click += new EventHandler(this.btnSave_Click);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(1329, 7);
      this.stdIconBtnReset.MouseDownImage = (Image) null;
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(25, 24);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 2;
      this.stdIconBtnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnReset, "Reset");
      this.stdIconBtnReset.Click += new EventHandler(this.btnReset_Click);
      this.gcPersona.Controls.Add((Control) this.stdIconBtnSave);
      this.gcPersona.Controls.Add((Control) this.stdIconBtnReset);
      this.gcPersona.Controls.Add((Control) this.tabControl1);
      this.gcPersona.Controls.Add((Control) this.cxbModify);
      this.gcPersona.Dock = DockStyle.Fill;
      this.gcPersona.HeaderForeColor = SystemColors.ControlText;
      this.gcPersona.Location = new Point(0, 0);
      this.gcPersona.Name = "gcPersona";
      this.gcPersona.Padding = new Padding(1, 0, 0, 0);
      this.gcPersona.Size = new Size(1362, 608);
      this.gcPersona.TabIndex = 2;
      this.gcPersona.Text = "Persona Settings";
      this.tabControl1.Controls.Add((Control) this.tabPageAccess);
      this.tabControl1.Controls.Add((Control) this.tabPageHome);
      this.tabControl1.Controls.Add((Control) this.tabPagePipeline);
      this.tabControl1.Controls.Add((Control) this.tabPageLoans);
      this.tabControl1.Controls.Add((Control) this.tabPageFormsTools);
      this.tabControl1.Controls.Add((Control) this.tabPageeFolder);
      this.tabControl1.Controls.Add((Control) this.tabPageEnhancedConditions);
      this.tabControl1.Controls.Add((Control) this.tabPageContacts);
      this.tabControl1.Controls.Add((Control) this.tabPageSettings);
      this.tabControl1.Controls.Add((Control) this.tabPageExtSettings);
      this.tabControl1.Controls.Add((Control) this.tabPageBrokerAccess);
      this.tabControl1.Controls.Add((Control) this.tabPageTPOAdministration);
      this.tabControl1.Controls.Add((Control) this.tabPageConsumerConnect);
      this.tabControl1.Controls.Add((Control) this.tabPageEVault);
      this.tabControl1.Controls.Add((Control) this.tabPageLOConnect);
      this.tabControl1.Controls.Add((Control) this.tabPageAIQ);
      this.tabControl1.Controls.Add((Control) this.tabPageDeveloperConnect);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(2, 26);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(1359, 581);
      this.tabControl1.TabIndex = 1;
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tabPageAccess.BackColor = Color.WhiteSmoke;
      this.tabPageAccess.Location = new Point(4, 29);
      this.tabPageAccess.Name = "tabPageAccess";
      this.tabPageAccess.Size = new Size(1351, 548);
      this.tabPageAccess.TabIndex = 12;
      this.tabPageAccess.Text = "Access";
      this.tabPageHome.BackColor = Color.WhiteSmoke;
      this.tabPageHome.Location = new Point(4, 29);
      this.tabPageHome.Name = "tabPageHome";
      this.tabPageHome.Size = new Size(2167, 841);
      this.tabPageHome.TabIndex = 1;
      this.tabPageHome.Text = "Home";
      this.tabPageHome.UseVisualStyleBackColor = true;
      this.tabPagePipeline.BackColor = Color.WhiteSmoke;
      this.tabPagePipeline.Location = new Point(4, 29);
      this.tabPagePipeline.Name = "tabPagePipeline";
      this.tabPagePipeline.Padding = new Padding(3);
      this.tabPagePipeline.Size = new Size(2167, 841);
      this.tabPagePipeline.TabIndex = 9;
      this.tabPagePipeline.Text = "Pipeline";
      this.tabPagePipeline.UseVisualStyleBackColor = true;
      this.tabPageLoans.BackColor = Color.WhiteSmoke;
      this.tabPageLoans.Location = new Point(4, 29);
      this.tabPageLoans.Name = "tabPageLoans";
      this.tabPageLoans.Size = new Size(2167, 841);
      this.tabPageLoans.TabIndex = 2;
      this.tabPageLoans.Text = "Loan";
      this.tabPageLoans.UseVisualStyleBackColor = true;
      this.tabPageLoans.Visible = false;
      this.tabPageFormsTools.BackColor = Color.WhiteSmoke;
      this.tabPageFormsTools.Location = new Point(4, 29);
      this.tabPageFormsTools.Name = "tabPageFormsTools";
      this.tabPageFormsTools.Size = new Size(2167, 841);
      this.tabPageFormsTools.TabIndex = 4;
      this.tabPageFormsTools.Text = "Forms/Tools";
      this.tabPageFormsTools.UseVisualStyleBackColor = true;
      this.tabPageFormsTools.Visible = false;
      this.tabPageeFolder.Location = new Point(4, 29);
      this.tabPageeFolder.Name = "tabPageeFolder";
      this.tabPageeFolder.Padding = new Padding(3, 3, 0, 0);
      this.tabPageeFolder.Size = new Size(2167, 841);
      this.tabPageeFolder.TabIndex = 11;
      this.tabPageeFolder.Text = "eFolder";
      this.tabPageeFolder.UseVisualStyleBackColor = true;
      this.tabPageEnhancedConditions.Location = new Point(4, 29);
      this.tabPageEnhancedConditions.Name = "tabPageEnhancedConditions";
      this.tabPageEnhancedConditions.Padding = new Padding(3);
      this.tabPageEnhancedConditions.Size = new Size(2167, 841);
      this.tabPageEnhancedConditions.TabIndex = 16;
      this.tabPageEnhancedConditions.Text = "Enhanced Conditions";
      this.tabPageEnhancedConditions.UseVisualStyleBackColor = true;
      this.tabPageContacts.BackColor = Color.WhiteSmoke;
      this.tabPageContacts.Location = new Point(4, 29);
      this.tabPageContacts.Name = "tabPageContacts";
      this.tabPageContacts.Size = new Size(2167, 841);
      this.tabPageContacts.TabIndex = 7;
      this.tabPageContacts.Text = "Trades/Contacts/Dashboard/Reports";
      this.tabPageContacts.UseVisualStyleBackColor = true;
      this.tabPageContacts.Visible = false;
      this.tabPageSettings.BackColor = Color.WhiteSmoke;
      this.tabPageSettings.Location = new Point(4, 29);
      this.tabPageSettings.Name = "tabPageSettings";
      this.tabPageSettings.Size = new Size(2167, 841);
      this.tabPageSettings.TabIndex = 8;
      this.tabPageSettings.Text = "Settings";
      this.tabPageSettings.UseVisualStyleBackColor = true;
      this.tabPageSettings.Visible = false;
      this.tabPageExtSettings.Location = new Point(4, 29);
      this.tabPageExtSettings.Name = "tabPageExtSettings";
      this.tabPageExtSettings.Size = new Size(2167, 841);
      this.tabPageExtSettings.TabIndex = 13;
      this.tabPageExtSettings.Text = "External Settings";
      this.tabPageExtSettings.UseVisualStyleBackColor = true;
      this.tabPageBrokerAccess.BackColor = Color.WhiteSmoke;
      this.tabPageBrokerAccess.Location = new Point(4, 29);
      this.tabPageBrokerAccess.Name = "tabPageBrokerAccess";
      this.tabPageBrokerAccess.Size = new Size(2167, 841);
      this.tabPageBrokerAccess.TabIndex = 10;
      this.tabPageBrokerAccess.Text = "Access";
      this.tabPageBrokerAccess.UseVisualStyleBackColor = true;
      this.tabPageTPOAdministration.BackColor = Color.WhiteSmoke;
      this.tabPageTPOAdministration.Location = new Point(4, 29);
      this.tabPageTPOAdministration.Name = "tabPageTPOAdministration";
      this.tabPageTPOAdministration.Size = new Size(2167, 841);
      this.tabPageTPOAdministration.TabIndex = 14;
      this.tabPageTPOAdministration.Text = "TPO Connect";
      this.tabPageTPOAdministration.UseVisualStyleBackColor = true;
      this.tabPageConsumerConnect.Location = new Point(4, 29);
      this.tabPageConsumerConnect.Name = "tabPageConsumerConnect";
      this.tabPageConsumerConnect.Padding = new Padding(3);
      this.tabPageConsumerConnect.Size = new Size(2167, 841);
      this.tabPageConsumerConnect.TabIndex = 15;
      this.tabPageConsumerConnect.Text = "Consumer Connect";
      this.tabPageConsumerConnect.UseVisualStyleBackColor = true;
      this.tabPageLOConnect.Location = new Point(4, 29);
      this.tabPageLOConnect.Name = "tabPageLOConnect";
      this.tabPageLOConnect.Size = new Size(2167, 841);
      this.tabPageLOConnect.TabIndex = 17;
      this.tabPageLOConnect.Text = "Web Version";
      this.tabPageLOConnect.UseVisualStyleBackColor = true;
      this.tabPageAIQ.Location = new Point(4, 29);
      this.tabPageAIQ.Name = "tabPageAIQ";
      this.tabPageAIQ.Padding = new Padding(3);
      this.tabPageAIQ.Size = new Size(1351, 548);
      this.tabPageAIQ.TabIndex = 18;
      this.tabPageAIQ.Text = "Data && Document Automation and Mortgage Analyzers";
      this.tabPageAIQ.UseVisualStyleBackColor = true;
      this.tabPageDeveloperConnect.Location = new Point(4, 29);
      this.tabPageDeveloperConnect.Name = "tabPageDeveloperConnect";
      this.tabPageDeveloperConnect.Size = new Size(1351, 548);
      this.tabPageDeveloperConnect.TabIndex = 20;
      this.tabPageDeveloperConnect.Text = "Developer Connect";
      this.cxbModify.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cxbModify.AutoSize = true;
      this.cxbModify.BackColor = Color.Transparent;
      this.cxbModify.Location = new Point(1085, 7);
      this.cxbModify.Name = "cxbModify";
      this.cxbModify.Size = new Size(199, 24);
      this.cxbModify.TabIndex = 1;
      this.cxbModify.Text = "Modify this user's rights";
      this.cxbModify.UseVisualStyleBackColor = false;
      this.cxbModify.CheckedChanged += new EventHandler(this.cxbModify_CheckedChanged);
      this.pnlExBottom.Controls.Add((Control) this.lblDisconnectedFromPersona);
      this.pnlExBottom.Controls.Add((Control) this.lblLinkedWithPersona);
      this.pnlExBottom.Controls.Add((Control) this.btnClose);
      this.pnlExBottom.Dock = DockStyle.Bottom;
      this.pnlExBottom.Location = new Point(0, 608);
      this.pnlExBottom.Name = "pnlExBottom";
      this.pnlExBottom.Size = new Size(1362, 52);
      this.pnlExBottom.TabIndex = 3;
      this.lblDisconnectedFromPersona.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDisconnectedFromPersona.AutoSize = true;
      this.lblDisconnectedFromPersona.Image = (Image) Resources.link_broken;
      this.lblDisconnectedFromPersona.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnectedFromPersona.Location = new Point(14, 29);
      this.lblDisconnectedFromPersona.Name = "lblDisconnectedFromPersona";
      this.lblDisconnectedFromPersona.Size = new Size(288, 20);
      this.lblDisconnectedFromPersona.TabIndex = 9;
      this.lblDisconnectedFromPersona.Text = "        Disconnected from Persona Rights";
      this.lblDisconnectedFromPersona.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersona.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblLinkedWithPersona.AutoSize = true;
      this.lblLinkedWithPersona.Image = (Image) Resources.link;
      this.lblLinkedWithPersona.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersona.Location = new Point(14, 6);
      this.lblLinkedWithPersona.Name = "lblLinkedWithPersona";
      this.lblLinkedWithPersona.Size = new Size(233, 20);
      this.lblLinkedWithPersona.TabIndex = 8;
      this.lblLinkedWithPersona.Text = "        Linked with Persona Rights";
      this.lblLinkedWithPersona.TextAlign = ContentAlignment.MiddleLeft;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(1210, 10);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(120, 34);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.tabPageEVault.Location = new Point(4, 29);
      this.tabPageEVault.Name = "tabPageEVault";
      this.tabPageEVault.Size = new Size(2167, 841);
      this.tabPageEVault.TabIndex = 19;
      this.tabPageEVault.Text = "eVault";
      this.tabPageEVault.UseVisualStyleBackColor = true;
      this.AutoScaleBaseSize = new Size(8, 19);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1362, 660);
      this.Controls.Add((Control) this.gcPersona);
      this.Controls.Add((Control) this.pnlExBottom);
      this.Name = nameof (PersonaSettingsMainForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Persona Settings";
      this.Closing += new CancelEventHandler(this.PersonaSettingsMainForm_Closing);
      this.BackColorChanged += new EventHandler(this.PersonaSettingsMainForm_BackColorChanged);
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      this.gcPersona.ResumeLayout(false);
      this.gcPersona.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.pnlExBottom.ResumeLayout(false);
      this.pnlExBottom.PerformLayout();
      this.ResumeLayout(false);
    }

    public string Title
    {
      set => this.gcPersona.Text = value;
    }

    private void onDirtyFlagChange(object sender, EventArgs e)
    {
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonasEdit) && !this.session.UserInfo.IsSuperAdministrator())
        return;
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = this.IsDirty();
    }

    private void loadTabPagesCommon()
    {
      if (this.dirtyFlagChangedEventHandler == null)
        this.dirtyFlagChangedEventHandler = new EventHandler(this.onDirtyFlagChange);
      if (this.isBanker)
      {
        this.pipelinePage.Visible = true;
        this.pipelinePage.Dock = DockStyle.Fill;
        this.tabPagePipeline.Controls.Add((Control) this.pipelinePage);
        if (!this.session.StartupInfo.EnhancedConditionSettings)
        {
          this.subpages = new IPersonaSecurityPage[15]
          {
            (IPersonaSecurityPage) this.accessConfigPage,
            (IPersonaSecurityPage) this.homePage,
            (IPersonaSecurityPage) this.pipelinePage,
            (IPersonaSecurityPage) this.loansPage,
            (IPersonaSecurityPage) this.formsToolsPage,
            (IPersonaSecurityPage) this.eFolderPage,
            (IPersonaSecurityPage) this.contactsPage,
            (IPersonaSecurityPage) this.settingsPage,
            (IPersonaSecurityPage) this.externalSettingsPage,
            (IPersonaSecurityPage) this.tpoAdminPage,
            (IPersonaSecurityPage) this.ccAdminPage,
            (IPersonaSecurityPage) this.eVaultPage,
            (IPersonaSecurityPage) this.loConnectPage,
            (IPersonaSecurityPage) this.AIQPage,
            (IPersonaSecurityPage) this.developerConnectPage
          };
          this.tabControl1.Controls.Remove((Control) this.tabPageEnhancedConditions);
        }
        else
          this.subpages = new IPersonaSecurityPage[16]
          {
            (IPersonaSecurityPage) this.accessConfigPage,
            (IPersonaSecurityPage) this.homePage,
            (IPersonaSecurityPage) this.pipelinePage,
            (IPersonaSecurityPage) this.loansPage,
            (IPersonaSecurityPage) this.formsToolsPage,
            (IPersonaSecurityPage) this.eFolderPage,
            (IPersonaSecurityPage) this.enhancedConditionsPage,
            (IPersonaSecurityPage) this.contactsPage,
            (IPersonaSecurityPage) this.settingsPage,
            (IPersonaSecurityPage) this.externalSettingsPage,
            (IPersonaSecurityPage) this.tpoAdminPage,
            (IPersonaSecurityPage) this.ccAdminPage,
            (IPersonaSecurityPage) this.eVaultPage,
            (IPersonaSecurityPage) this.loConnectPage,
            (IPersonaSecurityPage) this.AIQPage,
            (IPersonaSecurityPage) this.developerConnectPage
          };
      }
      else
      {
        this.brokerAccessPage.Visible = true;
        this.brokerAccessPage.Dock = DockStyle.Fill;
        this.tabPageBrokerAccess.Controls.Add((Control) this.brokerAccessPage);
        this.subpages = new IPersonaSecurityPage[4]
        {
          (IPersonaSecurityPage) this.homePage,
          (IPersonaSecurityPage) this.brokerAccessPage,
          (IPersonaSecurityPage) this.ccAdminPage,
          (IPersonaSecurityPage) this.developerConnectPage
        };
      }
      try
      {
        if (Session.StartupInfo.HasAIQLicense)
        {
          Tracing.Log(Tracing.SwSYSTEM, TraceLevel.Info, nameof (PersonaSettingsMainForm), string.Format("Data & Document Automation and Mortgage Analyzers License is {0} so Data & Document Automation and Mortgage Analyzers tab in Persona is enabled. ", (object) Session.StartupInfo.HasAIQLicense.ToString()));
        }
        else
        {
          this.tabControl1.Controls.Remove((Control) this.tabPageAIQ);
          Tracing.Log(Tracing.SwSYSTEM, TraceLevel.Info, nameof (PersonaSettingsMainForm), string.Format("Data & Document Automation and Mortgage Analyzers License is {0} so Data & Document Automation and Mortgage Analyzers tab in Persona is disabled. ", (object) Session.StartupInfo.HasAIQLicense.ToString()));
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwSYSTEM, TraceLevel.Error, nameof (PersonaSettingsMainForm), string.Format("Error in Checking and setting Data & Document Automation and Mortgage Analyzers tab in persona. Exception: {0}", (object) ex.Message));
      }
    }

    private void loadTabPagesForPersona()
    {
      if (this.isBanker)
      {
        this.pipelinePage = new PipelineConfiguration(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
        this.pipelinePage.FeatureStateChanged += new PipelineConfiguration.PipelineConfigChanged(this.pipelinePage_FeatureStateChanged);
        this.pipelinePage.HasContactOriginateLoanAccessEvent += new PipelineConfiguration.PersonaAccess(this.pipelinePage_HasContactOriginateLoanAccessEvent);
      }
      else
        this.brokerAccessPage = new AccessPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
      this.loadTabPagesCommon();
    }

    private void pipelinePage_FeatureStateChanged(
      AclFeature feature,
      AclTriState state,
      bool gotoContactTab)
    {
      if (feature != AclFeature.GlobalTab_Pipeline)
        return;
      this.requirePipelineLoanReadOnly = state != AclTriState.True;
      this.forcedReadOnly();
      if (((state != AclTriState.False ? 0 : (!this.suspendEvent ? 1 : 0)) & (gotoContactTab ? 1 : 0)) == 0)
        return;
      this.contactsPage.DisallowLoanOrigination();
    }

    private void loadTabPagesForUser()
    {
      this.currentUser = this.session.OrganizationManager.GetUser(this.userId);
      if (this.isBanker)
      {
        this.pipelinePage = new PipelineConfiguration(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler);
        this.pipelinePage.FeatureStateChanged += new PipelineConfiguration.PipelineConfigChanged(this.pipelinePage_FeatureStateChanged);
        this.pipelinePage.HasContactOriginateLoanAccessEvent += new PipelineConfiguration.PersonaAccess(this.pipelinePage_HasContactOriginateLoanAccessEvent);
      }
      else
        this.brokerAccessPage = new AccessPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler);
      this.loadTabPagesCommon();
    }

    private void PersonaSettingsMainForm_BackColorChanged(object sender, EventArgs e)
    {
      if (this.homePage != null)
        this.homePage.BackColor = this.BackColor;
      if (this.pipelinePage != null)
        this.pipelinePage.BackColor = this.BackColor;
      if (this.accessConfigPage != null)
        this.accessConfigPage.BackColor = this.BackColor;
      if (this.loansPage != null)
        this.loansPage.BackColor = this.BackColor;
      if (this.formsToolsPage != null)
        this.formsToolsPage.BackColor = this.BackColor;
      if (this.contactsPage != null)
        this.contactsPage.BackColor = this.BackColor;
      if (this.settingsPage != null)
        this.settingsPage.BackColor = this.BackColor;
      if (this.externalSettingsPage != null)
        this.externalSettingsPage.BackColor = this.BackColor;
      if (this.tpoAdminPage != null)
        this.tpoAdminPage.BackColor = this.BackColor;
      if (this.brokerAccessPage != null)
        this.brokerAccessPage.BackColor = this.BackColor;
      if (this.eFolderPage != null)
        this.eFolderPage.BackColor = this.BackColor;
      if (this.ccAdminPage != null)
        this.ccAdminPage.BackColor = this.BackColor;
      if (this.enhancedConditionsPage != null)
        this.enhancedConditionsPage.BackColor = this.BackColor;
      if (this.eVaultPage != null)
        this.eVaultPage.BackColor = this.BackColor;
      if (this.loConnectPage != null)
        this.loConnectPage.BackColor = this.BackColor;
      if (this.AIQPage != null)
        this.AIQPage.BackColor = this.BackColor;
      if (this.developerConnectPage == null)
        return;
      this.developerConnectPage.BackColor = this.BackColor;
    }

    private void loadModificationSetting()
    {
      bool flag = true;
      this.internalChange = true;
      if (this.CheckPersonalFeatures())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.checkPlanCodeAltLender())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalInputForms())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalFieldAccess())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalMilestones())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalLoanFolders())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalTools())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalServices())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalFeatureConfigs())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckEnhancedConditionByUserId())
      {
        this.cxbModify.Checked = true;
        flag = false;
      }
      if (flag && this.CheckPersonalExportServices())
        this.cxbModify.Checked = true;
      if (flag && this.CheckLoconnect())
        this.cxbModify.Checked = true;
      this.internalChange = false;
      this.forcedReadOnly();
    }

    private void forcedReadOnly()
    {
      if (this.isBanker)
        this.requirePipelineLoanReadOnly = !this.pipelinePage.HasPipelineLoanTabAccess();
      if (this.userId != string.Empty)
        this.makeAllReadOnly(!this.cxbModify.Checked);
      if (this.userId != string.Empty)
      {
        if (!this.cxbModify.Checked || !this.requirePipelineLoanReadOnly)
          return;
        if (this.loansPage != null)
          this.loansPage.MakeReadOnly(this.requirePipelineLoanReadOnly);
        if (this.formsToolsPage == null)
          return;
        this.formsToolsPage.MakeReadOnly(this.requirePipelineLoanReadOnly);
      }
      else
      {
        if (this.loansPage != null)
          this.loansPage.MakeReadOnly(this.requirePipelineLoanReadOnly);
        if (this.formsToolsPage == null)
          return;
        this.formsToolsPage.MakeReadOnly(this.requirePipelineLoanReadOnly);
      }
    }

    private void makeAllReadOnly(bool makeReadOnly)
    {
      if (this.accessConfigPage != null)
        this.accessConfigPage.MakeReadOnly(makeReadOnly);
      if (this.pipelinePage != null)
        this.pipelinePage.MakeReadOnly(makeReadOnly);
      if (this.loansPage != null)
        this.loansPage.MakeReadOnly(makeReadOnly);
      if (this.formsToolsPage != null)
        this.formsToolsPage.MakeReadOnly(makeReadOnly);
      if (this.contactsPage != null)
        this.contactsPage.MakeReadOnly(makeReadOnly);
      if (this.settingsPage != null)
        this.settingsPage.MakeReadOnly(makeReadOnly);
      if (this.externalSettingsPage != null)
        this.externalSettingsPage.MakeReadOnly(makeReadOnly);
      if (this.tpoAdminPage != null)
        this.tpoAdminPage.MakeReadOnly(makeReadOnly);
      if (this.brokerAccessPage != null)
        this.brokerAccessPage.MakeReadOnly(makeReadOnly);
      if (this.homePage != null)
        this.homePage.MakeReadOnly(makeReadOnly);
      if (this.eFolderPage != null)
        this.eFolderPage.MakeReadOnly(makeReadOnly);
      if (this.ccAdminPage != null)
        this.ccAdminPage.MakeReadOnly(makeReadOnly);
      if (this.enhancedConditionsPage != null)
        this.enhancedConditionsPage.MakeReadOnly(makeReadOnly);
      if (this.eVaultPage != null)
        this.eVaultPage.MakeReadOnly(makeReadOnly);
      if (this.loConnectPage != null)
        this.loConnectPage.MakeReadOnly(makeReadOnly);
      if (this.AIQPage != null)
        this.AIQPage.MakeReadOnly(makeReadOnly);
      if (this.developerConnectPage == null)
        return;
      this.developerConnectPage.MakeReadOnly(makeReadOnly);
    }

    private bool CheckPersonalFeatures()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      ArrayList arrayList = new ArrayList();
      if (this.session.EncompassEdition == EncompassEdition.Broker)
      {
        arrayList.AddRange((ICollection) FeatureSets.BrokerFeatures);
        arrayList.AddRange((ICollection) FeatureSets.LoanEFolderFeatures);
        arrayList.AddRange((ICollection) FeatureSets.ConsumerConnectTabFeatures);
        arrayList.AddRange((ICollection) FeatureSets.DeveloperConnectFeatures);
      }
      else
      {
        arrayList.AddRange((ICollection) FeatureSets.BizContacts);
        arrayList.AddRange((ICollection) FeatureSets.BorContacts);
        arrayList.AddRange((ICollection) FeatureSets.Contacts);
        arrayList.AddRange((ICollection) FeatureSets.Features);
        arrayList.AddRange((ICollection) FeatureSets.PipelineGlobalTabFeatures);
        arrayList.AddRange((ICollection) FeatureSets.LoanMgmtFeatures);
        arrayList.AddRange((ICollection) FeatureSets.LoansPrintFeatures);
        arrayList.AddRange((ICollection) FeatureSets.SettingsTabPersonalFeatures);
        arrayList.AddRange((ICollection) FeatureSets.SettingsTabCompanyFeatures);
        arrayList.AddRange((ICollection) FeatureSets.ToolsFeatures);
        arrayList.AddRange((ICollection) FeatureSets.LoanEFolderFeatures);
        arrayList.AddRange((ICollection) FeatureSets.LoanOtherFeatures);
        arrayList.AddRange((ICollection) FeatureSets.DashboardFeatures);
        arrayList.AddRange((ICollection) FeatureSets.ReportFeatures);
        arrayList.AddRange((ICollection) FeatureSets.TradeFeatures);
        arrayList.AddRange((ICollection) FeatureSets.HomeFeatures);
        arrayList.AddRange((ICollection) FeatureSets.EMClosingDocsFeatures);
        arrayList.AddRange((ICollection) FeatureSets.ExternalSettingTabFeatures);
        arrayList.AddRange((ICollection) FeatureSets.TPOAdministrationTabFeatures);
        arrayList.AddRange((ICollection) FeatureSets.ConsumerConnectTabFeatures);
        arrayList.AddRange((ICollection) FeatureSets.TPOSiteSettingsTabFeatures);
        arrayList.AddRange((ICollection) FeatureSets.eVaultFeatures);
        arrayList.AddRange((ICollection) FeatureSets.DeveloperConnectFeatures);
        arrayList.AddRange((ICollection) FeatureSets.LOConnectStandardFeatures);
      }
      return aclManager.GetPermissions((AclFeature[]) arrayList.ToArray(typeof (AclFeature)), this.currentUser.Userid).Count > 0;
    }

    private bool CheckEnhancedConditionByUserId()
    {
      Dictionary<string, Hashtable> permissionsByUser = ((EnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions)).GetPermissionsByUser(FeatureSets.AclEnhancedConditions, this.currentUser.Userid);
      return permissionsByUser != null && permissionsByUser.Keys.Count > 0;
    }

    private bool CheckPersonalInputForms()
    {
      bool flag = false;
      Hashtable permissionsForAllForms = ((InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms)).GetPermissionsForAllForms(this.currentUser.Userid);
      if (permissionsForAllForms != null && permissionsForAllForms.Count > 0)
        flag = true;
      return flag;
    }

    private bool CheckLoconnect()
    {
      bool flag = false;
      LoConnectServiceAclManager aclManager1 = (LoConnectServiceAclManager) this.session.ACL.GetAclManager(AclCategory.LOConnectCustomServices);
      StandardWebFormAclManager aclManager2 = (StandardWebFormAclManager) this.session.ACL.GetAclManager(AclCategory.StandardWebforms);
      if (this.loConnectPage == null)
        this.loConnectPage = this.personal ? new LOConnectConfig(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new LOConnectConfig(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
      Hashtable permissionsByUser1 = aclManager1.GetPermissionsByUser(this.loConnectPage.loConnectServices, this.currentUser.Userid);
      if (permissionsByUser1 != null && permissionsByUser1.Count > 0)
        flag = true;
      Hashtable permissionsByUser2 = aclManager2.GetPermissionsByUser(this.currentUser.Userid);
      if (permissionsByUser2 != null && permissionsByUser2.Count > 0)
        flag = true;
      return flag;
    }

    private bool CheckPersonalMilestones()
    {
      bool flag = false;
      Hashtable personalPermission = ((MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones)).GetPersonalPermission((AclMilestone[]) null, this.currentUser.Userid);
      if (personalPermission != null && personalPermission.Count > 0)
        flag = true;
      return flag;
    }

    private bool CheckPersonalLoanDuplication()
    {
      bool flag = false;
      LoanFoldersAclManager aclManager = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      LoanFolderAclInfo[] permissions1 = aclManager.GetPermissions(AclFeature.LoanMgmt_Move, this.currentUser.Userid);
      if (permissions1 != null && permissions1.Length != 0)
        flag = true;
      if (flag)
        return flag;
      LoanFolderAclInfo[] permissions2 = aclManager.GetPermissions(AclFeature.LoanMgmt_Import, this.currentUser.Userid);
      if (permissions2 != null && permissions2.Length != 0)
        flag = true;
      return flag;
    }

    private bool CheckPersonalLoanFolders()
    {
      bool flag = false;
      LoanFoldersAclManager aclManager = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      LoanFolderAclInfo[] permissions1 = aclManager.GetPermissions(AclFeature.LoanMgmt_Move, this.currentUser.Userid);
      if (permissions1 != null && permissions1.Length != 0)
        flag = true;
      if (flag)
        return flag;
      LoanFolderAclInfo[] permissions2 = aclManager.GetPermissions(AclFeature.LoanMgmt_Import, this.currentUser.Userid);
      if (permissions2 != null && permissions2.Length != 0)
        flag = true;
      return flag;
    }

    private bool CheckPersonalServices()
    {
      ServicesAclManager aclManager = (ServicesAclManager) this.session.ACL.GetAclManager(AclCategory.Services);
      if (aclManager.GetUserSpecificServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, this.currentUser.Userid, this.currentUser.UserPersonas) != ServiceAclInfo.ServicesDefaultSetting.NotSpecified)
        return true;
      ServiceAclInfo[] permissions = aclManager.GetPermissions(AclFeature.LoanTab_Other_ePASS, this.currentUser.Userid, this.currentUser.UserPersonas);
      bool flag = false;
      foreach (ServiceAclInfo serviceAclInfo in permissions)
      {
        if (serviceAclInfo.CustomAccess != AclResourceAccess.None)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private bool CheckPersonalExportServices()
    {
      ExportServicesAclManager aclManager = (ExportServicesAclManager) this.session.ACL.GetAclManager(AclCategory.ExportServices);
      if (aclManager.GetUserSpecificExportServicesDefaultSetting(AclFeature.LoanMgmt_MgmtPipelineServices, this.currentUser.Userid, this.currentUser.UserPersonas) != ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified)
        return true;
      ExportServiceAclInfo[] permissions = aclManager.GetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, this.currentUser.Userid, this.currentUser.UserPersonas, ExportServiceAclInfo.GetExportServicesList(ServicesMapping.Categories.ToArray(), 229).ToArray());
      bool flag = false;
      foreach (ExportServiceAclInfo exportServiceAclInfo in permissions)
      {
        if (exportServiceAclInfo.CustomAccess != AclResourceAccess.None)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private bool CheckPersonalTools()
    {
      bool flag = false;
      ToolsAclInfo[] permissions = ((ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess)).GetPermissions(this.currentUser.Userid);
      if (permissions != null && permissions.Length != 0)
        flag = true;
      return flag;
    }

    private bool CheckPersonalFeatureConfigs()
    {
      bool flag = false;
      Dictionary<AclFeature, int> permissions = ((FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs)).GetPermissions(this.currentUser.Userid);
      if (permissions != null && permissions.Count > 0)
        flag = true;
      return flag;
    }

    private bool checkPlanCodeAltLender()
    {
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetPermissions(new AclFeature[2]
      {
        AclFeature.LoanTab_Other_PlanCode,
        AclFeature.LoanTab_Other_AltLender
      }, this.currentUser.Userid).Count > 0;
    }

    private bool CheckPersonalFieldAccess()
    {
      bool flag = false;
      Dictionary<string, AclTriState> fieldIdsPermission = ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).GetFieldIDsPermission(this.currentUser.Userid);
      if (fieldIdsPermission != null && fieldIdsPermission.Count > 0)
        flag = true;
      return flag;
    }

    private void cxbModify_CheckedChanged(object sender, EventArgs e)
    {
      if (this.internalChange)
        this.internalChange = false;
      else if (this.cxbModify.Checked)
      {
        if (DialogResult.Yes == Utils.Dialog((IWin32Window) this, "Do you want to create personalized security settings for this user?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        {
          this.forcedReadOnly();
          if (!this.isBanker)
            return;
          if (this.pipelinePage.HasPipelineLoanTabAccess())
            this.pipelinePage.pipelineLoanTabPage_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, AclTriState.True, false);
          else
            this.pipelinePage.pipelineLoanTabPage_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, AclTriState.False, false);
        }
        else
        {
          this.internalChange = true;
          this.cxbModify.Checked = false;
        }
      }
      else if (DialogResult.Yes == Utils.Dialog((IWin32Window) this, "All the personalized security settings for this user will be removed. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
      {
        this.CleanUpPersonalSetting();
        this.Reset();
        this.forcedReadOnly();
      }
      else
      {
        this.internalChange = true;
        this.cxbModify.Checked = true;
        this.internalChange = false;
      }
    }

    private void CleanUpPersonalSetting()
    {
      FeaturesAclManager aclManager1 = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) FeatureSets.BizContacts);
      arrayList.AddRange((ICollection) FeatureSets.BorContacts);
      arrayList.AddRange((ICollection) FeatureSets.Contacts);
      arrayList.AddRange((ICollection) FeatureSets.Features);
      arrayList.AddRange((ICollection) FeatureSets.PipelineGlobalTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanMgmtFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoansPrintFeatures);
      arrayList.AddRange((ICollection) FeatureSets.SettingsTabPersonalFeatures);
      arrayList.AddRange((ICollection) FeatureSets.SettingsTabCompanyFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ToolsFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanEFolderFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanOtherFeatures);
      arrayList.AddRange((ICollection) FeatureSets.DashboardFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ReportFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TradeFeatures);
      arrayList.AddRange((ICollection) FeatureSets.HomeFeatures);
      arrayList.AddRange((ICollection) FeatureSets.EMClosingDocsFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ExternalSettingTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TPOAdministrationTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ConsumerConnectTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TPOSiteSettingsTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.eVaultFeatures);
      arrayList.AddRange((ICollection) FeatureSets.DeveloperConnectFeatures);
      IEnumerator enumerator = aclManager1.GetPermissions((AclFeature[]) arrayList.ToArray(typeof (AclFeature)), this.currentUser.Userid).Keys.GetEnumerator();
      Hashtable featureAccesses = new Hashtable();
      while (enumerator.MoveNext())
        featureAccesses.Add(enumerator.Current, (object) AclTriState.Unspecified);
      if (featureAccesses.Count > 0)
        aclManager1.SetPermissions(featureAccesses, this.currentUser.Userid);
      ((FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs)).ClearUserSpecificSettings(this.currentUser.Userid);
      InputFormsAclManager aclManager2 = (InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms);
      Hashtable permissionsForAllForms = aclManager2.GetPermissionsForAllForms(this.currentUser.Userid);
      if (permissionsForAllForms != null && permissionsForAllForms.Count > 0)
      {
        foreach (object key in (IEnumerable) permissionsForAllForms.Keys)
          aclManager2.SetPermission(string.Concat(key), this.currentUser.Userid, (object) null);
      }
      ((MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones)).DeleteUserSpecificSetting(this.currentUser.Userid);
      ((ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess)).SetPermissions((ToolsAclInfo[]) null, this.currentUser.Userid);
      LoanFoldersAclManager aclManager3 = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      aclManager3.SetPermissions(AclFeature.LoanMgmt_Move, (LoanFolderAclInfo[]) null, this.currentUser.Userid);
      aclManager3.SetPermissions(AclFeature.LoanMgmt_Import, (LoanFolderAclInfo[]) null, this.currentUser.Userid);
      ServicesAclManager aclManager4 = (ServicesAclManager) this.session.ACL.GetAclManager(AclCategory.Services);
      ServiceAclInfo[] permissions1 = aclManager4.GetPermissions(AclFeature.LoanTab_Other_ePASS, this.currentUser.Userid, this.currentUser.UserPersonas);
      foreach (ServiceAclInfo serviceAclInfo in permissions1)
        serviceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager4.SetPermissions(AclFeature.LoanTab_Other_ePASS, permissions1, this.currentUser.Userid);
      aclManager4.SetDefaultValue(AclFeature.LoanTab_Other_ePASS, this.currentUser.Userid, ServiceAclInfo.ServicesDefaultSetting.NotSpecified);
      ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).SetFieldIDsPermission(this.currentUser.Userid, new Dictionary<string, AclTriState>());
      ExportServicesAclManager aclManager5 = (ExportServicesAclManager) this.session.ACL.GetAclManager(AclCategory.ExportServices);
      ExportServiceAclInfo[] permissions2 = aclManager5.GetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, this.currentUser.Userid, this.currentUser.UserPersonas, ExportServiceAclInfo.GetExportServicesList(ServicesMapping.Categories.ToArray(), 229).ToArray());
      foreach (ExportServiceAclInfo exportServiceAclInfo in permissions2)
        exportServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager5.SetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, permissions2, this.currentUser.Userid);
      aclManager5.SetDefaultValue(AclFeature.LoanMgmt_MgmtPipelineServices, this.currentUser.Userid, ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified);
      InvestorServicesAclManager aclManager6 = (InvestorServicesAclManager) this.session.ACL.GetAclManager(AclCategory.InvestorServices);
      List<string> stringList = new List<string>()
      {
        SelectInvestorsPage.WellsFargoPartnerCompanyCode
      };
      string investorCategory = SelectInvestorsPage.InvestorCategory;
      stringList.AddRange(new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(investorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>());
      InvestorServiceAclInfo[] permissions3 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Service, this.currentUser.Userid, investorCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(stringList.ToArray(), 9007, investorCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions3)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Service, permissions3, this.currentUser.Userid, investorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Service, this.currentUser.Userid, investorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string warehouseLendersCategory = WarehouseLendersServicePage.InvestorCategory;
      string[] array1 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(warehouseLendersCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions4 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.currentUser.Userid, warehouseLendersCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array1, 9009, warehouseLendersCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions4)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, permissions4, this.currentUser.Userid, warehouseLendersCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.currentUser.Userid, warehouseLendersCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string wholesaleLendersCategory = WholesaleLenderServicePage.InvestorCategory;
      string[] array2 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(wholesaleLendersCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions5 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.currentUser.Userid, wholesaleLendersCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array2, 9017, wholesaleLendersCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions5)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, permissions5, this.currentUser.Userid, wholesaleLendersCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.currentUser.Userid, wholesaleLendersCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string dueDiligenceCategory = DueDiligenceServicePage.InvestorCategory;
      string[] array3 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(dueDiligenceCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions6 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Due_Diligence, this.currentUser.Userid, dueDiligenceCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array3, 9010, dueDiligenceCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions6)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Due_Diligence, permissions6, this.currentUser.Userid, dueDiligenceCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Due_Diligence, this.currentUser.Userid, dueDiligenceCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string hedgeAdvisoryCategory = HedgeAdvisoryServicePage.InvestorCategory;
      string[] array4 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(hedgeAdvisoryCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions7 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.currentUser.Userid, hedgeAdvisoryCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array4, 9011, hedgeAdvisoryCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions7)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Hedge_Advisory, permissions7, this.currentUser.Userid, hedgeAdvisoryCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.currentUser.Userid, hedgeAdvisoryCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string qcAuditCategory = QCAuditServicesPage.InvestorCategory;
      string[] array5 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(qcAuditCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions8 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.currentUser.Userid, qcAuditCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array5, 9014, qcAuditCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions8)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_QC_Audit_Services, permissions8, this.currentUser.Userid, qcAuditCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.currentUser.Userid, qcAuditCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string subservicingCategory = SubservicingServicePage.InvestorCategory;
      string[] array6 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(subservicingCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions9 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Subservicing_Services, this.currentUser.Userid, subservicingCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array6, 9012, subservicingCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions9)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Subservicing_Services, permissions9, this.currentUser.Userid, subservicingCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Subservicing_Services, this.currentUser.Userid, subservicingCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string bidTapeCategory = BidTapeServicePage.InvestorCategory;
      string[] array7 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(bidTapeCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions10 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.currentUser.Userid, bidTapeCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array7, 9013, bidTapeCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions10)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Bid_Tape_Services, permissions10, this.currentUser.Userid, bidTapeCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.currentUser.Userid, bidTapeCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      string servicingCategory = ServicingServicePage.InvestorCategory;
      string[] array8 = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(servicingCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
      InvestorServiceAclInfo[] permissions11 = aclManager6.GetPermissions(AclFeature.LoanMgmt_Investor_Servicing_Services, this.currentUser.Userid, servicingCategory, this.currentUser.UserPersonas, InvestorServiceAclInfo.GetInvestorServicesList(array8, 9020, servicingCategory).ToArray());
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions11)
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
      aclManager6.SetPermissions(AclFeature.LoanMgmt_Investor_Servicing_Services, permissions11, this.currentUser.Userid, servicingCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      aclManager6.SetDefaultValue(AclFeature.LoanMgmt_Investor_Servicing_Services, this.currentUser.Userid, servicingCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified);
      ((EnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions)).DeleteAllUserSpecificPermissions(this.currentUser.Userid);
      ((LoConnectServiceAclManager) this.session.ACL.GetAclManager(AclCategory.LOConnectCustomServices)).DeleteUserPermission(this.currentUser.Userid);
    }

    private void btnSave_Click(object sender, EventArgs e) => this.Save();

    public void Save()
    {
      if (!this.IsValid())
        return;
      if (!this.cxbModify.Visible || this.cxbModify.Visible && this.cxbModify.Checked)
      {
        bool flag = this.IsDirty();
        if (this.homePage != null)
          this.homePage.SaveData();
        if (this.formsToolsPage != null)
          this.formsToolsPage.Save();
        if (this.pipelinePage != null)
          this.pipelinePage.Save();
        if (this.accessConfigPage != null)
          this.accessConfigPage.Save();
        if (this.loansPage != null)
          this.loansPage.SaveData();
        if (this.homePage != null)
          this.homePage.SaveData();
        if (this.contactsPage != null)
          this.contactsPage.SaveData();
        if (this.settingsPage != null)
          this.settingsPage.SaveData();
        if (this.externalSettingsPage != null)
          this.externalSettingsPage.SaveData();
        if (this.tpoAdminPage != null)
          this.tpoAdminPage.SaveData();
        if (this.brokerAccessPage != null)
          this.brokerAccessPage.SaveData();
        if (this.eFolderPage != null)
          this.eFolderPage.SaveData();
        if (this.enhancedConditionsPage != null)
          this.enhancedConditionsPage.SaveData();
        if (this.accessConfigPage != null)
          this.accessConfigPage.Save();
        if (this.ccAdminPage != null)
          this.ccAdminPage.SaveData();
        if (this.eVaultPage != null)
          this.eVaultPage.SaveData();
        if (this.loConnectPage != null)
          this.loConnectPage.Save();
        if (this.AIQPage != null)
          this.AIQPage.Save();
        if (this.developerConnectPage != null)
          this.developerConnectPage.SaveData();
        if (flag)
          this.session.InsertSystemAuditRecord((SystemAuditRecord) new PersonaAuditRecord(this.session.UserID, this.session.UserInfo.FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaModified, DateTime.Now, this.currentPersonaId, this.personaName));
      }
      if (!this.cxbModify.Visible)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (ResetConfirmDialog.ShowDialog((IWin32Window) this, (string) null) == DialogResult.No)
        return;
      this.Reset();
    }

    public void Reset()
    {
      if (this.homePage != null)
        this.homePage.ResetData();
      if (this.pipelinePage != null)
        this.pipelinePage.Reset();
      if (this.accessConfigPage != null)
        this.accessConfigPage.Reset();
      if (this.loansPage != null)
        this.loansPage.ResetData();
      if (this.formsToolsPage != null)
        this.formsToolsPage.Reset();
      if (this.enhancedConditionsPage != null)
        this.enhancedConditionsPage.ResetData();
      if (this.contactsPage != null)
        this.contactsPage.ResetData();
      if (this.settingsPage != null)
        this.settingsPage.ResetData();
      if (this.externalSettingsPage != null)
        this.externalSettingsPage.ResetData();
      if (this.tpoAdminPage != null)
        this.tpoAdminPage.ResetData();
      if (this.brokerAccessPage != null)
        this.brokerAccessPage.ResetData();
      if (this.eFolderPage != null)
        this.eFolderPage.ResetData();
      if (this.ccAdminPage != null)
        this.ccAdminPage.ResetData();
      if (this.eVaultPage != null)
        this.eVaultPage.ResetData();
      if (this.loConnectPage != null)
        this.loConnectPage.Reset();
      if (this.AIQPage != null)
        this.AIQPage.Reset();
      if (this.developerConnectPage == null)
        return;
      this.developerConnectPage.ResetData();
    }

    public DialogResult CheckUnsavedData(bool onPersonaPage)
    {
      if (!this.IsValid())
        return DialogResult.Cancel;
      if (!this.IsDirty())
        return DialogResult.No;
      DialogResult dialogResult = Utils.Dialog((IWin32Window) this, ((this.personas == null ? 1 : (this.personas.Length == 0 ? 1 : 0)) & (onPersonaPage ? 1 : 0)) == 0 ? "Do you want to save your changes before proceeding?" : "Do you want to save your changes before selecting another persona?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      if (dialogResult == DialogResult.Yes)
        this.btnSave_Click((object) null, (EventArgs) null);
      return dialogResult;
    }

    public bool IsDirty()
    {
      bool flag = false;
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonasEdit) && !this.session.UserInfo.IsSuperAdministrator())
        return flag;
      if (this.formsToolsPage != null && this.formsToolsPage.IsDirty)
        flag = true;
      else if (this.homePage != null && this.homePage.NeedToSaveData())
        flag = true;
      else if (this.pipelinePage != null && this.pipelinePage.IsDirty)
        flag = true;
      else if (this.loansPage != null && this.loansPage.NeedToSaveData())
        flag = true;
      else if (this.contactsPage != null && this.contactsPage.NeedToSaveData())
        flag = true;
      else if (this.settingsPage != null && this.settingsPage.NeedToSaveData())
        flag = true;
      else if (this.externalSettingsPage != null && this.externalSettingsPage.NeedToSaveData())
        flag = true;
      else if (this.tpoAdminPage != null && this.tpoAdminPage.NeedToSaveData())
        flag = true;
      else if (this.eFolderPage != null && this.eFolderPage.NeedToSaveData())
        flag = true;
      else if (this.enhancedConditionsPage != null && this.enhancedConditionsPage.NeedToSaveData())
        flag = true;
      else if (this.brokerAccessPage != null && this.brokerAccessPage.IsDirty)
        flag = true;
      else if (this.accessConfigPage != null && this.accessConfigPage.IsDirty)
        flag = true;
      else if (this.ccAdminPage != null && this.ccAdminPage.NeedToSaveData())
        flag = true;
      else if (this.eVaultPage != null && this.eVaultPage.NeedToSaveData())
        flag = true;
      else if (this.loConnectPage != null && this.loConnectPage.IsDirty)
        flag = true;
      else if (this.AIQPage != null && this.AIQPage.IsDirty)
        flag = true;
      else if (this.developerConnectPage != null && this.developerConnectPage.NeedToSaveData())
        flag = true;
      return flag;
    }

    public bool IsValid()
    {
      if (!this.isBanker || this.pipelinePage.IsValid)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must enter at least one field in Pipeline View.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.tabControl1.SelectedTab = this.tabPagePipeline;
      return false;
    }

    private void PersonaSettingsMainForm_Closing(object sender, CancelEventArgs e)
    {
      if (!this.cxbModify.Checked || this.CheckUnsavedData(true) != DialogResult.Cancel)
        return;
      e.Cancel = true;
    }

    private void createPipelinePage()
    {
      this.pipelinePage = this.personal ? new PipelineConfiguration(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new PipelineConfiguration(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
      this.pipelinePage.HasContactOriginateLoanAccessEvent += new PipelineConfiguration.PersonaAccess(this.pipelinePage_HasContactOriginateLoanAccessEvent);
      this.pipelinePage.Visible = true;
      this.pipelinePage.Dock = DockStyle.Fill;
      this.pipelinePage.BackColor = this.BackColor;
      this.tabPagePipeline.Controls.Add((Control) this.pipelinePage);
      this.subpages[2] = (IPersonaSecurityPage) this.pipelinePage;
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.tabControl1.SelectedTab.Text)
      {
        case "Access":
          if (this.session.EncompassEdition == EncompassEdition.Broker)
          {
            if (this.brokerAccessPage == null)
            {
              this.brokerAccessPage = this.personal ? new AccessPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new AccessPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
              this.brokerAccessPage.Visible = true;
              this.brokerAccessPage.Dock = DockStyle.Fill;
              this.brokerAccessPage.BackColor = this.BackColor;
              this.tabPageBrokerAccess.Controls.Add((Control) this.brokerAccessPage);
              this.subpages[1] = (IPersonaSecurityPage) this.brokerAccessPage;
              break;
            }
            break;
          }
          if (this.accessConfigPage == null)
          {
            this.accessConfigPage = this.personal ? new AccessConfig(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new AccessConfig(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.accessConfigPage.Visible = true;
            this.accessConfigPage.Dock = DockStyle.Fill;
            this.accessConfigPage.BackColor = this.BackColor;
            this.tabPageAccess.Controls.Add((Control) this.accessConfigPage);
            this.subpages[0] = (IPersonaSecurityPage) this.accessConfigPage;
            break;
          }
          this.tabPageAccess.Controls.Add((Control) this.accessConfigPage);
          this.subpages[0] = (IPersonaSecurityPage) this.accessConfigPage;
          break;
        case "Consumer Connect":
          if (this.ccAdminPage == null)
          {
            this.ccAdminPage = this.personal ? new CCAdminPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new CCAdminPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.ccAdminPage.TopLevel = false;
            this.ccAdminPage.Visible = true;
            this.ccAdminPage.Dock = DockStyle.Fill;
            this.ccAdminPage.BackColor = this.BackColor;
            this.tabPageConsumerConnect.Controls.Add((Control) this.ccAdminPage);
            if (this.isBanker)
            {
              if (this.session.StartupInfo.EnhancedConditionSettings)
              {
                this.subpages[11] = (IPersonaSecurityPage) this.ccAdminPage;
                break;
              }
              this.subpages[10] = (IPersonaSecurityPage) this.ccAdminPage;
              break;
            }
            this.subpages[2] = (IPersonaSecurityPage) this.ccAdminPage;
            break;
          }
          this.tabPageConsumerConnect.Controls.Add((Control) this.ccAdminPage);
          if (this.isBanker)
          {
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[11] = (IPersonaSecurityPage) this.ccAdminPage;
              break;
            }
            this.subpages[10] = (IPersonaSecurityPage) this.ccAdminPage;
            break;
          }
          this.subpages[2] = (IPersonaSecurityPage) this.ccAdminPage;
          break;
        case "Data && Document Automation and Mortgage Analyzers":
          if (this.AIQPage == null)
          {
            this.AIQPage = this.personal ? new AIQPersonaSettingPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new AIQPersonaSettingPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.AIQPage.Visible = true;
            this.AIQPage.Dock = DockStyle.Fill;
            this.AIQPage.BackColor = this.BackColor;
            this.tabPageAIQ.Controls.Add((Control) this.AIQPage);
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[14] = (IPersonaSecurityPage) this.AIQPage;
              break;
            }
            this.subpages[13] = (IPersonaSecurityPage) this.AIQPage;
            break;
          }
          this.tabPageAIQ.Controls.Add((Control) this.AIQPage);
          if (this.session.StartupInfo.EnhancedConditionSettings)
          {
            this.subpages[14] = (IPersonaSecurityPage) this.AIQPage;
            break;
          }
          this.subpages[13] = (IPersonaSecurityPage) this.AIQPage;
          break;
        case "Developer Connect":
          if (this.developerConnectPage == null)
          {
            this.developerConnectPage = this.personal ? new DeveloperConnectPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new DeveloperConnectPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.developerConnectPage.TopLevel = false;
            this.developerConnectPage.Visible = true;
            this.developerConnectPage.Dock = DockStyle.Fill;
            this.developerConnectPage.BackColor = this.BackColor;
            this.tabPageDeveloperConnect.Controls.Add((Control) this.developerConnectPage);
            if (this.isBanker)
            {
              if (this.session.StartupInfo.EnhancedConditionSettings)
              {
                this.subpages[15] = (IPersonaSecurityPage) this.developerConnectPage;
                break;
              }
              this.subpages[14] = (IPersonaSecurityPage) this.developerConnectPage;
              break;
            }
            this.subpages[5] = (IPersonaSecurityPage) this.developerConnectPage;
            break;
          }
          this.tabPageDeveloperConnect.Controls.Add((Control) this.developerConnectPage);
          if (this.isBanker)
          {
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[15] = (IPersonaSecurityPage) this.developerConnectPage;
              break;
            }
            this.subpages[14] = (IPersonaSecurityPage) this.developerConnectPage;
            break;
          }
          this.subpages[5] = (IPersonaSecurityPage) this.developerConnectPage;
          break;
        case "Enhanced Conditions":
          if (this.enhancedConditionsPage == null)
          {
            this.enhancedConditionsPage = this.personal ? new EnhancedConditionsPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler, this.pipelinePage) : new EnhancedConditionsPage(this.session, this.currentPersonaId, this.pipelinePage, this.dirtyFlagChangedEventHandler);
            this.enhancedConditionsPage.Size = this.tabPageEnhancedConditions.Size;
            this.enhancedConditionsPage.TopLevel = false;
            this.enhancedConditionsPage.Visible = true;
            this.enhancedConditionsPage.Dock = DockStyle.Fill;
            this.enhancedConditionsPage.BackColor = this.BackColor;
            this.tabPageEnhancedConditions.Controls.Add((Control) this.enhancedConditionsPage);
            this.subpages[6] = (IPersonaSecurityPage) this.enhancedConditionsPage;
            break;
          }
          this.tabPageEnhancedConditions.Controls.Add((Control) this.enhancedConditionsPage);
          this.subpages[6] = (IPersonaSecurityPage) this.enhancedConditionsPage;
          break;
        case "External Settings":
          if (this.externalSettingsPage == null)
          {
            this.externalSettingsPage = this.personal ? new ExtSettingsPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new ExtSettingsPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.externalSettingsPage.TopLevel = false;
            this.externalSettingsPage.Visible = true;
            this.externalSettingsPage.Dock = DockStyle.Fill;
            this.externalSettingsPage.BackColor = this.BackColor;
            this.tabPageExtSettings.Controls.Add((Control) this.externalSettingsPage);
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[9] = (IPersonaSecurityPage) this.externalSettingsPage;
              break;
            }
            this.subpages[8] = (IPersonaSecurityPage) this.externalSettingsPage;
            break;
          }
          this.tabPageExtSettings.Controls.Add((Control) this.externalSettingsPage);
          if (this.session.StartupInfo.EnhancedConditionSettings)
          {
            this.subpages[9] = (IPersonaSecurityPage) this.externalSettingsPage;
            break;
          }
          this.subpages[8] = (IPersonaSecurityPage) this.externalSettingsPage;
          break;
        case "Forms/Tools":
          if (this.formsToolsPage == null)
          {
            if (this.pipelinePage == null)
              this.createPipelinePage();
            this.formsToolsPage = this.personal ? new FormsToolsConfig(this.session, this.userId, this.personas, this.pipelinePage, this.dirtyFlagChangedEventHandler) : new FormsToolsConfig(this.session, this.currentPersonaId, this.pipelinePage, this.dirtyFlagChangedEventHandler);
            this.formsToolsPage.Dock = DockStyle.Fill;
            this.tabPageFormsTools.Controls.Add((Control) this.formsToolsPage);
            this.subpages[4] = (IPersonaSecurityPage) this.formsToolsPage;
            break;
          }
          this.tabPageFormsTools.Controls.Add((Control) this.formsToolsPage);
          this.subpages[4] = (IPersonaSecurityPage) this.formsToolsPage;
          break;
        case "Home":
          if (this.homePage == null)
          {
            this.homePage = this.personal ? new EllieMae.EMLite.Setup.HomePage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new EllieMae.EMLite.Setup.HomePage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.homePage.TopLevel = false;
            this.homePage.Visible = true;
            this.homePage.Dock = DockStyle.Fill;
            this.homePage.BackColor = this.BackColor;
            this.tabPageHome.Controls.Add((Control) this.homePage);
            this.subpages[1] = (IPersonaSecurityPage) this.homePage;
            break;
          }
          this.tabPageHome.Controls.Add((Control) this.homePage);
          this.subpages[1] = (IPersonaSecurityPage) this.homePage;
          break;
        case "Loan":
          if (this.loansPage == null)
          {
            if (this.pipelinePage == null)
              this.createPipelinePage();
            this.loansPage = this.personal ? new LoansPage(this.session, this.userId, this.personas, this.pipelinePage, this.dirtyFlagChangedEventHandler) : new LoansPage(this.session, this.currentPersonaId, this.pipelinePage, this.dirtyFlagChangedEventHandler);
            this.loansPage.TopLevel = false;
            this.loansPage.Visible = true;
            this.loansPage.Dock = DockStyle.Fill;
            this.loansPage.BackColor = this.BackColor;
            this.tabPageLoans.Controls.Add((Control) this.loansPage);
            this.subpages[3] = (IPersonaSecurityPage) this.loansPage;
            break;
          }
          this.tabPageLoans.Controls.Add((Control) this.loansPage);
          this.subpages[3] = (IPersonaSecurityPage) this.loansPage;
          break;
        case "Pipeline":
          if (this.pipelinePage == null)
          {
            this.createPipelinePage();
            break;
          }
          break;
        case "Settings":
          if (this.settingsPage == null)
          {
            this.settingsPage = this.personal ? new SettingsPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new SettingsPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.settingsPage.TopLevel = false;
            this.settingsPage.Visible = true;
            this.settingsPage.Dock = DockStyle.Fill;
            this.settingsPage.BackColor = this.BackColor;
            this.tabPageSettings.Controls.Add((Control) this.settingsPage);
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[8] = (IPersonaSecurityPage) this.settingsPage;
              break;
            }
            this.subpages[7] = (IPersonaSecurityPage) this.settingsPage;
            break;
          }
          this.tabPageSettings.Controls.Add((Control) this.settingsPage);
          if (this.session.StartupInfo.EnhancedConditionSettings)
          {
            this.subpages[8] = (IPersonaSecurityPage) this.settingsPage;
            break;
          }
          this.subpages[7] = (IPersonaSecurityPage) this.settingsPage;
          break;
        case "TPO Connect":
          if (this.tpoAdminPage == null)
          {
            this.tpoAdminPage = this.personal ? new TPOAdminPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new TPOAdminPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.tpoAdminPage.TopLevel = false;
            this.tpoAdminPage.Visible = true;
            this.tpoAdminPage.Dock = DockStyle.Fill;
            this.tpoAdminPage.BackColor = this.BackColor;
          }
          this.tabPageTPOAdministration.Controls.Add((Control) this.tpoAdminPage);
          if (this.session.StartupInfo.EnhancedConditionSettings)
          {
            this.subpages[10] = (IPersonaSecurityPage) this.tpoAdminPage;
            break;
          }
          this.subpages[9] = (IPersonaSecurityPage) this.tpoAdminPage;
          break;
        case "Trades/Contacts/Dashboard/Reports":
          if (this.contactsPage == null)
          {
            this.contactsPage = this.personal ? new ContactsPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new ContactsPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.contactsPage.CreateLoanStatusChanged += new ContactsPage.OriginateLoanFeatureStatusChanged(this.contactsPage_CreateLoanStatusChanged);
            this.contactsPage.HasPipelineLoanTabAccessEvent += new ContactsPage.PersonaAccess(this.contactsPage_HasPipelineLoanTabAccessEvent);
            this.contactsPage.TopLevel = false;
            this.contactsPage.Visible = true;
            this.contactsPage.Dock = DockStyle.Fill;
            this.contactsPage.BackColor = this.BackColor;
            this.tabPageContacts.Controls.Add((Control) this.contactsPage);
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[7] = (IPersonaSecurityPage) this.contactsPage;
              break;
            }
            this.subpages[6] = (IPersonaSecurityPage) this.contactsPage;
            break;
          }
          this.tabPageContacts.Controls.Add((Control) this.contactsPage);
          if (this.session.StartupInfo.EnhancedConditionSettings)
          {
            this.subpages[7] = (IPersonaSecurityPage) this.contactsPage;
            break;
          }
          this.subpages[6] = (IPersonaSecurityPage) this.contactsPage;
          break;
        case "Web Version":
          if (this.loConnectPage == null)
          {
            if (this.pipelinePage == null)
              this.createPipelinePage();
            if (this.loConnectPage == null)
              this.loConnectPage = this.personal ? new LOConnectConfig(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new LOConnectConfig(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.loConnectPage.Dock = DockStyle.Fill;
            this.tabPageLOConnect.Controls.Add((Control) this.loConnectPage);
            if (this.isBanker)
            {
              if (this.session.StartupInfo.EnhancedConditionSettings)
              {
                this.subpages[13] = (IPersonaSecurityPage) this.loConnectPage;
                break;
              }
              this.subpages[12] = (IPersonaSecurityPage) this.loConnectPage;
              break;
            }
            this.subpages[4] = (IPersonaSecurityPage) this.loConnectPage;
            break;
          }
          this.loConnectPage.Dock = DockStyle.Fill;
          this.tabPageLOConnect.Controls.Add((Control) this.loConnectPage);
          if (this.isBanker)
          {
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[13] = (IPersonaSecurityPage) this.loConnectPage;
              break;
            }
            this.subpages[12] = (IPersonaSecurityPage) this.loConnectPage;
            break;
          }
          this.subpages[4] = (IPersonaSecurityPage) this.loConnectPage;
          break;
        case "eFolder":
          if (this.eFolderPage == null)
          {
            this.eFolderPage = this.personal ? new eFolderPage(this.session, this.userId, this.personas, this.pipelinePage, this.dirtyFlagChangedEventHandler) : new eFolderPage(this.session, this.currentPersonaId, this.pipelinePage, this.dirtyFlagChangedEventHandler);
            this.eFolderPage.TopLevel = false;
            this.eFolderPage.Visible = true;
            this.eFolderPage.Dock = DockStyle.Fill;
            this.eFolderPage.BackColor = this.BackColor;
            this.tabPageeFolder.Controls.Add((Control) this.eFolderPage);
            this.subpages[5] = (IPersonaSecurityPage) this.eFolderPage;
            break;
          }
          this.tabPageeFolder.Controls.Add((Control) this.eFolderPage);
          this.subpages[5] = (IPersonaSecurityPage) this.eFolderPage;
          break;
        case "eVault":
          if (this.eVaultPage == null)
          {
            this.eVaultPage = this.personal ? new EVaultPage(this.session, this.userId, this.personas, this.dirtyFlagChangedEventHandler) : new EVaultPage(this.session, this.currentPersonaId, this.dirtyFlagChangedEventHandler);
            this.eVaultPage.TopLevel = false;
            this.eVaultPage.Visible = true;
            this.eVaultPage.Dock = DockStyle.Fill;
            this.eVaultPage.BackColor = this.BackColor;
            this.tabPageEVault.Controls.Add((Control) this.eVaultPage);
            if (this.isBanker)
            {
              if (this.session.StartupInfo.EnhancedConditionSettings)
              {
                this.subpages[12] = (IPersonaSecurityPage) this.eVaultPage;
                break;
              }
              this.subpages[11] = (IPersonaSecurityPage) this.eVaultPage;
              break;
            }
            this.subpages[3] = (IPersonaSecurityPage) this.eVaultPage;
            break;
          }
          this.tabPageEVault.Controls.Add((Control) this.eVaultPage);
          if (this.isBanker)
          {
            if (this.session.StartupInfo.EnhancedConditionSettings)
            {
              this.subpages[12] = (IPersonaSecurityPage) this.eVaultPage;
              break;
            }
            this.subpages[11] = (IPersonaSecurityPage) this.eVaultPage;
            break;
          }
          this.subpages[3] = (IPersonaSecurityPage) this.eVaultPage;
          break;
      }
      this.forcedReadOnly();
    }

    private bool pipelinePage_HasContactOriginateLoanAccessEvent()
    {
      return this.contactsPage.HasOriginateLoanAccess();
    }

    private void contactsPage_CreateLoanStatusChanged(AclTriState state)
    {
      if (state == AclTriState.False || this.suspendEvent)
        return;
      this.pipelinePage.GrandAccessToPipelineLoanTab();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
    }
  }
}
