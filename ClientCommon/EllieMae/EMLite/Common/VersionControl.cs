// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.VersionControl
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class VersionControl : MarshalByRefObject
  {
    private const string className = "VersionControl";
    private static VersionDownloadHistory versionHistory = new VersionDownloadHistory(SystemSettings.UpdatesDir);

    public static VersionInformation CurrentVersion => VersionInformation.CurrentVersion;

    public static VersionDownloadHistory DownloadHistory => VersionControl.versionHistory;

    public static bool IsHotfixAvailable() => VersionControl.GetAvailableHotfixes().Length != 0;

    public static bool ApplyAvailableHotfixes(string appToRestart)
    {
      try
      {
        if (EnConfigurationSettings.GlobalSettings.HotfixOverrideOption == HotfixOverrideOption.NeverApply)
          return false;
        HotfixInfo[] availableHotfixes = VersionControl.GetAvailableHotfixes();
        if (availableHotfixes.Length == 0)
          return false;
        HotfixInfo hotfixInfo = (HotfixInfo) null;
        if (EnConfigurationSettings.GlobalSettings.HotfixOverrideOption == HotfixOverrideOption.AlwaysApply)
        {
          hotfixInfo = availableHotfixes[availableHotfixes.Length - 1];
        }
        else
        {
          if (!Session.IsConnected)
            return false;
          if (Session.StartupInfo.AuthorizedVersion == null)
          {
            hotfixInfo = availableHotfixes[availableHotfixes.Length - 1];
          }
          else
          {
            for (int index = availableHotfixes.Length - 1; index >= 0; --index)
            {
              if (availableHotfixes[index].Version.CompareTo((object) Session.StartupInfo.AuthorizedVersion) <= 0)
              {
                hotfixInfo = availableHotfixes[index];
                break;
              }
            }
          }
        }
        if (hotfixInfo == null)
          return false;
        VersionControl.StartHotfixInstallation(hotfixInfo.Version.HotfixSequenceNumber, appToRestart);
        return true;
      }
      catch (Exception ex)
      {
        ErrorDialog.Display("An error occurred while attempting to install the latest hotfixes. Another attempt will be made when you restart this application.", ex);
      }
      return false;
    }

    public static bool ApplyAvailableHotfixes()
    {
      return VersionControl.ApplyAvailableHotfixes(Application.ExecutablePath);
    }

    public static HotfixInfo[] GetAvailableHotfixes()
    {
      FileInfo[] files = new DirectoryInfo(VersionControl.HotfixPath).GetFiles(VersionControl.HotfixFileMask);
      HotfixInfo[] appliedHotfixes = VersionControl.CurrentVersion.GetAppliedHotfixes();
      ArrayList arrayList = new ArrayList();
      foreach (FileInfo fileInfo in files)
      {
        HotfixInfo hotfixInfo = new HotfixInfo(fileInfo);
        if (~Array.BinarySearch<HotfixInfo>(appliedHotfixes, hotfixInfo) == appliedHotfixes.Length)
          arrayList.Add((object) hotfixInfo);
      }
      arrayList.Sort();
      return (HotfixInfo[]) arrayList.ToArray(typeof (HotfixInfo));
    }

    public static void StartHotfixInstallation(int maxSequenceNumber, string restartExecutable)
    {
      VersionControl.ApplyVersionControlPatch();
      restartExecutable = AppSecurity.EncodeCommand(restartExecutable);
      string cmd = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "VersionControl.exe");
      string args = "hotfixn " + (object) maxSequenceNumber + ((restartExecutable ?? "") != "" ? (object) (" \"" + restartExecutable + "\"") : (object) "");
      if (Session.IsConnected)
      {
        args = args + " -u " + Session.UserID + " -p \"" + Session.Password.Replace("\"", "\"\"") + "\"";
        if ((Session.RemoteServer ?? "") != "")
          args = args + " -s \"" + Session.RemoteServer + "\"";
      }
      SystemUtil.ExecSystemCmd(cmd, args);
    }

    private static string HotfixPath => SystemUtil.NormalizePath(SystemSettings.UpdatesDir);

    private static string HotfixFileMask
    {
      get => "*" + VersionControl.CurrentVersion.Version.NormalizedVersion + ".*.emzip*";
    }

    public static void ApplyVersionControlPatch()
    {
      string str1 = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "VersionControl.patch");
      string str2 = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "VersionControl.exe");
      if (!File.Exists(str1))
        return;
      if (File.Exists(str2))
      {
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(str2);
        string str3 = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "VersionControl." + versionInfo.FileVersion + ".exe");
        int num = 1;
        string programDirectory;
        string path2;
        for (; File.Exists(str3); str3 = Path.Combine(programDirectory, path2))
        {
          programDirectory = EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory;
          path2 = "VersionControl." + versionInfo.FileVersion + "-" + (object) ++num + ".exe";
        }
        try
        {
          File.Move(str2, str3);
        }
        catch (Exception ex)
        {
          throw new Exception("Could not create VersionControl backup file '" + str3 + "': " + ex.Message, ex);
        }
      }
      try
      {
        File.Move(str1, str2);
      }
      catch (Exception ex)
      {
        throw new Exception("Could not apply VersionControl patch: " + ex.Message, ex);
      }
    }

    public static bool QueryInstallVersionUpdate(IVersionControl remoteVersionControl)
    {
      return VersionControl.InstallVersionUpdate(remoteVersionControl) == VersionCompatibilityResult.IncompatibleUpdateStarted;
    }

    public static VersionCompatibilityResult InstallVersionUpdate(
      IVersionControl remoteVersionControl)
    {
      string swVersionControl = Tracing.SwVersionControl;
      try
      {
        Tracing.Log(swVersionControl, TraceLevel.Info, nameof (VersionControl), "Checking version compatibility with Encompass server.");
        JedVersion version = VersionControl.CurrentVersion.Version;
        if (remoteVersionControl.IsVersionUpdateAvailable(version))
        {
          Tracing.Log(swVersionControl, TraceLevel.Info, nameof (VersionControl), "New software version available for download from remote server.");
          if (new VersionUpdateForm(remoteVersionControl).StartUpdate() == VersionUpdateResult.UpdateStarted)
            return VersionCompatibilityResult.IncompatibleUpdateStarted;
          if (!remoteVersionControl.IsCompatibleWithVersion(version))
            return VersionCompatibilityResult.IncompatibleUpdateFailed;
        }
        else if (!remoteVersionControl.IsCompatibleWithVersion(version))
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "This Encompass client version is not compatible with the specified Encompass server. Please contact your system administrator to resolve this problem.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          Tracing.Log(swVersionControl, TraceLevel.Info, nameof (VersionControl), "Current version (" + version.ToString() + ") is not compatible with server. Exiting...");
          return VersionCompatibilityResult.IncompatibleNoUpdateAvailable;
        }
        Tracing.Log(swVersionControl, TraceLevel.Info, nameof (VersionControl), "Client is version compatible with server. Proceeding with login.");
        return VersionCompatibilityResult.VersionsAreCompatible;
      }
      catch (Exception ex)
      {
        Tracing.Log(swVersionControl, TraceLevel.Warning, nameof (VersionControl), "An unexpected error occurred while detecting version compatibility between the Encompass client and server.  Probably the Encompass Server is not up and running.");
        if (File.Exists(SystemSettings.ServerFile))
        {
          string str = "  Ask your system administrator if the Encompass server is up and running.  It may be that the server name/address is incorrect.  Do you want to correct the server name/address next time when you log in?";
          if (Utils.Dialog((IWin32Window) null, ex.Message + str, MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
            File.Delete(SystemSettings.ServerFile);
        }
        else
        {
          string str = ".  Ask your system administrator if the Encompass server is up and running and also make sure the server name/address you typed in is correct.";
          int num = (int) Utils.Dialog((IWin32Window) null, ex.Message + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return VersionCompatibilityResult.CompatibilityCheckFailed;
      }
    }
  }
}
