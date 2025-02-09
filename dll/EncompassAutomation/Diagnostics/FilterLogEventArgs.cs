// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.FilterLogEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using Encompass.Diagnostics.Logging.Schema;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  public class FilterLogEventArgs
  {
    internal FilterLogEventArgs(Log log)
    {
      this.LogSwitch = log.Logger;
      this.LogLevel = Log.GetLogLevel(log.Level);
    }

    public string LogSwitch { get; }

    public LogLevel LogLevel { get; }

    public bool IsActive { get; set; }
  }
}
