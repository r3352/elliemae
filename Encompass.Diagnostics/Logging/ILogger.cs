// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.ILogger
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public interface ILogger
  {
    bool IsEnabled(LogLevel level);

    void Write(LogLevel level, string src, string message);

    void Write(LogLevel level, string src, Exception ex);

    void Write(LogLevel level, string src, string message, Exception ex);

    void Write(LogLevel level, string src, string message, LogFields info);

    void Write(LogLevel level, string src, string message, Exception ex, LogFields info);

    void When(LogLevel level, Action action);

    void Write<TExtendedLog>(TExtendedLog log) where TExtendedLog : Log;
  }
}
