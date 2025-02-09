// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EncompassSystemInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class EncompassSystemInfo
  {
    public readonly string SystemID;
    public readonly string DbVersion;
    public readonly DateTime CreationTime = DateTime.MaxValue;
    public readonly bool DataServicesOptOut;
    public readonly string DataServicesOptKey = "";
    public readonly string DbFullVersion;

    public EncompassSystemInfo(
      string systemID,
      string dbVersion,
      DateTime creationTime,
      bool DataServicesOptOut,
      string DataServicesOptKey,
      string dbFullVersion)
    {
      this.SystemID = systemID;
      this.DbVersion = dbVersion;
      this.CreationTime = creationTime;
      this.DataServicesOptOut = DataServicesOptOut;
      this.DataServicesOptKey = DataServicesOptKey;
      this.DbFullVersion = dbFullVersion;
    }
  }
}
