// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Schema.DatabaseSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.Client;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.Encompass.Configuration.Schema
{
  public class DatabaseSettings : SessionBoundObject
  {
    internal DatabaseSettings(Session session)
      : base(session)
    {
    }

    public string GetServerSchemaVersion(OrmSchemaType schemaType)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).GetDbSchemaVersion(schemaType.ToString());
    }

    public string GetLocalSchemaVersion(OrmSchemaType schemaType, string connectionString)
    {
      string cmdText = "select [VersionName] from [ElliDbVersion] where [SchemaName] = " + SQL.Encode((object) schemaType.ToString());
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return (string) SQL.Decode(new SqlCommand(cmdText, connection).ExecuteScalar(), (object) null);
      }
    }

    public void SetLocalSchemaVersion(string schemaVersion, string connectionString)
    {
      string cmdText = "update [ElliDbVersion] set [VersionName] = " + SQL.Encode((object) schemaVersion);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        new SqlCommand(cmdText, connection).ExecuteNonQuery();
      }
    }

    public OrmSchema GetSchema(OrmSchemaType schemaType)
    {
      IServerManager iserverManager = (IServerManager) this.Session.GetObject("ServerManager");
      return new OrmSchema(schemaType, iserverManager.GetDbSchemaVersion(OrmSchemaType.Loan.ToString()));
    }

    public DataObject GetOrmMappingAssembly(OrmSchemaType schemaType)
    {
      return new DataObject(((IServerManager) this.Session.GetObject("ServerManager")).GetElliDataAssembly());
    }

    public string DBScriptFolder
    {
      get
      {
        return AssemblyResolver.GetResourceFileFullPath("DBScripts\\Elli.Database.Mortgage.PostDeployment.sql");
      }
    }
  }
}
