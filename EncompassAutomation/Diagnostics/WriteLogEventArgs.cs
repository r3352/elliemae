// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.WriteLogEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  /// <summary>Application write log event arguments</summary>
  public class WriteLogEventArgs : EventArgs
  {
    internal WriteLogEventArgs(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      this.Log = (ILog) new EllieMae.Encompass.Diagnostics.Log(log);
    }

    /// <summary>Generated log</summary>
    public ILog Log { get; }

    /// <summary>
    /// Set this to true if downstream default log handling should be suppressed.
    /// </summary>
    public bool Cancel { get; set; }
  }
}
