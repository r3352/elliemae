// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Schema.OrmSchema
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataAccess.DbSchema;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Data.SqlClient;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.Configuration.Schema
{
  /// <summary>
  /// Provides a class that represents the schema for an Object-Relational Mapping
  /// </summary>
  public class OrmSchema
  {
    private OrmSchemaType schemaType;
    private string schemaVersion;
    private string schemaScript;
    private const string DBSCRIPT_FOLDER = "DBScripts\\�";

    internal OrmSchema(OrmSchemaType schemaType, string schemaVersion)
    {
      this.schemaType = schemaType;
      this.schemaVersion = schemaVersion;
      this.schemaScript = this.GetDbSchemaScript();
    }

    /// <summary>Get the script folder from smartclient</summary>
    public string GetDbScriptFullPath(string fileName)
    {
      string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath("DBScripts\\" + fileName);
      return string.IsNullOrEmpty(resourceFileFullPath) || !File.Exists(resourceFileFullPath) ? fileName : resourceFileFullPath;
    }

    /// <summary>Gets the version of the ORM schema</summary>
    public string Version => this.schemaVersion;

    /// <summary>
    /// Gets the script for creating the ElliLoan schema from
    /// </summary>
    private string GetDbSchemaScript()
    {
      string strA = this.schemaType.ToString();
      string str = "." + strA;
      if (string.Compare(strA, "Loan", true) == 0 && !File.Exists(this.GetDbScriptFullPath("Elli.Database" + str + ".DbSchema")))
        str = "";
      using (StringWriter writer1 = new StringWriter())
      {
        SqlScriptWriter writer2 = new SqlScriptWriter((TextWriter) writer1);
        string dbScriptFullPath1 = this.GetDbScriptFullPath("Elli.Database" + str + ".PreDeployment.sql");
        if (File.Exists(dbScriptFullPath1))
        {
          using (StreamReader sqlScript = new StreamReader(dbScriptFullPath1))
            new SqlScript((TextReader) sqlScript).WriteTo((IScriptWriter) writer2);
        }
        string dbScriptFullPath2 = this.GetDbScriptFullPath("Elli.Database" + str + ".DbSchema");
        if (!File.Exists(dbScriptFullPath2))
          throw new ArgumentException("Invalid schema name '" + strA + "'");
        using (StreamReader schemaReader = new StreamReader(dbScriptFullPath2))
          new EllieMae.EMLite.DataAccess.DbSchema.Schema((TextReader) schemaReader).Publish((IScriptWriter) writer2);
        string dbScriptFullPath3 = this.GetDbScriptFullPath("Elli.Database" + str + ".PostDeployment.sql");
        if (File.Exists(dbScriptFullPath3))
        {
          using (StreamReader sqlScript = new StreamReader(dbScriptFullPath3))
            new SqlScript((TextReader) sqlScript).WriteTo((IScriptWriter) writer2);
        }
        return writer1.ToString();
      }
    }

    /// <summary>Gets the schema type for the object</summary>
    public OrmSchemaType SchemaType => this.schemaType;

    /// <summary>
    /// Publishes the database schema to the database at the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string for the database to which the schema
    /// should be applied.</param>
    /// <param name="optionalCommandTimeout">Optional SQL CommandTimeout</param>
    public void Publish(string connectionString, int optionalCommandTimeout = 30)
    {
      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        new SqlScript(this.schemaScript).WriteTo((IScriptWriter) new SqlConnectionWriter(conn, optionalCommandTimeout));
      }
    }

    /// <summary>Returns a script version of the schema.</summary>
    /// <returns>Returns a SQL script that will apply the schema to a database.</returns>
    public override string ToString() => this.schemaScript;
  }
}
