// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.ITradeJob
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public interface ITradeJob
  {
    TradeJobProcessorState State { get; }

    void StartAsync();

    void CancelAsync();

    void StartAsyncBackground();

    string JobGuid { get; set; }

    List<string> SkipFieldList { get; set; }

    int LoansSkipped { get; }

    int LoansCompleted { get; }

    bool HadErrors { get; }

    bool ForceUpdateOfAllLoans { get; set; }

    List<TradeLoanUpdateError> Errors { get; }

    event TradeUpdateEventHandler Started;

    event TradeUpdateEventHandler Completed;

    event TradeUpdateEventHandler Cancelled;

    event ProgressChangedEventHandler ProgressChanged;
  }
}
