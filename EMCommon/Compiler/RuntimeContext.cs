// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.RuntimeContext
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  public class RuntimeContext : IDisposable
  {
    private const string className = "RuntimeContext";
    public static readonly RuntimeContext Current = new RuntimeContext(AppDomain.CurrentDomain);
    private static string sw = Tracing.SwCommon;
    private static Hashtable domainsToUnload = new Hashtable();
    private AppDomain domain;
    private AssemblyCache types;

    static RuntimeContext()
    {
      new Thread(new ThreadStart(RuntimeContext.runAppDomainCleanup))
      {
        IsBackground = true,
        Priority = ThreadPriority.Lowest
      }.Start();
    }

    public static RuntimeContext Create()
    {
      if (EnConfigurationSettings.AppSettings.DisableCrossAppDomainRemoting)
        return RuntimeContext.Current;
      try
      {
        return new RuntimeContext();
      }
      catch (Exception ex)
      {
        Tracing.Log(RuntimeContext.sw, TraceLevel.Warning, nameof (RuntimeContext), "Error creating runtime context: " + (object) ex + ". The current AppDomain Context will be used.");
        return RuntimeContext.Current;
      }
    }

    private RuntimeContext()
    {
      AppDomainSetup setupInformation = AppDomain.CurrentDomain.SetupInformation;
      if (AssemblyResolver.IsSmartClient)
      {
        setupInformation.ApplicationBase = AssemblyResolver.GetResourceFileFolderPath("");
        setupInformation.ApplicationName = "FormBuilder - SmartClient";
      }
      this.domain = AppDomain.CreateDomain(Guid.NewGuid().ToString("N"), AppDomain.CurrentDomain.Evidence, setupInformation);
      try
      {
        this.initializeAssemblyCache();
        Tracing.Log(RuntimeContext.sw, TraceLevel.Verbose, nameof (RuntimeContext), "Created new AppDomain: " + this.domain.FriendlyName);
      }
      catch
      {
        AppDomain.Unload(this.domain);
        throw;
      }
    }

    private RuntimeContext(AppDomain domain)
    {
      try
      {
        this.domain = domain;
        this.initializeAssemblyCache();
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "ERROR", nameof (RuntimeContext), "Error creating default runtime context: " + (object) ex);
        throw;
      }
    }

    private void initializeAssemblyCache()
    {
      this.types = (AssemblyCache) this.domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof (AssemblyCache).FullName);
    }

    ~RuntimeContext() => this.Dispose(false);

    public object CreateInstance(TypeIdentifier typeId, System.Type interfaceType)
    {
      return this == RuntimeContext.Current ? this.types.CreateLocalInstance(typeId, interfaceType) : RemotingServices.Unmarshal(this.types.CreateInstance(typeId, interfaceType), true);
    }

    public TypeIdentifier[] GetTypes(string assemblyName, System.Type baseType)
    {
      return this.types.GetTypes(assemblyName, baseType);
    }

    public Assembly GetAssembly(string assemblyName) => this.types.GetAssembly(assemblyName);

    public TypeDescriptor GetTypeInformation(TypeIdentifier typeId)
    {
      return this.types.GetTypeInformation(typeId);
    }

    public bool IsAssemblyLoaded(string assemblyName) => this.types.IsAssemblyLoaded(assemblyName);

    public void CompileAssembly(
      string assemblyName,
      string sourceCode,
      CodeLanguage language,
      string[] dependentAssemblies)
    {
      this.types.CompileAssembly(assemblyName, sourceCode, language, dependentAssemblies);
    }

    public Assembly LoadAssembly(string assemblyPath, bool revertPluginChanges)
    {
      if (this != RuntimeContext.Current)
        throw new InvalidOperationException("Operation is only valid for primary RuntimeContext");
      return this.types.LoadAssembly(assemblyPath, revertPluginChanges);
    }

    public void LoadAssembly(string assemblyName, byte[] data)
    {
      this.types.LoadAssembly(assemblyName, data);
    }

    public AssemblyName GetAssemblyName(string assemblyName)
    {
      return this.types.GetAssemblyName(assemblyName);
    }

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
      try
      {
        if (this.domain != null && this.domain != AppDomain.CurrentDomain)
        {
          RuntimeContext.queueAppDomainForCleanup(this.domain);
          this.domain = (AppDomain) null;
        }
        if (!disposing)
          return;
        GC.SuppressFinalize((object) this);
      }
      catch
      {
      }
    }

    private static void queueAppDomainForCleanup(AppDomain domain)
    {
      lock (RuntimeContext.domainsToUnload)
      {
        RuntimeContext.domainsToUnload.Add((object) domain.FriendlyName, (object) new RuntimeContext.AppDomainContainer(domain));
        Monitor.Pulse((object) RuntimeContext.domainsToUnload);
        Tracing.Log(RuntimeContext.sw, TraceLevel.Verbose, nameof (RuntimeContext), "Queued AppDomain '" + domain.FriendlyName + "' for unloading");
      }
    }

    private static void runAppDomainCleanup()
    {
      try
      {
        Tracing.Log(RuntimeContext.sw, TraceLevel.Info, nameof (RuntimeContext), "Started AppDomain cleanup thread");
        while (true)
        {
          Hashtable hashtable = (Hashtable) null;
          lock (RuntimeContext.domainsToUnload)
          {
            while (RuntimeContext.domainsToUnload.Count == 0)
              Monitor.Wait((object) RuntimeContext.domainsToUnload);
            hashtable = (Hashtable) RuntimeContext.domainsToUnload.Clone();
          }
          ArrayList arrayList = new ArrayList();
          foreach (string key in (IEnumerable) hashtable.Keys)
          {
            RuntimeContext.AppDomainContainer appDomainContainer = (RuntimeContext.AppDomainContainer) hashtable[(object) key];
            ++appDomainContainer.RetryCount;
            try
            {
              AppDomain.Unload(appDomainContainer.AppDomain);
              arrayList.Add((object) key);
              Tracing.Log(RuntimeContext.sw, TraceLevel.Verbose, nameof (RuntimeContext), "Successfully unloaded AppDomain '" + key + "' after " + (object) appDomainContainer.RetryCount + " attempt(s)");
            }
            catch
            {
              Tracing.Log(RuntimeContext.sw, TraceLevel.Verbose, nameof (RuntimeContext), "Failed to unload AppDomain '" + key + "' -- " + (object) appDomainContainer.RetryCount + " attempts made");
            }
          }
          lock (RuntimeContext.domainsToUnload)
          {
            foreach (string key in arrayList)
              RuntimeContext.domainsToUnload.Remove((object) key);
          }
          Thread.Sleep(TimeSpan.FromSeconds(3.0));
        }
      }
      catch
      {
      }
    }

    private class AppDomainContainer
    {
      public AppDomain AppDomain;
      public int RetryCount;

      public AppDomainContainer(AppDomain domain) => this.AppDomain = domain;
    }
  }
}
