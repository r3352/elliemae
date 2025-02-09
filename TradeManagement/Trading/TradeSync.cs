// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeSync
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeSync
  {
    public static void CorrespondentTradeSync(
      TradeInfoObj currentTradeInfo,
      CorrespondentTradeLoanAssignment[] assignments,
      SessionObjects sessionObjects,
      bool isExtension,
      string tradeExtensionInfo = null)
    {
      List<TradeAssignmentItem> items = new List<TradeAssignmentItem>();
      foreach (CorrespondentTradeLoanAssignment assignment in assignments)
      {
        BatchJobItemAction batchJobItemAction = BatchJobItemAction.None;
        if (isExtension)
          batchJobItemAction = BatchJobItemAction.TradeExtension;
        else if (assignment.AssignedStatus == CorrespondentTradeLoanStatus.Unassigned && assignment.PendingStatus == CorrespondentTradeLoanStatus.Assigned)
          batchJobItemAction = BatchJobItemAction.AssignToTrade;
        else if (assignment.AssignedStatus == CorrespondentTradeLoanStatus.Assigned && assignment.PendingStatus == CorrespondentTradeLoanStatus.Unassigned)
          batchJobItemAction = BatchJobItemAction.RemoveFromTrade;
        else if (assignment.AssignedStatus == CorrespondentTradeLoanStatus.Assigned && assignment.PendingStatus == CorrespondentTradeLoanStatus.None)
          batchJobItemAction = BatchJobItemAction.RefreshFromTrade;
        items.Add(new TradeAssignmentItem()
        {
          EntityId = assignment.PipelineInfo.GUID,
          LoanNumber = assignment.PipelineInfo.LoanNumber,
          Type = BatchJobItemEntityType.Loan,
          Action = batchJobItemAction,
          TotalPrice = assignment.TotalPrice,
          AssignedStatus = assignment.AssignedStatus.ToString(),
          InitialPendingStatus = CorrespondentTradeLoanStatus.Assigned.ToString(),
          PendingStatus = assignment.PendingStatus.ToString(),
          Rejected = assignment.Rejected
        });
      }
      string instanceName = EnConfigurationSettings.InstanceName;
      string str = Session.SessionObjects.GetCompanySettingsFromCache("POLICIES")[(object) "LockRequestToLoanDataSyncOption"]?.ToString();
      string lockLoanSyncOption = "syncLockToLoan";
      if (!string.IsNullOrEmpty(str) && (LoanDataMgr.LockLoanSyncOption) Enum.Parse(typeof (LoanDataMgr.LockLoanSyncOption), str) == LoanDataMgr.LockLoanSyncOption.noSync)
        lockLoanSyncOption = "noSync";
      sessionObjects.TradeSynchronizationManager.Assign(currentTradeInfo, items, new List<string>(), "", BatchJobApplicationChannel.Encompass, tradeExtensionInfo, lockLoanSyncOption);
    }

    public static void MbsPoolTradeSync(
      TradeInfoObj currentTradeInfo,
      MbsPoolLoanAssignment[] assignments,
      SessionObjects sessionObjects,
      List<string> skipFieldList)
    {
      List<TradeAssignmentItem> items = new List<TradeAssignmentItem>();
      foreach (MbsPoolLoanAssignment assignment in assignments)
      {
        BatchJobItemAction batchJobItemAction = BatchJobItemAction.None;
        if (assignment.AssignedStatus == MbsPoolLoanStatus.Unassigned && assignment.PendingStatus == MbsPoolLoanStatus.Assigned)
          batchJobItemAction = BatchJobItemAction.AssignToTrade;
        else if ((assignment.AssignedStatus == MbsPoolLoanStatus.Assigned || assignment.AssignedStatus == MbsPoolLoanStatus.Shipped || assignment.AssignedStatus == MbsPoolLoanStatus.Purchased) && assignment.PendingStatus == MbsPoolLoanStatus.Unassigned)
          batchJobItemAction = BatchJobItemAction.RemoveFromTrade;
        else if ((assignment.AssignedStatus == MbsPoolLoanStatus.Assigned || assignment.AssignedStatus == MbsPoolLoanStatus.Shipped || assignment.AssignedStatus == MbsPoolLoanStatus.Purchased) && assignment.PendingStatus == MbsPoolLoanStatus.None || assignment.PendingStatus == MbsPoolLoanStatus.Shipped || assignment.PendingStatus == MbsPoolLoanStatus.Purchased)
          batchJobItemAction = BatchJobItemAction.RefreshFromTrade;
        MbsPoolLoanStatus mbsPoolLoanStatus = assignment.PendingStatus;
        if (batchJobItemAction == BatchJobItemAction.RefreshFromTrade || batchJobItemAction == BatchJobItemAction.AssignToTrade)
        {
          if (currentTradeInfo.PurchaseDate != DateTime.MinValue && assignment.AssignedStatus != MbsPoolLoanStatus.Purchased)
            mbsPoolLoanStatus = MbsPoolLoanStatus.Purchased;
          else if (currentTradeInfo.ShipmentDate != DateTime.MinValue && assignment.AssignedStatus != MbsPoolLoanStatus.Shipped)
            mbsPoolLoanStatus = MbsPoolLoanStatus.Shipped;
        }
        items.Add(new TradeAssignmentItem()
        {
          EntityId = assignment.PipelineInfo.GUID,
          LoanNumber = assignment.PipelineInfo.LoanNumber,
          Type = BatchJobItemEntityType.Loan,
          Action = batchJobItemAction,
          AssignedStatus = assignment.AssignedStatus.ToString(),
          InitialPendingStatus = MbsPoolSecurityTradeStatus.Assigned.ToString(),
          PendingStatus = mbsPoolLoanStatus.ToString(),
          Rejected = assignment.Rejected,
          CommitmentContractNumber = assignment.CommitmentContractNumber,
          ProductName = assignment.ProductName,
          CPA = assignment.CPA
        });
      }
      string instanceName = EnConfigurationSettings.InstanceName;
      sessionObjects.TradeSynchronizationManager.Assign(currentTradeInfo, items, skipFieldList, "", BatchJobApplicationChannel.Encompass, lockLoanSyncOption: "syncLockToLoan");
    }

    public static void LoanTradeSync(
      TradeInfoObj currentTradeInfo,
      LoanTradeAssignment[] assignments,
      SessionObjects sessionObjects,
      List<string> skipFieldList)
    {
      List<TradeAssignmentItem> items = new List<TradeAssignmentItem>();
      foreach (LoanTradeAssignment assignment in assignments)
      {
        BatchJobItemAction batchJobItemAction = BatchJobItemAction.None;
        if (assignment.AssignedStatus == LoanTradeStatus.Unassigned && assignment.PendingStatus == LoanTradeStatus.Assigned)
          batchJobItemAction = BatchJobItemAction.AssignToTrade;
        else if ((assignment.AssignedStatus == LoanTradeStatus.Assigned || assignment.AssignedStatus == LoanTradeStatus.Shipped || assignment.AssignedStatus == LoanTradeStatus.Purchased) && assignment.PendingStatus == LoanTradeStatus.Unassigned)
          batchJobItemAction = BatchJobItemAction.RemoveFromTrade;
        else if ((assignment.AssignedStatus == LoanTradeStatus.Assigned || assignment.AssignedStatus == LoanTradeStatus.Shipped || assignment.AssignedStatus == LoanTradeStatus.Purchased) && assignment.PendingStatus == LoanTradeStatus.None || assignment.PendingStatus == LoanTradeStatus.Shipped || assignment.PendingStatus == LoanTradeStatus.Purchased)
          batchJobItemAction = BatchJobItemAction.RefreshFromTrade;
        LoanTradeStatus loanTradeStatus = assignment.PendingStatus;
        if (batchJobItemAction == BatchJobItemAction.RefreshFromTrade || batchJobItemAction == BatchJobItemAction.AssignToTrade)
        {
          if (currentTradeInfo.PurchaseDate != DateTime.MinValue && assignment.AssignedStatus != LoanTradeStatus.Purchased)
            loanTradeStatus = LoanTradeStatus.Purchased;
          else if (currentTradeInfo.ShipmentDate != DateTime.MinValue && assignment.AssignedStatus != LoanTradeStatus.Shipped)
            loanTradeStatus = LoanTradeStatus.Shipped;
        }
        items.Add(new TradeAssignmentItem()
        {
          EntityId = assignment.PipelineInfo.GUID,
          LoanNumber = assignment.PipelineInfo.LoanNumber,
          Type = BatchJobItemEntityType.Loan,
          Action = batchJobItemAction,
          TotalPrice = assignment.TotalPrice,
          AssignedStatus = assignment.AssignedStatus.ToString(),
          InitialPendingStatus = LoanTradeStatus.Assigned.ToString(),
          PendingStatus = loanTradeStatus.ToString(),
          Rejected = assignment.Rejected
        });
      }
      string instanceName = EnConfigurationSettings.InstanceName;
      sessionObjects.TradeSynchronizationManager.Assign(currentTradeInfo, items, skipFieldList, "", BatchJobApplicationChannel.Encompass, lockLoanSyncOption: "syncLockToLoan");
    }
  }
}
