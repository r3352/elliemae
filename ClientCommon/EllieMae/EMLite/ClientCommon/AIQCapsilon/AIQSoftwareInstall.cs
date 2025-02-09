// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.AIQCapsilon.AIQSoftwareInstall
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientCommon.AIQCapsilon
{
  public class AIQSoftwareInstall
  {
    private string softwareName = ConfigurationManager.AppSettings.Get("SoftwareName");

    public bool IsSoftwareInstalled()
    {
      try
      {
        string softwareName = this.softwareName;
        char[] chArray = new char[1]{ ',' };
        foreach (string name in softwareName.Split(chArray))
        {
          if (Registry.ClassesRoot.OpenSubKey(name, false) != null)
            return true;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwEFolder, TraceLevel.Error, nameof (IsSoftwareInstalled), string.Format("Error in reading the registry: {0}, installedProgramsPath: {1}", (object) ex.Message, (object) this.softwareName));
      }
      return false;
    }
  }
}
