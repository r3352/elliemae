// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Context.SSFEventArgs
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace Elli.Web.Host.SSF.Context
{
  public class SSFEventArgs : EventArgs
  {
    private string _objectName;
    private string _eventName;
    private string _eventPayload;

    public SSFEventArgs(string objectName, string eventName)
      : this(objectName, eventName, (object) null)
    {
    }

    public SSFEventArgs(string objectName, string eventName, object eventPayload)
    {
      this._objectName = objectName;
      this._eventName = eventName;
      if (eventPayload != null)
        this._eventPayload = JsonConvert.SerializeObject(eventPayload);
      else
        this._eventPayload = (string) null;
    }

    public string ObjectName => this._objectName;

    public string EventName => this._eventName;

    public string EventPayload => this._eventPayload;
  }
}
