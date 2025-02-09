// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.SystemSettings
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class SystemSettings
  {
    public static string IpcPortName;
    private const string className = "SystemSettings";
    private static readonly string sw = Tracing.SwOutsideLoan;
    public const string LogFileBaseName = "Encompass";
    public const string ServerLogFileBaseName = "EncompassServer";
    public const string LogFileExt = ".log";
    public const int MaxRecentLoans = 5;
    public static readonly string TrashFolder = "(Trash)";
    public static readonly string ArchiveFolder = "(Archive)";
    public static readonly string AllFolders = "<All Folders>";
    public const string LoanFileExt = ".em";
    public const string LoanFile = "loan.em";
    public const string BlankLoanName = "BlankLoan";
    private static int TempFileCounter = 0;
    private static System.Threading.Timer heartbeatTimer = (System.Threading.Timer) null;
    public static readonly string LocalAppDir = (string) null;
    public static readonly string LogDir = (string) null;
    public static readonly string LocalSettingsDir = (string) null;
    public static readonly string UserLocalSettingsDir = (string) null;
    public static readonly string MapFileRelPath = (string) null;
    public static readonly string FormRelDir = (string) null;
    public static readonly string ImageRelDir = (string) null;
    public static readonly string SettingsSplashImgFileRelPath = (string) null;
    public static readonly string UpdatesDir = (string) null;
    public static readonly string EpassDataDir = (string) null;
    public static readonly string EpassDirRelPath = (string) null;
    public static readonly string EpassDir = (string) null;
    public static readonly string LocalCustomLetterDir = (string) null;
    public static readonly string LocalBorLetterDir = (string) null;
    public static readonly string LocalBizLetterDir = (string) null;
    public static readonly string LocalExternalAttachmentsDir = (string) null;
    public static readonly string LocalDocumentDir = (string) null;
    public static readonly string CustomLetterMapRelPath = (string) null;
    public static readonly string CountyNameMapRelPath = (string) null;
    public static readonly string BorLetterMapRelPath = (string) null;
    public static readonly string BizLetterMapRelPath = (string) null;
    public static readonly string DocDirRelPath = (string) null;
    public static readonly string DocDirAbsPath = (string) null;
    public static readonly string FontDirRelPath = (string) null;
    public static readonly string ConfigDir = (string) null;
    public static readonly string PointMapFileRelPath = (string) null;
    public static readonly string ContourMapFileRelPath = (string) null;
    public static readonly string ContourMapFileLPRelPath = (string) null;
    public static readonly string ZipCodeFileRelPath = (string) null;
    public static readonly string ZipCodeFile = (string) null;
    public static readonly string OutputDir = (string) null;
    public static readonly string DownloadDir = (string) null;
    public static readonly string PdfDataDirRelPath = (string) null;
    public static readonly string JedScriptDirRelPath = (string) null;
    public static readonly bool Debug = false;
    public static string HelpFile = (string) null;
    public static readonly string ServerFile = (string) null;
    private static readonly string appModeFile = (string) null;
    public static readonly string ContactSearchXmlRelPath = (string) null;
    public static readonly string OutFormAndFileMappingRelPath = (string) null;
    public static readonly string EMFormGroupListRelPath = (string) null;
    public static readonly string InOutFormMappingRelPath = (string) null;
    public static readonly string MaventLicenseTypesRelPath = (string) null;
    public static readonly string UcdFeeTypeEnumsPath = (string) null;
    public static readonly PrintEngineEnum PdfPrintEngine;
    public static readonly InstallationMode InstallationMode;
    public static ApplicationMode ApplicationMode = ApplicationMode.Unknown;
    private static string tempFolderRoot = (string) null;
    private static string tempImageFolder = (string) null;
    private static string settingsFolderRoot = (string) null;

    static SystemSettings()
    {
      AppDomain.CurrentDomain.ProcessExit += new EventHandler(SystemSettings.onAppDomainProcessExit);
      SystemSettings.Debug = EnConfigurationSettings.GlobalSettings.Debug;
      SystemSettings.LocalAppDir = EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory == null ? Application.StartupPath : EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory;
      if (!SystemSettings.LocalAppDir.EndsWith("\\"))
        SystemSettings.LocalAppDir += "\\";
      SystemSettings.LogDir = EnConfigurationSettings.GlobalSettings.AppLogDirectory;
      if (!SystemSettings.LogDir.EndsWith("\\"))
        SystemSettings.LogDir += "\\";
      if (!Directory.Exists(SystemSettings.LogDir))
        SystemSettings.createDirectory(SystemSettings.LogDir);
      if (!Directory.Exists(SystemSettings.LogDir + Environment.UserName))
        SystemSettings.createDirectory(SystemSettings.LogDir + Environment.UserName);
      SystemSettings.LocalSettingsDir = EnConfigurationSettings.GlobalSettings.AppSharedSettingsDirectory;
      if (!SystemSettings.LocalSettingsDir.EndsWith("\\"))
        SystemSettings.LocalSettingsDir += "\\";
      if (!Directory.Exists(SystemSettings.LocalSettingsDir))
        SystemSettings.createDirectory(SystemSettings.LocalSettingsDir);
      SystemSettings.EpassDataDir = SystemSettings.SettingsFolderRoot + "Epass\\";
      if (!Directory.Exists(SystemSettings.EpassDataDir))
        SystemSettings.createDirectory(SystemSettings.EpassDataDir);
      SystemSettings.UpdatesDir = SystemSettings.SettingsFolderRoot + "Updates\\";
      if (!Directory.Exists(SystemSettings.UpdatesDir))
        SystemSettings.createDirectory(SystemSettings.UpdatesDir);
      SystemSettings.UserLocalSettingsDir = SystemSettings.LocalSettingsDir + Environment.UserName + "\\";
      if (!Directory.Exists(SystemSettings.UserLocalSettingsDir))
        SystemSettings.createDirectory(SystemSettings.UserLocalSettingsDir);
      SystemSettings.ServerFile = SystemSettings.UserLocalSettingsDir + "server";
      SystemSettings.appModeFile = SystemSettings.UserLocalSettingsDir + "AppMode";
      SystemSettings.MapFileRelPath = "documents\\EncompassFields.dat";
      SystemSettings.FormRelDir = "documents\\forms\\";
      SystemSettings.ImageRelDir = "documents\\images\\";
      SystemSettings.SettingsSplashImgFileRelPath = SystemSettings.ImageRelDir + "SettingsSplashImage.png";
      SystemSettings.DocDirRelPath = "documents\\";
      SystemSettings.DocDirAbsPath = SystemSettings.LocalAppDir + SystemSettings.DocDirRelPath;
      SystemSettings.EpassDirRelPath = "Epass";
      SystemSettings.EpassDir = !AssemblyResolver.IsSmartClient ? SystemSettings.LocalAppDir + SystemSettings.EpassDirRelPath + "\\" : AssemblyResolver.GetResourceFileFolderPath(SystemSettings.EpassDirRelPath) + "\\";
      SystemSettings.PdfDataDirRelPath = "PdfForms\\";
      SystemSettings.JedScriptDirRelPath = "JedScripts\\";
      SystemSettings.ConfigDir = SystemSettings.LocalAppDir + "config\\";
      SystemSettings.PointMapFileRelPath = SystemSettings.DocDirRelPath + "Pnt2EMLite.txt";
      SystemSettings.ContourMapFileRelPath = SystemSettings.DocDirRelPath + "Csi2EllieMae.txt";
      SystemSettings.ContourMapFileLPRelPath = SystemSettings.DocDirRelPath + "CsiLP2EllieMaeLP.txt";
      SystemSettings.ZipCodeFileRelPath = SystemSettings.DocDirRelPath + "Zipcode.dat";
      SystemSettings.ZipCodeFile = SystemSettings.LocalAppDir + SystemSettings.ZipCodeFileRelPath;
      SystemSettings.FontDirRelPath = "fonts\\";
      if (!AssemblyResolver.IsSmartClient)
        SystemSettings.createHeartbeatFile();
      SystemSettings.LocalCustomLetterDir = SystemSettings.TempFolderRoot + "CustomLetters\\";
      if (!Directory.Exists(SystemSettings.LocalCustomLetterDir))
        SystemSettings.createDirectory(SystemSettings.LocalCustomLetterDir);
      SystemSettings.LocalBorLetterDir = SystemSettings.TempFolderRoot + "BorLetters\\";
      if (!Directory.Exists(SystemSettings.LocalBorLetterDir))
        SystemSettings.createDirectory(SystemSettings.LocalBorLetterDir);
      SystemSettings.LocalBizLetterDir = SystemSettings.TempFolderRoot + "BizLetters\\";
      if (!Directory.Exists(SystemSettings.LocalBizLetterDir))
        SystemSettings.createDirectory(SystemSettings.LocalBizLetterDir);
      SystemSettings.LocalExternalAttachmentsDir = SystemSettings.TempFolderRoot + "ExternalAttachments\\";
      if (!Directory.Exists(SystemSettings.LocalExternalAttachmentsDir))
        SystemSettings.createDirectory(SystemSettings.LocalExternalAttachmentsDir);
      SystemSettings.LocalDocumentDir = SystemSettings.TempFolderRoot + "ExternalDocuments\\";
      if (!Directory.Exists(SystemSettings.LocalDocumentDir))
        SystemSettings.createDirectory(SystemSettings.LocalDocumentDir);
      SystemSettings.OutputDir = SystemSettings.TempFolderRoot + "OutputPdf\\";
      if (!Directory.Exists(SystemSettings.OutputDir))
        SystemSettings.createDirectory(SystemSettings.OutputDir);
      SystemSettings.DownloadDir = SystemSettings.TempFolderRoot + "eFolder\\";
      if (!Directory.Exists(SystemSettings.DownloadDir))
        SystemSettings.createDirectory(SystemSettings.DownloadDir);
      SystemSettings.CustomLetterMapRelPath = SystemSettings.DocDirRelPath + "LetterMap.txt";
      SystemSettings.CountyNameMapRelPath = SystemSettings.DocDirRelPath + "CountyNameMap.xml";
      SystemSettings.BorLetterMapRelPath = SystemSettings.DocDirRelPath + "BorLetterMap.txt";
      SystemSettings.BizLetterMapRelPath = SystemSettings.DocDirRelPath + "BizLetterMap.txt";
      SystemSettings.HelpFile = SystemSettings.LocalAppDir + "documents\\help\\Encompass_Help.chm";
      SystemSettings.ContactSearchXmlRelPath = SystemSettings.DocDirRelPath + "ContactSearch.xml";
      SystemSettings.OutFormAndFileMappingRelPath = SystemSettings.DocDirRelPath + "OutFormAndFileMapping.xml";
      SystemSettings.EMFormGroupListRelPath = SystemSettings.DocDirRelPath + "EMFormGroupList.xml";
      SystemSettings.InOutFormMappingRelPath = SystemSettings.DocDirRelPath + "InOutFormMapping.xml";
      SystemSettings.MaventLicenseTypesRelPath = SystemSettings.DocDirRelPath + "MaventLicenseTypes.txt";
      SystemSettings.UcdFeeTypeEnumsPath = SystemSettings.DocDirRelPath + "UcdFeeTypeEnums.txt";
      string appSetting = EnConfigurationSettings.AppSettings[nameof (PdfPrintEngine)];
      if ((appSetting ?? "").Trim() == "")
      {
        SystemSettings.PdfPrintEngine = PrintEngineEnum.AdobeReader;
      }
      else
      {
        string lower = appSetting.ToLower();
        SystemSettings.PdfPrintEngine = lower.IndexOf("adobe") >= 0 || lower.IndexOf("acrobat") >= 0 ? PrintEngineEnum.AdobeReader : (lower.IndexOf("amyuni") < 0 ? (lower.IndexOf("external") < 0 ? PrintEngineEnum.Unknown : PrintEngineEnum.External) : PrintEngineEnum.Amyuni);
      }
      SystemSettings.InstallationMode = EnConfigurationSettings.GlobalSettings.InstallationMode;
    }

    public static string TempFolderRoot
    {
      get
      {
        if (SystemSettings.tempFolderRoot == null)
        {
          SystemSettings.tempFolderRoot = EnConfigurationSettings.GlobalSettings.AppTempDirectory;
          if (!SystemSettings.tempFolderRoot.EndsWith("\\"))
            SystemSettings.tempFolderRoot += "\\";
          int num1 = SystemSettings.tempFolderRoot.IndexOf("\\EncompassSC\\");
          int num2 = 0;
          if (num1 >= 0)
            num2 = SystemSettings.tempFolderRoot.Substring(num1 + "\\EncompassSC\\".Length).IndexOf("\\");
          if (num1 < 0 || num2 != 36)
          {
            SystemSettings.tempFolderRoot = SystemSettings.tempFolderRoot + EnConfigurationSettings.ApplicationSessionID + "\\";
            try
            {
              if (!Directory.Exists(SystemSettings.tempFolderRoot))
                Directory.CreateDirectory(SystemSettings.tempFolderRoot);
            }
            catch
            {
            }
          }
        }
        return SystemSettings.tempFolderRoot;
      }
    }

    public static string TempImageFolder
    {
      get
      {
        if (SystemSettings.tempImageFolder == null)
        {
          SystemSettings.tempImageFolder = Path.Combine(SystemSettings.tempFolderRoot, "Images");
          if (!Directory.Exists(SystemSettings.tempImageFolder))
            SystemSettings.createDirectory(SystemSettings.tempImageFolder);
        }
        return SystemSettings.tempImageFolder;
      }
    }

    public static string SettingsFolderRoot
    {
      get
      {
        if (SystemSettings.settingsFolderRoot == null)
        {
          SystemSettings.settingsFolderRoot = EnConfigurationSettings.GlobalSettings.AppSettingsDirectory;
          if (SystemSettings.settingsFolderRoot == null)
          {
            Tracing.Log(SystemSettings.sw, TraceLevel.Error, nameof (SystemSettings), "Cannot get configuration setting 'settingsFolderRoot'");
            throw new ApplicationException("Cannot get configuration setting 'settingsFolderRoot'");
          }
          if (!SystemSettings.settingsFolderRoot.EndsWith("\\"))
            SystemSettings.settingsFolderRoot += "\\";
        }
        return SystemSettings.settingsFolderRoot;
      }
    }

    private static string[] getServers(string serverFile)
    {
      string str = SystemSettings.readFileContents(serverFile);
      if (str == "")
        return new string[0];
      string[] servers = str.Replace(Environment.NewLine, "\u0001").Split('\u0001');
      for (int index = 0; index < servers.Length; ++index)
        servers[index] = servers[index].Trim();
      return servers;
    }

    private static void setServers(string serverFile, string[] servers)
    {
      SystemSettings.writeFileContents(serverFile, string.Join(Environment.NewLine, servers), 5);
    }

    public static string[] Servers
    {
      get => SystemSettings.getServers(SystemSettings.ServerFile);
      set => SystemSettings.setServers(SystemSettings.ServerFile, value);
    }

    public static string[] Servers2
    {
      get => SystemSettings.getServers(SystemSettings.ServerFile + "2");
      set => SystemSettings.setServers(SystemSettings.ServerFile + "2", value);
    }

    public static ApplicationMode DefaultApplicationMode
    {
      get
      {
        try
        {
          return (ApplicationMode) Enum.Parse(typeof (ApplicationMode), SystemSettings.readFileContents(SystemSettings.appModeFile).Trim(), true);
        }
        catch
        {
          return SystemSettings.InstallationMode == InstallationMode.Local ? ApplicationMode.Local : ApplicationMode.Server;
        }
      }
      set => SystemSettings.writeFileContents(SystemSettings.appModeFile, value.ToString(), 5);
    }

    public static string LastLoginName
    {
      get
      {
        return SystemSettings.readFileContents(Path.Combine(SystemSettings.UserLocalSettingsDir, "LastLogin")).Trim();
      }
      set
      {
        SystemSettings.writeFileContents(Path.Combine(SystemSettings.UserLocalSettingsDir, "LastLogin"), value, 5);
      }
    }

    public static string GetTempFileName(string srcFileName)
    {
      int num = Interlocked.Increment(ref SystemSettings.TempFileCounter);
      string path = SystemSettings.OutputDir + Path.GetFileNameWithoutExtension(srcFileName) + "-" + num.ToString() + Path.GetExtension(srcFileName);
      SystemSettings.createDirectory(Path.GetDirectoryName(path));
      return path;
    }

    public static string GetTempFileNameWithGivenFileName(string directoryName, string fileName)
    {
      string str = SystemSettings.OutputDir + directoryName;
      if (!Directory.Exists(str))
        Directory.CreateDirectory(str);
      return Path.Combine(str, fileName);
    }

    public static string GetTempFileNameWithExtension(string extension)
    {
      int num = Interlocked.Increment(ref SystemSettings.TempFileCounter);
      return SystemSettings.OutputDir + EnConfigurationSettings.ApplicationSessionID + "-" + num.ToString() + "." + extension;
    }

    public static string GetTempFileNameWithExtensionForSDC(string existingFilePath)
    {
      int num = Interlocked.Increment(ref SystemSettings.TempFileCounter);
      string str1 = existingFilePath.Contains("Edited") ? "" : "-Edited-";
      string withoutExtension = Path.GetFileNameWithoutExtension(existingFilePath);
      string str2 = existingFilePath.Contains("Edited") ? withoutExtension.Substring(0, withoutExtension.LastIndexOf('-') + 1) : withoutExtension;
      return Path.GetDirectoryName(existingFilePath) + "\\" + str2 + str1 + num.ToString() + Path.GetExtension(existingFilePath);
    }

    public static int GetTempfileCounter()
    {
      return Interlocked.Increment(ref SystemSettings.TempFileCounter);
    }

    public static void DeleteTempFiles()
    {
      SystemSettings.deleteFolderContents(SystemSettings.OutputDir, false);
    }

    public static void DeleteTempFolderFiles(string[] excludedFiles = null)
    {
      if (excludedFiles == null)
        excludedFiles = new string[2]
        {
          "Encompass.pid",
          "proc_info.txt"
        };
      SystemSettings.deleteFolderFiles(SystemSettings.TempFolderRoot, excludedFiles);
    }

    public static void deleteFolderFiles(string folderPath, string[] excludedFiles)
    {
      try
      {
        foreach (string file in Directory.GetFiles(folderPath))
        {
          try
          {
            bool flag = false;
            if (excludedFiles != null)
            {
              foreach (string excludedFile in excludedFiles)
              {
                if (string.Compare(excludedFile, Path.GetFileName(file), true) == 0)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
              File.Delete(file);
          }
          catch
          {
          }
        }
        foreach (string directory in Directory.GetDirectories(folderPath))
          SystemSettings.deleteFolderFiles(directory, excludedFiles);
      }
      catch
      {
      }
    }

    private static void onAppDomainProcessExit(object sender, EventArgs e)
    {
      if (SystemSettings.heartbeatTimer != null)
        SystemSettings.heartbeatTimer.Dispose();
      SystemSettings.deleteFolderContents(SystemSettings.TempFolderRoot, true);
    }

    private static void deleteFolderContents(string folderPath, bool deleteCurrentFolder)
    {
      try
      {
        foreach (string file in Directory.GetFiles(folderPath))
        {
          try
          {
            File.Delete(file);
          }
          catch
          {
          }
        }
        foreach (string directory in Directory.GetDirectories(folderPath))
          SystemSettings.deleteFolderContents(directory, true);
        if (!deleteCurrentFolder)
          return;
        Directory.Delete(folderPath, true);
      }
      catch
      {
      }
    }

    private static string readFileContents(string path)
    {
      try
      {
        if (!File.Exists(path))
          return "";
        using (StreamReader streamReader = new StreamReader(path, Encoding.ASCII))
          return streamReader.ReadToEnd();
      }
      catch
      {
        return "";
      }
    }

    private static void writeFileContents(string path, string contents, int repeatNumber)
    {
      for (int index = repeatNumber; index > 0; --index)
      {
        try
        {
          using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.ASCII))
          {
            streamWriter.Write(contents);
            break;
          }
        }
        catch (Exception ex)
        {
          if (index <= 1)
            throw new Exception("Failed to write to file '" + path + "': " + ex.Message);
          Thread.Sleep(500);
        }
      }
    }

    private static void createDirectory(string path)
    {
      try
      {
        Directory.CreateDirectory(path);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to create/access folder '" + path + "': " + ex.Message);
      }
    }

    private static void createHeartbeatFile()
    {
      string heartbeatFilePath = SystemSettings.getHeartbeatFilePath(SystemSettings.TempFolderRoot);
      Directory.CreateDirectory(Path.GetDirectoryName(heartbeatFilePath));
      SystemSettings.writeProcessInfo(heartbeatFilePath);
      SystemSettings.heartbeatTimer = new System.Threading.Timer(new TimerCallback(SystemSettings.updateHeartbeat), (object) SystemSettings.TempFolderRoot, TimeSpan.Zero, TimeSpan.FromMinutes(5.0));
      SystemSettings.startTempFolderCleanupThread();
    }

    private static void updateHeartbeat(object tempDir)
    {
      try
      {
        FileInfo fileInfo = new FileInfo(SystemSettings.getHeartbeatFilePath(tempDir.ToString()));
        if (!fileInfo.Exists)
          return;
        fileInfo.LastWriteTime = DateTime.Now;
      }
      catch
      {
      }
    }

    private static string getHeartbeatFilePath(string tempFolder)
    {
      return Path.Combine(tempFolder, "proc_info.txt");
    }

    private static void writeProcessInfo(string filePath)
    {
      try
      {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream, Encoding.Default))
          {
            streamWriter.Write(Environment.MachineName);
            streamWriter.Write(Environment.UserName);
            streamWriter.Write(DateTime.Now.ToString());
            streamWriter.Flush();
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to write process info file", ex);
      }
    }

    private static void startTempFolderCleanupThread()
    {
      new Thread(new ParameterizedThreadStart(SystemSettings.cleanupTempFolders))
      {
        IsBackground = true
      }.Start((object) EnConfigurationSettings.GlobalSettings.AppTempDirectory);
    }

    private static void cleanupTempFolders(object encTempFolder)
    {
      try
      {
        foreach (string directory in Directory.GetDirectories(encTempFolder.ToString()))
        {
          if (Path.GetFileName(directory).Length == 32)
          {
            FileInfo fileInfo = new FileInfo(SystemSettings.getHeartbeatFilePath(directory));
            if (!fileInfo.Exists || fileInfo.LastWriteTime < DateTime.Now.AddDays(-1.0))
              SystemSettings.deleteFolderContents(directory, true);
          }
        }
      }
      catch
      {
      }
    }

    public static string[] readUcdFeeList()
    {
      try
      {
        string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(SystemSettings.UcdFeeTypeEnumsPath, SystemSettings.LocalAppDir);
        return File.Exists(resourceFileFullPath) ? File.ReadAllLines(resourceFileFullPath) : new string[0];
      }
      catch (Exception ex)
      {
        return new string[0];
      }
    }
  }
}
