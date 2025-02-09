// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.MongoUnitOfWork
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Common.Security;
using Elli.Domain;
using EllieMae.EMLite.Common.Settings;
using MongoDB.Driver;
using System;
using System.Text;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class MongoUnitOfWork : IUnitOfWork, IDisposable
  {
    private readonly object _lockObject = new object();
    [ThreadStatic]
    private static IStorageSettings _storageSettings;

    internal IMongoDatabase ActiveConnection()
    {
      lock (this._lockObject)
        return new MongoClient(this.CreateConnection(Rijndael256Util.Decrypt(MongoUnitOfWork._storageSettings.MongoActiveCredentials), MongoUnitOfWork._storageSettings.MongoActiveDatabase, MongoUnitOfWork._storageSettings.MongoActiveHosts, MongoUnitOfWork._storageSettings.MongoActiveOptions)).GetDatabase(MongoUnitOfWork._storageSettings.MongoActiveDatabase, (MongoDatabaseSettings) null);
    }

    internal IMongoDatabase ArchiveConnection()
    {
      lock (this._lockObject)
        return new MongoClient(this.CreateConnection(Rijndael256Util.Decrypt(MongoUnitOfWork._storageSettings.MongoArchiveCredentials), MongoUnitOfWork._storageSettings.MongoArchiveDatabase, MongoUnitOfWork._storageSettings.MongoArchiveHosts, MongoUnitOfWork._storageSettings.MongoArchiveOptions)).GetDatabase(MongoUnitOfWork._storageSettings.MongoArchiveDatabase, (MongoDatabaseSettings) null);
    }

    internal string GetClient()
    {
      lock (this._lockObject)
        return MongoUnitOfWork._storageSettings.ClientId;
    }

    internal string GetInstanceName()
    {
      lock (this._lockObject)
        return MongoUnitOfWork._storageSettings.InstanceId;
    }

    public void Begin()
    {
    }

    public void Begin(IStorageSettings storageSettings)
    {
      MongoUnitOfWork._storageSettings = storageSettings;
    }

    public void Begin(IStorageSettings storageSettings, out IMongoDatabase activeConnection)
    {
      this.Begin(storageSettings);
      activeConnection = this.ActiveConnection();
    }

    public void Commit()
    {
    }

    public void Dispose()
    {
    }

    internal string CreateConnection(
      string credentials,
      string database,
      string hosts,
      string options)
    {
      StringBuilder stringBuilder = new StringBuilder("mongodb://");
      if (!string.IsNullOrWhiteSpace(credentials))
        stringBuilder.Append(credentials + "@");
      stringBuilder.Append(hosts);
      if (!string.IsNullOrEmpty(options))
        stringBuilder.Append("/?" + this.GetConnectionOptions(options));
      return stringBuilder.ToString();
    }

    internal string GetConnectionOptions(string options)
    {
      if (string.IsNullOrEmpty(options))
        return string.Empty;
      string[] strArray1 = options.Split(',');
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in strArray1)
      {
        if (!str.Contains("="))
        {
          string[] strArray2 = str.Split(':');
          stringBuilder.Append(strArray2[0] + "=" + strArray2[1] + "&");
        }
        else
          stringBuilder.Append(str + "&");
      }
      return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
    }
  }
}
