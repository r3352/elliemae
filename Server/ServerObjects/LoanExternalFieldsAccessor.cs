// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.LoanExternalFieldsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class LoanExternalFieldsAccessor
  {
    private const string className = "LoanExternalFieldsAccessor�";
    public static readonly string TableName_LoanExternalFields = "[LoanExternalFields]";
    private const string StringFieldValueTableName = "LoanExternalFieldsStringValues�";
    private const string IntFieldValueTableName = "LoanExternalFieldsIntValues�";
    private const string DateFieldValueTableName = "LoanExternalFieldsDateValues�";

    public static List<LoanExternalFieldConfig> GetAllLoanExternalFieldsDefination()
    {
      ClientContext current = ClientContext.GetCurrent();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * From " + LoanExternalFieldsAccessor.TableName_LoanExternalFields + "(NOLOCK)");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      return current.Cache.Get<List<LoanExternalFieldConfig>>(nameof (LoanExternalFieldsAccessor), (Func<List<LoanExternalFieldConfig>>) (() => LoanExternalFieldsAccessor.dataSetToLoanExternalFieldConfig(dataSet)), CacheSetting.Disabled);
    }

    public static List<LoanExternalFieldConfig> GetLoanExternalFieldsDefinationForFieldIds(
      List<string> loanExternalFieldIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string[] array = loanExternalFieldIds.Select<string, string>((System.Func<string, string>) (x => SQL.Encode((object) x))).ToArray<string>();
      dbQueryBuilder.AppendLine("Select * From " + LoanExternalFieldsAccessor.TableName_LoanExternalFields + "(NOLOCK) le ");
      dbQueryBuilder.AppendLine("inner join INFORMATION_SCHEMA.COLUMNS sc on (Table_Name = le.FieldTypeTable and COLUMN_NAME = REPLACE(le.FieldID, '.', '_'))");
      dbQueryBuilder.AppendLine("where FieldId in (" + string.Join(",", array) + ")");
      return LoanExternalFieldsAccessor.dataSetToLoanExternalFieldConfig(dbQueryBuilder.ExecuteSetQuery());
    }

    public static LoanExternalFieldInfo GetLoanExternalFields(int xRefId)
    {
      List<LoanExternalFieldConfig> fieldsDefination = LoanExternalFieldsAccessor.GetAllLoanExternalFieldsDefination();
      if (fieldsDefination == null || !fieldsDefination.Any<LoanExternalFieldConfig>())
        return (LoanExternalFieldInfo) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from LoanExternalFieldsStringValues where XrefId = " + SQL.Encode((object) xRefId));
      dbQueryBuilder.AppendLine("select * from LoanExternalFieldsIntValues where XrefId = " + SQL.Encode((object) xRefId));
      dbQueryBuilder.AppendLine("select * from LoanExternalFieldsDateValues where XrefId = " + SQL.Encode((object) xRefId));
      return LoanExternalFieldsAccessor.ConvertDataSetToFieldInfo(dbQueryBuilder.ExecuteSetQuery(), fieldsDefination);
    }

    public static Dictionary<string, string> GetLoanExternalFields(string loanGuid)
    {
      List<LoanExternalFieldConfig> fieldsDefination = LoanExternalFieldsAccessor.GetAllLoanExternalFieldsDefination();
      if (fieldsDefination == null || !fieldsDefination.Any<LoanExternalFieldConfig>())
        return (Dictionary<string, string>) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select EXT_IncomeAnalyzerEligible,EXT_IncomeAnalyzerStatus,EXT_IncomeAnalyzerExceptions,EXT_CreditAnalyzerEligible,EXT_CreditAnalyzerStatus,EXT_CreditAnalyzerExceptions, Lastupdated from LoanExternalFieldsStringValues extString inner join LoanSummary ls on extString.XRefId = ls.XRefId where ls.Guid = " + SQL.Encode((object) loanGuid));
      return LoanExternalFieldsAccessor.MapExternalFieldsToStandardFields(LoanExternalFieldsAccessor.ConvertDataSetToFieldInfo(dbQueryBuilder.ExecuteSetQuery(), fieldsDefination));
    }

    private static Dictionary<string, string> MapExternalFieldsToStandardFields(
      LoanExternalFieldInfo loanExternalFieldInfo)
    {
      return loanExternalFieldInfo == null ? (Dictionary<string, string>) null : loanExternalFieldInfo.LoanExternalFields.Select<LoanExternalField, KeyValuePair<string, string>?>((System.Func<LoanExternalField, KeyValuePair<string, string>?>) (s =>
      {
        string fieldName = s.FieldName;
        string loanInternalValue = s.FieldValue.ToString();
        string key = string.Empty;
        switch (fieldName)
        {
          case "EXT.IncomeAnalyzerEligible":
            key = "ANALYZER.X1";
            loanInternalValue = Utils.ConvertToLoanInternalValue(loanInternalValue, FieldFormat.YN);
            break;
          case "EXT.IncomeAnalyzerExceptions":
            key = "ANALYZER.X2";
            int returnValue1;
            loanInternalValue = Utils.TryParseInt((object) loanInternalValue, out returnValue1) ? returnValue1.ToString() : (string) null;
            break;
          case "EXT.IncomeAnalyzerStatus":
            key = "ANALYZER.X3";
            break;
          case "EXT.CreditAnalyzerEligible":
            key = "ANALYZER.X4";
            loanInternalValue = Utils.ConvertToLoanInternalValue(loanInternalValue, FieldFormat.YN);
            break;
          case "EXT.CreditAnalyzerExceptions":
            key = "ANALYZER.X5";
            int returnValue2;
            loanInternalValue = Utils.TryParseInt((object) loanInternalValue, out returnValue2) ? returnValue2.ToString() : (string) null;
            break;
          case "EXT.CreditAnalyzerStatus":
            key = "ANALYZER.X6";
            break;
        }
        return loanInternalValue == null ? new KeyValuePair<string, string>?() : new KeyValuePair<string, string>?(new KeyValuePair<string, string>(key, loanInternalValue));
      })).Where<KeyValuePair<string, string>?>((System.Func<KeyValuePair<string, string>?, bool>) (w => w.HasValue)).ToDictionary<KeyValuePair<string, string>?, string, string>((System.Func<KeyValuePair<string, string>?, string>) (key => key.Value.Key), (System.Func<KeyValuePair<string, string>?, string>) (value => value.Value.Value));
    }

    public static void SaveLoanExternalFields(
      LoanExternalFieldInfo loanExternalFieldInfo,
      int xRefId)
    {
      List<LoanExternalFieldConfig> definationForFieldIds = LoanExternalFieldsAccessor.GetLoanExternalFieldsDefinationForFieldIds(loanExternalFieldInfo.LoanExternalFields.Select<LoanExternalField, string>((System.Func<LoanExternalField, string>) (a => a.FieldName)).ToList<string>());
      if (definationForFieldIds == null || !definationForFieldIds.Any<LoanExternalFieldConfig>())
        return;
      Dictionary<string, LoanExternalFieldConfig> externalFieldConfigDict = definationForFieldIds.ToDictionary<LoanExternalFieldConfig, string, LoanExternalFieldConfig>((System.Func<LoanExternalFieldConfig, string>) (key => key.FieldID), (System.Func<LoanExternalFieldConfig, LoanExternalFieldConfig>) (value => value));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (IGrouping<string, LoanExternalField> source in loanExternalFieldInfo.LoanExternalFields.GroupBy<LoanExternalField, string>((System.Func<LoanExternalField, string>) (g => externalFieldConfigDict[g.FieldName].FieldTypeTable)))
        dbQueryBuilder.AppendLine(LoanExternalFieldsAccessor.CreateUpsertQuery(LoanExternalFieldsAccessor.Transform(source.ToList<LoanExternalField>(), source.Key), externalFieldConfigDict, source.Key, xRefId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static List<LoanExternalField> Transform(List<LoanExternalField> oldData, string type)
    {
      switch (type)
      {
        case "LoanExternalFieldsStringValues":
          foreach (LoanExternalField loanExternalField in oldData)
            loanExternalField.FieldValue = string.IsNullOrEmpty(Convert.ToString(loanExternalField.FieldValue)) ? (object) null : (object) Convert.ToString(loanExternalField.FieldValue);
          return oldData;
        case "LoanExternalFieldsIntValues":
          foreach (LoanExternalField loanExternalField in oldData)
            loanExternalField.FieldValue = string.IsNullOrEmpty(Convert.ToString(loanExternalField.FieldValue)) ? (object) null : (object) Convert.ToInt32(loanExternalField.FieldValue);
          return oldData;
        case "LoanExternalFieldsDateValues":
          foreach (LoanExternalField loanExternalField in oldData)
            loanExternalField.FieldValue = string.IsNullOrEmpty(Convert.ToString(loanExternalField.FieldValue)) ? (object) null : (object) DateTimeOffset.Parse(loanExternalField.FieldValue.ToString()).DateTime;
          return oldData;
        default:
          return (List<LoanExternalField>) null;
      }
    }

    private static LoanExternalFieldInfo ConvertDataSetToFieldInfo(
      DataSet dataSet,
      List<LoanExternalFieldConfig> externalFieldConfigs)
    {
      if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0)
        return (LoanExternalFieldInfo) null;
      List<LoanExternalField> loanExternalFieldList = new List<LoanExternalField>();
      List<DateTime> source = new List<DateTime>();
      Dictionary<string, List<LoanExternalFieldConfig>> dictionary = externalFieldConfigs.GroupBy<LoanExternalFieldConfig, string>((System.Func<LoanExternalFieldConfig, string>) (g => g.FieldTypeTable)).ToDictionary<IGrouping<string, LoanExternalFieldConfig>, string, List<LoanExternalFieldConfig>>((System.Func<IGrouping<string, LoanExternalFieldConfig>, string>) (key => key.Key), (System.Func<IGrouping<string, LoanExternalFieldConfig>, List<LoanExternalFieldConfig>>) (value => value.ToList<LoanExternalFieldConfig>()));
      if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
      {
        loanExternalFieldList = LoanExternalFieldsAccessor.AddDataRowToFieldInfo(dataSet.Tables[0].Rows[0], dictionary["LoanExternalFieldsStringValues"], loanExternalFieldList);
        source.Add(SQL.DecodeDateTime(dataSet.Tables[0].Rows[0]["Lastupdated"]));
      }
      if (dataSet.Tables.Count > 1 && dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
      {
        loanExternalFieldList = LoanExternalFieldsAccessor.AddDataRowToFieldInfo(dataSet.Tables[1].Rows[0], dictionary["LoanExternalFieldsIntValues"], loanExternalFieldList);
        source.Add(SQL.DecodeDateTime(dataSet.Tables[1].Rows[0]["Lastupdated"]));
      }
      if (dataSet.Tables.Count > 2 && dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
      {
        loanExternalFieldList = LoanExternalFieldsAccessor.AddDataRowToFieldInfo(dataSet.Tables[2].Rows[0], dictionary["LoanExternalFieldsDateValues"], loanExternalFieldList);
        source.Add(SQL.DecodeDateTime(dataSet.Tables[2].Rows[0]["Lastupdated"]));
      }
      if (!loanExternalFieldList.Any<LoanExternalField>())
        return (LoanExternalFieldInfo) null;
      return new LoanExternalFieldInfo()
      {
        LoanExternalFields = loanExternalFieldList,
        LastUpdated = source.Max<DateTime>()
      };
    }

    private static List<LoanExternalField> AddDataRowToFieldInfo(
      DataRow dataRow,
      List<LoanExternalFieldConfig> loanExteranlFieldConfigs,
      List<LoanExternalField> result)
    {
      foreach (LoanExternalFieldConfig exteranlFieldConfig in loanExteranlFieldConfigs)
      {
        if (dataRow.Table.Columns.Contains(exteranlFieldConfig.ColumnName))
        {
          object obj = SQL.Decode(dataRow[exteranlFieldConfig.ColumnName]);
          if (!string.IsNullOrEmpty(Convert.ToString(obj)))
          {
            switch (exteranlFieldConfig.FieldTypeTable)
            {
              case "LoanExternalFieldsStringValues":
                obj = (object) Convert.ToString(obj);
                break;
              case "LoanExternalFieldsIntValues":
                obj = (object) Convert.ToInt32(obj);
                break;
              case "LoanExternalFieldsDateValues":
                obj = (object) ((DateTimeOffset) obj).UtcDateTime;
                break;
            }
            result.Add(new LoanExternalField()
            {
              FieldName = exteranlFieldConfig.FieldID,
              FieldValue = obj
            });
          }
        }
      }
      return result;
    }

    private static string CreateUpsertQuery(
      List<LoanExternalField> loanExternalFields,
      Dictionary<string, LoanExternalFieldConfig> loanExternalFieldConfigs,
      string loanExternalFieldsValueTable,
      int xRefId)
    {
      string str1 = string.Join(",", loanExternalFields.Select<LoanExternalField, string>((System.Func<LoanExternalField, string>) (x => loanExternalFieldConfigs[x.FieldName].ColumnName)).ToArray<string>());
      string str2 = string.Join(",", loanExternalFields.Select<LoanExternalField, string>((System.Func<LoanExternalField, string>) (x => SQL.Encode(x.FieldValue))).ToArray<string>());
      string str3 = string.Join(",", loanExternalFields.Select<LoanExternalField, string>((System.Func<LoanExternalField, string>) (x => loanExternalFieldConfigs[x.FieldName].ColumnName + "=" + SQL.Encode(x.FieldValue))).ToArray<string>());
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("MERGE " + loanExternalFieldsValueTable + " as tgt");
      stringBuilder.AppendLine(string.Format("USING (Select {0} as XRefId) as src", (object) xRefId));
      stringBuilder.AppendLine("ON (tgt.xRefId = src.XRefId)");
      stringBuilder.AppendLine("WHEN MATCHED THEN");
      stringBuilder.AppendLine("Update SET " + str3 + " , LastUpdated = " + SQL.Encode((object) DateTime.Now));
      stringBuilder.AppendLine("WHEN NOT MATCHED THEN");
      stringBuilder.AppendLine(string.Format("Insert ({0}, XRefId, LastUpdated) values ({1}, {2}, {3});", (object) str1, (object) str2, (object) xRefId, (object) SQL.Encode((object) DateTime.Now)));
      return stringBuilder.ToString();
    }

    public static string GetFolderID(string Guid)
    {
      ClientContext.GetCurrent();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select [EXT_DDAFolderId] From LoanExternalFieldsStringValues lmsd (NOLOCK) INNER JOIN [LoanSummary] ls on  lmsd.XrefId = ls.XRefID where Guid = '" + Guid + "'");
      return dbQueryBuilder.ExecuteScalar()?.ToString();
    }

    private static List<LoanExternalFieldConfig> dataSetToLoanExternalFieldConfig(DataSet dataSet)
    {
      if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0)
        return (List<LoanExternalFieldConfig>) null;
      List<LoanExternalFieldConfig> externalFieldConfig = new List<LoanExternalFieldConfig>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        externalFieldConfig.Add(LoanExternalFieldsAccessor.dataRowToUserPiplineView(row));
      return externalFieldConfig;
    }

    private static LoanExternalFieldConfig dataRowToUserPiplineView(DataRow viewRow)
    {
      string fieldTypeTable = (string) viewRow["FieldTypeTable"];
      return new LoanExternalFieldConfig((string) viewRow["FieldID"], fieldTypeTable, LoanExternalFieldsAccessor.parseFieldFormat(fieldTypeTable), (string) SQL.Decode(viewRow["Description"], (object) null), ((string) viewRow["FieldID"]).Replace('.', '_'));
    }

    private static FieldFormat parseFieldFormat(string value)
    {
      switch (value)
      {
        case "LoanExternalFieldsStringValues":
          return FieldFormat.STRING;
        case "LoanExternalFieldsIntValues":
          return FieldFormat.INTEGER;
        case "LoanExternalFieldsDateValues":
          return FieldFormat.DATETIME;
        default:
          return FieldFormat.STRING;
      }
    }
  }
}
