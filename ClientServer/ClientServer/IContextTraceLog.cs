// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IContextTraceLog
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IContextTraceLog : IDisposable
  {
    void Write(TraceLevel level, string className, string message);

    void Write(Encompass.Diagnostics.Logging.LogLevel level, string className, string message);

    void WriteException(TraceLevel level, string className, Exception ex);

    void WriteException(string className, Exception ex);

    void WriteInfo(string className, string message);

    void WriteVerbose(string className, string message);

    void WriteWarning(string className, string message);

    void WriteError(string className, string message);

    void WriteDebug(string className, string message);

    int GetErrorCount();

    string[] GetErrors();

    void ClearErrors();
  }
}
