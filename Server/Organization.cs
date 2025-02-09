// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Organization
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class Organization : IDisposable
  {
    private const string className = "Organization�";
    public static readonly string SyncRootKey = "OrganizationSyncRoot";
    private ICacheLock<OrgInfo> innerLock;
    private OrgInfo info;
    private int orgId;

    public Organization(ICacheLock<OrgInfo> innerLock)
    {
      this.innerLock = innerLock;
      this.orgId = (int) innerLock.Identifier;
      if (innerLock.Value == null)
        this.initializeOrganization();
      this.info = innerLock.Value;
    }

    public Organization(OrgInfo orgInfo)
    {
      this.info = orgInfo;
      if (orgInfo == null)
        return;
      this.orgId = orgInfo.Oid;
    }

    public bool Exists => this.info != null;

    public int OrganizationID => this.orgId;

    public string Name
    {
      get
      {
        this.validateExists();
        return this.info.OrgName;
      }
      set
      {
        this.validateInstance();
        if ((value ?? "") == "")
          Err.Raise(TraceLevel.Error, nameof (Organization), new ServerException("Organization name cannot be blank or null"));
        this.info.OrgName = value;
      }
    }

    public string Description
    {
      get
      {
        this.validateExists();
        return this.info.Description;
      }
      set
      {
        this.validateInstance();
        this.info.Description = value ?? "";
      }
    }

    public int[] Children
    {
      get
      {
        this.validateExists();
        return this.info.Children;
      }
    }

    public int ParentOrganizationID
    {
      get
      {
        this.validateExists();
        return this.info.Parent;
      }
    }

    public OrgInfo GetOrganizationInfo()
    {
      this.validateExists();
      return (OrgInfo) this.info.Clone();
    }

    public int CreateSuborganization(OrgInfo childInfo)
    {
      this.validateInstance();
      int orgInDatabase = Organization.createOrgInDatabase(childInfo, this.info.Oid);
      this.addChild(orgInDatabase);
      this.innerLock.CheckIn(this.info, true);
      return orgInDatabase;
    }

    public void Delete(Organization parentOrg)
    {
      this.validateInstance();
      parentOrg.validateInstance();
      if (parentOrg.OrganizationID != this.info.Parent)
        Err.Raise(TraceLevel.Error, nameof (Organization), new ServerException("Organization is not a child of the specified parent"));
      Organization.deleteOrgFromDatabase(this.info.Oid);
      parentOrg.removeChild(this.info.Oid);
      this.innerLock.CheckIn((OrgInfo) null, true);
      parentOrg.CheckIn(true);
    }

    public void CreateSettingsRptJob(SettingsRptJobInfo jobinfo)
    {
      Organization.createSettingsRptJob(jobinfo);
    }

    public void Move(Organization priorOrg, Organization newOrg)
    {
      this.validateInstance();
      priorOrg.validateInstance();
      newOrg.validateInstance();
      if (priorOrg.OrganizationID != this.info.Parent)
        Err.Raise(TraceLevel.Error, nameof (Organization), new ServerException("Organization is not a child of the specified parent"));
      if (priorOrg.OrganizationID == newOrg.OrganizationID)
        return;
      this.info.LOCompHistoryList.UseParentInfo = false;
      if (this.info.SSOSettings.UseParentInfo)
        this.info.SSOSettings.LoginAccess = newOrg.info.SSOSettings.LoginAccess;
      OrgInfo database = Organization.updateOrgToDatabase(new OrgInfo(this.info.Oid, this.info.OrgName, this.info.Description, newOrg.OrganizationID, this.info.OrgCode, this.info.CompanyName, this.info.CompanyAddress, this.info.CompanyPhone, this.info.CompanyFax, this.info.Children, this.info.NMLSCode, this.info.MERSMINCode, new string[4]
      {
        this.info.DBAName1,
        this.info.DBAName2,
        this.info.DBAName3,
        this.info.DBAName4
      }, this.info.OrgBranchLicensing, this.info.LOCompHistoryList, this.info.ONRPRetailBranchSettings, this.info.CCSiteSettings, this.info.SSOSettings, (this.info.ShowOrgInLOSearch ? 1 : 0) != 0, this.info.LOSearchOrgName, this.info.HMDAProfileId));
      priorOrg.removeChild(this.info.Oid);
      newOrg.addChild(this.info.Oid);
      if (this.info.Children != null)
        this.removeLOCompParentInfo(this.info.Children);
      this.info = database;
      priorOrg.CheckIn(true);
      newOrg.CheckIn(true);
      this.innerLock.CheckIn(database, true);
      if (this.info.SSOSettings == null)
        return;
      this.UpdateSSOInheritedOrgsSSOSetting(this.info.Oid, newOrg.info.SSOSettings.LoginAccess);
    }

    public void UpdateSSOInheritedOrgsSSOSetting(int orgId, bool IsSSOOnly)
    {
      foreach (int ssoInheritedOrgId in this.GetSSOInheritedOrgIds(orgId))
      {
        using (Organization organization = OrganizationStore.CheckOut(ssoInheritedOrgId))
        {
          organization.info.SSOSettings.LoginAccess = IsSSOOnly;
          organization.CheckIn();
        }
      }
    }

    private List<int> GetSSOInheritedOrgIds(int orgid)
    {
      List<int> ssoInheritedOrgIds = new List<int>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("WITH cte AS");
      dbQueryBuilder.AppendLine("(select oid, org_name, parent, ssoOnly, inheritParentSSO");
      dbQueryBuilder.AppendLine("from org_chart where parent =" + (object) orgid + " and inheritParentSSO =1");
      dbQueryBuilder.AppendLine("UNION ALL");
      dbQueryBuilder.AppendLine("select i.oid, i.org_name, i.parent, i.ssoOnly, i.inheritParentSSO from org_chart i inner JOIN cte c ON i.parent = c.oid");
      dbQueryBuilder.AppendLine("where c.oid <> 0 and i.inheritParentSSO = 1)");
      dbQueryBuilder.AppendLine("SELECT oid FROM cte");
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.ExecuteTableQuery().Rows)
        ssoInheritedOrgIds.Add((int) row[0]);
      return ssoInheritedOrgIds;
    }

    private void removeLOCompParentInfo(int[] orgChildren)
    {
      string str = string.Empty;
      if (orgChildren != null && orgChildren.Length != 0)
      {
        for (int index = 0; index < orgChildren.Length; ++index)
          str = str + (str != string.Empty ? "," : "") + orgChildren[index].ToString();
      }
      if (str == string.Empty)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("UPDATE [org_chart] SET inheritParentCompPlan = 'False' WHERE oid IN (" + str + ") AND inheritParentCompPlan = 'True'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public void CheckIn() => this.CheckIn(false);

    public void CheckIn(bool keepCheckedOut) => this.CheckIn(this.info, keepCheckedOut);

    public void CheckIn(OrgInfo newInfo) => this.CheckIn(newInfo, false);

    public void CheckIn(OrgInfo newInfo, bool keepCheckedOut)
    {
      this.validateInstance();
      if (newInfo == null)
        Err.Raise(TraceLevel.Error, nameof (Organization), new ServerException("Cannot set Organization Information to null"));
      if (newInfo.Oid != this.info.Oid)
        Err.Raise(TraceLevel.Error, nameof (Organization), new ServerException("Invalid organization ID or Parent ID"));
      string emailSignature = newInfo.EmailSignature;
      newInfo = new OrgInfo(this.info.Oid, newInfo.OrgName, newInfo.Description, this.info.Parent, newInfo.OrgCode, newInfo.CompanyName, newInfo.CompanyAddress, newInfo.CompanyPhone, newInfo.CompanyFax, this.info.Children, newInfo.NMLSCode, newInfo.MERSMINCode, new string[4]
      {
        newInfo.DBAName1,
        newInfo.DBAName2,
        newInfo.DBAName3,
        newInfo.DBAName4
      }, (BranchExtLicensing) newInfo.OrgBranchLicensing.Clone(), newInfo.LOCompHistoryList != null ? (LoanCompHistoryList) newInfo.LOCompHistoryList.Clone() : (LoanCompHistoryList) null, newInfo.ONRPRetailBranchSettings.Clone((IONRPRuleHandler) null, new ONRPBaseRule(), (LockDeskGlobalSettings) null), newInfo.CCSiteSettings, newInfo.SSOSettings, (newInfo.ShowOrgInLOSearch ? 1 : 0) != 0, newInfo.LOSearchOrgName, newInfo.HMDAProfileId);
      if (!string.IsNullOrEmpty(emailSignature))
        newInfo.EmailSignature = emailSignature;
      ONRPEntitySettings orgOnrpInfo = Organization.GetOrgOnrpInfo(this.info.Oid);
      bool flag = newInfo.ONRPRetailBranchSettings.NeedClearAccruedAmount(orgOnrpInfo);
      newInfo = Organization.updateOrgToDatabase(newInfo);
      if (flag)
        OverNightRateProtection.DeleteOnrpPeriodAccruedAmount(LoanChannel.BankedRetail, this.info.Oid.ToString(), false);
      this.info = newInfo;
      this.innerLock.CheckIn(newInfo, keepCheckedOut);
      if (keepCheckedOut)
        return;
      this.Dispose();
    }

    public void UndoCheckout()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.UndoCheckout();
      this.Dispose();
    }

    public void Dispose()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.Dispose();
      this.innerLock = (ICacheLock<OrgInfo>) null;
    }

    [PgReady]
    public static OrgInfo[] GetAllOrganizationInfo()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new DbValue("org_type", (object) DBNull.Value));
        DataTable dataTable1 = pgDbQueryBuilder.ExecuteTableQuery();
        Hashtable hashtable = new Hashtable();
        for (int index = 0; index < dataTable1.Rows.Count; ++index)
        {
          DataRow row = dataTable1.Rows[index];
          int num = (int) row["oid"];
          int key = (int) row["parent"];
          if (key != num)
          {
            if (!hashtable.ContainsKey((object) key))
              hashtable.Add((object) key, (object) new ArrayList());
            ((ArrayList) hashtable[(object) key]).Add((object) num);
          }
        }
        OrgInfo[] organizationInfo = new OrgInfo[dataTable1.Rows.Count];
        int[] orgIds = new int[dataTable1.Rows.Count];
        Dictionary<OrgInfo, bool> dictionary = new Dictionary<OrgInfo, bool>();
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_StateLicensing"));
        DataTable dataTable2 = pgDbQueryBuilder.ExecuteTableQuery();
        DataTable historyDataTable = LOCompAccessor.GetComPlanHistoryDataTable(false, false, false, (string) null);
        for (int index = 0; index < dataTable1.Rows.Count; ++index)
        {
          DataRow row = dataTable1.Rows[index];
          int key = (int) row["oid"];
          int[] childIds = hashtable.ContainsKey((object) key) ? (int[]) ((ArrayList) hashtable[(object) key]).ToArray(typeof (int)) : new int[0];
          organizationInfo[index] = Organization.dataRowToOrgInfo(row, childIds, dataTable2?.Select("orgid = " + (object) key), historyDataTable?.Select("oid = " + (object) key));
          orgIds[index] = organizationInfo[index].Oid;
          dictionary[organizationInfo[index]] = organizationInfo[index].CCSiteSettings.UseParentInfo;
        }
        Dictionary<int, CCSiteInfo> ccSiteInfo = CCSiteInfoAccessor.getCCSiteInfo(orgIds);
        foreach (OrgInfo key in organizationInfo)
        {
          key.CCSiteSettings = ccSiteInfo == null || !ccSiteInfo.ContainsKey(key.Oid) ? new CCSiteInfo() : ccSiteInfo[key.Oid];
          key.CCSiteSettings.UseParentInfo = dictionary[key];
        }
        return organizationInfo;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new DbValue("org_type", (object) DBNull.Value));
      DataTable dataTable3 = dbQueryBuilder.ExecuteTableQuery();
      Hashtable hashtable1 = new Hashtable();
      for (int index = 0; index < dataTable3.Rows.Count; ++index)
      {
        DataRow row = dataTable3.Rows[index];
        int num = (int) row["oid"];
        int key = (int) row["parent"];
        if (key != num)
        {
          if (!hashtable1.ContainsKey((object) key))
            hashtable1.Add((object) key, (object) new ArrayList());
          ((ArrayList) hashtable1[(object) key]).Add((object) num);
        }
      }
      OrgInfo[] organizationInfo1 = new OrgInfo[dataTable3.Rows.Count];
      int[] orgIds1 = new int[dataTable3.Rows.Count];
      Dictionary<OrgInfo, bool> dictionary1 = new Dictionary<OrgInfo, bool>();
      dbQueryBuilder.Reset();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_StateLicensing"));
      DataTable dataTable4 = dbQueryBuilder.ExecuteTableQuery();
      DataTable historyDataTable1 = LOCompAccessor.GetComPlanHistoryDataTable(false, false, false, (string) null);
      for (int index = 0; index < dataTable3.Rows.Count; ++index)
      {
        DataRow row = dataTable3.Rows[index];
        int key = (int) row["oid"];
        int[] childIds = hashtable1.ContainsKey((object) key) ? (int[]) ((ArrayList) hashtable1[(object) key]).ToArray(typeof (int)) : new int[0];
        organizationInfo1[index] = Organization.dataRowToOrgInfo(row, childIds, dataTable4?.Select("orgid = " + (object) key), historyDataTable1?.Select("oid = " + (object) key));
        orgIds1[index] = organizationInfo1[index].Oid;
        dictionary1[organizationInfo1[index]] = organizationInfo1[index].CCSiteSettings.UseParentInfo;
      }
      Dictionary<int, CCSiteInfo> ccSiteInfo1 = CCSiteInfoAccessor.getCCSiteInfo(orgIds1);
      foreach (OrgInfo key in organizationInfo1)
      {
        key.CCSiteSettings = ccSiteInfo1 == null || !ccSiteInfo1.ContainsKey(key.Oid) ? new CCSiteInfo() : ccSiteInfo1[key.Oid];
        key.CCSiteSettings.UseParentInfo = dictionary1[key];
      }
      return organizationInfo1;
    }

    public static OrgInfo[] GetAllIntAndExtOrganizationInfo()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"));
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < dataTable1.Rows.Count; ++index)
      {
        DataRow row = dataTable1.Rows[index];
        int num = (int) row["oid"];
        int key = (int) row["parent"];
        if (key != num)
        {
          if (!hashtable.ContainsKey((object) key))
            hashtable.Add((object) key, (object) new ArrayList());
          ((ArrayList) hashtable[(object) key]).Add((object) num);
        }
      }
      OrgInfo[] organizationInfo = new OrgInfo[dataTable1.Rows.Count];
      int[] orgIds = new int[dataTable1.Rows.Count];
      Dictionary<OrgInfo, bool> dictionary = new Dictionary<OrgInfo, bool>();
      dbQueryBuilder.Reset();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_StateLicensing"));
      DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
      DataTable historyDataTable = LOCompAccessor.GetComPlanHistoryDataTable(false, false, false, (string) null);
      for (int index = 0; index < dataTable1.Rows.Count; ++index)
      {
        DataRow row = dataTable1.Rows[index];
        int key = (int) row["oid"];
        int[] childIds = hashtable.ContainsKey((object) key) ? (int[]) ((ArrayList) hashtable[(object) key]).ToArray(typeof (int)) : new int[0];
        organizationInfo[index] = Organization.dataRowToOrgInfo(row, childIds, dataTable2?.Select("orgid = " + (object) key), historyDataTable?.Select("oid = " + (object) key));
        orgIds[index] = organizationInfo[index].Oid;
        dictionary[organizationInfo[index]] = organizationInfo[index].CCSiteSettings.UseParentInfo;
      }
      Dictionary<int, CCSiteInfo> ccSiteInfo = CCSiteInfoAccessor.getCCSiteInfo(orgIds);
      foreach (OrgInfo key in organizationInfo)
      {
        key.CCSiteSettings = ccSiteInfo == null || !ccSiteInfo.ContainsKey(key.Oid) ? new CCSiteInfo() : ccSiteInfo[key.Oid];
        key.CCSiteSettings.UseParentInfo = dictionary[key];
      }
      return organizationInfo;
    }

    public static bool OrganizationExists(int oid)
    {
      ClientContext.GetCurrent();
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new string[1]
        {
          nameof (oid)
        }, new DbValue(nameof (oid), (object) oid));
        return dbQueryBuilder.ExecuteSetQuery().Tables[0].Rows.Count > 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Organization), ex);
        return false;
      }
    }

    private void addChild(int orgId)
    {
      this.validateInstance();
      this.info = new OrgInfo(this.info.Oid, this.info.OrgName, this.info.Description, this.info.Parent, this.info.OrgCode, this.info.CompanyName, this.info.CompanyAddress, this.info.CompanyPhone, this.info.CompanyFax, (int[]) new ArrayList((ICollection) this.info.Children)
      {
        (object) orgId
      }.ToArray(typeof (int)), this.info.NMLSCode, this.info.MERSMINCode, new string[4]
      {
        this.info.DBAName1,
        this.info.DBAName2,
        this.info.DBAName3,
        this.info.DBAName4
      }, (BranchExtLicensing) this.info.OrgBranchLicensing.Clone(), (LoanCompHistoryList) this.info.LOCompHistoryList.Clone(), this.info.ONRPRetailBranchSettings, this.info.CCSiteSettings, this.info.SSOSettings, (this.info.ShowOrgInLOSearch ? 1 : 0) != 0, this.info.LOSearchOrgName, this.info.HMDAProfileId);
    }

    private void removeChild(int orgId)
    {
      this.validateInstance();
      ArrayList arrayList = new ArrayList((ICollection) this.info.Children);
      arrayList.Remove((object) orgId);
      this.info = new OrgInfo(this.info.Oid, this.info.OrgName, this.info.Description, this.info.Parent, this.info.OrgCode, this.info.CompanyName, this.info.CompanyAddress, this.info.CompanyPhone, this.info.CompanyFax, (int[]) arrayList.ToArray(typeof (int)), this.info.NMLSCode, this.info.MERSMINCode, new string[4]
      {
        this.info.DBAName1,
        this.info.DBAName2,
        this.info.DBAName3,
        this.info.DBAName4
      }, (BranchExtLicensing) this.info.OrgBranchLicensing.Clone(), this.info.LOCompHistoryList, this.info.ONRPRetailBranchSettings, this.info.CCSiteSettings, this.info.SSOSettings, (this.info.ShowOrgInLOSearch ? 1 : 0) != 0, this.info.LOSearchOrgName, this.info.HMDAProfileId);
    }

    private void validateInstance() => this.validateInstance(true);

    private void validateInstance(bool requireExists)
    {
      if (this.innerLock == null)
        Err.Raise(TraceLevel.Error, nameof (Organization), new ServerException("Attempt to access disposed Organization object"));
      if (!requireExists)
        return;
      this.validateExists();
    }

    private void validateExists()
    {
      if (this.Exists)
        return;
      Err.Raise(TraceLevel.Error, nameof (Organization), new ServerException("Object does not exist"));
    }

    private void initializeOrganization()
    {
      this.innerLock.CheckIn(Organization.getOrgFromDatabase(this.orgId), true);
    }

    public static OrgInfo LoadOrganization(int oid) => Organization.getOrgFromDatabase(oid);

    public static OrgInfo[] LoadOrganizations(int[] oids) => Organization.getOrgsFromDatabase(oids);

    private static OrgInfo[] getOrgsFromDatabase(int[] oids)
    {
      try
      {
        OrgInfo[] source = new OrgInfo[oids.Length];
        if (oids.Length == 0)
          return source;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        Hashtable hashtable1 = new Hashtable();
        Hashtable hashtable2 = new Hashtable();
        Hashtable hashtable3 = new Hashtable();
        foreach (int oid in oids)
        {
          dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new DbValue("oid", (object) oid));
          dbQueryBuilder.AppendLine("select oid from org_chart where (parent = " + (object) oid + ") and (oid != " + (object) oid + ")");
          dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_StateLicensing"), new DbValue("orgid", (object) oid));
        }
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet.Tables.Count < 3)
          return (OrgInfo[]) null;
        int num = 0;
        List<string> stringList = new List<string>();
        foreach (int oid in oids)
        {
          stringList.Add(oid.ToString());
          int index = num * 3;
          if (dataSet.Tables[index].Rows.Count <= 0)
          {
            ++num;
          }
          else
          {
            hashtable1.Add((object) oid, (object) dataSet.Tables[index]);
            if (dataSet.Tables[index + 1].Rows.Count > 0)
              hashtable2.Add((object) oid, (object) dataSet.Tables[index + 1]);
            if (dataSet.Tables[index + 2].Rows.Count > 0)
              hashtable3.Add((object) oid, (object) dataSet.Tables[index + 2]);
            ++num;
          }
        }
        DataTable historyDataTable = LOCompAccessor.GetComPlanHistoryDataTable(false, false, false, stringList.ToArray());
        Dictionary<OrgInfo, bool> dictionary = new Dictionary<OrgInfo, bool>();
        int index1 = 0;
        foreach (int oid in oids)
        {
          if (hashtable1[(object) oid] != null)
          {
            DataRow[] licenseRows = (DataRow[]) null;
            if (hashtable3[(object) oid] != null)
              licenseRows = ((DataTable) hashtable3[(object) oid]).Select("orgid = " + (object) oid);
            DataRow row1 = ((DataTable) hashtable1[(object) oid]).Rows[0];
            ArrayList arrayList = new ArrayList();
            if (hashtable2[(object) oid] != null)
            {
              foreach (DataRow row2 in (InternalDataCollectionBase) ((DataTable) hashtable2[(object) oid]).Rows)
                arrayList.Add((object) (int) row2["oid"]);
            }
            TraceLog.WriteVerbose(nameof (Organization), "Organization \"" + row1["org_name"].ToString() + "\" (" + (object) oid + ") retrieved from database.");
            source[index1] = Organization.dataRowToOrgInfo(row1, (int[]) arrayList.ToArray(typeof (int)), licenseRows, historyDataTable?.Select("oid = " + (object) oid));
            dictionary[source[index1]] = source[index1].CCSiteSettings.UseParentInfo;
          }
          ++index1;
        }
        OrgInfo[] array = ((IEnumerable<OrgInfo>) source).Where<OrgInfo>((System.Func<OrgInfo, bool>) (a => a != null)).ToArray<OrgInfo>();
        Dictionary<int, CCSiteInfo> ccSiteInfo = CCSiteInfoAccessor.getCCSiteInfo(oids);
        foreach (OrgInfo key in array)
        {
          key.CCSiteSettings = ccSiteInfo == null || !ccSiteInfo.ContainsKey(key.Oid) ? new CCSiteInfo() : ccSiteInfo[key.Oid];
          key.CCSiteSettings.UseParentInfo = dictionary[key];
        }
        return array;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Organization), ex);
        return (OrgInfo[]) null;
      }
    }

    public static string GetOrgPath(int oid)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @schema varchar(50)");
        dbQueryBuilder.AppendLine("select @schema = schema_name()");
        dbQueryBuilder.AppendLine("declare @sql varchar(500)");
        dbQueryBuilder.AppendLine("select @sql = 'select [' + @schema + '].FN_GetOrgPath(" + (object) oid + ") as orgpath'");
        dbQueryBuilder.AppendLine("exec(@sql)");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        return dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0 ? dataSet.Tables[0].Rows[0]["orgpath"] as string : (string) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Organization), ex);
        return (string) null;
      }
    }

    [PgReady]
    private static OrgInfo getOrgFromDatabase(int oid)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        try
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new DbValue(nameof (oid), (object) oid));
          pgDbQueryBuilder.AppendLine("select oid from org_chart where (parent = " + (object) oid + ") and (oid != " + (object) oid + ")");
          DataSet dataSet = pgDbQueryBuilder.ExecuteSetQuery();
          if (dataSet.Tables.Count != 2 || dataSet.Tables[0].Rows.Count == 0)
            return (OrgInfo) null;
          pgDbQueryBuilder.Reset();
          pgDbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_StateLicensing"), new DbValue("orgid", (object) oid));
          DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
          DataRow[] licenseRows = (DataRow[]) null;
          if (dataTable != null)
            licenseRows = dataTable.Select("orgid = " + (object) oid);
          DataTable historyDataTable = LOCompAccessor.GetComPlanHistoryDataTable(false, false, false, oid.ToString());
          DataRow row1 = dataSet.Tables[0].Rows[0];
          ArrayList arrayList = new ArrayList();
          foreach (DataRow row2 in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
            arrayList.Add((object) (int) row2[nameof (oid)]);
          TraceLog.WriteVerbose(nameof (Organization), "Organization \"" + row1["org_name"].ToString() + "\" (" + (object) oid + ") retrieved from database.");
          OrgInfo orgInfo = Organization.dataRowToOrgInfo(row1, (int[]) arrayList.ToArray(typeof (int)), licenseRows, historyDataTable?.Select("oid = " + (object) oid));
          bool useParentInfo = orgInfo.CCSiteSettings.UseParentInfo;
          orgInfo.CCSiteSettings = CCSiteInfoAccessor.getCCSiteInfo(orgInfo.Oid);
          if (orgInfo.CCSiteSettings == null)
            orgInfo.CCSiteSettings = new CCSiteInfo();
          orgInfo.CCSiteSettings.UseParentInfo = useParentInfo;
          return orgInfo;
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (Organization), ex);
          return (OrgInfo) null;
        }
      }
      else
      {
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_chart"), new DbValue(nameof (oid), (object) oid));
          dbQueryBuilder.AppendLine("select oid from org_chart where (parent = " + (object) oid + ") and (oid != " + (object) oid + ")");
          DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
          if (dataSet.Tables.Count != 2 || dataSet.Tables[0].Rows.Count == 0)
            return (OrgInfo) null;
          dbQueryBuilder.Reset();
          dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("org_StateLicensing"), new DbValue("orgid", (object) oid));
          DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
          DataRow[] licenseRows = (DataRow[]) null;
          if (dataTable != null)
            licenseRows = dataTable.Select("orgid = " + (object) oid);
          DataTable historyDataTable = LOCompAccessor.GetComPlanHistoryDataTable(false, false, false, oid.ToString());
          DataRow row3 = dataSet.Tables[0].Rows[0];
          ArrayList arrayList = new ArrayList();
          foreach (DataRow row4 in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
            arrayList.Add((object) (int) row4[nameof (oid)]);
          TraceLog.WriteVerbose(nameof (Organization), "Organization \"" + row3["org_name"].ToString() + "\" (" + (object) oid + ") retrieved from database.");
          OrgInfo orgInfo = Organization.dataRowToOrgInfo(row3, (int[]) arrayList.ToArray(typeof (int)), licenseRows, historyDataTable?.Select("oid = " + (object) oid));
          bool useParentInfo = orgInfo.CCSiteSettings.UseParentInfo;
          orgInfo.CCSiteSettings = CCSiteInfoAccessor.getCCSiteInfo(orgInfo.Oid);
          if (orgInfo.CCSiteSettings == null)
            orgInfo.CCSiteSettings = new CCSiteInfo();
          orgInfo.CCSiteSettings.UseParentInfo = useParentInfo;
          return orgInfo;
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (Organization), ex);
          return (OrgInfo) null;
        }
      }
    }

    [PgReady]
    private static OrgInfo updateOrgToDatabase(OrgInfo info)
    {
      try
      {
        if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.Update(DbAccessManager.GetTable("org_chart"), Organization.createDbValueList(info), new DbValue("oid", (object) info.Oid));
          pgDbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
          Organization.createStateLicensing(info, info.Oid);
          LOCompAccessor.CreateHistoryCompPlansForOrg(info.LOCompHistoryList, info.Oid, false, false);
          CCSiteInfoAccessor.updateCCSiteInfo(info.CCSiteSettings, info.Oid);
          TraceLog.WriteVerbose(nameof (Organization), "Organization \"" + info.OrgName + "\" (" + (object) info.Oid + ") saved to database.");
          return Organization.getOrgFromDatabase(info.Oid);
        }
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Update(DbAccessManager.GetTable("org_chart"), Organization.createDbValueList(info), new DbValue("oid", (object) info.Oid));
        dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
        Organization.createStateLicensing(info, info.Oid);
        LOCompAccessor.CreateHistoryCompPlansForOrg(info.LOCompHistoryList, info.Oid, false, false);
        CCSiteInfoAccessor.updateCCSiteInfo(info.CCSiteSettings, info.Oid);
        TraceLog.WriteVerbose(nameof (Organization), "Organization \"" + info.OrgName + "\" (" + (object) info.Oid + ") saved to database.");
        return Organization.getOrgFromDatabase(info.Oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Organization), ex);
        return (OrgInfo) null;
      }
    }

    private static void createSettingsRptJob(SettingsRptJobInfo jobinfo)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "INSERT INTO [ReportQueue] (jobtype, ID, reportname, status, createdby, createdate, description) VALUES(" + SQL.Encode((object) (int) jobinfo.Type) + "," + SQL.Encode((object) jobinfo.ID) + "," + SQL.Encode((object) jobinfo.ReportName) + "," + SQL.Encode((object) (int) jobinfo.Status) + "," + SQL.Encode((object) jobinfo.CreatedBy) + "," + SQL.Encode((object) jobinfo.CreateDate) + "," + SQL.Encode((object) jobinfo.Description) + ") ";
      dbQueryBuilder.AppendLine(text);
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    private static void createStateLicensing(OrgInfo info, int orgId)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("DELETE FROM [org_StateLicensing] WHERE [orgid] = " + (object) orgId + ";");
        pgDbQueryBuilder.ExecuteNonQuery();
        string empty = string.Empty;
        for (int index = 0; index < info.OrgBranchLicensing.StateLicenseExtTypes.Count; ++index)
        {
          StateLicenseExtType stateLicenseExtType = info.OrgBranchLicensing.StateLicenseExtTypes[index];
          if (!(stateLicenseExtType.LicenseType == string.Empty) || stateLicenseExtType.Selected || stateLicenseExtType.Exempt)
          {
            string text = "INSERT INTO [org_StateLicensing] (orgid, state, licenseType, licenseSelected, licenseExempt, LicenseNumber, IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate, SortIndex) VALUES (" + (object) orgId + ", " + SQL.Encode((object) stateLicenseExtType.StateAbbrevation) + ", " + SQL.Encode((object) stateLicenseExtType.LicenseType) + ", '" + (stateLicenseExtType.Selected ? (object) "1" : (object) "0") + "', '" + (stateLicenseExtType.Exempt ? (object) "1" : (object) "0") + "', '" + stateLicenseExtType.LicenseNo + "', " + SQL.EncodeDateTime(stateLicenseExtType.IssueDate) + ", " + SQL.EncodeDateTime(stateLicenseExtType.StartDate) + ", " + SQL.EncodeDateTime(stateLicenseExtType.EndDate) + ", '" + stateLicenseExtType.LicenseStatus + "', " + SQL.EncodeDateTime(stateLicenseExtType.StatusDate) + ", " + SQL.EncodeDateTime(stateLicenseExtType.LastChecked) + ", " + (object) stateLicenseExtType.SortIndex + ") ";
            pgDbQueryBuilder.AppendLine(text);
            pgDbQueryBuilder.Append(";");
          }
        }
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("DELETE FROM [org_StateLicensing] WHERE [orgid] = " + (object) orgId ?? "");
        dbQueryBuilder.ExecuteNonQuery();
        string empty = string.Empty;
        for (int index = 0; index < info.OrgBranchLicensing.StateLicenseExtTypes.Count; ++index)
        {
          StateLicenseExtType stateLicenseExtType = info.OrgBranchLicensing.StateLicenseExtTypes[index];
          if (!(stateLicenseExtType.LicenseType == string.Empty) || stateLicenseExtType.Selected || stateLicenseExtType.Exempt)
          {
            string text = "INSERT INTO [org_StateLicensing] (orgid, state, licenseType, licenseSelected, licenseExempt, LicenseNumber, IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate, SortIndex) VALUES (" + (object) orgId + ", " + SQL.Encode((object) stateLicenseExtType.StateAbbrevation) + ", " + SQL.Encode((object) stateLicenseExtType.LicenseType) + ", '" + (stateLicenseExtType.Selected ? (object) "1" : (object) "0") + "', '" + (stateLicenseExtType.Exempt ? (object) "1" : (object) "0") + "', '" + stateLicenseExtType.LicenseNo + "', " + SQL.EncodeDateTime(stateLicenseExtType.IssueDate) + ", " + SQL.EncodeDateTime(stateLicenseExtType.StartDate) + ", " + SQL.EncodeDateTime(stateLicenseExtType.EndDate) + ", '" + stateLicenseExtType.LicenseStatus + "', " + SQL.EncodeDateTime(stateLicenseExtType.StatusDate) + ", " + SQL.EncodeDateTime(stateLicenseExtType.LastChecked) + ", " + (object) stateLicenseExtType.SortIndex + ") ";
            dbQueryBuilder.AppendLine(text);
          }
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    private static void createHistoryCompPlans(OrgInfo info, int orgId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM [org_CompPlans] WHERE [oid] = " + (object) orgId ?? "");
      dbQueryBuilder.ExecuteNonQuery();
      if (info.LOCompHistoryList == null || info.LOCompHistoryList.Count == 0)
        return;
      string empty = string.Empty;
      for (int i = 0; i < info.LOCompHistoryList.Count; ++i)
      {
        LoanCompHistory historyAt = info.LOCompHistoryList.GetHistoryAt(i);
        string text = "INSERT INTO [org_CompPlans] (oid, compplanid, startDate, endDate) VALUES (" + (object) historyAt.IdForOrg + ", " + (object) historyAt.CompPlanId + ", " + SQL.EncodeDateTime(historyAt.StartDate, DateTime.MinValue) + ", " + SQL.EncodeDateTime(historyAt.EndDate, DateTime.MaxValue) + ") ";
        dbQueryBuilder.AppendLine(text);
      }
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Organization), "Cannot insert Loan Compensation History Information back to table \"org_CompPlans\" due to this error: " + ex.Message);
      }
    }

    private static int createOrgInDatabase(OrgInfo info, int parentOrgId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("org_chart"), Organization.createDbValueList(info), true, false);
        dbQueryBuilder.SelectIdentity();
        int orgId = (int) dbQueryBuilder.ExecuteScalar(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout);
        Organization.createStateLicensing(info, orgId);
        LOCompAccessor.CreateHistoryCompPlansForOrg(info.LOCompHistoryList, orgId, false, false);
        TraceLog.WriteVerbose(nameof (Organization), "Organization \"" + info.OrgName + "\" (" + (object) orgId + ") created in database.");
        return orgId;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Organization), ex);
        return -1;
      }
    }

    private static DbValueList createDbValueList(OrgInfo info)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("org_name", (object) info.OrgName);
      dbValueList.Add("description", (object) info.Description);
      dbValueList.Add("parent", (object) info.Parent);
      dbValueList.Add("company_name", (object) info.CompanyName);
      dbValueList.Add("address1", (object) info.CompanyAddress.Street1);
      dbValueList.Add("address2", (object) info.CompanyAddress.Street2);
      dbValueList.Add("city", (object) info.CompanyAddress.City);
      dbValueList.Add("state", (object) info.CompanyAddress.State);
      dbValueList.Add("zip", (object) info.CompanyAddress.Zip);
      dbValueList.Add("phone", (object) info.CompanyPhone);
      dbValueList.Add("fax", (object) info.CompanyFax);
      dbValueList.Add("org_code", (object) info.OrgCode);
      dbValueList.Add("nmls_code", (object) info.NMLSCode);
      dbValueList.Add("showorg_in_losearch", (object) (info.ShowOrgInLOSearch ? 1 : 0));
      dbValueList.Add("losearch_orgname", (object) info.LOSearchOrgName);
      dbValueList.Add("hmdaProfileId", (object) info.HMDAProfileId);
      if (info.CCSiteSettings != null)
        dbValueList.Add("inheritParentccsiteid", (object) (info.CCSiteSettings.UseParentInfo ? 1 : 0));
      else
        dbValueList.Add("inheritParentccsiteid", (object) 0);
      dbValueList.Add("mersmin_code", (object) info.MERSMINCode);
      dbValueList.Add("license_lender_type", (object) info.OrgBranchLicensing.LenderType);
      dbValueList.Add("license_home_state", (object) info.OrgBranchLicensing.HomeState);
      dbValueList.Add("license_statutory_maryland", (object) info.OrgBranchLicensing.StatutoryElectionInMaryland, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("license_statutory_maryland2", (object) info.OrgBranchLicensing.StatutoryElectionInMaryland2);
      dbValueList.Add("license_statutory_kansas", (object) info.OrgBranchLicensing.StatutoryElectionInKansas, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("license_use_custom_lender", (object) info.OrgBranchLicensing.UseCustomLenderProfile, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("license_dbaname1", (object) info.DBAName1);
      dbValueList.Add("license_dbaname2", (object) info.DBAName2);
      dbValueList.Add("license_dbaname3", (object) info.DBAName3);
      dbValueList.Add("license_dbaname4", (object) info.DBAName4);
      dbValueList.Add("emailSignature", (object) info.EmailSignature);
      dbValueList.Add("inheritParentCompPlan", (object) (bool) (info.LOCompHistoryList != null ? (info.LOCompHistoryList.UseParentInfo ? 1 : 0) : 0), (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("atrSmallCreditor", (object) (int) info.OrgBranchLicensing.ATRSmallCreditor);
      dbValueList.Add("atrExemptCreditor", (object) (int) info.OrgBranchLicensing.ATRExemptCreditor);
      dbValueList.Add("license_optout", (object) info.OrgBranchLicensing.OptOut);
      if (info.SSOSettings != null)
      {
        dbValueList.Add("ssoOnly", (object) (info.SSOSettings.LoginAccess ? 1 : 0));
        dbValueList.Add("inheritParentSSO", (object) (info.SSOSettings.UseParentInfo ? 1 : 0));
      }
      else
      {
        dbValueList.Add("ssoOnly", (object) 0);
        dbValueList.Add("inheritParentSSO", (object) 0);
      }
      if (!info.ONRPRetailBranchSettings.UseParentInfo && info.ONRPRetailBranchSettings.ONRPEndTime != null)
      {
        dbValueList.Add("onrp_enable", (object) (info.ONRPRetailBranchSettings.EnableONRP ? 1 : 0));
        dbValueList.Add("onrp_use_channel_default", (object) (info.ONRPRetailBranchSettings.UseChannelDefault ? 1 : 0));
        dbValueList.Add("onrp_continuous_coverage", (object) (info.ONRPRetailBranchSettings.ContinuousCoverage ? 1 : 0));
        dbValueList.Add("onrp_weekend_holiday_coverage", (object) (info.ONRPRetailBranchSettings.WeekendHolidayCoverage ? 1 : 0));
        dbValueList.Add("onrp_maximum_limit", (object) (info.ONRPRetailBranchSettings.MaximumLimit ? 1 : 0));
        dbValueList.Add("onrp_start_time", (object) info.ONRPRetailBranchSettings.ONRPStartTime);
        dbValueList.Add("onrp_end_time", (object) info.ONRPRetailBranchSettings.ONRPEndTime);
        dbValueList.Add("onrp_dollar_limit", (object) info.ONRPRetailBranchSettings.DollarLimit);
        dbValueList.Add("onrp_tolerance", (object) info.ONRPRetailBranchSettings.Tolerance);
        dbValueList.Add("inheritParentonrp", (object) (info.ONRPRetailBranchSettings.UseParentInfo ? 1 : 0));
        dbValueList.Add("onrp_sat_enable", (object) (info.ONRPRetailBranchSettings.EnableSatONRP ? 1 : 0));
        dbValueList.Add("onrp_sun_enable", (object) (info.ONRPRetailBranchSettings.EnableSunONRP ? 1 : 0));
        dbValueList.Add("onrp_sat_start_time", (object) info.ONRPRetailBranchSettings.ONRPSatStartTime);
        dbValueList.Add("onrp_sat_end_time", (object) info.ONRPRetailBranchSettings.ONRPSatEndTime);
        dbValueList.Add("onrp_sun_start_time", (object) info.ONRPRetailBranchSettings.ONRPSunStartTime);
        dbValueList.Add("onrp_sun_end_time", (object) info.ONRPRetailBranchSettings.ONRPSunEndTime);
      }
      else
      {
        dbValueList.Add("onrp_enable", (object) null);
        dbValueList.Add("onrp_use_channel_default", (object) null);
        dbValueList.Add("onrp_continuous_coverage", (object) null);
        dbValueList.Add("onrp_weekend_holiday_coverage", (object) null);
        dbValueList.Add("onrp_maximum_limit", (object) null);
        dbValueList.Add("onrp_start_time", (object) null);
        dbValueList.Add("onrp_end_time", (object) null);
        dbValueList.Add("onrp_dollar_limit", (object) null);
        dbValueList.Add("onrp_tolerance", (object) null);
        dbValueList.Add("inheritParentonrp", (object) (info.ONRPRetailBranchSettings.UseParentInfo ? 1 : 0));
        dbValueList.Add("onrp_sat_enable", (object) null);
        dbValueList.Add("onrp_sun_enable", (object) null);
        dbValueList.Add("onrp_sat_start_time", (object) null);
        dbValueList.Add("onrp_sat_end_time", (object) null);
        dbValueList.Add("onrp_sun_start_time", (object) null);
        dbValueList.Add("onrp_sun_end_time", (object) null);
      }
      dbValueList.Add("org_unitType", (object) info.CompanyAddress.UnitType);
      return dbValueList;
    }

    private static void deleteOrgFromDatabase(int oid)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("if exists (select 1 from org_chart where parent = " + (object) oid + ")");
        dbQueryBuilder.AppendLine("begin");
        dbQueryBuilder.AppendLine("    raiserror('Organization cannot be deleted because it is still in use', 16, 1)");
        dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.AppendLine("else if exists(select 1 from users where org_id = " + (object) oid + ")");
        dbQueryBuilder.AppendLine("begin");
        dbQueryBuilder.AppendLine("    raiserror('Organization cannot be deleted because it is still in use', 16, 1)");
        dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("begin");
        dbQueryBuilder.AppendLine("    delete from org_ccsite where oid = " + (object) oid ?? "");
        dbQueryBuilder.AppendLine("    delete from org_CompPlans where oid = " + (object) oid ?? "");
        dbQueryBuilder.AppendLine("    delete from org_chart where oid = " + (object) oid ?? "");
        dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
        TraceLog.WriteVerbose(nameof (Organization), "Organization (" + (object) oid + ") deleted from database.");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Organization), ex);
      }
    }

    [PgReady]
    private static OrgInfo dataRowToOrgInfo(
      DataRow r,
      int[] childIds,
      DataRow[] licenseRows,
      DataRow[] orgHistoryCompPlans)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        OrgInfo basicOrgInfo = Organization.CreateBasicOrgInfo(r, childIds);
        basicOrgInfo.EmailSignature = (string) SQL.Decode(r["emailSignature"], (object) "");
        string lenderType = (string) null;
        if (r["license_lender_type"] != DBNull.Value)
          lenderType = (string) SQL.Decode(r["license_lender_type"], (object) "");
        basicOrgInfo.OrgBranchLicensing = new BranchExtLicensing(lenderType, (string) SQL.Decode(r["license_home_state"], (object) ""), (string) SQL.Decode(r["license_optout"], (object) "N"), SQL.DecodeBoolean(r["license_statutory_maryland"]), (string) SQL.Decode(r["license_statutory_maryland2"]), SQL.DecodeBoolean(r["license_statutory_kansas"]), (List<StateLicenseType>) null, SQL.DecodeBoolean(r["license_use_custom_lender"]), BranchLicensing.ATRSmallCreditorToEnum(SQL.DecodeInt(r["atrSmallCreditor"], 0)), BranchLicensing.ATRExemptCreditorToEnum(SQL.DecodeInt(r["atrExemptCreditor"], 0)));
        if (licenseRows != null && licenseRows.Length != 0)
        {
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          DateTime minValue1 = DateTime.MinValue;
          DateTime minValue2 = DateTime.MinValue;
          DateTime minValue3 = DateTime.MinValue;
          DateTime minValue4 = DateTime.MinValue;
          DateTime minValue5 = DateTime.MinValue;
          for (int index = 0; index < licenseRows.Length; ++index)
          {
            string stateAbbrevation = (string) SQL.Decode(licenseRows[index]["state"], (object) "");
            string licenseType = (string) SQL.Decode(licenseRows[index]["licenseType"], (object) "");
            bool approved = SQL.DecodeBoolean(licenseRows[index]["licenseSelected"]);
            bool exempt = SQL.DecodeBoolean(licenseRows[index]["licenseExempt"]);
            string licenseNo = SQL.DecodeString(licenseRows[index]["LicenseNumber"], "");
            DateTime issueDate = SQL.DecodeDateTime(licenseRows[index]["IssueDate"], DateTime.MinValue);
            DateTime startDate = SQL.DecodeDateTime(licenseRows[index]["StartDate"], DateTime.MinValue);
            DateTime endDate = SQL.DecodeDateTime(licenseRows[index]["EndDate"], DateTime.MinValue);
            string licenseStatus = SQL.DecodeString(licenseRows[index]["Status"], "");
            DateTime statusDate = SQL.DecodeDateTime(licenseRows[index]["StatusDate"], DateTime.MinValue);
            DateTime lastChecked = SQL.DecodeDateTime(licenseRows[index]["LastCheckedDate"], DateTime.MinValue);
            int sortIndex = SQL.DecodeInt(licenseRows[index]["SortIndex"], 0);
            if (!(stateAbbrevation == string.Empty))
              basicOrgInfo.OrgBranchLicensing.AddStateLicenseExtType(new StateLicenseExtType(stateAbbrevation, licenseType, licenseNo, issueDate, startDate, endDate, licenseStatus, statusDate, approved, exempt, lastChecked, sortIndex));
          }
        }
        basicOrgInfo.LOCompHistoryList = new LoanCompHistoryList(basicOrgInfo.Oid.ToString());
        basicOrgInfo.LOCompHistoryList.UseParentInfo = SQL.DecodeBoolean(r["inheritParentCompPlan"]);
        if (orgHistoryCompPlans != null)
        {
          for (int index = 0; index < orgHistoryCompPlans.Length; ++index)
          {
            LoanCompHistory historyFromDatarow = LOCompAccessor.GetLoanCompHistoryFromDatarow(orgHistoryCompPlans[index], false);
            if (historyFromDatarow != null)
              basicOrgInfo.LOCompHistoryList.AddHistory(historyFromDatarow);
          }
        }
        basicOrgInfo.CCSiteSettings.UseParentInfo = SQL.DecodeBoolean(r["inheritParentccsiteid"]);
        basicOrgInfo.SSOSettings.LoginAccess = SQL.DecodeBoolean(r["ssoOnly"]);
        basicOrgInfo.SSOSettings.UseParentInfo = SQL.DecodeBoolean(r["inheritParentSSO"]);
        return basicOrgInfo;
      }
      OrgInfo basicOrgInfo1 = Organization.CreateBasicOrgInfo(r, childIds);
      basicOrgInfo1.EmailSignature = (string) SQL.Decode(r["emailSignature"], (object) "");
      string lenderType1 = (string) null;
      if (r["license_lender_type"] != DBNull.Value)
        lenderType1 = (string) SQL.Decode(r["license_lender_type"], (object) "");
      basicOrgInfo1.OrgBranchLicensing = new BranchExtLicensing(lenderType1, (string) SQL.Decode(r["license_home_state"], (object) ""), (string) SQL.Decode(r["license_optout"], (object) "N"), (bool) SQL.Decode(r["license_statutory_maryland"], (object) false), (string) SQL.Decode(r["license_statutory_maryland2"], (object) "00"), (bool) SQL.Decode(r["license_statutory_kansas"], (object) false), (List<StateLicenseType>) null, (bool) SQL.Decode(r["license_use_custom_lender"], (object) false), BranchLicensing.ATRSmallCreditorToEnum(SQL.DecodeInt(r["atrSmallCreditor"], 0)), BranchLicensing.ATRExemptCreditorToEnum(SQL.DecodeInt(r["atrExemptCreditor"], 0)));
      if (licenseRows != null && licenseRows.Length != 0)
      {
        string empty3 = string.Empty;
        string empty4 = string.Empty;
        DateTime minValue6 = DateTime.MinValue;
        DateTime minValue7 = DateTime.MinValue;
        DateTime minValue8 = DateTime.MinValue;
        DateTime minValue9 = DateTime.MinValue;
        DateTime minValue10 = DateTime.MinValue;
        for (int index = 0; index < licenseRows.Length; ++index)
        {
          string stateAbbrevation = (string) SQL.Decode(licenseRows[index]["state"], (object) "");
          string licenseType = (string) SQL.Decode(licenseRows[index]["licenseType"], (object) "");
          bool approved = (bool) SQL.Decode(licenseRows[index]["licenseSelected"], (object) false);
          bool exempt = (bool) SQL.Decode(licenseRows[index]["licenseExempt"], (object) false);
          string licenseNo = SQL.DecodeString(licenseRows[index]["LicenseNumber"], "");
          DateTime issueDate = SQL.DecodeDateTime(licenseRows[index]["IssueDate"], DateTime.MinValue);
          DateTime startDate = SQL.DecodeDateTime(licenseRows[index]["StartDate"], DateTime.MinValue);
          DateTime endDate = SQL.DecodeDateTime(licenseRows[index]["EndDate"], DateTime.MinValue);
          string licenseStatus = SQL.DecodeString(licenseRows[index]["Status"], "");
          DateTime statusDate = SQL.DecodeDateTime(licenseRows[index]["StatusDate"], DateTime.MinValue);
          DateTime lastChecked = SQL.DecodeDateTime(licenseRows[index]["LastCheckedDate"], DateTime.MinValue);
          int sortIndex = SQL.DecodeInt(licenseRows[index]["SortIndex"], 0);
          if (!(stateAbbrevation == string.Empty))
            basicOrgInfo1.OrgBranchLicensing.AddStateLicenseExtType(new StateLicenseExtType(stateAbbrevation, licenseType, licenseNo, issueDate, startDate, endDate, licenseStatus, statusDate, approved, exempt, lastChecked, sortIndex));
        }
      }
      basicOrgInfo1.LOCompHistoryList = new LoanCompHistoryList(basicOrgInfo1.Oid.ToString());
      basicOrgInfo1.LOCompHistoryList.UseParentInfo = (bool) SQL.Decode(r["inheritParentCompPlan"], (object) false);
      if (orgHistoryCompPlans != null)
      {
        for (int index = 0; index < orgHistoryCompPlans.Length; ++index)
        {
          LoanCompHistory historyFromDatarow = LOCompAccessor.GetLoanCompHistoryFromDatarow(orgHistoryCompPlans[index], false);
          if (historyFromDatarow != null)
            basicOrgInfo1.LOCompHistoryList.AddHistory(historyFromDatarow);
        }
      }
      basicOrgInfo1.CCSiteSettings.UseParentInfo = (bool) SQL.Decode(r["inheritParentccsiteid"], (object) false);
      basicOrgInfo1.SSOSettings.LoginAccess = (bool) SQL.Decode(r["ssoOnly"], (object) false);
      basicOrgInfo1.SSOSettings.UseParentInfo = (bool) SQL.Decode(r["inheritParentSSO"], (object) false);
      return basicOrgInfo1;
    }

    [PgReady]
    internal static OrgInfo CreateBasicOrgInfo(DataRow r, int[] childIds)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        int oid = (int) r["oid"];
        string orgName = r["org_name"].ToString();
        string description = (string) SQL.Decode(r["description"], (object) "");
        int parent = (int) r["parent"];
        string orgCode = (string) SQL.Decode(r["org_code"], (object) "");
        string companyName = (string) SQL.Decode(r["company_name"], (object) "");
        Address companyAddress = new Address((string) SQL.Decode(r["address1"], (object) ""), (string) SQL.Decode(r["address2"], (object) ""), (string) SQL.Decode(r["city"], (object) ""), (string) SQL.Decode(r["state"], (object) ""), (string) SQL.Decode(r["zip"], (object) ""), (string) SQL.Decode(r["org_unitType"], (object) ""));
        string phone = (string) SQL.Decode(r["phone"], (object) "");
        string fax = (string) SQL.Decode(r["fax"], (object) "");
        int[] children = childIds;
        string nmlsCode = (string) SQL.Decode(r["nmls_code"], (object) "");
        string mersminCode = (string) SQL.Decode(r["mersmin_code"], (object) "");
        string[] dbNames = new string[4]
        {
          (string) SQL.Decode(r["license_dbaname1"], (object) ""),
          (string) SQL.Decode(r["license_dbaname2"], (object) ""),
          (string) SQL.Decode(r["license_dbaname3"], (object) ""),
          (string) SQL.Decode(r["license_dbaname4"], (object) "")
        };
        ONRPEntitySettings ONRPBranchSettings = new ONRPEntitySettings();
        ONRPBranchSettings.ContinuousCoverage = SQL.DecodeBoolean(r["onrp_continuous_coverage"]);
        ONRPBranchSettings.DollarLimit = SQL.DecodeDouble(r["onrp_dollar_limit"], 0.0);
        ONRPBranchSettings.EnableONRP = SQL.DecodeBoolean(r["onrp_enable"]);
        ONRPBranchSettings.ONRPEndTime = (string) SQL.Decode(r["onrp_end_time"], (object) "");
        ONRPBranchSettings.MaximumLimit = SQL.DecodeBoolean(r["onrp_maximum_limit"]);
        ONRPBranchSettings.ONRPStartTime = (string) SQL.Decode(r["onrp_start_time"], (object) "");
        ONRPBranchSettings.Tolerance = (int) SQL.DecodeDecimal(r["onrp_tolerance"], 0M);
        ONRPBranchSettings.UseChannelDefault = SQL.DecodeBoolean(r["onrp_use_channel_default"], true);
        ONRPBranchSettings.WeekendHolidayCoverage = SQL.DecodeBoolean(r["onrp_weekend_holiday_coverage"]);
        ONRPBranchSettings.UseParentInfo = SQL.DecodeBoolean(r["inheritParentonrp"]);
        ONRPBranchSettings.EnableSatONRP = SQL.DecodeBoolean(r["onrp_sat_enable"], false);
        ONRPBranchSettings.EnableSunONRP = SQL.DecodeBoolean(r["onrp_sun_enable"]);
        ONRPBranchSettings.ONRPSatStartTime = (string) SQL.Decode(r["onrp_sat_start_time"], (object) "");
        ONRPBranchSettings.ONRPSatEndTime = (string) SQL.Decode(r["onrp_sat_end_time"], (object) "");
        ONRPBranchSettings.ONRPSunStartTime = (string) SQL.Decode(r["onrp_sun_start_time"], (object) "");
        ONRPBranchSettings.ONRPSunEndTime = (string) SQL.Decode(r["onrp_sun_end_time"], (object) "");
        CCSiteInfo ccSiteInfo = new CCSiteInfo();
        SSOInfo ssoSettings = new SSOInfo();
        int num = SQL.DecodeBoolean(r["showorg_in_losearch"]) ? 1 : 0;
        string loSearchOrgName = (string) SQL.Decode(r["losearch_orgname"], (object) "");
        int hmdaProfileId = SQL.DecodeInt(r["hmdaProfileId"]);
        return new OrgInfo(oid, orgName, description, parent, orgCode, companyName, companyAddress, phone, fax, children, nmlsCode, mersminCode, dbNames, (BranchExtLicensing) null, (LoanCompHistoryList) null, ONRPBranchSettings, ccSiteInfo, ssoSettings, num != 0, loSearchOrgName, hmdaProfileId);
      }
      int oid1 = (int) r["oid"];
      string orgName1 = r["org_name"].ToString();
      string description1 = (string) SQL.Decode(r["description"], (object) "");
      int parent1 = (int) r["parent"];
      string orgCode1 = (string) SQL.Decode(r["org_code"], (object) "");
      string companyName1 = (string) SQL.Decode(r["company_name"], (object) "");
      Address companyAddress1 = new Address((string) SQL.Decode(r["address1"], (object) ""), (string) SQL.Decode(r["address2"], (object) ""), (string) SQL.Decode(r["city"], (object) ""), (string) SQL.Decode(r["state"], (object) ""), (string) SQL.Decode(r["zip"], (object) ""), (string) SQL.Decode(r["org_unitType"], (object) ""));
      string phone1 = (string) SQL.Decode(r["phone"], (object) "");
      string fax1 = (string) SQL.Decode(r["fax"], (object) "");
      int[] children1 = childIds;
      string nmlsCode1 = (string) SQL.Decode(r["nmls_code"], (object) "");
      string mersminCode1 = (string) SQL.Decode(r["mersmin_code"], (object) "");
      string[] dbNames1 = new string[4]
      {
        (string) SQL.Decode(r["license_dbaname1"], (object) ""),
        (string) SQL.Decode(r["license_dbaname2"], (object) ""),
        (string) SQL.Decode(r["license_dbaname3"], (object) ""),
        (string) SQL.Decode(r["license_dbaname4"], (object) "")
      };
      ONRPEntitySettings ONRPBranchSettings1 = new ONRPEntitySettings();
      ONRPBranchSettings1.ContinuousCoverage = (bool) SQL.Decode(r["onrp_continuous_coverage"], (object) false);
      ONRPBranchSettings1.DollarLimit = SQL.DecodeDouble(r["onrp_dollar_limit"], 0.0);
      ONRPBranchSettings1.EnableONRP = (bool) SQL.Decode(r["onrp_enable"], (object) false);
      ONRPBranchSettings1.ONRPEndTime = (string) SQL.Decode(r["onrp_end_time"], (object) "");
      ONRPBranchSettings1.MaximumLimit = (bool) SQL.Decode(r["onrp_maximum_limit"], (object) false);
      ONRPBranchSettings1.ONRPStartTime = (string) SQL.Decode(r["onrp_start_time"], (object) "");
      ONRPBranchSettings1.Tolerance = (int) SQL.DecodeDecimal(r["onrp_tolerance"], 0M);
      ONRPBranchSettings1.UseChannelDefault = (bool) SQL.Decode(r["onrp_use_channel_default"], (object) true);
      ONRPBranchSettings1.WeekendHolidayCoverage = (bool) SQL.Decode(r["onrp_weekend_holiday_coverage"], (object) false);
      ONRPBranchSettings1.UseParentInfo = (bool) SQL.Decode(r["inheritParentonrp"], (object) false);
      ONRPBranchSettings1.EnableSatONRP = (bool) SQL.Decode(r["onrp_sat_enable"], (object) false);
      ONRPBranchSettings1.EnableSunONRP = (bool) SQL.Decode(r["onrp_sun_enable"], (object) false);
      ONRPBranchSettings1.ONRPSatStartTime = (string) SQL.Decode(r["onrp_sat_start_time"], (object) "");
      ONRPBranchSettings1.ONRPSatEndTime = (string) SQL.Decode(r["onrp_sat_end_time"], (object) "");
      ONRPBranchSettings1.ONRPSunStartTime = (string) SQL.Decode(r["onrp_sun_start_time"], (object) "");
      ONRPBranchSettings1.ONRPSunEndTime = (string) SQL.Decode(r["onrp_sun_end_time"], (object) "");
      CCSiteInfo ccSiteInfo1 = new CCSiteInfo();
      SSOInfo ssoSettings1 = new SSOInfo();
      int num1 = (bool) SQL.Decode(r["showorg_in_losearch"], (object) false) ? 1 : 0;
      string loSearchOrgName1 = (string) SQL.Decode(r["losearch_orgname"], (object) "");
      int hmdaProfileId1 = SQL.DecodeInt(r["hmdaProfileId"]);
      return new OrgInfo(oid1, orgName1, description1, parent1, orgCode1, companyName1, companyAddress1, phone1, fax1, children1, nmlsCode1, mersminCode1, dbNames1, (BranchExtLicensing) null, (LoanCompHistoryList) null, ONRPBranchSettings1, ccSiteInfo1, ssoSettings1, num1 != 0, loSearchOrgName1, hmdaProfileId1);
    }

    public static List<OrgHierarchyInfo> GetOrgHierarchy(int oid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendFormat("DECLARE  @RootOrgId as int ");
      dbQueryBuilder.AppendFormat("IF (SELECT org_type FROM org_chart WHERE oid= {0}) IS NULL SET @RootOrgId=0 ", (object) SQL.Encode((object) oid));
      dbQueryBuilder.AppendFormat("ELSE SELECT @RootOrgId = parent FROM org_chart WHERE oid = parent;");
      dbQueryBuilder.AppendFormat("WITH tblParent AS ( ");
      dbQueryBuilder.AppendFormat("SELECT oid,org_name,parent FROM org_chart t WHERE oid = {0} or (oid = @RootOrgId and parent = @RootOrgId)", (object) SQL.Encode((object) oid));
      dbQueryBuilder.AppendFormat("UNION ALL ");
      dbQueryBuilder.AppendFormat("SELECT oc.oid,oc.org_name,oc.parent FROM org_chart oc ");
      dbQueryBuilder.AppendFormat("JOIN tblParent ");
      dbQueryBuilder.AppendFormat("ON oc.oid = tblParent.parent where oc.oid <> oc.parent ) ");
      dbQueryBuilder.AppendFormat("SELECT oid,org_name,parent FROM tblParent order by oid desc");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<OrgHierarchyInfo> orgHierarchy = new List<OrgHierarchyInfo>(dataRowCollection.Count);
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        OrgHierarchyInfo orgHierarchyInfo = new OrgHierarchyInfo()
        {
          OrgId = dataRowCollection[index][nameof (oid)].ToString(),
          OrgName = dataRowCollection[index]["org_name"].ToString(),
          ParentId = dataRowCollection[index]["parent"].ToString()
        };
        orgHierarchy.Add(orgHierarchyInfo);
      }
      return orgHierarchy;
    }

    public static ONRPEntitySettings GetOrgOnrpInfo(int oid)
    {
      return Organization.OrgInfoToOrgOnrpSettings(Organization.getOrgFromDatabase(oid));
    }

    private static ONRPEntitySettings OrgInfoToOrgOnrpSettings(OrgInfo info)
    {
      return new ONRPEntitySettings()
      {
        ContinuousCoverage = info.ONRPRetailBranchSettings.ContinuousCoverage,
        EnableONRP = info.ONRPRetailBranchSettings.EnableONRP,
        ONRPEndTime = info.ONRPRetailBranchSettings.ONRPEndTime,
        ONRPStartTime = info.ONRPRetailBranchSettings.ONRPStartTime,
        UseChannelDefault = info.ONRPRetailBranchSettings.UseChannelDefault,
        WeekendHolidayCoverage = info.ONRPRetailBranchSettings.WeekendHolidayCoverage,
        MaximumLimit = info.ONRPRetailBranchSettings.MaximumLimit,
        Tolerance = info.ONRPRetailBranchSettings.Tolerance,
        DollarLimit = info.ONRPRetailBranchSettings.DollarLimit,
        EnableSatONRP = info.ONRPRetailBranchSettings.EnableSatONRP,
        EnableSunONRP = info.ONRPRetailBranchSettings.EnableSunONRP,
        ONRPSatStartTime = info.ONRPRetailBranchSettings.ONRPSatStartTime,
        ONRPSatEndTime = info.ONRPRetailBranchSettings.ONRPSatEndTime,
        ONRPSunStartTime = info.ONRPRetailBranchSettings.ONRPSunStartTime,
        ONRPSunEndTime = info.ONRPRetailBranchSettings.ONRPSunEndTime
      };
    }

    public static bool IsRetailBranchExist(int oid) => Organization.getOrgFromDatabase(oid) != null;
  }
}
