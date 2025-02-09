// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.TableDef
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public abstract class TableDef
  {
    protected string tableName;
    protected string updateTableName;
    protected List<string> dateTimeColumns = new List<string>();
    protected List<string> bitColumns = new List<string>();
    protected Dictionary<string, bool> skippedColumns = new Dictionary<string, bool>();
    protected List<CalculatedField> calculatedFields = new List<CalculatedField>();
    protected bool insertable = true;
    protected bool requireFileSystemSync;
    protected string categoryName;
    protected Dictionary<string, object> postSQLActionList = new Dictionary<string, object>();

    internal TableDef()
    {
    }

    public TableDef(XmlElement tableDefNode) => this.parseTableNode(tableDefNode);

    protected abstract void parseTableNode(XmlElement tableDefNode);

    protected void parseElement(XmlElement elem)
    {
      switch (elem.Name)
      {
        case "DateTimeColumns":
          this.parseDateColumnsNode(elem);
          break;
        case "BitColumns":
          this.parseBitColumnsNode(elem);
          break;
        case "SkipColumns":
          this.parseSkipColumnsNode(elem);
          break;
        case "CalculatedColumns":
          this.parseCalculatedColumnsNode(elem);
          break;
        case "PostSQLActions":
          this.parsePostSQLActionsNode(elem);
          break;
        default:
          throw new Exception(elem.Name + ": unrecognized element");
      }
    }

    public Dictionary<string, object> PostSQLActionList => this.postSQLActionList;

    public List<CalculatedField> CalculatedFields => this.calculatedFields;

    public bool RequireFileSystemSync => this.requireFileSystemSync;

    public Dictionary<string, bool> SkippedColumns
    {
      get
      {
        Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
        Dictionary<string, bool> skippedColumns = this.skippedColumns;
        foreach (CalculatedField calculatedField in this.calculatedFields)
        {
          if (!skippedColumns.ContainsKey(calculatedField.ColumnName))
            skippedColumns.Add(calculatedField.ColumnName, false);
        }
        return skippedColumns;
      }
    }

    public List<string> DateTimeColumns => this.dateTimeColumns;

    public List<string> BitColumns => this.bitColumns;

    public string TableName => this.tableName;

    public string UpdateTableName
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.updateTableName) ? this.updateTableName : this.TableName;
      }
    }

    public bool Insertable => this.insertable;

    public string CategoryName => this.categoryName;

    public string GetElementXPathWPlaceHolder(string elementName)
    {
      return this.GetElementXPath(elementName) + "[.= " + DataSyncUtils.ParaPlaceHolder + "]";
    }

    public string GetElementXPathWPlaceHolderIgnorecase(string elementName)
    {
      return this.GetElementXPath(elementName) + "[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=" + DataSyncUtils.ParaPlaceHolder + "]";
    }

    public string GetElementXPath(string elementName)
    {
      string elementXpath = "NewDataSet/" + this.tableName;
      if (elementName != "")
        elementXpath = elementXpath + "/" + elementName;
      return elementXpath;
    }

    private void parsePostSQLActionsNode(XmlElement currentElem)
    {
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "StoredProcedure")
        {
          string name = xmlElement.Attributes["name"].Value;
          List<SqlDbType> parameters = new List<SqlDbType>();
          foreach (XmlNode selectNode in xmlElement.SelectNodes("Parameter"))
          {
            switch (selectNode.Attributes["type"].Value)
            {
              case "Char":
                parameters.Add(SqlDbType.Char);
                continue;
              case "Int":
                parameters.Add(SqlDbType.Int);
                continue;
              case "NChar":
                parameters.Add(SqlDbType.NChar);
                continue;
              case "NText":
                parameters.Add(SqlDbType.NText);
                continue;
              case "NVarChar":
                parameters.Add(SqlDbType.NVarChar);
                continue;
              case "Text":
                parameters.Add(SqlDbType.Text);
                continue;
              case "VarChar":
                parameters.Add(SqlDbType.VarChar);
                continue;
              default:
                continue;
            }
          }
          if (this.postSQLActionList.ContainsKey("StoredProcedure"))
            ((List<StoredProcedureSignature>) this.postSQLActionList["StoredProcedure"]).Add(new StoredProcedureSignature(name, parameters));
          else
            this.postSQLActionList.Add("StoredProcedure", (object) new List<StoredProcedureSignature>((IEnumerable<StoredProcedureSignature>) new StoredProcedureSignature[1]
            {
              new StoredProcedureSignature(name, parameters)
            }));
        }
      }
    }

    private void parseDateColumnsNode(XmlElement currentElem)
    {
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "DateTimeColumn" && !this.dateTimeColumns.Contains(xmlElement.InnerText))
          this.dateTimeColumns.Add(xmlElement.InnerText);
      }
    }

    private void parseBitColumnsNode(XmlElement currentElem)
    {
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "BitColumn" && !this.bitColumns.Contains(xmlElement.InnerText))
          this.bitColumns.Add(xmlElement.InnerText);
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

    private void parseCalculatedColumnsNode(XmlElement currentElem)
    {
      foreach (XmlNode xmlNode in (XmlNode) currentElem)
      {
        if (xmlNode is XmlElement xmlElement && xmlElement.Name == "CalculatedColumn")
          this.calculatedFields.Add(new CalculatedField(xmlElement.Attributes["name"].Value, xmlElement.Attributes["source"].Value, xmlElement.Attributes["append"].Value));
      }
    }

    public override string ToString() => this.tableName;

    public override bool Equals(object obj)
    {
      return obj != null && obj is TableDef && ((TableDef) obj).tableName == this.tableName;
    }

    public override int GetHashCode() => base.GetHashCode();
  }
}
