// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ApplicationLog
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.RemotingServices;
using Encompass.Diagnostics.Config;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public static class ApplicationLog
  {
    static ApplicationLog() => ApplicationLog.Initialize();

    internal static void Initialize()
    {
      Tracing.Init(System.IO.Path.Combine(SystemSettings.LogDir, Environment.UserName + "\\Session"));
    }

    public static string Path => Tracing.LogFile;

    public static bool DebugEnabled
    {
      get => Tracing.Debug;
      set
      {
        Tracing.Debug = value;
        DiagConfig<ClientDiagConfigData>.Instance.ReloadConfig();
      }
    }

    public static void Write(string source, string message)
    {
      Tracing.Log(true, "INFO", source, message);
    }

    public static void WriteError(string source, string message)
    {
      Tracing.Log(true, "ERROR", source, message);
    }

    public static void WriteDebug(string source, string message)
    {
      Tracing.Log(false, "DEBUG", source, message);
    }
  }
}
