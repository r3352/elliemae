// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.LicenseInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  [Serializable]
  public class LicenseInfo
  {
    private static b jed = (b) null;
    private static LicenseHelper helper = new LicenseHelper();
    private const int KeyLength = 8;
    private string cdKey = "";
    private string clientId = "";
    private DateTime expirationDate = DateTime.MinValue;
    private EncompassEdition edition;
    private JedVersion version = JedVersion.Unknown;
    private LicenseType licenseType;
    private int userLimit = -1;
    private int userLimitFlexPct = 10;
    private bool enabled = true;

    static LicenseInfo() => LicenseInfo.jed = a.b("abc8yzl5dj3bc9");

    public LicenseInfo(
      string cdKey,
      string clientId,
      EncompassEdition edition,
      JedVersion version,
      int userLimit,
      int userLimitFlexPct,
      LicenseType type,
      DateTime expirationDate,
      bool enabled)
    {
      this.cdKey = cdKey;
      this.clientId = clientId;
      this.licenseType = type;
      this.expirationDate = expirationDate;
      this.edition = edition;
      this.version = version;
      this.userLimit = userLimit;
      this.userLimitFlexPct = userLimitFlexPct;
      this.enabled = enabled;
    }

    public string CDKey
    {
      get => this.cdKey;
      set => this.cdKey = value;
    }

    public string ClientID
    {
      get => this.clientId;
      set => this.clientId = value;
    }

    public LicenseType Type
    {
      get => this.licenseType;
      set => this.licenseType = value;
    }

    public DateTime Expires
    {
      get => this.expirationDate;
      set => this.expirationDate = value;
    }

    public EncompassEdition Edition
    {
      get => this.edition;
      set => this.edition = value;
    }

    public JedVersion Version
    {
      get => this.version;
      set => this.version = value;
    }

    public int UserLimit
    {
      get => this.licenseType != LicenseType.Trial ? this.userLimit : 0;
      set => this.userLimit = value;
    }

    public int UserLimitFlexPercent
    {
      get => this.userLimitFlexPct;
      set => this.userLimitFlexPct = value;
    }

    public int UserLimitWithFlex
    {
      get
      {
        return this.UserLimit <= 0 ? this.UserLimit : (int) Math.Ceiling((double) this.UserLimit + (double) (this.userLimitFlexPct * this.UserLimit) / 100.0);
      }
    }

    public int UserLimitWith90Percent
    {
      get => (int) Math.Ceiling((double) this.UserLimit - (double) (10 * this.UserLimit) / 100.0);
    }

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = value;
    }

    public bool IsBrokerEdition => this.edition == EncompassEdition.Broker;

    public bool IsBankerEdition => this.edition == EncompassEdition.Banker;

    public bool IsTrialVersion => this.licenseType == LicenseType.Trial;

    public bool IsFullVersion => this.licenseType == LicenseType.Full;

    public static LicenseInfo Parse(string licenseStr)
    {
      try
      {
        string str = (string) null;
        lock (LicenseInfo.jed)
        {
          LicenseInfo.jed.b();
          str = LicenseInfo.jed.a((Stream) new MemoryStream(LicenseInfo.helper.StringToBytes(licenseStr)));
        }
        string[] strArray = str.Split('|');
        if (strArray.Length >= 9)
          return new LicenseInfo(strArray[0], strArray[2], LicenseInfo.Convert3XEdition((Encompass3XEdition) int.Parse(strArray[4])), JedVersion.Parse(strArray[5]), int.Parse(strArray[6]), int.Parse(strArray[7]), (LicenseType) int.Parse(strArray[1]), DateTime.Parse(strArray[3]), strArray[8] == "1");
        if (strArray.Length >= 8)
          return new LicenseInfo(strArray[0], strArray[2], LicenseInfo.Convert3XEdition((Encompass3XEdition) int.Parse(strArray[4])), JedVersion.Parse(strArray[5]), int.Parse(strArray[6]), int.Parse(strArray[7]), (LicenseType) int.Parse(strArray[1]), DateTime.Parse(strArray[3]), true);
        if (strArray.Length >= 7)
          return new LicenseInfo(strArray[0], strArray[2], LicenseInfo.Convert3XEdition((Encompass3XEdition) int.Parse(strArray[4])), JedVersion.Parse(strArray[5]), int.Parse(strArray[6]), 10, (LicenseType) int.Parse(strArray[1]), DateTime.Parse(strArray[3]), true);
        return strArray.Length == 6 ? new LicenseInfo(strArray[0], strArray[2], LicenseInfo.Convert3XEdition((Encompass3XEdition) int.Parse(strArray[4])), JedVersion.Parse(strArray[5]), -1, 0, (LicenseType) int.Parse(strArray[1]), DateTime.Parse(strArray[3]), true) : new LicenseInfo(strArray[0], strArray[2], EncompassEdition.Broker, JedVersion.Unknown, -1, 0, (LicenseType) int.Parse(strArray[1]), DateTime.Parse(strArray[3]), true);
      }
      catch
      {
        throw new Exception("Invalid license format");
      }
    }

    public override string ToString()
    {
      string A_0 = this.cdKey + "|" + (object) (int) this.licenseType + "|" + this.clientId + "|" + this.expirationDate.ToShortDateString() + "|" + (object) (int) this.edition + "|" + (object) this.version + "|" + (object) this.userLimit + "|" + (object) this.userLimitFlexPct + "|" + (this.enabled ? (object) "1" : (object) "0");
      lock (LicenseInfo.jed)
      {
        LicenseInfo.jed.b();
        byte[] bytes = LicenseInfo.jed.b(A_0);
        return LicenseInfo.helper.BytesToString(bytes);
      }
    }

    public static EncompassEdition Convert3XEdition(Encompass3XEdition edition)
    {
      switch (edition)
      {
        case Encompass3XEdition.None:
          return EncompassEdition.None;
        case Encompass3XEdition.Custom:
        case Encompass3XEdition.Banker:
          return EncompassEdition.Banker;
        default:
          return EncompassEdition.Broker;
      }
    }
  }
}
