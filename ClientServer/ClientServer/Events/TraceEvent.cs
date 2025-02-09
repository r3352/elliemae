// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.TraceEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class TraceEvent : ServerEvent
  {
    private TraceLevel traceLevel;
    private string instanceName;
    private string category;
    private string message;

    public TraceEvent(TraceLevel traceLevel, string instanceName, string message, string category)
    {
      this.instanceName = instanceName;
      this.traceLevel = traceLevel;
      this.category = category;
      this.message = message;
    }

    public TraceLevel TraceLevel => this.traceLevel;

    public string InstanceName => this.instanceName;

    public string Category => this.category;

    public string Message => this.message;

    public override string ToString() => this.category + ": " + this.message;
  }
}
