// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.AsyncSubscriptionOption`1
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues
{
  [ExcludeFromCodeCoverage]
  public class AsyncSubscriptionOption<T>
  {
    public string SubscriptionName { get; set; }

    public ushort BatchSize { get; set; }

    public uint QueuePrefetchCount { get; set; }

    public IRouteFinder RouteFinder { get; set; }

    public bool Exclusive { get; set; }

    public Action<T, MessageDeliverEventArgs> MessageHandler { get; set; }

    public PollyCircuitBreakerConfig CircuitBreakerPolicy { get; set; }

    public CancellationToken CancellationToken { get; set; }
  }
}
