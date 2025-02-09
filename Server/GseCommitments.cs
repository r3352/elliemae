// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.GseCommitments
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Collections;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class GseCommitments
  {
    private static string className = nameof (GseCommitments);

    public static List<TradeType> BuySideTradeTypes
    {
      get
      {
        return new List<TradeType>()
        {
          TradeType.CorrespondentTrade
        };
      }
    }

    public static List<TradeType> SellSideTradeTypes
    {
      get
      {
        return new List<TradeType>()
        {
          TradeType.LoanTrade,
          TradeType.MbsPool,
          TradeType.GSECommitment
        };
      }
    }

    public static List<TradeType> TheTradeType
    {
      get
      {
        return new List<TradeType>()
        {
          TradeType.GSECommitment
        };
      }
    }

    public static GSECommitmentInfo[] GetAllTrades()
    {
      return GseCommitments.getTradesFromDatabase("select TradeID from GseCommitmentDetails", "TradeID");
    }

    public static GSECommitmentInfo GetTrade(int tradeId)
    {
      GSECommitmentInfo[] tradesFromDatabase = GseCommitments.getTradesFromDatabase(string.Concat((object) tradeId), "TradeID");
      return tradesFromDatabase.Length == 0 ? (GSECommitmentInfo) null : tradesFromDatabase[0];
    }

    private static GSECommitmentInfo[] getTradesFromDatabase(string criteria, string columnName = "TradeID�")
    {
      if ((criteria ?? "") == "")
        criteria = "select " + columnName + " from GseCommitmentDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select GseCommitmentDetails.*, TradeObjects.*, GseCommitmentSummary.* from GseCommitmentDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on GseCommitmentDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetGseCommitmentSummaryInline(" + (object) 1 + ") GseCommitmentSummary on GseCommitmentDetails.TradeID = GseCommitmentSummary.TradeID");
      dbQueryBuilder.AppendLine("   where GseCommitmentDetails." + columnName + " in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select g.TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment ta");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on ta.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" inner join gsecommitments g on g.ContractNumber = ta.CommitmentContractNumber");
      dbQueryBuilder.AppendLine(" where g." + columnName + " in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by g.TradeID, PendingStatus");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      GSECommitmentInfo[] tradesFromDatabase = new GSECommitmentInfo[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        GSECommitmentInfo tradeInfo = GseCommitments.dataRowToTradeInfo(table1.Rows[index], table2);
        tradesFromDatabase[index] = tradeInfo;
      }
      return tradesFromDatabase;
    }

    private static GSECommitmentViewModel[] getTradeViewsFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from GseCommitmentDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select GseCommitmentDetails.*, TradeObjects.*, GseCommitmentSummary.* ");
      dbQueryBuilder.AppendLine("   from GseCommitmentDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on GseCommitmentDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left join FN_GetGseCommitmentSummaryInline(" + (object) 1 + ") GseCommitmentSummary on GseCommitmentDetails.TradeID = GseCommitmentSummary.TradeID");
      dbQueryBuilder.AppendLine("   where GseCommitmentDetails.TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by TradeID, PendingStatus");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      GSECommitmentViewModel[] viewsFromDatabase = new GSECommitmentViewModel[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        GSECommitmentViewModel tradeViewInfo = GseCommitments.dataRowToTradeViewInfo(table1.Rows[index], table2);
        viewsFromDatabase[index] = tradeViewInfo;
      }
      return viewsFromDatabase;
    }

    private static GSECommitmentViewModel dataRowToTradeViewInfo(DataRow r, DataTable statusTable)
    {
      GseCommitments.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      GSECommitmentViewModel tradeViewInfo = new GSECommitmentViewModel();
      tradeViewInfo.TradeID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]);
      tradeViewInfo.Guid = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]);
      tradeViewInfo.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]);
      tradeViewInfo.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      tradeViewInfo.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      tradeViewInfo.CommitmentAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CommitmentAmount"]);
      tradeViewInfo.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]);
      tradeViewInfo.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"]);
      tradeViewInfo.BuyupBuydownGrid = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BuyupBuydownGrid"]);
      tradeViewInfo.ContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]);
      tradeViewInfo.FulfilledAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["FulfilledAmount"]);
      tradeViewInfo.IssueMonth = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["IssueMonth"]);
      tradeViewInfo.MaxBuyupAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxBuyupAmount"]);
      tradeViewInfo.MaxDeliveryAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxDeliveryAmount"]);
      tradeViewInfo.MaxRemainingAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxRemainingAmount"]);
      tradeViewInfo.MinDeliveryAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinDeliveryAmount"]);
      tradeViewInfo.MinRemainingAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinRemainingAmount"]);
      tradeViewInfo.OutstandingBalance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OutstandingBalance"]);
      tradeViewInfo.PairOffAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]);
      tradeViewInfo.PairOffFeeFactor = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffFeeFactor"]);
      tradeViewInfo.ParticipationPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ParticipationPercent"]);
      tradeViewInfo.PendingAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PendingAmount"]);
      tradeViewInfo.RemittanceCycle = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RemittanceCycle"]);
      tradeViewInfo.RemittanceCycleMonth = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["RemittanceDayOfMonth"]);
      tradeViewInfo.RollFeeFactor = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RollFeeFactor"]);
      tradeViewInfo.SellerNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SellerNumber"]);
      tradeViewInfo.ServicingOption = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ServicingOption"]);
      tradeViewInfo.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]);
      tradeViewInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OpenAmount"]);
      tradeViewInfo.RolledFrom = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RolledFrom"]);
      tradeViewInfo.RolledTo = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RolledTo"]);
      tradeViewInfo.BondType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BondType"]);
      tradeViewInfo.MinGFeeAfterBuydown = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinGFeeAfterBuydown"]);
      tradeViewInfo.FalloutAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["FalloutAmount"]);
      tradeViewInfo.Fees = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Fees"]);
      tradeViewInfo.RolledAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RolledAmount"]);
      tradeViewInfo.CompletionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      return tradeViewInfo;
    }

    private static GSECommitmentInfo dataRowToTradeInfo(DataRow r, DataTable statusTable)
    {
      GseCommitments.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      GSECommitmentInfo tradeInfo = new GSECommitmentInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), "");
      tradeInfo.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"], "");
      tradeInfo.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0);
      tradeInfo.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"], false);
      tradeInfo.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      tradeInfo.CommitmentAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CommitmentAmount"]);
      tradeInfo.BondType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BondType"], "");
      tradeInfo.BuyUpBuyDownGrid = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BuyUpBuyDownGrid"], "");
      tradeInfo.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      tradeInfo.ContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]);
      tradeInfo.FulfilledAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["FulfilledAmount"]);
      tradeInfo.IssueMonth = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["IssueMonth"]);
      tradeInfo.MaxBuyupAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxBuyupAmount"]);
      tradeInfo.MaxDeliveryAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxDeliveryAmount"]);
      tradeInfo.MaxRemainingAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxRemainingAmount"]);
      tradeInfo.MinDeliveryAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinDeliveryAmount"]);
      tradeInfo.MinRemainingAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinRemainingAmount"]);
      tradeInfo.OutstandingBalance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OutstandingBalance"]);
      tradeInfo.PairOffAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]);
      tradeInfo.PairOffFeeFactor = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffFeeFactor"]);
      tradeInfo.ParticipationPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ParticipationPercent"]);
      tradeInfo.PendingAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PendingAmount"]);
      tradeInfo.RemittanceCycle = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RemittanceCycle"]);
      tradeInfo.RemittanceDayOfMonth = (Decimal) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["RemittanceDayOfMonth"]);
      tradeInfo.RollFeeFactor = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RollFeeFactor"]);
      tradeInfo.SellerNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SellerNumber"]);
      tradeInfo.ServicingOption = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ServicingOption"]);
      tradeInfo.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]);
      tradeInfo.FalloutAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["FalloutAmount"]);
      tradeInfo.Fees = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Fees"]);
      tradeInfo.MinGFeeAfterBuydown = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinGFeeAfterBuydown"]);
      tradeInfo.RolledAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RolledAmount"]);
      tradeInfo.RolledFrom = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RolledFrom"]);
      tradeInfo.RolledTo = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RolledTo"]);
      tradeInfo.Notes = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Notes"]);
      tradeInfo.GSECommitmentPairOffs = BinaryConvertible<GSECommitmentPairOffs>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PairOffsXml"]));
      tradeInfo.ProductNames = BinaryConvertible<FannieMaeProducts>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ProductNameXml"]));
      tradeInfo.PriceAdjustments = TradePriceAdjustments.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AdjustmentsXml"]));
      tradeInfo.GuarantyFees = BinaryConvertible<GuarantyFeeItems>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["GuarantyFeeXml"]));
      tradeInfo.BuyUpDownItems = BinaryConvertible<MbsPoolBuyUpDownItems>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BuyUpDownXml"]));
      tradeInfo.OutstandingBalanceLock = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OutstandingBalanceLock"]) == "Y";
      tradeInfo.PairOffAmountLock = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PairOffAmountLock"]) == "Y";
      tradeInfo.Pricing = BinaryConvertible<TradePricingInfo>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PricingXml"]));
      return tradeInfo;
    }

    public static GseCommitmentHistoryItem[] GetTradeHistory(int tradeId)
    {
      return GseCommitments.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where TradeID = " + (object) tradeId);
    }

    private static GseCommitmentHistoryItem[] getTradeHistoryFromDatabase(string selectionQuery)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select TradeHistory.*, LoanSummary.LoanNumber, Trades.Name as TradeName, Trades.InvestorName, MasterContracts.ContractNumber");
      dbQueryBuilder.AppendLine("from TradeHistory inner join Trades on TradeHistory.TradeID = Trades.TradeID and TradeType = 7");
      dbQueryBuilder.AppendLine("   left outer join LoanSummary on TradeHistory.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on TradeHistory.ContractID = MasterContracts.ContractID");
      if ((selectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("where TradeHistory.HistoryID in (" + selectionQuery + ")");
      return GseCommitments.dataRowsToTradeHistoryItems((IEnumerable) dbQueryBuilder.Execute());
    }

    private static GseCommitmentHistoryItem[] dataRowsToTradeHistoryItems(IEnumerable rows)
    {
      List<GseCommitmentHistoryItem> commitmentHistoryItemList = new List<GseCommitmentHistoryItem>();
      foreach (DataRow row in rows)
        commitmentHistoryItemList.Add(GseCommitments.dataRowToHistoryItem(row));
      return commitmentHistoryItemList.ToArray();
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      int tradeId,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      return GseCommitments.GetAssignedOrPendingLoans(user, GseCommitments.GetTrade(tradeId) ?? throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Template, (object) tradeId), fields, isExternalOrganization, sqlRead);
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      GSECommitmentInfo trade,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      string identitySelectionQuery = "select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid  inner join gsecommitments g on g.contractnumber = TradeAssignment.commitmentcontractNumber where g.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 1;
      return Pipeline.Generate(user, identitySelectionQuery, fields, PipelineData.Trade, isExternalOrganization, TradeType.GSECommitment, sqlRead);
    }

    public static int CreateTrade(GSECommitmentInfo info, UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (GseCommitments));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      dbQueryBuilder.Declare("@tradeId", "int");
      dbQueryBuilder.InsertInto(table1, TradeUtils.CreateTradesValueList((TradeInfoObj) info), true, false);
      dbQueryBuilder.SelectIdentity("@tradeId");
      DbValue dbValue = new DbValue("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
      DbValueList commitmentValueList = GseCommitments.createGseCommitmentValueList(info);
      commitmentValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table2, commitmentValueList, true, false);
      DbValueList objectsValueList = GseCommitments.createTradeObjectsValueList(info);
      objectsValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.Select("@tradeId");
      int tradeId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      info = GseCommitments.GetTrade(tradeId);
      GseCommitments.AddTradeHistoryItem(new GseCommitmentHistoryItem(info, TradeHistoryAction.TradeCreated, currentUser));
      if (info.Status != TradeStatus.Open)
        GseCommitments.AddTradeHistoryItem(new GseCommitmentHistoryItem(info, info.Status, currentUser));
      if (info.Locked)
        GseCommitments.AddTradeHistoryItem(new GseCommitmentHistoryItem(info, TradeHistoryAction.TradeLocked, currentUser));
      return tradeId;
    }

    private static DbValueList createGseCommitmentValueList(GSECommitmentInfo info)
    {
      return new DbValueList()
      {
        {
          "ContractNumber",
          (object) info.ContractNumber
        },
        {
          "SellerNumber",
          (object) info.SellerNumber
        },
        {
          "IssueMonth",
          (object) info.IssueMonth
        },
        {
          "FulfilledAmount",
          (object) info.FulfilledAmount
        },
        {
          "PendingAmount",
          (object) info.PendingAmount
        },
        {
          "FalloutAmount",
          (object) info.FalloutAmount
        },
        {
          "RolledAmount",
          (object) info.RolledAmount
        },
        {
          "MaxRemainingAmount",
          (object) info.MaxRemainingAmount
        },
        {
          "MinRemainingAmount",
          (object) info.MinRemainingAmount
        },
        {
          "Fees",
          (object) info.Fees
        },
        {
          "SellerName",
          (object) info.SellerNumber
        },
        {
          "MinDeliveryAmount",
          (object) info.MinDeliveryAmount
        },
        {
          "MaxDeliveryAmount",
          (object) info.MaxDeliveryAmount
        },
        {
          "OutstandingBalance",
          (object) info.OutstandingBalance
        },
        {
          "BuyupBuydownGrid",
          (object) info.BuyUpBuyDownGrid
        },
        {
          "MaxBuyupAmount",
          (object) info.MaxBuyupAmount
        },
        {
          "MinGFeeAfterBuydown",
          (object) info.MinGFeeAfterBuydown
        },
        {
          "RemittanceCycle",
          (object) info.RemittanceCycle
        },
        {
          "RemittanceDayOfMonth",
          (object) info.RemittanceDayOfMonth
        },
        {
          "ServicingOption",
          (object) info.ServicingOption
        },
        {
          "BondType",
          (object) info.BondType
        },
        {
          "ParticipationPercent",
          (object) info.ParticipationPercent
        },
        {
          "PairOffFeeFactor",
          (object) info.PairOffFeeFactor
        },
        {
          "RollFeeFactor",
          (object) info.RollFeeFactor
        },
        {
          "RolledTo",
          (object) info.RolledTo
        },
        {
          "RolledFrom",
          (object) info.RolledFrom
        },
        {
          "OutstandingBalanceLock",
          info.OutstandingBalanceLock ? (object) "Y" : (object) "N"
        },
        {
          "PairOffAmountLock",
          info.PairOffAmountLock ? (object) "Y" : (object) "N"
        }
      };
    }

    public static int AddTradeHistoryItem(GseCommitmentHistoryItem item)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("TradeHistory");
      dbQueryBuilder.InsertInto(table1, new DbValueList()
      {
        {
          "TradeID",
          (object) item.TradeID
        },
        {
          "LoanGuid",
          (object) item.LoanGuid,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "UserID",
          (object) item.UserID
        },
        {
          "Action",
          (object) (int) item.Action
        },
        {
          "Status",
          (object) item.Status,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "Timestamp",
          (object) "GetDate()",
          (IDbEncoder) DbEncoding.None
        },
        {
          "DataXml",
          (object) item.Data.ToXml()
        }
      }, true, false);
      dbQueryBuilder.SelectIdentity();
      if (item.Action == TradeHistoryAction.LoanRejected)
      {
        DbTableInfo table2 = DbAccessManager.GetTable("TradeRejections");
        DbValueList dbValueList = new DbValueList();
        dbValueList.Add("LoanGuid", (object) item.LoanGuid);
        dbQueryBuilder.IfNotExists(table2, dbValueList);
        dbValueList.Add("Timestamp", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.InsertInto(table2, dbValueList, true, false);
      }
      return EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    public static void UpdateTrade(
      GSECommitmentInfo info,
      UserInfo currentUser,
      bool isExternalOrganization)
    {
      GSECommitmentInfo trade = GseCommitments.GetTrade(info.TradeID);
      if (trade == null)
        throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) info.TradeID);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (GseCommitments));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      DbValue key = new DbValue("TradeID", (object) info.TradeID);
      dbQueryBuilder.DeleteFrom(table3, key);
      dbQueryBuilder.Update(table1, TradeUtils.CreateTradesValueList((TradeInfoObj) info), key);
      dbQueryBuilder.Update(table2, GseCommitments.createGseCommitmentValueList(info), key);
      DbValueList objectsValueList = GseCommitments.createTradeObjectsValueList(info);
      objectsValueList.Add(key);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
      if (info.Locked != trade.Locked)
        GseCommitments.AddTradeHistoryItem(new GseCommitmentHistoryItem(info, info.Locked ? TradeHistoryAction.TradeLocked : TradeHistoryAction.TradeUnlocked, currentUser));
      if (info.Status == trade.Status)
        return;
      GseCommitments.AddTradeHistoryItem(new GseCommitmentHistoryItem(info, info.Status, currentUser));
    }

    public static void RecalculateProfits(GSECommitmentInfo trade, bool isExternalOrganization)
    {
      PipelineInfo[] assignedLoans = GseCommitments.GetAssignedLoans(trade, GseCommitments.GeneratePricingFieldList(trade), isExternalOrganization);
      GseCommitments.RecalculateProfits(trade, assignedLoans);
    }

    public static string[] GeneratePricingFieldList(GSECommitmentInfo trade)
    {
      return GseCommitments.GeneratePricingFieldList((IEnumerable<GSECommitmentInfo>) new GSECommitmentInfo[1]
      {
        trade
      });
    }

    public static string[] GeneratePricingFieldList(IEnumerable<GSECommitmentInfo> trades)
    {
      return new List<string>().ToArray();
    }

    public static PipelineInfo[] GetAssignedLoans(
      GSECommitmentInfo trade,
      string[] fields,
      bool isExternalOrganization)
    {
      return Pipeline.Generate("select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid  where TradeAssignment.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 2, fields, PipelineData.Trade, isExternalOrganization, TradeType.GSECommitment);
    }

    public static void RecalculateProfits(GSECommitmentInfo trade, PipelineInfo[] pinfos)
    {
      ChunkedList<PipelineInfo> chunkedList = new ChunkedList<PipelineInfo>(pinfos, 50);
      PipelineInfo[] pipelineInfoArray = (PipelineInfo[]) null;
      while ((pipelineInfoArray = chunkedList.Next()) != null)
        new DbQueryBuilder().ExecuteNonQuery();
    }

    public static GSECommitmentInfo GetTradeByName(string tradeName)
    {
      GSECommitmentInfo[] tradesFromDatabase = GseCommitments.getTradesFromDatabase("select TradeID from Trades where Name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(tradeName) + "'", "TradeID");
      return tradesFromDatabase.Length == 0 ? (GSECommitmentInfo) null : tradesFromDatabase[0];
    }

    public static GSECommitmentInfo GetTradeByContractNumber(string contractNumber)
    {
      GSECommitmentInfo[] tradesFromDatabase = GseCommitments.getTradesFromDatabase("select ContractNumber from gsecommitments where ContractNumber like '" + EllieMae.EMLite.DataAccess.SQL.Escape(contractNumber) + "'", nameof (contractNumber));
      return tradesFromDatabase.Length == 0 ? (GSECommitmentInfo) null : tradesFromDatabase[0];
    }

    private static DbValueList createTradeObjectsValueList(GSECommitmentInfo info)
    {
      return new DbValueList()
      {
        {
          "FilterQueryXml",
          info.Filter == null ? (object) (string) null : (object) info.Filter.ToXml()
        },
        {
          "InvestorXml",
          info.Investor == null ? (object) (string) null : (object) info.Investor.ToXml()
        },
        {
          "PairOffsXml",
          info.GSECommitmentPairOffs == null ? (object) (string) null : (object) info.GSECommitmentPairOffs.ToXml()
        },
        {
          "GuarantyFeeXml",
          info.GuarantyFees == null ? (object) (string) null : (object) info.GuarantyFees.ToXml()
        },
        {
          "AdjustmentsXml",
          info.PriceAdjustments == null ? (object) (string) null : (object) info.PriceAdjustments.ToXml()
        },
        {
          "DealerXml",
          info.Dealer == null ? (object) (string) null : (object) info.Dealer.ToXml()
        },
        {
          "AssigneeXml",
          info.Assignee == null ? (object) (string) null : (object) info.Assignee.ToXml()
        },
        {
          "Notes",
          info.Notes == null ? (object) (string) null : (object) info.Notes
        },
        {
          "BuyUpDownXml",
          info.BuyUpDownItems == null ? (object) (string) null : (object) info.BuyUpDownItems.ToXml()
        },
        {
          "ProductNameXml",
          info.ProductNames == null ? (object) (string) null : (object) info.ProductNames.ToXml()
        },
        {
          "PricingXml",
          info.Pricing == null ? (object) (string) null : (object) info.Pricing.ToXml()
        }
      };
    }

    private static Dictionary<GseCommitmentLoanStatus, int> dataRowsToPendingLoanCounts(
      IEnumerable rows)
    {
      Dictionary<GseCommitmentLoanStatus, int> pendingLoanCounts = new Dictionary<GseCommitmentLoanStatus, int>();
      foreach (DataRow row in rows)
        pendingLoanCounts[(GseCommitmentLoanStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["PendingStatus"])] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["LoanCount"]);
      return pendingLoanCounts;
    }

    private static GseCommitmentHistoryItem dataRowToHistoryItem(DataRow r)
    {
      return new GseCommitmentHistoryItem(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["HistoryID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), (TradeHistoryAction) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Action"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Timestamp"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UserID"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["DataXml"]));
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      int[] tradeIds,
      string[] fields,
      bool isExternalOrganization)
    {
      return GseCommitments.GetEligibleLoans(user, GseCommitments.GetTrades(tradeIds), fields, (SortField[]) null, isExternalOrganization);
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      GSECommitmentInfo[] trades,
      string[] fields,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      QueryCriterion queryCriterion1 = (QueryCriterion) null;
      foreach (GSECommitmentInfo trade in trades)
      {
        if (trade != null && trade.Filter != null)
        {
          QueryCriterion queryCriterion2 = ((TradeFilterEvaluator) trade.Filter.CreateEvaluator(typeof (GseCommitmentFilterEvaluator))).ToQueryCriterion((TradeInfoObj) trade, filterOption);
          if (queryCriterion2 != null)
            queryCriterion1 = queryCriterion1 == null ? queryCriterion2 : queryCriterion2.Or(queryCriterion1);
        }
      }
      bool flag = false;
      if (fields.Length == 0 && ((IEnumerable<SortField>) sortOrder).Any<SortField>((System.Func<SortField, bool>) (s => s.Term.FieldName.StartsWith("TradeEligibility"))))
      {
        fields = new string[3]
        {
          "Loan.LoanRate",
          "Loan.TotalBuyPrice",
          "Loan.TotalLoanAmount"
        };
        flag = true;
      }
      PipelineInfo[] pinfos = Pipeline.Generate(user, LoanInfo.Right.Access, fields, PipelineData.Trade, queryCriterion1, ((IEnumerable<SortField>) sortOrder).Where<SortField>((System.Func<SortField, bool>) (s => !s.Term.FieldName.StartsWith("TradeEligibility"))).ToArray<SortField>(), isExternalOrganization, TradeType.GSECommitment);
      foreach (PipelineInfo pinfo in pinfos)
        GseCommitments.PopulateTradeProfitData(pinfo, trades);
      if (flag)
        GseCommitments.SortByCalculatedData(pinfos, sortOrder);
      return pinfos;
    }

    private static void SortByCalculatedData(PipelineInfo[] pinfos, SortField[] sortFields)
    {
      foreach (SortField sortOrder in ((IEnumerable<SortField>) sortFields).Where<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.StartsWith("TradeEligibility"))))
        Array.Sort((Array) pinfos, (IComparer) new LoanTrades.TradeLoanCursorItemComparer(sortOrder));
    }

    public static GSECommitmentInfo[] GetTrades(int[] tradeIds)
    {
      return GseCommitments.getTradesFromDatabase(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds), "TradeID");
    }

    public static void PopulateTradeProfitData(PipelineInfo pinfo, GSECommitmentInfo[] trades)
    {
      PipelineInfo.TradeInfo[] tradeInfoArray = new PipelineInfo.TradeInfo[trades.Length];
      pinfo.EligibleTrades = tradeInfoArray;
    }

    public static int[] QueryTradeIds(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      StringBuilder stringBuilder = new StringBuilder(GseCommitments.getQueryTradeIdsSql(user, criteria, sortOrder, isExternalOrganization));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(stringBuilder.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["TradeID"]);
      return numArray;
    }

    public static string getQueryTradeIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      try
      {
        string fieldList = "GseCommitmentDetails.TradeID";
        GseCommitmentQuery gseCommitmentQuery = new GseCommitmentQuery(user);
        return GseCommitments.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) gseCommitmentQuery, isExternalOrganization).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(GseCommitments.className, ex);
        return (string) null;
      }
    }

    private static DbQueryBuilder generateQuerySql(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortFields,
      QueryEngine queryEngine,
      bool isExternalOrganization)
    {
      DbQueryBuilder querySql = new DbQueryBuilder();
      querySql.AppendLine("select " + fieldList + " ");
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(fieldList.Replace(" ", "").Split(','));
      QueryCriterion filter = QueryCriterion.Join(criteria, BinaryOperator.And);
      querySql.Append(queryEngine.GetTableSelectionClause(fields, filter, sortFields, true, true, isExternalOrganization));
      querySql.Append(GseCommitments.OrderbySettlementMonth(queryEngine, sortFields));
      return querySql;
    }

    public static Dictionary<int, GSECommitmentViewModel> GetTradeViews(int[] tradeIds)
    {
      List<GSECommitmentViewModel> commitmentViewModelList = new List<GSECommitmentViewModel>();
      foreach (int tradeId in tradeIds)
        commitmentViewModelList.AddRange((IEnumerable<GSECommitmentViewModel>) GseCommitments.getTradeViewsFromDatabase(string.Concat((object) tradeId)));
      Dictionary<int, GSECommitmentViewModel> tradeViews = new Dictionary<int, GSECommitmentViewModel>();
      foreach (GSECommitmentViewModel commitmentViewModel in commitmentViewModelList.ToArray())
      {
        if (commitmentViewModel != null)
          tradeViews[commitmentViewModel.TradeID] = commitmentViewModel;
      }
      for (int key = 0; key < tradeIds.Length; ++key)
      {
        if (!tradeViews.ContainsKey(tradeIds[key]))
          tradeViews[key] = (GSECommitmentViewModel) null;
      }
      return tradeViews;
    }

    public static GSECommitmentViewModel GetTradeView(int tradeId)
    {
      GSECommitmentViewModel[] viewsFromDatabase = GseCommitments.getTradeViewsFromDatabase(string.Concat((object) tradeId));
      return viewsFromDatabase.Length == 0 ? (GSECommitmentViewModel) null : viewsFromDatabase[0];
    }

    public static void DeleteTrade(int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("TradeAssignmentByTrade");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (GseCommitments));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeHistory");
      DbValue key = new DbValue("TradeID", (object) tradeId);
      dbQueryBuilder.AppendLine("delete TradeAssignmentByTrade where AssigneeTradeID = " + (object) tradeId);
      dbQueryBuilder.DeleteFrom(table1, key);
      dbQueryBuilder.DeleteFrom(table2, key);
      dbQueryBuilder.DeleteFrom(table3, key);
      dbQueryBuilder.ExecuteNonQuery();
      TradeUtils.DeleteTrade(tradeId);
    }

    public static void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update Trades set status = " + (object) (int) status + " where TradeID in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds) + ")");
      dbQueryBuilder.ExecuteNonQuery();
      foreach (GSECommitmentInfo trade in GseCommitments.GetTrades(tradeIds))
        GseCommitments.AddTradeHistoryItem(new GseCommitmentHistoryItem(trade, action, currentUser));
    }

    public static GSECommitmentViewModel GetTradeViewForLoan(string guid)
    {
      GSECommitmentViewModel[] viewsFromDatabase = GseCommitments.getTradeViewsFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, GseCommitments.TheTradeType));
      return viewsFromDatabase.Length == 0 ? (GSECommitmentViewModel) null : viewsFromDatabase[0];
    }

    public static GSECommitmentInfo GetTradeForLoan(string guid)
    {
      GSECommitmentInfo[] tradesFromDatabase = GseCommitments.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, GseCommitments.TheTradeType), "TradeID");
      return tradesFromDatabase.Length == 0 ? (GSECommitmentInfo) null : tradesFromDatabase[0];
    }

    public static GSECommitmentInfo GetTradeForRejectedLoan(string guid)
    {
      GSECommitmentInfo[] tradesFromDatabase = GseCommitments.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and isnull(Rejected, 0) = 1 and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), GseCommitments.TheTradeType), "TradeID");
      return tradesFromDatabase.Length == 0 ? (GSECommitmentInfo) null : tradesFromDatabase[0];
    }

    public static GSECommitmentViewModel[] GetActiveTradeView()
    {
      return GseCommitments.getTradeViewsFromDatabase("select TradeID from Trades where Status <> " + (object) 5);
    }

    public static GSECommitmentViewModel[] GetTradeViewsByContractID(int contractId)
    {
      return GseCommitments.getTradeViewsFromDatabase("select TradeID from Trades where ContractID = " + (object) contractId);
    }

    public static GSECommitmentInfo[] GetGseCommitmentsByContractID(int contractId)
    {
      return GseCommitments.getTradesFromDatabase("select TradeID from Trades where ContractID = " + (object) contractId, "TradeID");
    }

    public static void DeleteLoanFromTrades(
      string loanGuid,
      UserInfo currentUser,
      bool isExternalOrganization)
    {
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      GseCommitmentLoanStatus[] statuses,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      GseCommitments.SetPendingTradeStatus(tradeId, loanIds, statuses, (string[]) null, currentUser, isExternalOrganization, removePendingLoan);
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      GseCommitmentLoanStatus[] statuses,
      string[] comments,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      if (loanIds.Length == 0)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = loanIds.Length > 1 ? "'" + string.Join("', '", loanIds) + "'" : "'" + loanIds[0] + "'";
      dbQueryBuilder.AppendLine(string.Format("update TradeAssignment set PendingStatus = 0 where PendingStatus = AssignedStatus and tradeId = {0} and loanGuid in ({1})", (object) tradeId, (object) str));
      dbQueryBuilder.AppendLine(string.Format("delete from TradeAssignment where AssignedStatus = " + (object) 1 + " And PendingStatus = 0 And tradeId = {0} and loanGuid in ({1})", (object) tradeId, (object) str));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static bool setPendingTradeStatus(
      GSECommitmentViewModel trade,
      PipelineInfo loanInfo,
      GseCommitmentLoanStatus loanStatus,
      UserInfo currentUser,
      bool removePendingLoan)
    {
      return GseCommitments.setPendingTradeStatus(trade, loanInfo, loanStatus, "", currentUser, removePendingLoan);
    }

    private static bool setPendingTradeStatus(
      GSECommitmentViewModel trade,
      PipelineInfo loanInfo,
      GseCommitmentLoanStatus loanStatus,
      string comment,
      UserInfo currentUser,
      bool removePendingLoan)
    {
      if (loanStatus == GseCommitmentLoanStatus.None)
        return false;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) trade.TradeID);
      dbValueList.Add("LoanGuid", (object) loanInfo.GUID);
      DbValueList values = new DbValueList();
      values.Add("PendingStatus", (object) (int) loanStatus);
      values.Add("PendingStatusDate", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
      if (!removePendingLoan)
      {
        string format = "\r\n                    if exists (select 1\r\n                        from TradeAssignment ta\r\n                        inner join trades t on ta.tradeid = t.tradeid\r\n                        where ta.TradeID <> {0} And LoanGuid = {1}\r\n                        and TradeType in (2, 3)\r\n                        and ta.AssignedStatus >= {2}\r\n                    )\r\n                    return";
        dbQueryBuilder.AppendLine(string.Format(format, (object) trade.TradeID, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID), (object) 2));
      }
      dbQueryBuilder.IfExists(table, dbValueList);
      dbQueryBuilder.Begin();
      dbQueryBuilder.Update(table, values, dbValueList);
      dbQueryBuilder.AppendLine("select 0 as NewRow");
      dbQueryBuilder.End();
      values.Add("TradeID", (object) trade.TradeID);
      values.Add("LoanGuid", (object) loanInfo.GUID);
      values.Add("Profit", (object) 0);
      values.Add("AssignedStatus", (object) 1);
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.AppendLine("select 1 as NewRow");
      dbQueryBuilder.End();
      if (loanStatus != GseCommitmentLoanStatus.Unassigned)
      {
        dbQueryBuilder.AppendLine(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, " and TradeAssignment.Status > " + (object) 1 + " and TradeAssignment.TradeID <> " + (object) trade.TradeID + " And LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID), GseCommitments.SellSideTradeTypes));
        dbQueryBuilder.AppendLine("update TradeAssignment set PendingStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where TradeAssignment.TradeID <> " + (object) trade.TradeID);
        dbQueryBuilder.AppendLine(" and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID));
        dbQueryBuilder.AppendLine(" and Trades.TradeType in (" + (object) 7 + ", " + (object) 2 + ")");
      }
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        throw new Exception("The loan has been assigned to another trade or pool.");
      bool flag = string.Concat(dataSet.Tables[0].Rows[0][0]) == "1";
      if (loanStatus != GseCommitmentLoanStatus.Unassigned)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
          GseCommitments.GetTrade(EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeID"]));
      }
      return flag;
    }

    public static void RecalculateProfits(int tradeId, bool isExternalOrganization)
    {
      GseCommitments.RecalculateProfits(GseCommitments.GetTrade(tradeId) ?? throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Trade, (object) tradeId), isExternalOrganization);
    }

    public static void CommitPendingTradeStatus(
      int tradeId,
      string loanId,
      GseCommitmentLoanStatus loanStatus)
    {
      GseCommitments.CommitPendingTradeStatus(tradeId, loanId, loanStatus, false);
    }

    public static void CommitPendingTradeStatus(
      int tradeId,
      string loanId,
      GseCommitmentLoanStatus loanStatus,
      bool rejected)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) tradeId);
      dbValueList.Add("LoanGuid", (object) loanId);
      dbValueList.Add("PendingStatus", (object) (int) loanStatus);
      dbQueryBuilder.IfExists(table, dbValueList);
      dbQueryBuilder.Begin();
      DbValueList values = new DbValueList();
      if (!rejected)
        values.Add("PendingStatus", (object) 0);
      values.Add("Rejected", (object) rejected, (IDbEncoder) DbEncoding.Flag);
      dbQueryBuilder.Update(table, values, dbValueList);
      if (loanStatus >= GseCommitmentLoanStatus.Assigned)
        dbQueryBuilder.DeleteFrom(table, new DbValueList()
        {
          {
            "PendingStatus",
            (object) 1
          },
          {
            "LoanGuid",
            (object) loanId
          }
        });
      dbQueryBuilder.End();
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static GseCommitmentHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      return GseCommitments.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
    }

    public static void RecalculateProfitFromLoan(string loanGuid, bool isExternalOrganization)
    {
      GSECommitmentInfo[] loanTradesForLoan = GseCommitments.GetAllPendingLoanTradesForLoan(loanGuid);
      string[] pricingFieldList = GseCommitments.GeneratePricingFieldList((IEnumerable<GSECommitmentInfo>) loanTradesForLoan);
      PipelineInfo pipelineInfo = Pipeline.GetPipelineInfo((UserInfo) null, loanGuid, pricingFieldList, PipelineData.Fields, isExternalOrganization);
      if (pipelineInfo == null)
        return;
      GseCommitments.RecalculateProfits(loanTradesForLoan, pipelineInfo);
    }

    public static GSECommitmentInfo[] GetAllPendingLoanTradesForLoan(string guid)
    {
      return GseCommitments.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), GseCommitments.TheTradeType), "TradeID");
    }

    public static string[] GeneratePricingFieldList(int[] tradeIds)
    {
      return GseCommitments.GeneratePricingFieldList((IEnumerable<GSECommitmentInfo>) GseCommitments.GetTrades(tradeIds));
    }

    public static void ApplyTradeStatusFromLoan(PipelineInfo pinfo)
    {
      string tradeGuid = string.Concat(pinfo.GetField("TradeGuid"));
      string str = string.Concat(pinfo.GetField("InvestorStatus"));
      DateTime date = Utils.ParseDate(pinfo.GetField("InvestorStatusDate"), DateTime.Now);
      if (tradeGuid == "")
      {
        GseCommitments.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, GseCommitmentLoanStatus.Unassigned, date);
      }
      else
      {
        switch (str)
        {
          case "Purchased":
            GseCommitments.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, GseCommitmentLoanStatus.Purchased, date);
            break;
          case "Shipped":
            GseCommitments.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, GseCommitmentLoanStatus.Shipped, date);
            break;
          default:
            GseCommitments.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, GseCommitmentLoanStatus.Assigned, date);
            break;
        }
      }
    }

    public static void SetTradeAssignmentStatus(
      string loanId,
      string tradeGuid,
      GseCommitmentLoanStatus loanStatus,
      DateTime statusDate)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("LoanGuid", (object) loanId);
      dbValueList.Add("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
      DbValueList values = new DbValueList();
      values.Add("AssignedStatus", (object) (int) loanStatus);
      values.Add("AssignedStatusDate", (object) statusDate);
      if (loanStatus == GseCommitmentLoanStatus.Unassigned)
      {
        dbQueryBuilder.AppendLine("update TradeAssignment set AssignedStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId) + " and TradeType in (" + (object) 2 + ", " + (object) 3 + ")");
        dbQueryBuilder.AppendLine("update TradeAssignment set PendingStatus = 0 ");
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where PendingStatus = AssignedStatus and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId) + " and TradeType in (" + (object) 2 + ", " + (object) 3 + ")");
        dbQueryBuilder.AppendLine("delete from TradeAssignment ");
        dbQueryBuilder.AppendLine(" where AssignedStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" and PendingStatus = 0 and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        dbQueryBuilder.Declare("@tradeId", "int");
        dbQueryBuilder.AppendLine("select @tradeId = TradeID from Trades where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tradeGuid) + " and TradeType in (" + (object) 2 + ", " + (object) 3 + ")");
        dbQueryBuilder.If("@tradeId is not NULL");
        dbQueryBuilder.Begin();
        dbQueryBuilder.IfExists(table, dbValueList);
        dbQueryBuilder.Update(table, new DbValueList(values)
        {
          {
            "PendingStatus",
            (object) ("(case PendingStatus when " + (object) (int) loanStatus + " then 0 else PendingStatus end)"),
            (IDbEncoder) DbEncoding.None
          }
        }, dbValueList);
        dbQueryBuilder.Else();
        dbQueryBuilder.InsertInto(table, new DbValueList(dbValueList)
        {
          values,
          {
            "Profit",
            (object) 0
          }
        }, true, false);
        dbQueryBuilder.AppendLine("update TradeAssignment set AssignedStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId) + " and TradeAssignment.TradeID <> IsNull(@tradeId, -1) ");
        dbQueryBuilder.AppendLine(" and Trades.TradeType in(" + (object) 2 + ", " + (object) 3 + ")");
        dbQueryBuilder.End();
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static void RecalculateProfits(GSECommitmentInfo[] trades, PipelineInfo pinfo)
    {
      ChunkedList<GSECommitmentInfo> chunkedList = new ChunkedList<GSECommitmentInfo>(trades, 50);
      GSECommitmentInfo[] gseCommitmentInfoArray = (GSECommitmentInfo[]) null;
      while ((gseCommitmentInfoArray = chunkedList.Next()) != null)
        new DbQueryBuilder().ExecuteNonQuery();
    }

    private static string OrderbySettlementMonth(QueryEngine queryEngine, SortField[] sortFields)
    {
      string str1 = queryEngine.GetOrderByClause(sortFields);
      if (sortFields != null)
      {
        bool flag = false;
        string str2 = "SettlementMonth";
        for (int index = 0; index < sortFields.Length; ++index)
        {
          if (sortFields[index].Term.ToString().Contains(str2))
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          string[] strArray = new string[13]
          {
            "",
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
          };
          string str3 = "";
          for (int index = 0; index < strArray.Length; ++index)
            str3 = str3 + "when '" + strArray[index] + "' then " + index.ToString() + "\n";
          string str4 = str3 + "End\n";
          str1 = str1.Substring(0, str1.IndexOf("[GseCommitmentDetails].[SettlementMonth]")) + "case [GseCommitmentDetails].[SettlementMonth] " + str4 + str1.Substring(str1.IndexOf(str2) + str2.Length + 2);
        }
      }
      return str1;
    }

    public static List<GSECommitmentInfo> ValidateContractNumbers(
      List<string> commitmentContractNums)
    {
      List<GSECommitmentInfo> gseCommitmentInfoList = new List<GSECommitmentInfo>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select gsecommitmentdetails.Guid, gsecommitmentdetails.TradeID, Name, ContractNumber, ProductNameXml, GuarantyFeeXml from gsecommitmentdetails ");
      dbQueryBuilder.AppendLine("inner join TradeObjects on GseCommitmentDetails.TradeID = TradeObjects.TradeID ");
      dbQueryBuilder.AppendLine("where GseCommitmentDetails.ContractNumber in ('" + string.Join("', '", (IEnumerable<string>) commitmentContractNums) + "')");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
      {
        for (int index = 0; index < dataSet.Tables[0].Rows.Count; ++index)
        {
          DataRow row = dataSet.Tables[0].Rows[index];
          GSECommitmentInfo gseCommitmentInfo1 = new GSECommitmentInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]), "");
          gseCommitmentInfo1.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Name"], "");
          gseCommitmentInfo1.ContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ContractNumber"]);
          gseCommitmentInfo1.ProductNames = BinaryConvertible<FannieMaeProducts>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ProductNameXml"]));
          gseCommitmentInfo1.GuarantyFees = BinaryConvertible<GuarantyFeeItems>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["GuarantyFeeXml"]));
          GSECommitmentInfo gseCommitmentInfo2 = gseCommitmentInfo1;
          gseCommitmentInfoList.Add(gseCommitmentInfo2);
        }
      }
      return gseCommitmentInfoList;
    }
  }
}
