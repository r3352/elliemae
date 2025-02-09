// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
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
