// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.HMDAProfileDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class HMDAProfileDbAccessor
  {
    private static string HMDAProfileTable = "HMDAProfile";
    private static string orgChartTable = "org_chart";

    [PgReady]
    public static List<HMDAProfile> GetHMDAProfiles()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
        pgDbQueryBuilder.SelectFrom(table);
        pgDbQueryBuilder.Append("order by LastModifiedDate desc");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        List<HMDAProfile> hmdaProfiles = new List<HMDAProfile>();
        if (dataRowCollection != null)
        {
          foreach (DataRow dr in (InternalDataCollectionBase) dataRowCollection)
            hmdaProfiles.Add(HMDAProfileDbAccessor.convertDataRowToProfile(dr));
        }
        return hmdaProfiles;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
      dbQueryBuilder.SelectFrom(table1);
      dbQueryBuilder.Append("order by LastModifiedDate desc");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      List<HMDAProfile> hmdaProfiles1 = new List<HMDAProfile>();
      if (dataRowCollection1 != null)
      {
        foreach (DataRow dr in (InternalDataCollectionBase) dataRowCollection1)
          hmdaProfiles1.Add(HMDAProfileDbAccessor.convertDataRowToProfile(dr));
      }
      return hmdaProfiles1;
    }

    [PgReady]
    public static HMDAProfile GetHMDAProfileById(int profileId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
        pgDbQueryBuilder.SelectFrom(table);
        pgDbQueryBuilder.Where(new DbValueList()
        {
          (DbValue) new DbFilterValue(table, "HMDAProfileID", "HMDAProfileID", (object) Convert.ToString(profileId))
        });
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        return dataRowCollection == null || dataRowCollection.Count == 0 ? (HMDAProfile) null : HMDAProfileDbAccessor.convertDataRowToProfile(dataRowCollection[0]);
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
      dbQueryBuilder.SelectFrom(table1);
      dbQueryBuilder.Where(new DbValueList()
      {
        (DbValue) new DbFilterValue(table1, "HMDAProfileID", "HMDAProfileID", (object) Convert.ToString(profileId))
      });
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      return dataRowCollection1 == null || dataRowCollection1.Count == 0 ? (HMDAProfile) null : HMDAProfileDbAccessor.convertDataRowToProfile(dataRowCollection1[0]);
    }

    [PgReady]
    private static HMDAProfile convertDataRowToProfile(DataRow dr)
    {
      return new HMDAProfile()
      {
        HMDAProfileID = (int) dr["HMDAProfileID"],
        HMDAProfileName = dr["HMDAProfileName"] == DBNull.Value ? string.Empty : (string) dr["HMDAProfileName"],
        HMDAProfileRespondentID = dr["RespondentID"] == DBNull.Value ? string.Empty : (string) dr["RespondentID"],
        HMDAProfileLEI = dr["LEI"] == DBNull.Value ? string.Empty : (string) dr["LEI"],
        HMDAProfileCompanyName = dr["CompanyName"] == DBNull.Value ? string.Empty : (string) dr["CompanyName"],
        HMDAProfileAgency = dr["Agency"] == DBNull.Value ? string.Empty : (string) dr["Agency"],
        HMDAProfileLastModifiedBy = (string) dr["LastModifiedBy"],
        HMDAProfileLastModifiedDate = (DateTime) dr["LastModifiedDate"],
        HMDAProfileSetting = dr["HMDASettingValue"] == DBNull.Value ? string.Empty : (string) dr["HMDASettingValue"]
      };
    }

    public static void UpdateHMDAProfile(HMDAProfile hmdaProfile)
    {
      if (hmdaProfile.HMDAProfileID == 0)
        HMDAProfileDbAccessor.CreateHMDAProfile(hmdaProfile);
      else
        HMDAProfileDbAccessor.UpdateExistingHMDAProfile(hmdaProfile);
    }

    public static bool DoesProfileNameExist(string profileName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
      DbValue key = new DbValue("HMDAProfileName", (object) profileName);
      dbQueryBuilder.SelectFrom(table, key);
      return dbQueryBuilder.Execute().Count > 0;
    }

    public static void DeleteHMDAProfile(string profileName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
      DbValue key = new DbValue("HMDAProfileName", (object) profileName);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteScalar();
    }

    public static bool IsAssociateToOrg(int profileID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.orgChartTable);
      DbValue key = new DbValue("HMDAProfileID", (object) profileID);
      dbQueryBuilder.SelectFrom(table, key);
      return dbQueryBuilder.Execute().Count > 0;
    }

    public static string GetOrgNameByHMDAProfile(int profileID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.orgChartTable);
      DbValue key = new DbValue("HMDAProfileID", (object) profileID);
      dbQueryBuilder.SelectFrom(table, key);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count > 0 ? dataRowCollection[0]["org_name"].ToString() : string.Empty;
    }

    private static void CreateHMDAProfile(HMDAProfile hmdaProfile)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
      DbValueList values = new DbValueList();
      values.Add("HMDAProfileName", (object) hmdaProfile.HMDAProfileName);
      values.Add("RespondentID", (object) hmdaProfile.HMDAProfileRespondentID);
      values.Add("LEI", (object) hmdaProfile.HMDAProfileLEI);
      values.Add("CompanyName", (object) hmdaProfile.HMDAProfileCompanyName);
      values.Add("Agency", (object) hmdaProfile.HMDAProfileAgency);
      values.Add("LastModifiedBy", (object) hmdaProfile.HMDAProfileLastModifiedBy);
      values.Add("LastModifiedDate", (object) hmdaProfile.HMDAProfileLastModifiedDate);
      values.Add("HMDASettingValue", (object) hmdaProfile.HMDAProfileSetting);
      dbQueryBuilder.Declare("@HMDAProfileID", "int");
      DbValue dbValue = new DbValue("ruleId", (object) "@ruleId", (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.ExecuteScalar();
    }

    private static void UpdateExistingHMDAProfile(HMDAProfile hmdaProfile)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(HMDAProfileDbAccessor.HMDAProfileTable);
      DbValueList values = new DbValueList();
      values.Add("HMDAProfileName", (object) hmdaProfile.HMDAProfileName);
      values.Add("RespondentID", (object) hmdaProfile.HMDAProfileRespondentID);
      values.Add("LEI", (object) hmdaProfile.HMDAProfileLEI);
      values.Add("CompanyName", (object) hmdaProfile.HMDAProfileCompanyName);
      values.Add("Agency", (object) hmdaProfile.HMDAProfileAgency);
      values.Add("LastModifiedBy", (object) hmdaProfile.HMDAProfileLastModifiedBy);
      values.Add("LastModifiedDate", (object) hmdaProfile.HMDAProfileLastModifiedDate);
      values.Add("HMDASettingValue", (object) hmdaProfile.HMDAProfileSetting);
      DbValue key = new DbValue("HMDAProfileID", (object) hmdaProfile.HMDAProfileID, (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.Update(table, values, key);
      dbQueryBuilder.ExecuteScalar();
    }
  }
}
