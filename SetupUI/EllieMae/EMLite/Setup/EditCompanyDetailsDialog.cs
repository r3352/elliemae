// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyDetailsDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyDetailsDialog : Form, IHelp
  {
    private Sessions.Session session;
    private int parent;
    private int depth;
    private int oid;
    private int companyOrgId;
    private bool edit;
    private string hierarchyPath;
    private bool forLender;
    private Dictionary<int, string> alreadyExists = new Dictionary<int, string>();
    private Dictionary<int, string> hierarchyNodes = new Dictionary<int, string>();
    private EditCompanyLOCompControl companyLOCompControl;
    private EditCompanyFeesControl companyFeesControl;
    private EditOrgLicenseControl orgLicenseControl;
    private EditCompanyLoanTypeControl loanTypeControl;
    private EditCompanyContactControl contactControl;
    private EditCompanyNoteControl notesControl;
    private EditCompanyWebcenterControl webControl;
    private EditCompanyAttachmentControl attachmentControl;
    private EditCompanySalesRepControl salesRepControl;
    private EditCompanyLenderContactsControl lenderContactsControl;
    private EditCompanyInfoControl companyInfoControl;
    private EditCompanyCustomTabs companyCustomTabs;
    private EditCompanyCommitmentControl companyCommitmentControl;
    private EditCompanyTradeMgmtControl companyTradeMgmtControl;
    private EditCompanyONRPControl companyOnrpControl;
    private EditWebcenterDocsControl webcenterDocsControl;
    private EditCompanyDBAControl companyDBAControl;
    private EditCompanyWarehouseControl companyWarehouseControl;
    private bool hasCompanyInformationTabRight = true;
    private bool hasLicenseTabRight = true;
    private bool hasLoanTypeTabRight = true;
    private bool hasTPOContactsTabRight = true;
    private bool hasLOCompTabRight = true;
    private bool hasTradeMgmtTabRight = true;
    private bool hasONRPTabRight = true;
    private bool hasNotesTabRight = true;
    private bool hasTPOWebCenterTabRight = true;
    private bool hasAttachmentsTabRight = true;
    private bool hasTPOSalesRepsTabRight = true;
    private bool hasLenderContactsTabRight = true;
    private bool hasCustomFieldsTabRight = true;
    private bool hasCommitmentTabRight = true;
    private bool hasFeeTabRight = true;
    private bool hasDBATabRight = true;
    private bool hasTPODocTabRight = true;
    private bool hasWarehouseTabRight = true;
    private bool isTpoTool;
    public bool validationFailed;
    private bool validationFailedLicense;
    private bool validationFailedLoanType;
    private bool validationFailedWarehouseType;
    private bool validationFailedLOComp;
    private TabPage selectPage;
    private IContainer components;
    private Button btnOK;
    private TabControl tabControlExAll;
    private TabPage tabPageExCompany;
    private TabPage tabPageExLicense;
    private TabPage tabPageExLoanType;
    private TabPage tabPageExContacts;
    private TabPage tabPageExLOComp;
    private TabPage tabPageExNotes;
    private TabPage tabPageExTPOWeb;
    private TabPage tabPageExAttachments;
    private TabPage tabPageExSales;
    private TabPage tabPageExLenderContacts;
    private Label labelLastUpdate;
    private TabPage tabPageExCustomTabs;
    private EMHelpLink emHelpLink2;
    private TabPage tabPageExCommitment;
    private TabPage tabPageExFees;
    private TabPage tabPageExDBA;
    private TabPage tabPageExWebCenterDocs;
    private ImageList imageList1;
    private TabPage tabPageExWarehouse;
    private TabPage tabPageExOnrp;
    private TabPage tabPageExTradeMgmt;

    public Dictionary<int, string> HierarchyNodes => this.hierarchyNodes;

    public LoanCompHistoryList LOCompHistoryList => this.companyLOCompControl.LOCompHistoryList;

    public EditCompanyDetailsDialog(Sessions.Session session, ExternalOriginatorManagementData org)
      : this(session, org.oid, org.Parent, org.oid, org.Depth, org.HierarchyPath, true, true)
    {
      this.companyInfoControl.SaveButton_Clicked -= new EventHandler(this.companyInfoControl_SaveButtonClicked);
      this.companyInfoControl.Broker_Checked -= new EventHandler(this.companyInfoControl_BrokerChecked);
      this.companyLOCompControl.SaveButton_Clicked -= new EventHandler(this.companyLOCompControl_SaveButtonClicked);
      this.attachmentControl.AlertChanged -= new EventHandler(this.attachmentControl_AlertChanged);
      this.companyInfoControl.DisableControls();
      this.companyLOCompControl.DisableControls();
      this.attachmentControl.DisableControls();
    }

    public EditCompanyDetailsDialog(
      Sessions.Session session,
      int oid,
      int parent,
      int companyOrgId,
      int depth,
      string hierarchyPath,
      bool edit,
      bool isTpoTool)
    {
      this.session = session;
      this.parent = parent;
      this.depth = depth;
      this.oid = oid;
      this.companyOrgId = !isTpoTool || this.parent <= 0 ? companyOrgId : this.parent;
      this.edit = edit;
      this.hierarchyPath = hierarchyPath;
      this.isTpoTool = isTpoTool;
      if (this.hierarchyPath.StartsWith("Lenders"))
        this.forLender = true;
      this.InitializeComponent();
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.hasCompanyInformationTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOCompanyInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CompanyInformation);
      this.hasLicenseTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOLicenseInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_License);
      this.hasLoanTypeTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOLoanTypeInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_LoanType);
      this.hasTPOContactsTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOTPOContactsInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOContacts);
      this.hasLOCompTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOLOCompInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_LOComp);
      this.hasTradeMgmtTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOTradeMgmtInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TradeMgmt);
      this.hasONRPTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOONRPInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ONRP);
      this.hasNotesTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPONotesInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Notes);
      this.hasTPOWebCenterTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOWebCenterSetupInformationn) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_WebCenterSetup);
      this.hasAttachmentsTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOAttachmentsInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Attachments);
      this.hasTPOSalesRepsTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOSalesRepsInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_SalesReps);
      this.hasLenderContactsTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOLenderContactsInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_LenderContacts);
      this.hasCustomFieldsTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOCustomFieldsInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CustomFields);
      this.hasCommitmentTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOCommitmentInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Commitment);
      this.hasFeeTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOFeesInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOFeesTab);
      this.hasDBATabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPODBAInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DBATab);
      this.hasTPODocTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPODocsInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDocumentsTab);
      this.hasWarehouseTabRight = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOWarehouseInformation) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_WarehouseTab);
      if (this.isTpoTool)
        Session.clearTpoCache();
      this.labelLastUpdate.Text = "";
      this.companyInfoControl = new EditCompanyInfoControl(this.session, oid, this.forLender, parent, edit);
      this.companyInfoControl.SaveButton_Clicked += new EventHandler(this.companyInfoControl_SaveButtonClicked);
      this.companyInfoControl.Broker_Checked += new EventHandler(this.companyInfoControl_BrokerChecked);
      this.tabPageExCompany.Controls.Add((Control) this.companyInfoControl);
      this.companyLOCompControl = new EditCompanyLOCompControl(this.session, this.oid, parent, this.forLender, edit);
      this.companyLOCompControl.DisableButtons(this.companyInfoControl.IsBroker);
      this.companyLOCompControl.SaveButton_Clicked += new EventHandler(this.companyLOCompControl_SaveButtonClicked);
      this.tabPageExLOComp.Controls.Add((Control) this.companyLOCompControl);
      this.companyDBAControl = new EditCompanyDBAControl(this.session, this.oid, parent, edit, isTpoTool);
      this.tabPageExDBA.Controls.Add((Control) this.companyDBAControl);
      this.companyFeesControl = new EditCompanyFeesControl(this.session, this.oid, edit, isTpoTool);
      this.tabPageExFees.Controls.Add((Control) this.companyFeesControl);
      this.attachmentControl = new EditCompanyAttachmentControl(this.session, this.oid, edit);
      this.attachmentControl.AlertChanged += new EventHandler(this.attachmentControl_AlertChanged);
      this.tabPageExAttachments.Controls.Add((Control) this.attachmentControl);
      if (this.attachmentControl.AnyAttachmentExpired)
      {
        Element element = (Element) new FormattedText("Attachments");
        this.tabPageExAttachments.ImageIndex = 0;
      }
      else
      {
        this.tabPageExAttachments.ImageIndex = -1;
        this.tabPageExAttachments.Text = "Attachments";
      }
      this.TabControlEnforcement();
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    private void companyInfoControl_BrokerChecked(object sender, EventArgs e)
    {
      if (this.companyLOCompControl == null)
        return;
      this.companyLOCompControl.DisableButtons(this.companyInfoControl.IsBroker);
    }

    private void attachmentControl_AlertChanged(object sender, EventArgs e)
    {
      if (this.attachmentControl.AnyAttachmentExpired)
      {
        Element element = (Element) new FormattedText("Attachments");
        this.tabPageExAttachments.ImageIndex = 0;
      }
      else
      {
        this.tabPageExAttachments.ImageIndex = -1;
        this.tabPageExAttachments.Text = "Attachments";
      }
      this.tabControlExAll.Invalidate();
    }

    private void companyInfoControl_SaveButtonClicked(object sender, EventArgs e)
    {
      this.validationFailed = false;
      ExternalCompanyEventType eventType = ExternalCompanyEventType.Create;
      if (!this.companyInfoControl.DataValidated())
      {
        this.validationFailed = true;
      }
      else
      {
        bool flag1 = false;
        if (this.oid > 0 && this.parent == 0)
        {
          if (!this.companyInfoControl.IsBroker)
          {
            if (this.companyOnrpControl == null)
              this.companyOnrpControl = new EditCompanyONRPControl(this.session, this.oid, this.isTpoTool);
            if (this.companyOnrpControl.IsBrokerEnabled)
            {
              if (Utils.Dialog((IWin32Window) this, "TPO Broker is enabled for ONRP. Turning off the Broker Channel Type will no longer allow ONRP for this TPO Broker. Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3) == DialogResult.OK)
              {
                this.companyOnrpControl.PerformDisableBroker();
                flag1 = true;
              }
              else
              {
                this.validationFailed = true;
                this.companyInfoControl.EnableChannel(ExternalOriginatorEntityType.Broker);
                return;
              }
            }
          }
          if (!this.companyInfoControl.IsCorrespondent)
          {
            if (this.companyOnrpControl == null)
              this.companyOnrpControl = new EditCompanyONRPControl(this.session, this.oid, this.isTpoTool);
            if (this.companyOnrpControl.IsCorrespondentEnabled)
            {
              if (Utils.Dialog((IWin32Window) this, "TPO Correspondent is enabled for ONRP. Turning off the Correspondent Channel Type will no longer allow ONRP for this TPO Correspondent. Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3) == DialogResult.OK)
              {
                this.companyOnrpControl.PerformDisableCorrespondent();
                flag1 = true;
              }
              else
              {
                this.validationFailed = true;
                this.companyInfoControl.EnableChannel(ExternalOriginatorEntityType.Correspondent);
                return;
              }
            }
          }
        }
        ExternalOriginatorManagementData modifiedData = this.companyInfoControl.GetModifiedData();
        ExternalOrgURL[] organizationUrLs = this.session.ConfigurationManager.GetExternalOrganizationURLs();
        List<ExternalOrgURL> source = new List<ExternalOrgURL>();
        List<ExternalOrgURL> selectedOrgUrls1 = this.session.ConfigurationManager.GetSelectedOrgUrls(this.parent);
        if (organizationUrLs != null && ((IEnumerable<ExternalOrgURL>) organizationUrLs).Any<ExternalOrgURL>())
          source = ((IEnumerable<ExternalOrgURL>) organizationUrLs).Where<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => !a.isDeleted)).ToList<ExternalOrgURL>();
        if (this.edit)
        {
          ExternalOriginatorManagementData byoid = this.session.ConfigurationManager.GetByoid(this.forLender, this.oid);
          string hierarchyPath = modifiedData.HierarchyPath != null ? modifiedData.HierarchyPath : this.hierarchyPath;
          if (byoid.contactType == ExternalOriginatorContactType.TPO)
          {
            this.session.ConfigurationManager.UpdateTPOContact(this.forLender, string.Concat((object) this.oid), modifiedData, this.companyLOCompControl.GetUseParentInfo(), this.parent, this.depth, hierarchyPath, this.companyLOCompControl.LOCompHistoryList);
            List<int> organizationDesendents = this.session.ConfigurationManager.GetExternalOrganizationDesendents(this.oid);
            if (organizationDesendents != null && organizationDesendents.Any<int>())
              this.UpdateDescendants(organizationDesendents, this.depth, modifiedData.OrganizationName);
          }
          else if (byoid.contactType == ExternalOriginatorContactType.BusinessContacts)
            this.session.ConfigurationManager.UpdateBusinessContact(this.forLender, this.oid, modifiedData.OrganizationName, modifiedData.CompanyDBAName, modifiedData.CompanyLegalName, modifiedData.Address, modifiedData.City, modifiedData.State, modifiedData.Zip, modifiedData.entityType, this.companyLOCompControl.GetUseParentInfo(), this.parent, this.depth, hierarchyPath, this.companyLOCompControl.LOCompHistoryList);
          else if (byoid.contactType == ExternalOriginatorContactType.FreeEntry)
          {
            this.session.ConfigurationManager.UpdateTPOContact(this.forLender, string.Concat((object) this.oid), modifiedData, this.companyLOCompControl.GetUseParentInfo(), this.parent, this.depth, hierarchyPath, (LoanCompHistoryList) null);
            List<int> organizationDesendents = this.session.ConfigurationManager.GetExternalOrganizationDesendents(this.oid);
            if (byoid.OrganizationType != modifiedData.OrganizationType && (modifiedData.OrganizationType == ExternalOriginatorOrgType.CompanyExtension || modifiedData.OrganizationType == ExternalOriginatorOrgType.Branch))
            {
              int orgType = modifiedData.OrganizationType == ExternalOriginatorOrgType.CompanyExtension ? 1 : 3;
              if (organizationDesendents.Count != 0)
                this.session.ConfigurationManager.UpdateOrgTypeAndTpoID(organizationDesendents, orgType, modifiedData.ExternalID);
            }
            if (organizationDesendents.Count != 0)
              this.UpdateDescendants(organizationDesendents, this.depth, modifiedData.OrganizationName);
          }
          if (source != null && source.Count<ExternalOrgURL>() == 1)
          {
            List<ExternalOrgURL> selectedOrgUrls2 = this.session.ConfigurationManager.GetSelectedOrgUrls(this.oid);
            if (selectedOrgUrls2 == null || !selectedOrgUrls2.Any<ExternalOrgURL>())
            {
              source[0].EntityType = Convert.ToInt32((object) modifiedData.entityType);
              this.session.ConfigurationManager.UpdateExternalOrganizationSelectedURLs(this.oid, source.ToList<ExternalOrgURL>(), -1);
            }
          }
          this.companyFeesControl.ChangeChannel(modifiedData.entityType);
          eventType = ExternalCompanyEventType.Update;
          this.companyInfoControl.DefaultPrimarySalesRepAssignedDate = modifiedData.PrimarySalesRepAssignedDate;
        }
        else
        {
          this.oid = this.session.ConfigurationManager.AddManualContact(this.forLender, modifiedData, this.companyLOCompControl.GetUseParentInfo(), this.parent, this.depth, this.hierarchyPath + "\\" + modifiedData.OrganizationName, this.companyLOCompControl.LOCompHistoryList);
          Session.AddTpoHierarchyAccessCache(this.oid, true);
          this.companyInfoControl.AssignOid(this.oid);
          this.attachmentControl.AssignOid(this.oid);
          this.companyLOCompControl.AssignOid(this.oid);
          this.companyFeesControl.AssignOid(this.oid);
          this.companyDBAControl.AssignOid(this.oid);
          if (source != null && source.Count<ExternalOrgURL>() == 1)
          {
            source[0].EntityType = Convert.ToInt32((object) modifiedData.entityType);
            this.session.ConfigurationManager.UpdateExternalOrganizationSelectedURLs(this.oid, source.ToList<ExternalOrgURL>(), -1);
          }
          else if (selectedOrgUrls1 != null && selectedOrgUrls1.Any<ExternalOrgURL>() && (modifiedData.OrganizationType == ExternalOriginatorOrgType.CompanyExtension || modifiedData.OrganizationType == ExternalOriginatorOrgType.Branch || modifiedData.OrganizationType == ExternalOriginatorOrgType.BranchExtension))
            this.session.ConfigurationManager.UpdateExternalOrganizationSelectedURLs(this.oid, selectedOrgUrls1.ToList<ExternalOrgURL>(), -1);
          if (modifiedData.OrganizationType != ExternalOriginatorOrgType.Company)
          {
            if (modifiedData.OrganizationType == ExternalOriginatorOrgType.CompanyExtension || modifiedData.OrganizationType == ExternalOriginatorOrgType.Branch || modifiedData.OrganizationType == ExternalOriginatorOrgType.BranchExtension)
              Session.ConfigurationManager.UpdateCustomFieldValues(this.oid, Session.ConfigurationManager.GetCustomFieldValues(this.parent));
            if (this.orgLicenseControl == null && !this.isTpoTool)
            {
              this.orgLicenseControl = new EditOrgLicenseControl(true, this.parent, this.oid, this.session);
              this.orgLicenseControl.RefreshData();
              this.saveLicenseTypes();
              this.orgLicenseControl = (EditOrgLicenseControl) null;
            }
          }
          this.edit = true;
          this.companyInfoControl.edit = true;
          this.companyInfoControl.RefreshHierarchyPath(this.hierarchyPath + "\\" + modifiedData.OrganizationName);
        }
        if (this.companyInfoControl.IsPrimarySalesRepChanged || this.companyInfoControl.IsPrimaryAeAssignedDateChanged)
        {
          List<ExternalOrgSalesRep> repsForCurrentOrg = this.session.ConfigurationManager.GetExternalOrgSalesRepsForCurrentOrg(this.oid);
          bool flag2 = true;
          if (repsForCurrentOrg != null && repsForCurrentOrg.Count > 0)
          {
            for (int index = 0; index < repsForCurrentOrg.Count; ++index)
            {
              if (repsForCurrentOrg[index].userId == this.companyInfoControl.PrimarySalesRepUserId)
              {
                flag2 = false;
                break;
              }
            }
          }
          if (flag2)
            this.session.ConfigurationManager.AddExternalOrganizationSalesReps(new ExternalOrgSalesRep[1]
            {
              new ExternalOrgSalesRep(0, this.oid, this.companyInfoControl.PrimarySalesRepUserId, "", "", "", "", "", "")
            });
          if (this.session.ConfigurationManager.CheckIfSalesRepHasAnyContacts(this.companyInfoControl.DefaultPrimarySalesRepUserId, this.oid))
          {
            if (Utils.Dialog((IWin32Window) this, "Would you like to assign [" + this.companyInfoControl.PrimarySalesRepName + "] to all of the contacts/sub organizations that [" + this.companyInfoControl.DefaultPrimarySalesRepName + "] was assigned to in this organization and below?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
              this.session.ConfigurationManager.ChangeSalesRepForContacts(this.companyInfoControl.DefaultPrimarySalesRepUserId, this.companyInfoControl.PrimarySalesRepUserId, this.oid, modifiedData.PrimarySalesRepAssignedDate);
          }
          this.companyInfoControl.DefaultPrimarySalesRepUserId = this.companyInfoControl.PrimarySalesRepUserId;
          this.companyInfoControl.DefaultPrimarySalesRepName = this.companyInfoControl.PrimarySalesRepName;
          this.companyInfoControl.DefaultPrimarySalesRepAssignedDate = modifiedData.PrimarySalesRepAssignedDate;
        }
        if (!this.hierarchyNodes.ContainsKey(this.oid))
          this.hierarchyNodes.Add(this.oid, modifiedData.OrganizationName);
        else
          this.hierarchyNodes[this.oid] = modifiedData.OrganizationName;
        if (flag1 && this.companyOnrpControl != null)
          this.companyOnrpControl.PerformSave();
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.tabControlExAll.SelectedTab.Text, this.oid, this.forLender, eventType);
        this.companyInfoControl.SetButtonStatus(false);
      }
    }

    private void UpdateDescendants(List<int> descendants, int depth, string organizationName)
    {
      foreach (int descendant in descendants)
      {
        ExternalOriginatorManagementData byoid = this.session.ConfigurationManager.GetByoid(false, descendant);
        if (byoid != null)
        {
          string[] strArray = byoid.HierarchyPath.Split('\\');
          strArray[depth] = organizationName;
          byoid.HierarchyPath = string.Join("\\", strArray);
          this.session.ConfigurationManager.UpdateExternalOrgHeirarchyPath(descendant, byoid.HierarchyPath);
        }
      }
    }

    private void orgLicenseControl_SaveButtonClicked(object sender, EventArgs e)
    {
      this.saveLicenseTypes();
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.tabControlExAll.SelectedTab.Text, this.oid);
    }

    private void saveLicenseTypes()
    {
      this.validationFailedLicense = false;
      if (!this.orgLicenseControl.DataValidated())
      {
        this.validationFailedLicense = true;
      }
      else
      {
        this.session.ConfigurationManager.UpdateExternalLicence(this.orgLicenseControl.GetModifiedData(), this.oid);
        this.orgLicenseControl.SetButtonStatus(false);
      }
    }

    private void loanTypeControl_SaveButtonClicked(object sender, EventArgs e)
    {
      this.saveLoanTypes();
    }

    private void saveLoanTypes()
    {
      this.validationFailedLoanType = false;
      if (!this.loanTypeControl.DataValidated())
      {
        this.validationFailedLoanType = true;
      }
      else
      {
        this.session.ConfigurationManager.UpdateExternalOrganizationLoanTypes(this.oid, this.loanTypeControl.GetModifiedData());
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.tabControlExAll.SelectedTab.Text, this.oid);
        this.loanTypeControl.Reset();
        this.loanTypeControl.SetButtonStatus(false);
      }
    }

    private void warehouseTypeControl_SaveButtonClicked(object sender, EventArgs e)
    {
      this.saveWarehouseTypes(sender, e);
    }

    private void saveWarehouseTypes(object sender, EventArgs e)
    {
      this.validationFailedWarehouseType = false;
      if (!this.companyWarehouseControl.validate())
        this.validationFailedWarehouseType = true;
      else
        this.companyWarehouseControl.btSave_Click(sender, e);
    }

    private void companyLOCompControl_SaveButtonClicked(object sender, EventArgs e)
    {
      this.validationFailedLOComp = false;
      if (!this.companyLOCompControl.DataValidated())
      {
        this.validationFailedLOComp = true;
      }
      else
      {
        LoanCompHistoryList loCompHistoryList = this.companyLOCompControl.LOCompHistoryList;
        loCompHistoryList.UncheckParentInfo = this.companyLOCompControl.UncheckParentInfo();
        this.session.ConfigurationManager.UpdateExternalOrgLOCompPlans(false, this.oid, loCompHistoryList);
        this.session.ConfigurationManager.UpdateLOCompUseParentInfo(this.companyLOCompControl.GetUseParentInfo(), this.oid);
        this.companyLOCompControl.SetButtonStatus(false);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.tabControlExAll.SelectedTab.Text, this.oid, this.forLender);
      }
    }

    private void tabControlExAll_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectPage = this.tabControlExAll.SelectedTab;
      this.tabControlExAll.SelectedIndexChanged -= new EventHandler(this.tabControlExAll_SelectedIndexChanged);
      bool flag = false;
      if (this.companyInfoControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExCompany;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.companyInfoControl_SaveButtonClicked(sender, e);
          if (!this.validationFailed)
            this.tabControlExAll.SelectedTab = this.selectPage;
        }
      }
      if (this.companyLOCompControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExLOComp;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.companyLOCompControl_SaveButtonClicked(sender, e);
          if (!this.validationFailedLOComp)
            this.tabControlExAll.SelectedTab = this.selectPage;
        }
      }
      if (this.orgLicenseControl != null && this.orgLicenseControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExLicense;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.orgLicenseControl_SaveButtonClicked(sender, e);
          if (!this.validationFailedLicense)
            this.tabControlExAll.SelectedTab = this.selectPage;
        }
      }
      if (this.loanTypeControl != null && this.loanTypeControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExLoanType;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.loanTypeControl_SaveButtonClicked(sender, e);
          if (!this.validationFailedLoanType)
            this.tabControlExAll.SelectedTab = this.selectPage;
        }
      }
      if (this.companyCommitmentControl != null && this.companyCommitmentControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExCommitment;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
          this.companyCommitmentControl.PerformSave();
      }
      if (this.companyOnrpControl != null && this.companyOnrpControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExOnrp;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
          this.companyOnrpControl.PerformSave();
      }
      if (this.webControl != null && this.webControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExTPOWeb;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.webControl.standardIconButton1_Click(sender, e);
          if (!this.webControl.IsDirty)
            this.tabControlExAll.SelectedTab = this.selectPage;
        }
        flag = true;
      }
      if (this.companyDBAControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExDBA;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.companyDBAControl.Save();
          this.tabControlExAll.SelectedTab = this.selectPage;
        }
      }
      if (this.companyTradeMgmtControl != null && this.companyTradeMgmtControl.IsDirty)
      {
        this.tabControlExAll.SelectedTab = this.tabPageExTradeMgmt;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.companyTradeMgmtControl.PerformSave();
          this.tabControlExAll.SelectedTab = this.selectPage;
        }
      }
      if (this.attachmentControl.AnyAttachmentExpired)
      {
        Element element = (Element) new FormattedText("Attachments");
        this.tabPageExAttachments.ImageIndex = 0;
      }
      else
      {
        this.tabPageExAttachments.ImageIndex = -1;
        this.tabPageExAttachments.Text = "Attachments";
      }
      if (this.companyCustomTabs != null && (this.companyCustomTabs.IsDirty || this.companyCustomTabs.IsChkParentInfoUiDirty))
      {
        this.tabControlExAll.SelectedTab = this.tabPageExCustomTabs;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          this.companyCustomTabs.saveButton_Click(sender, e);
          this.tabControlExAll.SelectedTab = this.selectPage;
        }
      }
      if (this.companyWarehouseControl != null && (this.companyWarehouseControl.IsDirty || this.companyWarehouseControl.IsChkParentInfoDirty))
      {
        this.tabControlExAll.SelectedTab = this.tabPageExWarehouse;
        if (new SaveMessage().ShowDialog() == DialogResult.OK)
        {
          if (this.companyWarehouseControl.IsDirty)
            this.warehouseTypeControl_SaveButtonClicked(sender, e);
          if (!this.validationFailedWarehouseType)
          {
            if (this.companyWarehouseControl.IsChkParentInfoDirty)
              this.companyWarehouseControl.btnSave_Click(sender, e);
            this.tabControlExAll.SelectedTab = this.selectPage;
          }
        }
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExCompany && this.companyInfoControl != null && (!this.companyInfoControl.IsDirty || this.isTpoTool))
        this.companyInfoControl.RefreshPrimarySalesInfo(this.oid, this.parent);
      if (this.tabControlExAll.SelectedTab == this.tabPageExLicense && this.orgLicenseControl == null)
      {
        this.orgLicenseControl = new EditOrgLicenseControl(true, this.parent, this.oid, this.session);
        this.orgLicenseControl.RefreshData();
        if (!this.isTpoTool)
          this.orgLicenseControl.SaveButton_Clicked += new EventHandler(this.orgLicenseControl_SaveButtonClicked);
        this.tabPageExLicense.Controls.Add((Control) this.orgLicenseControl);
        if (this.isTpoTool)
          this.orgLicenseControl.DisableControls();
        if (!this.isTpoTool && this.orgLicenseControl.saveOnLoad)
          this.saveLicenseTypes();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExLoanType && this.loanTypeControl == null)
      {
        this.loanTypeControl = new EditCompanyLoanTypeControl(this.session, this.oid, this.parent);
        if (!this.isTpoTool)
          this.loanTypeControl.SaveButton_Clicked += new EventHandler(this.loanTypeControl_SaveButtonClicked);
        this.tabPageExLoanType.Controls.Add((Control) this.loanTypeControl);
        if (!this.isTpoTool && this.loanTypeControl.saveOnLoad)
          this.saveLoanTypes();
        if (this.isTpoTool)
          this.loanTypeControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExCommitment && this.companyCommitmentControl == null)
      {
        this.companyCommitmentControl = new EditCompanyCommitmentControl(this.session, this.oid, this.isTpoTool);
        this.companyCommitmentControl.SaveButton_Clicked += new EventHandler(this.companyCommitmentControl_SaveButton_Clicked);
        this.tabPageExCommitment.Controls.Add((Control) this.companyCommitmentControl);
        if (this.isTpoTool)
          this.companyCommitmentControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExTradeMgmt && this.companyTradeMgmtControl == null)
      {
        this.companyTradeMgmtControl = new EditCompanyTradeMgmtControl(this.session, this.oid, this.isTpoTool);
        this.companyTradeMgmtControl.SaveButton_Clicked += new EventHandler(this.companyTradeMgmtControl_SaveButton_Clicked);
        this.tabPageExTradeMgmt.Controls.Add((Control) this.companyTradeMgmtControl);
        if (this.isTpoTool)
          this.companyTradeMgmtControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExOnrp)
      {
        if (this.companyOnrpControl == null)
        {
          this.companyOnrpControl = new EditCompanyONRPControl(this.session, this.oid, this.isTpoTool);
          this.tabPageExOnrp.Controls.Add((Control) this.companyOnrpControl);
        }
        else
        {
          if (this.tabPageExOnrp.Controls.Count == 0)
            this.tabPageExOnrp.Controls.Add((Control) this.companyOnrpControl);
          (this.tabPageExOnrp.Controls[0] as EditCompanyONRPControl).LoadData();
        }
        if (this.isTpoTool)
          this.companyOnrpControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExContacts && this.contactControl == null)
      {
        this.contactControl = new EditCompanyContactControl(this.session, this.oid);
        this.tabPageExContacts.Controls.Add((Control) this.contactControl);
        if (this.isTpoTool)
          this.contactControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExWarehouse && this.companyWarehouseControl == null)
      {
        this.companyWarehouseControl = new EditCompanyWarehouseControl(this.session, this.oid, this.parent, this.edit, this.isTpoTool);
        this.tabPageExWarehouse.Controls.Add((Control) this.companyWarehouseControl);
        this.companyWarehouseControl.Dock = DockStyle.Fill;
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExNotes && this.notesControl == null)
      {
        this.notesControl = new EditCompanyNoteControl(this.session, this.oid);
        this.tabPageExNotes.Controls.Add((Control) this.notesControl);
        if (this.isTpoTool)
          this.notesControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExTPOWeb && (!flag || this.isTpoTool))
      {
        if (this.webControl != null)
          this.tabPageExTPOWeb.Controls.Remove((Control) this.webControl);
        this.webControl = new EditCompanyWebcenterControl(this.session, this.oid, this.parent);
        this.tabPageExTPOWeb.Controls.Add((Control) this.webControl);
        if (this.isTpoTool)
          this.webControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExWebCenterDocs)
      {
        this.webcenterDocsControl = new EditWebcenterDocsControl(this.session, this.oid, this.companyOrgId, this.isTpoTool);
        this.tabPageExWebCenterDocs.Controls.Add((Control) this.webcenterDocsControl);
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExAttachments && this.attachmentControl == null)
      {
        this.attachmentControl = new EditCompanyAttachmentControl(this.session, this.oid, this.edit);
        this.tabPageExAttachments.Controls.Add((Control) this.attachmentControl);
        if (this.attachmentControl.AnyAttachmentExpired)
        {
          Element element = (Element) new FormattedText("Attachments");
          this.tabPageExAttachments.ImageIndex = 0;
        }
        if (this.isTpoTool)
          this.attachmentControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExSales)
      {
        if (this.salesRepControl == null)
        {
          this.salesRepControl = new EditCompanySalesRepControl(this.session, this.oid, this.companyOrgId);
          this.tabPageExSales.Controls.Add((Control) this.salesRepControl);
        }
        else
          this.salesRepControl.RefreshCompanySalesRepControl(this.session, this.oid, this.companyOrgId);
        if (this.isTpoTool)
          this.salesRepControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExLenderContacts)
      {
        if (this.lenderContactsControl == null)
        {
          this.lenderContactsControl = new EditCompanyLenderContactsControl(this.session, this.oid, this.companyOrgId);
          this.tabPageExLenderContacts.Controls.Add((Control) this.lenderContactsControl);
        }
        else
          this.lenderContactsControl.RefreshCompanyLenderContactsControl(this.session, this.oid, this.companyOrgId);
        if (this.isTpoTool)
          this.lenderContactsControl.DisableControls();
      }
      if (this.tabControlExAll.SelectedTab == this.tabPageExCustomTabs && this.companyCustomTabs == null)
      {
        this.companyCustomTabs = new EditCompanyCustomTabs(this.session, this.oid, this.companyOrgId, this.parent, this.isTpoTool);
        this.companyCustomTabs.Dock = DockStyle.Fill;
        this.tabPageExCustomTabs.Controls.Add((Control) this.companyCustomTabs);
        if (this.isTpoTool)
          this.companyCustomTabs.DisableControls();
      }
      this.TabControlEnforcement();
      this.tabControlExAll.SelectedIndexChanged += new EventHandler(this.tabControlExAll_SelectedIndexChanged);
    }

    private void companyCommitmentControl_SaveButton_Clicked(object sender, EventArgs e)
    {
      this.validationFailed = false;
      this.companyCommitmentControl.ValidationFailed = false;
      if (!this.companyCommitmentControl.DataValidated())
      {
        this.companyCommitmentControl.ValidationFailed = true;
        this.validationFailed = true;
      }
      else
        this.companyInfoControl.ResetExternalObject();
    }

    private void companyTradeMgmtControl_SaveButton_Clicked(object sender, EventArgs e)
    {
      this.validationFailed = false;
      this.companyTradeMgmtControl.ValidationFailed = false;
      if (!this.companyTradeMgmtControl.DataValidated())
      {
        this.companyTradeMgmtControl.ValidationFailed = true;
        this.validationFailed = true;
      }
      else
        this.companyInfoControl.ResetExternalObject();
    }

    private void EditCompanyDetailsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.isTpoTool)
      {
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        if (this.companyInfoControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.companyInfoControl_SaveButtonClicked((object) null, (EventArgs) null);
              if (this.validationFailed)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.companyLOCompControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.companyLOCompControl_SaveButtonClicked((object) null, (EventArgs) null);
              if (this.validationFailedLOComp)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.companyDBAControl != null && this.companyDBAControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.companyDBAControl.Save();
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.orgLicenseControl != null && this.orgLicenseControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.orgLicenseControl_SaveButtonClicked((object) null, (EventArgs) null);
              if (this.validationFailedLicense)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.companyCommitmentControl != null && this.companyCommitmentControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.companyCommitmentControl.PerformSave();
              if (this.validationFailedLoanType)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.companyTradeMgmtControl != null && this.companyTradeMgmtControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.companyTradeMgmtControl.PerformSave();
              if (this.validationFailedLoanType)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.companyOnrpControl != null && this.companyOnrpControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              if (!this.companyOnrpControl.PerformSave() || this.validationFailedLoanType)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.loanTypeControl != null && this.loanTypeControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.loanTypeControl_SaveButtonClicked((object) null, (EventArgs) null);
              if (this.validationFailedLoanType)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.companyCustomTabs != null && (this.companyCustomTabs.IsDirty || this.companyCustomTabs.IsChkParentInfoUiDirty))
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.companyCustomTabs.saveButton_Click((object) null, (EventArgs) null);
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.companyWarehouseControl != null && (this.companyWarehouseControl.IsDirty || this.companyWarehouseControl.IsChkParentInfoDirty))
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              if (this.companyWarehouseControl.IsDirty)
                this.warehouseTypeControl_SaveButtonClicked(sender, (EventArgs) e);
              if (this.validationFailedWarehouseType)
                e.Cancel = true;
              if (this.companyWarehouseControl.IsChkParentInfoDirty)
                this.companyWarehouseControl.btnSave_Click(sender, (EventArgs) e);
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        else if (this.webControl != null && this.webControl.IsDirty)
        {
          switch (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Cancel:
              e.Cancel = true;
              break;
            case DialogResult.Yes:
              this.webControl.standardIconButton1_Click((object) null, (EventArgs) null);
              if (this.webControl.IsDirty)
                e.Cancel = true;
              this.DialogResult = DialogResult.OK;
              break;
            case DialogResult.No:
              this.DialogResult = DialogResult.Cancel;
              break;
          }
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Company Details");
    }

    public void TabControlEnforcement()
    {
      if (!this.hasCompanyInformationTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExCompany);
      if (!this.hasLicenseTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExLicense);
      if (!this.hasLoanTypeTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExLoanType);
      if (!this.hasTPOContactsTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExContacts);
      if (!this.hasLOCompTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExLOComp);
      if (!this.hasTradeMgmtTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExTradeMgmt);
      if (!this.hasONRPTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExOnrp);
      if (!this.hasNotesTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExNotes);
      if (!this.hasTPOWebCenterTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExTPOWeb);
      if (!this.hasAttachmentsTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExAttachments);
      if (!this.hasTPOSalesRepsTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExSales);
      if (!this.hasLenderContactsTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExLenderContacts);
      if (!this.hasCustomFieldsTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExCustomTabs);
      if (!this.hasCommitmentTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExCommitment);
      if (!this.hasFeeTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExFees);
      if (!this.hasDBATabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExDBA);
      if (!this.hasTPODocTabRight)
        this.tabControlExAll.TabPages.Remove(this.tabPageExWebCenterDocs);
      if (this.hasWarehouseTabRight)
        return;
      this.tabControlExAll.TabPages.Remove(this.tabPageExWarehouse);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyDetailsDialog));
      this.btnOK = new Button();
      this.labelLastUpdate = new Label();
      this.tabControlExAll = new TabControl();
      this.tabPageExCompany = new TabPage();
      this.tabPageExDBA = new TabPage();
      this.tabPageExLicense = new TabPage();
      this.tabPageExLoanType = new TabPage();
      this.tabPageExContacts = new TabPage();
      this.tabPageExWarehouse = new TabPage();
      this.tabPageExFees = new TabPage();
      this.tabPageExLOComp = new TabPage();
      this.tabPageExCommitment = new TabPage();
      this.tabPageExTradeMgmt = new TabPage();
      this.tabPageExOnrp = new TabPage();
      this.tabPageExNotes = new TabPage();
      this.tabPageExTPOWeb = new TabPage();
      this.tabPageExWebCenterDocs = new TabPage();
      this.tabPageExAttachments = new TabPage();
      this.tabPageExSales = new TabPage();
      this.tabPageExLenderContacts = new TabPage();
      this.tabPageExCustomTabs = new TabPage();
      this.imageList1 = new ImageList(this.components);
      this.emHelpLink2 = new EMHelpLink();
      this.tabControlExAll.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(910, 690);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 20;
      this.btnOK.Text = "&Close";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.labelLastUpdate.Location = new Point(9, 8);
      this.labelLastUpdate.Name = "labelLastUpdate";
      this.labelLastUpdate.Size = new Size(865, 16);
      this.labelLastUpdate.TabIndex = 23;
      this.labelLastUpdate.Text = "(last update)";
      this.labelLastUpdate.TextAlign = ContentAlignment.MiddleRight;
      this.tabControlExAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControlExAll.Controls.Add((Control) this.tabPageExCompany);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExDBA);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExLicense);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExLoanType);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExContacts);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExSales);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExLenderContacts);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExWarehouse);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExFees);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExLOComp);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExCommitment);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExTradeMgmt);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExOnrp);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExNotes);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExTPOWeb);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExWebCenterDocs);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExAttachments);
      this.tabControlExAll.Controls.Add((Control) this.tabPageExCustomTabs);
      this.tabControlExAll.ImageList = this.imageList1;
      this.tabControlExAll.Location = new Point(12, 27);
      this.tabControlExAll.Name = "tabControlExAll";
      this.tabControlExAll.SelectedIndex = 0;
      this.tabControlExAll.Size = new Size(977, 657);
      this.tabControlExAll.TabIndex = 22;
      this.tabControlExAll.TabStop = false;
      this.tabControlExAll.Text = "tabControlEx1";
      this.tabControlExAll.SelectedIndexChanged += new EventHandler(this.tabControlExAll_SelectedIndexChanged);
      this.tabPageExCompany.BackColor = Color.Transparent;
      this.tabPageExCompany.Location = new Point(4, 23);
      this.tabPageExCompany.Name = "tabPageExCompany";
      this.tabPageExCompany.Size = new Size(969, 630);
      this.tabPageExCompany.TabIndex = 0;
      this.tabPageExCompany.Text = "Basic Info";
      this.tabPageExDBA.BackColor = Color.Transparent;
      this.tabPageExDBA.Location = new Point(4, 23);
      this.tabPageExDBA.Name = "tabPageExDBA";
      this.tabPageExDBA.Size = new Size(969, 630);
      this.tabPageExDBA.TabIndex = 0;
      this.tabPageExDBA.Text = "DBA";
      this.tabPageExLicense.BackColor = Color.Transparent;
      this.tabPageExLicense.Location = new Point(4, 23);
      this.tabPageExLicense.Name = "tabPageExLicense";
      this.tabPageExLicense.Size = new Size(969, 630);
      this.tabPageExLicense.TabIndex = 0;
      this.tabPageExLicense.Text = "License";
      this.tabPageExLoanType.BackColor = Color.Transparent;
      this.tabPageExLoanType.Location = new Point(4, 23);
      this.tabPageExLoanType.Name = "tabPageExLoanType";
      this.tabPageExLoanType.Size = new Size(969, 630);
      this.tabPageExLoanType.TabIndex = 0;
      this.tabPageExLoanType.Text = "Loan Criteria";
      this.tabPageExContacts.BackColor = Color.Transparent;
      this.tabPageExContacts.Location = new Point(4, 23);
      this.tabPageExContacts.Name = "tabPageExContacts";
      this.tabPageExContacts.Size = new Size(969, 630);
      this.tabPageExContacts.TabIndex = 0;
      this.tabPageExContacts.Text = "TPO Contacts";
      this.tabPageExWarehouse.Location = new Point(4, 23);
      this.tabPageExWarehouse.Name = "tabPageExWarehouse";
      this.tabPageExWarehouse.Size = new Size(969, 630);
      this.tabPageExWarehouse.TabIndex = 1;
      this.tabPageExWarehouse.Text = "Warehouse";
      this.tabPageExWarehouse.UseVisualStyleBackColor = true;
      this.tabPageExFees.BackColor = Color.Transparent;
      this.tabPageExFees.Location = new Point(4, 23);
      this.tabPageExFees.Name = "tabPageExFees";
      this.tabPageExFees.Size = new Size(969, 630);
      this.tabPageExFees.TabIndex = 0;
      this.tabPageExFees.Text = "Fees";
      this.tabPageExLOComp.BackColor = Color.Transparent;
      this.tabPageExLOComp.Location = new Point(4, 23);
      this.tabPageExLOComp.Name = "tabPageExLOComp";
      this.tabPageExLOComp.Size = new Size(969, 630);
      this.tabPageExLOComp.TabIndex = 0;
      this.tabPageExLOComp.Text = "LO Comp";
      this.tabPageExCommitment.BackColor = Color.Transparent;
      this.tabPageExCommitment.Location = new Point(4, 23);
      this.tabPageExCommitment.Name = "tabPageExCommitment";
      this.tabPageExCommitment.Padding = new Padding(5);
      this.tabPageExCommitment.Size = new Size(969, 630);
      this.tabPageExCommitment.TabIndex = 0;
      this.tabPageExCommitment.Text = "Commitments";
      this.tabPageExTradeMgmt.Location = new Point(4, 23);
      this.tabPageExTradeMgmt.Name = "tabPageExTradeMgmt";
      this.tabPageExTradeMgmt.Size = new Size(969, 630);
      this.tabPageExTradeMgmt.TabIndex = 3;
      this.tabPageExTradeMgmt.Text = "Trade Mgmt";
      this.tabPageExTradeMgmt.UseVisualStyleBackColor = true;
      this.tabPageExOnrp.Location = new Point(4, 23);
      this.tabPageExOnrp.Name = "tabPageExOnrp";
      this.tabPageExOnrp.Size = new Size(969, 630);
      this.tabPageExOnrp.TabIndex = 2;
      this.tabPageExOnrp.Text = "ONRP";
      this.tabPageExOnrp.UseVisualStyleBackColor = true;
      this.tabPageExNotes.BackColor = Color.Transparent;
      this.tabPageExNotes.Location = new Point(4, 23);
      this.tabPageExNotes.Name = "tabPageExNotes";
      this.tabPageExNotes.Size = new Size(969, 630);
      this.tabPageExNotes.TabIndex = 0;
      this.tabPageExNotes.Text = "Notes";
      this.tabPageExTPOWeb.BackColor = Color.Transparent;
      this.tabPageExTPOWeb.Location = new Point(4, 23);
      this.tabPageExTPOWeb.Name = "tabPageExTPOWeb";
      this.tabPageExTPOWeb.Size = new Size(969, 630);
      this.tabPageExTPOWeb.TabIndex = 0;
      this.tabPageExTPOWeb.Text = "TPO Connect Setup";
      this.tabPageExWebCenterDocs.BackColor = Color.Transparent;
      this.tabPageExWebCenterDocs.Location = new Point(4, 23);
      this.tabPageExWebCenterDocs.Name = "tabPageExWebCenterDocs";
      this.tabPageExWebCenterDocs.Size = new Size(969, 630);
      this.tabPageExWebCenterDocs.TabIndex = 0;
      this.tabPageExWebCenterDocs.Text = "TPO Docs";
      this.tabPageExAttachments.BackColor = Color.Transparent;
      this.tabPageExAttachments.Location = new Point(4, 23);
      this.tabPageExAttachments.Name = "tabPageExAttachments";
      this.tabPageExAttachments.Size = new Size(969, 630);
      this.tabPageExAttachments.TabIndex = 0;
      this.tabPageExAttachments.Text = "Attachments";
      this.tabPageExSales.BackColor = Color.Transparent;
      this.tabPageExSales.Location = new Point(4, 23);
      this.tabPageExSales.Name = "tabPageExSales";
      this.tabPageExSales.Size = new Size(969, 630);
      this.tabPageExSales.TabIndex = 0;
      this.tabPageExSales.Text = "Sales Reps / AE";
      this.tabPageExLenderContacts.BackColor = Color.Transparent;
      this.tabPageExLenderContacts.Location = new Point(4, 23);
      this.tabPageExLenderContacts.Name = "tabPageExLenderContacts";
      this.tabPageExLenderContacts.Size = new Size(969, 630);
      this.tabPageExLenderContacts.TabIndex = 0;
      this.tabPageExLenderContacts.Text = "Lender Contacts";
      this.tabPageExCustomTabs.BackColor = Color.Transparent;
      this.tabPageExCustomTabs.Location = new Point(4, 23);
      this.tabPageExCustomTabs.Name = "tabPageExCustomTabs";
      this.tabPageExCustomTabs.Size = new Size(969, 630);
      this.tabPageExCustomTabs.TabIndex = 0;
      this.tabPageExCustomTabs.Text = "Custom Fields";
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "alert-16x16.png");
      this.emHelpLink2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink2.BackColor = Color.Transparent;
      this.emHelpLink2.Cursor = Cursors.Hand;
      this.emHelpLink2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink2.HelpTag = "Setup\\Company Details";
      this.emHelpLink2.Location = new Point(13, 691);
      this.emHelpLink2.Name = "emHelpLink2";
      this.emHelpLink2.Size = new Size(90, 17);
      this.emHelpLink2.TabIndex = 24;
      this.emHelpLink2.Tag = (object) "";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(996, 720);
      this.Controls.Add((Control) this.emHelpLink2);
      this.Controls.Add((Control) this.labelLastUpdate);
      this.Controls.Add((Control) this.tabControlExAll);
      this.Controls.Add((Control) this.btnOK);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(917, 758);
      this.Name = nameof (EditCompanyDetailsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Company Details";
      this.FormClosing += new FormClosingEventHandler(this.EditCompanyDetailsDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.tabControlExAll.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
