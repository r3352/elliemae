// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Plugins
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class Plugins
  {
    private const string className = "Plugins�";

    public static BinaryObject GetPluginAssembly(string assemblyName)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(Plugins.getPluginPath(assemblyName)))
        return latestVersion.GetData();
    }

    public static Version GetPluginVersion(string assemblyName)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(Plugins.getPluginPath(assemblyName)))
        return latestVersion.FileVersion;
    }

    public static string[] GetPluginAssemblyNames()
    {
      List<string> stringList = new List<string>();
      foreach (string directoryFileName in FileStore.GetDirectoryFileNames(Plugins.getPluginFolderPath(), true))
      {
        if (string.Compare(Path.GetExtension(directoryFileName), ".dll", true) == 0)
          stringList.Add(directoryFileName);
      }
      return stringList.ToArray();
    }

    public static PluginInfo[] GetPluginInfos()
    {
      List<PluginInfo> pluginInfoList = new List<PluginInfo>();
      foreach (string pluginAssemblyName in Plugins.GetPluginAssemblyNames())
      {
        try
        {
          Version pluginVersion = Plugins.GetPluginVersion(pluginAssemblyName);
          if (pluginVersion != (Version) null)
            pluginInfoList.Add(new PluginInfo(pluginAssemblyName, pluginVersion));
        }
        catch (Exception ex)
        {
          TraceLog.WriteWarning(nameof (Plugins), "Cannot read version number for plugin '" + pluginAssemblyName + "': " + (object) ex);
        }
      }
      return pluginInfoList.ToArray();
    }

    public static void InstallPlugin(string assemblyName, BinaryObject pluginAssembly)
    {
      using (DataFile dataFile = FileStore.CheckOut(Plugins.getPluginPath(assemblyName), MutexAccess.Write))
        dataFile.CheckIn(pluginAssembly);
    }

    public static void UninstallPlugin(string assemblyName)
    {
      using (DataFile dataFile = FileStore.CheckOut(Plugins.getPluginPath(assemblyName), MutexAccess.Write))
      {
        if (!dataFile.Exists)
          return;
        dataFile.Delete();
      }
    }

    private static string getPluginFolderPath()
    {
      return ClientContext.GetCurrent().Settings.GetDataFolderPath(nameof (Plugins));
    }

    private static string getPluginPath(string assemblyName)
    {
      return string.Compare(Path.GetExtension(assemblyName), ".dll", true) == 0 ? Path.Combine(Plugins.getPluginFolderPath(), assemblyName) : Path.Combine(Plugins.getPluginFolderPath(), assemblyName + ".dll");
    }
  }
}
