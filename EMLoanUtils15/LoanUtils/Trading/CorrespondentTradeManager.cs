// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Trading.CorrespondentTradeManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Trading
{
  public class CorrespondentTradeManager
  {
    private const string className = "CorrespondentTradeManager�";
    protected static string sw = Tracing.SwOutsideLoan;

    public static CorrespondentTradeInfo CreateCorrespondentTrade(
      Hashtable requestSnapshot,
      LoanData loanData,
      SessionObjects sessionObjects,
      bool isAutoCT)
    {
      CorrespondentTradeInfo correspondentTrade = new CorrespondentTradeInfo();
      try
      {
        CorrespondentMasterDeliveryType deliveryType = Utils.GetEnumValueFromDescription<CorrespondentMasterDeliveryType>(requestSnapshot[(object) "3911"].ToString());
        int num1 = 0;
        string autoCreateTradeName = sessionObjects.CorrespondentTradeManager.GetNextAutoCreateTradeName(loanData.GetField("364"), loanData.GetField("GUID"));
        bool boolean1 = Utils.ParseBoolean(sessionObjects.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentTrade"]);
        ExternalOriginatorManagementData originatorManagementData = sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(loanData.GetField("TPO.X15")).FirstOrDefault<ExternalOriginatorManagementData>();
        if (originatorManagementData != null)
          num1 = originatorManagementData.oid;
        if (originatorManagementData == null)
          return (CorrespondentTradeInfo) null;
        if (boolean1)
        {
          if (((!(autoCreateTradeName != string.Empty) ? 0 : (autoCreateTradeName.Length <= 21 ? 1 : 0)) | (isAutoCT ? 1 : 0)) != 0)
          {
            if (deliveryType != CorrespondentMasterDeliveryType.IndividualBestEfforts)
            {
              if (deliveryType != CorrespondentMasterDeliveryType.IndividualMandatory)
                goto label_19;
            }
            CorrespondentTradeInfo correspondentTradeInfo = new CorrespondentTradeInfo();
            correspondentTradeInfo.Name = autoCreateTradeName;
            bool boolean2 = Utils.ParseBoolean(sessionObjects.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentMaster"]);
            correspondentTradeInfo.CommitmentDate = Utils.ParseDate((object) requestSnapshot[(object) "2149"].ToString());
            correspondentTradeInfo.CommitmentType = deliveryType == CorrespondentMasterDeliveryType.IndividualMandatory ? CorrespondentTradeCommitmentType.Mandatory : CorrespondentTradeCommitmentType.BestEfforts;
            correspondentTradeInfo.CompanyName = loanData.GetField("TPO.X14");
            correspondentTradeInfo.TPOID = loanData.GetField("TPO.X15");
            correspondentTradeInfo.OrganizationID = loanData.GetField("TPO.X16");
            correspondentTradeInfo.ExternalOriginatorManagementID = num1;
            correspondentTradeInfo.DeliveryType = deliveryType;
            correspondentTradeInfo.TradeAmount = Utils.ParseDecimal((object) loanData.GetField("2"), 0M);
            correspondentTradeInfo.ExpirationDate = Utils.ParseDate((object) requestSnapshot[(object) "2151"].ToString());
            correspondentTradeInfo.DeliveryExpirationDate = Utils.ParseDate((object) requestSnapshot[(object) "2151"].ToString());
            correspondentTradeInfo.AutoCreated = true;
            correspondentTradeInfo.AutoCreateLoanGUID = loanData.GetField("GUID");
            if (boolean2)
            {
              List<CorrespondentMasterInfo> list = ((IEnumerable<CorrespondentMasterInfo>) sessionObjects.CorrespondentTradeManager.GetCorrespondentMasterInfos(correspondentTradeInfo)).Where<CorrespondentMasterInfo>((Func<CorrespondentMasterInfo, bool>) (x => x.DeliveryInfos.Where<MasterCommitmentDeliveryInfo>((Func<MasterCommitmentDeliveryInfo, bool>) (y => y.Type == deliveryType)).ToList<MasterCommitmentDeliveryInfo>().Count >= 1)).ToList<CorrespondentMasterInfo>();
              CorrespondentMasterInfo correspondentMasterInfo = list.Count == 1 ? list[0] : (CorrespondentMasterInfo) null;
              if (correspondentMasterInfo != null)
              {
                bool flag = true;
                if ((originatorManagementData.CommitmentUseBestEffortLimited || deliveryType != CorrespondentMasterDeliveryType.IndividualBestEfforts) && CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(correspondentMasterInfo.CommitmentAmount, originatorManagementData.CommitmentUseBestEffortLimited, sessionObjects.CorrespondentTradeManager.GetTradeInfosByMasterId(correspondentMasterInfo.ID)) < correspondentTradeInfo.TradeAmount)
                  flag = false;
                if (flag)
                {
                  correspondentTradeInfo.Tolerance = correspondentMasterInfo.DeliveryInfos.Where<MasterCommitmentDeliveryInfo>((Func<MasterCommitmentDeliveryInfo, bool>) (x => x.Type == deliveryType)).FirstOrDefault<MasterCommitmentDeliveryInfo>().Tolerance;
                  int deliveryDays = correspondentMasterInfo.DeliveryInfos.Where<MasterCommitmentDeliveryInfo>((Func<MasterCommitmentDeliveryInfo, bool>) (x => x.Type == deliveryType)).FirstOrDefault<MasterCommitmentDeliveryInfo>().DeliveryDays;
                  correspondentTradeInfo.DeliveryExpirationDate = correspondentTradeInfo.ExpirationDate.AddDays((double) deliveryDays);
                  correspondentTradeInfo.CorrespondentMasterCommitmentNumber = correspondentMasterInfo.Name;
                  correspondentTradeInfo.CorrespondentMasterID = correspondentMasterInfo.ID;
                }
              }
            }
            Decimal num2 = Utils.ParseDecimal((object) loanData.GetField("3"), 0M);
            correspondentTradeInfo.Pricing.IsAdvancedPricing = false;
            SimpleTradeFilter simpleFilter = new SimpleTradeFilter(false);
            if (num2 > 0M)
              simpleFilter.NoteRateRange = new Range<Decimal>(num2, num2);
            correspondentTradeInfo.Filter = new TradeFilter(simpleFilter);
            correspondentTradeInfo.ParseTradeObjects("", correspondentTradeInfo.Filter.ToXml(), "", correspondentTradeInfo.Pricing.ToXml(), correspondentTradeInfo.PriceAdjustments.ToXml(), correspondentTradeInfo.SRPTable.ToXml(), "", "", "", "", "", "", "");
            int trade = sessionObjects.CorrespondentTradeManager.CreateTrade(correspondentTradeInfo);
            correspondentTrade = sessionObjects.CorrespondentTradeManager.GetTrade(trade);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeManager.sw, nameof (CorrespondentTradeManager), TraceLevel.Error, "CreateCorrespondentTrade: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        return (CorrespondentTradeInfo) null;
      }
label_19:
      return correspondentTrade;
    }

    public static void AssignLoanToCorrespondentTrade(
      CorrespondentTradeInfo tradeInfo,
      string loanGuid,
      SessionObjects sessionObjects,
      LoanDataMgr loanDataMgr,
      LoanDataMgr.LockLoanSyncOption syncOption)
    {
      if (tradeInfo.TradeID < 0)
        return;
      try
      {
        CorrespondentTradeLoanAssignmentManager assignmentManager = new CorrespondentTradeLoanAssignmentManager(sessionObjects, tradeInfo.TradeID, false);
        assignmentManager.AssignLoan(new PipelineInfo(new Hashtable()
        {
          [(object) "Guid"] = (object) loanGuid
        }, (PipelineInfo.Borrower[]) null, (PipelineInfo.Alert[]) null, (PipelineInfo.AlertSummaryInfo) null, (PipelineInfo.LoanAssociateInfo[]) null, (LockInfo) null, (Hashtable) null, (PipelineInfo.MilestoneInfo[]) null, (PipelineInfo.TradeInfo) null, new PipelineInfo.TradeInfo[0], (string[]) null));
        assignmentManager.ApplyNewTradeID(tradeInfo.TradeID);
        assignmentManager.WritePendingChangesToServer();
        foreach (CorrespondentTradeLoanAssignment tradeLoanAssignment in assignmentManager)
        {
          tradeLoanAssignment.ApplyPendingStatusToLoan(loanDataMgr, tradeInfo, new List<string>(), syncOption: syncOption);
          tradeLoanAssignment.CommitPendingStatusToLoan(tradeInfo);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeManager.sw, nameof (CorrespondentTradeManager), TraceLevel.Error, "AssignLoanToCorrespondentTrade: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
      }
    }

    public static void ValidateCorrespondentTradeSettings(SessionObjects sessionObjects)
    {
      try
      {
        if (!Utils.ParseBoolean(sessionObjects.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentTrade"]))
          throw new TradeLoanUpdateException("Correspondent Trade is not Enabled.", (Exception) null);
      }
      catch (TradeLoanUpdateException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeManager.sw, nameof (CorrespondentTradeManager), TraceLevel.Error, "AssignLoanToCorrespondentTrade: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        throw ex;
      }
    }

    private static void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      Decimal totalPrice,
      CorrespondentTradeInfo tradeInfo,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      try
      {
        if (tradeInfo == null)
          throw new Exception("Loan " + loanNumber + " was not assigned because the Correspondent Trade does not exist.");
        if ((tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.Bulk || tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT) && !tradeInfo.IsWeightedAvgBulkPriceLocked && totalPrice == 0M)
          throw new Exception("Loan " + loanNumber + " was not assigned because Total Price is needed.");
        if (totalPrice != 0M && (tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.Bulk || tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT) && tradeInfo.IsWeightedAvgBulkPriceLocked)
          throw new Exception("Loan " + loanNumber + " was not assigned because only one price is allowed. This loan has both a manually entered Total Price and a manually entered Weighted Average Bulk Price.");
        if (totalPrice != 0M && tradeInfo.DeliveryType != CorrespondentMasterDeliveryType.Bulk && tradeInfo.DeliveryType != CorrespondentMasterDeliveryType.BulkAOT)
          throw new Exception("Loan " + loanNumber + " was not assigned because only one price is allowed. This loan has both a manually entered Total Price and a calculated Total Price in trade.");
        PipelineInfo[] assignedOrPendingLoans = sessionObjects.CorrespondentTradeManager.GetAssignedOrPendingLoans(tradeInfo.TradeID, tradeAssignmentFields, false);
        CorrespondentTradeLoanAssignmentManager assignmentManager = new CorrespondentTradeLoanAssignmentManager(sessionObjects, tradeInfo.TradeID, assignedOrPendingLoans);
        List<PipelineInfo> validPipelineInfos = (List<PipelineInfo>) null;
        bool flag = tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.Bulk || tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT || tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.CoIssue;
        CorrespondentTradeManager.ProcessTradeLoanErrors(assignmentManager.ValidateBeforeLoanAssignment(deliveryType, tradeInfo.Name, new string[1]
        {
          loanNumber
        }, tradeInfo.TPOID, false, (flag ? 1 : 0) != 0, out validPipelineInfos));
        CorrespondentTradeLoanAssignment[] pendingLoans = assignmentManager.GetPendingLoans();
        foreach (PipelineInfo pipelineInfo in validPipelineInfos)
        {
          PipelineInfo validPipelineInfo = pipelineInfo;
          if (!((IEnumerable<CorrespondentTradeLoanAssignment>) pendingLoans).Any<CorrespondentTradeLoanAssignment>((Func<CorrespondentTradeLoanAssignment, bool>) (a => a.Guid == validPipelineInfo.GUID)))
          {
            if (tradeInfo.DeliveryType != CorrespondentMasterDeliveryType.Bulk && tradeInfo.DeliveryType != CorrespondentMasterDeliveryType.BulkAOT)
              totalPrice = 0M;
            else if (tradeInfo.IsWeightedAvgBulkPriceLocked)
              totalPrice = tradeInfo.WeightedAvgBulkPrice;
            assignmentManager.AssignLoan(validPipelineInfo, totalPrice);
            assignmentManager.WritePendingChangesToServer(true);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeManager.sw, nameof (CorrespondentTradeManager), TraceLevel.Error, "AssignLoanToCorrespondentTrade: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        throw ex;
      }
    }

    public static void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      string tradeCommitmentNumber,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(sessionObjects);
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, 0M, sessionObjects.CorrespondentTradeManager.GetTrade(tradeCommitmentNumber), sessionObjects, tradeAssignmentFields);
    }

    public static void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      int tradeId,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(sessionObjects);
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, 0M, sessionObjects.CorrespondentTradeManager.GetTrade(tradeId), sessionObjects, tradeAssignmentFields);
    }

    public static void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      Decimal totalPrice,
      string tradeCommitmentNumber,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(sessionObjects);
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, totalPrice, sessionObjects.CorrespondentTradeManager.GetTrade(tradeCommitmentNumber), sessionObjects, tradeAssignmentFields);
    }

    public static void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      Decimal totalPrice,
      int tradeId,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      CorrespondentTradeManager.ValidateCorrespondentTradeSettings(sessionObjects);
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, totalPrice, sessionObjects.CorrespondentTradeManager.GetTrade(tradeId), sessionObjects, tradeAssignmentFields);
    }

    private static void ProcessTradeLoanErrors(List<TradeLoanUpdateError> errors)
    {
      if (errors.Count > 0)
        throw new TradeLoanUpdateException(string.Join("; ", errors.Select<TradeLoanUpdateError, string>((Func<TradeLoanUpdateError, string>) (e => e.Message)).ToArray<string>()), (Exception) null);
    }

    public static Dictionary<string, string> RemoveLoansFromCorrespondentTrade(
      string correspondentTradeName,
      List<string> loanNumbers,
      SessionObjects sessionObjects)
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      List<TradeLoanUpdateError> tradeLoanUpdateErrorList = new List<TradeLoanUpdateError>();
      CorrespondentTradeInfo trade = sessionObjects.CorrespondentTradeManager.GetTrade(correspondentTradeName);
      PipelineInfo[] pipelineInfoArray = (PipelineInfo[]) null;
      if (trade != null)
        pipelineInfoArray = sessionObjects.CorrespondentTradeManager.GetAssignedOrPendingLoans(trade.TradeID, (string[]) null, false);
      Dictionary<string, string> dictionary2 = CorrespondentTradeManager.ValidateSettingsAndTrade(correspondentTradeName, loanNumbers, sessionObjects, trade, pipelineInfoArray);
      if (dictionary2.Count >= 1)
        return dictionary2;
      List<PipelineInfo> validPipelineInfos = (List<PipelineInfo>) null;
      CorrespondentTradeLoanAssignmentManager assignmentManager = new CorrespondentTradeLoanAssignmentManager(sessionObjects, trade.TradeID, pipelineInfoArray);
      tradeLoanUpdateErrorList.AddRange((IEnumerable<TradeLoanUpdateError>) assignmentManager.ValidateLoansBeforeRemoveLoanAssignment(loanNumbers.ToArray(), trade, sessionObjects, out validPipelineInfos));
      foreach (TradeLoanUpdateError tradeLoanUpdateError in tradeLoanUpdateErrorList)
        dictionary2.Add(tradeLoanUpdateError.LoanGuid, tradeLoanUpdateError.Message);
      if (validPipelineInfos != null && validPipelineInfos.Count<PipelineInfo>() > 0)
      {
        foreach (PipelineInfo pipelineInfo in validPipelineInfos)
        {
          try
          {
            assignmentManager.RemoveLoan(pipelineInfo.GUID);
            dictionary2.Add(pipelineInfo.LoanNumber, "Processed");
          }
          catch (TradeLoanUpdateException ex)
          {
            dictionary2.Add(pipelineInfo.LoanNumber, ex.Error.Message);
          }
          catch (Exception ex)
          {
            dictionary2.Add(pipelineInfo.LoanNumber, ex.Message);
          }
        }
      }
      assignmentManager.WritePendingChangesToServer();
      return dictionary2;
    }

    private static Dictionary<string, string> ValidateSettingsAndTrade(
      string correspondentTradeName,
      List<string> loanNumbers,
      SessionObjects sessionObjects,
      CorrespondentTradeInfo tradeInfo,
      PipelineInfo[] assingedInfo)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      try
      {
        CorrespondentTradeManager.ValidateCorrespondentTradeSettings(sessionObjects);
      }
      catch (Exception ex)
      {
        dictionary.Add("Trade Error", ex.Message);
        return dictionary;
      }
      if (string.IsNullOrEmpty(correspondentTradeName.Trim()))
      {
        dictionary.Add("Trade Error", "The Correspondent Trade is required.");
        return dictionary;
      }
      if (loanNumbers.Count < 1)
      {
        dictionary.Add("Trade Error", "Loan Number(s) are required.");
        return dictionary;
      }
      if (tradeInfo == null)
      {
        dictionary.Add("Trade Error", "The Correspondent Trade does not exist.");
        return dictionary;
      }
      if (assingedInfo == null || assingedInfo.Length != 0)
        return dictionary;
      dictionary.Add("Trade Error", "No loan(s) are assigned for this trade.");
      return dictionary;
    }

    public static EPPSLoanProgramFilters GetCorrespondentTradeEppsLoanPrograms(
      string correspondentTradeName,
      SessionObjects sessionObjects)
    {
      return sessionObjects.CorrespondentTradeManager.GetTrade(correspondentTradeName)?.EPPSLoanProgramsFilter;
    }
  }
}
