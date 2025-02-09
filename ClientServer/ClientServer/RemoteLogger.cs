// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.RemoteLogger
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging.Utils;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class RemoteLogger
  {
    private static readonly RemoteLoggerUtil Instance = new RemoteLoggerUtil(DiagUtility.LoggerScopeProvider, DiagUtility.LogManager);

    public static void Write(TraceLevel level, string message)
    {
      RemoteLogger.Instance.WriteLog(level, message);
    }

    public static void Write(TraceLevel level, string src, string message, Exception ex)
    {
      RemoteLogger.Instance.WriteLog(level, src, message, ex);
    }

    public static void Write(Exception ex) => RemoteLogger.Instance.WriteLog(ex);
  }
}
