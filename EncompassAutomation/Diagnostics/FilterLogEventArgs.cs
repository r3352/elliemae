// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.FilterLogEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  /// <summary>Filter log event arguments</summary>
  public class FilterLogEventArgs
  {
    internal FilterLogEventArgs(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      this.LogSwitch = log.Logger;
      this.LogLevel = Log.GetLogLevel(log.Level);
    }

    /// <summary>
    /// Log switch, null value means a log switch was not specified.
    /// </summary>
    public string LogSwitch { get; }

    /// <summary>Log switch</summary>
    public LogLevel LogLevel { get; }

    /// <summary>
    /// Set this to true if LogSwitch and LogLevel combination is active for logging
    /// </summary>
    public bool IsActive { get; set; }
  }
}
