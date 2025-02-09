// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.SettingsManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class SettingsManager
  {
    private IServerManager iServerManager;

    public SettingsManager(IServerManager iServerManager) => this.iServerManager = iServerManager;

    public IDictionary GetServerSettings(string category)
    {
      IDictionary serverSettings = this.iServerManager.GetServerSettings(category, false);
      foreach (SettingDefinition settingDefinition in (IEnumerable) SettingDefinitions.GetSettingDefinitions(category).Values)
        serverSettings[(object) settingDefinition.Path] = settingDefinition.Parse((string) serverSettings[(object) settingDefinition.Path]);
      return serverSettings;
    }

    public object GetServerSetting(string path)
    {
      SettingDefinition settingDefinition = SettingDefinitions.GetSettingDefinition(path);
      if (settingDefinition == null)
        throw new Exception("Invalid setting path '" + path + "'");
      object serverSetting = this.iServerManager.GetServerSetting(path, false);
      return settingDefinition.Parse((string) serverSetting);
    }

    public void UpdateServerSettings(IDictionary settings)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (string key in (IEnumerable) settings.Keys)
        insensitiveHashtable[(object) key] = (object) (SettingDefinitions.GetSettingDefinition(key) ?? throw new Exception("Invalid setting path '" + key + "'")).ToString(settings[(object) key]);
      this.iServerManager.UpdateServerSettings((IDictionary) insensitiveHashtable, false);
    }

    public void UpdateServerSetting(string path, object value)
    {
      object obj = (object) (SettingDefinitions.GetSettingDefinition(path) ?? throw new Exception("Invalid setting path '" + path + "'")).ToString(value);
      this.iServerManager.UpdateServerSetting(path, obj, false);
    }
  }
}
