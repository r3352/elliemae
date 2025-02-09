// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Runtime.RuntimeServices
// Assembly: EllieMae.Encompass.Runtime, Version=3.5.1.5, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 0B524E99-EA3D-4CD5-831A-2EF408FA62CA
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.Runtime.dll

using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Runtime
{
  public class RuntimeServices : IRuntimeServices
  {
    private static bool initialized;

    public void Initialize() => this.Initialize(false);

    public void Initialize(bool displayMOTDMessage)
    {
      lock (typeof (RuntimeServices))
      {
        if (RuntimeServices.initialized)
          return;
        ServicePointManager.SecurityProtocol |= (SecurityProtocolType) 3840;
        AssemblyResolver.Start(Application.StartupPath, "SDKConfig.exe", (string[]) null, "Ellie Mae", "Encompass360");
        AssemblyResolver.FirstSmartClientAssembly = "EncompassObjects";
        RuntimeServices.initSmartClient(displayMOTDMessage);
        RuntimeServices.initialized = true;
      }
    }

    private static void initSmartClient(bool displayMOTDMessage)
    {
      SmartClientInitializer.Init(displayMOTDMessage);
    }
  }
}
