// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SchemaAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataAccess.DbSchema;
using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class SchemaAccessor
  {
    public static string GetDbSchemaVersion(string schemaName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("company_settings");
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        "value"
      }, new DbValueList()
      {
        {
          "category",
          (object) "ELLIDBVERSION"
        },
        {
          "attribute",
          (object) schemaName
        }
      });
      return (string) SQL.Decode(dbQueryBuilder.ExecuteScalar(), (object) null);
    }

    public static string GetDbSchemaScript(string schemaName)
    {
      string path1 = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "DBScripts");
      string str = "." + schemaName;
      if (string.Compare(schemaName, "Loan", true) == 0 && !File.Exists(Path.Combine(path1, "Elli.Database" + str + ".DbSchema")))
        str = "";
      using (StringWriter writer1 = new StringWriter())
      {
        SqlScriptWriter writer2 = new SqlScriptWriter((TextWriter) writer1);
        string path2 = Path.Combine(path1, "Elli.Database" + str + ".PreDeployment.sql");
        if (File.Exists(path2))
        {
          using (StreamReader sqlScript = new StreamReader(path2))
            new SqlScript((TextReader) sqlScript).WriteTo((IScriptWriter) writer2);
        }
        string path3 = Path.Combine(path1, "Elli.Database" + str + ".DbSchema");
        if (!File.Exists(path3))
          throw new ArgumentException("Invalid schema name '" + schemaName + "'");
        using (StreamReader schemaReader = new StreamReader(path3))
          new Schema((TextReader) schemaReader).Publish((IScriptWriter) writer2);
        string path4 = Path.Combine(path1, "Elli.Database" + str + ".PostDeployment.sql");
        if (File.Exists(path4))
        {
          using (StreamReader sqlScript = new StreamReader(path4))
            new SqlScript((TextReader) sqlScript).WriteTo((IScriptWriter) writer2);
        }
        return writer1.ToString();
      }
    }

    public static BinaryObject GetElliDataAssembly()
    {
      string path = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "Elli.Data.dll");
      return File.Exists(path) ? new BinaryObject(path, true) : throw new Exception("The Elli.Data assembly cannot be found at '" + path + "'");
    }
  }
}
