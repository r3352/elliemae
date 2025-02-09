// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ClientInfoS
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.Encompass.AsmResolver;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ClientInfoS
  {
    public static RuntimeEnvironment RuntimeEnvironment
    {
      get
      {
        return AssemblyResolver.IsSmartClient ? RuntimeEnvironment.SmartClient : EnConfigurationSettings.GlobalSettings.RuntimeEnvironment;
      }
    }

    public static string RuntimeEnvAsString
    {
      get
      {
        switch (ClientInfoS.RuntimeEnvironment)
        {
          case RuntimeEnvironment.Default:
            return "Default";
          case RuntimeEnvironment.Hosted:
            return "Hosted";
          case RuntimeEnvironment.SmartClient:
            return "SmartClient";
          default:
            return (string) null;
        }
      }
    }
  }
}
