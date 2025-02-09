// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.LogLevel
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Formatters;
using Newtonsoft.Json;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  [JsonConverter(typeof (LogLevelConverter))]
  public enum LogLevel
  {
    None = 0,
    ERROR = 2,
    WARN = 4,
    INFO = 8,
    DEBUG = 16, // 0x00000010
  }
}
