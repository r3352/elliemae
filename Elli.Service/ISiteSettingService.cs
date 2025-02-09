// Decompiled with JetBrains decompiler
// Type: Elli.Service.ISiteSettingService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public interface ISiteSettingService
  {
    bool CreateSiteSetting(object siteSettingData);

    bool UpdateSiteSetting(object siteSettingData);

    object GetSiteSetting(object siteSettingData, string guid = null);

    object GetSiteSettingByKey(string siteId, string Key);

    string GetSiteSettingLibraryId(string siteId);

    IList<object> GetSiteSettings();

    bool DeleteSiteSetting(string siteId);

    bool DeleteSiteSetting(string siteId, string guid);
  }
}
