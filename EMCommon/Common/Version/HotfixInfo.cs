// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Version.HotfixInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.VersionInterface15;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.Version
{
  [Serializable]
  public class HotfixInfo : IComparable
  {
    private string id;
    private ClientAppVersion clientVersion;
    private string filename;
    private DateTime installDate;
    private DateTime timestamp = DateTime.MinValue;

    public HotfixInfo(string filename)
    {
      this.installDate = DateTime.MinValue;
      this.filename = filename;
      this.parseHotfixName();
    }

    public HotfixInfo(FileInfo fileInfo)
      : this(fileInfo.Name)
    {
    }

    public HotfixInfo(XmlElement hfElement)
    {
      this.id = hfElement.GetAttribute(nameof (id));
      string attribute = hfElement.GetAttribute("revisionAndPatch");
      if ((attribute ?? "") == "")
        attribute = hfElement.GetAttribute("sequenceNumber");
      this.clientVersion = new ClientAppVersion(JedVersion.Parse(hfElement.GetAttribute("appliesToVersion")), int.Parse(attribute), hfElement.GetAttribute("displayVersionString"));
      this.filename = this.readXMLSubElement(hfElement, nameof (Filename));
      this.installDate = DateTime.Parse(this.readXMLSubElement(hfElement, nameof (InstallDate)));
      this.parseTimestamp();
    }

    public string ID => this.id;

    public ClientAppVersion Version => this.clientVersion;

    public string DisplayVersionString => this.clientVersion.DisplayVersionString;

    public string Filename => this.filename;

    public DateTime InstallDate => this.installDate;

    public DateTime Timestamp => this.timestamp;

    public int CompareTo(object obj)
    {
      HotfixInfo hotfixInfo = (HotfixInfo) obj;
      int num = this.Version.CompareTo((object) hotfixInfo.Version);
      if (num != 0)
        return num;
      if (this.Timestamp < hotfixInfo.Timestamp)
        return -1;
      return this.Timestamp > hotfixInfo.Timestamp ? 1 : 0;
    }

    private string parseTimestamp()
    {
      int length = this.filename.LastIndexOf("@");
      string timestamp = this.filename;
      if (length > 0)
      {
        string s = timestamp.Substring(length + 1).Replace("_", " ").Replace("%", ":").Replace("#", ":");
        try
        {
          timestamp = timestamp.Substring(0, length);
          this.timestamp = DateTime.Parse(s);
        }
        catch (Exception ex)
        {
          Tracing.Log(true, "Error", nameof (HotfixInfo), "Error parsing .emzip file date: " + ex.Message);
        }
      }
      return timestamp;
    }

    private void parseHotfixName()
    {
      Regex regex = new Regex("^(?<id>(?<version>[0-9]+\\.[0-9]+\\.[0-9]+)\\.(?<seqnum>[0-9]+))\\.emzip", RegexOptions.IgnoreCase);
      string input = this.parseTimestamp();
      string displayVersionString = (string) null;
      int length = input.IndexOf("-");
      if (length >= 0)
      {
        displayVersionString = this.filename.Substring(0, length);
        input = this.filename.Substring(length + 1);
      }
      Match match = regex.Match(input);
      if (!match.Success)
        throw new ApplicationException("Parse error in regular expression file name");
      this.id = match.Groups["id"].Value;
      this.clientVersion = new ClientAppVersion(JedVersion.Parse(match.Groups["version"].Value), int.Parse(match.Groups["seqnum"].Value), displayVersionString);
    }

    private string readXMLSubElement(XmlElement parentElement, string path)
    {
      XmlNode xmlNode = parentElement.SelectSingleNode(path);
      return xmlNode == null ? "" : xmlNode.InnerText;
    }
  }
}
