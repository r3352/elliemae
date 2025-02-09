// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.UserLicenseInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.JedLib;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  [Serializable]
  public class UserLicenseInfo
  {
    private static b jed = (b) null;
    private static LicenseHelper helper = new LicenseHelper();
    private string userId = "";
    private string clientId = "";
    private ModuleLicenseCollection modules;
    private DateTime timestamp = DateTime.MinValue;

    static UserLicenseInfo() => UserLicenseInfo.jed = a.b("abc8yzl5dj3bc9");

    public UserLicenseInfo(string clientId, string userId)
      : this(clientId, userId, DateTime.MinValue, new ModuleLicenseCollection())
    {
    }

    public UserLicenseInfo(
      string clientId,
      string userId,
      DateTime timestamp,
      ModuleLicenseCollection modules)
    {
      this.userId = userId.ToLower();
      this.clientId = clientId;
      this.timestamp = timestamp;
      this.modules = modules;
    }

    public string UserID
    {
      get => this.userId;
      set => this.userId = value;
    }

    public string ClientID
    {
      get => this.clientId;
      set => this.clientId = value;
    }

    public DateTime Timestamp
    {
      get => this.timestamp;
      set => this.timestamp = value;
    }

    public ModuleLicenseCollection LicensedModules => this.modules;

    public static UserLicenseInfo Parse(string licenseStr)
    {
      try
      {
        string str = (string) null;
        lock (UserLicenseInfo.jed)
        {
          UserLicenseInfo.jed.b();
          str = UserLicenseInfo.jed.a((Stream) new MemoryStream(UserLicenseInfo.helper.StringToBytes(licenseStr)));
        }
        string[] strArray = str.Split('|');
        return new UserLicenseInfo(strArray[0], strArray[1], DateTime.ParseExact(strArray[2], "yyyyMMddHHmmss", (IFormatProvider) null), ModuleLicenseCollection.Parse(strArray[3]));
      }
      catch
      {
        throw new Exception("Invalid license format");
      }
    }

    public override string ToString()
    {
      string A_0 = this.clientId + "|" + this.userId + "|" + this.timestamp.ToString("yyyyMMddHHmmss") + "|" + this.modules.ToString();
      lock (UserLicenseInfo.jed)
      {
        UserLicenseInfo.jed.b();
        byte[] bytes = UserLicenseInfo.jed.b(A_0);
        return UserLicenseInfo.helper.BytesToString(bytes);
      }
    }
  }
}
