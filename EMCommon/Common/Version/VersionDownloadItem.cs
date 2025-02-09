// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Version.VersionDownloadItem
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.VersionInterface15;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.Version
{
  public class VersionDownloadItem
  {
    private JedVersion sourceVersion;
    private JedVersion targetVersion;
    private string description;
    private string downloadFilePath;
    private string updateFilename;
    private string updateURL;
    private DateTime downloadDate = DateTime.MinValue;
    private AffectedSystems affectedSystems;
    private const string SourceVersionAttr = "sourceVersion";
    private const string TargetVersionAttr = "targetVersion";
    private const string AffectedSystemsAttr = "affectedSystems";
    private const string UpdateFileElemement = "UpdateFile";
    private const string UpdateURLElement = "UpdateURL";
    private const string DownloadTimeElement = "DownloadTime";
    private const string DescriptionElement = "Description";

    public VersionDownloadItem()
    {
    }

    public VersionDownloadItem(
      JedVersion sourceVersion,
      JedVersion targetVersion,
      string description,
      string updateURL,
      AffectedSystems affectedSystems)
    {
      this.sourceVersion = sourceVersion;
      this.targetVersion = targetVersion;
      this.updateURL = updateURL;
      this.description = description;
      this.affectedSystems = affectedSystems;
      this.updateFilename = updateURL.Substring(updateURL.LastIndexOf("/") + 1);
      if (!(this.description == ""))
        return;
      this.description = sourceVersion.ToString();
    }

    internal VersionDownloadItem(string downloadFilePath, XmlElement versionElement)
    {
      this.sourceVersion = JedVersion.Parse(versionElement.GetAttribute(nameof (sourceVersion)));
      this.targetVersion = JedVersion.Parse(versionElement.GetAttribute(nameof (targetVersion)));
      this.updateFilename = this.readXMLSubElement(versionElement, "UpdateFile");
      this.updateURL = this.readXMLSubElement(versionElement, nameof (UpdateURL));
      this.downloadDate = DateTime.Parse(this.readXMLSubElement(versionElement, "DownloadTime"));
      this.description = this.readXMLSubElement(versionElement, nameof (Description));
      this.downloadFilePath = downloadFilePath;
      if (!this.downloadFilePath.EndsWith("\\"))
        this.downloadFilePath += "\\";
      try
      {
        this.affectedSystems = (AffectedSystems) int.Parse(versionElement.GetAttribute(nameof (targetVersion)));
      }
      catch
      {
        this.affectedSystems = AffectedSystems.ClientAndServer;
      }
    }

    private string readXMLSubElement(XmlElement parentElement, string path)
    {
      XmlNode xmlNode = parentElement.SelectSingleNode(path);
      return xmlNode == null ? "" : xmlNode.InnerText;
    }

    internal XmlElement ToXml(XmlDocument parentDoc)
    {
      XmlElement element = parentDoc.CreateElement("VersionUpdate");
      element.SetAttribute("sourceVersion", this.sourceVersion.ToString());
      element.SetAttribute("targetVersion", this.targetVersion.ToString());
      element.SetAttribute("affectedSystems", ((int) this.affectedSystems).ToString());
      element.AppendChild((XmlNode) parentDoc.CreateElement("UpdateFile")).InnerText = this.updateFilename;
      element.AppendChild((XmlNode) parentDoc.CreateElement("UpdateURL")).InnerText = this.updateURL;
      element.AppendChild((XmlNode) parentDoc.CreateElement("DownloadTime")).InnerText = this.downloadDate.ToShortDateString();
      element.AppendChild((XmlNode) parentDoc.CreateElement("Description")).InnerText = this.description;
      return element;
    }

    public JedVersion SourceVersion => this.sourceVersion;

    public JedVersion TargetVersion => this.targetVersion;

    public string UpdateFilename
    {
      get => this.updateFilename;
      set => this.updateFilename = value;
    }

    public string DownloadFilePath
    {
      get => this.downloadFilePath;
      set => this.downloadFilePath = value;
    }

    public string FullUpdateFilename => this.downloadFilePath + this.updateFilename;

    public string UpdateURL => this.updateURL;

    public string Description => this.description;

    public DateTime DownloadDate => this.downloadDate;

    public AffectedSystems AffectedSystems => this.affectedSystems;

    public void UpdateDownloadDate(DateTime when) => this.downloadDate = when;

    public bool IsAffected(AffectedSystems affectedSystems)
    {
      return (this.affectedSystems & affectedSystems) == affectedSystems;
    }

    public bool UpdateFileExists() => File.Exists(this.FullUpdateFilename);

    public FileStream OpenUpdateFileStream()
    {
      return this.UpdateFileExists() ? new FileStream(this.FullUpdateFilename, FileMode.Open, FileAccess.Read, FileShare.Read) : (FileStream) null;
    }

    public FileStream CreateUpdateFileStream()
    {
      return new FileStream(this.FullUpdateFilename, FileMode.Create, FileAccess.Write, FileShare.None);
    }
  }
}
