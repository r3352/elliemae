// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EncompassSettings
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Microsoft.Win32;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class EncompassSettings
  {
    public static readonly EncompassSettings Default = new EncompassSettings("");
    private string keyName = "Encompass";

    public EncompassSettings(string instanceName)
    {
      if (!((instanceName ?? "") != ""))
        return;
      this.keyName = this.keyName + "$" + instanceName;
    }

    public bool Hosted => this.getRegistryValue(nameof (Hosted)).Trim() == "1";

    public bool ApplyServerHotUpdates
    {
      get => this.getRegistryValue(nameof (ApplyServerHotUpdates)).Trim() != "0";
    }

    public string IIsVirtualRootName
    {
      get => string.Concat(this.getRegistryValue("Server\\HTTP", "IIsVirtualRoot"));
    }

    public string IIsWebSiteID
    {
      get => string.Concat(this.getRegistryValue("Server\\HTTP", "IIsWebSite"));
    }

    private object getRegistryValue(string subKey, string name)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\" + this.keyName + "\\" + subKey))
        return registryKey?.GetValue(name);
    }

    private string getRegistryValue(string name)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\" + this.keyName))
        return string.Concat(registryKey.GetValue(name));
    }
  }
}
