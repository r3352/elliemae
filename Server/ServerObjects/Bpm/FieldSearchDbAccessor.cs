// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.FieldSearchDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common;
using Elli.Common.Fields;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class FieldSearchDbAccessor
  {
    private const string className = "FieldSearchDbAccessor�";
    private const string DATATABLE_STR = "|DDMDataTables|�";

    public static void CreateFieldSearchInfo(FieldSearchRule fieldSearchRule, EllieMae.EMLite.DataAccess.DbQueryBuilder sql = null)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("FS_Rules");
      bool flag = sql == null;
      DbValueList values = new DbValueList();
      values.Add("RuleId", (object) fieldSearchRule.RuleId);
      values.Add("RuleType", (object) (short) fieldSearchRule.RuleType);
      values.Add("Status", (object) (short) fieldSearchRule.Status);
      values.Add("RuleName", (object) fieldSearchRule.RuleName);
      values.Add("Identifier", (object) fieldSearchRule.Identifier);
      values.Add("ParentFSRuleId", (object) fieldSearchRule.ParentFSRuleId);
      if (flag)
        sql = (EllieMae.EMLite.DataAccess.DbQueryBuilder) new EllieMae.EMLite.Server.DbQueryBuilder();
      sql.InsertInto(table, values, true, false);
      if (flag)
        sql.Declare("@ruleId", "int");
      sql.SelectIdentity("@ruleId");
      if (flag)
      {
        sql.Select("@ruleId");
        object obj = sql.ExecuteScalar();
        fieldSearchRule.FsRuleId = Convert.ToInt32(obj);
      }
      FieldSearchDbAccessor.CreateFieldSearchFields(fieldSearchRule, flag ? (EllieMae.EMLite.DataAccess.DbQueryBuilder) null : sql);
    }

    public static void CreateFieldSearchFields(FieldSearchRule fieldSearchRule, EllieMae.EMLite.DataAccess.DbQueryBuilder sql = null)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("FS_Fields");
      bool flag = sql == null;
      if (flag)
        sql = (EllieMae.EMLite.DataAccess.DbQueryBuilder) new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (FieldSearchField fieldSearchField in fieldSearchRule.FieldSearchFields)
      {
        if (FieldSearchDbAccessor.GetFieldInfo(fieldSearchField.FieldId, FieldSearchDbAccessor.FieldInfoType.Description, EllieMae.EMLite.FieldSearch.FieldType.Standard) == null)
        {
          EllieMae.EMLite.FieldSearch.FieldType fieldType = EllieMae.EMLite.FieldSearch.FieldType.Unknown;
          switch (fieldSearchRule.RuleType)
          {
            case FieldSearchRuleType.LoanCustomFields:
              fieldType = EllieMae.EMLite.FieldSearch.FieldType.LoanCustom;
              break;
            case FieldSearchRuleType.BorrowerCustomFields:
            case FieldSearchRuleType.BusinessCustomFields:
              fieldType = EllieMae.EMLite.FieldSearch.FieldType.ContactCustom;
              break;
          }
          fieldSearchField.Description = FieldSearchDbAccessor.GetFieldDescription(fieldSearchField.FieldId, fieldType);
          fieldSearchField.Type = FieldSearchDbAccessor.GetFieldFormat(fieldSearchField.FieldId, fieldType);
          if (string.IsNullOrEmpty(fieldSearchField.Type) && !string.IsNullOrEmpty(fieldSearchField.Description))
            fieldSearchField.Type = fieldSearchField.Description;
          fieldSearchField.IsSystemField = false;
          if (string.IsNullOrEmpty(fieldSearchField.Description))
            fieldSearchField.Description = "";
          if (string.IsNullOrEmpty(fieldSearchField.Type))
            fieldSearchField.Type = "";
        }
        if (fieldSearchRule.FsRuleId > 0 | flag)
        {
          DbValueList values = new DbValueList();
          values.Add("FieldId", (object) fieldSearchField.FieldId);
          values.Add("FsRuleId", (object) fieldSearchRule.FsRuleId);
          values.Add("RelationshipType", (object) Convert.ToInt32((object) fieldSearchField.RelationshipType));
          if (!fieldSearchField.IsSystemField)
          {
            values.Add("IsSystemField", (object) 0);
            values.Add("Description", (object) fieldSearchField.Description);
            values.Add("Type", (object) fieldSearchField.Type);
          }
          sql.InsertInto(table, values, true, false);
        }
        else
        {
          sql.AppendLine("insert into " + table.Name);
          string str1 = "    ([FieldId], [FsRuleId], [RelationshipType";
          if (!fieldSearchField.IsSystemField)
            str1 += "], [IsSystemField], [Description], [Type";
          string text1 = str1 + "])";
          sql.AppendLine(text1);
          sql.AppendLine("values");
          DbColumnInfo dbColumnInfo1 = table["FieldId"];
          DbColumnInfo dbColumnInfo2 = table["Type"];
          DbColumnInfo dbColumnInfo3 = table["Description"];
          string str2 = "    (" + dbColumnInfo1.Encode((object) fieldSearchField.FieldId) + ", @ruleId, " + (object) Convert.ToInt32((object) fieldSearchField.RelationshipType);
          if (!fieldSearchField.IsSystemField)
            str2 = str2 + ", 0, " + dbColumnInfo3.Encode((object) fieldSearchField.Description) + ", " + dbColumnInfo2.Encode((object) fieldSearchField.Type);
          string text2 = str2 + ")";
          sql.AppendLine(text2);
        }
      }
      if (!flag || !(sql.ToString() != string.Empty))
        return;
      sql.ExecuteNonQuery();
    }

    public static FoundSearchFieldRules ReadFieldSearchInfo(
      List<string> fieldIds,
      List<FilterColumn> filterColumnList = null,
      List<SortColumn> sortColumnList = null,
      int pageSize = 0,
      int pageNumber = 0)
    {
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("FS_Rules");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("FS_Fields");
      FoundSearchFieldRules searchFieldRules = new FoundSearchFieldRules();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("select distinct(case when rules.ParentFSRuleId = 0 or rules.ParentFSRuleId is null THEN rules.FsRuleId else rules.ParentFSRuleId END) as FsRuleId, rules.RuleId, rules.RuleType, rules.Status, rules.RuleName");
      dbQueryBuilder1.AppendLine("from [FS_Rules] rules");
      for (int index = 0; index < fieldIds.Count; ++index)
        dbQueryBuilder1.AppendLine(string.Format("join (SELECT DISTINCT (CASE WHEN ParentFsRuleId = 0 OR ParentFSRuleId IS NULL THEN FS_Rules.FsRuleId ELSE FS_Rules.ParentFSRuleId END) AS FsRuleId, FieldId, RelationshipType FROM FS_Rules JOIN FS_Fields ON FS_Fields.FsRuleId = FS_Rules.FsRuleId) AS fields{0} on rules.FsRuleId = fields{1}.FsRuleId", (object) index, (object) index));
      DbValueList values = new DbValueList();
      for (int index = 0; index < fieldIds.Count; ++index)
        values.Add((DbValue) new DbFilterValue(table2, "FieldId", string.Format("fields{0}.FieldId", (object) index), (object) fieldIds[index]));
      if (filterColumnList != null)
      {
        foreach (FilterColumn filterColumn in filterColumnList)
          values.Add((DbValue) new DbFilterValue(table1, filterColumn.Name, string.Format("rules.{0}", (object) filterColumn.Name), (object) filterColumn.Values));
      }
      dbQueryBuilder1.Where(values);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      searchFieldRules.TotalCount = dbQueryBuilder2.Count(dbQueryBuilder1.ToString());
      DataTable page = new EllieMae.EMLite.Server.DbQueryBuilder().GetPage(dbQueryBuilder1.ToString(), pageSize, pageNumber, sortColumnList);
      Dictionary<int, FieldSearchRule> dictionary = new Dictionary<int, FieldSearchRule>();
      if (page != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) page.Rows)
        {
          FieldSearchRule fieldSearchRule = new FieldSearchRule(row);
          dictionary[fieldSearchRule.FsRuleId] = fieldSearchRule;
        }
      }
      if (dictionary.Values.Count > 0)
      {
        string str = "(" + string.Join<int>(",", (IEnumerable<int>) dictionary.Keys.ToArray<int>()) + ")";
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder3.AppendLine("select F.* from FS_Fields F inner JOIN FS_Rules R on F.FsRuleId = R.FsRuleId where (R.FsRuleId in " + str + " OR R.ParentFSRuleId in " + str + ") and [RelationshipType] = " + (object) Convert.ToInt32((object) RelationshipType.ConditionOf));
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder3.ExecuteTableQuery().Rows)
        {
          FieldSearchField fieldSearchField = new FieldSearchField(row);
          int int32 = Convert.ToInt32(row["FsRuleId"]);
          dictionary[int32].FieldSearchFields.Add(fieldSearchField);
        }
        searchFieldRules.Rules.AddRange((IEnumerable<FieldSearchRule>) dictionary.Values.ToList<FieldSearchRule>());
      }
      return searchFieldRules;
    }

    public static void DeleteFieldSearchInfo(
      string[] identifiersToKeep,
      FieldSearchRuleType ruleType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = "('" + string.Join("','", identifiersToKeep) + "')";
      dbQueryBuilder.AppendLine("delete F from FS_Fields F inner join FS_Rules R on r.FsRuleId = f.FsRuleId where r.RuleType = " + (object) Convert.ToInt32((object) ruleType) + " and r.Identifier NOT in " + str);
      dbQueryBuilder.AppendLine("delete from FS_Rules where RuleType = " + (object) Convert.ToInt32((object) ruleType) + " and Identifier NOT in " + str);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteFieldSearchInfo(int ruleId, FieldSearchRuleType ruleType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @fsRuleId int");
      dbQueryBuilder.AppendLine("select @fsRuleId = fsRuleId from [FS_Rules] where RuleId = " + (object) ruleId + " and RuleType = " + (object) Convert.ToInt32((object) ruleType));
      dbQueryBuilder.AppendLine("if @fsruleId is not null ");
      dbQueryBuilder.AppendLine("begin ");
      dbQueryBuilder.AppendLine("delete from [FS_Fields] where fsRuleId = @fsRuleId");
      dbQueryBuilder.AppendLine("delete from [FS_Rules] where fsRuleId = @fsRuleId");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteSetQuery();
    }

    public static void DeleteFieldSearchInfoWhithDataCheck(
      int ruleId,
      FieldSearchRuleType ruleType,
      string identifier)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @fsRuleId int");
      dbQueryBuilder.AppendLine("declare @ParentFSRuleId int");
      dbQueryBuilder.AppendLine("declare @newRuleId int");
      dbQueryBuilder.AppendLine("select @fsRuleId = fsRuleId, @ParentFSRuleId = ParentFSRuleId from [FS_Rules] where RuleId = " + (object) ruleId + " and RuleType = " + (object) Convert.ToInt32((object) ruleType) + " and identifier = '" + identifier.Replace("'", "''") + "'");
      dbQueryBuilder.AppendLine("if @fsruleId is not null ");
      dbQueryBuilder.AppendLine("begin ");
      dbQueryBuilder.AppendLine("delete from [FS_Fields] where fsRuleId = @fsRuleId");
      dbQueryBuilder.AppendLine("delete from [FS_Rules] where fsRuleId = @fsRuleId");
      dbQueryBuilder.If("@ParentFSRuleId is null");
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("select @newRuleId = fsRuleId from FS_Rules where FsRuleId = (select top 1 fsruleId from FS_Rules where ParentFSRuleId = @fsRuleId)");
      dbQueryBuilder.If("@newRuleId is not null");
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("update FS_Rules set ParentFSRuleId = null where fsRuleId = @newRuleId");
      dbQueryBuilder.AppendLine("update FS_Rules set ParentFSRuleId = @newRuleId where fsRuleId = @fsRuleId");
      dbQueryBuilder.End();
      dbQueryBuilder.End();
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteSetQuery();
    }

    public static void DeleteFieldSearchInfo(string identifier, FieldSearchRuleType ruleType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @fsRuleId int");
      dbQueryBuilder.AppendLine("select @fsRuleId = fsRuleId from [FS_Rules] where Identifier = '" + identifier.Replace("'", "''") + "' and RuleType = " + (object) Convert.ToInt32((object) ruleType));
      dbQueryBuilder.AppendLine("if @fsruleId is not null ");
      dbQueryBuilder.AppendLine("begin ");
      dbQueryBuilder.AppendLine("delete from [FS_Fields] where fsRuleId = @fsRuleId");
      dbQueryBuilder.AppendLine("delete from [FS_Rules] where fsRuleId = @fsRuleId");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteSetQuery();
    }

    public static void DeleteFieldSearchInfo()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [FS_Fields]");
      dbQueryBuilder.AppendLine("delete from [FS_Rules]");
      dbQueryBuilder.ExecuteSetQuery();
    }

    private static Dictionary<string, int> GetFSRuleIdsByIden(
      FieldSearchRuleType fsRuleType,
      List<string> ids)
    {
      string str = ids != null ? "inner join FN_Split('" + string.Join(",", (IEnumerable<string>) ids) + ",')  T on T.id = R.Identifier " : "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select R.FsRuleId, R.RuleId, R.Identifier from [FS_Rules] R " + str + "where RuleType = '" + (object) (int) fsRuleType + "'");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      Dictionary<string, int> fsRuleIdsByIden = new Dictionary<string, int>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string key = Convert.ToInt32(row["RuleId"]) == 0 ? Convert.ToString(row["Identifier"]) : Convert.ToString(row["RuleId"]);
        fsRuleIdsByIden[key] = Convert.ToInt32(row["FsRuleId"]);
      }
      return fsRuleIdsByIden;
    }

    public static Dictionary<string, int> GetFSRuleIds(FieldSearchRuleType fsRuleType)
    {
      return FieldSearchDbAccessor.GetFSRuleIdsByIden(fsRuleType, (List<string>) null);
    }

    public static int UpdateFieldSearchInfo(FieldSearchRule fieldSearchRule, EllieMae.EMLite.DataAccess.DbQueryBuilder sql)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("FS_Rules");
      bool flag = sql == null;
      if (flag)
        sql = (EllieMae.EMLite.DataAccess.DbQueryBuilder) new EllieMae.EMLite.Server.DbQueryBuilder();
      if (fieldSearchRule.FsRuleId > 0)
      {
        sql.AppendLine("delete from [FS_Fields] where fsRuleId = " + (object) fieldSearchRule.FsRuleId);
        sql.Update(table, new DbValueList()
        {
          {
            "RuleId",
            (object) fieldSearchRule.RuleId
          },
          {
            "RuleType",
            (object) (short) fieldSearchRule.RuleType
          },
          {
            "Status",
            (object) (short) fieldSearchRule.Status
          },
          {
            "RuleName",
            (object) fieldSearchRule.RuleName
          }
        }, new DbValueList()
        {
          {
            "FsRuleId",
            (object) fieldSearchRule.FsRuleId
          }
        });
        if (flag)
          sql.ExecuteNonQuery();
        if (fieldSearchRule.FieldSearchFields != null && fieldSearchRule.FieldSearchFields.Count > 0)
          FieldSearchDbAccessor.CreateFieldSearchFields(fieldSearchRule, flag ? (EllieMae.EMLite.DataAccess.DbQueryBuilder) null : sql);
      }
      else
        FieldSearchDbAccessor.CreateFieldSearchInfo(fieldSearchRule, flag ? (EllieMae.EMLite.DataAccess.DbQueryBuilder) null : sql);
      return fieldSearchRule.FsRuleId;
    }

    public static int UpdateFieldSearchInfo(FieldSearchRule fieldSearchRule)
    {
      fieldSearchRule.FsRuleId = FieldSearchDbAccessor.FindFsRuleId(fieldSearchRule.RuleId, fieldSearchRule.RuleType, fieldSearchRule.Identifier);
      return FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule, (EllieMae.EMLite.DataAccess.DbQueryBuilder) null);
    }

    public static void ChangeRuleStatus(BizRuleInfo ruleInfo, BizRule.RuleStatus status)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("FS_Rules");
      FieldSearchRuleType result;
      if (!Enum.TryParse<FieldSearchRuleType>(ruleInfo.RuleType.ToString(), true, out result))
        result = FieldSearchRuleType.None;
      int fsRuleId = FieldSearchDbAccessor.FindFsRuleId(ruleInfo.RuleID, result, "");
      if (fsRuleId > 0)
      {
        DbValueList values = new DbValueList();
        values.Add("Status", (object) (short) status);
        DbValueList keys = new DbValueList();
        keys.Add("FsRuleId", (object) fsRuleId);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Update(table, values, keys);
        dbQueryBuilder.ExecuteNonQuery();
      }
      else
        FieldSearchDbAccessor.CreateFieldSearchInfo(new FieldSearchRule(ruleInfo));
    }

    private static int FindFsRuleId(int ruleId, FieldSearchRuleType ruleType, string identifier = "�")
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @fsRuleId int");
      dbQueryBuilder.AppendLine("select @fsRuleId = fsRuleId from [FS_Rules] where RuleId = " + (object) ruleId + " and RuleType = " + (object) Convert.ToInt32((object) ruleType));
      if (ruleId == 0 && !string.IsNullOrEmpty(identifier))
        dbQueryBuilder.AppendLine(" and Identifier = '" + identifier.Replace("'", "''") + "'");
      dbQueryBuilder.Select("@fsRuleId");
      object obj = dbQueryBuilder.ExecuteScalar();
      int result = 0;
      if (obj != null)
        int.TryParse(obj.ToString(), out result);
      return result;
    }

    public static FieldSearchRuleType GetRuleType(int fsRuleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @ruleType int");
      dbQueryBuilder.AppendLine("select @ruleType = RuleType from [FS_Rules] where FsRuleId = " + (object) fsRuleId);
      dbQueryBuilder.Select("@ruleType");
      object obj = dbQueryBuilder.ExecuteScalar();
      int result = 0;
      if (obj != null)
        int.TryParse(obj.ToString(), out result);
      return (FieldSearchRuleType) result;
    }

    public static int GetRuleId(int fsRuleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @ruleId int");
      dbQueryBuilder.AppendLine("select @ruleId = RuleId from [FS_Rules] where FsRuleId = " + (object) fsRuleId);
      dbQueryBuilder.Select("@ruleId");
      object obj = dbQueryBuilder.ExecuteScalar();
      int result = 0;
      if (obj != null)
        int.TryParse(obj.ToString(), out result);
      return result;
    }

    public static string GetRuleIdentifier(int fsRuleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @identifier nvarchar(100)");
      dbQueryBuilder.AppendLine("select @identifier = Identifier from [FS_Rules] where FsRuleId = " + (object) fsRuleId);
      dbQueryBuilder.Select("@identifier");
      return dbQueryBuilder.ExecuteScalar().ToString();
    }

    public static int FindParentFSRuleId(
      int ruleId,
      FieldSearchRuleType ruleType,
      string identifier)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @fsRuleId int");
      dbQueryBuilder.AppendLine("select @fsRuleId = fsRuleId from [FS_Rules] where RuleId = " + (object) ruleId + " and RuleType = " + (object) Convert.ToInt32((object) ruleType) + " and (ParentFSRuleId is null or ParentFSRuleId = 0)");
      dbQueryBuilder.AppendLine(" and Identifier not like '" + identifier.Replace("'", "''") + "'");
      dbQueryBuilder.Select("@fsRuleId");
      object obj = dbQueryBuilder.ExecuteScalar();
      int result = 0;
      if (obj != null)
        int.TryParse(obj.ToString(), out result);
      return result;
    }

    public static FieldSearchRule GetRuleInfo(int fsRuleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("select R.*, F.FieldId, F.RelationshipType, F.IsSystemField, F.Description, F.Type from FS_Fields F inner JOIN FS_Rules R on F.FsRuleId = R.FsRuleId where R.FsRuleId = " + (object) fsRuleId + " OR R.ParentFSRuleId = " + (object) fsRuleId);
      DataTable dataTable1 = dbQueryBuilder1.ExecuteTableQuery();
      FieldSearchRule ruleInfo = (FieldSearchRule) null;
      if (dataTable1 != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
        {
          if (Convert.ToInt32(row["FsRuleId"]) == fsRuleId)
          {
            ruleInfo = new FieldSearchRule(row);
            IEnumerator enumerator = dataTable1.Rows.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                DataRow current = (DataRow) enumerator.Current;
                ruleInfo.FieldSearchFields.Add(new FieldSearchField(current));
              }
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          }
        }
      }
      List<FieldSearchField> fieldSearchFieldList = new List<FieldSearchField>();
      if (ruleInfo.FieldSearchFields.Any<FieldSearchField>())
      {
        foreach (FieldSearchField fieldSearchField in ruleInfo.FieldSearchFields.Where<FieldSearchField>((System.Func<FieldSearchField, bool>) (x => x.FieldId.Contains("|DDMDataTables|"))))
        {
          if (!string.IsNullOrEmpty(fieldSearchField.FieldId) && fieldSearchField.FieldId.Contains("|DDMDataTables|"))
          {
            string[] source = fieldSearchField.FieldId.Split('|');
            if (source != null && ((IEnumerable<string>) source).Count<string>() >= 3)
            {
              int fsRuleId1 = FieldSearchDbAccessor.FindFsRuleId(DDMDataTableDbAccessor.GetDDMDataTableAndFieldValuesByName(source[2]).Id, FieldSearchRuleType.DDMDataTables, "");
              EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
              dbQueryBuilder2.AppendLine("select * from FS_Fields where FsRuleId = " + (object) fsRuleId1);
              DataTable dataTable2 = dbQueryBuilder2.ExecuteTableQuery();
              if (dataTable2 != null)
              {
                foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
                  fieldSearchFieldList.Add(new FieldSearchField(row));
              }
            }
          }
        }
      }
      if (fieldSearchFieldList.Any<FieldSearchField>())
      {
        ruleInfo.FieldSearchFields.AddRange((IEnumerable<FieldSearchField>) fieldSearchFieldList);
        FieldSearchDbAccessor.ReplaceDDMDataTableFieldIds(ruleInfo.FieldSearchFields);
      }
      return ruleInfo;
    }

    private static void ReplaceDDMDataTableFieldIds(List<FieldSearchField> FieldSearchFields)
    {
      foreach (FieldSearchField fieldSearchField in FieldSearchFields)
      {
        if (fieldSearchField.FieldId.Contains("|DDMDataTables|"))
        {
          string[] strArray = fieldSearchField.FieldId.Split('|');
          fieldSearchField.FieldId = strArray[0];
        }
      }
    }

    private static string getRuleTypesInClause(List<FieldSearchRuleType> fsRuleTypes)
    {
      List<int> values = new List<int>();
      foreach (FieldSearchRuleType fsRuleType in fsRuleTypes)
        values.Add((int) fsRuleType);
      return "('" + string.Join<int>("','", (IEnumerable<int>) values) + "')";
    }

    public static List<int> GetFSRuleIds(List<FieldSearchRuleType> fsRuleTypes)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string ruleTypesInClause = FieldSearchDbAccessor.getRuleTypesInClause(fsRuleTypes);
      dbQueryBuilder.AppendLine("select FsRuleId from [FS_Rules] where RuleType in " + ruleTypesInClause);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      List<int> fsRuleIds = new List<int>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        fsRuleIds.Add(Convert.ToInt32(row["FsRuleId"]));
      return fsRuleIds;
    }

    public static void PurgeDangling(
      List<int> FsRuleIds,
      List<FieldSearchRuleType> fsRuleTypes,
      bool sanityCheck = true)
    {
      if (sanityCheck)
      {
        List<int> fsRuleIds = FieldSearchDbAccessor.GetFSRuleIds(fsRuleTypes);
        if (fsRuleIds != null)
        {
          HashSet<int> FsRuleIdHashSet = new HashSet<int>();
          foreach (int fsRuleId in FsRuleIds)
            FsRuleIdHashSet.Add(fsRuleId);
          fsRuleIds.RemoveAll((Predicate<int>) (n => FsRuleIdHashSet.Contains(n)));
          if (fsRuleIds.Count <= 0)
            return;
        }
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = "('" + string.Join<int>(",", (IEnumerable<int>) FsRuleIds) + ",')";
      string ruleTypesInClause = FieldSearchDbAccessor.getRuleTypesInClause(fsRuleTypes);
      dbQueryBuilder.AppendLine("delete F from [FS_Fields] F inner join [FS_Rules] R on R.FsRuleId = F.FsRuleId inner join (select R.FsRuleId from [FS_Rules] R except select T.id from FN_Split" + str + " T) O on O.FsRuleId = R.FsRuleId where RuleType in " + ruleTypesInClause);
      dbQueryBuilder.AppendLine("delete R from [FS_Rules] R inner join (select R.FsRuleId from [FS_Rules] R except select T.id from FN_Split" + str + " T) O on O.FsRuleId = R.FsRuleId where RuleType in " + ruleTypesInClause);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateLoanCustomFieldsFieldSearch(
      CustomFieldsInfo loanCustomFields,
      bool rebuild = true)
    {
      if (loanCustomFields.GetNonEmptyCount() <= 0)
      {
        if (!rebuild)
          return;
        FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.LoanCustomFields);
      }
      else
      {
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        Dictionary<string, int> fsRuleIds1 = FieldSearchDbAccessor.GetFSRuleIds(FieldSearchRuleType.LoanCustomFields);
        List<string> stringList = new List<string>();
        foreach (CustomFieldInfo loanCustomField in loanCustomFields)
        {
          if (!loanCustomField.IsEmpty())
          {
            if (fsRuleIds1.ContainsKey(loanCustomField.FieldID))
            {
              int key = fsRuleIds1[loanCustomField.FieldID];
              dictionary[key] = loanCustomField.FieldID;
            }
            else
              stringList.Add(loanCustomField.FieldID);
          }
        }
        int num1 = 0;
        if (rebuild)
        {
          foreach (string key1 in fsRuleIds1.Keys)
          {
            int key2 = fsRuleIds1[key1];
            if (key2 > 0 && !dictionary.ContainsKey(key2))
              ++num1;
          }
        }
        int num2 = 0;
        EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
        foreach (CustomFieldInfo loanCustomField in loanCustomFields)
        {
          if (!loanCustomField.IsEmpty())
          {
            if (num2 == 0)
              sql.Declare("@ruleId", "int");
            FieldSearchRule fieldSearchRule = new FieldSearchRule(loanCustomField);
            if (fsRuleIds1.ContainsKey(loanCustomField.FieldID))
              fieldSearchRule.FsRuleId = fsRuleIds1[loanCustomField.FieldID];
            FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule, (EllieMae.EMLite.DataAccess.DbQueryBuilder) sql);
            ++num2;
            if (num2 >= 5000)
            {
              if (sql.ToString() != string.Empty)
                sql.ExecuteNonQuery();
              sql.Reset();
              num2 = 0;
            }
          }
        }
        if (sql.ToString() != string.Empty)
          sql.ExecuteNonQuery();
        if (!(num1 > 0 & rebuild))
          return;
        if (stringList.Count > 0)
        {
          Dictionary<string, int> fsRuleIds2 = FieldSearchDbAccessor.GetFSRuleIds(FieldSearchRuleType.LoanCustomFields);
          foreach (string key in stringList)
            dictionary[fsRuleIds2[key]] = key;
        }
        FieldSearchDbAccessor.PurgeDangling(dictionary.Keys.ToList<int>(), new List<FieldSearchRuleType>()
        {
          FieldSearchRuleType.LoanCustomFields
        }, false);
      }
    }

    private static string GetFieldInfo(
      string fieldId,
      FieldSearchDbAccessor.FieldInfoType infoType,
      EllieMae.EMLite.FieldSearch.FieldType fieldType = EllieMae.EMLite.FieldSearch.FieldType.Unknown)
    {
      if (fieldType == EllieMae.EMLite.FieldSearch.FieldType.Unknown || EllieMae.EMLite.FieldSearch.FieldType.Standard == fieldType)
      {
        EncompassField encompassField = EncompassFieldData.Instance.Find(fieldId);
        if (encompassField != null)
        {
          if (infoType == FieldSearchDbAccessor.FieldInfoType.Description)
            return encompassField.Description;
          return infoType == FieldSearchDbAccessor.FieldInfoType.Format ? encompassField.Format : (string) null;
        }
        if (EllieMae.EMLite.FieldSearch.FieldType.Standard == fieldType)
          return (string) null;
      }
      if (fieldType == EllieMae.EMLite.FieldSearch.FieldType.Unknown || EllieMae.EMLite.FieldSearch.FieldType.LoanCustom == fieldType)
      {
        CustomFieldInfo field = SystemConfiguration.GetLoanCustomFields().GetField(fieldId);
        if (field != null && !field.IsEmpty())
        {
          if (infoType == FieldSearchDbAccessor.FieldInfoType.Description)
            return field.Description;
          return infoType == FieldSearchDbAccessor.FieldInfoType.Format ? FieldFormatEnumUtil.ValueToName(field.Format) : (string) null;
        }
        if (EllieMae.EMLite.FieldSearch.FieldType.LoanCustom == fieldType)
          return (string) null;
      }
      ContactCustomFieldInfoCollection[] fieldInfoCollectionArray = new ContactCustomFieldInfoCollection[3]
      {
        BorrowerCustomFields.Get(),
        BizPartnerCustomFields.Get(),
        ExternalOrgManagementAccessor.GetCustomFieldInfo()
      };
      foreach (ContactCustomFieldInfoCollection fieldInfoCollection in fieldInfoCollectionArray)
      {
        if (fieldInfoCollection != null)
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in fieldInfoCollection.Items)
          {
            if (string.Compare(contactCustomFieldInfo.Label, fieldId, true) == 0)
            {
              if (infoType == FieldSearchDbAccessor.FieldInfoType.Description)
                return contactCustomFieldInfo.Label;
              return infoType == FieldSearchDbAccessor.FieldInfoType.Format ? FieldFormatEnumUtil.ValueToName(contactCustomFieldInfo.FieldType) : (string) null;
            }
          }
        }
      }
      CustomFieldsDefinitionInfo[] fieldsDefinitions = BizPartnerContact.GetCustomFieldsDefinitions(CustomFieldsType.BizCategoryCustom | CustomFieldsType.BizCategoryStandard);
      if (fieldsDefinitions != null && fieldsDefinitions.Length != 0)
      {
        foreach (CustomFieldsDefinitionInfo fieldsDefinitionInfo in fieldsDefinitions)
        {
          foreach (CustomFieldDefinitionInfo customFieldDefinition in fieldsDefinitionInfo.CustomFieldDefinitions)
          {
            if (string.Compare(customFieldDefinition.FieldDescription, fieldId, true) == 0)
            {
              if (infoType == FieldSearchDbAccessor.FieldInfoType.Description)
                return customFieldDefinition.FieldDescription;
              return infoType == FieldSearchDbAccessor.FieldInfoType.Format ? FieldFormatEnumUtil.ValueToName(customFieldDefinition.FieldFormat) : (string) null;
            }
          }
        }
      }
      return (string) null;
    }

    public static string GetFieldDescription(string fieldId, EllieMae.EMLite.FieldSearch.FieldType fieldType = EllieMae.EMLite.FieldSearch.FieldType.Unknown)
    {
      string fieldDescription = FieldSearchDbAccessor.GetFieldInfo(fieldId, FieldSearchDbAccessor.FieldInfoType.Description, fieldType);
      if (fieldDescription == null)
      {
        FieldSettings fieldConfig = (FieldSettings) null;
        if (EllieMae.EMLite.FieldSearch.FieldType.Standard != fieldType)
          fieldConfig = LoanConfiguration.GetFieldSettings();
        fieldDescription = EncompassFields.GetDescription(fieldId, fieldConfig);
      }
      return fieldDescription;
    }

    public static string GetFieldFormat(string fieldId, EllieMae.EMLite.FieldSearch.FieldType fieldType = EllieMae.EMLite.FieldSearch.FieldType.Unknown)
    {
      return FieldSearchDbAccessor.GetFieldInfo(fieldId, FieldSearchDbAccessor.FieldInfoType.Format, fieldType);
    }

    public static DynamicBusinessRuleInfo GetDynamicBusinessRules(string[] fieldList)
    {
      string text1 = "SELECT DISTINCT f.FieldID AS FieldId\r\n                                    FROM FS_Fields f\r\n                                    INNER JOIN FS_Rules r ON f.FsRuleId = r.FsRuleId\r\n                                    WHERE r.Status = 1 AND r.RuleType IN (3, 4) AND f.RelationshipType = 6";
      string text2 = "SELECT DISTINCT f.FieldID AS FieldId\r\n                                    FROM FS_Fields f\r\n                                    INNER JOIN FS_Rules r ON f.FsRuleId = r.FsRuleId\r\n                                    WHERE r.Status = 1 AND r.RuleType = 6 AND f.RelationshipType = 6";
      string text3 = "SELECT fr.fieldId,fr.ruleType,fr.ruleValue\r\n                                    FROM [FR_FieldRule] fr\r\n                                    INNER JOIN FS_Rules r ON r.RuleId = fr.RuleId\r\n                                    INNER JOIN FS_Fields f ON f.FsRuleId = r.FsRuleId\r\n                                    WHERE r.Status = 1 AND r.RuleType IN (4) AND f.RelationshipType = 6";
      string text4 = "SELECT DISTINCT fr.FieldID AS FieldId\r\n                                    FROM [FAR_Fields] fr\r\n                                    INNER JOIN FS_Rules r ON r.RuleId = fr.RuleId\r\n                                    INNER JOIN FS_Fields f ON f.FsRuleId = r.FsRuleId\r\n                                    WHERE r.Status = 1 AND r.RuleType IN (3) AND f.RelationshipType = 6";
      if (fieldList != null && fieldList.Length != 0)
      {
        text1 = text1 + " AND f.FieldId IN (" + SQL.EncodeArray((Array) fieldList) + ")";
        text2 = text2 + " AND f.FieldId IN (" + SQL.EncodeArray((Array) fieldList) + ")";
        text3 = text3 + " AND f.FieldId IN (" + SQL.EncodeArray((Array) fieldList) + ")";
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(text1);
      dbQueryBuilder.AppendLine(text2);
      dbQueryBuilder.AppendLine(text3);
      dbQueryBuilder.AppendLine(text4);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DynamicBusinessRuleInfo dynamicBusinessRules = new DynamicBusinessRuleInfo();
      if (dataSet != null)
      {
        EmList<string> emList1 = new EmList<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          if (row["FieldId"] != DBNull.Value)
          {
            string str = row["FieldId"].ToString();
            emList1.Add(str);
          }
        }
        dynamicBusinessRules.ConditionalFields = (IList<string>) emList1;
        EmList<string> emList2 = new EmList<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
        {
          if (row["FieldId"] != DBNull.Value)
          {
            string str = row["FieldId"].ToString();
            emList2.Add(str);
          }
        }
        dynamicBusinessRules.TriggerActivationFields = (IList<string>) emList2;
        IDictionary<string, IList<FieldRule>> dictionary = (IDictionary<string, IList<FieldRule>>) new Dictionary<string, IList<FieldRule>>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[2].Rows)
        {
          string key = (string) row["fieldID"];
          BizRule.FieldRuleType fieldRuleType = (BizRule.FieldRuleType) (byte) row["ruleType"];
          string str = (string) row["ruleValue"];
          if (fieldRuleType != BizRule.FieldRuleType.Code)
          {
            if (!dictionary.ContainsKey(key))
              dictionary[key] = (IList<FieldRule>) new List<FieldRule>();
            dictionary[key].Add(new FieldRule()
            {
              ruleType = fieldRuleType,
              ruleValue = str
            });
          }
        }
        dynamicBusinessRules.FieldDataEntryRules = dictionary;
        EmList<string> emList3 = new EmList<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[3].Rows)
        {
          if (row["FieldId"] != DBNull.Value)
          {
            string str = row["FieldId"].ToString();
            emList3.Add(str);
          }
        }
        dynamicBusinessRules.PersonaAccessToFields = (IList<string>) emList3;
      }
      return dynamicBusinessRules;
    }

    private static List<string> GetDataTableFieldsReferedInDDMScenario(
      List<string> dataTableNames,
      HashSet<string> virtualFields)
    {
      List<string> values = new List<string>(dataTableNames.Count);
      foreach (string dataTableName in dataTableNames)
        values.Add(SQL.Encode((object) dataTableName));
      List<string> referedInDdmScenario = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct FieldId from FS_Fields inner join");
      dbQueryBuilder.AppendLine("(select FsRuleId from FS_Rules where RuleId in");
      dbQueryBuilder.AppendLine("(select dataTableID from DDM_DataTables");
      dbQueryBuilder.AppendLine("where dataTableName in (" + string.Join(", ", (IEnumerable<string>) values) + "))");
      dbQueryBuilder.AppendLine("and RuleType = " + (object) Convert.ToInt32((object) FieldSearchRuleType.DDMDataTables) + ")");
      dbQueryBuilder.AppendLine("as temp on FS_Fields.FsRuleId = temp.FsRuleId");
      dbQueryBuilder.ToString();
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          string fieldId = row["FieldId"].ToString();
          if (VirtualFields.GetField(fieldId) != null)
            virtualFields.Add(fieldId);
          else
            referedInDdmScenario.Add(fieldId);
        }
      }
      return referedInDdmScenario;
    }

    public static string GetFields(FieldSearchRuleType[] ruleTypes)
    {
      return FieldSearchDbAccessor.GetFieldsAndDataTableNames(ruleTypes).fields;
    }

    public static List<string> GetReferencedDataTables(FieldSearchRuleType[] ruleTypes)
    {
      return FieldSearchDbAccessor.GetFieldsAndDataTableNames(ruleTypes).dataTableNames;
    }

    [PgReady]
    public static DDMAffectedFieldsandDataTableNames GetFieldsAndDataTableNames(
      FieldSearchRuleType[] ruleTypes)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        string str1 = "SELECT DISTINCT f.FieldID AS FieldId, f.RelationshipType AS RelationshipType\r\n                                    FROM FS_Fields f\r\n                                    INNER JOIN FS_Rules r ON f.FsRuleId = r.FsRuleId\r\n                                    WHERE r.RuleType IN (";
        string str2 = "";
        for (int index = 0; index < ruleTypes.Length; ++index)
          str2 = str2 + (str2 != "" ? (object) "," : (object) "") + (object) Convert.ToInt32((object) ruleTypes[index]);
        string text = str1 + str2 + ") AND r.Status = 1";
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine(text);
        DataSet dataSet = pgDbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return (DDMAffectedFieldsandDataTableNames) null;
        HashSet<string> stringSet1 = new HashSet<string>((IEnumerable<string>) new string[70]
        {
          "3042",
          "MORNET.X40",
          "353",
          "2558",
          "2559",
          "2560",
          "2561",
          "2562",
          "3432",
          "1109",
          "MORNET.X41",
          "1107",
          "1826",
          "1760",
          "1199",
          "1198",
          "1201",
          "1200",
          "1205",
          "1045",
          "337",
          "232",
          "338",
          "2553",
          "19",
          "VAVOB.X72",
          "1771",
          "958",
          "745",
          "1107",
          "1826",
          "1760",
          "1199",
          "1198",
          "1201",
          "1200",
          "1205",
          "1045",
          "1050",
          "3560",
          "3566",
          "3561",
          "3562",
          "3563",
          "3565",
          "3564",
          "NEWHUD.X1707",
          "NEWHUD.X1708",
          "136",
          "647",
          "648",
          "374",
          "1641",
          "1644",
          "1637",
          "1638",
          "NEWHUD.X1082",
          "NEWHUD.X1083",
          "NEWHUD.X1084",
          "19",
          "2",
          "356",
          "NEWHUD.X572",
          "NEWHUD.X639",
          "NEWHUD.X808",
          "1172",
          "4",
          "VASUMM.X49",
          "748",
          "1887"
        });
        HashSet<string> stringSet2 = new HashSet<string>();
        HashSet<string> virtualFields = new HashSet<string>();
        HashSet<string> source1 = new HashSet<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          if (row["FieldId"] != DBNull.Value)
          {
            string fieldId = row["FieldId"].ToString();
            if (!stringSet1.Contains(fieldId))
            {
              if (VirtualFields.GetField(fieldId) != null)
                virtualFields.Add(fieldId);
              else if (row["RelationshipType"].ToString() == "11")
              {
                string[] source2 = fieldId.Split('|');
                if (source2 != null && ((IEnumerable<string>) source2).Count<string>() >= 3)
                {
                  stringSet2.Add(source2[0]);
                  source1.Add(source2[2]);
                }
              }
              else
                stringSet2.Add(fieldId);
            }
          }
        }
        List<string> list = source1.ToList<string>();
        if (list.Count > 0)
        {
          foreach (string str3 in FieldSearchDbAccessor.GetDataTableFieldsReferedInDDMScenario(list, virtualFields))
            stringSet2.Add(str3);
        }
        foreach (string str4 in stringSet1)
          stringSet2.Add(str4);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("|");
        foreach (string str5 in stringSet2)
          stringBuilder.Append(str5 + "|");
        if (virtualFields.Count > 0)
        {
          stringBuilder.Append("VF|");
          foreach (string str6 in virtualFields)
            stringBuilder.Append(str6 + "|");
        }
        return new DDMAffectedFieldsandDataTableNames(stringBuilder.ToString(), list);
      }
      string str7 = "SELECT DISTINCT f.FieldID AS FieldId, f.RelationshipType AS RelationshipType\r\n                                    FROM FS_Fields f\r\n                                    INNER JOIN FS_Rules r ON f.FsRuleId = r.FsRuleId\r\n                                    WHERE r.RuleType IN (";
      string str8 = "";
      for (int index = 0; index < ruleTypes.Length; ++index)
        str8 = str8 + (str8 != "" ? (object) "," : (object) "") + (object) Convert.ToInt32((object) ruleTypes[index]);
      string text1 = str7 + str8 + ") AND r.Status = 1";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(text1);
      DataSet dataSet1 = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet1 == null)
        return (DDMAffectedFieldsandDataTableNames) null;
      HashSet<string> stringSet3 = new HashSet<string>((IEnumerable<string>) new string[70]
      {
        "3042",
        "MORNET.X40",
        "353",
        "2558",
        "2559",
        "2560",
        "2561",
        "2562",
        "3432",
        "1109",
        "MORNET.X41",
        "1107",
        "1826",
        "1760",
        "1199",
        "1198",
        "1201",
        "1200",
        "1205",
        "1045",
        "337",
        "232",
        "338",
        "2553",
        "19",
        "VAVOB.X72",
        "1771",
        "958",
        "745",
        "1107",
        "1826",
        "1760",
        "1199",
        "1198",
        "1201",
        "1200",
        "1205",
        "1045",
        "1050",
        "3560",
        "3566",
        "3561",
        "3562",
        "3563",
        "3565",
        "3564",
        "NEWHUD.X1707",
        "NEWHUD.X1708",
        "136",
        "647",
        "648",
        "374",
        "1641",
        "1644",
        "1637",
        "1638",
        "NEWHUD.X1082",
        "NEWHUD.X1083",
        "NEWHUD.X1084",
        "19",
        "2",
        "356",
        "NEWHUD.X572",
        "NEWHUD.X639",
        "NEWHUD.X808",
        "1172",
        "4",
        "VASUMM.X49",
        "748",
        "1887"
      });
      HashSet<string> stringSet4 = new HashSet<string>();
      HashSet<string> virtualFields1 = new HashSet<string>();
      HashSet<string> source3 = new HashSet<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet1.Tables[0].Rows)
      {
        if (row["FieldId"] != DBNull.Value)
        {
          string fieldId = row["FieldId"].ToString();
          if (!stringSet3.Contains(fieldId))
          {
            if (VirtualFields.GetField(fieldId) != null)
              virtualFields1.Add(fieldId);
            else if (row["RelationshipType"].ToString() == "11")
            {
              string[] source4 = fieldId.Split('|');
              if (source4 != null && ((IEnumerable<string>) source4).Count<string>() >= 3)
              {
                stringSet4.Add(source4[0]);
                source3.Add(source4[2]);
              }
            }
            else
              stringSet4.Add(fieldId);
          }
        }
      }
      List<string> list1 = source3.ToList<string>();
      if (list1.Count > 0)
      {
        foreach (string str9 in FieldSearchDbAccessor.GetDataTableFieldsReferedInDDMScenario(list1, virtualFields1))
          stringSet4.Add(str9);
      }
      foreach (string str10 in stringSet3)
        stringSet4.Add(str10);
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("|");
      foreach (string str11 in stringSet4)
        stringBuilder1.Append(str11 + "|");
      if (virtualFields1.Count > 0)
      {
        stringBuilder1.Append("VF|");
        foreach (string str12 in virtualFields1)
          stringBuilder1.Append(str12 + "|");
      }
      return new DDMAffectedFieldsandDataTableNames(stringBuilder1.ToString(), list1);
    }

    public static List<DDMDataTableReference> GetDdmDatatableReferences(string dataTableName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "SELECT COUNT(*) AS ReferenceCount, RuleType, RuleName, ScenarioName \r\n                        FROM\r\n                        (\r\n                        SELECT r.FsRuleId, r.RuleId, r.RuleName, r.RuleType, f.FieldId, s.ruleName AS ScenarioName\r\n                        FROM FS_Fields AS f\r\n                          INNER JOIN FS_Rules AS r ON (f.FsRuleId = r.FsRuleId AND r.RuleType = 24)\r\n                          INNER JOIN DDM_FeeRuleScenario AS s ON (r.RuleId = s.ruleID)\r\n                        WHERE FieldId like '%{{TableName}}'\r\n                        UNION ALL\r\n                        SELECT r.FsRuleId, r.RuleId, r.RuleName, r.RuleType, f.FieldId, s.ruleName AS ScenarioName\r\n                        FROM FS_Fields AS f\r\n                          INNER JOIN FS_Rules AS r ON (f.FsRuleId = r.FsRuleId AND r.RuleType = 25)\r\n                          INNER JOIN DDM_FieldRuleScenario AS s ON (r.RuleId = s.ruleID)\r\n                        WHERE FieldId like '%{{TableName}}'\r\n                        ) AS FeeFieldScenario\r\n                        GROUP BY RuleType, RuleName, FsRuleId, ScenarioName".Replace("{{TableName}}", "|DDMDataTables|" + dataTableName);
      dbQueryBuilder.AppendLine(text);
      DataRowCollection source = dbQueryBuilder.Execute();
      return source != null ? source.Cast<DataRow>().Select<DataRow, DDMDataTableReference>((System.Func<DataRow, DDMDataTableReference>) (dr => new DDMDataTableReference(Convert.ToInt32(dr["RuleType"]), Convert.ToString(dr["RuleName"]), Convert.ToString(dr["ScenarioName"]), Convert.ToInt32(dr["ReferenceCount"])))).ToList<DDMDataTableReference>() : (List<DDMDataTableReference>) null;
    }

    public static void ResetDataTableReference(string dataTableName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "\r\n                  DELETE FROM FS_Fields\r\n                  WHERE FieldId LIKE '%|{{DDMDataTablesConst}}|{{DataTableName}}' AND RelationshipType = {{RelationShipType}}".Replace("{{DDMDataTablesConst}}", "|DDMDataTables|").Replace("{{DataTableName}}", dataTableName).Replace("{{RelationShipType}}", SQL.Encode((object) Convert.ToByte((object) RelationshipType.RefersRule)));
      dbQueryBuilder.AppendLine(text);
      dbQueryBuilder.Execute();
    }

    public static void UpdateDdmDataTableReference(string oldName, string newName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "WITH\r\n                  Decomposed AS (\r\n                    SELECT\r\n                      FsRuleId,\r\n                      FieldId,\r\n                      SUBSTRING(FieldId, 0, CHARINDEX('|', FieldId)) AS PartFieldId,\r\n                      SUBSTRING(SUBSTRING(FieldId, CHARINDEX('|', FieldId) + 1, LEN(FieldId)), --chop off the field-id\r\n                                CHARINDEX('|', SUBSTRING(FieldId, CHARINDEX('|', FieldId) + 1, LEN(FieldId))) + 1,\r\n                                LEN(FieldId)) AS TableName\r\n                    FROM FS_Fields\r\n                    WHERE RelationshipType = 11 AND IsSystemField = 0\r\n                  )\r\n                UPDATE fs\r\n                  SET fs.FieldId = d.PartFieldId + '{{DDMDataTablesConst}}' + '{{NewDataTableName}}'\r\n                FROM FS_Fields AS fs\r\n                 INNER JOIN Decomposed AS d ON (fs.FsRuleId = d.FsRuleId AND fs.FieldId = d.FieldId)\r\n                WHERE d.TableName = '{{OldDataTableName}}'".Replace("{{DDMDataTablesConst}}", "|DDMDataTables|").Replace("{{NewDataTableName}}", newName).Replace("{{OldDataTableName}}", oldName);
      dbQueryBuilder.AppendLine(text);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Dictionary<string, HashSet<string>> GetFieldsReferredInDataTables(
      List<string> dataTablesList)
    {
      Dictionary<string, HashSet<string>> referredInDataTables = new Dictionary<string, HashSet<string>>();
      if (dataTablesList.Count < 1)
        return referredInDataTables;
      List<string> values = new List<string>(dataTablesList.Count);
      values.AddRange(dataTablesList.Select<string, string>((System.Func<string, string>) (tableName => SQL.Encode((object) tableName))));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("SELECT DISTINCT FieldId, RuleName\r\n                            FROM FS_Fields\r\n                                INNER JOIN (SELECT FsRuleId, RuleName\r\n\t\t\t\t                            FROM FS_Rules\r\n\t\t\t\t                            WHERE RuleId IN (SELECT dataTableID FROM DDM_DataTables\r\n\t\t\t\t\t\t\t\t                            WHERE dataTableName IN (");
      dbQueryBuilder.Append(string.Join(", ", (IEnumerable<string>) values) + "))");
      dbQueryBuilder.AppendLine("and RuleType = " + (object) Convert.ToInt32((object) FieldSearchRuleType.DDMDataTables) + ")");
      dbQueryBuilder.AppendLine(" AS temp\r\n                ON FS_Fields.FsRuleId = temp.FsRuleId");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (Dictionary<string, HashSet<string>>) null;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = dataRow["RuleName"] == DBNull.Value ? (string) null : dataRow["RuleName"].ToString();
        if (key != null)
        {
          string str = dataRow["FieldId"] == DBNull.Value ? (string) null : dataRow["FieldId"].ToString();
          if (!referredInDataTables.ContainsKey(key))
            referredInDataTables.Add(key, new HashSet<string>());
          referredInDataTables[key].Add(str);
        }
      }
      return referredInDataTables;
    }

    private enum FieldInfoType
    {
      None,
      Description,
      Format,
    }
  }
}
