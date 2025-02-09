// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.ITradeEditorBase
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public interface ITradeEditorBase
  {
    TradeInfoObj CurrentTradeInfo { get; }

    Decimal TradeAmount { get; }

    bool SuspendEvents { get; set; }

    bool DataModified { get; }

    bool SaveTrade();
  }
}
