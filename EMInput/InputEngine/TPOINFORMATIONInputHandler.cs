// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TPOINFORMATIONInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TPOINFORMATIONInputHandler : InputHandlerBase
  {
    private DropdownBox siteField;
    private EllieMae.Encompass.Forms.TextBox companyField;
    private EllieMae.Encompass.Forms.TextBox branchField;
    private DropdownBox loField;
    private DropdownBox lpField;
    private ExternalOriginatorManagementData selectedCompany;
    private ExternalOriginatorManagementData selectedBranch;
    private List<ExternalOriginatorManagementData> tpoBranches;
    private List<UserInfo> primarySaleReps;
    private List<ExternalUserInfo> managersInfo;
    private ExternalUserInfo[] tpoUsers;
    private List<ExternalOrgURL> tpoURLs;
    private List<ExternalSettingValue> tpoRatings;
    private ExternalLateFeeSettings lateFeeSettings;
    private bool enabledViewSetting;

    public TPOINFORMATIONInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public TPOINFORMATIONInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public TPOINFORMATIONInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public TPOINFORMATIONInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.enabledViewSetting = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOCompanyInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOLicenseInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOLoanTypeInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOTPOContactsInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOLOCompInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPONotesInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPODocsInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOWebCenterSetupInformationn) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOAttachmentsInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOSalesRepsInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOCustomFieldsInformation) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOCommitmentInformation) || aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Commitment) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOFeesInformation) || aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOFeesTab) || aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPODBAInformation) || aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DBATab);
      try
      {
        if (this.siteField == null)
          this.siteField = (DropdownBox) this.currentForm.FindControl("dropdownEMSiteID");
        if (this.companyField == null)
          this.companyField = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("textboxCompanyDBAName");
        if (this.branchField == null)
          this.branchField = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("textboxBranchDBAName");
        if (this.loField == null)
          this.loField = (DropdownBox) this.currentForm.FindControl("dropdownLOID");
        if (this.lpField == null)
          this.lpField = (DropdownBox) this.currentForm.FindControl("dropdownLPID");
      }
      catch (Exception ex)
      {
      }
      try
      {
        if (this.companyField != null)
          this.fetchTPOSettings();
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.setDropdownFields(true, true, true);
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (string.Compare(this.GetField("2626"), "Correspondent", true) != 0 && string.Compare(this.GetField("2626"), "Banked - Wholesale", true) != 0)
      {
        controlState = !(id == "TPO.X88") ? ControlState.Disabled : ControlState.Default;
        if (id == "TPO.X3")
        {
          this.SetControlState("Calendar1", false);
          this.SetControlState("Calendar2", false);
          this.SetControlState("Calendar3", false);
          this.SetControlState("Calendar4", false);
          this.SetControlState("Calendar5", false);
          this.SetControlState("Calendar6", false);
          this.SetControlState("Calendar7", false);
          this.SetControlState("Calendar8", false);
          this.SetControlState("Calendar9", false);
        }
      }
      else
      {
        switch (id)
        {
          case "cleartpoorganization":
            if (this.GetField("TPO.X14") == string.Empty && this.GetField("TPO.X15") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          case "lookuptpodbaname":
            if (this.GetField("TPO.X14") == string.Empty && this.GetField("TPO.X15") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          case "viewtpocompany":
            if (!this.enabledViewSetting || this.GetField("TPO.X14") == string.Empty && this.GetField("TPO.X15") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          case "cleartpobranch":
            if (this.GetField("TPO.X38") == string.Empty && this.GetField("TPO.X39") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          case "lookuptpobranchdbaname":
            if (this.GetField("TPO.X38") == string.Empty && this.GetField("TPO.X39") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          case "viewtpobranch":
            if (!this.enabledViewSetting || this.GetField("TPO.X38") == string.Empty && this.GetField("TPO.X39") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          default:
            controlState = ControlState.Default;
            break;
        }
      }
      if (id == "CORRESPONDENT.X55")
        controlState = ControlState.Enabled;
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      switch (id)
      {
        case "TPO.X1":
          flag3 = val != this.inputData.GetField("TPO.X1");
          break;
        case "TPO.X15":
          flag1 = val != this.inputData.GetField("TPO.X15");
          break;
        case "TPO.X39":
          flag2 = val != this.inputData.GetField("TPO.X39");
          break;
        case "TPO.X62":
          flag4 = val != this.inputData.GetField("TPO.X62");
          break;
        case "TPO.X75":
          flag5 = val != this.inputData.GetField("TPO.X75");
          break;
      }
      base.UpdateFieldValue(id, val);
      switch (id)
      {
        case "TPO.X15":
          if (!flag1)
            break;
          this.fetchTPOSettings();
          this.setDropdownFields(true, true, true);
          this.populateTPOInformation(true, true, true, true);
          this.populateSiteID();
          break;
        case "TPO.X39":
          if (!flag2)
            break;
          this.fetchTPOSettings();
          this.setDropdownFields(true, true, true);
          this.populateBranchInfo();
          this.populateSiteID();
          this.populateTPOInformation(false, false, true, true);
          break;
        case "TPO.X1":
          if (!flag3)
            break;
          this.fetchTPOSettings();
          this.setDropdownFields(false, true, true);
          this.populateTPOInformation(false, false, true, true);
          break;
        case "TPO.X62":
          if (!flag4)
            break;
          this.populateLoanOfficer();
          break;
        default:
          if (!(id == "TPO.X75") || !flag5)
            break;
          this.populateLoanProcessor();
          break;
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "cleartpoorganization":
          this.UpdateFieldValue("TPO.X14", "");
          this.UpdateFieldValue("TPO.X15", "");
          this.SetFieldFocus("dropdownEMSiteID");
          break;
        case "lookuptpoorganization":
          this.SetFieldFocus("dropdownEMSiteID");
          break;
        case "cleartpobranch":
          this.UpdateFieldValue("TPO.X38", "");
          this.UpdateFieldValue("TPO.X39", "");
          this.SetFieldFocus("dropdownEMSiteID");
          break;
        case "lookuptpobranch":
          this.SetFieldFocus("dropdownEMSiteID");
          break;
        case "viewtpocompany":
        case "viewtpobranch":
          this.SetFieldFocus("textboxImportSource");
          break;
      }
    }

    private void fetchTPOSettings()
    {
      string field1 = this.GetField("TPO.X14");
      string field2 = this.GetField("TPO.X15");
      string field3 = this.GetField("TPO.X38");
      string field4 = this.GetField("TPO.X39");
      string siteID = this.GetField("TPO.X1");
      List<object> objectList = (List<object>) null;
      if (this.selectedCompany != null)
        this.selectedCompany = (ExternalOriginatorManagementData) null;
      if (this.managersInfo != null)
      {
        this.managersInfo.Clear();
        this.managersInfo = (List<ExternalUserInfo>) null;
      }
      if (this.primarySaleReps != null)
      {
        this.primarySaleReps.Clear();
        this.primarySaleReps = (List<UserInfo>) null;
      }
      if (this.selectedBranch != null)
        this.selectedBranch = (ExternalOriginatorManagementData) null;
      if (this.tpoBranches != null)
      {
        this.tpoBranches.Clear();
        this.tpoBranches = (List<ExternalOriginatorManagementData>) null;
      }
      if (this.tpoUsers != null)
        this.tpoUsers = (ExternalUserInfo[]) null;
      if (this.tpoURLs != null)
        this.tpoURLs = (List<ExternalOrgURL>) null;
      try
      {
        if (!string.IsNullOrEmpty(field2) || !string.IsNullOrEmpty(field1))
          objectList = this.session.ConfigurationManager.GetTPOInformationToolSettings(field2, field1, field4, field3, siteID, this.session.UserID);
        if (objectList != null)
        {
          for (int index = 0; index < objectList.Count; ++index)
          {
            if (objectList[index] is ExternalOriginatorManagementData)
              this.selectedCompany = (ExternalOriginatorManagementData) objectList[index];
            else if (objectList[index] is List<ExternalOriginatorManagementData>)
              this.tpoBranches = (List<ExternalOriginatorManagementData>) objectList[index];
            else if (objectList[index] is ExternalUserInfo[])
              this.tpoUsers = (ExternalUserInfo[]) objectList[index];
            else if (objectList[index] is List<UserInfo>)
              this.primarySaleReps = (List<UserInfo>) objectList[index];
            else if (objectList[index] is List<ExternalOrgURL>)
              this.tpoURLs = (List<ExternalOrgURL>) objectList[index];
            else if (objectList[index] is ExternalLateFeeSettings)
              this.lateFeeSettings = (ExternalLateFeeSettings) objectList[index];
            else if (objectList[index] is ExternalUserInfo)
            {
              if (this.managersInfo == null)
                this.managersInfo = new List<ExternalUserInfo>();
              this.managersInfo.Add((ExternalUserInfo) objectList[index]);
            }
            else if (objectList[index] is ExternalOriginatorManagementData[])
              this.selectedBranch = ((ExternalOriginatorManagementData[]) objectList[index])[0];
            else if (objectList[index] is List<ExternalSettingValue>)
              this.tpoRatings = (List<ExternalSettingValue>) objectList[index];
          }
        }
        this.SetControlState("btnLookUpBranch", this.tpoBranches != null && this.tpoBranches.Count > 0);
        if (this.tpoBranches == null || this.tpoBranches.Count == 0)
        {
          this.SetField("TPO.X38", "");
          this.SetField("TPO.X39", "");
          this.SetField("TPO.X40", "");
          this.SetField("TPO.X41", "");
          this.SetField("TPO.X42", "");
          this.SetField("TPO.X43", "");
          this.SetField("TPO.X44", "");
          this.SetField("TPO.X45", "");
          this.SetField("TPO.X46", "");
          this.SetField("TPO.X47", "");
          this.SetField("TPO.X48", "");
          this.SetField("TPO.X51", "");
          this.SetField("TPO.X52", "");
          this.SetField("TPO.X53", "");
          this.SetField("TPO.X54", "");
          this.SetField("TPO.X55", "");
        }
        this.SetControlState("btnDeleteBranch", this.tpoBranches != null && this.tpoBranches.Count > 0);
        if (!string.IsNullOrEmpty(siteID) && (this.tpoURLs == null || this.tpoURLs.Count == 0 || this.tpoURLs.FindIndex((Predicate<ExternalOrgURL>) (x => x.siteId == siteID)) == -1))
          this.SetField("TPO.X1", "");
        this.TPOBranches = this.tpoBranches;
      }
      catch (Exception ex)
      {
        this.tpoBranches = (List<ExternalOriginatorManagementData>) null;
        this.tpoUsers = (ExternalUserInfo[]) null;
        this.tpoURLs = (List<ExternalOrgURL>) null;
      }
    }

    private void populateTPOInformation(bool setCompany, bool setBranch, bool setLO, bool setLP)
    {
      if (setCompany)
        this.populateCompanyInfo();
      if (setBranch)
        this.populateBranchInfo();
      if (setLO)
        this.populateLoanOfficer();
      if (!setLP)
        return;
      this.populateLoanProcessor();
    }

    private void populateCompanyInfo()
    {
      if (this.selectedCompany != null)
      {
        ExternalOrgDBAName defaultDbaName = this.session.ConfigurationManager.GetDefaultDBAName(this.selectedCompany.oid);
        this.SetField("TPO.X14", this.selectedCompany.OrganizationName);
        this.SetField("TPO.X16", this.selectedCompany.OrgID);
        this.SetField("TPO.X17", this.selectedCompany.CompanyLegalName);
        this.SetField("TPO.X24", defaultDbaName != null ? defaultDbaName.Name : "");
        this.SetField("TPO.X18", this.selectedCompany.Address);
        this.SetField("TPO.X19", this.selectedCompany.City);
        this.SetField("TPO.X20", this.selectedCompany.State);
        this.SetField("TPO.X21", this.selectedCompany.Zip);
        this.SetField("TPO.X22", this.selectedCompany.PhoneNumber);
        this.SetField("TPO.X23", this.selectedCompany.FaxNumber);
        this.SetField("TPO.X27", this.translateRating(this.selectedCompany.CompanyRating));
        this.SetField("TPO.X28", "");
        this.SetField("TPO.X29", "");
        if (this.managersInfo != null && this.managersInfo.Count > 0)
        {
          foreach (ExternalUserInfo externalUserInfo in this.managersInfo)
          {
            if (externalUserInfo.ExternalUserID == this.selectedCompany.Manager)
            {
              this.SetField("TPO.X28", externalUserInfo.FirstName + " " + externalUserInfo.LastName);
              this.SetField("TPO.X29", externalUserInfo.Email);
              break;
            }
          }
        }
        this.SetField("TPO.X30", "");
        this.SetField("TPO.X31", "");
        if (this.primarySaleReps != null && this.primarySaleReps.Count > 0)
        {
          foreach (UserInfo primarySaleRep in this.primarySaleReps)
          {
            if (primarySaleRep.Userid == this.selectedCompany.PrimarySalesRepUserId)
            {
              this.SetField("TPO.X30", primarySaleRep.FullName);
              this.SetField("TPO.X31", primarySaleRep.Userid);
              break;
            }
          }
        }
      }
      else
      {
        this.SetField("TPO.X14", "");
        this.SetField("TPO.X15", "");
        this.SetField("TPO.X16", "");
        this.SetField("TPO.X17", "");
        this.SetField("TPO.X18", "");
        this.SetField("TPO.X19", "");
        this.SetField("TPO.X20", "");
        this.SetField("TPO.X21", "");
        this.SetField("TPO.X22", "");
        this.SetField("TPO.X23", "");
        this.SetField("TPO.X24", "");
        this.SetField("TPO.X27", "");
        this.SetField("TPO.X28", "");
        this.SetField("TPO.X29", "");
        this.SetField("TPO.X30", "");
        this.SetField("TPO.X31", "");
        this.lateFeeSettings = this.session.ConfigurationManager.GetGlobalLateFeeSettings();
      }
      this.populateLateFeeSetting();
    }

    private void populateBranchInfo()
    {
      if (this.selectedBranch != null)
      {
        ExternalOrgDBAName defaultDbaName = this.session.ConfigurationManager.GetDefaultDBAName(this.selectedBranch.oid);
        this.SetField("TPO.X38", this.selectedBranch.OrganizationName);
        this.SetField("TPO.X40", this.selectedBranch.OrgID);
        this.SetField("TPO.X41", this.selectedBranch.CompanyLegalName);
        this.SetField("TPO.X42", this.selectedBranch.Address);
        this.SetField("TPO.X43", this.selectedBranch.City);
        this.SetField("TPO.X44", this.selectedBranch.State);
        this.SetField("TPO.X45", this.selectedBranch.Zip);
        this.SetField("TPO.X46", this.selectedBranch.PhoneNumber);
        this.SetField("TPO.X47", this.selectedBranch.FaxNumber);
        this.SetField("TPO.X48", defaultDbaName != null ? defaultDbaName.Name : "");
        this.SetField("TPO.X51", this.translateRating(this.selectedBranch.CompanyRating));
        this.SetField("TPO.X52", "");
        this.SetField("TPO.X53", "");
        if (this.managersInfo != null && this.managersInfo.Count > 0)
        {
          foreach (ExternalUserInfo externalUserInfo in this.managersInfo)
          {
            if (externalUserInfo.ExternalUserID == this.selectedBranch.Manager)
            {
              this.SetField("TPO.X52", externalUserInfo.FirstName + " " + externalUserInfo.LastName);
              this.SetField("TPO.X53", externalUserInfo.Email);
              break;
            }
          }
        }
        this.SetField("TPO.X54", "");
        this.SetField("TPO.X55", "");
        if (this.primarySaleReps == null || this.primarySaleReps.Count <= 0)
          return;
        foreach (UserInfo primarySaleRep in this.primarySaleReps)
        {
          if (primarySaleRep.Userid == this.selectedBranch.PrimarySalesRepUserId)
          {
            this.SetField("TPO.X54", primarySaleRep.FullName);
            this.SetField("TPO.X55", primarySaleRep.Userid);
            break;
          }
        }
      }
      else
      {
        this.SetField("TPO.X38", "");
        this.SetField("TPO.X39", "");
        this.SetField("TPO.X40", "");
        this.SetField("TPO.X41", "");
        this.SetField("TPO.X42", "");
        this.SetField("TPO.X43", "");
        this.SetField("TPO.X44", "");
        this.SetField("TPO.X45", "");
        this.SetField("TPO.X46", "");
        this.SetField("TPO.X47", "");
        this.SetField("TPO.X48", "");
        this.SetField("TPO.X51", "");
        this.SetField("TPO.X52", "");
        this.SetField("TPO.X53", "");
        this.SetField("TPO.X54", "");
        this.SetField("TPO.X55", "");
      }
    }

    private string translateRating(int ratingID)
    {
      if (this.tpoRatings == null || this.tpoRatings.Count == 0 || ratingID == -1)
        return string.Empty;
      ExternalSettingValue externalSettingValue = this.tpoRatings.Find((Predicate<ExternalSettingValue>) (x => x.settingId == ratingID));
      return externalSettingValue == null ? string.Empty : externalSettingValue.settingValue;
    }

    private void populateSiteID()
    {
      if (this.siteField == null)
        return;
      if (this.siteField.Options.Count == 1)
      {
        base.UpdateFieldValue("TPO.X1", "");
      }
      else
      {
        string field = this.GetField("TPO.X1");
        if (!(field != string.Empty))
          return;
        for (int index = 0; index < this.siteField.Options.Count; ++index)
        {
          if (string.Compare(this.siteField.Options[index].Value, field, true) == 0)
          {
            this.siteField.SelectedIndex = index;
            return;
          }
        }
        base.UpdateFieldValue("TPO.X1", "");
      }
    }

    private void populateLoanOfficer()
    {
      ExternalUserInfo externalUserInfo = (ExternalUserInfo) null;
      this.GetField("TPO.X39");
      string field = this.GetField("TPO.X62");
      if (this.selectedCompany != null && field != string.Empty && this.tpoUsers != null && this.tpoUsers.Length != 0)
      {
        foreach (ExternalUserInfo tpoUser in this.tpoUsers)
        {
          if (tpoUser.ContactID == field)
          {
            externalUserInfo = tpoUser;
            break;
          }
        }
      }
      if ((UserInfo) externalUserInfo != (UserInfo) null)
      {
        this.SetField("TPO.X61", externalUserInfo.FirstName + " " + (externalUserInfo.MiddleName != string.Empty ? externalUserInfo.MiddleName + " " : "") + externalUserInfo.LastName + (externalUserInfo.LastName != string.Empty ? " " + externalUserInfo.Suffix : ""));
        this.SetField("TPO.X63", externalUserInfo.Email);
        this.SetField("TPO.X64", externalUserInfo.DisabledLogin ? "Disabled" : "Enabled");
        this.SetField("TPO.X65", externalUserInfo.Phone);
        this.SetField("TPO.X66", externalUserInfo.Fax);
        this.SetField("TPO.X67", externalUserInfo.CellPhone);
        this.SetField("TPO.X68", externalUserInfo.Address);
        this.SetField("TPO.X69", externalUserInfo.City);
        this.SetField("TPO.X70", externalUserInfo.State);
        this.SetField("TPO.X71", externalUserInfo.Zipcode);
        this.SetField("TPO.X72", externalUserInfo.Notes);
        this.SetField("TPO.X56", "");
        this.SetField("TPO.X57", "");
        if (this.primarySaleReps != null && this.primarySaleReps.Count > 0)
        {
          foreach (UserInfo primarySaleRep in this.primarySaleReps)
          {
            if (primarySaleRep.Userid == externalUserInfo.SalesRepID)
            {
              this.SetField("TPO.X56", primarySaleRep.FullName);
              this.SetField("TPO.X57", primarySaleRep.Userid);
              break;
            }
          }
        }
        for (int index = 0; index < this.loField.Options.Count; ++index)
        {
          if (string.Compare(this.loField.Options[index].Value, externalUserInfo.ContactID, true) == 0)
          {
            this.loField.SelectedIndex = index;
            break;
          }
        }
      }
      else
      {
        for (int index = 61; index <= 72; ++index)
          this.SetField("TPO.X" + (object) index, "");
        this.SetField("TPO.X56", "");
        this.SetField("TPO.X57", "");
        if (this.loField == null || this.loField.Options == null || this.loField.Options.Count <= 0)
          return;
        this.loField.SelectedIndex = 0;
      }
    }

    private void populateLoanProcessor()
    {
      ExternalUserInfo externalUserInfo = (ExternalUserInfo) null;
      this.GetField("TPO.X39");
      string field = this.GetField("TPO.X75");
      if (this.selectedCompany != null && field != string.Empty && this.tpoUsers != null && this.tpoUsers.Length != 0)
      {
        foreach (ExternalUserInfo tpoUser in this.tpoUsers)
        {
          if (tpoUser.ContactID == field)
          {
            externalUserInfo = tpoUser;
            break;
          }
        }
      }
      if ((UserInfo) externalUserInfo != (UserInfo) null)
      {
        this.SetField("TPO.X74", externalUserInfo.FirstName + " " + (externalUserInfo.MiddleName != string.Empty ? externalUserInfo.MiddleName + " " : "") + externalUserInfo.LastName + (externalUserInfo.LastName != string.Empty ? " " + externalUserInfo.Suffix : ""));
        this.SetField("TPO.X76", externalUserInfo.Email);
        this.SetField("TPO.X77", externalUserInfo.DisabledLogin ? "Disabled" : "Enabled");
        this.SetField("TPO.X78", externalUserInfo.Phone);
        this.SetField("TPO.X79", externalUserInfo.Fax);
        this.SetField("TPO.X80", externalUserInfo.CellPhone);
        this.SetField("TPO.X81", externalUserInfo.Address);
        this.SetField("TPO.X82", externalUserInfo.City);
        this.SetField("TPO.X83", externalUserInfo.State);
        this.SetField("TPO.X84", externalUserInfo.Zipcode);
        this.SetField("TPO.X85", externalUserInfo.Notes);
        this.SetField("TPO.X58", "");
        this.SetField("TPO.X59", "");
        if (this.primarySaleReps != null && this.primarySaleReps.Count > 0)
        {
          foreach (UserInfo primarySaleRep in this.primarySaleReps)
          {
            if (primarySaleRep.Userid == externalUserInfo.SalesRepID)
            {
              this.SetField("TPO.X58", primarySaleRep.FullName);
              this.SetField("TPO.X59", primarySaleRep.Userid);
              break;
            }
          }
        }
        for (int index = 0; index < this.lpField.Options.Count; ++index)
        {
          if (string.Compare(this.lpField.Options[index].Value, externalUserInfo.ContactID, true) == 0)
          {
            this.lpField.SelectedIndex = index;
            break;
          }
        }
      }
      else
      {
        for (int index = 74; index <= 85; ++index)
          this.SetField("TPO.X" + (object) index, "");
        this.SetField("TPO.X58", "");
        this.SetField("TPO.X59", "");
        if (this.lpField == null || this.lpField.Options == null || this.lpField.Options.Count <= 0)
          return;
        this.lpField.SelectedIndex = 0;
      }
    }

    private void populateLateFeeSetting()
    {
      if (this.lateFeeSettings == null)
      {
        this.setLateFeeSettingField("3927", "");
        this.setLateFeeSettingField("3931", "");
        this.setLateFeeSettingField("3932", "");
        this.setLateFeeSettingField("3933", "");
        this.setLateFeeSettingField("3936", "");
      }
      else
      {
        this.setLateFeeSettingField("3927", this.lateFeeSettings.GracePeriodDays >= 0 ? string.Concat((object) this.lateFeeSettings.GracePeriodDays) : "");
        this.setLateFeeSettingField("3931", this.lateFeeSettings.LateFee > 0.0 ? string.Concat((object) this.lateFeeSettings.LateFee) : "");
        this.setLateFeeSettingField("3932", this.lateFeeSettings.Amount > 0.0 ? string.Concat((object) this.lateFeeSettings.Amount) : "");
        this.setLateFeeSettingField("3933", this.lateFeeSettings.CalculateAs == 1 ? "Daily Fee" : "Flat");
        this.setLateFeeSettingField("3936", this.lateFeeSettings.LateFeeBasedOn == 1 ? "Base Loan Amount" : "Total Loan Amount");
      }
      if (this.loan == null || this.loan.Calculator == null)
        return;
      this.loan.Calculator.FormCalculation("UPDATECORRESPONDENTLOANSTATUS", "", "");
    }

    private void setLateFeeSettingField(string id, string val)
    {
      if (this.inputData.IsLocked(id))
      {
        string field = this.inputData.GetField(id);
        this.inputData.RemoveLock(id);
        this.inputData.SetField(id, val);
        this.inputData.AddLock(id);
        this.inputData.SetField(id, field);
      }
      else
        this.inputData.SetField(id, val);
    }

    private void setDropdownFields(bool setSite, bool setLO, bool setLP)
    {
      if (setSite)
        this.setSiteField();
      if (setLO)
        this.setLOField();
      if (!setLP)
        return;
      this.setLPField();
    }

    private void setSiteField()
    {
      if (this.siteField == null)
        return;
      this.siteField.Options.Clear();
      this.siteField.Enabled = false;
      if (this.GetField("TPO.X15") == string.Empty && this.GetField("TPO.X39") == string.Empty)
        return;
      List<DropdownOption> optionList = new List<DropdownOption>();
      optionList.Add(new DropdownOption("", ""));
      if (this.tpoURLs != null && this.tpoURLs.Count > 0)
      {
        foreach (ExternalOrgURL tpoUrL in this.tpoURLs)
          optionList.Add(new DropdownOption(tpoUrL.URL, tpoUrL.siteId));
      }
      this.siteField.Options.AddRange((ICollection) optionList);
      this.siteField.Enabled = this.siteField.Options.Count > 1;
    }

    private void setBranchField()
    {
      if (this.branchField == null)
        return;
      if (this.GetField("TPO.X15") == string.Empty || this.tpoBranches == null || this.tpoBranches.Count == 0)
      {
        this.SetControlState("btnLookUpBranch", false);
        this.SetControlState("btnDeleteBranch", false);
      }
      else
      {
        this.SetControlState("btnLookUpBranch", true);
        this.SetControlState("btnDeleteBranch", true);
      }
    }

    private void setLOField()
    {
      if (this.loField == null)
        return;
      this.loField.Options.Clear();
      this.loField.Enabled = false;
      if (this.GetField("TPO.X15") == string.Empty)
        return;
      bool flag = this.session.ConfigurationManager.CheckIfNewTPOSiteExists(this.GetField("TPO.X1"));
      List<DropdownOption> optionList = new List<DropdownOption>();
      optionList.Add(new DropdownOption("", ""));
      if (this.tpoUsers != null && this.tpoUsers.Length != 0)
      {
        WorkflowManager workflowManager = (WorkflowManager) null;
        if (flag)
          workflowManager = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
        foreach (ExternalUserInfo tpoUser in this.tpoUsers)
        {
          if (!flag)
          {
            if ((tpoUser.Roles & 1) == 1)
              optionList.Add(new DropdownOption(tpoUser.FirstName + " " + tpoUser.LastName + " (" + tpoUser.ContactID + ")", tpoUser.ContactID));
          }
          else if (workflowManager.GetUsersRoleMapping(tpoUser.ContactID, RealWorldRoleID.TPOLoanOfficer) != null)
            optionList.Add(new DropdownOption(tpoUser.FirstName + " " + tpoUser.LastName + " (" + tpoUser.ContactID + ")", tpoUser.ContactID));
        }
      }
      this.loField.Options.AddRange((ICollection) optionList);
      this.loField.Enabled = this.loField.Options.Count > 1;
    }

    private void setLPField()
    {
      if (this.lpField == null)
        return;
      this.lpField.Options.Clear();
      this.lpField.Enabled = false;
      if (this.GetField("TPO.X15") == string.Empty)
        return;
      bool flag = this.session.ConfigurationManager.CheckIfNewTPOSiteExists(this.GetField("TPO.X1"));
      List<DropdownOption> optionList = new List<DropdownOption>();
      optionList.Add(new DropdownOption("", ""));
      if (this.tpoUsers != null && this.tpoUsers.Length != 0)
      {
        WorkflowManager workflowManager = (WorkflowManager) null;
        if (flag)
          workflowManager = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
        foreach (ExternalUserInfo tpoUser in this.tpoUsers)
        {
          if (!flag)
          {
            if ((tpoUser.Roles & 2) == 2)
              optionList.Add(new DropdownOption(tpoUser.FirstName + " " + tpoUser.LastName + " (" + tpoUser.ContactID + ")", tpoUser.ContactID));
          }
          else if (workflowManager.GetUsersRoleMapping(tpoUser.ContactID, RealWorldRoleID.TPOLoanProcessor) != null)
            optionList.Add(new DropdownOption(tpoUser.FirstName + " " + tpoUser.LastName + " (" + tpoUser.ContactID + ")", tpoUser.ContactID));
        }
      }
      this.lpField.Options.AddRange((ICollection) optionList);
      this.lpField.Enabled = this.lpField.Options.Count > 1;
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      ExternalLateFeeSettings orgLateFeeSettings = this.session.ConfigurationManager.GetExternalOrgLateFeeSettings(this.inputData.GetField("TPO.X15"), true);
      if (orgLateFeeSettings.DayCleared == 2)
        this.inputData.SetField("4112", "Cleared for Purchase Date");
      if (orgLateFeeSettings.DayCleared == 4)
        this.inputData.SetField("4112", LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.AllDatabaseFields).GetFieldByCriterionName(orgLateFeeSettings.DayClearedOtherDate).Description);
      if (orgLateFeeSettings.DayCleared == 1)
        this.inputData.SetField("4112", "Purchase Approval Date");
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
    }
  }
}
