// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.ITunnel
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.MessageQueues
{
  public interface ITunnel : IDisposable
  {
    event Action OnOpened;

    event Action OnClosed;

    event Action<ISubscription> ConsumerDisconnected;

    bool IsOpened { get; }

    void Publish<T>(T message);

    void Publish<T>(T message, string routingKey);

    void PublishingChannelConfirmSelect();

    void PublishingChannelWaitForConfirmsOrDie(int timeout = 0);

    void Publish<T>(T message, IDictionary<string, object> customHeaders);

    ISubscription Subscribe<T>(SubscriptionOption<T> subscriptionOption);

    ISubscription SubscribeAsync<T>(AsyncSubscriptionOption<T> subscriptionOption);

    void SetRouteFinder(IRouteFinder routeFinder);

    void SetSerializer(ISerializer serializer);

    void SetPersistentMode(bool persistentMode);

    uint GetMessageCount<T>(SubscriptionOption<T> subscriptionOption);

    uint GetMessageCount(string queueName);
  }
}
