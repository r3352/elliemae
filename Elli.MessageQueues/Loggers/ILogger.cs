// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Loggers.ILogger
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;

#nullable disable
namespace Elli.MessageQueues.Loggers
{
  public interface ILogger
  {
    bool IsDebugEnable { get; set; }

    void DebugFormat(string format, params object[] args);

    void InfoFormat(string format, params object[] args);

    void WarnFormat(string format, params object[] args);

    void ErrorFormat(string format, params object[] args);

    void Error(Exception exception);
  }
}
