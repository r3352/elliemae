// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DeepLinking.Context.Contract.BaseLoanContext
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.DeepLinking.Context.Contract
{
  public abstract class BaseLoanContext : IDeepLinkContext
  {
    protected string _loanGuid;

    public string AdditionalLog { get; set; }

    public abstract string Source { get; }

    public string LoanGUID => this._loanGuid;

    public BaseLoanContext(string loanGuid)
    {
      this._loanGuid = DeepLinkHelper.SanitizeLoanGuid(loanGuid);
      this.AdditionalLog = "LoanGuid: " + loanGuid;
    }
  }
}
