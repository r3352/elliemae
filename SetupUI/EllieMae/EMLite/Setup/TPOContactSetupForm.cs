// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPOContactSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TPOContactSetupForm : Form, IHelp
  {
    protected static string sw = Tracing.SwOutsideLoan;
    private AddWebcenterURLControl addURLControl;
    private int externalOrgID = -1;
    private ExternalUserInfo externalUserInfo;
    private Sessions.Session session;
    private List<ExternalUserInfo.ContactRoles> rolesNotIncluded = new List<ExternalUserInfo.ContactRoles>();
    private const string ALLSTATES = "All States";
    private Hashtable stateTable;
    private string primarySalesRepUserId;
    private IOrganizationManager rOrg;
    private IConfigurationManager config;
    private UserInfo[] allInternalUsers;
    private Hashtable orgLookup;
    private List<ExternalSettingValue> settingsList;
    private List<long> allContactId = new List<long>();
    private const string className = "TPOContactSetupForm";
    private DateTime welcomeEmailDate = DateTime.MinValue;
    private DateTime passwordResetEmailDate = DateTime.MinValue;
    private bool requiredPasswordChange;
    private string welcomeEmailUserName = string.Empty;
    private WelcomeEmail email;
    private SessionObjects objs;
    private string newPassword = "";
    private bool edit;
    private ExternalOriginatorManagementData extOrgData;
    private Persona[] personas;
    private Persona[] personaList;
    private StateLicenseSetupControl contactLicenseSetupControl;
    private bool isTPOMVP;
    private AclGroup[] assignedGroup;
    private IContainer components;
    private Button btnCancel;
    private Button btnSave;
    private GroupContainer groupContainer2;
    private CheckBox chkUseParentRateSheet;
    private TextBox txtEmailRateSheet;
    private Label label10;
    private Label label11;
    private TextBox txtEmailLockInfo;
    private TextBox txtFaxRateSheet;
    private TextBox txtFaxLockInfo;
    private Label label12;
    private Label label13;
    private GroupContainer groupContainer1;
    private ComboBox cboStateAddr;
    private TextBox txtSSN;
    private Label label9;
    private TextBox txtNMLSID;
    private TextBox txtEmail;
    private Label label6;
    private Label label7;
    private TextBox txtFax;
    private TextBox txtPhone;
    private Label label4;
    private Label label5;
    private TextBox txtLastName;
    private Label label3;
    private TextBox txtMiddleName;
    private Label label2;
    private CheckBox chkUserParent;
    private TextBox txtZip;
    private TextBox txtFirstName;
    private Label lblAddress;
    private TextBox txtTitle;
    private TextBox txtCity;
    private TextBox txtSuffix;
    private Label lblCity;
    private TextBox txtAddress;
    private Label lblState;
    private Label lblName;
    private Label lblZip;
    private Label label1;
    private Label lblID;
    private TextBox txtCell;
    private Label label14;
    private GroupContainer groupContainer3;
    private CheckBox chkAddWatchList;
    private DatePicker dpApprovalDate;
    private DatePicker dpApprovalCurrentStatusDate;
    private ComboBox cboCurrentStatus;
    private CheckBox chkUserParentApprovalStatus;
    private Label label8;
    private Label label16;
    private Label label17;
    private CheckBox chkUseParentInfoLicense;
    private GroupContainer grpRoles;
    private StandardIconButton btnDeleteRole;
    private StandardIconButton btnAddRole;
    private GridView gridViewRoles;
    private GroupContainer grpLicenseList;
    private GroupContainer groupContainer8;
    private Panel panelWebTop;
    private Label label21;
    private Button btnPreviewSend;
    private Label label24;
    private Button btnReset;
    private Label label23;
    private CheckBox chkDisableLogin;
    private GroupContainer grpNotes;
    private TextBox txtNotes;
    private Label labelLastUpdate;
    private Label lblLastPasswordChange;
    private TextBox txtLoginEmail;
    private Label lblContactID;
    private TextBox txtContactID;
    private Label lblWelcomeEmail;
    private CheckBox chkNmlsCurrent;
    private EMHelpLink emHelpLink2;
    private FlowLayoutPanel flowLayoutPanel1;
    private Panel panel1;
    private GroupContainer gcSalesRepInfo;
    private StandardIconButton btnAddRep;
    private TextBox txtNameSalesRep;
    private Label label15;
    private Label label18;
    private TextBox txtPhoneSalesRep;
    private TextBox txtPersonaSalesRep;
    private TextBox txtEmailSalesRep;
    private Label label19;
    private Label label20;
    private Label label50;
    private Label label22;
    private Label label25;
    private Label label26;
    private Label label27;
    private GroupContainer grpLoanAccess;
    private CheckBox chbEditSubordinate;
    private CheckBox chbPeerAccess;
    private GroupContainer grpPersonas;
    private Label label28;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnViewPersonaRights;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnRemovePersona;
    private StandardIconButton btnAssignPersona;
    private GridView gvPersonas;
    private GroupContainer grpGroups;
    private FlowLayoutPanel flowLayoutPanel3;
    private Button btnViewGroupRights;
    private GridView gvGroups;
    private PanelEx panelExTop;
    private PanelEx panelExBottom;
    private PanelEx panelExCenter;
    private PanelEx panelExCenterLeft;
    private PanelEx panelExCenterRight;
    private PanelEx panelExGroups;
    private PanelEx panelExPersonas;
    private PanelEx panelExRoles;
    private PanelEx panelExLoanAccess;
    private PanelEx panelExLicense;
    private PanelEx panelExNotes;

    public TPOContactSetupForm(
      int externalOrgID,
      SessionObjects objs,
      ExternalUserInfo externalUserInfo = null)
    {
      this.InitializeComponent();
      this.externalOrgID = externalOrgID;
      this.objs = objs;
      this.settingsList = this.objs.ConfigurationManager.GetExternalOrgSettings()["Current Company Status"];
      this.settingsList.Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.rolesNotIncluded.AddRange((IEnumerable<ExternalUserInfo.ContactRoles>) new ExternalUserInfo.ContactRoles[4]
      {
        ExternalUserInfo.ContactRoles.LoanOfficer,
        ExternalUserInfo.ContactRoles.LoanProcessor,
        ExternalUserInfo.ContactRoles.Manager,
        ExternalUserInfo.ContactRoles.Administrator
      });
      this.externalUserInfo = externalUserInfo;
      this.allContactId = this.objs.ConfigurationManager.GetAllContactID();
      this.rOrg = this.objs.OrganizationManager;
      this.config = this.objs.ConfigurationManager;
      List<ExternalOriginatorManagementData> externalOrganizations = this.objs.ConfigurationManager.GetExternalOrganizations(false, new List<int>()
      {
        externalOrgID
      });
      if (externalOrganizations != null && externalOrganizations.Any<ExternalOriginatorManagementData>())
        this.extOrgData = externalOrganizations[0];
      this.initializeSubordinates();
      this.contactLicenseSetupControl = new StateLicenseSetupControl((UserInfo) externalUserInfo != (UserInfo) null ? externalUserInfo.Licenses : (List<StateLicenseExtType>) null, this.chkUserParent.Checked, false, false);
      this.contactLicenseSetupControl.EditorClosing += new EventHandler(this.contactLicenseSetupControl_EditorClosing);
      this.contactLicenseSetupControl.EditorOpening += new EventHandler(this.contactLicenseSetupControl_EditorOpening);
      this.contactLicenseSetupControl.SubitemChecked += new EventHandler(this.contactLicenseSetupControl_SubitemChecked);
      this.grpLicenseList.Controls.Add((Control) this.contactLicenseSetupControl);
    }

    public TPOContactSetupForm(
      int externalOrgID,
      Sessions.Session session,
      ExternalUserInfo externalUserInfo = null)
    {
      this.session = session;
      this.InitializeComponent();
      this.edit = (UserInfo) externalUserInfo != (UserInfo) null;
      this.externalOrgID = externalOrgID;
      List<ExternalOriginatorManagementData> externalOrganizations = this.session.ConfigurationManager.GetExternalOrganizations(false, new List<int>()
      {
        externalOrgID
      });
      if (externalOrganizations != null && externalOrganizations.Any<ExternalOriginatorManagementData>())
        this.extOrgData = externalOrganizations[0];
      this.rolesNotIncluded.AddRange((IEnumerable<ExternalUserInfo.ContactRoles>) new ExternalUserInfo.ContactRoles[4]
      {
        ExternalUserInfo.ContactRoles.LoanOfficer,
        ExternalUserInfo.ContactRoles.LoanProcessor,
        ExternalUserInfo.ContactRoles.Manager,
        ExternalUserInfo.ContactRoles.Administrator
      });
      this.externalUserInfo = externalUserInfo;
      this.allContactId = this.session.ConfigurationManager.GetAllContactID();
      this.config = this.session.ConfigurationManager;
      this.initializeSubordinates();
      this.setPersonaControls();
      if ((UserInfo) externalUserInfo != (UserInfo) null)
      {
        if (!externalUserInfo.UpdatedByExternal)
        {
          UserInfo user = this.session.OrganizationManager.GetUser(externalUserInfo.UpdatedBy);
          if (user != (UserInfo) null)
            this.labelLastUpdate.Text = string.Format("Record last updated on {0} at {1} by {2}({3})", (object) externalUserInfo.UpdatedDateTime.Date.ToString("MM/dd/yyyy", (IFormatProvider) CultureInfo.InvariantCulture), (object) externalUserInfo.UpdatedDateTime.ToString("HH:mm:ss"), (object) user.FullName, (object) externalUserInfo.UpdatedBy);
        }
        else if (externalUserInfo.UpdatedBy != "")
        {
          ExternalUserInfo userInfoByContactId = this.session.ConfigurationManager.GetExternalUserInfoByContactId(externalUserInfo.UpdatedBy);
          if ((UserInfo) userInfoByContactId != (UserInfo) null)
          {
            string str1 = string.Format("{0} {1} {2}", (object) userInfoByContactId.FirstName, (object) userInfoByContactId.MiddleName, (object) userInfoByContactId.LastName);
            Label labelLastUpdate = this.labelLastUpdate;
            object[] objArray = new object[4];
            DateTime dateTime = externalUserInfo.UpdatedDateTime.Date;
            objArray[0] = (object) dateTime.ToString("MM/dd/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
            dateTime = externalUserInfo.UpdatedDateTime;
            objArray[1] = (object) dateTime.ToString("HH:mm:ss");
            objArray[2] = (object) str1;
            objArray[3] = (object) userInfoByContactId.Email;
            string str2 = string.Format("Record last updated on {0} at {1} by {2}({3})", objArray);
            labelLastUpdate.Text = str2;
          }
        }
      }
      else if (this.extOrgData != null)
        this.chkDisableLogin.Checked = this.extOrgData.DisabledLogin;
      this.stateTable = new Hashtable();
      this.cboStateAddr.Items.AddRange((object[]) Utils.GetStates());
      this.primarySalesRepUserId = !((UserInfo) externalUserInfo == (UserInfo) null) ? this.session.ConfigurationManager.GetExternalUserInfo(externalUserInfo.ExternalUserID).SalesRepID : this.session.ConfigurationManager.GetPrimarySalesRep(externalOrgID);
      if ((this.primarySalesRepUserId ?? "") != string.Empty)
        this.populateSalesRepInfo(this.session.OrganizationManager.GetUser(this.primarySalesRepUserId));
      this.addURLControl = new AddWebcenterURLControl(false, true, this.session, -1, externalUserInfo: externalUserInfo, orgID: (UserInfo) externalUserInfo == (UserInfo) null ? this.externalOrgID : externalUserInfo.ExternalOrgID);
      this.addURLControl.ChangedEvent += new EventHandler(this.textField_Changed);
      this.addURLControl.Dock = DockStyle.Left;
      this.flowLayoutPanel1.AutoScroll = true;
      this.flowLayoutPanel1.WrapContents = false;
      this.flowLayoutPanel1.Controls.Add((Control) this.addURLControl);
      this.addURLControl.Width = this.panelWebTop.Width;
      this.settingsList = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Current Contact Status");
      this.settingsList.Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.cboCurrentStatus.DataSource = (object) this.settingsList;
      this.cboCurrentStatus.DisplayMember = "settingValue";
      this.cboCurrentStatus.ValueMember = "settingId";
      this.initialPageValue();
      this.initFieldEvents();
      this.SetButtonStatus(false);
      this.contactLicenseSetupControl = new StateLicenseSetupControl((UserInfo) externalUserInfo != (UserInfo) null ? externalUserInfo.Licenses : (List<StateLicenseExtType>) null, this.chkUserParent.Checked, false, false);
      this.contactLicenseSetupControl.EditorClosing += new EventHandler(this.contactLicenseSetupControl_EditorClosing);
      this.contactLicenseSetupControl.EditorOpening += new EventHandler(this.contactLicenseSetupControl_EditorOpening);
      this.contactLicenseSetupControl.SubitemChecked += new EventHandler(this.contactLicenseSetupControl_SubitemChecked);
      this.grpLicenseList.Controls.Add((Control) this.contactLicenseSetupControl);
      if (!(this.GetType() == typeof (Form)))
        return;
      TPOClientUtils.DisableFormControls((Form) this);
    }

    private void setPersonaControls()
    {
      this.isTPOMVP = this.session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      if (this.isTPOMVP)
      {
        this.panelExRoles.Visible = false;
        this.panelExPersonas.Visible = true;
        this.panelExGroups.Visible = true;
        this.panelExLoanAccess.Visible = true;
        this.personaList = this.session.PersonaManager.GetAllPersonas();
        if (this.edit)
        {
          this.externalUserInfo.UserPersonas = this.session.ConfigurationManager.GetExternalUserInfoUserPersonas(this.externalUserInfo.ContactID);
          if (UserInfo.IsSuperAdministrator(this.externalUserInfo.ContactID, this.externalUserInfo.UserPersonas))
          {
            this.btnViewPersonaRights.Enabled = false;
            this.btnViewGroupRights.Enabled = false;
          }
          this.loadCurrentAssignedGroup();
        }
        else
        {
          this.btnViewPersonaRights.Enabled = false;
          this.loadDefaultAssignedGroup();
        }
      }
      else
      {
        this.panelExLicense.Height = 310;
        this.panelExNotes.Height = 109;
      }
    }

    private void btnAssignPersona_Click(object sender, EventArgs e)
    {
      using (PersonaSelectionForm personaSelectionForm = new PersonaSelectionForm(this.session, this.personas, this.session.UserInfo, this.externalUserInfo, this.externalOrgID, new PersonaType[2]
      {
        PersonaType.External,
        PersonaType.BothInternalExternal
      }))
      {
        if (DialogResult.Cancel == personaSelectionForm.ShowDialog((IWin32Window) this))
          return;
        this.personas = personaSelectionForm.SelectedPersonas;
        this.refreshPersonas();
        this.SetButtonStatus(true);
      }
    }

    private void btnRemovePersona_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList((ICollection) this.personas);
      foreach (GVItem selectedItem in this.gvPersonas.SelectedItems)
        arrayList.Remove(selectedItem.Value);
      this.personas = (Persona[]) arrayList.ToArray(typeof (Persona));
      this.refreshPersonas();
      this.SetButtonStatus(true);
    }

    private void refreshPersonas()
    {
      this.gvPersonas.Items.Clear();
      if (this.personas != null && this.personas.Length != 0)
      {
        foreach (object persona in this.personas)
          this.gvPersonas.Items.Add(persona);
        this.btnViewPersonaRights.Enabled = (UserInfo) this.externalUserInfo == (UserInfo) null || !this.externalUserInfo.IsSuperAdministrator();
        this.btnViewGroupRights.Enabled = (UserInfo) this.externalUserInfo == (UserInfo) null || !this.externalUserInfo.IsSuperAdministrator();
      }
      else
        this.btnViewPersonaRights.Enabled = false;
    }

    private void loadPersonas()
    {
      if (!this.isTPOMVP)
        return;
      this.personas = this.externalUserInfo.UserPersonas;
      if (this.externalUserInfo.UserPersonas != null && this.externalUserInfo.UserPersonas.Length != 0)
      {
        ArrayList arrayList = new ArrayList();
        foreach (Persona userPersona in this.externalUserInfo.UserPersonas)
        {
          foreach (Persona persona in this.personaList)
          {
            if (persona.ID == userPersona.ID)
              arrayList.Add((object) persona);
          }
        }
        this.personas = (Persona[]) arrayList.ToArray(typeof (Persona));
        this.refreshPersonas();
      }
      else
        this.btnViewPersonaRights.Enabled = false;
    }

    private void gvPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemovePersona.Enabled = this.gvPersonas.SelectedItems.Count > 0;
    }

    private void btnViewPersonaRights_Click(object sender, EventArgs e)
    {
      if (!this.edit)
      {
        if (Utils.Dialog((IWin32Window) this, "Before you can view/edit persona rights, you need to save this new account first.  Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes || !this.semiSave())
          return;
      }
      else
      {
        bool flag1 = true;
        Persona[] userPersonas = this.externalUserInfo.UserPersonas;
        if (userPersonas != null && userPersonas.Length != 0)
        {
          flag1 = false;
          if (this.personas.Length != userPersonas.Length)
          {
            flag1 = true;
          }
          else
          {
            foreach (Persona persona1 in this.personas)
            {
              bool flag2 = false;
              foreach (Persona persona2 in userPersonas)
              {
                if (persona1.ID == persona2.ID)
                {
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
              {
                flag1 = true;
                break;
              }
            }
          }
        }
        if (flag1 && (Utils.Dialog((IWin32Window) this, "Before you can view/edit persona rights, you need to save your new settings first.  Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes || !this.semiSave()))
          return;
      }
      using (PersonaSettingsMainForm settingsMainForm = new PersonaSettingsMainForm((IWin32Window) this, this.session, this.externalUserInfo.ContactID, this.personas))
      {
        int num = (int) settingsMainForm.ShowDialog((IWin32Window) this);
      }
    }

    private void btnViewGroupRights_Click(object sender, EventArgs e)
    {
      using (SecurityGroupSettingsMainForm settingsMainForm = new SecurityGroupSettingsMainForm(this.session, (IWin32Window) null, (UserInfo) this.externalUserInfo == (UserInfo) null ? (string) null : this.externalUserInfo.ContactID, this.assignedGroup, this.isTPOMVP))
      {
        int num = (int) settingsMainForm.ShowDialog((IWin32Window) this);
      }
    }

    private void loadCurrentAssignedGroup()
    {
      this.assignedGroup = this.session.AclGroupManager.GetGroupsOfUser(this.externalUserInfo.ContactID);
      foreach (object itemValue in this.assignedGroup)
        this.gvGroups.Items.Add(itemValue);
      if (this.assignedGroup.Length != 0)
        return;
      this.btnViewGroupRights.Enabled = false;
    }

    private void loadDefaultAssignedGroup()
    {
      this.assignedGroup = this.session.AclGroupManager.GetGroupsOfOrganization(this.session.ConfigurationManager.GetOrgIdForExternalOrgID(this.externalOrgID));
      foreach (object itemValue in this.assignedGroup)
        this.gvGroups.Items.Add(itemValue);
      if (this.assignedGroup.Length != 0)
        return;
      this.btnViewGroupRights.Enabled = false;
    }

    private void contactLicenseSetupControl_EditorClosing(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    private void contactLicenseSetupControl_EditorOpening(object sender, EventArgs e)
    {
      if (!this.chkUseParentInfoLicense.Checked)
        return;
      ((GVSubItemCancelEventArgs) sender).Cancel = true;
    }

    private void contactLicenseSetupControl_SubitemChecked(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    private void populateSalesRepInfo(UserInfo salesRepInfo)
    {
      if (salesRepInfo == (UserInfo) null)
        return;
      this.txtNameSalesRep.Text = salesRepInfo.FullName;
      this.txtPersonaSalesRep.Text = Persona.ToString(salesRepInfo.UserPersonas);
      this.txtEmailSalesRep.Text = salesRepInfo.Email;
      this.txtPhoneSalesRep.Text = salesRepInfo.Phone;
    }

    private void initFieldEvents()
    {
      InputEventHelper inputEventHelper = new InputEventHelper();
      inputEventHelper.AddZipcodeFieldHelper(this.txtZip, this.txtCity, this.cboStateAddr);
      inputEventHelper.AddPhoneFieldHelper(this.txtPhone);
      inputEventHelper.AddPhoneFieldHelper(this.txtFax);
      inputEventHelper.AddPhoneFieldHelper(this.txtFaxRateSheet);
      inputEventHelper.AddPhoneFieldHelper(this.txtFaxLockInfo);
      inputEventHelper.AddPhoneFieldHelper(this.txtPhoneSalesRep);
      inputEventHelper.AddPhoneFieldHelper(this.txtCell);
      inputEventHelper.AddSSNFieldHelper(this.txtSSN);
    }

    private void initialPageValue()
    {
      if ((UserInfo) this.externalUserInfo == (UserInfo) null)
      {
        this.btnDeleteRole.Enabled = false;
        this.txtContactID.Text = this.NewContactID().ToString();
        this.chkUseParentRateSheet.Checked = this.chkUserParent.Checked = true;
      }
      else
      {
        this.requiredPasswordChange = this.externalUserInfo.RequirePasswordChange;
        this.chkUserParent.Checked = this.externalUserInfo.UseCompanyAddress;
        this.chkNmlsCurrent.Checked = this.externalUserInfo.NMLSCurrent;
        this.txtContactID.Text = this.externalUserInfo.ContactID;
        this.txtFirstName.Text = this.externalUserInfo.FirstName;
        this.txtMiddleName.Text = this.externalUserInfo.MiddleName;
        this.txtLastName.Text = this.externalUserInfo.LastName;
        this.txtSuffix.Text = this.externalUserInfo.Suffix;
        this.txtTitle.Text = this.externalUserInfo.Title;
        this.txtAddress.Text = this.externalUserInfo.Address;
        this.txtCity.Text = this.externalUserInfo.City;
        this.cboStateAddr.Text = this.externalUserInfo.State;
        this.txtZip.Text = this.externalUserInfo.Zipcode;
        this.txtPhone.Text = this.externalUserInfo.Phone;
        this.txtCell.Text = this.externalUserInfo.CellPhone;
        this.txtFax.Text = this.externalUserInfo.Fax;
        this.txtEmail.Text = this.externalUserInfo.Email;
        this.txtNMLSID.Text = this.externalUserInfo.NmlsID;
        this.txtSSN.Text = this.externalUserInfo.SSN;
        this.chkUseParentRateSheet.Checked = this.externalUserInfo.UseParentInfoForRateLock;
        this.txtEmailRateSheet.Text = this.externalUserInfo.EmailForRateSheet;
        this.txtFaxRateSheet.Text = this.externalUserInfo.FaxForRateSheet;
        this.txtEmailLockInfo.Text = this.externalUserInfo.EmailForLockInfo;
        this.txtFaxLockInfo.Text = this.externalUserInfo.FaxForLockInfo;
        this.chkDisableLogin.Checked = this.externalUserInfo.DisabledLogin;
        this.txtLoginEmail.Text = this.externalUserInfo.EmailForLogin;
        this.cboCurrentStatus.SelectedValue = (object) this.externalUserInfo.ApprovalCurrentStatus;
        this.chkAddWatchList.Checked = this.externalUserInfo.AddToWatchlist;
        this.dpApprovalCurrentStatusDate.Value = this.externalUserInfo.ApprovalCurrentStatusDate;
        this.dpApprovalDate.Value = this.externalUserInfo.ApprovalDate;
        if ((this.externalUserInfo.Roles & 1) == 1)
        {
          this.gridViewRoles.Items.Add("Loan Officer");
          this.rolesNotIncluded.Remove(ExternalUserInfo.ContactRoles.LoanOfficer);
        }
        if ((this.externalUserInfo.Roles & 2) == 2)
        {
          this.gridViewRoles.Items.Add("Loan Processor");
          this.rolesNotIncluded.Remove(ExternalUserInfo.ContactRoles.LoanProcessor);
        }
        if ((this.externalUserInfo.Roles & 4) == 4)
        {
          this.gridViewRoles.Items.Add("Manager");
          this.rolesNotIncluded.Remove(ExternalUserInfo.ContactRoles.Manager);
        }
        if ((this.externalUserInfo.Roles & 8) == 8)
        {
          this.gridViewRoles.Items.Add("Administrator");
          this.rolesNotIncluded.Remove(ExternalUserInfo.ContactRoles.Administrator);
        }
        if (this.gridViewRoles.Items.Count == 4)
          this.btnAddRole.Enabled = false;
        this.initializeSubordinates();
        for (int index = 0; index < this.externalUserInfo.Licenses.Count; ++index)
        {
          if (!(this.externalUserInfo.Licenses[index].StateAbbrevation == "GU") && !(this.externalUserInfo.Licenses[index].StateAbbrevation == "PR") && !(this.externalUserInfo.Licenses[index].StateAbbrevation == "VI") && !this.stateTable.ContainsKey((object) this.externalUserInfo.Licenses[index].StateAbbrevation))
            this.stateTable.Add((object) this.externalUserInfo.Licenses[index].StateAbbrevation, (object) this.externalUserInfo.Licenses[index]);
        }
        this.txtNotes.Text = this.externalUserInfo.Notes;
        this.welcomeEmailDate = this.externalUserInfo.WelcomeEmailDate;
        this.welcomeEmailUserName = this.externalUserInfo.WelcomeEmailUserName;
        this.passwordResetEmailDate = this.externalUserInfo.PasswordChangedDate;
        if (this.welcomeEmailDate != DateTime.MinValue)
          this.lblWelcomeEmail.Text = "Sent on " + this.welcomeEmailDate.Date.ToString("d", (IFormatProvider) CultureInfo.InvariantCulture) + " at " + this.welcomeEmailDate.ToString("HH:mm:ss");
        if (this.passwordResetEmailDate != DateTime.MinValue && this.passwordResetEmailDate.Date != new DateTime(1900, 1, 1, 0, 0, 0))
          this.lblLastPasswordChange.Text = "Last reset on " + this.externalUserInfo.PasswordChangedDate.ToString("d", (IFormatProvider) CultureInfo.InvariantCulture) + " at " + this.externalUserInfo.PasswordChangedDate.ToString("HH:mm:ss");
        this.loadPersonas();
      }
    }

    private void initializeSubordinates()
    {
      if (this.externalUserInfo != null)
        this.chbEditSubordinate.Checked = this.externalUserInfo.AccessMode == UserInfo.AccessModeEnum.ReadWrite;
      if (this.externalUserInfo == null || this.externalUserInfo.PeerView != UserInfo.UserPeerView.ViewOnly && this.externalUserInfo.PeerView != UserInfo.UserPeerView.Edit)
        return;
      this.chbPeerAccess.Checked = true;
      this.chbEditSubordinate.Enabled = true;
    }

    private void populateExternalUserInfoObj()
    {
      string externalUserId = (UserInfo) this.externalUserInfo == (UserInfo) null ? "" : this.externalUserInfo.ExternalUserID;
      UserInfo.UserPeerView userPeerView = !this.chbPeerAccess.Checked ? UserInfo.UserPeerView.Disabled : (this.chbEditSubordinate.Checked ? UserInfo.UserPeerView.Edit : UserInfo.UserPeerView.ViewOnly);
      UserInfo.AccessModeEnum accessModeEnum = !this.chbEditSubordinate.Checked ? UserInfo.AccessModeEnum.ReadOnly : UserInfo.AccessModeEnum.ReadWrite;
      ExternalUserInfo externalUserInfo = new ExternalUserInfo();
      externalUserInfo.ExternalUserID = externalUserId;
      externalUserInfo.UseCompanyAddress = this.chkUserParent.Checked;
      externalUserInfo.ExternalOrgID = this.externalOrgID;
      externalUserInfo.ContactID = this.txtContactID.Text;
      externalUserInfo.FirstName = this.txtFirstName.Text;
      externalUserInfo.MiddleName = this.txtMiddleName.Text;
      externalUserInfo.LastName = this.txtLastName.Text;
      externalUserInfo.Suffix = this.txtSuffix.Text;
      externalUserInfo.Title = this.txtTitle.Text;
      externalUserInfo.Address = this.txtAddress.Text;
      externalUserInfo.City = this.txtCity.Text;
      externalUserInfo.State = this.cboStateAddr.Text;
      externalUserInfo.Zipcode = this.txtZip.Text;
      externalUserInfo.Phone = this.txtPhone.Text;
      externalUserInfo.CellPhone = this.txtCell.Text;
      externalUserInfo.Fax = this.txtFax.Text;
      externalUserInfo.Email = this.txtEmail.Text;
      externalUserInfo.NmlsID = this.txtNMLSID.Text;
      externalUserInfo.SSN = this.txtSSN.Text;
      externalUserInfo.UseParentInfoForRateLock = this.chkUseParentRateSheet.Checked;
      externalUserInfo.EmailForRateSheet = this.txtEmailRateSheet.Text;
      externalUserInfo.FaxForRateSheet = this.txtFaxRateSheet.Text;
      externalUserInfo.EmailForLockInfo = this.txtEmailLockInfo.Text;
      externalUserInfo.FaxForLockInfo = this.txtFaxLockInfo.Text;
      externalUserInfo.DisabledLogin = this.chkDisableLogin.Checked;
      externalUserInfo.EmailForLogin = this.txtLoginEmail.Text;
      externalUserInfo.WelcomeEmailDate = this.welcomeEmailDate;
      externalUserInfo.WelcomeEmailUserName = this.welcomeEmailUserName;
      externalUserInfo.PasswordChangedDate = this.passwordResetEmailDate;
      externalUserInfo.RequirePasswordChange = this.requiredPasswordChange;
      externalUserInfo.ApprovalCurrentStatus = this.cboCurrentStatus.SelectedIndex > 0 ? ((ExternalSettingValue) this.cboCurrentStatus.SelectedItem).settingId : -1;
      externalUserInfo.AddToWatchlist = this.chkAddWatchList.Checked;
      externalUserInfo.ApprovalCurrentStatusDate = this.dpApprovalCurrentStatusDate.Value;
      externalUserInfo.ApprovalDate = this.dpApprovalDate.Value;
      externalUserInfo.SalesRepID = this.primarySalesRepUserId;
      externalUserInfo.Roles = this.getModifiedRoles();
      externalUserInfo.Licenses = this.contactLicenseSetupControl.GetLicenses(false);
      externalUserInfo.Notes = this.txtNotes.Text.Trim();
      externalUserInfo.UpdatedByExternalAdmin = "";
      externalUserInfo.NMLSCurrent = this.chkNmlsCurrent.Checked;
      externalUserInfo.PeerView = userPeerView;
      externalUserInfo.AccessMode = accessModeEnum;
      externalUserInfo.UserPersonas = this.personas;
      this.externalUserInfo = externalUserInfo;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.contactLicenseSetupControl.StopEditing() || !this.DataValidated())
        return;
      this.populateExternalUserInfoObj();
      this.externalUserInfo = this.session.ConfigurationManager.SaveExternalUserInfo(this.externalUserInfo);
      this.session.ConfigurationManager.SaveExternalUserInfoURLs(this.externalUserInfo.ExternalUserID, this.addURLControl.GetSelectedURLIDs());
      if (this.isTPOMVP)
        this.externalUserInfo.UserPersonas = this.session.ConfigurationManager.GetExternalUserInfoUserPersonas(this.externalUserInfo.ContactID);
      if (!this.edit)
        this.sendEmailTriggerTemplate(this.salesRepEmailTemplate());
      this.DialogResult = DialogResult.OK;
    }

    private bool semiSave()
    {
      if (!this.contactLicenseSetupControl.StopEditing() || !this.DataValidated())
        return false;
      this.populateExternalUserInfoObj();
      this.externalUserInfo = this.session.ConfigurationManager.SaveExternalUserInfo(this.externalUserInfo);
      this.session.ConfigurationManager.SaveExternalUserInfoURLs(this.externalUserInfo.ExternalUserID, this.addURLControl.GetSelectedURLIDs());
      if (this.isTPOMVP)
        this.externalUserInfo.UserPersonas = this.session.ConfigurationManager.GetExternalUserInfoUserPersonas(this.externalUserInfo.ContactID);
      this.SetButtonStatus(false);
      if (!this.edit)
      {
        this.sendEmailTriggerTemplate(this.salesRepEmailTemplate());
        this.edit = true;
      }
      return true;
    }

    private void loadAllInternalUsers()
    {
      this.loadOrgLookUp();
      if (this.allInternalUsers != null)
        return;
      this.allInternalUsers = this.rOrg.GetAllAccessibleSalesRepUsers();
    }

    private UserInfo[] checkAllInternalUsers() => this.allInternalUsers;

    private void loadOrgLookUp()
    {
      if (this.rOrg == null)
        this.rOrg = this.session.OrganizationManager;
      if (this.orgLookup != null)
        return;
      OrgInfo[] allOrganizations = this.rOrg.GetAllOrganizations();
      this.orgLookup = new Hashtable(allOrganizations.Length);
      for (int index = 0; index < allOrganizations.Length; ++index)
        this.orgLookup.Add((object) allOrganizations[index].Oid, (object) allOrganizations[index].OrgName);
    }

    private bool validateUniqueLoginEmailAddressAndUrl(
      string emailID,
      string extUserParameter,
      int[] urlIds)
    {
      List<ExternalUserInfoURL> urlsByContactIds = this.config.GetExternalUserInfoUrlsByContactIds(extUserParameter);
      if (urlsByContactIds != null)
      {
        foreach (int urlId1 in urlIds)
        {
          int urlId = urlId1;
          if (urlsByContactIds.FirstOrDefault<ExternalUserInfoURL>((Func<ExternalUserInfoURL, bool>) (item => item.URLID == urlId)) != null)
            return false;
        }
      }
      return true;
    }

    private bool DataValidated()
    {
      if (this.txtFirstName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The contact first name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtFirstName.Focus();
        return false;
      }
      if (this.txtLastName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The contact last name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLastName.Focus();
        return false;
      }
      if (this.txtEmail.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the contact’s email address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmail.Focus();
        return false;
      }
      if (this.txtEmail.Text.Trim() != "" && !Utils.ValidateEmail(this.txtEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmail.Focus();
        return false;
      }
      if (this.txtEmailLockInfo.Text.Trim() != "" && !Utils.ValidateEmail(this.txtEmailLockInfo.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format for Lock Info is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmailLockInfo.Focus();
        return false;
      }
      if (this.txtEmailRateSheet.Text.Trim() != "" && !Utils.ValidateEmail(this.txtEmailRateSheet.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format for Rate Sheet is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmailRateSheet.Focus();
        return false;
      }
      if (this.txtLoginEmail.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The login email address cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLoginEmail.Focus();
        return false;
      }
      if (this.txtLoginEmail.Text.Trim() != "" && !Utils.ValidateEmail(this.txtLoginEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format for Login Email is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLoginEmail.Focus();
        return false;
      }
      List<ExternalUserInfo> source = (UserInfo) this.externalUserInfo == (UserInfo) null ? this.config.GetAllContactsByLoginEmailId(this.txtLoginEmail.Text.Trim(), "") : this.config.GetAllContactsByLoginEmailId(this.txtLoginEmail.Text.Trim(), this.externalUserInfo.ExternalUserID);
      if (source != null && !this.chkDisableLogin.Checked)
      {
        string extUserParameter = string.Join("','", source.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (e => e.ExternalUserID)).ToArray<string>());
        if (!this.validateUniqueLoginEmailAddressAndUrl(this.txtLoginEmail.Text.Trim(), extUserParameter, this.addURLControl.GetSelectedURLIDs()))
        {
          if (Utils.Dialog((IWin32Window) this, "There are other enabled contacts with the same Login Email Address/URL exists. Do you want to disable them?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          {
            this.txtLoginEmail.Focus();
            return false;
          }
          bool applicationRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_EditContacts);
          if (Session.UserInfo.IsAdministrator() | applicationRight)
          {
            this.disableContactsLogin(extUserParameter, this.addURLControl.GetSelectedURLIDs());
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You don't have access to disable other enabled contacts.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
      }
      if (this.primarySalesRepUserId == null || this.primarySalesRepUserId == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Sales Rep Info is required for the contact.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!this.isTPOMVP && this.gridViewRoles.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a role for the contact.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.btnAddRole_Click((object) null, (EventArgs) null);
        return false;
      }
      if (this.isTPOMVP && this.gvPersonas.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a persona for the contact.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.btnAssignPersona_Click((object) null, (EventArgs) null);
        return false;
      }
      return this.contactLicenseSetupControl.DataValidated();
    }

    private void disableContactsLogin(string extUserParameter, int[] urlIds)
    {
      string externalUserIds = "";
      List<ExternalUserInfoURL> urlsByContactIds = this.config.GetExternalUserInfoUrlsByContactIds(extUserParameter);
      if (urlsByContactIds != null)
      {
        foreach (ExternalUserInfoURL externalUserInfoUrl in urlsByContactIds)
        {
          if (((IEnumerable<int>) urlIds).Contains<int>(externalUserInfoUrl.URLID))
            externalUserIds = externalUserIds + externalUserInfoUrl.ExternalUserID + "','";
        }
      }
      if (externalUserIds.EndsWith("','"))
        externalUserIds = externalUserIds.Substring(0, externalUserIds.Length - 3);
      if (!(externalUserIds != ""))
        return;
      this.config.DisableContactsLogin(externalUserIds);
    }

    private void btnAddRep_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.loadAllInternalUsers();
      List<string> existingUserIDs = new List<string>();
      if (this.primarySalesRepUserId != null)
        existingUserIDs.Add(this.primarySalesRepUserId);
      Cursor.Current = Cursors.Default;
      using (AddSalesRepForm addSalesRepForm = new AddSalesRepForm(this.allInternalUsers, existingUserIDs))
      {
        if (addSalesRepForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (addSalesRepForm.SelectedUsers.Length != 0)
        {
          this.primarySalesRepUserId = addSalesRepForm.SelectedUsers[0].Userid;
          bool flag;
          Session.TpoHierarchyAccessCache.TryGetValue(this.externalOrgID, out flag);
          if (!this.primarySalesRepUserId.Equals(Session.UserID) && !flag && Utils.Dialog((IWin32Window) this, "Are you sure you want to remove yourself as a Sales Rep? You will loose access to this contact later.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
          this.populateSalesRepInfo(addSalesRepForm.SelectedUsers[0]);
        }
        this.SetButtonStatus(true);
      }
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if ((UserInfo) this.externalUserInfo == (UserInfo) null || this.IsDirty)
      {
        switch (Utils.Dialog((IWin32Window) this, "You need to save the new contact first. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        {
          case DialogResult.Yes:
            if (!this.semiSave())
              return;
            break;
          case DialogResult.No:
            return;
        }
      }
      if (!((IEnumerable<ExternalUserURL>) this.session.ConfigurationManager.GetExternalUserInfoURLs(this.externalUserInfo.ExternalUserID)).Any<ExternalUserURL>())
      {
        int num1 = (int) MessageBox.Show("There is no URL selected for this user.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Click OK to generate a temporary password for the contact.  This password will be sent to the contact.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
          return;
        this.newPassword = this.resetPassword(true);
        this.sendEmailTriggerTemplate(this.passwordResetEmailTemplate(this.txtLoginEmail.Text.Trim(), this.newPassword));
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The following password has been generated and emailed to this contact: " + this.newPassword);
      }
    }

    private string resetPassword(bool generateNew)
    {
      if (!generateNew && this.newPassword != "")
        return this.newPassword;
      string newPassword = this.externalUserInfo.GenerateNewPassword();
      this.passwordResetEmailDate = DateTime.Now;
      this.requiredPasswordChange = true;
      this.config.ResetExternalUserInfoPassword(this.externalUserInfo.ExternalUserID, newPassword, this.passwordResetEmailDate, true);
      this.lblLastPasswordChange.Text = "Last Reset on " + this.passwordResetEmailDate.ToString("MM/dd/yyyy", (IFormatProvider) CultureInfo.InvariantCulture) + " at " + this.passwordResetEmailDate.ToString("HH:mm:ss");
      return newPassword;
    }

    public string NewPassword
    {
      get => this.newPassword;
      set => this.newPassword = value;
    }

    private TriggerEmailTemplate passwordResetEmailTemplate(string emailID, string password)
    {
      OrgInfo organization = this.session.OrganizationManager.GetOrganization(this.session.UserInfo.OrgId);
      ExternalUserURL[] externalUserInfoUrLs = this.session.ConfigurationManager.GetExternalUserInfoURLs(this.externalUserInfo.ExternalUserID);
      string subject = organization.OrgName + " - Password Reset";
      string str = "Dear " + this.externalUserInfo.FirstName + ",<br><br>" + "Your " + organization.OrgName + " website password has been reset as indicated below:<br>" + "User Name: " + this.externalUserInfo.EmailForLogin + "<br>" + "Temporary Password: " + password + "<br>" + "You can reset your password at: <br>";
      foreach (ExternalUserURL externalUserUrl in externalUserInfoUrLs)
        str = str + this.session.ConfigurationManager.GetUrlLink(externalUserUrl.URLID) + "<br>or<br>";
      string body = str.Substring(0, str.Length - 6) + "<br>Thank you, <br>" + organization.OrgName + "<br><br>";
      return new TriggerEmailTemplate(subject, body, new List<string>()
      {
        emailID
      }.ToArray(), new int[0], true);
    }

    private TriggerEmailTemplate salesRepEmailTemplate()
    {
      UserInfo userInfo = this.session.UserInfo;
      ExternalOriginatorManagementData externalOrganization = this.session.ConfigurationManager.GetExternalOrganization(false, this.externalUserInfo.ExternalOrgID);
      ExternalUserURL[] externalUserInfoUrLs = this.session.ConfigurationManager.GetExternalUserInfoURLs(this.externalUserInfo.ExternalUserID);
      string subject = "Encompass Notification: A new contact has been created";
      string str1 = "This message is to inform you that " + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(userInfo.FullName) + " (" + userInfo.Email + ") has created a new contact.<br><br>" + "Contact Created: " + DateTime.Now.ToString("d") + " at " + string.Format("{0:t}", (object) DateTime.Now) + " Pacific Standard Time<br>" + "Contact's Name: " + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.externalUserInfo.LastName) + ", " + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.externalUserInfo.FirstName) + "<br>";
      string str2;
      if (externalOrganization.OrganizationType == ExternalOriginatorOrgType.Company)
      {
        str2 = !(externalOrganization.CompanyLegalName != "") ? str1 + "Contact's Company: " + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(externalOrganization.CompanyDBAName) + "<br>" : str1 + "Contact's Company: " + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(externalOrganization.CompanyLegalName) + "<br>";
      }
      else
      {
        string str3 = str1 + "Contact's Company: " + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(externalOrganization.HierarchyPath.Split('\\')[1]) + "<br>";
        if (externalOrganization.OrganizationType == ExternalOriginatorOrgType.Branch)
          str3 += "Contact's Branch: ";
        else if (externalOrganization.OrganizationType == ExternalOriginatorOrgType.CompanyExtension)
          str3 += "Contact's Company Extension: ";
        else if (externalOrganization.OrganizationType == ExternalOriginatorOrgType.BranchExtension)
          str3 += "Contact's Branch Extension: ";
        str2 = !(externalOrganization.CompanyLegalName != "") ? str3 + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(externalOrganization.CompanyDBAName) + "<br>" : str3 + Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(externalOrganization.CompanyLegalName) + "<br>";
      }
      string body = !this.isTPOMVP ? str2 + "Contact's Role: " + TPOUtils.returnRoles(this.externalUserInfo.Roles) + "<br>" + "Encompass TPO WebCenter site(s):<br>" : str2 + "Contact's Persona: " + Persona.ToString(this.externalUserInfo.UserPersonas) + "<br>" + "Encompass TPO Connect site(s):<br>";
      foreach (ExternalUserURL externalUserUrl in externalUserInfoUrLs)
        body = body + this.session.ConfigurationManager.GetUrlLink(externalUserUrl.URLID) + "<br>";
      return new TriggerEmailTemplate(subject, body, new List<string>()
      {
        this.session.OrganizationManager.GetUser(this.externalUserInfo.SalesRepID).Email
      }.ToArray(), new int[0], true);
    }

    private int getModifiedRoles()
    {
      int modifiedRoles = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewRoles.Items)
      {
        if (gvItem.Text == "Loan Officer")
          modifiedRoles |= 1;
        if (gvItem.Text == "Loan Processor")
          modifiedRoles |= 2;
        if (gvItem.Text == "Manager")
          modifiedRoles |= 4;
        if (gvItem.Text == "Administrator")
          modifiedRoles |= 8;
      }
      return modifiedRoles;
    }

    private void btnAddRole_Click(object sender, EventArgs e)
    {
      AddRolesForContacts rolesForContacts = new AddRolesForContacts(this.rolesNotIncluded);
      int num = (int) rolesForContacts.ShowDialog();
      if (rolesForContacts.DialogResult == DialogResult.OK)
      {
        foreach (ExternalUserInfo.ContactRoles contactRoles in rolesForContacts.RolesAdded)
        {
          switch (contactRoles)
          {
            case ExternalUserInfo.ContactRoles.LoanOfficer:
              this.gridViewRoles.Items.Add("Loan Officer");
              break;
            case ExternalUserInfo.ContactRoles.LoanProcessor:
              this.gridViewRoles.Items.Add("Loan Processor");
              break;
            case ExternalUserInfo.ContactRoles.Manager:
              this.gridViewRoles.Items.Add("Manager");
              break;
            case ExternalUserInfo.ContactRoles.Administrator:
              this.gridViewRoles.Items.Add("Administrator");
              break;
          }
          this.rolesNotIncluded.Remove(contactRoles);
        }
        this.SetButtonStatus(true);
      }
      if (this.gridViewRoles.Items.Count == 4)
        this.btnAddRole.Enabled = false;
      if (this.gridViewRoles.Items.Count == 0)
      {
        this.btnDeleteRole.Enabled = false;
      }
      else
      {
        this.btnDeleteRole.Enabled = true;
        this.gridViewRoles.Items[this.gridViewRoles.Items.Count - 1].Selected = true;
      }
    }

    private void btnDeleteRole_Click(object sender, EventArgs e)
    {
      if (this.gridViewRoles.SelectedItems.Count == 0)
        return;
      GVItem selectedItem = this.gridViewRoles.SelectedItems[0];
      if (selectedItem.Text == "Manager" && (UserInfo) this.externalUserInfo != (UserInfo) null && this.session.ConfigurationManager.GetExternalOrganizationCountForManagerID(this.externalUserInfo.ExternalUserID) > 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You cannot have the manager role removed until the Primary Manager is reassigned for the organization in which they are the Primary Manager.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove this role?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          if (selectedItem.Text == "Loan Officer")
            this.rolesNotIncluded.Add(ExternalUserInfo.ContactRoles.LoanOfficer);
          if (selectedItem.Text == "Loan Processor")
            this.rolesNotIncluded.Add(ExternalUserInfo.ContactRoles.LoanProcessor);
          if (selectedItem.Text == "Manager")
            this.rolesNotIncluded.Add(ExternalUserInfo.ContactRoles.Manager);
          if (selectedItem.Text == "Administrator")
            this.rolesNotIncluded.Add(ExternalUserInfo.ContactRoles.Administrator);
          this.gridViewRoles.Items.Remove(selectedItem);
          this.SetButtonStatus(true);
        }
        if (this.gridViewRoles.Items.Count == 4)
          this.btnAddRole.Enabled = false;
        else
          this.btnAddRole.Enabled = true;
        if (this.gridViewRoles.Items.Count == 0)
          this.btnDeleteRole.Enabled = false;
        else
          this.gridViewRoles.Items[0].Selected = true;
      }
    }

    private void chkUserParent_CheckedChanged(object sender, EventArgs e)
    {
      this.txtAddress.Enabled = this.txtCity.Enabled = this.cboStateAddr.Enabled = this.txtZip.Enabled = !this.chkUserParent.Checked;
      this.SetButtonStatus(true);
      if (!this.chkUserParent.Checked)
        return;
      this.txtCity.Text = this.extOrgData.City;
      this.txtAddress.Text = this.extOrgData.Address;
      this.cboStateAddr.Text = this.extOrgData.State;
      this.txtZip.Text = this.extOrgData.Zip;
    }

    private void chkUseParentRateSheet_CheckedChanged(object sender, EventArgs e)
    {
      this.txtEmailRateSheet.Enabled = this.txtFaxRateSheet.Enabled = this.txtEmailLockInfo.Enabled = this.txtFaxLockInfo.Enabled = !this.chkUseParentRateSheet.Checked;
      this.SetButtonStatus(true);
      if (!this.chkUseParentRateSheet.Checked)
        return;
      this.txtEmailRateSheet.Text = this.extOrgData.EmailForRateSheet;
      this.txtFaxRateSheet.Text = this.extOrgData.FaxForRateSheet;
      this.txtEmailLockInfo.Text = this.extOrgData.EmailForLockInfo;
      this.txtFaxLockInfo.Text = this.extOrgData.FaxForLockInfo;
    }

    private void chkUserParentApprovalStatus_CheckedChanged(object sender, EventArgs e)
    {
      this.cboCurrentStatus.Enabled = this.chkAddWatchList.Enabled = this.dpApprovalCurrentStatusDate.Enabled = this.dpApprovalDate.Enabled = !this.chkUserParentApprovalStatus.Checked;
      this.SetButtonStatus(true);
      if (!this.chkUserParentApprovalStatus.Checked)
        return;
      this.cboCurrentStatus.Text = Convert.ToString(this.extOrgData.CurrentStatus);
      this.dpApprovalCurrentStatusDate.Text = Convert.ToString(this.extOrgData.CurrentStatusDate);
      this.dpApprovalDate.Text = Convert.ToString(this.extOrgData.ApplicationDate);
      this.chkAddWatchList.Checked = this.extOrgData.AddToWatchlist;
    }

    private void txtNMLSID_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private long NewContactID() => ExternalUserInfo.NewContactID(this.allContactId);

    private void btnPreviewSend_Click(object sender, EventArgs e)
    {
      if ((UserInfo) this.externalUserInfo == (UserInfo) null || this.IsDirty)
      {
        switch (Utils.Dialog((IWin32Window) this, "You need to save the new contact first. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        {
          case DialogResult.Yes:
            if (!this.semiSave())
              return;
            break;
          case DialogResult.No:
            return;
        }
      }
      if (!((IEnumerable<ExternalUserURL>) this.session.ConfigurationManager.GetExternalUserInfoURLs(this.externalUserInfo.ExternalUserID)).Any<ExternalUserURL>())
      {
        int num1 = (int) MessageBox.Show("There is no URL selected for this user.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.newPassword = this.resetPassword(false);
        this.email = new WelcomeEmail(this.session, this.externalUserInfo, this.session.UserInfo, this.txtLoginEmail.Text.Trim(), this.welcomeEmailDate, this.welcomeEmailUserName, this.newPassword);
        this.email.SendEmail += new EventHandler(this.email_SendEmail);
        int num2 = (int) this.email.ShowDialog();
        if (this.email.DialogResult != DialogResult.OK)
          return;
        this.welcomeEmailDate = this.email.DateTime;
        this.welcomeEmailUserName = this.session.UserInfo.FullName;
      }
    }

    private void email_SendEmail(object sender, EventArgs e)
    {
      this.sendEmailTriggerTemplate((TriggerEmailTemplate) sender);
      this.session.ConfigurationManager.SendWelcomeEmailUserInfo(this.externalUserInfo.ExternalUserID, this.email.DateTime, this.session.UserInfo.FullName);
      Label lblWelcomeEmail = this.lblWelcomeEmail;
      DateTime dateTime = this.email.DateTime;
      string str1 = dateTime.ToString("MM/dd/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
      dateTime = this.email.DateTime;
      string str2 = dateTime.ToString("HH:mm:ss");
      string str3 = "Sent on " + str1 + " at " + str2;
      lblWelcomeEmail.Text = str3;
    }

    private bool sendEmailTriggerTemplate(TriggerEmailTemplate template)
    {
      MailMessage message = new MailMessage();
      message.IsBodyHtml = true;
      try
      {
        message.From = new MailAddress(Session.UserInfo.Email, Session.UserInfo.FullName);
        message.Subject = FieldReplacementRegex.ReplaceLiteral(template.Subject, Session.LoanData);
        message.Body = FieldReplacementRegex.ReplaceLiteral(template.Body, Session.LoanData);
      }
      catch (Exception ex)
      {
        Tracing.Log(TPOContactSetupForm.sw, nameof (TPOContactSetupForm), TraceLevel.Info, "Error initializing MailMessage object for '" + template.Subject + "' -- sender address is invalid or subject/body are corrupt. (" + ex.Message + ")");
        return false;
      }
      List<string> source = new List<string>();
      source.AddRange((IEnumerable<string>) template.RecipientUsers);
      if (source.Count == 0)
      {
        Tracing.Log(TPOContactSetupForm.sw, nameof (TPOContactSetupForm), TraceLevel.Info, "Cannot send trigger email '" + template.Subject + "' -- no recipients found");
        return false;
      }
      bool flag = false;
      foreach (string address in source.ToList<string>())
      {
        if (address != null)
        {
          if ((address ?? "") == "")
          {
            Tracing.Log(TPOContactSetupForm.sw, nameof (TPOContactSetupForm), TraceLevel.Warning, "Cannot send trigger email '" + template.Subject + "' to user '" + address + "' -- no email address available.");
            source.Remove(address);
          }
          else
          {
            message.To.Add(new MailAddress(address, ""));
            flag = true;
          }
        }
      }
      if (!flag)
      {
        Tracing.Log(TPOContactSetupForm.sw, nameof (TPOContactSetupForm), TraceLevel.Info, "Cannot send trigger email '" + template.Subject + "' -- no valid recipients found");
        return false;
      }
      try
      {
        ContactUtils.SendMail(message);
        Tracing.Log(TPOContactSetupForm.sw, nameof (TPOContactSetupForm), TraceLevel.Info, "Successfully sent trigger email '" + template.Subject + "' to recipients: " + string.Join(", ", source.ToArray()));
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(TPOContactSetupForm.sw, nameof (TPOContactSetupForm), TraceLevel.Error, "Failed to send trigger email '" + template.Subject + "' to recipients " + string.Join(", ", source.ToArray()) + ": " + (object) ex);
        return false;
      }
    }

    private void textField_Changed(object sender, EventArgs e) => this.SetButtonStatus(true);

    public void SetButtonStatus(bool value) => this.btnSave.Enabled = value;

    public bool IsDirty => this.btnSave.Enabled;

    private void chkNmlsCurrent_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    private void gridViewRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridViewRoles.Items.Any<GVItem>() && this.gridViewRoles.SelectedItems.Any<GVItem>())
        this.btnDeleteRole.Enabled = true;
      else
        this.btnDeleteRole.Enabled = false;
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Add TPO Contact");
    }

    private void TPOContactSetupForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    private void txtEmail_Leave(object sender, EventArgs e)
    {
      if (this.txtLoginEmail.Text.Trim() == "")
        this.txtLoginEmail.Text = this.txtEmail.Text;
      this.SetButtonStatus(true);
    }

    private void panel1_Resize(object sender, EventArgs e)
    {
      this.groupContainer3.Width = this.panel1.Width / 2 - 5;
      this.gcSalesRepInfo.Width = this.panel1.Width / 2 - 5;
    }

    private void cboCurrentStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.dpApprovalCurrentStatusDate.Value = !((UserInfo) this.externalUserInfo != (UserInfo) null) || Convert.ToInt32(this.cboCurrentStatus.SelectedValue) != this.externalUserInfo.ApprovalCurrentStatus ? (!((UserInfo) this.externalUserInfo == (UserInfo) null) || this.cboCurrentStatus.SelectedIndex != 0 && this.cboCurrentStatus.SelectedIndex != -1 ? DateTime.Now : new DateTime()) : (!(this.externalUserInfo.ApprovalCurrentStatusDate != DateTime.MinValue) ? new DateTime() : this.externalUserInfo.ApprovalCurrentStatusDate);
      }
      catch
      {
        this.dpApprovalCurrentStatusDate.Value = new DateTime();
      }
      this.SetButtonStatus(true);
    }

    private void chbPeerAccess_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
      if (!this.chbPeerAccess.Checked)
      {
        this.chbEditSubordinate.Checked = false;
        this.chbEditSubordinate.Enabled = false;
      }
      else
        this.chbEditSubordinate.Enabled = true;
    }

    private void chbEditSubordinate_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.labelLastUpdate = new Label();
      this.grpNotes = new GroupContainer();
      this.txtNotes = new TextBox();
      this.groupContainer8 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.panelWebTop = new Panel();
      this.label26 = new Label();
      this.lblWelcomeEmail = new Label();
      this.txtLoginEmail = new TextBox();
      this.lblLastPasswordChange = new Label();
      this.label21 = new Label();
      this.btnPreviewSend = new Button();
      this.label24 = new Label();
      this.btnReset = new Button();
      this.label23 = new Label();
      this.chkDisableLogin = new CheckBox();
      this.grpLicenseList = new GroupContainer();
      this.chkUseParentInfoLicense = new CheckBox();
      this.grpRoles = new GroupContainer();
      this.label27 = new Label();
      this.btnDeleteRole = new StandardIconButton();
      this.btnAddRole = new StandardIconButton();
      this.gridViewRoles = new GridView();
      this.groupContainer3 = new GroupContainer();
      this.chkAddWatchList = new CheckBox();
      this.dpApprovalDate = new DatePicker();
      this.dpApprovalCurrentStatusDate = new DatePicker();
      this.cboCurrentStatus = new ComboBox();
      this.chkUserParentApprovalStatus = new CheckBox();
      this.label8 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.chkUseParentRateSheet = new CheckBox();
      this.txtEmailRateSheet = new TextBox();
      this.label10 = new Label();
      this.label11 = new Label();
      this.txtEmailLockInfo = new TextBox();
      this.txtFaxRateSheet = new TextBox();
      this.txtFaxLockInfo = new TextBox();
      this.label12 = new Label();
      this.label13 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.label25 = new Label();
      this.label22 = new Label();
      this.label50 = new Label();
      this.chkNmlsCurrent = new CheckBox();
      this.lblContactID = new Label();
      this.txtContactID = new TextBox();
      this.txtCell = new TextBox();
      this.label14 = new Label();
      this.cboStateAddr = new ComboBox();
      this.txtSSN = new TextBox();
      this.label9 = new Label();
      this.txtNMLSID = new TextBox();
      this.txtEmail = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.txtFax = new TextBox();
      this.txtPhone = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.txtLastName = new TextBox();
      this.label3 = new Label();
      this.txtMiddleName = new TextBox();
      this.label2 = new Label();
      this.chkUserParent = new CheckBox();
      this.txtZip = new TextBox();
      this.txtFirstName = new TextBox();
      this.lblAddress = new Label();
      this.txtTitle = new TextBox();
      this.txtCity = new TextBox();
      this.txtSuffix = new TextBox();
      this.lblCity = new Label();
      this.txtAddress = new TextBox();
      this.lblState = new Label();
      this.lblName = new Label();
      this.lblZip = new Label();
      this.label1 = new Label();
      this.lblID = new Label();
      this.emHelpLink2 = new EMHelpLink();
      this.panel1 = new Panel();
      this.gcSalesRepInfo = new GroupContainer();
      this.btnAddRep = new StandardIconButton();
      this.txtNameSalesRep = new TextBox();
      this.label15 = new Label();
      this.label18 = new Label();
      this.txtPhoneSalesRep = new TextBox();
      this.txtPersonaSalesRep = new TextBox();
      this.txtEmailSalesRep = new TextBox();
      this.label19 = new Label();
      this.label20 = new Label();
      this.grpLoanAccess = new GroupContainer();
      this.chbEditSubordinate = new CheckBox();
      this.chbPeerAccess = new CheckBox();
      this.grpPersonas = new GroupContainer();
      this.label28 = new Label();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnViewPersonaRights = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnRemovePersona = new StandardIconButton();
      this.btnAssignPersona = new StandardIconButton();
      this.gvPersonas = new GridView();
      this.grpGroups = new GroupContainer();
      this.flowLayoutPanel3 = new FlowLayoutPanel();
      this.btnViewGroupRights = new Button();
      this.gvGroups = new GridView();
      this.panelExTop = new PanelEx();
      this.panelExBottom = new PanelEx();
      this.panelExCenter = new PanelEx();
      this.panelExCenterRight = new PanelEx();
      this.panelExNotes = new PanelEx();
      this.panelExLicense = new PanelEx();
      this.panelExLoanAccess = new PanelEx();
      this.panelExGroups = new PanelEx();
      this.panelExPersonas = new PanelEx();
      this.panelExRoles = new PanelEx();
      this.panelExCenterLeft = new PanelEx();
      this.grpNotes.SuspendLayout();
      this.groupContainer8.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.panelWebTop.SuspendLayout();
      this.grpLicenseList.SuspendLayout();
      this.grpRoles.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteRole).BeginInit();
      ((ISupportInitialize) this.btnAddRole).BeginInit();
      this.groupContainer3.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gcSalesRepInfo.SuspendLayout();
      ((ISupportInitialize) this.btnAddRep).BeginInit();
      this.grpLoanAccess.SuspendLayout();
      this.grpPersonas.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnRemovePersona).BeginInit();
      ((ISupportInitialize) this.btnAssignPersona).BeginInit();
      this.grpGroups.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.panelExTop.SuspendLayout();
      this.panelExBottom.SuspendLayout();
      this.panelExCenter.SuspendLayout();
      this.panelExCenterRight.SuspendLayout();
      this.panelExNotes.SuspendLayout();
      this.panelExLicense.SuspendLayout();
      this.panelExLoanAccess.SuspendLayout();
      this.panelExGroups.SuspendLayout();
      this.panelExPersonas.SuspendLayout();
      this.panelExRoles.SuspendLayout();
      this.panelExCenterLeft.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(920, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 43;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(839, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 42;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.labelLastUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.labelLastUpdate.Location = new Point(12, 9);
      this.labelLastUpdate.Name = "labelLastUpdate";
      this.labelLastUpdate.Size = new Size(984, 13);
      this.labelLastUpdate.TabIndex = 37;
      this.labelLastUpdate.Text = "(Last Update)";
      this.labelLastUpdate.TextAlign = ContentAlignment.MiddleRight;
      this.grpNotes.Controls.Add((Control) this.txtNotes);
      this.grpNotes.Dock = DockStyle.Fill;
      this.grpNotes.HeaderForeColor = SystemColors.ControlText;
      this.grpNotes.Location = new Point(3, 3);
      this.grpNotes.Name = "grpNotes";
      this.grpNotes.Size = new Size(555, 79);
      this.grpNotes.TabIndex = 7;
      this.grpNotes.Text = "Notes";
      this.txtNotes.Dock = DockStyle.Fill;
      this.txtNotes.Location = new Point(1, 26);
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.ScrollBars = ScrollBars.Both;
      this.txtNotes.Size = new Size(553, 52);
      this.txtNotes.TabIndex = 41;
      this.txtNotes.TextChanged += new EventHandler(this.textField_Changed);
      this.groupContainer8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupContainer8.AutoScroll = true;
      this.groupContainer8.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer8.Controls.Add((Control) this.chkDisableLogin);
      this.groupContainer8.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer8.Location = new Point(12, 504);
      this.groupContainer8.Name = "groupContainer8";
      this.groupContainer8.Size = new Size(422, 160);
      this.groupContainer8.TabIndex = 2;
      this.groupContainer8.Text = "TPO WebCenter Setup";
      this.flowLayoutPanel1.Controls.Add((Control) this.panelWebTop);
      this.flowLayoutPanel1.Dock = DockStyle.Fill;
      this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
      this.flowLayoutPanel1.Location = new Point(1, 26);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(420, 133);
      this.flowLayoutPanel1.TabIndex = 3;
      this.panelWebTop.Controls.Add((Control) this.label26);
      this.panelWebTop.Controls.Add((Control) this.lblWelcomeEmail);
      this.panelWebTop.Controls.Add((Control) this.txtLoginEmail);
      this.panelWebTop.Controls.Add((Control) this.lblLastPasswordChange);
      this.panelWebTop.Controls.Add((Control) this.label21);
      this.panelWebTop.Controls.Add((Control) this.btnPreviewSend);
      this.panelWebTop.Controls.Add((Control) this.label24);
      this.panelWebTop.Controls.Add((Control) this.btnReset);
      this.panelWebTop.Controls.Add((Control) this.label23);
      this.panelWebTop.Location = new Point(3, 3);
      this.panelWebTop.Name = "panelWebTop";
      this.panelWebTop.Size = new Size(415, 74);
      this.panelWebTop.TabIndex = 2;
      this.label26.AutoSize = true;
      this.label26.BackColor = Color.Transparent;
      this.label26.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label26.ForeColor = Color.Red;
      this.label26.Location = new Point(67, 12);
      this.label26.Name = "label26";
      this.label26.Size = new Size(13, 16);
      this.label26.TabIndex = 45;
      this.label26.Text = "*";
      this.lblWelcomeEmail.AutoSize = true;
      this.lblWelcomeEmail.Location = new Point(227, 55);
      this.lblWelcomeEmail.Name = "lblWelcomeEmail";
      this.lblWelcomeEmail.Size = new Size(0, 13);
      this.lblWelcomeEmail.TabIndex = 32;
      this.txtLoginEmail.Location = new Point(107, 7);
      this.txtLoginEmail.MaxLength = 64;
      this.txtLoginEmail.Name = "txtLoginEmail";
      this.txtLoginEmail.Size = new Size(291, 20);
      this.txtLoginEmail.TabIndex = 2;
      this.txtLoginEmail.TextChanged += new EventHandler(this.textField_Changed);
      this.lblLastPasswordChange.AutoSize = true;
      this.lblLastPasswordChange.Location = new Point(185, 32);
      this.lblLastPasswordChange.Name = "lblLastPasswordChange";
      this.lblLastPasswordChange.Size = new Size(0, 13);
      this.lblLastPasswordChange.TabIndex = 31;
      this.label21.AutoSize = true;
      this.label21.Location = new Point(6, 14);
      this.label21.Name = "label21";
      this.label21.Size = new Size(61, 13);
      this.label21.TabIndex = 28;
      this.label21.Text = "Login Email";
      this.btnPreviewSend.Location = new Point(107, 49);
      this.btnPreviewSend.Name = "btnPreviewSend";
      this.btnPreviewSend.Size = new Size(119, 23);
      this.btnPreviewSend.TabIndex = 4;
      this.btnPreviewSend.Text = "&Preview and Send...";
      this.btnPreviewSend.UseVisualStyleBackColor = true;
      this.btnPreviewSend.Click += new EventHandler(this.btnPreviewSend_Click);
      this.label24.AutoSize = true;
      this.label24.Location = new Point(5, 54);
      this.label24.Name = "label24";
      this.label24.Size = new Size(80, 13);
      this.label24.TabIndex = 24;
      this.label24.Text = "Welcome Email";
      this.btnReset.Location = new Point(107, 27);
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(75, 23);
      this.btnReset.TabIndex = 3;
      this.btnReset.Text = "&Reset";
      this.btnReset.UseVisualStyleBackColor = true;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.label23.AutoSize = true;
      this.label23.Location = new Point(5, 32);
      this.label23.Name = "label23";
      this.label23.Size = new Size(53, 13);
      this.label23.TabIndex = 22;
      this.label23.Text = "Password";
      this.chkDisableLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkDisableLogin.AutoSize = true;
      this.chkDisableLogin.BackColor = Color.Transparent;
      this.chkDisableLogin.Location = new Point(304, 5);
      this.chkDisableLogin.Name = "chkDisableLogin";
      this.chkDisableLogin.Size = new Size(90, 17);
      this.chkDisableLogin.TabIndex = 1;
      this.chkDisableLogin.Text = "Disable Login";
      this.chkDisableLogin.UseVisualStyleBackColor = false;
      this.chkDisableLogin.CheckedChanged += new EventHandler(this.textField_Changed);
      this.grpLicenseList.Controls.Add((Control) this.chkUseParentInfoLicense);
      this.grpLicenseList.Dock = DockStyle.Fill;
      this.grpLicenseList.HeaderForeColor = SystemColors.ControlText;
      this.grpLicenseList.Location = new Point(3, 3);
      this.grpLicenseList.Name = "grpLicenseList";
      this.grpLicenseList.Size = new Size(555, 143);
      this.grpLicenseList.TabIndex = 6;
      this.grpLicenseList.Text = "License";
      this.chkUseParentInfoLicense.AutoSize = true;
      this.chkUseParentInfoLicense.BackColor = Color.Transparent;
      this.chkUseParentInfoLicense.Location = new Point(444, 6);
      this.chkUseParentInfoLicense.Name = "chkUseParentInfoLicense";
      this.chkUseParentInfoLicense.Size = new Size(100, 17);
      this.chkUseParentInfoLicense.TabIndex = 38;
      this.chkUseParentInfoLicense.Text = "Use Parent Info";
      this.chkUseParentInfoLicense.UseVisualStyleBackColor = false;
      this.chkUseParentInfoLicense.Visible = false;
      this.chkUseParentInfoLicense.CheckedChanged += new EventHandler(this.textField_Changed);
      this.grpRoles.Controls.Add((Control) this.label27);
      this.grpRoles.Controls.Add((Control) this.btnDeleteRole);
      this.grpRoles.Controls.Add((Control) this.btnAddRole);
      this.grpRoles.Controls.Add((Control) this.gridViewRoles);
      this.grpRoles.Dock = DockStyle.Fill;
      this.grpRoles.HeaderForeColor = SystemColors.ControlText;
      this.grpRoles.Location = new Point(3, 3);
      this.grpRoles.Name = "grpRoles";
      this.grpRoles.Size = new Size(555, 120);
      this.grpRoles.TabIndex = 5;
      this.grpRoles.Text = "Roles";
      this.label27.AutoSize = true;
      this.label27.BackColor = Color.Transparent;
      this.label27.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label27.ForeColor = Color.Red;
      this.label27.Location = new Point(45, 6);
      this.label27.Name = "label27";
      this.label27.Size = new Size(13, 16);
      this.label27.TabIndex = 45;
      this.label27.Text = "*";
      this.btnDeleteRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteRole.BackColor = Color.Transparent;
      this.btnDeleteRole.Enabled = false;
      this.btnDeleteRole.Location = new Point(528, 6);
      this.btnDeleteRole.MouseDownImage = (Image) null;
      this.btnDeleteRole.Name = "btnDeleteRole";
      this.btnDeleteRole.Size = new Size(16, 16);
      this.btnDeleteRole.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteRole.TabIndex = 32;
      this.btnDeleteRole.TabStop = false;
      this.btnDeleteRole.Click += new EventHandler(this.btnDeleteRole_Click);
      this.btnAddRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddRole.BackColor = Color.Transparent;
      this.btnAddRole.Location = new Point(506, 6);
      this.btnAddRole.MouseDownImage = (Image) null;
      this.btnAddRole.Name = "btnAddRole";
      this.btnAddRole.Size = new Size(16, 16);
      this.btnAddRole.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRole.TabIndex = 31;
      this.btnAddRole.TabStop = false;
      this.btnAddRole.Click += new EventHandler(this.btnAddRole_Click);
      this.gridViewRoles.AllowMultiselect = false;
      this.gridViewRoles.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnRole";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Role";
      gvColumn1.Width = 553;
      this.gridViewRoles.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gridViewRoles.Dock = DockStyle.Fill;
      this.gridViewRoles.HeaderHeight = 0;
      this.gridViewRoles.HeaderVisible = false;
      this.gridViewRoles.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewRoles.Location = new Point(1, 26);
      this.gridViewRoles.Name = "gridViewRoles";
      this.gridViewRoles.Size = new Size(553, 93);
      this.gridViewRoles.TabIndex = 37;
      this.gridViewRoles.SelectedIndexChanged += new EventHandler(this.gridViewRoles_SelectedIndexChanged);
      this.groupContainer3.Controls.Add((Control) this.chkAddWatchList);
      this.groupContainer3.Controls.Add((Control) this.dpApprovalDate);
      this.groupContainer3.Controls.Add((Control) this.dpApprovalCurrentStatusDate);
      this.groupContainer3.Controls.Add((Control) this.cboCurrentStatus);
      this.groupContainer3.Controls.Add((Control) this.chkUserParentApprovalStatus);
      this.groupContainer3.Controls.Add((Control) this.label8);
      this.groupContainer3.Controls.Add((Control) this.label16);
      this.groupContainer3.Controls.Add((Control) this.label17);
      this.groupContainer3.Dock = DockStyle.Left;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(3, 3);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(289, 122);
      this.groupContainer3.TabIndex = 3;
      this.groupContainer3.Text = "Approval Status";
      this.chkAddWatchList.AutoSize = true;
      this.chkAddWatchList.Location = new Point(143, 58);
      this.chkAddWatchList.Name = "chkAddWatchList";
      this.chkAddWatchList.Size = new Size(104, 17);
      this.chkAddWatchList.TabIndex = 29;
      this.chkAddWatchList.Text = "Add to Watchlist";
      this.chkAddWatchList.UseVisualStyleBackColor = true;
      this.chkAddWatchList.CheckedChanged += new EventHandler(this.textField_Changed);
      this.dpApprovalDate.BackColor = SystemColors.Window;
      this.dpApprovalDate.Location = new Point(142, 99);
      this.dpApprovalDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpApprovalDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpApprovalDate.Name = "dpApprovalDate";
      this.dpApprovalDate.Size = new Size(85, 21);
      this.dpApprovalDate.TabIndex = 31;
      this.dpApprovalDate.Tag = (object) "763";
      this.dpApprovalDate.ToolTip = "";
      this.dpApprovalDate.Value = new DateTime(0L);
      this.dpApprovalDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.dpApprovalCurrentStatusDate.BackColor = SystemColors.Window;
      this.dpApprovalCurrentStatusDate.Location = new Point(142, 77);
      this.dpApprovalCurrentStatusDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpApprovalCurrentStatusDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpApprovalCurrentStatusDate.Name = "dpApprovalCurrentStatusDate";
      this.dpApprovalCurrentStatusDate.Size = new Size(85, 21);
      this.dpApprovalCurrentStatusDate.TabIndex = 30;
      this.dpApprovalCurrentStatusDate.Tag = (object) "763";
      this.dpApprovalCurrentStatusDate.ToolTip = "";
      this.dpApprovalCurrentStatusDate.Value = new DateTime(0L);
      this.dpApprovalCurrentStatusDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.cboCurrentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCurrentStatus.FormattingEnabled = true;
      this.cboCurrentStatus.Location = new Point(142, 33);
      this.cboCurrentStatus.Name = "cboCurrentStatus";
      this.cboCurrentStatus.Size = new Size(129, 21);
      this.cboCurrentStatus.TabIndex = 28;
      this.cboCurrentStatus.SelectedIndexChanged += new EventHandler(this.cboCurrentStatus_SelectedIndexChanged);
      this.chkUserParentApprovalStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUserParentApprovalStatus.AutoSize = true;
      this.chkUserParentApprovalStatus.BackColor = Color.Transparent;
      this.chkUserParentApprovalStatus.Location = new Point(186, 6);
      this.chkUserParentApprovalStatus.Name = "chkUserParentApprovalStatus";
      this.chkUserParentApprovalStatus.Size = new Size(100, 17);
      this.chkUserParentApprovalStatus.TabIndex = 27;
      this.chkUserParentApprovalStatus.Text = "Use Parent Info";
      this.chkUserParentApprovalStatus.UseVisualStyleBackColor = false;
      this.chkUserParentApprovalStatus.Visible = false;
      this.chkUserParentApprovalStatus.CheckedChanged += new EventHandler(this.chkUserParentApprovalStatus_CheckedChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 36);
      this.label8.Name = "label8";
      this.label8.Size = new Size(74, 13);
      this.label8.TabIndex = 28;
      this.label8.Text = "Current Status";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(6, 80);
      this.label16.Name = "label16";
      this.label16.Size = new Size(100, 13);
      this.label16.TabIndex = 22;
      this.label16.Text = "Current Status Date";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(6, 102);
      this.label17.Name = "label17";
      this.label17.Size = new Size(79, 13);
      this.label17.TabIndex = 24;
      this.label17.Text = "Approved Date";
      this.groupContainer2.Controls.Add((Control) this.chkUseParentRateSheet);
      this.groupContainer2.Controls.Add((Control) this.txtEmailRateSheet);
      this.groupContainer2.Controls.Add((Control) this.label10);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.Controls.Add((Control) this.txtEmailLockInfo);
      this.groupContainer2.Controls.Add((Control) this.txtFaxRateSheet);
      this.groupContainer2.Controls.Add((Control) this.txtFaxLockInfo);
      this.groupContainer2.Controls.Add((Control) this.label12);
      this.groupContainer2.Controls.Add((Control) this.label13);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(12, 372);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(422, (int) sbyte.MaxValue);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Rate Sheet and Lock Information";
      this.chkUseParentRateSheet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentRateSheet.AutoSize = true;
      this.chkUseParentRateSheet.BackColor = Color.Transparent;
      this.chkUseParentRateSheet.Location = new Point(304, 5);
      this.chkUseParentRateSheet.Name = "chkUseParentRateSheet";
      this.chkUseParentRateSheet.Size = new Size(100, 17);
      this.chkUseParentRateSheet.TabIndex = 18;
      this.chkUseParentRateSheet.Text = "Use Parent Info";
      this.chkUseParentRateSheet.UseVisualStyleBackColor = false;
      this.chkUseParentRateSheet.CheckedChanged += new EventHandler(this.chkUseParentRateSheet_CheckedChanged);
      this.txtEmailRateSheet.Location = new Point(144, 33);
      this.txtEmailRateSheet.MaxLength = 64;
      this.txtEmailRateSheet.Name = "txtEmailRateSheet";
      this.txtEmailRateSheet.Size = new Size((int) byte.MaxValue, 20);
      this.txtEmailRateSheet.TabIndex = 19;
      this.txtEmailRateSheet.TextChanged += new EventHandler(this.textField_Changed);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(6, 36);
      this.label10.Name = "label10";
      this.label10.Size = new Size(89, 13);
      this.label10.TabIndex = 28;
      this.label10.Text = "Rate Sheet Email";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(6, 102);
      this.label11.Name = "label11";
      this.label11.Size = new Size(112, 13);
      this.label11.TabIndex = 26;
      this.label11.Text = "Lock Info Fax Number";
      this.txtEmailLockInfo.Location = new Point(144, 77);
      this.txtEmailLockInfo.MaxLength = 64;
      this.txtEmailLockInfo.Name = "txtEmailLockInfo";
      this.txtEmailLockInfo.Size = new Size((int) byte.MaxValue, 20);
      this.txtEmailLockInfo.TabIndex = 21;
      this.txtEmailLockInfo.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFaxRateSheet.Location = new Point(144, 55);
      this.txtFaxRateSheet.MaxLength = 25;
      this.txtFaxRateSheet.Name = "txtFaxRateSheet";
      this.txtFaxRateSheet.Size = new Size((int) byte.MaxValue, 20);
      this.txtFaxRateSheet.TabIndex = 20;
      this.txtFaxRateSheet.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFaxLockInfo.Location = new Point(144, 99);
      this.txtFaxLockInfo.MaxLength = 25;
      this.txtFaxLockInfo.Name = "txtFaxLockInfo";
      this.txtFaxLockInfo.Size = new Size((int) byte.MaxValue, 20);
      this.txtFaxLockInfo.TabIndex = 22;
      this.txtFaxLockInfo.TextChanged += new EventHandler(this.textField_Changed);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(6, 58);
      this.label12.Name = "label12";
      this.label12.Size = new Size(121, 13);
      this.label12.TabIndex = 22;
      this.label12.Text = "Rate Sheet Fax Number";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(6, 80);
      this.label13.Name = "label13";
      this.label13.Size = new Size(80, 13);
      this.label13.TabIndex = 24;
      this.label13.Text = "Lock Info Email";
      this.groupContainer1.Controls.Add((Control) this.label25);
      this.groupContainer1.Controls.Add((Control) this.label22);
      this.groupContainer1.Controls.Add((Control) this.label50);
      this.groupContainer1.Controls.Add((Control) this.chkNmlsCurrent);
      this.groupContainer1.Controls.Add((Control) this.lblContactID);
      this.groupContainer1.Controls.Add((Control) this.txtContactID);
      this.groupContainer1.Controls.Add((Control) this.txtCell);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.cboStateAddr);
      this.groupContainer1.Controls.Add((Control) this.txtSSN);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.txtNMLSID);
      this.groupContainer1.Controls.Add((Control) this.txtEmail);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.txtFax);
      this.groupContainer1.Controls.Add((Control) this.txtPhone);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.txtLastName);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.txtMiddleName);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.chkUserParent);
      this.groupContainer1.Controls.Add((Control) this.txtZip);
      this.groupContainer1.Controls.Add((Control) this.txtFirstName);
      this.groupContainer1.Controls.Add((Control) this.lblAddress);
      this.groupContainer1.Controls.Add((Control) this.txtTitle);
      this.groupContainer1.Controls.Add((Control) this.txtCity);
      this.groupContainer1.Controls.Add((Control) this.txtSuffix);
      this.groupContainer1.Controls.Add((Control) this.lblCity);
      this.groupContainer1.Controls.Add((Control) this.txtAddress);
      this.groupContainer1.Controls.Add((Control) this.lblState);
      this.groupContainer1.Controls.Add((Control) this.lblName);
      this.groupContainer1.Controls.Add((Control) this.lblZip);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.lblID);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 3);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(422, 365);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "TPO Contact Information";
      this.label25.AutoSize = true;
      this.label25.BackColor = Color.Transparent;
      this.label25.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label25.ForeColor = Color.Red;
      this.label25.Location = new Point(38, 296);
      this.label25.Name = "label25";
      this.label25.Size = new Size(13, 16);
      this.label25.TabIndex = 44;
      this.label25.Text = "*";
      this.label22.AutoSize = true;
      this.label22.BackColor = Color.Transparent;
      this.label22.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label22.ForeColor = Color.Red;
      this.label22.Location = new Point(62, 98);
      this.label22.Name = "label22";
      this.label22.Size = new Size(13, 16);
      this.label22.TabIndex = 43;
      this.label22.Text = "*";
      this.label50.AutoSize = true;
      this.label50.BackColor = Color.Transparent;
      this.label50.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label50.ForeColor = Color.Red;
      this.label50.Location = new Point(62, 56);
      this.label50.Name = "label50";
      this.label50.Size = new Size(13, 16);
      this.label50.TabIndex = 42;
      this.label50.Text = "*";
      this.chkNmlsCurrent.AutoSize = true;
      this.chkNmlsCurrent.Location = new Point(306, 319);
      this.chkNmlsCurrent.Name = "chkNmlsCurrent";
      this.chkNmlsCurrent.Size = new Size(93, 17);
      this.chkNmlsCurrent.TabIndex = 37;
      this.chkNmlsCurrent.Text = "NMLS Current";
      this.chkNmlsCurrent.UseVisualStyleBackColor = true;
      this.chkNmlsCurrent.CheckedChanged += new EventHandler(this.chkNmlsCurrent_CheckedChanged);
      this.lblContactID.AutoSize = true;
      this.lblContactID.Location = new Point(6, 38);
      this.lblContactID.Name = "lblContactID";
      this.lblContactID.Size = new Size(58, 13);
      this.lblContactID.TabIndex = 36;
      this.lblContactID.Text = "Contact ID";
      this.txtContactID.Location = new Point(143, 31);
      this.txtContactID.Name = "txtContactID";
      this.txtContactID.ReadOnly = true;
      this.txtContactID.Size = new Size(256, 20);
      this.txtContactID.TabIndex = 2;
      this.txtCell.Location = new Point(144, 251);
      this.txtCell.MaxLength = 25;
      this.txtCell.Name = "txtCell";
      this.txtCell.Size = new Size((int) byte.MaxValue, 20);
      this.txtCell.TabIndex = 13;
      this.txtCell.TextChanged += new EventHandler(this.textField_Changed);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(6, 254);
      this.label14.Name = "label14";
      this.label14.Size = new Size(98, 13);
      this.label14.TabIndex = 34;
      this.label14.Text = "Cell Phone Number";
      this.cboStateAddr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStateAddr.FormattingEnabled = true;
      this.cboStateAddr.Location = new Point(144, 207);
      this.cboStateAddr.Name = "cboStateAddr";
      this.cboStateAddr.Size = new Size(56, 21);
      this.cboStateAddr.TabIndex = 10;
      this.cboStateAddr.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.txtSSN.Location = new Point(144, 339);
      this.txtSSN.MaxLength = 11;
      this.txtSSN.Name = "txtSSN";
      this.txtSSN.Size = new Size((int) byte.MaxValue, 20);
      this.txtSSN.TabIndex = 17;
      this.txtSSN.TextChanged += new EventHandler(this.textField_Changed);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 342);
      this.label9.Name = "label9";
      this.label9.Size = new Size(87, 13);
      this.label9.TabIndex = 32;
      this.label9.Text = "Social Security #";
      this.txtNMLSID.Location = new Point(144, 317);
      this.txtNMLSID.MaxLength = 12;
      this.txtNMLSID.Name = "txtNMLSID";
      this.txtNMLSID.Size = new Size(156, 20);
      this.txtNMLSID.TabIndex = 16;
      this.txtNMLSID.TextChanged += new EventHandler(this.textField_Changed);
      this.txtNMLSID.KeyPress += new KeyPressEventHandler(this.txtNMLSID_KeyPress);
      this.txtEmail.Location = new Point(144, 295);
      this.txtEmail.MaxLength = 64;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size((int) byte.MaxValue, 20);
      this.txtEmail.TabIndex = 15;
      this.txtEmail.TextChanged += new EventHandler(this.textField_Changed);
      this.txtEmail.Leave += new EventHandler(this.txtEmail_Leave);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 298);
      this.label6.Name = "label6";
      this.label6.Size = new Size(32, 13);
      this.label6.TabIndex = 26;
      this.label6.Text = "Email";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 320);
      this.label7.Name = "label7";
      this.label7.Size = new Size(51, 13);
      this.label7.TabIndex = 28;
      this.label7.Text = "NMLS ID";
      this.txtFax.Location = new Point(144, 273);
      this.txtFax.MaxLength = 25;
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size((int) byte.MaxValue, 20);
      this.txtFax.TabIndex = 14;
      this.txtFax.TextChanged += new EventHandler(this.textField_Changed);
      this.txtPhone.Location = new Point(144, 229);
      this.txtPhone.MaxLength = 25;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size((int) byte.MaxValue, 20);
      this.txtPhone.TabIndex = 12;
      this.txtPhone.TextChanged += new EventHandler(this.textField_Changed);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 232);
      this.label4.Name = "label4";
      this.label4.Size = new Size(78, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "Phone Number";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 276);
      this.label5.Name = "label5";
      this.label5.Size = new Size(64, 13);
      this.label5.TabIndex = 24;
      this.label5.Text = "Fax Number";
      this.txtLastName.Location = new Point(144, 97);
      this.txtLastName.MaxLength = 64;
      this.txtLastName.Name = "txtLastName";
      this.txtLastName.Size = new Size((int) byte.MaxValue, 20);
      this.txtLastName.TabIndex = 5;
      this.txtLastName.TextChanged += new EventHandler(this.textField_Changed);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 100);
      this.label3.Name = "label3";
      this.label3.Size = new Size(58, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "Last Name";
      this.txtMiddleName.Location = new Point(144, 75);
      this.txtMiddleName.MaxLength = 64;
      this.txtMiddleName.Name = "txtMiddleName";
      this.txtMiddleName.Size = new Size((int) byte.MaxValue, 20);
      this.txtMiddleName.TabIndex = 4;
      this.txtMiddleName.TextChanged += new EventHandler(this.textField_Changed);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 78);
      this.label2.Name = "label2";
      this.label2.Size = new Size(69, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "Middle Name";
      this.chkUserParent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUserParent.AutoSize = true;
      this.chkUserParent.BackColor = Color.Transparent;
      this.chkUserParent.Location = new Point(286, 6);
      this.chkUserParent.Name = "chkUserParent";
      this.chkUserParent.Size = new Size(133, 17);
      this.chkUserParent.TabIndex = 1;
      this.chkUserParent.Text = "Use Company Address";
      this.chkUserParent.UseVisualStyleBackColor = false;
      this.chkUserParent.CheckedChanged += new EventHandler(this.chkUserParent_CheckedChanged);
      this.txtZip.Location = new Point(236, 207);
      this.txtZip.MaxLength = 15;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(163, 20);
      this.txtZip.TabIndex = 11;
      this.txtZip.Tag = (object) "2284";
      this.txtZip.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFirstName.Location = new Point(144, 53);
      this.txtFirstName.MaxLength = 64;
      this.txtFirstName.Name = "txtFirstName";
      this.txtFirstName.Size = new Size((int) byte.MaxValue, 20);
      this.txtFirstName.TabIndex = 3;
      this.txtFirstName.TextChanged += new EventHandler(this.textField_Changed);
      this.lblAddress.AutoSize = true;
      this.lblAddress.Location = new Point(6, 166);
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = new Size(45, 13);
      this.lblAddress.TabIndex = 11;
      this.lblAddress.Text = "Address";
      this.txtTitle.Location = new Point(144, 141);
      this.txtTitle.MaxLength = 64;
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.Size = new Size((int) byte.MaxValue, 20);
      this.txtTitle.TabIndex = 7;
      this.txtTitle.TextChanged += new EventHandler(this.textField_Changed);
      this.txtCity.Location = new Point(144, 185);
      this.txtCity.MaxLength = 50;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size((int) byte.MaxValue, 20);
      this.txtCity.TabIndex = 9;
      this.txtCity.TextChanged += new EventHandler(this.textField_Changed);
      this.txtSuffix.Location = new Point(144, 119);
      this.txtSuffix.MaxLength = 64;
      this.txtSuffix.Name = "txtSuffix";
      this.txtSuffix.Size = new Size((int) byte.MaxValue, 20);
      this.txtSuffix.TabIndex = 6;
      this.txtSuffix.TextChanged += new EventHandler(this.textField_Changed);
      this.lblCity.AutoSize = true;
      this.lblCity.Location = new Point(6, 188);
      this.lblCity.Name = "lblCity";
      this.lblCity.Size = new Size(24, 13);
      this.lblCity.TabIndex = 13;
      this.lblCity.Text = "City";
      this.txtAddress.Location = new Point(144, 163);
      this.txtAddress.MaxLength = 100;
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size((int) byte.MaxValue, 20);
      this.txtAddress.TabIndex = 8;
      this.txtAddress.TextChanged += new EventHandler(this.textField_Changed);
      this.lblState.AutoSize = true;
      this.lblState.Location = new Point(6, 210);
      this.lblState.Name = "lblState";
      this.lblState.Size = new Size(32, 13);
      this.lblState.TabIndex = 14;
      this.lblState.Text = "State";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(6, 122);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(33, 13);
      this.lblName.TabIndex = 7;
      this.lblName.Text = "Suffix";
      this.lblZip.AutoSize = true;
      this.lblZip.Location = new Point(209, 210);
      this.lblZip.Name = "lblZip";
      this.lblZip.Size = new Size(22, 13);
      this.lblZip.TabIndex = 16;
      this.lblZip.Text = "Zip";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 144);
      this.label1.Name = "label1";
      this.label1.Size = new Size(27, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "Title";
      this.lblID.AutoSize = true;
      this.lblID.Location = new Point(6, 56);
      this.lblID.Name = "lblID";
      this.lblID.Size = new Size(57, 13);
      this.lblID.TabIndex = 5;
      this.lblID.Text = "First Name";
      this.emHelpLink2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink2.BackColor = Color.Transparent;
      this.emHelpLink2.Cursor = Cursors.Hand;
      this.emHelpLink2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink2.HelpTag = "Setup\\Company Details\\AddTPO Contact";
      this.emHelpLink2.Location = new Point(12, 9);
      this.emHelpLink2.Name = "emHelpLink2";
      this.emHelpLink2.Size = new Size(90, 17);
      this.emHelpLink2.TabIndex = 44;
      this.emHelpLink2.Tag = (object) "";
      this.panel1.Controls.Add((Control) this.groupContainer3);
      this.panel1.Controls.Add((Control) this.gcSalesRepInfo);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(3, 3, 12, 0);
      this.panel1.Size = new Size(570, 125);
      this.panel1.TabIndex = 45;
      this.panel1.Resize += new EventHandler(this.panel1_Resize);
      this.gcSalesRepInfo.Controls.Add((Control) this.btnAddRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtNameSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.label15);
      this.gcSalesRepInfo.Controls.Add((Control) this.label18);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtPhoneSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtPersonaSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtEmailSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.label19);
      this.gcSalesRepInfo.Controls.Add((Control) this.label20);
      this.gcSalesRepInfo.Dock = DockStyle.Right;
      this.gcSalesRepInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcSalesRepInfo.Location = new Point(298, 3);
      this.gcSalesRepInfo.Name = "gcSalesRepInfo";
      this.gcSalesRepInfo.Size = new Size(260, 122);
      this.gcSalesRepInfo.TabIndex = 4;
      this.gcSalesRepInfo.Text = "TPO Sales Rep Information";
      this.btnAddRep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddRep.BackColor = Color.Transparent;
      this.btnAddRep.Location = new Point(233, 6);
      this.btnAddRep.MouseDownImage = (Image) null;
      this.btnAddRep.Name = "btnAddRep";
      this.btnAddRep.Size = new Size(16, 16);
      this.btnAddRep.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRep.TabIndex = 29;
      this.btnAddRep.TabStop = false;
      this.btnAddRep.Click += new EventHandler(this.btnAddRep_Click);
      this.txtNameSalesRep.Location = new Point(91, 33);
      this.txtNameSalesRep.Name = "txtNameSalesRep";
      this.txtNameSalesRep.ReadOnly = true;
      this.txtNameSalesRep.Size = new Size(158, 20);
      this.txtNameSalesRep.TabIndex = 33;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(6, 36);
      this.label15.Name = "label15";
      this.label15.Size = new Size(35, 13);
      this.label15.TabIndex = 28;
      this.label15.Text = "Name";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(6, 102);
      this.label18.Name = "label18";
      this.label18.Size = new Size(32, 13);
      this.label18.TabIndex = 26;
      this.label18.Text = "Email";
      this.txtPhoneSalesRep.Location = new Point(91, 77);
      this.txtPhoneSalesRep.MaxLength = 64;
      this.txtPhoneSalesRep.Name = "txtPhoneSalesRep";
      this.txtPhoneSalesRep.ReadOnly = true;
      this.txtPhoneSalesRep.Size = new Size(158, 20);
      this.txtPhoneSalesRep.TabIndex = 35;
      this.txtPersonaSalesRep.Location = new Point(91, 55);
      this.txtPersonaSalesRep.MaxLength = 64;
      this.txtPersonaSalesRep.Name = "txtPersonaSalesRep";
      this.txtPersonaSalesRep.ReadOnly = true;
      this.txtPersonaSalesRep.Size = new Size(158, 20);
      this.txtPersonaSalesRep.TabIndex = 34;
      this.txtEmailSalesRep.Location = new Point(91, 99);
      this.txtEmailSalesRep.MaxLength = 100;
      this.txtEmailSalesRep.Name = "txtEmailSalesRep";
      this.txtEmailSalesRep.ReadOnly = true;
      this.txtEmailSalesRep.Size = new Size(158, 20);
      this.txtEmailSalesRep.TabIndex = 36;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(6, 58);
      this.label19.Name = "label19";
      this.label19.Size = new Size(46, 13);
      this.label19.TabIndex = 22;
      this.label19.Text = "Persona";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(6, 80);
      this.label20.Name = "label20";
      this.label20.Size = new Size(78, 13);
      this.label20.TabIndex = 24;
      this.label20.Text = "Phone Number";
      this.grpLoanAccess.Controls.Add((Control) this.chbEditSubordinate);
      this.grpLoanAccess.Controls.Add((Control) this.chbPeerAccess);
      this.grpLoanAccess.Dock = DockStyle.Fill;
      this.grpLoanAccess.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpLoanAccess.HeaderForeColor = SystemColors.ControlText;
      this.grpLoanAccess.Location = new Point(3, 3);
      this.grpLoanAccess.Name = "grpLoanAccess";
      this.grpLoanAccess.Size = new Size(555, 88);
      this.grpLoanAccess.TabIndex = 46;
      this.grpLoanAccess.Text = "TPO Access to Loans";
      this.chbEditSubordinate.Enabled = false;
      this.chbEditSubordinate.Location = new Point(10, 62);
      this.chbEditSubordinate.Name = "chbEditSubordinate";
      this.chbEditSubordinate.Size = new Size(184, 19);
      this.chbEditSubordinate.TabIndex = 4;
      this.chbEditSubordinate.Text = "Edit team's loans";
      this.chbEditSubordinate.CheckedChanged += new EventHandler(this.chbEditSubordinate_CheckedChanged);
      this.chbPeerAccess.CheckAlign = ContentAlignment.TopLeft;
      this.chbPeerAccess.Location = new Point(10, 35);
      this.chbPeerAccess.Name = "chbPeerAccess";
      this.chbPeerAccess.Size = new Size(204, 19);
      this.chbPeerAccess.TabIndex = 1;
      this.chbPeerAccess.Text = "View access to team's loans";
      this.chbPeerAccess.CheckedChanged += new EventHandler(this.chbPeerAccess_CheckedChanged);
      this.grpPersonas.Controls.Add((Control) this.label28);
      this.grpPersonas.Controls.Add((Control) this.flowLayoutPanel2);
      this.grpPersonas.Controls.Add((Control) this.gvPersonas);
      this.grpPersonas.Dock = DockStyle.Fill;
      this.grpPersonas.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpPersonas.HeaderForeColor = SystemColors.ControlText;
      this.grpPersonas.Location = new Point(3, 3);
      this.grpPersonas.Name = "grpPersonas";
      this.grpPersonas.Size = new Size(555, 100);
      this.grpPersonas.TabIndex = 46;
      this.grpPersonas.Text = "Personas";
      this.label28.AutoSize = true;
      this.label28.BackColor = Color.Transparent;
      this.label28.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label28.ForeColor = Color.FromArgb(238, 0, 0);
      this.label28.Location = new Point(1, 6);
      this.label28.Name = "label28";
      this.label28.Size = new Size(13, 16);
      this.label28.TabIndex = 18;
      this.label28.Text = "*";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnViewPersonaRights);
      this.flowLayoutPanel2.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnRemovePersona);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAssignPersona);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(389, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(161, 22);
      this.flowLayoutPanel2.TabIndex = 3;
      this.btnViewPersonaRights.Location = new Point(61, 0);
      this.btnViewPersonaRights.Margin = new Padding(0);
      this.btnViewPersonaRights.Name = "btnViewPersonaRights";
      this.btnViewPersonaRights.Size = new Size(100, 22);
      this.btnViewPersonaRights.TabIndex = 20;
      this.btnViewPersonaRights.Text = "View/Edit Rights";
      this.btnViewPersonaRights.Click += new EventHandler(this.btnViewPersonaRights_Click);
      this.verticalSeparator1.Location = new Point(56, 3);
      this.verticalSeparator1.Margin = new Padding(1, 3, 3, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 21;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnRemovePersona.BackColor = Color.Transparent;
      this.btnRemovePersona.Enabled = false;
      this.btnRemovePersona.Location = new Point(36, 3);
      this.btnRemovePersona.Margin = new Padding(2, 3, 3, 3);
      this.btnRemovePersona.MouseDownImage = (Image) null;
      this.btnRemovePersona.Name = "btnRemovePersona";
      this.btnRemovePersona.Size = new Size(16, 16);
      this.btnRemovePersona.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemovePersona.TabIndex = 22;
      this.btnRemovePersona.TabStop = false;
      this.btnRemovePersona.Click += new EventHandler(this.btnRemovePersona_Click);
      this.btnAssignPersona.BackColor = Color.Transparent;
      this.btnAssignPersona.Location = new Point(15, 3);
      this.btnAssignPersona.Margin = new Padding(2, 3, 3, 3);
      this.btnAssignPersona.MouseDownImage = (Image) null;
      this.btnAssignPersona.Name = "btnAssignPersona";
      this.btnAssignPersona.Size = new Size(16, 16);
      this.btnAssignPersona.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAssignPersona.TabIndex = 23;
      this.btnAssignPersona.TabStop = false;
      this.btnAssignPersona.Click += new EventHandler(this.btnAssignPersona_Click);
      this.gvPersonas.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "chPersonas";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "State";
      gvColumn2.Width = 553;
      this.gvPersonas.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvPersonas.Dock = DockStyle.Fill;
      this.gvPersonas.HeaderHeight = 0;
      this.gvPersonas.HeaderVisible = false;
      this.gvPersonas.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPersonas.Location = new Point(1, 26);
      this.gvPersonas.Name = "gvPersonas";
      this.gvPersonas.Size = new Size(553, 73);
      this.gvPersonas.TabIndex = 2;
      this.gvPersonas.SelectedIndexChanged += new EventHandler(this.gvPersonas_SelectedIndexChanged);
      this.grpGroups.Controls.Add((Control) this.flowLayoutPanel3);
      this.grpGroups.Controls.Add((Control) this.gvGroups);
      this.grpGroups.Dock = DockStyle.Fill;
      this.grpGroups.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpGroups.HeaderForeColor = SystemColors.ControlText;
      this.grpGroups.Location = new Point(3, 3);
      this.grpGroups.Name = "grpGroups";
      this.grpGroups.Size = new Size(555, 100);
      this.grpGroups.TabIndex = 47;
      this.grpGroups.Text = "Group Membership";
      this.flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel3.BackColor = Color.Transparent;
      this.flowLayoutPanel3.Controls.Add((Control) this.btnViewGroupRights);
      this.flowLayoutPanel3.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel3.Location = new Point(432, 2);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new Size(117, 22);
      this.flowLayoutPanel3.TabIndex = 3;
      this.btnViewGroupRights.Location = new Point(17, 0);
      this.btnViewGroupRights.Margin = new Padding(0);
      this.btnViewGroupRights.Name = "btnViewGroupRights";
      this.btnViewGroupRights.Size = new Size(100, 22);
      this.btnViewGroupRights.TabIndex = 20;
      this.btnViewGroupRights.Text = "View Rights";
      this.btnViewGroupRights.Click += new EventHandler(this.btnViewGroupRights_Click);
      this.gvGroups.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "State";
      gvColumn3.Width = 553;
      this.gvGroups.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gvGroups.Dock = DockStyle.Fill;
      this.gvGroups.HeaderHeight = 0;
      this.gvGroups.HeaderVisible = false;
      this.gvGroups.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvGroups.Location = new Point(1, 26);
      this.gvGroups.Name = "gvGroups";
      this.gvGroups.Selectable = false;
      this.gvGroups.Size = new Size(553, 73);
      this.gvGroups.TabIndex = 2;
      this.panelExTop.Controls.Add((Control) this.labelLastUpdate);
      this.panelExTop.Dock = DockStyle.Top;
      this.panelExTop.Location = new Point(0, 0);
      this.panelExTop.Name = "panelExTop";
      this.panelExTop.Padding = new Padding(0, 0, 12, 0);
      this.panelExTop.Size = new Size(1008, 24);
      this.panelExTop.TabIndex = 48;
      this.panelExBottom.Controls.Add((Control) this.emHelpLink2);
      this.panelExBottom.Controls.Add((Control) this.btnSave);
      this.panelExBottom.Controls.Add((Control) this.btnCancel);
      this.panelExBottom.Dock = DockStyle.Bottom;
      this.panelExBottom.Location = new Point(0, 688);
      this.panelExBottom.Name = "panelExBottom";
      this.panelExBottom.Size = new Size(1008, 34);
      this.panelExBottom.TabIndex = 49;
      this.panelExCenter.Controls.Add((Control) this.panelExCenterRight);
      this.panelExCenter.Controls.Add((Control) this.panelExCenterLeft);
      this.panelExCenter.Dock = DockStyle.Fill;
      this.panelExCenter.Location = new Point(0, 24);
      this.panelExCenter.Name = "panelExCenter";
      this.panelExCenter.Size = new Size(1008, 664);
      this.panelExCenter.TabIndex = 50;
      this.panelExCenterRight.Controls.Add((Control) this.panelExNotes);
      this.panelExCenterRight.Controls.Add((Control) this.panelExLicense);
      this.panelExCenterRight.Controls.Add((Control) this.panelExLoanAccess);
      this.panelExCenterRight.Controls.Add((Control) this.panelExGroups);
      this.panelExCenterRight.Controls.Add((Control) this.panelExPersonas);
      this.panelExCenterRight.Controls.Add((Control) this.panelExRoles);
      this.panelExCenterRight.Controls.Add((Control) this.panel1);
      this.panelExCenterRight.Dock = DockStyle.Fill;
      this.panelExCenterRight.Location = new Point(438, 0);
      this.panelExCenterRight.Name = "panelExCenterRight";
      this.panelExCenterRight.Size = new Size(570, 664);
      this.panelExCenterRight.TabIndex = 1;
      this.panelExNotes.Controls.Add((Control) this.grpNotes);
      this.panelExNotes.Dock = DockStyle.Top;
      this.panelExNotes.Location = new Point(0, 691);
      this.panelExNotes.Name = "panelExNotes";
      this.panelExNotes.Padding = new Padding(3, 3, 12, 0);
      this.panelExNotes.Size = new Size(570, 82);
      this.panelExNotes.TabIndex = 53;
      this.panelExLicense.Controls.Add((Control) this.grpLicenseList);
      this.panelExLicense.Dock = DockStyle.Top;
      this.panelExLicense.Location = new Point(0, 545);
      this.panelExLicense.Name = "panelExLicense";
      this.panelExLicense.Padding = new Padding(3, 3, 12, 0);
      this.panelExLicense.Size = new Size(570, 146);
      this.panelExLicense.TabIndex = 52;
      this.panelExLoanAccess.Controls.Add((Control) this.grpLoanAccess);
      this.panelExLoanAccess.Dock = DockStyle.Top;
      this.panelExLoanAccess.Location = new Point(0, 454);
      this.panelExLoanAccess.Name = "panelExLoanAccess";
      this.panelExLoanAccess.Padding = new Padding(3, 3, 12, 0);
      this.panelExLoanAccess.Size = new Size(570, 91);
      this.panelExLoanAccess.TabIndex = 51;
      this.panelExLoanAccess.Visible = false;
      this.panelExGroups.Controls.Add((Control) this.grpGroups);
      this.panelExGroups.Dock = DockStyle.Top;
      this.panelExGroups.Location = new Point(0, 351);
      this.panelExGroups.Name = "panelExGroups";
      this.panelExGroups.Padding = new Padding(3, 3, 12, 0);
      this.panelExGroups.Size = new Size(570, 103);
      this.panelExGroups.TabIndex = 48;
      this.panelExGroups.Visible = false;
      this.panelExPersonas.Controls.Add((Control) this.grpPersonas);
      this.panelExPersonas.Dock = DockStyle.Top;
      this.panelExPersonas.Location = new Point(0, 248);
      this.panelExPersonas.Name = "panelExPersonas";
      this.panelExPersonas.Padding = new Padding(3, 3, 12, 0);
      this.panelExPersonas.Size = new Size(570, 103);
      this.panelExPersonas.TabIndex = 50;
      this.panelExPersonas.Visible = false;
      this.panelExRoles.Controls.Add((Control) this.grpRoles);
      this.panelExRoles.Dock = DockStyle.Top;
      this.panelExRoles.Location = new Point(0, 125);
      this.panelExRoles.Name = "panelExRoles";
      this.panelExRoles.Padding = new Padding(3, 3, 12, 0);
      this.panelExRoles.Size = new Size(570, 123);
      this.panelExRoles.TabIndex = 49;
      this.panelExCenterLeft.Controls.Add((Control) this.groupContainer1);
      this.panelExCenterLeft.Controls.Add((Control) this.groupContainer2);
      this.panelExCenterLeft.Controls.Add((Control) this.groupContainer8);
      this.panelExCenterLeft.Dock = DockStyle.Left;
      this.panelExCenterLeft.Location = new Point(0, 0);
      this.panelExCenterLeft.Name = "panelExCenterLeft";
      this.panelExCenterLeft.Size = new Size(438, 664);
      this.panelExCenterLeft.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1008, 722);
      this.Controls.Add((Control) this.panelExCenter);
      this.Controls.Add((Control) this.panelExBottom);
      this.Controls.Add((Control) this.panelExTop);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (TPOContactSetupForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "TPO Contact Details";
      this.KeyDown += new KeyEventHandler(this.TPOContactSetupForm_KeyDown);
      this.grpNotes.ResumeLayout(false);
      this.grpNotes.PerformLayout();
      this.groupContainer8.ResumeLayout(false);
      this.groupContainer8.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.panelWebTop.ResumeLayout(false);
      this.panelWebTop.PerformLayout();
      this.grpLicenseList.ResumeLayout(false);
      this.grpLicenseList.PerformLayout();
      this.grpRoles.ResumeLayout(false);
      this.grpRoles.PerformLayout();
      ((ISupportInitialize) this.btnDeleteRole).EndInit();
      ((ISupportInitialize) this.btnAddRole).EndInit();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.gcSalesRepInfo.ResumeLayout(false);
      this.gcSalesRepInfo.PerformLayout();
      ((ISupportInitialize) this.btnAddRep).EndInit();
      this.grpLoanAccess.ResumeLayout(false);
      this.grpPersonas.ResumeLayout(false);
      this.grpPersonas.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemovePersona).EndInit();
      ((ISupportInitialize) this.btnAssignPersona).EndInit();
      this.grpGroups.ResumeLayout(false);
      this.flowLayoutPanel3.ResumeLayout(false);
      this.panelExTop.ResumeLayout(false);
      this.panelExBottom.ResumeLayout(false);
      this.panelExCenter.ResumeLayout(false);
      this.panelExCenterRight.ResumeLayout(false);
      this.panelExNotes.ResumeLayout(false);
      this.panelExLicense.ResumeLayout(false);
      this.panelExLoanAccess.ResumeLayout(false);
      this.panelExGroups.ResumeLayout(false);
      this.panelExPersonas.ResumeLayout(false);
      this.panelExRoles.ResumeLayout(false);
      this.panelExCenterLeft.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
