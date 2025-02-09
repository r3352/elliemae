// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.PersistentClientCacheFeature
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class PersistentClientCacheFeature
  {
    public const string FeatureID = "EnablePersistentClientCache�";

    public static bool IsEnabled()
    {
      return PersistentClientCacheFeature.IsEnabled(Company.GetCompanySetting("FEATURE", "EnablePersistentClientCache"));
    }

    public static bool IsEnabled(IClientContext context)
    {
      return PersistentClientCacheFeature.IsEnabled(Company.GetCompanySetting(context, "FEATURE", "EnablePersistentClientCache"));
    }

    public static bool IsEnabled(string settingValue)
    {
      bool result;
      return !bool.TryParse(settingValue, out result) || result;
    }
  }
}
