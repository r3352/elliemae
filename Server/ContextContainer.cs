// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContextContainer
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Listeners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  [Serializable]
  internal class ContextContainer : 
    IRequestContext,
    ILoggerScope,
    IDisposable,
    IDbQueryContext,
    ISerializable
  {
    private readonly ClientContext context;
    private readonly ContextContainer parent;
    private readonly string stackTraceString;
    private readonly string correlationId;
    private readonly Guid transactionId;
    private readonly int currentThreadId;
    private readonly int threadNestLevel;
    private readonly bool forcePrimaryDB;
    private bool logflag;
    private DateTime startTime;
    private string className;
    private string apiName;
    private ISession session;
    private IDisposable cacheContext;
    private string assemblyName;
    private object[] parms;
    public Dictionary<DBReadReplicaFeature, bool> ReadReplicaFlags = new Dictionary<DBReadReplicaFeature, bool>();

    public ContextContainer(
      ClientContext context,
      StackTrace stackTrace,
      IDataCache requestCache,
      string correlationId,
      Guid? transactionId,
      bool? forcePrimaryDB)
    {
      this.context = context;
      this.stackTraceString = stackTrace.ToString();
      this.RequestCache = requestCache ?? (IDataCache) new InProcessCache(false);
      this.correlationId = correlationId;
      Guid guid;
      if (transactionId.HasValue)
      {
        Guid? nullable = transactionId;
        Guid empty = Guid.Empty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
        {
          guid = transactionId.Value;
          goto label_4;
        }
      }
      guid = Guid.NewGuid();
label_4:
      this.transactionId = guid;
      this.currentThreadId = Thread.CurrentThread.ManagedThreadId;
      this.threadNestLevel = 1;
      this.forcePrimaryDB = ((int) forcePrimaryDB ?? 0) != 0;
      this.cacheContext = HzcLoanLockFactory.Instance?.EnterContext((IClientContext) context) ?? context.Cache.EnterContext();
    }

    public ContextContainer(ContextContainer parent, StackTrace stackTrace, bool? forcePrimaryDB)
    {
      this.context = parent.context;
      this.parent = parent;
      this.stackTraceString = stackTrace.ToString();
      this.RequestCache = parent.RequestCache;
      this.correlationId = parent.correlationId;
      this.transactionId = parent.transactionId;
      this.currentThreadId = Thread.CurrentThread.ManagedThreadId;
      this.forcePrimaryDB = ((int) forcePrimaryDB ?? (parent.forcePrimaryDB ? 1 : 0)) != 0;
      this.threadNestLevel = parent.CurrentThreadId == this.currentThreadId ? parent.threadNestLevel + 1 : 1;
    }

    public IDataCache RequestCache { get; private set; }

    public void RecordClassName(string c) => this.className = c;

    public void RecordApiName(string a) => this.apiName = a;

    public void RecordParms(object[] p) => this.parms = p;

    public void addParm(object p)
    {
      try
      {
        Array.Resize<object>(ref this.parms, this.parms.Length + 1);
        this.parms[this.parms.Length - 1] = p;
      }
      catch
      {
      }
    }

    public void RecordSession(ISession s) => this.session = s;

    public void RecordLogflag(bool flag) => this.logflag = flag;

    public void Recordstartime() => this.startTime = DateTime.UtcNow;

    public void RecordAssemblyName(string AssemblyName) => this.assemblyName = AssemblyName;

    public IClientContext Context => (IClientContext) this.context;

    public ContextContainer Parent => this.parent;

    public string StackTraceString => this.stackTraceString;

    public string CorrelationId => this.correlationId;

    public Guid? TransactionId => new Guid?(this.transactionId);

    public int CurrentThreadId => this.currentThreadId;

    public string Instance => this.Context.InstanceName;

    public int ThreadNestLevel => this.threadNestLevel;

    public bool ForcePrimaryDB => this.forcePrimaryDB;

    public void Dispose()
    {
      try
      {
        if (!this.logflag)
          return;
        TraceLog.WriteApiTime(this.session, this.className, this.apiName, this.startTime, this.parms);
      }
      finally
      {
        try
        {
          this.cacheContext?.Dispose();
        }
        catch
        {
        }
        this.context.Release();
      }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext streamingContext)
    {
      if (info == null)
        throw new ArgumentNullException("info: SerializationInfo class must be provided for GetObjectData method");
      info.AddValue("InstanceName", (object) this.Context.InstanceName);
      info.AddValue("StackTraceString", (object) this.StackTraceString);
    }

    public void AddListener(ILogListener logListener)
    {
      throw new NotSupportedException("Adding listeners is not supported");
    }

    public bool RemoveListener(ILogListener logListener, bool dispose)
    {
      throw new NotSupportedException("Removing listeners is not supported");
    }

    public IEnumerable<ILogListener> GetListeners()
    {
      throw new NotSupportedException("Getting listeners is not supported");
    }

    public void WriteToLogTargets(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      this.context.WriteToLogTargets(log);
    }

    public bool IsActiveFor(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      return this.context.IsActiveFor(log);
    }

    private ContextContainer(SerializationInfo info, StreamingContext streamingContext)
    {
      this.context = ClientContext.Get(info.GetString("InstanceName"));
      if (this.context == null)
        throw new Exception("ClientContext was missing in deserialization process");
      this.stackTraceString = info.GetString(nameof (StackTraceString));
    }

    public void OverrideDisableReadReplicaFor(DBReadReplicaFeature feature, bool? value)
    {
      if (value.HasValue)
        this.ReadReplicaFlags[feature] = value.Value;
      else
        this.ReadReplicaFlags.Remove(feature);
    }

    public bool? GetDisableReadReplicaOverrideFor(DBReadReplicaFeature feature)
    {
      bool flag;
      return this.ReadReplicaFlags.TryGetValue(feature, out flag) ? new bool?(flag) : new bool?();
    }
  }
}
