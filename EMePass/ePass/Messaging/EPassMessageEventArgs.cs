// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Messaging.EPassMessageEventArgs
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Messaging
{
  public class EPassMessageEventArgs : EventArgs
  {
    private EPassMessageEventType eventType;
    private EPassMessageInfo message;

    public EPassMessageEventArgs(EPassMessageEventType eventType)
      : this(eventType, (EPassMessageInfo) null)
    {
    }

    public EPassMessageEventArgs(EPassMessageEventType eventType, EPassMessageInfo message)
    {
      this.eventType = eventType;
      this.message = message;
    }

    public EPassMessageEventType EventType => this.eventType;

    public EPassMessageInfo Message => this.message;
  }
}
