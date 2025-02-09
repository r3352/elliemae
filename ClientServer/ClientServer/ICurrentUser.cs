// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ICurrentUser
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ICurrentUser
  {
    UserInfo GetUserInfo();

    void UpdatePersonalInfo(
      string firstName,
      string lastName,
      string email,
      string phone,
      string cellPhone,
      string fax);

    void UpdatePersonalInfo(
      string firstName,
      string lastName,
      string email,
      string phone,
      string cellPhone,
      string fax,
      string eMailSignature,
      string photoURL);

    void SetWorkingFolder(string workingFolder);

    string GetPrivateProfileString(string section, string key);

    void WritePrivateProfileString(string section, string key, string str);

    void RenameFavoriteLoanTemplateSet(string oldPath, string newPath, bool isFile);

    void DeleteFavoriteLoanTemplateSet(string path);

    bool ComparePassword(string password);

    void ChangePassword(string password);

    DateTime GetPasswordExpirationDate();

    bool IsPasswordChangeRequired();

    string[] GetUserSettingsNames();

    BinaryObject GetUserSettings(string name);

    void SaveUserSettings(string name, BinaryObject data);

    BinaryObject GetUserDashboardSettings(string name);

    void SaveUserDashboardSettings(string name, BinaryObject data);

    string GetDefaultProviderInfo();

    void UpdateDefaultProviderInfo(string providerInfo);

    UserLicenseInfo GetUserLicense();

    void UpdateUserLicense(UserLicenseInfo license);

    bool IsMemberOfAclGroup(int groupId);

    string[] AddRemoveAndGetRecentSettings(
      bool isCompanySettings,
      string settingTitle,
      bool add,
      int maxCount,
      bool deleteExtra);

    AccessToken GetAccessToken(string scope);

    AccessToken GetAccessTokenForGuest(string clientId, string scope, string redirectUri);

    AuthorizationCode GetAuthCodeForGuest(string clientId);

    AuthorizationCode GetAuthCodeFromAuthToken();
  }
}
