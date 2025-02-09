// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateError
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeLoanUpdateError
  {
    public TradeLoanUpdateError()
    {
    }

    public TradeLoanUpdateError(string loanGuid, PipelineInfo loanInfo, string message)
    {
      this.LoanGuid = loanGuid;
      this.LoanInfo = loanInfo;
      this.Message = message;
    }

    public string LoanGuid { get; set; }

    public PipelineInfo LoanInfo { get; set; }

    public string Message { get; set; }

    public override string ToString()
    {
      return string.Format("LoanGuid: {0}" + Environment.NewLine + "Message: {1}", (object) this.LoanGuid, (object) this.Message);
    }
  }
}
