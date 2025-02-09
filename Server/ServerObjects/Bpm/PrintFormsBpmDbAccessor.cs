// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.PrintFormsBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public sealed class PrintFormsBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "PrintFormsBpmDbAccessor�";
    private const string CUSTOMLETTERS = "CustomLetters�";

    public PrintFormsBpmDbAccessor()
      : base(ClientSessionCacheID.BpmPrintForms)
    {
    }

    protected override string RuleTableName => "[BR_PrintFormRules]";

    protected override Type RuleType => typeof (PrintFormRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[BBR_RequiredFields]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[BBR_AdvancedCoding]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[BBR_PrintFormRules]"), ruleIdValue);
      sql.AppendLine("delete from BBR_PrintFormRuleXRef where ruleID = " + ruleIdValue.Value.ToString());
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      ArrayList arrayList = new ArrayList();
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      PrintFormRuleInfo printFormRuleInfo = (PrintFormRuleInfo) rule;
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[BBR_RequiredFields]");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[BBR_AdvancedCoding]");
      DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[BBR_PrintFormRules]");
      string empty = string.Empty;
      foreach (PrintRequiredFieldsInfo formRule in printFormRuleInfo.FormRules)
      {
        string str = formRule.FormID;
        if (formRule.FormType == OutputFormType.CustomLetters)
        {
          CustomLetterXRef xref = (CustomLetterXRef) this.generateXRef(formRule.FormID);
          if (xref != null)
          {
            str = xref.Guid;
            arrayList.Add((object) xref);
          }
          else
            continue;
        }
        if (!insensitiveHashtable.ContainsKey((object) formRule.FormID))
          insensitiveHashtable.Add((object) formRule.FormID, (object) str);
        sql.InsertInto(table3, new DbValueList()
        {
          ruleIdValue,
          {
            "formID",
            (object) str
          },
          {
            "formType",
            (object) formRule.FormType.ToString()
          }
        }, true, false);
      }
      foreach (PrintRequiredFieldsInfo formRule in printFormRuleInfo.FormRules)
      {
        if (formRule.FieldIDs != null)
        {
          foreach (string fieldId in formRule.FieldIDs)
          {
            string formId = formRule.FormID;
            if (insensitiveHashtable.ContainsKey((object) formRule.FormID))
              formId = insensitiveHashtable[(object) formRule.FormID].ToString();
            sql.InsertInto(table1, new DbValueList()
            {
              ruleIdValue,
              {
                "formID",
                (object) formId
              },
              {
                "requiredFieldID",
                (object) fieldId
              }
            }, true, false);
          }
        }
      }
      foreach (PrintRequiredFieldsInfo formRule in printFormRuleInfo.FormRules)
      {
        if ((formRule.AdvancedCoding ?? "") != string.Empty)
        {
          string formId = formRule.FormID;
          if (insensitiveHashtable.ContainsKey((object) formRule.FormID))
            formId = insensitiveHashtable[(object) formRule.FormID].ToString();
          sql.InsertInto(table2, new DbValueList()
          {
            ruleIdValue,
            {
              "formID",
              (object) formId
            },
            {
              "advancedCodes",
              (object) formRule.AdvancedCoding
            }
          }, true, false);
        }
      }
      CustomLetterXRef[] array = (CustomLetterXRef[]) arrayList.ToArray(typeof (CustomLetterXRef));
      if (array == null || array.Length == 0)
        return;
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index] != null)
        {
          if (ruleIdValue != null && ruleIdValue.Value.ToString() != string.Empty)
            sql.AppendLine("insert into BBR_PrintFormRuleXRef([Guid], [ruleID], [XRef], [FormType]) values(" + SQL.Encode((object) array[index].Guid) + "," + ruleIdValue.Value.ToString() + "," + SQL.Encode((object) array[index].XRef.ToString()) + ",'CustomLetters')");
          else
            sql.AppendLine("insert into BBR_PrintFormRuleXRef([Guid], [ruleID], [XRef], [FormType]) values(" + SQL.Encode((object) array[index].Guid) + ",@ruleId, " + SQL.Encode((object) array[index].XRef.ToString()) + ",'CustomLetters')");
        }
      }
    }

    [PgReady]
    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [BR_PrintFormRules] rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select formRules.* from [BBR_PrintFormRules] formRules");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_PrintFormRules] rules on formRules.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        pgDbQueryBuilder.AppendLine("select fields.* from [BBR_RequiredFields] fields");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_PrintFormRules] rules on fields.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        pgDbQueryBuilder.AppendLine("select advCodes.* from [BBR_AdvancedCoding] advCodes");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_PrintFormRules] rules on advCodes.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        return (BizRuleInfo[]) PrintFormsBpmDbAccessor.dataSetToRuleInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select * from [BR_PrintFormRules] rules where " + filter);
      dbQueryBuilder.AppendLine("select formRules.* from [BBR_PrintFormRules] formRules");
      dbQueryBuilder.AppendLine("\tinner join [BR_PrintFormRules] rules on formRules.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select fields.* from [BBR_RequiredFields] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_PrintFormRules] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select advCodes.* from [BBR_AdvancedCoding] advCodes");
      dbQueryBuilder.AppendLine("\tinner join [BR_PrintFormRules] rules on advCodes.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      return (BizRuleInfo[]) PrintFormsBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static PrintFormRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation1 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataRelation relation2 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[2].Columns["ruleID"]);
      DataRelation relation3 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[3].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      ArrayList arrayList1 = new ArrayList();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      List<string> stringList1 = new List<string>();
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        Hashtable hashtable = PrintFormsBpmDbAccessor.readLetterXRefsFromDatabase((int) row["ruleID"]);
        Hashtable reqFieldsHt = PrintFormsBpmDbAccessor.dataRowsToReqFieldsHT((ICollection) row.GetChildRows(relation2));
        Hashtable advancedCodingHt = PrintFormsBpmDbAccessor.dataRowsToAdvancedCodingHT((ICollection) row.GetChildRows(relation3));
        Hashtable reqFormsHt = PrintFormsBpmDbAccessor.dataRowsToReqFormsHT((ICollection) row.GetChildRows(relation1));
        ArrayList arrayList2 = new ArrayList();
        if (reqFormsHt != null)
        {
          foreach (DictionaryEntry dictionaryEntry in reqFormsHt)
          {
            string displayString = dictionaryEntry.Key.ToString();
            OutputFormType formType = (OutputFormType) dictionaryEntry.Value;
            List<string> stringList2 = !reqFieldsHt.ContainsKey((object) displayString) ? new List<string>() : (List<string>) reqFieldsHt[(object) displayString];
            string empty3 = string.Empty;
            if (advancedCodingHt.ContainsKey((object) displayString))
              empty3 = (string) advancedCodingHt[(object) displayString];
            if (stringList2 != null && stringList2.Count != 0 || !(empty3 == string.Empty))
            {
              if (hashtable.ContainsKey((object) displayString))
                displayString = ((CustomLetterXRef) hashtable[(object) displayString]).XRef.ToDisplayString();
              arrayList2.Add((object) new PrintRequiredFieldsInfo(displayString, formType, stringList2?.ToArray(), empty3));
            }
          }
        }
        if (arrayList2.Count > 0)
          arrayList1.Add((object) new PrintFormRuleInfo(row, (PrintRequiredFieldsInfo[]) arrayList2.ToArray(typeof (PrintRequiredFieldsInfo))));
      }
      return (PrintFormRuleInfo[]) arrayList1.ToArray(typeof (PrintFormRuleInfo));
    }

    private static Hashtable dataRowsToReqFormsHT(ICollection rows)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (rows.Count == 0)
        return insensitiveHashtable;
      ArrayList arrayList = new ArrayList();
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key = (string) row["formID"];
        OutputFormType outputFormType = SQL.DecodeEnum<OutputFormType>(row["formType"]);
        if (!insensitiveHashtable.ContainsKey((object) key))
          insensitiveHashtable.Add((object) key, (object) outputFormType);
      }
      return insensitiveHashtable;
    }

    private static Hashtable dataRowsToReqFieldsHT(ICollection rows)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (rows.Count == 0)
        return insensitiveHashtable;
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key = (string) row["formID"];
        string str = (string) row["requiredFieldID"];
        if (!insensitiveHashtable.ContainsKey((object) key))
          insensitiveHashtable.Add((object) key, (object) new List<string>());
        ((List<string>) insensitiveHashtable[(object) key]).Add(str);
      }
      return insensitiveHashtable;
    }

    private static Hashtable dataRowsToAdvancedCodingHT(ICollection rows)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (rows.Count == 0)
        return insensitiveHashtable;
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key = (string) row["formID"];
        string str = (string) row["advancedCodes"];
        if (!insensitiveHashtable.ContainsKey((object) key))
          insensitiveHashtable.Add((object) key, (object) str);
      }
      return insensitiveHashtable;
    }

    private static Hashtable readLetterXRefsFromDatabase(int ruleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid, XRef from BBR_PrintFormRuleXRef where FormType = 'CustomLetters' AND ruleID = " + (object) ruleId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable hashtable = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        try
        {
          CustomLetterXRef customLetterXref = new CustomLetterXRef(dataRow["Guid"].ToString(), FileSystemEntry.Parse(dataRow["XRef"].ToString()));
          hashtable.Add((object) customLetterXref.Guid, (object) customLetterXref);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (PrintFormsBpmDbAccessor), "PrintFormsBpmDbAccessor:Error reading Print Auto Selection XRef: " + (object) ex);
        }
      }
      return hashtable;
    }

    private object generateXRef(string path)
    {
      try
      {
        return (object) new CustomLetterXRef(Guid.NewGuid().ToString(), FileSystemEntry.Parse(path));
      }
      catch
      {
        return (object) null;
      }
    }
  }
}
