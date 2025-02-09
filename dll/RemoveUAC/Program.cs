// Decompiled with JetBrains decompiler
// Type: RemoveUAC.Program
// Assembly: RemoveUAC, Version=4.5.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 77B208E8-E0D8-4A0C-958C-E5CF190AB691
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RemoveUAC.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace RemoveUAC
{
  internal static class Program
  {
    private static MD5CryptoServiceProvider _md5;
    private static SHA1CryptoServiceProvider _sha1;
    private static string appStartupPath;
    private static string appDataHashFolder;
    private static string uacHashFolder;
    private static string regKeyPath;
    private static bool removeUacInAppStartupPath;
    private static bool quietMode;

    private static MD5CryptoServiceProvider md5
    {
      get
      {
        if (Program._md5 == null)
          Program._md5 = new MD5CryptoServiceProvider();
        return Program._md5;
      }
    }

    private static SHA1CryptoServiceProvider sha1
    {
      get
      {
        if (Program._sha1 == null)
          Program._sha1 = new SHA1CryptoServiceProvider();
        return Program._sha1;
      }
    }

    [STAThread]
    private static int Main(string[] args)
    {
      int num1 = Program.init(args);
      if (num1 != 0)
      {
        int num2 = (int) MessageBox.Show("Usage: RemoveUAC.exe [-q] [<install dir/startup path>]");
        return num1;
      }
      SelectionForm selectionForm = (SelectionForm) null;
      if (!Program.quietMode)
      {
        selectionForm = new SelectionForm(Program.uacHashFolder, Program.appDataHashFolder, Program.appStartupPath, (Program.isAllUsersInstall((string) null) ? "HKLM\\" : "HKCU\\") + Program.regKeyPath);
        if (selectionForm.ShowDialog() == DialogResult.Cancel)
          return 0;
      }
      bool flag1 = Program.quietMode || selectionForm.RemoveUAC;
      bool flag2 = Program.quietMode || selectionForm.RemoveInstallDir;
      bool rmConfigFiles = Program.quietMode || selectionForm.RemoveInstallDir && selectionForm.RemoveConfigFiles;
      bool flag3 = Program.quietMode || selectionForm.RemoveRegistry;
      bool quietMode = Program.quietMode;
      if (flag1)
        Program.removeUAC();
      if (flag2)
        Program.removeInstallDirFiles(rmConfigFiles);
      if (flag3)
        Program.removeRegistryEntries(Program.quietMode);
      if (quietMode)
      {
        Program.removeProgramsDir("Ellie Mae Encompass");
        Program.removeProgramsDir("Ellie Mae Encompass360");
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        Program.removeFile(Path.Combine(folderPath, "Encompass360.lnk"));
        Program.removeFile(Path.Combine(folderPath, "Encompass.lnk"));
      }
      return 0;
    }

    private static int init(string[] args)
    {
      int cmdArgs = Program.parseCmdArgs(args);
      Program.uacHashFolder = Program.constructUacHashFolder("Ellie Mae", Program.appStartupPath);
      Program.appDataHashFolder = Program.constructAppDataHashFolder("Ellie Mae", Program.appStartupPath);
      Program.regKeyPath = "Software\\Ellie Mae\\SmartClient\\" + Program.appStartupPath.Replace("\\", "/");
      return cmdArgs;
    }

    private static int parseCmdArgs(string[] args)
    {
      bool flag = false;
      if (args.Length == 0)
      {
        Program.appStartupPath = Application.StartupPath;
        Program.removeUacInAppStartupPath = true;
      }
      else if (args.Length == 1)
      {
        if (args[0] == "-q")
        {
          Program.quietMode = true;
          Program.appStartupPath = Application.StartupPath;
          Program.removeUacInAppStartupPath = true;
        }
        else
          Program.appStartupPath = args[0];
      }
      else if (args.Length == 2)
      {
        if (args[0] != "-q")
        {
          flag = true;
        }
        else
        {
          Program.quietMode = true;
          Program.appStartupPath = args[1];
        }
      }
      else
        flag = true;
      if (flag)
        return -1;
      if (!Path.IsPathRooted(Program.appStartupPath))
        Program.appStartupPath = Path.Combine(Environment.CurrentDirectory, Program.appStartupPath);
      while (Program.appStartupPath.EndsWith("\\"))
        Program.appStartupPath = Program.appStartupPath.Substring(0, Program.appStartupPath.Length - 1);
      return 0;
    }

    private static void removeUAC()
    {
      if (!Directory.Exists(Program.uacHashFolder))
        return;
label_1:
      try
      {
        Directory.Delete(Program.uacHashFolder, true);
      }
      catch (Exception ex)
      {
        if (MessageBox.Show("Unable to delete UAC folder:\r\n\r\n" + Program.uacHashFolder + "\r\n\r\nYou can manually delete it later. Do you want to fix the below issue and try to delete it again now?\r\n\r\n" + ex.Message, "Remove UAC", MessageBoxButtons.YesNo) != DialogResult.No)
          goto label_1;
      }
    }

    private static void removeInstallDirFiles(bool rmConfigFiles)
    {
      string[] directories = Directory.GetDirectories(Program.appStartupPath);
      string[] files1 = Directory.GetFiles(Program.appStartupPath);
      List<string> stringList = new List<string>();
      if (files1 == null)
        return;
      int num = 0;
      foreach (string path in files1)
      {
        string lower = Path.GetFileName(path).Trim().ToLower();
        if (lower == "applauncher.exe" || lower == "elliemae.encompass.asmresolver.dll")
          ++num;
        if (lower != "applauncher.exe" && lower != "applauncher.exe.config" && lower != "elliemae.encompass.asmresolver.dll" && (!Program.removeUacInAppStartupPath || lower != "removeuac.exe") && (Program.quietMode || lower != "restoreapplauncher.exe") && !lower.EndsWith(".exe.config"))
          stringList.Add(path);
      }
      if (num < 2)
        return;
      string[] array = stringList.ToArray();
      if (directories != null && directories.Length > 0 || array != null && array.Length > 0)
      {
        foreach (string path in array)
        {
label_14:
          try
          {
            File.Delete(path);
          }
          catch (Exception ex)
          {
            if (MessageBox.Show("Unable to delete file:\r\n\r\n" + path + "\r\n\r\nYou can manually delete it later. Do you want to fix the below issue and try to delete it again now?\r\n\r\n" + ex.Message, "Remove UAC", MessageBoxButtons.YesNo) != DialogResult.No)
              goto label_14;
          }
        }
        foreach (string path in directories)
        {
          if (string.Compare(Path.GetFileName(path), "ConfigBackup", true) != 0)
          {
label_20:
            try
            {
              Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
              if (MessageBox.Show("Unable to delete folder:\r\n\r\n" + path + "\r\n\r\nYou can manually delete it later. Do you want to fix the below issue and try to delete it again now?\r\n\r\n" + ex.Message, "Remove UAC", MessageBoxButtons.YesNo) != DialogResult.No)
                goto label_20;
            }
          }
        }
      }
      if (!rmConfigFiles)
        return;
      string[] files2 = Directory.GetFiles(Program.appStartupPath, "*.exe.config");
      if (files2 == null || files2.Length <= 0)
        return;
      foreach (string path in files2)
      {
        if (string.Compare(Path.GetFileName(path), "AppLauncher.exe.config", true) != 0)
        {
label_30:
          try
          {
            File.Delete(path);
          }
          catch (Exception ex)
          {
            if (MessageBox.Show("Unable to delete file '" + path + "'. You can manually delete it later.\r\nDo you want to fix the below issue and try to delete it again now?\r\n\r\n" + ex.Message, "Remove UAC", MessageBoxButtons.YesNo) != DialogResult.No)
              goto label_30;
          }
        }
      }
    }

    private static void removeRegistryEntries(bool quietMode)
    {
      RegistryKey registryKey = Program.getRegistryHive((string) null).OpenSubKey(Program.regKeyPath);
      if (registryKey == null)
        return;
      string str1 = (string) registryKey.GetValue("AuthServerURL");
      string str2 = (string) registryKey.GetValue("AuthServerURLs");
      string str3 = (string) registryKey.GetValue("SmartClientIDs");
      registryKey.Close();
label_2:
      try
      {
        Program.getRegistryHive((string) null).DeleteSubKeyTree(Program.regKeyPath);
        try
        {
          if (!quietMode)
          {
            if (str1 != null)
              Program.getRegistryHive((string) null).CreateSubKey(Program.regKeyPath).SetValue("AuthServerURL", (object) str1);
          }
        }
        catch
        {
        }
        try
        {
          if (str2 != null)
            Program.getRegistryHive((string) null).CreateSubKey(Program.regKeyPath).SetValue("AuthServerURLs", (object) str2);
        }
        catch
        {
        }
        try
        {
          if (str3 == null)
            return;
          Program.getRegistryHive((string) null).CreateSubKey(Program.regKeyPath).SetValue("SmartClientIDs", (object) str3);
        }
        catch
        {
        }
      }
      catch (Exception ex)
      {
        if (MessageBox.Show("Unable to delete registry key '" + Program.regKeyPath + "'. You can manually delete it later.\r\nDo you want to fix the below issue and try to delete it again now?\r\n\r\n" + ex.Message, "Remove UAC", MessageBoxButtons.YesNo) != DialogResult.No)
          goto label_2;
      }
    }

    private static void removeProgramsDir(string folderName)
    {
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), folderName);
      if (!Directory.Exists(path))
        return;
label_1:
      try
      {
        Directory.Delete(path, true);
      }
      catch (Exception ex)
      {
        if (MessageBox.Show("Unable to delete folder:\r\n\r\n" + path + "\r\n\r\nYou can manually delete it later. Do you want to fix the below issue and try to delete it again now?\r\n\r\n" + ex.Message, "Remove UAC", MessageBoxButtons.YesNo) != DialogResult.No)
          goto label_1;
      }
    }

    private static void removeFile(string filePath)
    {
      if (!File.Exists(filePath))
        return;
label_1:
      try
      {
        File.Delete(filePath);
      }
      catch (Exception ex)
      {
        if (MessageBox.Show("Unable to delete file:\r\n\r\n" + filePath + "\r\n\r\nYou can manually delete it later. Do you want to fix the below issue and try to delete it again now?\r\n\r\n" + ex.Message, "Remove UAC", MessageBoxButtons.YesNo) != DialogResult.No)
          goto label_1;
      }
    }

    private static string constructAppDataHashFolder(string appCompanyName, string appStartupPath)
    {
      return Path.Combine(Path.GetDirectoryName(Program.getUacFolder(appCompanyName, true)), appCompanyName + "\\" + Program.computeHashB64FilePath(Program.HashAlgorithm.SHA1, appStartupPath));
    }

    private static string constructUacHashFolder(string appCompanyName, string appStartupPath)
    {
      return Path.Combine(Program.getUacFolder(appCompanyName, false), appCompanyName + "\\" + Program.computeHashB64FilePath(Program.HashAlgorithm.SHA1, appStartupPath));
    }

    private static string getUacFolder(string appCompanyName, bool skipHKLMRegistry)
    {
      string path1 = (string) null;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient", false))
      {
        if (registryKey != null)
          path1 = (string) registryKey.GetValue("UACFolder");
      }
      if (Program.isAllUsersInstall(appCompanyName) && !skipHKLMRegistry && (path1 ?? "").Trim() == "")
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient", false))
        {
          if (registryKey != null)
            path1 = (string) registryKey.GetValue("UACFolder");
          if (path1 == null)
            throw new Exception("Registry entry UACFolder not set");
        }
      }
      if ((path1 ?? "").Trim() == "")
      {
        string str = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (str.ToLower().EndsWith("application data"))
          str = Path.GetDirectoryName(str);
        else if (str.ToLower().EndsWith("appdata\\local"))
          str += "Low";
        path1 = Path.Combine(str, "Apps\\UAC");
      }
      while (path1.EndsWith("\\"))
        path1 = path1.Substring(0, path1.Length - 1);
      if (!path1.ToUpper().EndsWith("UAC"))
        path1 = Path.Combine(path1, "UAC");
      return path1;
    }

    private static string computeHashB64FilePath(Program.HashAlgorithm algorithm, string pathString)
    {
      byte[] bytes = Encoding.Default.GetBytes(pathString.Trim().ToLower());
      return Program.computeHashB64(algorithm, bytes).Replace("/", "#");
    }

    private static byte[] computeHash(Program.HashAlgorithm algorithm, byte[] buffer)
    {
      if (algorithm == Program.HashAlgorithm.MD5)
        return Program.md5.ComputeHash(buffer);
      return algorithm == Program.HashAlgorithm.SHA1 ? Program.sha1.ComputeHash(buffer) : (byte[]) null;
    }

    private static string computeHashB64(Program.HashAlgorithm algorithm, byte[] buffer)
    {
      return Convert.ToBase64String(Program.computeHash(algorithm, buffer));
    }

    public static bool isAllUsersInstall(string appCompanyName)
    {
      if (appCompanyName == null)
        appCompanyName = "Ellie Mae";
      bool flag = false;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient"))
      {
        if (registryKey != null)
        {
          if (((string) registryKey.GetValue("AllUsersInstall") ?? "").Trim() == "1")
            flag = true;
        }
      }
      return flag;
    }

    private static RegistryKey getRegistryHive(string appCompanyName)
    {
      if (appCompanyName == null)
        appCompanyName = "Ellie Mae";
      return Program.isAllUsersInstall(appCompanyName) ? Registry.LocalMachine : Registry.CurrentUser;
    }

    private enum HashAlgorithm
    {
      MD5,
      SHA1,
    }
  }
}
