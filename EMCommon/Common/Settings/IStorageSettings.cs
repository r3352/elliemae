// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Settings.IStorageSettings
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.Settings
{
  public interface IStorageSettings
  {
    string SqlConnectionString { get; set; }

    string ClientId { get; set; }

    string InstanceId { get; set; }

    string MongoActiveCredentials { get; set; }

    string MongoActiveDatabase { get; set; }

    string MongoActiveHosts { get; set; }

    string MongoActiveOptions { get; set; }

    string MongoArchiveCredentials { get; set; }

    string MongoArchiveDatabase { get; set; }

    string MongoArchiveHosts { get; set; }

    string MongoArchiveOptions { get; set; }

    string PostgreSqlConnectionString { get; set; }
  }
}
