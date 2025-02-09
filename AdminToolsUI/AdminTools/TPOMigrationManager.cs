// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.TPOMigrationManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  internal class TPOMigrationManager
  {
    private static Dictionary<string, List<ExternalSettingValue>> allSettings = Session.ConfigurationManager.GetExternalOrgSettings();
    private static ContactCustomFieldInfoCollection customFields = Session.ConfigurationManager.GetCustomFieldInfo();
    private static Dictionary<string, string> licenseTypes = new Dictionary<string, string>();

    internal static string GetMigrationFolder()
    {
      string path = Path.Combine(TPOMigrationManager.GetBackupFolder(), "TPOMigration_" + (object) DateTime.Now.Ticks);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    internal static void BackupDatabase(string filePath)
    {
      string xml = Session.ConfigurationManager.BackupTPOData();
      if (!(xml != string.Empty))
        return;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      xmlDocument.Save(filePath);
    }

    internal static string GetTPOWebUrl(string clientid)
    {
      string str = FormsAuthentication.HashPasswordForStoringInConfigFile(clientid + clientid, "sha1");
      return string.Format("https://www.encompasswebcenter.com/TPOExportOriginatorManagement/TPOExport.svc/TPODataForClientID?clientid={0}&key={1}", (object) clientid, (object) str);
    }

    internal static string GetBackupFilePath(string transactionFolder)
    {
      return Path.Combine(transactionFolder, "Backup.xml");
    }

    internal static string GetTPOWebCenterFilePath(string transactionFolder)
    {
      return Path.Combine(transactionFolder, "TPOWebSource.xml");
    }

    internal static string GetResultFilePath(string transactionFolder)
    {
      return Path.Combine(transactionFolder, "Result.xml");
    }

    internal static bool RestoreTPOData(string restoreData)
    {
      return Session.ConfigurationManager.RestoreTPOData(restoreData);
    }

    internal static string[] GetTransactionFolderList()
    {
      return Directory.GetDirectories(TPOMigrationManager.GetBackupFolder());
    }

    internal static Dictionary<string, DataSet> GetTransactionHistory(string transactionFolder)
    {
      Dictionary<string, DataSet> transactionHistory = new Dictionary<string, DataSet>();
      if (File.Exists(TPOMigrationManager.GetBackupFilePath(transactionFolder)))
      {
        DataSet dataSet = new DataSet();
        int num = (int) dataSet.ReadXml(TPOMigrationManager.GetBackupFilePath(transactionFolder));
        transactionHistory.Add("original", dataSet);
      }
      if (File.Exists(TPOMigrationManager.GetResultFilePath(transactionFolder)))
      {
        DataSet dataSet = new DataSet();
        int num = (int) dataSet.ReadXml(TPOMigrationManager.GetResultFilePath(transactionFolder));
        transactionHistory.Add("result", dataSet);
      }
      return transactionHistory;
    }

    internal static string GetBackupFolder()
    {
      string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TPOMigration"), Session.StartupInfo.CompanyInfo.ClientID);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    internal static void ParseXml(XmlDocument xmlDoc)
    {
      if (!TPOMigrationManager.allSettings.ContainsKey("Current Company Status"))
        TPOMigrationManager.allSettings.Add("Current Company Status", new List<ExternalSettingValue>());
      if (!TPOMigrationManager.allSettings.ContainsKey("Company Rating"))
        TPOMigrationManager.allSettings.Add("Company Rating", new List<ExternalSettingValue>());
      if (!TPOMigrationManager.allSettings.ContainsKey("Price Group"))
        TPOMigrationManager.allSettings.Add("Price Group", new List<ExternalSettingValue>());
      if (TPOMigrationManager.customFields.Page1Name != "Legacy")
      {
        TPOMigrationManager.customFields.Page1Name = "Legacy";
        ArrayList arrayList = new ArrayList((ICollection) new Collection<ContactCustomFieldInfo>()
        {
          new ContactCustomFieldInfo(1, string.Empty, "LegacyID", FieldFormat.STRING, string.Empty, false, new string[0])
        });
        TPOMigrationManager.customFields.Items = (ContactCustomFieldInfo[]) arrayList.ToArray(typeof (ContactCustomFieldInfo));
        Session.ConfigurationManager.UpdateCustomFieldInfo(TPOMigrationManager.customFields, new ArrayList());
        TPOMigrationManager.customFields = Session.ConfigurationManager.GetCustomFieldInfo();
      }
      foreach (XmlNode xmlNode in xmlDoc.GetElementsByTagName("TPOSite"))
      {
        ExternalOrgURL siteURL = (ExternalOrgURL) null;
        if (xmlNode["TPOSiteID"].InnerText != "" && xmlNode["URL"].InnerText != "")
          siteURL = Session.ConfigurationManager.AddExternalOrganizationURL(xmlNode["TPOSiteID"].InnerText, xmlNode["URL"].InnerText);
        foreach (XmlNode selectNode1 in xmlNode.SelectNodes("TPOCompanyBranchList"))
        {
          foreach (XmlNode selectNode2 in selectNode1.SelectNodes("TPOCompany"))
          {
            if (selectNode2 != null)
              TPOMigrationManager.parseOrg(selectNode2, false, 0, 1, ExternalOriginatorEntityType.None, siteURL);
          }
        }
      }
    }

    private static void parseOrg(
      XmlNode node,
      bool branch,
      int parent,
      int depth,
      ExternalOriginatorEntityType entityType,
      ExternalOrgURL siteURL)
    {
      ExternalOriginatorManagementData originatorManagementData1 = new ExternalOriginatorManagementData();
      originatorManagementData1.Parent = parent;
      originatorManagementData1.Depth = depth;
      if (!branch)
      {
        originatorManagementData1.ExternalID = node["CompanyID"].InnerText;
        originatorManagementData1.OrganizationType = ExternalOriginatorOrgType.Company;
      }
      else
      {
        originatorManagementData1.ExternalID = node["BranchID"].InnerText;
        originatorManagementData1.OrganizationType = ExternalOriginatorOrgType.Branch;
      }
      originatorManagementData1.CompanyDBAName = node["DBAName"].InnerText;
      originatorManagementData1.OrganizationName = node["DBAName"].InnerText;
      originatorManagementData1.CompanyLegalName = node["LegalName"].InnerText;
      originatorManagementData1.Address = node["StreetAddress"].InnerText;
      originatorManagementData1.City = node["City"].InnerText;
      originatorManagementData1.State = node["State"].InnerText;
      originatorManagementData1.Zip = node["Zip"].InnerText;
      if (node["OriginatorLOSClientID"] != null)
        originatorManagementData1.OrgID = node["OriginatorLOSClientID"].InnerText.Length <= 25 ? node["OriginatorLOSClientID"].InnerText : node["OriginatorLOSClientID"].InnerText.Substring(0, 25);
      if (node["Originator"] != null)
      {
        switch (node["Originator"].InnerText)
        {
          case "Wholesale":
            originatorManagementData1.entityType = ExternalOriginatorEntityType.Broker;
            break;
          case "Correspondent":
            originatorManagementData1.entityType = ExternalOriginatorEntityType.Correspondent;
            break;
          case "Both":
            originatorManagementData1.entityType = ExternalOriginatorEntityType.Both;
            break;
          default:
            originatorManagementData1.entityType = ExternalOriginatorEntityType.None;
            break;
        }
      }
      else
        originatorManagementData1.entityType = entityType;
      if (node["CompanyStatus"] != null)
      {
        List<ExternalSettingValue> allSetting = TPOMigrationManager.allSettings["Current Company Status"];
        if (allSetting.Count != 0)
        {
          foreach (ExternalSettingValue externalSettingValue in allSetting)
          {
            if (externalSettingValue.settingValue.Replace(" ", "") == node["CompanyStatus"].InnerText)
            {
              originatorManagementData1.CurrentStatus = externalSettingValue.settingId;
              break;
            }
          }
          if (originatorManagementData1.CurrentStatus == 0)
          {
            int sortId = 0;
            if (allSetting != null && allSetting.Count > 0)
              sortId = allSetting[allSetting.Count - 1].sortId + 1;
            ExternalSettingValue externalSettingValue = new ExternalSettingValue(-1, 1, (string) null, node["CompanyStatus"].InnerText, sortId);
            int num = Session.ConfigurationManager.AddExternalOrgSettingValue(externalSettingValue);
            externalSettingValue.settingId = num;
            TPOMigrationManager.allSettings["Current Company Status"].Add(externalSettingValue);
            originatorManagementData1.CurrentStatus = num;
          }
        }
      }
      if (node["MERSOriginatingOrgID"] != null)
        originatorManagementData1.MERSOriginatingORGID = node["MERSOriginatingOrgID"].InnerText;
      originatorManagementData1.ApplicationDate = node["ApplicationDate"].InnerText != "" ? Convert.ToDateTime(node["ApplicationDate"].InnerText) : DateTime.MinValue;
      originatorManagementData1.ApprovedDate = node["ApprovalDate"].InnerText != "" ? Convert.ToDateTime(node["ApprovalDate"].InnerText) : DateTime.MinValue;
      originatorManagementData1.LastLoanSubmitted = node["LastLoanRegisteredDate"].InnerText != "" ? Convert.ToDateTime(node["LastLoanRegisteredDate"].InnerText) : DateTime.MinValue;
      if (node["Rating"] != null)
      {
        List<ExternalSettingValue> allSetting = TPOMigrationManager.allSettings["Company Rating"];
        if (allSetting.Count != 0)
        {
          foreach (ExternalSettingValue externalSettingValue in allSetting)
          {
            if (externalSettingValue.settingValue.Replace(" ", "") == node["Rating"].InnerText)
            {
              originatorManagementData1.CompanyRating = externalSettingValue.settingId;
              break;
            }
          }
          if (originatorManagementData1.CompanyRating == 0)
          {
            int sortId = 0;
            if (allSetting != null && allSetting.Count > 0)
              sortId = allSetting[allSetting.Count - 1].sortId + 1;
            ExternalSettingValue externalSettingValue = new ExternalSettingValue(-1, 3, (string) null, node["Rating"].InnerText, sortId);
            int num = Session.ConfigurationManager.AddExternalOrgSettingValue(externalSettingValue);
            externalSettingValue.settingId = num;
            TPOMigrationManager.allSettings["Company Rating"].Add(externalSettingValue);
            originatorManagementData1.CompanyRating = num;
          }
        }
      }
      originatorManagementData1.CanCloseInOwnName = node["CanCloseINOwnName"] != null ? (!(node["CanCloseINOwnName"].InnerText == "true") ? (!(node["CanCloseINOwnName"].InnerText == "false") ? 0 : 2) : 1) : 0;
      originatorManagementData1.CanFundInOwnName = node["CanFundInOwnName"] != null ? (!(node["CanFundInOwnName"].InnerText == "true") ? (!(node["CanFundInOwnName"].InnerText == "false") ? 0 : 2) : 1) : 0;
      originatorManagementData1.DUSponsored = node["DUSponsored"] != null ? (!(node["DUSponsored"].InnerText == "true") ? (!(node["DUSponsored"].InnerText == "false") ? 0 : 2) : 1) : 0;
      originatorManagementData1.Website = node["WebsiteAddress"].InnerText;
      if (node["BusinessPhone"] != null)
        originatorManagementData1.PhoneNumber = node["BusinessPhone"].InnerText;
      bool needsUpdate = false;
      if (originatorManagementData1.PhoneNumber != null && originatorManagementData1.PhoneNumber != "")
        originatorManagementData1.PhoneNumber = Utils.FormatInput(originatorManagementData1.PhoneNumber, FieldFormat.PHONE, ref needsUpdate);
      if (node["BusinessFax"] != null)
        originatorManagementData1.FaxNumber = node["BusinessFax"].InnerText;
      if (originatorManagementData1.FaxNumber != null && originatorManagementData1.FaxNumber != "")
        originatorManagementData1.FaxNumber = Utils.FormatInput(originatorManagementData1.FaxNumber, FieldFormat.PHONE, ref needsUpdate);
      originatorManagementData1.DateOfIncorporation = node["DateOfIncorporation"].InnerText != "" ? Convert.ToDateTime(node["DateOfIncorporation"].InnerText) : DateTime.MinValue;
      if (node["TypeOfEntity"] == null)
      {
        originatorManagementData1.TypeOfEntity = 0;
      }
      else
      {
        switch (node["TypeOfEntity"].InnerText)
        {
          case "Individual":
            originatorManagementData1.TypeOfEntity = 1;
            break;
          case "Sole Proprietorship":
            originatorManagementData1.TypeOfEntity = 2;
            break;
          case "Partnership":
            originatorManagementData1.TypeOfEntity = 3;
            break;
          case "Corporation":
            originatorManagementData1.TypeOfEntity = 4;
            break;
          case "Limited Liability Company":
            originatorManagementData1.TypeOfEntity = 5;
            break;
          case "Other (please specify)":
            originatorManagementData1.TypeOfEntity = 6;
            originatorManagementData1.OtherEntityDescription = node["TypeOfEntityOtherDescription"].InnerText;
            break;
          default:
            originatorManagementData1.TypeOfEntity = 0;
            break;
        }
      }
      originatorManagementData1.UseSSNFormat = node["TaxIDorSSNType"].InnerText.Contains("SSN");
      originatorManagementData1.TaxID = node["TaxIDNumberOrSocialSecurityNumber"].InnerText;
      if (node["NMLSID"] != null)
        originatorManagementData1.NmlsId = node["NMLSID"].InnerText.Length <= 12 ? node["NMLSID"].InnerText : node["NMLSID"].InnerText.Substring(0, 12);
      originatorManagementData1.EOCompany = node["EAndOInsuranceProviderName"].InnerText;
      originatorManagementData1.EOPolicyNumber = node["EAndOInsurancePolicyNumber"].InnerText;
      string xpath1 = branch ? "BranchEPPSConfig" : "CompanyEPPSConfig";
      foreach (XmlNode selectNode in node.SelectNodes(xpath1))
      {
        if (selectNode["CompensationModel"] != null)
        {
          switch (selectNode["CompensationModel"].InnerText)
          {
            case "AllowSelect":
              originatorManagementData1.EPPSCompModel = "2";
              break;
            case "AlwaysCreditor":
              originatorManagementData1.EPPSCompModel = "1";
              break;
            case "AlwaysBorrower":
              originatorManagementData1.EPPSCompModel = "0";
              break;
          }
        }
        originatorManagementData1.EPPSUserName = selectNode["PricingUsername"].InnerText;
      }
      string[] strArray = node["PriceGroup"].InnerText.Split('-');
      if (strArray.Length == 2 && strArray[0].Trim() != "" && strArray[1].Trim() != "")
      {
        List<ExternalSettingValue> allSetting = TPOMigrationManager.allSettings["Price Group"];
        if (allSetting.Count != 0)
        {
          foreach (ExternalSettingValue externalSettingValue in allSetting)
          {
            if (externalSettingValue.settingValue == strArray[1].Trim() && externalSettingValue.settingCode == strArray[0].Trim())
            {
              originatorManagementData1.EPPSPriceGroup = externalSettingValue.settingId.ToString();
              break;
            }
          }
          if (originatorManagementData1.EPPSPriceGroup == null || originatorManagementData1.EPPSPriceGroup == "")
          {
            int sortId = 0;
            if (allSetting != null && allSetting.Count > 0)
              sortId = allSetting[allSetting.Count - 1].sortId + 1;
            ExternalSettingValue externalSettingValue = new ExternalSettingValue(-1, 5, strArray[0].Trim(), strArray[1].Trim(), sortId);
            int num = Session.ConfigurationManager.AddExternalOrgSettingValue(externalSettingValue);
            externalSettingValue.settingId = num;
            TPOMigrationManager.allSettings["Price Group"].Add(externalSettingValue);
            originatorManagementData1.EPPSPriceGroup = num.ToString();
          }
        }
      }
      originatorManagementData1.HierarchyPath = originatorManagementData1.Parent != 0 ? Session.ConfigurationManager.GetExternalOrganization(false, originatorManagementData1.Parent).HierarchyPath + "\\" + originatorManagementData1.OrganizationName : "Third Party Originators\\" + originatorManagementData1.OrganizationName;
      List<ExternalOrgSalesRep> externalOrgSalesRepList = new List<ExternalOrgSalesRep>();
      string xpath2 = branch ? "TPOBranchAEList" : "TPOCompanyAEList";
      foreach (XmlNode selectNode1 in node.SelectNodes(xpath2))
      {
        foreach (XmlNode selectNode2 in selectNode1.SelectNodes("TPOAE"))
        {
          if (selectNode2["UserID"] != null && !selectNode2["UserID"].InnerText.Contains("@"))
          {
            string innerText;
            if (!selectNode2["UserID"].InnerText.Contains(";"))
              innerText = selectNode2["UserID"].InnerText;
            else
              innerText = selectNode2["UserID"].InnerText.Split(';')[0];
            string str = innerText;
            externalOrgSalesRepList.Add(new ExternalOrgSalesRep(0, originatorManagementData1.oid, str.Trim(), "", "", "", "", "", ""));
            if (originatorManagementData1.PrimarySalesRepUserId == null || originatorManagementData1.PrimarySalesRepUserId == "")
              originatorManagementData1.PrimarySalesRepUserId = str.Trim();
          }
        }
      }
      if (!TPOMigrationManager.validateOrg(originatorManagementData1))
        return;
      List<ExternalOriginatorManagementData> organizationByTpoid1 = Session.ConfigurationManager.GetExternalOrganizationByTPOID(originatorManagementData1.ExternalID);
      if (organizationByTpoid1.Count == 0)
      {
        ExternalOriginatorManagementData organizationByTpoid2 = Session.ConfigurationManager.GetOldExternalOrganizationByTPOID(originatorManagementData1.ExternalID);
        originatorManagementData1.oid = organizationByTpoid2.oid;
        try
        {
          if (originatorManagementData1.Parent == organizationByTpoid2.Parent)
            originatorManagementData1.UseParentInfo = organizationByTpoid2.UseParentInfo;
          Session.ConfigurationManager.UpdateExternalContact(false, originatorManagementData1, (LoanCompHistoryList) null);
        }
        catch
        {
        }
      }
      else
      {
        foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid1)
        {
          if (originatorManagementData2.OrganizationType == originatorManagementData1.OrganizationType && originatorManagementData2.CompanyDBAName == originatorManagementData1.CompanyDBAName)
          {
            originatorManagementData1.oid = originatorManagementData2.oid;
            try
            {
              if (originatorManagementData1.Parent == originatorManagementData2.Parent)
                originatorManagementData1.UseParentInfo = originatorManagementData2.UseParentInfo;
              Session.ConfigurationManager.UpdateExternalContact(false, originatorManagementData1, (LoanCompHistoryList) null);
              break;
            }
            catch
            {
              break;
            }
          }
        }
      }
      if (originatorManagementData1.oid == 0)
      {
        try
        {
          originatorManagementData1.oid = Session.ConfigurationManager.AddManualContact(false, originatorManagementData1, false, originatorManagementData1.Parent, originatorManagementData1.Depth, originatorManagementData1.HierarchyPath, (LoanCompHistoryList) null);
        }
        catch
        {
        }
      }
      if (originatorManagementData1.oid < 0)
        return;
      Session.ConfigurationManager.AddExternalOrganizationSalesReps(externalOrgSalesRepList.ToArray());
      siteURL.EntityType = (int) originatorManagementData1.entityType;
      Session.ConfigurationManager.UpdateExternalOrganizationSelectedURLs(originatorManagementData1.oid, new List<ExternalOrgURL>()
      {
        siteURL
      }, -1);
      foreach (ExternalOrgURL selectedOrgUrl in Session.ConfigurationManager.GetSelectedOrgUrls(originatorManagementData1.oid))
      {
        if (selectedOrgUrl.siteId == siteURL.siteId)
        {
          siteURL = selectedOrgUrl;
          break;
        }
      }
      ExternalOrgLoanTypes loanTypes = new ExternalOrgLoanTypes()
      {
        Broker = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType()
      };
      loanTypes.Broker.ExternalOrgID = originatorManagementData1.oid;
      loanTypes.Broker.MsgUploadNonApprovedLoans = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
      loanTypes.Broker.AllowLoansWithIssues = 0;
      loanTypes.Broker.ChannelType = 0;
      loanTypes.Broker.LoanTypes |= 1;
      loanTypes.Broker.LoanTypes |= 64;
      loanTypes.Broker.LoanTypes |= 16;
      loanTypes.Broker.LoanTypes |= 128;
      loanTypes.Broker.LoanTypes |= 128;
      if (node["FHAOriginator"] != null && node["FHAOriginator"].InnerText != "false")
        loanTypes.Broker.LoanTypes |= 2;
      if (node["VAOriginator"] != null && node["VAOriginator"].InnerText != "false")
        loanTypes.Broker.LoanTypes |= 4;
      if (node["USDAOriginator"] != null && node["USDAOriginator"].InnerText != "false")
        loanTypes.Broker.LoanTypes |= 8;
      loanTypes.Broker.LoanPurpose = 63;
      loanTypes.CorrespondentDelegated = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType();
      loanTypes.CorrespondentDelegated.ExternalOrgID = originatorManagementData1.oid;
      loanTypes.CorrespondentDelegated.MsgUploadNonApprovedLoans = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
      loanTypes.CorrespondentDelegated.AllowLoansWithIssues = 0;
      loanTypes.CorrespondentDelegated.ChannelType = 1;
      loanTypes.CorrespondentDelegated.LoanTypes = loanTypes.Broker.LoanTypes;
      loanTypes.CorrespondentDelegated.LoanPurpose = 63;
      loanTypes.CorrespondentNonDelegated = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType();
      loanTypes.CorrespondentNonDelegated.ExternalOrgID = originatorManagementData1.oid;
      loanTypes.CorrespondentNonDelegated.MsgUploadNonApprovedLoans = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
      loanTypes.CorrespondentNonDelegated.AllowLoansWithIssues = 0;
      loanTypes.CorrespondentNonDelegated.ChannelType = 2;
      loanTypes.CorrespondentNonDelegated.LoanTypes = loanTypes.Broker.LoanTypes;
      loanTypes.CorrespondentNonDelegated.LoanPurpose = 63;
      Session.ConfigurationManager.UpdateExternalOrganizationLoanTypes(originatorManagementData1.oid, loanTypes);
      foreach (XmlNode selectNode in node.SelectNodes("Notes"))
      {
        if (selectNode.Value != null)
          Session.ConfigurationManager.AddExternalOrganizationNotes(new ExternalOrgNote(Session.UserID, selectNode.Value)
          {
            ExternalCompanyID = originatorManagementData1.oid
          });
      }
      BranchExtLicensing license = new BranchExtLicensing();
      if (node["InheritCompanyLicense"] != null)
        license.UseParentInfo = node["InheritCompanyLicense"].InnerText == "true";
      foreach (XmlNode selectNode3 in node.SelectNodes("TPOCompanyLicenses"))
      {
        foreach (XmlNode selectNode4 in selectNode3.SelectNodes("TPOLicense"))
        {
          if (selectNode4 != null)
            license.AddStateLicenseExtType(TPOMigrationManager.parseOrgLicense(selectNode4));
        }
      }
      Session.ConfigurationManager.UpdateExternalLicence(license, originatorManagementData1.oid);
      if (node["LegacyID"] != null)
      {
        ContactCustomField contactCustomField = (ContactCustomField) null;
        if (node["LegacyID"].InnerText != "")
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in TPOMigrationManager.customFields.Items)
          {
            if (contactCustomFieldInfo.Label == "LegacyID")
            {
              contactCustomField = new ContactCustomField(originatorManagementData1.oid, contactCustomFieldInfo.LabelID, contactCustomFieldInfo.OwnerID, node["LegacyID"].InnerText);
              break;
            }
          }
          Session.ConfigurationManager.UpdateCustomFieldValues(originatorManagementData1.oid, new ContactCustomField[1]
          {
            contactCustomField
          });
        }
      }
      foreach (XmlNode selectNode5 in node.SelectNodes("TPOCompanyContacts"))
      {
        foreach (XmlNode selectNode6 in selectNode5.SelectNodes("TPOContact"))
        {
          if (selectNode6 != null)
            TPOMigrationManager.parseContact(selectNode6, originatorManagementData1.oid, originatorManagementData1.PrimarySalesRepUserId, siteURL);
        }
      }
      foreach (XmlNode selectNode7 in node.SelectNodes("TPOBranchList"))
      {
        foreach (XmlNode selectNode8 in selectNode7.SelectNodes("TPOBranch"))
        {
          if (selectNode8 != null)
            TPOMigrationManager.parseOrg(selectNode8, true, originatorManagementData1.oid, 2, originatorManagementData1.entityType, siteURL);
        }
      }
    }

    private static bool validateOrg(ExternalOriginatorManagementData org)
    {
      return !(org.OrganizationName == "") && org.entityType != ExternalOriginatorEntityType.None && !(org.ExternalID == "") && !(org.Address == "") && !(org.City == "") && !(org.State == "") && !(org.PhoneNumber == "") && !(org.PrimarySalesRepUserId == "");
    }

    private static void parseContact(
      XmlNode nodeContact,
      int oid,
      string salesRep,
      ExternalOrgURL siteURL)
    {
      ExternalUserInfo externalUserInfo1 = new ExternalUserInfo();
      externalUserInfo1.ExternalOrgID = oid;
      externalUserInfo1.FirstName = nodeContact["FirstName"].InnerText;
      externalUserInfo1.LastName = nodeContact["LastName"].InnerText;
      externalUserInfo1.Fax = nodeContact["BusinessFax"].InnerText;
      bool needsUpdate = false;
      if (externalUserInfo1.Fax != null && externalUserInfo1.Fax != "")
        externalUserInfo1.Fax = Utils.FormatInput(externalUserInfo1.Fax, FieldFormat.PHONE, ref needsUpdate);
      externalUserInfo1.Phone = nodeContact["BusinessPhone"].InnerText.Length <= 20 ? nodeContact["BusinessPhone"].InnerText : nodeContact["BusinessPhone"].InnerText.Substring(0, 20);
      if (externalUserInfo1.Phone != null && externalUserInfo1.Phone != "")
        externalUserInfo1.Phone = Utils.FormatInput(externalUserInfo1.Phone, FieldFormat.PHONE, ref needsUpdate);
      externalUserInfo1.City = nodeContact["City"].InnerText;
      externalUserInfo1.ContactID = nodeContact["ContactID"].InnerText;
      externalUserInfo1.Email = nodeContact["Email"].InnerText;
      if (nodeContact["AEUserID"] != null)
      {
        ExternalUserInfo externalUserInfo2 = externalUserInfo1;
        string innerText;
        if (!nodeContact["AEUserID"].InnerText.Contains(";"))
          innerText = nodeContact["AEUserID"].InnerText;
        else
          innerText = nodeContact["AEUserID"].InnerText.Split(';')[0];
        externalUserInfo2.SalesRepID = innerText;
      }
      else
        externalUserInfo1.SalesRepID = salesRep;
      externalUserInfo1.RequirePasswordChange = nodeContact["IsTemporaryPassword"].InnerText == "true";
      externalUserInfo1.PasswordChangedDate = nodeContact["PasswordChangedAt"].InnerText != "" ? Convert.ToDateTime(nodeContact["PasswordChangedAt"].InnerText) : DateTime.MinValue;
      externalUserInfo1.WelcomeEmailDate = nodeContact["MailSentDate"].InnerText != "" ? Convert.ToDateTime(nodeContact["MailSentDate"].InnerText) : DateTime.MinValue;
      externalUserInfo1.CellPhone = nodeContact["MobilePhone"].InnerText;
      if (externalUserInfo1.CellPhone != null && externalUserInfo1.CellPhone != "")
        externalUserInfo1.CellPhone = Utils.FormatInput(externalUserInfo1.CellPhone, FieldFormat.PHONE, ref needsUpdate);
      externalUserInfo1.Notes = nodeContact["Notes"].InnerText;
      externalUserInfo1.NmlsID = nodeContact["NMLSID"].InnerText.Length <= 12 ? nodeContact["NMLSID"].InnerText : nodeContact["NMLSID"].InnerText.Substring(0, 12);
      if (nodeContact["Role"].InnerText.ToLower().Contains("lo"))
        externalUserInfo1.Roles |= 1;
      if (nodeContact["Role"].InnerText.ToLower().Contains("lp"))
        externalUserInfo1.Roles |= 2;
      if (nodeContact["Role"].InnerText.ToLower().Contains("manager"))
        externalUserInfo1.Roles |= 4;
      if (nodeContact["Role"].InnerText.ToLower().Contains("admin"))
        externalUserInfo1.Roles |= 8;
      externalUserInfo1.SSN = nodeContact["SocialSecurityNumber"].InnerText;
      externalUserInfo1.Address = nodeContact["StreetAddress"].InnerText;
      externalUserInfo1.State = nodeContact["State"].InnerText;
      externalUserInfo1.Zipcode = nodeContact["Zip"].InnerText;
      externalUserInfo1.DisabledLogin = nodeContact["Status"].InnerText != "true";
      externalUserInfo1.UpdatedByExternal = true;
      externalUserInfo1.UpdatedBy = nodeContact["UpdateBy"].InnerText;
      externalUserInfo1.UpdatedDateTime = nodeContact["UpdateDate"].InnerText != "" ? Convert.ToDateTime(nodeContact["UpdateDate"].InnerText) : DateTime.MinValue;
      externalUserInfo1.EmailForLogin = externalUserInfo1.Email;
      foreach (XmlNode selectNode1 in nodeContact.SelectNodes("TPOContactLicenses"))
      {
        foreach (XmlNode selectNode2 in selectNode1.SelectNodes("TPOLicense"))
        {
          if (selectNode2 != null)
            externalUserInfo1.Licenses.Add(TPOMigrationManager.parseUserLicense(selectNode2));
        }
      }
      if (!TPOMigrationManager.validateContact(externalUserInfo1))
        return;
      ExternalUserInfo userInfoByContactId = Session.ConfigurationManager.GetExternalUserInfoByContactId(externalUserInfo1.ContactID);
      if ((UserInfo) userInfoByContactId != (UserInfo) null)
        externalUserInfo1.ExternalUserID = userInfoByContactId.ExternalUserID;
      ExternalUserInfo externalUserInfo3 = Session.ConfigurationManager.SaveExternalUserInfo(externalUserInfo1);
      Session.ConfigurationManager.SaveExternalUserInfoURLs(externalUserInfo3.ExternalUserID, new int[1]
      {
        siteURL.URLID
      });
      Session.ConfigurationManager.ResetExternalUserInfoPassword(externalUserInfo3.ExternalUserID, nodeContact["Password"].InnerText, externalUserInfo3.PasswordChangedDate, externalUserInfo3.RequirePasswordChange);
    }

    private static bool validateContact(ExternalUserInfo user)
    {
      return !(user.ContactID == "") && !(user.FirstName == "") && !(user.LastName == "") && !(user.Email == "") && !(user.EmailForLogin == "") && !(user.SalesRepID == "") && user.Roles != 0;
    }

    private static StateLicenseExtType parseOrgLicense(XmlNode nodeLicense)
    {
      StateLicenseExtType orgLicense = new StateLicenseExtType(nodeLicense["State"].InnerText, "", false, false);
      orgLicense.IssueDate = !(nodeLicense["LicenseIssueDate"].InnerText != "") ? DateTime.MinValue : (!(Convert.ToDateTime(nodeLicense["LicenseIssueDate"].InnerText) >= new DateTime(2079, 6, 6)) ? Convert.ToDateTime(nodeLicense["LicenseIssueDate"].InnerText) : new DateTime(2079, 6, 5));
      orgLicense.EndDate = !(nodeLicense["LicenseExpirationDate"].InnerText != "") ? DateTime.MinValue : (!(Convert.ToDateTime(nodeLicense["LicenseExpirationDate"].InnerText) >= new DateTime(2079, 6, 6)) ? Convert.ToDateTime(nodeLicense["LicenseExpirationDate"].InnerText) : new DateTime(2079, 6, 5));
      orgLicense.LicenseNo = nodeLicense["Number"].InnerText;
      orgLicense.LicenseStatus = nodeLicense["Status"].InnerText;
      orgLicense.Approved = nodeLicense["Status"].InnerText.ToLower() == "approved";
      if (nodeLicense["Type"] != null && nodeLicense["Type"].InnerText != "")
        orgLicense.LicenseType = TPOMigrationManager.getCorrespondingLicenseType(nodeLicense["State"].InnerText, nodeLicense["Type"].InnerText);
      return orgLicense;
    }

    private static StateLicenseExtType parseUserLicense(XmlNode nodeLicense)
    {
      return new StateLicenseExtType(nodeLicense["State"].InnerText, "", false, false)
      {
        Approved = nodeLicense["Status"].InnerText.ToLower() == "approved"
      };
    }

    private static string getCorrespondingLicenseType(string state, string licenseType)
    {
      if (TPOMigrationManager.licenseTypes.Count == 0)
        TPOMigrationManager.createLicenseTable();
      string key = state + "_" + licenseType;
      return TPOMigrationManager.licenseTypes.ContainsKey(key) ? TPOMigrationManager.licenseTypes[key] : "";
    }

    private static void createLicenseTable()
    {
      TPOMigrationManager.licenseTypes.Clear();
      TPOMigrationManager.licenseTypes.Add("AK_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("AK_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("AK_Mortgage Lender/Broker", "Mortgage Lender/Broker");
      TPOMigrationManager.licenseTypes.Add("AK_Small Loan Company", "Small Loan Company");
      TPOMigrationManager.licenseTypes.Add("AK_Exempt Mortgage Lender/Broker", "Exempt Mortgage Lender/Broker");
      TPOMigrationManager.licenseTypes.Add("AL_Consumer Finance License", "Consumer Credit License");
      TPOMigrationManager.licenseTypes.Add("AL_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("AR_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("AR_Mortgage Banker", "Mortgage Banker");
      TPOMigrationManager.licenseTypes.Add("AZ_Consumer Lender", "Consumer Lender");
      TPOMigrationManager.licenseTypes.Add("AZ_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("AZ_Mortgage Banker", "Mortgage Banker License");
      TPOMigrationManager.licenseTypes.Add("CA_California Finance Broker", "Finance Lenders Law License (Broker)");
      TPOMigrationManager.licenseTypes.Add("CA_California Finance Lender", "Finance Lenders Law License (Lender)");
      TPOMigrationManager.licenseTypes.Add("CA_Corporation", "Real Estate Corporation License");
      TPOMigrationManager.licenseTypes.Add("CA_Broker", "Real Estate Broker License");
      TPOMigrationManager.licenseTypes.Add("CA_Residential Mortgage Lender", "Residential Mortgage Lending Act License");
      TPOMigrationManager.licenseTypes.Add("CO_Supervised Lender's License", "Supervised Lender's License");
      TPOMigrationManager.licenseTypes.Add("CT_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("CT_Mortgage Correspondent Lender", "Mortgage Correspondent Lender");
      TPOMigrationManager.licenseTypes.Add("CT_Mortgage Lender/Broker", "Mortgage Lender License");
      TPOMigrationManager.licenseTypes.Add("DC_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("DC_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("DE_Licensed Lender", "Licensed Lender");
      TPOMigrationManager.licenseTypes.Add("DE_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("DE_Exempt Licensed Lender", "Exempt Licensed Lender");
      TPOMigrationManager.licenseTypes.Add("FL_Consumer Finance Lender", "Consumer Finance Lender");
      TPOMigrationManager.licenseTypes.Add("FL_Correspondent Mortgage Lender", "Correspondent Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("FL_Correspondent Mortgage Lender Branch", "Correspondent Mortgage Lender Branch");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Broker Business", "Mortgage Broker Business");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Broker Business Branch", "Mortgage Broker Business Branch");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Lender Branch", "Mortgage Lender Branch");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Lender Savings Clause", "Mortgage Lender Savings Clause");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Lender Savings Clause Transfer", "Mortgage Lender Savings Clause Transfer");
      TPOMigrationManager.licenseTypes.Add("FL_Mortgage Lender Savings Clause Transfer Branch", "Mortgage Lender Savings Clause Transfer Branch");
      TPOMigrationManager.licenseTypes.Add("GA_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("GA_Mortgage Lender License", "Mortgage Lender License");
      TPOMigrationManager.licenseTypes.Add("GA_Registrant", "Registrant");
      TPOMigrationManager.licenseTypes.Add("HI_Financial Services Loan Company", "Financial Services Loan Company");
      TPOMigrationManager.licenseTypes.Add("HI_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("HI_Mortgage Broker Branch", "Mortgage Broker Branch");
      TPOMigrationManager.licenseTypes.Add("IA_Industrial Loan License", "Industrial Loan License");
      TPOMigrationManager.licenseTypes.Add("IA_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("IA_Mortgage Banker", "Mortgage Banker");
      TPOMigrationManager.licenseTypes.Add("IA_Regulated", "Regulated");
      TPOMigrationManager.licenseTypes.Add("ID_Mortgage Lender/Broker", "Mortgage Broker/Lender License");
      TPOMigrationManager.licenseTypes.Add("IL_Mortgage Broker", "Residential Mortgage License (Brokering Activity)");
      TPOMigrationManager.licenseTypes.Add("IL_Mortgage Originator", "Residential Mortgage License (Originating Activity)");
      TPOMigrationManager.licenseTypes.Add("IN_First Lien Mortgage Lending License", "First Lien Mortgage Lending License");
      TPOMigrationManager.licenseTypes.Add("IN_Loan Broker", "Loan Broker License");
      TPOMigrationManager.licenseTypes.Add("IN_Second Mortgage Lender / Consumer Loan License", "Subordinate Lien Mortgage Lending License");
      TPOMigrationManager.licenseTypes.Add("KS_Mortgage Company", "Mortgage Company License");
      TPOMigrationManager.licenseTypes.Add("KS_Supervised Lender", "Supervised Loan License");
      TPOMigrationManager.licenseTypes.Add("KY_Consumer Loan Company", "Consumer Loan Company");
      TPOMigrationManager.licenseTypes.Add("KY_Mortgage Loan Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("KY_Mortgage Loan Company", "Mortgage Company License");
      TPOMigrationManager.licenseTypes.Add("LA_Residential Mortgage Lender/Broker", "Residential Mortgage Lending License");
      TPOMigrationManager.licenseTypes.Add("MA_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("MA_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("MA_Small Loan Agency", "Small Loan Agency License");
      TPOMigrationManager.licenseTypes.Add("MD_Mortgage Lenders", "Mortgage Lender License");
      TPOMigrationManager.licenseTypes.Add("ME_Loan Brokers", "Loan Brokers");
      TPOMigrationManager.licenseTypes.Add("ME_Supervised Lender", "Supervised Lender");
      TPOMigrationManager.licenseTypes.Add("MI_Consumer Finance License", "Consumer Finance License");
      TPOMigrationManager.licenseTypes.Add("MI_First Mortgage Broker", "First Mortgage Broker License/Registrant");
      TPOMigrationManager.licenseTypes.Add("MI_First Mortgage Lender", "First Mortgage Lender License/Registrant");
      TPOMigrationManager.licenseTypes.Add("MI_Second Mortgage Broker", "Second Mortgage Broker License/Registrant");
      TPOMigrationManager.licenseTypes.Add("MI_Second Mortgage Lender", "Second Mortgage Lender License/Registrant");
      TPOMigrationManager.licenseTypes.Add("MN_Minnesota Industrial Loan Company", "Minnesota Industrial Loan Company");
      TPOMigrationManager.licenseTypes.Add("MN_Mortgage Originator", "Residential Mortgage Originator License");
      TPOMigrationManager.licenseTypes.Add("MN_Minnesota Regulated Lender", "Minnesota Regulated Lender");
      TPOMigrationManager.licenseTypes.Add("MN_Originator Exempt", "Originator Exempt");
      TPOMigrationManager.licenseTypes.Add("MO_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("MO_Exempt Mortgage Broker", "Exempt Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("MS_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("MS_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("MT_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("MT_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("NC_Mortgage Broker (2009)", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("NC_Mortgage Lender (2009)", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("ND_Money Broker", "Money Broker License");
      TPOMigrationManager.licenseTypes.Add("NE_Mortgage Banker", "Mortgage Bankers");
      TPOMigrationManager.licenseTypes.Add("NH_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("NH_Mortgage Banker", "Mortgage Banker");
      TPOMigrationManager.licenseTypes.Add("NJ_Correspondent Mortgage Banker", "Correspondent Mortgage Banker");
      TPOMigrationManager.licenseTypes.Add("NJ_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("NJ_Mortgage Banker", "Mortgage Banker");
      TPOMigrationManager.licenseTypes.Add("NJ_Residential Correspondent Mortgage Lender", "Correspondent Residential Mortgage Lender License");
      TPOMigrationManager.licenseTypes.Add("NJ_Residential Mortgage Broker", "Residential Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("NJ_Residential Mortgage Lender", "Residential Mortgage Lender License");
      TPOMigrationManager.licenseTypes.Add("NJ_Secondary Mortgage Loan Licensee", "Secondary Mortgage Loan Licensee");
      TPOMigrationManager.licenseTypes.Add("NM_Registered Mortgage Company", "Mortgage Loan Company License");
      TPOMigrationManager.licenseTypes.Add("NV_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("NV_Mortgage Banker", "Mortgage Banker");
      TPOMigrationManager.licenseTypes.Add("NV_Thrift Companies License", "Thrift Companies License");
      TPOMigrationManager.licenseTypes.Add("NV_Exempt Mortgage Broker", "Exempt Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("NV_Exempt Mortgage Banker", "Exempt Mortgage Banker");
      TPOMigrationManager.licenseTypes.Add("NY_Mortgage Broker", "Mortgage Broker Registration");
      TPOMigrationManager.licenseTypes.Add("NY_Mortgage Banker", "Mortgage Banker License");
      TPOMigrationManager.licenseTypes.Add("NY_Mortgage Banker Exempt", "Mortgage Banker Exempt");
      TPOMigrationManager.licenseTypes.Add("OH_Mortgage Broker", "Mortgage Broker Act Certificate of Registration");
      TPOMigrationManager.licenseTypes.Add("OH_Second Mortgage Lender", "Mortgage Loan Act Certificate of Registration");
      TPOMigrationManager.licenseTypes.Add("OK_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("OK_Supervised Lender", "Supervised Lender");
      TPOMigrationManager.licenseTypes.Add("OR_Consumer Finance Company", "Consumer Finance Company");
      TPOMigrationManager.licenseTypes.Add("OR_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("PA_Consumer Discount Company", "Mortgage Consumer Discount Company License");
      TPOMigrationManager.licenseTypes.Add("PA_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("PA_Mortgage Lender", "Mortgage Lender License");
      TPOMigrationManager.licenseTypes.Add("PA_Mortgage Loan Correspondent", "Mortgage Loan Correspondent License");
      TPOMigrationManager.licenseTypes.Add("RI_Licensed Broker", "Loan Broker License");
      TPOMigrationManager.licenseTypes.Add("RI_Licensed Lender", "Lender License");
      TPOMigrationManager.licenseTypes.Add("SC_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("SC_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("SC_Supervised Lender", "Supervised Lender");
      TPOMigrationManager.licenseTypes.Add("SD_Mortgage Lender", "Mortgage Lender License");
      TPOMigrationManager.licenseTypes.Add("SD_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("TN_Industrial Loan and Thrift", "Industrial Loan and Thrift");
      TPOMigrationManager.licenseTypes.Add("TN_Mortgage Company", "Mortgage License");
      TPOMigrationManager.licenseTypes.Add("TX_Mortgage Broker", "Mortgage Company License");
      TPOMigrationManager.licenseTypes.Add("TX_Mortgage Banker", "Mortgage Banker Registration");
      TPOMigrationManager.licenseTypes.Add("TX_Regulated Loan", "Regulated Lender License");
      TPOMigrationManager.licenseTypes.Add("TX_Constitution Approved Lender", "Both Mortgage Banker and Regulated Lender License");
      TPOMigrationManager.licenseTypes.Add("UT_Mortgage Lender", "Company\tMortgage Entity License");
      TPOMigrationManager.licenseTypes.Add("UT_Regulated Lender", "Regulated Lender");
      TPOMigrationManager.licenseTypes.Add("VA_Mortgage Broker", "Mortgage Broker");
      TPOMigrationManager.licenseTypes.Add("VA_Mortgage Lender", "Mortgage Lender");
      TPOMigrationManager.licenseTypes.Add("VA_Mortgage Lender/Broker", "Mortgage Lender/Broker");
      TPOMigrationManager.licenseTypes.Add("VT_Lenders Licensed", "Lender License");
      TPOMigrationManager.licenseTypes.Add("VT_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("WA_Consumer Loan", "Consumer Loan License");
      TPOMigrationManager.licenseTypes.Add("WA_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("WA_Exempt Mortgage Broker", "Exempt Mortgage Broker Registration");
      TPOMigrationManager.licenseTypes.Add("WI_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("WI_Mortgage Banker", "Mortgage Banker License");
      TPOMigrationManager.licenseTypes.Add("WI_Mortgage Banker and Loan Company", "Mortgage Banker License and Loan Companies License");
      TPOMigrationManager.licenseTypes.Add("WV_Mortgage Broker", "Mortgage Broker License");
      TPOMigrationManager.licenseTypes.Add("WV_Mortgage Lender", "Mortgage Banker License");
      TPOMigrationManager.licenseTypes.Add("WV_Regulated Consumer Lender", "Regulated Consumer Lenders");
      TPOMigrationManager.licenseTypes.Add("WV_Exempt Mortgage Lender", "Exempt Company Registration");
      TPOMigrationManager.licenseTypes.Add("WY_Mortgage Lender/Broker", "Mortgage Lender/Broker License");
      TPOMigrationManager.licenseTypes.Add("WY_Supervised Lender", "Supervised Lender");
    }
  }
}
