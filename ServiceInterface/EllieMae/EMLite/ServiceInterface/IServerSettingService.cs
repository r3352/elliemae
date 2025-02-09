// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IServerSettingService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IServerSettingService
  {
    IDictionary GetServerSettings(string category, bool checkDefinition = true);

    object GetServerSetting(string category, string setting, bool checkDefinition = true);

    IDictionary GetServerSettings();

    void SetServerSetting(string path, object value, bool checkDefinition = true);

    void DeleteVendorSetting(string category, string setting);

    void SetVendorSetting(string path, object value, bool checkDefinition = true);
  }
}
