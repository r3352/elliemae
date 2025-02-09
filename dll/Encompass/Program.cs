// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Encompass.Program
// Assembly: Encompass, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 2C6CFF83-8576-4740-ABAA-C9635DDA95F0
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Encompass.exe

using EllieMae.EMLite.Diagnostics;
using EllieMae.EMLite.MainUI;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Encompass
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
      AssemblyResolver.FirstSmartClientAssembly = "EllieMae.Encompass.SmartClient";
      Program.start(args);
    }

    private static void start(string[] args)
    {
      SmartClientInitializer.Initialize(false);
      if (DiagnosticSession.IsDiagnosticsSessionRequired(args))
        Program.startDiagnostics(args);
      else
        Program.startApp(args);
    }

    private static void startApp(string[] args)
    {
      SmartClientInitializer.Initialize(true);
      EncompassSC.main(args);
    }

    private static void startDiagnostics(string[] args)
    {
      new DiagnosticSession().Execute(Environment.GetCommandLineArgs()[0], args);
    }
  }
}
