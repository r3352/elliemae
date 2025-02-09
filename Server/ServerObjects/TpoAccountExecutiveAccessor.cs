// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TpoAccountExecutiveAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class TpoAccountExecutiveAccessor
  {
    private const string ClassName = "TpoAccountExecutiveAccessor�";

    public static List<AeTpoContactInfo> GetAeTpoContacts(
      string aeSalesRep,
      string siteId,
      AeTpoContactSearchRequest searchRequest = null,
      bool canAccessGlobalTpoUsers = false,
      int start = 0,
      int limit = 0,
      bool includePersonaDetails = false)
    {
      IEnumerable<AeTpoContactInfo> source = (IEnumerable<AeTpoContactInfo>) new List<AeTpoContactInfo>();
      try
      {
        User latestVersion = UserStore.GetLatestVersion(aeSalesRep);
        bool isAdmin = latestVersion != null && !(latestVersion.UserInfo == (UserInfo) null) ? TpoAccountExecutiveAccessor.CheckUserIsAdmin(latestVersion.UserInfo) : throw new Exception("User " + aeSalesRep + " not found. ");
        int orgId = latestVersion.UserInfo.OrgId;
        QueryCriterion[] criteria = (QueryCriterion[]) null;
        string companyName = (string) null;
        string branchName = (string) null;
        if (searchRequest != null)
        {
          criteria = searchRequest.ContactSearchCriteria;
          companyName = searchRequest.CompanyName;
          branchName = searchRequest.BranchName;
        }
        source = TpoAccountExecutiveAccessor.GetAeTpoContacts(aeSalesRep, orgId, siteId, criteria, isAdmin, canAccessGlobalTpoUsers, start, limit, includePersonaDetails);
        if (companyName != null && !companyName.Trim().IsNullOrEmpty())
          source = source.Where<AeTpoContactInfo>((System.Func<AeTpoContactInfo, bool>) (tpo => tpo.CompanyName != null && tpo.CompanyName.IndexOf(companyName.Trim(), StringComparison.OrdinalIgnoreCase) >= 0));
        if (branchName != null)
        {
          if (!branchName.Trim().IsNullOrEmpty())
            source = source.Where<AeTpoContactInfo>((System.Func<AeTpoContactInfo, bool>) (tpo => tpo.BranchName != null && tpo.BranchName.IndexOf(branchName.Trim(), StringComparison.OrdinalIgnoreCase) >= 0));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TpoAccountExecutiveAccessor), ex);
      }
      return source.ToList<AeTpoContactInfo>();
    }

    private static bool CheckUserIsAdmin(UserInfo userInfo)
    {
      if (userInfo == (UserInfo) null)
        return false;
      if (userInfo.IsAdministrator() || userInfo.IsSuperAdministrator())
        return true;
      return !FeaturesAclDbAccessor.CheckPermission(AclFeature.ExternalSettings_ContactSalesRep, userInfo) && FeaturesAclDbAccessor.CheckPermission(AclFeature.ExternalSettings_OrganizationSettings, userInfo);
    }

    private static IEnumerable<AeTpoContactInfo> GetAeTpoContacts(
      string salesRepId,
      int orgId,
      string siteId,
      QueryCriterion[] criteria,
      bool isAdmin = false,
      bool canAccessGlobalTpoUsers = false,
      int start = 0,
      int limit = 0,
      bool includePersonaDetails = false)
    {
      List<AeTpoContactInfo> aeTpoContacts = new List<AeTpoContactInfo>();
      ArrayList arrayList = ExternalOrgManagementAccessor.ConstructTpoAeView(salesRepId, orgId, siteId, criteria, isAdmin, canAccessGlobalTpoUsers, start, limit, includePersonaDetails);
      DataTable dataTable = (DataTable) arrayList[0];
      Dictionary<string, List<string>> dictionary = (Dictionary<string, List<string>>) arrayList[1];
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        Persona[] allPersonas = PersonaAccessor.GetAllPersonas();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          string str1 = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["personasList"]);
          List<Persona> source = new List<Persona>();
          if (!string.IsNullOrEmpty(str1))
          {
            string str2 = str1;
            char[] chArray = new char[1]{ ',' };
            foreach (string s in str2.Split(chArray))
            {
              int personaId = 0;
              if (int.TryParse(s, out personaId))
                source.Add(Array.Find<Persona>(allPersonas, (Predicate<Persona>) (p => p.ID == personaId)));
            }
          }
          if (row != null)
          {
            AeTpoContactInfo aeTpoContactInfo = new AeTpoContactInfo()
            {
              ContactId = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ContactID"]),
              ExternalContactId = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ID"]),
              FirstName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["FirstName"]),
              LastName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["LastName"]),
              Email = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Login_email"]),
              BusinessPhone = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Phone"]),
              BranchName = "Corporate",
              Personas = source.Any<Persona>() ? source.ToArray() : (Persona[]) null
            };
            string key = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ContactOrgID"]);
            List<string> stringList = dictionary[key];
            if (!string.IsNullOrWhiteSpace(stringList[0]))
              aeTpoContactInfo.CompanyId = Convert.ToInt32(stringList[0]);
            if (!string.IsNullOrWhiteSpace(stringList[1]))
              aeTpoContactInfo.CompanyName = stringList[1];
            if (!string.IsNullOrWhiteSpace(stringList[2]))
              aeTpoContactInfo.CompanyExternalId = stringList[2];
            if (!string.IsNullOrWhiteSpace(stringList[3]))
              aeTpoContactInfo.BranchOrgId = Convert.ToInt32(stringList[3]);
            if (!string.IsNullOrWhiteSpace(stringList[4]))
              aeTpoContactInfo.BranchName = stringList[4];
            if (!string.IsNullOrWhiteSpace(stringList[5]))
              aeTpoContactInfo.BranchId = stringList[5];
            aeTpoContacts.Add(aeTpoContactInfo);
          }
        }
      }
      return (IEnumerable<AeTpoContactInfo>) aeTpoContacts;
    }

    private static int GetExternalOrgUrlId(string siteId)
    {
      ExternalOrgURL externalOrgUrl = ((IEnumerable<ExternalOrgURL>) ExternalOrgManagementAccessor.GetExternalOrganizationURLs()).FirstOrDefault<ExternalOrgURL>((System.Func<ExternalOrgURL, bool>) (x => x.siteId == siteId));
      return externalOrgUrl == null ? -1 : externalOrgUrl.URLID;
    }
  }
}
