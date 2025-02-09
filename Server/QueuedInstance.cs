// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.QueuedInstance
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Hazelcast;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class QueuedInstance
  {
    private readonly string ClassName = nameof (QueuedInstance);
    private static int count;
    private static int totalRefCount;
    private int refCount;
    private readonly Func<IHazelcastClient> instanceFactory;
    private DateTime lastResetAttempt = DateTime.UtcNow;
    private Lazy<IHazelcastClient> instance;

    public QueuedInstance(Func<IHazelcastClient> instanceFactory)
    {
      this.Id = Interlocked.Increment(ref QueuedInstance.count);
      this.instanceFactory = instanceFactory;
      this.instance = new Lazy<IHazelcastClient>(this.instanceFactory);
      ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore", true);
      logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() => logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (QueuedInstance), "Created new Hazelcast QueuedInstance with id: " + (object) this.Id)));
    }

    public IHazelcastClient Instance => this.instance.Value;

    public int Id { get; }

    public int Refcount => this.refCount;

    public static int TotalRefCount => QueuedInstance.totalRefCount;

    public void Increment()
    {
      Interlocked.Increment(ref this.refCount);
      Interlocked.Increment(ref QueuedInstance.totalRefCount);
    }

    public void Decrement()
    {
      Interlocked.Decrement(ref this.refCount);
      Interlocked.Decrement(ref QueuedInstance.totalRefCount);
    }

    public bool Reset()
    {
      lock (this)
      {
        if ((DateTime.UtcNow - this.lastResetAttempt).TotalSeconds <= 60.0)
          return false;
        this.lastResetAttempt = DateTime.UtcNow;
        ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore");
        logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() => logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (QueuedInstance), "Resetting Hazelcast QueuedInstance with id: " + (object) this.Id)));
        try
        {
          IHazelcastClient newInstance = this.instanceFactory();
          Lazy<IHazelcastClient> currentInstance = this.instance;
          this.instance = new Lazy<IHazelcastClient>((Func<IHazelcastClient>) (() => newInstance));
          if (currentInstance != null)
            Task.Run((Func<Task>) (async () =>
            {
              try
              {
                await currentInstance.Value.DisposeAsync();
              }
              catch (Exception ex)
              {
                TraceLog.WriteWarning(this.ClassName, "Exception while shutting down Hazelast client instance: " + ex.Message);
              }
            }));
        }
        catch (Exception ex)
        {
          logger.Write(Encompass.Diagnostics.Logging.LogLevel.WARN, nameof (QueuedInstance), string.Format("Reset Attempt failed for Hazelcast QueuedInstance with id: {0} Error: {1}", (object) this.Id, (object) ex.Message));
          return false;
        }
        return true;
      }
    }
  }
}
