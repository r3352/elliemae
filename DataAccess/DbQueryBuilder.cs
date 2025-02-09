// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbQueryBuilder
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbQueryBuilder(string dbConnectionString) : BaseDbQueryBuilder(DbServerType.SqlServer, dbConnectionString)
  {
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
        strArray1[index] = values[index].ColumnName;
        strArray2[index] = values[index].Encode(table[values[index].ColumnName]);
      }
      this.AppendLine("insert into " + table.Name);
      this.AppendLine("   ([" + string.Join("], [", strArray1) + "])");
      if (returnInsertedId)
        this.AppendLine("output INSERTED.ID");
      this.AppendLine(nameof (values));
      this.AppendLine("   (" + string.Join(", ", strArray2) + ")");
    }

    public void InsertIntoDataTable(DataTable dtTable, DbTableInfo tableInfo, DbValueList values)
    {
      string[] strArray1 = new string[values.Count];
      string[] strArray2 = new string[values.Count];
      DataRow row = dtTable.NewRow();
      for (int index = 0; index < values.Count; ++index)
      {
        DbColumnInfo dbColumnInfo = tableInfo[values[index].ColumnName];
        strArray1[index] = values[index].ColumnName;
        row[values[index].ColumnName] = dbColumnInfo == null ? values[index].Value : (!(dbColumnInfo.DataType == typeof (string)) ? values[index].Value : (object) SQL.EncodeString(tableInfo[values[index].ColumnName].SizeToFit(Convert.ToString(values[index].Value)), false));
      }
      dtTable.Rows.Add(row);
    }

    public override void InsertInto(DbTableInfo table, List<DbValueList> values, DbVersion dbVer)
    {
      if (DbVersion.SQL2000 == dbVer || DbVersion.SQL2005 == dbVer || dbVer == DbVersion.None)
      {
        foreach (DbValueList values1 in values)
          this.InsertInto(table, values1, true, false);
      }
      else
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
                strArray1[index2] = values[index1][index2].ColumnName;
              else if (strArray1[index2] != values[index1][index2].ColumnName)
              {
                flag = false;
                break;
              }
            }
          }
          if (!flag)
          {
            this.InsertInto(table, values, DbVersion.SQL2005);
          }
          else
          {
            for (int index3 = 0; index3 < values.Count; ++index3)
            {
              string[] strArray2 = new string[values[index3].Count];
              for (int index4 = 0; index4 < values[index3].Count; ++index4)
                strArray2[index4] = values[index3][index4].Encode(table[values[index3][index4].ColumnName]);
              if (index3 == 0 || index3 % 1000 == 0)
              {
                this.AppendLine("insert into " + table.Name);
                this.AppendLine("   ([" + string.Join("], [", strArray1) + "])");
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
    }

    public override void Upsert(DbTableInfo table, DbValueList values, DbValue key)
    {
      this.Upsert(table, values, new DbValueList(new DbValue[1]
      {
        key
      }));
    }

    public override void Upsert(DbTableInfo table, DbValueList values, DbValueList keys)
    {
      this.AppendLine("IF EXISTS (SELECT 1 FROM " + table.Name);
      this.appendWhereClause(table, keys, true);
      this.Append(")");
      this.Update(table, values, keys);
      this.AppendLine("ELSE");
      DbValueList values1 = values.Clone();
      values1.Add(keys);
      this.InsertInto(table, values1, true, false);
    }

    public override void Upsert(
      DbTableInfo table,
      DbValueList insertValues,
      DbValueList updateValues,
      DbValueList keys)
    {
      this.AppendLine("IF EXISTS (SELECT 1 FROM " + table.Name);
      this.appendWhereClause(table, keys, true);
      this.Append(")");
      this.Update(table, updateValues, keys);
      this.AppendLine("ELSE");
      DbValueList values = insertValues.Clone();
      this.InsertInto(table, values, true, false);
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
        if (flag1 && values[index].ColumnName.ToLower() == "categoryid")
        {
          flag2 = true;
          dbValue = values[index];
        }
        else
          this.AppendLine("   [" + values[index].ColumnName + "] = " + values[index].Encode(table[values[index].ColumnName]) + (index < values.Count - 1 ? "," : ""));
      }
      int length = this.sql.Length;
      this.appendWhereClause(table, keys, true);
      if (!flag2)
        return;
      StringBuilder stringBuilder = new StringBuilder(this.sql.ToString());
      stringBuilder.Insert(length, "    ,[" + dbValue.ColumnName + "] = " + dbValue.Encode(table[dbValue.ColumnName]) + "\n");
      this.sql.Insert(0, "IF NOT exists (select * from BizCategory where categoryid = " + dbValue.Encode(table[dbValue.ColumnName]) + ")\n begin\n");
      this.sql.Append("\n end\n else\n begin\n" + stringBuilder.ToString() + "\n end");
    }

    public override void SelectFrom(DbTableInfo table, string[] columnNames, DbValueList keys)
    {
      this.Append("select ");
      if (columnNames != null && columnNames.Length != 0)
      {
        for (int index = 0; index < columnNames.Length; ++index)
          this.Append((index > 0 ? ", " : "") + "[" + columnNames[index] + "]");
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
      bool withsemicolon = true)
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
          this.AppendLine("([" + keys[index].ColumnName + "] is NULL)");
        else if (keys[index].Value is Array)
          this.AppendLine("([" + keys[index].ColumnName + "] in (" + str + "))");
        else
          this.AppendLine("([" + keys[index].ColumnName + "] = " + str + ")");
      }
    }

    public virtual void Select(string varName) => this.AppendLine("select " + varName);

    public virtual void Declare(string varName, string varType)
    {
      this.AppendLine("declare " + varName + " " + varType + "; ");
    }

    public virtual void SelectVar(string varName, object value)
    {
      this.SelectVar(varName, value, (IDbEncoder) DbEncoding.Default);
    }

    public virtual void SelectVar(string varName, object value, IDbEncoder encoding)
    {
      this.AppendLine("select " + varName + " = " + encoding.Encode(value, (DbColumnInfo) null));
    }

    public virtual void SelectIdentity(string varName)
    {
      this.AppendLine("select " + varName + " = convert(int, scope_identity())");
    }

    public virtual void SelectIdentity()
    {
      this.AppendLine("select convert(int, scope_identity())");
    }

    public override void RaiseError(string description)
    {
      this.AppendLine("raiserror(" + SQL.Encode((object) description) + ", 16, 1)");
    }

    public virtual void Begin() => this.AppendLine("begin");

    public virtual void End() => this.AppendLine("end");

    public virtual void If(string sql) => this.AppendLine("if (" + sql + ")");

    public virtual void IfExists(DbTableInfo table, DbValue keyValue)
    {
      this.IfExists(table, new DbValueList(new DbValue[1]
      {
        keyValue
      }));
    }

    public virtual void IfExists(DbTableInfo table, DbValueList keyValues)
    {
      this.Append("if exists(select 1 from " + table.Name + " ");
      this.appendWhereClause(table, keyValues, true);
      this.AppendLine(")");
    }

    public virtual void IfNotExists(DbTableInfo table, DbValue keyValue)
    {
      this.IfNotExists(table, new DbValueList(new DbValue[1]
      {
        keyValue
      }));
    }

    public virtual void IfNotExists(DbTableInfo table, DbValueList keyValues)
    {
      this.Append("if not exists(select 1 from " + table.Name + " ");
      this.appendWhereClause(table, keyValues, true);
      this.AppendLine(")");
    }

    public virtual void Else() => this.AppendLine("else");

    public override void AppendStoredProcedureCall(string spName, IEnumerable<string> values)
    {
      this.AppendLine("EXEC " + spName + (values == null || !values.Any<string>() ? (string) null : " " + string.Join(", ", values)));
    }

    public override string Variable(string baseName) => "@" + baseName;

    public override void AppendTake(int toTake, string query)
    {
      int num = query.IndexOf(' ');
      if (-1 >= num || num >= query.Length)
        throw new ArgumentException("SQL must use spaces");
      this.AppendLine(query.Substring(0, num) + " TOP " + (object) toTake + query.Substring(num));
    }

    public void AppendMsMergeTable(MergeTable table, bool useDelete)
    {
      MsMergeTable.AppendMergeTable(this, table, useDelete);
    }
  }
}
