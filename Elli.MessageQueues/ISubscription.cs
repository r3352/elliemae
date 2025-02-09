// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.ISubscription
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues
{
  public interface ISubscription
  {
    string QueueName { get; set; }

    string SubscriptionName { get; set; }

    CancellationToken CancellationToken { get; set; }

    void Cancel();

    void Ack(ulong deliveryTag);

    void AckAllUpTo(ulong deliveryTag);

    void Ack(IEnumerable<ulong> deliveryTags);

    void AckAllOutstandingMessages();

    void Nack(ulong deliveryTag, bool requeue);

    void NackAllUpTo(ulong deliveryTag, bool requeue);

    void NackAllOutstandingMessages(bool requeue);
  }
}
