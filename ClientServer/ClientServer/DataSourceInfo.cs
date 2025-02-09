// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DataSourceInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DataSourceInfo
  {
    public string LogDir;
    public string DataDir;
    public string Server;
    public string Database;
    public string UserID;
    public string Password;
    public string AGListener;

    public bool IsMultiTargetFileEnabled { get; }

    public bool IsSingleTargetFileEnabled { get; }

    public string singleTargetBaseDir { get; }

    public DataSourceInfo(
      string logDir,
      string dataDir,
      string server,
      string dbName,
      string userId,
      string password)
    {
      this.LogDir = logDir;
      this.DataDir = dataDir;
      this.Server = server;
      this.Database = dbName;
      this.UserID = userId;
      this.Password = password;
    }

    public DataSourceInfo(
      string logDir,
      string dataDir,
      string server,
      string dbName,
      string userId,
      string password,
      string AGListener,
      bool isMultiTargetFileEnabled,
      bool isSingleTargetFileEnabled,
      string singleTargetBaseDir)
    {
      this.LogDir = logDir;
      this.DataDir = dataDir;
      this.Server = server;
      this.Database = dbName;
      this.UserID = userId;
      this.Password = password;
      this.AGListener = AGListener;
      this.IsMultiTargetFileEnabled = isMultiTargetFileEnabled;
      this.IsSingleTargetFileEnabled = isSingleTargetFileEnabled;
      this.singleTargetBaseDir = singleTargetBaseDir;
    }
  }
}
