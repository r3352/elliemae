// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Version.ServerHotUpdateInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.Version
{
  [Serializable]
  public class ServerHotUpdateInfo : IComparable
  {
    private string filename;
    private JedServerVersion jedServerVersion;
    private DateTime installDate;

    public ServerHotUpdateInfo(string filename)
    {
      this.installDate = DateTime.MinValue;
      this.filename = Path.GetFileName(filename);
      this.jedServerVersion = new JedServerVersion(Path.GetFileNameWithoutExtension(this.filename));
    }

    public ServerHotUpdateInfo(FileInfo fileInfo)
      : this(fileInfo.Name)
    {
    }

    public ServerHotUpdateInfo(XmlElement huElement)
    {
      this.jedServerVersion = new JedServerVersion(huElement.GetAttribute("id"));
      this.filename = Path.GetFileName(this.readXMLSubElement(huElement, nameof (Filename)));
      this.installDate = DateTime.Parse(this.readXMLSubElement(huElement, nameof (InstallDate)));
    }

    public JedServerVersion Version => this.jedServerVersion;

    public string Filename => this.filename;

    public DateTime InstallDate => this.installDate;

    public int CompareTo(object obj)
    {
      return this.Version.CompareTo((object) ((ServerHotUpdateInfo) obj).Version);
    }

    private string readXMLSubElement(XmlElement parentElement, string path)
    {
      XmlNode xmlNode = parentElement.SelectSingleNode(path);
      return xmlNode == null ? "" : xmlNode.InnerText;
    }
  }
}
