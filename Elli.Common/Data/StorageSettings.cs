// Decompiled with JetBrains decompiler
// Type: Elli.Common.Data.StorageSettings
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Common.Settings;
using System;

#nullable disable
namespace Elli.Common.Data
{
  [Serializable]
  public class StorageSettings : IStorageSettings
  {
    public string SqlConnectionString { get; set; }

    public string ClientId { get; set; }

    public string InstanceId { get; set; }

    public string MongoActiveCredentials { get; set; }

    public string MongoActiveDatabase { get; set; }

    public string MongoActiveHosts { get; set; }

    public string MongoActiveOptions { get; set; }

    public string MongoArchiveCredentials { get; set; }

    public string MongoArchiveDatabase { get; set; }

    public string MongoArchiveHosts { get; set; }

    public string MongoArchiveOptions { get; set; }

    public string PostgreSqlConnectionString { get; set; }
  }
}
