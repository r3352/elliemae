// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ConventionalCountyLimitStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class ConventionalCountyLimitStore
  {
    private const string className = "ConventionalCountyLimitStore�";

    public static ConventionalCountyLimit[] GetConventionalCountyLimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from ConventionalCountyLimit");
      List<ConventionalCountyLimit> conventionalCountyLimitList = new List<ConventionalCountyLimit>();
      try
      {
        TraceLog.WriteInfo(nameof (ConventionalCountyLimitStore), "Execute SQL query for method GetConventionalCountyLimits(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          conventionalCountyLimitList.Add(ConventionalCountyLimitStore.ConvertToCountyLimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ConventionalCountyLimitStore), ex);
        throw new Exception("Cannot read ConventionalCountyLimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return conventionalCountyLimitList.ToArray();
    }

    public static ConventionalCountyLimit[] GetConventionalCountyLimits(string state, int year)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from ConventionalCountyLimit where StateCode = '" + state + "' and LimitYear = '" + (object) year + "';");
      List<ConventionalCountyLimit> conventionalCountyLimitList = new List<ConventionalCountyLimit>();
      try
      {
        TraceLog.WriteInfo(nameof (ConventionalCountyLimitStore), "Execute SQL query for method GetConventionalCountyLimits(string state, int year): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          conventionalCountyLimitList.Add(ConventionalCountyLimitStore.ConvertToCountyLimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ConventionalCountyLimitStore), ex);
        throw new Exception("Cannot read ConventionalCountyLimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return conventionalCountyLimitList.ToArray();
    }

    public static ConventionalCountyLimit[] GetConventionalCountyLimits(string state)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from ConventionalCountyLimit where StateCode = '" + state + "';");
      List<ConventionalCountyLimit> conventionalCountyLimitList = new List<ConventionalCountyLimit>();
      try
      {
        TraceLog.WriteInfo(nameof (ConventionalCountyLimitStore), "Execute SQL query for method GetConventionalCountyLimits(string state): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          conventionalCountyLimitList.Add(ConventionalCountyLimitStore.ConvertToCountyLimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ConventionalCountyLimitStore), ex);
        throw new Exception("Cannot read ConventionalCountyLimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return conventionalCountyLimitList.ToArray();
    }

    private static void DeleteAllConventionalCountyLimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete ConventionalCountyLimit");
      try
      {
        TraceLog.WriteInfo(nameof (ConventionalCountyLimitStore), "Execute SQL query for method DeleteAllConventionalCountyLimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ConventionalCountyLimitStore), ex);
        throw new Exception("Cannot delete ConventionalCountyLimit record. Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static void ResetConventionalCountyLimits(ConventionalCountyLimit[] countyLimits)
    {
      ConventionalCountyLimitStore.DeleteAllConventionalCountyLimits();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      foreach (ConventionalCountyLimit countyLimit in countyLimits)
      {
        string text = "Insert into ConventionalCountyLimit " + "Values ('" + (object) countyLimit.LimitYear + "', " + SQL.EncodeString(countyLimit.FIPSStateCode) + ", " + SQL.EncodeString(countyLimit.FIPSCountyCode) + ", " + SQL.EncodeString(countyLimit.CountyName) + ", " + SQL.EncodeString(countyLimit.StateCode) + ", " + SQL.EncodeString(countyLimit.CBSANumber) + ", '" + (object) countyLimit.LimitFor1Unit + "', '" + (object) countyLimit.LimitFor2Units + "', '" + (object) countyLimit.LimitFor3Units + "', '" + (object) countyLimit.LimitFor4Units + "', " + SQL.EncodeDateTime(countyLimit.LastModifiedDateTime, DateTime.MinValue) + ")";
        dbQueryBuilder.AppendLine(text);
      }
      try
      {
        TraceLog.WriteInfo(nameof (ConventionalCountyLimitStore), "Execute SQL query for method ResetConventionalCountyLimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ConventionalCountyLimitStore), ex);
        throw new Exception("Cannot reset ConventionalCountyLimit table. Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static ConventionalCountyLimit ConvertToCountyLimitObj(DataRow row)
    {
      return new ConventionalCountyLimit(Utils.ParseInt(row["ID"]), Utils.ParseInt(row["LimitYear"]), string.Concat(row["FIPSStateCode"]), string.Concat(row["FIPSCountyCode"]), string.Concat(row["CountyName"]), string.Concat(row["StateCode"]), string.Concat(row["CBSANumber"]), Utils.ParseInt(row["Limit1"]), Utils.ParseInt(row["Limit2"]), Utils.ParseInt(row["Limit3"]), Utils.ParseInt(row["Limit4"]), DateTime.Parse(string.Concat(row["LastModifiedDateTime"])));
    }
  }
}
