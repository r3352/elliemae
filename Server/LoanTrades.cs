// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanTrades
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class LoanTrades
  {
    private static string className = nameof (LoanTrades);
    private static string UPDATETRADEERROR = "This trade is currently pending in the Trade Update Queue and cannot be modified until completed.";

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
          TradeType.MbsPool
        };
      }
    }

    public static List<TradeType> TheTradeType
    {
      get => new List<TradeType>() { TradeType.LoanTrade };
    }

    public static LoanTradeInfo[] GetAllTrades()
    {
      return LoanTrades.getTradesFromDatabase("select TradeID from LoanTradeDetails");
    }

    public static LoanTradeInfo GetTrade(int tradeId)
    {
      LoanTradeInfo[] tradesFromDatabase = LoanTrades.getTradesFromDatabase(string.Concat((object) tradeId));
      return tradesFromDatabase.Length == 0 ? (LoanTradeInfo) null : tradesFromDatabase[0];
    }

    public static LoanTradeInfo GetTrade(string tradeName)
    {
      LoanTradeInfo[] tradesFromDatabase = LoanTrades.getTradesFromDatabase("select TradeID from Trades where Name = '" + tradeName + "'");
      return tradesFromDatabase.Length == 0 ? (LoanTradeInfo) null : tradesFromDatabase[0];
    }

    private static LoanTradeInfo[] getTradesFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from LoanTradeDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select LoanTradeDetails.*, TradeObjects.*, TradeLoanTradeSummary.*, MasterContracts.ContractNumber from LoanTradeDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on LoanTradeDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on LoanTradeDetails.ContractID = MasterContracts.ContractID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetLoanTradeSummaryInline(" + (object) 1 + ") TradeLoanTradeSummary on LoanTradeDetails.TradeID = TradeLoanTradeSummary.TradeID");
      dbQueryBuilder.AppendLine("   where LoanTradeDetails.TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by TradeID, PendingStatus");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      LoanTradeInfo[] tradesFromDatabase = new LoanTradeInfo[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        LoanTradeInfo tradeInfo = LoanTrades.dataRowToTradeInfo(table1.Rows[index], table2);
        tradeInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(table1.Rows[index]["OpenAmount"]);
        tradesFromDatabase[index] = tradeInfo;
      }
      return tradesFromDatabase;
    }

    private static LoanTradeViewModel[] getTradeViewsFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from LoanTradeDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select LoanTradeDetails.*, TradeObjects.*, TradeLoanTradeSummary.*, MasterContracts.ContractNumber, SecurityTradeDetails.TradeID as SecurityTradeID, SecurityTradeDetails.SecurityType, SecurityTradeDetails.Name as AssignedSecurityID, SecurityTradeDetails.Coupon, SecurityTradeDetails.Price from LoanTradeDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on LoanTradeDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on LoanTradeDetails.ContractID = MasterContracts.ContractID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetLoanTradeSummaryInline(" + (object) 1 + ") TradeLoanTradeSummary on LoanTradeDetails.TradeID = TradeLoanTradeSummary.TradeID");
      dbQueryBuilder.AppendLine("   left outer join TradeAssignmentByTrade AssignedSecurityTrade on AssignedSecurityTrade.AssigneeTradeID = LoanTradeDetails.TradeID");
      dbQueryBuilder.AppendLine("   left outer join SecurityTradeDetails SecurityTradeDetails on SecurityTradeDetails.TradeID = AssignedSecurityTrade.TradeID");
      dbQueryBuilder.AppendLine("   left outer join FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") SecurityTradesummary on SecurityTradeDetails.TradeID = SecurityTradeSummary.TradeID");
      dbQueryBuilder.AppendLine("   where LoanTradeDetails.TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by TradeID, PendingStatus");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      LoanTradeViewModel[] viewsFromDatabase = new LoanTradeViewModel[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        LoanTradeViewModel tradeViewInfo = LoanTrades.dataRowToTradeViewInfo(table1.Rows[index], table2);
        tradeViewInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(table1.Rows[index]["OpenAmount"]);
        viewsFromDatabase[index] = tradeViewInfo;
      }
      return viewsFromDatabase;
    }

    private static LoanTradeViewModel dataRowToTradeViewInfo(DataRow r, DataTable statusTable)
    {
      Dictionary<LoanTradeStatus, int> pendingLoanCounts = LoanTrades.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      return new LoanTradeViewModel(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"], ""), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"]), string.Concat(r["DealerName"]), string.Concat(r["AssigneeName"]), string.Concat(r["InvestorName"]), string.Concat(r["InvestorTradeNum"]), string.Concat(r["InvestorCommitmentNum"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InvestorDeliveryDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EarlyDeliveryDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["TargetDeliveryDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffFee"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ShipmentDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PurchaseDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RateAdjustment"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyUpAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyDownAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MiscAdjustment"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastModified"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["LoanCount"]), pendingLoanCounts, EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PendingLoanCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentType"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["SecurityTradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AssignedStatusDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GainLossAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["NetProfit"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AssignedSecurityID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Servicer"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ServicingType"], -999), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PairOffsXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsWeightedAvgBulkPriceLocked"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["WeightedAvgBulkPrice"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsBulkDelivery"]))
      {
        OpenAmount = 0M,
        NotWithdrawnLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["NotWithdrawnLoanCount"])
      };
    }

    private static LoanTradeInfo dataRowToTradeInfo(DataRow r, DataTable statusTable)
    {
      Dictionary<LoanTradeStatus, int> pendingLoanCounts = LoanTrades.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      return new LoanTradeInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"], ""), (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"]), string.Concat(r["DealerName"]), string.Concat(r["AssigneeName"]), string.Concat(r["InvestorName"]), string.Concat(r["InvestorTradeNum"]), string.Concat(r["InvestorCommitmentNum"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InvestorDeliveryDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EarlyDeliveryDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["TargetDeliveryDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffFee"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ShipmentDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PurchaseDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RateAdjustment"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyUpAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyDownAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MiscAdjustment"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastModified"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["LoanCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Notes"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FilterQueryXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SRPTableXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DealerXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AssigneeXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PricingXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AdjustmentsXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PairOffsXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]), 0, EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAmount"]), pendingLoanCounts, EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentType"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["SecurityTradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GainLossAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["NetProfit"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Servicer"]), (ServicingType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ServicingType"], -999), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsWeightedAvgBulkPriceLocked"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["WeightedAvgBulkPrice"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsBulkDelivery"]));
    }

    public static MasterContractSummaryInfo[] GetActiveContracts(bool includeTradeData)
    {
      return LoanTrades.getContractsFromDatabase("select ContractID from MasterContracts where Status <> " + (object) 1, includeTradeData, true);
    }

    private static MasterContractSummaryInfo[] getContractsFromDatabase(
      string criteria,
      bool includeTradeData,
      bool summariesOnly)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (includeTradeData)
      {
        dbQueryBuilder.Append("select *, (case ContractAmount when 0 Then 0 Else AssignedAmount * 100/ContractAmount End) As CompletionPercent from MasterContracts ");
        if (!summariesOnly)
          dbQueryBuilder.AppendLine("  left outer join MasterContractObjects on MasterContractObjects.ContractID = MasterContracts.ContractID");
        dbQueryBuilder.AppendLine("   left outer join FN_GetContractLoanSummaryInline() ContractTrades on MasterContracts.ContractID = ContractTrades.ContractID");
        dbQueryBuilder.AppendLine("   where MasterContracts.ContractID in (" + criteria + ")");
        dbQueryBuilder.AppendLine("   order by MasterContracts.ContractNumber");
      }
      else if (summariesOnly)
        dbQueryBuilder.AppendLine("select MasterContracts.*, -1 As TradeCount, 0 As TotalProfit, 0 As AssignedAmount from MasterContracts where MasterContracts.ContractID in (" + criteria + ") order by MasterContracts.ContractNumber");
      else
        dbQueryBuilder.AppendLine("select MasterContracts.*, MasterContractObjects.*, -1 As TradeCount, 0 As TotalProfit, 0 As AssignedAmount from MasterContracts left outer join MasterContractObjects on MasterContractObjects.ContractID = MasterContracts.ContractID where MasterContracts.ContractID in (" + criteria + ") order by MasterContracts.ContractNumber");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      MasterContractSummaryInfo[] contractsFromDatabase = new MasterContractSummaryInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        contractsFromDatabase[index] = !summariesOnly ? (MasterContractSummaryInfo) LoanTrades.dataRowToMasterContractInfo(dataRowCollection[index]) : LoanTrades.dataRowToMasterContractSummaryInfo(dataRowCollection[index]);
      return contractsFromDatabase;
    }

    private static MasterContractSummaryInfo dataRowToMasterContractSummaryInfo(DataRow r)
    {
      Decimal completionPercent = 0M;
      try
      {
        completionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      }
      catch
      {
      }
      return new MasterContractSummaryInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorName"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorContractNum"]), (MasterContractTerm) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ContractAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["StartDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EndDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AssignedAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]), completionPercent);
    }

    private static MasterContractInfo dataRowToMasterContractInfo(DataRow r)
    {
      Decimal completionPercent = 0M;
      try
      {
        completionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      }
      catch
      {
      }
      return new MasterContractInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorName"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorContractNum"]), (MasterContractTerm) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ContractAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["StartDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EndDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AssignedAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["InvestorXml"]), completionPercent);
    }

    public static LoanTradeHistoryItem[] GetTradeHistory(int tradeId)
    {
      return LoanTrades.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where TradeID = " + (object) tradeId);
    }

    private static LoanTradeHistoryItem[] getTradeHistoryFromDatabase(string selectionQuery)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select TradeHistory.*, LoanSummary.LoanNumber, Trades.Name as TradeName, Trades.InvestorName, MasterContracts.ContractNumber");
      dbQueryBuilder.AppendLine("from TradeHistory inner join Trades on TradeHistory.TradeID = Trades.TradeID and TradeType = 2 ");
      dbQueryBuilder.AppendLine("   left outer join LoanSummary on TradeHistory.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on TradeHistory.ContractID = MasterContracts.ContractID");
      if ((selectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("where TradeHistory.HistoryID in (" + selectionQuery + ")");
      return LoanTrades.dataRowsToTradeHistoryItems((IEnumerable) dbQueryBuilder.Execute());
    }

    private static LoanTradeHistoryItem[] dataRowsToTradeHistoryItems(IEnumerable rows)
    {
      List<LoanTradeHistoryItem> tradeHistoryItemList = new List<LoanTradeHistoryItem>();
      foreach (DataRow row in rows)
        tradeHistoryItemList.Add(LoanTrades.dataRowToHistoryItem(row));
      return tradeHistoryItemList.ToArray();
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      int tradeId,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      return LoanTrades.GetAssignedOrPendingLoans(user, LoanTrades.GetTrade(tradeId) ?? throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Template, (object) tradeId), fields, isExternalOrganization, sqlRead);
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      LoanTradeInfo trade,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      string identitySelectionQuery = "select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid where TradeAssignment.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 1;
      return Pipeline.Generate(user, identitySelectionQuery, fields, PipelineData.Trade, isExternalOrganization, TradeType.LoanTrade, sqlRead);
    }

    public static MasterContractInfo GetContractForTrade(int tradeId)
    {
      MasterContractSummaryInfo[] contractsFromDatabase = LoanTrades.getContractsFromDatabase("select ContractID from LoanTrades where TradeID = " + (object) tradeId, false, false);
      return contractsFromDatabase.Length == 0 ? (MasterContractInfo) null : (MasterContractInfo) contractsFromDatabase[0];
    }

    public static int CreateTrade(LoanTradeInfo info, UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (LoanTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      dbQueryBuilder.Declare("@tradeId", "int");
      dbQueryBuilder.InsertInto(table1, LoanTrades.createTradesValueList(info), true, false);
      dbQueryBuilder.SelectIdentity("@tradeId");
      DbValue dbValue = new DbValue("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
      DbValueList loanTradeValueList = LoanTrades.createLoanTradeValueList(info);
      loanTradeValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table2, loanTradeValueList, true, false);
      DbValueList objectsValueList = LoanTrades.createTradeObjectsValueList(info);
      objectsValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.Select("@tradeId");
      int tradeId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      info = LoanTrades.GetTrade(tradeId);
      LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, TradeHistoryAction.TradeCreated, currentUser));
      if (info.Status != TradeStatus.Open)
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, info.Status, currentUser));
      if (info.Locked)
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, TradeHistoryAction.TradeLocked, currentUser));
      if ((info.AssigneeName ?? "").Trim() != "")
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, TradeHistoryAction.AssigneeAssigned, currentUser));
      if (info.ContractID > 0)
      {
        MasterContractSummaryInfo contract = (MasterContractSummaryInfo) LoanTrades.GetContract(info.ContractID);
        if (contract != null)
          LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, contract, TradeHistoryAction.ContractAssigned, currentUser));
      }
      return tradeId;
    }

    private static DbValueList createTradesValueList(LoanTradeInfo info, bool isUpdateStatus = true)
    {
      DbValueList tradesValueList = new DbValueList();
      tradesValueList.Add("Guid", (object) info.Guid);
      tradesValueList.Add("Name", (object) info.Name);
      if (isUpdateStatus)
        tradesValueList.Add("Status", (object) (int) info.Status);
      tradesValueList.Add("Locked", (object) info.Locked, (IDbEncoder) DbEncoding.Flag);
      tradesValueList.Add("DealerName", (object) info.DealerName);
      tradesValueList.Add("AssigneeName", (object) info.AssigneeName);
      tradesValueList.Add("Tolerance", (object) info.Tolerance);
      tradesValueList.Add("PairOffFee", (object) info.PairOffFee);
      tradesValueList.Add("TradeAmount", (object) info.TradeAmount);
      tradesValueList.Add("CommitmentDate", (object) info.CommitmentDate);
      tradesValueList.Add("PairOffAmount", (object) info.PairOffAmount);
      tradesValueList.Add("CommitmentType", (object) info.CommitmentType);
      tradesValueList.Add("TradeDescription", (object) info.TradeDescription);
      tradesValueList.Add("TradeType", (object) (int) info.TradeType);
      tradesValueList.Add("PendingBy", (object) info.PendingBy);
      return tradesValueList;
    }

    private static DbValueList createLoanTradeValueList(LoanTradeInfo info)
    {
      return new DbValueList()
      {
        {
          "ContractID",
          (object) info.ContractID,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "InvestorName",
          (object) info.InvestorName
        },
        {
          "InvestorTradeNum",
          (object) info.InvestorTradeNumber
        },
        {
          "InvestorCommitmentNum",
          (object) info.InvestorCommitmentNumber
        },
        {
          "InvestorDeliveryDate",
          (object) info.InvestorDeliveryDate
        },
        {
          "EarlyDeliveryDate",
          (object) info.EarlyDeliveryDate
        },
        {
          "TargetDeliveryDate",
          (object) info.TargetDeliveryDate
        },
        {
          "TradeAmount",
          (object) info.TradeAmount
        },
        {
          "ShipmentDate",
          (object) info.ShipmentDate
        },
        {
          "PurchaseDate",
          (object) info.PurchaseDate
        },
        {
          "RateAdjustment",
          (object) info.RateAdjustment
        },
        {
          "BuyUpAmount",
          (object) info.BuyUpAmount
        },
        {
          "BuyDownAmount",
          (object) info.BuyDownAmount
        },
        {
          "MiscAdjustment",
          (object) info.MiscAdjustment
        },
        {
          "GainLossAmount",
          (object) info.GainLossAmount
        },
        {
          "NetProfit",
          (object) info.NetProfit
        },
        {
          "Servicer",
          (object) info.Servicer
        },
        {
          "ServicingType",
          (object) (int) info.ServicingType
        },
        {
          "LastModified",
          (object) "GetDate()",
          (IDbEncoder) DbEncoding.None
        },
        {
          "IsWeightedAvgBulkPriceLocked",
          (object) (info.IsWeightedAvgBulkPriceLocked ? 1 : 0)
        },
        {
          "WeightedAvgBulkPrice",
          (object) info.WeightedAvgBulkPrice
        },
        {
          "IsBulkDelivery",
          (object) (info.IsBulkDelivery ? 1 : 0)
        }
      };
    }

    public static int AddTradeHistoryItem(LoanTradeHistoryItem item)
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
          "ContractID",
          (object) item.ContractID,
          (IDbEncoder) DbEncoding.MinusOneAsNull
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
        dbValueList.Add("Investor", (object) item.InvestorName);
        dbQueryBuilder.IfNotExists(table2, dbValueList);
        dbValueList.Add("Timestamp", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.InsertInto(table2, dbValueList, true, false);
      }
      return EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    public static void UpdateTrade(
      LoanTradeInfo info,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool checkStatus = true,
      bool isUpdateStatus = true)
    {
      LoanTradeInfo trade = LoanTrades.GetTrade(info.TradeID);
      if (trade == null)
        throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) info.TradeID);
      if (checkStatus && (trade.Status == TradeStatus.Pending || trade.Status == TradeStatus.Archived))
        throw new TradeNotUpdateException(LoanTrades.UPDATETRADEERROR);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (LoanTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      DbValue key = new DbValue("TradeID", (object) info.TradeID);
      dbQueryBuilder.DeleteFrom(table3, key);
      dbQueryBuilder.Update(table1, LoanTrades.createTradesValueList(info, isUpdateStatus), key);
      dbQueryBuilder.Update(table2, LoanTrades.createLoanTradeValueList(info), key);
      DbValueList objectsValueList = LoanTrades.createTradeObjectsValueList(info);
      objectsValueList.Add(key);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
      if (info.Locked != trade.Locked)
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, info.Locked ? TradeHistoryAction.TradeLocked : TradeHistoryAction.TradeUnlocked, currentUser));
      if (info.Status != trade.Status)
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, info.Status, currentUser));
      if ((trade.AssigneeName ?? "").Trim() == "" && (info.AssigneeName ?? "").Trim() != "")
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, TradeHistoryAction.AssigneeAssigned, currentUser));
      else if ((trade.AssigneeName ?? "").Trim() != "" && (info.AssigneeName ?? "").Trim() == "")
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, TradeHistoryAction.AssigneeUnassigned, currentUser));
      else if ((trade.AssigneeName ?? "").Trim() != (info.AssigneeName ?? "").Trim())
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, TradeHistoryAction.AssigneeChanged, currentUser));
      if (info.ContractID != trade.ContractID)
      {
        if (trade.ContractID > 0)
        {
          MasterContractSummaryInfo contract = (MasterContractSummaryInfo) LoanTrades.GetContract(trade.ContractID);
          if (contract != null)
            LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, contract, TradeHistoryAction.ContractUnassigned, currentUser));
        }
        if (info.ContractID > 0)
        {
          MasterContractSummaryInfo contract = (MasterContractSummaryInfo) LoanTrades.GetContract(info.ContractID);
          if (contract != null)
            LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(info, contract, TradeHistoryAction.ContractAssigned, currentUser));
        }
      }
      if (LoanTradeInfo.ComparePricing(info, trade))
        return;
      LoanTrades.RecalculateProfits(info, isExternalOrganization);
    }

    public static void RecalculateProfits(LoanTradeInfo trade, bool isExternalOrganization)
    {
      PipelineInfo[] assignedLoans = LoanTrades.GetAssignedLoans(trade, LoanTrades.GeneratePricingFieldList(trade), isExternalOrganization);
      LoanTrades.RecalculateProfits(trade, assignedLoans);
    }

    public static string[] GeneratePricingFieldList(LoanTradeInfo trade)
    {
      return LoanTrades.GeneratePricingFieldList((IEnumerable<LoanTradeInfo>) new LoanTradeInfo[1]
      {
        trade
      });
    }

    public static string[] GeneratePricingFieldList(IEnumerable<LoanTradeInfo> trades)
    {
      List<string> stringList = new List<string>();
      foreach (LoanTradeInfo trade in trades)
      {
        if (trade != null)
        {
          foreach (string pricingField in trade.GetPricingFields())
          {
            if (!stringList.Contains(pricingField))
              stringList.Add(pricingField);
          }
        }
      }
      return stringList.ToArray();
    }

    public static PipelineInfo[] GetAssignedLoans(
      LoanTradeInfo trade,
      string[] fields,
      bool isExternalOrganization)
    {
      return Pipeline.Generate("select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid  where TradeAssignment.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 2, fields, PipelineData.Trade, isExternalOrganization, TradeType.LoanTrade);
    }

    public static void RecalculateProfits(LoanTradeInfo trade, PipelineInfo[] pinfos)
    {
      ChunkedList<PipelineInfo> chunkedList = new ChunkedList<PipelineInfo>(pinfos, 50);
      PipelineInfo[] pipelineInfoArray;
      while ((pipelineInfoArray = chunkedList.Next()) != null)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        for (int index = 0; index < pipelineInfoArray.Length; ++index)
        {
          Decimal num;
          if (!trade.Pricing.IsAdvancedPricing)
          {
            num = trade.CalculateProfit(pipelineInfoArray[index], 0M);
          }
          else
          {
            SecurityTradeInfo trade1 = SecurityTrades.GetTrade(trade.SecurityTradeID);
            num = trade1 != null ? trade.CalculateProfit(pipelineInfoArray[index], trade1.Price) : trade.CalculateProfit(pipelineInfoArray[index], 0M);
          }
          dbQueryBuilder.Append("update TradeAssignment set Profit = " + num.ToString("0.00") + " where TradeID = " + (object) trade.TradeID + " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) pipelineInfoArray[index].GUID));
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static LoanTradeInfo GetTradeByName(string tradeName)
    {
      LoanTradeInfo[] tradesFromDatabase = LoanTrades.getTradesFromDatabase("select TradeID from Trades where Name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(tradeName) + "'");
      return tradesFromDatabase.Length == 0 ? (LoanTradeInfo) null : tradesFromDatabase[0];
    }

    private static DbValueList createTradeObjectsValueList(LoanTradeInfo info)
    {
      DbValueList objectsValueList = new DbValueList();
      objectsValueList.Add("FilterQueryXml", info.Filter == null ? (object) (string) null : (object) info.Filter.ToXml());
      objectsValueList.Add("SRPTableXml", info.SRPTable == null ? (object) (string) null : (object) info.SRPTable.ToXml());
      objectsValueList.Add("InvestorXml", (object) info.Investor.ToXml());
      string xml = info.LoanTradePairOffs.ToXml();
      objectsValueList.Add("PairOffsXml", (object) xml);
      objectsValueList.Add("PairOffDate1", (object) LoanTrades.GetDateFromXml(xml, "date", 0));
      objectsValueList.Add("PairOffDate2", (object) LoanTrades.GetDateFromXml(xml, "date", 1));
      objectsValueList.Add("PairOffDate3", (object) LoanTrades.GetDateFromXml(xml, "date", 2));
      objectsValueList.Add("PairOffDate4", (object) LoanTrades.GetDateFromXml(xml, "date", 3));
      objectsValueList.Add("PairOffAmount1", (object) LoanTrades.GetDecimalFromXml(xml, "amt", 0, true));
      objectsValueList.Add("PairOffAmount2", (object) LoanTrades.GetDecimalFromXml(xml, "amt", 1, true));
      objectsValueList.Add("PairOffAmount3", (object) LoanTrades.GetDecimalFromXml(xml, "amt", 2, true));
      objectsValueList.Add("PairOffAmount4", (object) LoanTrades.GetDecimalFromXml(xml, "amt", 3, true));
      objectsValueList.Add("PairOffBuyPrice1", (object) LoanTrades.GetDecimalFromXml(xml, "pairOffFeePercentage", 0));
      objectsValueList.Add("PairOffBuyPrice2", (object) LoanTrades.GetDecimalFromXml(xml, "pairOffFeePercentage", 1));
      objectsValueList.Add("PairOffBuyPrice3", (object) LoanTrades.GetDecimalFromXml(xml, "pairOffFeePercentage", 2));
      objectsValueList.Add("PairOffBuyPrice4", (object) LoanTrades.GetDecimalFromXml(xml, "pairOffFeePercentage", 3));
      objectsValueList.Add("PairOffGainLoss1", (object) LoanTrades.GetDecimalFromXml(xml, "fee", 0));
      objectsValueList.Add("PairOffGainLoss2", (object) LoanTrades.GetDecimalFromXml(xml, "fee", 1));
      objectsValueList.Add("PairOffGainLoss3", (object) LoanTrades.GetDecimalFromXml(xml, "fee", 2));
      objectsValueList.Add("PairOffGainLoss4", (object) LoanTrades.GetDecimalFromXml(xml, "fee", 3));
      objectsValueList.Add("PricingXml", (object) info.Pricing.ToXml());
      objectsValueList.Add("AdjustmentsXml", (object) info.PriceAdjustments.ToXml());
      objectsValueList.Add("DealerXml", (object) info.Dealer.ToXml());
      objectsValueList.Add("AssigneeXml", (object) info.Assignee.ToXml());
      objectsValueList.Add("Notes", (object) info.Notes);
      return objectsValueList;
    }

    private static string GetDateFromXml(string xml, string field, int index)
    {
      if (string.IsNullOrEmpty(xml))
        return "NULL";
      string xpath = string.Format("//element[@name='{0}']/element[@name='{1}']", (object) index, (object) field);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath);
      if (xmlNode == null)
        return "NULL";
      DateTime result = new DateTime();
      DateTime.TryParse(xmlNode.InnerText, out result);
      return result == DateTime.MinValue ? "NULL" : result.ToString();
    }

    private static string GetDecimalFromXml(string xml, string field, int index, bool negative = false)
    {
      if (string.IsNullOrEmpty(xml))
        return "NULL";
      string xpath = string.Format("//element[@name='{0}']/element[@name='{1}']", (object) index, (object) field);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath);
      if (xmlNode == null)
        return "NULL";
      Decimal result;
      Decimal.TryParse(xmlNode.InnerText, out result);
      if (result == 0M)
        return "NULL";
      return !negative ? result.ToString() : (0M - result).ToString();
    }

    private static Dictionary<LoanTradeStatus, int> dataRowsToPendingLoanCounts(IEnumerable rows)
    {
      Dictionary<LoanTradeStatus, int> pendingLoanCounts = new Dictionary<LoanTradeStatus, int>();
      foreach (DataRow row in rows)
        pendingLoanCounts[(LoanTradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["PendingStatus"])] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["LoanCount"]);
      return pendingLoanCounts;
    }

    private static LoanTradeHistoryItem dataRowToHistoryItem(DataRow r)
    {
      return new LoanTradeHistoryItem(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["HistoryID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), (TradeHistoryAction) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Action"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Timestamp"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UserID"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["DataXml"]));
    }

    public static MasterContractInfo GetContract(int contractId)
    {
      MasterContractSummaryInfo[] contractsFromDatabase = LoanTrades.getContractsFromDatabase(string.Concat((object) contractId), true, false);
      return contractsFromDatabase.Length == 0 ? (MasterContractInfo) null : (MasterContractInfo) contractsFromDatabase[0];
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      int[] tradeIds,
      string[] fields,
      bool isExternalOrganization)
    {
      return LoanTrades.GetEligibleLoans(user, LoanTrades.GetTrades(tradeIds), fields, (SortField[]) null, isExternalOrganization);
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      LoanTradeInfo[] trades,
      string[] fields,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      QueryCriterion queryCriterion1 = (QueryCriterion) null;
      foreach (LoanTradeInfo trade in trades)
      {
        if (trade != null && trade.Filter != null)
        {
          QueryCriterion queryCriterion2 = ((TradeFilterEvaluator) trade.Filter.CreateEvaluator(typeof (LoanTradeFilterEvaluator))).ToQueryCriterion((TradeInfoObj) trade, filterOption);
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
      PipelineInfo[] pinfos = Pipeline.Generate(user, LoanInfo.Right.Access, fields, PipelineData.Trade, queryCriterion1, ((IEnumerable<SortField>) sortOrder).Where<SortField>((System.Func<SortField, bool>) (s => !s.Term.FieldName.StartsWith("TradeEligibility"))).ToArray<SortField>(), isExternalOrganization, TradeType.LoanTrade);
      Dictionary<int, SecurityTradeInfo> secTrades = new Dictionary<int, SecurityTradeInfo>();
      for (int index = 0; index < trades.Length; ++index)
      {
        if (trades[index].SecurityTradeID > 0)
          secTrades.Add(trades[index].TradeID, SecurityTrades.GetTrade(trades[index].SecurityTradeID));
      }
      foreach (PipelineInfo pinfo in pinfos)
        LoanTrades.PopulateTradeProfitData(pinfo, trades, secTrades);
      if (flag)
        LoanTrades.SortByCalculatedData(pinfos, sortOrder);
      TraceLog.Write(TraceLevel.Info, nameof (LoanTrades), "After PopulateTradeProfitData");
      return pinfos;
    }

    private static void SortByCalculatedData(PipelineInfo[] pinfos, SortField[] sortFields)
    {
      foreach (SortField sortOrder in ((IEnumerable<SortField>) sortFields).Where<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.StartsWith("TradeEligibility"))))
        Array.Sort((Array) pinfos, (IComparer) new LoanTrades.TradeLoanCursorItemComparer(sortOrder));
    }

    public static LoanTradeInfo[] GetTrades(int[] tradeIds)
    {
      return LoanTrades.getTradesFromDatabase(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds));
    }

    public static void PopulateTradeProfitData(
      PipelineInfo pinfo,
      LoanTradeInfo[] trades,
      Dictionary<int, SecurityTradeInfo> secTrades)
    {
      PipelineInfo.TradeInfo[] tradeInfoArray = new PipelineInfo.TradeInfo[trades.Length];
      for (int index = 0; index < trades.Length; ++index)
      {
        PipelineInfo.TradeInfo tradeInfo = new PipelineInfo.TradeInfo();
        tradeInfo.TradeID = trades[index].TradeID;
        SecurityTradeInfo securityTradeInfo = (SecurityTradeInfo) null;
        if (secTrades.ContainsKey(trades[index].TradeID))
          securityTradeInfo = secTrades[trades[index].TradeID];
        Decimal num = trades[index].Pricing.IsAdvancedPricing ? (securityTradeInfo != null ? trades[index].CalculateProfit(pinfo, securityTradeInfo.Price) : trades[index].CalculateProfit(pinfo, 0M)) : trades[index].CalculateProfit(pinfo, 0M);
        tradeInfo.Profit = num;
        tradeInfoArray[index] = tradeInfo;
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("Profit")] = (object) tradeInfo.Profit;
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("NetPrice")] = (object) trades[index].CalculatePriceIndex(pinfo, false, securityTradeInfo == null ? 0M : securityTradeInfo.Price);
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("SRP")] = (object) trades[index].SRPTable.GetAdjustmentForLoan(pinfo);
        if (trades[index].Pricing.IsAdvancedPricing)
          pinfo.Info[(object) trades[index].CreateEligiblityDataKey("TotalPrice")] = (object) trades[index].CalculatePriceIndex(pinfo, securityTradeInfo == null ? 0M : securityTradeInfo.Price);
        else
          pinfo.Info[(object) trades[index].CreateEligiblityDataKey("TotalPrice")] = (object) trades[index].CalculatePriceIndex(pinfo);
      }
      pinfo.EligibleTrades = tradeInfoArray;
    }

    public static int[] QueryTradeIds(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      StringBuilder stringBuilder = new StringBuilder(LoanTrades.getQueryTradeIdsSql(user, criteria, sortOrder, isExternalOrganization));
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
        string fieldList = "LoanTradeDetails.TradeID";
        TradeQuery tradeQuery = new TradeQuery(user);
        return LoanTrades.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) tradeQuery, isExternalOrganization).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(LoanTrades.className, ex);
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
      querySql.Append(queryEngine.GetOrderByClause(sortFields));
      return querySql;
    }

    public static Dictionary<int, LoanTradeViewModel> GetTradeViews(int[] tradeIds)
    {
      List<LoanTradeViewModel> loanTradeViewModelList = new List<LoanTradeViewModel>();
      foreach (int tradeId in tradeIds)
        loanTradeViewModelList.AddRange((IEnumerable<LoanTradeViewModel>) LoanTrades.getTradeViewsFromDatabase(string.Concat((object) tradeId)));
      Dictionary<int, LoanTradeViewModel> tradeViews = new Dictionary<int, LoanTradeViewModel>();
      foreach (LoanTradeViewModel loanTradeViewModel in loanTradeViewModelList.ToArray())
      {
        if (loanTradeViewModel != null)
          tradeViews[loanTradeViewModel.TradeID] = loanTradeViewModel;
      }
      for (int key = 0; key < tradeIds.Length; ++key)
      {
        if (!tradeViews.ContainsKey(tradeIds[key]))
          tradeViews[key] = (LoanTradeViewModel) null;
      }
      return tradeViews;
    }

    public static LoanTradeViewModel GetTradeView(int tradeId)
    {
      LoanTradeViewModel[] viewsFromDatabase = LoanTrades.getTradeViewsFromDatabase(string.Concat((object) tradeId));
      return viewsFromDatabase.Length == 0 ? (LoanTradeViewModel) null : viewsFromDatabase[0];
    }

    public static void DeleteTrade(int tradeId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("Trades");
        DbValue key = new DbValue("TradeID", (object) tradeId);
        dbQueryBuilder.AppendLine("delete TradeAssignmentByTrade where AssigneeTradeID = " + (object) tradeId);
        dbQueryBuilder.DeleteFrom(table, key);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(LoanTrades.UPDATETRADEERROR);
        throw;
      }
    }

    public static void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      UserInfo currentUser,
      bool needPendingCheck = true)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (needPendingCheck && tradeIds.Length == 1)
        {
          dbQueryBuilder.AppendLine("if exists( SELECT * FROM Trades where TradeId = " + (object) tradeIds[0] + " and status in (" + (object) 6 + ", " + (object) 5 + "))");
          dbQueryBuilder.AppendLine("RAISERROR('UPDATETRADEERROR', 16, 1)");
          dbQueryBuilder.AppendLine("else");
        }
        dbQueryBuilder.AppendLine("update Trades set status = " + (object) (int) status + " where TradeID in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds) + ")");
        dbQueryBuilder.ExecuteNonQuery();
        foreach (LoanTradeInfo trade in LoanTrades.GetTrades(tradeIds))
          LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(trade, status == TradeStatus.Archived ? TradeHistoryAction.TradeArchived : TradeHistoryAction.TradeActivated, currentUser));
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(LoanTrades.UPDATETRADEERROR);
        throw;
      }
    }

    public static void UpdateTradeStatus(
      int tradeId,
      TradeStatus status,
      UserInfo currentUser,
      bool needPendingCheck = true)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (needPendingCheck)
        {
          dbQueryBuilder.AppendLine("if exists( SELECT * FROM Trades where TradeId = " + (object) tradeId + " and status in (" + (object) 6 + ", " + (object) 5 + "))");
          dbQueryBuilder.AppendLine("RAISERROR('UPDATETRADEERROR', 16, 1)");
          dbQueryBuilder.AppendLine("else");
        }
        if (status == TradeStatus.Pending)
          dbQueryBuilder.AppendLine("update Trades set status = " + (object) (int) status + ", pendingBy = '" + currentUser.FullName + "' where TradeID in (" + (object) tradeId + ")");
        else
          dbQueryBuilder.AppendLine("update Trades set status = " + (object) (int) status + " where TradeID in (" + (object) tradeId + ")");
        dbQueryBuilder.ExecuteNonQuery();
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(LoanTrades.GetTrade(tradeId), status, currentUser));
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(LoanTrades.UPDATETRADEERROR);
        throw;
      }
    }

    public static LoanTradeViewModel GetTradeViewForLoan(string guid)
    {
      LoanTradeViewModel[] viewsFromDatabase = LoanTrades.getTradeViewsFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, LoanTrades.TheTradeType));
      return viewsFromDatabase.Length == 0 ? (LoanTradeViewModel) null : viewsFromDatabase[0];
    }

    public static LoanTradeInfo GetTradeForLoan(string guid)
    {
      LoanTradeInfo[] tradesFromDatabase = LoanTrades.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, LoanTrades.TheTradeType));
      return tradesFromDatabase.Length == 0 ? (LoanTradeInfo) null : tradesFromDatabase[0];
    }

    public static LoanTradeInfo GetTradeForRejectedLoan(string guid)
    {
      LoanTradeInfo[] tradesFromDatabase = LoanTrades.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and isnull(Rejected, 0) = 1 and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), LoanTrades.TheTradeType));
      return tradesFromDatabase.Length == 0 ? (LoanTradeInfo) null : tradesFromDatabase[0];
    }

    public static LoanTradeViewModel[] GetActiveTradeView()
    {
      return LoanTrades.getTradeViewsFromDatabase("select TradeID from Trades where Status <> " + (object) 5);
    }

    public static LoanTradeViewModel[] GetTradeViewsByContractID(int contractId)
    {
      return LoanTrades.getTradeViewsFromDatabase("select TradeID from LoanTrades where ContractID = " + (object) contractId);
    }

    public static LoanTradeInfo[] GetLoanTradesByContractID(int contractId)
    {
      return LoanTrades.getTradesFromDatabase("select TradeID from LoanTrades where ContractID = " + (object) contractId);
    }

    public static void DeleteLoanFromTrades(
      string loanGuid,
      UserInfo currentUser,
      bool isExternalOrganization)
    {
      LoanTradeInfo tradeForLoan = LoanTrades.GetTradeForLoan(loanGuid);
      if (tradeForLoan != null)
      {
        PipelineInfo pipelineInfo = Pipeline.GetPipelineInfo((UserInfo) null, loanGuid, LoanTradeHistoryItem.RequiredPipelineInfoFields, PipelineData.Fields, isExternalOrganization);
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(tradeForLoan, pipelineInfo, LoanTradeStatus.Unassigned, currentUser));
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete ta from TradeAssignment ta inner join Trades t on t.TradeID = ta.TradeID where ta.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid) + " and t.TradeType = " + (object) 2);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      LoanTradeStatus[] statuses,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      LoanTrades.SetPendingTradeStatus(tradeId, loanIds, statuses, (string[]) null, currentUser, isExternalOrganization, removePendingLoan);
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      LoanTradeStatus[] statuses,
      string[] comments,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan,
      Decimal[] totalPrice = null,
      bool forceUpdateAllLoans = false)
    {
      bool flag = false;
      LoanTradeViewModel tradeView = LoanTrades.GetTradeView(tradeId);
      PipelineInfo[] pipelineInfoArray = Pipeline.Generate((UserInfo) null, loanIds, LoanTradeHistoryItem.RequiredPipelineInfoFields, PipelineData.Fields, isExternalOrganization, tradeType: TradeType.LoanTrade);
      for (int index = 0; index < loanIds.Length; ++index)
      {
        if (comments == null)
          flag |= LoanTrades.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], currentUser, removePendingLoan, totalPrice[index], forceUpdateAllLoans);
        else
          flag |= LoanTrades.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], comments[index], currentUser, removePendingLoan, totalPrice[index], forceUpdateAllLoans);
      }
      if (loanIds.Length != 0)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string str = loanIds.Length > 1 ? "'" + string.Join("', '", loanIds) + "'" : "'" + loanIds[0] + "'";
        dbQueryBuilder.AppendLine(string.Format("update TradeAssignment set PendingStatus = 0 where PendingStatus = AssignedStatus and tradeId = {0} and loanGuid in ({1})", (object) tradeId, (object) str));
        dbQueryBuilder.AppendLine(string.Format("delete from TradeAssignment where AssignedStatus = " + (object) 1 + " And PendingStatus = 0 And tradeId = {0} and loanGuid in ({1})", (object) tradeId, (object) str));
        dbQueryBuilder.ExecuteNonQuery();
      }
      if (!flag)
        return;
      LoanTrades.RecalculateProfits(tradeId, false);
    }

    private static bool setPendingTradeStatus(
      LoanTradeViewModel trade,
      PipelineInfo loanInfo,
      LoanTradeStatus status,
      UserInfo currentUser,
      bool removePendingLoan,
      Decimal totalPrice = 0M,
      bool forceUpdateAllLoans = false)
    {
      return LoanTrades.setPendingTradeStatus(trade, loanInfo, status, "", currentUser, removePendingLoan, totalPrice, forceUpdateAllLoans);
    }

    private static bool setPendingTradeStatus(
      LoanTradeViewModel trade,
      PipelineInfo loanInfo,
      LoanTradeStatus status,
      string comment,
      UserInfo currentUser,
      bool removePendingLoan,
      Decimal totalPrice = 0M,
      bool forceUpdateAllLoans = false)
    {
      if (status == LoanTradeStatus.None && !forceUpdateAllLoans)
        return false;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) trade.TradeID);
      dbValueList.Add("LoanGuid", (object) loanInfo.GUID);
      DbValueList values = new DbValueList();
      values.Add("PendingStatus", (object) (int) status);
      values.Add("PendingStatusDate", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
      values.Add("TotalPrice", (object) totalPrice);
      string format1 = "\r\n                    declare @tradeType int\r\n                    select @tradeType = tradetype from Trades where TradeID = {0}";
      dbQueryBuilder.AppendLine(string.Format(format1, (object) trade.TradeID, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID)));
      if (!removePendingLoan)
      {
        string format2 = "\r\n                    if exists (select 1\r\n                    from TradeAssignment ta\r\n                    inner join trades t on ta.tradeid = t.tradeid\r\n                    where ta.TradeID <> {0} And LoanGuid = {1}\r\n                    and  \r\n                    (\r\n                        t.TradeType = \r\n                           case @tradeType \r\n\t                            when 4 then 4   \r\n\t                            when 2 then 2\r\n\t                            when 3 then 3\r\n                            end\r\n                        or t.TradeType = \r\n                            case @tradeType \r\n\t                            when 4 then 4 \r\n\t                            when 2 then 3\r\n\t                            when 3 then 2\r\n                            end\r\n                    )\r\n                    and ta.AssignedStatus >= {2}\r\n                    )\r\n                    \r\n                    return";
        dbQueryBuilder.AppendLine(string.Format(format2, (object) trade.TradeID, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID), (object) 2));
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
      if (status != LoanTradeStatus.Unassigned)
      {
        string format3 = "\r\n                    select ta.TradeID, t.TradeType\r\n                    from TradeAssignment ta\r\n                    inner join trades t on ta.tradeid = t.tradeid\r\n                    where ta.TradeID <> {0} And LoanGuid = {1}\r\n                    and  \r\n                    (\r\n                        t.TradeType = \r\n                           case @tradeType \r\n\t                            when 4 then 4   \r\n\t                            when 2 then 2\r\n\t                            when 3 then 3\r\n                            end\r\n                        or t.TradeType = \r\n                            case @tradeType \r\n\t                            when 4 then 4 \r\n\t                            when 2 then 3\r\n\t                            when 3 then 2\r\n                            end\r\n                    )";
        dbQueryBuilder.AppendLine(string.Format(format3, (object) trade.TradeID, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID)));
        string format4 = "\r\n                    update TradeAssignment set PendingStatus = {1}\r\n                    from TradeAssignment ta\r\n                    inner join trades t on ta.tradeid = t.tradeid\r\n                    where ta.TradeID <> {0} And LoanGuid = {2}\r\n                    and  \r\n                    (\r\n                        t.TradeType = \r\n                           case @tradeType \r\n\t                            when 4 then 4   \r\n\t                            when 2 then 2\r\n\t                            when 3 then 3\r\n                            end\r\n                        or t.TradeType = \r\n                            case @tradeType \r\n\t                            when 4 then 4 \r\n\t                            when 2 then 3\r\n\t                            when 3 then 2\r\n                            end\r\n                    )";
        dbQueryBuilder.AppendLine(string.Format(format4, (object) trade.TradeID, (object) 1, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID)));
      }
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        throw new Exception("The loan has been assigned to another trade or pool.");
      bool flag = string.Concat(dataSet.Tables[0].Rows[0][0]) == "1";
      if (status != LoanTradeStatus.Unassigned)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
        {
          int tradeId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeID"]);
          switch (EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeType"]))
          {
            case 2:
              LoanTradeInfo trade1 = LoanTrades.GetTrade(tradeId);
              if (trade1 != null)
              {
                LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(trade1, loanInfo, LoanTradeStatus.Unassigned, currentUser));
                continue;
              }
              continue;
            case 3:
              MbsPoolInfo trade2 = MbsPools.GetTrade(tradeId);
              if (trade2 != null)
              {
                MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(trade2, loanInfo, MbsPoolLoanStatus.Unassigned, currentUser));
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      if (trade != null)
      {
        LoanTradeInfo trade3 = LoanTrades.GetTrade(trade.TradeID);
        if (flag && status != LoanTradeStatus.Assigned)
          LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(trade3, loanInfo, LoanTradeStatus.Assigned, currentUser));
        LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(trade3, loanInfo, status, comment, currentUser));
      }
      return flag;
    }

    public static void RecalculateProfits(int tradeId, bool isExternalOrganization)
    {
      LoanTrades.RecalculateProfits(LoanTrades.GetTrade(tradeId) ?? throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Trade, (object) tradeId), isExternalOrganization);
    }

    public static void CommitPendingTradeStatus(int tradeId, string loanId, LoanTradeStatus status)
    {
      LoanTrades.CommitPendingTradeStatus(tradeId, loanId, status, false);
    }

    public static void CommitPendingTradeStatus(
      int tradeId,
      string loanId,
      LoanTradeStatus status,
      bool rejected)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) tradeId);
      dbValueList.Add("LoanGuid", (object) loanId);
      dbValueList.Add("PendingStatus", (object) (int) status);
      dbQueryBuilder.IfExists(table, dbValueList);
      dbQueryBuilder.Begin();
      DbValueList values = new DbValueList();
      if (!rejected)
        values.Add("PendingStatus", (object) 0);
      values.Add("Rejected", (object) rejected, (IDbEncoder) DbEncoding.Flag);
      dbQueryBuilder.Update(table, values, dbValueList);
      dbQueryBuilder.End();
      if (status >= LoanTradeStatus.Assigned)
      {
        dbQueryBuilder.AppendLine("delete from TradeAssignment ");
        dbQueryBuilder.AppendLine(" where PendingStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId));
        dbQueryBuilder.AppendLine(" and TradeID <> " + (object) tradeId);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static LoanTradeHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      return LoanTrades.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
    }

    public static void RecalculateProfitFromLoan(string loanGuid, bool isExternalOrganization)
    {
      LoanTradeInfo[] loanTradesForLoan = LoanTrades.GetAllPendingLoanTradesForLoan(loanGuid);
      string[] pricingFieldList = LoanTrades.GeneratePricingFieldList((IEnumerable<LoanTradeInfo>) loanTradesForLoan);
      PipelineInfo pipelineInfo = Pipeline.GetPipelineInfo((UserInfo) null, loanGuid, pricingFieldList, PipelineData.Fields, isExternalOrganization);
      if (pipelineInfo == null)
        return;
      LoanTrades.RecalculateProfits(loanTradesForLoan, pipelineInfo);
    }

    public static LoanTradeInfo[] GetAllPendingLoanTradesForLoan(string guid)
    {
      return LoanTrades.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), LoanTrades.TheTradeType));
    }

    public static string[] GeneratePricingFieldList(int[] tradeIds)
    {
      return LoanTrades.GeneratePricingFieldList((IEnumerable<LoanTradeInfo>) LoanTrades.GetTrades(tradeIds));
    }

    public static void ApplyTradeStatusFromLoan(PipelineInfo pinfo)
    {
      string tradeGuid = string.Concat(pinfo.GetField("TradeGuid"));
      if (tradeGuid == string.Empty)
        tradeGuid = string.Concat(pinfo.GetField("CorrespondentTradeGuid"));
      string str = string.Concat(pinfo.GetField("InvestorStatus"));
      DateTime date = Utils.ParseDate(pinfo.GetField("InvestorStatusDate"), DateTime.Now);
      if (tradeGuid == "")
      {
        LoanTrades.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, LoanTradeStatus.Unassigned, date);
      }
      else
      {
        switch (str)
        {
          case "Purchased":
            LoanTrades.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, LoanTradeStatus.Purchased, date);
            break;
          case "Shipped":
            LoanTrades.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, LoanTradeStatus.Shipped, date);
            break;
          default:
            LoanTrades.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, LoanTradeStatus.Assigned, date);
            break;
        }
      }
    }

    public static void SetTradeAssignmentStatus(
      string loanId,
      string tradeGuid,
      LoanTradeStatus loanStatus,
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
      if (loanStatus == LoanTradeStatus.Unassigned)
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
            (object) "(case PendingStatus when 2 then 0 when 3 then 0 when 4 then 0 else PendingStatus end)",
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

    public static void RecalculateProfits(LoanTradeInfo[] trades, PipelineInfo pinfo)
    {
      ChunkedList<LoanTradeInfo> chunkedList = new ChunkedList<LoanTradeInfo>(trades, 50);
      LoanTradeInfo[] loanTradeInfoArray;
      while ((loanTradeInfoArray = chunkedList.Next()) != null)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        for (int index = 0; index < loanTradeInfoArray.Length; ++index)
        {
          Decimal num;
          if (!loanTradeInfoArray[index].Pricing.IsAdvancedPricing)
          {
            num = loanTradeInfoArray[index].CalculateProfit(pinfo, 0M);
          }
          else
          {
            SecurityTradeInfo trade = SecurityTrades.GetTrade(loanTradeInfoArray[index].SecurityTradeID);
            num = trade != null ? loanTradeInfoArray[index].CalculateProfit(pinfo, trade.Price) : loanTradeInfoArray[index].CalculateProfit(pinfo, 0M);
          }
          dbQueryBuilder.Append("update TradeAssignment set Profit = " + num.ToString("0.00") + " where TradeID = " + (object) loanTradeInfoArray[index].TradeID + " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) pinfo.GUID));
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static Dictionary<int, string> GetEligibleLoanTradeByLoanInfo()
    {
      Dictionary<int, string> loanTradeByLoanInfo = new Dictionary<int, string>();
      string text = string.Format("SELECT LoanTradeDetails.TradeID, LoanTradeDetails.Name \r\n                              FROM LoanTradeDetails\r\n                              LEFT JOIN FN_GetLoanTradeSummaryInline({0}) TradeLoanTradeSummary\r\n                                  ON LoanTradeDetails.TradeID = TradeLoanTradeSummary.TradeID \r\n                              WHERE LoanTradeDetails.Status < 5\r\n                              AND Locked = 0", (object) 1);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          loanTradeByLoanInfo.Add((int) dataRow["TradeID"], dataRow["Name"].ToString());
      }
      return loanTradeByLoanInfo;
    }

    public static bool IsTradeAssignmentUpdateCompleted(
      int tradeId,
      string loanGuid,
      LoanTradeStatus status)
    {
      string text = string.Empty;
      if (status == LoanTradeStatus.Unassigned)
        text = string.Format("SELECT * \r\n                              FROM TradeAssignment\r\n                              WHERE TradeID = {0}\r\n                              AND LoanGuid = '{1}'", (object) tradeId, (object) loanGuid);
      else if (status >= LoanTradeStatus.Assigned)
        text = string.Format("SELECT * \r\n                              FROM TradeAssignment\r\n                              WHERE TradeID = {0}\r\n                              AND LoanGuid = '{1}'\r\n                              AND AssignedStatus >= 2\r\n                              AND PendingStatus = 0", (object) tradeId, (object) loanGuid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return status == LoanTradeStatus.Unassigned ? dataRowCollection == null || dataRowCollection.Count <= 0 : status >= LoanTradeStatus.Assigned && dataRowCollection != null && dataRowCollection.Count > 0;
    }

    public static List<TradeUnlockInfo> GetPendingTrades(List<TradeType> tradeTypes, int timeWait)
    {
      string str = string.Empty;
      foreach (TradeType tradeType in tradeTypes)
        str = str + (object) (int) tradeType + ",";
      if (str.Length > 1)
        str = str.Substring(0, str.Length - 1);
      List<TradeUnlockInfo> pendingTrades = new List<TradeUnlockInfo>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select t.tradeId,");
      dbQueryBuilder.AppendLine("       case when b.status in (3, 4, 5, 6, 8) then -1 else b.batchJobId end as batchJobId,");
      dbQueryBuilder.AppendLine("       t.name, t.status, t.tradeType, t.PendingBy, b.metadata");
      dbQueryBuilder.AppendLine("  from trades t inner join batchjobs b on b.EntityId = t.TradeID");
      dbQueryBuilder.AppendLine(" where t.status = 6 and tradetype in (" + str + ") ");
      dbQueryBuilder.AppendLine("   and b.BatchJobId in");
      dbQueryBuilder.AppendLine("        (select max(batchjobid)");
      dbQueryBuilder.AppendLine("           from batchjobs");
      dbQueryBuilder.AppendLine("          where type = 1");
      dbQueryBuilder.AppendLine("         group by EntityId)");
      dbQueryBuilder.AppendLine("   and (b.status in (3, 5, 6, 8)");
      dbQueryBuilder.AppendLine("        or b.status in (1, 2, 4, 7) and DATEDIFF(minute, b.StatusDate, getUtcDate()) > " + (object) (timeWait > 0 ? timeWait : 20) + ")");
      DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
      if (table == null)
        return new List<TradeUnlockInfo>();
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        TradeUnlockInfo tradeUnlockInfo = new TradeUnlockInfo();
        tradeUnlockInfo.batchJobId = (int) table.Rows[index]["BatchJobId"];
        tradeUnlockInfo.jobMetaData = table.Rows[index]["MetaData"].ToString();
        TradeInfoObj tradeInfoObj1 = new TradeInfoObj();
        tradeInfoObj1.TradeID = (int) table.Rows[index]["TradeId"];
        tradeInfoObj1.Name = table.Rows[index]["Name"].ToString();
        tradeInfoObj1.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(table.Rows[index]["Status"], 0);
        tradeInfoObj1.TradeType = (TradeType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(table.Rows[index]["TradeType"], 0);
        tradeInfoObj1.PendingBy = table.Rows[index]["PendingBy"].ToString();
        TradeInfoObj tradeInfoObj2 = tradeInfoObj1;
        tradeUnlockInfo.tradeInfo = tradeInfoObj2;
        pendingTrades.Add(tradeUnlockInfo);
      }
      return pendingTrades;
    }

    public static TradeStatus GetTradeStatus(int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select LoanTradeDetails.Status from LoanTradeDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on LoanTradeDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine(" where LoanTradeDetails.TradeID =" + (object) tradeId);
      return (TradeStatus) Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
    }

    public class TradeLoanCursorItemComparer : IComparer
    {
      private SortField sortField;

      public TradeLoanCursorItemComparer(SortField sortOrder) => this.sortField = sortOrder;

      public int Compare(object a, object b)
      {
        PipelineInfo pinfo1 = (PipelineInfo) a;
        PipelineInfo pinfo2 = (PipelineInfo) b;
        object sortValue1 = this.getSortValue(pinfo1, this.sortField);
        object sortValue2 = this.getSortValue(pinfo2, this.sortField);
        if (!object.Equals(sortValue1, sortValue2))
        {
          int num;
          if (sortValue1 == null)
            num = -1;
          else if (sortValue2 == null)
          {
            num = 1;
          }
          else
          {
            try
            {
              num = ((IComparable) sortValue1).CompareTo(sortValue2);
            }
            catch
            {
              num = 0;
            }
          }
          if (num != 0)
            return this.sortField.SortOrder != FieldSortOrder.Ascending ? -num : num;
        }
        return 0;
      }

      private object getSortValue(PipelineInfo pinfo, SortField field)
      {
        string key = "SortKey." + field.Term.FieldName;
        if (pinfo.Info.ContainsKey((object) key))
          return pinfo.Info[(object) key];
        object sortValue = pinfo.Info[(object) field.Term.FieldName];
        pinfo.Info[(object) key] = sortValue;
        return sortValue;
      }
    }
  }
}
