// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanXDBField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanXDBField
  {
    private const string invalidStr = " *!@#$%^&*()-+=~`'[]{}:;<>/?,|\"\\�";
    public const string DbFieldPrefix = "Fields�";
    private string tableName = "";
    private string fieldID = "";
    private string description = "";
    private string fieldSize = "";
    private bool useIndex;
    private bool includeInReports = true;
    private bool auditable;
    private LoanXDBTableList.TableTypes fieldType;
    private int comortgagorPair = 1;
    private int fieldXRefId = -1;
    private LoanXDBField.FieldStatus fieldCurrentStatus;
    private bool indexIsChanged;
    private bool auditIsChanged;
    private bool sizeIsChanged;
    private bool descriptionIsChanged;
    private int fieldNewSize;
    private bool forceRebuild;

    public LoanXDBField(FieldDefinition fieldDef)
      : this(fieldDef.FieldID, fieldDef.Description, LoanXDBField.ColumnTypeToTableType(fieldDef.ReportingDatabaseColumnType), string.Concat((object) fieldDef.ReportingDatabaseColumnSize), false, false, -1, LoanXDBField.FieldStatus.New)
    {
    }

    public LoanXDBField(LoanXDBField dbField)
    {
      this.tableName = dbField.TableName;
      this.fieldID = dbField.FieldID;
      this.description = dbField.Description;
      this.fieldType = dbField.FieldType;
      this.fieldSize = dbField.FieldSizeToString;
      this.includeInReports = dbField.includeInReports;
      this.auditable = dbField.auditable;
      this.comortgagorPair = dbField.ComortgagorPair;
      this.fieldXRefId = dbField.fieldXRefId;
    }

    public LoanXDBField(
      string fieldID,
      string description,
      LoanXDBTableList.TableTypes fieldType,
      string fieldSize,
      bool useIndex,
      bool auditable,
      int fieldXRefId,
      LoanXDBField.FieldStatus status)
      : this(fieldID, description, fieldType, fieldSize, useIndex, auditable, fieldXRefId, 1, status)
    {
    }

    public LoanXDBField(
      string fieldID,
      string description,
      LoanXDBTableList.TableTypes fieldType,
      string fieldSize,
      bool useIndex,
      bool auditable,
      int fieldXRefId,
      int comortgagorPair,
      LoanXDBField.FieldStatus status)
    {
      this.tableName = "";
      this.fieldID = fieldID;
      this.description = description;
      this.fieldType = fieldType;
      if (fieldSize == "")
        fieldSize = 64.ToString();
      this.fieldSize = fieldSize;
      this.useIndex = useIndex;
      this.auditIsChanged = false;
      this.auditable = auditable;
      this.comortgagorPair = comortgagorPair;
      this.fieldXRefId = fieldXRefId;
      this.fieldCurrentStatus = status;
      if (this.fieldCurrentStatus != LoanXDBField.FieldStatus.Updated)
        return;
      this.forceRebuild = true;
    }

    public LoanXDBField(BinaryReader br)
    {
      this.tableName = br.ReadString();
      this.fieldID = br.ReadString();
      this.description = br.ReadString();
      this.fieldSize = br.ReadString();
      this.useIndex = br.ReadBoolean();
      this.includeInReports = br.ReadBoolean();
      this.auditable = br.ReadBoolean();
      this.fieldType = (LoanXDBTableList.TableTypes) br.ReadInt32();
      this.comortgagorPair = br.ReadInt32();
      this.fieldXRefId = br.ReadInt32();
      this.fieldCurrentStatus = (LoanXDBField.FieldStatus) br.ReadInt32();
      this.indexIsChanged = br.ReadBoolean();
      this.auditIsChanged = br.ReadBoolean();
      this.sizeIsChanged = br.ReadBoolean();
      this.descriptionIsChanged = br.ReadBoolean();
      this.fieldNewSize = br.ReadInt32();
      this.forceRebuild = br.ReadBoolean();
    }

    public LoanXDBField(
      string fieldID,
      string description,
      LoanXDBTableList.TableTypes fieldType,
      string fieldSize,
      bool useIndex,
      bool auditable,
      int comortgagorPair,
      LoanXDBField.FieldStatus status,
      bool includeInReports)
      : this(fieldID, description, fieldType, fieldSize, useIndex, auditable, comortgagorPair, status)
    {
      this.includeInReports = includeInReports;
    }

    public string LegacyAuditTableName
    {
      get => LoanXDBAuditField.GetLegacyAuditTableName(this.FieldIDWithCoMortgagor);
    }

    public string ReportingCriterionName => "Fields." + this.FieldIDWithCoMortgagor;

    public string TableName
    {
      get => this.tableName;
      set => this.tableName = value;
    }

    public string ColumnName => LoanXDBField.GetDbColumnName(this.FieldIDWithCoMortgagor);

    public string FieldID
    {
      get => this.fieldID;
      set => this.fieldID = value;
    }

    public string FieldIDWithCoMortgagor
    {
      get
      {
        return this.comortgagorPair > 1 ? FieldPairParser.GetFieldIDForBorrowerPair(this.fieldID, this.comortgagorPair) : this.fieldID;
      }
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public LoanXDBField.FieldStatus FieldCurrentStatus
    {
      get => this.fieldCurrentStatus;
      set => this.fieldCurrentStatus = value;
    }

    public LoanXDBTableList.TableTypes FieldType
    {
      get => this.fieldType;
      set => this.fieldType = value;
    }

    public bool UseIndex
    {
      get => this.useIndex;
      set => this.useIndex = value;
    }

    public bool IndexIsChanged
    {
      get => this.indexIsChanged;
      set => this.indexIsChanged = value;
    }

    public bool DescriptionIsChanged
    {
      get => this.descriptionIsChanged;
      set => this.descriptionIsChanged = value;
    }

    public int ComortgagorPair
    {
      get => this.comortgagorPair;
      set => this.comortgagorPair = value;
    }

    public bool Auditable
    {
      get => this.auditable;
      set => this.auditable = value;
    }

    public bool AuditIsChanged
    {
      get => this.auditIsChanged;
      set => this.auditIsChanged = value;
    }

    public bool SizeIsChanged
    {
      get => this.sizeIsChanged;
      set => this.sizeIsChanged = value;
    }

    public bool IncludeInReports
    {
      get => this.includeInReports;
      set => this.includeInReports = value;
    }

    public int FieldTypeToInteger
    {
      get
      {
        if (this.fieldType == LoanXDBTableList.TableTypes.IsNumeric)
          return 1;
        return this.fieldType == LoanXDBTableList.TableTypes.IsDate ? 2 : 0;
      }
    }

    public int FieldNewSize
    {
      set => this.fieldNewSize = value;
      get => this.fieldNewSize;
    }

    public int FieldXRefID => this.fieldXRefId;

    public string FieldSizeToString
    {
      get
      {
        if (this.fieldType == LoanXDBTableList.TableTypes.IsNumeric)
          return "23, 10";
        return this.fieldSize == "" ? 64.ToString() : this.fieldSize;
      }
    }

    public bool RequiresRebuild
    {
      get
      {
        if (this.FieldCurrentStatus == LoanXDBField.FieldStatus.New)
          return true;
        if (this.FieldCurrentStatus != LoanXDBField.FieldStatus.Updated)
          return false;
        return this.SizeIsChanged || this.forceRebuild;
      }
    }

    public bool RequiresDatabaseChange
    {
      get
      {
        return this.fieldCurrentStatus == LoanXDBField.FieldStatus.New || this.fieldCurrentStatus == LoanXDBField.FieldStatus.Removed || this.fieldCurrentStatus != LoanXDBField.FieldStatus.None && (this.sizeIsChanged || this.indexIsChanged);
      }
    }

    private string getColumnCreationSql(bool createIndex)
    {
      string str1 = "VARCHAR";
      if (this.fieldType == LoanXDBTableList.TableTypes.IsNumeric)
        str1 = "DECIMAL";
      else if (this.fieldType == LoanXDBTableList.TableTypes.IsDate)
        str1 = "DATETIME";
      string str2 = "(" + this.FieldSizeToString + ")";
      if (this.fieldType == LoanXDBTableList.TableTypes.IsDate)
        str2 = "";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.getIndexDeletionSql());
      stringBuilder.AppendLine("IF EXISTS (SELECT * FROM syscolumns INNER JOIN sysobjects ON syscolumns.id = sysobjects.id WHERE sysobjects.id = object_id(N'[" + this.tableName + "]') AND OBJECTPROPERTY(sysobjects.id, N'IsUserTable') = 1 AND syscolumns.name = N'" + this.ColumnName + "')");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("     alter table " + this.tableName + " drop column " + this.ColumnName);
      stringBuilder.AppendLine("END");
      stringBuilder.AppendLine("alter table [" + this.tableName + "] add [" + this.ColumnName + "] [" + str1 + "] " + str2 + " NULL");
      if (this.useIndex & createIndex)
        stringBuilder.Append(this.getIndexCreationSql());
      return stringBuilder.ToString();
    }

    private string getIndexCreationSql()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("if not exists (select * FROM sysindexes where name = 'IX_" + this.tableName + "_" + this.ColumnName + "' and id = object_id(N'[" + this.tableName + "]'))");
      stringBuilder.AppendLine("  create index IX_" + this.tableName + "_" + this.ColumnName + " on " + this.tableName + "(" + this.ColumnName + ")");
      return stringBuilder.ToString();
    }

    private string getIndexDeletionSql()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("if exists (select * from sysindexes where name = 'IX_" + this.tableName + "_" + this.ColumnName + "' and id = object_id(N'[" + this.tableName + "]'))");
      stringBuilder.AppendLine("  drop index " + this.tableName + ".IX_" + this.tableName + "_" + this.ColumnName);
      return stringBuilder.ToString();
    }

    private string getColumnUpdateSql()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.getIndexDeletionSql());
      if (this.sizeIsChanged)
      {
        stringBuilder.AppendLine("IF EXISTS (SELECT * FROM syscolumns INNER JOIN sysobjects ON syscolumns.id = sysobjects.id WHERE sysobjects.id = object_id(N'[" + this.tableName + "]') AND OBJECTPROPERTY(sysobjects.id, N'IsUserTable') = 1 AND syscolumns.name = N'" + this.ColumnName + "')");
        stringBuilder.AppendLine("BEGIN");
        stringBuilder.AppendLine("    alter table " + this.tableName + " drop column " + this.ColumnName);
        stringBuilder.AppendLine("END");
        stringBuilder.Append(this.getColumnCreationSql(false));
      }
      if (this.useIndex)
        stringBuilder.Append(this.getIndexCreationSql());
      return stringBuilder.ToString();
    }

    private string getColumnDeletionSql()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.getIndexDeletionSql());
      stringBuilder.AppendLine("IF EXISTS (SELECT * FROM syscolumns INNER JOIN sysobjects ON syscolumns.id = sysobjects.id WHERE sysobjects.id = object_id(N'[" + this.tableName + "]') AND OBJECTPROPERTY(sysobjects.id, N'IsUserTable') = 1 AND syscolumns.name = N'" + this.ColumnName + "')");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("     alter table " + this.tableName + " drop column " + this.ColumnName);
      stringBuilder.AppendLine("END");
      return stringBuilder.ToString();
    }

    internal string ToSQLString()
    {
      if (this.FieldCurrentStatus == LoanXDBField.FieldStatus.New)
        return this.getColumnCreationSql(true);
      if (this.FieldCurrentStatus == LoanXDBField.FieldStatus.Updated)
        return this.getColumnUpdateSql();
      return this.FieldCurrentStatus == LoanXDBField.FieldStatus.Removed ? this.getColumnDeletionSql() : "";
    }

    public int FieldSize
    {
      set => this.fieldSize = value.ToString();
    }

    public int FieldSizeToInteger
    {
      get
      {
        if (this.fieldType != LoanXDBTableList.TableTypes.IsString)
          return 0;
        if (this.fieldSize == "")
          return 64;
        try
        {
          return int.Parse(this.fieldSize, NumberStyles.Any);
        }
        catch
        {
          return 64;
        }
      }
    }

    public LoanXDBAuditField GetAuditTrailField(AuditTrailDataElement dataElement)
    {
      return new LoanXDBAuditField(this, dataElement);
    }

    public LoanXDBAuditField[] GetAuditTrailFields()
    {
      return new List<LoanXDBAuditField>()
      {
        this.GetAuditTrailField(AuditTrailDataElement.ModifiedBy),
        this.GetAuditTrailField(AuditTrailDataElement.ModifiedByFirstName),
        this.GetAuditTrailField(AuditTrailDataElement.ModifiedByLastName),
        this.GetAuditTrailField(AuditTrailDataElement.ModifiedDate),
        this.GetAuditTrailField(AuditTrailDataElement.ModifiedValue),
        this.GetAuditTrailField(AuditTrailDataElement.PreviousValue)
      }.ToArray();
    }

    public override bool Equals(object obj)
    {
      LoanXDBField loanXdbField = obj as LoanXDBField;
      return obj != null && string.Compare(loanXdbField.ColumnName, this.ColumnName, true) == 0;
    }

    public override int GetHashCode() => this.ColumnName.ToLower().GetHashCode();

    public void WriteBytes(BinaryWriter bw)
    {
      bw.Write(this.tableName);
      bw.Write(this.fieldID);
      bw.Write(this.description);
      bw.Write(this.fieldSize);
      bw.Write(this.useIndex);
      bw.Write(this.includeInReports);
      bw.Write(this.auditable);
      bw.Write((int) this.fieldType);
      bw.Write(this.comortgagorPair);
      bw.Write(this.fieldXRefId);
      bw.Write((int) this.fieldCurrentStatus);
      bw.Write(this.indexIsChanged);
      bw.Write(this.auditIsChanged);
      bw.Write(this.sizeIsChanged);
      bw.Write(this.descriptionIsChanged);
      bw.Write(this.fieldNewSize);
      bw.Write(this.forceRebuild);
    }

    public static string GetDbColumnName(string uiFieldID)
    {
      FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(uiFieldID);
      bool flag = VirtualFields.GetField(fieldPairInfo.FieldID) != null;
      string str = fieldPairInfo.FieldID;
      for (int index = 0; index < " *!@#$%^&*()-+=~`'[]{}:;<>/?,|\"\\".Length; ++index)
        str = str.Replace(" *!@#$%^&*()-+=~`'[]{}:;<>/?,|\"\\"[index].ToString() ?? "", "");
      string dbColumnName = str.Replace(".", "_");
      if (!flag)
        dbColumnName = "_" + dbColumnName;
      if (fieldPairInfo.PairIndex > 1)
        dbColumnName = dbColumnName + "_P" + (object) fieldPairInfo.PairIndex;
      return dbColumnName;
    }

    public static LoanXDBTableList.TableTypes ColumnTypeToTableType(
      ReportingDatabaseColumnType columnType)
    {
      switch (columnType)
      {
        case ReportingDatabaseColumnType.Text:
          return LoanXDBTableList.TableTypes.IsString;
        case ReportingDatabaseColumnType.Numeric:
          return LoanXDBTableList.TableTypes.IsNumeric;
        case ReportingDatabaseColumnType.Date:
        case ReportingDatabaseColumnType.DateTime:
          return LoanXDBTableList.TableTypes.IsDate;
        default:
          throw new ArgumentException("Invalid column type specified");
      }
    }

    public enum FieldStatus
    {
      None,
      New,
      Updated,
      Removed,
    }
  }
}
