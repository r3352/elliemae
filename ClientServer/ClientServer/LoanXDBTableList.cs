// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanXDBTableList
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Cache;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanXDBTableList : 
    FastSerializable,
    ICriterionTranslator,
    IEnumerable,
    IHashableContents,
    ISerializable
  {
    public const int DBStringDefaultLength = 64;
    private const int DBMaxFieldCount = 100;
    private const int DBMaxTableSize = 8000;
    private const int DBMaxStringFieldCount = 249;
    private const string DBPrefixName = "LOANXDB_�";
    private const string DBTableMiddleSName = "S_�";
    private const string DBTableMiddleNName = "N_�";
    private const string DBTableMiddleDName = "D_�";
    private const string FastSerializeVersion = "{v1}�";
    private ArrayList tableList;
    private Hashtable updateList;
    private Hashtable tableNameList;
    private Hashtable auditableTableList;
    private DateTime lastModifiedDate = DateTime.UtcNow;

    [PgReady]
    public LoanXDBTableList(DataRowCollection rows)
      : base("{v1}")
    {
      this.tableList = new ArrayList();
      this.tableNameList = new Hashtable();
      this.updateList = new Hashtable();
      this.auditableTableList = new Hashtable();
      if (rows == null || rows.Count == 0)
        return;
      for (int index = 0; index < rows.Count; ++index)
      {
        object obj1 = rows[index]["TableName"];
        if (obj1 != DBNull.Value)
        {
          string tableName = ((string) obj1).Trim();
          object obj2 = rows[index]["FieldID"];
          if (obj2 != DBNull.Value)
          {
            string fieldID = ((string) obj2).Trim();
            if (fieldID.StartsWith("*"))
              fieldID = fieldID.Substring(1);
            int num1 = (int) rows[index]["TableType"];
            int num2 = rows[index]["FieldSize"] == DBNull.Value ? 0 : (int) rows[index]["FieldSize"];
            object obj3 = rows[index]["Description"];
            string description = (obj3 == DBNull.Value ? "" : (string) obj3).Trim();
            bool useIndex = false;
            bool auditable = false;
            if (EnConfigurationSettings.GlobalSettings.DatabaseType == DbServerType.Postgres)
            {
              object obj4 = rows[index]["UseIndex"];
              if (obj4 != DBNull.Value && (int) obj4 != 0)
                useIndex = true;
              object obj5 = rows[index]["Auditable"];
              if (obj5 != DBNull.Value && (int) obj5 != 0)
                auditable = true;
            }
            else
            {
              object obj6 = rows[index]["UseIndex"];
              if (obj6 != DBNull.Value)
                useIndex = (bool) obj6;
              object obj7 = rows[index]["Auditable"];
              if (obj7 != DBNull.Value)
                auditable = (bool) obj7;
            }
            int comortgagorPair = rows[index]["Pair"] == DBNull.Value ? 0 : (int) rows[index]["Pair"];
            object obj8 = rows[index]["XRefID"];
            int fieldXRefId = obj8 != DBNull.Value ? (int) obj8 : throw new Exception("Invalid XRefID specified for field " + fieldID);
            LoanXDBField.FieldStatus status = (LoanXDBField.FieldStatus) rows[index]["Status"];
            LoanXDBTableList.TableTypes tableTypes = LoanXDBTableList.TableTypes.IsString;
            switch (num1)
            {
              case 1:
                tableTypes = LoanXDBTableList.TableTypes.IsNumeric;
                break;
              case 2:
                tableTypes = LoanXDBTableList.TableTypes.IsDate;
                break;
            }
            LoanXDBTable loanXdbTable1;
            if (!this.tableNameList.ContainsKey((object) tableName.ToUpper()))
            {
              loanXdbTable1 = new LoanXDBTable(tableName, tableTypes);
              this.tableList.Add((object) loanXdbTable1);
              this.tableNameList.Add((object) tableName.ToUpper(), (object) loanXdbTable1);
            }
            else
              loanXdbTable1 = (LoanXDBTable) this.tableNameList[(object) tableName.ToUpper()];
            LoanXDBField dbField = new LoanXDBField(fieldID, description, tableTypes, num2.ToString(), useIndex, auditable, fieldXRefId, comortgagorPair, status);
            if (auditable)
            {
              LoanXDBTable loanXdbTable2;
              if (!this.auditableTableList.ContainsKey((object) tableName.ToUpper()))
              {
                loanXdbTable2 = new LoanXDBTable(tableName, tableTypes);
                this.auditableTableList.Add((object) tableName.ToUpper(), (object) loanXdbTable2);
              }
              else
                loanXdbTable2 = (LoanXDBTable) this.auditableTableList[(object) tableName.ToUpper()];
              loanXdbTable2.AddField(dbField);
            }
            loanXdbTable1.AddField(dbField);
          }
        }
      }
    }

    public LoanXDBTableList()
      : base("{v1}")
    {
      this.tableList = new ArrayList();
      this.updateList = new Hashtable();
      this.tableNameList = new Hashtable();
    }

    public LoanXDBTableList(SerializationInfo info, StreamingContext context)
      : base("{v1}", info, context)
    {
    }

    protected override void Initialize(BinaryReader br)
    {
      this.tableList = new ArrayList();
      this.tableNameList = new Hashtable();
      this.updateList = new Hashtable();
      this.auditableTableList = new Hashtable();
      this.lastModifiedDate = new DateTime(br.ReadInt64(), DateTimeKind.Utc);
      HashSet<string> source = new HashSet<string>();
      for (int index = br.ReadInt32(); index > 0; --index)
        source.Add(br.ReadString());
      string str = (string) null;
      LoanXDBTable loanXdbTable1 = (LoanXDBTable) null;
      LoanXDBTable loanXdbTable2 = (LoanXDBTable) null;
      while (br.PeekChar() != -1)
      {
        LoanXDBField dbField = new LoanXDBField(br);
        string tableName = dbField.TableName;
        if (!string.Equals(str, tableName))
        {
          str = tableName;
          loanXdbTable1 = new LoanXDBTable(str, dbField.FieldType);
          this.tableList.Add((object) loanXdbTable1);
          this.tableNameList.Add((object) str.ToUpper(), (object) loanXdbTable1);
          loanXdbTable2 = (LoanXDBTable) null;
        }
        loanXdbTable1.AddField(dbField);
        if (dbField.Auditable)
        {
          if (loanXdbTable2 == null)
          {
            loanXdbTable2 = new LoanXDBTable(str, dbField.FieldType);
            this.auditableTableList.Add((object) str.ToUpper(), (object) loanXdbTable2);
          }
          loanXdbTable2.AddField(dbField);
        }
        if (source.Any<string>())
        {
          string columnName = dbField.ColumnName;
          if (source.Contains(columnName))
          {
            this.updateList[(object) columnName] = (object) dbField;
            source.Remove(columnName);
          }
        }
      }
    }

    public void AddTable(string tableName, LoanXDBTableList.TableTypes tableType)
    {
      this.tableList.Add((object) new LoanXDBTable(tableName, tableType));
    }

    public string[] GetTableNameList()
    {
      string[] tableNameList = new string[this.tableList.Count];
      for (int index = 0; index < this.tableList.Count; ++index)
        tableNameList[index] = ((LoanXDBTable) this.tableList[index]).TableName;
      return tableNameList;
    }

    public int TableCount => this.tableList.Count;

    public LoanXDBTable GetTableAt(int i) => (LoanXDBTable) this.tableList[i];

    public Hashtable GetFields()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      string empty = string.Empty;
      for (int index = 0; index < this.tableList.Count; ++index)
      {
        LoanXDBTable table = (LoanXDBTable) this.tableList[index];
        for (int i = 0; i < table.FieldCount; ++i)
        {
          LoanXDBField fieldAt = table.GetFieldAt(i);
          string key = fieldAt.FieldID;
          if (fieldAt.ComortgagorPair > 1)
            key = FieldPairParser.GetFieldIDForBorrowerPair(fieldAt.FieldID, fieldAt.ComortgagorPair);
          if (!insensitiveHashtable.ContainsKey((object) key))
            insensitiveHashtable.Add((object) key, (object) fieldAt);
        }
      }
      return insensitiveHashtable;
    }

    public LoanXDBField[] GetAllFields()
    {
      Hashtable fields = this.GetFields();
      if (fields.Count == 0)
        return new LoanXDBField[0];
      LoanXDBField[] allFields = new LoanXDBField[fields.Count];
      fields.Values.CopyTo((Array) allFields, 0);
      return allFields;
    }

    public LoanXDBField GetField(string fieldId)
    {
      FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
      int comortgagorPair = fieldPairInfo.PairIndex;
      if (comortgagorPair < 1)
        comortgagorPair = 1;
      return this.GetField(fieldPairInfo.FieldID, comortgagorPair) ?? this.GetField(fieldId, 1);
    }

    public LoanXDBField GetField(string fieldId, int comortgagorPair)
    {
      for (int index = 0; index < this.tableList.Count; ++index)
      {
        LoanXDBTable table = (LoanXDBTable) this.tableList[index];
        for (int i = 0; i < table.FieldCount; ++i)
        {
          LoanXDBField fieldAt = table.GetFieldAt(i);
          int num = fieldAt.ComortgagorPair;
          if (num == 0)
            num = 1;
          if (string.Compare(fieldAt.FieldID, fieldId, true) == 0 && num == comortgagorPair)
            return fieldAt;
        }
      }
      return (LoanXDBField) null;
    }

    public LoanXDBField GetFieldByColumnName(string columnId)
    {
      for (int index = 0; index < this.tableList.Count; ++index)
      {
        LoanXDBTable table = (LoanXDBTable) this.tableList[index];
        for (int i = 0; i < table.FieldCount; ++i)
        {
          LoanXDBField fieldAt = table.GetFieldAt(i);
          if (fieldAt.ColumnName == columnId)
            return fieldAt;
        }
      }
      return (LoanXDBField) null;
    }

    public LoanXDBField[] GetFieldsRequiringRebuild()
    {
      ArrayList arrayList = new ArrayList();
      foreach (LoanXDBTable table in this.tableList)
      {
        foreach (LoanXDBField loanXdbField in table)
        {
          if (loanXdbField.RequiresRebuild)
            arrayList.Add((object) loanXdbField);
        }
      }
      return (LoanXDBField[]) arrayList.ToArray(typeof (LoanXDBField));
    }

    public bool ContainsField(string fieldId) => this.GetField(fieldId) != null;

    public LoanXDBField[] GetTableFields()
    {
      int length = 0;
      for (int index = 0; index < this.tableList.Count; ++index)
      {
        LoanXDBTable table = (LoanXDBTable) this.tableList[index];
        length += table.FieldCount;
      }
      LoanXDBField[] tableFields = new LoanXDBField[length];
      int num = -1;
      for (int index = 0; index < this.tableList.Count; ++index)
      {
        LoanXDBTable table = (LoanXDBTable) this.tableList[index];
        for (int i = 0; i < table.FieldCount; ++i)
        {
          LoanXDBField fieldAt = table.GetFieldAt(i);
          tableFields[++num] = fieldAt;
        }
      }
      return tableFields;
    }

    public void ClearUpdateList() => this.updateList.Clear();

    public LoanXDBField[] GetUpdateList()
    {
      LoanXDBField[] updateList = new LoanXDBField[this.updateList.Count];
      IDictionaryEnumerator enumerator = this.updateList.GetEnumerator();
      int num = -1;
      while (enumerator.MoveNext())
      {
        LoanXDBField loanXdbField = (LoanXDBField) enumerator.Value;
        updateList[++num] = loanXdbField;
      }
      return updateList;
    }

    public void AddField(LoanXDBField dbField)
    {
      if (!(dbField.TableName != string.Empty) || !this.tableNameList.ContainsKey((object) dbField.TableName.ToUpper()))
        return;
      ((LoanXDBTable) this.tableNameList[(object) dbField.TableName.ToUpper()])?.AddField(dbField);
    }

    public void RemoveField(LoanXDBField dbField)
    {
      if (!this.updateList.ContainsKey((object) dbField.ColumnName) || dbField.FieldCurrentStatus != LoanXDBField.FieldStatus.New)
        return;
      this.updateList.Remove((object) dbField.ColumnName);
    }

    public void AddUpdateList(LoanXDBField dbField, LoanXDBField.FieldStatus status)
    {
      dbField.FieldCurrentStatus = status;
      this.updateList[(object) dbField.ColumnName] = (object) dbField;
    }

    public void UpdateTable()
    {
      IDictionaryEnumerator enumerator = this.updateList.GetEnumerator();
      while (enumerator.MoveNext())
      {
        LoanXDBField dbField = (LoanXDBField) enumerator.Value;
        if (dbField != null && dbField.FieldCurrentStatus == LoanXDBField.FieldStatus.New)
        {
          bool flag = false;
          for (int index = 0; index < this.tableList.Count; ++index)
          {
            LoanXDBTable table = (LoanXDBTable) this.tableList[index];
            if (table != null && dbField.FieldType == table.TableType)
            {
              if (table.TableType == LoanXDBTableList.TableTypes.IsDate || table.TableType == LoanXDBTableList.TableTypes.IsNumeric)
              {
                if (table.FieldCount < 100)
                {
                  dbField.TableName = table.TableName;
                  table.AddField(dbField);
                  flag = true;
                  break;
                }
              }
              else
              {
                int num = 0;
                for (int i = 0; i < table.FieldCount; ++i)
                {
                  LoanXDBField fieldAt = table.GetFieldAt(i);
                  if (fieldAt != null)
                    num += fieldAt.FieldSizeToInteger;
                }
                if (num + dbField.FieldSizeToInteger <= 8000 && table.FieldCount < 249)
                {
                  dbField.TableName = table.TableName;
                  table.AddField(dbField);
                  flag = true;
                  break;
                }
              }
            }
          }
          if (!flag)
          {
            string str1 = "LOANXDB_";
            string str2 = dbField.FieldType != LoanXDBTableList.TableTypes.IsDate ? (dbField.FieldType != LoanXDBTableList.TableTypes.IsNumeric ? str1 + "S_" : str1 + "N_") : str1 + "D_";
            string tableName = string.Empty;
            int num = 0;
            while (num < 4000)
            {
              ++num;
              tableName = str2 + num.ToString("00");
              if (!this.tableNameList.ContainsKey((object) tableName.ToUpper()))
                break;
            }
            LoanXDBTable loanXdbTable = new LoanXDBTable(tableName, dbField.FieldType);
            loanXdbTable.AddField(dbField);
            this.tableList.Add((object) loanXdbTable);
            if (!this.tableNameList.ContainsKey((object) tableName.ToUpper()))
              this.tableNameList.Add((object) tableName.ToUpper(), (object) loanXdbTable);
          }
        }
      }
    }

    public bool RequiresDatabaseChange()
    {
      foreach (LoanXDBTable table in this.tableList)
      {
        if (table.RequiresDatabaseChange())
          return true;
      }
      return false;
    }

    public IEnumerator GetEnumerator() => this.tableList.GetEnumerator();

    public Hashtable GetAuditableTableList() => this.auditableTableList;

    public string GetUpdateSQLQuery()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (LoanXDBTable table in this.tableList)
      {
        if (table.RequiresDatabaseChange())
          stringBuilder.AppendLine(table.ToSQLString());
      }
      return stringBuilder.ToString();
    }

    public void ResetLastModifiedDate() => this.lastModifiedDate = DateTime.UtcNow;

    int IHashableContents.GetContentsHashCode()
    {
      return ObjectArrayHelpers.GetAggregateHash((object) nameof (LoanXDBTableList), (object) this.lastModifiedDate);
    }

    public ICriterionNameFormatter CriterionNameFormatter { get; set; }

    public CriterionName TranslateName(string fieldName)
    {
      CriterionName criName = CriterionName.Parse(fieldName, this.CriterionNameFormatter);
      if (string.Compare(criName.FieldSource, "AuditTrail", true) == 0)
        return this.auditFieldToDbColumn(criName);
      if (string.Compare(criName.FieldSource, "Fields", true) == 0)
        return this.rdbFieldToDbColumn(criName);
      return criName.FieldSource.StartsWith("LoanExternalFields") ? new CriterionName(criName.FieldSource, criName.FieldName.Replace(".", "_")) : criName;
    }

    public QueryCriterion TranslateCriterion(QueryCriterion cri) => cri;

    private CriterionName auditFieldToDbColumn(CriterionName criName)
    {
      string[] strArray = criName.FieldName.Split('.');
      string str1 = strArray.Length >= 1 ? string.Join(".", strArray, 0, strArray.Length - 1) : throw new Exception("Invalid field specification '" + (object) criName + "'");
      string str2 = strArray[strArray.Length - 1];
      LoanXDBField field = this.GetField(str1);
      if (field == null)
        throw new FieldNotInDBException(str1, "The field '" + str1 + "' is not in the reporting database");
      if (!field.Auditable)
        throw new Exception("The field '" + str1 + "' is not auditable");
      AuditTrailDataElement dataElement = (AuditTrailDataElement) Enum.Parse(typeof (AuditTrailDataElement), str2);
      return new CriterionName("AuditTrail_" + (object) field.FieldXRefID, LoanXDBAuditField.GetColumnNameForDataElement(dataElement));
    }

    private CriterionName rdbFieldToDbColumn(CriterionName criName)
    {
      LoanXDBField field = this.GetField(criName.FieldName);
      if (field == null)
        throw new FieldNotInDBException(criName.FieldName, "The field '" + criName.FieldName + "' is not in the reporting database");
      return new CriterionName(field.TableName, field.ColumnName);
    }

    protected override void WriteBytes(BinaryWriter bw)
    {
      bw.Write(this.lastModifiedDate.Ticks);
      bw.Write(this.updateList == null ? 0 : this.updateList.Count);
      foreach (string key in (IEnumerable) this.updateList.Keys)
        bw.Write(key);
      foreach (LoanXDBTable table in this.tableList)
      {
        foreach (LoanXDBField loanXdbField in table)
          loanXdbField.WriteBytes(bw);
      }
    }

    public enum TableTypes
    {
      IsString,
      IsNumeric,
      IsDate,
    }
  }
}
