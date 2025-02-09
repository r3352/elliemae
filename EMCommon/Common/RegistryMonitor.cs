// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.RegistryMonitor
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class RegistryMonitor : IDisposable
  {
    private const int KEY_QUERY_VALUE = 1;
    private const int KEY_NOTIFY = 16;
    private const int STANDARD_RIGHTS_READ = 131072;
    private static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(int.MinValue);
    private static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);
    private static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);
    private static readonly IntPtr HKEY_USERS = new IntPtr(-2147483645);
    private static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);
    private static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);
    private static readonly IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);
    private IntPtr _registryHive;
    private string _registrySubName;
    private object _threadLock = new object();
    private Thread _thread;
    private bool _disposed;
    private ManualResetEvent _eventTerminate = new ManualResetEvent(false);
    private RegChangeNotifyFilter _regFilter = RegChangeNotifyFilter.Key | RegChangeNotifyFilter.Attribute | RegChangeNotifyFilter.Value | RegChangeNotifyFilter.Security;

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegOpenKeyEx(
      IntPtr hKey,
      string subKey,
      uint options,
      int samDesired,
      out IntPtr phkResult);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegNotifyChangeKeyValue(
      IntPtr hKey,
      bool bWatchSubtree,
      RegChangeNotifyFilter dwNotifyFilter,
      IntPtr hEvent,
      bool fAsynchronous);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegCloseKey(IntPtr hKey);

    public event EventHandler RegChanged;

    protected virtual void OnRegChanged()
    {
      EventHandler regChanged = this.RegChanged;
      if (regChanged == null)
        return;
      regChanged((object) this, (EventArgs) null);
    }

    public event ErrorEventHandler Error;

    protected virtual void OnError(Exception e)
    {
      ErrorEventHandler error = this.Error;
      if (error == null)
        return;
      error((object) this, new ErrorEventArgs(e));
    }

    public RegistryMonitor(RegistryKey registryKey) => this.InitRegistryKey(registryKey.Name);

    public RegistryMonitor(string name)
    {
      if (name == null || name.Length == 0)
        throw new ArgumentNullException(nameof (name));
      this.InitRegistryKey(name);
    }

    public RegistryMonitor(RegistryHive registryHive, string subKey)
    {
      this.InitRegistryKey(registryHive, subKey);
    }

    public void Dispose()
    {
      this.Stop();
      this._disposed = true;
      GC.SuppressFinalize((object) this);
    }

    public RegChangeNotifyFilter RegChangeNotifyFilter
    {
      get => this._regFilter;
      set
      {
        lock (this._threadLock)
        {
          if (this.IsMonitoring)
            throw new InvalidOperationException("Monitoring thread is already running");
          this._regFilter = value;
        }
      }
    }

    private void InitRegistryKey(RegistryHive hive, string name)
    {
      switch (hive)
      {
        case RegistryHive.ClassesRoot:
          this._registryHive = RegistryMonitor.HKEY_CLASSES_ROOT;
          break;
        case RegistryHive.CurrentUser:
          this._registryHive = RegistryMonitor.HKEY_CURRENT_USER;
          break;
        case RegistryHive.LocalMachine:
          this._registryHive = RegistryMonitor.HKEY_LOCAL_MACHINE;
          break;
        case RegistryHive.Users:
          this._registryHive = RegistryMonitor.HKEY_USERS;
          break;
        case RegistryHive.PerformanceData:
          this._registryHive = RegistryMonitor.HKEY_PERFORMANCE_DATA;
          break;
        case RegistryHive.CurrentConfig:
          this._registryHive = RegistryMonitor.HKEY_CURRENT_CONFIG;
          break;
        case RegistryHive.DynData:
          this._registryHive = RegistryMonitor.HKEY_DYN_DATA;
          break;
        default:
          throw new InvalidEnumArgumentException(nameof (hive), (int) hive, typeof (RegistryHive));
      }
      this._registrySubName = name;
    }

    private void InitRegistryKey(string name)
    {
      string[] strArray = name.Split('\\');
      switch (strArray[0])
      {
        case "HKCR":
        case "HKEY_CLASSES_ROOT":
          this._registryHive = RegistryMonitor.HKEY_CLASSES_ROOT;
          break;
        case "HKCU":
        case "HKEY_CURRENT_USER":
          this._registryHive = RegistryMonitor.HKEY_CURRENT_USER;
          break;
        case "HKEY_CURRENT_CONFIG":
          this._registryHive = RegistryMonitor.HKEY_CURRENT_CONFIG;
          break;
        case "HKEY_LOCAL_MACHINE":
        case "HKLM":
          this._registryHive = RegistryMonitor.HKEY_LOCAL_MACHINE;
          break;
        case "HKEY_USERS":
          this._registryHive = RegistryMonitor.HKEY_USERS;
          break;
        default:
          this._registryHive = IntPtr.Zero;
          throw new ArgumentException("The registry hive '" + strArray[0] + "' is not supported", "value");
      }
      this._registrySubName = string.Join("\\", strArray, 1, strArray.Length - 1);
    }

    public bool IsMonitoring => this._thread != null;

    public void Start()
    {
      if (this._disposed)
        throw new ObjectDisposedException((string) null, "This instance is already disposed");
      lock (this._threadLock)
      {
        if (this.IsMonitoring)
          return;
        this._eventTerminate.Reset();
        this._thread = new Thread(new ThreadStart(this.MonitorThread));
        this._thread.IsBackground = true;
        this._thread.Start();
      }
    }

    public void Stop()
    {
      if (this._disposed)
        throw new ObjectDisposedException((string) null, "This instance is already disposed");
      lock (this._threadLock)
      {
        Thread thread = this._thread;
        if (thread == null)
          return;
        this._eventTerminate.Set();
        thread.Join();
      }
    }

    private void MonitorThread()
    {
      try
      {
        this.ThreadLoop();
      }
      catch (Exception ex)
      {
        this.OnError(ex);
      }
      this._thread = (Thread) null;
    }

    private void ThreadLoop()
    {
      IntPtr phkResult;
      int error1 = RegistryMonitor.RegOpenKeyEx(this._registryHive, this._registrySubName, 0U, 131089, out phkResult);
      if (error1 != 0)
        throw new Win32Exception(error1);
      try
      {
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        WaitHandle[] waitHandles = new WaitHandle[2]
        {
          (WaitHandle) autoResetEvent,
          (WaitHandle) this._eventTerminate
        };
        while (!this._eventTerminate.WaitOne(0, true))
        {
          int error2 = RegistryMonitor.RegNotifyChangeKeyValue(phkResult, true, this._regFilter, autoResetEvent.SafeWaitHandle.DangerousGetHandle(), true);
          if (error2 != 0)
            throw new Win32Exception(error2);
          if (WaitHandle.WaitAny(waitHandles) == 0)
            this.OnRegChanged();
        }
      }
      finally
      {
        if (phkResult != IntPtr.Zero)
          RegistryMonitor.RegCloseKey(phkResult);
      }
    }
  }
}
