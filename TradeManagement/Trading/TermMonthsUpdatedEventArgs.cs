// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TermMonthsUpdatedEventArgs
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TermMonthsUpdatedEventArgs : EventArgs
  {
    private int term1;
    private int term2;

    public TermMonthsUpdatedEventArgs(int term1, int term2)
    {
      this.term1 = term1;
      this.term2 = term2;
    }

    public int Term1 => this.term1;

    public int Term2 => this.term2;
  }
}
