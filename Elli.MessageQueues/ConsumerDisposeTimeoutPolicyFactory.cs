// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.ConsumerDisposeTimeoutPolicyFactory
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues
{
  [ExcludeFromCodeCoverage]
  public class ConsumerDisposeTimeoutPolicyFactory : IServiceFactory<IConsumerDisposeTimeoutPolicy>
  {
    private ConsumerDisposeTimeoutPolicy _policy;

    public ConsumerDisposeTimeoutPolicyFactory(int timeoutInSeconds)
    {
      if (timeoutInSeconds < 0)
        timeoutInSeconds = 10;
      this._policy = new ConsumerDisposeTimeoutPolicy(timeoutInSeconds);
    }

    public IConsumerDisposeTimeoutPolicy CreateInstance()
    {
      return (IConsumerDisposeTimeoutPolicy) this._policy;
    }
  }
}
