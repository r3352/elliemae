// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.BrowserEngine
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Browser;
using DotNetBrowser.Cookies;
using DotNetBrowser.Engine;
using DotNetBrowser.Engine.Events;
using DotNetBrowser.Logging;
using Elli.Web.Host.Login;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host
{
  public static class BrowserEngine
  {
    private const string className = "BrowserEngine";
    private static readonly string sw = Tracing.SwThinThick;
    private static object _browserEngineLock = new object();
    private static object _chromeWinDownloadLock = new object();
    private static IEngine _browserEngine = (IEngine) null;
    private static bool _applicationExit = false;
    private static bool _isChromeWinAssemblyLoaded = false;
    private static Task _startEngineTask;

    public static IBrowser CreateBrowser()
    {
      BrowserEngine.CheckAwaitEngineTask();
      Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "CreateBrowser: Locking BrowserEngine");
      lock (BrowserEngine._browserEngineLock)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Checking BrowserEngine");
        if (BrowserEngine._browserEngine == null)
        {
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Starting BrowserEngine");
          if (!BrowserEngine.StartEngine())
            throw new Exception("Unable to start the BrowserEngine");
        }
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Creating Browser");
        return BrowserEngine._browserEngine.CreateBrowser();
      }
    }

    private static void CheckAwaitEngineTask()
    {
      if (BrowserEngine._startEngineTask == null || WebLoginUtil.IsChromiumForWebLoginEnabled)
        return;
      Tracing.Log(BrowserEngine.sw, TraceLevel.Info, nameof (BrowserEngine), string.Format("BrowserEngine Status - {0}", (object) BrowserEngine._startEngineTask.Status));
      do
        ;
      while (!BrowserEngine._startEngineTask.IsCompleted);
      Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), string.Format("BrowserEngine Init Completed - {0}", (object) BrowserEngine._startEngineTask.Status));
    }

    private static bool IsChromeWinAssemblyLoaded
    {
      get
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), string.Format("IsChromeWinAssemblyLoaded Check - {0} ", (object) BrowserEngine._isChromeWinAssemblyLoaded));
        if (!BrowserEngine._isChromeWinAssemblyLoaded)
        {
          lock (BrowserEngine._chromeWinDownloadLock)
          {
            if (!BrowserEngine._isChromeWinAssemblyLoaded)
            {
              string assemblyString = "DotNetBrowser.Chromium.Win-x86";
              if (Environment.Is64BitProcess)
                assemblyString = "DotNetBrowser.Chromium.Win-x64";
              Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Begin assembly loading - " + assemblyString);
              Assembly.Load(assemblyString);
              BrowserEngine._isChromeWinAssemblyLoaded = true;
            }
          }
        }
        return BrowserEngine._isChromeWinAssemblyLoaded;
      }
    }

    public static void StartEngineAsync()
    {
      if (BrowserEngine._browserEngine != null)
        return;
      BrowserEngine._startEngineTask = Task.Run((Action) (() =>
      {
        if (!BrowserEngine.IsChromeWinAssemblyLoaded)
          return;
        BrowserEngine.StartEngine();
      }));
      Tracing.Log(BrowserEngine.sw, TraceLevel.Info, nameof (BrowserEngine), "BrowserEngine Started");
    }

    public static bool StartBinaryDownload()
    {
      try
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "StartEngineAync: Starting Task");
        lock (BrowserEngine._browserEngineLock)
        {
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Checking IsSmartClient");
          if (!AssemblyResolver.IsSmartClient)
            return true;
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), string.Format("IsChromeWinAssemblyLoaded Check - {0} ", (object) BrowserEngine._isChromeWinAssemblyLoaded));
          if (!BrowserEngine._isChromeWinAssemblyLoaded)
          {
            lock (BrowserEngine._chromeWinDownloadLock)
            {
              string assemblyString = "DotNetBrowser.Chromium.Win-x86";
              if (Environment.Is64BitProcess)
                assemblyString = "DotNetBrowser.Chromium.Win-x64";
              Assembly.Load(assemblyString);
              BrowserEngine._isChromeWinAssemblyLoaded = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Error, nameof (BrowserEngine), "Error loading the chromium binary dll - " + ex.Message);
        BrowserEngine._isChromeWinAssemblyLoaded = false;
      }
      return BrowserEngine._isChromeWinAssemblyLoaded;
    }

    private static bool StartEngine()
    {
      bool flag = false;
      try
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "StartEngine: Locking BrowserEngine");
        lock (BrowserEngine._browserEngineLock)
        {
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Checking BrowserEngine");
          if (BrowserEngine._browserEngine != null)
            return true;
          string chromiumContextPath = Paths.ChromiumContextPath;
          if (!Directory.Exists(chromiumContextPath))
            Directory.CreateDirectory(chromiumContextPath);
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Creating BrowserContextParams");
          EngineOptions.Builder builder = new EngineOptions.Builder()
          {
            RenderingMode = RenderingMode.HardwareAccelerated,
            UserDataDirectory = chromiumContextPath,
            TouchMenuDisabled = false,
            SandboxDisabled = true
          };
          if (WebLoginUtil.IsEnableChromeDebugMode)
          {
            Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Enabling port 9222 for automation");
            builder.RemoteDebuggingPort = 9222U;
            Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Enabling Chromium Debugging");
            builder.ChromiumSwitches.Add("--force-renderer-accessibility");
            builder.ChromiumSwitches.Add("--ignore-certificate-errors");
            builder.ChromiumSwitches.Add("--remote-allow-origins=http://localhost:" + (object) builder.RemoteDebuggingPort);
            Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Enabling Chromium Logging");
            LoggerProvider.Instance.Level = SourceLevels.All;
            LoggerProvider.Instance.FileLoggingEnabled = true;
            LoggerProvider.Instance.OutputFile = Path.Combine(SystemSettings.TempFolderRoot, "Chromium.log");
          }
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Creating BrowserEngine");
          BrowserEngine._browserEngine = EngineFactory.Create(builder.Build());
          BrowserEngine._browserEngine.Profiles.Default.Preferences.AutofillEnabled = false;
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Subscribing to BrowserEngine Events");
          BrowserEngine._browserEngine.Disposed += new EventHandler<EngineDisposedEventArgs>(BrowserEngine.browserEngine_Disposed);
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Restoring Cookies");
          BrowserEngine.RestoreCookies();
          if (!BrowserEngine._applicationExit)
          {
            Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Subscribing to Application Events");
            Application.ApplicationExit += new EventHandler(BrowserEngine.application_ApplicationExit);
            BrowserEngine._applicationExit = true;
          }
          flag = true;
        }
      }
      catch (ChromiumBinariesMissingException ex)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Error, nameof (BrowserEngine), "ChromiumBinariesMissingException: " + ex.Message);
        BrowserEngine.StopEngine();
      }
      catch (Exception ex)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Error, nameof (BrowserEngine), string.Format("Exception while initializing browser engine: {0} type : {1}, stackTrace - {2}", (object) ex.Message, (object) ex.GetType(), (object) ex.StackTrace));
        BrowserEngine.StopEngine();
      }
      return flag;
    }

    public static void StopEngine()
    {
      Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "StopEngine: Locking BrowserEngine");
      lock (BrowserEngine._browserEngineLock)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Checking BrowserEngine");
        if (BrowserEngine._browserEngine == null)
          return;
        BrowserEngine.BackupCookies();
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Checking BrowserEngine.IsDisposed");
        if (!BrowserEngine._browserEngine.IsDisposed)
        {
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Unsubscribing to BrowserEngine Events");
          BrowserEngine._browserEngine.Disposed -= new EventHandler<EngineDisposedEventArgs>(BrowserEngine.browserEngine_Disposed);
          Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Disposing BrowserEngine");
          BrowserEngine._browserEngine.Dispose();
        }
        Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Releasing BrowserEngine");
        BrowserEngine._browserEngine = (IEngine) null;
      }
    }

    private static void browserEngine_Disposed(object sender, EngineDisposedEventArgs e)
    {
      Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "BrowserEngine: Disposed: " + (object) e.ExitCode);
      if (e.ExitCode != 0L)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Error, nameof (BrowserEngine), string.Format("BrowserEngine: Crashed: {0}", (object) e.ExitCode));
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The Chromium Engine has shutdown unexpectedly. A new instance will be created the next time that a Browser is being created.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Stopping BrowserEngine");
      BrowserEngine.StopEngine();
    }

    private static void application_ApplicationExit(object sender, EventArgs e)
    {
      Tracing.Log(BrowserEngine.sw, TraceLevel.Verbose, nameof (BrowserEngine), "Stopping BrowserEngine");
      BrowserEngine.StopEngine();
    }

    private static void BackupCookies()
    {
      Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Calling GetActiveCookies");
      IEnumerable<Cookie> activeCookies = BrowserEngine.GetActiveCookies();
      if (activeCookies == null)
        return;
      Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Calling CookieFileHelper.WriteToBackup");
      CookieFileHelper.WriteToBackup(activeCookies);
    }

    private static void RestoreCookies()
    {
      Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Calling CookieFileHelper.ReadFromBackup");
      IEnumerable<Cookie> cookies = CookieFileHelper.ReadFromBackup();
      if (cookies == null)
        return;
      Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Calling SetCookies");
      BrowserEngine.SetCookies(cookies);
    }

    private static IEnumerable<Cookie> GetActiveCookies()
    {
      IEnumerable<Cookie> activeCookies = (IEnumerable<Cookie>) null;
      try
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Calling CookieStore.GetAllCookies");
        activeCookies = BrowserEngine._browserEngine.Profiles.Default.CookieStore.GetAllCookies().Result;
      }
      catch (Exception ex)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Failed to read cookies: " + ex.Message);
      }
      return activeCookies;
    }

    private static bool SetCookies(IEnumerable<Cookie> cookies)
    {
      bool flag = false;
      try
      {
        if (cookies == null)
          return true;
        foreach (Cookie cookie in cookies)
        {
          Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Calling CookieStore.SetCookie: " + cookie.Name);
          if (BrowserEngine._browserEngine.Profiles.Default.CookieStore.SetCookie(cookie).Result)
          {
            Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Calling CookieStore.Flush: " + cookie.Name);
            BrowserEngine._browserEngine.Profiles.Default.CookieStore.Flush();
          }
        }
        flag = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(BrowserEngine.sw, TraceLevel.Warning, nameof (BrowserEngine), "Failed to set cookie: " + ex.Message);
      }
      return flag;
    }
  }
}
