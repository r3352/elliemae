// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.Notifications.TradeLoanUpdateArgs
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading.Notifications
{
  public class TradeLoanUpdateArgs : EventArgs
  {
    public TradeLoanUpdateArgs(
      string tradeId,
      string tradeStatus,
      DateTime timestamp,
      string correlationId)
    {
      this.TradeId = tradeId;
      this.TradeStatus = tradeStatus;
      this.Timestamp = timestamp;
      this.CorrelationId = correlationId;
    }

    public string TradeId { get; private set; }

    public string TradeStatus { get; private set; }

    public DateTime Timestamp { get; private set; }

    public string CorrelationId { get; private set; }
  }
}
