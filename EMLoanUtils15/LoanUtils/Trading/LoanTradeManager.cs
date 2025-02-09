// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Trading.LoanTradeManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Trading
{
  public class LoanTradeManager
  {
    private const string className = "LoanTradeManager�";
    protected static string sw = Tracing.SwOutsideLoan;

    public static void AllocateLoanToLoanTrade(
      string loanNumber,
      string tradeNumber,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      LoanTradeManager.AllocateLoanToLoanTrade(loanNumber, 0M, sessionObjects.LoanTradeManager.GetTrade(tradeNumber), sessionObjects, tradeAssignmentFields);
    }

    public static void AllocateLoanToLoanTrade(
      string loanNumber,
      int tradeId,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      LoanTradeManager.AllocateLoanToLoanTrade(loanNumber, 0M, sessionObjects.LoanTradeManager.GetTrade(tradeId), sessionObjects, tradeAssignmentFields);
    }

    public static void AllocateLoanToLoanTrade(
      string loanNumber,
      Decimal totalPrice,
      string tradeNumber,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      LoanTradeManager.AllocateLoanToLoanTrade(loanNumber, totalPrice, sessionObjects.LoanTradeManager.GetTrade(tradeNumber), sessionObjects, tradeAssignmentFields);
    }

    public static void AllocateLoanToLoanTrade(
      string loanNumber,
      Decimal totalPrice,
      int tradeId,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      LoanTradeManager.AllocateLoanToLoanTrade(loanNumber, totalPrice, sessionObjects.LoanTradeManager.GetTrade(tradeId), sessionObjects, tradeAssignmentFields);
    }

    private static void AllocateLoanToLoanTrade(
      string loanNumber,
      Decimal totalPrice,
      LoanTradeInfo tradeInfo,
      SessionObjects sessionObjects,
      string[] tradeAssignmentFields)
    {
      try
      {
        if (tradeInfo == null)
          throw new Exception("Loan " + loanNumber + " was not assigned because the Loan Trade does not exist.");
        if (tradeInfo.IsBulkDelivery && !tradeInfo.IsWeightedAvgBulkPriceLocked && totalPrice == 0M)
          throw new Exception("Loan " + loanNumber + " was not assigned because Total Price is needed.");
        if (totalPrice != 0M && tradeInfo.IsBulkDelivery && tradeInfo.IsWeightedAvgBulkPriceLocked)
          throw new Exception("Loan " + loanNumber + " was not assigned because only one price is allowed. This loan has both a manually entered Total Price and a manually entered Weighted Average Bulk Price.");
        if (totalPrice != 0M && !tradeInfo.IsBulkDelivery)
          throw new Exception("Loan " + loanNumber + " was not assigned because only one price is allowed. This loan has both a manually entered Total Price and a calculated Total Price in trade.");
        PipelineInfo[] assignedOrPendingLoans = sessionObjects.LoanTradeManager.GetAssignedOrPendingLoans(tradeInfo.TradeID, tradeAssignmentFields, false);
        LoanTradeAssignmentManager assignmentManager = new LoanTradeAssignmentManager(sessionObjects, tradeInfo.TradeID, assignedOrPendingLoans);
        List<PipelineInfo> validPipelineInfos = (List<PipelineInfo>) null;
        LoanTradeManager.ProcessTradeLoanErrors(assignmentManager.ValidateBeforeLoanAssignment(tradeInfo.Name, new string[1]
        {
          loanNumber
        }, false, out validPipelineInfos));
        LoanTradeAssignment assignment = (LoanTradeAssignment) null;
        LoanTradeAssignment[] pendingLoans = assignmentManager.GetPendingLoans();
        foreach (PipelineInfo pipelineInfo in validPipelineInfos)
        {
          PipelineInfo validPipelineInfo = pipelineInfo;
          if (!((IEnumerable<LoanTradeAssignment>) pendingLoans).Any<LoanTradeAssignment>((Func<LoanTradeAssignment, bool>) (a => a.Guid == validPipelineInfo.GUID)))
          {
            if (!tradeInfo.IsBulkDelivery)
              totalPrice = 0M;
            else if (tradeInfo.IsWeightedAvgBulkPriceLocked)
              totalPrice = tradeInfo.WeightedAvgBulkPrice;
            assignmentManager.AssignLoan(validPipelineInfo, totalPrice);
            assignmentManager.WritePendingChangesToServer(true);
            pendingLoans = assignmentManager.GetPendingLoans();
          }
          if (pendingLoans != null && ((IEnumerable<LoanTradeAssignment>) pendingLoans).Any<LoanTradeAssignment>((Func<LoanTradeAssignment, bool>) (a => a.Guid == validPipelineInfo.GUID)))
            assignment = ((IEnumerable<LoanTradeAssignment>) pendingLoans).First<LoanTradeAssignment>((Func<LoanTradeAssignment, bool>) (x => x.Guid == validPipelineInfo.GUID));
          if (assignment != null)
          {
            new UpdateTradeToLoanProcess().CommitLoanTradeAssignment(sessionObjects, assignment, tradeInfo, true, new List<string>(), 0M);
            if (tradeInfo.IsBulkDelivery && !tradeInfo.IsWeightedAvgBulkPriceLocked)
            {
              LoanTradeAssignment[] assignedPendingLoans = assignmentManager.GetAllAssignedPendingLoans();
              if (assignedPendingLoans != null && assignedPendingLoans.Length != 0)
              {
                Decimal totalPrice1 = 0M;
                Decimal totalAmount = 0M;
                foreach (LoanTradeAssignment loanTradeAssignment in assignedPendingLoans)
                {
                  totalPrice1 += loanTradeAssignment.TotalPrice;
                  if (loanTradeAssignment.PipelineInfo.Info.ContainsKey((object) "TotalLoanAmount"))
                    totalAmount += (Decimal) loanTradeAssignment.PipelineInfo.Info[(object) "TotalLoanAmount"];
                  else if (loanTradeAssignment.PipelineInfo.Info.ContainsKey((object) "Loan.TotalLoanAmount"))
                    totalAmount += (Decimal) loanTradeAssignment.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"];
                }
                tradeInfo.WeightedAvgBulkPrice = LoanTradeCalculation.CalculateWeightedAvgBulkPrice(totalPrice1, totalAmount, tradeInfo);
              }
            }
            sessionObjects.LoanTradeManager.UpdateTrade(tradeInfo, false);
          }
        }
      }
      catch (TradeLoanUpdateException ex)
      {
        if (ex.Error != null)
          throw new TradeLoanUpdateException("Loan " + ex.Error.LoanInfo.LoanNumber + " was added to the loan trade, but the loan is not updated successfully because of the following reason: " + ex.Error.Message, (Exception) null);
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanTradeManager.sw, nameof (LoanTradeManager), TraceLevel.Error, "AssignLoanToLoanTrade: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        throw ex;
      }
    }

    private static void ProcessTradeLoanErrors(List<TradeLoanUpdateError> errors)
    {
      if (errors.Count > 0)
        throw new TradeLoanUpdateException(string.Join("; ", errors.Select<TradeLoanUpdateError, string>((Func<TradeLoanUpdateError, string>) (e => e.Message)).ToArray<string>()), (Exception) null);
    }
  }
}
