// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Schema.OrmSchema
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataAccess.DbSchema;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Data.SqlClient;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.Configuration.Schema
{
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

    public string GetDbScriptFullPath(string fileName)
    {
      string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath("DBScripts\\" + fileName);
      return string.IsNullOrEmpty(resourceFileFullPath) || !File.Exists(resourceFileFullPath) ? fileName : resourceFileFullPath;
    }

    public string Version => this.schemaVersion;

    private string GetDbSchemaScript()
    {
      string strA = this.schemaType.ToString();
      string str = "." + strA;
      if (string.Compare(strA, "Loan", true) == 0 && !File.Exists(this.GetDbScriptFullPath("Elli.Database" + str + ".DbSchema")))
        str = "";
      using (StringWriter stringWriter = new StringWriter())
      {
        SqlScriptWriter sqlScriptWriter = new SqlScriptWriter((TextWriter) stringWriter);
        string dbScriptFullPath1 = this.GetDbScriptFullPath("Elli.Database" + str + ".PreDeployment.sql");
        if (File.Exists(dbScriptFullPath1))
        {
          using (StreamReader streamReader = new StreamReader(dbScriptFullPath1))
            new SqlScript((TextReader) streamReader).WriteTo((IScriptWriter) sqlScriptWriter);
        }
        string dbScriptFullPath2 = this.GetDbScriptFullPath("Elli.Database" + str + ".DbSchema");
        if (!File.Exists(dbScriptFullPath2))
          throw new ArgumentException("Invalid schema name '" + strA + "'");
        using (StreamReader streamReader = new StreamReader(dbScriptFullPath2))
          new EllieMae.EMLite.DataAccess.DbSchema.Schema((TextReader) streamReader).Publish((IScriptWriter) sqlScriptWriter);
        string dbScriptFullPath3 = this.GetDbScriptFullPath("Elli.Database" + str + ".PostDeployment.sql");
        if (File.Exists(dbScriptFullPath3))
        {
          using (StreamReader streamReader = new StreamReader(dbScriptFullPath3))
            new SqlScript((TextReader) streamReader).WriteTo((IScriptWriter) sqlScriptWriter);
        }
        return stringWriter.ToString();
      }
    }

    public OrmSchemaType SchemaType => this.schemaType;

    public void Publish(string connectionString, int optionalCommandTimeout = 30)
    {
      using (SqlConnection sqlConnection = new SqlConnection(connectionString))
      {
        sqlConnection.Open();
        new SqlScript(this.schemaScript).WriteTo((IScriptWriter) new SqlConnectionWriter(sqlConnection, optionalCommandTimeout));
      }
    }

    public override string ToString() => this.schemaScript;
  }
}
