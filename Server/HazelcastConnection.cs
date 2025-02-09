// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.HazelcastConnection
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using Hazelcast;
using Hazelcast.Exceptions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class HazelcastConnection : IDisposable
  {
    private readonly string ClassName = nameof (HazelcastConnection);
    private QueuedInstance instance;
    private Stopwatch sw;

    public HazelcastConnection(QueuedInstance inst)
    {
      HazelcastConnection hazelcastConnection = this;
      this.instance = inst;
      this.instance.Increment();
      ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore");
      logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() =>
      {
        closure_0.sw = Stopwatch.StartNew();
        LogFields info = new LogFields().Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceId, closure_0.instance.Id).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceRefCount, closure_0.instance.Refcount).Set<string>(HazelCastCacheStore.LogFieldNames.Action, "Opened");
        logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (HazelcastConnection), "Opening HazelcastConnection using QueuedInstance: " + (object) inst.Id, info);
      }));
    }

    public IHazelcastClient Client => this.instance.Instance;

    public QueuedInstance QueuedHazelCastInstance => this.instance;

    public async Task<T> HazelCastStoreOperation<T>(
      string operationName,
      Func<IHazelcastClient, Task<T>> operation)
    {
      return await this.HazelCastStoreOperation<T>(operationName, operation, true);
    }

    private async Task<T> HazelCastStoreOperation<T>(
      string operationName,
      Func<IHazelcastClient, Task<T>> operation,
      bool retry)
    {
      T returnValue = default (T);
      try
      {
        returnValue = await operation(this.instance.Instance);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(this.ClassName, string.Format("HazelCast {0} failed with error = {1}", (object) operationName, (object) ex.Message));
        if (retry && this.IsReTryableException(ex) && this.instance.Reset())
          return await this.HazelCastStoreOperation<T>(operationName, operation, false);
        throw ex;
      }
      return returnValue;
    }

    private bool IsReTryableException(Exception ex)
    {
      switch (ex)
      {
        case TargetUnreachableException _:
        case TargetDisconnectedException _:
        case ClientOfflineException _:
          return true;
        case InvalidOperationException _:
          return ex.Message.StartsWith("Unable to connect to any address in the config", StringComparison.OrdinalIgnoreCase);
        default:
          return false;
      }
    }

    public void Dispose()
    {
      ILogger logger = DiagUtility.LogManager.GetLogger("Metrics.CacheStore");
      if (this.instance != null)
        this.instance.Decrement();
      logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() =>
      {
        LogFields info = new LogFields().Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceId, this.instance.Id).Set<int>(HazelCastCacheStore.LogFieldNames.QueuedInstanceRefCount, this.instance.Refcount).Set<string>(HazelCastCacheStore.LogFieldNames.Action, "Disposed").Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) this.sw.ElapsedMilliseconds);
        logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (HazelcastConnection), "Disposing HazelcastConnection using QueuedInstance: " + (object) this.instance.Id, info);
      }));
      this.instance = (QueuedInstance) null;
    }
  }
}
