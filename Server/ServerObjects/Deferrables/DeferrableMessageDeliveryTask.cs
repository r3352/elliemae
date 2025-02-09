// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.DeferrableMessageDeliveryTask
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public sealed class DeferrableMessageDeliveryTask
  {
    public Guid Id { get; private set; }

    public IMessage Message { get; private set; }

    public string RoutingKey { get; private set; }

    public DeferrableMessageDeliveryTask(IMessage messge, string routingKey)
    {
      this.Id = Guid.NewGuid();
      this.Message = messge;
      this.RoutingKey = routingKey;
    }
  }
}
