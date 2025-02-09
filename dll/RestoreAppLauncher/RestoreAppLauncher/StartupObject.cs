// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.StartupObject
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using EllieMae.Encompass.AsmResolver.Utils;
using System;

#nullable disable
namespace RestoreAppLauncher
{
  public class StartupObject
  {
    public readonly string ScInstallDir;
    private string scRegKeyPath;
    private static StartupObject instance;

    public string ScRegKeyPath
    {
      get
      {
        if (this.scRegKeyPath == null)
        {
          if (this.ScInstallDir == null)
            throw new Exception("Startup object has not been initialized yet.");
          this.scRegKeyPath = "Software\\" + Consts.AppCompanyName + "\\SmartClient\\" + this.ScInstallDir.Replace("\\", "/");
        }
        return this.scRegKeyPath;
      }
    }

    private StartupObject(string scInstallDir) => this.ScInstallDir = scInstallDir;

    public static StartupObject Instance
    {
      get
      {
        return StartupObject.instance != null ? StartupObject.instance : throw new Exception("Startup object has not been initialized yet.");
      }
    }

    public static void InitInstance(string scInstallDir)
    {
      StartupObject.instance = StartupObject.instance == null ? new StartupObject(scInstallDir) : throw new Exception("Startup object has already been initialized.");
    }
  }
}
