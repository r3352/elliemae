// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IOrganizations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("1078B271-AA1A-4c3f-BA03-8A08379D9301")]
  public interface IOrganizations
  {
    Organization GetTopMostOrganization();

    OrganizationList GetAllOrganizations();

    ExternalOrganizationList GetAllExternalOrganizations();

    OrganizationList GetOrganizationsByName(string orgName);

    Organization GetOrganization(int orgId);

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      int orgId);

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      string externalID);

    List<PriceGroup> GetPriceGroups();

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      int orgId,
      bool getAllDetails);

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      string externalID,
      bool getAllDetails);

    AEExternalAccessibleEntity GetAccessibleExternalOrganizations(string encompassUserId);

    void AddSiteUrl(string url, string siteId);

    void DeleteSiteUrl(ExternalSiteUrl url);

    void UpdateSiteUrl(ExternalSiteUrl url);

    List<ExternalSiteUrl> GetSiteUrls();

    void ArchiveExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs);

    void DeleteExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs);

    void UnArchiveExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs);

    List<ExternalDocumentsSettings> GetAllArchivedDocuments();

    List<ExternalDocumentsSettings> GetUnArchivedDocuments();

    DataObject RetrieveDocument(ExternalDocumentsSettings docObject);

    void EditDocument(ExternalDocumentsSettings docObject);

    void UploadDocument(
      DataObject fileObject,
      string filename,
      string displayName,
      DateTime startDate,
      DateTime endDate,
      DocumentCategory Category,
      ExternalOrganizationEntityType channel,
      bool availbleAllTPO);

    List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExternalOrganizationsWithExtension(
      string externalID);

    List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExternalOrganizationsWithExtension(
      string externalID,
      bool getAllDetails);

    AEExternalAccessibleEntity GetVisibleExternalOrganizations(string aeUserId);
  }
}
