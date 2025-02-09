// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Licensing.LicenseFile
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.VersionInterface15;
using System;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.Licensing
{
  /// <summary>Summary description for LicenseFile.</summary>
  [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100ED611F91C2EEA49D628904356176F8405967CF8FD01EE7F2914038B5BA5F70C9EB8BE8489CD4F8E6DC00CE61D5127EE2B7D97AC08EA11B7D33829FFE313EDB7408F67A09F6226F942F86B01311DC7EE0088AA29491178EFEFF12B2826097CE5CAEE7B0DF8E69EE50D07034E8F79FA95CCD98BF9D9615E9EF53D5647217C330B9")]
  internal sealed class LicenseFile
  {
    private string licenseKey = "";
    private string clientId = "";
    private JedVersion version = JedVersion.Unknown;
    private string[] macAddresses = new string[0];
    private bool authorizeAllSessions;

    public string LicenseKey
    {
      get => this.licenseKey;
      set => this.licenseKey = value;
    }

    public string ClientID
    {
      get => this.clientId;
      set => this.clientId = value;
    }

    public JedVersion Version
    {
      get => this.version;
      set => this.version = value;
    }

    public string[] MacAddresses
    {
      get => this.macAddresses;
      set => this.macAddresses = value;
    }

    public bool AuthorizeAllSessions
    {
      get => this.authorizeAllSessions;
      set => this.authorizeAllSessions = value;
    }

    public bool IsValidMacAddress(string addr)
    {
      for (int index = 0; index < this.macAddresses.Length; ++index)
      {
        if (this.macAddresses[index] == addr)
          return true;
      }
      return false;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.licenseKey + "\n");
      stringBuilder.Append(this.clientId + "\n");
      stringBuilder.Append(this.version.ToString() + "\n");
      stringBuilder.Append(string.Join("|", this.macAddresses) + "\n");
      stringBuilder.Append(this.authorizeAllSessions ? "1" : "0");
      return stringBuilder.ToString();
    }

    public static LicenseFile Parse(string licenseInfo)
    {
      string[] strArray = licenseInfo.Split('\n');
      if (strArray.Length < 4)
        throw new ArgumentException("Invalid license file format", nameof (licenseInfo));
      LicenseFile licenseFile = new LicenseFile();
      try
      {
        licenseFile.LicenseKey = strArray[0];
        licenseFile.ClientID = strArray[1];
        licenseFile.Version = JedVersion.Parse(strArray[2]);
        licenseFile.MacAddresses = strArray[3].Split('|');
        if (strArray.Length > 4)
          licenseFile.AuthorizeAllSessions = strArray[4] == "1";
      }
      catch
      {
        throw new ArgumentException("Invalid license file format", nameof (licenseInfo));
      }
      return licenseFile;
    }
  }
}
