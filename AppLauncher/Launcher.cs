// Decompiled with JetBrains decompiler
// Type: AppLauncher.Launcher
// Assembly: AppLauncher, Version=4.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 332B99CD-CD6E-4691-BDB2-CE964521D35B
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AppLauncher.exe

using AppLauncherLib;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Net;

#nullable disable
namespace AppLauncher
{
  public class Launcher
  {
    [STAThread]
    public static int Main(string[] args)
    {
      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      AssemblyResolver.Start(args);
      return Launcher.startApp(args);
    }

    private static int startApp(string[] args) => AppLauncherImpl.main(args);
  }
}
