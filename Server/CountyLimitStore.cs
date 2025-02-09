// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CountyLimitStore
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
  public sealed class CountyLimitStore
  {
    private const string className = "CountyLimitStore�";

    public static CountyLimit[] GetCountyLimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from CountyLimit");
      List<CountyLimit> countyLimitList = new List<CountyLimit>();
      try
      {
        TraceLog.WriteInfo(nameof (CountyLimitStore), "Execute SQL query for method GetCountyLimits(): '" + dbQueryBuilder.ToString() + "'.");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          countyLimitList.Add(CountyLimitStore.ConvertToCountyLimitObj(row));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (CountyLimitStore), ex);
        throw new Exception("Cannot read CountyLimit table. Due to the following problem:\r\n" + ex.Message);
      }
      return countyLimitList.ToArray();
    }

    public static void UpdateCountyLimits(CountyLimit[] countyLimits)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = "";
      foreach (CountyLimit countyLimit in countyLimits)
      {
        string text = "Update CountyLimit set " + "Limit1 = " + (object) countyLimit.LimitFor1Unit + ", Limit2 = " + (object) countyLimit.LimitFor2Units + ", Limit3 = " + (object) countyLimit.LimitFor3Units + ", Limit4 = " + (object) countyLimit.LimitFor4Units + ", LastModifiedDTTM = " + SQL.EncodeDateTime(countyLimit.LastModifiedDateTime, DateTime.MinValue) + ", Customized = " + SQL.EncodeFlag(countyLimit.Customized) + " Where ID = " + (object) countyLimit.ID;
        dbQueryBuilder.AppendLine(text);
        str = "";
      }
      try
      {
        TraceLog.WriteInfo(nameof (CountyLimitStore), "Execute SQL query for method UpdateCountyLimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (CountyLimitStore), ex);
        throw new Exception("Cannot update CountyLimit table. Due to the following problem:\r\n" + ex.Message);
      }
    }

    private static void DeleteAllCountyLimits()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete CountyLimit");
      try
      {
        TraceLog.WriteInfo(nameof (CountyLimitStore), "Execute SQL query for method DeleteAllCountyLimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (CountyLimitStore), ex);
        throw new Exception("Cannot delete CountyLimit record. Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static void ResetCountyLimits(CountyLimit[] countyLimits)
    {
      CountyLimitStore.DeleteAllCountyLimits();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      foreach (CountyLimit countyLimit in countyLimits)
      {
        string text = "Insert into CountyLimit (MSACode, MetDivisionCode, MSAName, SOACode, LimitType, MedianPrice, Limit1, " + "Limit2, Limit3, Limit4, StateAbb, CountyCode, StateName, CountyName, CountyTransactionDate, LimitTransactionDate, LastModifiedDTTM, Customized) " + "Values ('" + (object) countyLimit.MsaCode + "', '" + (object) countyLimit.MetropolitanDivisionCode + "', " + SQL.EncodeString(countyLimit.MsaName) + ", " + SQL.EncodeString(countyLimit.SoaCode) + ", " + SQL.EncodeString(countyLimit.LimitType) + ", '" + (object) countyLimit.MedianPrice + "', '" + (object) countyLimit.LimitFor1Unit + "', '" + (object) countyLimit.LimitFor2Units + "', '" + (object) countyLimit.LimitFor3Units + "', '" + (object) countyLimit.LimitFor4Units + "', " + SQL.EncodeString(countyLimit.StateAbbreviation) + ", '" + (object) countyLimit.CountyCode + "', " + SQL.EncodeString(countyLimit.StateName) + ", " + SQL.EncodeString(countyLimit.CountyName) + ", " + SQL.EncodeString(countyLimit.CountyTransactionDate) + ", " + SQL.EncodeString(countyLimit.LimitTransactionDate) + ", " + SQL.EncodeDateTime(countyLimit.LastModifiedDateTime, DateTime.MinValue) + ", " + SQL.EncodeFlag(countyLimit.Customized) + ")";
        dbQueryBuilder.AppendLine(text);
      }
      try
      {
        TraceLog.WriteInfo(nameof (CountyLimitStore), "Execute SQL query for method ResetCountyLimits(): " + dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (CountyLimitStore), ex);
        throw new Exception("Cannot reset CountyLimit table. Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static CountyLimit ConvertToCountyLimitObj(DataRow row)
    {
      return new CountyLimit(Utils.ParseInt(row["ID"]), Utils.ParseInt(row["MSACode"]), Utils.ParseInt(row["MetDivisionCode"]), string.Concat(row["MSAName"]), string.Concat(row["SOACode"]), string.Concat(row["LimitType"]), Utils.ParseInt(row["MedianPrice"]), Utils.ParseInt(row["Limit1"]), Utils.ParseInt(row["Limit2"]), Utils.ParseInt(row["Limit3"]), Utils.ParseInt(row["Limit4"]), string.Concat(row["StateAbb"]), Utils.ParseInt(row["CountyCode"]), string.Concat(row["StateName"]), string.Concat(row["CountyName"]), string.Concat(row["CountyTransactionDate"]), string.Concat(row["LimitTransactionDate"]), DateTime.Parse(string.Concat(row["LastModifiedDTTM"])), SQL.DecodeBoolean(row["Customized"]));
    }

    public static int GetCountyLimit(string stateAbb, string countyName, int numOfUnits)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from CountyLimit where lower(CountyName) = " + SQL.EncodeString(countyName.ToLower()) + " and lower(StateAbb) = " + SQL.EncodeString(stateAbb.ToLower()));
      int countyLimit = -999;
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            switch (numOfUnits)
            {
              case 1:
                countyLimit = Utils.ParseInt(dataRowCollection[0]["Limit1"]);
                break;
              case 2:
                countyLimit = Utils.ParseInt(dataRowCollection[0]["Limit2"]);
                break;
              case 3:
                countyLimit = Utils.ParseInt(dataRowCollection[0]["Limit3"]);
                break;
              case 4:
                countyLimit = Utils.ParseInt(dataRowCollection[0]["Limit4"]);
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (CountyLimitStore), ex);
        throw new Exception("Cannot retrieve CountyLimit data. Due to the following problem:\r\n" + ex.Message);
      }
      return countyLimit;
    }

    public static int GetCountyLimitByFips(string stateAbb, string fips, int numOfUnits)
    {
      int countyLimitByFips = -999;
      if (string.IsNullOrEmpty(fips) || fips.Length > 5)
        return countyLimitByFips;
      if (fips.Length == 5)
        fips = fips.Substring(2);
      else if (fips.Length == 4)
        fips = fips.Substring(1);
      fips = Utils.ParseInt((object) fips).ToString();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from CountyLimit where CountyCode = " + SQL.EncodeString(fips) + " and lower(StateAbb) = " + SQL.EncodeString(stateAbb.ToLower()));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            switch (numOfUnits)
            {
              case 1:
                countyLimitByFips = Utils.ParseInt(dataRowCollection[0]["Limit1"]);
                break;
              case 2:
                countyLimitByFips = Utils.ParseInt(dataRowCollection[0]["Limit2"]);
                break;
              case 3:
                countyLimitByFips = Utils.ParseInt(dataRowCollection[0]["Limit3"]);
                break;
              case 4:
                countyLimitByFips = Utils.ParseInt(dataRowCollection[0]["Limit4"]);
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (CountyLimitStore), ex);
        throw new Exception("Cannot retrieve CountyLimit data. Due to the following problem:\r\n" + ex.Message);
      }
      return countyLimitByFips;
    }
  }
}
