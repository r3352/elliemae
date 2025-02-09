// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LockExtensionPriceAdjustmentcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LockExtensionPriceAdjustmentcessor
  {
    private const string className = "LockExtensionPriceAdjustmentcessor�";
    private const string tableName = "LockExtensionPriceAdjustments�";

    private LockExtensionPriceAdjustmentcessor()
    {
    }

    public static LockExtensionPriceAdjustment[] GetLockExtensionPriceAdjustments()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from LockExtensionPriceAdjustments order by DaysToExtend");
      return LockExtensionPriceAdjustmentcessor.translateToObject(dbQueryBuilder.Execute());
    }

    private static LockExtensionPriceAdjustment[] translateToObject(DataRowCollection rawResult)
    {
      List<LockExtensionPriceAdjustment> extensionPriceAdjustmentList = new List<LockExtensionPriceAdjustment>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) rawResult)
        extensionPriceAdjustmentList.Add(new LockExtensionPriceAdjustment(Utils.ParseInt(dataRow["DaysToExtend"], 0), Utils.ParseDecimal(dataRow["PriceAdjustment"], 0M, 3)));
      extensionPriceAdjustmentList.Sort();
      return extensionPriceAdjustmentList.ToArray();
    }

    public static bool UpdateLockExtensionPriceAdjustment(LockExtensionPriceAdjustment[] adjustments)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete LockExtensionPriceAdjustments");
      foreach (LockExtensionPriceAdjustment adjustment in adjustments)
        dbQueryBuilder.AppendLine("insert into LockExtensionPriceAdjustments (DaysToExtend, PriceAdjustment) values (" + (object) adjustment.DaysToExtend + ", " + adjustment.PriceAdjustment.ToString("#.000") + ")");
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public static bool DeleteLockExtensionPriceAdjustments(
      LockExtensionPriceAdjustment[] adjustments)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        foreach (LockExtensionPriceAdjustment adjustment in adjustments)
          dbQueryBuilder.AppendLine("delete LockExtensionPriceAdjustments where DaysToExtend = " + (object) adjustment.DaysToExtend);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }
  }
}
