// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.FormBuilder.Program
// Assembly: FormBuilder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 3ABE0CC1-A4DA-4A95-9C00-27989EE4B4F4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\FormBuilder.exe

using EllieMae.EMLite.FormEditor;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.FormBuilder
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
      Program.startApp(args);
    }

    private static void startApp(string[] args)
    {
      SmartClientInitializer.Initialize(true);
      HostWinSC.main(args);
    }
  }
}
