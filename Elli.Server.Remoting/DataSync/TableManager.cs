// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.TableManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class TableManager
  {
    private Dictionary<string, TableDef> tblDefs = new Dictionary<string, TableDef>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private List<HeadTableDef> headTables;

    public HeadTableDef[] HeadTables => this.headTables.ToArray();

    public TableManager(string tableDefsXml)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(tableDefsXml);
      foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
      {
        if (childNode is XmlElement currentElem)
        {
          switch (currentElem.Name)
          {
            case nameof (HeadTables):
              this.parseHeadTablesNode(currentElem);
              continue;
            case "RelationTables":
              this.parseRelationTablesNode(currentElem);
              continue;
            case "HeadRelationTables":
              this.parseHeadRelationTablesNode(currentElem);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void parseHeadTablesNode(XmlElement currentElem)
    {
      this.headTables = new List<HeadTableDef>();
      foreach (XmlNode childNode in currentElem.ChildNodes)
      {
        if (childNode is XmlElement tableDefNode && tableDefNode.Name == "HeadTable")
        {
          HeadTableDef headTableDef = new HeadTableDef(tableDefNode);
          this.headTables.Add(headTableDef);
          this.tblDefs.Add(headTableDef.TableName, (TableDef) headTableDef);
        }
      }
    }

    private void parseRelationTablesNode(XmlElement currentElem)
    {
      foreach (XmlNode childNode in currentElem.ChildNodes)
      {
        if (childNode is XmlElement tableDefNode && tableDefNode.Name == "RelationTable")
        {
          RelationTableDef relationTableDef = new RelationTableDef(tableDefNode);
          this.tblDefs.Add(relationTableDef.TableName, (TableDef) relationTableDef);
        }
      }
    }

    private void parseHeadRelationTablesNode(XmlElement currentElem)
    {
      foreach (XmlNode childNode in currentElem.ChildNodes)
      {
        if (childNode is XmlElement tableDefNode && tableDefNode.Name == "HeadRelationTable")
        {
          HeadRelationTableDef relationTableDef = new HeadRelationTableDef(tableDefNode);
          this.headTables.Add((HeadTableDef) relationTableDef);
          this.tblDefs.Add(relationTableDef.TableName, (TableDef) relationTableDef);
        }
      }
    }

    public Dictionary<string, TableDef> TableDefinitions => this.tblDefs;

    public TableDef GetTableDef(string tableName)
    {
      return this.tblDefs.ContainsKey(tableName) ? this.tblDefs[tableName] : (TableDef) null;
    }

    public HeadTableDef GetHeadTableDef(string tableName)
    {
      return this.GetTableDef(tableName) as HeadTableDef;
    }

    public string[] GetTableNameList()
    {
      string[] array = new string[this.tblDefs.Keys.Count];
      this.tblDefs.Keys.CopyTo(array, 0);
      return array;
    }

    public TableDef[] GetRelatedTables(HeadTableDef headTable, bool includeSelf)
    {
      List<TableDef> tableDefList = new List<TableDef>();
      HeadTableDef[] relatedHeadTables = this.GetRelatedHeadTables(headTable, includeSelf);
      if (relatedHeadTables != null)
        tableDefList.AddRange((IEnumerable<TableDef>) relatedHeadTables);
      RelationTableDef[] relatedRelationTables = this.GetRelatedRelationTables((RelationTableDef) headTable);
      if (relatedRelationTables != null)
        tableDefList.AddRange((IEnumerable<TableDef>) relatedRelationTables);
      return tableDefList.ToArray();
    }

    public HeadTableDef[] GetRelatedHeadTables(HeadTableDef headTable, bool includeSelf)
    {
      List<HeadTableDef> headTableDefList = new List<HeadTableDef>();
      foreach (ForeignKey foreignKey in headTable.ForeignKeys.ToArray())
      {
        if (!(foreignKey.PrimaryKeyTableName == ""))
        {
          HeadTableDef tableDef = (HeadTableDef) this.GetTableDef(foreignKey.PrimaryKeyTableName);
          if (!headTableDefList.Contains(tableDef))
            headTableDefList.Add(tableDef);
        }
      }
      if (includeSelf)
        headTableDefList.Add(headTable);
      foreach (string associateTable in headTable.AssociateTableList)
      {
        HeadTableDef tableDef = (HeadTableDef) this.GetTableDef(associateTable);
        if (!headTableDefList.Contains(tableDef))
          headTableDefList.Add(tableDef);
      }
      return headTableDefList.ToArray();
    }

    public HeadTableDef[] GetRelatedHeadTables(string headTableName, bool includeSelf)
    {
      return this.GetRelatedHeadTables(this.GetHeadTableDef(headTableName), includeSelf);
    }

    public HeadTableDef[] GetMappingHeadTables(HeadTableDef headTable, bool includeSelf)
    {
      List<HeadTableDef> headTableDefList = new List<HeadTableDef>();
      foreach (ForeignKey foreignKey in headTable.ForeignKeys.ToArray())
      {
        if (!(foreignKey.PrimaryKeyTableName == ""))
        {
          HeadTableDef tableDef = (HeadTableDef) this.GetTableDef(foreignKey.PrimaryKeyTableName);
          if (!headTableDefList.Contains(tableDef))
            headTableDefList.Add(tableDef);
        }
      }
      if (includeSelf)
        headTableDefList.Add(headTable);
      foreach (string associateTable in headTable.AssociateTableList)
      {
        HeadTableDef tableDef1 = (HeadTableDef) this.GetTableDef(associateTable);
        if (!headTableDefList.Contains(tableDef1))
          headTableDefList.Add(tableDef1);
        if (tableDef1.ForeignKeys.Count != 0)
        {
          foreach (ForeignKey foreignKey in tableDef1.ForeignKeys)
          {
            if (!(foreignKey.PrimaryKeyTableName == ""))
            {
              HeadTableDef tableDef2 = (HeadTableDef) this.GetTableDef(foreignKey.PrimaryKeyTableName);
              if (!headTableDefList.Contains(tableDef2))
                headTableDefList.Add(tableDef2);
            }
          }
        }
      }
      return headTableDefList.ToArray();
    }

    public RelationTableDef[] GetRelatedRelationTables(RelationTableDef headTable)
    {
      List<RelationTableDef> relationTableDefList = new List<RelationTableDef>();
      foreach (string relationTable in headTable.RelationTableList)
      {
        RelationTableDef tableDef = (RelationTableDef) this.GetTableDef(relationTable);
        relationTableDefList.Add(tableDef);
        if (tableDef.RelationTableList.Count > 0)
          relationTableDefList.AddRange((IEnumerable<RelationTableDef>) this.GetRelatedRelationTables(tableDef));
      }
      return relationTableDefList.ToArray();
    }

    public HeadTableDef CreateVirtualHeadTable(HeadRelationTableDef headRelationTable)
    {
      return this.GetHeadTableDef(headRelationTable.VirtualHeadTableName).CreateVirtualHeadTable(headRelationTable);
    }
  }
}
