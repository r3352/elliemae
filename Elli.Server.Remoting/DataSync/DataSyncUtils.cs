// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.DataSyncUtils
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  internal class DataSyncUtils
  {
    private const string className = "DataSyncUtils";
    internal static string ParaPlaceHolder = "##Para##";
    internal static string OtherRoleFilter = "roleID = -1";
    private static string dbNullValue = "##DBNull.Value##";

    public static string ConstructFilter(HeadTableDef tableDef, string[] uiKeyValues)
    {
      string str = DataSyncUtils.ConstructFilter(tableDef.UIKeyColumn, uiKeyValues);
      string specialFilter = DataSyncUtils.getSpecialFilter(tableDef.TableName, uiKeyValues);
      if (specialFilter != "")
        str = str + " or " + specialFilter;
      return str;
    }

    public static string ConstructFilter(string uiKeyColumnName, string[] uiKeyValues)
    {
      for (int index = 0; index < uiKeyValues.Length; ++index)
        uiKeyValues[index] = uiKeyValues[index].Trim();
      StringBuilder stringBuilder = new StringBuilder();
      if (uiKeyValues != null && uiKeyValues.Length != 0)
      {
        stringBuilder.Append("where " + uiKeyColumnName + " in ('");
        string str = string.Join("', '", DataSyncUtils.formatDataBaseKeyCharacters(uiKeyValues));
        stringBuilder.Append(str);
        stringBuilder.Append("')");
      }
      return stringBuilder.ToString();
    }

    private static string getSpecialFilter(string tableName, string[] uiKeys)
    {
      string specialFilter = "";
      if (tableName == "Roles")
      {
        foreach (string uiKey in uiKeys)
        {
          if (uiKey == "Others")
          {
            specialFilter = DataSyncUtils.OtherRoleFilter;
            break;
          }
        }
      }
      return specialFilter;
    }

    public static XmlDocument TransformSourceToDestinationXml(
      string sourceDataXmlString,
      HeadTableDef[] relatedHeadTables,
      RelationTableDef[] relatedRelationTables,
      Dictionary<string, Dictionary<string, string>> keyMapping,
      HeadTableDef headTable)
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(sourceDataXmlString);
      foreach (HeadTableDef relatedHeadTable in relatedHeadTables)
      {
        DataSyncUtils.mapUniqueID(xmlDoc, (RelationTableDef) relatedHeadTable, keyMapping[relatedHeadTable.TableName]);
        DataSyncUtils.mapSelfReferenceID(xmlDoc, relatedHeadTable, keyMapping[relatedHeadTable.TableName]);
        DataSyncUtils.mapHeadTableForeignKey(xmlDoc, relatedHeadTable, keyMapping);
      }
      foreach (RelationTableDef relatedRelationTable in relatedRelationTables)
      {
        if (relatedRelationTable.DBKeyColumn != "")
          DataSyncUtils.mapUniqueID(xmlDoc, relatedRelationTable, new Dictionary<string, string>());
      }
      foreach (RelationTableDef relatedRelationTable in relatedRelationTables)
        DataSyncUtils.mapForeignKey(xmlDoc, relatedRelationTable, keyMapping);
      if (headTable.ForeignKeys.Count > 0)
        DataSyncUtils.mapHeadTableForeignKey(xmlDoc, headTable, keyMapping);
      DataSyncUtils.mapNewRecords(xmlDoc, relatedHeadTables, relatedRelationTables, keyMapping, headTable);
      DataSyncUtils.mapNewRecords(xmlDoc, relatedRelationTables);
      return xmlDoc;
    }

    public static List<FileSystemSyncRecord> GetFileSystemImportOption(
      string sourceDataXmlString,
      HeadTableDef[] relatedHeadTables,
      HeadTableDef headTable,
      ISession session)
    {
      List<FileSystemSyncRecord> systemImportOption = new List<FileSystemSyncRecord>();
      FileSystemManager fileSystemManager = new FileSystemManager("", sourceDataXmlString, session);
      if (headTable.RequireFileSystemSync)
        systemImportOption = fileSystemManager.GetImportOption((TableDef) headTable);
      List<FileSystemSyncRecord> systemSyncRecordList1 = new List<FileSystemSyncRecord>();
      foreach (HeadTableDef relatedHeadTable in relatedHeadTables)
      {
        List<FileSystemSyncRecord> systemSyncRecordList2 = new List<FileSystemSyncRecord>();
        if (relatedHeadTable.RequireFileSystemSync)
          systemSyncRecordList2 = fileSystemManager.GetImportOption((TableDef) relatedHeadTable);
        if (systemSyncRecordList2.Count > 0)
        {
          foreach (FileSystemSyncRecord systemSyncRecord in systemSyncRecordList2.ToArray())
          {
            if (!systemImportOption.Contains(systemSyncRecord))
              systemImportOption.Add(systemSyncRecord);
          }
        }
      }
      return systemImportOption;
    }

    public static Dictionary<string, List<string>> GetImportOption(
      string sourceDataXmlString,
      HeadTableDef[] relatedHeadTables,
      RelationTableDef[] relatedRelationTables,
      Dictionary<string, Dictionary<string, string>> keyMapping,
      HeadTableDef headTable)
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(sourceDataXmlString);
      foreach (HeadTableDef relatedHeadTable in relatedHeadTables)
      {
        DataSyncUtils.mapUniqueID(xmlDoc, (RelationTableDef) relatedHeadTable, keyMapping[relatedHeadTable.TableName]);
        DataSyncUtils.mapSelfReferenceID(xmlDoc, relatedHeadTable, keyMapping[relatedHeadTable.TableName]);
        DataSyncUtils.mapHeadTableForeignKey(xmlDoc, relatedHeadTable, keyMapping);
      }
      foreach (RelationTableDef relatedRelationTable in relatedRelationTables)
        DataSyncUtils.mapForeignKey(xmlDoc, relatedRelationTable, keyMapping);
      if (headTable.ForeignKeys.Count > 0)
        DataSyncUtils.mapHeadTableForeignKey(xmlDoc, headTable, keyMapping);
      return DataSyncUtils.getNewRecordList(xmlDoc, relatedHeadTables, relatedRelationTables, keyMapping, headTable);
    }

    private static Dictionary<string, List<string>> getNewRecordList(
      XmlDocument xmlDoc,
      HeadTableDef[] relatedHeadTables,
      RelationTableDef[] relatedRelationTables,
      Dictionary<string, Dictionary<string, string>> mapping,
      HeadTableDef headTable)
    {
      Dictionary<string, List<string>> newRecordList1 = new Dictionary<string, List<string>>();
      foreach (HeadTableDef relatedHeadTable in relatedHeadTables)
      {
        List<string> stringList = new List<string>();
        XmlNodeList xmlNodeList = xmlDoc.SelectNodes(relatedHeadTable.GetElementXPath(relatedHeadTable.DBKeyColumn));
        if (xmlNodeList != null && xmlNodeList.Count != 0)
        {
          foreach (XmlNode xmlNode1 in xmlNodeList)
          {
            if (!mapping[relatedHeadTable.TableName].ContainsValue(xmlNode1.InnerText))
            {
              XmlNode xmlNode2 = xmlNode1.ParentNode.SelectSingleNode(relatedHeadTable.UIKeyColumn);
              if (xmlNode2 != null)
              {
                if (relatedHeadTable.TableName != "FileResource")
                {
                  stringList.Add(xmlNode2.InnerText);
                }
                else
                {
                  string[] strArray = xmlNode2.InnerText.Split('|');
                  FileResourceManager.FileType fileType = (FileResourceManager.FileType) Enum.Parse(typeof (FileResourceManager.FileType), strArray[0]);
                  string str = strArray[3];
                  switch (fileType)
                  {
                    case FileResourceManager.FileType.LoanProgram:
                      str = "Loan Program:" + str;
                      break;
                    case FileResourceManager.FileType.ClosingCost:
                      str = "Closing Cost Template:" + str;
                      break;
                    case FileResourceManager.FileType.MiscData:
                      str = "Misc Data Template:" + str;
                      break;
                    case FileResourceManager.FileType.FormList:
                      str = "Form List Template:" + str;
                      break;
                    case FileResourceManager.FileType.DocumentSet:
                      str = "Document Set Template:" + str;
                      break;
                    case FileResourceManager.FileType.LoanTemplate:
                      str = "Loan Template:" + str;
                      break;
                    case FileResourceManager.FileType.CustomPrintForms:
                      str = "Custom Output Forms:" + str;
                      break;
                    case FileResourceManager.FileType.PrintGroups:
                      str = "Print Group:" + str;
                      break;
                    case FileResourceManager.FileType.Reports:
                      str = "Report Template:" + str;
                      break;
                    case FileResourceManager.FileType.BorrowerCustomLetters:
                      str = "Borrower Custom Letter:" + str;
                      break;
                    case FileResourceManager.FileType.BizCustomLetters:
                      str = "Business Custom Letter:" + str;
                      break;
                    case FileResourceManager.FileType.CampaignTemplate:
                      str = "Campaign Template:" + str;
                      break;
                    case FileResourceManager.FileType.DashboardTemplate:
                      str = "Dashboard Template:" + str;
                      break;
                    case FileResourceManager.FileType.DashboardViewTemplate:
                      str = "Dashboard View Template:" + str;
                      break;
                    case FileResourceManager.FileType.TaskSet:
                      str = "Task Set:" + str;
                      break;
                    case FileResourceManager.FileType.ConditionalApprovalLetter:
                      str = "Conditional Approval Letter:" + str;
                      break;
                    case FileResourceManager.FileType.SettlementServiceProviders:
                      str = "Settlement Service Provider:" + str;
                      break;
                    case FileResourceManager.FileType.AffiliatedBusinessArrangements:
                      str = "Affiliate Template:" + str;
                      break;
                  }
                  stringList.Add(str);
                }
              }
            }
          }
          if (stringList.Count > 0)
            newRecordList1.Add(relatedHeadTable.TableName, stringList);
        }
      }
      Dictionary<string, List<string>> newRecordList2 = new Dictionary<string, List<string>>();
      foreach (HeadTableDef relatedHeadTable in relatedHeadTables)
      {
        if (!relatedHeadTable.Insertable && newRecordList1.ContainsKey(relatedHeadTable.TableName) && !newRecordList2.ContainsKey(relatedHeadTable.TableName))
          newRecordList2.Add(relatedHeadTable.TableName, newRecordList1[relatedHeadTable.TableName]);
      }
      if (newRecordList2.Count > 0)
        throw new EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo("Missing Pre-required Data", newRecordList2, "", (string) null, new List<FileSystemSyncRecord>());
      return newRecordList1;
    }

    private static void mapUniqueID(
      XmlDocument xmlDoc,
      RelationTableDef tableDef,
      Dictionary<string, string> mapping)
    {
      string dbKeyColumnXpath = tableDef.GetDBKeyColumnXPath();
      foreach (XmlNode selectNode in xmlDoc.SelectNodes(dbKeyColumnXpath.Substring(0, dbKeyColumnXpath.IndexOf("["))))
      {
        string innerText = selectNode.InnerText;
        if (mapping.ContainsKey(innerText))
        {
          if (selectNode.Attributes["Mapped"] == null)
          {
            selectNode.InnerText = mapping[innerText];
            XmlAttribute attribute = selectNode.OwnerDocument.CreateAttribute("Mapped");
            attribute.Value = "true";
            selectNode.Attributes.Append(attribute);
          }
        }
        else
          selectNode.InnerText = "@" + selectNode.InnerText + "@";
      }
    }

    private static void mapSelfReferenceID(
      XmlDocument xmlDoc,
      HeadTableDef tableDef,
      Dictionary<string, string> mapping)
    {
      if (tableDef.SelfReferenceColumns == null || tableDef.SelfReferenceColumns.Count == 0)
        return;
      foreach (string selfReferenceColumn in tableDef.SelfReferenceColumns)
      {
        string columnXpath = tableDef.GetColumnXPath(selfReferenceColumn);
        foreach (string key in mapping.Keys)
        {
          XmlNode xmlNode = xmlDoc.SelectSingleNode(columnXpath.Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(key)));
          if (xmlNode != null && xmlNode.Attributes["Mapped"] == null)
          {
            xmlNode.InnerText = mapping[key];
            XmlAttribute attribute = xmlNode.OwnerDocument.CreateAttribute("Mapped");
            attribute.Value = "true";
            xmlNode.Attributes.Append(attribute);
          }
        }
      }
    }

    private static void mapHeadTableForeignKey(
      XmlDocument xmlDoc,
      HeadTableDef headTable,
      Dictionary<string, Dictionary<string, string>> mappings)
    {
      foreach (string foreignKeyColumn in headTable.GetForeignKeyColumns())
      {
        foreach (ForeignKey foreignKey in headTable.GetForeignKeyByForeignKeyColumn(foreignKeyColumn).ToArray())
        {
          if (mappings.ContainsKey(foreignKey.PrimaryKeyTableName))
          {
            Dictionary<string, string> mapping = mappings[foreignKey.PrimaryKeyTableName];
            foreach (string key in mapping.Keys)
            {
              XmlNodeList xmlNodeList = xmlDoc.SelectNodes(headTable.GetElementXPathWPlaceHolder(foreignKeyColumn).Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(key)));
              if (xmlNodeList != null && xmlNodeList.Count > 0)
              {
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                  if (xmlNode.Attributes["Mapped"] == null)
                  {
                    xmlNode.InnerText = mapping[key];
                    XmlAttribute attribute = xmlNode.OwnerDocument.CreateAttribute("Mapped");
                    attribute.Value = "true";
                    xmlNode.Attributes.Append(attribute);
                  }
                }
              }
            }
          }
        }
      }
    }

    private static void mapForeignKey(
      XmlDocument xmlDoc,
      RelationTableDef tableDef,
      Dictionary<string, Dictionary<string, string>> mappings)
    {
      foreach (string foreignKeyColumn in tableDef.GetForeignKeyColumns())
      {
        foreach (ForeignKey foreignKey in tableDef.GetForeignKeyByForeignKeyColumn(foreignKeyColumn).ToArray())
        {
          if (mappings.ContainsKey(foreignKey.PrimaryKeyTableName))
          {
            Dictionary<string, string> mapping = mappings[foreignKey.PrimaryKeyTableName];
            foreach (string key in mapping.Keys)
            {
              XmlNodeList xmlNodeList = xmlDoc.SelectNodes(tableDef.GetElementXPathWPlaceHolder(foreignKeyColumn).Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(key)));
              if (xmlNodeList != null && xmlNodeList.Count > 0)
              {
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                  if (xmlNode.Attributes["Mapped"] == null)
                  {
                    xmlNode.InnerText = mapping[key];
                    XmlAttribute attribute = xmlNode.OwnerDocument.CreateAttribute("Mapped");
                    attribute.Value = "true";
                    xmlNode.Attributes.Append(attribute);
                  }
                }
              }
            }
          }
        }
      }
    }

    public static void CopyCalculatedFieldToSource(
      List<CalculatedField> fieldList,
      XmlDocument xmlDoc,
      TableDef table)
    {
      foreach (CalculatedField field in fieldList)
      {
        foreach (XmlNode selectNode in xmlDoc.SelectNodes(table.GetElementXPath("")))
        {
          if (selectNode.SelectSingleNode(field.ColumnName) != null && selectNode.SelectSingleNode(field.SourceColumnName) != null && (field.Append || !(selectNode.SelectSingleNode(field.ColumnName).InnerText == "")))
            selectNode.SelectSingleNode(field.SourceColumnName).InnerText = selectNode.SelectSingleNode(field.ColumnName).InnerText;
        }
      }
    }

    private static void mapNewRecords(
      XmlDocument xmlDoc,
      HeadTableDef[] relatedHeadTables,
      RelationTableDef[] relatedRelationTables,
      Dictionary<string, Dictionary<string, string>> mapping,
      HeadTableDef headTable)
    {
      foreach (HeadTableDef relatedHeadTable1 in relatedHeadTables)
      {
        XmlNodeList xmlNodeList = xmlDoc.SelectNodes(relatedHeadTable1.GetElementXPath(relatedHeadTable1.DBKeyColumn));
        if (xmlNodeList != null && xmlNodeList.Count != 0)
        {
          int num = 0;
          foreach (XmlNode xmlNode in xmlNodeList)
          {
            if (!mapping[relatedHeadTable1.TableName].ContainsValue(xmlNode.InnerText))
            {
              string key = xmlNode.InnerText;
              if (key.StartsWith("@") && key.EndsWith("@"))
              {
                key = key.Substring(1, key.Length - 2);
                xmlNode.InnerText = key;
              }
              switch (relatedHeadTable1.DBKeyColumnType)
              {
                case RelationTableDef.IDColumnType.AutoIncrement:
                  xmlNode.InnerText = "@" + relatedHeadTable1.TableName + (object) num + "@";
                  ++num;
                  break;
                case RelationTableDef.IDColumnType.ManualIncrement:
                  xmlNode.InnerText = "@#" + relatedHeadTable1.TableName + (object) num + "@";
                  ++num;
                  break;
                case RelationTableDef.IDColumnType.Guid:
                  xmlNode.InnerText = Guid.NewGuid().ToString();
                  break;
              }
              mapping[relatedHeadTable1.TableName].Add(key, xmlNode.InnerText);
              foreach (TableDef relatedRelationTable in relatedRelationTables)
                DataSyncUtils.mapForeignKey(xmlDoc, (RelationTableDef) relatedRelationTable, mapping);
              foreach (HeadTableDef relatedHeadTable2 in relatedHeadTables)
                DataSyncUtils.mapHeadTableForeignKey(xmlDoc, relatedHeadTable2, mapping);
              DataSyncUtils.mapHeadTableSelfReferenceKey(xmlDoc, relatedHeadTable1, mapping);
            }
          }
        }
      }
      DataSyncUtils.mapHeadTableForeignKey(xmlDoc, headTable, mapping);
      DataSyncUtils.mapHeadTableSelfReferenceKey(xmlDoc, headTable, mapping);
    }

    private static void mapNewRecords(XmlDocument xmlDoc, RelationTableDef[] relatedRelationTables)
    {
      Dictionary<string, Dictionary<string, string>> mappings = new Dictionary<string, Dictionary<string, string>>();
      foreach (RelationTableDef relatedRelationTable in relatedRelationTables)
      {
        if (relatedRelationTable.DBKeyColumn != "")
        {
          XmlNodeList xmlNodeList = xmlDoc.SelectNodes(relatedRelationTable.GetElementXPath(relatedRelationTable.DBKeyColumn));
          if (xmlNodeList != null && xmlNodeList.Count != 0)
          {
            mappings.Add(relatedRelationTable.TableName, new Dictionary<string, string>());
            int num = 0;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
              string key = xmlNode.InnerText;
              if (key.StartsWith("@") && key.EndsWith("@"))
              {
                key = key.Substring(1, key.Length - 2);
                xmlNode.InnerText = key;
              }
              switch (relatedRelationTable.DBKeyColumnType)
              {
                case RelationTableDef.IDColumnType.AutoIncrement:
                  xmlNode.InnerText = "@" + relatedRelationTable.TableName + (object) num + "@";
                  ++num;
                  break;
                case RelationTableDef.IDColumnType.ManualIncrement:
                  xmlNode.InnerText = "@#" + relatedRelationTable.TableName + (object) num + "@";
                  ++num;
                  break;
                case RelationTableDef.IDColumnType.Guid:
                  xmlNode.InnerText = Guid.NewGuid().ToString();
                  break;
              }
              mappings[relatedRelationTable.TableName].Add(key, xmlNode.InnerText);
            }
          }
          else
            continue;
        }
        DataSyncUtils.mapForeignKey(xmlDoc, relatedRelationTable, mappings);
      }
    }

    private static void mapHeadTableSelfReferenceKey(
      XmlDocument xmlDoc,
      HeadTableDef headTable,
      Dictionary<string, Dictionary<string, string>> mappings)
    {
      if (headTable.SelfReferenceColumns == null || headTable.SelfReferenceColumns.Count == 0)
        return;
      foreach (string selfReferenceColumn in headTable.SelfReferenceColumns)
      {
        Dictionary<string, string> mapping = mappings[headTable.TableName];
        foreach (string key in mapping.Keys)
        {
          XmlNodeList xmlNodeList = xmlDoc.SelectNodes(headTable.GetElementXPathWPlaceHolder(selfReferenceColumn).Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(key)));
          if (xmlNodeList != null && xmlNodeList.Count > 0)
          {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
              if (xmlNode.Attributes["Mapped"] == null)
              {
                xmlNode.InnerText = mapping[key];
                XmlAttribute attribute = xmlNode.OwnerDocument.CreateAttribute("Mapped");
                attribute.Value = "true";
                xmlNode.Attributes.Append(attribute);
              }
            }
          }
        }
      }
    }

    public static string GetSqlUpdateStatement(
      XmlDocument doc,
      HeadTableDef headTable,
      HeadTableDef[] relatedHeadTables,
      RelationTableDef[] relatedRelationTables,
      Dictionary<string, DataColumnCollection> columnDefs)
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("-- Head Tables ----------------------------------\r\n");
      foreach (HeadTableDef relatedHeadTable in relatedHeadTables)
        DataSyncUtils.getTableUpdateStatement(sb, doc, relatedHeadTable, columnDefs[relatedHeadTable.TableName]);
      sb.AppendLine("-- Relation Tables ----------------------------------\r\n");
      foreach (RelationTableDef relatedRelationTable in relatedRelationTables)
        DataSyncUtils.getTableUpdateStatement(sb, doc, relatedRelationTable, columnDefs[relatedRelationTable.TableName], headTable);
      return sb.ToString();
    }

    private static string constructInsertStatement(
      string tableName,
      XmlNode node,
      Dictionary<string, bool> skipColumns)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.Append("Insert into [" + tableName + "] (");
      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (!skipColumns.ContainsKey(childNode.Name) || skipColumns[childNode.Name])
        {
          if (stringBuilder2.Length != 0)
          {
            stringBuilder2.Append(", ");
            stringBuilder3.Append(", ");
          }
          stringBuilder2.Append("[" + childNode.Name + "]");
          if (childNode.InnerText == DataSyncUtils.dbNullValue)
            stringBuilder3.Append("NULL");
          else if (childNode.InnerText.StartsWith("@#") && childNode.InnerText.EndsWith("@"))
            stringBuilder3.Append(childNode.InnerText);
          else if (childNode.InnerText.StartsWith("@") && childNode.InnerText.EndsWith("@"))
            stringBuilder3.Append(childNode.InnerText);
          else
            stringBuilder3.Append("'" + DataSyncUtils.formatDataBaseKeyCharacters(childNode.InnerText) + "'");
        }
      }
      stringBuilder2.Append(" ) values (" + stringBuilder3.ToString() + ")");
      stringBuilder1.Append(stringBuilder2.ToString());
      return stringBuilder1.ToString();
    }

    private static string constructUpdateStatement(
      string tableName,
      XmlNode node,
      Dictionary<string, bool> skipColumns)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder1.Append("Update [" + tableName + "] Set ");
      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (!skipColumns.ContainsKey(childNode.Name))
        {
          if (stringBuilder2.Length != 0)
            stringBuilder2.Append(", ");
          stringBuilder2.Append("[" + childNode.Name + "] = ");
          if (childNode.InnerText == DataSyncUtils.dbNullValue)
            stringBuilder2.Append("NULL");
          else if (childNode.InnerText.StartsWith("@#") && childNode.InnerText.EndsWith("@"))
            stringBuilder2.Append(childNode.InnerText);
          else if (childNode.InnerText.StartsWith("@") && childNode.InnerText.EndsWith("@"))
            stringBuilder2.Append(childNode.InnerText);
          else
            stringBuilder2.Append("'" + DataSyncUtils.formatDataBaseKeyCharacters(childNode.InnerText) + "'");
        }
      }
      stringBuilder1.Append(stringBuilder2.ToString());
      return stringBuilder1.ToString();
    }

    private static string constructInsertUpdateStatement(
      string tableName,
      string filter,
      XmlNode node,
      Dictionary<string, bool> skipColumns)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("If not exists (select * from [" + tableName + "] where " + filter + ")");
      stringBuilder.AppendLine("begin");
      stringBuilder.AppendLine("    " + DataSyncUtils.constructInsertStatement(tableName, node, skipColumns));
      stringBuilder.AppendLine("end");
      stringBuilder.AppendLine("else");
      stringBuilder.AppendLine("begin");
      stringBuilder.AppendLine("    " + DataSyncUtils.constructUpdateStatement(tableName, node, skipColumns) + " where " + filter);
      stringBuilder.AppendLine("end");
      return stringBuilder.ToString();
    }

    private static void getTableUpdateStatement(
      StringBuilder sb,
      XmlDocument xmlDoc,
      HeadTableDef tableDef,
      DataColumnCollection columnDefs)
    {
      string updateTableName = tableDef.UpdateTableName;
      sb.AppendLine("-- " + updateTableName + " --\r\n");
      XmlNodeList dbDataFormat = DataSyncUtils.convertToDbDataFormat(xmlDoc, (TableDef) tableDef, columnDefs);
      string dbKeyColumn = tableDef.DBKeyColumn;
      Dictionary<string, bool> skipColumns = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (tableDef.DBKeyColumnType == RelationTableDef.IDColumnType.AutoIncrement)
        skipColumns.Add(tableDef.DBKeyColumn, false);
      foreach (string key in tableDef.SkippedColumns.Keys)
        skipColumns.Add(key, tableDef.SkippedColumns[key]);
      foreach (XmlNode node in dbDataFormat)
      {
        string innerText = node.SelectSingleNode(dbKeyColumn).InnerText;
        if (!innerText.StartsWith("@" + updateTableName) && !innerText.StartsWith("@#" + updateTableName))
        {
          string filter = "[" + dbKeyColumn + "] = '" + DataSyncUtils.formatDataBaseKeyCharacters(innerText) + "'";
          sb.AppendLine(DataSyncUtils.constructInsertUpdateStatement(updateTableName, filter, node, skipColumns));
        }
        else if (innerText.StartsWith("@#" + updateTableName))
        {
          sb.AppendLine("declare " + innerText + " int");
          sb.AppendLine(tableDef.GetManualIncrementSQL(innerText));
          sb.AppendLine(DataSyncUtils.constructInsertStatement(updateTableName, node, skipColumns));
        }
        else if (innerText.StartsWith("@" + updateTableName))
        {
          sb.AppendLine("declare " + innerText + " int");
          sb.AppendLine(DataSyncUtils.constructInsertStatement(updateTableName, node, skipColumns));
          sb.AppendLine("Select " + innerText + " = SCOPE_IDENTITY()\r\n");
        }
      }
    }

    private static void getTableUpdateStatement(
      StringBuilder sb,
      XmlDocument xmlDoc,
      RelationTableDef tableDef,
      DataColumnCollection columnDefs,
      HeadTableDef headerTable)
    {
      string tableName = tableDef.TableName;
      sb.AppendLine("-- " + tableName + " --\r\n");
      StringBuilder stringBuilder1 = new StringBuilder();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      XmlNodeList dbDataFormat = DataSyncUtils.convertToDbDataFormat(xmlDoc, (TableDef) tableDef, columnDefs);
      Dictionary<string, bool> skipColumns = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (string key in tableDef.SkippedColumns.Keys)
        skipColumns.Add(key, tableDef.SkippedColumns[key]);
      string dbKeyColumn = tableDef.DBKeyColumn;
      if (dbKeyColumn != "" && tableDef.DBKeyColumnType == RelationTableDef.IDColumnType.AutoIncrement)
        skipColumns.Add(dbKeyColumn, false);
      foreach (XmlNode node in dbDataFormat)
      {
        string filter = "";
        string variable = "";
        if (dbKeyColumn != "")
          variable = node.SelectSingleNode(dbKeyColumn).InnerText;
        foreach (string foreignKeyColumn1 in tableDef.GetForeignKeyColumns())
        {
          List<ForeignKey> foreignKeyColumn2 = tableDef.GetForeignKeyByForeignKeyColumn(foreignKeyColumn1);
          if (node.SelectSingleNode(foreignKeyColumn1) == null)
            xmlDoc.CreateNode(XmlNodeType.Element, foreignKeyColumn1, (string) null);
          string rawData = node.SelectSingleNode(foreignKeyColumn1) == null ? DataSyncUtils.dbNullValue : node.SelectSingleNode(foreignKeyColumn1).InnerText;
          bool flag = false;
          if (foreignKeyColumn2.Count > 0)
          {
            foreach (ForeignKey foreignKey in foreignKeyColumn2.ToArray())
            {
              if (rawData.StartsWith("@" + foreignKey.PrimaryKeyTableName) || rawData.StartsWith("@#" + foreignKey.PrimaryKeyTableName))
                flag = true;
              if (foreignKey.IsEntryColumn && string.Compare(foreignKey.PrimaryKeyTableName, headerTable.TableName, true) == 0)
              {
                StringBuilder stringBuilder2 = new StringBuilder();
                if (rawData.StartsWith("@" + foreignKey.PrimaryKeyTableName) || rawData.StartsWith("@#" + foreignKey.PrimaryKeyTableName))
                  stringBuilder2.Append(string.Format("[{0}] = {1}", (object) foreignKeyColumn1, (object) rawData));
                else
                  stringBuilder2.Append(string.Format("[{0}] = '{1}'", (object) foreignKeyColumn1, (object) DataSyncUtils.formatDataBaseKeyCharacters(rawData)));
                string str;
                if (dictionary.ContainsKey(foreignKey.PrimaryKeyTableName))
                {
                  str = dictionary[foreignKey.PrimaryKeyTableName];
                  if (!str.Contains(stringBuilder2.ToString()))
                    str = str + " and " + stringBuilder2.ToString();
                }
                else
                  str = "Delete from [" + tableName + "] where " + stringBuilder2.ToString();
                stringBuilder2.Clear();
                if (!dictionary.ContainsKey(foreignKey.PrimaryKeyTableName))
                  dictionary.Add(foreignKey.PrimaryKeyTableName, str);
                else
                  dictionary[foreignKey.PrimaryKeyTableName] = str;
              }
            }
          }
          if (filter != "")
            filter += " and ";
          string str1 = filter + "[" + foreignKeyColumn1 + "]";
          filter = !flag ? (!(rawData == DataSyncUtils.dbNullValue) ? str1 + " = '" + DataSyncUtils.formatDataBaseKeyCharacters(rawData) + "'" : str1 + " is null") : str1 + " = " + rawData;
        }
        if (variable.StartsWith("@#" + tableName))
        {
          stringBuilder1.AppendLine("declare " + variable + " int");
          stringBuilder1.AppendLine(tableDef.GetManualIncrementSQL(variable));
          stringBuilder1.AppendLine(DataSyncUtils.constructInsertUpdateStatement(tableName, filter, node, skipColumns));
        }
        else if (variable.StartsWith("@" + tableName))
        {
          stringBuilder1.AppendLine("declare " + variable + " int");
          stringBuilder1.AppendLine(DataSyncUtils.constructInsertUpdateStatement(tableName, filter, node, skipColumns));
          stringBuilder1.AppendLine("Select " + variable + " = SCOPE_IDENTITY()\r\n");
        }
        else
          stringBuilder1.AppendLine(DataSyncUtils.constructInsertUpdateStatement(tableName, filter, node, skipColumns));
      }
      if (dbDataFormat.Count == 0)
      {
        foreach (string foreignKeyColumn3 in tableDef.GetForeignKeyColumns())
        {
          List<ForeignKey> foreignKeyColumn4 = tableDef.GetForeignKeyByForeignKeyColumn(foreignKeyColumn3);
          if (foreignKeyColumn4.Count > 0)
          {
            foreach (ForeignKey foreignKey in foreignKeyColumn4.ToArray())
            {
              if (foreignKey.IsEntryColumn && !string.IsNullOrEmpty(foreignKey.PrimaryKeyTableName))
              {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("NewDataSet/" + foreignKey.PrimaryKeyTableName);
                if (xmlNodeList != null && xmlNodeList.Count != 0)
                {
                  XmlNode xmlNode = xmlNodeList[0].SelectSingleNode(foreignKeyColumn3);
                  if (xmlNode != null)
                  {
                    if (tableDef != null && !foreignKey.PrimaryKeyTableName.Equals(headerTable.TableName))
                    {
                      IEnumerator enumerator = xmlNodeList[0].SelectNodes(foreignKeyColumn3).GetEnumerator();
                      try
                      {
                        while (enumerator.MoveNext())
                        {
                          string innerText = ((XmlNode) enumerator.Current).InnerText;
                          string str2;
                          if (dictionary.ContainsKey(foreignKey.PrimaryKeyTableName))
                            str2 = dictionary[foreignKey.PrimaryKeyTableName] + " and [" + foreignKeyColumn3 + "] = ";
                          else
                            str2 = "Delete from [" + tableName + "] where [" + foreignKeyColumn3 + "] = ";
                          string str3 = innerText.StartsWith("@" + foreignKey.PrimaryKeyTableName) || innerText.StartsWith("@#" + foreignKey.PrimaryKeyTableName) ? str2 + innerText : str2 + "'" + DataSyncUtils.formatDataBaseKeyCharacters(innerText) + "'";
                          if (!dictionary.ContainsKey(foreignKey.PrimaryKeyTableName))
                            dictionary.Add(foreignKey.PrimaryKeyTableName, str3);
                          else
                            dictionary[foreignKey.PrimaryKeyTableName] = str3;
                        }
                        break;
                      }
                      finally
                      {
                        if (enumerator is IDisposable disposable)
                          disposable.Dispose();
                      }
                    }
                    else
                    {
                      string innerText = xmlNode.InnerText;
                      string str4 = "";
                      StringBuilder stringBuilder3 = new StringBuilder();
                      if (innerText.StartsWith("@" + foreignKey.PrimaryKeyTableName) || innerText.StartsWith("@#" + foreignKey.PrimaryKeyTableName))
                        stringBuilder3.Append(string.Format("[{0}] = {1}", (object) foreignKeyColumn3, (object) innerText));
                      else
                        stringBuilder3.Append(string.Format("[{0}] = '{1}'", (object) foreignKeyColumn3, (object) DataSyncUtils.formatDataBaseKeyCharacters(innerText)));
                      if (dictionary.ContainsKey(foreignKey.PrimaryKeyTableName))
                      {
                        string str5 = dictionary[foreignKey.PrimaryKeyTableName];
                        str4 = str5.Contains(stringBuilder3.ToString()) ? "Delete from [" + tableName + "] where " + stringBuilder3.ToString() : str5 + " and " + stringBuilder3.ToString();
                      }
                      stringBuilder3.Clear();
                      if (!dictionary.ContainsKey(foreignKey.PrimaryKeyTableName))
                      {
                        dictionary.Add(foreignKey.PrimaryKeyTableName, str4);
                        break;
                      }
                      dictionary[foreignKey.PrimaryKeyTableName] = str4;
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }
      foreach (string str in dictionary.Values)
        sb.AppendLine(str);
      sb.AppendLine("");
      sb.Append(stringBuilder1.ToString());
    }

    private static XmlNodeList convertToDbDataFormat(
      XmlDocument xmlDoc,
      TableDef tableDef,
      DataColumnCollection columnDefs)
    {
      XmlNodeList dbDataFormat = xmlDoc.SelectNodes("NewDataSet/" + tableDef.TableName);
      if (tableDef.DateTimeColumns.Count == 0 && tableDef.BitColumns.Count == 0)
        return dbDataFormat;
      foreach (XmlNode xmlNode1 in dbDataFormat)
      {
        foreach (string dateTimeColumn in tableDef.DateTimeColumns)
        {
          XmlNode xmlNode2 = xmlNode1.SelectSingleNode(dateTimeColumn);
          if (xmlNode2 != null)
          {
            string str = DateTime.Parse(xmlNode2.InnerText).ToString("MM/dd/yyyy HH:mm:ss tt");
            xmlNode2.InnerText = str;
          }
        }
        foreach (string bitColumn in tableDef.BitColumns)
        {
          XmlNode xmlNode3 = xmlNode1.SelectSingleNode(bitColumn);
          if (xmlNode3 != null)
          {
            string lower = xmlNode3.InnerText.ToLower();
            xmlNode3.InnerText = lower == "true" ? "1" : "0";
          }
        }
      }
      foreach (XmlNode xmlNode in dbDataFormat)
      {
        foreach (DataColumn columnDef in (InternalDataCollectionBase) columnDefs)
        {
          if (xmlNode.SelectSingleNode(columnDef.ColumnName) == null)
          {
            XmlElement element = xmlDoc.CreateElement(columnDef.ColumnName);
            element.InnerText = DataSyncUtils.dbNullValue;
            xmlNode.AppendChild((XmlNode) element);
          }
        }
      }
      return dbDataFormat;
    }

    public static Dictionary<string, Dictionary<string, string>> CreateMapping(
      string sourceXml,
      HeadTableDef[] tables,
      DataSet destinationData)
    {
      Dictionary<string, Dictionary<string, string>> mapping = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      XmlDocument srcDataXmlDoc = new XmlDocument();
      srcDataXmlDoc.XmlResolver = (XmlResolver) null;
      srcDataXmlDoc.LoadXml(sourceXml);
      foreach (HeadTableDef table in tables)
      {
        Dictionary<string, string> dictionary = DataSyncUtils.populateMapping(srcDataXmlDoc, table, destinationData.Tables[table.TableName]);
        if (table.TableName == "Roles")
          dictionary.Add("-1", "-1");
        mapping.Add(table.TableName, dictionary);
      }
      return mapping;
    }

    private static Dictionary<string, string> populateMapping(
      XmlDocument srcDataXmlDoc,
      HeadTableDef tableDef,
      DataTable dstDT)
    {
      string uiKeyColumn = tableDef.UIKeyColumn;
      string dbKeyColumn = tableDef.DBKeyColumn;
      string str1 = tableDef.GetUIKeyColumnXPath();
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (DataRow row in (InternalDataCollectionBase) dstDT.Rows)
      {
        string input = string.Concat(row[uiKeyColumn]);
        string str2 = string.Concat(row[dbKeyColumn]);
        XmlNodeList xmlNodeList = srcDataXmlDoc.SelectNodes(str1.Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(input)));
        if (xmlNodeList.Count == 0)
        {
          str1 = tableDef.GetUIKeyColumnXPathIgnorecase();
          xmlNodeList = srcDataXmlDoc.SelectNodes(str1.Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(input.ToLower())));
        }
        if (xmlNodeList.Count != 0)
        {
          if (xmlNodeList.Count > 1)
            throw new Exception("There are multiple records with [" + uiKeyColumn + "] = '" + input + "' in the source database table '" + tableDef.TableName + "'.");
          string innerText = xmlNodeList[0].ParentNode.SelectSingleNode(dbKeyColumn).InnerText;
          try
          {
            dictionary.Add(innerText, str2);
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(nameof (DataSyncUtils), tableDef.TableName + " " + uiKeyColumn + " " + innerText + ": " + ex.Message);
          }
        }
      }
      return dictionary;
    }

    public static string GetEmbeddedXml(string resourceName)
    {
      string embeddedXml = (string) null;
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
      {
        if (manifestResourceStream != null)
        {
          using (StreamReader streamReader = new StreamReader(manifestResourceStream))
            embeddedXml = streamReader.ReadToEnd();
        }
      }
      return embeddedXml;
    }

    private static string formatXmlKeyCharacters(string rawData) => rawData.Replace("'", "&quot;");

    private static string formatDataBaseKeyCharacters(string rawData) => rawData.Replace("'", "''");

    private static string[] formatDataBaseKeyCharacters(string[] rawDataList)
    {
      List<string> stringList = new List<string>();
      foreach (string rawData in rawDataList)
        stringList.Add(rawData.Replace("'", "''"));
      return stringList.ToArray();
    }
  }
}
