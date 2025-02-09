// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.SystemUtils
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class SystemUtils
  {
    public static Process ExecSystemCmd(string fileName, string[] args)
    {
      if (args == null || args.Length == 0)
        return SystemUtils.ExecSystemCmd(fileName, "");
      for (int index = 0; index < args.Length; ++index)
      {
        if (args[index].IndexOf(' ') >= 0 || args[index].IndexOf('\t') >= 0 || args[index] == "")
          args[index] = "\"" + args[index] + "\"";
      }
      return SystemUtils.ExecSystemCmd(fileName, string.Join(" ", args));
    }

    public static Process ExecSystemCmd(string cmd, string args)
    {
      Process process = new Process();
      process.StartInfo.FileName = cmd;
      if (!BasicUtils.IsNullOrEmpty(args))
        process.StartInfo.Arguments = args;
      process.Start();
      return process;
    }

    public static Process ExecSystemCmd(string commandLine)
    {
      Process process = new Process();
      process.StartInfo = new ProcessStartInfo(commandLine);
      process.Start();
      return process;
    }

    public static string MutexFileReadAllText(string filePath)
    {
      using (new FileMutex(filePath))
        return File.ReadAllText(filePath);
    }

    public static byte[] MutexFileReadAllBytes(string filePath)
    {
      using (new FileMutex(filePath))
        return File.ReadAllBytes(filePath);
    }

    public static void MutexFileWrite(string filePath, byte[] buf)
    {
      using (new FileMutex(filePath))
      {
        if (File.Exists(filePath))
          File.Delete(filePath);
        File.WriteAllBytes(filePath, buf);
      }
    }

    public static void MutexFileWrite(string filePath, string text)
    {
      SystemUtils.MutexFileWrite(filePath, Encoding.ASCII.GetBytes(text));
    }

    public static void MutexFileCopy(string sourceFileName, string destFileName, bool overwrite)
    {
      string directoryName = Path.GetDirectoryName(destFileName);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      using (new FileMutex(destFileName))
      {
        try
        {
          File.Copy(sourceFileName, destFileName, overwrite);
        }
        catch (Exception ex)
        {
          bool flag = false;
          if (string.Compare(Path.GetExtension(destFileName), ".exe", true) == 0 && string.Compare(Path.GetFileName(destFileName), ResolverConsts.RestoreAppLauncherExe, true) != 0)
            flag = SystemUtils.RunRestoreAppLauncher(Path.GetFileName(destFileName));
          if (!flag)
            throw ex;
        }
      }
    }

    public static bool RunRestoreAppLauncher(string filename)
    {
      string path = Path.Combine(Application.StartupPath, ResolverConsts.RestoreAppLauncherExe);
      if (!File.Exists(path))
        return false;
      Process process = new Process();
      process.StartInfo.FileName = path;
      process.StartInfo.Arguments = "\"" + filename + "\"";
      process.Start();
      process.WaitForExit();
      return process.ExitCode == 0;
    }

    public static void MutexFileMove(string sourceFileName, string destFileName, bool overwrite)
    {
      string directoryName = Path.GetDirectoryName(destFileName);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      using (new FileMutex(destFileName))
      {
        if (overwrite && File.Exists(destFileName))
          File.Delete(destFileName);
        File.Move(sourceFileName, destFileName);
      }
    }

    public static void MutexFileDelete(string filePath)
    {
      using (new FileMutex(filePath))
      {
        try
        {
          if (!File.Exists(filePath))
            return;
          File.Delete(filePath);
        }
        catch (Exception ex)
        {
          AssemblyResolver.Instance.WriteToEventLog(ex.Message, EventLogEntryType.Warning);
        }
      }
    }
  }
}
