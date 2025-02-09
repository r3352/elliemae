// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.IIs.IIs6
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common.IIs
{
  public class IIs6 : IWebServer
  {
    public const string ADSIWebServicePath = "IIS://localhost/w3svc";

    public bool IsInstalled()
    {
      try
      {
        return new DirectoryEntry("IIS://localhost/w3svc").SchemaClassName == "IIsWebService";
      }
      catch
      {
        return false;
      }
    }

    public IWebSite GetWebSite(string name)
    {
      foreach (IWebSite webSite in this.GetWebSites())
      {
        if (string.Compare(webSite.SiteName, name, true) == 0)
          return webSite;
      }
      return (IWebSite) null;
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
      foreach (DirectoryEntry child in new DirectoryEntry("IIS://localhost/w3svc").Children)
      {
        if (child.SchemaClassName == "IIsWebServer")
          arrayList.Add((object) new IIs6.DirectoryEntryWrapper(child));
      }
      return (IWebSite[]) arrayList.ToArray(typeof (IWebSite));
    }

    public bool SupportsApplicationPools => EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs6;

    public IApplicationPool GetApplicationPool(string name)
    {
      DirectoryEntry e = new DirectoryEntry("IIS://localhost/w3svc/AppPools/" + name);
      try
      {
        if (e.SchemaClassName != "IIsApplicationPool")
          e = (DirectoryEntry) null;
      }
      catch
      {
        e = (DirectoryEntry) null;
      }
      if (e == null)
        return (IApplicationPool) null;
      return (IApplicationPool) new IIs6.DirectoryEntryWrapper(e);
    }

    public IApplicationPool CreateApplicationPool(string name)
    {
      IIs6.DirectoryEntryWrapper applicationPool = (IIs6.DirectoryEntryWrapper) this.GetApplicationPool(name);
      DirectoryEntry e = applicationPool == null ? new DirectoryEntry("IIS://localhost/w3svc/AppPools").Children.Add(name, "IIsApplicationPool") : applicationPool.DirectoryEntry;
      e.Properties["AppPoolRecycleTime"][0] = (object) false;
      e.Properties["AppPoolRecycleRequests"][0] = (object) false;
      e.Properties["AppPoolRecycleSchedule"][0] = (object) false;
      e.Properties["AppPoolRecycleMemory"][0] = (object) false;
      e.Properties["AppPoolRecycleConfigChange"][0] = (object) false;
      e.Properties["AppPoolRecyclePrivateMemory"][0] = (object) false;
      e.Properties["PeriodicRestartTime"][0] = (object) 0;
      e.Properties["IdleTimeout"][0] = (object) 0;
      e.Properties["DisallowOverlappingRotation"][0] = (object) true;
      e.Properties["DisallowRotationOnConfigChange"][0] = (object) true;
      e.CommitChanges();
      return (IApplicationPool) new IIs6.DirectoryEntryWrapper(e);
    }

    public bool AreASPNETExtensionsInstalled()
    {
      if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs5)
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\ASP.NET\\2.0.50727.0"))
          return registryKey != null && string.Concat(registryKey.GetValue("Path")) != "";
      }
      else
      {
        try
        {
          object[] objArray = (object[]) new DirectoryEntry("IIS://localhost/w3svc").Invoke("ListWebServiceExtensions");
          for (int index = 0; index < objArray.Length; ++index)
          {
            if (objArray[index].ToString().IndexOf("ASP.NET") >= 0 && objArray[index].ToString().IndexOf("v4.0") >= 0)
              return true;
          }
          return false;
        }
        catch
        {
          return false;
        }
      }
    }

    public void InstallASPNETExtensions() => EllieMae.EMLite.Common.IIs.IIs.InstallASPNET();

    public void EnableASPNETExtensions()
    {
      if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() != IIsVersion.IIs6)
        return;
      try
      {
        DirectoryEntry directoryEntry = new DirectoryEntry("IIS://localhost/w3svc");
        object[] objArray = (object[]) directoryEntry.Invoke("ListWebServiceExtensions");
        string str = (string) null;
        for (int index = 0; index < objArray.Length; ++index)
        {
          if (objArray[index].ToString().IndexOf("ASP.NET") >= 0 && objArray[index].ToString().IndexOf("v4.0") >= 0)
          {
            str = objArray[index].ToString();
            break;
          }
        }
        if (str == null)
          throw new ApplicationException("ASP.NET Server Extension not found");
        directoryEntry.Invoke("EnableWebServiceExtension", (object) str);
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Error attempting to enable ASP.NET Web Server Extensions: " + ex.Message);
      }
    }

    public class DirectoryEntryWrapper : IWebSite, IWebRoot, IApplicationPool
    {
      private DirectoryEntry dirEntry;

      public DirectoryEntryWrapper(DirectoryEntry e) => this.dirEntry = e;

      public DirectoryEntry DirectoryEntry => this.dirEntry;

      public string SiteName => string.Concat(this.dirEntry.Properties["ServerComment"][0]);

      public string SiteID => this.dirEntry.Name;

      public IWebRoot[] GetWebRoots()
      {
        ArrayList arrayList = new ArrayList();
        foreach (DirectoryEntry child in new DirectoryEntry(this.dirEntry.Path + "/Root").Children)
        {
          if (IIs6.DirectoryEntryWrapper.IsWebRoot(child))
            arrayList.Add((object) new IIs6.DirectoryEntryWrapper(child));
        }
        return (IWebRoot[]) arrayList.ToArray(typeof (IWebRoot));
      }

      public IWebRoot GetWebRoot(string vrootName)
      {
        if (!vrootName.StartsWith("/"))
          vrootName = "/" + vrootName;
        DirectoryEntry e = new DirectoryEntry(this.dirEntry.Path + "/Root" + vrootName);
        try
        {
          if (IIs6.DirectoryEntryWrapper.IsWebRoot(e))
            return (IWebRoot) new IIs6.DirectoryEntryWrapper(e);
        }
        catch
        {
        }
        return (IWebRoot) null;
      }

      public IWebRoot CreateVirtualDirectory(string vrootName, string path)
      {
        ((IIs6.DirectoryEntryWrapper) this.GetWebRoot(vrootName))?.Delete();
        DirectoryEntry e = new DirectoryEntry(this.dirEntry.Path + "/Root").Children.Add(vrootName, "IIsWebVirtualDir");
        e.Properties["Path"][0] = (object) path;
        e.Properties["AccessRead"][0] = (object) "True";
        e.Properties["AccessWrite"][0] = (object) "False";
        e.Properties["AccessExecute"][0] = (object) "False";
        e.Properties["AccessScript"][0] = (object) "True";
        e.Properties["AuthAnonymous"][0] = (object) "True";
        e.CommitChanges();
        return (IWebRoot) new IIs6.DirectoryEntryWrapper(e);
      }

      public IWebRoot CreateWebApplication(string vrootName, string path, string appPoolName)
      {
        IIs6.DirectoryEntryWrapper virtualDirectory = (IIs6.DirectoryEntryWrapper) this.CreateVirtualDirectory(vrootName, path);
        DirectoryEntry directoryEntry = virtualDirectory.DirectoryEntry;
        if ((int) directoryEntry.Invoke("AppGetStatus2") == 2)
        {
          if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs5)
            directoryEntry.Invoke("AppCreate2", (object) 2);
          else
            directoryEntry.Invoke("AppCreate3", (object) 0, (object) appPoolName, (object) true);
          directoryEntry.Properties["AppFriendlyName"][0] = (object) vrootName;
          directoryEntry.CommitChanges();
        }
        return (IWebRoot) virtualDirectory;
      }

      public string GetDefaultRootPath(string vrootName)
      {
        try
        {
          string path1 = string.Concat(new DirectoryEntry(this.dirEntry.Path + "/Root").Properties["Path"][0]);
          if (path1 != "")
            return Path.Combine(path1, vrootName);
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

      public string Name => this.dirEntry.Name;

      public void Delete()
      {
        try
        {
          this.dirEntry.Invoke("AppDelete");
        }
        catch
        {
        }
        try
        {
          DirectoryEntry parent = this.dirEntry.Parent;
          parent.Invoke(nameof (Delete), (object) this.dirEntry.SchemaClassName, (object) this.dirEntry.Name);
          parent.CommitChanges();
        }
        catch
        {
        }
      }

      public void UnloadApplication()
      {
        try
        {
          this.dirEntry.Invoke("AppUnload");
        }
        catch
        {
        }
      }

      public bool IsApplication() => IIs6.DirectoryEntryWrapper.IsApplication(this.dirEntry);

      private static bool IsWebRoot(DirectoryEntry e)
      {
        return e.SchemaClassName == "IIsWebVirtualDir" || IIs6.DirectoryEntryWrapper.IsApplication(e);
      }

      private static bool IsApplication(DirectoryEntry e)
      {
        try
        {
          return (int) e.Invoke("AppGetStatus2") != 2;
        }
        catch
        {
          return false;
        }
      }

      public string PhysicalPath
      {
        get
        {
          try
          {
            return string.Concat(this.dirEntry.Properties["Path"][0]);
          }
          catch
          {
          }
          try
          {
            return this.dirEntry.Parent.Properties["Path"][0].ToString() + "\\" + this.Name;
          }
          catch
          {
          }
          return "";
        }
      }

      public IApplicationPool GetApplicationPool()
      {
        if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs5)
          return (IApplicationPool) null;
        string name = string.Concat(this.dirEntry.Properties["AppPoolId"][0]);
        return name == "" ? (IApplicationPool) null : new IIs6().GetApplicationPool(name);
      }

      public bool AreASPNETMappingsRegistered()
      {
        PropertyValueCollection property = this.dirEntry.Properties["ScriptMaps"];
        for (int index = 0; index < property.Count; ++index)
        {
          string lower = property[index].ToString().ToLower();
          if (lower.IndexOf(".rem,") >= 0 && lower.IndexOf("v4.0.30319\\aspnet_isapi") >= 0)
            return true;
        }
        return false;
      }

      public void ApplyASPNETScriptMaps()
      {
        bool flag = false;
        if (EllieMae.EMLite.Common.IIs.IIs.IsRunning())
        {
          flag = true;
          try
          {
            EllieMae.EMLite.Common.IIs.IIs.StopService();
          }
          catch
          {
            EllieMae.EMLite.Common.IIs.IIs.RunIisReset();
          }
        }
        string aspnetInstallRoot = EllieMae.EMLite.Common.IIs.IIs.GetASPNETInstallRoot("v4.0.30319");
        string str1 = this.dirEntry.Path;
        int startIndex = str1.IndexOf("W3SVC/", StringComparison.CurrentCultureIgnoreCase);
        if (startIndex > 0)
          str1 = str1.Substring(startIndex);
        try
        {
          string str2 = Path.Combine(aspnetInstallRoot, "aspnet_regiis.exe");
          Process process = new Process();
          process.StartInfo.FileName = str2;
          process.StartInfo.Arguments = "-s " + str1;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
          process.StartInfo.CreateNoWindow = true;
          process.Start();
          process.WaitForExit();
          if (flag)
            EllieMae.EMLite.Common.IIs.IIs.StartService();
          if (process.ExitCode != 0)
            throw new ApplicationException("ASP.NET script map installation processs terminated with non-zero exit code");
        }
        catch (Exception ex)
        {
          throw new ApplicationException("An error occurred while updating the script maps for the Encompass web application: " + ex.Message);
        }
      }

      private static string getASPNETInstallRoot(string version)
      {
        string path1;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework"))
          path1 = (string) registryKey.GetValue("InstallRoot") ?? "";
        string path = path1 != null ? Path.Combine(path1, version) : throw new ApplicationException("Missing or invalid .NET Framework installation root.");
        return Directory.Exists(path) ? path : throw new ApplicationException(".NET Framework version " + version + " is not installed.");
      }

      public void Start()
      {
        try
        {
          this.dirEntry.Invoke(nameof (Start));
        }
        catch
        {
        }
      }

      public void Stop()
      {
        try
        {
          this.dirEntry.Invoke(nameof (Stop));
        }
        catch
        {
        }
      }

      public void Recycle()
      {
        try
        {
          this.dirEntry.Invoke(nameof (Recycle));
        }
        catch
        {
        }
      }

      public bool IsRunning()
      {
        try
        {
          return ((int) this.dirEntry.Properties["AppPoolState"][0] & 4) == 0;
        }
        catch
        {
          return false;
        }
      }
    }
  }
}
