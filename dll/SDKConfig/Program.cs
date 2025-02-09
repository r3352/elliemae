// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.SDKConfig.Program
// Assembly: SDKConfig, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 42A5FE88-B490-4649-A561-72AB2A7848BE
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDKConfig.exe

using EllieMae.EMLite.SDK;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.SDKConfig
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      ServicePointManager.SecurityProtocol |= (SecurityProtocolType) 3840;
      AssemblyResolver.Start(args);
      AssemblyResolver.FirstSmartClientAssembly = "EncompassObjects";
      Program.initSmartClient();
      Program.startApp(args);
    }

    private static void initSmartClient() => SmartClientInitializer.Init(false);

    private static void startApp(string[] args) => MainForm.main(args);
  }
}
