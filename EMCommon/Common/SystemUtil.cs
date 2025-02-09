// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SystemUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceProcess;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class SystemUtil
  {
    public const string InvalidFilenameChars = "\\/:*?\"<>|";
    public const string InvalidFilenameCharsDisplayStr = "\\ / : * ? \" < > |";
    public static readonly char[] InvalidFilenameCharArray = "\\/:*?\"<>|".ToCharArray();
    private static string hostName = (string) null;
    private static IPAddress[] hostIPAddressList = (IPAddress[]) null;
    private static string[] macAddresses = (string[]) null;
    private const byte VER_NT_WORKSTATION = 1;

    [DllImport("kernel32")]
    private static extern void GlobalMemoryStatus(ref SystemUtil.MEMORY_INFO meminfo);

    [DllImport("kernel32.dll")]
    private static extern bool GlobalMemoryStatusEx(ref SystemUtil.MEMORYSTATUSEX lpBuffer);

    [DllImport("kernel32")]
    private static extern bool GetVersionEx(ref SystemUtil.OSVERSIONINFOEX osvi);

    public static string HostName
    {
      get
      {
        if (SystemUtil.hostName == null)
          SystemUtil.hostName = Dns.GetHostName();
        return SystemUtil.hostName;
      }
    }

    public static IPAddress[] HostIPAddressList
    {
      get
      {
        if (SystemUtil.hostIPAddressList == null)
          SystemUtil.hostIPAddressList = Dns.GetHostEntry(SystemUtil.HostName).AddressList;
        return SystemUtil.hostIPAddressList;
      }
    }

    public static string[] MacAddresses
    {
      get
      {
        if (SystemUtil.macAddresses == null)
        {
          try
          {
            ManagementObjectCollection objectCollection = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration").Get();
            ArrayList arrayList = new ArrayList();
            foreach (ManagementObject managementObject in objectCollection)
            {
              if (managementObject["MacAddress"] != null)
                arrayList.Add((object) managementObject["MacAddress"].ToString());
            }
            SystemUtil.macAddresses = (string[]) arrayList.ToArray(typeof (string));
          }
          catch (Exception ex)
          {
            throw new Exception("Error reading MAC Address information: " + ex.Message, ex);
          }
        }
        return SystemUtil.macAddresses;
      }
    }

    public static OSVersion OS
    {
      get
      {
        OperatingSystem osVersion = Environment.OSVersion;
        SystemUtil.OSVERSIONINFOEX osvi = new SystemUtil.OSVERSIONINFOEX();
        osvi.dwOSVersionInfoSize = Marshal.SizeOf<SystemUtil.OSVERSIONINFOEX>(osvi);
        SystemUtil.GetVersionEx(ref osvi);
        switch (osVersion.Platform)
        {
          case PlatformID.Win32Windows:
            switch (osVersion.Version.Minor)
            {
              case 0:
                return OSVersion.Windows95;
              case 10:
                return osVersion.Version.Revision.ToString() == "2222A" ? OSVersion.Windows98SE : OSVersion.Windows98;
              case 90:
                return OSVersion.WindowsMe;
            }
            break;
          case PlatformID.Win32NT:
            switch (osVersion.Version.Major)
            {
              case 3:
                return OSVersion.WindowsNT351;
              case 4:
                return OSVersion.WindowsNT4;
              case 5:
                if (osVersion.Version.Minor == 0)
                  return OSVersion.Windows2000;
                return osVersion.Version.Minor == 1 ? OSVersion.WindowsXP : OSVersion.Windows2003;
              case 6:
                if (osVersion.Version.Minor == 0 && osvi.wProductType == (byte) 1)
                  return OSVersion.WindowsVista;
                if (osVersion.Version.Minor == 0)
                  return OSVersion.Windows2008;
                if (osVersion.Version.Minor == 1 && osvi.wProductType == (byte) 1)
                  return OSVersion.Windows7;
                return osVersion.Version.Minor == 1 ? OSVersion.Windows2008R2 : OSVersion.Unknown;
            }
            break;
        }
        return OSVersion.Unknown;
      }
    }

    public static Process ExecSystemCmd(string cmd, string args)
    {
      Process process = new Process();
      process.EnableRaisingEvents = false;
      process.StartInfo.FileName = cmd;
      if (args != null)
        process.StartInfo.Arguments = args;
      process.Start();
      return process;
    }

    public static Process ExecSystemCmd(string cmd) => SystemUtil.ExecSystemCmd(cmd, (string) null);

    public static Process ShellExecute(string cmd)
    {
      Process process = new Process()
      {
        StartInfo = new ProcessStartInfo(cmd)
      };
      process.StartInfo.UseShellExecute = true;
      process.Start();
      return process;
    }

    public static string GetServiceStatusAsString(string serviceName)
    {
      return SystemUtil.GetServiceStatusAsString(new ServiceController(serviceName));
    }

    public static string GetServiceStatusAsString(ServiceController service)
    {
      if (service == null)
        return (string) null;
      try
      {
        service.Refresh();
        switch (service.Status)
        {
          case ServiceControllerStatus.Stopped:
            return "Stopped";
          case ServiceControllerStatus.StartPending:
            return "Start Pending";
          case ServiceControllerStatus.StopPending:
            return "Stop Pending";
          case ServiceControllerStatus.Running:
            return "Running";
          case ServiceControllerStatus.ContinuePending:
            return "Continue Pending";
          case ServiceControllerStatus.PausePending:
            return "Pause Pending";
          case ServiceControllerStatus.Paused:
            return "Paused";
          default:
            return "Unknown";
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static string NormalizePath(string path)
    {
      if (path == null || path == "")
        return "";
      return path.EndsWith("\\") ? path : path + "\\";
    }

    public static string CombinePath(params string[] paths)
    {
      if (paths == null)
        return (string) null;
      if (paths.Length == 0)
        return "";
      string str = paths[0];
      for (int index = 1; index < paths.Length; ++index)
      {
        if (str == null)
          str = paths[index];
        else if (paths[index] != null)
          str = !str.EndsWith("\\") || !paths[index].StartsWith("\\") ? (str.EndsWith("\\") || paths[index].StartsWith("\\") ? str + paths[index] : str + "\\" + paths[index]) : str + paths[index].Substring(1);
      }
      return str;
    }

    public static string GetFileNameFromPath(string path)
    {
      int num = path.LastIndexOf('\\');
      if (num < 0)
        return path;
      return num == path.Length - 1 ? "" : path.Substring(num + 1, path.Length - num - 1);
    }

    public static string GetParentDirectory(string path)
    {
      for (; path.EndsWith("\\"); path = path.Substring(0, path.Length - 1))
      {
        if (path.Length <= 1)
          return "";
      }
      int length = path.LastIndexOf('\\');
      return length <= 0 ? "\\" : path.Substring(0, length);
    }

    public static bool StartService(string name)
    {
      using (ServiceController service = new ServiceController(name))
        return SystemUtil.StartService(service);
    }

    public static bool StartService(ServiceController service)
    {
      try
      {
        if (service.Status == ServiceControllerStatus.Stopped)
          service.Start();
        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 20));
        service.Refresh();
        if (service.Status != ServiceControllerStatus.Running)
          throw new Exception("The server could not be started or stopped immediately.");
        return service.Status == ServiceControllerStatus.Running;
      }
      catch (Exception ex)
      {
        throw new Exception("Error while attempting to start service: " + ex.Message);
      }
    }

    public static bool StopService(string name)
    {
      using (ServiceController service = new ServiceController(name))
        return SystemUtil.StopService(service);
    }

    public static bool StopService(ServiceController service)
    {
      try
      {
        if (service.Status == ServiceControllerStatus.Running)
          service.Stop();
        service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 20));
        service.Refresh();
        if (service.Status != ServiceControllerStatus.Stopped)
          throw new Exception("The service could not be stopped.");
        return service.Status == ServiceControllerStatus.Stopped;
      }
      catch (Exception ex)
      {
        throw new Exception("Error while attempting to stop the service: " + ex.Message);
      }
    }

    public static bool IsServiceRunning(string name)
    {
      using (ServiceController serviceController = new ServiceController(name))
        return serviceController.Status == ServiceControllerStatus.Running;
    }

    public static bool IsEmailAddress(string emailAddress) => Utils.ValidateEmail(emailAddress);

    public static bool IsValidURL(string url)
    {
      if (url != null)
      {
        if (!(url == ""))
        {
          try
          {
            return new Uri(url).IsWellFormedOriginalString();
          }
          catch
          {
            return false;
          }
        }
      }
      return false;
    }

    public static SystemUtil.MemoryInfo GetMemoryInformation()
    {
      try
      {
        SystemUtil.MEMORYSTATUSEX lpBuffer = new SystemUtil.MEMORYSTATUSEX();
        lpBuffer.dwLength = (uint) Marshal.SizeOf(typeof (SystemUtil.MEMORYSTATUSEX));
        SystemUtil.GlobalMemoryStatusEx(ref lpBuffer);
        return new SystemUtil.MemoryInfo(lpBuffer.ullAvailPhys, lpBuffer.ullTotalPhys);
      }
      catch
      {
        return new SystemUtil.MemoryInfo(0UL, 0UL);
      }
    }

    public static string GetServerChangesetNumber()
    {
      try
      {
        object obj = (object) null;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass"))
          obj = registryKey?.GetValue("Version");
        return obj == null ? string.Empty : obj.ToString();
      }
      catch
      {
        return string.Empty;
      }
    }

    public static SystemUtil.ProcessorInfo GetProcessorInformation()
    {
      try
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0"))
          return new SystemUtil.ProcessorInfo(string.Concat(registryKey.GetValue("Identifier")), string.Concat(registryKey.GetValue("~MHz")), string.Concat(registryKey.GetValue("ProcessorNameString")));
      }
      catch
      {
        return new SystemUtil.ProcessorInfo("", "", "");
      }
    }

    [Serializable]
    public class ProcessorInfo
    {
      public readonly string ProcessorID;
      public readonly string ProcessorSpeed;
      public readonly string ProcessorName;

      public ProcessorInfo(string processorId, string processorSpeed)
      {
        this.ProcessorID = processorId;
        this.ProcessorSpeed = processorSpeed;
      }

      public ProcessorInfo(string processorId, string processorSpeed, string processorName)
      {
        this.ProcessorID = processorId;
        this.ProcessorSpeed = processorSpeed;
        this.ProcessorName = processorName;
      }
    }

    [Serializable]
    public class MemoryInfo
    {
      [CLSCompliant(false)]
      public readonly ulong AvailablePhysicalMemory;
      [CLSCompliant(false)]
      public readonly ulong TotalPhysicalMemory;

      [CLSCompliant(false)]
      public MemoryInfo(ulong availablePhysicalMemory, ulong totalPhysicalMemory)
      {
        this.AvailablePhysicalMemory = availablePhysicalMemory;
        this.TotalPhysicalMemory = totalPhysicalMemory;
      }
    }

    private struct MEMORY_INFO
    {
      public uint dwLength;
      public uint dwMemoryLoad;
      public uint dwTotalPhys;
      public uint dwAvailPhys;
      public uint dwTotalPageFile;
      public uint dwAvailPageFile;
      public uint dwTotalVirtual;
      public uint dwAvailVirtual;
    }

    public struct MEMORYSTATUSEX
    {
      public uint dwLength;
      public uint dwMemoryLoad;
      public ulong ullTotalPhys;
      public ulong ullAvailPhys;
      public ulong ullTotalPageFile;
      public ulong ullAvailPageFile;
      public ulong ullTotalVirtual;
      public ulong ullAvailVirtual;
      public ulong ullAvailExtendedVirtual;
    }

    private struct OSVERSIONINFOEX
    {
      public int dwOSVersionInfoSize;
      public int dwMajorVersion;
      public int dwMinorVersion;
      public int dwBuildNumber;
      public int dwPlatformId;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string szCSDVersion;
      public ushort wServicePackMajor;
      public ushort wServicePackMinor;
      public ushort wSuiteMask;
      public byte wProductType;
      public byte wReserved;
    }
  }
}
