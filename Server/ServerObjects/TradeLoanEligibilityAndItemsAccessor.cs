// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TradeLoanEligibilityAndItemsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Trading;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class TradeLoanEligibilityAndItemsAccessor
  {
    private const string className = "TradeLoanEligibilityAndItemsAccessor�";
    private const string TradeLoanEligibilityPricingTableName = "TradeLoanEligibilityPricingEpps�";
    private const string TradeLoanEligibilityPricingItemTableName = "TradeLoanEligibilityPricingItemEpps�";

    public static int CreateTradeLoanEligibility(
      TradeLoanEligibilityPricingInfo tradeElegibilityInfo)
    {
      if (tradeElegibilityInfo == null)
        return -1;
      return TradeLoanEligibilityAndItemsAccessor.CreateTradeLoanEligibilityDb(tradeElegibilityInfo);
    }

    public static TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityPricingByPricingId(
      int eligibilityPricingId)
    {
      TradeLoanEligibilityPricingInfo eligibilityPricingInfo = (TradeLoanEligibilityPricingInfo) null;
      if (eligibilityPricingId > 0)
        eligibilityPricingInfo = TradeLoanEligibilityAndItemsAccessor.GeTradeLoanEligibilityPricingInfoDb(eligibilityPricingId);
      return eligibilityPricingInfo;
    }

    public static TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityByTradeIdAndLoanId(
      string loanId,
      int tradeId)
    {
      TradeLoanEligibilityPricingInfo eligibilityPricingInfo = (TradeLoanEligibilityPricingInfo) null;
      if (loanId != null)
        eligibilityPricingInfo = TradeLoanEligibilityAndItemsAccessor.GeTradeLoanEligibilityPricingInfoDb(loanId, tradeId);
      return eligibilityPricingInfo;
    }

    public static void DeleteTradeLoanEligibility(int eligibilityPricingId)
    {
      TradeLoanEligibilityAndItemsAccessor.DeleteTradeLoanEligibilityDb(eligibilityPricingId);
    }

    public static void DeleteTradeLoanEligibility(string loanId, int tradeId)
    {
      int eligibilityPricingId = TradeLoanEligibilityAndItemsAccessor.GetTradeLoanEligibilityPricingId(loanId, tradeId);
      if (eligibilityPricingId == 0)
        return;
      TradeLoanEligibilityAndItemsAccessor.DeleteTradeLoanEligibilityDb(eligibilityPricingId);
    }

    public static TradeLoanEligibilityPricingSummary[] GetTradeLoanSummaryByTradeId(int tradeId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "SELECT EligibilityPricingId, ProgramId, TradeId, LoanGUID, IsEligible, IneligibilityReason  FROM  TradeLoanEligibilityPricingEpps WHERE TradeId = " + (object) tradeId;
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<TradeLoanEligibilityPricingSummary> eligibilityPricingSummaryList = new List<TradeLoanEligibilityPricingSummary>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        TradeLoanEligibilityPricingSummary eligibilityPricingSummary = new TradeLoanEligibilityPricingSummary()
        {
          IsEligible = Convert.ToBoolean(dataRowCollection[index]["IsEligible"]),
          ProgramId = Convert.ToInt32(dataRowCollection[index]["ProgramId"]),
          IneligiblityReason = Convert.ToString(dataRowCollection[index]["IneligibilityReason"]),
          EligibilityPricingId = Convert.ToInt32(dataRowCollection[index]["ProgramId"]),
          LoanGuid = Convert.ToString(dataRowCollection[index]["LoanGUID"]),
          TradeId = Convert.ToInt32(dataRowCollection[index]["TradeId"])
        };
        eligibilityPricingSummaryList.Add(eligibilityPricingSummary);
      }
      return eligibilityPricingSummaryList.ToArray();
    }

    private static int CreateTradeLoanEligibilityDb(
      TradeLoanEligibilityPricingInfo tradeElegibilityInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("TradeLoanEligibilityPricingEpps");
      DbValueList eligibilityDbValueList = TradeLoanEligibilityAndItemsAccessor.CreateTradeLoanEligibilityDbValueList(tradeElegibilityInfo);
      dbQueryBuilder.Declare("@EligibilityPricingId", "int");
      dbQueryBuilder.InsertInto(table1, eligibilityDbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@EligibilityPricingId");
      if (tradeElegibilityInfo.TradeLoanEligibilityPricingItems != null && tradeElegibilityInfo.TradeLoanEligibilityPricingItems.Count > 0)
      {
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("TradeLoanEligibilityPricingItemEpps");
        foreach (TradeLoanEligibilityPricingItem eligibilityPricingItem in tradeElegibilityInfo.TradeLoanEligibilityPricingItems)
        {
          DbValueList eligibilityItemDbValueList = TradeLoanEligibilityAndItemsAccessor.CreateTradeLoanEligibilityItemDbValueList(eligibilityPricingItem);
          DbValue dbValue = new DbValue("EligibilityPricingId", (object) "@EligibilityPricingId", (IDbEncoder) DbEncoding.None);
          eligibilityItemDbValueList.Add(dbValue);
          dbQueryBuilder.InsertInto(table2, eligibilityItemDbValueList, true, false);
        }
      }
      dbQueryBuilder.Select("@EligibilityPricingId");
      return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    private static TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityPricingInfoDb(
      int eligibilityPricingId)
    {
      TradeLoanEligibilityPricingInfo eligibilityPricingInfo = (TradeLoanEligibilityPricingInfo) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT tlep.* FROM TradeLoanEligibilityPricingEpps tlep");
      dbQueryBuilder.AppendLine("   WHERE tlep.EligibilityPricingId = " + (object) eligibilityPricingId);
      dbQueryBuilder.AppendLine("SELECT tlepi.* FROM TradeLoanEligibilityPricingItemEpps tlepi");
      dbQueryBuilder.AppendLine("   WHERE tlepi.EligibilityPricingId = " + (object) eligibilityPricingId);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      for (int index = 0; index < table1.Rows.Count; ++index)
        eligibilityPricingInfo = TradeLoanEligibilityAndItemsAccessor.DataRowToTradeEligibilityPricingInfo(table1.Rows[index], table2);
      return eligibilityPricingInfo;
    }

    private static TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityPricingInfoDb(
      string loandId,
      int tradeId)
    {
      int eligibilityPricingId = TradeLoanEligibilityAndItemsAccessor.GetTradeLoanEligibilityPricingId(loandId, tradeId);
      return eligibilityPricingId != 0 ? TradeLoanEligibilityAndItemsAccessor.GeTradeLoanEligibilityPricingInfoDb(eligibilityPricingId) : (TradeLoanEligibilityPricingInfo) null;
    }

    private static int GetTradeLoanEligibilityPricingId(string loanId, int tradeId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@EligibilityPricingId", "int ");
      dbQueryBuilder.AppendLine(" SELECT @EligibilityPricingId = tlep.EligibilityPricingId ");
      dbQueryBuilder.AppendLine("   FROM TradeLoanEligibilityPricingEpps tlep");
      dbQueryBuilder.AppendLine("   WHERE tlep.TradeId = " + (object) tradeId);
      dbQueryBuilder.AppendLine("   AND tlep.LoanGUID = '" + loanId + "'");
      dbQueryBuilder.Select("@EligibilityPricingId");
      return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    private static void DeleteTradeLoanEligibilityDb(int pricingId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("TradeLoanEligibilityPricingEpps");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("TradeLoanEligibilityPricingItemEpps");
      dbQueryBuilder.DeleteFrom(table2, new DbValueList()
      {
        {
          "EligibilityPricingId",
          (object) pricingId
        }
      });
      dbQueryBuilder.DeleteFrom(table1, new DbValueList()
      {
        {
          "EligibilityPricingId",
          (object) pricingId
        }
      });
      dbQueryBuilder.Execute();
    }

    private static TradeLoanEligibilityPricingInfo DataRowToTradeEligibilityPricingInfo(
      DataRow row,
      DataTable tradeEligibilityPricingItemTable)
    {
      List<TradeLoanEligibilityPricingItem> eligibilityPricingItemList = new List<TradeLoanEligibilityPricingItem>();
      if (tradeEligibilityPricingItemTable != null)
      {
        foreach (DataRow row1 in tradeEligibilityPricingItemTable.Select())
          eligibilityPricingItemList.Add(TradeLoanEligibilityAndItemsAccessor.DataRowToTradeEligibilityPricingItem(row1));
      }
      return new TradeLoanEligibilityPricingInfo()
      {
        EligibilityPricingId = SQL.DecodeInt(row["EligibilityPricingId"]),
        TradeId = SQL.DecodeInt(row["TradeId"]),
        LoanGuid = SQL.DecodeString(row["LoanGUID"]),
        ProgramId = SQL.DecodeInt(row["ProgramID"]),
        LoanNumber = SQL.DecodeString(row["LoanNumber"]),
        LoanAmount = SQL.DecodeDecimal(row["LoanAmount"]),
        BasePriceNoteRate = SQL.DecodeDecimal(row["BasePriceNoteRate"]),
        BasePrice = SQL.DecodeDecimal(row["BasePrice"]),
        TotalBuyPrice = SQL.DecodeDecimal(row["TotalBuyPrice"]),
        ExtendedTotalBuyPrice = SQL.DecodeDecimal(row["ExtendedTotalBuyPrice"]),
        IsEligible = SQL.DecodeBoolean(row["IsEligible"]),
        IneligiblityReason = SQL.DecodeString(row["IneligibilityReason"]),
        TradeLoanEligibilityPricingItems = eligibilityPricingItemList
      };
    }

    private static TradeLoanEligibilityPricingItem DataRowToTradeEligibilityPricingItem(DataRow row)
    {
      return new TradeLoanEligibilityPricingItem()
      {
        EligibilityPricingItemId = SQL.DecodeInt(row["EligibilityPricingItemId"]),
        EligibilityPricingId = SQL.DecodeInt(row["EligibilityPricingId"]),
        Order = SQL.DecodeInt(row["Order"]),
        Description = SQL.DecodeString(row["Description"]),
        Rate = SQL.DecodeDecimal(row["Rate"]),
        Price = SQL.DecodeDecimal(row["Price"]),
        Admin = SQL.DecodeInt(row["Admin"]),
        Type = (TradeLoanEligibilityType) SQL.DecodeInt(row["Type"]),
        TypeText = SQL.DecodeString(row["TypeText"])
      };
    }

    private static DbValueList CreateTradeLoanEligibilityDbValueList(
      TradeLoanEligibilityPricingInfo tradeEligibilityPricingInfo)
    {
      return new DbValueList()
      {
        {
          "TradeId",
          (object) tradeEligibilityPricingInfo.TradeId
        },
        {
          "LoanGUID",
          (object) tradeEligibilityPricingInfo.LoanGuid
        },
        {
          "ProgramID",
          (object) tradeEligibilityPricingInfo.ProgramId
        },
        {
          "LoanNumber",
          (object) tradeEligibilityPricingInfo.LoanNumber
        },
        {
          "LoanAmount",
          (object) tradeEligibilityPricingInfo.LoanAmount
        },
        {
          "BasePriceNoteRate",
          (object) tradeEligibilityPricingInfo.BasePriceNoteRate
        },
        {
          "BasePrice",
          (object) tradeEligibilityPricingInfo.BasePrice
        },
        {
          "TotalBuyPrice",
          (object) tradeEligibilityPricingInfo.TotalBuyPrice
        },
        {
          "ExtendedTotalBuyPrice",
          (object) tradeEligibilityPricingInfo.ExtendedTotalBuyPrice
        },
        {
          "IsEligible",
          (object) new SqlBoolean(tradeEligibilityPricingInfo.IsEligible).ByteValue
        },
        {
          "IneligibilityReason",
          (object) tradeEligibilityPricingInfo.IneligiblityReason
        }
      };
    }

    private static DbValueList CreateTradeLoanEligibilityItemDbValueList(
      TradeLoanEligibilityPricingItem info)
    {
      return new DbValueList()
      {
        {
          "Order",
          (object) info.Order
        },
        {
          "Description",
          (object) info.Description
        },
        {
          "Rate",
          (object) info.Rate
        },
        {
          "Price",
          (object) info.Price
        },
        {
          "Admin",
          (object) info.Admin
        },
        {
          "Type",
          (object) (int) info.Type
        },
        {
          "TypeText",
          (object) info.TypeText
        }
      };
    }
  }
}
