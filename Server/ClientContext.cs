// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ClientContext
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Configuration;
using EllieMae.EMLite.ReportingDbUtils.Interfaces;
using EllieMae.EMLite.Server.Configuration;
using EllieMae.EMLite.Server.ServerCommon;
using EllieMae.EMLite.Server.Tasks;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Listeners;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class ClientContext : IClientContext, IGlobalSettingsFactory
  {
    private const string className = "ClientContext�";
    private static Hashtable contexts = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static readonly ConcurrentDictionary<string, object> creationLocks = new ConcurrentDictionary<string, object>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static readonly CacheStoreSource _defaultCacheStoreSource = CacheStoreSource.Unknown;
    private static readonly string _cacheInstanceSwitch = (string) null;
    private const string staticInstanceConfigCacheKeyEBS = "Static.Instance.Configuration.EBS�";
    private const string staticInstanceConfigCacheKeyCore = "Static.Instance.Configuration.Core�";
    private const string instanceConfigCacheKeyEBS = "Instance.Configuration.EBS�";
    private const string instanceConfigCacheKeyCore = "Instance.Configuration.Core�";
    private EnGlobalSettings cachedSettings;
    private DateTime LastInstanceConfigDateTime = DateTime.MinValue;
    [ThreadStatic]
    private static ContextContainer _currentContainer = (ContextContainer) null;
    private string instanceName;
    private DateTime instanceStartTime;
    private ServerSettings settings;
    private ClientCache cache;
    private EllieMae.EMLite.Server.Tasks.ConcurrentUpdateNotificationHandler concurrentUpdateNotificationHandler;
    private DateTime concurrentUpdateNotificationSubscriptionTime = DateTime.MaxValue;
    private EllieMae.EMLite.Server.Tasks.TradeLoanUpdateNotificationHandler tradeLoanUpdateNotificationHandler;
    private DateTime tradeLoanUpdateNotificationSubscriptionTime = DateTime.MaxValue;
    private EllieMae.EMLite.Server.Tasks.LockComparisonFieldsNotificationHandler lockComparisonFieldsNotificationHandler;
    private DateTime lockComparisonFieldsNotificationSubscriptionTime = DateTime.MaxValue;
    private IContextTraceLog traceLog;
    private IDiagConfigChangeHandler<ServerDiagConfigData> logConfigChangeHandler;
    private LogListenerCollection logListenerCollection;
    private ISessionManager sessions;
    private bool isDefault;
    private TaskQueue taskQueue;
    private string encompassSystemID;
    private int allowConcurrentEditing = -1;
    private const string erdbCacheName = "ERDBCache�";

    private static ContextContainer currentContainer
    {
      get
      {
        return EncompassServerMode.Service == EncompassServer.ServerMode ? CallContext.LogicalGetData("CurrentContextContainer") as ContextContainer : ClientContext._currentContainer;
      }
      set
      {
        if (EncompassServerMode.Service == EncompassServer.ServerMode)
        {
          if (value == null)
            CallContext.FreeNamedDataSlot("CurrentContextContainer");
          else
            CallContext.LogicalSetData("CurrentContextContainer", (object) value);
        }
        else
          ClientContext._currentContainer = value;
      }
    }

    static ClientContext()
    {
      string str = CacheStoreConfiguration.CurrentConfiguration?.InstanceSwitch;
      if (EncompassServerMode.Service == EncompassServer.ServerMode || EncompassServerMode.Offline == EncompassServer.ServerMode)
      {
        ClientContext._defaultCacheStoreSource = CacheStoreSource.Disabled;
      }
      else
      {
        ClientContext._defaultCacheStoreSource = CacheStoreSource.InProcess;
        if (string.IsNullOrEmpty(ClientContext._cacheInstanceSwitch))
          str = "EncompassServer";
      }
      ClientContext._cacheInstanceSwitch = str;
      DiagUtility.SetLoggerScopeProvider((ILoggerScopeProvider) new ServerScopeProvider());
      DiagConfig<ServerDiagConfigData>.Instance.AddHandler((IDiagConfigChangeHandler<ServerDiagConfigData>) new GlobalLogConfigChangeHandler());
    }

    public bool IsConcurrentUpdateNotificationEnabled
    {
      get
      {
        return EncompassServer.ServerMode != EncompassServerMode.Service && EncompassServer.ServerMode != EncompassServerMode.Offline && Company.GetCompanySetting((IClientContext) this, "FEATURE", "ConcurrentUpdateNotification").ToLower() == "true";
      }
    }

    public IConcurrentUpdateNotificationHandler ConcurrentUpdateNotificationHandler
    {
      get
      {
        if (!this.IsConcurrentUpdateNotificationEnabled)
          return (IConcurrentUpdateNotificationHandler) null;
        if (this.concurrentUpdateNotificationHandler != null)
          return (IConcurrentUpdateNotificationHandler) this.concurrentUpdateNotificationHandler;
        this.concurrentUpdateNotificationHandler = new EllieMae.EMLite.Server.Tasks.ConcurrentUpdateNotificationHandler(this);
        this.concurrentUpdateNotificationSubscriptionTime = DateTime.Now;
        return (IConcurrentUpdateNotificationHandler) this.concurrentUpdateNotificationHandler;
      }
    }

    public DateTime ConcurrentUpdateNotificationSubscriptionTime
    {
      get => this.concurrentUpdateNotificationSubscriptionTime;
    }

    public ITradeLoanUpdateNotificationHandler TradeLoanUpdateNotificationHandler
    {
      get
      {
        if (!this.IsTradeLoanUpdateNotificationEnabled)
          return (ITradeLoanUpdateNotificationHandler) null;
        if (this.tradeLoanUpdateNotificationHandler != null)
          return (ITradeLoanUpdateNotificationHandler) this.tradeLoanUpdateNotificationHandler;
        this.tradeLoanUpdateNotificationHandler = new EllieMae.EMLite.Server.Tasks.TradeLoanUpdateNotificationHandler(this);
        this.tradeLoanUpdateNotificationSubscriptionTime = DateTime.Now;
        return (ITradeLoanUpdateNotificationHandler) this.tradeLoanUpdateNotificationHandler;
      }
    }

    public bool IsTradeLoanUpdateNotificationEnabled
    {
      get
      {
        return EncompassServer.ServerMode != EncompassServerMode.Service && EncompassServer.ServerMode != EncompassServerMode.Offline && Company.GetCompanySetting((IClientContext) this, "FEATURE", "TradeLoanUpdateNotification").ToLower() == "true";
      }
    }

    public DateTime TradeLoanUpdateNotificationSubscriptionTime
    {
      get => this.tradeLoanUpdateNotificationSubscriptionTime;
    }

    public ILockComparisonFieldsNotificationHandler LockComparisonFieldsNotificationHandler
    {
      get
      {
        if (!this.IsNotEBSOrOffline)
          return (ILockComparisonFieldsNotificationHandler) null;
        if (this.lockComparisonFieldsNotificationHandler != null)
          return (ILockComparisonFieldsNotificationHandler) this.lockComparisonFieldsNotificationHandler;
        this.lockComparisonFieldsNotificationHandler = new EllieMae.EMLite.Server.Tasks.LockComparisonFieldsNotificationHandler(this);
        this.lockComparisonFieldsNotificationSubscriptionTime = DateTime.Now;
        return (ILockComparisonFieldsNotificationHandler) this.lockComparisonFieldsNotificationHandler;
      }
    }

    public bool IsNotEBSOrOffline
    {
      get
      {
        return EncompassServer.ServerMode != EncompassServerMode.Service && EncompassServer.ServerMode != EncompassServerMode.Offline;
      }
    }

    public DateTime LockComparisonFieldsNotificationSubscriptionTime
    {
      get => this.lockComparisonFieldsNotificationSubscriptionTime;
    }

    private ClientContext(string instanceName)
    {
      this.instanceName = instanceName;
      this.instanceStartTime = DateTime.UtcNow;
      this.isDefault = string.Compare(instanceName, EnConfigurationSettings.InstanceName, true) == 0;
      try
      {
        EnGlobalSettings globalSettings = new EnGlobalSettings(this.InstanceName, this.GetInstanceConfiguration());
        this.cache = new ClientCache((IClientContext) this, this.GetCacheStore(this.GetCacheStoreSource(globalSettings)));
        this.settings = new ServerSettings(this, globalSettings);
        this.traceLog = (IContextTraceLog) new ContextTraceLog(this);
        this.sessions = (ISessionManager) new SessionManager((IClientContext) this);
        this.taskQueue = new TaskQueue(this);
        this.logListenerCollection = new LogListenerCollection();
        using (this.cache.EnterContext())
        {
          this.logConfigChangeHandler = DiagConfig<ServerDiagConfigData>.Instance.AddHandler((IDiagConfigChangeHandler<ServerDiagConfigData>) new InstanceLogConfigChangeHandler(this.logListenerCollection, this));
          if (this.IsConcurrentUpdateNotificationEnabled)
            this.ConcurrentUpdateNotificationHandler.StartProcess();
          if (this.IsTradeLoanUpdateNotificationEnabled)
            this.TradeLoanUpdateNotificationHandler.StartProcess();
          if (EncompassServer.ServerMode != EncompassServerMode.Service)
          {
            if (!this.settings.Disabled)
            {
              if (!EncompassServer.IsRunningInProcess)
                LicenseManager.ReleaseAllLicenses(this);
            }
          }
        }
        try
        {
          DataAccessBootstrapper.Initialize();
        }
        catch
        {
        }
        IocContainer.Register();
        if (EncompassServer.ServerMode != EncompassServerMode.Service)
          ServerStatusMonitor.NotifyAlive(this);
        this.settings.EnableRegistryMonitoring();
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(nameof (ClientContext), "Error starting ClientContext '" + this.instanceName + "': " + (object) ex);
        if (this.traceLog != null)
          this.traceLog.Dispose();
        if (this.sessions != null)
          this.sessions.Dispose();
        if (ex is ServerException)
          throw;
        else
          Err.Raise((IClientContext) null, nameof (ClientContext), (ServerException) new SecurityException("Error starting ClientContext '" + this.instanceName + "': " + ex.Message));
      }
    }

    public void ResetConcurrentUpdateNotificationSubscription()
    {
      lock (this)
      {
        if (this.concurrentUpdateNotificationHandler != null)
        {
          this.concurrentUpdateNotificationHandler.StopProcess();
          this.concurrentUpdateNotificationHandler = (EllieMae.EMLite.Server.Tasks.ConcurrentUpdateNotificationHandler) null;
          this.concurrentUpdateNotificationSubscriptionTime = DateTime.MaxValue;
        }
        if (!this.IsConcurrentUpdateNotificationEnabled)
          return;
        this.ConcurrentUpdateNotificationHandler.StartProcess();
        this.concurrentUpdateNotificationSubscriptionTime = DateTime.Now;
      }
    }

    public void ResetTradeLoanUpdateNotificationSubscription()
    {
      lock (this)
      {
        if (this.tradeLoanUpdateNotificationHandler != null)
        {
          this.tradeLoanUpdateNotificationHandler.StopProcess();
          this.tradeLoanUpdateNotificationHandler = (EllieMae.EMLite.Server.Tasks.TradeLoanUpdateNotificationHandler) null;
          this.tradeLoanUpdateNotificationSubscriptionTime = DateTime.MaxValue;
        }
        if (!this.IsTradeLoanUpdateNotificationEnabled)
          return;
        this.TradeLoanUpdateNotificationHandler.StartProcess();
        this.tradeLoanUpdateNotificationSubscriptionTime = DateTime.Now;
      }
    }

    public void ResetLockComparisonFieldsNotificationSubscription()
    {
      lock (this)
      {
        if (this.lockComparisonFieldsNotificationHandler != null)
        {
          this.lockComparisonFieldsNotificationHandler.StopProcess();
          this.lockComparisonFieldsNotificationHandler = (EllieMae.EMLite.Server.Tasks.LockComparisonFieldsNotificationHandler) null;
          this.lockComparisonFieldsNotificationSubscriptionTime = DateTime.MaxValue;
        }
        if (!this.IsNotEBSOrOffline)
          return;
        this.LockComparisonFieldsNotificationHandler.StartProcess();
        this.lockComparisonFieldsNotificationSubscriptionTime = DateTime.Now;
      }
    }

    public bool IPRestrictionSetting
    {
      get => Convert.ToBoolean(this.Settings.GetServerSetting("Policies.IPRestriction"));
    }

    public bool IPRestrictionSettingAllApplications
    {
      get
      {
        return Convert.ToBoolean(this.Settings.GetServerSetting("Policies.IPRestrictionAllApplications", false));
      }
    }

    internal bool? IsTPOClient
    {
      get
      {
        object obj = this.Cache.Get(nameof (IsTPOClient));
        return obj == null ? new bool?() : new bool?((bool) obj);
      }
      set => this.Cache.Put(nameof (IsTPOClient), (object) value);
    }

    public void ClearIsTPOClient() => this.Cache.Remove("IsTPOClient");

    public string InstanceName => this.instanceName;

    public DateTime InstanceStartTime => this.instanceStartTime;

    public bool VirtualUser { get; set; }

    public bool AllowConcurrentEditing
    {
      get
      {
        if (this.allowConcurrentEditing < 0)
          this.allowConcurrentEditing = (bool) this.Settings.GetServerSetting("Policies.AllowConcurrentEditing") ? 1 : 0;
        return this.allowConcurrentEditing == 1;
      }
    }

    public bool EnableLoanSoftArchival
    {
      get => Convert.ToBoolean(this.Settings.GetServerSetting("Policies.EnableLoanSoftArchival"));
    }

    public bool ExclusiveLockCurrLoginsOnly
    {
      get => (bool) this.Settings.GetServerSetting("Unpublished.ExclusiveLockCurrLoginsOnly");
    }

    public bool UseERDB => false;

    public IServerSettings Settings => (IServerSettings) this.settings;

    public IClientCache Cache => (IClientCache) this.cache;

    public IContextTraceLog TraceLog => this.traceLog;

    public void ResetTraceLog()
    {
      int num = this.isDefault ? 1 : 0;
    }

    public ISessionManager Sessions => this.sessions;

    public TaskQueue TaskQueue => this.taskQueue;

    public bool IsDefault() => this.isDefault;

    public IRequestContext MakeCurrent(
      IDataCache requestCache = null,
      string correlationId = null,
      Guid? transactionId = null,
      bool? forcePrimaryDB = null)
    {
      ContextContainer currentContainer = ClientContext.currentContainer;
      if (currentContainer != null)
      {
        if (currentContainer.Context == this)
        {
          ContextContainer contextContainer = new ContextContainer(currentContainer, new StackTrace(1), forcePrimaryDB);
          ClientContext.currentContainer = contextContainer;
          contextContainer.Recordstartime();
          return (IRequestContext) contextContainer;
        }
        ServerGlobals.TraceLog.WriteError(nameof (ClientContext), "Context.MakeCurrent() called for context '" + this.instanceName + "' while context '" + ClientContext.currentContainer.Context.InstanceName + "' is current. Previous context was replaced " + ClientContext.currentContainer.StackTraceString);
      }
      if (EncompassServerMode.Service == EncompassServer.ServerMode && HttpContext.Current != null && string.IsNullOrEmpty(correlationId))
      {
        correlationId = HttpContext.Current.Request.Headers["X-Correlation-ID"];
        if (string.IsNullOrEmpty(correlationId))
        {
          correlationId = HttpContext.Current.Request.Headers["CorrelationID"];
          if (string.IsNullOrEmpty(correlationId))
          {
            correlationId = Guid.NewGuid().ToString();
            HttpContext.Current.Request.Headers["X-Correlation-ID"] = correlationId;
          }
        }
      }
      if (EncompassServerMode.HTTP == EncompassServer.ServerMode && string.IsNullOrEmpty(correlationId))
        correlationId = GCUtility.GetCurrentCorrelationIdentifer();
      ContextContainer contextContainer1 = new ContextContainer(this, new StackTrace(1), requestCache, correlationId, transactionId, forcePrimaryDB);
      try
      {
        PerformanceMeter.FilePath = this.settings.GetLogFolderPath("PerfLogs" + (string.IsNullOrEmpty(this.InstanceName) ? "" : "." + this.InstanceName), true);
      }
      catch
      {
      }
      contextContainer1.Recordstartime();
      ClientContext.currentContainer = contextContainer1;
      return (IRequestContext) contextContainer1;
    }

    public void RecordLogflag() => ClientContext.currentContainer.RecordLogflag(true);

    public void RecordClassName(string c) => ClientContext.currentContainer.RecordClassName(c);

    public void RecordApiName(string a) => ClientContext.currentContainer.RecordApiName(a);

    public void RecordParms(object[] p) => ClientContext.currentContainer.RecordParms(p);

    public void AddParm(object p) => ClientContext.currentContainer.addParm(p);

    public void RecordSession(ISession s) => ClientContext.currentContainer.RecordSession(s);

    public void RecordAssemblyName(string s)
    {
      ClientContext.currentContainer.RecordAssemblyName(s);
    }

    public void Release()
    {
      ContextContainer currentContainer = ClientContext.currentContainer;
      if (currentContainer == null)
        this.traceLog.WriteWarning(nameof (ClientContext), "Context.Release called when no context is current " + new StackTrace(1).ToString());
      else if (currentContainer.Context != this)
        this.traceLog.WriteWarning(nameof (ClientContext), "Context.Release called on non-current context " + new StackTrace(1).ToString());
      else
        ClientContext.currentContainer = currentContainer.Parent;
    }

    public void RefreshCache(bool async)
    {
      if (async)
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.refreshCacheInternal), (object) true);
      else
        this.refreshCacheInternal((object) false);
    }

    public ERDBCache ERDBCache
    {
      get
      {
        return this.Cache.Get<ERDBCache>(nameof (ERDBCache), (Func<ERDBCache>) (() => new ERDBCache(this, this.instanceName, this.ClientID, this.EncompassSystemID)));
      }
    }

    public void ClearERDBCache()
    {
      using (this.Cache.Lock("ERDBCache"))
        this.Cache.Remove("ERDBCache");
    }

    public IERDBRegistrationMgr ERDBRegistrationMgr => this.ERDBCache.ERDBRegistrationMgr;

    public IERDBSession ERDBSession => this.ERDBCache.ERDBSession;

    public string ERDBServer => this.ERDBCache.ERDBServer;

    public int ERDBServerPort => this.ERDBCache.ERDBServerPort;

    public void ResetRemotingInterfaces() => this.ERDBCache.ResetRemotingInterfaces();

    public string ClientID
    {
      get
      {
        return this.Cache.Get<string>("ClientContext.ClientID", (Func<string>) (() => Company.GetCompanyInfo((IClientContext) this).ClientID));
      }
    }

    public string EncompassSystemID
    {
      get
      {
        if (this.encompassSystemID == null)
          this.encompassSystemID = EncompassSystemDbAccessor.GetEncompassSystemInfo((IClientContext) this).SystemID;
        return this.encompassSystemID;
      }
    }

    private void refreshCacheInternal(object asyncObj)
    {
      bool flag = (bool) asyncObj;
      try
      {
        using (this.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          this.Cache.ClearAll();
          lock (this)
            this.cachedSettings = (EnGlobalSettings) null;
          foreach (ISession session in this.Sessions)
            session.SecurityManager.RefreshUser();
        }
      }
      catch (Exception ex)
      {
        if (flag)
          this.TraceLog.WriteError(nameof (ClientContext), "Error flushing server cache for instance '" + this.instanceName + "': " + (object) ex);
        else
          throw;
      }
    }

    public void ResetCacheStore(bool force)
    {
      using (this.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        if (force)
          this.Cache.Clear();
        CacheStoreSource cacheStoreSource1 = this.Cache.CacheStoreSource;
        CacheStoreSource cacheStoreSource2 = this.GetCacheStoreSource(new EnGlobalSettings(this.InstanceName, this.GetInstanceConfiguration()));
        if (cacheStoreSource1 != cacheStoreSource2 | force)
        {
          this.Cache.ResetCacheStore(this.GetCacheStore(cacheStoreSource2));
          lock (this)
            this.cachedSettings = (EnGlobalSettings) null;
          foreach (ISession session in this.Sessions)
            session.SecurityManager.RefreshUser();
        }
        else
        {
          lock (this)
            this.cachedSettings = (EnGlobalSettings) null;
        }
      }
    }

    public static void Init()
    {
      Trace.WriteLine("Application starts at " + DateTime.Now.ToString());
    }

    public static ClientContext Open(string instanceName) => ClientContext.Open(instanceName, true);

    public static ClientContext Open(string instanceName, bool translateDefaultInstance)
    {
      if (translateDefaultInstance && (instanceName ?? "") == "")
        instanceName = EnConfigurationSettings.InstanceName;
      ClientContext clientContext = ClientContext.Get(instanceName);
      if (clientContext == null)
      {
        lock (ClientContext.creationLocks.GetOrAdd(instanceName ?? "", (Func<string, object>) (key => new object())))
        {
          clientContext = ClientContext.Get(instanceName);
          if (clientContext != null)
            return clientContext;
          clientContext = InstanceConfiguration.Provider == null || !(InstanceConfiguration.Provider is RegistryConfigurationProvider) || ClientContext.isInstanceConfigured(instanceName) ? new ClientContext(instanceName) : throw new InvalidOperationException(string.Format("The operation you attempted is invalid. The instance {0} is not configured on this server {1}.", (object) instanceName, (object) Environment.MachineName));
          lock (ClientContext.contexts)
            ClientContext.contexts.Add((object) instanceName, (object) clientContext);
          clientContext.TaskQueue.Activate();
        }
      }
      return clientContext;
    }

    private static bool isInstanceConfigured(string instanceName)
    {
      return new RegistryInstanceConfiguration(instanceName).RegistryHive.Exists();
    }

    public static ClientContext GetCurrent() => (ClientContext) ClientContext.GetCurrent(true);

    public static IClientContext GetCurrent(bool throwOnMissing)
    {
      if (ClientContext.currentContainer == null)
      {
        if (throwOnMissing)
          Err.Raise(TraceLevel.Error, nameof (ClientContext), new ServerException("ClientContext not set for current execution path"));
        return (IClientContext) null;
      }
      if (ClientContext.currentContainer.CurrentThreadId != Thread.CurrentThread.ManagedThreadId & throwOnMissing)
        ServerGlobals.TraceLog.WriteWarning(nameof (ClientContext), string.Format("ClientContext.GetCurrent() called without calling ClientContext.MakeCurrent(). Container's thread id: {0}. Current thread id: {1}. Stack Trace: {2}", (object) ClientContext.currentContainer.CurrentThreadId, (object) Thread.CurrentThread.ManagedThreadId, (object) new StackTrace(1)));
      return ClientContext.currentContainer.Context;
    }

    public static ClientContext[] GetAll() => ClientContext.GetAll(false);

    public static ClientContext[] GetAll(bool includeDisabled)
    {
      ClientContext[] source = (ClientContext[]) null;
      lock (ClientContext.contexts)
        source = ClientContext.contexts.Values.OfType<ClientContext>().ToArray<ClientContext>();
      return includeDisabled ? source : ((IEnumerable<ClientContext>) source).Where<ClientContext>((Func<ClientContext, bool>) (context => !context.Settings.Disabled)).ToArray<ClientContext>();
    }

    public static int ClientContextCount
    {
      get => ClientContext.contexts != null ? ClientContext.contexts.Count : 0;
    }

    public static ClientContext Get(string instanceName) => ClientContext.Get(instanceName, true);

    public static ClientContext Get(string instanceName, bool translateDefaultInstance)
    {
      if (translateDefaultInstance && instanceName == "")
        instanceName = EnConfigurationSettings.InstanceName;
      lock (ClientContext.contexts)
        return (ClientContext) ClientContext.contexts[(object) instanceName];
    }

    public IDataCache CurrentRequestCache
    {
      get
      {
        return ClientContext.currentContainer != null && ClientContext.currentContainer.Context == this ? ClientContext.currentContainer.RequestCache : (IDataCache) null;
      }
    }

    public static IRequestContext CurrentRequest
    {
      get => (IRequestContext) ClientContext.currentContainer;
    }

    private ICacheStore GetCacheStore(CacheStoreSource cacheStoreSource)
    {
      if (cacheStoreSource != CacheStoreSource.HazelCast)
        return cacheStoreSource == CacheStoreSource.Disabled ? (ICacheStore) new InProcCacheStore(false) : (ICacheStore) new InProcCacheStore(true);
      try
      {
        return (ICacheStore) new HazelCastCacheStore(this.InstanceName, "CACHE");
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(nameof (ClientContext), "HazelCastCacheStore initialization failed: " + ex.GetFullStackTrace());
        return (ICacheStore) new InProcCacheStore(true);
      }
    }

    private CacheStoreSource GetCacheStoreSource(EnGlobalSettings globalSettings)
    {
      if (!string.IsNullOrEmpty(this.instanceName) && !string.IsNullOrEmpty(ClientContext._cacheInstanceSwitch))
      {
        Hashtable companySettingsFromDb = Company.GetCompanySettingsFromDB(globalSettings.DatabaseConnectionString, "EnableHazelCast");
        if (this.IsHazelCastEnabled(companySettingsFromDb, "EncompassServer") && this.IsHazelCastEnabled(companySettingsFromDb, ClientContext._cacheInstanceSwitch))
          return CacheStoreSource.HazelCast;
      }
      return ClientContext._defaultCacheStoreSource;
    }

    private bool IsHazelCastEnabled(Hashtable settings, string instanceSwitch)
    {
      bool result;
      return ((!settings.ContainsKey((object) instanceSwitch) ? 0 : (bool.TryParse(settings[(object) instanceSwitch] as string, out result) ? 1 : 0)) & (result ? 1 : 0)) != 0;
    }

    public IInstanceConfiguration GetInstanceConfiguration()
    {
      return (IInstanceConfiguration) new StaticInstanceConfiguration(InstanceConfiguration.Provider.GetConfiguration(this.InstanceName));
    }

    private IInstanceConfiguration GetInstanceConfigurationFromCache()
    {
      return this.Cache.Get<IInstanceConfiguration>(EncompassServer.ServerMode == EncompassServerMode.Service ? "Static.Instance.Configuration.EBS" : "Static.Instance.Configuration.Core", (Func<IInstanceConfiguration>) (() => this.GetInstanceConfiguration()));
    }

    public EnGlobalSettings GetEnGlobalSettings()
    {
      string key = EncompassServer.ServerMode == EncompassServerMode.Service ? "Instance.Configuration.EBS" : "Instance.Configuration.Core";
      if (ClientContext.CurrentRequest != null && ClientContext.CurrentRequest.Context == this)
      {
        EnGlobalSettings enGlobalSettings = ClientContext.CurrentRequest.RequestCache.Get<EnGlobalSettings>(key);
        if (enGlobalSettings != null)
          return enGlobalSettings;
      }
      if (this.cachedSettings == null)
      {
        lock (this)
        {
          if (this.cachedSettings == null)
          {
            this.cachedSettings = new EnGlobalSettings(this.InstanceName, this.GetInstanceConfigurationFromCache());
            this.LastInstanceConfigDateTime = DateTime.Now;
          }
        }
      }
      if (ClientContext.CurrentRequest != null && ClientContext.CurrentRequest.Context == this)
        ClientContext.CurrentRequest.RequestCache.Set<EnGlobalSettings>(key, this.cachedSettings);
      return this.cachedSettings;
    }

    public void WriteToLogTargets(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      this.logListenerCollection.WriteToLogTargets(log);
    }

    public bool IsActiveFor(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      return this.logListenerCollection.IsActiveFor(log);
    }

    public void RemoveConfigChangeHandler()
    {
      if (this.logConfigChangeHandler == null)
        return;
      DiagConfig<ServerDiagConfigData>.Instance.RemoveHandler(this.logConfigChangeHandler);
      this.logConfigChangeHandler = (IDiagConfigChangeHandler<ServerDiagConfigData>) null;
    }
  }
}
