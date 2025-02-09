// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DeepLinking.DeepLinkURLHelper
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Common.DeepLinking
{
  public static class DeepLinkURLHelper
  {
    public static string LOConnect => Session.StartupInfo?.LOConnectUrl;

    public static string Pipeline => DeepLinkURLHelper.LOConnect.Combine("pipeline");

    public static string Opportunities => DeepLinkURLHelper.LOConnect.Combine("opportunities");

    public static string Prospects => DeepLinkURLHelper.LOConnect.Combine("prospects");
  }
}
