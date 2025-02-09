// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DeepLinking.UnderwriterCenter
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.DeepLinking.Activity.Contract;
using EllieMae.EMLite.Common.DeepLinking.Context.Contract;
using EllieMae.EMLite.Common.DeepLinking.Contract;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.DeepLinking
{
  public class UnderwriterCenter : BaseDeepLink
  {
    private BaseLoanContext _context;

    public UnderwriterCenter(
      DeepLinkType applicationName,
      IDeepLinkContext deepLinkApplicationContext,
      IPreDeepLinkActivity preDeepLinkActivity)
      : base(applicationName, deepLinkApplicationContext, preDeepLinkActivity)
    {
      this._context = deepLinkApplicationContext is BaseLoanContext ? deepLinkApplicationContext as BaseLoanContext : throw new Exception("Invalid DeepLink application context.");
    }

    public override string URL
    {
      get
      {
        return DeepLinkURLHelper.Pipeline.Combine(this._context.LoanGUID, "underwriting-center") ?? "";
      }
    }

    public override string KPIDescription
    {
      get => this.DeepLinkApplicationContext.Source + " --> Underwriter Center";
    }
  }
}
