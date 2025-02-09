// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.PgDbQueryBuilder
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class PgDbQueryBuilder(string dbConnectionString) : BaseDbQueryBuilder(DbServerType.Postgres, dbConnectionString)
  {
    public override void AppendStoredProcedureCall(string spName, IEnumerable<string> values)
    {
      this.AppendLine("CALL " + spName.ToLowerInvariant() + ("(" + string.Join(", ", values) + ");"));
    }

    public override string Variable(string baseName) => "v_" + baseName;

    public override void InsertInto(
      DbTableInfo table,
      DbValueList values,
      bool withSemicolon = true,
      bool returnInsertedId = false)
    {
      string[] strArray1 = new string[values.Count];
      string[] strArray2 = new string[values.Count];
      for (int index = 0; index < values.Count; ++index)
      {
        strArray1[index] = values[index].ColumnName.ToLowerInvariant();
        strArray2[index] = values[index].Encode(table[values[index].ColumnName]);
      }
      this.AppendLine("insert into " + table.Name);
      this.AppendLine("   (\"" + string.Join("\", \"", strArray1) + "\")");
      this.AppendLine(nameof (values));
      this.AppendLine("   (" + string.Join(", ", strArray2) + ")");
      if (!withSemicolon)
        return;
      this.Append(";");
    }

    public override void DeleteFrom(DbTableInfo table, DbValueList keys)
    {
      base.DeleteFrom(table, keys);
    }

    public void InsertIntoReturnCurrVal(
      DbTableInfo table,
      List<DbValueList> values,
      string variableName)
    {
      this.InsertInto(table, values, DbVersion.PostgreSQL11);
      this.AppendLine("; select currval('" + table.Name + "_seq') into " + variableName + ";");
    }

    public override void InsertInto(DbTableInfo table, List<DbValueList> values, DbVersion dbVer)
    {
      if (values.Count <= 0)
        return;
      if (1 == values.Count)
      {
        this.InsertInto(table, values[0], true, false);
      }
      else
      {
        bool flag = true;
        string[] strArray1 = new string[values[0].Count];
        for (int index1 = 0; flag && index1 < values.Count; ++index1)
        {
          if (index1 > 0 && values[index1].Count != strArray1.Length)
            flag = false;
          for (int index2 = 0; index2 < values[index1].Count; ++index2)
          {
            if (index1 == 0)
              strArray1[index2] = values[index1][index2].ColumnName.ToLowerInvariant();
            else if (strArray1[index2] != values[index1][index2].ColumnName.ToLowerInvariant())
            {
              flag = false;
              break;
            }
          }
        }
        if (!flag)
        {
          foreach (DbValueList values1 in values)
            this.InsertInto(table, values1, true, false);
        }
        else
        {
          for (int index3 = 0; index3 < values.Count; ++index3)
          {
            string[] strArray2 = new string[values[index3].Count];
            for (int index4 = 0; index4 < values[index3].Count; ++index4)
              strArray2[index4] = values[index3][index4].Encode(table[values[index3][index4].ColumnName]);
            if (index3 == 0)
            {
              this.AppendLine("insert into " + table.Name);
              this.AppendLine("   (\"" + string.Join("\", \"", strArray1) + "\")");
              this.AppendLine(nameof (values));
            }
            else
              this.AppendLine(",");
            this.Append("   (" + string.Join(", ", strArray2) + ")");
          }
          this.AppendLine("");
        }
      }
    }

    public override void Upsert(DbTableInfo table, DbValueList values, DbValue key)
    {
      throw new NotImplementedException();
    }

    public override void Upsert(DbTableInfo table, DbValueList values, DbValueList key)
    {
      throw new NotImplementedException();
    }

    public override void Upsert(
      DbTableInfo table,
      DbValueList insertValues,
      DbValueList updateValues,
      DbValueList keys)
    {
      throw new NotImplementedException();
    }

    public override void Update(DbTableInfo table, DbValueList values, DbValueList keys)
    {
      bool flag1 = false;
      if (table.Name.ToLower() == "bizpartner")
        flag1 = true;
      this.AppendLine("update " + table.Name + " set");
      bool flag2 = false;
      DbValue dbValue = (DbValue) null;
      for (int index = 0; index < values.Count; ++index)
      {
        if (flag1 && values[index].ColumnName.ToLowerInvariant() == "categoryid")
        {
          flag2 = true;
          dbValue = values[index];
        }
        else
          this.AppendLine("   \"" + values[index].ColumnName.ToLowerInvariant() + "\" = " + values[index].Encode(table[values[index].ColumnName]) + (index < values.Count - 1 ? "," : ""));
      }
      int length = this.sql.Length;
      this.appendWhereClause(table, keys, true);
      if (!flag2)
        return;
      StringBuilder stringBuilder = new StringBuilder(this.sql.ToString());
      stringBuilder.Insert(length, "    ,\"" + dbValue.ColumnName.ToLowerInvariant() + "\" = " + dbValue.Encode(table[dbValue.ColumnName]) + "\n");
      this.sql.Append(";");
      this.sql.Insert(0, "DO $$ \n BEGIN \n IF NOT exists (select * from BizCategory where categoryid = " + dbValue.Encode(table[dbValue.ColumnName]) + ") then\n");
      this.sql.Append("\n ELSE\n " + stringBuilder.ToString() + ";\n END IF;\n END $$;");
    }

    public override void SelectFrom(DbTableInfo table, string[] columnNames, DbValueList keys)
    {
      this.Append("select ");
      if (columnNames != null && columnNames.Length != 0)
      {
        for (int index = 0; index < columnNames.Length; ++index)
          this.Append((index > 0 ? ", " : "") + "\"" + columnNames[index].ToLowerInvariant() + "\"");
      }
      else
        this.Append("*");
      this.AppendLine("");
      this.AppendLine("from " + table.Name);
      this.appendWhereClause(table, keys, true);
    }

    protected override void appendWhereClause(
      DbTableInfo table,
      DbValueList keys,
      bool withSemicolon = true)
    {
      if (keys == null || keys.Count == 0)
        return;
      this.Append("where ");
      for (int index = 0; index < keys.Count; ++index)
      {
        string str = keys[index].Encode(table[keys[index].ColumnName]);
        if (index > 0)
          this.Append("   and ");
        if (str == "NULL")
          this.AppendLine("(\"" + keys[index].ColumnName.ToLowerInvariant() + "\" is NULL)");
        else if (keys[index].Value is Array)
          this.AppendLine("(\"" + keys[index].ColumnName.ToLowerInvariant() + "\" in (" + str + "))");
        else
          this.AppendLine("(\"" + keys[index].ColumnName.ToLowerInvariant() + "\" = " + str + ")");
      }
      if (!withSemicolon)
        return;
      this.AppendLine(";");
    }

    public override void RaiseError(string description)
    {
      this.AppendLine("raise exception " + SQL.Encode((object) description) + "; ");
    }

    public virtual void DoStatement() => this.AppendLine("do $$ ");

    public virtual void BeginStatement() => this.AppendLine("begin ");

    public virtual void Endstatement() => this.AppendLine("end$$;");

    public virtual void SetValue(string variable, object value)
    {
      this.AppendLine(this.Variable(variable) + " = " + DbEncoding.Default.Encode(value, (DbColumnInfo) null) + "; ");
    }

    public virtual void SelectIdentity(string varName, string tableName)
    {
      this.AppendLine("select currval('" + tableName + "_seq') into " + this.Variable(varName) + "; ");
    }

    public override void AppendTake(int toTake, string query)
    {
      this.AppendLine(query + " LIMIT " + (object) toTake);
    }
  }
}
