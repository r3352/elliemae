// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanXDBTable
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanXDBTable : IEnumerable
  {
    private string tableName = "";
    private LoanXDBTableList.TableTypes tableType;
    private ArrayList fieldList;
    private LoanXDBField lastModifiedField;

    public LoanXDBTable(string tableName, LoanXDBTableList.TableTypes tableType)
    {
      this.tableName = tableName;
      this.tableType = tableType;
      this.fieldList = new ArrayList();
    }

    public LoanXDBField GetFieldAt(int i) => (LoanXDBField) this.fieldList[i];

    public LoanXDBField LastModifiedField => this.lastModifiedField;

    public void AddField(
      string fieldID,
      string description,
      LoanXDBTableList.TableTypes fieldType,
      string fieldSize,
      bool useIndex,
      bool auditable,
      int xrefId,
      int pair,
      LoanXDBField.FieldStatus status)
    {
      this.AddField(new LoanXDBField(fieldID, description, fieldType, fieldSize, useIndex, auditable, xrefId, pair, status));
    }

    public void AddField(LoanXDBField dbField)
    {
      dbField.TableName = this.tableName;
      dbField.FieldType = this.tableType;
      this.fieldList.Add((object) dbField);
      if (this.lastModifiedField != null || !(dbField.FieldID == "LOANLASTMODIFIED"))
        return;
      this.lastModifiedField = dbField;
    }

    public void RemoveField(LoanXDBField dbField)
    {
      if (!this.fieldList.Contains((object) dbField))
        return;
      this.fieldList.Remove((object) dbField);
    }

    private object toEnum(string value, Type enumType)
    {
      try
      {
        return Enum.Parse(enumType, value, true);
      }
      catch
      {
        return (object) 0;
      }
    }

    public string TableName => this.tableName;

    public LoanXDBTableList.TableTypes TableType => this.tableType;

    public int FieldCount => this.fieldList.Count;

    public IEnumerator GetEnumerator() => this.fieldList.GetEnumerator();

    public string[] GetFieldNames()
    {
      string[] fieldNames = new string[this.FieldCount];
      for (int index = 0; index < this.FieldCount; ++index)
        fieldNames[index] = ((LoanXDBField) this.fieldList[index]).ColumnName;
      return fieldNames;
    }

    public static FieldFormat TableTypeToFieldFormat(LoanXDBTableList.TableTypes tableType)
    {
      if (tableType == LoanXDBTableList.TableTypes.IsNumeric)
        return FieldFormat.DECIMAL;
      return tableType == LoanXDBTableList.TableTypes.IsDate ? FieldFormat.DATE : FieldFormat.STRING;
    }

    public bool RequiresDatabaseChange()
    {
      if (this.fieldList.Count == 0)
        return true;
      foreach (LoanXDBField field in this.fieldList)
      {
        if (field.RequiresDatabaseChange)
          return true;
      }
      return false;
    }

    internal string ToSQLString()
    {
      bool flag1 = true;
      bool flag2 = true;
      foreach (LoanXDBField field in this.fieldList)
      {
        if (field.FieldCurrentStatus == LoanXDBField.FieldStatus.New)
          flag1 = false;
        else if (field.FieldCurrentStatus == LoanXDBField.FieldStatus.Removed)
        {
          flag2 = false;
        }
        else
        {
          flag1 = flag2 = false;
          break;
        }
      }
      if (flag1)
        return this.createTableDropScript();
      return flag2 ? this.createTableCreationScript() : this.createTableUpdateScript();
    }

    private string createTableUpdateScript()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (LoanXDBField field in this.fieldList)
      {
        if (field.RequiresDatabaseChange)
          stringBuilder.Append(field.ToSQLString());
      }
      return stringBuilder.ToString();
    }

    private string createTableCreationScript()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = Guid.NewGuid().ToString().Replace("-", "_");
      stringBuilder.AppendLine("if not exists (select 1 from sysobjects where id = object_id('" + this.tableName + "') and OBJECTPROPERTY(id, N'IsUserTable') = 1)");
      stringBuilder.AppendLine("Begin");
      stringBuilder.AppendLine("create table [" + this.tableName + "] (");
      stringBuilder.AppendLine("  [XrefId] [int] NOT NULL,");
      stringBuilder.AppendLine("  constraint PK_" + this.TableName + "_XrefId primary key clustered (XrefId)");
      stringBuilder.AppendLine(")");
      stringBuilder.AppendLine("End");
      stringBuilder.AppendLine("Else");
      stringBuilder.AppendLine("Begin");
      stringBuilder.AppendLine("DECLARE @idxsql" + str + " NVARCHAR(max)");
      stringBuilder.AppendLine("SELECT @idxsql" + str + " = ISNULL(@idxsql" + str + " + Char(13) + Char(10),'') + 'DROP INDEX " + this.tableName + ".' + QUOTENAME(name)");
      stringBuilder.AppendLine("FROM sys.indexes WHERE name like ('IX_LOANXDB_%')");
      stringBuilder.AppendLine("   AND OBJECT_ID = OBJECT_ID('" + this.tableName + "')");
      stringBuilder.AppendLine("IF(@@ROWCOUNT > 0)");
      stringBuilder.AppendLine("  EXEC (@idxsql" + str + ")");
      stringBuilder.AppendLine("DECLARE @dynsql" + str + " NVARCHAR(max)");
      stringBuilder.AppendLine("SELECT @dynsql" + str + " = ISNULL(@dynsql" + str + " + ',','') + QUOTENAME(name)");
      stringBuilder.AppendLine(" FROM sys.columns WHERE name not in ('XrefId')");
      stringBuilder.AppendLine(" AND OBJECT_ID=OBJECT_ID('" + this.tableName + "')");
      stringBuilder.AppendLine(" IF(@@ROWCOUNT > 0)");
      stringBuilder.AppendLine("    EXEC ('ALTER TABLE " + this.tableName + " DROP COLUMN ' + @dynsql" + str + ")");
      stringBuilder.AppendLine("End");
      foreach (LoanXDBField field in this.fieldList)
        stringBuilder.Append(field.ToSQLString());
      return stringBuilder.ToString();
    }

    private string createTableDropScript()
    {
      return "Delete From " + this.tableName + "\r\n" + this.createTableUpdateScript();
    }
  }
}
