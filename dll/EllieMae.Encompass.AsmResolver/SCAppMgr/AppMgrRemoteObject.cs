// Decompiled with JetBrains decompiler
// Type: SCAppMgr.AppMgrRemoteObject
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;

#nullable disable
namespace SCAppMgr
{
  public class AppMgrRemoteObject : MarshalByRefObject
  {
    public string DotNetVersion = "4.0.30319";

    public Version AssemblyVersion => (Version) null;

    public Version FileVersion => (Version) null;

    public Version SCAppMgrInstallerFileVersion => (Version) null;

    public void CheckUpdatesAndUpgrade()
    {
    }

    public bool SCAppMgrUpdateAvailable() => false;

    public string FileCopy(string srcFilePath, bool deleteSrcAfterCopy) => (string) null;

    public void Regsvr32(string srcFilePath, bool deleteSrcAfterCopy)
    {
    }

    public void UnRegsvr32(string srcFilePath, bool deleteAfterUnregister)
    {
    }

    public void Regsvr32Creator15(string srcFilePath, bool deleteSrcAfterCopy)
    {
    }

    public void UnRegsvr32Creator15(string srcFilePath, bool deleteAfterUnregister)
    {
    }

    public void Regsvr32Creator(string version, string srcFilePath, bool deleteSrcAfterCopy)
    {
    }

    public void UnRegsvr32Creator(string version, string srcFilePath, bool deleteAfterUnregister)
    {
    }

    public void Regasm(string filePath, bool codebase)
    {
    }

    public void UnRegasm(string filePath)
    {
    }

    public void DownloadAndExecute(
      bool waitForExit,
      string urlPath,
      string args,
      bool deleteFileAfterExec)
    {
    }

    public override object InitializeLifetimeService() => (object) null;
  }
}
