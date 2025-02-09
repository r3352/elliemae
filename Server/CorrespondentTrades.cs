// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CorrespondentTrades
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Trading;
using EllieMae.EMLite.Collections;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Trading;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class CorrespondentTrades
  {
    private static string className = nameof (CorrespondentTrades);
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

    public static CorrespondentTradeInfo[] GetAllTrades()
    {
      return CorrespondentTrades.getTradeInfosFromDatabase("select TradeID from CorrespondentTrades", false);
    }

    public static CorrespondentTradeInfo GetTrade(int tradeId)
    {
      CorrespondentTradeInfo[] infosFromDatabase = CorrespondentTrades.getTradeInfosFromDatabase(string.Concat((object) tradeId), true, assingedAndPendingLoans: true);
      return infosFromDatabase.Length == 0 ? (CorrespondentTradeInfo) null : infosFromDatabase[0];
    }

    public static CorrespondentTradeInfo GetTrade(string tradeName)
    {
      CorrespondentTradeInfo[] infosFromDatabase = CorrespondentTrades.getTradeInfosFromDatabase("select TradeID from Trades where Name = '" + tradeName + "'", true);
      return infosFromDatabase.Length == 0 ? (CorrespondentTradeInfo) null : infosFromDatabase[0];
    }

    private static CorrespondentTradeInfo[] getTradeInfosFromDatabase(
      string criteria,
      bool checkId,
      bool parseObjects = true,
      bool assingedAndPendingLoans = false)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from CorrespondentTradeDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select CorrespondentTradeDetails.*, TradeObjects.*, TradeCorrespondentTradeSummary.* from CorrespondentTradeDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on CorrespondentTradeDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetCorrespondentTradeSummaryInline(" + (object) 1 + ") TradeCorrespondentTradeSummary on CorrespondentTradeDetails.TradeID = TradeCorrespondentTradeSummary.TradeID");
      dbQueryBuilder.AppendLine("   where CorrespondentTradeDetails.TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by TradeID, PendingStatus");
      dbQueryBuilder.AppendLine("select a.tradeid, l.*, LoanSummary.WithdrawnDate from LoanSummaryExtension l");
      dbQueryBuilder.AppendLine(" inner join TradeAssignment a on a.LoanGuid = l.Guid");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on l.Guid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where a.TradeID in (" + criteria + ")");
      if (!assingedAndPendingLoans)
        dbQueryBuilder.AppendLine("   and AssignedStatus = 2 and PendingStatus = 0");
      else
        dbQueryBuilder.AppendLine("   and AssignedStatus >= 1");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      DataTable table3 = dataSet.Tables[2];
      CorrespondentTradeInfo[] infosFromDatabase = new CorrespondentTradeInfo[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        CorrespondentTradeInfo tradeInfo = CorrespondentTrades.dataRowToTradeInfo(table1.Rows[index], table2, table3, parseObjects);
        infosFromDatabase[index] = tradeInfo;
        if (checkId & parseObjects && CorrespondentTrades.EntityIdIsNull(table1.Rows[index]))
          CorrespondentTrades.UpdateTradeObjects(tradeInfo);
      }
      return infosFromDatabase;
    }

    private static bool EntityIdIsNull(DataRow r)
    {
      TradePriceAdjustmentsNoId source = TradePriceAdjustmentsNoId.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AdjustmentsXml"]));
      return source == null || source.FirstOrDefault<TradePriceAdjustmentNoId>((System.Func<TradePriceAdjustmentNoId, bool>) (x => x.Id != Guid.Empty)) == null;
    }

    private static void UpdateTradeObjects(CorrespondentTradeInfo tradeInfo)
    {
      DbValueList objectsValueList = CorrespondentTrades.createTradeObjectsValueList(tradeInfo);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeObjects");
      DbValue key = new DbValue("TradeID", (object) tradeInfo.TradeID);
      dbQueryBuilder.DeleteFrom(table, key);
      objectsValueList.Add(key);
      dbQueryBuilder.InsertInto(table, objectsValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static CorrespondentTradeViewModel[] getViewModelsFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from CorrespondentTradeDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ");
      dbQueryBuilder.AppendLine("\tCorrespondentTradeDetails.*, ");
      dbQueryBuilder.AppendLine("\tTradeObjects.*, ");
      dbQueryBuilder.AppendLine("\tTradeCorrespondentTradeSummary.* ");
      dbQueryBuilder.AppendLine("from ");
      dbQueryBuilder.AppendLine("\tCorrespondentTradeDetails ");
      dbQueryBuilder.AppendLine("\tinner join TradeObjects ");
      dbQueryBuilder.AppendLine("\t\ton  CorrespondentTradeDetails.TradeID = TradeObjects.TradeID ");
      dbQueryBuilder.AppendLine("\tleft outer join\tFN_GetCorrespondentTradeSummaryInline(" + (object) 1 + ") TradeCorrespondentTradeSummary ");
      dbQueryBuilder.AppendLine("\t\ton CorrespondentTradeDetails.TradeID = TradeCorrespondentTradeSummary.TradeID ");
      dbQueryBuilder.AppendLine("where ");
      dbQueryBuilder.AppendLine("\tCorrespondentTradeDetails.TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by TradeID, PendingStatus");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      CorrespondentTradeViewModel[] modelsFromDatabase = new CorrespondentTradeViewModel[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        CorrespondentTradeViewModel correspondentTradeViewModel = CorrespondentTrades.dataRowToCorrespondentTradeViewModel(table1.Rows[index], table2);
        modelsFromDatabase[index] = correspondentTradeViewModel;
      }
      return modelsFromDatabase;
    }

    private static CorrespondentTradeViewModel dataRowToCorrespondentTradeViewModel(
      DataRow r,
      DataTable statusTable)
    {
      Dictionary<CorrespondentTradeLoanStatus, int> pendingLoanCounts = CorrespondentTrades.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      CorrespondentTradeViewModel correspondentTradeViewModel = new CorrespondentTradeViewModel();
      correspondentTradeViewModel.TradeID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]);
      correspondentTradeViewModel.CommitmentID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CorrespondentMasterID"]);
      correspondentTradeViewModel.CorrespondentMasterCommitmentNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]);
      correspondentTradeViewModel.Guid = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]);
      correspondentTradeViewModel.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]);
      correspondentTradeViewModel.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      correspondentTradeViewModel.CommitmentType = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CommitmentType"]).ToString();
      correspondentTradeViewModel.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]).ToString();
      correspondentTradeViewModel.CompanyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CompanyName"]);
      correspondentTradeViewModel.DeliveryType = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["DeliveryType"]).ToString();
      correspondentTradeViewModel.TPOID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ExternalID"]);
      correspondentTradeViewModel.OrganizationID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrganizationID"]);
      correspondentTradeViewModel.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      correspondentTradeViewModel.Tolerance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]);
      correspondentTradeViewModel.MinAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAmount"]);
      correspondentTradeViewModel.MaxAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAmount"]);
      correspondentTradeViewModel.ExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ExpirationDate"]);
      correspondentTradeViewModel.DeliveryExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["DeliveryExpirationDate"]);
      correspondentTradeViewModel.AOTOriginalTradeDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AOTOriginalTradeDate"]);
      correspondentTradeViewModel.AOTOriginalTradeDealer = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTOriginalTradeDealer"]);
      correspondentTradeViewModel.AOTSecurityType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTSecurityType"]);
      correspondentTradeViewModel.AOTSecurityTerm = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTSecurityTerm"]);
      correspondentTradeViewModel.AOTSecurityPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AOTSecurityPrice"]);
      correspondentTradeViewModel.AOTSecurityCoupon = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AOTSecurityCoupon"]);
      correspondentTradeViewModel.AOTSettlementDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AOTSettlementDate"]);
      correspondentTradeViewModel.PairOffAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]);
      correspondentTradeViewModel.CompletionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      correspondentTradeViewModel.ExternalOriginatorManagementID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOriginatorManagementID"]);
      correspondentTradeViewModel.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]);
      correspondentTradeViewModel.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OpenAmount1"]);
      correspondentTradeViewModel.AssignedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalAmount"]);
      correspondentTradeViewModel.PendingLoanCounts = pendingLoanCounts;
      correspondentTradeViewModel.PendingLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PendingLoanCount"]);
      correspondentTradeViewModel.GainLossAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GainLossAmount"]);
      correspondentTradeViewModel.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"]);
      correspondentTradeViewModel.AutoCreated = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["AutoCreated"]);
      correspondentTradeViewModel.IsWeightedAvgBulkPriceLocked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsWeightedAvgBulkPriceLocked"]);
      correspondentTradeViewModel.WeightedAvgBulkPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["WeightedAvgBulkPrice"]);
      correspondentTradeViewModel.LoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["LoanCount"]);
      correspondentTradeViewModel.NotWithdrawnLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["NotWithdrawnLoanCount"]);
      correspondentTradeViewModel.IsToleranceLocked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsToleranceLocked"]);
      correspondentTradeViewModel.FundType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FundType"]);
      correspondentTradeViewModel.OriginationRepWarrantType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OriginationRepWarrantType"]);
      correspondentTradeViewModel.AgencyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AgencyName"]);
      correspondentTradeViewModel.AgencyDeliveryType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AgencyDeliveryType"]);
      correspondentTradeViewModel.DocCustodian = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DocCustodian"]);
      correspondentTradeViewModel.AuthorizedTraderUserId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderUserId"]);
      correspondentTradeViewModel.AuthorizedTraderName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderName"]);
      correspondentTradeViewModel.AuthorizedTraderEmail = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderEmail"]);
      correspondentTradeViewModel.LastPublishedDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LastPublishedDateTime"]);
      correspondentTradeViewModel.DeliveredAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["DeliveredAmount"]);
      correspondentTradeViewModel.DeliveredPercentage = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["DeliveredPercentage"]);
      correspondentTradeViewModel.PurchasedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PurchasedAmount"]);
      correspondentTradeViewModel.PurchasedPercentage = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PurchasedPercentage"]);
      correspondentTradeViewModel.RejectedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RejectedAmount"]);
      correspondentTradeViewModel.RejectedPercentage = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RejectedPercentage"]);
      correspondentTradeViewModel.MinNetOpen = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinNetOpen"]);
      correspondentTradeViewModel.MaxNetOpen = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxNetOpen"]);
      correspondentTradeViewModel.PurchasedLoanAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PurchasedLoanAmount"]);
      correspondentTradeViewModel.PurchasedLoanAmountPercentage = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PurchasedLoanAmountPercentage"]);
      return correspondentTradeViewModel;
    }

    private static CorrespondentTradeInfo dataRowToTradeInfo(
      DataRow r,
      DataTable statusTable,
      DataTable loanStatusTable,
      bool parseObjects = true)
    {
      Dictionary<CorrespondentTradeLoanStatus, int> dictionary = new Dictionary<CorrespondentTradeLoanStatus, int>();
      if (statusTable != null)
        dictionary = CorrespondentTrades.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      List<LoanSummaryExtension> summaryExtensionList = new List<LoanSummaryExtension>();
      if (loanStatusTable != null)
        summaryExtensionList = CorrespondentTrades.dataRowsToLoans((IEnumerable) loanStatusTable.Select("TradeId = " + r["TradeID"]));
      CorrespondentTradeInfo correspondentTradeInfo = new CorrespondentTradeInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]));
      correspondentTradeInfo.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]);
      correspondentTradeInfo.CorrespondentMasterID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CorrespondentMasterID"], -1);
      correspondentTradeInfo.CorrespondentMasterCommitmentNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]);
      correspondentTradeInfo.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      correspondentTradeInfo.CommitmentType = (CorrespondentTradeCommitmentType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CommitmentType"]);
      correspondentTradeInfo.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]);
      correspondentTradeInfo.CompanyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CompanyName"]);
      correspondentTradeInfo.DeliveryType = (CorrespondentMasterDeliveryType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["DeliveryType"]);
      correspondentTradeInfo.TPOID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ExternalID"]);
      correspondentTradeInfo.OrganizationID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrganizationID"]);
      correspondentTradeInfo.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      correspondentTradeInfo.Tolerance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]);
      correspondentTradeInfo.MinAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAmount"]);
      correspondentTradeInfo.MaxAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAmount"]);
      correspondentTradeInfo.ExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ExpirationDate"]);
      correspondentTradeInfo.DeliveryExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["DeliveryExpirationDate"]);
      correspondentTradeInfo.AOTOriginalTradeDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AOTOriginalTradeDate"]);
      correspondentTradeInfo.AOTOriginalTradeDealer = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTOriginalTradeDealer"]);
      correspondentTradeInfo.AOTSecurityType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTSecurityType"]);
      correspondentTradeInfo.AOTSecurityTerm = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTSecurityTerm"]);
      correspondentTradeInfo.AOTSecurityPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AOTSecurityPrice"]);
      correspondentTradeInfo.AOTSecurityCoupon = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AOTSecurityCoupon"]);
      correspondentTradeInfo.AOTSettlementDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AOTSettlementDate"]);
      correspondentTradeInfo.PairOffAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]);
      correspondentTradeInfo.RejectedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RejectedAmount"]);
      correspondentTradeInfo.PurchasedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PurchasedAmount"]);
      correspondentTradeInfo.DeliveredAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["DeliveredAmount"]);
      correspondentTradeInfo.CompletionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      correspondentTradeInfo.ExternalOriginatorManagementID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOriginatorManagementID"]);
      correspondentTradeInfo.CorrespondentTradePairOffs = BinaryConvertible<CorrespondentTradePairOffs>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PairOffsXml"]));
      correspondentTradeInfo.CopyOfCorrespondentTradePairOffs = BinaryConvertible<CorrespondentTradePairOffs>.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PairOffsXml"]));
      correspondentTradeInfo.PendingLoanList = dictionary;
      correspondentTradeInfo.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0);
      correspondentTradeInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OpenAmount"]);
      correspondentTradeInfo.GainLossAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GainLossAmount"]);
      correspondentTradeInfo.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"]);
      correspondentTradeInfo.AssignedLoanList = summaryExtensionList;
      correspondentTradeInfo.AutoCreated = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["AutoCreated"]);
      correspondentTradeInfo.AutoCreateLoanGUID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AutoCreateLoanGUID"]);
      correspondentTradeInfo.OverrideTradeName = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["OverrideTradeName"]);
      correspondentTradeInfo.SRPfromPPE = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["SRPfromPPE"]);
      correspondentTradeInfo.AdjustmentsfromPPE = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["AdjustmentsfromPPE"]);
      correspondentTradeInfo.WeightedAvgBulkPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["WeightedAvgBulkPrice"]);
      correspondentTradeInfo.IsWeightedAvgBulkPriceLocked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsWeightedAvgBulkPriceLocked"]);
      correspondentTradeInfo.IsToleranceLocked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsToleranceLocked"]);
      correspondentTradeInfo.FundType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FundType"]);
      correspondentTradeInfo.OriginationRepWarrantType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OriginationRepWarrantType"]);
      correspondentTradeInfo.AgencyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AgencyName"]);
      correspondentTradeInfo.AgencyDeliveryType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AgencyDeliveryType"]);
      correspondentTradeInfo.DocCustodian = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DocCustodian"]);
      correspondentTradeInfo.AuthorizedTraderUserId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderUserId"]);
      correspondentTradeInfo.AuthorizedTraderName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderName"]);
      correspondentTradeInfo.AuthorizedTraderEmail = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderEmail"]);
      correspondentTradeInfo.LastPublishedDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastPublishedDateTime"]);
      CorrespondentTradeInfo tradeInfo = correspondentTradeInfo;
      if (parseObjects)
        tradeInfo.ParseTradeObjects(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Notes"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FilterQueryXml"]), "", EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PricingXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AdjustmentsXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SRPTableXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DealerXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AssigneeXml"]), "", "", "", EllieMae.EMLite.DataAccess.SQL.DecodeString(r["EPPSLoanProgramQueryFilterXml"]));
      for (short index = 0; (int) index < tradeInfo.CorrespondentTradePairOffs.Count; ++index)
      {
        tradeInfo.CorrespondentTradePairOffs[(int) index].Guid = Guid.NewGuid();
        tradeInfo.CopyOfCorrespondentTradePairOffs[(int) index].Guid = tradeInfo.CorrespondentTradePairOffs[(int) index].Guid;
        tradeInfo.CorrespondentTradePairOffs[(int) index].TradeHistoryAction = TradeHistoryAction.PairOffNotChanged;
      }
      return tradeInfo;
    }

    public static CorrespondentTradeHistoryItem[] GetTradeHistory(int tradeId)
    {
      return CorrespondentTrades.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where TradeID = " + (object) tradeId);
    }

    private static CorrespondentTradeHistoryItem[] getTradeHistoryFromDatabase(string selectionQuery)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select TradeHistory.*, LoanSummary.LoanNumber, Trades.Name as TradeName, Trades.InvestorName, MasterContracts.ContractNumber");
      dbQueryBuilder.AppendLine("from TradeHistory inner join Trades on TradeHistory.TradeID = Trades.TradeID and TradeType = 4");
      dbQueryBuilder.AppendLine("   left outer join LoanSummary on TradeHistory.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on TradeHistory.ContractID = MasterContracts.ContractID");
      if ((selectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("where TradeHistory.HistoryID in (" + selectionQuery + ")");
      dbQueryBuilder.AppendLine("order by Timestamp asc");
      return CorrespondentTrades.dataRowsToTradeHistoryItems((IEnumerable) dbQueryBuilder.Execute());
    }

    private static CorrespondentTradeHistoryItem[] dataRowsToTradeHistoryItems(IEnumerable rows)
    {
      List<CorrespondentTradeHistoryItem> tradeHistoryItemList = new List<CorrespondentTradeHistoryItem>();
      foreach (DataRow row in rows)
        tradeHistoryItemList.Add(CorrespondentTrades.dataRowToHistoryItem(row));
      return tradeHistoryItemList.ToArray();
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      int tradeId,
      string[] fields,
      bool isExternalOrganization)
    {
      return CorrespondentTrades.GetAssignedOrPendingLoans(user, tradeId, fields, isExternalOrganization, 0);
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      int tradeId,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      CorrespondentTradeInfo trade = new CorrespondentTradeInfo();
      trade.TradeID = tradeId;
      if (trade == null)
        throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Template, (object) tradeId);
      return CorrespondentTrades.GetAssignedOrPendingLoans(user, trade, fields, isExternalOrganization, sqlRead);
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      CorrespondentTradeInfo trade,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      string identitySelectionQuery = "select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid where TradeAssignment.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 1;
      return Pipeline.Generate(user, identitySelectionQuery, fields, PipelineData.Trade, isExternalOrganization, TradeType.CorrespondentTrade, sqlRead);
    }

    public static CorrespondentMasterInfo GetMasterByMasterId(int masterId)
    {
      return CorrespondentMasters.GetCorrespondentMaster(masterId);
    }

    public static CorrespondentMasterInfo[] GetActiveMasters(CorrespondentTradeInfo info)
    {
      IList<MasterCommitmentType> masterCommitmentTypeList = (IList<MasterCommitmentType>) new List<MasterCommitmentType>();
      switch (info.CommitmentType)
      {
        case CorrespondentTradeCommitmentType.BestEfforts:
          masterCommitmentTypeList.Add(MasterCommitmentType.BestEfforts);
          masterCommitmentTypeList.Add(MasterCommitmentType.BothMandatoryAndBestEfforts);
          break;
        case CorrespondentTradeCommitmentType.Mandatory:
          masterCommitmentTypeList.Add(MasterCommitmentType.Mandatory);
          masterCommitmentTypeList.Add(MasterCommitmentType.BothMandatoryAndBestEfforts);
          break;
      }
      int originatorManagementId = info.ExternalOriginatorManagementID;
      IList<MasterCommitmentType> commitmentTypes = masterCommitmentTypeList;
      DateTime commitmentDate = info.CommitmentDate;
      DateTime expireDateTime = info.CommitmentDate != DateTime.MinValue ? info.CommitmentDate : DateTime.Today;
      return CorrespondentMasters.GetActiveCorrespondentMasterByTpo(originatorManagementId, commitmentTypes, expireDateTime);
    }

    public static CorrespondentMasterInfo GetMasterForTrade(int tradeId)
    {
      return CorrespondentMasters.GetCorrespondentMasterForTrade(tradeId);
    }

    public static int CreateTrade(CorrespondentTradeInfo info, UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (CorrespondentTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      dbQueryBuilder.Declare("@tradeId", "int");
      dbQueryBuilder.InsertInto(table1, CorrespondentTrades.createTradesValueList(info, true), true, false);
      dbQueryBuilder.SelectIdentity("@tradeId");
      DbValue dbValue = new DbValue("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
      DbValueList correspondentTradeValueList = CorrespondentTrades.createCorrespondentTradeValueList(info);
      correspondentTradeValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table2, correspondentTradeValueList, true, false);
      DbValueList objectsValueList = CorrespondentTrades.createTradeObjectsValueList(info, currentUser);
      objectsValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.Select("@tradeId");
      int tradeId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      info = CorrespondentTrades.GetTrade(tradeId);
      CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, TradeHistoryAction.TradeCreated, currentUser));
      foreach (CorrespondentTradePairOff correspondentTradePairOff in info.CorrespondentTradePairOffs)
      {
        TradeHistoryAction action = correspondentTradePairOff.TradeAmount < 0M ? TradeHistoryAction.PairOffReversed : TradeHistoryAction.PairOffCreated;
        CorrespondentTrades.AddPairOffHistoryItem(new CorrespondentTradePairOffHistoryItem(correspondentTradePairOff, info.TradeID, action, -1, currentUser));
      }
      if (info.Status != TradeStatus.Open)
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, info.Status, currentUser));
      if (info.Status == TradeStatus.Committed && info.LastPublishedDateTime != DateTime.MinValue)
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info.TradeID, TradeHistoryAction.TradePublished, info.LastPublishedDateTime, info.Status, currentUser));
      if (info.Locked)
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, TradeHistoryAction.TradeLocked, currentUser));
      if (info.CorrespondentMasterID > 0)
      {
        CorrespondentMasterInfo masterByMasterId = CorrespondentTrades.GetMasterByMasterId(info.CorrespondentMasterID);
        if (masterByMasterId != null)
          CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, masterByMasterId, TradeHistoryAction.ContractAssigned, currentUser));
      }
      return tradeId;
    }

    public static string ValidateAndCreateTrade(CorrespondentTradeInfo info, UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (CorrespondentTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      DbTableInfo table4 = DbAccessManager.GetTable("TradeNameAudit");
      if (string.IsNullOrEmpty(info.Name))
        info.Name = CorrespondentTrades.GenerateNextAutoNumber();
      dbQueryBuilder.Declare("@tradeId", "Varchar(1024)");
      DbValue[] values = new DbValue[2]
      {
        new DbValue("Name", (object) info.Name),
        new DbValue("TradeType", (object) 4)
      };
      DbValueList keyValues1 = new DbValueList(new DbValue[2]
      {
        new DbValue("TradeName", (object) info.Name),
        new DbValue("TradeType", (object) 4)
      });
      DbValueList keyValues2 = new DbValueList(values);
      dbQueryBuilder.IfExists(table4, keyValues1);
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("SET @tradeId = 'A correspondent trade with the commitment # ''" + info.Name + "'' was previously deleted.'");
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.IfExists(table1, keyValues2);
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("SET @tradeId = 'A correspondent trade with the commitment # ''" + info.Name + "'' already exists.'");
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table1, CorrespondentTrades.createTradesValueList(info, true), true, false);
      dbQueryBuilder.AppendLine("select @tradeId = convert(varchar(128), scope_identity())");
      DbValue dbValue = new DbValue("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
      DbValueList correspondentTradeValueList = CorrespondentTrades.createCorrespondentTradeValueList(info);
      correspondentTradeValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table2, correspondentTradeValueList, true, false);
      DbValueList objectsValueList = CorrespondentTrades.createTradeObjectsValueList(info, currentUser);
      objectsValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.End();
      dbQueryBuilder.End();
      dbQueryBuilder.Select("@tradeId");
      string s = EllieMae.EMLite.DataAccess.SQL.DecodeString(dbQueryBuilder.ExecuteScalar());
      int result;
      if (!int.TryParse(s, out result))
        return s;
      info = CorrespondentTrades.GetTrade(result);
      CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, TradeHistoryAction.TradeCreated, currentUser));
      foreach (CorrespondentTradePairOff correspondentTradePairOff in info.CorrespondentTradePairOffs)
      {
        TradeHistoryAction action = correspondentTradePairOff.TradeAmount < 0M ? TradeHistoryAction.PairOffReversed : TradeHistoryAction.PairOffCreated;
        CorrespondentTrades.AddPairOffHistoryItem(new CorrespondentTradePairOffHistoryItem(correspondentTradePairOff, info.TradeID, action, -1, currentUser));
      }
      if (info.Status != TradeStatus.Open)
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, info.Status, currentUser));
      if (info.Locked)
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, TradeHistoryAction.TradeLocked, currentUser));
      if (info.CorrespondentMasterID > 0)
      {
        CorrespondentMasterInfo masterByMasterId = CorrespondentTrades.GetMasterByMasterId(info.CorrespondentMasterID);
        if (masterByMasterId != null)
          CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, masterByMasterId, TradeHistoryAction.ContractAssigned, currentUser));
      }
      return s;
    }

    public static string GenerateNextAutoNumber()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select TheNumber from FN_GetNextCorrespondentCommitmentNumber()");
      string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(dbQueryBuilder.ExecuteScalar());
      return str.Equals("999999999999999999") || str.Equals("1000000000000000000") ? "" : str;
    }

    private static DbValueList createTradesValueList(CorrespondentTradeInfo info, bool isCreateNew)
    {
      DbValueList tradesValueList = new DbValueList();
      if (isCreateNew)
        tradesValueList.Add("Guid", (object) info.Guid);
      if (info.Name.ToLower() == "use_autonumber_reserved")
        tradesValueList.Add("Name", (object) "(select * from FN_GetNextCorrespondentCommitmentNumber())", (IDbEncoder) DbEncoding.None);
      else
        tradesValueList.Add("Name", (object) info.Name);
      if (info.Status != TradeStatus.Settled)
        tradesValueList.Add("Status", (object) (int) info.Status);
      tradesValueList.Add("Locked", (object) info.Locked, (IDbEncoder) DbEncoding.Flag);
      tradesValueList.Add("Tolerance", (object) info.Tolerance);
      tradesValueList.Add("PairOffFee", (object) info.PairOffFee);
      tradesValueList.Add("TradeAmount", (object) info.TradeAmount);
      tradesValueList.Add("CommitmentDate", (object) info.CommitmentDate);
      tradesValueList.Add("PairOffAmount", (object) info.PairOffAmount);
      tradesValueList.Add("CommitmentType", (object) (int) info.CommitmentType);
      tradesValueList.Add("TradeDescription", (object) info.TradeDescription);
      tradesValueList.Add("TradeType", (object) (int) info.TradeType);
      tradesValueList.Add("LastModified", (object) DateTime.Now);
      tradesValueList.Add("PendingBy", (object) info.PendingBy);
      return tradesValueList;
    }

    private static DbValueList createCorrespondentTradeValueList(CorrespondentTradeInfo info)
    {
      return new DbValueList()
      {
        {
          "CorrespondentMasterID",
          (object) info.CorrespondentMasterID,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "DeliveryType",
          (object) (int) info.DeliveryType
        },
        {
          "CompanyName",
          (object) info.CompanyName
        },
        {
          "ExternalID",
          (object) info.TPOID
        },
        {
          "OrganizationID",
          (object) info.OrganizationID
        },
        {
          "ExpirationDate",
          (object) info.ExpirationDate
        },
        {
          "DeliveryExpirationDate",
          (object) info.DeliveryExpirationDate
        },
        {
          "ExternalOriginatorManagementID",
          (object) info.ExternalOriginatorManagementID
        },
        {
          "AOTSecurityType",
          (object) info.AOTSecurityType
        },
        {
          "AOTSecurityTerm",
          (object) info.AOTSecurityTerm
        },
        {
          "AOTSecurityCoupon",
          (object) info.AOTSecurityCoupon
        },
        {
          "AOTSecurityPrice",
          (object) info.AOTSecurityPrice
        },
        {
          "AOTSettlementDate",
          (object) info.AOTSettlementDate
        },
        {
          "AOTOriginalTradeDate",
          (object) info.AOTOriginalTradeDate
        },
        {
          "AOTOriginalTradeDealer",
          (object) info.AOTOriginalTradeDealer
        },
        {
          "GainLossAmount",
          (object) info.GainLossAmount
        },
        {
          "AutoCreated",
          (object) (info.AutoCreated ? 1 : 0)
        },
        {
          "AutoCreateLoanGUID",
          (object) info.AutoCreateLoanGUID
        },
        {
          "OverrideTradeName",
          (object) (info.OverrideTradeName ? 1 : 0)
        },
        {
          "AdjustmentsfromPPE",
          (object) (info.AdjustmentsfromPPE ? 1 : 0)
        },
        {
          "SRPfromPPE",
          (object) (info.SRPfromPPE ? 1 : 0)
        },
        {
          "WeightedAvgBulkPrice",
          (object) info.WeightedAvgBulkPrice
        },
        {
          "IsWeightedAvgBulkPriceLocked",
          (object) (info.IsWeightedAvgBulkPriceLocked ? 1 : 0)
        },
        {
          "IsToleranceLocked",
          (object) (info.IsToleranceLocked ? 1 : 0)
        },
        {
          "FundType",
          (object) info.FundType
        },
        {
          "OriginationRepWarrantType",
          (object) info.OriginationRepWarrantType
        },
        {
          "AgencyName",
          (object) info.AgencyName
        },
        {
          "AgencyDeliveryType",
          (object) info.AgencyDeliveryType
        },
        {
          "DocCustodian",
          (object) info.DocCustodian
        },
        {
          "AuthorizedTraderUserId",
          (object) info.AuthorizedTraderUserId
        },
        {
          "LastPublishedDateTime",
          (object) info.LastPublishedDateTime
        }
      };
    }

    public static int AddPairOffHistoryItem(CorrespondentTradePairOffHistoryItem item)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeHistory");
      dbQueryBuilder.InsertInto(table, new DbValueList()
      {
        {
          "TradeID",
          (object) item.TradeID
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
        },
        {
          "PriorDataXml",
          (object) item.PriorTradeValues?.ToXml()
        }
      }, true, false);
      dbQueryBuilder.SelectIdentity();
      return EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    public static int AddTradeHistoryItem(CorrespondentTradeHistoryItem item)
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
        },
        {
          "PriorDataXml",
          (object) item.PriorTradeValues?.ToXml()
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
      CorrespondentTradeInfo info,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool checkStatus)
    {
      CorrespondentTradeInfo trade = CorrespondentTrades.GetTrade(info.TradeID);
      if (trade == null)
        throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) info.TradeID);
      if (checkStatus && (trade.Status == TradeStatus.Pending || trade.Status == TradeStatus.Archived))
        throw new TradeNotUpdateException(CorrespondentTrades.UPDATETRADEERROR);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (CorrespondentTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      DbValue key = new DbValue("TradeID", (object) info.TradeID);
      dbQueryBuilder.DeleteFrom(table3, key);
      dbQueryBuilder.Update(table1, CorrespondentTrades.createTradesValueList(info, false), key);
      dbQueryBuilder.Update(table2, CorrespondentTrades.createCorrespondentTradeValueList(info), key);
      DbValueList objectsValueList = CorrespondentTrades.createTradeObjectsValueList(info, currentUser);
      objectsValueList.Add(key);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
      CorrespondentTrades.AddHistoryForUpdateTrade(info, trade, currentUser, true);
    }

    public static void PublishTrade(
      CorrespondentTradeInfo info,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool checkStatus)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DateTime utc = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
      TradeStatus tradeStatus = TradeStatus.Committed;
      if (checkStatus)
      {
        dbQueryBuilder.AppendLine("if exists( SELECT * FROM Trades where TradeId = " + (object) info.TradeID + " and status in (" + (object) 6 + ", " + (object) 5 + "))");
        dbQueryBuilder.AppendLine("RAISERROR('UPDATETRADEERROR', 16, 1)");
        dbQueryBuilder.AppendLine("else");
      }
      dbQueryBuilder.AppendLine("update Trades set status = " + (object) (int) tradeStatus + " where TradeID  = " + (object) info.TradeID + "; update CorrespondentTrades set LastPublishedDateTime = '" + (object) utc + "' where TradeID  = " + (object) info.TradeID + ";");
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(CorrespondentTrades.UPDATETRADEERROR);
        throw;
      }
      CorrespondentTrades.AddHistoryForPublishTrade(info, tradeStatus, utc, currentUser);
    }

    public static void UpdateTrade(
      CorrespondentTradeInfo info,
      UserInfo currentUser,
      bool checkStatus,
      string action,
      List<string> modified)
    {
      if (!string.IsNullOrEmpty(action))
      {
        switch (action.ToUpper().Replace(" ", ""))
        {
          case "PUBLISH":
            CorrespondentTrades.PublishTrade(info, currentUser, false, true);
            break;
          case "ARCHIVE":
            CorrespondentTrades.SetTradeStatus(new int[1]
            {
              info.TradeID
            }, TradeStatus.Archived, TradeHistoryAction.TradeArchived, currentUser);
            return;
          case "VOID":
            CorrespondentTrades.SetTradeStatus(new int[1]
            {
              info.TradeID
            }, TradeStatus.Voided, TradeHistoryAction.TradeVoided, currentUser);
            return;
          case "MOVETOCURRENT":
            CorrespondentTrades.SetTradeStatus(new int[1]
            {
              info.TradeID
            }, TradeStatus.Open, TradeHistoryAction.TradeActivated, currentUser, false);
            return;
          case "UPDATESTATUS":
            CorrespondentTrades.SetTradeStatus(new int[1]
            {
              info.TradeID
            }, info.Status, TradeHistoryAction.StatusChangedManually, currentUser);
            return;
          default:
            return;
        }
      }
      if (modified.Count == 0)
        return;
      CorrespondentTradeInfo trade = CorrespondentTrades.GetTrade(info.TradeID);
      if (trade == null)
        throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) info.TradeID);
      if (CorrespondentTrades.IsModified("CommitmentNumber", modified, (object) info.Name, (object) trade.Name))
      {
        if (CorrespondentTrades.CheckTradeByName(info.Name))
          throw new TradeNotUpdateException("A correspondent trade with the commitment # '" + info.Name + "' was previously deleted.");
        if (CorrespondentTrades.CheckOtherTradeWithSameNameExists(info.TradeID, info.Name))
          throw new TradeNotUpdateException("A correspondent trade with the commitment # '" + info.Name + "' already exists.");
      }
      if (checkStatus && (trade.Status == TradeStatus.Pending || trade.Status == TradeStatus.Archived))
        throw new TradeNotUpdateException("The Trade must not be pending before updating. The Trade cannot be updated.");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (CorrespondentTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      DbValue key = new DbValue("TradeID", (object) info.TradeID);
      dbQueryBuilder.Update(table1, CorrespondentTrades.createTradesModifiedValueList(info, trade, modified), key);
      DbValueList modifiedValueList = CorrespondentTrades.createCorrespondentTradeModifiedValueList(info, trade, modified);
      if (modifiedValueList.Count > 0)
        dbQueryBuilder.Update(table2, modifiedValueList, key);
      DbValueList values = CorrespondentTrades.updateTradeObjectsValueList(info, trade, modified);
      if (values.Count > 0)
        dbQueryBuilder.Update(table3, values, key);
      dbQueryBuilder.ExecuteNonQuery();
      CorrespondentTrades.AddHistoryForUpdateTrade(CorrespondentTrades.GetTrade(info.TradeID), trade, currentUser, false);
    }

    private static DbValueList createTradesModifiedValueList(
      CorrespondentTradeInfo info,
      CorrespondentTradeInfo priorValue,
      List<string> modified)
    {
      DbValueList modifiedValueList = new DbValueList();
      if (CorrespondentTrades.IsModified("CommitmentNumber", modified, (object) info.Name, (object) priorValue.Name))
        modifiedValueList.Add("Name", (object) info.Name);
      if (CorrespondentTrades.IsModified("Status", modified, (object) info.Status, (object) priorValue.Status))
        modifiedValueList.Add("Status", (object) (int) info.Status);
      if (CorrespondentTrades.IsModified("Locked", modified, (object) info.Locked, (object) priorValue.Locked))
        modifiedValueList.Add("Locked", (object) info.Locked, (IDbEncoder) DbEncoding.Flag);
      if (CorrespondentTrades.IsModified("Tolerance", modified, (object) info.Tolerance, (object) priorValue.Tolerance))
        modifiedValueList.Add("Tolerance", (object) info.Tolerance);
      if (CorrespondentTrades.IsModified("PairOffFee", modified, (object) info.PairOffFee, (object) priorValue.PairOffFee))
        modifiedValueList.Add("PairOffFee", (object) info.PairOffFee);
      if (CorrespondentTrades.IsModified("TradeAmount", modified, (object) info.TradeAmount, (object) priorValue.TradeAmount))
        modifiedValueList.Add("TradeAmount", (object) info.TradeAmount);
      if (CorrespondentTrades.IsModified("CommitmentDate", modified, (object) info.CommitmentDate, (object) priorValue.CommitmentDate))
        modifiedValueList.Add("CommitmentDate", (object) info.CommitmentDate);
      if (CorrespondentTrades.IsModified("PairOffAmount", modified, (object) info.PairOffAmount, (object) priorValue.PairOffAmount))
        modifiedValueList.Add("PairOffAmount", (object) info.PairOffAmount);
      if (CorrespondentTrades.IsModified("CommitmentType", modified, (object) info.CommitmentType, (object) priorValue.CommitmentType))
        modifiedValueList.Add("CommitmentType", (object) (int) info.CommitmentType);
      if (CorrespondentTrades.IsModified("Description", modified, (object) info.TradeDescription, (object) priorValue.TradeDescription))
        modifiedValueList.Add("TradeDescription", (object) info.TradeDescription);
      if (CorrespondentTrades.IsModified("TradeType", modified, (object) info.TradeType, (object) priorValue.TradeType))
        modifiedValueList.Add("TradeType", (object) (int) info.TradeType);
      modifiedValueList.Add("LastModified", (object) DateTime.Now);
      if (CorrespondentTrades.IsModified("PendingBy", modified, (object) info.PendingBy, (object) priorValue.PendingBy))
        modifiedValueList.Add("PendingBy", (object) info.PendingBy);
      return modifiedValueList;
    }

    private static DbValueList createCorrespondentTradeModifiedValueList(
      CorrespondentTradeInfo info,
      CorrespondentTradeInfo priorValue,
      List<string> modified)
    {
      DbValueList modifiedValueList = new DbValueList();
      if (CorrespondentTrades.IsModified("CorrespondentMasterID", modified, (object) info.CorrespondentMasterID, (object) priorValue.CorrespondentMasterID))
        modifiedValueList.Add("CorrespondentMasterID", (object) info.CorrespondentMasterID, (IDbEncoder) DbEncoding.MinusOneAsNull);
      if (CorrespondentTrades.IsModified("DeliveryType", modified, (object) info.DeliveryType, (object) priorValue.DeliveryType))
        modifiedValueList.Add("DeliveryType", (object) (int) info.DeliveryType);
      if (CorrespondentTrades.IsModified("TpoCompanyName", modified, (object) info.CompanyName, (object) priorValue.CompanyName))
        modifiedValueList.Add("CompanyName", (object) info.CompanyName);
      if (CorrespondentTrades.IsModified("TPOID", modified, (object) info.TPOID, (object) priorValue.TPOID))
        modifiedValueList.Add("ExternalID", (object) info.TPOID);
      if (CorrespondentTrades.IsModified("OrganizationID", modified, (object) info.OrganizationID, (object) priorValue.OrganizationID))
        modifiedValueList.Add("OrganizationID", (object) info.OrganizationID);
      if (CorrespondentTrades.IsModified("ExpirationDate", modified, (object) info.ExpirationDate, (object) priorValue.ExpirationDate))
        modifiedValueList.Add("ExpirationDate", (object) info.ExpirationDate);
      if (CorrespondentTrades.IsModified("DeliveryExpirationDate", modified, (object) info.DeliveryExpirationDate, (object) priorValue.DeliveryExpirationDate))
        modifiedValueList.Add("DeliveryExpirationDate", (object) info.DeliveryExpirationDate);
      if (CorrespondentTrades.IsModified("ExternalOriginatorManagementID", modified, (object) info.ExternalOriginatorManagementID, (object) priorValue.ExternalOriginatorManagementID))
        modifiedValueList.Add("ExternalOriginatorManagementID", (object) info.ExternalOriginatorManagementID);
      if (CorrespondentTrades.IsModified("AOTSecurityType", modified, (object) info.AOTSecurityType, (object) priorValue.AOTSecurityType))
        modifiedValueList.Add("AOTSecurityType", (object) info.AOTSecurityType);
      if (CorrespondentTrades.IsModified("AOTSecurityTerm", modified, (object) info.AOTSecurityTerm, (object) priorValue.AOTSecurityTerm))
        modifiedValueList.Add("AOTSecurityTerm", (object) info.AOTSecurityTerm);
      if (CorrespondentTrades.IsModified("AOTSecurityCoupon", modified, (object) info.AOTSecurityCoupon, (object) priorValue.AOTSecurityCoupon))
        modifiedValueList.Add("AOTSecurityCoupon", (object) info.AOTSecurityCoupon);
      if (CorrespondentTrades.IsModified("AOTSecurityPrice", modified, (object) info.AOTSecurityPrice, (object) priorValue.AOTSecurityPrice))
        modifiedValueList.Add("AOTSecurityPrice", (object) info.AOTSecurityPrice);
      if (CorrespondentTrades.IsModified("AOTSettlementDate", modified, (object) info.AOTSettlementDate, (object) priorValue.AOTSettlementDate))
        modifiedValueList.Add("AOTSettlementDate", (object) info.AOTSettlementDate);
      if (CorrespondentTrades.IsModified("AOTOriginalTradeDate", modified, (object) info.AOTOriginalTradeDate, (object) priorValue.AOTOriginalTradeDate))
        modifiedValueList.Add("AOTOriginalTradeDate", (object) info.AOTOriginalTradeDate);
      if (CorrespondentTrades.IsModified("AOTOriginalTradeDealer", modified, (object) info.AOTOriginalTradeDealer, (object) priorValue.AOTOriginalTradeDealer))
        modifiedValueList.Add("AOTOriginalTradeDealer", (object) info.AOTOriginalTradeDealer);
      if (CorrespondentTrades.IsModified("GainLossAmount", modified, (object) info.GainLossAmount, (object) priorValue.GainLossAmount))
        modifiedValueList.Add("GainLossAmount", (object) info.GainLossAmount);
      if (CorrespondentTrades.IsModified("IsAutoCreated", modified, (object) info.AutoCreated, (object) priorValue.AutoCreated))
        modifiedValueList.Add("AutoCreated", (object) (info.AutoCreated ? 1 : 0));
      if (CorrespondentTrades.IsModified("AutoCreateLoanGUID", modified, (object) info.AutoCreateLoanGUID, (object) priorValue.AutoCreateLoanGUID))
        modifiedValueList.Add("AutoCreateLoanGUID", (object) info.AutoCreateLoanGUID);
      if (CorrespondentTrades.IsModified("OverrideTradeName", modified, (object) info.OverrideTradeName, (object) priorValue.OverrideTradeName))
        modifiedValueList.Add("OverrideTradeName", (object) (info.OverrideTradeName ? 1 : 0));
      if (CorrespondentTrades.IsModified("AdjustmentsfromPPE", modified, (object) info.AdjustmentsfromPPE, (object) priorValue.AdjustmentsfromPPE))
        modifiedValueList.Add("AdjustmentsfromPPE", (object) (info.AdjustmentsfromPPE ? 1 : 0));
      if (CorrespondentTrades.IsModified("SRPfromPPE", modified, (object) info.SRPfromPPE, (object) priorValue.SRPfromPPE))
        modifiedValueList.Add("SRPfromPPE", (object) (info.SRPfromPPE ? 1 : 0));
      if (CorrespondentTrades.IsModified("WeightedAvgBulkPrice", modified, (object) info.WeightedAvgBulkPrice, (object) priorValue.WeightedAvgBulkPrice))
        modifiedValueList.Add("WeightedAvgBulkPrice", (object) info.WeightedAvgBulkPrice);
      if (CorrespondentTrades.IsModified("IsWeightedAvgBulkPriceLocked", modified, (object) info.IsWeightedAvgBulkPriceLocked, (object) priorValue.IsWeightedAvgBulkPriceLocked))
        modifiedValueList.Add("IsWeightedAvgBulkPriceLocked", (object) (info.IsWeightedAvgBulkPriceLocked ? 1 : 0));
      if (CorrespondentTrades.IsModified("IsToleranceLocked", modified, (object) info.IsToleranceLocked, (object) priorValue.IsToleranceLocked))
        modifiedValueList.Add("IsToleranceLocked", (object) (info.IsToleranceLocked ? 1 : 0));
      if (CorrespondentTrades.IsModified("FundType", modified, (object) info.FundType, (object) priorValue.FundType))
        modifiedValueList.Add("FundType", (object) info.FundType);
      if (CorrespondentTrades.IsModified("OriginationRepWarrantType", modified, (object) info.OriginationRepWarrantType, (object) priorValue.OriginationRepWarrantType))
        modifiedValueList.Add("OriginationRepWarrantType", (object) info.OriginationRepWarrantType);
      if (CorrespondentTrades.IsModified("AgencyName", modified, (object) info.AgencyName, (object) priorValue.AgencyName))
        modifiedValueList.Add("AgencyName", (object) info.AgencyName);
      if (CorrespondentTrades.IsModified("AgencyDeliveryType", modified, (object) info.AgencyDeliveryType, (object) priorValue.AgencyDeliveryType))
        modifiedValueList.Add("AgencyDeliveryType", (object) info.AgencyDeliveryType);
      if (CorrespondentTrades.IsModified("DocCustodian", modified, (object) info.DocCustodian, (object) priorValue.DocCustodian))
        modifiedValueList.Add("DocCustodian", (object) info.DocCustodian);
      if (CorrespondentTrades.IsModified("AuthorizedTraderUserId", modified, (object) info.AuthorizedTraderUserId, (object) priorValue.AuthorizedTraderUserId))
        modifiedValueList.Add("AuthorizedTraderUserId", (object) info.AuthorizedTraderUserId);
      if (CorrespondentTrades.IsModified("LastPublishedDateTime", modified, (object) info.LastPublishedDateTime, (object) priorValue.LastPublishedDateTime))
        modifiedValueList.Add("LastPublishedDateTime", (object) info.LastPublishedDateTime);
      return modifiedValueList;
    }

    private static DbValueList updateTradeObjectsValueList(
      CorrespondentTradeInfo info,
      CorrespondentTradeInfo priorValue,
      List<string> modified)
    {
      DbValueList dbValueList = new DbValueList();
      if (modified.Contains("Filter") && info.Filter.FilterType == TradeFilterType.Advanced)
        dbValueList.Add("FilterQueryXml", info.Filter == null ? (object) (string) null : (object) info.Filter.ToXml());
      else if (modified.Contains("Filter") && info.Filter != null && info.Filter.FilterType == TradeFilterType.Simple)
      {
        if (priorValue.Filter.FilterType == TradeFilterType.Advanced)
        {
          TradeFilter tradeFilter = new TradeFilter(info.Filter.GetSimpleFilter(), info.Filter.DataLayout);
          dbValueList.Add("FilterQueryXml", tradeFilter == null ? (object) (string) null : (object) tradeFilter.ToXml());
        }
        else
        {
          bool flag1 = CorrespondentTrades.IsModified("MinNoteRate", modified, (object) info.Filter.GetSimpleFilter().NoteRateRange?.Minimum, (object) priorValue.Filter.GetSimpleFilter().NoteRateRange?.Minimum);
          bool flag2 = CorrespondentTrades.IsModified("MaxNoteRate", modified, (object) info.Filter.GetSimpleFilter().NoteRateRange?.Maximum, (object) priorValue.Filter.GetSimpleFilter().NoteRateRange?.Maximum);
          if (flag1 | flag2)
          {
            SimpleTradeFilter simpleFilter = priorValue.Filter.GetSimpleFilter();
            simpleFilter.NoteRateRange = new Range<Decimal>(flag1 ? info.Filter.GetSimpleFilter().NoteRateRange.Minimum : priorValue.Filter.GetSimpleFilter().NoteRateRange.Minimum, flag2 ? info.Filter.GetSimpleFilter().NoteRateRange.Maximum : priorValue.Filter.GetSimpleFilter().NoteRateRange.Maximum);
            priorValue.Filter = new TradeFilter(simpleFilter, priorValue.Filter.DataLayout);
            dbValueList.Add("FilterQueryXml", priorValue.Filter == null ? (object) (string) null : (object) priorValue.Filter.ToXml());
          }
        }
      }
      if (modified.Contains("PairOffs"))
        dbValueList.Add("PairOffsXml", info.CorrespondentTradePairOffs == null ? (object) (string) null : (object) info.CorrespondentTradePairOffs.ToXml());
      if (modified.Contains("PriceAdjustments"))
        dbValueList.Add("AdjustmentsXml", info.PriceAdjustments == null ? (object) (string) null : (object) info.PriceAdjustments.ToXml());
      if (modified.Contains("SrpTable"))
        dbValueList.Add("SRPTableXml", info.SRPTable == null ? (object) (string) null : (object) info.SRPTable.ToXml());
      if (modified.Contains("SimpleTradePricingItems"))
        dbValueList.Add("PricingXml", info.Pricing == null ? (object) (string) null : (object) info.Pricing.ToXml());
      if (modified.Contains("EppsLoanPrograms"))
        dbValueList.Add("EPPSLoanProgramQueryFilterXml", info.EPPSLoanProgramsFilter == null ? (object) (string) null : (object) info.EPPSLoanProgramsFilter.ToXml());
      return dbValueList;
    }

    private static bool IsModified(
      string key,
      List<string> allModified,
      object newValue,
      object priorValue)
    {
      bool flag = false;
      if (!allModified.Contains<string>(key, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) || (!(newValue.GetType() == typeof (string)) || ((string) newValue).Equals((string) priorValue, StringComparison.OrdinalIgnoreCase)) && newValue.Equals(priorValue))
        return flag;
      flag = true;
      return flag;
    }

    private static void AddHistoryForUpdateTrade(
      CorrespondentTradeInfo info,
      CorrespondentTradeInfo priorValue,
      UserInfo currentUser,
      bool forEncompass)
    {
      bool flag = false;
      if (info.Locked != priorValue.Locked)
      {
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, info.Locked ? TradeHistoryAction.TradeLocked : TradeHistoryAction.TradeUnlocked, currentUser));
        flag = true;
      }
      if (info.Status != priorValue.Status)
      {
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, info.Status, currentUser, priorValue));
        flag = true;
      }
      if (info.CorrespondentMasterID != priorValue.CorrespondentMasterID)
      {
        if (priorValue.CorrespondentMasterID > 0)
        {
          CorrespondentMasterInfo masterByMasterId = CorrespondentTrades.GetMasterByMasterId(priorValue.CorrespondentMasterID);
          if (masterByMasterId != null)
          {
            CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, masterByMasterId, TradeHistoryAction.ContractUnassigned, currentUser, priorValue));
            flag = true;
          }
        }
        if (info.CorrespondentMasterID > 0)
        {
          CorrespondentMasterInfo masterByMasterId = CorrespondentTrades.GetMasterByMasterId(info.CorrespondentMasterID);
          if (masterByMasterId != null)
          {
            CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, masterByMasterId, TradeHistoryAction.ContractAssigned, currentUser, priorValue));
            flag = true;
          }
        }
      }
      if (info.LastPublishedDateTime != DateTime.MinValue && info.LastPublishedDateTime != priorValue.LastPublishedDateTime)
      {
        flag = true;
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info.TradeID, TradeHistoryAction.TradePublished, info.LastPublishedDateTime, info.Status, currentUser));
      }
      if (!flag)
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, TradeHistoryAction.TradeUpdated, currentUser, priorValue));
      if (forEncompass)
        CorrespondentTrades.AddPairOffHistoryForEncompass(info, currentUser);
      else
        CorrespondentTrades.AddPairOffHistoryForAPI(info, currentUser, priorValue);
    }

    private static void AddPairOffHistoryForEncompass(
      CorrespondentTradeInfo info,
      UserInfo currentUser)
    {
      IList<CorrespondentTradePairOff> correspondentTradePairOffList = (IList<CorrespondentTradePairOff>) new List<CorrespondentTradePairOff>();
      foreach (CorrespondentTradePairOff correspondentTradePairOff in info.CorrespondentTradePairOffs)
      {
        if (correspondentTradePairOff.TradeHistoryAction != TradeHistoryAction.PairOffNotChanged)
          CorrespondentTrades.CreatePairOffsToAddInHisotry(correspondentTradePairOffList, correspondentTradePairOff);
      }
      if (info.CopyOfCorrespondentTradePairOffs != null)
      {
        foreach (CorrespondentTradePairOff correspondentTradePairOff in info.CopyOfCorrespondentTradePairOffs)
        {
          if (correspondentTradePairOff.TradeHistoryAction == TradeHistoryAction.PairOffDeleted)
            CorrespondentTrades.CreatePairOffsToAddInHisotry(correspondentTradePairOffList, correspondentTradePairOff);
        }
      }
      foreach (CorrespondentTradePairOff correspondentTradePairOff in (IEnumerable<CorrespondentTradePairOff>) correspondentTradePairOffList.OrderBy<CorrespondentTradePairOff, DateTime>((System.Func<CorrespondentTradePairOff, DateTime>) (x => x.ActionDateTime)).ToList<CorrespondentTradePairOff>())
      {
        CorrespondentTradePairOff item = correspondentTradePairOff;
        CorrespondentTradePairOff priorTrade = (CorrespondentTradePairOff) null;
        CorrespondentTradePairOff pairOff;
        if (item.TradeHistoryAction == TradeHistoryAction.PairOffDeleted)
        {
          pairOff = info.CopyOfCorrespondentTradePairOffs.FirstOrDefault<CorrespondentTradePairOff>((System.Func<CorrespondentTradePairOff, bool>) (x => x.Guid == item.Guid));
        }
        else
        {
          pairOff = info.CorrespondentTradePairOffs.FirstOrDefault<CorrespondentTradePairOff>((System.Func<CorrespondentTradePairOff, bool>) (x => x.Guid == item.Guid));
          priorTrade = info.CopyOfCorrespondentTradePairOffs.FirstOrDefault<CorrespondentTradePairOff>((System.Func<CorrespondentTradePairOff, bool>) (x => x.Guid == item.Guid));
        }
        CorrespondentTrades.AddPairOffHistoryItem(new CorrespondentTradePairOffHistoryItem(pairOff, info.TradeID, item.TradeHistoryAction, -1, currentUser, priorTrade));
      }
    }

    private static void AddPairOffHistoryForAPI(
      CorrespondentTradeInfo info,
      UserInfo currentUser,
      CorrespondentTradeInfo priorValue)
    {
      foreach (CorrespondentTradePairOff correspondentTradePairOff in priorValue.CorrespondentTradePairOffs.Intersect<CorrespondentTradePairOff>((IEnumerable<CorrespondentTradePairOff>) info.CorrespondentTradePairOffs, (IEqualityComparer<CorrespondentTradePairOff>) new CorrespondentTradePairOffUpdateComparer()))
      {
        CorrespondentTradePairOff pairOff = correspondentTradePairOff;
        CorrespondentTradePairOff pairOff1 = info.CorrespondentTradePairOffs.FirstOrDefault<CorrespondentTradePairOff>((System.Func<CorrespondentTradePairOff, bool>) (c => c.Id == pairOff.Id));
        pairOff1.ActionDateTime = DateTime.Now;
        TradeHistoryAction action = pairOff1.TradeAmount < 0M ? TradeHistoryAction.PairOffReversed : TradeHistoryAction.PairOffUpdated;
        CorrespondentTrades.AddPairOffHistoryItem(new CorrespondentTradePairOffHistoryItem(pairOff1, info.TradeID, action, -1, currentUser, pairOff));
      }
      foreach (CorrespondentTradePairOff pairOff in priorValue.CorrespondentTradePairOffs.Except<CorrespondentTradePairOff>((IEnumerable<CorrespondentTradePairOff>) info.CorrespondentTradePairOffs, (IEqualityComparer<CorrespondentTradePairOff>) new CorrespondentTradePairOffIdComparer()))
      {
        pairOff.ActionDateTime = DateTime.Now;
        CorrespondentTrades.AddPairOffHistoryItem(new CorrespondentTradePairOffHistoryItem(pairOff, info.TradeID, TradeHistoryAction.PairOffDeleted, -1, currentUser));
      }
      foreach (CorrespondentTradePairOff pairOff in info.CorrespondentTradePairOffs.Except<CorrespondentTradePairOff>((IEnumerable<CorrespondentTradePairOff>) priorValue.CorrespondentTradePairOffs, (IEqualityComparer<CorrespondentTradePairOff>) new CorrespondentTradePairOffIdComparer()))
      {
        pairOff.ActionDateTime = DateTime.Now;
        TradeHistoryAction action = pairOff.TradeAmount < 0M ? TradeHistoryAction.PairOffReversed : TradeHistoryAction.PairOffCreated;
        CorrespondentTrades.AddPairOffHistoryItem(new CorrespondentTradePairOffHistoryItem(pairOff, info.TradeID, action, -1, currentUser));
      }
    }

    private static void AddHistoryForPublishTrade(
      CorrespondentTradeInfo info,
      TradeStatus tradeStatus,
      DateTime lastPublishDateTime,
      UserInfo currentUser)
    {
      DateTime publishedDateTime = info.LastPublishedDateTime;
      TradeStatus status = info.Status;
      switch (status)
      {
        case TradeStatus.Shipped:
        case TradeStatus.Purchased:
        case TradeStatus.Voided:
          tradeStatus = status;
          break;
      }
      CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info.TradeID, TradeHistoryAction.TradePublished, lastPublishDateTime, tradeStatus, currentUser));
      if (status != TradeStatus.Unpublished || !(publishedDateTime == DateTime.MinValue))
        return;
      CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(info, TradeStatus.Committed, currentUser));
    }

    private static void CreatePairOffsToAddInHisotry(
      IList<CorrespondentTradePairOff> pairoffs,
      CorrespondentTradePairOff item)
    {
      pairoffs.Add(new CorrespondentTradePairOff()
      {
        Guid = item.Guid,
        ActionDateTime = item.ActionDateTime,
        TradeHistoryAction = item.TradeHistoryAction
      });
    }

    public static void RecalculateProfits(CorrespondentTradeInfo trade, bool isExternalOrganization)
    {
      PipelineInfo[] assignedLoans = CorrespondentTrades.GetAssignedLoans(trade, CorrespondentTrades.GeneratePricingFieldList(trade), isExternalOrganization);
      CorrespondentTrades.RecalculateProfits(trade, assignedLoans);
    }

    public static string[] GeneratePricingFieldList(CorrespondentTradeInfo trade)
    {
      return CorrespondentTrades.GeneratePricingFieldList((IEnumerable<CorrespondentTradeInfo>) new CorrespondentTradeInfo[1]
      {
        trade
      });
    }

    public static string[] GeneratePricingFieldList(IEnumerable<CorrespondentTradeInfo> trades)
    {
      List<string> stringList = new List<string>();
      foreach (CorrespondentTradeInfo trade in trades)
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
      CorrespondentTradeInfo trade,
      string[] fields,
      bool isExternalOrganization)
    {
      return Pipeline.Generate("select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid  where TradeAssignment.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 2, fields, PipelineData.Trade, isExternalOrganization, TradeType.CorrespondentTrade);
    }

    public static List<string> GetAssignedLoans(int TradeID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid ");
      dbQueryBuilder.AppendFormat("where TradeAssignment.TradeID = {0} and TradeAssignment.Status >= {1} ", (object) TradeID, (object) 2);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      List<string> assignedLoans = new List<string>();
      for (int index = 0; index < dataSet.Tables[0].Rows.Count; ++index)
        assignedLoans.Add(dataSet.Tables[0].Rows[index][0].ToString());
      return assignedLoans;
    }

    public static void RecalculateProfits(CorrespondentTradeInfo trade, PipelineInfo[] pinfos)
    {
      ChunkedList<PipelineInfo> chunkedList = new ChunkedList<PipelineInfo>(pinfos, 50);
      PipelineInfo[] pipelineInfoArray = (PipelineInfo[]) null;
      do
        ;
      while ((pipelineInfoArray = chunkedList.Next()) != null);
    }

    private static DbValueList createTradeObjectsValueList(
      CorrespondentTradeInfo info,
      UserInfo currentUser = null)
    {
      DbValueList objectsValueList = new DbValueList();
      objectsValueList.Add("FilterQueryXml", info.Filter == null ? (object) (string) null : (object) info.Filter.ToXml());
      objectsValueList.Add("SRPTableXml", info.SRPTable == null ? (object) (string) null : (object) info.SRPTable.ToXml());
      objectsValueList.Add("InvestorXml", info.Investor == null ? (object) (string) null : (object) info.Investor.ToXml());
      objectsValueList.Add("PairOffsXml", info.CorrespondentTradePairOffs == null ? (object) (string) null : (object) info.CorrespondentTradePairOffs.ToXml());
      objectsValueList.Add("PricingXml", info.Pricing == null ? (object) (string) null : (object) info.Pricing.ToXml());
      objectsValueList.Add("AdjustmentsXml", info.PriceAdjustments == null ? (object) (string) null : (object) info.PriceAdjustments.ToXml());
      objectsValueList.Add("DealerXml", info.Dealer == null ? (object) (string) null : (object) info.Dealer.ToXml());
      objectsValueList.Add("AssigneeXml", info.Assignee == null ? (object) (string) null : (object) info.Assignee.ToXml());
      objectsValueList.Add("EPPSLoanProgramQueryFilterXml", info.EPPSLoanProgramsFilter == null ? (object) (string) null : (object) info.EPPSLoanProgramsFilter.ToXml());
      string tradeNotes = CorrespondentTrades.parseTradeNotes(info.TradeID, info.Notes, currentUser);
      objectsValueList.Add("Notes", (object) tradeNotes);
      return objectsValueList;
    }

    private static string parseTradeNotes(int tradeId, string notes, UserInfo currentUser)
    {
      if (TradeNoteUtils.DeserializeTradeNotes(notes) == null && notes.Trim().Length > 0)
      {
        string userId = string.Empty;
        string userName = string.Empty;
        if (currentUser != (UserInfo) null)
        {
          userId = currentUser.Userid;
          userName = currentUser.userName;
        }
        string notes1 = TradeNoteUtils.SerializeTradeNotes(Convert.ToString(notes), userId, userName, (SessionObjects) null);
        CorrespondentTrades.UpdateCTNotesToJson(notes1, tradeId);
        if (notes1.Length > 0)
          notes = notes1;
      }
      return notes;
    }

    private static Dictionary<CorrespondentTradeLoanStatus, int> dataRowsToPendingLoanCounts(
      IEnumerable rows)
    {
      Dictionary<CorrespondentTradeLoanStatus, int> pendingLoanCounts = new Dictionary<CorrespondentTradeLoanStatus, int>();
      foreach (DataRow row in rows)
        pendingLoanCounts[(CorrespondentTradeLoanStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["PendingStatus"])] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["LoanCount"]);
      return pendingLoanCounts;
    }

    private static List<LoanSummaryExtension> dataRowsToLoans(IEnumerable rows)
    {
      List<LoanSummaryExtension> loans = new List<LoanSummaryExtension>();
      foreach (DataRow row in rows)
        loans.Add(new LoanSummaryExtension()
        {
          CorrespondentTradeId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["CorrespondentTradeId"]),
          Guid = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]),
          LoanNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["LoanNumber"]),
          LoanAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["LoanAmount"]),
          CorrespondentLoanStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["CorrespondentLoanStatus"]),
          PurchaseDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["PurchaseDate"]),
          ReceivedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["ReceivedDate"]),
          RejectedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["RejectedDate"]),
          DeliveryType = (CorrespondentMasterDeliveryType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["DeliveryType"]),
          TpoCompanyId = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOCompanyID"]),
          SubmittedForReviewDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["SubmittedForReviewDate"]),
          PurchaseSuspenseDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["PurchaseSuspenseDate"]),
          PurchaseApprovalDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["PurchaseApprovalDate"]),
          ClearedForPurchaseDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["ClearedForPurchaseDate"]),
          CancelledDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["CancelledDate"]),
          VoidedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["VoidedDate"]),
          WithdrawnDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["WithdrawnDate"]),
          PurchasedPrinciple = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["PurchasedPrinciple"]),
          WithdrawalRequestedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["WithdrawalRequestedDate"]),
          LenderCaseIdentifier = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["LenderCaseIdentifier"])
        });
      return loans;
    }

    private static CorrespondentTradeHistoryItem dataRowToHistoryItem(DataRow r)
    {
      return new CorrespondentTradeHistoryItem(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["HistoryID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), (TradeHistoryAction) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Action"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Timestamp"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UserID"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["DataXml"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["PriorDataXml"]));
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      int[] tradeIds,
      string[] fields,
      bool isExternalOrganization,
      int? maxCount = null)
    {
      return CorrespondentTrades.GetEligibleLoans(user, CorrespondentTrades.GetTrades(tradeIds), fields, (SortField[]) null, isExternalOrganization, maxCount: maxCount);
    }

    public static List<string> GetEligibleLoanGuids(UserInfo user, int tradeId, int? maxCount = null)
    {
      List<string> eligibleLoanGuids = new List<string>();
      UserInfo user1 = user;
      int[] tradeIds = new int[1]{ tradeId };
      string[] fields = new string[1]{ "GUID" };
      int? maxCount1 = maxCount;
      foreach (PipelineInfo eligibleLoan in CorrespondentTrades.GetEligibleLoans(user1, tradeIds, fields, false, maxCount1))
      {
        if (!string.IsNullOrWhiteSpace(eligibleLoan.GUID))
          eligibleLoanGuids.Add(eligibleLoan.GUID);
      }
      return eligibleLoanGuids;
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      CorrespondentTradeInfo[] trades,
      string[] fields,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All,
      int? maxCount = null)
    {
      QueryCriterion queryCriterion1 = (QueryCriterion) null;
      foreach (CorrespondentTradeInfo trade in trades)
      {
        if (trade != null && trade.Filter != null)
        {
          QueryCriterion queryCriterion2 = ((TradeFilterEvaluator) trade.Filter.CreateEvaluator(typeof (CorrespondentTradeFilterEvaluator))).ToQueryCriterion((TradeInfoObj) trade, filterOption);
          if (queryCriterion2 != null)
            queryCriterion1 = queryCriterion1 == null ? queryCriterion2 : queryCriterion2.Or(queryCriterion1);
        }
      }
      PipelineInfo[] eligibleLoans = Pipeline.Generate(user, (string[]) null, LoanInfo.Right.Access, fields, PipelineData.Trade, queryCriterion1, sortOrder, (ICriterionTranslator) null, isExternalOrganization, TradeType.CorrespondentTrade, maxCount);
      foreach (PipelineInfo pinfo in eligibleLoans)
        CorrespondentTrades.PopulateTradeProfitData(pinfo, trades);
      return eligibleLoans;
    }

    public static CorrespondentTradeInfo[] GetTrades(int[] tradeIds)
    {
      return CorrespondentTrades.getTradeInfosFromDatabase(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds), false);
    }

    public static void PopulateTradeProfitData(PipelineInfo pinfo, CorrespondentTradeInfo[] trades)
    {
      PipelineInfo.TradeInfo[] tradeInfoArray = new PipelineInfo.TradeInfo[trades.Length];
      for (int index = 0; index < trades.Length; ++index)
      {
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("Profit")] = (object) new PipelineInfo.TradeInfo()
        {
          TradeID = trades[index].TradeID
        }.Profit;
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("SRP")] = (object) trades[index].SRPTable.GetAdjustmentForLoan(pinfo);
      }
      pinfo.EligibleTrades = tradeInfoArray;
    }

    public static int[] QueryTradeIds(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      StringBuilder stringBuilder = new StringBuilder(CorrespondentTrades.getQueryTradeIdsSql(user, criteria, sortOrder, isExternalOrganization));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(stringBuilder.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["TradeID"]);
      return numArray;
    }

    public static int[] QueryTradeIds(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization,
      out int totalCount,
      int start = 0,
      int limit = 1000)
    {
      StringBuilder stringBuilder = new StringBuilder(CorrespondentTrades.getQueryTradeIdsSql(user, criteria, sortOrder, isExternalOrganization, true, start, limit));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(stringBuilder.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["TradeID"]);
      totalCount = 0;
      if (dataRowCollection.Count > 0)
        totalCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[0]["TotalCount"]);
      return numArray;
    }

    public static string getQueryTradeIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization,
      bool isPagination = false,
      int start = 0,
      int limit = 1000)
    {
      try
      {
        string fieldList = "CorrespondentTradeDetails.TradeID";
        CorrespondentTradeQuery correspondentTradeQuery = new CorrespondentTradeQuery(user);
        return CorrespondentTrades.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) correspondentTradeQuery, isExternalOrganization, isPagination, start, limit).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(CorrespondentTrades.className, ex);
        return (string) null;
      }
    }

    private static DbQueryBuilder generateQuerySql(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortFields,
      QueryEngine queryEngine,
      bool isExternalOrganization,
      bool isPagination = false,
      int start = 0,
      int limit = 1000)
    {
      string empty = string.Empty;
      List<SortField> source = new List<SortField>();
      if ((sortFields != null ? (((IEnumerable<SortField>) sortFields).Any<SortField>() ? 1 : 0) : 0) != 0)
      {
        source.AddRange((IEnumerable<SortField>) sortFields);
        if (source.Where<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.Equals("CorrespondentTradeDetails.DeliveryType"))).Count<SortField>() > 0)
        {
          SortField sortField1 = source.Where<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.Equals("CorrespondentTradeDetails.DeliveryType"))).First<SortField>();
          source.Remove(sortField1);
          source.Add(new SortField("CTDeliveryTypes.Description", sortField1.SortOrder));
        }
        if (source.Where<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.Equals("CorrespondentTradeDetails.Status"))).Count<SortField>() > 0)
        {
          SortField sortField2 = source.Where<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.Equals("CorrespondentTradeDetails.Status"))).First<SortField>();
          source.Remove(sortField2);
          source.Add(new SortField("CTStatus.Description", sortField2.SortOrder));
        }
      }
      string text;
      if ((sortFields != null ? (!((IEnumerable<SortField>) sortFields).Any<SortField>() ? 1 : 0) : 1) != 0)
      {
        text = "order by CorrespondentTradeDetails.TradeID ASC";
      }
      else
      {
        if (!source.Any<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.Equals("CorrespondentTradeDetails.TradeID"))))
          source.Add(new SortField("CorrespondentTradeDetails.TradeID", FieldSortOrder.Ascending));
        text = queryEngine.GetOrderByClause(source.ToArray());
      }
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(fieldList.Replace(" ", "").Split(','));
      QueryCriterion filter = QueryCriterion.Join(criteria, BinaryOperator.And);
      string tableSelectionClause = queryEngine.GetTableSelectionClause(fields, filter, sortFields, true, true, isExternalOrganization);
      DbQueryBuilder querySql = new DbQueryBuilder();
      if (isPagination)
      {
        querySql.AppendLine("SELECT * FROM (");
        querySql.AppendLine("SELECT ROW_NUMBER() OVER ( " + text + " ) AS RowNum, " + fieldList + ", COUNT(*) OVER() AS TotalCount ");
        querySql.AppendLine(tableSelectionClause);
        querySql.AppendLine(") AS t ");
        querySql.AppendLine("ORDER BY t.RowNum ");
        querySql.AppendLine(string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", (object) start, (object) limit));
      }
      else
      {
        querySql.AppendLine("select " + fieldList + " ");
        querySql.AppendLine(tableSelectionClause);
        querySql.AppendLine(text);
      }
      return querySql;
    }

    public static Dictionary<int, CorrespondentTradeViewModel> GetCorrespondentTradeViews(
      int[] tradeIds)
    {
      List<CorrespondentTradeViewModel> source = new List<CorrespondentTradeViewModel>();
      List<CorrespondentTradeViewModel> correspondentTradeViewModelList = new List<CorrespondentTradeViewModel>();
      Dictionary<int, CorrespondentTradeViewModel> correspondentTradeViews = new Dictionary<int, CorrespondentTradeViewModel>();
      if (tradeIds == null || tradeIds.Length == 0)
        return correspondentTradeViews;
      string str1 = "";
      List<string> stringList = new List<string>();
      if (((IEnumerable<int>) tradeIds).Count<int>() < 1000)
      {
        foreach (int tradeId in tradeIds)
          str1 = str1 + (object) tradeId + ",";
        source.AddRange((IEnumerable<CorrespondentTradeViewModel>) CorrespondentTrades.getViewModelsFromDatabase(str1.Substring(0, str1.Length - 1)));
      }
      else
      {
        foreach (IGrouping<int, \u003C\u003Ef__AnonymousType2<int, int>> grouping in ((IEnumerable<int>) tradeIds).Select((s, i) => new
        {
          Value = s,
          Index = i
        }).GroupBy(o => o.Index / 1000))
        {
          string str2 = "";
          foreach (var data in grouping)
            str2 = str2 + (object) data.Value + ",";
          source.AddRange((IEnumerable<CorrespondentTradeViewModel>) CorrespondentTrades.getViewModelsFromDatabase(str2.Substring(0, str2.Length - 1)));
        }
      }
      foreach (int tradeId1 in tradeIds)
      {
        int tradeId = tradeId1;
        if (source.Any<CorrespondentTradeViewModel>((System.Func<CorrespondentTradeViewModel, bool>) (t => t.TradeID == tradeId)))
          correspondentTradeViewModelList.AddRange(source.Where<CorrespondentTradeViewModel>((System.Func<CorrespondentTradeViewModel, bool>) (t => t.TradeID == tradeId)));
      }
      foreach (CorrespondentTradeViewModel correspondentTradeViewModel in correspondentTradeViewModelList.ToArray())
      {
        if (correspondentTradeViewModel != null)
          correspondentTradeViews[correspondentTradeViewModel.TradeID] = correspondentTradeViewModel;
      }
      for (int key = 0; key < tradeIds.Length; ++key)
      {
        if (!correspondentTradeViews.ContainsKey(tradeIds[key]))
          correspondentTradeViews[key] = (CorrespondentTradeViewModel) null;
      }
      return correspondentTradeViews;
    }

    public static CorrespondentTradeViewModel GetTradeView(int tradeId)
    {
      CorrespondentTradeViewModel[] modelsFromDatabase = CorrespondentTrades.getViewModelsFromDatabase(string.Concat((object) tradeId));
      return modelsFromDatabase.Length == 0 ? (CorrespondentTradeViewModel) null : modelsFromDatabase[0];
    }

    public static void DeleteTrade(int tradeId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table1 = DbAccessManager.GetTable(nameof (CorrespondentTrades));
        DbTableInfo table2 = DbAccessManager.GetTable("Trades");
        DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
        DbValue key = new DbValue("TradeID", (object) tradeId);
        dbQueryBuilder.AppendLine("insert into TradeNameAudit(TradeName, TradeType) (select Name, " + (object) 4 + " from CorrespondentTradeDetails where TradeId = " + (object) tradeId + ")");
        dbQueryBuilder.AppendLine("delete TradeAssignmentByTrade where AssigneeTradeID = " + (object) tradeId);
        dbQueryBuilder.DeleteFrom(table3, key);
        dbQueryBuilder.DeleteFrom(table1, key);
        dbQueryBuilder.DeleteFrom(table2, key);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(CorrespondentTrades.UPDATETRADEERROR);
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
        foreach (CorrespondentTradeInfo trade in CorrespondentTrades.GetTrades(tradeIds))
        {
          if (status == TradeStatus.Delivered || status == TradeStatus.Committed || status == TradeStatus.Settled || action == TradeHistoryAction.TradeStatusChanged)
            CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade, action, status, currentUser));
          else
            CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade, action, currentUser));
        }
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(CorrespondentTrades.UPDATETRADEERROR);
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
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(CorrespondentTrades.GetTrade(tradeId), status, currentUser));
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(CorrespondentTrades.UPDATETRADEERROR);
        throw;
      }
    }

    public static CorrespondentTradeViewModel GetTradeViewForLoan(string guid)
    {
      CorrespondentTradeViewModel[] modelsFromDatabase = CorrespondentTrades.getViewModelsFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, CorrespondentTrades.BuySideTradeTypes));
      return modelsFromDatabase.Length == 0 ? (CorrespondentTradeViewModel) null : modelsFromDatabase[0];
    }

    public static CorrespondentTradeInfo GetTradeForLoan(string guid)
    {
      CorrespondentTradeInfo[] infosFromDatabase = CorrespondentTrades.getTradeInfosFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, CorrespondentTrades.BuySideTradeTypes), false);
      return infosFromDatabase.Length == 0 ? (CorrespondentTradeInfo) null : infosFromDatabase[0];
    }

    public static CorrespondentTradeInfo GetTradeForRejectedLoan(string guid)
    {
      CorrespondentTradeInfo[] infosFromDatabase = CorrespondentTrades.getTradeInfosFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and isnull(Rejected, 0) = 1 and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), CorrespondentTrades.BuySideTradeTypes), false);
      return infosFromDatabase.Length == 0 ? (CorrespondentTradeInfo) null : infosFromDatabase[0];
    }

    public static CorrespondentTradeViewModel[] GetActiveTradeView()
    {
      return CorrespondentTrades.getViewModelsFromDatabase("select TradeID from Trades where Status <> " + (object) 5);
    }

    public static CorrespondentTradeViewModel[] GetTradeViewsByMasterId(int masterId)
    {
      return CorrespondentTrades.getViewModelsFromDatabase("select TradeID from CorrespondentTrades where CorrespondentMasterID = " + (object) masterId);
    }

    public static CorrespondentTradeInfo[] GetTradeInfosByMasterId(int masterId)
    {
      return CorrespondentTrades.getTradeInfosFromDatabase("select TradeID from CorrespondentTrades where CorrespondentMasterID = " + (object) masterId, false, false);
    }

    public static CorrespondentTradeInfo[] GetTradeInfosByExternalOrgId(int externalOrgId)
    {
      return CorrespondentTrades.getTradeInfosFromDatabase("select TradeID from CorrespondentTrades where ExternalOriginatorManagementID = " + (object) externalOrgId, false, false);
    }

    public static void DeleteLoanFromTrades(
      string loanGuid,
      UserInfo currentUser,
      bool isExternalOrganization)
    {
      CorrespondentTradeInfo tradeForLoan = CorrespondentTrades.GetTradeForLoan(loanGuid);
      if (tradeForLoan != null)
      {
        PipelineInfo pipelineInfo = Pipeline.GetPipelineInfo((UserInfo) null, loanGuid, CorrespondentTradeHistoryItem.RequiredPipelineInfoFields, PipelineData.Fields, isExternalOrganization);
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(tradeForLoan, pipelineInfo, CorrespondentTradeLoanStatus.Unassigned, currentUser));
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete ta from TradeAssignment ta inner join Trades t on t.TradeID = ta.TradeID where ta.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid) + " and t.TradeType = " + (object) 4);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      CorrespondentTradeLoanStatus[] statuses,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      CorrespondentTrades.SetPendingTradeStatus(tradeId, loanIds, statuses, (string[]) null, currentUser, isExternalOrganization, removePendingLoan);
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      CorrespondentTradeLoanStatus[] statuses,
      string[] comments,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan,
      Decimal[] totalPrice = null,
      bool forceUpdateAllLoans = false)
    {
      bool flag = false;
      CorrespondentTradeViewModel tradeView = CorrespondentTrades.GetTradeView(tradeId);
      PipelineInfo[] pipelineInfoArray = Pipeline.Generate((UserInfo) null, loanIds, CorrespondentTradeHistoryItem.RequiredPipelineInfoFields, PipelineData.Fields, isExternalOrganization, tradeType: TradeType.CorrespondentTrade);
      for (int index = 0; index < loanIds.Length; ++index)
      {
        if (comments == null)
          flag |= CorrespondentTrades.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], currentUser, removePendingLoan, totalPrice[index], forceUpdateAllLoans);
        else
          flag |= CorrespondentTrades.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], comments[index], currentUser, removePendingLoan, totalPrice[index], forceUpdateAllLoans);
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
      CorrespondentTrades.RecalculateProfits(tradeId, false);
    }

    private static bool setPendingTradeStatus(
      CorrespondentTradeViewModel trade,
      PipelineInfo loanInfo,
      CorrespondentTradeLoanStatus status,
      UserInfo currentUser,
      bool removePendingLoan,
      Decimal totalPrice = 0M,
      bool forceUpdateAllLoans = false)
    {
      return CorrespondentTrades.setPendingTradeStatus(trade, loanInfo, status, "", currentUser, removePendingLoan, totalPrice, forceUpdateAllLoans);
    }

    private static bool setPendingTradeStatus(
      CorrespondentTradeViewModel trade,
      PipelineInfo loanInfo,
      CorrespondentTradeLoanStatus status,
      string comment,
      UserInfo currentUser,
      bool removePendingLoan,
      Decimal totalPrice = 0M,
      bool forceUpdateAllLoans = false)
    {
      if (status == CorrespondentTradeLoanStatus.None && !forceUpdateAllLoans)
        return false;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) trade.TradeID);
      dbValueList.Add("LoanGuid", (object) loanInfo.GUID);
      DbValueList values = new DbValueList();
      values.Add("PendingStatus", (object) (int) status);
      values.Add("PendingStatusDate", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
      if (!string.IsNullOrEmpty(comment) && comment == "voided")
        values.Add("Rejected", (object) true, (IDbEncoder) DbEncoding.Flag);
      if ((CorrespondentMasterDeliveryType) Enum.Parse(typeof (CorrespondentMasterDeliveryType), trade.DeliveryType) == CorrespondentMasterDeliveryType.Bulk || (CorrespondentMasterDeliveryType) Enum.Parse(typeof (CorrespondentMasterDeliveryType), trade.DeliveryType) == CorrespondentMasterDeliveryType.BulkAOT)
        values.Add("TotalPrice", (object) totalPrice);
      if (!removePendingLoan)
      {
        string format = "\r\n                    if exists (select 1\r\n                        from TradeAssignment ta\r\n                        inner join trades t on ta.tradeid = t.tradeid\r\n                        where ta.TradeID <> {0} And LoanGuid = {1}\r\n                        and TradeType in (4)\r\n                        and ta.AssignedStatus >= {2}\r\n                    )\r\n                    return";
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
      if (status != CorrespondentTradeLoanStatus.Unassigned)
      {
        dbQueryBuilder.AppendLine(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, " and TradeAssignment.Status > " + (object) 1 + " and TradeAssignment.TradeID <> " + (object) trade.TradeID + " And LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID), CorrespondentTrades.BuySideTradeTypes));
        dbQueryBuilder.AppendLine("update TradeAssignment set PendingStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where TradeAssignment.TradeID <> " + (object) trade.TradeID);
        dbQueryBuilder.AppendLine(" and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID));
        dbQueryBuilder.AppendLine(" and Trades.TradeType = " + (object) 4);
      }
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        throw new Exception("The loan has been assigned to another correspondent trade.");
      bool flag = string.Concat(dataSet.Tables[0].Rows[0][0]) == "1";
      if (status != CorrespondentTradeLoanStatus.Unassigned)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
        {
          CorrespondentTradeInfo trade1 = CorrespondentTrades.GetTrade(EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeID"]));
          if (trade1 != null)
            CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade1, loanInfo, CorrespondentTradeLoanStatus.Unassigned, currentUser));
        }
      }
      if (trade != null)
      {
        CorrespondentTradeInfo trade2 = CorrespondentTrades.GetTrade(trade.TradeID);
        if (flag && status != CorrespondentTradeLoanStatus.Assigned)
          CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade2, loanInfo, CorrespondentTradeLoanStatus.Assigned, currentUser));
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade2, loanInfo, status, comment, currentUser));
      }
      return flag;
    }

    public static void RecalculateProfits(int tradeId, bool isExternalOrganization)
    {
      CorrespondentTrades.RecalculateProfits(CorrespondentTrades.GetTrade(tradeId) ?? throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Trade, (object) tradeId), isExternalOrganization);
    }

    public static void CommitPendingTradeStatus(
      int tradeId,
      string loanId,
      CorrespondentTradeLoanStatus status)
    {
      CorrespondentTrades.CommitPendingTradeStatus(tradeId, loanId, status, false);
    }

    public static void CommitPendingTradeStatus(
      int tradeId,
      string loanId,
      CorrespondentTradeLoanStatus status,
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
      if (status >= CorrespondentTradeLoanStatus.Assigned)
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

    public static CorrespondentTradeHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      return CorrespondentTrades.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
    }

    public static void RecalculateProfitFromLoan(string loanGuid, bool isExternalOrganization)
    {
      CorrespondentTradeInfo[] correspondentTradesForLoan = CorrespondentTrades.GetAllPendingCorrespondentTradesForLoan(loanGuid);
      string[] pricingFieldList = CorrespondentTrades.GeneratePricingFieldList((IEnumerable<CorrespondentTradeInfo>) correspondentTradesForLoan);
      PipelineInfo pipelineInfo = Pipeline.GetPipelineInfo((UserInfo) null, loanGuid, pricingFieldList, PipelineData.Fields, isExternalOrganization);
      if (pipelineInfo == null)
        return;
      CorrespondentTrades.RecalculateProfits(correspondentTradesForLoan, pipelineInfo);
    }

    public static CorrespondentTradeInfo[] GetAllPendingCorrespondentTradesForLoan(string guid)
    {
      return CorrespondentTrades.getTradeInfosFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), CorrespondentTrades.BuySideTradeTypes), false);
    }

    public static string[] GeneratePricingFieldList(int[] tradeIds)
    {
      return CorrespondentTrades.GeneratePricingFieldList((IEnumerable<CorrespondentTradeInfo>) CorrespondentTrades.GetTrades(tradeIds));
    }

    public static void RecalculateProfits(CorrespondentTradeInfo[] trades, PipelineInfo pinfo)
    {
      ChunkedList<CorrespondentTradeInfo> chunkedList = new ChunkedList<CorrespondentTradeInfo>(trades, 50);
      CorrespondentTradeInfo[] correspondentTradeInfoArray;
      while ((correspondentTradeInfoArray = chunkedList.Next()) != null)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        int num = 0;
        while (num < correspondentTradeInfoArray.Length)
          ++num;
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static void ApplyTradeStatusFromLoan(PipelineInfo pinfo)
    {
      string tradeGuid = string.Concat(pinfo.GetField("CorrespondentTradeGuid"));
      DateTime date = Utils.ParseDate(pinfo.GetField("InvestorStatusDate"), DateTime.Now);
      if (tradeGuid == "")
        CorrespondentTrades.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, CorrespondentTradeLoanStatus.Unassigned, date);
      else
        CorrespondentTrades.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, CorrespondentTradeLoanStatus.Assigned, date);
    }

    public static void SetTradeAssignmentStatus(
      string loanId,
      string tradeGuid,
      CorrespondentTradeLoanStatus loanStatus,
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
      if (loanStatus == CorrespondentTradeLoanStatus.Unassigned)
      {
        dbQueryBuilder.AppendLine("update TradeAssignment set AssignedStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId) + " and TradeType in (" + (object) 4 + ")");
        dbQueryBuilder.AppendLine("update TradeAssignment set PendingStatus = 0 ");
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where PendingStatus = AssignedStatus and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId) + " and TradeType in (" + (object) 4 + ")");
        dbQueryBuilder.AppendLine("delete from TradeAssignment ");
        dbQueryBuilder.AppendLine(" where AssignedStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" and PendingStatus = 0 and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        dbQueryBuilder.Declare("@tradeId", "int");
        dbQueryBuilder.AppendLine("select @tradeId = TradeID from Trades where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tradeGuid));
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
        dbQueryBuilder.AppendLine(" and Trades.TradeType in(" + (object) 4 + ")");
        dbQueryBuilder.End();
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static bool CheckTradeByName(string name)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select COUNT(*) from TradeNameAudit where TradeName = '" + name + "' and TradeType = 4");
      return (int) dbQueryBuilder.ExecuteScalar() > 0;
    }

    public static bool CheckExistingTradeByName(string name)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select COUNT(*) from Trades where Name = '" + name + "' and TradeType = 4");
      return (int) dbQueryBuilder.ExecuteScalar() > 0;
    }

    public static bool CheckOtherTradeWithSameNameExists(int tradeId, string name)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select COUNT(*) from Trades where Name = '" + name + "' and TradeType = 4 and tradeId != " + (object) tradeId);
      return (int) dbQueryBuilder.ExecuteScalar() > 0;
    }

    public static string GetNextAutoCreateTradeName(string name, string loanGUID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Name from CorrespondentTradeDetails where AutoCreateLoanGUID = '" + loanGUID + "'");
      dbQueryBuilder.AppendLine("select Name from CorrespondentTradeDetails where Name like '" + name + "(%[0-9])' or Name = '" + name + "'");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      if (table1.Rows.Count > 0)
        return "";
      if (table2.Rows.Count <= 0)
        return name;
      int[] numArray = new int[table2.Rows.Count];
      for (int index = 0; index < table2.Rows.Count; ++index)
      {
        string[] strArray = table2.Rows[index][0].ToString().Split('(', ')');
        int result = 0;
        if (strArray.Length >= 2)
          int.TryParse(strArray[1], out result);
        numArray[index] = result;
      }
      Array.Reverse((Array) numArray);
      return name + "(" + (object) (numArray[0] + 1) + ")";
    }

    public static Dictionary<int, string> GetEligibleCorrespondentTradeByLoanInfo(
      string externalId,
      CorrespondentMasterDeliveryType deliveryType,
      double loanAmount)
    {
      Dictionary<int, string> correspondentTradeByLoanInfo = new Dictionary<int, string>();
      string text = string.Format("SELECT CorrespondentTradeDetails.TradeID, CorrespondentTradeDetails.Name \r\n                              FROM CorrespondentTradeDetails\r\n                              LEFT JOIN FN_GetCorrespondentTradeSummaryInline({3}) TradeCorrespondentTradeSummary\r\n                                  ON CorrespondentTradeDetails.TradeID = TradeCorrespondentTradeSummary.TradeID \r\n                              WHERE CorrespondentTradeDetails.Status < 5\r\n                              AND Locked = 0\r\n                              AND ExternalId = '{0}'\r\n                              AND DeliveryType = {1}\r\n                              AND TradeCorrespondentTradeSummary.OpenAmount + (CorrespondentTradeDetails.MaxAmount - CorrespondentTradeDetails.TradeAmount) >= {2}", (object) externalId, (object) (int) deliveryType, (object) loanAmount, (object) 1);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          correspondentTradeByLoanInfo.Add((int) dataRow["TradeID"], dataRow["Name"].ToString());
      }
      return correspondentTradeByLoanInfo;
    }

    public static Dictionary<int, string> GetEligibleCorrespondentTradeByLoanNumber(
      CorrespondentMasterDeliveryType deliveryType,
      string loanNumber)
    {
      Dictionary<int, string> tradeByLoanNumber = new Dictionary<int, string>();
      string text = string.Format("\r\n                              DECLARE @externalId varchar(10)\r\n                              DECLARE @loanAmount money\r\n                              SELECT @externalId = TPOCompanyId, @loanAmount = TotalLoanAmount\r\n                              FROM LoanSummary\r\n                              WHERE LoanNumber = '{1}'\r\n                              SELECT CorrespondentTradeDetails.TradeID, CorrespondentTradeDetails.Name \r\n                              FROM CorrespondentTradeDetails\r\n                              LEFT JOIN FN_GetCorrespondentTradeSummaryInline({2}) TradeCorrespondentTradeSummary\r\n                                  ON CorrespondentTradeDetails.TradeID = TradeCorrespondentTradeSummary.TradeID \r\n                              WHERE CorrespondentTradeDetails.Status < 5\r\n                              AND Locked = 0\r\n                              AND ExternalId = @externalId\r\n                              AND DeliveryType = {0}\r\n                              AND TradeCorrespondentTradeSummary.OpenAmount + (CorrespondentTradeDetails.MaxAmount - CorrespondentTradeDetails.TradeAmount) >= @loanAmount", (object) (int) deliveryType, (object) loanNumber, (object) 1);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          tradeByLoanNumber.Add((int) dataRow["TradeID"], dataRow["Name"].ToString());
      }
      return tradeByLoanNumber;
    }

    public static Dictionary<int, string> GetCorrespondentMastersByCorrespndentTradeId(
      int correspondentTradeId)
    {
      CorrespondentTradeInfo[] info = CorrespondentTrades.getTradeInfosFromDatabase(correspondentTradeId.ToString(), false);
      if (info.Length == 0)
        return new Dictionary<int, string>();
      return string.IsNullOrEmpty(info[0].CorrespondentMasterCommitmentNumber) ? ((IEnumerable<CorrespondentMasterInfo>) CorrespondentTrades.GetActiveMasters(info[0])).Where<CorrespondentMasterInfo>((System.Func<CorrespondentMasterInfo, bool>) (cm => cm.AvailableAmount >= info[0].TradeAmount)).Select(cm => new
      {
        ID = cm.ID,
        Name = cm.Name
      }).ToDictionary(x => x.ID, x => x.Name) : new Dictionary<int, string>();
    }

    public static Dictionary<int, string> GetEligibleCorrespondentTradeByLoanNumber(
      string deliveryType,
      string loanNumber,
      string correspondentMasterNumber)
    {
      Dictionary<int, string> tradeByLoanNumber = new Dictionary<int, string>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DECLARE @externalId varchar(10) ");
      dbQueryBuilder.AppendLine("DECLARE @loanAmount money ");
      dbQueryBuilder.AppendLine("SELECT @externalId = TPOCompanyId, @loanAmount = TotalLoanAmount ");
      dbQueryBuilder.AppendLine("FROM LoanSummary ");
      dbQueryBuilder.AppendLine("WHERE LoanNumber = '" + loanNumber + "' ");
      dbQueryBuilder.AppendLine("select ctd.* ");
      dbQueryBuilder.AppendLine("from CorrespondentTradeDetails ctd ");
      dbQueryBuilder.AppendLine("left outer join ");
      dbQueryBuilder.AppendLine("FN_GetCorrespondentTradeSummaryInline(" + (object) 1 + ") TradeCorrespondentTradeSummary on ctd.TradeID = TradeCorrespondentTradeSummary.TradeID ");
      dbQueryBuilder.AppendLine("where ExternalID = @externalId AND TradeCorrespondentTradeSummary.OpenAmount + (ctd.MaxAmount - ctd.TradeAmount) >= @loanAmount and ctd.Status < 5 and Locked = 0 and ctd.ContractNumber = '" + correspondentMasterNumber + "' and ctd.DeliveryType = " + (object) (int) TradeUtils.GetDeliveryTypeEnum(deliveryType));
      DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
      for (int index = 0; index < table.Rows.Count; ++index)
        tradeByLoanNumber.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(table.Rows[index]["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(table.Rows[index]["Name"]));
      return tradeByLoanNumber;
    }

    public static bool IsTradeAssignmentUpdateCompleted(
      int tradeId,
      string loanGuid,
      CorrespondentTradeLoanStatus status)
    {
      string text = string.Empty;
      if (status == CorrespondentTradeLoanStatus.Unassigned)
        text = string.Format("SELECT * \r\n                              FROM TradeAssignment\r\n                              WHERE TradeID = {0}\r\n                              AND LoanGuid = '{1}'", (object) tradeId, (object) loanGuid);
      else if (status >= CorrespondentTradeLoanStatus.Assigned)
        text = string.Format("SELECT * \r\n                              FROM TradeAssignment\r\n                              WHERE TradeID = {0}\r\n                              AND LoanGuid = '{1}'\r\n                              AND AssignedStatus >= 2\r\n                              AND PendingStatus = 0", (object) tradeId, (object) loanGuid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return status == CorrespondentTradeLoanStatus.Unassigned ? dataRowCollection == null || dataRowCollection.Count <= 0 : status >= CorrespondentTradeLoanStatus.Assigned && dataRowCollection != null && dataRowCollection.Count > 0;
    }

    public static void VoidAssigedPendingLoanAssignment(int tradeId, string[] loanIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("delete from TradeAssignment where AssignedStatus = 1 and TradeId = {0} and LoanGuid in ({1})", (object) tradeId, (object) ("'" + string.Join("','", loanIds) + "'")));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateAssignmentsWithTradeExtension(
      int tradeId,
      string[] loanGuids,
      string tradeExtensionInfo)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      tradeExtensionInfo = tradeExtensionInfo.Replace("'", "''");
      dbQueryBuilder.AppendLine(string.Format("UPDATE TradeAssignment SET TradeExtensionInfo = '{0}' WHERE TradeId = {1} and LoanGuid in ({2}) and AssignedStatus >= 2 and PendingStatus = 0", (object) tradeExtensionInfo, (object) tradeId, (object) ("'" + string.Join("','", loanGuids) + "'")));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static CorrespondentTradeInfo[] GetTradeInfosSummaryByExternalOrgId(int externalOrgId)
    {
      return CorrespondentTrades.getTradeSummaryInfosFromDatabase("select TradeID from CorrespondentTrades where ExternalOriginatorManagementID = " + (object) externalOrgId);
    }

    private static CorrespondentTradeInfo[] getTradeSummaryInfosFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from CorrespondentTradeDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select CorrespondentTradeDetails.*,  TradeCorrespondentTradeSummary.* from CorrespondentTradeDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on CorrespondentTradeDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetCorrespondentTradeSummaryInline(" + (object) 1 + ") TradeCorrespondentTradeSummary on CorrespondentTradeDetails.TradeID = TradeCorrespondentTradeSummary.TradeID");
      dbQueryBuilder.AppendLine("   where CorrespondentTradeDetails.TradeID in (" + criteria + ")");
      DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
      CorrespondentTradeInfo[] infosFromDatabase = new CorrespondentTradeInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        CorrespondentTradeInfo tradeInfoSummary = CorrespondentTrades.dataRowToTradeInfoSummary(table.Rows[index]);
        infosFromDatabase[index] = tradeInfoSummary;
      }
      return infosFromDatabase;
    }

    private static CorrespondentTradeInfo dataRowToTradeInfoSummary(DataRow r)
    {
      CorrespondentTradeInfo tradeInfoSummary = new CorrespondentTradeInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]));
      tradeInfoSummary.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]);
      tradeInfoSummary.CorrespondentMasterID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CorrespondentMasterID"], -1);
      tradeInfoSummary.CorrespondentMasterCommitmentNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]);
      tradeInfoSummary.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      tradeInfoSummary.CommitmentType = (CorrespondentTradeCommitmentType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CommitmentType"]);
      tradeInfoSummary.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]);
      tradeInfoSummary.CompanyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CompanyName"]);
      tradeInfoSummary.DeliveryType = (CorrespondentMasterDeliveryType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["DeliveryType"]);
      tradeInfoSummary.TPOID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ExternalID"]);
      tradeInfoSummary.OrganizationID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrganizationID"]);
      tradeInfoSummary.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      tradeInfoSummary.Tolerance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]);
      tradeInfoSummary.MinAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAmount"]);
      tradeInfoSummary.MaxAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAmount"]);
      tradeInfoSummary.ExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ExpirationDate"]);
      tradeInfoSummary.DeliveryExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["DeliveryExpirationDate"]);
      tradeInfoSummary.AOTOriginalTradeDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AOTOriginalTradeDate"]);
      tradeInfoSummary.AOTOriginalTradeDealer = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTOriginalTradeDealer"]);
      tradeInfoSummary.AOTSecurityType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTSecurityType"]);
      tradeInfoSummary.AOTSecurityTerm = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AOTSecurityTerm"]);
      tradeInfoSummary.AOTSecurityPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AOTSecurityPrice"]);
      tradeInfoSummary.AOTSecurityCoupon = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AOTSecurityCoupon"]);
      tradeInfoSummary.AOTSettlementDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AOTSettlementDate"]);
      tradeInfoSummary.PairOffAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]);
      tradeInfoSummary.RejectedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RejectedAmount"]);
      tradeInfoSummary.PurchasedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PurchasedAmount"]);
      tradeInfoSummary.DeliveredAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["DeliveredAmount"]);
      tradeInfoSummary.CompletionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      tradeInfoSummary.ExternalOriginatorManagementID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOriginatorManagementID"]);
      tradeInfoSummary.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0);
      tradeInfoSummary.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OpenAmount"]);
      tradeInfoSummary.GainLossAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GainLossAmount"]);
      tradeInfoSummary.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"]);
      tradeInfoSummary.AssignedLoanList = new List<LoanSummaryExtension>();
      tradeInfoSummary.AutoCreated = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["AutoCreated"]);
      tradeInfoSummary.AutoCreateLoanGUID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AutoCreateLoanGUID"]);
      tradeInfoSummary.OverrideTradeName = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["OverrideTradeName"]);
      tradeInfoSummary.SRPfromPPE = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["SRPfromPPE"]);
      tradeInfoSummary.AdjustmentsfromPPE = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["AdjustmentsfromPPE"]);
      tradeInfoSummary.WeightedAvgBulkPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["WeightedAvgBulkPrice"]);
      tradeInfoSummary.IsWeightedAvgBulkPriceLocked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsWeightedAvgBulkPriceLocked"]);
      tradeInfoSummary.IsToleranceLocked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsToleranceLocked"]);
      tradeInfoSummary.FundType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FundType"]);
      tradeInfoSummary.OriginationRepWarrantType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OriginationRepWarrantType"]);
      tradeInfoSummary.AgencyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AgencyName"]);
      tradeInfoSummary.AgencyDeliveryType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AgencyDeliveryType"]);
      tradeInfoSummary.DocCustodian = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DocCustodian"]);
      tradeInfoSummary.AuthorizedTraderUserId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderUserId"]);
      tradeInfoSummary.AuthorizedTraderName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderName"]);
      tradeInfoSummary.AuthorizedTraderEmail = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AuthorizedTraderEmail"]);
      tradeInfoSummary.LastPublishedDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastPublishedDateTime"]);
      return tradeInfoSummary;
    }

    public static void UpdateCTNotesToJson(string notes, int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("Update TradeObjects set Notes = {0} WHERE TradeId = {1} ", (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(notes), (object) tradeId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static List<int> UpdateCorrespondentTradeStatus()
    {
      List<int> intList = new List<int>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DECLARE @TempTrades TABLE ");
      dbQueryBuilder.AppendLine("( TradeId bigint NOT NULL PRIMARY KEY ) ");
      dbQueryBuilder.AppendLine("DECLARE @tradeCount int ");
      dbQueryBuilder.AppendLine("INSERT INTO @TempTrades ");
      dbQueryBuilder.AppendLine("SELECT t.tradeId FROM Trades t ");
      dbQueryBuilder.AppendLine("INNER JOIN CorrespondentTradeDetails v ON v.tradeId = t.tradeId ");
      dbQueryBuilder.AppendLine("WHERE t.status != v.status AND v.status = 10 ");
      dbQueryBuilder.AppendLine("SELECT @tradeCount = count(*) from @TempTrades ");
      dbQueryBuilder.AppendLine("IF @tradeCount > 0 ");
      dbQueryBuilder.AppendLine("BEGIN ");
      dbQueryBuilder.AppendLine("    UPDATE Trades SET status = 10 WHERE TradeId in ");
      dbQueryBuilder.AppendLine("    ( SELECT TradeId FROM @TempTrades ) ");
      dbQueryBuilder.AppendLine("END ");
      dbQueryBuilder.AppendLine(" SELECT TradeId FROM @TempTrades ");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet.Tables.Count == 0)
        return intList;
      DataTable table = dataSet.Tables[0];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        if (!intList.Contains(EllieMae.EMLite.DataAccess.SQL.DecodeInt(table.Rows[index]["TradeId"])))
          intList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(table.Rows[index]["TradeId"]));
      }
      return intList;
    }

    public static TradeStatus GetTradeStatus(int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @schema varchar(50)");
      dbQueryBuilder.AppendLine("select @schema = schema_name()");
      dbQueryBuilder.AppendLine("declare @sql varchar(1000)");
      dbQueryBuilder.AppendLine("select @sql = 'select [' + @schema + '].FN_CalcCorrTradeStatus(Trades.TradeID, Trades.[Status], CorrespondentTrades.ExpirationDate) AS[Status] FROM CorrespondentTrades INNER JOIN Trades ON CorrespondentTrades.TradeID = Trades.TradeID AND CorrespondentTrades.TradeID =" + (object) tradeId + "'");
      dbQueryBuilder.AppendLine("exec(@sql)");
      return (TradeStatus) Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
    }

    public static TradeNoteModel CreateTradeNote(
      int tradeId,
      TradeNoteModel tradeNoteModel,
      UserInfo currentUser)
    {
      CorrespondentTradeInfo trade = CorrespondentTrades.GetTrade(tradeId);
      if (trade == null)
        throw new ObjectNotFoundException(string.Format("Trade id {0} not found ", (object) tradeId), ObjectType.Trade, (object) tradeId);
      string tradeNotes = CorrespondentTrades.parseTradeNotes(trade.TradeID, trade.Notes, currentUser);
      List<TradeNoteModel> tradeNoteModelList = new List<TradeNoteModel>();
      if (!string.IsNullOrEmpty(tradeNotes))
      {
        TradeNoteModel[] collection = TradeNoteUtils.DeserializeTradeNotes(tradeNotes);
        tradeNoteModelList.AddRange((IEnumerable<TradeNoteModel>) collection);
      }
      int num = -1;
      if (tradeNoteModelList.Count > 0)
      {
        for (int index = 0; index < tradeNoteModelList.Count; ++index)
        {
          if (tradeNoteModelList[index].Id > num)
            num = tradeNoteModelList[index].Id;
        }
      }
      TradeNoteModel tradeNote = TradeNoteUtils.PrepareTradeNoteModel(num + 1, tradeNoteModel.Details, currentUser.Userid, currentUser.userName);
      if (tradeNote != null)
        tradeNoteModelList.Add(tradeNote);
      string notes = JsonConvert.SerializeObject((object) tradeNoteModelList);
      if (!string.IsNullOrEmpty(notes))
        CorrespondentTrades.UpdateCTNotesToJson(notes, tradeId);
      CorrespondentTrades.AddHistoryForUpdateTrade(CorrespondentTrades.GetTrade(tradeId), trade, currentUser, false);
      return tradeNote;
    }

    public static TradeNoteModel UpdateTradeNote(
      int tradeId,
      TradeNoteModel tradeNoteModel,
      UserInfo currentUser,
      bool isRemove = false)
    {
      CorrespondentTradeInfo trade = CorrespondentTrades.GetTrade(tradeId);
      if (trade == null)
        throw new ObjectNotFoundException(string.Format("Trade id {0} not found ", (object) tradeId), ObjectType.Trade, (object) tradeId);
      string tradeNotes = CorrespondentTrades.parseTradeNotes(trade.TradeID, trade.Notes, currentUser);
      TradeNoteModel[] tradeNoteModelArray = (TradeNoteModel[]) null;
      if (!string.IsNullOrEmpty(tradeNotes))
        tradeNoteModelArray = TradeNoteUtils.DeserializeTradeNotes(tradeNotes);
      TradeNoteModel tradeNoteModel1 = (TradeNoteModel) null;
      foreach (TradeNoteModel tradeNoteModel2 in tradeNoteModelArray)
      {
        if (tradeNoteModel2.Id == tradeNoteModel.Id)
        {
          tradeNoteModel1 = tradeNoteModel2;
          if (isRemove)
          {
            tradeNoteModel2.Details += string.Format(" (Removed {0} by user {1})", (object) DateTime.UtcNow.ToString("u"), (object) currentUser.FullName);
            break;
          }
          tradeNoteModel2.Details = string.Format("{0} (Was \"{1}\" modified {2} by user {3})", (object) tradeNoteModel.Details, (object) tradeNoteModel2.Details, (object) DateTime.UtcNow.ToString("u"), (object) tradeNoteModel2.CreateBy.EntityName);
          break;
        }
      }
      if (tradeNoteModel1 == null)
        throw new ObjectNotFoundException(string.Format("Message id {0} for trade id {1} not found ", (object) tradeNoteModel.Id, (object) tradeId), ObjectType.Trade, (object) trade.TradeID);
      string notes = JsonConvert.SerializeObject((object) tradeNoteModelArray);
      if (!string.IsNullOrEmpty(notes))
        CorrespondentTrades.UpdateCTNotesToJson(notes, tradeId);
      CorrespondentTrades.AddHistoryForUpdateTrade(CorrespondentTrades.GetTrade(tradeId), trade, currentUser, false);
      return tradeNoteModel1;
    }

    public static List<TradeNoteModel> UpdateTradeNotes(
      int tradeId,
      List<TradeNoteModel> tradeNoteUpdates,
      UserInfo currentUser)
    {
      CorrespondentTradeInfo trade = CorrespondentTrades.GetTrade(tradeId);
      string notes = JsonConvert.SerializeObject((object) tradeNoteUpdates);
      if (!string.IsNullOrEmpty(notes))
        CorrespondentTrades.UpdateCTNotesToJson(notes, tradeId);
      CorrespondentTrades.AddHistoryForUpdateTrade(CorrespondentTrades.GetTrade(tradeId), trade, currentUser, false);
      return tradeNoteUpdates;
    }
  }
}
