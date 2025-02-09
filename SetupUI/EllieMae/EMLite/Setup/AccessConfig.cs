// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AccessConfig
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AccessConfig : UserControl, IPersonaSecurityPage
  {
    private int personaID = -1;
    private Persona[] personas;
    private string userID = "";
    private Sessions.Session session;
    private bool isModified;
    private bool suspendEvent;
    private FeatureConfigsAclManager featureConfigAclMgr;
    private int currentPlatformAccessSelection;
    private VersionManagementGroup productionGroup;
    private ReturnResult clientInfo;
    private const string serverUri = "/EncompassSCWS/SmartClientService.asmx";
    private IContainer components;
    private BorderPanel borderPanel1;
    private GroupContainer groupContainer1;
    private RadioButton rdoBoth;
    private RadioButton rdoDesktop;
    private GradientPanel gradientPanel1;
    private Label label1;
    private Label lblPlatforms;
    private ImageList imgListTv;
    private GroupContainer groupContainerViewAccess;
    private Panel panelPlatformAccess;
    private Panel panelAccess;
    private CheckBox chkPipelineWebView;
    private CheckBox chkAnalysisTool;
    private Label lblPipelineWebView;
    private Label lblAnalysisTool;
    private CheckBox chkTradesWebView;
    private Label lblTradesWebView;
    private Label lblMsg;

    public event EventHandler DirtyFlagChanged;

    public void loadVersionManager()
    {
      if (AssemblyResolver.IsSmartClient)
      {
        List<AccessConfig.SCPackage> scPackageList = new List<AccessConfig.SCPackage>();
        int num = 0;
        using (SmartClientService smartClientService = new SmartClientService(AssemblyResolver.AuthServerURL + "/EncompassSCWS/SmartClientService.asmx"))
        {
          this.clientInfo = smartClientService.GetClientInfo(this.getSmartClientCID(), Session.CompanyInfo.Password);
          if (this.clientInfo.ReturnCode == ReturnCode.AuthenticationFailed)
            throw new Exception("Authentication request failed. Contact ICE Mortgage Technology Customer Support at 1-800-777-1718 for assistance.");
          if (this.clientInfo.ReturnCode != ReturnCode.Success)
            throw new Exception("Unexpected error returned by server");
          foreach (SCPackageInfo settings in smartClientService.GetSCPackageInfo(this.getSmartClientCID(), VersionInformation.CurrentVersion.Version.FullVersion))
            scPackageList.Add(new AccessConfig.SCPackage(settings, num++));
        }
        if (scPackageList.Count == 0)
          throw new Exception("No package information received");
        if (this.clientInfo.UpdateByEM)
        {
          this.rdoBoth.Enabled = true;
          this.lblMsg.Visible = false;
        }
        else
        {
          this.rdoBoth.Enabled = false;
          this.lblMsg.Visible = true;
        }
      }
      else
      {
        foreach (VersionManagementGroup versionManagementGroup in this.session.ServerManager.GetVersionManagementGroups())
        {
          if (versionManagementGroup.IsDefaultGroup)
            this.productionGroup = versionManagementGroup;
        }
        if (this.productionGroup == null)
          throw new Exception("Default version management group is missing. Version management feature cannot be used");
        if (this.productionGroup.AuthorizedVersion != null)
        {
          this.rdoBoth.Enabled = false;
          this.lblMsg.Visible = true;
        }
        else
        {
          this.rdoBoth.Enabled = true;
          this.lblMsg.Visible = false;
        }
      }
    }

    private string getSmartClientCID() => AssemblyResolver.SCClientID;

    public AccessConfig(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.suspendEvent = true;
      this.session = session;
      this.personaID = personaId;
      this.featureConfigAclMgr = (FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs);
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.loadPageForPersona();
      this.hideShowSectionAndControls();
      this.suspendEvent = false;
    }

    public AccessConfig(
      Sessions.Session session,
      string userID,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.suspendEvent = true;
      this.session = session;
      this.userID = userID;
      this.personas = personas;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.featureConfigAclMgr = (FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs);
      this.InitializeComponent();
      this.loadPageForUser();
      this.hideShowSectionAndControls();
      this.suspendEvent = false;
    }

    private void loadPageForUser()
    {
      this.loadPageForUserByFeature(AclFeature.PlatForm_Access);
      this.loadPageForUserByFeature(AclFeature.ThinThick_AnalysisTool_Access);
      this.loadPageForUserByFeature(AclFeature.ThinThick_PipelineTab_Access);
      this.loadPageForUserByFeature(AclFeature.ThinThick_TradesTab_Access);
    }

    private void loadPageForUserByFeature(AclFeature aclFeature)
    {
      AccessConfig.PersonaAccessRight personaAccessRight = this.getPersonaAccessRight(aclFeature);
      if (personaAccessRight.UserSpecificAccessRight < 0)
      {
        this.MakeReadOnly(true, aclFeature);
        this.switchLinkImageIndex(aclFeature, 1);
        this.setUserControlOption(personaAccessRight.PersonaSetting, aclFeature);
      }
      else
      {
        this.MakeReadOnly(false, aclFeature);
        this.switchLinkImageIndex(aclFeature, 0);
        this.setUserControlOption(personaAccessRight.UserSpecificAccessRight, aclFeature);
      }
    }

    private void switchLinkImageIndex(AclFeature aclFeature, int index)
    {
      switch (aclFeature)
      {
        case AclFeature.PlatForm_Access:
          this.lblPlatforms.ImageIndex = index;
          break;
        case AclFeature.ThinThick_AnalysisTool_Access:
          this.lblAnalysisTool.ImageIndex = index;
          break;
        case AclFeature.ThinThick_PipelineTab_Access:
          this.lblPipelineWebView.ImageIndex = index;
          break;
        case AclFeature.ThinThick_TradesTab_Access:
          this.lblTradesWebView.ImageIndex = index;
          break;
      }
    }

    private void loadPageForPersona()
    {
      this.loadPageForPersonaByFeature(AclFeature.PlatForm_Access);
      this.loadPageForPersonaByFeature(AclFeature.ThinThick_AnalysisTool_Access);
      this.loadPageForPersonaByFeature(AclFeature.ThinThick_PipelineTab_Access);
      this.loadPageForPersonaByFeature(AclFeature.ThinThick_TradesTab_Access);
    }

    private void loadPageForPersonaByFeature(AclFeature aclFeature)
    {
      this.setUserControlOption(this.featureConfigAclMgr.GetPermission(aclFeature, this.personaID), aclFeature);
      this.setStatus(false);
    }

    private AccessConfig.PersonaAccessRight getPersonaAccessRight(AclFeature aclFeature)
    {
      AccessConfig.PersonaAccessRight personaAccessRight = new AccessConfig.PersonaAccessRight();
      personaAccessRight.UserSpecificAccessRight = this.featureConfigAclMgr.GetPermission(aclFeature, this.userID);
      Dictionary<AclFeature, int> permissions = this.featureConfigAclMgr.GetPermissions(new AclFeature[1]
      {
        aclFeature
      }, this.personas);
      personaAccessRight.PersonaSetting = permissions[aclFeature];
      return personaAccessRight;
    }

    private int selectedPlatformAccess => this.rdoDesktop.Checked ? 0 : 1;

    public void SetPersona(int personaId)
    {
      this.suspendEvent = true;
      this.personaID = personaId;
      this.loadPageForPersona();
      this.suspendEvent = false;
    }

    private void hideShowSectionAndControls()
    {
      this.groupContainerViewAccess.Visible = false;
      this.chkPipelineWebView.Visible = AccessUtils.IsFeatureEnabled(Feature.WebPipeline, this.session);
      this.chkTradesWebView.Visible = AccessUtils.IsFeatureEnabled(Feature.WebTrading, this.session);
    }

    private void setUserControlOption(int accessRight, AclFeature aclFeature)
    {
      switch (aclFeature)
      {
        case AclFeature.PlatForm_Access:
          switch (accessRight)
          {
            case -1:
            case 0:
            case int.MaxValue:
              this.rdoDesktop.Checked = true;
              break;
            default:
              this.rdoBoth.Checked = true;
              break;
          }
          this.currentPlatformAccessSelection = accessRight;
          break;
        case AclFeature.ThinThick_AnalysisTool_Access:
          this.chkAnalysisTool.Checked = accessRight > 0;
          break;
        case AclFeature.ThinThick_PipelineTab_Access:
          this.chkPipelineWebView.Checked = accessRight > 0;
          break;
        case AclFeature.ThinThick_TradesTab_Access:
          this.chkTradesWebView.Checked = accessRight > 0;
          break;
      }
    }

    public bool IsDirty => this.isModified;

    private void setStatus(bool modified)
    {
      this.isModified = modified;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    public void Save()
    {
      if (!this.IsDirty)
        return;
      if (this.personas == null)
      {
        this.featureConfigAclMgr.SetPermission(AclFeature.PlatForm_Access, this.personaID, this.selectedPlatformAccess);
        this.featureConfigAclMgr.SetPermission(AclFeature.ThinThick_AnalysisTool_Access, this.personaID, this.chkAnalysisTool.Checked ? 1 : 0);
        this.featureConfigAclMgr.SetPermission(AclFeature.ThinThick_PipelineTab_Access, this.personaID, this.chkPipelineWebView.Checked ? 1 : 0);
        this.featureConfigAclMgr.SetPermission(AclFeature.ThinThick_TradesTab_Access, this.personaID, this.chkTradesWebView.Checked ? 1 : 0);
      }
      else
      {
        if (this.lblPlatforms.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.PlatForm_Access, this.userID, this.selectedPlatformAccess);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.PlatForm_Access, this.userID);
        if (this.lblAnalysisTool.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.ThinThick_AnalysisTool_Access, this.userID, this.chkAnalysisTool.Checked ? 1 : 0);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.ThinThick_AnalysisTool_Access, this.userID);
        if (this.lblPipelineWebView.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.ThinThick_PipelineTab_Access, this.userID, this.chkPipelineWebView.Checked ? 1 : 0);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.ThinThick_PipelineTab_Access, this.userID);
        if (this.lblTradesWebView.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.ThinThick_TradesTab_Access, this.userID, this.chkTradesWebView.Checked ? 1 : 0);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.ThinThick_TradesTab_Access, this.userID);
      }
      this.setStatus(false);
    }

    public void Reset()
    {
      this.suspendEvent = true;
      if (this.userID == "")
        this.loadPageForPersona();
      else
        this.loadPageForUser();
      this.suspendEvent = false;
      this.setStatus(false);
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.MakeReadOnly(makeReadOnly, AclFeature.PlatForm_Access);
      this.MakeReadOnly(makeReadOnly, AclFeature.ThinThick_AnalysisTool_Access);
      this.MakeReadOnly(makeReadOnly, AclFeature.ThinThick_PipelineTab_Access);
      this.MakeReadOnly(makeReadOnly, AclFeature.ThinThick_TradesTab_Access);
    }

    public void MakeReadOnly(bool makeReadOnly, AclFeature aclFeature)
    {
      switch (aclFeature)
      {
        case AclFeature.PlatForm_Access:
          this.rdoBoth.Enabled = this.rdoDesktop.Enabled = !makeReadOnly;
          Label lblPlatforms = this.lblPlatforms;
          RadioButton rdoBoth = this.rdoBoth;
          ContextMenu contextMenu1;
          this.rdoDesktop.ContextMenu = contextMenu1 = !makeReadOnly ? this.createContextMenu(aclFeature) : (ContextMenu) null;
          ContextMenu contextMenu2;
          ContextMenu contextMenu3 = contextMenu2 = contextMenu1;
          rdoBoth.ContextMenu = contextMenu2;
          ContextMenu contextMenu4 = contextMenu3;
          lblPlatforms.ContextMenu = contextMenu4;
          break;
        case AclFeature.ThinThick_AnalysisTool_Access:
          this.chkAnalysisTool.Enabled = !makeReadOnly;
          this.lblAnalysisTool.ContextMenu = this.chkAnalysisTool.ContextMenu = !makeReadOnly ? this.createContextMenu(aclFeature) : (ContextMenu) null;
          break;
        case AclFeature.ThinThick_PipelineTab_Access:
          this.chkPipelineWebView.Enabled = !makeReadOnly;
          this.lblPipelineWebView.ContextMenu = this.chkPipelineWebView.ContextMenu = !makeReadOnly ? this.createContextMenu(aclFeature) : (ContextMenu) null;
          break;
        case AclFeature.ThinThick_TradesTab_Access:
          this.chkTradesWebView.Enabled = !makeReadOnly;
          this.lblTradesWebView.ContextMenu = this.chkTradesWebView.ContextMenu = !makeReadOnly ? this.createContextMenu(aclFeature) : (ContextMenu) null;
          break;
      }
    }

    private void rdo_PlatformAccessCheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setStatus(true);
      if (this.userID != "")
        this.lblPlatforms.ImageIndex = 0;
      if ((this.currentPlatformAccessSelection == 0 || this.currentPlatformAccessSelection == -1 || this.currentPlatformAccessSelection == int.MaxValue) && this.rdoBoth.Checked)
      {
        this.currentPlatformAccessSelection = 1;
      }
      else
      {
        if (!this.rdoDesktop.Checked)
          return;
        this.currentPlatformAccessSelection = 0;
      }
    }

    private void chkAnalysisTool_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.lblAnalysisTool.ImageIndex = 0;
      this.setStatus(true);
    }

    private void chkPipelineWebView_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.lblPipelineWebView.ImageIndex = 0;
      this.setStatus(true);
    }

    private void chkTradesWebView_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.lblTradesWebView.ImageIndex = 0;
      this.setStatus(true);
    }

    private void miDefaultLinkTo_Click(object sender, EventArgs e)
    {
      if (this.userID == "")
        return;
      this.suspendEvent = true;
      switch ((AclFeature) ((MenuItem) sender).Parent.Tag)
      {
        case AclFeature.PlatForm_Access:
          this.lblPlatforms.ImageIndex = 1;
          this.setUserControlOption(this.getPersonaAccessRight(AclFeature.PlatForm_Access).PersonaSetting, AclFeature.PlatForm_Access);
          break;
        case AclFeature.ThinThick_AnalysisTool_Access:
          this.lblAnalysisTool.ImageIndex = 1;
          this.setUserControlOption(this.getPersonaAccessRight(AclFeature.ThinThick_AnalysisTool_Access).PersonaSetting, AclFeature.ThinThick_AnalysisTool_Access);
          break;
        case AclFeature.ThinThick_PipelineTab_Access:
          this.lblPipelineWebView.ImageIndex = 1;
          this.setUserControlOption(this.getPersonaAccessRight(AclFeature.ThinThick_PipelineTab_Access).PersonaSetting, AclFeature.ThinThick_PipelineTab_Access);
          break;
        case AclFeature.ThinThick_TradesTab_Access:
          this.lblTradesWebView.ImageIndex = 1;
          this.setUserControlOption(this.getPersonaAccessRight(AclFeature.ThinThick_TradesTab_Access).PersonaSetting, AclFeature.ThinThick_TradesTab_Access);
          break;
      }
      this.suspendEvent = false;
      this.setStatus(true);
    }

    private void miDefaultDisconFrom_Click(object sender, EventArgs e)
    {
      if (this.userID == "")
        return;
      switch ((AclFeature) ((MenuItem) sender).Parent.Tag)
      {
        case AclFeature.PlatForm_Access:
          this.lblPlatforms.ImageIndex = 0;
          break;
        case AclFeature.ThinThick_AnalysisTool_Access:
          this.lblAnalysisTool.ImageIndex = 0;
          break;
        case AclFeature.ThinThick_PipelineTab_Access:
          this.lblPipelineWebView.ImageIndex = 0;
          break;
        case AclFeature.ThinThick_TradesTab_Access:
          this.lblTradesWebView.ImageIndex = 0;
          break;
      }
      this.setStatus(true);
    }

    private ContextMenu createContextMenu(AclFeature aclFeature)
    {
      MenuItem menuItem1 = new MenuItem();
      MenuItem menuItem2 = new MenuItem();
      ContextMenu contextMenu = new ContextMenu();
      MenuItem menuItem3 = new MenuItem();
      MenuItem menuItem4 = new MenuItem();
      menuItem1.Index = 0;
      menuItem1.Text = "Link with Persona Rights";
      menuItem2.Index = 1;
      menuItem2.Text = "Disconnect from Persona Rights";
      contextMenu.MenuItems.AddRange(new MenuItem[2]
      {
        menuItem3,
        menuItem4
      });
      menuItem3.Index = 0;
      menuItem3.Text = "Link with Persona Rights";
      menuItem3.Click += new EventHandler(this.miDefaultLinkTo_Click);
      menuItem4.Index = 1;
      menuItem4.Text = "Disconnect from Persona Rights";
      menuItem4.Click += new EventHandler(this.miDefaultDisconFrom_Click);
      contextMenu.Tag = (object) aclFeature;
      return contextMenu;
    }

    private void label2_Click(object sender, EventArgs e)
    {
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AccessConfig));
      this.borderPanel1 = new BorderPanel();
      this.groupContainer1 = new GroupContainer();
      this.panelAccess = new Panel();
      this.groupContainerViewAccess = new GroupContainer();
      this.lblTradesWebView = new Label();
      this.imgListTv = new ImageList(this.components);
      this.chkTradesWebView = new CheckBox();
      this.lblPipelineWebView = new Label();
      this.lblAnalysisTool = new Label();
      this.chkPipelineWebView = new CheckBox();
      this.chkAnalysisTool = new CheckBox();
      this.panelPlatformAccess = new Panel();
      this.lblPlatforms = new Label();
      this.rdoDesktop = new RadioButton();
      this.rdoBoth = new RadioButton();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.lblMsg = new Label();
      this.borderPanel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panelAccess.SuspendLayout();
      this.groupContainerViewAccess.SuspendLayout();
      this.panelPlatformAccess.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.borderPanel1.Controls.Add((Control) this.groupContainer1);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(582, 353);
      this.borderPanel1.TabIndex = 0;
      this.groupContainer1.Borders = AnchorStyles.None;
      this.groupContainer1.Controls.Add((Control) this.panelAccess);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(1, 1);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(580, 351);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Encompass Access";
      this.panelAccess.Controls.Add((Control) this.groupContainerViewAccess);
      this.panelAccess.Controls.Add((Control) this.panelPlatformAccess);
      this.panelAccess.Dock = DockStyle.Fill;
      this.panelAccess.Location = new Point(0, 56);
      this.panelAccess.Name = "panelAccess";
      this.panelAccess.Size = new Size(580, 295);
      this.panelAccess.TabIndex = 7;
      this.groupContainerViewAccess.Controls.Add((Control) this.lblTradesWebView);
      this.groupContainerViewAccess.Controls.Add((Control) this.chkTradesWebView);
      this.groupContainerViewAccess.Controls.Add((Control) this.lblPipelineWebView);
      this.groupContainerViewAccess.Controls.Add((Control) this.lblAnalysisTool);
      this.groupContainerViewAccess.Controls.Add((Control) this.chkPipelineWebView);
      this.groupContainerViewAccess.Controls.Add((Control) this.chkAnalysisTool);
      this.groupContainerViewAccess.Dock = DockStyle.Fill;
      this.groupContainerViewAccess.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerViewAccess.Location = new Point(0, 148);
      this.groupContainerViewAccess.Name = "groupContainerViewAccess";
      this.groupContainerViewAccess.Size = new Size(580, 147);
      this.groupContainerViewAccess.TabIndex = 6;
      this.groupContainerViewAccess.Text = "Web View Access";
      this.groupContainerViewAccess.Visible = false;
      this.lblTradesWebView.AutoSize = true;
      this.lblTradesWebView.ImageAlign = ContentAlignment.MiddleRight;
      this.lblTradesWebView.ImageList = this.imgListTv;
      this.lblTradesWebView.Location = new Point(217, 87);
      this.lblTradesWebView.Name = "lblTradesWebView";
      this.lblTradesWebView.Size = new Size(28, 13);
      this.lblTradesWebView.TabIndex = 10;
      this.lblTradesWebView.Text = "       ";
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.chkTradesWebView.AutoSize = true;
      this.chkTradesWebView.Location = new Point(7, 86);
      this.chkTradesWebView.Name = "chkTradesWebView";
      this.chkTradesWebView.Size = new Size(202, 17);
      this.chkTradesWebView.TabIndex = 8;
      this.chkTradesWebView.Text = "Enable EncompassWeb Trades View";
      this.chkTradesWebView.UseVisualStyleBackColor = true;
      this.chkTradesWebView.CheckedChanged += new EventHandler(this.chkTradesWebView_CheckedChanged);
      this.lblPipelineWebView.AutoSize = true;
      this.lblPipelineWebView.ImageAlign = ContentAlignment.MiddleRight;
      this.lblPipelineWebView.ImageList = this.imgListTv;
      this.lblPipelineWebView.Location = new Point(217, 63);
      this.lblPipelineWebView.Name = "lblPipelineWebView";
      this.lblPipelineWebView.Size = new Size(28, 13);
      this.lblPipelineWebView.TabIndex = 6;
      this.lblPipelineWebView.Text = "       ";
      this.lblAnalysisTool.AutoSize = true;
      this.lblAnalysisTool.ImageAlign = ContentAlignment.MiddleRight;
      this.lblAnalysisTool.ImageList = this.imgListTv;
      this.lblAnalysisTool.Location = new Point(135, 41);
      this.lblAnalysisTool.Name = "lblAnalysisTool";
      this.lblAnalysisTool.Size = new Size(28, 13);
      this.lblAnalysisTool.TabIndex = 5;
      this.lblAnalysisTool.Text = "       ";
      this.chkPipelineWebView.AutoSize = true;
      this.chkPipelineWebView.Location = new Point(7, 62);
      this.chkPipelineWebView.Name = "chkPipelineWebView";
      this.chkPipelineWebView.Size = new Size(206, 17);
      this.chkPipelineWebView.TabIndex = 7;
      this.chkPipelineWebView.Text = "Enable EncompassWeb Pipeline View";
      this.chkPipelineWebView.UseVisualStyleBackColor = true;
      this.chkPipelineWebView.CheckedChanged += new EventHandler(this.chkPipelineWebView_CheckedChanged);
      this.chkAnalysisTool.AutoSize = true;
      this.chkAnalysisTool.Location = new Point(7, 38);
      this.chkAnalysisTool.Name = "chkAnalysisTool";
      this.chkAnalysisTool.Size = new Size(124, 17);
      this.chkAnalysisTool.TabIndex = 5;
      this.chkAnalysisTool.Text = "Enable Analysis Tool";
      this.chkAnalysisTool.UseVisualStyleBackColor = true;
      this.chkAnalysisTool.CheckedChanged += new EventHandler(this.chkAnalysisTool_CheckedChanged);
      this.panelPlatformAccess.Controls.Add((Control) this.lblMsg);
      this.panelPlatformAccess.Controls.Add((Control) this.lblPlatforms);
      this.panelPlatformAccess.Controls.Add((Control) this.rdoDesktop);
      this.panelPlatformAccess.Controls.Add((Control) this.rdoBoth);
      this.panelPlatformAccess.Dock = DockStyle.Top;
      this.panelPlatformAccess.Location = new Point(0, 0);
      this.panelPlatformAccess.Name = "panelPlatformAccess";
      this.panelPlatformAccess.Size = new Size(580, 118);
      this.panelPlatformAccess.TabIndex = 5;
      this.lblPlatforms.AutoSize = true;
      this.lblPlatforms.ImageAlign = ContentAlignment.MiddleRight;
      this.lblPlatforms.ImageList = this.imgListTv;
      this.lblPlatforms.Location = new Point(4, 5);
      this.lblPlatforms.Name = "lblPlatforms";
      this.lblPlatforms.Size = new Size(82, 13);
      this.lblPlatforms.TabIndex = 4;
      this.lblPlatforms.Text = "Select one:       ";
      this.rdoDesktop.AutoSize = true;
      this.rdoDesktop.Location = new Point(7, 20);
      this.rdoDesktop.Name = "rdoDesktop";
      this.rdoDesktop.Size = new Size(202, 17);
      this.rdoDesktop.TabIndex = 1;
      this.rdoDesktop.TabStop = true;
      this.rdoDesktop.Text = "Desktop version of Encompass";
      this.rdoDesktop.UseVisualStyleBackColor = true;
      this.rdoDesktop.CheckedChanged += new EventHandler(this.rdo_PlatformAccessCheckedChanged);
      this.rdoBoth.AutoSize = true;
      this.rdoBoth.Location = new Point(7, 43);
      this.rdoBoth.Name = "rdoBoth";
      this.rdoBoth.Size = new Size(423, 17);
      this.rdoBoth.TabIndex = 3;
      this.rdoBoth.TabStop = true;
      this.rdoBoth.Text = "Both desktop and web versions of Encompass (web version allows browser based access across desktop, tablet, and mobile devices)";
      this.rdoBoth.UseVisualStyleBackColor = true;
      this.rdoBoth.CheckedChanged += new EventHandler(this.rdo_PlatformAccessCheckedChanged);
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 25);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(580, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(8, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(276, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Indicate what versions of Encompass, users with this persona can access:";
      this.lblMsg.AutoSize = true;
      this.lblMsg.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMsg.Location = new Point(4, 63);
      this.lblMsg.MaximumSize = new Size(550, 0);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new Size(548, 39);
      this.lblMsg.TabIndex = 6;
      this.lblMsg.Text = componentResourceManager.GetString("lblMsg.Text");
      this.lblMsg.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.borderPanel1);
      this.Name = nameof (AccessConfig);
      this.Size = new Size(582, 353);
      this.borderPanel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.panelAccess.ResumeLayout(false);
      this.groupContainerViewAccess.ResumeLayout(false);
      this.groupContainerViewAccess.PerformLayout();
      this.panelPlatformAccess.ResumeLayout(false);
      this.panelPlatformAccess.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private class SCPackage
    {
      public readonly SCPackageInfo Settings;
      public readonly int Index;

      public SCPackage(SCPackageInfo settings, int index)
      {
        this.Settings = settings;
        this.Index = index;
      }
    }

    private struct PersonaAccessRight
    {
      public int UserSpecificAccessRight { get; set; }

      public int PersonaSetting { get; set; }
    }
  }
}
