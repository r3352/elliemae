// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.EncAppMgr.IEncAppMgrRO
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.Encompass.EncAppMgr
{
  public interface IEncAppMgrRO
  {
    bool IsAlive();

    string DownloadServerHotUpdates(
      string userid,
      string password,
      string clientID,
      string encMajorVersion,
      int tipHotUpdateNumber);

    string DownloadServerMajorUpdates(
      string userid,
      string password,
      string clientID,
      string encMajorVersion,
      int tipMajorUpdateNumber);

    string PrepareServerHotUpdates(
      string userid,
      string password,
      string encMajorVersion,
      int maxInstalledShuNum,
      string[] shuFilenames);

    string PrepareServerMajorUpdates(
      string userid,
      string password,
      string encMajorVersion,
      int maxInstalledSmuNum,
      string[] smuFilenames);

    void InstallServerMajorUpdates(
      string userid,
      string password,
      string encMajorVersion,
      int maxInstalledSmuNum,
      string[] smuDirs,
      bool stopServer);

    void InstallServerHotUpdates(
      string userid,
      string password,
      string encMajorVersion,
      int maxInstalledShuNum,
      string[] shuDirs,
      bool stopServer);

    string[] GetServerVersionUpdates(
      string userid,
      string password,
      string clientID,
      string encMajorVersion,
      string updateType);

    void StopIIsEncompassServer(string userid, string password);

    string FileCopy(string srcFilePath, bool deleteSrcAfterCopy);

    void Regsvr32(string srcFilePath, bool deleteSrcAfterCopy);

    void UnRegsvr32(string srcFilePath, bool deleteAfterUnregister);

    void Regasm(string filePath, bool codebase);

    void UnRegasm(string filePath);

    void DownloadAndExecute(
      bool waitForExit,
      string urlPath,
      string args,
      bool deleteFileAfterExec);
  }
}
