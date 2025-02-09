// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
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
