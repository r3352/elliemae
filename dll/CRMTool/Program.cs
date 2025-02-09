// Decompiled with JetBrains decompiler
// Type: CRMTool.Program
// Assembly: CRMTool, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: C4E26DB0-5EEF-43E1-8127-BF24D4B06853
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\CRMTool.exe

using EllieMae.EMLite.CRMToolUI;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace CRMTool
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
      CRMToolsSC.main(args);
    }
  }
}
