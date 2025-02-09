// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateQueueItem
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.eFolder.Files;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  internal class TradeLoanUpdateQueueItem : IComparable<TradeLoanUpdateQueueItem>
  {
    private const string className = "TradeLoanUpdateQueueItem";
    private static readonly string sw = Tracing.SwEFolder;
    private List<IAssignmentItem> assignments;
    public bool ForceUpdateOfAllLoans;

    public TradeLoanUpdateJobInfo LoanUpdateJobInfo { get; set; }

    public TradeInfoObj TradePoolInfo { get; set; }

    public List<string> SkipFieldList { get; set; }

    public List<IAssignmentItem> AssignmentItems
    {
      get => this.assignments;
      set => this.assignments = value;
    }

    public event TransferProgressEventHandler TradeLoanUpdateProgress;

    private void OnTradeLoanUpdateProgress(TransferProgressEventArgs e)
    {
      if (this.TradeLoanUpdateProgress == null)
        return;
      this.TradeLoanUpdateProgress((object) this, e);
    }

    public int CompareTo(TradeLoanUpdateQueueItem other) => other == null ? -1 : 0;

    public override bool Equals(object obj)
    {
      return obj is TradeLoanUpdateQueueItem loanUpdateQueueItem && string.Compare(this.LoanUpdateJobInfo.JobGuid, loanUpdateQueueItem.LoanUpdateJobInfo.JobGuid, true) == 0;
    }

    public override int GetHashCode() => this.LoanUpdateJobInfo.JobGuid.ToLower().GetHashCode();

    public override string ToString() => this.LoanUpdateJobInfo.JobGuid;
  }
}
