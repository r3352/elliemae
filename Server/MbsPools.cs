// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.MbsPools
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
  public static class MbsPools
  {
    private static string className = nameof (MbsPools);
    private static string UPDATETRADEERROR = "This pool is currently pending in the Trade Update Queue and cannot be modified until completed.";

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
      get => new List<TradeType>() { TradeType.MbsPool };
    }

    public static MbsPoolInfo[] GetAllTrades()
    {
      return MbsPools.getTradesFromDatabase("select TradeID from MbsPoolDetails");
    }

    public static MbsPoolInfo GetTrade(int tradeId)
    {
      MbsPoolInfo[] tradesFromDatabase = MbsPools.getTradesFromDatabase(string.Concat((object) tradeId));
      return tradesFromDatabase.Length == 0 ? (MbsPoolInfo) null : tradesFromDatabase[0];
    }

    private static MbsPoolInfo[] getTradesFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from MbsPoolDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select MbsPoolDetails.*, TradeObjects.*, TradeMbsPoolSummary.*, MasterContracts.ContractNumber AS MasterContractNumber from MbsPoolDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on MbsPoolDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on MbsPoolDetails.ContractID = MasterContracts.ContractID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetMbsPoolSummaryInline(" + (object) 1 + ") TradeMbsPoolSummary on MbsPoolDetails.TradeID = TradeMbsPoolSummary.TradeID");
      dbQueryBuilder.AppendLine("   where MbsPoolDetails.TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by TradeID, PendingStatus");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      MbsPoolInfo[] tradesFromDatabase = new MbsPoolInfo[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        MbsPoolInfo tradeInfo = MbsPools.dataRowToTradeInfo(table1.Rows[index], table2);
        tradesFromDatabase[index] = tradeInfo;
      }
      return tradesFromDatabase;
    }

    private static MbsPoolViewModel[] getTradeViewsFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from MbsPoolDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select MbsPoolDetails.*, TradeObjects.*, TradeMbsPoolSummary.*, MasterContracts.ContractNumber AS MasterContractNumber, SecurityTradeDetails.TradeID as SecurityTradeID, SecurityTradeDetails.SecurityType, AssignedSecurityTradeDetails1.Name as AssignedSecurityID, SecurityTradeDetails.Price from MbsPoolDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on MbsPoolDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on MbsPoolDetails.ContractID = MasterContracts.ContractID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetMbsPoolSummaryInline(" + (object) 1 + ") TradeMbsPoolSummary on MbsPoolDetails.TradeID = TradeMbsPoolSummary.TradeID");
      dbQueryBuilder.AppendLine("   left outer join TradeAssignmentByTrade AssignedSecurityTrade on AssignedSecurityTrade.AssigneeTradeID = MbsPoolDetails.TradeID");
      dbQueryBuilder.AppendLine("   left outer join SecurityTradeDetails SecurityTradeDetails on SecurityTradeDetails.TradeID = AssignedSecurityTrade.TradeID");
      dbQueryBuilder.AppendLine("   left outer join FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") SecurityTradesummary on SecurityTradeDetails.TradeID = SecurityTradeSummary.TradeID");
      dbQueryBuilder.AppendLine("   left outer join TradeAssignmentByTrade AssignedSecurityTrade1 on AssignedSecurityTrade1.TradeID = MbsPoolDetails.TradeID");
      dbQueryBuilder.AppendLine("   left outer join Trades AssignedSecurityTradeDetails1 on AssignedSecurityTradeDetails1.TradeID = AssignedSecurityTrade1.AssigneeTradeID");
      dbQueryBuilder.AppendLine("   where MbsPoolDetails.TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("select TradeID, PendingStatus, Count(*) as LoanCount from TradeAssignment");
      dbQueryBuilder.AppendLine(" inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine(" where TradeID in (" + criteria + ")");
      dbQueryBuilder.AppendLine("   and PendingStatus > 0");
      dbQueryBuilder.AppendLine(" group by TradeID, PendingStatus");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      MbsPoolViewModel[] viewsFromDatabase = new MbsPoolViewModel[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        MbsPoolViewModel tradeViewInfo = MbsPools.dataRowToTradeViewInfo(table1.Rows[index], table2);
        viewsFromDatabase[index] = tradeViewInfo;
      }
      return viewsFromDatabase;
    }

    private static MbsPoolViewModel dataRowToTradeViewInfo(DataRow r, DataTable statusTable)
    {
      Dictionary<MbsPoolLoanStatus, int> pendingLoanCounts = MbsPools.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      MbsPoolViewModel tradeViewInfo = new MbsPoolViewModel(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastModified"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["LoanCount"]), pendingLoanCounts);
      tradeViewInfo.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"], false);
      tradeViewInfo.ContractID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"]);
      tradeViewInfo.InvestorName = string.Concat(r["InvestorName"]);
      tradeViewInfo.InvestorDeliveryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InvestorDeliveryDate"]);
      tradeViewInfo.EarlyDeliveryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EarlyDeliveryDate"]);
      tradeViewInfo.TargetDeliveryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["TargetDeliveryDate"]);
      tradeViewInfo.ShipmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ShipmentDate"]);
      tradeViewInfo.PurchaseDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PurchaseDate"]);
      tradeViewInfo.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      tradeViewInfo.AssignedProfit = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]);
      tradeViewInfo.PendingLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PendingLoanCount"]);
      tradeViewInfo.NotWithdrawnLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["NotWithdrawnLoanCount"]);
      tradeViewInfo.PoolMortgageType = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PoolMortgageType"], 0).ToString();
      tradeViewInfo.PoolNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolNumber"], "");
      tradeViewInfo.SuffixID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SuffixID"], "");
      tradeViewInfo.CUSIP = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CUSIP"], "");
      tradeViewInfo.MortgageType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MortgageType"], "");
      tradeViewInfo.AmortizationType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AmortizationType"], "");
      tradeViewInfo.Term = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term"]);
      tradeViewInfo.ServicingType = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ServicingType"]);
      tradeViewInfo.WeightedAvgPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["WeightedAvgPrice"]);
      tradeViewInfo.WeightedAvgPriceLocked = r["WeightedAvgPriceLocked"] is DBNull || r["WeightedAvgPriceLocked"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["WeightedAvgPriceLocked"], false) ? "1" : "0");
      tradeViewInfo.SettlementDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["SettlementDate"]);
      tradeViewInfo.SettlementMonth = string.Concat(r["SettlementMonth"]);
      tradeViewInfo.NotificationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["NotificationDate"]);
      tradeViewInfo.ContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MasterContractNumber"], "");
      tradeViewInfo.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]);
      tradeViewInfo.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      tradeViewInfo.CommitmentType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentType"]);
      tradeViewInfo.Coupon = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Coupon"]);
      tradeViewInfo.Servicer = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Servicer"]);
      tradeViewInfo.RateAdjustment = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RateAdjustment"]);
      tradeViewInfo.BuyUpAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyUpAmount"]);
      tradeViewInfo.BuyDownAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyDownAmount"]);
      tradeViewInfo.MiscAdjustment = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MiscAdjustment"]);
      tradeViewInfo.DocCustodianID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DocCustodianID"], "");
      tradeViewInfo.ServicerID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ServicerID"], "");
      tradeViewInfo.InvestorProductPlanID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorProductPlanID"], "");
      tradeViewInfo.InvestorFeatureID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorFeatureID"], "");
      tradeViewInfo.LoanDefaultLossParty = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanDefaultLossParty"], "");
      tradeViewInfo.ReoMarketingParty = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ReoMarketingParty"], "");
      tradeViewInfo.BaseGuarantyFee = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BaseGuarantyFee"]);
      tradeViewInfo.GFeeAfterAltPaymentMethod = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GFeeAfterAltPaymentMethod"]);
      tradeViewInfo.GuaranteeFee = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GuaranteeFee"]);
      tradeViewInfo.InvestorRemittanceDay = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["InvestorRemittanceDay"]);
      tradeViewInfo.ContractNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNum"], "");
      tradeViewInfo.IssueDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["IssueDate"]);
      tradeViewInfo.OwnershipPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OwnershipPercent"]);
      tradeViewInfo.StructureType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["StructureType"], "");
      tradeViewInfo.AccrualRateStructureType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AccrualRateStructureType"], "");
      tradeViewInfo.SecurityIssueDateIntRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["SecurityIssueDateIntRate"]);
      tradeViewInfo.MinAccuralRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAccuralRate"]);
      tradeViewInfo.MaxAccuralRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAccuralRate"]);
      tradeViewInfo.MarginRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MarginRate"]);
      tradeViewInfo.IntRateRoundingType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["IntRateRoundingType"], "");
      tradeViewInfo.IntRateRoundingPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["IntRateRoundingPercent"]);
      tradeViewInfo.IsInterestOnly = r["IsInterestOnly"] is DBNull || r["IsInterestOnly"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsInterestOnly"], false) ? "1" : "0");
      tradeViewInfo.IntPaymentAdjIndexLeadDays = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["IntPaymentAdjIndexLeadDays"]);
      tradeViewInfo.IsAssumability = r["IsAssumability"] is DBNull || r["IsAssumability"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsAssumability"], false) ? "1" : "0");
      tradeViewInfo.IsBalloon = r["IsBalloon"] is DBNull || r["IsBalloon"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsBalloon"], false) ? "1" : "0");
      tradeViewInfo.FixedServicingFeePercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["FixedServicingFeePercent"]);
      tradeViewInfo.ScheduledRemittancePaymentDay = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ScheduledRemittancePaymentDay"]);
      tradeViewInfo.SecurityTradeBookEntryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["SecurityTradeBookEntryDate"]);
      tradeViewInfo.PayeeCode = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PayeeCode"], "");
      tradeViewInfo.CommitmentPeriod = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentPeriod"], "");
      tradeViewInfo.SubmissionType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubmissionType"], "");
      tradeViewInfo.PlanNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PlanNum"], "");
      tradeViewInfo.PassThruRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PassThruRate"]);
      tradeViewInfo.ForecloseRiskCode = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ForecloseRiskCode"], "");
      tradeViewInfo.MbsMargin = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MbsMargin"]);
      tradeViewInfo.ContractType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractType"], "");
      tradeViewInfo.DeliveryRegion = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DeliveryRegion"], "");
      tradeViewInfo.InterestOnlyEndDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InterestOnlyEndDate"]);
      tradeViewInfo.IsMultiFamily = r["IsMultiFamily"] is DBNull || r["IsMultiFamily"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsMultiFamily"], false) ? "1" : "0");
      tradeViewInfo.NoteCustodian = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["NoteCustodian"], "");
      tradeViewInfo.FinancialInstNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FinancialInstNum"], "");
      tradeViewInfo.StandardLookback = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["StandardLookback"], "");
      tradeViewInfo.IsGuarantyFeeAddOn = r["IsGuarantyFeeAddOn"] is DBNull || r["IsGuarantyFeeAddOn"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsGuarantyFeeAddOn"], false) ? "1" : "0");
      tradeViewInfo.InvestorRemittanceType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorRemittanceType"], "");
      tradeViewInfo.MinServicingFee = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinServicingFee"]);
      tradeViewInfo.MaxBU = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxBU"]);
      tradeViewInfo.SellerId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SellerId"], "");
      tradeViewInfo.IssuerNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["IssuerNum"], "");
      tradeViewInfo.IssueType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["IssueType"], "");
      tradeViewInfo.ARMIndex = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ARMIndex"]);
      tradeViewInfo.CertAgreement = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CertAgreement"], "");
      tradeViewInfo.PnICustodialABA = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PnICustodialABA"], "");
      tradeViewInfo.SubscriberRecordABA = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordABA"], "");
      tradeViewInfo.SubscriberRecordFRBPosDesc = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordFRBPosDesc"], "");
      tradeViewInfo.SubscriberRecordFRBAcctDesc = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordFRBAcctDesc"], "");
      tradeViewInfo.MasterTnIABA = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MasterTnIABA"], "");
      tradeViewInfo.NewTransferIssuerNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["NewTransferIssuerNum"], "");
      tradeViewInfo.MaturityDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["MaturityDate"]);
      tradeViewInfo.PoolTaxID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolTaxID"], "");
      tradeViewInfo.ChangeDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ChangeDate"]);
      tradeViewInfo.IsBondFinancePool = r["IsBondFinancePool"] is DBNull || r["IsBondFinancePool"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsBondFinancePool"], false) ? "1" : "0");
      tradeViewInfo.IsSent1711ToCustodian = r["IsSent1711ToCustodian"] is DBNull || r["IsSent1711ToCustodian"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsSent1711ToCustodian"], false) ? "1" : "0");
      tradeViewInfo.PnICustodialAcctNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PnICustodialAcctNum"], "");
      tradeViewInfo.SubscriberRecordAcctNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordAcctNum"], "");
      tradeViewInfo.MasterTnIAcctNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MasterTnIAcctNum"], "");
      tradeViewInfo.SubservicerIssuerNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubservicerIssuerNum"], "");
      tradeViewInfo.GinniePoolType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["GinniePoolType"], "");
      tradeViewInfo.InitialDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InitialDate"]);
      tradeViewInfo.LastPaidInstallmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastPaidInstallmentDate"]);
      tradeViewInfo.UnpaidBalanceDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["UnpaidBalanceDate"]);
      tradeViewInfo.ACHBankAccountPurposeType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHBankAccountPurposeType"], "");
      tradeViewInfo.ACHABARoutingAndTransitNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHABARoutingTransitNum"], "");
      tradeViewInfo.ACHABARoutingAndTransitId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHABARoutingTransitId"], "");
      tradeViewInfo.ACHInsitutionTelegraphicName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHInstitTelName"], "");
      tradeViewInfo.ACHReceiverSubaccountName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHReceiverSubacctName"], "");
      tradeViewInfo.ACHBankAccountDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHBankAccountDesc"], "");
      tradeViewInfo.GinniePoolIndexType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["GinnieIndexType"], "");
      tradeViewInfo.PoolIssuerTransferee = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolIssuerTransferee"], "");
      tradeViewInfo.PoolCertificatePaymentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PoolCertPaymentDate"]);
      tradeViewInfo.BondFinanceProgramType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BondFinProgType"], "");
      tradeViewInfo.BondFinanceProgramName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BondFinProgName"], "");
      tradeViewInfo.GinniePoolClassType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolClassType"], "");
      tradeViewInfo.GinniePoolConcurrentTransferIndicator = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolConcurrTransferIndc"], "");
      tradeViewInfo.PoolCurrentLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PoolCurrentLoanCount"], 0);
      tradeViewInfo.PoolCurrentPrincipalBalAmt = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PoolCurrentPrinBal"], 0M);
      tradeViewInfo.PoolingMethodType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolingMethodType"], "");
      tradeViewInfo.PoolInterestAdjustmentEffectiveDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PoolInterestAdjEffectiveDate"]);
      tradeViewInfo.PoolMaturityPeriodCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PoolMaturityPeriodCount"], 0);
      tradeViewInfo.DocSubmissionIndicator = r["DocSubmissionIndic"] is DBNull || r["DocSubmissionIndic"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["DocSubmissionIndic"], false) ? "1" : "0");
      tradeViewInfo.DocReqIndicator = r["DocReqIndic"] is DBNull || r["DocReqIndic"] == null ? "" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["DocReqIndic"], false) ? "1" : "0");
      tradeViewInfo.SecurityOrigSubscriptionAmt = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["SecurityOriginalSubscrAmt"], 0M);
      tradeViewInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["LoanOpenAmount"]);
      tradeViewInfo.CompletionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      tradeViewInfo.TBAOpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OpenAmount"]);
      tradeViewInfo.AssignedAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalAmount"]);
      tradeViewInfo.AssignedSecurityID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AssignedSecurityID"], "");
      return tradeViewInfo;
    }

    private static MbsPoolInfo dataRowToTradeInfo(DataRow r, DataTable statusTable)
    {
      Dictionary<MbsPoolLoanStatus, int> pendingLoanCounts = MbsPools.dataRowsToPendingLoanCounts((IEnumerable) statusTable.Select("TradeID = " + r["TradeID"]));
      MbsPoolInfo tradeInfo = new MbsPoolInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Notes"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FilterQueryXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SRPTableXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DealerXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AssigneeXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PricingXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AdjustmentsXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BuyUpDownXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ProductNameXml"]), pendingLoanCounts);
      tradeInfo.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"], "");
      tradeInfo.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0);
      tradeInfo.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"], false);
      tradeInfo.ContractID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"], -1);
      tradeInfo.InvestorName = string.Concat(r["InvestorName"]);
      tradeInfo.InvestorDeliveryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InvestorDeliveryDate"]);
      tradeInfo.EarlyDeliveryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EarlyDeliveryDate"]);
      tradeInfo.TargetDeliveryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["TargetDeliveryDate"]);
      tradeInfo.ShipmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ShipmentDate"]);
      tradeInfo.PurchaseDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PurchaseDate"]);
      tradeInfo.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      tradeInfo.PoolMortgageType = (MbsPoolMortgageType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PoolMortgageType"], 0);
      tradeInfo.PoolNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolNumber"], "");
      tradeInfo.SuffixID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SuffixID"], "");
      tradeInfo.CUSIP = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CUSIP"], "");
      tradeInfo.MortgageType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MortgageType"], "");
      tradeInfo.AmortizationType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AmortizationType"], "");
      tradeInfo.Term = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term"]);
      tradeInfo.ServicingType = (ServicingType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ServicingType"], 0);
      tradeInfo.WeightedAvgPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["WeightedAvgPrice"]);
      tradeInfo.WeightedAvgPriceLocked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["WeightedAvgPriceLocked"], false);
      tradeInfo.SettlementDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["SettlementDate"]);
      tradeInfo.SettlementMonth = string.Concat(r["SettlementMonth"]);
      tradeInfo.NotificationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["NotificationDate"]);
      tradeInfo.ContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MasterContractNumber"], "");
      tradeInfo.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      tradeInfo.CommitmentType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentType"]);
      tradeInfo.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]);
      tradeInfo.Coupon = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Coupon"]);
      tradeInfo.Servicer = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Servicer"]);
      tradeInfo.RateAdjustment = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["RateAdjustment"]);
      tradeInfo.BuyUpAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyUpAmount"]);
      tradeInfo.BuyDownAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BuyDownAmount"]);
      tradeInfo.MiscAdjustment = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MiscAdjustment"]);
      tradeInfo.DocCustodianID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DocCustodianID"], "");
      tradeInfo.ServicerID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ServicerID"], "");
      tradeInfo.InvestorProductPlanID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorProductPlanID"], "");
      tradeInfo.InvestorFeatureID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorFeatureID"], "");
      tradeInfo.LoanDefaultLossParty = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanDefaultLossParty"], "");
      tradeInfo.ReoMarketingParty = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ReoMarketingParty"], "");
      tradeInfo.BaseGuarantyFee = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["BaseGuarantyFee"]);
      tradeInfo.GFeeAfterAltPaymentMethod = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GFeeAfterAltPaymentMethod"]);
      tradeInfo.GuaranteeFee = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["GuaranteeFee"]);
      tradeInfo.InvestorRemittanceDay = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["InvestorRemittanceDay"]);
      tradeInfo.ContractNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNum"], "");
      tradeInfo.IssueDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["IssueDate"]);
      tradeInfo.OwnershipPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OwnershipPercent"]);
      tradeInfo.StructureType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["StructureType"]);
      tradeInfo.AccrualRateStructureType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AccrualRateStructureType"]);
      tradeInfo.SecurityIssueDateIntRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["SecurityIssueDateIntRate"]);
      tradeInfo.MinAccuralRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAccuralRate"]);
      tradeInfo.MaxAccuralRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAccuralRate"]);
      tradeInfo.MarginRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MarginRate"]);
      tradeInfo.IntRateRoundingType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["IntRateRoundingType"]);
      tradeInfo.IntRateRoundingPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["IntRateRoundingPercent"]);
      tradeInfo.IsInterestOnly = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsInterestOnly"], false);
      tradeInfo.IntPaymentAdjIndexLeadDays = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["IntPaymentAdjIndexLeadDays"]);
      tradeInfo.IsAssumability = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsAssumability"], false);
      tradeInfo.IsBalloon = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsBalloon"], false);
      tradeInfo.FixedServicingFeePercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["FixedServicingFeePercent"]);
      tradeInfo.ScheduledRemittancePaymentDay = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ScheduledRemittancePaymentDay"]);
      tradeInfo.SecurityTradeBookEntryDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["SecurityTradeBookEntryDate"]);
      tradeInfo.PayeeCode = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PayeeCode"], "");
      tradeInfo.CommitmentPeriod = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentPeriod"], "");
      tradeInfo.SubmissionType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubmissionType"], "");
      tradeInfo.PlanNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PlanNum"], "");
      tradeInfo.PassThruRate = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PassThruRate"]);
      tradeInfo.ForecloseRiskCode = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ForecloseRiskCode"], "");
      tradeInfo.MbsMargin = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MbsMargin"]);
      tradeInfo.ContractType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractType"], "");
      tradeInfo.DeliveryRegion = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DeliveryRegion"], "");
      tradeInfo.InterestOnlyEndDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InterestOnlyEndDate"]);
      tradeInfo.IsMultiFamily = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsMultiFamily"], false);
      tradeInfo.NoteCustodian = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["NoteCustodian"], "");
      tradeInfo.FinancialInstNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FinancialInstNum"], "");
      tradeInfo.StandardLookback = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["StandardLookback"], "");
      tradeInfo.IsGuarantyFeeAddOn = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsGuarantyFeeAddOn"], false);
      tradeInfo.InvestorRemittanceType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorRemittanceType"], "");
      tradeInfo.SellerId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SellerId"], "");
      tradeInfo.MinServicingFee = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinServicingFee"]);
      tradeInfo.MaxBU = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxBU"]);
      tradeInfo.IssuerNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["IssuerNum"], "");
      tradeInfo.IssueType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["IssueType"], "");
      tradeInfo.ARMIndex = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ARMIndex"]);
      tradeInfo.CertAgreement = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CertAgreement"], "");
      tradeInfo.PnICustodialABA = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PnICustodialABA"], "");
      tradeInfo.SubscriberRecordABA = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordABA"], "");
      tradeInfo.SubscriberRecordFRBPosDesc = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordFRBPosDesc"], "");
      tradeInfo.SubscriberRecordFRBAcctDesc = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordFRBAcctDesc"], "");
      tradeInfo.MasterTnIABA = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MasterTnIABA"], "");
      tradeInfo.NewTransferIssuerNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["NewTransferIssuerNum"], "");
      tradeInfo.MaturityDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["MaturityDate"]);
      tradeInfo.PoolTaxID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolTaxID"], "");
      tradeInfo.ChangeDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ChangeDate"]);
      tradeInfo.IsBondFinancePool = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsBondFinancePool"], false);
      tradeInfo.IsSent1711ToCustodian = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsSent1711ToCustodian"], false);
      tradeInfo.PnICustodialAcctNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PnICustodialAcctNum"], "");
      tradeInfo.SubscriberRecordAcctNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubscriberRecordAcctNum"], "");
      tradeInfo.MasterTnIAcctNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["MasterTnIAcctNum"], "");
      tradeInfo.SubservicerIssuerNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SubservicerIssuerNum"], "");
      tradeInfo.GinniePoolType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["GinniePoolType"], "");
      tradeInfo.InitialDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["InitialDate"]);
      tradeInfo.LastPaidInstallmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastPaidInstallmentDate"]);
      tradeInfo.UnpaidBalanceDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["UnpaidBalanceDate"]);
      tradeInfo.ACHBankAccountPurposeType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHBankAccountPurposeType"], "");
      tradeInfo.ACHABARoutingAndTransitNum = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHABARoutingTransitNum"], "");
      tradeInfo.ACHABARoutingAndTransitId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHABARoutingTransitId"], "");
      tradeInfo.ACHInsitutionTelegraphicName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHInstitTelName"], "");
      tradeInfo.ACHReceiverSubaccountName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHReceiverSubacctName"], "");
      tradeInfo.ACHBankAccountDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ACHBankAccountDesc"], "");
      tradeInfo.GinniePoolIndexType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["GinnieIndexType"], "");
      tradeInfo.PoolIssuerTransferee = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolIssuerTransferee"], "");
      tradeInfo.PoolCertificatePaymentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PoolCertPaymentDate"]);
      tradeInfo.BondFinanceProgramType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BondFinProgType"], "");
      tradeInfo.BondFinanceProgramName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["BondFinProgName"], "");
      tradeInfo.GinniePoolClassType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolClassType"], "");
      tradeInfo.GinniePoolConcurrentTransferIndicator = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["PoolConcurrTransferIndc"]);
      tradeInfo.PoolCurrentLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PoolCurrentLoanCount"], 0);
      tradeInfo.PoolCurrentPrincipalBalAmt = EllieMae.EMLite.DataAccess.SQL.DecodeLong(r["PoolCurrentPrinBal"], 0L);
      tradeInfo.PoolingMethodType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PoolingMethodType"], "");
      tradeInfo.PoolInterestAdjustmentEffectiveDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PoolInterestAdjEffectiveDate"]);
      tradeInfo.PoolMaturityPeriodCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PoolMaturityPeriodCount"], 0);
      tradeInfo.DocSubmissionIndicator = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["DocSubmissionIndic"], false);
      tradeInfo.DocReqIndicator = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["DocReqIndic"], false);
      tradeInfo.SecurityOrigSubscriptionAmt = EllieMae.EMLite.DataAccess.SQL.DecodeLong(r["SecurityOriginalSubscrAmt"], 0L);
      tradeInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["LoanOpenAmount"]);
      tradeInfo.TBAOpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OpenAmount"]);
      return tradeInfo;
    }

    public static MasterContractSummaryInfo[] GetActiveContracts(bool includeTradeData)
    {
      return MbsPools.getContractsFromDatabase("select ContractID from MasterContracts where Status <> " + (object) 1, includeTradeData, true);
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
        contractsFromDatabase[index] = !summariesOnly ? (MasterContractSummaryInfo) MbsPools.dataRowToMasterContractInfo(dataRowCollection[index]) : MbsPools.dataRowToMasterContractSummaryInfo(dataRowCollection[index]);
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

    public static MbsPoolHistoryItem[] GetTradeHistory(int tradeId)
    {
      return MbsPools.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where TradeID = " + (object) tradeId);
    }

    private static MbsPoolHistoryItem[] getTradeHistoryFromDatabase(string selectionQuery)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select TradeHistory.*, LoanSummary.LoanNumber, Trades.Name as TradeName, Trades.InvestorName, MasterContracts.ContractNumber");
      dbQueryBuilder.AppendLine("from TradeHistory inner join Trades on TradeHistory.TradeID = Trades.TradeID and TradeType = 3");
      dbQueryBuilder.AppendLine("   left outer join LoanSummary on TradeHistory.LoanGuid = LoanSummary.Guid");
      dbQueryBuilder.AppendLine("   left outer join MasterContracts on TradeHistory.ContractID = MasterContracts.ContractID");
      if ((selectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("where TradeHistory.HistoryID in (" + selectionQuery + ")");
      return MbsPools.dataRowsToTradeHistoryItems((IEnumerable) dbQueryBuilder.Execute());
    }

    private static MbsPoolHistoryItem[] dataRowsToTradeHistoryItems(IEnumerable rows)
    {
      List<MbsPoolHistoryItem> mbsPoolHistoryItemList = new List<MbsPoolHistoryItem>();
      foreach (DataRow row in rows)
        mbsPoolHistoryItemList.Add(MbsPools.dataRowToHistoryItem(row));
      return mbsPoolHistoryItemList.ToArray();
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      int tradeId,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      return MbsPools.GetAssignedOrPendingLoans(user, MbsPools.GetTrade(tradeId) ?? throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Template, (object) tradeId), fields, isExternalOrganization, sqlRead);
    }

    public static PipelineInfo[] GetAssignedOrPendingLoans(
      UserInfo user,
      MbsPoolInfo trade,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      string identitySelectionQuery = "select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid where TradeAssignment.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 1;
      return Pipeline.Generate(user, identitySelectionQuery, fields, PipelineData.Trade, isExternalOrganization, TradeType.MbsPool, sqlRead);
    }

    public static MasterContractInfo GetContractForTrade(int tradeId)
    {
      MasterContractSummaryInfo[] contractsFromDatabase = MbsPools.getContractsFromDatabase("select ContractID from MbsPools inner join Trades on MbsPools.TradeID = Trades.TradeID  where Trades.TradeID = " + (object) tradeId, false, false);
      return contractsFromDatabase.Length == 0 ? (MasterContractInfo) null : (MasterContractInfo) contractsFromDatabase[0];
    }

    public static int CreateTrade(MbsPoolInfo info, UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (MbsPools));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      dbQueryBuilder.Declare("@tradeId", "int");
      dbQueryBuilder.InsertInto(table1, TradeUtils.CreateTradesValueList((TradeInfoObj) info), true, false);
      dbQueryBuilder.SelectIdentity("@tradeId");
      DbValue dbValue = new DbValue("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
      DbValueList mbsPoolValueList = MbsPools.createMbsPoolValueList(info);
      mbsPoolValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table2, mbsPoolValueList, true, false);
      DbValueList objectsValueList = MbsPools.createTradeObjectsValueList(info);
      objectsValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.Select("@tradeId");
      int tradeId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      info = MbsPools.GetTrade(tradeId);
      MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, TradeHistoryAction.TradeCreated, currentUser));
      if (info.Status != TradeStatus.Open)
        MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, info.Status, currentUser));
      if (info.Locked)
        MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, TradeHistoryAction.TradeLocked, currentUser));
      if (info.ContractID > 0)
      {
        MasterContractSummaryInfo contract = (MasterContractSummaryInfo) MbsPools.GetContract(info.ContractID);
        if (contract != null)
          MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, contract, TradeHistoryAction.ContractAssigned, currentUser));
      }
      return tradeId;
    }

    private static DbValueList createMbsPoolValueList(MbsPoolInfo info)
    {
      return new DbValueList()
      {
        {
          "PoolMortgageType",
          (object) (int) info.PoolMortgageType
        },
        {
          "PoolNumber",
          (object) info.PoolNumber
        },
        {
          "SuffixID",
          (object) info.SuffixID
        },
        {
          "CUSIP",
          (object) info.CUSIP
        },
        {
          "MortgageType",
          (object) info.MortgageType
        },
        {
          "AmortizationType",
          (object) info.AmortizationType
        },
        {
          "Term",
          (object) info.Term
        },
        {
          "ServicingType",
          (object) (int) info.ServicingType
        },
        {
          "WeightedAvgPrice",
          (object) info.WeightedAvgPrice
        },
        {
          "WeightedAvgPriceLocked",
          (object) info.WeightedAvgPriceLocked,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "SettlementDate",
          (object) info.SettlementDate
        },
        {
          "SettlementMonth",
          (object) info.SettlementMonth
        },
        {
          "NotificationDate",
          (object) info.NotificationDate
        },
        {
          "Coupon",
          (object) info.Coupon
        },
        {
          "Servicer",
          (object) info.Servicer
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
          "DocCustodianID",
          (object) info.DocCustodianID
        },
        {
          "ServicerID",
          (object) info.ServicerID
        },
        {
          "InvestorProductPlanID",
          (object) info.InvestorProductPlanID
        },
        {
          "InvestorFeatureID",
          (object) info.InvestorFeatureID
        },
        {
          "LoanDefaultLossParty",
          (object) info.LoanDefaultLossParty
        },
        {
          "ReoMarketingParty",
          (object) info.ReoMarketingParty
        },
        {
          "BaseGuarantyFee",
          (object) info.BaseGuarantyFee
        },
        {
          "GFeeAfterAltPaymentMethod",
          (object) info.GFeeAfterAltPaymentMethod
        },
        {
          "GuaranteeFee",
          (object) info.GuaranteeFee
        },
        {
          "InvestorRemittanceDay",
          (object) info.InvestorRemittanceDay
        },
        {
          "ContractNum",
          (object) info.ContractNum
        },
        {
          "IssueDate",
          (object) info.IssueDate
        },
        {
          "OwnershipPercent",
          (object) info.OwnershipPercent
        },
        {
          "StructureType",
          (object) info.StructureType
        },
        {
          "AccrualRateStructureType",
          (object) info.AccrualRateStructureType
        },
        {
          "SecurityIssueDateIntRate",
          (object) info.SecurityIssueDateIntRate
        },
        {
          "MinAccuralRate",
          (object) info.MinAccuralRate
        },
        {
          "MaxAccuralRate",
          (object) info.MaxAccuralRate
        },
        {
          "MarginRate",
          (object) info.MarginRate
        },
        {
          "IntRateRoundingType",
          (object) info.IntRateRoundingType
        },
        {
          "IntRateRoundingPercent",
          (object) info.IntRateRoundingPercent
        },
        {
          "IsInterestOnly",
          (object) info.IsInterestOnly,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "IntPaymentAdjIndexLeadDays",
          (object) info.IntPaymentAdjIndexLeadDays
        },
        {
          "IsAssumability",
          (object) info.IsAssumability,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "IsBalloon",
          (object) info.IsBalloon,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "FixedServicingFeePercent",
          (object) info.FixedServicingFeePercent
        },
        {
          "ScheduledRemittancePaymentDay",
          (object) info.ScheduledRemittancePaymentDay
        },
        {
          "SecurityTradeBookEntryDate",
          (object) info.SecurityTradeBookEntryDate
        },
        {
          "PayeeCode",
          (object) info.PayeeCode
        },
        {
          "CommitmentPeriod",
          (object) info.CommitmentPeriod
        },
        {
          "SubmissionType",
          (object) info.SubmissionType
        },
        {
          "PlanNum",
          (object) info.PlanNum
        },
        {
          "PassThruRate",
          (object) info.PassThruRate
        },
        {
          "ForecloseRiskCode",
          (object) info.ForecloseRiskCode
        },
        {
          "MbsMargin",
          (object) info.MbsMargin
        },
        {
          "ContractType",
          (object) info.ContractType
        },
        {
          "DeliveryRegion",
          (object) info.DeliveryRegion
        },
        {
          "InterestOnlyEndDate",
          (object) info.InterestOnlyEndDate
        },
        {
          "IsMultiFamily",
          (object) info.IsMultiFamily,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "NoteCustodian",
          (object) info.NoteCustodian
        },
        {
          "FinancialInstNum",
          (object) info.FinancialInstNum
        },
        {
          "StandardLookback",
          (object) info.StandardLookback
        },
        {
          "IsGuarantyFeeAddOn",
          (object) info.IsGuarantyFeeAddOn,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "InvestorRemittanceType",
          (object) info.InvestorRemittanceType
        },
        {
          "SellerId",
          (object) info.SellerId
        },
        {
          "MinServicingFee",
          (object) info.MinServicingFee
        },
        {
          "MaxBU",
          (object) info.MaxBU
        },
        {
          "IssuerNum",
          (object) info.IssuerNum
        },
        {
          "IssueType",
          (object) info.IssueType
        },
        {
          "ARMIndex",
          (object) info.ARMIndex
        },
        {
          "CertAgreement",
          (object) info.CertAgreement
        },
        {
          "PnICustodialABA",
          (object) info.PnICustodialABA
        },
        {
          "SubscriberRecordABA",
          (object) info.SubscriberRecordABA
        },
        {
          "SubscriberRecordFRBPosDesc",
          (object) info.SubscriberRecordFRBPosDesc
        },
        {
          "SubscriberRecordFRBAcctDesc",
          (object) info.SubscriberRecordFRBAcctDesc
        },
        {
          "MasterTnIABA",
          (object) info.MasterTnIABA
        },
        {
          "NewTransferIssuerNum",
          (object) info.NewTransferIssuerNum
        },
        {
          "MaturityDate",
          (object) info.MaturityDate
        },
        {
          "PoolTaxID",
          (object) info.PoolTaxID
        },
        {
          "ChangeDate",
          (object) info.ChangeDate
        },
        {
          "IsBondFinancePool",
          (object) info.IsBondFinancePool,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "IsSent1711ToCustodian",
          (object) info.IsSent1711ToCustodian,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "PnICustodialAcctNum",
          (object) info.PnICustodialAcctNum
        },
        {
          "SubscriberRecordAcctNum",
          (object) info.SubscriberRecordAcctNum
        },
        {
          "MasterTnIAcctNum",
          (object) info.MasterTnIAcctNum
        },
        {
          "SubservicerIssuerNum",
          (object) info.SubservicerIssuerNum
        },
        {
          "GinniePoolType",
          (object) info.GinniePoolType
        },
        {
          "InitialDate",
          (object) info.InitialDate
        },
        {
          "LastPaidInstallmentDate",
          (object) info.LastPaidInstallmentDate
        },
        {
          "UnpaidBalanceDate",
          (object) info.UnpaidBalanceDate
        },
        {
          "ACHBankAccountPurposeType",
          (object) info.ACHBankAccountPurposeType
        },
        {
          "ACHABARoutingTransitNum",
          (object) info.ACHABARoutingAndTransitNum
        },
        {
          "ACHABARoutingTransitId",
          (object) info.ACHABARoutingAndTransitId
        },
        {
          "ACHInstitTelName",
          (object) info.ACHInsitutionTelegraphicName
        },
        {
          "ACHReceiverSubacctName",
          (object) info.ACHReceiverSubaccountName
        },
        {
          "ACHBankAccountDesc",
          (object) info.ACHBankAccountDescription
        },
        {
          "GinnieIndexType",
          (object) info.GinniePoolIndexType
        },
        {
          "PoolIssuerTransferee",
          (object) info.PoolIssuerTransferee
        },
        {
          "PoolCertPaymentDate",
          (object) info.PoolCertificatePaymentDate
        },
        {
          "BondFinProgType",
          (object) info.BondFinanceProgramType
        },
        {
          "BondFinProgName",
          (object) info.BondFinanceProgramName
        },
        {
          "PoolClassType",
          (object) info.GinniePoolClassType
        },
        {
          "PoolConcurrTransferIndc",
          (object) info.GinniePoolConcurrentTransferIndicator,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "PoolCurrentLoanCount",
          (object) info.PoolCurrentLoanCount
        },
        {
          "PoolCurrentPrinBal",
          (object) info.PoolCurrentPrincipalBalAmt
        },
        {
          "PoolingMethodType",
          (object) info.PoolingMethodType
        },
        {
          "PoolInterestAdjEffectiveDate",
          (object) info.PoolInterestAdjustmentEffectiveDate
        },
        {
          "PoolMaturityPeriodCount",
          (object) info.PoolMaturityPeriodCount
        },
        {
          "DocSubmissionIndic",
          (object) info.DocSubmissionIndicator,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "DocReqIndic",
          (object) info.DocReqIndicator,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "SecurityOriginalSubscrAmt",
          (object) info.SecurityOrigSubscriptionAmt
        },
        {
          "LastModified",
          (object) "GetDate()",
          (IDbEncoder) DbEncoding.None
        }
      };
    }

    public static int AddTradeHistoryItem(MbsPoolHistoryItem item)
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
      MbsPoolInfo info,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool checkStatus)
    {
      MbsPoolInfo trade = MbsPools.GetTrade(info.TradeID);
      if (trade == null)
        throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) info.TradeID);
      if (checkStatus && (trade.Status == TradeStatus.Pending || trade.Status == TradeStatus.Archived))
        throw new TradeNotUpdateException(MbsPools.UPDATETRADEERROR);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (MbsPools));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      DbValue key = new DbValue("TradeID", (object) info.TradeID);
      dbQueryBuilder.DeleteFrom(table3, key);
      dbQueryBuilder.Update(table1, TradeUtils.CreateTradesValueList((TradeInfoObj) info), key);
      dbQueryBuilder.Update(table2, MbsPools.createMbsPoolValueList(info), key);
      DbValueList objectsValueList = MbsPools.createTradeObjectsValueList(info);
      objectsValueList.Add(key);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
      if (info.Locked != trade.Locked)
        MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, info.Locked ? TradeHistoryAction.TradeLocked : TradeHistoryAction.TradeUnlocked, currentUser));
      if (info.Status != trade.Status)
        MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, info.Status, currentUser));
      if (info.ContractID != trade.ContractID)
      {
        if (trade.ContractID > 0)
        {
          MasterContractSummaryInfo contract = (MasterContractSummaryInfo) MbsPools.GetContract(trade.ContractID);
          if (contract != null)
            MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, contract, TradeHistoryAction.ContractUnassigned, currentUser));
        }
        if (info.ContractID > 0)
        {
          MasterContractSummaryInfo contract = (MasterContractSummaryInfo) MbsPools.GetContract(info.ContractID);
          if (contract != null)
            MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(info, contract, TradeHistoryAction.ContractAssigned, currentUser));
        }
      }
      if (MbsPoolInfo.ComparePricing(info, trade))
        return;
      MbsPools.RecalculateProfits(info, isExternalOrganization);
    }

    public static void RecalculateProfits(MbsPoolInfo trade, bool isExternalOrganization)
    {
      PipelineInfo[] assignedLoans = MbsPools.GetAssignedLoans(trade, MbsPools.GeneratePricingFieldList(trade), isExternalOrganization);
      MbsPools.RecalculateProfits(trade, assignedLoans);
    }

    public static string[] GeneratePricingFieldList(MbsPoolInfo trade)
    {
      return MbsPools.GeneratePricingFieldList((IEnumerable<MbsPoolInfo>) new MbsPoolInfo[1]
      {
        trade
      });
    }

    public static string[] GeneratePricingFieldList(IEnumerable<MbsPoolInfo> trades)
    {
      List<string> stringList = new List<string>();
      foreach (MbsPoolInfo trade in trades)
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
      MbsPoolInfo trade,
      string[] fields,
      bool isExternalOrganization)
    {
      return Pipeline.Generate("select LoanGuid from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid  where TradeAssignment.TradeID = " + (object) trade.TradeID + " and TradeAssignment.Status >= " + (object) 2, fields, PipelineData.Trade, isExternalOrganization, TradeType.MbsPool);
    }

    public static void RecalculateProfits(MbsPoolInfo trade, PipelineInfo[] pinfos)
    {
      ChunkedList<PipelineInfo> chunkedList = new ChunkedList<PipelineInfo>(pinfos, 50);
      PipelineInfo[] pipelineInfoArray;
      while ((pipelineInfoArray = chunkedList.Next()) != null)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        for (int index = 0; index < pipelineInfoArray.Length; ++index)
        {
          Decimal num = trade.Pricing.IsAdvancedPricing ? trade.CalculateProfit(pipelineInfoArray[index], trade.WeightedAvgPrice) : trade.CalculateProfit(pipelineInfoArray[index], 0M);
          dbQueryBuilder.Append("update TradeAssignment set Profit = " + num.ToString("0.00") + " where TradeID = " + (object) trade.TradeID + " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) pipelineInfoArray[index].GUID));
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static MbsPoolInfo GetTradeByName(string tradeName)
    {
      MbsPoolInfo[] tradesFromDatabase = MbsPools.getTradesFromDatabase("select TradeID from Trades where Name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(tradeName) + "'");
      return tradesFromDatabase.Length == 0 ? (MbsPoolInfo) null : tradesFromDatabase[0];
    }

    private static DbValueList createTradeObjectsValueList(MbsPoolInfo info)
    {
      return new DbValueList()
      {
        {
          "FilterQueryXml",
          info.Filter == null ? (object) (string) null : (object) info.Filter.ToXml()
        },
        {
          "InvestorXml",
          (object) info.Investor.ToXml()
        },
        {
          "PricingXml",
          (object) info.Pricing.ToXml()
        },
        {
          "AdjustmentsXml",
          (object) info.PriceAdjustments.ToXml()
        },
        {
          "DealerXml",
          (object) info.Dealer.ToXml()
        },
        {
          "AssigneeXml",
          (object) info.Assignee.ToXml()
        },
        {
          "Notes",
          (object) info.Notes
        },
        {
          "BuyUpDownXml",
          (object) info.BuyUpDownItems.ToXml()
        },
        {
          "ProductNameXml",
          (object) info.ProductNames.ToXml()
        }
      };
    }

    private static Dictionary<MbsPoolLoanStatus, int> dataRowsToPendingLoanCounts(IEnumerable rows)
    {
      Dictionary<MbsPoolLoanStatus, int> pendingLoanCounts = new Dictionary<MbsPoolLoanStatus, int>();
      foreach (DataRow row in rows)
        pendingLoanCounts[(MbsPoolLoanStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["PendingStatus"])] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["LoanCount"]);
      return pendingLoanCounts;
    }

    private static MbsPoolHistoryItem dataRowToHistoryItem(DataRow r)
    {
      return new MbsPoolHistoryItem(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["HistoryID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), (TradeHistoryAction) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Action"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Timestamp"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UserID"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["DataXml"]));
    }

    public static MasterContractInfo GetContract(int contractId)
    {
      MasterContractSummaryInfo[] contractsFromDatabase = MbsPools.getContractsFromDatabase(string.Concat((object) contractId), true, false);
      return contractsFromDatabase.Length == 0 ? (MasterContractInfo) null : (MasterContractInfo) contractsFromDatabase[0];
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      int[] tradeIds,
      string[] fields,
      bool isExternalOrganization)
    {
      return MbsPools.GetEligibleLoans(user, MbsPools.GetTrades(tradeIds), fields, (SortField[]) null, isExternalOrganization);
    }

    public static PipelineInfo[] GetEligibleLoans(
      UserInfo user,
      MbsPoolInfo[] trades,
      string[] fields,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      QueryCriterion queryCriterion1 = (QueryCriterion) null;
      foreach (MbsPoolInfo trade in trades)
      {
        if (trade != null && trade.Filter != null)
        {
          QueryCriterion queryCriterion2 = ((TradeFilterEvaluator) trade.Filter.CreateEvaluator(typeof (MbsPoolFilterEvaluator))).ToQueryCriterion((TradeInfoObj) trade, filterOption);
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
      PipelineInfo[] pinfos = Pipeline.Generate(user, LoanInfo.Right.Access, fields, PipelineData.Trade, queryCriterion1, ((IEnumerable<SortField>) sortOrder).Where<SortField>((System.Func<SortField, bool>) (s => !s.Term.FieldName.StartsWith("TradeEligibility"))).ToArray<SortField>(), isExternalOrganization, TradeType.MbsPool);
      foreach (PipelineInfo pinfo in pinfos)
        MbsPools.PopulateTradeProfitData(pinfo, trades);
      if (flag)
        MbsPools.SortByCalculatedData(pinfos, sortOrder);
      return pinfos;
    }

    private static void SortByCalculatedData(PipelineInfo[] pinfos, SortField[] sortFields)
    {
      foreach (SortField sortOrder in ((IEnumerable<SortField>) sortFields).Where<SortField>((System.Func<SortField, bool>) (sortField => sortField.Term.FieldName.StartsWith("TradeEligibility"))))
        Array.Sort((Array) pinfos, (IComparer) new LoanTrades.TradeLoanCursorItemComparer(sortOrder));
    }

    public static MbsPoolInfo[] GetTrades(int[] tradeIds)
    {
      return MbsPools.getTradesFromDatabase(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds));
    }

    public static void PopulateTradeProfitData(PipelineInfo pinfo, MbsPoolInfo[] trades)
    {
      PipelineInfo.TradeInfo[] tradeInfoArray = new PipelineInfo.TradeInfo[trades.Length];
      for (int index = 0; index < trades.Length; ++index)
      {
        PipelineInfo.TradeInfo tradeInfo = new PipelineInfo.TradeInfo();
        tradeInfo.TradeID = trades[index].TradeID;
        Decimal num = trades[index].Pricing.IsAdvancedPricing ? trades[index].CalculateProfit(pinfo, trades[index].WeightedAvgPrice) : trades[index].CalculateProfit(pinfo, 0M);
        tradeInfo.Profit = num;
        tradeInfoArray[index] = tradeInfo;
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("Profit")] = (object) tradeInfo.Profit;
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("NetPrice")] = (object) trades[index].CalculatePriceIndex(pinfo, false, trades[index].WeightedAvgPrice);
        pinfo.Info[(object) trades[index].CreateEligiblityDataKey("SRP")] = (object) trades[index].SRPTable.GetAdjustmentForLoan(pinfo);
        if (trades[index].Pricing.IsAdvancedPricing)
          pinfo.Info[(object) trades[index].CreateEligiblityDataKey("TotalPrice")] = (object) trades[index].CalculatePriceIndex(pinfo, trades[index].WeightedAvgPrice);
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
      StringBuilder stringBuilder = new StringBuilder(MbsPools.getQueryTradeIdsSql(user, criteria, sortOrder, isExternalOrganization));
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
        string fieldList = "MbsPoolDetails.TradeID";
        MbsPoolQuery mbsPoolQuery = new MbsPoolQuery(user);
        return MbsPools.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) mbsPoolQuery, isExternalOrganization).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(MbsPools.className, ex);
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
      querySql.Append(MbsPools.OrderbySettlementMonth(queryEngine, sortFields));
      return querySql;
    }

    public static Dictionary<int, MbsPoolViewModel> GetTradeViews(int[] tradeIds)
    {
      List<MbsPoolViewModel> mbsPoolViewModelList = new List<MbsPoolViewModel>();
      foreach (int tradeId in tradeIds)
        mbsPoolViewModelList.AddRange((IEnumerable<MbsPoolViewModel>) MbsPools.getTradeViewsFromDatabase(string.Concat((object) tradeId)));
      Dictionary<int, MbsPoolViewModel> tradeViews = new Dictionary<int, MbsPoolViewModel>();
      foreach (MbsPoolViewModel mbsPoolViewModel in mbsPoolViewModelList.ToArray())
      {
        if (mbsPoolViewModel != null)
          tradeViews[mbsPoolViewModel.TradeID] = mbsPoolViewModel;
      }
      for (int key = 0; key < tradeIds.Length; ++key)
      {
        if (!tradeViews.ContainsKey(tradeIds[key]))
          tradeViews[key] = (MbsPoolViewModel) null;
      }
      return tradeViews;
    }

    public static MbsPoolViewModel GetTradeView(int tradeId)
    {
      MbsPoolViewModel[] viewsFromDatabase = MbsPools.getTradeViewsFromDatabase(string.Concat((object) tradeId));
      return viewsFromDatabase.Length == 0 ? (MbsPoolViewModel) null : viewsFromDatabase[0];
    }

    public static void DeleteTrade(int tradeId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table1 = DbAccessManager.GetTable("TradeAssignmentByTrade");
        DbTableInfo table2 = DbAccessManager.GetTable(nameof (MbsPools));
        DbTableInfo table3 = DbAccessManager.GetTable("TradeHistory");
        DbValue key = new DbValue("TradeID", (object) tradeId);
        dbQueryBuilder.AppendLine("delete TradeAssignmentByTrade where AssigneeTradeID = " + (object) tradeId);
        dbQueryBuilder.DeleteFrom(table1, key);
        dbQueryBuilder.DeleteFrom(table2, key);
        dbQueryBuilder.DeleteFrom(table3, key);
        dbQueryBuilder.ExecuteNonQuery();
        TradeUtils.DeleteTrade(tradeId);
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(MbsPools.UPDATETRADEERROR);
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
        foreach (MbsPoolInfo trade in MbsPools.GetTrades(tradeIds))
          MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(trade, action, currentUser));
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(MbsPools.UPDATETRADEERROR);
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
        MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(MbsPools.GetTrade(tradeId), status, currentUser));
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("UPDATETRADEERROR") >= 0)
          throw new TradeNotUpdateException(MbsPools.UPDATETRADEERROR);
        throw;
      }
    }

    public static MbsPoolViewModel GetTradeViewForLoan(string guid)
    {
      MbsPoolViewModel[] viewsFromDatabase = MbsPools.getTradeViewsFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, MbsPools.TheTradeType));
      return viewsFromDatabase.Length == 0 ? (MbsPoolViewModel) null : viewsFromDatabase[0];
    }

    public static MbsPoolInfo GetTradeForLoan(string guid)
    {
      MbsPoolInfo[] tradesFromDatabase = MbsPools.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " And TradeAssignment.Status >= " + (object) 2, MbsPools.TheTradeType));
      return tradesFromDatabase.Length == 0 ? (MbsPoolInfo) null : tradesFromDatabase[0];
    }

    public static MbsPoolInfo GetTradeForRejectedLoan(string guid)
    {
      MbsPoolInfo[] tradesFromDatabase = MbsPools.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, "and isnull(Rejected, 0) = 1 and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), MbsPools.TheTradeType));
      return tradesFromDatabase.Length == 0 ? (MbsPoolInfo) null : tradesFromDatabase[0];
    }

    public static MbsPoolViewModel[] GetActiveTradeView()
    {
      return MbsPools.getTradeViewsFromDatabase("select TradeID from Trades where Status <> " + (object) 5);
    }

    public static MbsPoolViewModel[] GetTradeViewsByContractID(int contractId)
    {
      return MbsPools.getTradeViewsFromDatabase("select TradeID from Trades where ContractID = " + (object) contractId);
    }

    public static MbsPoolInfo[] GetMbsPoolsByContractID(int contractId)
    {
      return MbsPools.getTradesFromDatabase("select TradeID from Trades where ContractID = " + (object) contractId);
    }

    public static void DeleteLoanFromTrades(
      string loanGuid,
      UserInfo currentUser,
      bool isExternalOrganization)
    {
      MbsPoolInfo tradeForLoan = MbsPools.GetTradeForLoan(loanGuid);
      if (tradeForLoan != null)
      {
        PipelineInfo pipelineInfo = Pipeline.GetPipelineInfo((UserInfo) null, loanGuid, MbsPoolHistoryItem.RequiredPipelineInfoFields, PipelineData.Fields, isExternalOrganization);
        MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(tradeForLoan, pipelineInfo, MbsPoolLoanStatus.Unassigned, currentUser));
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete ta from TradeAssignment ta inner join Trades t on t.TradeID = ta.TradeID where ta.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid) + " and t.TradeType = " + (object) 3);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      MbsPoolLoanStatus[] statuses,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      MbsPools.SetPendingTradeStatus(tradeId, loanIds, statuses, (string[]) null, currentUser, isExternalOrganization, removePendingLoan);
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      MbsPoolLoanStatus[] statuses,
      string[] comments,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      MbsPools.SetPendingTradeStatus(tradeId, loanIds, statuses, (string[]) null, (string[]) null, (string[]) null, currentUser, isExternalOrganization, removePendingLoan);
    }

    public static void SetPendingTradeStatus(
      int tradeId,
      string[] loanIds,
      MbsPoolLoanStatus[] statuses,
      string[] comments,
      string[] commitmentContractNumber,
      string[] productName,
      UserInfo currentUser,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      bool flag = false;
      MbsPoolViewModel tradeView = MbsPools.GetTradeView(tradeId);
      PipelineInfo[] pipelineInfoArray = Pipeline.Generate((UserInfo) null, loanIds, MbsPoolHistoryItem.RequiredPipelineInfoFields, PipelineData.Fields, isExternalOrganization, tradeType: TradeType.MbsPool);
      for (int index = 0; index < loanIds.Length; ++index)
      {
        if (comments == null)
        {
          if (commitmentContractNumber != null)
            flag |= MbsPools.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], commitmentContractNumber[index], productName[index], currentUser, removePendingLoan);
          else
            flag |= MbsPools.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], currentUser, removePendingLoan);
        }
        else if (commitmentContractNumber != null)
          flag |= MbsPools.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], comments[index], commitmentContractNumber[index], productName[index], currentUser, removePendingLoan);
        else
          flag |= MbsPools.setPendingTradeStatus(tradeView, pipelineInfoArray[index], statuses[index], comments[index], currentUser, removePendingLoan);
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
      MbsPools.RecalculateProfits(tradeId, false);
    }

    private static bool setPendingTradeStatus(
      MbsPoolViewModel trade,
      PipelineInfo loanInfo,
      MbsPoolLoanStatus loanStatus,
      UserInfo currentUser,
      bool removePendingLoan)
    {
      return MbsPools.setPendingTradeStatus(trade, loanInfo, loanStatus, "", currentUser, removePendingLoan);
    }

    private static bool setPendingTradeStatus(
      MbsPoolViewModel trade,
      PipelineInfo loanInfo,
      MbsPoolLoanStatus loanStatus,
      string commitmentContractNumber,
      string productName,
      UserInfo currentUser,
      bool removePendingLoan)
    {
      return MbsPools.setPendingTradeStatus(trade, loanInfo, loanStatus, "", commitmentContractNumber, productName, currentUser, removePendingLoan);
    }

    private static bool setPendingTradeStatus(
      MbsPoolViewModel trade,
      PipelineInfo loanInfo,
      MbsPoolLoanStatus loanStatus,
      string comment,
      UserInfo currentUser,
      bool removePendingLoan)
    {
      return MbsPools.setPendingTradeStatus(trade, loanInfo, loanStatus, comment, "", "", currentUser, removePendingLoan);
    }

    private static bool setPendingTradeStatus(
      MbsPoolViewModel trade,
      PipelineInfo loanInfo,
      MbsPoolLoanStatus loanStatus,
      string comment,
      string commitmentContractNumber,
      string productName,
      UserInfo currentUser,
      bool removePendingLoan)
    {
      if (loanStatus == MbsPoolLoanStatus.None)
        return false;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) trade.TradeID);
      dbValueList.Add("LoanGuid", (object) loanInfo.GUID);
      DbValueList values = new DbValueList();
      values.Add("PendingStatus", (object) (int) loanStatus);
      values.Add("PendingStatusDate", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
      if (commitmentContractNumber != null)
        values.Add("CommitmentContractNumber", (object) commitmentContractNumber);
      else
        values.Add("CommitmentContractNumber", (object) string.Empty);
      if (productName != null)
        values.Add("ProductName", (object) productName);
      else
        values.Add("ProductName", (object) string.Empty);
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
      if (loanStatus != MbsPoolLoanStatus.Unassigned)
      {
        dbQueryBuilder.AppendLine(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID, Trades.TradeType", (string) null, " and TradeAssignment.Status > " + (object) 1 + " and TradeAssignment.TradeID <> " + (object) trade.TradeID + " And LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID), MbsPools.SellSideTradeTypes));
        dbQueryBuilder.AppendLine("update TradeAssignment set PendingStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID");
        dbQueryBuilder.AppendLine(" where TradeAssignment.TradeID <> " + (object) trade.TradeID);
        dbQueryBuilder.AppendLine(" and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID));
        dbQueryBuilder.AppendLine(" and Trades.TradeType in (" + (object) 3 + ", " + (object) 2 + ")");
      }
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        throw new Exception("The loan has been assigned to another trade or pool.");
      bool flag = string.Concat(dataSet.Tables[0].Rows[0][0]) == "1";
      if (loanStatus != MbsPoolLoanStatus.Unassigned)
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
        MbsPoolInfo trade3 = MbsPools.GetTrade(trade.TradeID);
        if (flag && loanStatus != MbsPoolLoanStatus.Assigned)
          MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(trade3, loanInfo, MbsPoolLoanStatus.Assigned, currentUser));
        MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(trade3, loanInfo, loanStatus, comment, currentUser));
      }
      return flag;
    }

    public static void RecalculateProfits(int tradeId, bool isExternalOrganization)
    {
      MbsPools.RecalculateProfits(MbsPools.GetTrade(tradeId) ?? throw new ObjectNotFoundException("Trade " + (object) tradeId + " does not exist", ObjectType.Trade, (object) tradeId), isExternalOrganization);
    }

    public static void CommitPendingTradeStatus(
      int tradeId,
      string loanId,
      MbsPoolLoanStatus loanStatus)
    {
      MbsPools.CommitPendingTradeStatus(tradeId, loanId, loanStatus, false);
    }

    public static void CommitPendingTradeStatus(
      int tradeId,
      string loanId,
      MbsPoolLoanStatus loanStatus,
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
      dbQueryBuilder.End();
      if (loanStatus >= MbsPoolLoanStatus.Assigned)
      {
        dbQueryBuilder.AppendLine("delete from TradeAssignment ");
        dbQueryBuilder.AppendLine(" where PendingStatus = " + (object) 1);
        dbQueryBuilder.AppendLine(" and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId));
        dbQueryBuilder.AppendLine(" and TradeID <> " + (object) tradeId);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static MbsPoolHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      return MbsPools.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
    }

    public static void RecalculateProfitFromLoan(string loanGuid, bool isExternalOrganization)
    {
      MbsPoolInfo[] loanTradesForLoan = MbsPools.GetAllPendingLoanTradesForLoan(loanGuid);
      string[] pricingFieldList = MbsPools.GeneratePricingFieldList((IEnumerable<MbsPoolInfo>) loanTradesForLoan);
      PipelineInfo pipelineInfo = Pipeline.GetPipelineInfo((UserInfo) null, loanGuid, pricingFieldList, PipelineData.Fields, isExternalOrganization);
      if (pipelineInfo == null)
        return;
      MbsPools.RecalculateProfits(loanTradesForLoan, pipelineInfo);
    }

    public static MbsPoolInfo[] GetAllPendingLoanTradesForLoan(string guid)
    {
      return MbsPools.getTradesFromDatabase(TradeAssignments.BuildSelectSql("TradeAssignment.TradeID", (string) null, " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid), MbsPools.TheTradeType));
    }

    public static string[] GeneratePricingFieldList(int[] tradeIds)
    {
      return MbsPools.GeneratePricingFieldList((IEnumerable<MbsPoolInfo>) MbsPools.GetTrades(tradeIds));
    }

    public static void ApplyTradeStatusFromLoan(PipelineInfo pinfo)
    {
      string tradeGuid = string.Concat(pinfo.GetField("TradeGuid"));
      string str = string.Concat(pinfo.GetField("InvestorStatus"));
      DateTime date = Utils.ParseDate(pinfo.GetField("InvestorStatusDate"), DateTime.Now);
      if (tradeGuid == "")
      {
        MbsPools.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, MbsPoolLoanStatus.Unassigned, date);
      }
      else
      {
        switch (str)
        {
          case "Purchased":
            MbsPools.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, MbsPoolLoanStatus.Purchased, date);
            break;
          case "Shipped":
            MbsPools.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, MbsPoolLoanStatus.Shipped, date);
            break;
          default:
            MbsPools.SetTradeAssignmentStatus(pinfo.GUID, tradeGuid, MbsPoolLoanStatus.Assigned, date);
            break;
        }
      }
    }

    public static void SetTradeAssignmentStatus(
      string loanId,
      string tradeGuid,
      MbsPoolLoanStatus loanStatus,
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
      if (loanStatus == MbsPoolLoanStatus.Unassigned)
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

    public static void RecalculateProfits(MbsPoolInfo[] trades, PipelineInfo pinfo)
    {
      ChunkedList<MbsPoolInfo> chunkedList = new ChunkedList<MbsPoolInfo>(trades, 50);
      MbsPoolInfo[] mbsPoolInfoArray;
      while ((mbsPoolInfoArray = chunkedList.Next()) != null)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        for (int index = 0; index < mbsPoolInfoArray.Length; ++index)
        {
          Decimal num = mbsPoolInfoArray[index].Pricing.IsAdvancedPricing ? mbsPoolInfoArray[index].CalculateProfit(pinfo, mbsPoolInfoArray[index].WeightedAvgPrice) : mbsPoolInfoArray[index].CalculateProfit(pinfo, 0M);
          dbQueryBuilder.Append("update TradeAssignment set Profit = " + num.ToString("0.00") + " where TradeID = " + (object) mbsPoolInfoArray[index].TradeID + " and LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) pinfo.GUID));
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
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
          str1 = str1.Substring(0, str1.IndexOf("[MbsPoolDetails].[SettlementMonth]")) + "case [MbsPoolDetails].[SettlementMonth] " + str4 + str1.Substring(str1.IndexOf(str2) + str2.Length + 2);
        }
      }
      return str1;
    }

    public static void SetCommitmentInfo(
      int tradeId,
      string[] loanIds,
      string[] commitmentContractNumber,
      string[] productName,
      UserInfo currentUser,
      bool isExternalOrganization)
    {
      MbsPoolViewModel tradeView = MbsPools.GetTradeView(tradeId);
      PipelineInfo[] pipelineInfoArray = Pipeline.Generate((UserInfo) null, loanIds, MbsPoolHistoryItem.RequiredPipelineInfoFields, PipelineData.Fields, isExternalOrganization, tradeType: TradeType.MbsPool);
      for (int index = 0; index < loanIds.Length; ++index)
        MbsPools.SetCommitmentInfo(tradeView, pipelineInfoArray[index], commitmentContractNumber[index], productName[index], currentUser);
    }

    private static void SetCommitmentInfo(
      MbsPoolViewModel trade,
      PipelineInfo loanInfo,
      string commitmentContractNumber,
      string productName,
      UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeAssignment");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) trade.TradeID);
      dbValueList.Add("LoanGuid", (object) loanInfo.GUID);
      DbValueList values = new DbValueList();
      if (commitmentContractNumber != null)
        values.Add("CommitmentContractNumber", (object) commitmentContractNumber);
      else
        values.Add("CommitmentContractNumber", (object) string.Empty);
      if (productName != null)
        values.Add("ProductName", (object) productName);
      else
        values.Add("ProductName", (object) string.Empty);
      string format = "\r\n                if exists (select 1\r\n                    from TradeAssignment ta\r\n                    inner join trades t on ta.tradeid = t.tradeid\r\n                    where ta.TradeID <> {0} And LoanGuid = {1}\r\n                    and TradeType in (2, 3)\r\n                    and ta.AssignedStatus >= {2}\r\n                )\r\n                return";
      dbQueryBuilder.AppendLine(string.Format(format, (object) trade.TradeID, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loanInfo.GUID), (object) 2));
      dbQueryBuilder.IfExists(table, dbValueList);
      dbQueryBuilder.Begin();
      dbQueryBuilder.Update(table, values, dbValueList);
      dbQueryBuilder.AppendLine("select 0 as NewRow");
      dbQueryBuilder.End();
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        throw new Exception("The loan has been assigned to another trade or pool.");
    }

    public static bool IsTradeAssignmentUpdateCompleted(
      int tradeId,
      string loanGuid,
      MbsPoolLoanStatus status)
    {
      string text = string.Empty;
      if (status == MbsPoolLoanStatus.Unassigned)
        text = string.Format("SELECT * \r\n                              FROM TradeAssignment\r\n                              WHERE TradeID = {0}\r\n                              AND LoanGuid = '{1}'", (object) tradeId, (object) loanGuid);
      else if (status >= MbsPoolLoanStatus.Assigned)
        text = string.Format("SELECT * \r\n                              FROM TradeAssignment\r\n                              WHERE TradeID = {0}\r\n                              AND LoanGuid = '{1}'\r\n                              AND AssignedStatus >= 2\r\n                              AND PendingStatus = 0", (object) tradeId, (object) loanGuid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return status == MbsPoolLoanStatus.Unassigned ? dataRowCollection == null || dataRowCollection.Count <= 0 : status >= MbsPoolLoanStatus.Assigned && dataRowCollection != null && dataRowCollection.Count > 0;
    }

    public static TradeStatus GetTradeStatus(int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select MbsPoolDetails.Status from MbsPoolDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on MbsPoolDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine(" where MbsPoolDetails.TradeID =" + (object) tradeId);
      return (TradeStatus) Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
    }

    public static MbsPoolMortgageType GetMbsPoolMortgageType(string tradeGuid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select PoolMortgageType from MbsPoolDetails where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tradeGuid));
      return (MbsPoolMortgageType) Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
    }
  }
}
