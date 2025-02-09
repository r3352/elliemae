// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AMILimitStore
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
  public sealed class AMILimitStore
  {
    private const string className = "AMILimitStore�";

    public static AMILimit[] GetAMILimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from AMILimits Order by LimitYear DESC");
      List<AMILimit> amiLimitList = new List<AMILimit>();
      try
      {
        TraceLog.WriteInfo(nameof (AMILimitStore), "Execute SQL query for method GetAMILimits(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          amiLimitList.Add(AMILimitStore.ConvertToAMILimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (AMILimitStore), ex);
        throw new Exception("Cannot read AMILimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return amiLimitList.ToArray();
    }

    public static AMILimit[] GetAMILimits(int year)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from AMILimits where LimitYear = '" + (object) year + "';");
      List<AMILimit> amiLimitList = new List<AMILimit>();
      try
      {
        TraceLog.WriteInfo(nameof (AMILimitStore), "Execute SQL query for method GetAMILimits(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          amiLimitList.Add(AMILimitStore.ConvertToAMILimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (AMILimitStore), ex);
        throw new Exception("Cannot read AMILimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return amiLimitList.ToArray();
    }

    public static AMILimit[] GetAMILimits(string fipsCode)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from AMILimits where FIPSCode = '" + fipsCode + "' Order by LimitYear DESC");
      List<AMILimit> amiLimitList = new List<AMILimit>();
      try
      {
        TraceLog.WriteInfo(nameof (AMILimitStore), "Execute SQL query for method GetAMILimits(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          amiLimitList.Add(AMILimitStore.ConvertToAMILimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (AMILimitStore), ex);
        throw new Exception("Cannot read AMILimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return amiLimitList.ToArray();
    }

    public static void ResetAMILimits(AMILimit[] limits)
    {
      AMILimitStore.DeleteAllAMILimits();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      foreach (AMILimit limit in limits)
      {
        string text = "Insert into AMILimits " + "Values ('" + (object) limit.LimitYear + "', " + SQL.EncodeString(limit.FIPSCode) + ", " + SQL.EncodeString(limit.StateName) + ", " + SQL.EncodeString(limit.CountyName) + ", " + SQL.EncodeString(limit.AmiLimit100) + ", " + SQL.EncodeString(limit.AmiLimit80) + ", " + SQL.EncodeString(limit.AmiLimit50) + ", " + SQL.EncodeDateTime(limit.LastModifiedDateTime, DateTime.MinValue) + ")";
        dbQueryBuilder.AppendLine(text);
      }
      try
      {
        TraceLog.WriteInfo(nameof (AMILimitStore), "Execute SQL query for method ResetAMILimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (AMILimitStore), ex);
        throw new Exception("Cannot reset AMILimit table. Due to the following problem:\r\n" + ex.Message);
      }
    }

    private static void DeleteAllAMILimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete AMILimits");
      try
      {
        TraceLog.WriteInfo(nameof (AMILimitStore), "Execute SQL query for method DeleteAllAMILimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (AMILimitStore), ex);
        throw new Exception("Cannot delete AMILimits record. Due to the following problem:\r\n" + ex.Message);
      }
    }

    private static AMILimit ConvertToAMILimitObj(DataRow row)
    {
      return new AMILimit(Utils.ParseInt(row["ID"]), Utils.ParseInt(row["LimitYear"]), string.Concat(row["FIPSCode"]), string.Concat(row["StateName"]), string.Concat(row["CountyName"]), string.Concat(row["AmiLimit100"]), string.Concat(row["AmiLimit80"]), string.Concat(row["AmiLimit50"]), DateTime.Parse(string.Concat(row["LastModifiedDateTime"])));
    }
  }
}
