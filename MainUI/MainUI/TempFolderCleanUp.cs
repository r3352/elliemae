// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.TempFolderCleanUp
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.VersionInterface15;
using EncSCAppMgr;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class TempFolderCleanUp
  {
    private static readonly string className = nameof (TempFolderCleanUp);
    private static readonly string sw = Tracing.SwCommon;
    private static readonly string scAppMgrIpcUrl = "ipc://SCAppMgr/EncSCAppMgrRO.rem";

    public static void RegisterTempFolder(string encIpcPortName)
    {
      try
      {
        string str1 = SystemSettings.TempFolderRoot.TrimEnd('\\');
        if (string.IsNullOrWhiteSpace(str1))
          return;
        if (Guid.TryParse(Path.GetFileName(str1), out Guid _))
        {
          try
          {
            if (!Directory.Exists(str1))
              Directory.CreateDirectory(str1);
            Process currentProcess = Process.GetCurrentProcess();
            string str2 = JedVersion.Encompass ?? "";
            File.WriteAllLines(Path.Combine(str1, "Encompass.pid"), new string[4]
            {
              string.Concat((object) currentProcess.Id),
              string.Concat((object) currentProcess.StartTime.Ticks),
              encIpcPortName ?? "",
              str2
            });
            ((EncSCAppMgrRO) Activator.GetObject(typeof (EncSCAppMgrRO), TempFolderCleanUp.scAppMgrIpcUrl)).AddTempFolderForLaterCleanUp(currentProcess.Id, currentProcess.StartTime.Ticks, encIpcPortName ?? "", str1);
          }
          catch (Exception ex)
          {
            Tracing.Log(TempFolderCleanUp.sw, TraceLevel.Info, TempFolderCleanUp.className, str1 + ": error writing temp folder or adding it to SCAppMgr clean up list: " + ex.Message);
          }
        }
        else
          Tracing.Log(TempFolderCleanUp.sw, TraceLevel.Info, TempFolderCleanUp.className, "Last part of the temp folder not a GUID: " + str1);
      }
      catch (Exception ex)
      {
        Tracing.Log(TempFolderCleanUp.sw, TraceLevel.Info, TempFolderCleanUp.className, "Error registering temp folder: " + ex.Message);
      }
    }
  }
}
