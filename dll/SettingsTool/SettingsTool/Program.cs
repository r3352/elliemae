// Decompiled with JetBrains decompiler
// Type: SettingsTool.Program
// Assembly: SettingsTool, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 33CBBD71-E357-4B07-850C-DC1AB565CFFD
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SettingsTool.exe

using EllieMae.EMLite.SettingsToolUI;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace SettingsTool
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      ServicePointManager.SecurityProtocol |= (SecurityProtocolType) 3840;
      AssemblyResolver.Start(args);
      AssemblyResolver.FirstSmartClientAssembly = "EllieMae.Encompass.SmartClient";
      Program.startApp(args);
    }

    private static void startApp(string[] args)
    {
      SmartClientInitializer.Initialize(true);
      SettingsToolMain.main(args);
    }
  }
}
