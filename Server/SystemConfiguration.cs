// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SystemConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class SystemConfiguration
  {
    private const string className = "SystemConfiguration�";
    private const string customFieldsCacheName = "CachedCustomFields�";
    private const string lateChargeCacheName = "CachedLateCharge�";
    private const string mortgageFormsCacheName = "CachedMortgageForms�";
    private const string feeManagementSettingCacheName = "FeeManagementSetting�";
    private const string zipcodeSettingCacheName = "CachedZipcodeUserDefined�";
    private const string changeCircumstanceCacheName = "CachedChangeCircumstanceOptions�";
    private const string cust_TableName = "LoanCustomField�";
    private const string cust_FieldId = "fieldID�";
    private const string cust_Description = "description�";
    private const string cust_Format = "format�";
    private const string cust_MaxLength = "maxLength�";
    private const string cust_Calculation = "calculation�";

    public static string[] GetSystemSettingsNames()
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(ClientContext.GetCurrent().Settings.AppDataDir, "LoanSettings"));
      if (!directoryInfo.Exists)
        return new string[0];
      FileInfo[] files = directoryInfo.GetFiles();
      string[] systemSettingsNames = new string[files.Length];
      for (int index = 0; index < files.Length; ++index)
        systemSettingsNames[index] = files[index].Name;
      return systemSettingsNames;
    }

    public static BinaryObject GetHelocRateTable(string name)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(SystemConfiguration.getHelocTableFilePath(name)))
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
    }

    public static bool SaveHelocRateTable(string name, BinaryObject data)
    {
      string helocTableFilePath = SystemConfiguration.getHelocTableFilePath(name);
      try
      {
        using (DataFile dataFile = FileStore.CheckOut(helocTableFilePath, MutexAccess.Write))
        {
          if (data == null)
            dataFile.Delete();
          else
            dataFile.CheckIn(data);
        }
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public static bool DeleteHelocRateTable(string name)
    {
      string helocTableFilePath = SystemConfiguration.getHelocTableFilePath(name);
      try
      {
        using (DataFile dataFile = FileStore.CheckOut(helocTableFilePath, MutexAccess.Write))
        {
          if (dataFile.Exists)
            dataFile.Delete();
        }
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public static bool HelocTableObjectExists(string name)
    {
      return File.Exists(SystemConfiguration.getHelocTableFilePath(name));
    }

    public static BinaryObject GetVerifDaysSettings()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from VerifDays");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      StringBuilder stringBuilder = new StringBuilder();
      BinaryObject verifDaysSettings;
      if (dataTable.Rows.Count == 0)
      {
        verifDaysSettings = SystemConfiguration.GetSystemSettings("VerifDays", true);
        if (verifDaysSettings == null)
        {
          stringBuilder.AppendLine("<objdata><element name=\"root\">");
          stringBuilder.AppendLine("</element></objdata>");
          verifDaysSettings = new BinaryObject(stringBuilder.ToString(), Encoding.UTF8);
        }
        SystemConfiguration.SaveVerifDaysSettings(verifDaysSettings);
      }
      else
      {
        stringBuilder.AppendLine("<objdata><element name=\"root\">");
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          stringBuilder.AppendLine("<element name=\"" + row["Name"] + "\" >" + row["Days"] + "</element>");
        stringBuilder.AppendLine("</element></objdata>");
        verifDaysSettings = new BinaryObject(stringBuilder.ToString(), Encoding.UTF8);
      }
      return verifDaysSettings;
    }

    public static void SaveVerifDaysSettings(BinaryObject obj)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string xml = obj.ToString(Encoding.UTF8);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("objdata/element/element");
      dbQueryBuilder.AppendLine("Delete VerifDays");
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        string str = xmlNode.Attributes["name"].Value;
        int num = int.Parse(xmlNode.InnerText);
        dbQueryBuilder.AppendLine("Insert into VerifDays (Name, Days) Values (" + SQL.EncodeString(str) + ", " + (object) num + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static BinaryObject GetSystemSettings(string name, bool forceReadFromDisk = false)
    {
      if (name == "VerifDays" && !forceReadFromDisk)
        return SystemConfiguration.GetVerifDaysSettings();
      using (DataFile latestVersion = FileStore.GetLatestVersion(SystemConfiguration.getSettingsFilePath(name)))
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
    }

    public static void SaveSystemSettings(string name, BinaryObject data)
    {
      using (DataFile dataFile = FileStore.CheckOut(SystemConfiguration.getSettingsFilePath(name), MutexAccess.Write))
      {
        if (data == null)
        {
          if (!dataFile.Exists)
            return;
          dataFile.Delete();
        }
        else
          dataFile.CheckIn(data);
      }
    }

    public static string[] GetCustomDataObjectNames()
    {
      string customDataFolderPath = SystemConfiguration.getCustomDataFolderPath();
      ArrayList arrayList = new ArrayList();
      foreach (string file in Directory.GetFiles(customDataFolderPath))
        arrayList.Add((object) Path.GetFileName(file));
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public static BinaryObject GetCustomDataObject(string name)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(SystemConfiguration.getCustomDataFilePath(name)))
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
    }

    public static void SaveCustomDataObject(string name, BinaryObject data)
    {
      using (DataFile dataFile = FileStore.CheckOut(SystemConfiguration.getCustomDataFilePath(name), MutexAccess.Write))
      {
        if (data == null)
          dataFile.Delete();
        else
          dataFile.CheckIn(data);
      }
    }

    public static void AppendToCustomDataObject(string name, BinaryObject data)
    {
      using (DataFile dataFile = FileStore.CheckOut(SystemConfiguration.getCustomDataFilePath(name), MutexAccess.Write))
      {
        if (!dataFile.Exists)
          dataFile.CheckIn(data);
        else
          dataFile.Append(data, false);
      }
    }

    public static void SetSecondaryFields(ArrayList list, SecondaryFieldTypes type)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.Append("delete from SecondaryMarketingFields where optionType = " + (object) (int) type);
      dbQueryBuilder1.ExecuteNonQuery();
      if (list == null || list.Count == 0)
        return;
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("SecondaryMarketingFields");
      for (int index = 0; index < list.Count; ++index)
        dbQueryBuilder2.InsertInto(table, new DbValueList()
        {
          {
            "optionValues",
            (object) list[index].ToString()
          },
          {
            "optionType",
            (object) (int) type
          },
          {
            "optionOrder",
            (object) (index + 1)
          }
        }, true, false);
      dbQueryBuilder2.ExecuteNonQuery();
    }

    public static object[] GetAllSecondaryFields()
    {
      object[] allSecondaryFields = new object[6];
      ArrayList secondaryFields1 = SystemConfiguration.GetSecondaryFields(SecondaryFieldTypes.BaseRate);
      if (secondaryFields1 != null)
        allSecondaryFields[0] = (object) (string[]) secondaryFields1.ToArray(typeof (string));
      ArrayList secondaryFields2 = SystemConfiguration.GetSecondaryFields(SecondaryFieldTypes.BasePrice);
      if (secondaryFields2 != null)
        allSecondaryFields[1] = (object) (string[]) secondaryFields2.ToArray(typeof (string));
      ArrayList secondaryFields3 = SystemConfiguration.GetSecondaryFields(SecondaryFieldTypes.BaseMargin);
      if (secondaryFields3 != null)
        allSecondaryFields[2] = (object) (string[]) secondaryFields3.ToArray(typeof (string));
      ArrayList secondaryFields4 = SystemConfiguration.GetSecondaryFields(SecondaryFieldTypes.Payouts);
      if (secondaryFields4 != null)
        allSecondaryFields[3] = (object) (string[]) secondaryFields4.ToArray(typeof (string));
      ArrayList secondaryFields5 = SystemConfiguration.GetSecondaryFields(SecondaryFieldTypes.ProfitabilityOption);
      if (secondaryFields5 != null)
        allSecondaryFields[4] = (object) (string[]) secondaryFields5.ToArray(typeof (string));
      ArrayList secondaryFields6 = SystemConfiguration.GetSecondaryFields(SecondaryFieldTypes.LockTypeOption);
      if (secondaryFields6 != null)
        allSecondaryFields[5] = (object) (string[]) secondaryFields6.ToArray(typeof (string));
      return allSecondaryFields;
    }

    public static ArrayList GetSecondaryFields(SecondaryFieldTypes type)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select optionValues from SecondaryMarketingFields Where OptionType = " + (object) (int) type + " Order by optionOrder");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
        {
          TraceLog.WriteWarning(nameof (SystemConfiguration), "No Secondary Field data found in database.");
          return (ArrayList) null;
        }
        ArrayList secondaryFields = new ArrayList();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          string str = (string) dataRowCollection[index]["optionValues"];
          StringBuilder stringBuilder = new StringBuilder();
          foreach (int num in str)
          {
            if (num == 251)
              stringBuilder.Append('-');
            else
              stringBuilder.Append((char) num);
          }
          secondaryFields.Add((object) stringBuilder.ToString());
        }
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved secondary fields");
        return secondaryFields;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (ArrayList) null;
      }
    }

    public static DataTable GetLoanSynchronizationFields()
    {
      try
      {
        DbAccessManager.GetTable("LoanDataSynchronizationFieldSettings");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select [TradeType],[FieldId],[Editable],[SourceFieldDesc],[DestinationFieldDesc],[ParentFieldId],[Value],[Order] from [LoanDataSynchronizationFieldSettings] ORDER BY [TradeType], [Order]");
        DataTable synchronizationFields = dbQueryBuilder.ExecuteTableQuery();
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved loan synchronization fields from LoanDataSynchronizationFieldSettings.");
        return synchronizationFields;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (DataTable) null;
      }
    }

    public static void SetLoanSynchronizationFields(DataTable table)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.Append("delete from LoanDataSynchronizationFieldSettings");
      dbQueryBuilder1.ExecuteNonQuery();
      if (table == null || table.Rows.Count == 0)
        return;
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("LoanDataSynchronizationFieldSettings");
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        dbQueryBuilder2.InsertInto(table1, new DbValueList()
        {
          {
            "TradeType",
            row["TradeType"]
          },
          {
            "FieldId",
            row["FieldId"]
          },
          {
            "Editable",
            row["Editable"]
          },
          {
            "SourceFieldDesc",
            row["SourceFieldDesc"]
          },
          {
            "DestinationFieldDesc",
            row["DestinationFieldDesc"]
          },
          {
            "ParentFieldId",
            row["ParentFieldId"]
          },
          {
            "Value",
            row["Value"]
          },
          {
            "Order",
            row["Order"]
          }
        }, true, false);
      dbQueryBuilder2.ExecuteNonQuery();
    }

    public static void SetSecondarySecurityTypes(DataTable table)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.Append("delete from SecondarySecurityTypes");
      dbQueryBuilder1.ExecuteNonQuery();
      if (table == null || table.Rows.Count == 0)
        return;
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("SecondarySecurityTypes");
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        dbQueryBuilder2.InsertInto(table1, new DbValueList()
        {
          {
            "Name",
            row["Name"]
          },
          {
            "ProgramType",
            row["ProgramType"]
          },
          {
            "Term1",
            row["Term1"]
          },
          {
            "Term2",
            row["Term2"]
          }
        }, true, false);
      dbQueryBuilder2.ExecuteNonQuery();
    }

    public static DataTable GetSecondarySecurityTypes()
    {
      try
      {
        DbAccessManager.GetTable("SecondarySecurityTypes");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select Name, ProgramType, Term1, Term2 from SecondarySecurityTypes order by SecondarySecurityTypesID");
        DataTable secondarySecurityTypes = dbQueryBuilder.ExecuteTableQuery();
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved secondary security types.");
        return secondarySecurityTypes;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (DataTable) null;
      }
    }

    public static void SetFannieMaeProductNames(DataTable table)
    {
      if (table == null || table.Rows.Count == 0)
        return;
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.Append("delete from FannieMaeProductName");
      dbQueryBuilder1.ExecuteNonQuery();
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("FannieMaeProductName");
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        dbQueryBuilder2.InsertInto(table1, new DbValueList()
        {
          {
            "ProductName",
            row["ProductName"]
          },
          {
            "DisplayName",
            row["DisplayName"]
          },
          {
            "Description",
            row["Description"]
          }
        }, true, false);
      dbQueryBuilder2.ExecuteNonQuery();
    }

    public static DataTable GetFannieMaeProductNames()
    {
      try
      {
        DbAccessManager.GetTable("FannieMaeProductName");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select ProductName, DisplayName, Description from FannieMaeProductName");
        DataTable fannieMaeProductNames = dbQueryBuilder.ExecuteTableQuery();
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved fannie mae product names.");
        return fannieMaeProductNames;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (DataTable) null;
      }
    }

    public static DefaultFieldsInfo GetFhaDefaultFieldInfo()
    {
      DefaultFieldsInfo defaultFields = SystemConfiguration.GetDefaultFields("FHAConsumerChoiceFieldList");
      if (defaultFields != null && defaultFields.Map.Count == 0)
      {
        string filePath = SystemSettings.LocalAppDir + SystemSettings.DocDirRelPath + "FHAConsumerChoiceFieldList.xml";
        defaultFields.Map = Utils.LoanDefaultFileFromDocumentFolder(filePath, "FHAConsumerChoiceFieldList");
      }
      return defaultFields;
    }

    public static DefaultFieldsInfo GetDefaultFields(string rootNode)
    {
      DataFile latestVersion = FileStore.GetLatestVersion(ClientContext.GetCurrent().Settings.GetDataFilePath("Users\\" + rootNode + ".XML"));
      return latestVersion.Exists ? new DefaultFieldsInfo(latestVersion.GetText(), rootNode) : new DefaultFieldsInfo("", rootNode);
    }

    public static void SaveDefaultFields(DefaultFieldsInfo fieldInfo, string rootNode)
    {
      using (DataFile dataFile = FileStore.CheckOut(ClientContext.GetCurrent().Settings.GetDataFilePath("Users\\" + rootNode + ".XML"), MutexAccess.Write))
        dataFile.CheckIn(new BinaryObject(fieldInfo.ToXMLString(rootNode), Encoding.Default));
    }

    public static int[] GetChannelOption()
    {
      string companySetting = Company.GetCompanySetting("CHANNELOPTION", "DISPLAY");
      if (!((companySetting ?? "") != string.Empty))
        return (int[]) null;
      string[] strArray = companySetting.Split(',');
      int[] channelOption = new int[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        channelOption[index] = Utils.ParseInt((object) strArray[index]);
      return channelOption;
    }

    public static void MigrateLoanCustomFields(string userId)
    {
      string customFieldsFilePath = SystemConfiguration.getCustomFieldsFilePath();
      if (!File.Exists(customFieldsFilePath))
        return;
      string xmlData = "<CustomFieldList/>";
      using (DataFile latestVersion = FileStore.GetLatestVersion(customFieldsFilePath))
      {
        if (latestVersion.Exists)
          xmlData = latestVersion.GetText(Encoding.Default);
      }
      SystemConfiguration.UpdateLoanCustomFields(new CustomFieldsInfo(xmlData), userId);
      string str = customFieldsFilePath + ".bak";
      int num = 1;
      for (; File.Exists(str); str = customFieldsFilePath + ".bak." + (object) num)
        ++num;
      File.Move(customFieldsFilePath, str);
    }

    [PgReady]
    public static DateTime GetLoanCustomFieldsModificationDate()
    {
      string companySetting = Company.GetCompanySetting("LoanCustomFields", "LastModifiedTime");
      try
      {
        return string.IsNullOrEmpty(companySetting) ? DateTime.MinValue : Convert.ToDateTime(companySetting);
      }
      catch
      {
        return DateTime.MinValue;
      }
    }

    private static bool CacheEnabled(IClientContext context) => context.Cache.CacheSetting != 0;

    public static CustomFieldInfo GetLoanCustomField(string fieldID)
    {
      ClientContext current = ClientContext.GetCurrent();
      return SystemConfiguration.CacheEnabled((IClientContext) current) ? SystemConfiguration.GetLoanCustomFields().GetField(fieldID) : current.Cache.Get<CustomFieldInfo>("CachedCustomFields/" + fieldID, (Func<CustomFieldInfo>) (() => SystemConfiguration.GetLoanCustomFieldFromDB(fieldID)), CacheSetting.Low);
    }

    private static CustomFieldInfo GetLoanCustomFieldFromDB(string fieldID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("select F.*, A.[auditField], A.[auditData] from [LoanCustomField] F left outer join [LoanCustomFieldAudit] A on F.fieldID = A.fieldID where F.fieldID = " + SQL.Encode((object) fieldID));
        DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection1.Count == 0)
          return (CustomFieldInfo) null;
        string desc = (string) dataRowCollection1[0]["description"];
        FieldFormat fieldFormat = FieldFormatEnumUtil.NameToValue((string) dataRowCollection1[0]["format"]);
        int maxLength = (int) dataRowCollection1[0]["maxLength"];
        string calculation = (string) dataRowCollection1[0]["calculation"];
        CustomFieldInfo customFieldFromDb = new CustomFieldInfo(fieldID, desc, fieldFormat, maxLength, calculation);
        if (customFieldFromDb.IsAuditField())
        {
          string fieldId = (string) dataRowCollection1[0]["auditField"];
          AuditData auditData = FieldAuditSettings.StringToAuditData((string) dataRowCollection1[0]["auditData"]);
          customFieldFromDb.AuditSettings = new FieldAuditSettings(fieldId, auditData);
        }
        if (customFieldFromDb.IsListValued())
        {
          dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("select * from [LoanCustomFieldOption] where fieldID = " + SQL.Encode((object) fieldID) + " order by [sequenceNum] asc");
          DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute(DbTransactionType.None);
          List<string> stringList = new List<string>();
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection2)
            stringList.Add((string) dataRow["option"]);
          customFieldFromDb.Options = stringList.ToArray();
        }
        return customFieldFromDb;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), new Exception(ex.Message + "\r\n\r\n" + dbQueryBuilder.ToString()));
        return (CustomFieldInfo) null;
      }
    }

    public static CustomFieldsInfo GetLoanCustomFields()
    {
      ClientContext context = ClientContext.GetCurrent();
      string connStr = DbQueryBuilder.getConnectionString(DBReadReplicaFeature.Settings);
      return context.Cache.Get<CustomFieldsInfo>("CachedCustomFields", (Func<CustomFieldsInfo>) (() => LoanCustomFields.GetLoanCustomFields(connStr, context.Settings.DbServerType)), CacheSetting.Low);
    }

    public static string[] GetCustomFieldsByTemplates(
      string customFieldColumn,
      string table1,
      string table2,
      string table1PrimaryKeyColumn,
      string table2ForeignKeyColumn,
      string whereClauseColumn,
      string[] templates)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendFormat("SELECT {0} FROM {1} t1 INNER JOIN {2} t2 ON t1.[{3}] = t2.[{4}] WHERE t1.[{5}] IN ", (object) customFieldColumn, (object) table1, (object) table2, (object) table1PrimaryKeyColumn, (object) table2ForeignKeyColumn, (object) whereClauseColumn);
      dbQueryBuilder.Append("(" + string.Join(",", SQL.EncodeString(templates, true)) + ")");
      return SystemConfiguration.getCustomFieldsFromDataTable(dbQueryBuilder.ExecuteTableQuery(), customFieldColumn);
    }

    public static string[] GetCustomFieldsByTemplates(
      string customFieldColumn,
      string table,
      string whereClauseColumn,
      string[] templates)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendFormat("SELECT {0} FROM {1} t WHERE t.[{2}] IN ", (object) customFieldColumn, (object) table, (object) whereClauseColumn);
      dbQueryBuilder.Append("(" + string.Join(",", SQL.EncodeString(templates, true)) + ")");
      return SystemConfiguration.getCustomFieldsFromDataTable(dbQueryBuilder.ExecuteTableQuery(), customFieldColumn);
    }

    private static string[] getCustomFieldsFromDataTable(
      DataTable dtLoanCustomFields,
      string customFieldColumn)
    {
      string[] fieldsFromDataTable = (string[]) null;
      if (dtLoanCustomFields != null && dtLoanCustomFields.Rows.Count > 0)
      {
        fieldsFromDataTable = new string[dtLoanCustomFields.Rows.Count];
        for (int index = 0; index < dtLoanCustomFields.Rows.Count; ++index)
          fieldsFromDataTable[index] = SQL.DecodeString(dtLoanCustomFields.Rows[index][customFieldColumn]);
      }
      return fieldsFromDataTable;
    }

    public static void constructLoanCustomFieldUpdateSql(
      DbQueryBuilder sql,
      CustomFieldInfo fieldInfo,
      string userId)
    {
      string fieldId = fieldInfo.FieldID;
      string description = fieldInfo.Description;
      string name = FieldFormatEnumUtil.ValueToName(fieldInfo.Format);
      string str1 = string.Concat((object) fieldInfo.MaxLength);
      string calculation = fieldInfo.Calculation;
      string str2 = (string) null;
      string str3 = (string) null;
      if (fieldInfo.IsAuditField())
      {
        str2 = fieldInfo.AuditSettings.FieldID;
        str3 = fieldInfo.AuditSettings.AuditDataAsString;
      }
      DbTableInfo table1 = DbAccessManager.GetTable("LoanCustomField");
      sql.Upsert(table1, new DbValueList()
      {
        {
          "fieldID",
          (object) fieldId
        },
        {
          "description",
          (object) description
        },
        {
          "format",
          (object) name
        },
        {
          "maxLength",
          (object) str1
        },
        {
          "calculation",
          (object) calculation
        },
        {
          "CreatedUtcDate",
          (object) DateTimeOffset.UtcNow.ToString()
        },
        {
          "CreatedBy",
          (object) userId
        }
      }, new DbValueList()
      {
        {
          "description",
          (object) description
        },
        {
          "format",
          (object) name
        },
        {
          "maxLength",
          (object) str1
        },
        {
          "calculation",
          (object) calculation
        },
        {
          "ModifiedUtcDate",
          (object) DateTimeOffset.UtcNow.ToString()
        },
        {
          "ModifiedBy",
          (object) userId
        }
      }, new DbValueList()
      {
        {
          "fieldID",
          (object) fieldId
        }
      });
      if (fieldInfo.IsAuditField())
      {
        DbTableInfo table2 = DbAccessManager.GetTable("LoanCustomFieldAudit");
        DbValue key = new DbValue("fieldID", (object) fieldId);
        sql.Upsert(table2, new DbValueList()
        {
          {
            "auditField",
            (object) str2
          },
          {
            "auditData",
            (object) str3
          }
        }, key);
      }
      if (!fieldInfo.IsListValued())
        return;
      sql.AppendLine("DELETE FROM [LoanCustomFieldOption] WHERE [fieldID] = " + SQL.Encode((object) fieldId));
      for (int index = 0; index < fieldInfo.Options.Length; ++index)
        sql.AppendLine(string.Format("insert into [LoanCustomFieldOption] ([fieldID], [option], [sequenceNum]) values ({0}, {1}, {2})", (object) SQL.Encode((object) fieldId), (object) SQL.Encode((object) fieldInfo.Options[index]), (object) string.Concat((object) index)));
    }

    public static void UpdateLoanCustomField(CustomFieldInfo fieldInfo, string userId)
    {
      SystemConfiguration.UpdateLoanCustomFields(new CustomFieldsInfo(false)
      {
        fieldInfo
      }, userId);
    }

    public static void UpdateLoanCustomFields(CustomFieldsInfo fieldsInfo, string userId)
    {
      ClientContext context = ClientContext.GetCurrent();
      if (fieldsInfo == null || fieldsInfo.GetNonEmptyCount() <= 0)
        return;
      context.Cache.Put<CustomFieldsInfo>("CachedCustomFields", (Action) (() =>
      {
        // ISSUE: unable to decompile the method.
      }), (Func<CustomFieldsInfo>) (() => LoanCustomFields.GetLoanCustomFields(context.Settings.ConnectionString, context.Settings.DbServerType)), CacheSetting.Low);
      Company.SetCompanySetting("LoanCustomFields", "LastModifiedTime", DateTime.Now.ToString());
    }

    public static void DeleteLoanCustomField(string fieldID)
    {
      SystemConfiguration.DeleteLoanCustomFields(new string[1]
      {
        fieldID
      });
    }

    public static void DeleteLoanCustomFields(string[] fieldIDs)
    {
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<CustomFieldsInfo>("CachedCustomFields", (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        try
        {
          foreach (string fieldId in fieldIDs)
            dbQueryBuilder.AppendLine("delete from [LoanCustomField] where [fieldID] = " + SQL.Encode((object) fieldId));
          dbQueryBuilder.ExecuteNonQuery();
          foreach (string fieldId in fieldIDs)
            FieldSearchDbAccessor.DeleteFieldSearchInfo(fieldId, FieldSearchRuleType.LoanCustomFields);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (SystemConfiguration), new Exception(ex.Message + "\r\n\r\n" + dbQueryBuilder.ToString()));
        }
      }), (Func<CustomFieldsInfo>) (() => LoanCustomFields.GetLoanCustomFields(context.Settings.ConnectionString, context.Settings.DbServerType)), CacheSetting.Low);
      Company.SetCompanySetting("LoanCustomFields", "LastModifiedTime", DateTime.Now.ToString());
    }

    private static string getCustomFieldsFilePath()
    {
      return ClientContext.GetCurrent().Settings.GetDataFilePath("Users\\CUSTOMFIELDLIST.XML");
    }

    private static string getSettingsFilePath(string name)
    {
      if (Path.GetExtension(name) == "")
        name += ".xml";
      if (!DataFile.IsValidFilename(name))
        Err.Raise(TraceLevel.Error, nameof (SystemConfiguration), new ServerException("Invalid system settings file path \"" + name + "\""));
      return ClientContext.GetCurrent().Settings.GetDataFilePath("LoanSettings\\" + name);
    }

    private static string getCustomDataFolderPath()
    {
      string dataFolderPath = ClientContext.GetCurrent().Settings.GetDataFolderPath("CustomData");
      Directory.CreateDirectory(dataFolderPath);
      return dataFolderPath;
    }

    private static string getCustomDataFilePath(string name)
    {
      if (!DataFile.IsValidFilename(name))
        Err.Raise(TraceLevel.Error, nameof (SystemConfiguration), new ServerException("Invalid file name \"" + name + "\""));
      return Path.Combine(SystemConfiguration.getCustomDataFolderPath(), name);
    }

    private static string getHelocTableFilePath(string name)
    {
      if (Path.GetExtension(name) == "" || Path.GetExtension(name).ToLower() != ".xml")
        name += ".xml";
      if (!DataFile.IsValidFilename(name))
        Err.Raise(TraceLevel.Error, nameof (SystemConfiguration), new ServerException("Invalid HELOC Rate Table file path \"" + name + "\""));
      return ClientContext.GetCurrent().Settings.GetDataFilePath("HelocTables\\" + name);
    }

    public static void UpdateServicingMortgageStatements(Hashtable formNames)
    {
      ClientContext current = ClientContext.GetCurrent();
      DbQueryBuilder sql = (DbQueryBuilder) null;
      current.Cache.Put<Hashtable>("CachedMortgageForms", (Action) (() =>
      {
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Deleting and Updating Servicing Mortgage Form table from SQL...");
        try
        {
          sql = new DbQueryBuilder();
          sql.AppendLine("Delete from [ServicingMortgageForms]");
          if (formNames != null && formNames.Count > 0)
          {
            DbTableInfo table = DbAccessManager.GetTable("ServicingMortgageForms");
            foreach (DictionaryEntry formName in formNames)
              sql.InsertInto(table, new DbValueList()
              {
                {
                  "FormName",
                  (object) formName.Key.ToString()
                },
                {
                  "TemplatePath",
                  (object) formName.Value.ToString()
                }
              }, true, false);
          }
          sql.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfiguration), "Servicing Mortgage Form records cannot be updated. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfiguration), ex);
        }
      }), new Func<Hashtable>(SystemConfiguration.GetServicingMortgageStatementsFromDB), CacheSetting.Low);
    }

    public static Hashtable GetServicingMortgageStatements()
    {
      return ClientContext.GetCurrent().Cache.Get<Hashtable>("CachedMortgageForms", new Func<Hashtable>(SystemConfiguration.GetServicingMortgageStatementsFromDB), CacheSetting.Low);
    }

    private static Hashtable GetServicingMortgageStatementsFromDB()
    {
      try
      {
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieving Servicing Mortgage Statement table from SQL table...");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from [ServicingMortgageForms]");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
        {
          TraceLog.WriteWarning(nameof (SystemConfiguration), "No Servicing Mortgage Statement data found in database.");
          return (Hashtable) null;
        }
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          DataRow dataRow = dataRowCollection[index];
          string key = dataRow["FormName"].ToString();
          string str = SQL.DecodeString(dataRow["TemplatePath"]);
          if (!insensitiveHashtable.ContainsKey((object) key))
            insensitiveHashtable.Add((object) key, (object) str);
        }
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved Servicing Mortgage Statement table.");
        return insensitiveHashtable;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (Hashtable) null;
      }
    }

    public static void UpdateServicingLateCharge(Hashtable lateCharge)
    {
      ClientContext.GetCurrent().Cache.Put<Hashtable>("CachedLateCharge", (Action) (() =>
      {
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Updating Servicing Late Charge table from SQL...");
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("delete from ServicingLateCharge");
          if (lateCharge != null && lateCharge.Count > 0)
          {
            DbTableInfo table = DbAccessManager.GetTable("ServicingLateCharge");
            foreach (DictionaryEntry dictionaryEntry in lateCharge)
            {
              DbValueList values = new DbValueList();
              values.Add("state", (object) dictionaryEntry.Key.ToString());
              double[] numArray = (double[]) dictionaryEntry.Value;
              values.Add("minimum", (object) numArray[0]);
              values.Add("maximum", (object) numArray[1]);
              dbQueryBuilder.InsertInto(table, values, true, false);
            }
          }
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfiguration), "Late Charge records cannot be updated. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfiguration), ex);
        }
      }), new Func<Hashtable>(SystemConfiguration.GetServicingLateChargeFromDB), CacheSetting.Low);
    }

    public static Hashtable GetServicingLateCharge(string stateAbbr)
    {
      Hashtable servicingLateCharge = ClientContext.GetCurrent().Cache.Get<Hashtable>("CachedLateCharge", new Func<Hashtable>(SystemConfiguration.GetServicingLateChargeFromDB), CacheSetting.Low);
      if (servicingLateCharge == null || string.IsNullOrEmpty(stateAbbr) || !servicingLateCharge.ContainsKey((object) stateAbbr))
        return servicingLateCharge;
      return new Hashtable()
      {
        {
          (object) stateAbbr,
          servicingLateCharge[(object) stateAbbr]
        }
      };
    }

    private static Hashtable GetServicingLateChargeFromDB()
    {
      try
      {
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieving Servicing Late Charge table from SQL table...");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from ServicingLateCharge");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
        {
          TraceLog.WriteInfo(nameof (SystemConfiguration), "No Servicing Late Charge data found in database.");
          return (Hashtable) null;
        }
        Hashtable lateChargeFromDb = new Hashtable();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          DataRow dataRow = dataRowCollection[index];
          string key = dataRow["state"].ToString();
          double num1 = Convert.ToDouble(SQL.Decode(dataRow["minimum"], (object) 0));
          double num2 = Convert.ToDouble(SQL.Decode(dataRow["maximum"], (object) 0));
          if (!lateChargeFromDb.ContainsKey((object) key))
            lateChargeFromDb.Add((object) key, (object) new double[2]
            {
              num1,
              num2
            });
        }
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved Servicing Late Charge table");
        return lateChargeFromDb;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (Hashtable) null;
      }
    }

    public static void UpdateFeeManagement(FeeManagementSetting feeManagementSetting)
    {
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<FeeManagementSetting>("FeeManagementSetting", (Action) (() =>
      {
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Updating Fee Management List Setting from SQL...");
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("Delete from [FeeManagementList]");
          if (feeManagementSetting != null)
          {
            context.Settings.SetServerSetting("Policies.FeeListOption", (object) feeManagementSetting.CompanyOptIn);
            if (feeManagementSetting.NumberOfFees > 0)
            {
              DbTableInfo table = DbAccessManager.GetTable("FeeManagementList");
              foreach (FeeManagementRecord allFee in feeManagementSetting.GetAllFees())
              {
                DbValueList values = new DbValueList();
                values.Add("feeName", allFee.FeeName.Length > 100 ? (object) allFee.FeeName.Substring(0, 100) : (object) allFee.FeeName);
                values.Add("forSection700", allFee.For700 ? (object) "1" : (object) "0");
                values.Add("forSection800", allFee.For800 ? (object) "1" : (object) "0");
                values.Add("forSection900", allFee.For900 ? (object) "1" : (object) "0");
                values.Add("forSection1000", allFee.For1000 ? (object) "1" : (object) "0");
                values.Add("forSection1100", allFee.For1100 ? (object) "1" : (object) "0");
                values.Add("forSection1200", allFee.For1200 ? (object) "1" : (object) "0");
                values.Add("forSection1300", allFee.For1300 ? (object) "1" : (object) "0");
                values.Add("forSectionPC", allFee.ForPC ? (object) "1" : (object) "0");
                if (allFee.FeeSource != null)
                  values.Add("feeSource", (object) allFee.FeeSource);
                if (allFee.FeeNameInMavent != null)
                  values.Add("feeNameInMavent", allFee.FeeNameInMavent.Length > 100 ? (object) allFee.FeeNameInMavent.Substring(0, 100) : (object) allFee.FeeNameInMavent);
                if (allFee.FeeIDInMavent != null)
                  values.Add("feeIDInMavent", (object) allFee.FeeIDInMavent);
                if (allFee.FeeNameInUCD != null)
                  values.Add("feeNameInUCD", allFee.FeeNameInUCD.Length > 100 ? (object) allFee.FeeNameInUCD.Substring(0, 100) : (object) allFee.FeeNameInUCD);
                dbQueryBuilder.InsertInto(table, values, true, false);
              }
            }
          }
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfiguration), "Fee Management List Setting cannot be updated. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfiguration), ex);
        }
      }), new Func<FeeManagementSetting>(SystemConfiguration.GetFeeManagementFromDB), CacheSetting.Low);
    }

    public static FeeManagementSetting GetFeeManagement()
    {
      return ClientContext.GetCurrent().Cache.Get<FeeManagementSetting>("FeeManagementSetting", new Func<FeeManagementSetting>(SystemConfiguration.GetFeeManagementFromDB), CacheSetting.Low);
    }

    private static FeeManagementSetting GetFeeManagementFromDB()
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieving Fee List Setting from SQL table...");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from [FeeManagementList] ORDER BY [feeName]");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection.Count == 0)
        {
          TraceLog.WriteInfo(nameof (SystemConfiguration), "No Fee List Setting found in database.");
          return (FeeManagementSetting) null;
        }
        FeeManagementSetting managementFromDb = new FeeManagementSetting();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          DataRow dataRow = dataRowCollection[index];
          int int32 = Convert.ToInt32(dataRow["feeID"]);
          string feeName = dataRow["feeName"].ToString();
          bool for700 = SQL.DecodeBoolean(dataRow["forSection700"], false);
          bool for800 = SQL.DecodeBoolean(dataRow["forSection800"], false);
          bool for900 = SQL.DecodeBoolean(dataRow["forSection900"], false);
          bool for1000 = SQL.DecodeBoolean(dataRow["forSection1000"], false);
          bool for1100 = SQL.DecodeBoolean(dataRow["forSection1100"], false);
          bool for1200 = SQL.DecodeBoolean(dataRow["forSection1200"], false);
          bool for1300 = SQL.DecodeBoolean(dataRow["forSection1300"], false);
          bool forPC = SQL.DecodeBoolean(dataRow["forSectionPC"], false);
          string feeSource = dataRow["feeSource"] != null ? dataRow["feeSource"].ToString() : (string) null;
          string feeNameInMavent = dataRow["feeNameInMavent"] != null ? dataRow["feeNameInMavent"].ToString() : (string) null;
          string feeIDInMavent = dataRow["feeIDInMavent"] != null ? dataRow["feeIDInMavent"].ToString() : (string) null;
          string feeNameInUCD = dataRow["feeNameInUCD"] != null ? dataRow["feeNameInUCD"].ToString() : (string) null;
          managementFromDb.AddFee(new FeeManagementRecord(int32, feeName, for700, for800, for900, for1000, for1100, for1200, for1300, forPC, feeSource, feeNameInMavent, feeIDInMavent, feeNameInUCD));
        }
        managementFromDb.CompanyOptIn = (bool) current.Settings.GetServerSetting("Policies.FeeListOption");
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved Fee List Setting table");
        return managementFromDb;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (FeeManagementSetting) null;
      }
    }

    public static bool IsZipcodeUserDefinedDuplicated(
      string zipcode,
      string zipcodeExt,
      string state,
      string city)
    {
      try
      {
        ZipcodeUserDefinedList zipcodeUserDefinedList = SystemConfiguration.GetZipcodeUserDefinedList();
        if (zipcodeUserDefinedList != null)
        {
          TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved Zipcode Database Setting from Cache successfully.");
          return zipcodeUserDefinedList.IsZipcodeUserDefinedDuplicated(zipcode, zipcodeExt, state, city);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SystemConfiguration), "Cannot load Zipcode Database Setting from Cache. Error: " + ex.Message);
      }
      return false;
    }

    public static ZipcodeInfoUserDefined[] GetZipcodeUserDefined(string zipcode)
    {
      string empty = string.Empty;
      if ((zipcode ?? "") != string.Empty && zipcode.IndexOf("-") > -1)
      {
        string[] strArray = zipcode.Split('-');
        zipcode = strArray[0];
        empty = strArray[1];
      }
      try
      {
        ZipcodeUserDefinedList zipcodeUserDefinedList = SystemConfiguration.GetZipcodeUserDefinedList();
        if (zipcodeUserDefinedList != null)
        {
          TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved Zipcode Database Setting from Cache successfully.");
          return zipcodeUserDefinedList.GetZipcodeUserDefineds(zipcode, empty);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SystemConfiguration), "Cannot load Zipcode Database Setting from Cache. Error: " + ex.Message);
      }
      return (ZipcodeInfoUserDefined[]) null;
    }

    public static ZipcodeUserDefinedList GetZipcodeUserDefinedList()
    {
      return ClientContext.GetCurrent().Cache.Get<ZipcodeUserDefinedList>("CachedZipcodeUserDefined", new Func<ZipcodeUserDefinedList>(SystemConfiguration.GetZipcodeUserDefinedListFromDB), CacheSetting.Low);
    }

    public static ZipcodeUserDefinedList GetZipcodeUserDefinedListFromDB()
    {
      try
      {
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieving Zipcode Database Setting from SQL table...");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from [ZipcodeUserDefinedList] ORDER BY Zipcode, ZipcodeExt, StateName, CityName ASC");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
        {
          TraceLog.WriteInfo(nameof (SystemConfiguration), "No Zipcode Database Setting found in database.");
          return new ZipcodeUserDefinedList();
        }
        ZipcodeUserDefinedList definedListFromDb = new ZipcodeUserDefinedList();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          DataRow dataRow = dataRowCollection[index];
          string zipcode = dataRow["Zipcode"].ToString();
          string zipcodeExtension = dataRow["ZipcodeExt"].ToString();
          string city = dataRow["CityName"].ToString();
          string state = dataRow["StateName"].ToString();
          string county = dataRow["CountyName"] != null ? dataRow["CountyName"].ToString() : string.Empty;
          definedListFromDb.Add(new ZipcodeInfoUserDefined(zipcode, zipcodeExtension, city, state, county));
        }
        TraceLog.WriteVerbose(nameof (SystemConfiguration), "Retrieved Zipcode Database Setting table");
        return definedListFromDb;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfiguration), ex);
        return (ZipcodeUserDefinedList) null;
      }
    }

    public static LoanImportRequirement GetLoanImportRequirements()
    {
      LoanImportRequirement importRequirements = new LoanImportRequirement();
      Hashtable companySettings = Company.GetCompanySettings("LoanImportRequirements");
      if (companySettings != null && companySettings.Count > 0)
      {
        foreach (string key in (IEnumerable) companySettings.Keys)
        {
          try
          {
            string str = companySettings[(object) key] as string;
            switch (key.ToLower())
            {
              case "fanniemaeimportrequirementtype":
                importRequirements.FannieMaeImportRequirementType = (LoanImportRequirement.LoanImportRequirementType) Enum.Parse(typeof (LoanImportRequirement.LoanImportRequirementType), str, true);
                continue;
              case "templateforfanniemaeimport":
                importRequirements.TemplateForFannieMaeImport = str;
                continue;
              case "webcenterimportrequirementtype":
                importRequirements.WebCenterImportRequirementType = (LoanImportRequirement.LoanImportRequirementType) Enum.Parse(typeof (LoanImportRequirement.LoanImportRequirementType), str, true);
                continue;
              case "templateforwebcenterimport":
                importRequirements.TemplateForWebCenterImport = str;
                continue;
              default:
                continue;
            }
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(nameof (SystemConfiguration), "GetLoanImportRequirements: Loan Import Template Requirement setting error. Error: " + ex.Message);
          }
        }
      }
      return importRequirements;
    }

    public static void SetLoanImportRequirements(LoanImportRequirement loanImportRequirement)
    {
      if (loanImportRequirement == null)
        loanImportRequirement = new LoanImportRequirement();
      Dictionary<string, string> settings = new Dictionary<string, string>();
      settings["FannieMaeImportRequirementType"] = string.Concat((object) Utils.ParseInt((object) loanImportRequirement.FannieMaeImportRequirementType));
      settings["TemplateForFannieMaeImport"] = loanImportRequirement.TemplateForFannieMaeImport ?? "";
      settings["WebCenterImportRequirementType"] = string.Concat((object) Utils.ParseInt((object) loanImportRequirement.WebCenterImportRequirementType));
      settings["TemplateForWebCenterImport"] = loanImportRequirement.TemplateForWebCenterImport ?? "";
      Company.DeleteCompanySettings("LoanImportRequirements");
      Company.SetCompanySettings("LoanImportRequirements", settings);
    }

    public static List<CustomFieldInfo> CreateLoanCustomFields(List<CustomFieldInfo> fieldInfos)
    {
      CustomFieldsInfo fieldsInfo = new CustomFieldsInfo(false);
      foreach (CustomFieldInfo fieldInfo in fieldInfos)
        fieldsInfo.Add(fieldInfo);
      SystemConfiguration.CreateLoanCustomFields(fieldsInfo);
      return fieldInfos;
    }

    public static void CreateLoanCustomFields(CustomFieldsInfo fieldsInfo)
    {
      ClientContext context = ClientContext.GetCurrent();
      if (fieldsInfo == null || fieldsInfo.GetNonEmptyCount() <= 0)
        return;
      context.Cache.Put<CustomFieldsInfo>("CachedCustomFields", (Action) (() =>
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        try
        {
          SystemConfiguration.CustomFieldBulkCopy(fieldsInfo);
          SystemConfiguration.CreateLoanCustomFieldSql(sql, fieldsInfo);
          if (!string.IsNullOrEmpty(sql.ToString()))
          {
            sql.ExecuteNonQuery();
            sql.Reset();
          }
          FieldSearchDbAccessor.UpdateLoanCustomFieldsFieldSearch(fieldsInfo, false);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (SystemConfiguration), new Exception(ex.Message + "\r\n\r\n" + sql.ToString()));
        }
      }), (Func<CustomFieldsInfo>) (() => LoanCustomFields.GetLoanCustomFields(context.Settings.ConnectionString, context.Settings.DbServerType)), CacheSetting.Low);
      Company.SetCompanySetting("LoanCustomFields", "LastModifiedTime", DateTime.Now.ToString());
    }

    public static void CreateLoanCustomFieldSql(DbQueryBuilder sql, CustomFieldsInfo fieldInfos)
    {
      foreach (CustomFieldInfo fieldInfo in fieldInfos)
      {
        string fieldId1 = fieldInfo.FieldID;
        if (fieldInfo.IsAuditField())
        {
          string fieldId2 = fieldInfo.AuditSettings.FieldID;
          string auditDataAsString = fieldInfo.AuditSettings.AuditDataAsString;
          sql.AppendLine(string.Format("insert into [LoanCustomFieldAudit] (fieldID, auditField, auditData) values ({0}, {1}, {2})", (object) SQL.Encode((object) fieldId1), (object) SQL.Encode((object) fieldId2), (object) SQL.Encode((object) auditDataAsString)));
        }
        if (fieldInfo.IsListValued())
        {
          for (int index = 0; index < fieldInfo.Options.Length; ++index)
            sql.AppendLine(string.Format("insert into [LoanCustomFieldOption] ([fieldID], [option], [sequenceNum]) values ({0}, {1}, {2})", (object) SQL.Encode((object) fieldId1), (object) SQL.Encode((object) fieldInfo.Options[index]), (object) string.Concat((object) index)));
        }
      }
    }

    public static void CustomFieldBulkCopy(CustomFieldsInfo fieldValues)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DataTable dataTable = new DataTable("LoanCustomField");
      SystemConfiguration.createTableSchema(dataTable);
      foreach (CustomFieldInfo fieldValue in fieldValues)
      {
        DataRow row = dataTable.NewRow();
        row["fieldID"] = (object) fieldValue.FieldID;
        row["description"] = (object) fieldValue.Description;
        row["format"] = (object) FieldFormatEnumUtil.ValueToName(fieldValue.Format);
        row["maxLength"] = (object) string.Concat((object) fieldValue.MaxLength);
        row["calculation"] = (object) fieldValue.Calculation;
        dataTable.Rows.Add(row);
      }
      dataTable.AcceptChanges();
      dbQueryBuilder.DoBulkCopy("LoanCustomField", dataTable);
    }

    private static void createTableSchema(DataTable dataTable)
    {
      dataTable.Columns.Add(new DataColumn("fieldID", typeof (string)));
      dataTable.Columns.Add(new DataColumn("description", typeof (string)));
      dataTable.Columns.Add(new DataColumn("format", typeof (string)));
      dataTable.Columns.Add(new DataColumn("maxLength", typeof (int)));
      dataTable.Columns.Add(new DataColumn("calculation", typeof (string)));
    }
  }
}
