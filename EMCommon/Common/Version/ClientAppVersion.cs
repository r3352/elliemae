// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Version.ClientAppVersion
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.VersionInterface15;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.Version
{
  [Serializable]
  public class ClientAppVersion : IComparable
  {
    public static readonly ClientAppVersion MinVersion = new ClientAppVersion(new JedVersion(0, 0, 0), 0, (string) null);
    public static readonly ClientAppVersion MaxVersion = new ClientAppVersion(new JedVersion(99, 99, 999), 999, (string) null);
    private JedVersion majorVersion;
    private int sequenceNumber;
    private int patch;
    private string displayVersionString;

    public ClientAppVersion(
      JedVersion majorVersion,
      int sequenceNumber,
      string displayVersionString)
    {
      this.majorVersion = majorVersion;
      this.patch = this.getPatchNumber(sequenceNumber);
      this.sequenceNumber = this.normalizeSequenceNumber(sequenceNumber);
      this.displayVersionString = displayVersionString;
    }

    public ClientAppVersion(string majorVersion, int sequenceNumber, string displayVersionString)
    {
      this.majorVersion = JedVersion.Parse(majorVersion);
      this.patch = this.getPatchNumber(sequenceNumber);
      this.sequenceNumber = this.normalizeSequenceNumber(sequenceNumber);
      this.displayVersionString = displayVersionString;
    }

    private int normalizeSequenceNumber(int sequenceNumber)
    {
      return sequenceNumber >= 100 ? sequenceNumber / 100 : sequenceNumber;
    }

    private int getPatchNumber(int sequenceNumber)
    {
      return sequenceNumber >= 100 ? sequenceNumber % 100 : 0;
    }

    public JedVersion MajorVersion => this.majorVersion;

    public int HotfixSequenceNumber => this.sequenceNumber;

    public int RevisionAndPatch => this.sequenceNumber * 100 + this.patch;

    public string NormalizedVersion
    {
      get => this.majorVersion.NormalizedVersion + "." + this.sequenceNumber.ToString("000");
    }

    public string DisplayVersion
    {
      get => this.majorVersion.FullVersion + "." + (object) this.sequenceNumber;
    }

    public string DisplayVersionString
    {
      get
      {
        return (this.displayVersionString ?? "").Trim() == "" ? this.DisplayVersion : this.displayVersionString;
      }
    }

    public int CompareTo(object obj)
    {
      ClientAppVersion clientAppVersion = (ClientAppVersion) obj;
      int num = this.majorVersion.CompareTo((object) clientAppVersion.majorVersion);
      return num != 0 ? num : this.RevisionAndPatch - clientAppVersion.RevisionAndPatch;
    }

    public static ClientAppVersion Parse(string versionText)
    {
      string[] strArray = versionText.Split('.');
      return strArray.Length == 4 ? new ClientAppVersion(JedVersion.Parse(string.Join(".", strArray, 0, 3)), int.Parse(strArray[3]), (string) null) : throw new ArgumentException("Invalid version number format");
    }

    public override string ToString() => this.DisplayVersion;

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (!(obj is ClientAppVersion clientAppVersion))
        throw new ArgumentException("Invalid object type specified");
      return this.MajorVersion.Equals((object) clientAppVersion.MajorVersion) && this.HotfixSequenceNumber == clientAppVersion.HotfixSequenceNumber;
    }

    public override int GetHashCode()
    {
      return this.MajorVersion.GetHashCode() ^ this.HotfixSequenceNumber.GetHashCode();
    }
  }
}
