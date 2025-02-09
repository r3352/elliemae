// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.Bootstrapper
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.MessageQueues;
using Elli.MessageQueues.Loggers;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public class Bootstrapper
  {
    private static readonly object LockObject = new object();

    public static bool Initialized { get; private set; }

    public static void InitializeMessageQueueFramework(
      bool enableMetrics,
      int consumerDisposeTimeoutSeconds)
    {
      enableMetrics = false;
      lock (Bootstrapper.LockObject)
      {
        if (Bootstrapper.Initialized)
          return;
        MessageQueueFramework.Runtime.InjectJsonMessageSerializer().Inject<ILogger>((IServiceFactory<ILogger>) new LoggerFactory()).InjectSignalFxPolicy(enableMetrics).InjectSignalFxMetrics("MessageQueue", enableMetrics).InjectDefaultFaultHandler().InjectDefaultRetryPolicy().Inject<IPasswordPolicy>((IServiceFactory<IPasswordPolicy>) new PasswordPolicyFactory()).InjectConsumerDisposeTimeoutPolicy(consumerDisposeTimeoutSeconds).InjectHaConnectionRoundRobinPolicy(true).Initialize();
        Bootstrapper.Initialized = true;
      }
    }
  }
}
