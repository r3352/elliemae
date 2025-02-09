// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CalculateFeeEventArgs
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CalculateFeeEventArgs : EventArgs
  {
    private Decimal undeliveredAmt;
    private Decimal fee;

    public CalculateFeeEventArgs(Decimal undeliveredAmt) => this.undeliveredAmt = undeliveredAmt;

    public Decimal UndelieveredAmount => this.undeliveredAmt;

    public Decimal Fee
    {
      get => this.fee;
      set => this.fee = value;
    }
  }
}
