// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.DDMDiagnosticSession
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Microsoft.Win32;
using System;

#nullable disable
namespace EllieMae.EMLite.Diagnostics
{
  public class DDMDiagnosticSession : MarshalByRefObject
  {
    private string sessionId;

    public DDMDiagnosticSession(string sessionId) => this.sessionId = sessionId;

    public static DDMDiagnosticsMode DiagnosticsMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass", false))
          return registryKey == null || !(string.Concat(registryKey.GetValue("DDMDiagnostics")).ToLower() == "1") ? DDMDiagnosticsMode.Disabled : DDMDiagnosticsMode.Enabled;
      }
      set
      {
        using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey("Software\\Ellie Mae\\Encompass"))
        {
          if (value == DDMDiagnosticsMode.Enabled)
            subKey.SetValue("DDMDiagnostics", (object) "1");
          else
            subKey.SetValue("DDMDiagnostics", (object) "0");
        }
      }
    }
  }
}
