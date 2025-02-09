// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.HeadTableDef
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class HeadTableDef : RelationTableDef
  {
    private string uiKeyColumn = "";
    private bool recursiveSync;
    private string deliminator = "";
    private List<string> selfReferenceColumns = new List<string>();
    private string storedProcedureName = "";

    private HeadTableDef()
    {
    }

    public HeadTableDef(XmlElement tableDefNode)
      : base(tableDefNode)
    {
    }

    protected override void parseTableNode(XmlElement tableDefNode)
    {
      this.tableName = tableDefNode.GetAttribute("name");
      string attribute = tableDefNode.GetAttribute("updateName");
      if (!string.IsNullOrWhiteSpace(attribute))
        this.updateTableName = attribute;
      this.requireFileSystemSync = tableDefNode.GetAttribute("fileSystemSync") == "True";
      this.insertable = !(tableDefNode.GetAttribute("insertable") == "False");
      this.categoryName = tableDefNode.GetAttribute("category");
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
        case "UIKeyColumn":
          this.uiKeyColumn = elem.InnerText;
          if (elem.Attributes["recursiveSync"] == null)
            break;
          this.recursiveSync = elem.GetAttribute("recursiveSync").ToLower() == "true";
          if (!this.recursiveSync)
            break;
          this.deliminator = elem.GetAttribute("deliminator");
          this.storedProcedureName = elem.GetAttribute("storedProcedureName");
          break;
        case "SelfReferenceColumns":
          this.parseSelfReferenceColumns(elem);
          break;
        default:
          base.parseElement(elem);
          break;
      }
    }

    public string GetForeignKeyFilter(string filterStatement, string associatedTableName)
    {
      string columnName = (string) null;
      string primaryKeyColumn = (string) null;
      this.getForeignKeyMapping(associatedTableName, false, out columnName, out primaryKeyColumn);
      if (columnName == "" || primaryKeyColumn == "")
        return "";
      return "[" + primaryKeyColumn + "] in (Select [" + columnName + "] from [" + this.tableName + "] " + filterStatement + ")";
    }

    private void parseDateColumnsNode(XmlElement currentElem)
    {
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "DateTimeColumn" && !this.dateTimeColumns.Contains(xmlElement.InnerText))
          this.dateTimeColumns.Add(xmlElement.InnerText);
      }
    }

    private void parseSkipColumnsNode(XmlElement currentElem)
    {
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "SkipColumn")
        {
          bool flag = false;
          if (xmlElement.Attributes["updateOnly"] != null)
            flag = xmlElement.Attributes["updateOnly"].Value == "True";
          if (!this.skippedColumns.ContainsKey(xmlElement.InnerText))
            this.skippedColumns.Add(xmlElement.InnerText, flag);
        }
      }
    }

    private void parseSelfReferenceColumns(XmlElement currentElem)
    {
      XmlNodeList xmlNodeList = currentElem.SelectNodes("SelfReferenceColumn");
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return;
      foreach (XmlNode xmlNode in xmlNodeList)
        this.selfReferenceColumns.Add(xmlNode.InnerText);
    }

    public List<string> SelfReferenceColumns => this.selfReferenceColumns;

    public string UIKeyColumn => this.uiKeyColumn;

    public override string ToString() => this.tableName;

    public string GetUIKeyColumnXPath() => this.GetElementXPathWPlaceHolder(this.UIKeyColumn);

    public string GetUIKeyColumnXPathIgnorecase()
    {
      return this.GetElementXPathWPlaceHolderIgnorecase(this.UIKeyColumn);
    }

    public bool RecursiveSync => this.recursiveSync;

    public string Deliminator => this.deliminator;

    public string StoredProcedureName => this.storedProcedureName;

    public string GetColumnXPath(string columnName) => this.GetElementXPathWPlaceHolder(columnName);

    public string QueryForRetrievingUIKeyAndDBKeyColumns
    {
      get
      {
        return "Select [" + this.uiKeyColumn + "], [" + this.dbKeyColumn + "] from [" + this.tableName + "]";
      }
    }

    internal HeadTableDef CreateVirtualHeadTable(HeadRelationTableDef headRelationTable)
    {
      if (this.tableName != headRelationTable.VirtualHeadTableName)
        throw new Exception("Cannot create virtual head table due to mismatched table names: head table name '" + this.tableName + "' != head relation table's virtual head table name '" + headRelationTable.VirtualHeadTableName + "'");
      HeadTableDef virtualHeadTable = new HeadTableDef();
      virtualHeadTable.tableName = this.tableName;
      virtualHeadTable.updateTableName = this.updateTableName;
      virtualHeadTable.dateTimeColumns = this.dateTimeColumns;
      virtualHeadTable.bitColumns = this.bitColumns;
      virtualHeadTable.skippedColumns = this.skippedColumns;
      virtualHeadTable.calculatedFields = this.calculatedFields;
      virtualHeadTable.foreignKeys = this.foreignKeys;
      virtualHeadTable.dbKeyColumn = this.dbKeyColumn;
      virtualHeadTable.uiKeyColumn = this.uiKeyColumn;
      virtualHeadTable.dbKeyColumnType = this.dbKeyColumnType;
      virtualHeadTable.relationTables = this.relationTables;
      virtualHeadTable.associateTables = this.associateTables;
      if (!virtualHeadTable.relationTables.Contains(headRelationTable.tableName))
        virtualHeadTable.relationTables.Add(headRelationTable.tableName);
      foreach (string foreignKeyTable in headRelationTable.ForeignKeyTables)
      {
        if (!(foreignKeyTable == virtualHeadTable.TableName) && !virtualHeadTable.associateTables.Contains(foreignKeyTable) && !string.IsNullOrEmpty(foreignKeyTable))
          virtualHeadTable.associateTables.Add(foreignKeyTable);
      }
      return virtualHeadTable;
    }
  }
}
