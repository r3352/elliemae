// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Trading.BatchJobManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Trading
{
  public class BatchJobManager
  {
    private const string className = "CorrespondentTradeManager�";
    protected static string sw = Tracing.SwOutsideLoan;

    public static int SubmitTradeBatchUpdateJob(
      CorrespondentTradeInfo tradeInfo,
      CorrespondentTradeLoanAssignment[] assignments,
      BatchJobApplicationChannel channel,
      SessionObjects sessionObjects)
    {
      List<TradeAssignmentItem> items = new List<TradeAssignmentItem>();
      foreach (CorrespondentTradeLoanAssignment assignment in assignments)
        items.Add(new TradeAssignmentItem()
        {
          EntityId = assignment.PipelineInfo.GUID,
          LoanNumber = assignment.PipelineInfo.LoanNumber,
          Type = BatchJobItemEntityType.Loan,
          Action = BatchJobItemAction.AssignToTrade,
          TotalPrice = assignment.TotalPrice,
          AssignedStatus = assignment.AssignedStatus.ToString(),
          InitialPendingStatus = CorrespondentTradeLoanStatus.None.ToString(),
          PendingStatus = assignment.PendingStatus.ToString(),
          Rejected = assignment.Rejected
        });
      return sessionObjects.TradeSynchronizationManager.Assign((TradeInfoObj) tradeInfo, items, new List<string>(), "", BatchJobApplicationChannel.SDK, lockLoanSyncOption: "syncLockToLoan");
    }
  }
}
