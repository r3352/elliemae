// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.WriteLogEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  public class WriteLogEventArgs : EventArgs
  {
    internal WriteLogEventArgs(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      this.Log = (ILog) new EllieMae.Encompass.Diagnostics.Log(log);
    }

    public ILog Log { get; }

    public bool Cancel { get; set; }
  }
}
