// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.DeploymentHandlers.DeploymentHandler
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Manifests;
using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.DeploymentHandlers
{
  internal class DeploymentHandler : DeploymentHandlerBase
  {
    private string installationURL;
    internal readonly string AppCompanyName;
    private SourceDeploymentHandler sourceDeployHandler;
    private FileStream deployManifestFileStream;
    private FileStream appManifestFileStream;
    private FileStream installationUrlFileStream;
    private const string installationUrlFile = "InstallationURL";

    internal DeploymentHandler(string root, string appSuiteName, string executableName)
      : base(root, appSuiteName, executableName)
    {
      this.init();
      string str1 = this.Root.Replace("/", "\\");
      int num = str1.LastIndexOf("\\UAC\\");
      string str2 = str1.Substring(num + 5);
      int length = str2.IndexOf("\\");
      if (length <= 0)
        return;
      this.AppCompanyName = str2.Substring(0, length);
    }

    private void init()
    {
      if (BasicUtils.IsHttpOrHttps(this.Root))
        throw new Exception(this.Root + ": cannot use http URL; must be a local path");
      if (Directory.Exists(this.Root))
        return;
      Directory.CreateDirectory(this.Root);
    }

    private string startupPathRegKeyPath
    {
      get
      {
        return "Software\\" + this.AppCompanyName + "\\SmartClient\\" + AssemblyResolver.AppStartupPath.Replace("\\", "/");
      }
    }

    internal bool OfflineReady
    {
      get
      {
        if (this.deployManifest == null && !File.Exists(this.deployManifestFilePath))
          return false;
        bool offlineReady = false;
        ApplicationManifest applicationManifest = this.appManifest;
        if (applicationManifest == null)
        {
          try
          {
            applicationManifest = this.GetCachedAppManifest();
          }
          catch (Exception ex)
          {
            if (ex.Message.IndexOf("Root element is missing") >= 0)
            {
              this.GetAndCacheManifestsFromSource();
              this.appManifest = this.GetCachedAppManifest();
            }
          }
        }
        if (applicationManifest != null)
          offlineReady = applicationManifest.DoBackgroundDownload || this.offlineReadyVersion != null ? applicationManifest.AsmIdVersion.ToString() == this.offlineReadyVersion : this.isOfflineReady(applicationManifest.AsmIdVersion);
        if (!offlineReady && this.offlineReadyFilePath != null)
          SystemUtils.MutexFileDelete(this.offlineReadyFilePath);
        return offlineReady;
      }
    }

    private string offlineReadyFilePath
    {
      get
      {
        try
        {
          return this.Root + this.UrlSeparator + this.DeployManifest.GetUACAppFolder(this.executableName) + this.UrlSeparator + ResolverConsts.OfflineReadyFileNamePrefix + this.executableName;
        }
        catch
        {
          return (string) null;
        }
      }
    }

    private string offlineReadyVersion
    {
      get
      {
        if (this.offlineReadyFilePath == null)
          return (string) null;
        if (!File.Exists(this.offlineReadyFilePath))
          return (string) null;
        string[] strArray = File.ReadAllLines(this.offlineReadyFilePath);
        return strArray.Length != 0 ? strArray[0] : (string) null;
      }
    }

    private void writeToOfflineReadyFile(string msg)
    {
      if (this.offlineReadyFilePath == null)
        return;
      SystemUtils.MutexFileWrite(this.offlineReadyFilePath, msg);
    }

    private void writeVersionToOfflineReadyFile(Version version)
    {
      if (this.offlineReadyFilePath == null)
        return;
      SystemUtils.MutexFileWrite(this.offlineReadyFilePath, version.ToString());
    }

    internal string InstallationURL
    {
      get
      {
        string path = (string) null;
        if (this.installationURL == null)
        {
          try
          {
            path = Path.Combine(this.Root, nameof (InstallationURL));
            if (File.Exists(path))
            {
              string dd = File.ReadAllText(path);
              if ((dd ?? "").Trim() != "")
              {
                this.installationURL = XT.DSB64(dd, ResolverConsts.KB64);
                BasicUtils.DisplayDebuggingInfo(nameof (InstallationURL), "Read installation URL '" + this.installationURL + "' from file '" + path + "'.");
              }
            }
          }
          catch (Exception ex)
          {
            AssemblyResolver.Instance.WriteToEventLog("Error reading installation URL from file '" + path + "': " + ex.Message, EventLogEntryType.Information);
            this.installationURL = (string) null;
          }
          if (this.installationURL == null)
          {
            using (RegistryKey registryKey = BasicUtils.GetRegistryHive(this.AppCompanyName).OpenSubKey(this.startupPathRegKeyPath))
            {
              if (registryKey == null)
                return (string) null;
              string str = (string) registryKey.GetValue(nameof (InstallationURL));
              if (BasicUtils.IsNullOrEmpty(str))
              {
                this.installationURL = (string) null;
              }
              else
              {
                this.installationURL = XT.DSB64(str, ResolverConsts.KB64);
                BasicUtils.DisplayDebuggingInfo(nameof (InstallationURL), "Read installation URL '" + this.installationURL + "' from registry.");
              }
            }
          }
        }
        return this.installationURL;
      }
      set
      {
        string d = value;
        string path = (string) null;
        if (d != null)
        {
          try
          {
            path = Path.Combine(this.Root, nameof (InstallationURL));
            File.WriteAllText(path, XT.ESB64(d, ResolverConsts.KB64));
            BasicUtils.DisplayDebuggingInfo(nameof (InstallationURL), "Wrote installation URL '" + d + "' to file '" + path + "'.");
          }
          catch
          {
            try
            {
              d = XT.DSB64(File.ReadAllText(path), ResolverConsts.KB64);
              BasicUtils.DisplayDebuggingInfo(nameof (InstallationURL), "Setting installation URL using value '" + d + "' from file '" + path + "'.");
            }
            catch (Exception ex)
            {
              AssemblyResolver.Instance.WriteToEventLog("Unable to write to/read from installation URL file '" + path + "': " + ex.Message, EventLogEntryType.Information);
            }
          }
          if (File.Exists(path))
            this.installationUrlFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        RegistryKey registryKey = (RegistryKey) null;
        try
        {
          registryKey = BasicUtils.GetRegistryHive(this.AppCompanyName).OpenSubKey(this.startupPathRegKeyPath, true) ?? BasicUtils.GetRegistryHive(this.AppCompanyName).CreateSubKey(this.startupPathRegKeyPath);
          if (d != null)
          {
            registryKey.SetValue(nameof (InstallationURL), (object) XT.ESB64(d, ResolverConsts.KB64));
            BasicUtils.DisplayDebuggingInfo(nameof (InstallationURL), "Wrote installation URL '" + d + "' to registry.");
          }
          else
          {
            registryKey.DeleteValue(nameof (InstallationURL), false);
            BasicUtils.DisplayDebuggingInfo(nameof (InstallationURL), "Deleted installation URL from registry.");
          }
        }
        catch (SecurityException ex)
        {
          AssemblyResolver.Instance.WriteToEventLog("[Deployment Handler] Error opening registry '" + this.startupPathRegKeyPath + "' for write access: " + ex.Message, EventLogEntryType.Warning);
        }
        finally
        {
          registryKey?.Close();
        }
        this.installationURL = d;
      }
    }

    internal void SetInstallationURL(string value, bool alsoSetRegistry)
    {
      if (alsoSetRegistry)
        this.InstallationURL = value;
      else
        this.installationURL = value;
    }

    private string deployManifestFilePath
    {
      get => Path.Combine(this.Root, this.appSuiteName + ResolverConsts.DeployManifestExt);
    }

    private string appManifestFilePath
    {
      get
      {
        return this.AppFolderUrl == null ? (string) null : Path.Combine(this.AppFolderUrl, this.executableName + ResolverConsts.AppManifestExt);
      }
    }

    internal override string AppFolderUrl
    {
      get
      {
        if (this.DeployManifest == null)
          return (string) null;
        string path = this.Root + this.UrlSeparator + this.DeployManifest.GetUACAppFolder(this.executableName);
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
    }

    internal override DeploymentManifest DeployManifest
    {
      get
      {
        if (this.deployManifest == null)
        {
          try
          {
            this.deployManifest = this.GetCachedDeployManifest();
          }
          catch (Exception ex)
          {
            if (ex.Message.IndexOf("Root element is missing") >= 0)
            {
              this.GetAndCacheManifestsFromSource();
              this.deployManifest = this.GetCachedDeployManifest();
            }
          }
          if (this.deployManifest == null)
            throw new Exception("Cannot find deployment manifest from local UAC folder.");
        }
        return this.deployManifest;
      }
      set
      {
        this.deployManifest = value;
        DeploymentManifest deploymentManifest = (DeploymentManifest) null;
        try
        {
          if (File.Exists(this.deployManifestFilePath))
            deploymentManifest = new DeploymentManifest(File.ReadAllText(this.deployManifestFilePath));
        }
        catch (Exception ex)
        {
          AssemblyResolver.Instance.WriteToEventLog("Error reading deployment manifest from '" + this.deployManifestFilePath + "': " + ex.Message, EventLogEntryType.Warning);
        }
        try
        {
          if (deploymentManifest != null)
          {
            if (!(deploymentManifest.AsmIdVersion != this.deployManifest.AsmIdVersion))
              goto label_19;
          }
          Process[] processesByName = Process.GetProcessesByName("EEFPrinterHandler");
          if (processesByName != null && processesByName.Length != 0)
          {
            foreach (Process process in processesByName)
            {
              try
              {
                process.Kill();
              }
              catch
              {
              }
            }
            int num = (int) MessageBox.Show("There is an update of Encompass SmartClient.  SmartClient will terminate and apply the update.", "SmartClient");
            Process.GetCurrentProcess().Kill();
          }
          this.deployManifest.Save(this.deployManifestFilePath, DeploymentHandlerBase.defaultEncoding);
        }
        catch (Exception ex)
        {
          if (!File.Exists(this.deployManifestFilePath))
            throw new Exception("Unable to save deployment manifest: " + ex.Message);
          AssemblyResolver.Instance.WriteToEventLog("Cannot save downloaded deployment manifest to '" + this.deployManifestFilePath + "': " + ex.Message + "\r\nThe manifest file may be locked by another SmartClient process. Use cached deployment manifest.", EventLogEntryType.Information);
          this.deployManifest = (DeploymentManifest) null;
          DeploymentManifest deployManifest = this.DeployManifest;
        }
label_19:
        this.deployManifestFileStream = new FileStream(this.deployManifestFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      }
    }

    internal DeploymentManifest GetCachedDeployManifest()
    {
      return !File.Exists(this.deployManifestFilePath) ? (DeploymentManifest) null : new DeploymentManifest(File.ReadAllText(this.deployManifestFilePath, DeploymentHandlerBase.defaultEncoding));
    }

    internal override ApplicationManifest AppManifest
    {
      get
      {
        if (this.appManifest == null)
        {
          try
          {
            this.appManifest = this.GetCachedAppManifest();
          }
          catch (Exception ex)
          {
            if (ex.Message.IndexOf("Root element is missing") >= 0)
            {
              this.GetAndCacheManifestsFromSource();
              this.appManifest = this.GetCachedAppManifest();
            }
          }
          if (this.appManifest == null)
            throw new Exception("Cannot find application manifest from local UAC folder.");
        }
        return this.appManifest;
      }
      set
      {
        this.appManifest = value;
        if (!this.sourceDeployHandler.AppManifestLoadedFromCache)
        {
          try
          {
            this.appManifest.Save(this.appManifestFilePath, DeploymentHandlerBase.defaultEncoding);
          }
          catch (Exception ex)
          {
            if (ex is UnauthorizedAccessException && string.Compare(Path.GetFileName(this.appManifestFilePath), "ClickLoanMain.exe.man", true) == 0)
            {
              AssemblyResolver.Instance.WriteToEventLog("[ClickLoan] Unauthorized access exception thrown while saving ClickLoanMain.exe.man: " + ex.Message, EventLogEntryType.Warning);
            }
            else
            {
              if (!File.Exists(this.appManifestFilePath))
                throw new Exception("Unable to save application manifest: " + ex.Message);
              AssemblyResolver.Instance.WriteToEventLog("Cannot save application manifest to '" + this.appManifestFilePath + "'. The manifest file may be locked by another SmartClient process. Use cached application manifest.", EventLogEntryType.Information);
              this.appManifest = (ApplicationManifest) null;
              ApplicationManifest appManifest = this.AppManifest;
            }
          }
        }
        try
        {
          this.appManifestFileStream = new FileStream(this.appManifestFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        catch (Exception ex)
        {
          if (!(ex is FileNotFoundException) || string.Compare(Path.GetFileName(this.appManifestFilePath), "ClickLoanMain.exe.man", true) != 0)
            throw ex;
          AssemblyResolver.Instance.WriteToEventLog("[ClickLoan] Error trying to read-lock ClickLoanMain.exe.man: " + ex.Message, EventLogEntryType.Warning);
        }
      }
    }

    internal ApplicationManifest GetCachedAppManifest()
    {
      return !File.Exists(this.appManifestFilePath) ? (ApplicationManifest) null : new ApplicationManifest(File.ReadAllText(this.appManifestFilePath, DeploymentHandlerBase.defaultEncoding));
    }

    private void getManifestsFromSource() => this.getManifestsFromSource((string) null);

    private void getManifestsFromSource(string installUrl)
    {
      this.sourceDeployHandler = new SourceDeploymentHandler(this, installUrl ?? this.InstallationURL, this.appSuiteName, this.executableName);
      this.DeployManifest = this.sourceDeployHandler.DeployManifest;
      this.AppManifest = this.sourceDeployHandler.AppManifest;
      string offlineReadyVersion = this.offlineReadyVersion;
      if (offlineReadyVersion == null)
        return;
      if (!(this.AppManifest.AsmIdVersion.ToString() != offlineReadyVersion))
        return;
      try
      {
        if (this.offlineReadyFilePath == null || !File.Exists(this.offlineReadyFilePath))
          return;
        File.Delete(this.offlineReadyFilePath);
      }
      catch
      {
      }
    }

    internal void GetAndCacheManifestsFromSource()
    {
      this.GetAndCacheManifestsFromSource((string) null);
    }

    internal void GetAndCacheManifestsFromSource(string installUrl)
    {
      this.getManifestsFromSource(installUrl);
    }

    internal ResFileInfo GetResFileInfo(string resFile)
    {
      return this.AppManifest != null ? this.AppManifest.GetResFileInfo(resFile) : throw new Exception("No application manifest info.");
    }

    internal AsmFileInfo GetAsmFileInfo(string asmNameWithoutExt)
    {
      return this.AppManifest != null ? this.AppManifest.GetAsmFileInfo(asmNameWithoutExt) : throw new Exception("No application manifest info.");
    }

    internal byte[] GetAssemblyBytes(string asmNameWOExt)
    {
      AsmFileInfo asmFileInfo = this.GetAsmFileInfo(asmNameWOExt);
      if (asmFileInfo == null)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "Cannot find the entry of assembly " + asmNameWOExt + " in the application manifest.", this.executableName);
        return (byte[]) null;
      }
      string path = DeployUtils.RemoveDeployExtension(Path.Combine(this.AppFolderUrl, asmFileInfo.Codebase));
      if (!BasicUtils.IsNullOrEmpty(asmFileInfo.Group))
        this.downloadAndCacheAppFileGroup(asmFileInfo.Group);
      return this.isCachedFileUpToDate((AppFileInfo) asmFileInfo, this.AppFolderUrl) ? File.ReadAllBytes(path) : this.downloadAndCacheFile((AppFileInfo) asmFileInfo, true);
    }

    internal string GetAndCacheAssembly(string asmNameWOExt)
    {
      AsmFileInfo asmFileInfo = this.GetAsmFileInfo(asmNameWOExt);
      string progressTitle = string.Empty;
      if (asmFileInfo == null)
      {
        string message = "Cannot find the entry for assembly " + asmNameWOExt + " in the application manifest.";
        if (BasicUtils.RegistryDebugLevel >= 1)
          AssemblyResolver.WriteToEventLogS(message, EventLogEntryType.Warning);
        if (BasicUtils.RegistryDebugLevel >= 5)
        {
          int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, message, this.executableName, msgBoxIcon: MessageBoxIcon.Exclamation);
        }
        return (string) null;
      }
      string andCacheAssembly = DeployUtils.RemoveDeployExtension(Path.Combine(this.AppFolderUrl, asmFileInfo.Codebase));
      if (this.isCachedFileUpToDate((AppFileInfo) asmFileInfo, this.AppFolderUrl))
        return andCacheAssembly;
      if (!BasicUtils.IsNullOrEmpty(asmFileInfo.Group))
        this.downloadAndCacheAppFileGroup(asmFileInfo.Group);
      if (asmFileInfo.Name.Contains("DotNetBrowser.Chromium.Win"))
        progressTitle = asmFileInfo.Name;
      this.downloadAndCacheFile((AppFileInfo) asmFileInfo, true, (string) null, progressTitle);
      return andCacheAssembly;
    }

    internal KeyValuePair<string, bool> GetAndCacheResourceFileFromSource(string filePath)
    {
      return this.GetAndCacheResourceFileFromSource(filePath, (string) null);
    }

    internal KeyValuePair<string, bool> GetAndCacheResourceFileFromSource(
      string filePath,
      string cachePath)
    {
      if (BasicUtils.IsNullOrEmpty(cachePath))
        cachePath = this.AppFolderUrl;
      ResFileInfo resFileInfo = !Path.IsPathRooted(filePath) ? this.AppManifest.GetResFileInfo(filePath) : throw new IOException(filePath + ": " + this.executableName + " system file path should not be rooted");
      if (resFileInfo == null)
        throw new FileNotFoundException(filePath + ": cannot find the resource file information from the application manifest");
      bool flag = false;
      if (!this.isCachedFileUpToDate((AppFileInfo) resFileInfo, cachePath))
      {
        this.downloadAndCacheFile((AppFileInfo) resFileInfo, true, cachePath, (string) null);
        flag = true;
      }
      return new KeyValuePair<string, bool>(Path.Combine(cachePath, filePath), flag);
    }

    internal void GetAndCacheResourceFilesFromSource(string searchPattern)
    {
      this.GetAndCacheResourceFilesFromSource(searchPattern, (string) null);
    }

    internal void GetAndCacheResourceFilesFromSource(string searchPattern, string cachePath)
    {
      if (BasicUtils.IsNullOrEmpty(cachePath))
        cachePath = this.AppFolderUrl;
      ResFileInfo[] allResFileInfos = this.AppManifest.GetAllResFileInfos();
      if (allResFileInfos == null)
        return;
      foreach (AppFileInfo appFileInfo in allResFileInfos)
      {
        string str = DeployUtils.RemoveDeployExtension(appFileInfo.FilePath);
        if (BasicUtils.PathMatch(str, searchPattern, false))
          this.GetAndCacheResourceFileFromSource(str, cachePath);
      }
    }

    private bool isCachedFileUpToDate(AppFileInfo appFileInfo)
    {
      return this.isCachedFileUpToDate(appFileInfo, (string) null);
    }

    private bool isCertExpired(AppFileInfo appFileInfo, string cachePath)
    {
      bool flag = false;
      try
      {
        if (BasicUtils.IsNullOrEmpty(cachePath))
          cachePath = this.AppFolderUrl;
        string path2 = DeployUtils.RemoveDeployExtension(appFileInfo.FilePath);
        string str = Path.Combine(cachePath, path2);
        if (File.Exists(str))
          flag = CertificateHelper.isCertExpired(str);
      }
      catch
      {
        AssemblyResolver.WriteToEventLogS("Verify the cert on :" + appFileInfo.FilePath, EventLogEntryType.Information);
        flag = false;
      }
      return flag;
    }

    private bool isCachedFileUpToDate(AppFileInfo appFileInfo, string cachePath)
    {
      if (BasicUtils.IsNullOrEmpty(cachePath))
        cachePath = this.AppFolderUrl;
      string str = DeployUtils.RemoveDeployExtension(appFileInfo.FilePath);
      string path = Path.Combine(cachePath, str);
      if (File.Exists(path))
      {
        if (this.AppManifest == null)
          this.GetAndCacheManifestsFromSource();
        if (appFileInfo is AsmFileInfo asmFileInfo)
        {
          if (new AsmFileInfo(cachePath, str).IsSameVersion(asmFileInfo) && !this.isCertExpired(appFileInfo, cachePath))
            return true;
        }
        else
        {
          ResFileInfo resFileInfo1 = (ResFileInfo) null;
          try
          {
            resFileInfo1 = new ResFileInfo(cachePath, str, HashAlgorithm.SHA1);
          }
          catch
          {
          }
          ResFileInfo resFileInfo2 = appFileInfo as ResFileInfo;
          if (resFileInfo1 == null || resFileInfo2.IsCLRAssembly && !resFileInfo1.IsCLRAssembly)
          {
            string fileName = Path.GetFileName(path);
            if (string.Compare(Path.GetExtension(fileName), ".exe", true) == 0 && string.Compare(fileName, ResolverConsts.RestoreAppLauncherExe, true) != 0 && SystemUtils.RunRestoreAppLauncher(fileName))
              resFileInfo1 = new ResFileInfo(cachePath, str, HashAlgorithm.SHA1);
          }
          if (resFileInfo1.IsSameVersion(resFileInfo2, HashAlgorithm.SHA1) && !this.isCertExpired(appFileInfo, cachePath))
            return true;
        }
      }
      return false;
    }

    private byte[] downloadAndCacheFile(string relativePath, long size)
    {
      return this.downloadAndCacheFile(relativePath, size, (string) null, (string) null);
    }

    private byte[] downloadAndCacheFile(
      string relativePath,
      long size,
      string cachePath,
      string progressTitle)
    {
      if (BasicUtils.IsNullOrEmpty(cachePath))
        cachePath = this.AppFolderUrl;
      bool flag1 = false;
      bool flag2 = false;
      string str1 = Path.Combine(cachePath.Trim(), relativePath.Trim());
      string str2 = DeployUtils.RemoveB64Extension(str1);
      if (str2.Length != str1.Length)
      {
        str1 = str2;
        flag1 = true;
      }
      string str3 = DeployUtils.RemoveDeployZipExtension(str1);
      if (str3.Length != str1.Length)
      {
        str1 = str3;
        flag2 = true;
      }
      long blockSize = Math.Min(size, this.AppManifest.DownloadBlockSize);
      string url = DeployUtils.AddDeployFileExtension(this.sourceDeployHandler.AppFolderUrl + this.sourceDeployHandler.UrlSeparator + relativePath);
      byte[] numArray;
      try
      {
        numArray = DeploymentHandlerBase.getFile(url, blockSize, size, progressTitle);
      }
      catch (Exception ex)
      {
        throw new Exception("Error downloading file " + url + "\r\n" + ex.Message);
      }
      if (numArray == null)
        throw new Exception("Cannot download assembly/resource file " + relativePath);
      if (flag1)
        numArray = Convert.FromBase64String(Encoding.ASCII.GetString(numArray));
      if (flag2)
        numArray = FileCompressor.UnzipBuffer(numArray);
      string directoryName = Path.GetDirectoryName(str1);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      SystemUtils.MutexFileWrite(str1, numArray);
      return numArray;
    }

    private byte[] downloadAndCacheFile(AppFileInfo appFileInfo, bool checkGroup)
    {
      return this.downloadAndCacheFile(appFileInfo, checkGroup, (string) null, (string) null);
    }

    private byte[] downloadAndCacheFile(
      AppFileInfo appFileInfo,
      bool checkGroup,
      string cachePath,
      string progressTitle)
    {
      cachePath = !BasicUtils.IsNullOrEmpty(cachePath) ? cachePath.Trim() : this.AppFolderUrl;
      if (appFileInfo == null)
        return (byte[]) null;
      if (checkGroup && !BasicUtils.IsNullOrEmpty(appFileInfo.Group))
      {
        this.downloadAndCacheAppFileGroup(appFileInfo.Group);
        string str1 = DeployUtils.RemoveDeployZipExtension(Path.Combine(this.AppFolderUrl, appFileInfo.FilePath.Trim()));
        if (cachePath.ToLower() != this.AppFolderUrl.ToLower())
        {
          if (!(appFileInfo is ResFileInfo resFileInfo1))
            throw new Exception("Only resource files are allowed to be downloaded to a folder other than the UAC folder.");
          string str2 = DeployUtils.RemoveDeployZipExtension(Path.Combine(cachePath, appFileInfo.FilePath.Trim()));
          ResFileInfo resFileInfo2 = (ResFileInfo) null;
          if (File.Exists(str2))
            resFileInfo2 = new ResFileInfo(cachePath, DeployUtils.RemoveDeployZipExtension(resFileInfo1.FilePath), HashAlgorithm.SHA1);
          if (resFileInfo2 == null || this.isCertExpired(appFileInfo, cachePath) || !resFileInfo2.IsSameVersion(resFileInfo1, HashAlgorithm.SHA1))
          {
            SystemUtils.MutexFileCopy(str1, str2, true);
            if (!resFileInfo1.IsCLRAssembly)
              SystemUtils.MutexFileCopy(str1 + ResolverConsts.Sha1HashFileExt, str2 + ResolverConsts.Sha1HashFileExt, true);
          }
        }
        return File.ReadAllBytes(str1);
      }
      byte[] numArray = this.downloadAndCacheFile(appFileInfo.FilePath, appFileInfo.Size, cachePath, progressTitle);
      if (!(appFileInfo is ResFileInfo resFileInfo))
        return numArray;
      resFileInfo.WriteHashToHashFile(cachePath, HashAlgorithm.SHA1, true);
      return numArray;
    }

    internal byte[] GetResourceFileBytes(string filePath)
    {
      return this.AppManifest.GetResFileInfo(filePath) == null ? (byte[]) null : File.ReadAllBytes(this.GetAndCacheResourceFileFromSource(filePath, this.AppFolderUrl).Key);
    }

    internal string GetResourceFileText(string filePath)
    {
      byte[] resourceFileBytes = this.GetResourceFileBytes(filePath);
      return resourceFileBytes == null ? (string) null : Encoding.ASCII.GetString(resourceFileBytes);
    }

    private byte[] handleAppFileGroupDownload(AsmFileInfo asmFileInfo)
    {
      return this.handleAppFileGroupDownload(asmFileInfo, (string) null);
    }

    private byte[] handleAppFileGroupDownload(AsmFileInfo asmFileInfo, string cachePath)
    {
      return this.handleAppFileGroupDownload(this.AppManifest.GetAppFileGroupInfo(asmFileInfo.Group), asmFileInfo.Codebase, cachePath);
    }

    private byte[] handleAppFileGroupDownload(AppFileGroupInfo groupInfo, string asmCodebase)
    {
      return this.handleAppFileGroupDownload(groupInfo, asmCodebase, (string) null);
    }

    private byte[] handleAppFileGroupDownload(
      AppFileGroupInfo groupInfo,
      string asmCodebase,
      string cachePath)
    {
      if (BasicUtils.IsNullOrEmpty(cachePath))
        cachePath = this.AppFolderUrl;
      byte[] bytes = DeploymentHandlerBase.getFile(this.sourceDeployHandler.AppFolderUrl + this.sourceDeployHandler.UrlSeparator + ResolverConsts.AppFileGroupsDir + this.sourceDeployHandler.UrlSeparator + this.executableName + this.sourceDeployHandler.UrlSeparator + groupInfo.Codebase, this.AppManifest.DownloadBlockSize, groupInfo.Size, "Downloading application file group " + groupInfo.Name);
      if (bytes == null)
        throw new Exception("Cannot load group file " + groupInfo.Codebase);
      if (groupInfo.Codebase.EndsWith(ResolverConsts.B64Ext + ".deploy", StringComparison.CurrentCultureIgnoreCase))
        bytes = Convert.FromBase64String(Encoding.ASCII.GetString(bytes));
      string str1 = Path.Combine(cachePath, Guid.NewGuid().ToString());
      try
      {
        Directory.CreateDirectory(str1);
        string str2 = Path.Combine(str1, DeployUtils.RemoveB64Extension(DeployUtils.RemoveDeployFileExtension(groupInfo.Codebase)));
        File.WriteAllBytes(str2, bytes);
        IProgressBar progBar = (IProgressBar) new ProgressBarForm("Unzipping Assemblies");
        FileCompressor.Unzip(str2, str1, progBar);
        File.Delete(str2);
        foreach (string file in Directory.GetFiles(str1, "*", SearchOption.AllDirectories))
        {
          string filePath = file.Substring(str1.Length);
          while (filePath.Length > 0 && filePath[0] == '\\')
            filePath = filePath.Substring(1);
          string str3 = DeployUtils.RemoveDeployExtension(filePath);
          ResFileInfo resFileInfo = this.AppManifest.GetResFileInfo(str3);
          if (!this.isCachedFileUpToDate(resFileInfo == null ? (AppFileInfo) this.AppManifest.GetAsmFileInfo(Path.GetFileNameWithoutExtension(str3)) : (AppFileInfo) resFileInfo))
            SystemUtils.MutexFileMove(file, Path.Combine(cachePath, str3), true);
          else if (BasicUtils.GetRegistryDebugLevel(this.AppCompanyName) >= 3)
            AssemblyResolver.Instance.WriteToEventLog("File '" + str3 + "' in app file group '" + groupInfo.Name + "' is up to date.", EventLogEntryType.Information);
          if (resFileInfo != null && !resFileInfo.IsCLRAssembly)
            resFileInfo.WriteHashToHashFile(cachePath, HashAlgorithm.SHA1, true);
        }
      }
      finally
      {
        if (Directory.Exists(str1))
          Directory.Delete(str1, true);
      }
      return asmCodebase == null ? (byte[]) null : File.ReadAllBytes(Path.Combine(cachePath, DeployUtils.RemoveDeployExtension(asmCodebase)));
    }

    private void downloadAndCacheAppFileGroup(string groupName)
    {
      this.downloadAndCacheAppFileGroup(groupName, (string) null);
    }

    private void downloadAndCacheAppFileGroup(string groupName, string cachePath)
    {
      if (BasicUtils.IsNullOrEmpty(cachePath))
        cachePath = this.AppFolderUrl;
      AppFileGroupInfo appFileGroupInfo = this.AppManifest.GetAppFileGroupInfo(groupName);
      if (appFileGroupInfo == null)
        return;
      AppFileInfo[] groupAppFileInfos = this.AppManifest.GetGroupAppFileInfos(groupName);
      if (groupAppFileInfos == null || groupAppFileInfos.Length == 0)
        return;
      long num1 = 0;
      bool[] flagArray = new bool[groupAppFileInfos.Length];
      for (int index = 0; index < groupAppFileInfos.Length; ++index)
      {
        flagArray[index] = !this.isCachedFileUpToDate(groupAppFileInfos[index], cachePath);
        if (flagArray[index])
          num1 += groupAppFileInfos[index].Size;
      }
      if (num1 == 0L)
        return;
      long num2 = 110;
      if (!BasicUtils.IsNullOrEmpty(appFileGroupInfo.Codebase) && appFileGroupInfo.Size <= num1 * num2 / 100L)
      {
        this.handleAppFileGroupDownload(appFileGroupInfo, (string) null, cachePath);
      }
      else
      {
        IProgressBar progressBar = (IProgressBar) null;
        try
        {
          progressBar = (IProgressBar) new ProgressBarForm("Downloading Startup Group Application (Assembly and Resource) Files");
          progressBar.Minimum = 0;
          progressBar.Maximum = groupAppFileInfos.Length;
          progressBar.Value = 0;
          progressBar.ShowProgressBar();
          for (int index = 0; index < groupAppFileInfos.Length; ++index)
          {
            if (flagArray[index])
              this.downloadAndCacheFile(groupAppFileInfos[index], false, cachePath, (string) null);
            progressBar.Value = index + 1;
          }
        }
        finally
        {
          progressBar?.CloseProgressBar();
        }
      }
    }

    internal void StartBackgroundDownload()
    {
      if (!this.AppManifest.DoBackgroundDownload)
        return;
      ThreadPriority threadPriority;
      switch (this.AppManifest.BgDownloadThreadPriority.Trim().ToLower())
      {
        case "highest":
        case "abovenormal":
        case "normal":
          threadPriority = ThreadPriority.Normal;
          break;
        case "belownormal":
          threadPriority = ThreadPriority.BelowNormal;
          break;
        case "lowest":
          threadPriority = ThreadPriority.Lowest;
          break;
        default:
          threadPriority = ThreadPriority.BelowNormal;
          break;
      }
      new Thread(new ThreadStart(this.backgroundDownloadThreadStart))
      {
        Priority = threadPriority,
        IsBackground = true,
        Name = (this.executableName + " Background Download")
      }.Start();
    }

    private void backgroundDownloadThreadStart()
    {
      AsmFileInfo[] allAsmFileInfos = this.AppManifest.GetAllAsmFileInfos();
      ResFileInfo[] allResFileInfos = this.AppManifest.GetAllResFileInfos();
      BasicUtils.DisplayDebuggingInfo("BgDownloadInterval", "BgDownloadInterval = " + this.appManifest.BgDownloadInterval.ToString());
      int num1 = 5;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        if (allAsmFileInfos != null)
        {
          for (int index2 = 0; index2 < allAsmFileInfos.Length; ++index2)
          {
            if (!allAsmFileInfos[index2].OnDemandOnly)
            {
              for (int index3 = 30; index3 >= 0; --index3)
              {
                try
                {
                  if (!this.isCachedFileUpToDate((AppFileInfo) allAsmFileInfos[index2]))
                  {
                    this.downloadAndCacheFile((AppFileInfo) allAsmFileInfos[index2], true, this.AppFolderUrl, (string) null);
                    if (this.appManifest.BgDownloadInterval > 0)
                    {
                      Thread.Sleep(this.appManifest.BgDownloadInterval);
                      break;
                    }
                    break;
                  }
                  break;
                }
                catch (Exception ex)
                {
                  if (index3 == 0)
                    AssemblyResolver.Instance.WriteToEventLog("An error occurred while background downloading assembly '" + allAsmFileInfos[index2].Codebase + "'.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), EventLogEntryType.Error);
                  else
                    Thread.Sleep(100);
                }
              }
            }
          }
        }
        if (allResFileInfos != null)
        {
          for (int index4 = 0; index4 < allResFileInfos.Length; ++index4)
          {
            for (int index5 = 30; index5 >= 0; --index5)
            {
              if (!allResFileInfos[index4].OnDemandOnly)
              {
                try
                {
                  if (!this.isCachedFileUpToDate((AppFileInfo) allResFileInfos[index4]))
                  {
                    this.downloadAndCacheFile((AppFileInfo) allResFileInfos[index4], true, this.AppFolderUrl, (string) null);
                    if (this.appManifest.BgDownloadInterval > 0)
                    {
                      Thread.Sleep(this.appManifest.BgDownloadInterval);
                      break;
                    }
                    break;
                  }
                  break;
                }
                catch (Exception ex)
                {
                  if (index5 == 0)
                    AssemblyResolver.Instance.WriteToEventLog("An error occurred while background downloading resource file '" + allResFileInfos[index4].FilePath + "'.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString(), EventLogEntryType.Error);
                  else
                    Thread.Sleep(100);
                }
              }
            }
          }
        }
        try
        {
          if (this.isOfflineReady(this.AppManifest.AsmIdVersion))
            break;
          if (this.offlineReadyFilePath != null)
            SystemUtils.MutexFileDelete(this.offlineReadyFilePath);
        }
        catch (Exception ex)
        {
          if (ex is UnauthorizedAccessException && string.Compare(AssemblyResolver.Instance.ExecutableNameWOExt, "ClickLoanMain", true) == 0)
          {
            AssemblyResolver.Instance.WriteToEventLog("[ClickLoan] Unauthorized access exception thrown while saving " + this.offlineReadyFilePath + ": " + ex.Message, EventLogEntryType.Warning);
          }
          else
          {
            string str = "Error writing version to offline-ready file.\r\nDetails: " + ex.Message + "\r\n" + ex.StackTrace.ToString();
            AssemblyResolver.Instance.WriteToEventLog(str, EventLogEntryType.Error);
            if (index1 == num1 - 1)
            {
              int num2 = (int) MessageBox.Show(str, this.executableName);
            }
          }
        }
      }
    }

    private bool isOfflineReady(Version asmIdVersion)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      ResFileInfo[] allResFileInfos = this.AppManifest.GetAllResFileInfos();
      if (allResFileInfos != null)
      {
        for (int index = 0; index < allResFileInfos.Length; ++index)
        {
          if (!this.isCachedFileUpToDate((AppFileInfo) allResFileInfos[index], this.AppFolderUrl))
          {
            if (!allResFileInfos[index].OnDemandOnly)
              return false;
            stringList1.Add(allResFileInfos[index].FilePath);
          }
        }
      }
      AsmFileInfo[] allAsmFileInfos = this.AppManifest.GetAllAsmFileInfos();
      if (allAsmFileInfos != null)
      {
        for (int index = 0; index < allAsmFileInfos.Length; ++index)
        {
          string lower = allAsmFileInfos[index].Name.ToLower();
          if (!(lower == this.executableName.ToLower()) && !(lower == ResolverConsts.AsmResolver.ToLower()) && !this.isCachedFileUpToDate((AppFileInfo) allAsmFileInfos[index], this.AppFolderUrl))
          {
            if (!allAsmFileInfos[index].OnDemandOnly)
              return false;
            stringList2.Add(allAsmFileInfos[index].FilePath);
          }
        }
      }
      if (stringList1.Count == 0 && stringList2.Count == 0)
      {
        this.writeVersionToOfflineReadyFile(asmIdVersion);
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(asmIdVersion.ToString());
        if (stringList2.Count > 0)
        {
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("[Assemblies]");
          stringBuilder.AppendLine(string.Join("\r\n", stringList2.ToArray()));
        }
        if (stringList1.Count > 0)
        {
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("[Resource Files]");
          stringBuilder.AppendLine(string.Join("\r\n", stringList1.ToArray()));
        }
        this.writeToOfflineReadyFile(stringBuilder.ToString());
      }
      return true;
    }
  }
}
