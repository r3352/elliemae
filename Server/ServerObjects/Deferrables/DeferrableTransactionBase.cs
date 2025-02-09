// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.DeferrableTransactionBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public abstract class DeferrableTransactionBase : IDeferrableTransaction, IDisposable
  {
    private Dictionary<Type, object> _processorFactories = new Dictionary<Type, object>();
    private Dictionary<string, string> _processorErrors = new Dictionary<string, string>();
    private Dictionary<string, IDeferrableProcessor> _injectedProcessors = new Dictionary<string, IDeferrableProcessor>();
    private bool _switchedToRealTimeMode;
    private Dictionary<string, DeferrableProcessStatus> _processStatus = new Dictionary<string, DeferrableProcessStatus>();

    public IDeferrableTransactionContext CurrentContext { get; private set; }

    public DeferrableType DeferrableType { get; private set; }

    protected bool Initialized { get; set; }

    protected PerformanceMeter Meter { get; set; }

    protected DeferrableTransactionBase(IDeferrableTransactionContext transactionContext)
    {
      this.CurrentContext = transactionContext;
      this.DeferrableType = DeferrableManager.GetDeferrableType(this.CurrentContext.Role);
    }

    public abstract void Initialize(PerformanceMeter meter = null);

    public abstract void Complete(DeferrableType? deferrableType = null);

    protected void SetDeferrableType(DeferrableType deferrableType)
    {
      this.DeferrableType = deferrableType;
    }

    public void RecordError(string key, string error)
    {
      if (this._processorErrors.ContainsKey(key))
        this._processorErrors[key] = error;
      else
        this._processorErrors.Add(key, error);
    }

    public string GetError(string key)
    {
      string error;
      this._processorErrors.TryGetValue(key, out error);
      return error;
    }

    public int GetErrorCount() => this._processorErrors.Count;

    public IDeferrableProcessor GetInjectedSuccessorProcessor(IDeferrableProcessor current)
    {
      return !this._injectedProcessors.ContainsKey(current.GetKey()) ? (IDeferrableProcessor) null : this._injectedProcessors[current.GetKey()];
    }

    public DeferrableTransactionBase InjectProcessorAfter(
      string[] predecessorKeys,
      IDeferrableProcessor deferrableProcessor)
    {
      foreach (string predecessorKey in predecessorKeys)
        this.InjectProcessorAfter(predecessorKey, deferrableProcessor);
      return this;
    }

    public DeferrableTransactionBase InjectProcessorAfter(
      string predecessorKey,
      IDeferrableProcessor deferrableProcessor)
    {
      if (this.Initialized)
        throw new Exception("Cannot inject processor after deferrable transaction initalization has been done.");
      if (this._injectedProcessors.ContainsKey(deferrableProcessor.GetKey()))
        throw new Exception("Cannot inject duplicate processor key.");
      this._injectedProcessors.Add(predecessorKey, deferrableProcessor);
      return this;
    }

    public DeferrableTransactionBase Inject<T>(
      IDeferrableProcessorFactory<T> deferrableProcessorFactory)
      where T : class
    {
      this._processorFactories[typeof (T)] = (object) deferrableProcessorFactory;
      return this;
    }

    public T CreateProcessor<T>() where T : class
    {
      IDeferrableProcessorFactory<T> processorFactory = this.GetProcessorFactory<T>();
      return processorFactory == null ? default (T) : processorFactory.CreateInstance();
    }

    public IDeferrableProcessorFactory<T> GetProcessorFactory<T>() where T : class
    {
      object obj = (object) null;
      return !this._processorFactories.TryGetValue(typeof (T), out obj) ? (IDeferrableProcessorFactory<T>) null : (IDeferrableProcessorFactory<T>) obj;
    }

    public void Dispose()
    {
      this._processorFactories.Clear();
      this._processorErrors.Clear();
      this.Initialized = false;
    }

    public bool SwitchedToRealTimeMode
    {
      get => this._switchedToRealTimeMode;
      set
      {
        this._switchedToRealTimeMode = value;
        if (!this._switchedToRealTimeMode)
          return;
        this.SetDeferrableType(DeferrableType.RealTime);
      }
    }

    public void SetProcessStatus(IDeferrableProcessor processor)
    {
      string key = processor.GetKey();
      DeferrableProcessStatus deferrableProcessStatus = processor.GetDeferrableType() == DeferrableType.Deferred ? DeferrableProcessStatus.DeferredProcessed : DeferrableProcessStatus.RealtimeProcessed;
      if (this._processStatus.ContainsKey(key))
        this._processStatus[key] = deferrableProcessStatus;
      else
        this._processStatus.Add(key, deferrableProcessStatus);
    }

    public bool NeedToRunRealTimeProcessor(IDeferrableProcessor processor)
    {
      string key = processor.GetKey();
      DeferrableProcessStatus deferrableProcessStatus = DeferrableProcessStatus.None;
      if (this._processStatus.ContainsKey(key))
        deferrableProcessStatus = this._processStatus[key];
      return deferrableProcessStatus != DeferrableProcessStatus.RealtimeProcessed;
    }
  }
}
