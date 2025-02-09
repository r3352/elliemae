// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.AppConfigRepository
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Collections.Generic;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class AppConfigRepository : IConfigurationRepository
  {
    public T GetConfigurationValue<T>(string key)
    {
      string appSetting = ConfigurationManager.AppSettings[key];
      if (appSetting == null)
        throw new KeyNotFoundException("Key " + key + " not found. ");
      try
      {
        return typeof (Enum).IsAssignableFrom(typeof (T)) ? (T) Enum.Parse(typeof (T), appSetting) : (T) Convert.ChangeType((object) appSetting, typeof (T));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public T GetConfigurationValue<T>(string key, T defaultValue)
    {
      string appSetting = ConfigurationManager.AppSettings[key];
      if (appSetting == null)
        return defaultValue;
      try
      {
        return typeof (Enum).IsAssignableFrom(typeof (T)) ? (T) Enum.Parse(typeof (T), appSetting) : (T) Convert.ChangeType((object) appSetting, typeof (T));
      }
      catch (Exception ex)
      {
        return defaultValue;
      }
    }
  }
}
