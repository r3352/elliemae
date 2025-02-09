// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.ILogFormatter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System.IO;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public interface ILogFormatter
  {
    void WriteFormatted(Log log, TextWriter textWriter);

    string FormatLog(Log log);

    LogFormat GetFormat();
  }
}
