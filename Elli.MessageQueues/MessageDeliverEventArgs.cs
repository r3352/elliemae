// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.MessageDeliverEventArgs
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.MessageQueues
{
  public class MessageDeliverEventArgs
  {
    public string SubscriptionName { get; set; }

    public string ConsumerTag { get; set; }

    public ulong DeliveryTag { get; set; }

    public uint MessagePriority { get; set; }

    public bool Redelivered { get; set; }

    public string Exchange { get; set; }

    public string RoutingKey { get; set; }

    public int DeathCount { get; set; }

    public IDictionary<string, object> Headers { get; set; }
  }
}
