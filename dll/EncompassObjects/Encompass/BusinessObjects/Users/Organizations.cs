// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Organizations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class Organizations : SessionBoundObject, IOrganizations
  {
    internal Organizations(Session session)
      : base(session)
    {
    }

    public Organization GetTopMostOrganization()
    {
      return new Organization(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetRootOrganization());
    }

    internal ExternalOrganizationList GetTopMostExternalOrganizations()
    {
      return EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization.ToList(this.Session, ((IEnumerable<ExternalOriginatorManagementData>) ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllExternalOrganizations(false).ToArray()).TakeWhile<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.Parent == 0)).ToArray<ExternalOriginatorManagementData>(), this.HasPerformancePatch());
    }

    public OrganizationList GetAllOrganizations()
    {
      return Organization.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetAllOrganizations());
    }

    public ExternalOrganizationList GetAllExternalOrganizations()
    {
      return EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization.ToList(this.Session, ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllExternalOrganizations(false).ToArray(), this.HasPerformancePatch());
    }

    private bool HasPerformancePatch()
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      try
      {
        iconfigurationManager.GetExternalAdditionalDetails(123, new List<ExternalOriginatorOrgSetting>());
      }
      catch
      {
        return false;
      }
      return true;
    }

    public OrganizationList GetOrganizationsByName(string orgName)
    {
      return Organization.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetOrganizationsByName(orgName));
    }

    public Organization GetOrganization(int orgId)
    {
      OrgInfo organization = ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetOrganization(orgId);
      return organization == null ? (Organization) null : new Organization(this.Session, organization);
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      int orgId)
    {
      return this.GetExternalOrganization(orgId, false);
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      int orgId,
      bool getAllDetails)
    {
      List<ExternalOriginatorManagementData> companyOrganizations = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanyOrganizations(orgId);
      return companyOrganizations.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == orgId)) == null ? (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization) null : new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, companyOrganizations, orgId, getAllDetails, this.HasPerformancePatch());
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      string externalID)
    {
      return this.GetExternalOrganization(externalID, false);
    }

    public List<PriceGroup> GetPriceGroups()
    {
      Dictionary<string, List<ExternalSettingValue>> externalOrgSettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrgSettings();
      if (!externalOrgSettings.ContainsKey("Price Group") || externalOrgSettings["Price Group"] == null)
        externalOrgSettings["Price Group"] = new List<ExternalSettingValue>();
      List<PriceGroup> result = new List<PriceGroup>();
      externalOrgSettings["Price Group"].ForEach((Action<ExternalSettingValue>) (x => result.Add(new PriceGroup(this.Session, x.settingValue, x.settingCode, x.settingId))));
      return result;
    }

    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExternalOrganizationsWithExtension(
      string externalID)
    {
      return this.GetExternalOrganizationsWithExtension(externalID, false);
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      string externalID,
      bool getAllDetails)
    {
      List<ExternalOriginatorManagementData> companyOrganizations = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanyOrganizations(externalID);
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      foreach (ExternalOriginatorManagementData originatorManagementData2 in companyOrganizations.ToArray())
      {
        string str = originatorManagementData2.ExternalID;
        if (originatorManagementData2.ExternalID.Length > 10)
          str = originatorManagementData2.ExternalID.Substring(1);
        if (str == externalID && (originatorManagementData2.OrganizationType == null || originatorManagementData2.OrganizationType == 2))
        {
          originatorManagementData1 = originatorManagementData2;
          break;
        }
      }
      return originatorManagementData1 == null ? (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization) null : new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, companyOrganizations, originatorManagementData1.oid, getAllDetails, this.HasPerformancePatch());
    }

    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExternalOrganizationsWithExtension(
      string externalID,
      bool getAllDetails)
    {
      List<ExternalOriginatorManagementData> companyOrganizations = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanyOrganizations(externalID);
      List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> organizationsWithExtension = new List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization>();
      foreach (ExternalOriginatorManagementData originatorManagementData in companyOrganizations.ToArray())
      {
        string str = originatorManagementData.ExternalID;
        if (originatorManagementData.ExternalID.Length > 10)
          str = originatorManagementData.ExternalID.Substring(1);
        if (str == externalID)
          organizationsWithExtension.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, companyOrganizations, originatorManagementData.oid, getAllDetails, this.HasPerformancePatch()));
      }
      return organizationsWithExtension;
    }

    internal List<object> GetExternalAdditionalDetails(int oid)
    {
      return ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalAdditionalDetails(oid);
    }

    internal List<object> GetExternalAdditionalDetails(string externalUserID)
    {
      return ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalAdditionalDetails(externalUserID);
    }

    public AEExternalAccessibleEntity GetAccessibleExternalOrganizations(string aeUserId)
    {
      IConfigurationManager mngr = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      mngr.GetAllExternalOrganizations(false);
      List<ExternalOriginatorManagementData> organizationBySalesRep = mngr.GetExternalOrganizationBySalesRep(aeUserId);
      List<ExternalUserInfo> userInfoBySalesRep = mngr.GetExternalUserInfoBySalesRep(aeUserId);
      List<string> companyList = new List<string>();
      Dictionary<string, string[]> branchList = new Dictionary<string, string[]>();
      List<string> userList = new List<string>();
      organizationBySalesRep.ForEach((Action<ExternalOriginatorManagementData>) (x =>
      {
        if (x.OrganizationType == null && !companyList.Contains(x.ExternalID))
        {
          companyList.Add(x.ExternalID);
        }
        else
        {
          if (branchList.ContainsKey(x.ExternalID))
            return;
          List<string> organizationDesendentsTpoid = mngr.GetExternalOrganizationDesendentsTPOID(x.oid);
          if (organizationDesendentsTpoid.Count > 0)
            branchList.Add(x.ExternalID, organizationDesendentsTpoid.ToArray());
          else
            branchList.Add(x.ExternalID, new string[0]);
        }
      }));
      userInfoBySalesRep.ForEach((Action<ExternalUserInfo>) (x => userList.Add(x.ExternalUserID)));
      return new AEExternalAccessibleEntity(companyList, branchList, userList);
    }

    public AEExternalAccessibleEntity GetVisibleExternalOrganizations(string aeUserId)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      UserInfo user = ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetUser(aeUserId);
      ArrayList andOrgBySalesRep = iconfigurationManager.GetExternalAndInternalUserAndOrgBySalesRep(user.Userid, user.OrgId);
      List<string> companyList = new List<string>();
      Dictionary<string, string[]> accessibleBranchies = new Dictionary<string, string[]>();
      List<string> stringList = new List<string>();
      ((List<ExternalOriginatorManagementData>) andOrgBySalesRep[1]).ForEach((Action<ExternalOriginatorManagementData>) (x => companyList.Add(x.ExternalID)));
      List<string> accessibleContacts = (List<string>) andOrgBySalesRep[4];
      return new AEExternalAccessibleEntity(companyList, accessibleBranchies, accessibleContacts);
    }

    internal static ExternalOriginatorManagementData GetExternalCompany(
      int orgID,
      List<ExternalOriginatorManagementData> orgList)
    {
      return orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.Depth == 1));
    }

    internal static ExternalOriginatorManagementData GetExternalBranch(
      int orgID,
      List<ExternalOriginatorManagementData> orgList)
    {
      ExternalOriginatorManagementData company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == orgID));
      if (company.Depth < 2)
        return (ExternalOriginatorManagementData) null;
      if (company.OrganizationType == 1)
        return (ExternalOriginatorManagementData) null;
      if (company.OrganizationType == 2)
        return company;
      while (company != null && company.Parent > 0)
      {
        company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == company.Parent));
        if (company.OrganizationType == 2)
          break;
      }
      return company;
    }

    public void AddSiteUrl(string url, string siteId)
    {
      if (siteId == null || siteId == string.Empty)
        throw new Exception("Site Id cannot be null or empty.");
      if (url == null || url == string.Empty)
        throw new Exception("URL cannot be null or empty.");
      if (!SystemUtil.IsValidURL(url))
        throw new Exception("Not a valid URL.");
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalOrgURL[] organizationUrLs = iconfigurationManager.GetExternalOrganizationURLs();
      ExternalOrgURL externalOrgUrl1 = ((IEnumerable<ExternalOrgURL>) organizationUrLs).FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.siteId == siteId));
      ExternalOrgURL externalOrgUrl2 = ((IEnumerable<ExternalOrgURL>) organizationUrLs).FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == url));
      if (externalOrgUrl1 != null)
        throw new Exception("Site Id already exists.");
      if (externalOrgUrl2 != null)
        throw new Exception("URL already exists.");
      iconfigurationManager.AddExternalOrganizationURL(siteId, url);
    }

    public void DeleteSiteUrl(ExternalSiteUrl url)
    {
      if (url == null || url.SiteId == null || url.SiteId == string.Empty)
        throw new Exception("Site Id cannot be null or empty.");
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).DeleteExternalOrganizationURL(url.SiteId);
    }

    public void UpdateSiteUrl(ExternalSiteUrl url)
    {
      if (url == null || url.SiteId == null || url.SiteId == string.Empty)
        throw new Exception("Site Id cannot be null or empty.");
      if (url.URL == null || url.URL == string.Empty)
        throw new Exception("URL cannot be null or empty.");
      if (!SystemUtil.IsValidURL(url.URL))
        throw new Exception("Not a valid URL.");
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalOrgURL externalOrgUrl = ((IEnumerable<ExternalOrgURL>) iconfigurationManager.GetExternalOrganizationURLs()).FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == url.URL));
      if (externalOrgUrl != null && externalOrgUrl.siteId != url.SiteId)
        throw new Exception("URL already exists.");
      iconfigurationManager.UpdateExternalOrganizationURL(new ExternalOrgURL()
      {
        siteId = url.SiteId,
        URL = url.URL
      });
    }

    public List<ExternalSiteUrl> GetSiteUrls()
    {
      ExternalOrgURL[] organizationUrLs = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrganizationURLs();
      List<ExternalSiteUrl> siteUrls = new List<ExternalSiteUrl>();
      if (organizationUrLs != null)
      {
        foreach (ExternalOrgURL externalOrgUrl in organizationUrLs)
        {
          if (!externalOrgUrl.isDeleted)
            siteUrls.Add(new ExternalSiteUrl(externalOrgUrl));
        }
      }
      return siteUrls;
    }

    public void ArchiveExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (settingObjs == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      List<string> stringList = new List<string>();
      foreach (ExternalDocumentsSettings settingObj in settingObjs)
        stringList.Add(settingObj.Guid.ToString());
      iconfigurationManager.ArchiveDocuments(-1, stringList);
    }

    public void DeleteExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (settingObjs == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      List<string> stringList = new List<string>();
      foreach (ExternalDocumentsSettings settingObj in settingObjs)
      {
        FileSystemEntry fileSystemEntry = new FileSystemEntry("\\\\" + (settingObj.Guid.ToString() + "." + ((IEnumerable<string>) settingObj.FileName.Split('.')).Last<string>()), (FileSystemEntry.Types) 2, (string) null);
        iconfigurationManager.DeleteDocument(-1, settingObj.Guid, fileSystemEntry);
      }
    }

    public void UnArchiveExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (settingObjs == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      List<string> stringList = new List<string>();
      foreach (ExternalDocumentsSettings settingObj in settingObjs)
        stringList.Add(settingObj.Guid.ToString());
      iconfigurationManager.UnArchiveDocuments(-1, stringList);
    }

    public List<ExternalDocumentsSettings> GetAllArchivedDocuments()
    {
      List<DocumentSettingInfo> archiveDocuments = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllArchiveDocuments(-1);
      List<ExternalDocumentsSettings> archivedDocuments = new List<ExternalDocumentsSettings>();
      foreach (DocumentSettingInfo docSetting in archiveDocuments)
        archivedDocuments.Add(new ExternalDocumentsSettings(docSetting));
      return archivedDocuments;
    }

    public List<ExternalDocumentsSettings> GetUnArchivedDocuments()
    {
      List<DocumentSettingInfo> externalDocuments = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalDocuments(-1, -1, -1);
      List<ExternalDocumentsSettings> archivedDocuments = new List<ExternalDocumentsSettings>();
      foreach (DocumentSettingInfo docSetting in externalDocuments)
        archivedDocuments.Add(new ExternalDocumentsSettings(docSetting));
      return archivedDocuments;
    }

    public DataObject RetrieveDocument(ExternalDocumentsSettings docObject)
    {
      if (docObject == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      try
      {
        IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
        // ISSUE: variable of a boxed type
        __Boxed<Guid> guid = (ValueType) docObject.Guid;
        string str1 = ((IEnumerable<string>) docObject.FileName.Split('.')).Last<string>();
        string str2 = guid.ToString() + "." + str1;
        ((IEnumerable<string>) docObject.FileName.Split('\\')).Last<string>();
        return new DataObject(iconfigurationManager.ReadDocumentFromDataFolder(str2));
      }
      catch (Exception ex)
      {
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentDoesNotExist, "Document not found.");
      }
    }

    public void EditDocument(ExternalDocumentsSettings docObject)
    {
      if (docObject == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      List<DocumentSettingInfo> archiveDocuments = iconfigurationManager.GetAllArchiveDocuments(-1);
      List<DocumentSettingInfo> externalDocuments = iconfigurationManager.GetExternalDocuments(-1, -1, -1);
      if (archiveDocuments.Find((Predicate<DocumentSettingInfo>) (a => a.Guid == docObject.Guid)) == null && externalDocuments.Find((Predicate<DocumentSettingInfo>) (a => a.Guid == docObject.Guid)) == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentDoesNotExist, "Document does not exists");
      DocumentSettingInfo documentSettingInfo = new DocumentSettingInfo();
      documentSettingInfo.Active = docObject.Active;
      documentSettingInfo.Guid = docObject.Guid;
      documentSettingInfo.FileName = docObject.FileName;
      documentSettingInfo.DisplayName = docObject.DisplayName;
      documentSettingInfo.StartDate = docObject.StartDate;
      documentSettingInfo.EndDate = docObject.EndDate;
      documentSettingInfo.Category = docObject.Category;
      documentSettingInfo.Channel = (ExternalOriginatorEntityType) (int) (byte) Convert.ToInt32((object) docObject.Channel);
      documentSettingInfo.DateAdded = docObject.DateAdded;
      documentSettingInfo.AvailbleAllTPO = docObject.AvailbleAllTPO;
      documentSettingInfo.AddedBy = docObject.AddedBy;
      documentSettingInfo.Status = (ExternalOriginatorStatus) (int) (byte) Convert.ToInt32((object) docObject.Status);
      documentSettingInfo.IsArchive = docObject.IsArchive;
      iconfigurationManager.UpdateDocument(-1, documentSettingInfo);
      if (documentSettingInfo.AvailbleAllTPO)
        iconfigurationManager.AssignDefaultDocumentToAll(documentSettingInfo);
      else
        iconfigurationManager.RemoveDefaultDocumentFromAll(documentSettingInfo);
    }

    public void UploadDocument(
      DataObject fileObject,
      string filename,
      string displayName,
      DateTime startDate,
      DateTime endDate,
      DocumentCategory Category,
      ExternalOrganizationEntityType channel,
      bool availbleAllTPO)
    {
      string[] array = new string[12]
      {
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".txt",
        ".tif",
        ".jpg",
        ".jpeg",
        ".jpe",
        ".csv",
        ".xml"
      };
      string str = Path.GetExtension(filename).ToLower().Trim();
      if (str == string.Empty || Array.IndexOf<string>(array, str) < 0)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidDocumentExtension, "The '" + str + "' file type is not supported. The allowed file types are '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.txt', '.tif', '.jpg', '.jpeg', '.jpe', '.csv', and '.xml'.");
      if (fileObject.Data.Length > 25000000)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentExceedSize, "File attachments cannot exceed 25 MB. Please select another file.");
      Guid guid = Guid.NewGuid();
      DocumentSettingInfo documentSettingInfo = new DocumentSettingInfo();
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      documentSettingInfo.Active = true;
      documentSettingInfo.Guid = guid;
      documentSettingInfo.FileName = filename;
      documentSettingInfo.DisplayName = displayName;
      documentSettingInfo.StartDate = startDate;
      documentSettingInfo.EndDate = endDate;
      documentSettingInfo.Category = Category.ID;
      documentSettingInfo.Channel = (ExternalOriginatorEntityType) (int) (byte) Convert.ToInt32((object) channel);
      documentSettingInfo.DateAdded = DateTime.Now;
      documentSettingInfo.AvailbleAllTPO = availbleAllTPO;
      documentSettingInfo.AddedBy = this.Session.GetUserInfo().FullName;
      documentSettingInfo.IsArchive = false;
      documentSettingInfo.FileSize = Utils.FormatByteSize((long) fileObject.Data.Length);
      iconfigurationManager.AddDocument(-1, documentSettingInfo, false);
      FileSystemEntry fileSystemEntry = new FileSystemEntry("\\\\" + guid.ToString() + str, (FileSystemEntry.Types) 2, (string) null);
      BinaryObject binaryObject = new BinaryObject(fileObject.Data);
      iconfigurationManager.CreateDocumentInDataFolder(fileSystemEntry, binaryObject);
      if (!availbleAllTPO)
        return;
      iconfigurationManager.AssignDefaultDocumentToAll(documentSettingInfo);
    }
  }
}
