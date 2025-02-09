// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DeepLinking.DeepLinkFactory
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.DeepLinking.Activity;
using EllieMae.EMLite.Common.DeepLinking.Activity.Contract;
using EllieMae.EMLite.Common.DeepLinking.Context.Contract;
using EllieMae.EMLite.Common.DeepLinking.Contract;

#nullable disable
namespace EllieMae.EMLite.Common.DeepLinking
{
  public class DeepLinkFactory
  {
    public static IDeepLink Create(
      DeepLinkType applicationName,
      IDeepLinkContext deepLinkApplicationContext)
    {
      switch (applicationName)
      {
        case DeepLinkType.LoanDefaultPage:
          return (IDeepLink) new LoanDefaultPage(applicationName, deepLinkApplicationContext, PreDeepLinkActivityFactory.Create(deepLinkApplicationContext));
        case DeepLinkType.UnderwriterCenter:
          return (IDeepLink) new UnderwriterCenter(applicationName, deepLinkApplicationContext, (IPreDeepLinkActivity) new CloseLoanActivity());
        case DeepLinkType.Opportunities:
          return (IDeepLink) new Opportunities(applicationName, deepLinkApplicationContext);
        case DeepLinkType.Prospects:
          return (IDeepLink) new Prospects(applicationName, deepLinkApplicationContext);
        default:
          return (IDeepLink) null;
      }
    }
  }
}
