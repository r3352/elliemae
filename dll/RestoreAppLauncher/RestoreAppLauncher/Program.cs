// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.Program
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace RestoreAppLauncher
{
  public static class Program
  {
    private static bool useCurrentDirAsInstallDir = false;
    private static string installDir = SCUtil.GetInstallDirFromUacFolder();
    private static string uacHashFolder;
    private static List<string> encFilenames = new List<string>();
    private static List<string> scAppMgrFilenames = new List<string>();
    private static bool ui = false;
    private static bool excludeUpdtr = false;
    private static bool skipUAC = true;
    private static bool deleteFirst = true;
    private static bool dontElevate = false;
    private static CheckItems checkItem = CheckItems.None;

    [STAThread]
    public static int Main(string[] args)
    {
      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(BasicUtils.CurrentDomain_AssemblyResolve);
      Program.parseArgs(args);
      if (Program.encFilenames.Count == 0 && Program.scAppMgrFilenames.Count == 0)
      {
        if (Program.excludeUpdtr)
          Program.encFilenames.AddRange((IEnumerable<string>) Consts.EncNoUpdtrFiles);
        else
          Program.encFilenames.AddRange((IEnumerable<string>) Consts.EncFiles);
      }
      if (Program.ui)
      {
        List<string> stringList = new List<string>();
        stringList.AddRange((IEnumerable<string>) Program.encFilenames.ToArray());
        stringList.AddRange((IEnumerable<string>) Program.scAppMgrFilenames.ToArray());
        FileSelectionForm fileSelectionForm = new FileSelectionForm(stringList.ToArray(), Program.useCurrentDirAsInstallDir);
        if (fileSelectionForm.ShowDialog() != DialogResult.OK)
          return -2;
        Program.encFilenames.Clear();
        Program.encFilenames.AddRange((IEnumerable<string>) fileSelectionForm.SelectedEncFiles);
        Program.scAppMgrFilenames.Clear();
        Program.scAppMgrFilenames.AddRange((IEnumerable<string>) fileSelectionForm.SelectedSCAppFiles);
        if (Program.encFilenames.Count == 0 && Program.scAppMgrFilenames.Count == 0)
          return 0;
        Program.installDir = !fileSelectionForm.UseCurrentDirAsInstallDir ? SCUtil.GetInstallDirFromUacFolder() : Application.StartupPath;
      }
      if (!Directory.Exists(Program.installDir))
      {
        int num = (int) MessageBox.Show("Could not find a part of the path '" + Program.installDir + "'.", Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      StartupObject.InitInstance(Program.installDir);
      Program.uacHashFolder = UACUtil.ConstructUacHashFolder(Program.installDir);
      if (BasicUtils.IsAdministrator())
        Program.dontElevate = true;
      if (!Program.dontElevate)
      {
        foreach (string scAppMgrFilename in Program.scAppMgrFilenames)
        {
          if (scAppMgrFilename.IndexOf("SCAppMgr", StringComparison.OrdinalIgnoreCase) == 0)
          {
            BasicUtils.Elevate(args);
            return 0;
          }
        }
      }
      bool flag = true;
      string[] filenames1 = Program.restoreEncFiles(Program.encFilenames.ToArray(), false);
      if (filenames1.Length > 0 && Program.restoreEncFiles(filenames1, true).Length > 0)
        flag = false;
      string[] filenames2 = Program.restoreSCAppMgrFiles(Program.scAppMgrFilenames.ToArray(), false);
      if (filenames2.Length > 0 && Program.restoreSCAppMgrFiles(filenames2, true).Length > 0)
        flag = false;
      if (flag)
      {
        int num1 = (int) MessageBox.Show("Encompass SmartClient successfully updated files.", Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      return !flag ? -1 : 0;
    }

    private static void parseArgs(string[] args)
    {
      if (args == null || args.Length == 0)
        return;
      for (int index = 0; index < args.Length; ++index)
      {
        if (string.Compare("-ui", args[index], true) == 0)
          Program.ui = true;
        else if (string.Compare("-ExcludeUpdtr", args[index], true) == 0)
          Program.excludeUpdtr = true;
        else if (string.Compare("-SkipUAC", args[index], true) == 0)
          Program.skipUAC = true;
        else if (string.Compare("-UAC", args[index], true) == 0)
          Program.skipUAC = false;
        else if (string.Compare("-DeleteFirst", args[index], true) == 0)
          Program.deleteFirst = true;
        else if (string.Compare("-DontDelete", args[index], true) == 0)
          Program.deleteFirst = false;
        else if (string.Compare("-DontElevate", args[index], true) == 0)
          Program.dontElevate = true;
        else if (string.Compare("-CheckItem", args[index], true) == 0)
        {
          ++index;
          if (string.Compare(args[index], "BOTH", true) == 0)
            Program.checkItem = CheckItems.Both;
          else if (string.Compare(args[index], "AE", true) == 0)
            Program.checkItem = CheckItems.AE;
          else if (string.Compare(args[index], "EX", true) == 0)
            Program.checkItem = CheckItems.EX;
          else if (string.Compare(args[index], "NONE", true) == 0)
          {
            Program.checkItem = CheckItems.None;
          }
          else
          {
            int num = (int) MessageBox.Show(args[index - 1] + " " + args[index] + ": unrecognized command-line argument.", Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        else if (string.Compare("-CWD", args[index], true) == 0)
        {
          Program.useCurrentDirAsInstallDir = true;
          Program.installDir = Application.StartupPath;
        }
        else if (args[index].StartsWith("-"))
        {
          int num1 = (int) MessageBox.Show(args[index] + ": unrecognized command-line argument.", Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (Consts.EncFileMapping.ContainsKey(args[index]))
          Program.encFilenames.Add(Consts.EncFileMapping[args[index]]);
        else if (Consts.SCAppMgrFileMapping.ContainsKey(args[index]))
          Program.scAppMgrFilenames.Add(Consts.SCAppMgrFileMapping[args[index]]);
        else
          Program.encFilenames.Add(args[index]);
      }
    }

    private static string[] restoreSCAppMgrFiles(string[] filenames, bool retry)
    {
      List<string> stringList = new List<string>();
      bool flag = false;
      try
      {
        foreach (string filename in filenames)
        {
          if (string.Compare("SCAppMgr.exe", filename, true) == 0)
          {
            flag = true;
            BasicUtils.StopService("SCAppMgr");
          }
          CopyWRetryResult copyWretryResult = Program.handleSCAppMgrFiles(filename, retry);
          if (copyWretryResult == CopyWRetryResult.Failure || retry && copyWretryResult == CopyWRetryResult.Abort)
            stringList.Add(filename);
        }
      }
      finally
      {
        if (flag)
          BasicUtils.StartService("SCAppMgr");
      }
      return stringList.ToArray();
    }

    private static CopyWRetryResult handleSCAppMgrFiles(string filename, bool retry)
    {
      try
      {
        byte[] file = WebUtil.GetFile("https://hosted.elliemae.com/EncompassSCFiles/" + filename + Consts.DeployFileExt, 0L, 0L, "Downloading " + filename + ". Please wait...");
        string str1 = Path.Combine(SCUtil.SCAppMgrFolder, filename);
        if (string.Compare("SCAppMgr.exe", filename, true) == 0)
        {
          try
          {
            string str2 = str1 + DateTime.Now.ToString(".yyyy-MM-dd");
            if (System.IO.File.Exists(str2))
              System.IO.File.Delete(str2);
            System.IO.File.Move(str1, str2);
          }
          catch
          {
            Thread.Sleep(1000);
          }
        }
        return Program.copyWRetry((string) null, str1, file, retry);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Unable to download and update file '" + filename + "': " + ex.Message + "\r\n\r\nPlease wait for a couple of minutes and try again.", Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return CopyWRetryResult.Failure;
    }

    private static string[] restoreEncFiles(string[] filenames, bool retry)
    {
      List<string> stringList = new List<string>();
      foreach (string filename in filenames)
      {
        CopyWRetryResult copyWretryResult = Program.restoreFile(filename, retry);
        if (copyWretryResult == CopyWRetryResult.Failure || retry && copyWretryResult == CopyWRetryResult.Abort)
          stringList.Add(filename);
      }
      return stringList.ToArray();
    }

    private static CopyWRetryResult restoreFile(string filename, bool retry)
    {
      CopyWRetryResult copyWretryResult = CopyWRetryResult.Failure;
      if (!Program.skipUAC)
        copyWretryResult = Program.copyFromUAC(filename, retry);
      if (copyWretryResult == CopyWRetryResult.Failure)
        copyWretryResult = Program.downloadAndOverwrite(filename, retry);
      return copyWretryResult;
    }

    private static CopyWRetryResult copyFromUAC(string filename, bool retry)
    {
      string path2 = "Encompass360";
      if (filename.IndexOf("AppUpdtr.exe", StringComparison.OrdinalIgnoreCase) == 0)
        path2 = "AppUpdtr";
      string str = Path.Combine(Program.uacHashFolder, path2, filename);
      if (System.IO.File.Exists(str))
      {
        try
        {
          return Program.copyWRetry(str, Path.Combine(Program.installDir, filename), (byte[]) null, retry);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Unable to copy file from the UAC folder '" + str + "': " + ex.Message, Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      return CopyWRetryResult.Failure;
    }

    private static CopyWRetryResult downloadAndOverwrite(string filename, bool retry)
    {
      try
      {
        byte[] sourceBytes = FileCompressor.UnzipBuffer(WebUtil.GetFile(SCUtil.GetDownloadFolderUrl(filename) + "/" + filename + Consts.DeployZipExt, 0L, 0L, "Downloading " + filename + ". Please wait..."));
        return Program.copyWRetry((string) null, Path.Combine(Program.installDir, filename), sourceBytes, retry);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Unable to download and update file '" + filename + "': " + ex.Message + "\r\n\r\nPlease wait for a couple of minutes and try again.", Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return CopyWRetryResult.Failure;
    }

    private static CopyWRetryResult copyWRetry(
      string sourceFilePath,
      string destFilePath,
      byte[] sourceBytes,
      bool retry)
    {
      try
      {
        Program.copyOrOverwirte(sourceFilePath, destFilePath, sourceBytes, Program.deleteFirst);
        return CopyWRetryResult.Success;
      }
      catch (Exception ex1)
      {
        Exception exception = ex1;
        DialogResult dialogResult = DialogResult.Yes;
        int num1 = -1;
        if (retry && Program.toRetry(ex1))
        {
          bool flag = true;
          while (dialogResult == DialogResult.Yes)
          {
            string fileName = Path.GetFileName(destFilePath);
            if (flag)
            {
              int num2 = (int) new AppExperienceForm(fileName).ShowDialog();
              flag = false;
            }
            using (new PgBarThreadStart(fileName, 0, 50, 0, 1, 3000, true))
            {
              for (num1 = 12; num1 >= 0; --num1)
              {
                try
                {
                  Program.copyOrOverwirte(sourceFilePath, destFilePath, sourceBytes, Program.deleteFirst);
                  dialogResult = DialogResult.No;
                  break;
                }
                catch (Exception ex2)
                {
                  exception = ex2;
                  if (num1 > 0)
                    Thread.Sleep(15000);
                }
              }
            }
            if (num1 < 0)
              dialogResult = MessageBox.Show("Updating " + fileName + " failed. Do you want to try again?\r\n\r\n" + exception.Message, Consts.EncompassSmartClient, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
          }
        }
        if (num1 >= 0)
          return CopyWRetryResult.Success;
        return retry && dialogResult == DialogResult.No ? CopyWRetryResult.Abort : CopyWRetryResult.Failure;
      }
    }

    private static void copyOrOverwirte(
      string sourceFilePath,
      string destFilePath,
      byte[] sourceBytes,
      bool deleteFirst)
    {
      if (deleteFirst && System.IO.File.Exists(destFilePath))
        System.IO.File.Delete(destFilePath);
      if (sourceBytes != null)
        System.IO.File.WriteAllBytes(destFilePath, sourceBytes);
      else
        System.IO.File.Copy(sourceFilePath, destFilePath, true);
    }

    private static bool toRetry(Exception ex)
    {
      if (Program.checkItem == CheckItems.None || (Program.checkItem == CheckItems.AE || Program.checkItem == CheckItems.Both) && BasicUtils.IsAeLookupSvcDisabled())
        return true;
      if (Program.checkItem == CheckItems.EX || Program.checkItem == CheckItems.Both)
      {
        switch (ex)
        {
          case AccessViolationException _:
          case UnauthorizedAccessException _:
          case IOException _:
            return true;
        }
      }
      return false;
    }
  }
}
