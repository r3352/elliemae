// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.CurrentUser
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.Authentication;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class CurrentUser : SessionBoundObject, ICurrentUser
  {
    private const string className = "CurrentUser";

    public CurrentUser Initialize(ISession session)
    {
      this.InitializeInternal(session, "user");
      return this;
    }

    public virtual UserInfo GetUserInfo()
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetUserInfo), Array.Empty<object>());
      try
      {
        return this.getLatestVersion().UserInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return (UserInfo) null;
      }
    }

    public virtual void UpdatePersonalInfo(
      string firstName,
      string lastName,
      string email,
      string phone,
      string cellPhone,
      string fax)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (UpdatePersonalInfo), new object[6]
      {
        (object) firstName,
        (object) lastName,
        (object) email,
        (object) phone,
        (object) cellPhone,
        (object) fax
      });
      try
      {
        using (User user = this.checkOut())
        {
          UserInfo userInfo = user.UserInfo;
          if ((firstName ?? "") != "")
            userInfo.FirstName = firstName;
          if ((lastName ?? "") != "")
            userInfo.LastName = lastName;
          if ((email ?? "") != "")
            userInfo.Email = email;
          if (phone != null)
            userInfo.Phone = phone;
          if (cellPhone != null)
            userInfo.CellPhone = cellPhone;
          if (fax != null)
            userInfo.Fax = fax;
          user.CheckIn(this.Session.UserID, false);
        }
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Updated current user's personal information"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual void UpdatePersonalInfo(
      string firstName,
      string lastName,
      string email,
      string phone,
      string cellPhone,
      string fax,
      string emailSignature,
      string photoURL)
    {
      this.onApiCalled(nameof (CurrentUser), "UpdatePersonalInfo2", new object[8]
      {
        (object) firstName,
        (object) lastName,
        (object) email,
        (object) phone,
        (object) cellPhone,
        (object) fax,
        (object) emailSignature,
        (object) photoURL
      });
      try
      {
        using (User user = this.checkOut())
        {
          UserInfo userInfo = user.UserInfo;
          if ((firstName ?? "") != "")
            userInfo.FirstName = firstName;
          if ((lastName ?? "") != "")
            userInfo.LastName = lastName;
          if ((email ?? "") != "")
            userInfo.Email = email;
          if (phone != null)
            userInfo.Phone = phone;
          if (cellPhone != null)
            userInfo.CellPhone = cellPhone;
          if (fax != null)
            userInfo.Fax = fax;
          userInfo.EmailSignature = emailSignature;
          userInfo.ProfileURL = photoURL;
          user.CheckIn(this.Session.UserID, false);
        }
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Updated current user's personal information with EmailSignature"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual void SetWorkingFolder(string workingFolder)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (SetWorkingFolder), new object[1]
      {
        (object) workingFolder
      });
      if ((workingFolder ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (CurrentUser), (ServerException) new ServerArgumentException("Working Folder cannot be blank or null", nameof (workingFolder), this.Session.SessionInfo));
      try
      {
        using (User user = this.checkOut())
        {
          user.UserInfo.WorkingFolder = workingFolder;
          user.CheckIn(this.Session.UserID, false);
        }
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Set current user's working folder to \"" + workingFolder + "\""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual string GetPrivateProfileString(string section, string key)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetPrivateProfileString), new object[2]
      {
        (object) section,
        (object) key
      });
      try
      {
        return User.GetPrivateProfileString(this.Session.UserID, section, key);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return (string) null;
      }
    }

    public virtual void WritePrivateProfileString(string section, string key, string str)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (WritePrivateProfileString), new object[3]
      {
        (object) section,
        (object) key,
        (object) str
      });
      try
      {
        User.WritePrivateProfileString(this.Session.UserID, section, key, str);
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Wrote current user's profile string value for \"" + section + "/" + key + "\""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual void RenameFavoriteLoanTemplateSet(string oldPath, string newPath, bool isFile)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (RenameFavoriteLoanTemplateSet), Array.Empty<object>());
      try
      {
        string path1 = oldPath.Replace(":\\", "/").Replace(" ", "+");
        char lowerInvariant = path1[0];
        if (lowerInvariant.Equals('P'))
        {
          lowerInvariant = char.ToLowerInvariant(path1[0]);
          path1 = lowerInvariant.ToString() + path1.Substring(1);
        }
        string path2 = newPath.Replace(":\\", "/").Replace(" ", "+");
        lowerInvariant = path2[0];
        if (lowerInvariant.Equals('P'))
        {
          lowerInvariant = char.ToLowerInvariant(path2[0]);
          path2 = lowerInvariant.ToString() + path2.Substring(1);
        }
        if (isFile)
        {
          string[] strArray1 = oldPath.Split('\\');
          string file1 = strArray1[strArray1.Length - 1];
          string[] strArray2 = newPath.Split('\\');
          string file2 = strArray2[strArray2.Length - 1];
          string existingPath = this.ConstructEncodedFullPath(path1, file1);
          string replacementPath = this.ConstructEncodedFullPath(path2, file2);
          User.RenameFavoriteLoanTemplateSet(isFile, existingPath, replacementPath, "FavoriteLoanTemplates");
        }
        else
        {
          string existingPath = this.ConstructEncodedPartialPath(path1);
          string replacementPath = this.ConstructEncodedPartialPath(path2);
          User.RenameFavoriteLoanTemplateSet(isFile, existingPath, replacementPath, "FavoriteLoanTemplates");
        }
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("update user's favorite loan template path."));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual void DeleteFavoriteLoanTemplateSet(string path)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (DeleteFavoriteLoanTemplateSet), Array.Empty<object>());
      try
      {
        string path1 = path.Replace(":\\", "/").Replace(" ", "+");
        string[] strArray = path.Split('\\');
        string file = strArray[strArray.Length - 1];
        string path2 = string.Empty;
        foreach (DictionaryEntry privateProfileSetting in User.GetPrivateProfileSettings(this.Session.UserID, "FavoriteLoanTemplates"))
        {
          string str = HttpUtility.UrlDecode(HttpUtility.UrlDecode(privateProfileSetting.Value.ToString()));
          if (str.IndexOf("/v2/templates/LoanTemplate/", StringComparison.InvariantCultureIgnoreCase) >= 0)
          {
            if (str.IndexOf(path1, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
              path2 = this.ConstructEncodedFullPath(path1, file);
              break;
            }
          }
          else if (str.IndexOf("public:", StringComparison.InvariantCultureIgnoreCase) >= 0 && str.IndexOf(path, StringComparison.InvariantCultureIgnoreCase) >= 0)
          {
            path2 = "@" + Uri.EscapeUriString(string.Format("{{\"name\":\"{0}\",\"uri\":\"{1}\"}}", (object) file, (object) HttpUtility.UrlEncode(path)));
            break;
          }
        }
        User.DeleteFavoriteLoanTemplateSet(path2, "FavoriteLoanTemplates");
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("delete user's favorite loan template path."));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    private string ConstructEncodedFullPath(string path, string file)
    {
      return ("@" + Uri.EscapeUriString(string.Format("{{\"name\":\"{0}\",\"uri\":\"/v2/templates/LoanTemplate/{1}\",\"IsPublic\":\"true\"}}", (object) file, (object) path))).Replace("%5C", "%255C");
    }

    private string ConstructEncodedPartialPath(string path)
    {
      return Uri.EscapeUriString(path).Replace("%5C", "%255C");
    }

    public virtual bool ComparePassword(string password)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (ComparePassword), new object[1]
      {
        (object) "******"
      });
      try
      {
        using (User user = this.checkOut())
          return user.ComparePassword(password);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return false;
      }
    }

    public virtual void ChangePassword(string newPassword)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (ChangePassword), new object[1]
      {
        (object) "******"
      });
      try
      {
        using (User user = this.checkOut())
        {
          user.VerifyPasswordHistoryRules(newPassword);
          user.UserInfo.Password = newPassword;
          user.UserInfo.RequirePasswordChange = false;
          user.CheckIn(this.Session.UserID, false);
        }
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Changed current user's password"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual DateTime GetPasswordExpirationDate()
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetPasswordExpirationDate), Array.Empty<object>());
      try
      {
        using (User user = this.checkOut())
          return user.GetPasswordExpirationDate();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return DateTime.Now;
      }
    }

    public virtual bool IsPasswordChangeRequired()
    {
      this.onApiCalled(nameof (CurrentUser), nameof (IsPasswordChangeRequired), Array.Empty<object>());
      try
      {
        using (User user = this.checkOut())
          return user.IsPasswordChangeRequired();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return false;
      }
    }

    public virtual string[] GetUserSettingsNames()
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetUserSettingsNames), Array.Empty<object>());
      try
      {
        return User.GetSettingsFileNames(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual BinaryObject GetUserSettings(string name)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetUserSettings), new object[1]
      {
        (object) name
      });
      try
      {
        return BinaryObject.Marshal(User.GetSettingsFile(this.Session.UserID, name));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return (BinaryObject) null;
      }
    }

    public virtual BinaryObject GetUserDashboardSettings(string name)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetUserDashboardSettings), new object[1]
      {
        (object) name
      });
      try
      {
        return BinaryObject.Marshal(User.GetDashboardMapFile(this.Session.UserID, name));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveUserSettings(string name, BinaryObject data)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (SaveUserSettings), new object[2]
      {
        (object) name,
        (object) data
      });
      data.Download();
      try
      {
        User.SaveSettingsFile(this.Session.UserID, name, data);
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Saved settings \"" + name + "\" for current user"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual void SaveUserDashboardSettings(string name, BinaryObject data)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (SaveUserDashboardSettings), new object[2]
      {
        (object) name,
        (object) data
      });
      data.Download();
      try
      {
        User.SaveDashboardMapFile(this.Session.UserID, name, data);
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Saved settings \"" + name + "\" for current user"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual string GetDefaultProviderInfo()
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetDefaultProviderInfo), Array.Empty<object>());
      try
      {
        return User.GetDefaultProviderInfo(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return (string) null;
      }
    }

    public virtual void UpdateDefaultProviderInfo(string providerInfo)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (UpdateDefaultProviderInfo), new object[1]
      {
        (object) providerInfo
      });
      try
      {
        User.SaveDefaultProviderInfo(this.Session.UserID, providerInfo);
        TraceLog.WriteInfo(nameof (CurrentUser), this.formatMsg("Saved default provider info for current user"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    public virtual UserLicenseInfo GetUserLicense()
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetUserLicense), Array.Empty<object>());
      try
      {
        using (User latestVersion = this.getLatestVersion())
          return latestVersion.GetUserLicense();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
        return (UserLicenseInfo) null;
      }
    }

    public virtual void UpdateUserLicense(UserLicenseInfo license)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (UpdateUserLicense), new object[1]
      {
        (object) license
      });
      try
      {
        using (User user = this.checkOut())
          user.UpdateUserLicense(license);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex);
      }
    }

    private User checkOut()
    {
      User user = UserStore.CheckOut(this.Session.UserID);
      if (!user.Exists)
      {
        user.UndoCheckout();
        Err.Raise(TraceLevel.Error, nameof (CurrentUser), new ServerException("Cannot locate UserInfo for user '" + this.Session.UserID + "'", this.Session.SessionInfo));
      }
      return user;
    }

    private User getLatestVersion()
    {
      User latestVersion = UserStore.GetLatestVersion(this.Session.UserID);
      if (latestVersion.Exists)
        return latestVersion;
      Err.Raise(TraceLevel.Error, nameof (CurrentUser), new ServerException("Cannot locate UserInfo for user '" + this.Session.UserID + "'", this.Session.SessionInfo));
      return latestVersion;
    }

    public virtual string[] AddRemoveAndGetRecentSettings(
      bool isCompanySettings,
      string settingTitle,
      bool add,
      int maxCount,
      bool deleteExtra)
    {
      this.onApiCalled(nameof (CurrentUser), "GetRecentSettings", new object[1]
      {
        (object) maxCount
      });
      try
      {
        using (User user = UserStore.CheckOut(this.Session.UserID))
        {
          string[] recentSettings = user.AddRemoveAndGetRecentSettings(isCompanySettings, settingTitle, add, maxCount, deleteExtra);
          user.CheckIn(this.Session.UserID, false);
          return recentSettings;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual bool IsMemberOfAclGroup(int groupId)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (IsMemberOfAclGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        foreach (AclGroup aclGroup in AclGroupAccessor.GetGroupsOfUser(this.Session.UserID))
        {
          if (aclGroup.ID == groupId)
            return true;
        }
        return false;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual AccessToken GetAccessToken(string scope)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetAccessToken), new object[1]
      {
        (object) scope
      });
      try
      {
        Task<AccessToken> accessToken = new OAuth2Client(this.Session.Context).GetAccessToken(this.Session.SessionID, scope);
        Task.WaitAll((Task) accessToken);
        return accessToken.Result;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex, this.Session.SessionInfo);
        return (AccessToken) null;
      }
    }

    public AccessToken GetAccessTokenForGuest(string clientId, string scope, string redirectUri)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetAccessTokenForGuest), new object[3]
      {
        (object) clientId,
        (object) scope,
        (object) redirectUri
      });
      try
      {
        OAuth2Client oauth2Client = new OAuth2Client(this.Session.Context);
        Task<AccessToken> accessToken = oauth2Client.GetAccessToken(this.Session.SessionID, "sc");
        Task.WaitAll((Task) accessToken);
        Task<AuthorizationCode> authCodeForGuest = oauth2Client.GetAuthCodeForGuest(accessToken.Result.Token, clientId, scope, redirectUri);
        Task.WaitAll((Task) authCodeForGuest);
        Task<AccessToken> task = oauth2Client.ExchangeAuthCodeForAccessToken(authCodeForGuest.Result.Code, clientId, scope, redirectUri);
        Task.WaitAll((Task) task);
        return task.Result;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex, this.Session.SessionInfo);
        return (AccessToken) null;
      }
    }

    public virtual AuthorizationCode GetAuthCodeForGuest(string clientId)
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetAuthCodeForGuest), new object[1]
      {
        (object) clientId
      });
      try
      {
        OAuth2Client oauth2Client = new OAuth2Client(this.Session.Context);
        Task<AccessToken> accessToken = oauth2Client.GetAccessToken(this.Session.SessionID, "sc");
        Task.WaitAll((Task) accessToken);
        Task<AuthorizationCode> authCodeForGuest = oauth2Client.GetAuthCodeForGuest(accessToken.Result.Token, clientId);
        Task.WaitAll((Task) authCodeForGuest);
        return authCodeForGuest.Result;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex, this.Session.SessionInfo);
        return (AuthorizationCode) null;
      }
    }

    public virtual AuthorizationCode GetAuthCodeFromAuthToken()
    {
      this.onApiCalled(nameof (CurrentUser), nameof (GetAuthCodeFromAuthToken), Array.Empty<object>());
      try
      {
        OAuth2Client oauth2Client = new OAuth2Client(this.Session.Context);
        Task<AccessToken> accessTokenForRfc = oauth2Client.GetAccessTokenForRfc(this.Session.SessionID);
        Task.WaitAll((Task) accessTokenForRfc);
        Task<AuthorizationCode> codeFromAuthToken = oauth2Client.GetAuthCodeFromAuthToken(accessTokenForRfc.Result.Token);
        Task.WaitAll((Task) codeFromAuthToken);
        return codeFromAuthToken.Result;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CurrentUser), ex, this.Session.SessionInfo);
        return (AuthorizationCode) null;
      }
    }
  }
}
