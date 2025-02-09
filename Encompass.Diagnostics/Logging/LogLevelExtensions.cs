// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.LogLevelExtensions
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public static class LogLevelExtensions
  {
    private static readonly LogLevel _force = (LogLevel) 1;
    private static readonly LogLevel _clearForce = ~LogLevelExtensions._force;

    public static LogLevel Force(this LogLevel logEventType)
    {
      return LogLevelExtensions._force | logEventType;
    }

    public static LogLevel GetBaseType(this LogLevel logEventType)
    {
      return LogLevelExtensions._clearForce & logEventType;
    }
  }
}
