// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Win64
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.Encompass.AsmResolver;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class Win64
  {
    public const string EncompassRegistryRoot = "HKEY_LOCAL_MACHINE\\Software\\Ellie Mae\\Encompass";
    public const string SmartClientRegistryRoot = "HKEY_LOCAL_MACHINE\\Software\\Ellie Mae\\SmartClient";

    public static bool Is64BitOS()
    {
      try
      {
        return Directory.Exists(Win64.get32BitSystemRoot());
      }
      catch
      {
        return false;
      }
    }

    public static void SyncEncompass32To64()
    {
      try
      {
        if (!Win64.Is64BitOS())
          return;
        Win64.CopyRegistryTree32to64("HKEY_LOCAL_MACHINE\\Software\\Ellie Mae\\Encompass");
        if (!AssemblyResolver.IsSmartClient)
          return;
        Win64.CopyRegistryTree32to64("HKEY_LOCAL_MACHINE\\Software\\Ellie Mae\\SmartClient");
      }
      catch
      {
      }
    }

    public static void SyncEncompass64To32()
    {
      try
      {
        if (!Win64.Is64BitOS())
          return;
        Win64.CopyRegistryTree64to32("HKEY_LOCAL_MACHINE\\Software\\Ellie Mae\\Encompass");
      }
      catch
      {
      }
    }

    private static string get32BitSystemRoot()
    {
      return Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "sysWOW64");
    }

    private static string get64BitSystemRoot()
    {
      return Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "system32");
    }

    public static bool CopyRegistryTree32to64(string rootPath)
    {
      string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".reg");
      try
      {
        Process process = Process.Start(Path.Combine(Win64.get32BitSystemRoot(), "regedit.exe"), "/e \"" + path + "\" \"" + rootPath + "\"");
        process.WaitForExit();
        if (process.ExitCode != 0)
          throw new Exception("regedt32 returned non-zero result (" + (object) process.ExitCode + ")");
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to export registry tree '" + rootPath + "': " + ex.Message);
      }
      if (!File.Exists(path))
        return false;
      try
      {
        Process process = Process.Start(Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "regedit.exe"), "/s /i \"" + path + "\"");
        process.WaitForExit();
        if (process.ExitCode != 0)
          throw new Exception("regedt32 returned non-zero result (" + (object) process.ExitCode + ")");
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to import registry tree '" + rootPath + "': " + ex.Message);
      }
      File.Delete(path);
      return true;
    }

    public static bool CopyRegistryTree64to32(string rootPath)
    {
      string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".reg");
      try
      {
        Process process = Process.Start(Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "regedit.exe"), "/e \"" + path + "\" \"" + rootPath + "\"");
        process.WaitForExit();
        if (process.ExitCode != 0)
          throw new Exception("regedt32 returned non-zero result (" + (object) process.ExitCode + ")");
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to export registry tree '" + rootPath + "': " + ex.Message);
      }
      if (!File.Exists(path))
        return false;
      try
      {
        Process process = Process.Start(Path.Combine(Win64.get32BitSystemRoot(), "regedit.exe"), "/s /i \"" + path + "\"");
        process.WaitForExit();
        if (process.ExitCode != 0)
          throw new Exception("regedt32 returned non-zero result (" + (object) process.ExitCode + ")");
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to import registry tree '" + rootPath + "': " + ex.Message);
      }
      File.Delete(path);
      return true;
    }
  }
}
