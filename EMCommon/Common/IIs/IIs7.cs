// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.IIs.IIs7
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Web.Administration;
using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common.IIs
{
  public class IIs7 : IWebServer
  {
    private ServerManager serverMngr;

    public IIs7()
    {
      try
      {
        this.serverMngr = new ServerManager();
      }
      catch
      {
      }
    }

    public ServerManager ServerManager => this.serverMngr;

    public bool IsInstalled()
    {
      try
      {
        return this.serverMngr != null && this.serverMngr.Sites.Count > 0;
      }
      catch
      {
        return false;
      }
    }

    public IWebSite GetWebSite(string name)
    {
      return this.serverMngr.Sites[name] == null ? (IWebSite) null : (IWebSite) new IIs7.WebSite(this, name);
    }

    public IWebSite GetWebSiteByID(string siteId)
    {
      foreach (IWebSite webSite in this.GetWebSites())
      {
        if (string.Compare(webSite.SiteID, siteId, true) == 0)
          return webSite;
      }
      return (IWebSite) null;
    }

    public IWebSite[] GetWebSites()
    {
      ArrayList arrayList = new ArrayList();
      foreach (Site site in (ConfigurationElementCollectionBase<Site>) this.serverMngr.Sites)
        arrayList.Add((object) new IIs7.WebSite(this, site.Name));
      return (IWebSite[]) arrayList.ToArray(typeof (IWebSite));
    }

    public bool SupportsApplicationPools => true;

    public IApplicationPool GetApplicationPool(string name)
    {
      return this.serverMngr.ApplicationPools[name] == null ? (IApplicationPool) null : (IApplicationPool) new IIs7.WebApplicationPool(this, name);
    }

    public IApplicationPool CreateApplicationPool(string name)
    {
      ApplicationPool applicationPool = this.serverMngr.ApplicationPools[name] ?? this.serverMngr.ApplicationPools.Add(name);
      applicationPool.Cpu.ResetInterval = TimeSpan.Zero;
      applicationPool.ManagedPipelineMode = ManagedPipelineMode.Classic;
      applicationPool.ManagedRuntimeVersion = "v4.0";
      applicationPool.Recycling.DisallowOverlappingRotation = true;
      applicationPool.Recycling.DisallowRotationOnConfigChange = true;
      applicationPool.Recycling.PeriodicRestart.Requests = 0L;
      applicationPool.Recycling.PeriodicRestart.Memory = 0L;
      applicationPool.Recycling.PeriodicRestart.PrivateMemory = 0L;
      applicationPool.Recycling.PeriodicRestart.Time = TimeSpan.Zero;
      if (applicationPool.Recycling.PeriodicRestart.Schedule.AllowsClear)
        applicationPool.Recycling.PeriodicRestart.Schedule.Clear();
      this.serverMngr.CommitChanges();
      return (IApplicationPool) new IIs7.WebApplicationPool(this, name);
    }

    public bool AreASPNETExtensionsInstalled()
    {
      foreach (ConfigurationElement configurationElement in (ConfigurationElementCollectionBase<ConfigurationElement>) this.serverMngr.GetApplicationHostConfiguration().GetSection("system.webServer/security/isapiCgiRestriction").GetCollection())
      {
        if (string.Concat(configurationElement.GetAttributeValue("groupId")) == "ASP.NET v4.0.30319")
          return true;
      }
      return false;
    }

    public virtual void InstallASPNETExtensions() => EllieMae.EMLite.Common.IIs.IIs.InstallASPNET();

    public void EnableASPNETExtensions()
    {
      foreach (ConfigurationElement configurationElement in (ConfigurationElementCollectionBase<ConfigurationElement>) this.serverMngr.GetApplicationHostConfiguration().GetSection("system.webServer/security/isapiCgiRestriction").GetCollection())
      {
        if (string.Concat(configurationElement["groupId"]) == "ASP.NET v4.0.30319")
          configurationElement["allowed"] = (object) true;
      }
      this.serverMngr.CommitChanges();
    }

    public class WebSite : IWebSite
    {
      private IIs7 server;
      private string name;

      public WebSite(IIs7 server, string name)
      {
        this.server = server;
        this.name = name;
      }

      public Site GetSite() => this.server.ServerManager.Sites[this.name];

      public Application GetRootApplication() => this.GetSite().Applications["/"];

      public IIs7 Server => this.server;

      public string SiteName => this.name;

      public string SiteID => string.Concat((object) this.GetSite().Id);

      public IWebRoot[] GetWebRoots()
      {
        ArrayList arrayList = new ArrayList();
        Site site = this.GetSite();
        foreach (Application application in (ConfigurationElementCollectionBase<Application>) site.Applications)
        {
          if (application.Path != "/")
            arrayList.Add((object) new IIs7.WebApplication(this, application.Path));
        }
        foreach (VirtualDirectory virtualDirectory in (ConfigurationElementCollectionBase<VirtualDirectory>) site.Applications["/"].VirtualDirectories)
        {
          if (virtualDirectory.Path != "/")
            arrayList.Add((object) new IIs7.WebDirectory(this, virtualDirectory.Path));
        }
        return (IWebRoot[]) arrayList.ToArray(typeof (IWebRoot));
      }

      public IWebRoot GetWebRoot(string vrootName)
      {
        foreach (IWebRoot webRoot in this.GetWebRoots())
        {
          if (string.Compare(webRoot.Name, vrootName, true) == 0)
            return webRoot;
        }
        return (IWebRoot) null;
      }

      public IWebRoot CreateVirtualDirectory(string vrootName, string path)
      {
        this.GetWebRoot(vrootName)?.Delete();
        if (!vrootName.StartsWith("/"))
          vrootName = "/" + vrootName;
        this.GetRootApplication().VirtualDirectories.Add(vrootName, path);
        this.server.ServerManager.CommitChanges();
        return (IWebRoot) new IIs7.WebDirectory(this, vrootName);
      }

      public IWebRoot CreateWebApplication(string vrootName, string path, string appPoolName)
      {
        this.GetWebRoot(vrootName)?.Delete();
        if (!vrootName.StartsWith("/"))
          vrootName = "/" + vrootName;
        this.GetSite().Applications.Add(vrootName, path).ApplicationPoolName = appPoolName;
        this.server.ServerManager.CommitChanges();
        return (IWebRoot) new IIs7.WebApplication(this, vrootName);
      }

      public string GetDefaultRootPath(string vrootName)
      {
        try
        {
          string physicalPath = this.GetRootApplication().VirtualDirectories["/"].PhysicalPath;
          if (physicalPath != "")
          {
            string path1 = physicalPath.Replace("%SystemDrive%", Environment.GetEnvironmentVariable("SystemDrive"));
            if (!path1.Contains("%"))
              return Path.Combine(path1, vrootName);
          }
        }
        catch
        {
        }
        try
        {
          return Path.Combine(Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\InetStp").GetValue("PathWWWRoot").ToString(), vrootName);
        }
        catch
        {
        }
        return "";
      }
    }

    public class WebApplication : IWebRoot
    {
      private IIs7.WebSite site;
      private string path;

      public WebApplication(IIs7.WebSite site, string path)
      {
        this.site = site;
        this.path = path;
      }

      public Application GetApplication() => this.site.GetSite().Applications[this.path];

      public string Name => this.path.Length > 1 ? this.path.Substring(1) : this.path;

      public string PhysicalPath
      {
        get
        {
          return this.GetApplication().VirtualDirectories["/"].PhysicalPath.Replace("%SystemDrive%", Environment.GetEnvironmentVariable("SystemDrive"));
        }
      }

      public void Delete()
      {
        Site site = this.site.GetSite();
        site.Applications.Remove(site.Applications[this.path]);
        this.site.Server.ServerManager.CommitChanges();
      }

      public bool IsApplication() => true;

      public void UnloadApplication()
      {
        int num = (int) this.site.Server.ServerManager.ApplicationPools[this.GetApplication().ApplicationPoolName].Recycle();
      }

      public IApplicationPool GetApplicationPool()
      {
        ApplicationPool applicationPool = this.site.Server.ServerManager.ApplicationPools[this.GetApplication().ApplicationPoolName];
        return applicationPool == null ? (IApplicationPool) null : (IApplicationPool) new IIs7.WebApplicationPool(this.site.Server, applicationPool.Name);
      }

      public bool AreASPNETMappingsRegistered()
      {
        return this.site.Server.ServerManager.ApplicationPools[this.GetApplication().ApplicationPoolName].ManagedRuntimeVersion == "v4.0";
      }

      public void ApplyASPNETScriptMaps()
      {
        this.site.Server.ServerManager.ApplicationPools[this.GetApplication().ApplicationPoolName].ManagedRuntimeVersion = "v4.0";
        this.site.Server.ServerManager.CommitChanges();
      }
    }

    public class WebDirectory : IWebRoot
    {
      private IIs7.WebSite site;
      private string path;

      public WebDirectory(IIs7.WebSite site, string path)
      {
        this.site = site;
        this.path = path;
      }

      public VirtualDirectory GetVirtualDirectory()
      {
        return this.site.GetRootApplication().VirtualDirectories[this.path];
      }

      public string Name => this.path.Length > 1 ? this.path.Substring(1) : this.path;

      public string PhysicalPath
      {
        get
        {
          return this.GetVirtualDirectory().PhysicalPath.Replace("%SystemDrive%", Environment.GetEnvironmentVariable("SystemDrive"));
        }
      }

      public void Delete()
      {
        Application rootApplication = this.site.GetRootApplication();
        rootApplication.VirtualDirectories.Remove(rootApplication.VirtualDirectories[this.path]);
        this.site.Server.ServerManager.CommitChanges();
      }

      public bool IsApplication() => false;

      public void UnloadApplication()
      {
      }

      public IApplicationPool GetApplicationPool()
      {
        throw new Exception("This method can only be applied to web applications");
      }

      public bool AreASPNETMappingsRegistered()
      {
        throw new Exception("This method can only be applied to web applications");
      }

      public void ApplyASPNETScriptMaps()
      {
        throw new Exception("This method can only be applied to web applications");
      }
    }

    public class WebApplicationPool : IApplicationPool
    {
      private IIs7 server;
      private string name;

      public WebApplicationPool(IIs7 server, string name)
      {
        this.server = server;
        this.name = name;
      }

      public ApplicationPool GetApplicationPool()
      {
        return this.server.ServerManager.ApplicationPools[this.name];
      }

      public string Name => this.name;

      public void Start()
      {
        try
        {
          int num = (int) this.GetApplicationPool().Start();
        }
        catch
        {
        }
      }

      public void Stop()
      {
        try
        {
          int num = (int) this.GetApplicationPool().Stop();
        }
        catch
        {
        }
      }

      public void Recycle()
      {
        try
        {
          int num = (int) this.GetApplicationPool().Recycle();
        }
        catch
        {
        }
      }

      public void Delete()
      {
        this.server.ServerManager.ApplicationPools.Remove(this.server.ServerManager.ApplicationPools[this.name]);
        this.server.ServerManager.CommitChanges();
      }

      public bool IsRunning()
      {
        ApplicationPool applicationPool = this.GetApplicationPool();
        return applicationPool.State == ObjectState.Started || applicationPool.State == ObjectState.Starting;
      }
    }
  }
}
