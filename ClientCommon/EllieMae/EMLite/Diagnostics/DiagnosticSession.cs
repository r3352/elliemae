// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.DiagnosticSession
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Diagnostics
{
  public class DiagnosticSession : MarshalByRefObject
  {
    private const string AppDiagnosticsCommandFlag = "-diag";
    private const string SuppressDiagnosticsCommandFlag = "-nodiag";
    private string sessionId;
    private DateTime startTime = DateTime.MinValue;
    private DateTime endTime = DateTime.MinValue;

    public DiagnosticSession() => this.sessionId = Guid.NewGuid().ToString("N");

    public DiagnosticSession(string sessionId) => this.sessionId = sessionId;

    public string SessionID => this.sessionId;

    public DateTime StartTime => this.startTime;

    public DateTime EndTime => this.endTime;

    public void Execute(string exePath, string[] args)
    {
      EnConfigurationSettings.ApplyInstanceFromCommandLine(args);
      try
      {
        Exception runtimeException = this.executeApplication(exePath, args);
        if (!this.IsInitialized() || DiagnosticSession.DiagnosticsMode != DiagnosticsMode.AutoSubmit)
          return;
        this.submitDiagnosticData(runtimeException);
      }
      finally
      {
        this.purgeSessionData();
      }
    }

    private void submitDiagnosticData(Exception runtimeException)
    {
      new DiagnosticSubmissionProcess(this, runtimeException).Execute();
    }

    private Exception executeApplication(string exePath, string[] args)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) args);
      stringList.Add("-nodiag");
      stringList.Add("-debug");
      stringList.Add("-gctrace");
      stringList.Add("-logsid");
      stringList.Add("-sid");
      stringList.Add(this.sessionId);
      ProcessStartInfo startInfo = new ProcessStartInfo(exePath);
      startInfo.Arguments = "\"" + string.Join("\" \"", (IEnumerable<string>) AppSecurity.EncodeArguments(stringList.ToArray())) + "\"";
      try
      {
        this.startTime = DateTime.Now;
        Process.Start(startInfo).WaitForExit();
        return (Exception) null;
      }
      catch (Exception ex)
      {
        return ex;
      }
      finally
      {
        this.endTime = DateTime.Now;
      }
    }

    private void purgeSessionData()
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass\\Diagnostics", true))
          registryKey?.DeleteSubKeyTree(this.sessionId);
      }
      catch
      {
      }
      try
      {
        if (DiagnosticSession.DiagnosticsMode == DiagnosticsMode.PreserveLocal)
          return;
        Directory.Delete(this.GetSessionLogDir(), true);
      }
      catch
      {
      }
    }

    public string GetSessionLogDir()
    {
      return EnConfigurationSettings.GlobalSettings.GetUniqueLogDirectoryForSession(this.sessionId);
    }

    public string GetVariable(string name)
    {
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass\\Diagnostics\\" + this.sessionId, false))
        return registryKey == null ? "" : string.Concat(registryKey.GetValue(name));
    }

    public string[] GetVariableNames()
    {
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass\\Diagnostics\\" + this.sessionId, false))
        return registryKey == null ? new string[0] : registryKey.GetValueNames();
    }

    public void SetVariable(string name, string value)
    {
      using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey("Software\\Ellie Mae\\Encompass\\Diagnostics\\" + this.sessionId))
        subKey.SetValue(name, (object) (value ?? ""));
    }

    public static DiagnosticSession GetCurrent()
    {
      return DiagnosticSession.IsDiagnosticSession() ? new DiagnosticSession(EnConfigurationSettings.ApplicationSessionID) : (DiagnosticSession) null;
    }

    public static bool IsDiagnosticSession()
    {
      return EnConfigurationSettings.ApplicationArgumentExists("-nodiag");
    }

    public static void SetSessionVariable(string name, string value)
    {
      DiagnosticSession.GetCurrent()?.SetVariable(name, value);
    }

    public static bool IsDiagnosticsSessionRequired(string[] args)
    {
      foreach (string strA in args)
      {
        if (string.Compare(strA, "-nodiag", true) == 0)
          return false;
      }
      if (DiagnosticSession.DiagnosticsMode != DiagnosticsMode.Disabled)
        return true;
      foreach (string strA in args)
      {
        if (string.Compare(strA, "-diag", true) == 0)
          return true;
      }
      return false;
    }

    public static DiagnosticsMode DiagnosticsMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass", false))
        {
          if (registryKey == null)
            return DiagnosticsMode.Disabled;
          switch (string.Concat(registryKey.GetValue("Diagnostics")).ToLower())
          {
            case "1":
            case "auto":
              return DiagnosticsMode.AutoSubmit;
            case "local":
              return DiagnosticsMode.PreserveLocal;
            default:
              return DiagnosticsMode.Disabled;
          }
        }
      }
      set
      {
        using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey("Software\\Ellie Mae\\Encompass"))
        {
          if (value == DiagnosticsMode.AutoSubmit)
            subKey.SetValue("Diagnostics", (object) "Auto");
          else if (value == DiagnosticsMode.PreserveLocal)
          {
            subKey.SetValue("Diagnostics", (object) "Local");
          }
          else
          {
            if (!(string.Concat(subKey.GetValue("Diagnostics")) != ""))
              return;
            subKey.DeleteValue("Diagnostics");
          }
        }
      }
    }

    public static string CaseNumber
    {
      get
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass", false))
          return registryKey == null ? "" : string.Concat(registryKey.GetValue(nameof (CaseNumber)));
      }
      set
      {
        using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey("Software\\Ellie Mae\\Encompass"))
          subKey.SetValue(nameof (CaseNumber), (object) value);
      }
    }

    public static void InitializeSessionVariables()
    {
      DiagnosticSession current = DiagnosticSession.GetCurrent();
      if (current == null)
        return;
      current.SetVariable("ClientID", Session.CompanyInfo.ClientID);
      current.SetVariable("UserID", Session.UserID);
      current.SetVariable("Server", Session.RemoteServer);
      string str = "";
      foreach (Persona userPersona in Session.UserInfo.UserPersonas)
        str = str + userPersona.Name + ";";
      current.SetVariable("Personas", str);
    }

    public bool IsInitialized() => this.GetVariable("ClientID") != "";
  }
}
