// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.OrganizationStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class OrganizationStore
  {
    private const string className = "OrganizationStore�";

    private OrganizationStore()
    {
    }

    public static int RootOrganizationID
    {
      get
      {
        string str = "rootOrgId";
        ClientContext current = ClientContext.GetCurrent();
        object rootOrganizationId1 = current.Cache.Get(str);
        if (rootOrganizationId1 != null)
          return (int) rootOrganizationId1;
        try
        {
          using (current.Cache.Lock(str, timeout: ServerGlobals.LockTimeoutDuringGetCache))
          {
            if (ServerGlobals.CacheRegetFromCache)
            {
              object rootOrganizationId2 = current.Cache.Get(str);
              if (rootOrganizationId2 != null)
                return (int) rootOrganizationId2;
            }
            int rootOrgId = OrganizationStore.getRootOrgID(current);
            current.Cache.Put(str, (object) rootOrgId);
            return rootOrgId;
          }
        }
        catch (TimeoutException ex)
        {
          try
          {
            TraceLog.WriteWarning(nameof (OrganizationStore), "Timeout expired while acquiring lock on OrganizationStore");
          }
          catch
          {
          }
          if (ServerGlobals.CacheRegetFromDB)
            return OrganizationStore.getRootOrgID(current);
          throw;
        }
        catch (ApplicationException ex)
        {
          if (ex.Message.IndexOf("timeout period expired") > 0 || ex.HResult == -2147023436)
          {
            try
            {
              TraceLog.WriteWarning(nameof (OrganizationStore), "Timeout expired while acquiring lock on OrganizationStore");
            }
            catch
            {
            }
            if (ServerGlobals.CacheRegetFromDB)
              return OrganizationStore.getRootOrgID(current);
            throw;
          }
          else
            throw;
        }
      }
    }

    public static Organization CheckOut(int orgId)
    {
      ICacheLock<OrgInfo> innerLock = ClientContext.GetCurrent().Cache.CheckOut<OrgInfo>(nameof (OrganizationStore), orgId.ToString(), (object) orgId);
      try
      {
        return new Organization(innerLock);
      }
      catch (Exception ex)
      {
        innerLock.UndoCheckout();
        Err.Reraise(nameof (OrganizationStore), ex);
        return (Organization) null;
      }
    }

    public static Organization GetLatestVersion(int orgId)
    {
      OrgInfo orgInfo = ClientContext.GetCurrent().Cache.Get<OrgInfo>(nameof (OrganizationStore), orgId.ToString(), (Func<OrgInfo>) (() => Organization.LoadOrganization(orgId)), CacheSetting.Low);
      try
      {
        return new Organization(orgInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationStore), ex);
        return (Organization) null;
      }
    }

    public static bool OrganizationExists(int orgId)
    {
      try
      {
        return Organization.OrganizationExists(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationStore), ex);
        return false;
      }
    }

    [PgReady]
    public static int[] GetDescendentsOfOrgUsingLOComp(int orgId)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder((IClientContext) current);
        pgDbQueryBuilder.AppendLine("SELECT oid FROM [org_chart] WHERE [parent] = " + (object) orgId + " AND [inheritParentCompPlan] = 1 AND org_type is null ");
        return pgDbQueryBuilder.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, int>((System.Func<DataRow, int>) (row => (int) row[0])).ToArray<int>();
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid FROM [org_chart] WHERE [parent] = " + (object) orgId + " AND [inheritParentCompPlan] = 'True' AND org_type is null ");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return (int[]) null;
      List<int> intList = new List<int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        intList.Add((int) dataRowCollection[index][0]);
      return intList?.ToArray();
    }

    [PgReady]
    public static int[] GetDescendentsOfOrgUsingCCSiteParenInfo(int orgId)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder((IClientContext) current);
        pgDbQueryBuilder.AppendLine("SELECT oid FROM [org_chart] WHERE [parent] = " + (object) orgId + " AND [inheritParentccsiteid] = 'True'" ?? "");
        return pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, int>((System.Func<DataRow, int>) (row => (int) row[0])).ToArray<int>();
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid FROM [org_chart] WHERE [parent] = " + (object) orgId + " AND [inheritParentccsiteid] = 'True'" ?? "");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return (int[]) null;
      List<int> intList = new List<int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        intList.Add((int) dataRowCollection[index][0]);
      return intList?.ToArray();
    }

    public static int[] GetDescendentsOfOrg(int orgId)
    {
      return OrganizationStore.getDescendentsOfOrgFromSQL(orgId);
    }

    public static HashSet<int> GetValidOrganizationIds(int orgId, HashSet<int> oidList)
    {
      string str = string.Join<int>(",", (IEnumerable<int>) oidList);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT descendent FROM org_descendents WHERE oid = {0} AND descendent in ({1})", (object) orgId, (object) str));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      HashSet<int> validOrganizationIds = new HashSet<int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        validOrganizationIds.Add((int) dataRowCollection[index]["descendent"]);
      validOrganizationIds.Add(orgId);
      return validOrganizationIds;
    }

    public static int[] GetPaginatedRecordsForOrgWithDescendants(
      int orgId,
      int offset,
      int endRecordNumber,
      out int totalRecords,
      int parentId = -1)
    {
      totalRecords = 0;
      if (OrganizationStore.IsDescendentOfOrg(orgId, parentId))
        orgId = parentId;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("select oc.oid from org_chart oc inner join (select oid from org_chart where oid = " + (object) orgId + " union all select descendent from org_descendents where oid = " + (object) orgId + ") od");
      stringBuilder.Append(" on od.oid = oc.oid and oc.org_type is null");
      List<SortColumn> sortColumnList = new List<SortColumn>()
      {
        new SortColumn("oid", SortOrder.Ascending)
      };
      DataTable paginatedRecords = new DbQueryBuilder().GetPaginatedRecords(stringBuilder.ToString(), offset, endRecordNumber, sortColumnList);
      if (paginatedRecords == null)
        return (int[]) null;
      int[] orgWithDescendants = new int[paginatedRecords.Rows.Count];
      for (int index = 0; index < paginatedRecords.Rows.Count; ++index)
        orgWithDescendants[index] = (int) paginatedRecords.Rows[index][0];
      totalRecords = orgWithDescendants.Length != 0 ? Convert.ToInt32(paginatedRecords.Rows[0]["TotalRowCount"]) : 0;
      return orgWithDescendants;
    }

    [PgReady]
    public static OrgInfo[] GetOrganizationsByName(string orgName)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new string[1]
        {
          "oid"
        }, new DbValue("org_name", (object) orgName));
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.None);
        OrgInfo[] organizationsByName = new OrgInfo[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          using (Organization latestVersion = OrganizationStore.GetLatestVersion((int) dataRowCollection[index][0]))
            organizationsByName[index] = latestVersion.GetOrganizationInfo();
        }
        return organizationsByName;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new string[1]
      {
        "oid"
      }, new DbValue("org_name", (object) orgName));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute(DbTransactionType.None);
      OrgInfo[] organizationsByName1 = new OrgInfo[dataRowCollection1.Count];
      for (int index = 0; index < dataRowCollection1.Count; ++index)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion((int) dataRowCollection1[index][0]))
          organizationsByName1[index] = latestVersion.GetOrganizationInfo();
      }
      return organizationsByName1;
    }

    private static int[] getDescendentsOfOrgFromSQL(int orgId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_descendents"), new string[1]
      {
        "descendent"
      }, new DbValue("oid", (object) orgId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      int[] descendentsOfOrgFromSql = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        descendentsOfOrgFromSql[index] = (int) dataRowCollection[index][0];
      return descendentsOfOrgFromSql;
    }

    public static bool IsDescendentOfOrg(int parentOrgId, int childOrgId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select 1 from org_descendents where oid = " + (object) parentOrgId + " and descendent = " + (object) childOrgId);
      return dbQueryBuilder.Execute(DbTransactionType.None).Count > 0;
    }

    public static int[] GetAncestorsOfOrg(int orgId, bool inclusive)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select oid from org_descendents where descendent = " + (object) orgId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ArrayList arrayList = new ArrayList();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        arrayList.Add((object) EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["oid"]));
      if (inclusive)
        arrayList.Add((object) orgId);
      return (int[]) arrayList.ToArray(typeof (int));
    }

    public static int[] GetAncestorsOfUser(string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select org_id from users where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
      object orgId = dbQueryBuilder.ExecuteScalar();
      return orgId == null ? (int[]) null : OrganizationStore.GetAncestorsOfOrg((int) orgId, true);
    }

    public static OrgInfo GetOrganizationForClosingVendorInformation(int orgId)
    {
      OrgInfo vendorInformation = (OrgInfo) null;
      using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
      {
        vendorInformation = latestVersion.GetOrganizationInfo();
        OrgInfo avaliableOrganization = OrganizationStore.GetFirstAvaliableOrganization(orgId, true);
        if (avaliableOrganization != null)
        {
          if (string.IsNullOrEmpty(vendorInformation.CompanyName))
            vendorInformation.CompanyName = avaliableOrganization.CompanyName;
          vendorInformation.CompanyAddress.Street1 = avaliableOrganization.CompanyAddress.Street1;
          vendorInformation.CompanyAddress.Street2 = avaliableOrganization.CompanyAddress.Street2;
          vendorInformation.CompanyAddress.City = avaliableOrganization.CompanyAddress.City;
          vendorInformation.CompanyAddress.State = avaliableOrganization.CompanyAddress.State;
          vendorInformation.CompanyAddress.Zip = avaliableOrganization.CompanyAddress.Zip;
        }
        OrgInfo organizationWithNmls = OrganizationStore.GetFirstOrganizationWithNMLS(orgId);
        if (organizationWithNmls != null)
          vendorInformation.NMLSCode = organizationWithNmls.NMLSCode;
        OrgInfo withStateLicensing = OrganizationStore.GetFirstOrganizationWithStateLicensing(orgId);
        if (withStateLicensing != null)
          vendorInformation.OrgBranchLicensing = withStateLicensing.OrgBranchLicensing;
      }
      return vendorInformation;
    }

    internal static OrgInfo GetFirstAvaliableOrganizationForLOComp(int orgId)
    {
      int orgId1 = orgId;
      string str = string.Empty;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (str == string.Empty && organizationInfo.OrgName != string.Empty)
            str = organizationInfo.OrgName;
          if (organizationInfo.OrgCode != string.Empty && organizationInfo.OrgCode != null || organizationInfo.CompanyName != string.Empty && organizationInfo.CompanyName != null)
          {
            organizationInfo.OrgName = str;
            return organizationInfo;
          }
          if (orgId1 == organizationInfo.Parent)
          {
            CompanyInfo companyInfo = Company.GetCompanyInfo();
            if (companyInfo == null)
              return (OrgInfo) null;
            return new OrgInfo()
            {
              CompanyName = companyInfo.Name,
              CompanyAddress = {
                Street1 = companyInfo.Address,
                City = companyInfo.City,
                State = companyInfo.State,
                Zip = companyInfo.Zip
              },
              OrgName = str
            };
          }
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstAvaliableOrganization(int orgId)
    {
      return OrganizationStore.GetFirstAvaliableOrganization(orgId, false);
    }

    public static OrgInfo GetFirstAvaliableOrganization(int orgId, bool getInstalledInfoIfNotFound)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (organizationInfo.OrgCode != string.Empty && organizationInfo.OrgCode != null || organizationInfo.CompanyName != string.Empty && organizationInfo.CompanyName != null)
          {
            OrgInfo withStateLicensing = OrganizationStore.GetFirstOrganizationWithStateLicensing(orgId);
            organizationInfo.OrgBranchLicensing = (BranchExtLicensing) withStateLicensing.OrgBranchLicensing.Clone();
            return organizationInfo;
          }
          if (orgId1 == organizationInfo.Parent)
          {
            if (!getInstalledInfoIfNotFound)
              return (OrgInfo) null;
            CompanyInfo companyInfo = Company.GetCompanyInfo();
            if (companyInfo == null)
              return (OrgInfo) null;
            OrgInfo avaliableOrganization = new OrgInfo();
            avaliableOrganization.CompanyName = companyInfo.Name;
            avaliableOrganization.CompanyAddress.Street1 = companyInfo.Address;
            avaliableOrganization.CompanyAddress.City = companyInfo.City;
            avaliableOrganization.CompanyAddress.State = companyInfo.State;
            avaliableOrganization.CompanyAddress.Zip = companyInfo.Zip;
            avaliableOrganization.CompanyPhone = companyInfo.Phone;
            avaliableOrganization.CompanyFax = companyInfo.Fax;
            avaliableOrganization.DBAName1 = companyInfo.DBAName1;
            avaliableOrganization.DBAName2 = companyInfo.DBAName2;
            avaliableOrganization.DBAName3 = companyInfo.DBAName3;
            avaliableOrganization.DBAName4 = companyInfo.DBAName4;
            OrgInfo withStateLicensing = OrganizationStore.GetFirstOrganizationWithStateLicensing(orgId);
            avaliableOrganization.OrgBranchLicensing = (BranchExtLicensing) withStateLicensing.OrgBranchLicensing.Clone();
            return avaliableOrganization;
          }
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithNMLS(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (organizationInfo == null)
            return (OrgInfo) null;
          if (organizationInfo.NMLSCode != string.Empty && organizationInfo.NMLSCode != null)
            return organizationInfo;
          if (orgId1 == organizationInfo.Parent)
            return (OrgInfo) null;
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithLOSearch(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (organizationInfo == null)
            return (OrgInfo) null;
          if (organizationInfo.ShowOrgInLOSearch)
            return organizationInfo;
          if (orgId1 == organizationInfo.Parent)
            return (OrgInfo) null;
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithCCSiteId(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          CCSiteInfo ccSiteInfo = CCSiteInfoAccessor.getCCSiteInfo(orgId1);
          if (ccSiteInfo != null && ccSiteInfo.SiteId.Length > 0)
            return organizationInfo;
          if (organizationInfo == null || orgId1 == organizationInfo.Parent)
            return (OrgInfo) null;
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithLEI(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (organizationInfo == null)
            return (OrgInfo) null;
          if (!string.IsNullOrEmpty(Convert.ToString(organizationInfo.HMDAProfileId)) && organizationInfo.HMDAProfileId > 0)
            return organizationInfo;
          if (orgId1 == organizationInfo.Parent)
            return (OrgInfo) null;
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithMERSMIN(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (organizationInfo.MERSMINCode != string.Empty && organizationInfo.MERSMINCode != null || orgId1 == organizationInfo.Parent)
            return organizationInfo;
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithONRP(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (organizationInfo == null)
            return (OrgInfo) null;
          if (!organizationInfo.ONRPRetailBranchSettings.UseParentInfo)
            return organizationInfo;
          if (orgId1 == organizationInfo.Parent)
            return (OrgInfo) null;
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithStateLicensing(
      string companyName,
      string streetAddress,
      string loID)
    {
      if (!string.IsNullOrEmpty(loID))
      {
        UserInfo userById = User.GetUserById(loID);
        if (userById != (UserInfo) null)
        {
          OrgInfo withStateLicensing = OrganizationStore.GetFirstOrganizationWithStateLicensing(userById.OrgId);
          if (withStateLicensing != null)
            return withStateLicensing;
        }
      }
      OrgInfo[] organizationsByName = OrganizationStore.GetOrganizationsByName(companyName);
      if (organizationsByName == null || organizationsByName.Length == 0)
      {
        CompanyInfo companyInfo = Company.GetCompanyInfo();
        if (companyInfo == null)
          return (OrgInfo) null;
        if (string.Compare(companyInfo.Name, companyName, true) != 0)
          return (OrgInfo) null;
        return new OrgInfo()
        {
          OrgBranchLicensing = companyInfo.StateBranchLicensing
        };
      }
      OrgInfo orgInfo = organizationsByName[0];
      if (!string.IsNullOrEmpty(streetAddress))
      {
        for (int index = 0; index < organizationsByName.Length; ++index)
        {
          if (string.Compare(streetAddress, organizationsByName[index].CompanyAddress.Street1 + (!string.IsNullOrEmpty(organizationsByName[index].CompanyAddress.Street2) ? " " + organizationsByName[index].CompanyAddress.Street2 : ""), true) == 0)
          {
            orgInfo = organizationsByName[index];
            break;
          }
        }
      }
      return OrganizationStore.GetFirstOrganizationWithStateLicensing(orgInfo.Oid);
    }

    public static OrgInfo GetFirstOrganizationWithStateLicensing(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          if (!latestVersion.Exists)
          {
            Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            return (OrgInfo) null;
          }
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (organizationInfo.OrgBranchLicensing.LenderType != null)
            return organizationInfo;
          if (orgId1 == organizationInfo.Parent)
          {
            CompanyInfo companyInfo = Company.GetCompanyInfo();
            if (companyInfo == null)
              return (OrgInfo) null;
            return new OrgInfo()
            {
              OrgBranchLicensing = companyInfo.StateBranchLicensing
            };
          }
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static OrgInfo GetFirstOrganizationWithLOComp(int orgId)
    {
      using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
      {
        if (latestVersion.Exists)
          return latestVersion.GetOrganizationInfo();
        Err.Raise(TraceLevel.Warning, nameof (OrganizationStore), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
        return (OrgInfo) null;
      }
    }

    public static bool GetParentOrganizationSsoSetting(int orgId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("WITH cte AS");
      dbQueryBuilder.AppendLine("(");
      dbQueryBuilder.AppendLine("select oid, org_name, parent, ssoOnly, inheritParentSSO from org_chart where oid =" + (object) orgId);
      dbQueryBuilder.AppendLine("UNION ALL");
      dbQueryBuilder.AppendLine("select i.oid, i.org_name, i.parent, i.ssoOnly, i.inheritParentSSO  from org_chart i");
      dbQueryBuilder.AppendLine("inner JOIN cte c ON c.parent = i.oid");
      dbQueryBuilder.AppendLine("where c.oid <> 0");
      dbQueryBuilder.AppendLine(")");
      dbQueryBuilder.AppendLine("SELECT top 1.ssoOnly FROM cte where inheritParentSSO = 0");
      object obj = dbQueryBuilder.ExecuteScalar();
      return obj != null && DBNull.Value != obj && (bool) obj;
    }

    public static OrgInfo GetFirstOrganizationForSSO(int orgId)
    {
      int orgId1 = orgId;
      while (true)
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId1))
        {
          OrgInfo organizationInfo = latestVersion.GetOrganizationInfo();
          if (!organizationInfo.SSOSettings.UseParentInfo || organizationInfo.Oid == organizationInfo.Parent)
            return organizationInfo;
          orgId1 = organizationInfo.Parent;
        }
      }
    }

    public static List<Tuple<string, string, string, string>> GetChildOrganizationAndUsers(
      int orgId,
      int offset,
      int endRecordNumber,
      out int totalRecords,
      string orgType = "Both�",
      bool isRecursive = false)
    {
      totalRecords = 0;
      StringBuilder stringBuilder = new StringBuilder();
      if (isRecursive)
      {
        if (orgType.Equals("organization", StringComparison.OrdinalIgnoreCase) || orgType.Equals("Both", StringComparison.OrdinalIgnoreCase))
          stringBuilder.Append("select CAST(descendent as varchar(10)) as 'id', org_name as 'name', oc.parent as 'parentId', 'Organization' as 'Type' from org_descendents od inner join org_chart oc on od.descendent = oc.oid and oc.org_type is null and od.oid = " + (object) orgId);
        if (orgType.Equals("user", StringComparison.OrdinalIgnoreCase) || orgType.Equals("Both", StringComparison.OrdinalIgnoreCase))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(" union all ");
          stringBuilder.Append("select userid as 'id' , first_name + ' ' + last_name as 'name' ,org_id as 'parentId' , 'User' as 'Type' from users u ");
          stringBuilder.Append("inner join org_chart oc on u.org_id = oc.oid and oc.org_type is null where u.org_id = " + (object) orgId + " or u.org_id in (");
          stringBuilder.Append(" select descendent from org_descendents where oid = " + (object) orgId + ")");
        }
      }
      else
      {
        if (orgType.Equals("organization", StringComparison.OrdinalIgnoreCase) || orgType.Equals("Both", StringComparison.OrdinalIgnoreCase))
          stringBuilder.Append("select CAST(oc.oid as varchar(10)) as 'id', org_name as 'name',oc.parent as 'parentId', 'Organization' as 'Type' from org_chart oc where oc.parent = " + (object) orgId + "and oc.org_type is null and oc.oid != " + (object) orgId);
        if (orgType.Equals("user", StringComparison.OrdinalIgnoreCase) || orgType.Equals("Both", StringComparison.OrdinalIgnoreCase))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(" union all ");
          stringBuilder.Append("select userid as 'id', first_name + ' ' + last_name as 'name' , org_id as 'parentId','User' as 'Type' from users u ");
          stringBuilder.Append("inner join org_chart oc on u.org_id = oc.oid and oc.org_type is null and org_id = " + (object) orgId);
        }
      }
      DataTable paginatedRecords = new DbQueryBuilder().GetPaginatedRecords(stringBuilder.ToString(), offset, endRecordNumber, (List<SortColumn>) null);
      if (paginatedRecords == null)
        return (List<Tuple<string, string, string, string>>) null;
      DataRowCollection rows = paginatedRecords.Rows;
      List<Tuple<string, string, string, string>> organizationAndUsers = new List<Tuple<string, string, string, string>>(rows.Count);
      for (int index = 0; index < rows.Count; ++index)
      {
        Tuple<string, string, string, string> tuple = new Tuple<string, string, string, string>(EllieMae.EMLite.DataAccess.SQL.Decode(rows[index]["id"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(rows[index]["name"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(rows[index]["type"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(rows[index]["parentId"], (object) "").ToString());
        organizationAndUsers.Add(tuple);
      }
      totalRecords = rows.Count > 0 ? Convert.ToInt32(paginatedRecords.Rows[0]["TotalRowCount"]) : 0;
      return organizationAndUsers;
    }

    [PgReady]
    private static int getRootOrgID(ClientContext context)
    {
      if (context.Settings.DbServerType == DbServerType.Postgres)
      {
        DbValueList keys = new DbValueList();
        keys.Add(new DbValue("oid", (object) "parent", (IDbEncoder) DbEncoding.None));
        keys.Add("org_type", (object) DBNull.Value);
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder((IClientContext) context);
        pgDbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new string[1]
        {
          "oid"
        }, keys);
        return EllieMae.EMLite.DataAccess.SQL.DecodeInt(pgDbQueryBuilder.ExecuteScalar());
      }
      DbValueList keys1 = new DbValueList();
      keys1.Add(new DbValue("oid", (object) "parent", (IDbEncoder) DbEncoding.None));
      keys1.Add("org_type", (object) DBNull.Value);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder((IClientContext) context);
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new string[1]
      {
        "oid"
      }, keys1);
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    private static void preloadCache()
    {
      ClientContext current = ClientContext.GetCurrent();
      Organization latestVersion = OrganizationStore.GetLatestVersion(OrganizationStore.getRootOrgID(current));
      OrganizationStore.preloadChildren(current, latestVersion.OrganizationID);
    }

    [PgReady]
    private static void preloadChildren(ClientContext context, int orgId)
    {
      if (context.Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder((IClientContext) context);
        pgDbQueryBuilder.Append("select oid from org_chart where (parent = " + (object) orgId + ") and (oid <> " + (object) orgId + ")");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          Organization latestVersion = OrganizationStore.GetLatestVersion((int) dataRowCollection[index]["oid"]);
          OrganizationStore.preloadChildren(context, latestVersion.OrganizationID);
        }
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder((IClientContext) context);
        dbQueryBuilder.Append("select oid from org_chart where (parent = " + (object) orgId + ") and (oid <> " + (object) orgId + ")");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          Organization latestVersion = OrganizationStore.GetLatestVersion((int) dataRowCollection[index]["oid"]);
          OrganizationStore.preloadChildren(context, latestVersion.OrganizationID);
        }
      }
    }

    private static void onPreloadCache(object sender, EventArgs e)
    {
      if (ClientContext.GetCurrent().Cache.CacheSetting < CacheSetting.Low)
        return;
      OrganizationStore.preloadCache();
    }
  }
}
