// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.PollyBrokenCircuitEventArgs
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Polly.CircuitBreaker;
using System;

#nullable disable
namespace Elli.MessageQueues
{
  public class PollyBrokenCircuitEventArgs : EventArgs
  {
    public PollyBrokenCircuitEventArgs(
      bool messageHandled,
      MessageUnhandledEventArgs messageUnhandledEventArgs,
      BrokenCircuitException brokenCircuitException)
    {
      this.MessageUnhandledEventArgs = messageUnhandledEventArgs;
      this.BrokenCircuitException = brokenCircuitException;
      this.MessageWasHandled = messageHandled;
    }

    public MessageUnhandledEventArgs MessageUnhandledEventArgs { get; private set; }

    public bool MessageWasHandled { get; private set; }

    public BrokenCircuitException BrokenCircuitException { get; private set; }
  }
}
