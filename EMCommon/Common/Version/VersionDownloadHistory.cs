// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Version.VersionDownloadHistory
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.VersionInterface15;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.Version
{
  public class VersionDownloadHistory
  {
    private const string historyFilename = "history.xml";
    private ArrayList historyItems = new ArrayList();
    private string downloadFilePath;
    private string historyFilePath;
    public static readonly JedVersion WildcardVersion = new JedVersion(99, 99, 999);

    public VersionDownloadHistory(string updatesFolder)
    {
      this.downloadFilePath = updatesFolder;
      if (!this.downloadFilePath.EndsWith("\\"))
        this.downloadFilePath += "\\";
      this.historyFilePath = this.downloadFilePath + "history.xml";
      this.Refresh();
    }

    public void Clear()
    {
      this.historyItems.Clear();
      this.saveHistory();
    }

    public void Refresh()
    {
      this.historyItems.Clear();
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        xmlDocument.Load(this.historyFilePath);
      }
      catch
      {
        return;
      }
      foreach (XmlElement selectNode in xmlDocument.SelectNodes("VersionHistory/VersionUpdate"))
      {
        try
        {
          this.historyItems.Add((object) new VersionDownloadItem(this.downloadFilePath, selectNode));
        }
        catch
        {
        }
      }
    }

    public void AddItem(VersionDownloadItem versionItem)
    {
      this.DeleteVersion(versionItem.SourceVersion, versionItem.TargetVersion);
      versionItem.DownloadFilePath = this.downloadFilePath;
      this.historyItems.Add((object) versionItem);
      this.saveHistory();
    }

    public void DeleteVersion(JedVersion sourceVersion, JedVersion targetVersion)
    {
      ArrayList arrayList = new ArrayList();
      foreach (VersionDownloadItem historyItem in this.historyItems)
      {
        if ((historyItem.SourceVersion == sourceVersion || historyItem.SourceVersion == VersionDownloadHistory.WildcardVersion) && historyItem.TargetVersion == targetVersion)
          arrayList.Add((object) historyItem);
      }
      if (arrayList.Count == 0)
        return;
      foreach (VersionDownloadItem versionDownloadItem in arrayList)
        this.historyItems.Remove((object) versionDownloadItem);
      this.saveHistory();
    }

    public VersionDownloadItem FindItem(
      JedVersion sourceVersion,
      JedVersion targetVersion,
      VersionMatchType matchType)
    {
      VersionDownloadItem versionDownloadItem = (VersionDownloadItem) null;
      foreach (VersionDownloadItem historyItem in this.historyItems)
      {
        if (matchType == VersionMatchType.ExactSource && (historyItem.SourceVersion == sourceVersion || historyItem.SourceVersion == VersionDownloadHistory.WildcardVersion) || matchType == VersionMatchType.ExactTarget && historyItem.TargetVersion == targetVersion || matchType == VersionMatchType.Exact && (historyItem.SourceVersion == sourceVersion || historyItem.SourceVersion == VersionDownloadHistory.WildcardVersion) && historyItem.TargetVersion == targetVersion)
          return historyItem;
        if (matchType == VersionMatchType.UpgradePath && (historyItem.SourceVersion == sourceVersion || historyItem.SourceVersion == VersionDownloadHistory.WildcardVersion) && historyItem.TargetVersion <= targetVersion && (versionDownloadItem == null || versionDownloadItem.TargetVersion < historyItem.TargetVersion))
          versionDownloadItem = historyItem;
      }
      return versionDownloadItem;
    }

    private void saveHistory()
    {
      XmlDocument parentDoc = new XmlDocument();
      XmlElement xmlElement = (XmlElement) parentDoc.AppendChild((XmlNode) parentDoc.CreateElement("VersionHistory"));
      foreach (VersionDownloadItem historyItem in this.historyItems)
        xmlElement.AppendChild((XmlNode) historyItem.ToXml(parentDoc));
      parentDoc.Save(this.historyFilePath);
    }
  }
}
