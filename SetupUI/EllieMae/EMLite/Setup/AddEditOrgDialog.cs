// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddEditOrgDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.OverNightRateProtection;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddEditOrgDialog : Form, IHelp
  {
    private const string className = "AddEditOrgDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private TextBox orgNameTxt;
    private TextBox orgDescTxt;
    private Label label1;
    private Label label2;
    private Button okBtn;
    private Button cancelBtn;
    private System.ComponentModel.Container components;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private TextBox companyTxt;
    private TextBox cityTxt;
    private TextBox stateTxt;
    private TextBox zipTxt;
    private TextBox phoneTxt;
    private TextBox faxTxt;
    private TextBox address1Txt;
    private TextBox address2Txt;
    private TextBox orgCodeTxt;
    private Label label10;
    private CheckBox parentBox;
    private LOCompCurrentControl loCompCurrentControl;
    private LOCompHistoryControl loCompHistoryControl;
    private ONRPBranchSettingsCtrl ONRPBranchSettingsControl;
    private CCSiteControl ccSiteControl;
    private IOrganizationManager rOrg;
    private IConfigurationManager configMgr;
    private FeaturesAclManager aclManager;
    private bool deleteBackKey;
    private string orgName = string.Empty;
    private string orgDescription = string.Empty;
    private string orgCode = string.Empty;
    private string nmlsCode = string.Empty;
    private bool showOrgInLOSearch;
    private string loSearch_OrgName = string.Empty;
    private int hmdaProfileId;
    private string mersminCode = string.Empty;
    private string[] dbNames;
    private string orgCoName = string.Empty;
    private string orgAddress1 = string.Empty;
    private string orgAddress2 = string.Empty;
    private string orgUnitType = string.Empty;
    private string orgCity = string.Empty;
    private string orgState = string.Empty;
    private string orgZip = string.Empty;
    private string orgPhone = string.Empty;
    private string orgFax = string.Empty;
    private BranchExtLicensing orgBranchLicensing;
    private LoanCompHistoryList loanCompHistoryList;
    private ONRPEntitySettings ONRPBranchSettings;
    private CCSiteInfo ccSiteSettings;
    private SSOInfo ssoSettings;
    private Sessions.Session session;
    public Dictionary<string, string> ccSitesLookup;
    private int oid = -1;
    private EMHelpLink emHelpLink1;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer3;
    private Label label11;
    private Label label12;
    private TextBox txtNMLS;
    private TextBox txtMERSMIN;
    private CheckBox chkNMLS;
    private CheckBox chkMERSMIN;
    private Panel panelLicense;
    private int parentID = -1;
    private int parentIDforCCSite = -1;
    private Button btnAddDBA;
    private StandardIconButton iconBtnSelect;
    private Panel panelDBA4;
    private Label label16;
    private TextBox txtDBAName4;
    private Panel panelDBA3;
    private Label label15;
    private TextBox txtDBAName3;
    private Panel panelDBA2;
    private Label label14;
    private TextBox txtDBAName2;
    private Panel panelDBA1;
    private Label label13;
    private TextBox txtDBAName1;
    private Panel panelAddMore;
    private Panel panelLOCompLatest;
    private Panel panelLOCompHistory;
    private Panel panelTop;
    private Panel panelMiddle;
    private Panel panelBottom;
    private Panel panel1;
    private Panel panelONRPSettings;
    private GroupContainer groupContainer4;
    private CheckBox chkShowOrgInLOSearch;
    private CheckBox chkCCLOSearch;
    private TextBox txtLOSearchOrgName;
    private Label label17;
    private Panel panel2;
    private CheckBox chkLEI;
    private TextBox txtLEI;
    private Label label18;
    private TextBox txtHMDAProfile;
    private Label label19;
    private Panel panel3;
    private StandardIconButton stdButtonViewHMDAProfile;
    private StandardIconButton stdButtonNewHMDAProfile;
    private StandardIconButton stdButtonEditHMDAProfile;
    private GroupContainer groupContainer6;
    private Panel panelCCSite;
    private AddEditOrgLicenseControl licenseControl;
    private AddEditOrgSSOControl SSOControl;
    private HMDAProfile profile;
    private bool hasAddLEIPermission;
    private Label label20;
    private bool hasEditLEIPermission;
    private Label unitTypeLabel;
    private ComboBoxEx unitTypeCombobox;
    private SplitContainer splitContainer1;
    private Panel pnlSSO;
    private SplitContainer splitContainer2;
    private bool hasHMDAProfilePermission;

    public string OrgName => this.orgName;

    public string OrgDescription => this.orgDescription;

    public string OrgCode => this.orgCode;

    public string NMLSCode => this.nmlsCode;

    public bool ShowOrgInLOSearch => this.showOrgInLOSearch;

    public string LOSearch_OrgName => this.loSearch_OrgName;

    public int HMDAProfileId => this.hmdaProfileId;

    public string MERSMinCode => this.mersminCode;

    public string[] DBNames => this.dbNames;

    public string OrgCoName => this.orgCoName;

    public string OrgAddress1 => this.orgAddress1;

    public string OrgAddress2 => this.orgAddress2;

    public string OrgUnitType => this.orgUnitType;

    public string OrgCity => this.orgCity;

    public string OrgState => this.orgState;

    public string OrgZip => this.orgZip;

    public string OrgPhone => this.orgPhone;

    public string OrgFax => this.orgFax;

    public BranchExtLicensing OrgBranchLicensing => this.orgBranchLicensing;

    public LoanCompHistoryList LOCompHistoryList => this.loanCompHistoryList;

    public ONRPEntitySettings ONRPRetailBranchSettings => this.ONRPBranchSettings;

    public CCSiteInfo CCSiteSettings => this.ccSiteSettings;

    public SSOInfo SSOSettings => this.ssoSettings;

    public AddEditOrgDialog(Sessions.Session session, int oid, int parentID)
    {
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.configMgr = this.session.ConfigurationManager;
      this.aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.hasAddLEIPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_OrgUserAddLEI);
      this.hasEditLEIPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_OrgUserEditLEI);
      this.hasHMDAProfilePermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_HMDASetup);
      this.oid = oid;
      this.parentID = parentID;
      this.InitializeComponent();
      Rectangle bounds = Screen.FromControl((Control) this).Bounds;
      if (this.Height > bounds.Height)
      {
        this.AutoScroll = true;
        this.Height = bounds.Height - 50;
        this.Width += 10;
      }
      this.licenseControl = new AddEditOrgLicenseControl(false, this.rOrg, parentID, oid);
      this.panelLicense.Controls.Add((Control) this.licenseControl);
      this.ONRPBranchSettingsControl = new ONRPBranchSettingsCtrl(this.session, oid);
      this.ONRPBranchSettingsControl.Dock = DockStyle.Fill;
      this.panelONRPSettings.Controls.Add((Control) this.ONRPBranchSettingsControl);
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.loCompCurrentControl = new LOCompCurrentControl(this.session, false, false);
        this.panelLOCompLatest.Controls.Add((Control) this.loCompCurrentControl);
        this.loCompHistoryControl = new LOCompHistoryControl(this.session, false, false);
        this.panelLOCompHistory.Controls.Add((Control) this.loCompHistoryControl);
        this.loanCompHistoryList = new LoanCompHistoryList(oid.ToString());
        this.loCompCurrentControl.RefreshDate(this.loanCompHistoryList, parentID.ToString(), oid.ToString());
        this.loCompHistoryControl.RefreshData(this.loanCompHistoryList, parentID.ToString(), oid.ToString());
        this.loCompHistoryControl.HistorySelectedIndexChanged += new EventHandler(this.loCompHistoryControl_HistorySelectedIndexChanged);
        this.loCompHistoryControl.UseParentInfoClicked += new EventHandler(this.loCompCurrentControl_UseParentInfoClicked);
        this.loCompHistoryControl.AssignPlanButtonClicked += new EventHandler(this.loCompHistoryControl_AssignPlanButtonClicked);
        this.loCompCurrentControl.StartDateChanged += new EventHandler(this.loCompCurrentControl_StartDateChanged);
      }
      else
      {
        this.panelMiddle.Visible = false;
        this.Height = this.panelBottom.Top + this.panelBottom.Height;
      }
      OrgInfo organization = this.rOrg.GetOrganization(parentID);
      organization.CCSiteSettings.Id = "";
      organization.CCSiteSettings.SiteId = "";
      organization.CCSiteSettings.Url = "";
      if (organization == null)
      {
        this.licenseControl.RefreshData((BranchExtLicensing) null);
        this.licenseControl.ReloadStateView();
      }
      else
      {
        this.ONRPBranchSettings = organization.ONRPRetailBranchSettings;
        this.ONRPBranchSettingsControl.RefreshData(this.ONRPRetailBranchSettings, parentID);
        this.ccSiteControl = new CCSiteControl(this.session, organization, true);
        this.ccSiteControl.Dock = DockStyle.Fill;
        this.panelCCSite.Controls.Add((Control) this.ccSiteControl);
        this.splitContainer1.Panel1Collapsed = !this.session.StartupInfo.EnableSSO;
        this.SSOControl = new AddEditOrgSSOControl(this.session, organization, true);
        if (!this.IsRestrictedAccessEnabled())
        {
          foreach (Control control in (ArrangedElementCollection) this.SSOControl.Controls)
            control.Enabled = false;
        }
        this.pnlSSO.Controls.Add((Control) this.SSOControl);
        this.orgCodeTxt.Text = organization.OrgCode;
        this.parentBox.CheckedChanged += new EventHandler(this.parentBox_CheckedChanged);
        this.chkNMLS.CheckedChanged += new EventHandler(this.chkNMLS_CheckedChanged);
        this.chkCCLOSearch.CheckedChanged += new EventHandler(this.chkCCLOSearch_CheckedChanged);
        this.chkMERSMIN.CheckedChanged += new EventHandler(this.chkMERSMIN_CheckedChanged);
        this.chkLEI.CheckedChanged += new EventHandler(this.chkLEI_CheckedChanged);
        this.ccSiteSettings = this.rOrg.getCCSiteInfo(oid);
        this.ssoSettings = organization.SSOSettings;
        this.getParentInfo(parentID, this.parentBox);
        this.getParentInfo(parentID, this.chkNMLS);
        this.getParentInfo(parentID, this.chkMERSMIN);
        this.getParentInfo(parentID, this.chkCCLOSearch);
        this.getParentInfo(parentID, this.chkLEI);
        this.getParentStateLicensingInfo(parentID);
        if (organization.ONRPRetailBranchSettings.UseParentInfo)
          this.getParentONRPInfo(parentID);
        this.parentBox.Enabled = true;
        this.parentBox.Checked = true;
        this.chkMERSMIN.Enabled = true;
        this.chkMERSMIN.Checked = true;
        this.chkNMLS.Enabled = true;
        this.chkNMLS.Checked = true;
        this.chkCCLOSearch.Enabled = true;
        this.chkCCLOSearch.Enabled = this.chkCCLOSearch.Checked = true;
        this.chkLEI.Enabled = this.chkLEI.Checked = true;
        this.stdButtonNewHMDAProfile.Enabled = false;
        if (oid == -1)
        {
          this.chkCCLOSearch.Checked = false;
          this.txtLOSearchOrgName.Enabled = false;
        }
        else
        {
          this.chkCCLOSearch.Checked = true;
          this.txtLOSearchOrgName.Enabled = true;
        }
        if (this.hasAddLEIPermission)
          this.stdButtonNewHMDAProfile.Enabled = true;
        else
          this.stdButtonNewHMDAProfile.Enabled = false;
        if (this.hasEditLEIPermission)
          this.stdButtonEditHMDAProfile.Enabled = true;
        else
          this.stdButtonEditHMDAProfile.Enabled = false;
        if (this.hasHMDAProfilePermission)
          this.stdButtonViewHMDAProfile.Enabled = true;
        else
          this.stdButtonViewHMDAProfile.Enabled = false;
        this.changeFieldStatus(true, this.parentBox);
        this.changeFieldStatus(true, this.chkNMLS);
        this.changeFieldStatus(this.chkCCLOSearch.Checked, this.chkCCLOSearch);
        this.changeFieldStatus(true, this.chkMERSMIN);
        this.changeFieldStatus(this.chkLEI.Checked, this.chkLEI);
        if (this.session.StartupInfo.AllowURLA2020)
          return;
        this.unitTypeCombobox.Hide();
        this.unitTypeLabel.Hide();
        this.address2Txt.Location = new Point(89, 70);
        this.address2Txt.Size = new Size(248, 20);
      }
    }

    private void getParentONRPInfo(int uId)
    {
      this.ONRPBranchSettingsControl.ReloadParentONRPInfo(uId);
    }

    private void chkMERSMIN_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkMERSMIN.Checked)
        this.getParentInfo((this.oid != -1 || this.parentID == -1 ? this.rOrg.GetOrganization(this.oid) : this.rOrg.GetOrganization(this.parentID)).Parent, this.chkMERSMIN);
      else
        this.changeFieldStatus(false, this.chkMERSMIN);
    }

    private void chkNMLS_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkNMLS.Checked)
        this.getParentInfo((this.oid != -1 || this.parentID == -1 ? this.rOrg.GetOrganization(this.oid) : this.rOrg.GetOrganization(this.parentID)).Parent, this.chkNMLS);
      else
        this.changeFieldStatus(false, this.chkNMLS);
    }

    private void chkLEI_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkLEI.Checked)
      {
        this.getParentInfo((this.oid != -1 || this.parentID == -1 ? this.rOrg.GetOrganization(this.oid) : this.rOrg.GetOrganization(this.parentID)).Parent, this.chkLEI);
        this.stdButtonNewHMDAProfile.Enabled = this.stdButtonEditHMDAProfile.Enabled = false;
        this.stdButtonViewHMDAProfile.Enabled = true;
      }
      else
      {
        this.profile = (HMDAProfile) null;
        this.txtHMDAProfile.Text = this.txtLEI.Text = string.Empty;
        this.changeFieldStatus(false, this.chkLEI);
        if (this.profile != null)
        {
          this.stdButtonEditHMDAProfile.Enabled = this.hasEditLEIPermission;
          this.stdButtonNewHMDAProfile.Enabled = this.hasAddLEIPermission;
          this.stdButtonViewHMDAProfile.Enabled = true;
        }
        else
        {
          this.stdButtonEditHMDAProfile.Enabled = false;
          this.stdButtonViewHMDAProfile.Enabled = false;
          this.stdButtonNewHMDAProfile.Enabled = this.hasAddLEIPermission;
        }
      }
    }

    private void chkCCLOSearch_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkCCLOSearch.Checked)
        this.getParentInfo((this.oid != -1 || this.parentID == -1 ? this.rOrg.GetOrganization(this.oid) : this.rOrg.GetOrganization(this.parentID)).Parent, this.chkCCLOSearch);
      else
        this.changeFieldStatus(false, this.chkCCLOSearch);
    }

    public AddEditOrgDialog(Sessions.Session session, int oid, bool readOnly)
    {
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.configMgr = this.session.ConfigurationManager;
      this.aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.hasAddLEIPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_OrgUserAddLEI);
      this.hasEditLEIPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_OrgUserEditLEI);
      this.hasHMDAProfilePermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_HMDASetup);
      this.oid = oid;
      this.InitializeComponent();
      Rectangle bounds = Screen.FromControl((Control) this).Bounds;
      if (this.Height > bounds.Height)
      {
        this.AutoScroll = true;
        this.Height = bounds.Height - 50;
        this.Width += 10;
      }
      this.licenseControl = new AddEditOrgLicenseControl(false, this.rOrg, this.parentID, oid);
      this.panelLicense.Controls.Add((Control) this.licenseControl);
      this.licenseControl.RefreshData((BranchExtLicensing) null);
      this.licenseControl.ReloadStateView();
      this.ONRPBranchSettingsControl = new ONRPBranchSettingsCtrl(this.session, oid);
      this.ONRPBranchSettingsControl.Dock = DockStyle.Fill;
      this.panelONRPSettings.Controls.Add((Control) this.ONRPBranchSettingsControl);
      if (!this.session.StartupInfo.AllowURLA2020)
      {
        this.unitTypeCombobox.Hide();
        this.unitTypeLabel.Hide();
        this.address2Txt.Location = new Point(89, 70);
        this.address2Txt.Size = new Size(248, 20);
      }
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.loCompCurrentControl = new LOCompCurrentControl(this.session, false, false);
        this.panelLOCompLatest.Controls.Add((Control) this.loCompCurrentControl);
        this.loCompHistoryControl = new LOCompHistoryControl(this.session, false, false);
        this.panelLOCompHistory.Controls.Add((Control) this.loCompHistoryControl);
      }
      if (readOnly)
      {
        foreach (Control control in (ArrangedElementCollection) this.Controls)
        {
          switch (control)
          {
            case TextBox _:
              ((TextBoxBase) control).ReadOnly = true;
              continue;
            case Button _:
            case GroupBox _:
            case CheckBox _:
              control.Enabled = false;
              continue;
            default:
              continue;
          }
        }
        this.licenseControl.SetReadOnly(readOnly);
        if (this.loCompCurrentControl != null)
          this.loCompCurrentControl.SetReadOnly(readOnly);
        if (this.loCompHistoryControl != null)
          this.loCompHistoryControl.SetReadOnly(readOnly);
        this.cancelBtn.Enabled = true;
      }
      if (this.loCompHistoryControl != null)
      {
        this.loCompHistoryControl.HistorySelectedIndexChanged += new EventHandler(this.loCompHistoryControl_HistorySelectedIndexChanged);
        this.loCompHistoryControl.UseParentInfoClicked += new EventHandler(this.loCompCurrentControl_UseParentInfoClicked);
        this.loCompHistoryControl.AssignPlanButtonClicked += new EventHandler(this.loCompHistoryControl_AssignPlanButtonClicked);
        this.loCompCurrentControl.StartDateChanged += new EventHandler(this.loCompCurrentControl_StartDateChanged);
      }
      else
      {
        this.panelMiddle.Visible = false;
        this.Height = this.panelTop.Height + this.panelBottom.Height * 2;
      }
      if (oid < 0)
      {
        this.parentBox.Enabled = true;
        this.parentBox.Checked = true;
        this.chkNMLS.Enabled = true;
        this.chkNMLS.Checked = true;
        this.chkMERSMIN.Enabled = true;
        this.chkMERSMIN.Checked = true;
        this.chkCCLOSearch.Enabled = true;
        this.chkCCLOSearch.Checked = true;
        this.chkLEI.Checked = this.chkLEI.Enabled = true;
        this.stdButtonNewHMDAProfile.Enabled = false;
        this.changeFieldStatus(true, this.parentBox);
        this.changeFieldStatus(true, this.chkMERSMIN);
        this.changeFieldStatus(true, this.chkNMLS);
        this.changeFieldStatus(true, this.chkCCLOSearch);
        this.changeFieldStatus(true, this.chkLEI);
        this.licenseControl.SetUseParentInfo(true);
        this.refreshAddDBAButton();
      }
      else
      {
        OrgInfo organization = this.rOrg.GetOrganization(oid);
        this.parentIDforCCSite = organization.Parent;
        if (organization == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The organization does not exist.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.DialogResult = DialogResult.Cancel;
          this.Close();
        }
        else
        {
          this.ccSiteControl = new CCSiteControl(this.session, organization);
          this.ccSiteControl.Dock = DockStyle.Fill;
          this.panelCCSite.Controls.Add((Control) this.ccSiteControl);
          this.splitContainer1.Panel1Collapsed = !this.session.StartupInfo.EnableSSO;
          this.SSOControl = new AddEditOrgSSOControl(this.session, organization);
          if (!this.IsRestrictedAccessEnabled())
          {
            foreach (Control control in (ArrangedElementCollection) this.SSOControl.Controls)
              control.Enabled = false;
          }
          this.pnlSSO.Controls.Add((Control) this.SSOControl);
          this.orgNameTxt.Text = organization.OrgName;
          this.orgDescTxt.Text = organization.Description;
          this.orgCodeTxt.Text = organization.OrgCode;
          this.txtNMLS.Text = organization.NMLSCode;
          this.txtMERSMIN.Text = organization.MERSMINCode;
          this.txtDBAName1.Text = organization.DBAName1;
          this.txtDBAName2.Text = organization.DBAName2;
          this.txtDBAName3.Text = organization.DBAName3;
          this.txtDBAName4.Text = organization.DBAName4;
          this.ONRPBranchSettings = organization.ONRPRetailBranchSettings;
          this.ONRPBranchSettingsControl.RefreshData(this.ONRPRetailBranchSettings, organization.Parent);
          this.parentBox.CheckedChanged += new EventHandler(this.parentBox_CheckedChanged);
          this.chkNMLS.CheckedChanged += new EventHandler(this.chkNMLS_CheckedChanged);
          this.chkCCLOSearch.CheckedChanged += new EventHandler(this.chkCCLOSearch_CheckedChanged);
          this.chkMERSMIN.CheckedChanged += new EventHandler(this.chkMERSMIN_CheckedChanged);
          this.chkLEI.CheckedChanged += new EventHandler(this.chkLEI_CheckedChanged);
          this.getParentInfo(oid, this.parentBox);
          this.getParentInfo(oid, this.chkMERSMIN);
          this.getParentInfo(oid, this.chkNMLS);
          this.getParentInfo(oid, this.chkCCLOSearch);
          this.getParentInfo(oid, this.chkLEI);
          this.getParentStateLicensingInfo(oid);
          if (organization.ONRPRetailBranchSettings.UseParentInfo)
            this.getParentONRPInfo(organization.Parent);
          this.refreshAddDBAButton();
          if (this.loCompHistoryControl != null)
          {
            this.loanCompHistoryList = organization.LOCompHistoryList;
            LOCompCurrentControl compCurrentControl = this.loCompCurrentControl;
            LoanCompHistoryList loanCompHistoryList1 = this.loanCompHistoryList;
            int parent = organization.Parent;
            string parentID1 = parent.ToString();
            string currentID1 = this.oid.ToString();
            compCurrentControl.RefreshDate(loanCompHistoryList1, parentID1, currentID1);
            LOCompHistoryControl compHistoryControl = this.loCompHistoryControl;
            LoanCompHistoryList loanCompHistoryList2 = this.loanCompHistoryList;
            parent = organization.Parent;
            string parentID2 = parent.ToString();
            string currentID2 = this.oid.ToString();
            compHistoryControl.RefreshData(loanCompHistoryList2, parentID2, currentID2);
          }
          if (this.hasAddLEIPermission)
            this.stdButtonNewHMDAProfile.Enabled = true;
          else
            this.stdButtonNewHMDAProfile.Enabled = false;
          if (this.hasEditLEIPermission)
            this.stdButtonEditHMDAProfile.Enabled = true;
          else
            this.stdButtonEditHMDAProfile.Enabled = false;
          if (this.hasHMDAProfilePermission)
            this.stdButtonViewHMDAProfile.Enabled = true;
          else
            this.stdButtonViewHMDAProfile.Enabled = false;
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.orgNameTxt = new TextBox();
      this.orgDescTxt = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.panelLicense = new Panel();
      this.panelLOCompLatest = new Panel();
      this.panelLOCompHistory = new Panel();
      this.panelTop = new Panel();
      this.splitContainer1 = new SplitContainer();
      this.pnlSSO = new Panel();
      this.splitContainer2 = new SplitContainer();
      this.panelCCSite = new Panel();
      this.groupContainer6 = new GroupContainer();
      this.label20 = new Label();
      this.stdButtonViewHMDAProfile = new StandardIconButton();
      this.stdButtonNewHMDAProfile = new StandardIconButton();
      this.stdButtonEditHMDAProfile = new StandardIconButton();
      this.txtHMDAProfile = new TextBox();
      this.label19 = new Label();
      this.chkLEI = new CheckBox();
      this.txtLEI = new TextBox();
      this.groupContainer4 = new GroupContainer();
      this.chkShowOrgInLOSearch = new CheckBox();
      this.chkCCLOSearch = new CheckBox();
      this.txtLOSearchOrgName = new TextBox();
      this.label17 = new Label();
      this.groupContainer3 = new GroupContainer();
      this.chkMERSMIN = new CheckBox();
      this.txtMERSMIN = new TextBox();
      this.label12 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.parentBox = new CheckBox();
      this.panel2 = new Panel();
      this.unitTypeLabel = new Label();
      this.unitTypeCombobox = new ComboBoxEx();
      this.panelAddMore = new Panel();
      this.btnAddDBA = new Button();
      this.label4 = new Label();
      this.label9 = new Label();
      this.cityTxt = new TextBox();
      this.panelDBA4 = new Panel();
      this.label16 = new Label();
      this.txtDBAName4 = new TextBox();
      this.address2Txt = new TextBox();
      this.stateTxt = new TextBox();
      this.panelDBA3 = new Panel();
      this.label15 = new Label();
      this.txtDBAName3 = new TextBox();
      this.label5 = new Label();
      this.zipTxt = new TextBox();
      this.panelDBA2 = new Panel();
      this.label14 = new Label();
      this.txtDBAName2 = new TextBox();
      this.companyTxt = new TextBox();
      this.label3 = new Label();
      this.label8 = new Label();
      this.panelDBA1 = new Panel();
      this.label13 = new Label();
      this.txtDBAName1 = new TextBox();
      this.orgCodeTxt = new TextBox();
      this.label6 = new Label();
      this.phoneTxt = new TextBox();
      this.iconBtnSelect = new StandardIconButton();
      this.label10 = new Label();
      this.faxTxt = new TextBox();
      this.label7 = new Label();
      this.address1Txt = new TextBox();
      this.groupContainer2 = new GroupContainer();
      this.chkNMLS = new CheckBox();
      this.txtNMLS = new TextBox();
      this.label11 = new Label();
      this.label18 = new Label();
      this.panelMiddle = new Panel();
      this.panel1 = new Panel();
      this.panelONRPSettings = new Panel();
      this.panelBottom = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.panel3 = new Panel();
      this.panelTop.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.groupContainer6.SuspendLayout();
      ((ISupportInitialize) this.stdButtonViewHMDAProfile).BeginInit();
      ((ISupportInitialize) this.stdButtonNewHMDAProfile).BeginInit();
      ((ISupportInitialize) this.stdButtonEditHMDAProfile).BeginInit();
      this.groupContainer4.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panelAddMore.SuspendLayout();
      this.panelDBA4.SuspendLayout();
      this.panelDBA3.SuspendLayout();
      this.panelDBA2.SuspendLayout();
      this.panelDBA1.SuspendLayout();
      ((ISupportInitialize) this.iconBtnSelect).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.orgNameTxt.Location = new Point(100, 6);
      this.orgNameTxt.MaxLength = 32;
      this.orgNameTxt.Name = "orgNameTxt";
      this.orgNameTxt.Size = new Size(381, 20);
      this.orgNameTxt.TabIndex = 1;
      this.orgDescTxt.Location = new Point(100, 28);
      this.orgDescTxt.MaxLength = 256;
      this.orgDescTxt.Name = "orgDescTxt";
      this.orgDescTxt.Size = new Size(834, 20);
      this.orgDescTxt.TabIndex = 2;
      this.label1.Location = new Point(9, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(68, 23);
      this.label1.TabIndex = 0;
      this.label1.Text = "Organization";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Location = new Point(9, 26);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 23);
      this.label2.TabIndex = 0;
      this.label2.Text = "Description";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.okBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.okBtn.Location = new Point(788, 6);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(72, 24);
      this.okBtn.TabIndex = 23;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(866, 6);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(72, 24);
      this.cancelBtn.TabIndex = 24;
      this.cancelBtn.Text = "Cancel";
      this.panelLicense.Dock = DockStyle.Fill;
      this.panelLicense.Location = new Point(0, 0);
      this.panelLicense.Name = "panelLicense";
      this.panelLicense.Size = new Size(547, 488);
      this.panelLicense.TabIndex = 22;
      this.panelLOCompLatest.Location = new Point(10, 4);
      this.panelLOCompLatest.Name = "panelLOCompLatest";
      this.panelLOCompLatest.Size = new Size(374, 192);
      this.panelLOCompLatest.TabIndex = 67;
      this.panelLOCompHistory.Dock = DockStyle.Top;
      this.panelLOCompHistory.Location = new Point(0, 0);
      this.panelLOCompHistory.Name = "panelLOCompHistory";
      this.panelLOCompHistory.Size = new Size(543, 100);
      this.panelLOCompHistory.TabIndex = 66;
      this.panelTop.Controls.Add((Control) this.splitContainer1);
      this.panelTop.Controls.Add((Control) this.groupContainer6);
      this.panelTop.Controls.Add((Control) this.groupContainer4);
      this.panelTop.Controls.Add((Control) this.label1);
      this.panelTop.Controls.Add((Control) this.orgNameTxt);
      this.panelTop.Controls.Add((Control) this.orgDescTxt);
      this.panelTop.Controls.Add((Control) this.label2);
      this.panelTop.Controls.Add((Control) this.groupContainer3);
      this.panelTop.Controls.Add((Control) this.groupContainer1);
      this.panelTop.Controls.Add((Control) this.groupContainer2);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(1, 1);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(935, 734);
      this.panelTop.TabIndex = 68;
      this.splitContainer1.Location = new Point(386, 52);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.pnlSSO);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Size = new Size(547, 676);
      this.splitContainer1.SplitterDistance = 91;
      this.splitContainer1.SplitterWidth = 2;
      this.splitContainer1.TabIndex = 70;
      this.pnlSSO.Dock = DockStyle.Fill;
      this.pnlSSO.Location = new Point(0, 0);
      this.pnlSSO.Name = "pnlSSO";
      this.pnlSSO.Size = new Size(547, 91);
      this.pnlSSO.TabIndex = 69;
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.AutoScroll = true;
      this.splitContainer2.Panel1.Controls.Add((Control) this.panelLicense);
      this.splitContainer2.Panel2.AutoScroll = true;
      this.splitContainer2.Panel2.Controls.Add((Control) this.panelCCSite);
      this.splitContainer2.Size = new Size(547, 583);
      this.splitContainer2.SplitterDistance = 488;
      this.splitContainer2.SplitterWidth = 2;
      this.splitContainer2.TabIndex = 0;
      this.panelCCSite.Dock = DockStyle.Fill;
      this.panelCCSite.Location = new Point(0, 0);
      this.panelCCSite.Name = "panelCCSite";
      this.panelCCSite.Size = new Size(547, 93);
      this.panelCCSite.TabIndex = 68;
      this.groupContainer6.Controls.Add((Control) this.label20);
      this.groupContainer6.Controls.Add((Control) this.stdButtonViewHMDAProfile);
      this.groupContainer6.Controls.Add((Control) this.stdButtonNewHMDAProfile);
      this.groupContainer6.Controls.Add((Control) this.stdButtonEditHMDAProfile);
      this.groupContainer6.Controls.Add((Control) this.txtHMDAProfile);
      this.groupContainer6.Controls.Add((Control) this.label19);
      this.groupContainer6.Controls.Add((Control) this.chkLEI);
      this.groupContainer6.Controls.Add((Control) this.txtLEI);
      this.groupContainer6.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer6.Location = new Point(10, 528);
      this.groupContainer6.Name = "groupContainer6";
      this.groupContainer6.Size = new Size(374, 76);
      this.groupContainer6.TabIndex = 23;
      this.groupContainer6.Text = "Legal Entity Identifier (LEI)";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(10, 55);
      this.label20.Name = "label20";
      this.label20.Size = new Size(23, 13);
      this.label20.TabIndex = 22;
      this.label20.Text = "LEI";
      this.stdButtonViewHMDAProfile.BackColor = Color.Transparent;
      this.stdButtonViewHMDAProfile.Location = new Point(322, 5);
      this.stdButtonViewHMDAProfile.MouseDownImage = (Image) null;
      this.stdButtonViewHMDAProfile.Name = "stdButtonViewHMDAProfile";
      this.stdButtonViewHMDAProfile.Size = new Size(16, 16);
      this.stdButtonViewHMDAProfile.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.stdButtonViewHMDAProfile.TabIndex = 113;
      this.stdButtonViewHMDAProfile.TabStop = false;
      this.stdButtonViewHMDAProfile.Click += new EventHandler(this.stdButtonViewHMDAProfile_Click);
      this.stdButtonNewHMDAProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonNewHMDAProfile.BackColor = Color.Transparent;
      this.stdButtonNewHMDAProfile.Location = new Point(300, 5);
      this.stdButtonNewHMDAProfile.MouseDownImage = (Image) null;
      this.stdButtonNewHMDAProfile.Name = "stdButtonNewHMDAProfile";
      this.stdButtonNewHMDAProfile.Size = new Size(16, 16);
      this.stdButtonNewHMDAProfile.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdButtonNewHMDAProfile.TabIndex = 111;
      this.stdButtonNewHMDAProfile.TabStop = false;
      this.stdButtonNewHMDAProfile.Click += new EventHandler(this.stdButtonNewHMDAProfile_Click);
      this.stdButtonEditHMDAProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonEditHMDAProfile.BackColor = Color.Transparent;
      this.stdButtonEditHMDAProfile.Location = new Point(344, 5);
      this.stdButtonEditHMDAProfile.MouseDownImage = (Image) null;
      this.stdButtonEditHMDAProfile.Name = "stdButtonEditHMDAProfile";
      this.stdButtonEditHMDAProfile.Size = new Size(16, 16);
      this.stdButtonEditHMDAProfile.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdButtonEditHMDAProfile.TabIndex = 112;
      this.stdButtonEditHMDAProfile.TabStop = false;
      this.stdButtonEditHMDAProfile.Click += new EventHandler(this.stdButtonEditHMDAProfile_Click);
      this.txtHMDAProfile.Location = new Point(90, 30);
      this.txtHMDAProfile.MaxLength = 64;
      this.txtHMDAProfile.Name = "txtHMDAProfile";
      this.txtHMDAProfile.ReadOnly = true;
      this.txtHMDAProfile.Size = new Size(270, 20);
      this.txtHMDAProfile.TabIndex = 24;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(8, 33);
      this.label19.Name = "label19";
      this.label19.Size = new Size(71, 13);
      this.label19.TabIndex = 20;
      this.label19.Text = "HMDA Profile";
      this.chkLEI.AutoSize = true;
      this.chkLEI.BackColor = Color.Transparent;
      this.chkLEI.Location = new Point(194, 5);
      this.chkLEI.Name = "chkLEI";
      this.chkLEI.Size = new Size(100, 17);
      this.chkLEI.TabIndex = 23;
      this.chkLEI.Text = "Use Parent Info";
      this.chkLEI.UseVisualStyleBackColor = false;
      this.txtLEI.Location = new Point(90, 52);
      this.txtLEI.MaxLength = 64;
      this.txtLEI.Name = "txtLEI";
      this.txtLEI.ReadOnly = true;
      this.txtLEI.Size = new Size(270, 20);
      this.txtLEI.TabIndex = 25;
      this.groupContainer4.Controls.Add((Control) this.chkShowOrgInLOSearch);
      this.groupContainer4.Controls.Add((Control) this.chkCCLOSearch);
      this.groupContainer4.Controls.Add((Control) this.txtLOSearchOrgName);
      this.groupContainer4.Controls.Add((Control) this.label17);
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(10, 613);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(374, 71);
      this.groupContainer4.TabIndex = 23;
      this.groupContainer4.Text = "Consumer Connect LO Search";
      this.chkShowOrgInLOSearch.AutoSize = true;
      this.chkShowOrgInLOSearch.BackColor = Color.Transparent;
      this.chkShowOrgInLOSearch.Location = new Point(11, 29);
      this.chkShowOrgInLOSearch.Name = "chkShowOrgInLOSearch";
      this.chkShowOrgInLOSearch.Size = new Size(178, 17);
      this.chkShowOrgInLOSearch.TabIndex = 27;
      this.chkShowOrgInLOSearch.Text = "Show organization in LO Search";
      this.chkShowOrgInLOSearch.UseVisualStyleBackColor = false;
      this.chkShowOrgInLOSearch.CheckedChanged += new EventHandler(this.chkShowOrgInLOSearch_CheckedChanged);
      this.chkCCLOSearch.AutoSize = true;
      this.chkCCLOSearch.BackColor = Color.Transparent;
      this.chkCCLOSearch.Location = new Point(267, 6);
      this.chkCCLOSearch.Name = "chkCCLOSearch";
      this.chkCCLOSearch.Size = new Size(100, 17);
      this.chkCCLOSearch.TabIndex = 26;
      this.chkCCLOSearch.Text = "Use Parent Info";
      this.chkCCLOSearch.UseVisualStyleBackColor = false;
      this.txtLOSearchOrgName.Location = new Point(90, 48);
      this.txtLOSearchOrgName.MaxLength = 64;
      this.txtLOSearchOrgName.Name = "txtLOSearchOrgName";
      this.txtLOSearchOrgName.Size = new Size(270, 20);
      this.txtLOSearchOrgName.TabIndex = 28;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(8, 51);
      this.label17.Name = "label17";
      this.label17.Size = new Size(35, 13);
      this.label17.TabIndex = 0;
      this.label17.Text = "Name";
      this.groupContainer3.Controls.Add((Control) this.chkMERSMIN);
      this.groupContainer3.Controls.Add((Control) this.txtMERSMIN);
      this.groupContainer3.Controls.Add((Control) this.label12);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(10, 464);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(374, 55);
      this.groupContainer3.TabIndex = 21;
      this.groupContainer3.Text = "MERS MIN";
      this.chkMERSMIN.AutoSize = true;
      this.chkMERSMIN.BackColor = Color.Transparent;
      this.chkMERSMIN.Location = new Point(267, 6);
      this.chkMERSMIN.Name = "chkMERSMIN";
      this.chkMERSMIN.Size = new Size(100, 17);
      this.chkMERSMIN.TabIndex = 21;
      this.chkMERSMIN.Text = "Use Parent Info";
      this.chkMERSMIN.UseVisualStyleBackColor = false;
      this.txtMERSMIN.Location = new Point(90, 32);
      this.txtMERSMIN.MaxLength = 7;
      this.txtMERSMIN.Name = "txtMERSMIN";
      this.txtMERSMIN.Size = new Size(270, 20);
      this.txtMERSMIN.TabIndex = 22;
      this.txtMERSMIN.KeyPress += new KeyPressEventHandler(this.txtMERSMIN_KeyPress);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(9, 35);
      this.label12.Name = "label12";
      this.label12.Size = new Size(75, 13);
      this.label12.TabIndex = 1;
      this.label12.Text = "MERS MIN ID";
      this.groupContainer1.Controls.Add((Control) this.parentBox);
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 52);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(373, 339);
      this.groupContainer1.TabIndex = 19;
      this.groupContainer1.Text = "Organization Information";
      this.parentBox.AutoSize = true;
      this.parentBox.BackColor = Color.Transparent;
      this.parentBox.Location = new Point(267, 5);
      this.parentBox.Name = "parentBox";
      this.parentBox.Size = new Size(100, 17);
      this.parentBox.TabIndex = 3;
      this.parentBox.Text = "Use Parent Info";
      this.parentBox.UseVisualStyleBackColor = false;
      this.panel2.AutoScroll = true;
      this.panel2.Controls.Add((Control) this.unitTypeLabel);
      this.panel2.Controls.Add((Control) this.unitTypeCombobox);
      this.panel2.Controls.Add((Control) this.panelAddMore);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.label9);
      this.panel2.Controls.Add((Control) this.cityTxt);
      this.panel2.Controls.Add((Control) this.panelDBA4);
      this.panel2.Controls.Add((Control) this.address2Txt);
      this.panel2.Controls.Add((Control) this.stateTxt);
      this.panel2.Controls.Add((Control) this.panelDBA3);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.zipTxt);
      this.panel2.Controls.Add((Control) this.panelDBA2);
      this.panel2.Controls.Add((Control) this.companyTxt);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label8);
      this.panel2.Controls.Add((Control) this.panelDBA1);
      this.panel2.Controls.Add((Control) this.orgCodeTxt);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.phoneTxt);
      this.panel2.Controls.Add((Control) this.iconBtnSelect);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.faxTxt);
      this.panel2.Controls.Add((Control) this.label7);
      this.panel2.Controls.Add((Control) this.address1Txt);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(371, 312);
      this.panel2.TabIndex = 0;
      this.unitTypeLabel.Location = new Point(7, 70);
      this.unitTypeLabel.Name = "unitTypeLabel";
      this.unitTypeLabel.Size = new Size(55, 20);
      this.unitTypeLabel.TabIndex = 30;
      this.unitTypeLabel.Text = "Unit Type";
      this.unitTypeLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.unitTypeCombobox.FormattingEnabled = true;
      this.unitTypeCombobox.Items.AddRange(new object[24]
      {
        (object) "Apartment",
        (object) "Basement",
        (object) "Building",
        (object) "Condo ",
        (object) "Department",
        (object) "Floor",
        (object) "Front",
        (object) "Hangar ",
        (object) "Key ",
        (object) "Lobby",
        (object) "Lot ",
        (object) "Lower ",
        (object) "Office",
        (object) "Penthouse ",
        (object) "Pier",
        (object) "Rear ",
        (object) "Room ",
        (object) "Side",
        (object) "Space ",
        (object) "Stop ",
        (object) "Suite",
        (object) "Trailer",
        (object) "Unit",
        (object) "Upper"
      });
      this.unitTypeCombobox.Location = new Point(89, 70);
      this.unitTypeCombobox.Name = "unitTypeCombobox";
      this.unitTypeCombobox.SelectedBGColor = SystemColors.Highlight;
      this.unitTypeCombobox.Size = new Size(81, 21);
      this.unitTypeCombobox.TabIndex = 7;
      this.panelAddMore.Controls.Add((Control) this.btnAddDBA);
      this.panelAddMore.Location = new Point(3, 268);
      this.panelAddMore.Name = "panelAddMore";
      this.panelAddMore.Size = new Size(345, 22);
      this.panelAddMore.TabIndex = 28;
      this.btnAddDBA.Location = new Point(256, 0);
      this.btnAddDBA.Name = "btnAddDBA";
      this.btnAddDBA.Size = new Size(75, 23);
      this.btnAddDBA.TabIndex = 18;
      this.btnAddDBA.Text = "Add More";
      this.btnAddDBA.UseVisualStyleBackColor = true;
      this.btnAddDBA.Click += new EventHandler(this.btnAddMore_Click);
      this.label4.Location = new Point(7, 48);
      this.label4.Name = "label4";
      this.label4.Size = new Size(55, 20);
      this.label4.TabIndex = 11;
      this.label4.Text = "Address";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label9.Location = new Point(7, 159);
      this.label9.Name = "label9";
      this.label9.Size = new Size(41, 20);
      this.label9.TabIndex = 16;
      this.label9.Text = "Fax";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.cityTxt.Location = new Point(89, 92);
      this.cityTxt.MaxLength = 50;
      this.cityTxt.Name = "cityTxt";
      this.cityTxt.Size = new Size(248, 20);
      this.cityTxt.TabIndex = 9;
      this.panelDBA4.Controls.Add((Control) this.label16);
      this.panelDBA4.Controls.Add((Control) this.txtDBAName4);
      this.panelDBA4.Location = new Point(3, 246);
      this.panelDBA4.Name = "panelDBA4";
      this.panelDBA4.Size = new Size(345, 22);
      this.panelDBA4.TabIndex = 27;
      this.panelDBA4.Visible = false;
      this.label16.AutoSize = true;
      this.label16.Location = new Point(4, 4);
      this.label16.Name = "label16";
      this.label16.Size = new Size(47, 13);
      this.label16.TabIndex = 18;
      this.label16.Text = "D.B.A. 4";
      this.label16.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDBAName4.Location = new Point(86, 0);
      this.txtDBAName4.MaxLength = 30;
      this.txtDBAName4.Name = "txtDBAName4";
      this.txtDBAName4.Size = new Size(248, 20);
      this.txtDBAName4.TabIndex = 17;
      this.address2Txt.Location = new Point(176, 70);
      this.address2Txt.MaxLength = 11;
      this.address2Txt.Name = "address2Txt";
      this.address2Txt.Size = new Size(161, 20);
      this.address2Txt.TabIndex = 8;
      this.stateTxt.Location = new Point(89, 114);
      this.stateTxt.MaxLength = 20;
      this.stateTxt.Name = "stateTxt";
      this.stateTxt.Size = new Size(56, 20);
      this.stateTxt.TabIndex = 10;
      this.panelDBA3.Controls.Add((Control) this.label15);
      this.panelDBA3.Controls.Add((Control) this.txtDBAName3);
      this.panelDBA3.Location = new Point(3, 224);
      this.panelDBA3.Name = "panelDBA3";
      this.panelDBA3.Size = new Size(345, 22);
      this.panelDBA3.TabIndex = 26;
      this.panelDBA3.Visible = false;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(4, 4);
      this.label15.Name = "label15";
      this.label15.Size = new Size(47, 13);
      this.label15.TabIndex = 18;
      this.label15.Text = "D.B.A. 3";
      this.label15.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDBAName3.Location = new Point(86, 0);
      this.txtDBAName3.MaxLength = 30;
      this.txtDBAName3.Name = "txtDBAName3";
      this.txtDBAName3.Size = new Size(248, 20);
      this.txtDBAName3.TabIndex = 16;
      this.label5.Location = new Point(7, 93);
      this.label5.Name = "label5";
      this.label5.Size = new Size(41, 20);
      this.label5.TabIndex = 12;
      this.label5.Text = "City";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.zipTxt.Location = new Point(201, 114);
      this.zipTxt.MaxLength = 10;
      this.zipTxt.Name = "zipTxt";
      this.zipTxt.Size = new Size(82, 20);
      this.zipTxt.TabIndex = 11;
      this.zipTxt.KeyUp += new KeyEventHandler(this.keyupZip);
      this.zipTxt.Leave += new EventHandler(this.zipTxt_Leave);
      this.panelDBA2.Controls.Add((Control) this.label14);
      this.panelDBA2.Controls.Add((Control) this.txtDBAName2);
      this.panelDBA2.Location = new Point(3, 202);
      this.panelDBA2.Name = "panelDBA2";
      this.panelDBA2.Size = new Size(345, 22);
      this.panelDBA2.TabIndex = 25;
      this.panelDBA2.Visible = false;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(4, 4);
      this.label14.Name = "label14";
      this.label14.Size = new Size(47, 13);
      this.label14.TabIndex = 18;
      this.label14.Text = "D.B.A. 2";
      this.label14.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDBAName2.Location = new Point(86, 0);
      this.txtDBAName2.MaxLength = 30;
      this.txtDBAName2.Name = "txtDBAName2";
      this.txtDBAName2.Size = new Size(248, 20);
      this.txtDBAName2.TabIndex = 15;
      this.companyTxt.Location = new Point(89, 26);
      this.companyTxt.MaxLength = 64;
      this.companyTxt.Name = "companyTxt";
      this.companyTxt.Size = new Size(248, 20);
      this.companyTxt.TabIndex = 5;
      this.label3.Location = new Point(7, 27);
      this.label3.Name = "label3";
      this.label3.Size = new Size(47, 20);
      this.label3.TabIndex = 10;
      this.label3.Text = "Name";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.label8.Location = new Point(7, 140);
      this.label8.Name = "label8";
      this.label8.Size = new Size(41, 16);
      this.label8.TabIndex = 15;
      this.label8.Text = "Phone";
      this.label8.TextAlign = ContentAlignment.MiddleLeft;
      this.panelDBA1.Controls.Add((Control) this.label13);
      this.panelDBA1.Controls.Add((Control) this.txtDBAName1);
      this.panelDBA1.Location = new Point(3, 180);
      this.panelDBA1.Name = "panelDBA1";
      this.panelDBA1.Size = new Size(345, 22);
      this.panelDBA1.TabIndex = 24;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(4, 4);
      this.label13.Name = "label13";
      this.label13.Size = new Size(38, 13);
      this.label13.TabIndex = 18;
      this.label13.Text = "D.B.A.";
      this.label13.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDBAName1.Location = new Point(86, 0);
      this.txtDBAName1.MaxLength = 30;
      this.txtDBAName1.Name = "txtDBAName1";
      this.txtDBAName1.Size = new Size(248, 20);
      this.txtDBAName1.TabIndex = 14;
      this.orgCodeTxt.Location = new Point(89, 4);
      this.orgCodeTxt.MaxLength = 6;
      this.orgCodeTxt.Name = "orgCodeTxt";
      this.orgCodeTxt.Size = new Size(72, 20);
      this.orgCodeTxt.TabIndex = 4;
      this.label6.Location = new Point(7, 118);
      this.label6.Name = "label6";
      this.label6.Size = new Size(33, 16);
      this.label6.TabIndex = 13;
      this.label6.Text = "State";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.phoneTxt.Location = new Point(89, 136);
      this.phoneTxt.MaxLength = 30;
      this.phoneTxt.Name = "phoneTxt";
      this.phoneTxt.Size = new Size(248, 20);
      this.phoneTxt.TabIndex = 12;
      this.phoneTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.iconBtnSelect.BackColor = Color.Transparent;
      this.iconBtnSelect.Location = new Point(337, 50);
      this.iconBtnSelect.MouseDownImage = (Image) null;
      this.iconBtnSelect.Name = "iconBtnSelect";
      this.iconBtnSelect.Size = new Size(16, 16);
      this.iconBtnSelect.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.iconBtnSelect.TabIndex = 23;
      this.iconBtnSelect.TabStop = false;
      this.iconBtnSelect.Click += new EventHandler(this.selectBtn_Click);
      this.label10.Location = new Point(7, 1);
      this.label10.Name = "label10";
      this.label10.Size = new Size(33, 23);
      this.label10.TabIndex = 19;
      this.label10.Text = "Code";
      this.label10.TextAlign = ContentAlignment.MiddleLeft;
      this.faxTxt.Location = new Point(89, 158);
      this.faxTxt.MaxLength = 30;
      this.faxTxt.Name = "faxTxt";
      this.faxTxt.Size = new Size(248, 20);
      this.faxTxt.TabIndex = 13;
      this.faxTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.label7.Location = new Point(163, 115);
      this.label7.Name = "label7";
      this.label7.Size = new Size(32, 16);
      this.label7.TabIndex = 14;
      this.label7.Text = "Zip";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.address1Txt.Location = new Point(89, 48);
      this.address1Txt.MaxLength = (int) byte.MaxValue;
      this.address1Txt.Name = "address1Txt";
      this.address1Txt.Size = new Size(248, 20);
      this.address1Txt.TabIndex = 6;
      this.groupContainer2.Controls.Add((Control) this.chkNMLS);
      this.groupContainer2.Controls.Add((Control) this.txtNMLS);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(9, 400);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(374, 55);
      this.groupContainer2.TabIndex = 20;
      this.groupContainer2.Text = "NMLS";
      this.chkNMLS.AutoSize = true;
      this.chkNMLS.BackColor = Color.Transparent;
      this.chkNMLS.Location = new Point(267, 6);
      this.chkNMLS.Name = "chkNMLS";
      this.chkNMLS.Size = new Size(100, 17);
      this.chkNMLS.TabIndex = 19;
      this.chkNMLS.Text = "Use Parent Info";
      this.chkNMLS.UseVisualStyleBackColor = false;
      this.txtNMLS.Location = new Point(90, 32);
      this.txtNMLS.MaxLength = 12;
      this.txtNMLS.Name = "txtNMLS";
      this.txtNMLS.Size = new Size(270, 20);
      this.txtNMLS.TabIndex = 20;
      this.txtNMLS.TextChanged += new EventHandler(this.txtNMLS_TextChanged);
      this.txtNMLS.KeyDown += new KeyEventHandler(this.txtNMLS_KeyDown);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(8, 35);
      this.label11.Name = "label11";
      this.label11.Size = new Size(37, 13);
      this.label11.TabIndex = 0;
      this.label11.Text = "NMLS";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(8, 55);
      this.label18.Name = "label18";
      this.label18.Size = new Size(23, 13);
      this.label18.TabIndex = 0;
      this.label18.Text = "LEI";
      this.panelMiddle.Controls.Add((Control) this.panel1);
      this.panelMiddle.Controls.Add((Control) this.panelLOCompLatest);
      this.panelMiddle.Dock = DockStyle.Top;
      this.panelMiddle.Location = new Point(1, 735);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Padding = new Padding(3);
      this.panelMiddle.Size = new Size(935, 214);
      this.panelMiddle.TabIndex = 69;
      this.panel1.Controls.Add((Control) this.panelLOCompHistory);
      this.panel1.Controls.Add((Control) this.panelONRPSettings);
      this.panel1.Location = new Point(390, 0);
      this.panel1.Margin = new Padding(0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(543, 187);
      this.panel1.TabIndex = 68;
      this.panelONRPSettings.Dock = DockStyle.Bottom;
      this.panelONRPSettings.Location = new Point(0, 103);
      this.panelONRPSettings.Margin = new Padding(0);
      this.panelONRPSettings.Name = "panelONRPSettings";
      this.panelONRPSettings.Size = new Size(543, 84);
      this.panelONRPSettings.TabIndex = 67;
      this.panelBottom.Controls.Add((Control) this.emHelpLink1);
      this.panelBottom.Controls.Add((Control) this.okBtn);
      this.panelBottom.Controls.Add((Control) this.cancelBtn);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 852);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(954, 35);
      this.panelBottom.TabIndex = 70;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Organization Details";
      this.emHelpLink1.Location = new Point(10, 12);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 18;
      this.panel3.AutoScroll = true;
      this.panel3.Controls.Add((Control) this.panelMiddle);
      this.panel3.Controls.Add((Control) this.panelTop);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Padding = new Padding(1);
      this.panel3.Size = new Size(954, 834);
      this.panel3.TabIndex = 71;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(954, 887);
      this.Controls.Add((Control) this.panelBottom);
      this.Controls.Add((Control) this.panel3);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditOrgDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Organization Details";
      this.Shown += new EventHandler(this.AddEditDialog_Warning);
      this.ResizeEnd += new EventHandler(this.AddEditOrgDialog_ResizeEnd);
      this.SizeChanged += new EventHandler(this.AddEditOrgDialog_SizeChanged);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.Resize += new EventHandler(this.AddEditOrgDialog_Resize);
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.groupContainer6.ResumeLayout(false);
      this.groupContainer6.PerformLayout();
      ((ISupportInitialize) this.stdButtonViewHMDAProfile).EndInit();
      ((ISupportInitialize) this.stdButtonNewHMDAProfile).EndInit();
      ((ISupportInitialize) this.stdButtonEditHMDAProfile).EndInit();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panelAddMore.ResumeLayout(false);
      this.panelDBA4.ResumeLayout(false);
      this.panelDBA4.PerformLayout();
      this.panelDBA3.ResumeLayout(false);
      this.panelDBA3.PerformLayout();
      this.panelDBA2.ResumeLayout(false);
      this.panelDBA2.PerformLayout();
      this.panelDBA1.ResumeLayout(false);
      this.panelDBA1.PerformLayout();
      ((ISupportInitialize) this.iconBtnSelect).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.panelMiddle.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panelBottom.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.orgNameTxt.Text = this.orgNameTxt.Text.Trim();
      if (this.orgNameTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter an organization name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.orgNameTxt.Focus();
      }
      else if (this.companyTxt.Text.Trim() == string.Empty && !this.parentBox.Checked && this.oid != 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a company name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.companyTxt.Focus();
      }
      else
      {
        OrgInfo[] organizationsByName = this.session.OrganizationManager.GetOrganizationsByName(this.orgNameTxt.Text);
        if (this.parentID == -1)
          this.parentID = this.session.OrganizationManager.GetOrganization(this.oid).Parent;
        if (organizationsByName != null && organizationsByName.Length != 0)
        {
          foreach (OrgInfo orgInfo in organizationsByName)
          {
            if (orgInfo.Parent == this.parentID && this.oid != orgInfo.Oid)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Organization name '" + this.orgNameTxt.Text + "' already exists at this level. Please enter another name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.orgNameTxt.Focus();
              return;
            }
          }
        }
        if (this.txtMERSMIN.Text.Trim() != string.Empty && Utils.IsInt((object) this.txtMERSMIN.Text.Trim()) && this.txtMERSMIN.Text.Trim().Length != 7)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "MERS MIN ID must be seven (7) digits.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtMERSMIN.Focus();
        }
        else
        {
          if (this.loCompHistoryControl != null && !this.loCompHistoryControl.DataValidation(true) || this.licenseControl != null && !this.licenseControl.DataValidated() || this.ccSiteControl.userChangedURLText && !this.ccSiteControl.doSearch())
            return;
          this.orgBranchLicensing = this.licenseControl.CurrentBranchLicensing;
          if (!this.orgBranchLicensing.UseParentInfo && this.orgBranchLicensing.StateLicenseExtTypes.Count == 0 && this.orgBranchLicensing.LenderType == "" && Utils.Dialog((IWin32Window) this, "There is no license selected in any state. This might cause bad or incorrect review results. Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            return;
          if (this.loanCompHistoryList != null)
          {
            this.loanCompHistoryList.UseParentInfo = this.loanCompHistoryList.UseParentInfo;
            this.loanCompHistoryList.UncheckParentInfo = this.loCompHistoryControl.UncheckParentInfo;
          }
          this.orgName = this.orgNameTxt.Text;
          this.orgDescription = this.orgDescTxt.Text;
          if (this.parentBox.Checked)
          {
            this.orgCode = "";
            this.orgCoName = "";
            this.orgAddress1 = "";
            this.orgAddress2 = "";
            this.orgCity = "";
            this.orgState = "";
            this.orgZip = "";
            this.orgPhone = "";
            this.orgFax = "";
            this.orgUnitType = "";
          }
          else
          {
            this.orgCode = this.orgCodeTxt.Text.Trim();
            this.orgCoName = this.companyTxt.Text.Trim();
            this.orgAddress1 = this.address1Txt.Text.Trim();
            this.orgAddress2 = this.address2Txt.Text.Trim();
            this.orgCity = this.cityTxt.Text.Trim();
            this.orgState = this.stateTxt.Text.Trim();
            this.orgZip = this.zipTxt.Text.Trim();
            this.orgPhone = this.phoneTxt.Text.Trim();
            this.orgFax = this.faxTxt.Text.Trim();
            this.orgUnitType = this.unitTypeCombobox.SelectedItem != null ? this.unitTypeCombobox.SelectedItem.ToString() : "";
          }
          this.nmlsCode = !this.chkNMLS.Checked ? this.txtNMLS.Text.Trim() : "";
          this.mersminCode = !this.chkMERSMIN.Checked ? this.txtMERSMIN.Text.Trim() : "";
          if (this.chkCCLOSearch.Checked)
          {
            this.showOrgInLOSearch = false;
            this.loSearch_OrgName = "";
          }
          else
          {
            this.showOrgInLOSearch = this.chkShowOrgInLOSearch.Checked;
            this.loSearch_OrgName = string.IsNullOrEmpty(this.txtLOSearchOrgName.Text.Trim()) ? this.companyTxt.Text.Trim() : this.txtLOSearchOrgName.Text.Trim();
          }
          this.hmdaProfileId = !this.chkLEI.Checked ? (this.profile != null ? this.profile.HMDAProfileID : 0) : 0;
          this.dbNames = new string[4]
          {
            this.txtDBAName1.Text.Trim(),
            this.txtDBAName2.Text.Trim(),
            this.txtDBAName3.Text.Trim(),
            this.txtDBAName4.Text.Trim()
          };
          this.ONRPBranchSettings = this.oid != 0 ? this.ONRPBranchSettingsControl.settings : (ONRPEntitySettings) null;
          this.ccSiteSettings = this.ccSiteControl.ccSiteSettings;
          if (this.SSOControl != null)
            this.ssoSettings = this.SSOControl.ssoSettings;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void selectBtn_Click(object sender, EventArgs e)
    {
      RxBusinessContact rxBusinessContact = new RxBusinessContact("Organization", this.companyTxt.Text, "", (RxContactInfo) null, CRMRoleType.NotFound, true, "");
      if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      if (rxBusinessContact.GoToContact)
      {
        Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
      }
      else
      {
        RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
        this.companyTxt.Text = rxContactRecord.CompanyName;
        this.address1Txt.Text = rxContactRecord.BizAddress1;
        this.address2Txt.Text = rxContactRecord.BizAddress2;
        this.cityTxt.Text = rxContactRecord.BizCity;
        this.stateTxt.Text = rxContactRecord.BizState;
        this.zipTxt.Text = rxContactRecord.BizZip;
        this.phoneTxt.Text = rxContactRecord.WorkPhone;
        this.faxTxt.Text = rxContactRecord.FaxNumber;
      }
    }

    private void parentBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.parentBox.Checked)
      {
        this.getParentInfo((this.oid != -1 || this.parentID == -1 ? this.rOrg.GetOrganization(this.oid) : this.rOrg.GetOrganization(this.parentID)).Parent, this.parentBox);
        this.panelDBA2.Visible = this.panelDBA3.Visible = this.panelDBA4.Visible = false;
        this.refreshAddDBAButton();
      }
      else
        this.changeFieldStatus(false, this.parentBox);
    }

    private void getParentStateLicensingInfo(int uID)
    {
      this.licenseControl.ReloadParentStateLicensingInfo(uID);
    }

    private void getParentInfo(int uID, CheckBox control)
    {
      OrgInfo orgInfo = (OrgInfo) null;
      if (control == this.parentBox)
      {
        orgInfo = this.rOrg.GetFirstAvaliableOrganization(uID);
        this.parentBox.CheckedChanged -= new EventHandler(this.parentBox_CheckedChanged);
      }
      else if (control == this.chkMERSMIN)
      {
        orgInfo = this.rOrg.GetFirstOrganizationWithMERSMIN(uID);
        this.chkMERSMIN.CheckedChanged -= new EventHandler(this.chkMERSMIN_CheckedChanged);
      }
      else if (control == this.chkNMLS)
      {
        orgInfo = this.rOrg.GetFirstOrganizationWithNMLS(uID);
        this.chkNMLS.CheckedChanged -= new EventHandler(this.chkNMLS_CheckedChanged);
      }
      else if (control == this.chkCCLOSearch)
      {
        orgInfo = this.rOrg.GetFirstOrganizationWithLOSearch(uID);
        this.chkCCLOSearch.CheckedChanged -= new EventHandler(this.chkCCLOSearch_CheckedChanged);
      }
      else if (control == this.chkLEI)
      {
        orgInfo = this.rOrg.GetFirstOrganizationWithLEI(uID);
        this.chkLEI.CheckedChanged -= new EventHandler(this.chkLEI_CheckedChanged);
      }
      if (orgInfo != null)
        control.Checked = orgInfo.Oid != this.oid;
      else if (uID != 0)
        control.Checked = true;
      if (this.oid == 0)
        control.Enabled = false;
      else
        control.Enabled = true;
      this.changeFieldStatus(control.Checked, control);
      if (control == this.parentBox)
        this.parentBox.CheckedChanged += new EventHandler(this.parentBox_CheckedChanged);
      else if (control == this.chkNMLS)
        this.chkNMLS.CheckedChanged += new EventHandler(this.chkNMLS_CheckedChanged);
      else if (control == this.chkMERSMIN)
        this.chkMERSMIN.CheckedChanged += new EventHandler(this.chkMERSMIN_CheckedChanged);
      else if (control == this.chkCCLOSearch)
        this.chkCCLOSearch.CheckedChanged += new EventHandler(this.chkCCLOSearch_CheckedChanged);
      else if (control == this.chkLEI)
        this.chkLEI.CheckedChanged += new EventHandler(this.chkLEI_CheckedChanged);
      if (control == this.parentBox)
      {
        if (orgInfo != null)
          this.orgCodeTxt.Text = orgInfo.OrgCode;
        else
          this.orgCodeTxt.Text = string.Empty;
      }
      if (orgInfo != null)
      {
        if (control == this.parentBox)
        {
          this.companyTxt.Text = orgInfo.CompanyName;
          this.address1Txt.Text = orgInfo.CompanyAddress.Street1;
          this.address2Txt.Text = orgInfo.CompanyAddress.Street2;
          this.unitTypeCombobox.SelectedItem = (object) orgInfo.CompanyAddress.UnitType;
          this.cityTxt.Text = orgInfo.CompanyAddress.City;
          this.stateTxt.Text = orgInfo.CompanyAddress.State;
          this.zipTxt.Text = orgInfo.CompanyAddress.Zip;
          this.phoneTxt.Text = orgInfo.CompanyPhone;
          this.faxTxt.Text = orgInfo.CompanyFax;
          this.txtDBAName1.Text = orgInfo.DBAName1;
          this.txtDBAName2.Text = orgInfo.DBAName2;
          this.txtDBAName3.Text = orgInfo.DBAName3;
          this.txtDBAName4.Text = orgInfo.DBAName4;
          this.refreshAddDBAButton();
        }
        else if (control == this.chkNMLS)
          this.txtNMLS.Text = orgInfo.NMLSCode;
        else if (control == this.chkMERSMIN)
          this.txtMERSMIN.Text = orgInfo.MERSMINCode;
        else if (control == this.chkCCLOSearch)
        {
          if (this.oid == -1)
          {
            this.chkShowOrgInLOSearch.Checked = false;
            this.txtLOSearchOrgName.Enabled = false;
            this.txtLOSearchOrgName.Text = "";
          }
          else
          {
            this.chkShowOrgInLOSearch.Checked = !string.IsNullOrEmpty(orgInfo.LOSearchOrgName.Trim()) && orgInfo.ShowOrgInLOSearch;
            this.txtLOSearchOrgName.Enabled = this.chkShowOrgInLOSearch.Checked;
            this.txtLOSearchOrgName.Text = orgInfo.LOSearchOrgName;
          }
        }
        else
        {
          if (control != this.chkLEI)
            return;
          if (orgInfo.HMDAProfileId > 0)
          {
            this.profile = this.configMgr.GetHMDAProfileById(orgInfo.HMDAProfileId);
            if (this.profile != null)
            {
              this.txtHMDAProfile.Text = this.profile.HMDAProfileName;
              this.txtLEI.Text = this.profile.HMDAProfileLEI;
            }
            if (control.Checked)
            {
              this.stdButtonNewHMDAProfile.Enabled = this.stdButtonEditHMDAProfile.Enabled = false;
            }
            else
            {
              this.stdButtonEditHMDAProfile.Enabled = this.hasEditLEIPermission;
              this.stdButtonNewHMDAProfile.Enabled = this.hasAddLEIPermission;
            }
          }
          else
          {
            this.txtHMDAProfile.Text = this.txtLEI.Text = string.Empty;
            this.stdButtonNewHMDAProfile.Enabled = this.hasAddLEIPermission;
            this.stdButtonEditHMDAProfile.Enabled = this.hasEditLEIPermission;
          }
        }
      }
      else
      {
        if (orgInfo != null)
          return;
        if (control == this.parentBox)
        {
          CompanyInfo companyInfo = this.session.RecacheCompanyInfo();
          this.companyTxt.Text = companyInfo.Name;
          this.address1Txt.Text = companyInfo.Address;
          this.address2Txt.Text = "";
          this.cityTxt.Text = companyInfo.City;
          this.stateTxt.Text = companyInfo.State;
          this.zipTxt.Text = companyInfo.Zip;
          this.phoneTxt.Text = companyInfo.Phone;
          this.faxTxt.Text = companyInfo.Fax;
          this.txtDBAName1.Text = companyInfo.DBAName1;
          this.txtDBAName2.Text = companyInfo.DBAName2;
          this.txtDBAName3.Text = companyInfo.DBAName3;
          this.txtDBAName4.Text = companyInfo.DBAName4;
          this.refreshAddDBAButton();
        }
        else if (control == this.chkNMLS)
          this.txtNMLS.Text = "";
        else if (control == this.chkMERSMIN)
          this.txtMERSMIN.Text = "";
        else if (control == this.chkCCLOSearch)
        {
          this.chkShowOrgInLOSearch.Checked = false;
          this.txtLOSearchOrgName.Text = "";
        }
        else
        {
          if (control != this.chkLEI)
            return;
          this.txtHMDAProfile.Text = this.txtLEI.Text = string.Empty;
          if (uID == 0)
            this.stdButtonViewHMDAProfile.Enabled = false;
          else
            this.stdButtonViewHMDAProfile.Enabled = true;
        }
      }
    }

    private void changeFieldStatus(bool state, CheckBox control)
    {
      if (control == this.parentBox)
      {
        if (this.oid != 0)
          this.orgCodeTxt.ReadOnly = state;
        this.companyTxt.ReadOnly = state;
        this.address1Txt.ReadOnly = state;
        this.address2Txt.ReadOnly = state;
        this.unitTypeCombobox.Enabled = !state;
        this.cityTxt.ReadOnly = state;
        this.stateTxt.ReadOnly = state;
        this.zipTxt.ReadOnly = state;
        this.phoneTxt.ReadOnly = state;
        this.faxTxt.ReadOnly = state;
        this.btnAddDBA.Enabled = !state;
        this.iconBtnSelect.Enabled = !state;
        this.txtDBAName1.ReadOnly = this.txtDBAName2.ReadOnly = this.txtDBAName3.ReadOnly = this.txtDBAName4.ReadOnly = state;
      }
      else if (control == this.chkMERSMIN)
        this.txtMERSMIN.ReadOnly = state;
      else if (control == this.chkNMLS)
        this.txtNMLS.ReadOnly = state;
      else if (control == this.chkCCLOSearch)
      {
        this.chkShowOrgInLOSearch.Enabled = !state;
        this.txtLOSearchOrgName.ReadOnly = state;
      }
      else
      {
        if (control != this.chkLEI)
          return;
        if (!state)
        {
          this.stdButtonNewHMDAProfile.Enabled = this.hasAddLEIPermission;
          this.stdButtonEditHMDAProfile.Enabled = this.hasEditLEIPermission;
        }
        else
          this.stdButtonNewHMDAProfile.Enabled = this.stdButtonEditHMDAProfile.Enabled = !state;
      }
    }

    private void zipTxt_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox == null)
        return;
      string str = textBox.Text.Trim();
      if (str.Length < 5)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(str.Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(str.Substring(0, 5)));
      if (zipCodeInfo == null)
        return;
      this.cityTxt.Text = zipCodeInfo.City;
      this.stateTxt.Text = zipCodeInfo.State.ToUpper();
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        return;
      FieldFormat dataFormat = FieldFormat.PHONE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void keyupZip(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.ZIPCODE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (AddEditOrgDialog));
    }

    private void txtMERSMIN_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void txtNMLS_TextChanged(object sender, EventArgs e)
    {
      if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        Regex regex = new Regex("^\\d+$");
        if (this.txtNMLS.Text.Length <= 0 || regex.IsMatch(this.txtNMLS.Text))
          return;
        this.txtNMLS.Text = this.txtNMLS.Text.Substring(0, this.txtNMLS.Text.Length - 1);
        this.txtNMLS.SelectionStart = this.txtNMLS.Text.Length;
      }
    }

    private void txtNMLS_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void btnAddMore_Click(object sender, EventArgs e)
    {
      if (!this.panelDBA2.Visible)
      {
        this.panelDBA2.Visible = true;
        this.txtDBAName2.Focus();
      }
      else if (!this.panelDBA3.Visible)
      {
        this.panelDBA3.Location = new Point(this.panelDBA3.Location.X, this.panelDBA2.Location.Y + this.panelDBA3.Height);
        this.panelDBA3.Visible = true;
        this.txtDBAName3.Focus();
      }
      else if (!this.panelDBA4.Visible)
      {
        this.panelDBA4.Location = new Point(this.panelDBA4.Location.X, this.panelDBA3.Location.Y + this.panelDBA4.Height);
        this.panelDBA4.Visible = true;
        this.txtDBAName4.Focus();
      }
      this.refreshAddDBAButton();
    }

    private void refreshAddDBAButton()
    {
      if (this.txtDBAName2.Text.Trim() != string.Empty)
        this.panelDBA2.Visible = true;
      if (this.txtDBAName3.Text.Trim() != string.Empty)
        this.panelDBA3.Visible = true;
      if (this.txtDBAName4.Text.Trim() != string.Empty)
        this.panelDBA4.Visible = true;
      this.panelAddMore.Top = this.panelDBA1.Top + this.panelDBA1.Height;
      if (this.panelDBA2.Visible || this.txtDBAName2.Text.Trim() != string.Empty)
        this.panelAddMore.Top = this.panelDBA2.Top + this.panelDBA2.Height;
      if (this.panelDBA3.Visible || this.txtDBAName3.Text.Trim() != string.Empty)
        this.panelAddMore.Top = this.panelDBA3.Top + this.panelDBA3.Height;
      if (this.panelDBA4.Visible || this.txtDBAName4.Text.Trim() != string.Empty)
        this.panelAddMore.Top = this.panelDBA4.Top + this.panelDBA4.Height;
      this.btnAddDBA.Enabled = !this.panelDBA4.Visible && !this.parentBox.Checked;
    }

    private void loCompHistoryControl_HistorySelectedIndexChanged(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompCurrentControl_UseParentInfoClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompHistoryControl_AssignPlanButtonClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompCurrentControl_StartDateChanged(object sender, EventArgs e)
    {
      this.loCompHistoryControl.RefreshHistoryList((LoanCompHistory) sender);
    }

    private void AddEditOrgDialog_ResizeEnd(object sender, EventArgs e)
    {
    }

    private void AddEditOrgDialog_Resize(object sender, EventArgs e)
    {
    }

    private void AddEditOrgDialog_SizeChanged(object sender, EventArgs e)
    {
    }

    private void AddEditDialog_Warning(object sender, EventArgs e)
    {
      if (!this.ccSiteControl.invalidCCSiteURL)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Consumer Connect Site URL is not valid or not available. If you have linked this site from a parent organization, please change the linked site URL from that level.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.ccSiteControl.invalidCCSiteURL = false;
    }

    private void chkShowOrgInLOSearch_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkShowOrgInLOSearch.Checked)
      {
        this.txtLOSearchOrgName.Text = this.session.RecacheCompanyInfo().Name;
        this.txtLOSearchOrgName.Enabled = true;
      }
      else
      {
        this.txtLOSearchOrgName.Text = "";
        this.txtLOSearchOrgName.Enabled = false;
      }
    }

    private void ccSiteURLCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      if (comboBox.SelectedItem == null)
        return;
      this.ccSiteSettings.Url = comboBox.SelectedItem.ToString();
      this.ccSiteSettings.SiteId = this.ccSitesLookup[comboBox.SelectedItem.ToString()];
    }

    private void stdButtonNewHMDAProfile_Click(object sender, EventArgs e)
    {
      if (this.hasHMDAProfilePermission)
      {
        AddEditLEIDialog addEditLeiDialog = new AddEditLEIDialog(this.session, 0, true);
        if (addEditLeiDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.profile = addEditLeiDialog.Profile;
        this.refreshLEISection();
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please contact the System Administrator to have access to HMDA Profiles.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void refreshLEISection()
    {
      if (this.profile != null)
      {
        this.stdButtonEditHMDAProfile.Enabled = this.hasEditLEIPermission;
        this.stdButtonNewHMDAProfile.Enabled = this.hasAddLEIPermission;
        this.txtHMDAProfile.Text = this.profile != null ? this.profile.HMDAProfileName : "";
        this.txtLEI.Text = this.profile != null ? this.profile.HMDAProfileLEI : "";
        this.stdButtonViewHMDAProfile.Enabled = true;
      }
      else
      {
        this.stdButtonEditHMDAProfile.Enabled = false;
        this.stdButtonNewHMDAProfile.Enabled = this.hasAddLEIPermission;
        this.stdButtonViewHMDAProfile.Enabled = false;
      }
    }

    private void stdButtonEditHMDAProfile_Click(object sender, EventArgs e)
    {
      if (this.hasHMDAProfilePermission)
      {
        AddEditLEIDialog addEditLeiDialog = new AddEditLEIDialog(this.session, this.profile != null ? this.profile.HMDAProfileID : 0, true);
        if (addEditLeiDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.profile = addEditLeiDialog.Profile;
        this.refreshLEISection();
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please contact the System Administrator to have access to HMDA Profiles.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void stdButtonViewHMDAProfile_Click(object sender, EventArgs e)
    {
      int num = (int) new AddEditLEIDialog(this.session, this.profile != null ? this.profile.HMDAProfileID : 0, false).ShowDialog((IWin32Window) this);
    }

    private bool IsRestrictedAccessEnabled()
    {
      return this.session.UserInfo.IsAdministrator() && this.session.StartupInfo.EnableSSO && this.session.StartupInfo.IsWebLoginEnabled;
    }
  }
}
