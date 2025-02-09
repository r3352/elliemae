// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.DbTableEncryption.SettingsFlag
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.DbTableEncryption
{
  public class SettingsFlag
  {
    private SqlConnection DbConn { get; }

    protected string TableName { get; }

    protected string PkWhereClause { get; private set; }

    protected string PkInsertValues { get; private set; }

    protected string PkInsertColumnNames { get; private set; }

    protected Dictionary<string, string> PkNameValues { get; }

    protected IEnumerable<SqlParameter> PkParameters { get; set; }

    public SettingsFlag(SqlConnection dbConn, string tableName, string category, string attribute)
    {
      this.DbConn = dbConn;
      this.TableName = tableName;
      this.PkNameValues = new Dictionary<string, string>()
      {
        {
          "Category",
          category
        },
        {
          "Attribute",
          attribute
        }
      };
    }

    public void Init(Dictionary<string, string> additionalPkColumns = null)
    {
      if (additionalPkColumns != null)
        additionalPkColumns.ToList<KeyValuePair<string, string>>().ForEach((Action<KeyValuePair<string, string>>) (kvp => this.PkNameValues.Add(kvp.Key, kvp.Value)));
      this.PkWhereClause = "WHERE " + string.Join(" AND ", this.PkNameValues.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kvp => pkColumnName(kvp.Key) + " = " + pkParameterName(kvp.Key))));
      this.PkInsertColumnNames = string.Join(", ", this.PkNameValues.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kvp => pkColumnName(kvp.Key))));
      this.PkInsertValues = string.Join(", ", this.PkNameValues.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kvp => pkParameterName(kvp.Key))));
      this.PkParameters = this.PkNameValues.Select<KeyValuePair<string, string>, SqlParameter>((Func<KeyValuePair<string, string>, SqlParameter>) (kvp => new SqlParameter(pkParameterName(kvp.Key), (object) kvp.Value)));

      static string pkParameterName(string key) => "@pk" + key;

      static string pkColumnName(string key) => "[" + key + "]";
    }

    private SqlCommand MakeSqlCommand(string sql) => new SqlCommand(sql, this.DbConn);

    protected string FlagValue
    {
      get
      {
        SqlCommand sqlCommand = this.MakeSqlCommand("SELECT [value] FROM " + this.TableName + " " + this.PkWhereClause);
        sqlCommand.Parameters.AddRange(this.PkParameters.ToArray<SqlParameter>());
        object obj = sqlCommand.ExecuteScalar();
        if (obj is DBNull)
          return (string) null;
        return obj?.ToString();
      }
      set
      {
        SqlCommand sqlCommand = this.MakeSqlCommand("IF NOT EXISTS (SELECT 1 FROM " + this.TableName + " " + this.PkWhereClause + ")\nINSERT INTO " + this.TableName + " (" + this.PkInsertColumnNames + ", [value]) VALUES (" + this.PkInsertValues + ", @flagValue)\nELSE UPDATE " + this.TableName + " SET [value] = @flagValue " + this.PkWhereClause);
        sqlCommand.Parameters.AddWithValue("@flagValue", (object) (value ?? ""));
        sqlCommand.Parameters.AddRange(this.PkParameters.ToArray<SqlParameter>());
        sqlCommand.ExecuteNonQuery();
      }
    }
  }
}
