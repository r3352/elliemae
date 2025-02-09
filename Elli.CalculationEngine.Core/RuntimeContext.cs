// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.RuntimeContext
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class RuntimeContext : IDisposable
  {
    private const string ClassName = "RuntimeContext";
    public static readonly RuntimeContext Current = new RuntimeContext(AppDomain.CurrentDomain);
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

    private RuntimeContext(AppDomain domain)
    {
      try
      {
        this.domain = domain;
        this.initializeAssemblyCache();
      }
      catch (Exception ex)
      {
        Tracing.Log(TraceLevel.Error, nameof (RuntimeContext), "Error creating default runtime context: " + (object) ex);
        throw;
      }
    }

    private void initializeAssemblyCache()
    {
      this.types = (AssemblyCache) this.domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof (AssemblyCache).FullName);
    }

    ~RuntimeContext() => this.Dispose(false);

    public object CreateInstance(IElementIdentity elementId, Type interfaceType)
    {
      return this.types.CreateInstance(elementId, interfaceType);
    }

    public object CreateInstance(string assemblyName, string className, Type interfaceType)
    {
      return this.types.CreateInstance(assemblyName, className, interfaceType);
    }

    public Version CompileAssembly(
      string assemblyName,
      string sourceCode,
      CodeLanguage language,
      string[] dependentAssemblies,
      string[] resources,
      string assemblyPath,
      bool saveAssembly = false,
      bool optionStrict = false,
      bool includeDebugInfo = false)
    {
      return this.types.CompileAssembly(assemblyName, sourceCode, language, dependentAssemblies, resources, assemblyPath, saveAssembly, optionStrict, includeDebugInfo);
    }

    public Version LoadAssembly(string assemblyName, byte[] data)
    {
      return this.types.LoadAssembly(assemblyName, data);
    }

    public Version LoadAssembly(string assemblyName, string assemblyPath)
    {
      byte[] numArray = (byte[]) null;
      using (FileStream fileStream = File.OpenRead(assemblyPath))
      {
        numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, (int) fileStream.Length);
      }
      return this.LoadAssembly(assemblyName, numArray);
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
        Tracing.Log(TraceLevel.Verbose, nameof (RuntimeContext), "Queued AppDomain '" + domain.FriendlyName + "' for unloading");
      }
    }

    private static void runAppDomainCleanup()
    {
      try
      {
        Tracing.Log(TraceLevel.Info, nameof (RuntimeContext), "Started AppDomain cleanup thread");
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
              Tracing.Log(TraceLevel.Verbose, nameof (RuntimeContext), "Successfully unloaded AppDomain '" + key + "' after " + (object) appDomainContainer.RetryCount + " attempt(s)");
            }
            catch
            {
              Tracing.Log(TraceLevel.Verbose, nameof (RuntimeContext), "Failed to unload AppDomain '" + key + "' -- " + (object) appDomainContainer.RetryCount + " attempts made");
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
