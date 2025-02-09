// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.FedTresholdAdjustmentStore
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
  public sealed class FedTresholdAdjustmentStore
  {
    private const string className = "FedTresholdAdjustmentStore�";

    public static FedTresholdAdjustment[] GetFedTresholdAdjustments()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from FedTresholdAdjustment Order by AdjustmentYear DESC, RuleIndex ASC");
      List<FedTresholdAdjustment> tresholdAdjustmentList = new List<FedTresholdAdjustment>();
      try
      {
        TraceLog.WriteInfo(nameof (FedTresholdAdjustmentStore), "Execute SQL query for method GetFedTresholdAdjustments(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          tresholdAdjustmentList.Add(FedTresholdAdjustmentStore.ConvertToFedTresholdObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (FedTresholdAdjustmentStore), ex);
        throw new Exception("Cannot read FedTresholdAdjustment table. Due to the following problem:\r\n" + ex.Message);
      }
      return tresholdAdjustmentList.ToArray();
    }

    public static FedTresholdAdjustment[] GetFedTresholdAdjustments(int year)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from FedTresholdAdjustment where AdjustmentYear = '" + (object) year + "';");
      List<FedTresholdAdjustment> tresholdAdjustmentList = new List<FedTresholdAdjustment>();
      try
      {
        TraceLog.WriteInfo(nameof (FedTresholdAdjustmentStore), "Execute SQL query for method GetFedTresholdAdjustments(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          tresholdAdjustmentList.Add(FedTresholdAdjustmentStore.ConvertToFedTresholdObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (FedTresholdAdjustmentStore), ex);
        throw new Exception("Cannot read FedTresholdAdjustment table. Due to the following problem:\r\n" + ex.Message);
      }
      return tresholdAdjustmentList.ToArray();
    }

    public static void ResetFedTresholdAdjustments(FedTresholdAdjustment[] adjustments)
    {
      FedTresholdAdjustmentStore.DeleteAllFedTresholdAdjustments();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      foreach (FedTresholdAdjustment adjustment in adjustments)
      {
        string text = "Insert into FedTresholdAdjustment " + "Values ('" + (object) adjustment.RuleIndex + "', " + (object) adjustment.AdjustmentYear + ", " + SQL.EncodeString(adjustment.LowerRange) + ", " + SQL.EncodeString(adjustment.UpperRange) + ", " + SQL.EncodeString(adjustment.RuleValue) + ", " + SQL.EncodeString(adjustment.RuleType) + ", " + SQL.EncodeDateTime(adjustment.LastModifiedDateTime, DateTime.MinValue) + ")";
        dbQueryBuilder.AppendLine(text);
      }
      try
      {
        TraceLog.WriteInfo(nameof (FedTresholdAdjustmentStore), "Execute SQL query for method ResetFedTresholdAdjustments(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (FedTresholdAdjustmentStore), ex);
        throw new Exception("Cannot reset FedTresholdAdjustment table. Due to the following problem:\r\n" + ex.Message);
      }
    }

    private static void DeleteAllFedTresholdAdjustments()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete FedTresholdAdjustment");
      try
      {
        TraceLog.WriteInfo(nameof (FedTresholdAdjustmentStore), "Execute SQL query for method DeleteAllFedTresholdAdjustments(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (FedTresholdAdjustmentStore), ex);
        throw new Exception("Cannot delete DeleteAllFedTresholdAdjustments record. Due to the following problem:\r\n" + ex.Message);
      }
    }

    private static FedTresholdAdjustment ConvertToFedTresholdObj(DataRow row)
    {
      return new FedTresholdAdjustment(Utils.ParseInt(row["ID"]), Utils.ParseInt(row["RuleIndex"]), Utils.ParseInt(row["AdjustmentYear"]), string.Concat(row["LowerRange"]), string.Concat(row["UpperRange"]), string.Concat(row["RuleValue"]), string.Concat(row["RuleType"]), DateTime.Parse(string.Concat(row["LastModifiedDateTime"])));
    }
  }
}
