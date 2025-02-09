// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Version.VersionInformation
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.VersionInterface15;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.Version
{
  public class VersionInformation
  {
    private const string className = "VersionInformation";
    private static string sw = Tracing.SwCommon;
    private static VersionInformation currentVersion = (VersionInformation) null;
    private XmlDocument versionDoc;
    private JedVersion version = JedVersion.Unknown;
    private JedVersion origVersion = JedVersion.Unknown;
    private string versionDescription = "";
    private string aboutPageVersionInfo;
    private ArrayList hotfixes = new ArrayList();
    private List<ServerHotUpdateInfo> hotUpdates = new List<ServerHotUpdateInfo>();
    private static FileSystemWatcher versionInformationWatcher = (FileSystemWatcher) null;

    public static VersionInformation CurrentVersion => VersionInformation.currentVersion;

    static VersionInformation() => VersionInformation.ReloadVersionInformation();

    public static void WaitForVersionControlToExit()
    {
      int num = 1000;
      Process[] processesByName;
      do
      {
        Thread.Sleep(300);
        processesByName = Process.GetProcessesByName("VersionControl");
      }
      while (--num > 0 && processesByName != null && processesByName.Length != 0);
    }

    public static void ReloadVersionInformation()
    {
      VersionInformation.currentVersion = new VersionInformation(VersionInformation.getVersionFile());
    }

    private static string getVersionFile()
    {
      return !AssemblyResolver.IsSmartClient ? EnConfigurationSettings.GlobalSettings.VersionInformationFile : AssemblyResolver.GetResourceFileFullPath(EnGlobalSettings.EncompassVersionFileName);
    }

    public static void AutoReloadVersionInformation()
    {
      if (VersionInformation.versionInformationWatcher != null)
        return;
      string versionFile = VersionInformation.getVersionFile();
      VersionInformation.versionInformationWatcher = new FileSystemWatcher(Path.GetDirectoryName(versionFile), Path.GetFileName(versionFile));
      VersionInformation.versionInformationWatcher.Created += new FileSystemEventHandler(VersionInformation.onVersionFileChanged);
      VersionInformation.versionInformationWatcher.Changed += new FileSystemEventHandler(VersionInformation.onVersionFileChanged);
      VersionInformation.versionInformationWatcher.EnableRaisingEvents = true;
    }

    private static void onVersionFileChanged(object sender, FileSystemEventArgs args)
    {
      Tracing.Log(VersionInformation.sw, nameof (VersionInformation), TraceLevel.Verbose, "Before reloading EncompassVersion.xml: " + VersionInformation.CurrentVersion.ToString());
      VersionInformation.ReloadVersionInformation();
      Tracing.Log(VersionInformation.sw, nameof (VersionInformation), TraceLevel.Verbose, "After reloading EncompassVersion.xml: " + VersionInformation.CurrentVersion.ToString());
    }

    public VersionInformation(string versionFilePath)
    {
      try
      {
        this.versionDoc = new XmlDocument();
        this.versionDoc.Load(versionFilePath);
        this.version = JedVersion.Parse(this.readNodeText((XmlNode) this.versionDoc, "VersionInfo/Version", true));
        this.versionDescription = this.readNodeText((XmlNode) this.versionDoc, "VersionInfo/Description", false);
        this.aboutPageVersionInfo = this.readNodeText((XmlNode) this.versionDoc, "VersionInfo/AboutPageVersionInfo", false);
        if (this.versionDescription == "")
          this.versionDescription = this.version.ToString();
        try
        {
          this.origVersion = JedVersion.Parse(this.readNodeText((XmlNode) this.versionDoc, "VersionInfo/RevisionOf", true));
        }
        catch
        {
          this.origVersion = this.version;
        }
        foreach (XmlElement selectNode in this.versionDoc.SelectNodes("VersionInfo/Hotfix"))
          this.hotfixes.Add((object) new HotfixInfo(selectNode));
        this.hotfixes.Sort();
        foreach (XmlElement selectNode in this.versionDoc.SelectNodes("VersionInfo/ServerHotUpdate"))
          this.hotUpdates.Add(new ServerHotUpdateInfo(selectNode));
        this.hotUpdates.Sort();
      }
      catch (Exception ex)
      {
        throw new Exception("Error parsing Encompass version file \"" + versionFilePath + "\".", ex);
      }
    }

    public JedVersion Version => this.version;

    public JedVersion OriginalVersion => this.origVersion;

    public string VersionDescription => this.versionDescription;

    public string DisplayVersion
    {
      get => this.version.ToString() + "." + (object) this.GetMaxInstalledHotfixSequenceNumber();
    }

    public string DisplayVersionString
    {
      get
      {
        string displayVersionString = this.GetMaxInstalledHotfixSequenceNumberDisplayVersionString();
        return (displayVersionString ?? "").Trim() == "" ? this.DisplayVersion : displayVersionString;
      }
    }

    public string AboutPageVersionInfo => this.aboutPageVersionInfo;

    public string GetExtendedVersion(EncompassEdition edition)
    {
      string normalizedVersion = this.version.NormalizedVersion;
      switch (edition)
      {
        case EncompassEdition.Broker:
          normalizedVersion += "O";
          break;
        case EncompassEdition.Banker:
          normalizedVersion += "B";
          break;
      }
      return normalizedVersion;
    }

    public string GetExtendedVersionWithHotfix(EncompassEdition edition)
    {
      HotfixInfo maxInstalledHotfix = this.GetMaxInstalledHotfix();
      string normalizedVersion = (maxInstalledHotfix != null ? maxInstalledHotfix.Version : new ClientAppVersion(this.Version, 0, "")).NormalizedVersion;
      switch (edition)
      {
        case EncompassEdition.Broker:
          normalizedVersion += "O";
          break;
        case EncompassEdition.Banker:
          normalizedVersion += "B";
          break;
      }
      return normalizedVersion;
    }

    public HotfixInfo[] GetAppliedHotfixes()
    {
      return (HotfixInfo[]) this.hotfixes.ToArray(typeof (HotfixInfo));
    }

    public ServerHotUpdateInfo[] GetAppliedServerHotUpdates() => this.hotUpdates.ToArray();

    public HotfixInfo GetMaxInstalledHotfix()
    {
      HotfixInfo[] appliedHotfixes = this.GetAppliedHotfixes();
      HotfixInfo maxInstalledHotfix = (HotfixInfo) null;
      foreach (HotfixInfo hotfixInfo in appliedHotfixes)
      {
        if (maxInstalledHotfix == null || hotfixInfo.Version.RevisionAndPatch > maxInstalledHotfix.Version.RevisionAndPatch)
          maxInstalledHotfix = hotfixInfo;
      }
      return maxInstalledHotfix;
    }

    public int GetMaxInstalledHotfixSequenceNumber()
    {
      HotfixInfo maxInstalledHotfix = this.GetMaxInstalledHotfix();
      return maxInstalledHotfix == null ? 0 : maxInstalledHotfix.Version.HotfixSequenceNumber;
    }

    public int GetMaxInstalledServerHotUpdateNumber()
    {
      ServerHotUpdateInfo[] serverHotUpdates = this.GetAppliedServerHotUpdates();
      int serverHotUpdateNumber = 0;
      foreach (ServerHotUpdateInfo serverHotUpdateInfo in serverHotUpdates)
      {
        if (serverHotUpdateInfo.Version.UpdateNumber > serverHotUpdateNumber)
          serverHotUpdateNumber = serverHotUpdateInfo.Version.UpdateNumber;
      }
      return serverHotUpdateNumber;
    }

    public int GetMaxInstalledServerMajorUpdateNumber() => this.version.Revision;

    public string GetMaxInstalledHotfixSequenceNumberDisplayVersionString()
    {
      return this.GetMaxInstalledHotfix()?.DisplayVersionString;
    }

    private string readNodeText(XmlNode parentNode, string name, bool required)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(name);
      if (xmlNode == null & required)
        throw new Exception("Version document missing required element " + name);
      return xmlNode == null ? "" : xmlNode.InnerText;
    }

    public string[] GetDownloadedSuFiles(System.Version encMajorVersion, bool isSHU)
    {
      string shusDirectory = EnConfigurationSettings.GlobalSettings.GetShusDirectory(encMajorVersion);
      string searchPattern;
      if (isSHU)
        searchPattern = encMajorVersion.Major.ToString() + "." + (object) encMajorVersion.Minor + "." + (object) encMajorVersion.Build + ".*.?emzip";
      else
        searchPattern = encMajorVersion.Major.ToString() + "." + (object) encMajorVersion.Minor + ".*.emzip";
      return Directory.GetFiles(shusDirectory, searchPattern, SearchOption.TopDirectoryOnly);
    }

    public override string ToString() => this.versionDoc.OuterXml;
  }
}
