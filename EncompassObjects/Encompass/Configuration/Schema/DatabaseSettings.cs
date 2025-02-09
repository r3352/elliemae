// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Schema.DatabaseSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.Client;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.Encompass.Configuration.Schema
{
  /// <summary>
  /// Provides schema information about the Encompass loan database
  /// </summary>
  public class DatabaseSettings : SessionBoundObject
  {
    /// <summary>Constructor for the DatabaseSettings object</summary>
    internal DatabaseSettings(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Gets the version number of the database schema for the specified ORM mapping from the
    /// Encompass Server.
    /// </summary>
    /// <param name="schemaType">The <see cref="T:EllieMae.Encompass.Configuration.Schema.OrmSchemaType" /> for the schema to be retrieved.</param>
    /// <returns>Returns the version number of the current ORM Schema.</returns>
    public string GetServerSchemaVersion(OrmSchemaType schemaType)
    {
      return ((IServerManager) this.Session.GetObject("ServerManager")).GetDbSchemaVersion(schemaType.ToString());
    }

    /// <summary>
    /// Gets the version number of the database schema for the specified business object from
    /// a local database.
    /// </summary>
    /// <param name="schemaType">The <see cref="T:EllieMae.Encompass.Configuration.Schema.OrmSchemaType" /> for the schema to be retrieved.</param>
    /// <param name="connectionString">The connection string to access the local database.</param>
    /// <returns>Returns the version number of the ORM Schema from the local system.</returns>
    public string GetLocalSchemaVersion(OrmSchemaType schemaType, string connectionString)
    {
      string cmdText = "select [VersionName] from [ElliDbVersion] where [SchemaName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) schemaType.ToString());
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return (string) EllieMae.EMLite.DataAccess.SQL.Decode(new SqlCommand(cmdText, connection).ExecuteScalar(), (object) null);
      }
    }

    /// <summary>
    /// Sets the version number from encompass to the ElliDBVersion table in local
    /// </summary>
    /// <param name="schemaVersion"></param>
    /// <param name="connectionString"></param>
    public void SetLocalSchemaVersion(string schemaVersion, string connectionString)
    {
      string cmdText = "update [ElliDbVersion] set [VersionName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) schemaVersion);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        new SqlCommand(cmdText, connection).ExecuteNonQuery();
      }
    }

    /// <summary>Retrieves the specified ORM schema from smartclient.</summary>
    /// <param name="schemaType">The <see cref="T:EllieMae.Encompass.Configuration.Schema.OrmSchemaType" /> of the schema to be retrieved.</param>
    /// <returns>Returns an <see cref="T:EllieMae.Encompass.Configuration.Schema.OrmSchema" /> for the specified schema type.</returns>
    public OrmSchema GetSchema(OrmSchemaType schemaType)
    {
      IServerManager serverManager = (IServerManager) this.Session.GetObject("ServerManager");
      return new OrmSchema(schemaType, serverManager.GetDbSchemaVersion(OrmSchemaType.Loan.ToString()));
    }

    /// <summary>
    /// Retrieves the mapping assembly for the specified schema type.
    /// </summary>
    /// <param name="schemaType">The <see cref="T:EllieMae.Encompass.Configuration.Schema.OrmSchemaType" /> for which the mapping assembly will be retrieved.</param>
    /// <returns>A <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> containing the image of the assembly that contains the
    /// mapping for populating data using the specified OrmSchema.</returns>
    public DataObject GetOrmMappingAssembly(OrmSchemaType schemaType)
    {
      return new DataObject(((IServerManager) this.Session.GetObject("ServerManager")).GetElliDataAssembly());
    }

    /// <summary>
    /// Get the script folder from smartclient. Just a property to show the console where the file is being fetched.
    /// </summary>
    public string DBScriptFolder
    {
      get
      {
        return AssemblyResolver.GetResourceFileFullPath("DBScripts\\Elli.Database.Mortgage.PostDeployment.sql");
      }
    }
  }
}
