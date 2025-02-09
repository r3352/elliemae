// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.MFILimitStore
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
  public sealed class MFILimitStore
  {
    private const string className = "MFILimitStore�";

    public static MFILimit[] GetMFILimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from MFILimits Order by SourceFileYear DESC");
      List<MFILimit> mfiLimitList = new List<MFILimit>();
      try
      {
        TraceLog.WriteInfo(nameof (MFILimitStore), "Execute SQL query for method GetMFILimits(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          mfiLimitList.Add(MFILimitStore.ConvertToMFILimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MFILimitStore), ex);
        throw new Exception("Cannot read MFILimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return mfiLimitList.ToArray();
    }

    public static MFILimit[] GetMFILimits(string msaCode, string subjectState)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      List<MFILimit> mfiLimitList = new List<MFILimit>();
      bool flag = false;
      if ("99999" == msaCode)
      {
        if (string.IsNullOrEmpty(subjectState))
          return mfiLimitList.ToArray();
        flag = true;
      }
      try
      {
        if (!flag)
        {
          dbQueryBuilder.AppendLine("Select * from MFILimits where MSAMDCode = '" + msaCode + "' Order by SourceFileYear DESC");
          TraceLog.WriteInfo(nameof (MFILimitStore), "Execute SQL query for method GetMFILimits(): '" + dbQueryBuilder.ToString() + "'.");
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
              mfiLimitList.Add(MFILimitStore.ConvertToMFILimitObj(row));
          }
          else
            flag = true;
        }
        if (flag)
        {
          if (dbQueryBuilder.Length > 0)
            dbQueryBuilder.Reset();
          string str = "nonmetro portion of " + Utils.GetFullStateName(subjectState);
          dbQueryBuilder.AppendLine("Select * from MFILimits where MSAMDCode = '99999' and MSAMDName = '" + str + "' Order by SourceFileYear DESC");
          TraceLog.WriteInfo(nameof (MFILimitStore), "Execute 2nd SQL query for method GetMFILimits(): '" + dbQueryBuilder.ToString() + "'.");
          foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
            mfiLimitList.Add(MFILimitStore.ConvertToMFILimitObj(row));
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MFILimitStore), ex);
        throw new Exception("Not able to retrieve MFI data due to the following problem:\r\n" + ex.Message);
      }
      return mfiLimitList.ToArray();
    }

    public static void ResetMFILimits(MFILimit[] limits)
    {
      MFILimitStore.DeleteAllMFILimits();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      foreach (MFILimit limit in limits)
      {
        string text = "Insert into MFILimits " + "Values ('" + (object) limit.SourceFileYear + "', " + SQL.EncodeString(limit.MSAMDCode) + ", " + SQL.EncodeString(limit.MSAMDName) + ", " + (object) limit.ActualMFIYear + ", " + SQL.EncodeString(limit.ActualMFIAmount) + ", " + (object) limit.EstimatedMFIYear + ", " + SQL.EncodeString(limit.EstimatedMFIAmount) + ", " + SQL.EncodeDateTime(limit.LastModifiedDateTime, DateTime.MinValue) + ")";
        dbQueryBuilder.AppendLine(text);
      }
      try
      {
        TraceLog.WriteInfo(nameof (MFILimitStore), "Execute SQL query for method ResetMFILimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MFILimitStore), ex);
        throw new Exception("Cannot reset MFILimit table. Due to the following problem:\r\n" + ex.Message);
      }
    }

    private static void DeleteAllMFILimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete MFILimits");
      try
      {
        TraceLog.WriteInfo(nameof (MFILimitStore), "Execute SQL query for method DeleteAllMFILimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MFILimitStore), ex);
        throw new Exception("Cannot delete MFILimits record. Due to the following problem:\r\n" + ex.Message);
      }
    }

    private static MFILimit ConvertToMFILimitObj(DataRow row)
    {
      return new MFILimit(Utils.ParseInt(row["ID"]), Utils.ParseInt(row["SourceFileYear"]), string.Concat(row["MSAMDCode"]), string.Concat(row["MSAMDName"]), Utils.ParseInt(row["ActualMFIYear"]), string.Concat(row["ActualMFIAmount"]), Utils.ParseInt(row["EstimatedMFIYear"]), string.Concat(row["EstimatedMFIAmount"]), DateTime.Parse(string.Concat(row["LastModifiedDateTime"])));
    }
  }
}
