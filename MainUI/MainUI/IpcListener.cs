// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.IpcListener
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClickLoanImpl;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using SCAppMgrEnc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  internal class IpcListener
  {
    private const string className = "IpcListener";
    protected static string sw = Tracing.SwOutsideLoan;
    private static IChannel serverChannel = (IChannel) null;
    private static bool createIpcChannel = true;
    private static int numTries = 3;
    private static bool ipcLoginCheckServer = true;

    private IpcListener()
    {
    }

    static IpcListener()
    {
      ClickLoanWrapperImpl.GetIpcChannelRegistrySettings(ref IpcListener.createIpcChannel, ref IpcListener.ipcLoginCheckServer);
    }

    internal static void StartListening()
    {
      if (!IpcListener.createIpcChannel || IpcListener.serverChannel != null)
        return;
      Tracing.Log(IpcListener.sw, nameof (IpcListener), TraceLevel.Verbose, "Try registering IPC server channel");
      string str = Guid.NewGuid().ToString();
      for (int index = 0; index < IpcListener.numTries; ++index)
      {
        try
        {
          IpcListener.serverChannel = (IChannel) new IpcServerChannel((IDictionary) new Dictionary<string, string>()
          {
            {
              "authorizedGroup",
              "Everyone"
            },
            {
              "portName",
              str
            }
          }, (IServerChannelSinkProvider) new BinaryServerFormatterSinkProvider()
          {
            TypeFilterLevel = TypeFilterLevel.Full
          });
          ChannelServices.RegisterChannel(IpcListener.serverChannel, false);
          RemotingConfiguration.ApplicationName = "EncRemoteObject.rem";
          RemotingConfiguration.RegisterActivatedServiceType(typeof (EncRemoteObject));
          RemotingConfiguration.RegisterWellKnownServiceType(typeof (SCAppMgrEncRO), "SCAppMgrEncRO.rem", WellKnownObjectMode.Singleton);
          ((IpcServerChannel) IpcListener.serverChannel).StartListening((object) null);
          Registry.CurrentUser.CreateSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot + "\\" + str);
          using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot + "\\" + str, true))
          {
            registryKey.SetValue("CreationTime", (object) DateTime.Now.ToString());
            registryKey.SetValue("ErrorCount", (object) 0, RegistryValueKind.DWord);
          }
          SystemSettings.IpcPortName = str;
          break;
        }
        catch (Exception ex)
        {
          if (IpcListener.serverChannel != null)
          {
            try
            {
              ChannelServices.UnregisterChannel(IpcListener.serverChannel);
            }
            catch
            {
            }
            finally
            {
              IpcListener.serverChannel = (IChannel) null;
            }
          }
          if (index == IpcListener.numTries - 1)
          {
            Tracing.Log(IpcListener.sw, nameof (IpcListener), TraceLevel.Error, "Error registering IPC server channel with port name " + str + ": " + ex.Message);
            return;
          }
        }
      }
      Tracing.Log(IpcListener.sw, nameof (IpcListener), TraceLevel.Verbose, "IPC server channel registered with port name " + SystemSettings.IpcPortName);
    }

    private static bool prevInstance
    {
      get => Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
    }

    internal static void UnregisterServerChannel()
    {
      try
      {
        if (IpcListener.serverChannel != null)
          ChannelServices.UnregisterChannel(IpcListener.serverChannel);
      }
      catch (Exception ex)
      {
        Tracing.Log(IpcListener.sw, nameof (IpcListener), TraceLevel.Error, "Error unregistering IPC server channel: " + ex.Message);
      }
      IpcListener.serverChannel = (IChannel) null;
      if (IpcListener.prevInstance)
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot, true))
        {
          try
          {
            registryKey?.DeleteSubKey(SystemSettings.IpcPortName);
          }
          catch (Exception ex)
          {
            Tracing.Log(IpcListener.sw, nameof (IpcListener), TraceLevel.Warning, "Error deleting IPC registry '" + SystemSettings.IpcPortName + "': " + ex.Message);
          }
        }
      }
      else
      {
        try
        {
          if (Registry.CurrentUser.OpenSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot) != null)
            Registry.CurrentUser.DeleteSubKeyTree(ClickLoanWrapperImpl.EncIpcRegistryRoot);
        }
        catch (Exception ex)
        {
          Tracing.Log(IpcListener.sw, nameof (IpcListener), TraceLevel.Warning, "Error deleting IPC registry '" + ClickLoanWrapperImpl.EncIpcRegistryRoot + "': " + ex.Message);
        }
      }
      Tracing.Log(IpcListener.sw, nameof (IpcListener), TraceLevel.Verbose, "IPC server channel unregistered.");
    }
  }
}
