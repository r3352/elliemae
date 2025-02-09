// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.RelationTableDef
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class RelationTableDef : TableDef
  {
    protected List<ForeignKey> foreignKeys = new List<ForeignKey>();
    protected RelationTableDef.IDColumnType dbKeyColumnType;
    protected string dbKeyColumn = "";
    private int minIDValue;
    protected List<string> relationTables = new List<string>();
    protected List<string> associateTables = new List<string>();

    internal RelationTableDef()
    {
    }

    public RelationTableDef(XmlElement tableDefNode)
      : base(tableDefNode)
    {
    }

    public string GetManualIncrementSQL(string variable)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.minIDValue == 0)
      {
        stringBuilder.AppendLine("Select " + variable + " = max(" + this.dbKeyColumn + ") +1 from " + this.tableName);
      }
      else
      {
        stringBuilder.AppendLine("if exists (select 1 from " + this.tableName + " where " + (object) this.minIDValue + " > (select max(" + this.dbKeyColumn + ") +1 from " + this.tableName + "))");
        stringBuilder.AppendLine("begin");
        stringBuilder.AppendLine("select " + variable + " = " + (object) this.minIDValue);
        stringBuilder.AppendLine("end");
        stringBuilder.AppendLine("else");
        stringBuilder.AppendLine("begin");
        stringBuilder.AppendLine("select " + variable + " = max(" + this.dbKeyColumn + ") +1 from " + this.tableName);
        stringBuilder.AppendLine("end");
      }
      return stringBuilder.ToString();
    }

    protected override void parseTableNode(XmlElement tableDefNode)
    {
      this.tableName = tableDefNode.GetAttribute("name");
      this.requireFileSystemSync = tableDefNode.GetAttribute("fileSystemSync") == "True";
      foreach (XmlNode childNode in tableDefNode.ChildNodes)
      {
        if (childNode is XmlElement elem)
          this.parseElement(elem);
      }
    }

    protected new void parseElement(XmlElement elem)
    {
      switch (elem.Name)
      {
        case "DBKeyColumn":
          this.dbKeyColumn = elem.InnerText;
          switch ((elem.GetAttribute("type") ?? "").ToLower())
          {
            case "autoincrement":
              this.dbKeyColumnType = RelationTableDef.IDColumnType.AutoIncrement;
              return;
            case "manualincrement":
              this.dbKeyColumnType = RelationTableDef.IDColumnType.ManualIncrement;
              if (!(elem.GetAttribute("minValue") != ""))
                return;
              this.minIDValue = int.Parse(elem.GetAttribute("minValue"));
              return;
            case "guid":
              this.dbKeyColumnType = RelationTableDef.IDColumnType.Guid;
              return;
            case "other":
              this.dbKeyColumnType = RelationTableDef.IDColumnType.Other;
              return;
            default:
              throw new Exception("Unknown idColumn type");
          }
        case "RelatedTables":
          IEnumerator enumerator1 = elem.ChildNodes.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              if ((XmlNode) enumerator1.Current is XmlElement current)
              {
                switch (current.Name)
                {
                  case "Relations":
                    this.parseRelationsNode(current);
                    continue;
                  case "Associates":
                    this.parseAssociatesNode(current);
                    continue;
                  default:
                    continue;
                }
              }
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case "ForeignKeys":
          IEnumerator enumerator2 = elem.ChildNodes.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              if ((XmlNode) enumerator2.Current is XmlElement current && current.Name == "ForeignKey")
                this.parseForeignKeyNode(current);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        default:
          base.parseElement(elem);
          break;
      }
    }

    protected void parseRelationsNode(XmlElement RelationsNode)
    {
      foreach (XmlNode xmlNode in (XmlNode) RelationsNode)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "RelationTbl")
        {
          string innerText = xmlElement.InnerText;
          if (!this.relationTables.Contains(innerText))
            this.relationTables.Add(innerText);
        }
      }
    }

    protected void parseAssociatesNode(XmlElement currentElem)
    {
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "AssociateTbl")
        {
          string innerText = xmlElement.InnerText;
          if (!this.associateTables.Contains(innerText))
            this.associateTables.Add(innerText);
        }
      }
    }

    private void parseForeignKeyNode(XmlElement currentElem)
    {
      string foreignKeyColumn = currentElem.GetAttribute("columnName") ?? "";
      string isEntryColumn = currentElem.GetAttribute("isEntryColumn") ?? "";
      bool flag = false;
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "PrimaryKey")
        {
          string attribute1 = xmlElement.GetAttribute("keyTableName");
          string attribute2 = xmlElement.GetAttribute("keyColumnName");
          this.foreignKeys.Add(new ForeignKey(foreignKeyColumn, attribute1, attribute2, isEntryColumn));
          flag = true;
        }
      }
      if (flag)
        return;
      this.foreignKeys.Add(new ForeignKey(foreignKeyColumn, "", "", isEntryColumn));
    }

    public override string ToString() => this.tableName;

    public List<ForeignKey> ForeignKeys => this.foreignKeys;

    public string GetSQLStatementForPrimaryKeyTable(
      string primaryTableName,
      string filterStatement,
      TableDef[] relationTableDefs)
    {
      string columnName = (string) null;
      string primaryKeyColumn = (string) null;
      this.getForeignKeyMapping(primaryTableName, false, out columnName, out primaryKeyColumn);
      if (columnName == "" || primaryKeyColumn == "")
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      if (primaryTableName == "Roles" && filterStatement.Contains(DataSyncUtils.OtherRoleFilter) && this.tableName == "BR_DefaultDocumentAccessRules")
        stringBuilder.AppendLine("Select * from [" + this.tableName + "] where [" + columnName + "] in (Select [" + primaryKeyColumn + "] from [" + primaryTableName + "] " + filterStatement + " union select -1 from " + primaryTableName + " where 1=1)");
      else
        stringBuilder.AppendLine("Select * from [" + this.tableName + "] where [" + columnName + "] in (Select [" + primaryKeyColumn + "] from [" + primaryTableName + "] " + filterStatement + ")");
      if (relationTableDefs != null && relationTableDefs.Length != 0 && this.relationTables.Count > 0)
      {
        string filterStatement1 = "where [" + columnName + "] in (Select [" + primaryKeyColumn + "] from [" + primaryTableName + "] " + filterStatement + ")";
        foreach (string str in this.relationTables.ToArray())
        {
          foreach (TableDef relationTableDef1 in relationTableDefs)
          {
            if (relationTableDef1.TableName == str)
            {
              RelationTableDef relationTableDef2 = (RelationTableDef) relationTableDef1;
              stringBuilder.AppendLine(relationTableDef2.GetSQLStatementForPrimaryKeyTable(this.tableName, filterStatement1, relationTableDefs));
            }
          }
        }
      }
      return stringBuilder.ToString();
    }

    public string GetSQLStatementForNonEntryForeignKeyColumn(
      string primaryTableName,
      string filterStatement)
    {
      string columnName1 = (string) null;
      string primaryKeyColumn1 = (string) null;
      string columnName2 = (string) null;
      string primaryKeyColumn2 = (string) null;
      this.getForeignKeyMapping(primaryTableName, true, out columnName1, out primaryKeyColumn1);
      this.getForeignKeyMapping(primaryTableName, false, out columnName2, out primaryKeyColumn2);
      if (columnName1 == "" || primaryKeyColumn1 == "")
        return "";
      List<ForeignKey> foreignKeyColumn1 = this.GetForeignKeyByForeignKeyColumn(columnName1);
      string foreignKeyColumn2 = "";
      foreach (ForeignKey foreignKey in foreignKeyColumn1)
      {
        if (foreignKeyColumn2 == "")
          foreignKeyColumn2 = "Select * from [" + primaryTableName + "] where " + foreignKey.PrimaryKeyTableColumn + " in (Select " + columnName1 + " from [" + this.tableName + "] where [" + columnName2 + "] in (Select [" + primaryKeyColumn2 + "] from [" + primaryTableName + "] " + filterStatement + "))";
        else
          foreignKeyColumn2 = " Union Select * from [" + primaryTableName + "] where " + foreignKey.PrimaryKeyTableColumn + " in (Select " + columnName1 + " from [" + this.tableName + "] where [" + columnName2 + "] in (Select [" + primaryKeyColumn2 + "] from [" + primaryTableName + "] " + filterStatement + "))";
      }
      return foreignKeyColumn2;
    }

    public string GetForeignKeyFilter(
      string headTableName,
      string filterStatement,
      string associatedTableName)
    {
      string columnName1 = (string) null;
      string primaryKeyColumn1 = (string) null;
      this.getForeignKeyMapping(associatedTableName, false, out columnName1, out primaryKeyColumn1);
      if (columnName1 == "" || primaryKeyColumn1 == "")
        return "";
      string columnName2 = (string) null;
      string primaryKeyColumn2 = (string) null;
      this.getForeignKeyMapping(headTableName, false, out columnName2, out primaryKeyColumn2);
      string foreignKeyFilter = "";
      if (columnName2 != "" && primaryKeyColumn2 != "")
        foreignKeyFilter = "[" + primaryKeyColumn1 + "] in (Select [" + columnName1 + "] from [" + this.tableName + "] where [" + columnName2 + "] in (Select [" + primaryKeyColumn2 + "] from [" + headTableName + "] " + filterStatement + "))";
      return foreignKeyFilter;
    }

    public string GetExtendedForeignKeyFilter(string filterStatement, string associatedTableName)
    {
      string columnName = (string) null;
      string primaryKeyColumn = (string) null;
      this.getForeignKeyMapping(associatedTableName, false, out columnName, out primaryKeyColumn);
      if (columnName == "" || primaryKeyColumn == "")
        return "";
      return "[" + primaryKeyColumn + "] in (Select [" + columnName + "] from [" + this.tableName + "] where (" + filterStatement + "))";
    }

    public List<ForeignKey> GetForeignKeyByForeignKeyColumn(string columnName)
    {
      List<ForeignKey> foreignKeyColumn = new List<ForeignKey>();
      foreach (ForeignKey foreignKey in this.foreignKeys)
      {
        if (foreignKey.ForeignKeyColumn == columnName)
          foreignKeyColumn.Add(foreignKey);
      }
      return foreignKeyColumn;
    }

    public List<ForeignKey> GetForeignKeyByPrimaryKeyTable(string tableName)
    {
      List<ForeignKey> byPrimaryKeyTable = new List<ForeignKey>();
      foreach (ForeignKey foreignKey in this.foreignKeys)
      {
        if (foreignKey.PrimaryKeyTableName == tableName)
          byPrimaryKeyTable.Add(foreignKey);
      }
      return byPrimaryKeyTable;
    }

    public string[] GetForeignKeyPrimaryKeyTables()
    {
      List<string> stringList = new List<string>();
      foreach (ForeignKey foreignKey in this.foreignKeys)
      {
        if (!stringList.Contains(foreignKey.PrimaryKeyTableName))
          stringList.Add(foreignKey.PrimaryKeyTableName);
      }
      return stringList.ToArray();
    }

    public string[] GetForeignKeyColumns()
    {
      List<string> stringList = new List<string>();
      foreach (ForeignKey foreignKey in this.foreignKeys)
      {
        if (!stringList.Contains(foreignKey.ForeignKeyColumn))
          stringList.Add(foreignKey.ForeignKeyColumn);
      }
      return stringList.ToArray();
    }

    protected void getForeignKeyMapping(
      string primaryKeyTableName,
      bool nonEntryColumnOnly,
      out string columnName,
      out string primaryKeyColumn)
    {
      columnName = primaryKeyColumn = "";
      foreach (ForeignKey foreignKey in this.GetForeignKeyByPrimaryKeyTable(primaryKeyTableName))
      {
        if (columnName == "" && !(foreignKey.IsEntryColumn & nonEntryColumnOnly))
        {
          columnName = foreignKey.ForeignKeyColumn;
          primaryKeyColumn = foreignKey.PrimaryKeyTableColumn;
        }
        else if (!nonEntryColumnOnly && foreignKey.IsEntryColumn)
        {
          columnName = foreignKey.ForeignKeyColumn;
          primaryKeyColumn = foreignKey.PrimaryKeyTableColumn;
        }
      }
    }

    public List<string> RelationTableList => this.relationTables;

    public List<string> AssociateTableList => this.associateTables;

    public string DBKeyColumn => this.dbKeyColumn;

    public RelationTableDef.IDColumnType DBKeyColumnType => this.dbKeyColumnType;

    public string GetDBKeyColumnXPath() => this.GetElementXPathWPlaceHolder(this.DBKeyColumn);

    public enum IDColumnType
    {
      Other,
      AutoIncrement,
      ManualIncrement,
      Guid,
    }
  }
}
