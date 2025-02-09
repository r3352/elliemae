// Decompiled with JetBrains decompiler
// Type: EncAdminTools.Program
// Assembly: EncAdminTools, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B3F2D746-98FD-4B75-A0A0-95A0CD29645D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EncAdminTools.exe

using EllieMae.EMLite.AdminTools;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EncAdminTools
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
      AdminToolsSC.main(args, false);
    }
  }
}
