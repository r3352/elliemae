// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.PollyCircuitBreakerConfig
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Polly.CircuitBreaker;
using System;

#nullable disable
namespace Elli.MessageQueues
{
  public class PollyCircuitBreakerConfig
  {
    public event BrokenCircuitExceptionHandler OnBrokenCircuitException;

    public CircuitBreakerPolicy CircuitBreakerPolicy { get; private set; }

    public TimeSpan WaitTimeWhenCircuitBreakerIsOpen { get; set; }

    public PollyCircuitBreakerConfig(
      CircuitBreakerPolicy circuitBreakerPolicy,
      TimeSpan waitTimeInMsWhenCircuitBreakerIsOpen)
    {
      this.CircuitBreakerPolicy = circuitBreakerPolicy;
      this.WaitTimeWhenCircuitBreakerIsOpen = waitTimeInMsWhenCircuitBreakerIsOpen;
    }

    internal virtual void TriggerOnBrokenCircuitException(PollyBrokenCircuitEventArgs args)
    {
      if (this.OnBrokenCircuitException == null)
        return;
      this.OnBrokenCircuitException(args);
    }
  }
}
