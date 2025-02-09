// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.ExceptionEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class ExceptionEvent : ServerMonitorEvent
  {
    private Exception ex;

    public ExceptionEvent(Exception ex) => this.ex = ex;

    public Exception Exception => this.ex;

    public override string ToString() => "[Exception] " + this.ex.Message + " in " + this.ex.Source;
  }
}
