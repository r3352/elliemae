// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoanProxy.Program
// Assembly: ClickLoanProxy, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: DA4F2767-FCBC-4A3F-8890-DE624D1776E1
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\ClickLoanProxy.exe

using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.SmartClient;
using System;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.ClickLoanProxy
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      ServicePointManager.SecurityProtocol |= (SecurityProtocolType) 3840;
      AssemblyResolver.Start(args);
      AssemblyResolver.FirstSmartClientAssembly = "EllieMae.Encompass.SmartClient";
      Program.startApp(args);
    }

    private static void startApp(string[] args)
    {
      SmartClientInitializer.Initialize(false);
      ClickLoanProxySC.main(args);
    }
  }
}
