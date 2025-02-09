// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.DiagUtility
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Encompass.Diagnostics
{
  public static class DiagUtility
  {
    static DiagUtility()
    {
      string appName = DiagUtility.GetAppName();
      string version = DiagUtility.GetVersion();
      string environmentName = DiagUtility.GetEnvironmentName();
      DiagUtility.ApplicationEventHandler = (IApplicationEventHandler) new Encompass.Diagnostics.Events.ApplicationEventHandler(appName);
      DiagUtility.LogManager = (ILogManager) new Encompass.Diagnostics.Logging.LogManager(appName, version, environmentName);
    }

    public static IApplicationEventHandler ApplicationEventHandler { get; }

    public static ILoggerScopeProvider LoggerScopeProvider { get; private set; }

    public static ILogManager LogManager { get; }

    public static void SetLoggerScopeProvider(ILoggerScopeProvider loggerScopeProvider)
    {
      lock (typeof (DiagUtility))
      {
        DiagUtility.LoggerScopeProvider = loggerScopeProvider;
        DiagUtility.LogManager.SetLoggerScopeProvider(loggerScopeProvider);
      }
    }

    public static ILogger DefaultLogger => DiagUtility.LogManager.GetLogger();

    public static ILogger GlobalLogger => DiagUtility.LogManager.GetLogger((string) null, true);

    private static string GetVersion()
    {
      try
      {
        string assemblyName = ConfigurationManager.AppSettings["VersionAssemblyNameForLog"];
        int result;
        if (!int.TryParse(ConfigurationManager.AppSettings["VersionFieldCountForLog"], out result))
          result = 0;
        Assembly assembly = (Assembly) null;
        if (!string.IsNullOrEmpty(assemblyName))
          assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => string.Equals(a.GetName().Name, assemblyName)));
        if (assembly == (Assembly) null)
          assembly = Assembly.GetEntryAssembly();
        if (assembly != (Assembly) null)
        {
          Version version1 = new Version(1, 0, 0, 0);
          Version version2 = new Version(FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion);
          return result > 0 ? version2.ToString(result) : version2.ToString();
        }
      }
      catch
      {
      }
      return "0.0";
    }

    public static string GetAppName()
    {
      string appName = ConfigurationManager.AppSettings["AppNameForLog"];
      if (string.IsNullOrEmpty(appName))
        appName = Assembly.GetEntryAssembly()?.GetName()?.Name;
      if (string.IsNullOrEmpty(appName))
        appName = AppDomain.CurrentDomain.FriendlyName;
      return appName;
    }

    public static string GetEnvironmentName()
    {
      return ConfigurationManager.AppSettings["EnvironmentNameForLog"];
    }
  }
}
