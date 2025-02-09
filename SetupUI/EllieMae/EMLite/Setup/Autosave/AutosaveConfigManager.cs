// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Autosave.AutosaveConfigManager
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;

#nullable disable
namespace EllieMae.EMLite.Setup.Autosave
{
  public class AutosaveConfigManager
  {
    private const int minInterval = 30;
    private const int maxInterval = 3600;
    private static readonly int defaultInterval = 0;
    private static RegistryKey regKey = (RegistryKey) null;
    internal static readonly bool ServerSettingEnabled = true;
    public static readonly bool UserConfigEnabled = true;

    public static event AutosaveSettingsChangeEventHandler OnAutosaveSettingsChanged = null;

    private AutosaveConfigManager()
    {
    }

    static AutosaveConfigManager()
    {
      AutosaveConfigManager.defaultInterval = (int) Session.GetComponentSetting("AutosaveInterval", (object) 300);
      AutosaveConfigManager.ServerSettingEnabled = (EnableDisableSetting) Session.GetComponentSetting("AutoRecover", (object) EnableDisableSetting.Enabled) != EnableDisableSetting.Disabled;
      AutosaveConfigManager.UserConfigEnabled = (EnableDisableSetting) Session.GetComponentSetting("AutosaveUserConfig", (object) EnableDisableSetting.Enabled) != EnableDisableSetting.Disabled;
      try
      {
        AutosaveConfigManager.regKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
        if (AutosaveConfigManager.regKey == null)
          return;
        string str = (string) AutosaveConfigManager.regKey.GetValue("AutosaveUserConfig");
        if (str == null)
          return;
        AutosaveConfigManager.UserConfigEnabled = !(str.Trim() == "0");
      }
      catch
      {
      }
      finally
      {
        if (AutosaveConfigManager.regKey != null)
        {
          AutosaveConfigManager.regKey.Close();
          AutosaveConfigManager.regKey = (RegistryKey) null;
        }
      }
    }

    public static int GetInterval()
    {
      if (!AutosaveConfigManager.UserConfigEnabled)
        return AutosaveConfigManager.defaultInterval;
      int interval = 0;
      try
      {
        AutosaveConfigManager.regKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass");
        if (AutosaveConfigManager.regKey != null)
        {
          string str = (string) AutosaveConfigManager.regKey.GetValue("AutosaveInterval");
          if (str != null)
            interval = Convert.ToInt32(str);
        }
      }
      catch
      {
      }
      finally
      {
        if (AutosaveConfigManager.regKey != null)
        {
          AutosaveConfigManager.regKey.Close();
          AutosaveConfigManager.regKey = (RegistryKey) null;
        }
      }
      if (interval > 0)
        return interval;
      try
      {
        AutosaveConfigManager.regKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
        if (AutosaveConfigManager.regKey != null)
        {
          string str = (string) AutosaveConfigManager.regKey.GetValue("AutosaveInterval");
          if (str != null)
            interval = Convert.ToInt32(str);
        }
      }
      catch
      {
      }
      finally
      {
        if (AutosaveConfigManager.regKey != null)
        {
          AutosaveConfigManager.regKey.Close();
          AutosaveConfigManager.regKey = (RegistryKey) null;
        }
      }
      return interval > 0 ? interval : AutosaveConfigManager.defaultInterval;
    }

    internal static bool IsEnabledOnClient()
    {
      int num = -1;
      if (AutosaveConfigManager.UserConfigEnabled)
      {
        try
        {
          AutosaveConfigManager.regKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass");
          if (AutosaveConfigManager.regKey != null)
          {
            string str = (string) AutosaveConfigManager.regKey.GetValue("AutosaveEnabled");
            if (str != null)
              num = !(str.Trim() == "0") ? 1 : 0;
          }
        }
        catch
        {
        }
        finally
        {
          if (AutosaveConfigManager.regKey != null)
          {
            AutosaveConfigManager.regKey.Close();
            AutosaveConfigManager.regKey = (RegistryKey) null;
          }
        }
      }
      return num < 0 || num != 0;
    }

    public static bool IsAutosaveEnabled()
    {
      return AutosaveConfigManager.ServerSettingEnabled && AutosaveConfigManager.IsEnabledOnClient();
    }

    public static void SaveSettings(bool enabled, int interval)
    {
      if (interval > 3600)
        interval = 3600;
      else if (interval < 30)
        interval = 30;
      try
      {
        AutosaveConfigManager.regKey = Registry.CurrentUser.CreateSubKey("Software\\Ellie Mae\\Encompass");
      }
      catch
      {
      }
      finally
      {
        if (AutosaveConfigManager.regKey != null)
        {
          AutosaveConfigManager.regKey.Close();
          AutosaveConfigManager.regKey = (RegistryKey) null;
        }
      }
      try
      {
        AutosaveConfigManager.regKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass", true);
        if (AutosaveConfigManager.regKey == null)
          throw new Exception("Cannot open registry key HKCU\\Software\\Ellie Mae\\Encompass");
        AutosaveConfigManager.regKey.SetValue("AutosaveInterval", (object) string.Concat((object) interval));
        AutosaveConfigManager.regKey.SetValue("AutosaveEnabled", enabled ? (object) "1" : (object) "0");
        AutosaveConfigManager.regKey.Flush();
        if (AutosaveConfigManager.OnAutosaveSettingsChanged == null)
          return;
        AutosaveConfigManager.OnAutosaveSettingsChanged((object) null, new AutosaveSettingsChangeEventArgs(enabled, interval));
      }
      finally
      {
        if (AutosaveConfigManager.regKey != null)
        {
          AutosaveConfigManager.regKey.Close();
          AutosaveConfigManager.regKey = (RegistryKey) null;
        }
      }
    }
  }
}
