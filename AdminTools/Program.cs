// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.Program
// Assembly: AdminTools, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 1D4C5CFF-FD87-4955-B8BE-0E0BA9BBC09D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AdminTools.exe

using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      AssemblyResolver.Start(args);
      AssemblyResolver.FirstSmartClientAssembly = "EllieMae.Encompass.SmartClient";
      Program.startApp(args);
    }

    private static void startApp(string[] args)
    {
      SmartClientInitializer.Initialize(true);
      AdminToolsSC.main(args);
    }
  }
}
