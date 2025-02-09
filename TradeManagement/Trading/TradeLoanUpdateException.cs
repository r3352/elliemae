// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateException
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeLoanUpdateException : Exception
  {
    public TradeLoanUpdateException()
    {
    }

    public TradeLoanUpdateException(TradeLoanUpdateError error) => this.Error = error;

    public TradeLoanUpdateException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public TradeLoanUpdateError Error { get; set; }

    public override string ToString()
    {
      return this.Error == null ? base.ToString() : this.Error.ToString() + Environment.NewLine + base.ToString();
    }
  }
}
