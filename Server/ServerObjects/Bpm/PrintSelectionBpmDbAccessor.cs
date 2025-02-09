// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.PrintSelectionBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class PrintSelectionBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "PrintSelectionBpmDbAccessor�";
    private const string CUSTOMLETTERS = "CustomLetters�";
    private const string FORMGROUP = "FormGroup�";

    public PrintSelectionBpmDbAccessor()
      : base(ClientSessionCacheID.PrintSelection)
    {
    }

    protected override Type RuleType => typeof (PrintSelectionRuleInfo);

    protected override string RuleTableName => "BR_PrintSelection";

    [PgReady]
    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from BR_PrintSelection rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select items.* from [PAS_PreselectForms] items");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_PrintSelection] rules on items.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter);
        pgDbQueryBuilder.AppendLine("    order by items.ruleID, items.eventIndex, items.formOrder;");
        return this.dataRowToPrintSelectionRuleInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select * from BR_PrintSelection rules where " + filter);
      dbQueryBuilder.AppendLine("select items.* from [PAS_PreselectForms] items");
      dbQueryBuilder.AppendLine("\tinner join [BR_PrintSelection] rules on items.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("    order by items.ruleID, items.eventIndex, items.formOrder");
      return this.dataRowToPrintSelectionRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    [PgReady]
    private BizRuleInfo[] dataRowToPrintSelectionRuleInfos(DataSet ds)
    {
      DataTable table1 = ds.Tables[0];
      DataTable table2 = ds.Tables[1];
      List<PrintSelectionRuleInfo> selectionRuleInfoList = new List<PrintSelectionRuleInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
        selectionRuleInfoList.Add(this.dataRowToPrintSelectionRuleInfo(row, table2));
      return (BizRuleInfo[]) selectionRuleInfoList.ToArray();
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue keyValue)
    {
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      PrintSelectionRuleInfo selectionRuleInfo = (PrintSelectionRuleInfo) rule;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("PAS_PreselectForms");
      sql.DeleteFrom(table, keyValue);
      for (int index1 = 0; index1 < selectionRuleInfo.Events.Count; ++index1)
      {
        string xml = selectionRuleInfo.Events[index1].ToXml();
        sql.InsertInto(table, new DbValueList()
        {
          keyValue,
          {
            "eventIndex",
            (object) index1
          },
          {
            "formOrder",
            (object) 0
          },
          {
            "eventFormType",
            (object) ""
          },
          {
            "eventFormName",
            (object) xml
          }
        }, true, false);
        for (int index2 = 0; index2 < selectionRuleInfo.Events[index1].SelectedForms.Length; ++index2)
        {
          string path = selectionRuleInfo.Events[index1].SelectedForms[index2].Name;
          if (selectionRuleInfo.Events[index1].SelectedForms[index2].Type == OutputFormType.CustomLetters)
          {
            CustomLetterXRef xref = (CustomLetterXRef) this.generateXRef(path, "CustomLetters");
            if (xref != null)
            {
              path = xref.Guid;
              arrayList1.Add((object) xref);
            }
            else
              continue;
          }
          else if (selectionRuleInfo.Events[index1].SelectedForms[index2].Type == OutputFormType.FormGroup)
          {
            FormGroupXRef xref = (FormGroupXRef) this.generateXRef(path, "FormGroup");
            if (xref != null)
            {
              path = xref.Guid;
              arrayList2.Add((object) xref);
            }
            else
              continue;
          }
          sql.InsertInto(table, new DbValueList()
          {
            keyValue,
            {
              "eventIndex",
              (object) index1
            },
            {
              "formOrder",
              (object) (index2 + 1)
            },
            {
              "eventFormType",
              (object) selectionRuleInfo.Events[index1].SelectedForms[index2].Type.ToString()
            },
            {
              "eventFormName",
              (object) path
            }
          }, true, false);
        }
      }
      if (keyValue != null && keyValue.Value.ToString() != string.Empty)
        sql.AppendLine("delete from PAS_PreselectFormsXRef where ruleID = " + keyValue.Value.ToString());
      else
        sql.AppendLine("delete from PAS_PreselectFormsXRef where ruleID = @ruleId");
      CustomLetterXRef[] array1 = (CustomLetterXRef[]) arrayList1.ToArray(typeof (CustomLetterXRef));
      if (array1 != null && array1.Length != 0)
      {
        for (int index = 0; index < array1.Length; ++index)
        {
          if (array1[index] != null)
          {
            if (keyValue != null && keyValue.Value.ToString() != string.Empty)
              sql.AppendLine("insert into PAS_PreselectFormsXRef([Guid], [ruleID], [XRef], [FormType]) values(" + SQL.Encode((object) array1[index].Guid) + "," + keyValue.Value.ToString() + "," + SQL.Encode((object) array1[index].XRef.ToString()) + ",'CustomLetters')");
            else
              sql.AppendLine("insert into PAS_PreselectFormsXRef([Guid], [ruleID], [XRef], [FormType]) values(" + SQL.Encode((object) array1[index].Guid) + ",@ruleId, " + SQL.Encode((object) array1[index].XRef.ToString()) + ",'CustomLetters')");
          }
        }
      }
      FormGroupXRef[] array2 = (FormGroupXRef[]) arrayList2.ToArray(typeof (FormGroupXRef));
      if (array2 == null || array2.Length == 0)
        return;
      for (int index = 0; index < array2.Length; ++index)
      {
        if (array2[index] != null)
        {
          if (keyValue != null && keyValue.Value.ToString() != string.Empty)
            sql.AppendLine("insert into PAS_PreselectFormsXRef([Guid], [ruleID], [XRef], [FormType]) values(" + SQL.Encode((object) array2[index].Guid) + "," + keyValue.Value.ToString() + "," + SQL.Encode((object) array2[index].XRef.ToString()) + ",'FormGroup')");
          else
            sql.AppendLine("insert into PAS_PreselectFormsXRef([Guid], [ruleID], [XRef], [FormType]) values(" + SQL.Encode((object) array2[index].Guid) + ",@ruleId, " + SQL.Encode((object) array2[index].XRef.ToString()) + ",'FormGroup')");
        }
      }
    }

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue)
    {
      try
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("PAS_PreselectForms");
        sql.DeleteFrom(table, keyValue);
        sql.AppendLine("delete from PAS_PreselectFormsXRef where ruleID = " + keyValue.Value.ToString());
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PrintSelectionBpmDbAccessor), "DeleteAuxiliaryDataFromQuery: Error deleting record in PAS_PreselectForms and PAS_PreselectFormsXRef. Error: " + ex.Message);
      }
    }

    [PgReady]
    private PrintSelectionRuleInfo dataRowToPrintSelectionRuleInfo(DataRow r, DataTable itemTable)
    {
      int ruleId = (int) r["ruleID"];
      XmlSerializer xmlSerializer = new XmlSerializer();
      List<PrintSelectionEvent> printSelectionEventList = new List<PrintSelectionEvent>();
      foreach (DataRow dataRow in itemTable.Select("ruleID = " + (object) ruleId))
      {
        int num = (int) dataRow["eventIndex"];
        if ((int) dataRow["formOrder"] == 0)
          printSelectionEventList.Add((PrintSelectionEvent) xmlSerializer.Deserialize(dataRow["eventFormName"].ToString(), typeof (PrintSelectionEvent)));
      }
      Hashtable hashtable1 = this.readLetterXRefsFromDatabase(ruleId);
      Hashtable hashtable2 = this.readGroupXRefsFromDatabase(ruleId);
      PrintSelectionRuleInfo selectionRuleInfo = new PrintSelectionRuleInfo(r, printSelectionEventList.ToArray());
      foreach (DataRow dataRow in itemTable.Select("ruleID = " + (object) ruleId))
      {
        if ((int) dataRow["formOrder"] > 0)
        {
          int index = (int) dataRow["eventIndex"];
          OutputFormType type = SQL.DecodeEnum<OutputFormType>(dataRow["eventFormType"]);
          string str = (string) dataRow["eventFormName"];
          switch (type)
          {
            case OutputFormType.CustomLetters:
              if (hashtable1.ContainsKey((object) str))
              {
                str = ((CustomLetterXRef) hashtable1[(object) str]).XRef.ToString();
                break;
              }
              continue;
            case OutputFormType.FormGroup:
              if (hashtable2.ContainsKey((object) str))
              {
                str = ((FormGroupXRef) hashtable2[(object) str]).XRef.ToString();
                break;
              }
              continue;
          }
          selectionRuleInfo.Events[index].AddForm(new FormInfo(str, type));
        }
      }
      return selectionRuleInfo;
    }

    [PgReady]
    private Hashtable readLetterXRefsFromDatabase(int ruleId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select Guid, XRef from PAS_PreselectFormsXRef where FormType = 'CustomLetters' AND ruleID = " + (object) ruleId);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
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
            TraceLog.WriteError(nameof (PrintSelectionBpmDbAccessor), "readLetterXRefsFromDatabase:Error reading Print Auto Selection XRef: " + (object) ex);
          }
        }
        return hashtable;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid, XRef from PAS_PreselectFormsXRef where FormType = 'CustomLetters' AND ruleID = " + (object) ruleId);
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      Hashtable hashtable1 = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
      {
        try
        {
          CustomLetterXRef customLetterXref = new CustomLetterXRef(dataRow["Guid"].ToString(), FileSystemEntry.Parse(dataRow["XRef"].ToString()));
          hashtable1.Add((object) customLetterXref.Guid, (object) customLetterXref);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (PrintSelectionBpmDbAccessor), "readLetterXRefsFromDatabase:Error reading Print Auto Selection XRef: " + (object) ex);
        }
      }
      return hashtable1;
    }

    [PgReady]
    private Hashtable readGroupXRefsFromDatabase(int ruleId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select Guid, XRef from PAS_PreselectFormsXRef where FormType = 'FormGroup' AND ruleID = " + (object) ruleId);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        Hashtable hashtable = new Hashtable();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          try
          {
            FormGroupXRef formGroupXref = new FormGroupXRef(dataRow["Guid"].ToString(), FileSystemEntry.Parse(dataRow["XRef"].ToString()));
            hashtable.Add((object) formGroupXref.Guid, (object) formGroupXref);
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(nameof (PrintSelectionBpmDbAccessor), "readGroupXRefsFromDatabase:Error reading Print Auto Selection XRef: " + (object) ex);
          }
        }
        return hashtable;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid, XRef from PAS_PreselectFormsXRef where FormType = 'FormGroup' AND ruleID = " + (object) ruleId);
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      Hashtable hashtable1 = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
      {
        try
        {
          FormGroupXRef formGroupXref = new FormGroupXRef(dataRow["Guid"].ToString(), FileSystemEntry.Parse(dataRow["XRef"].ToString()));
          hashtable1.Add((object) formGroupXref.Guid, (object) formGroupXref);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (PrintSelectionBpmDbAccessor), "readGroupXRefsFromDatabase:Error reading Print Auto Selection XRef: " + (object) ex);
        }
      }
      return hashtable1;
    }

    private object generateXRef(string path, string formType)
    {
      try
      {
        return formType == "FormGroup" ? (object) new FormGroupXRef(Guid.NewGuid().ToString(), FileSystemEntry.Parse(path)) : (object) new CustomLetterXRef(Guid.NewGuid().ToString(), FileSystemEntry.Parse(path));
      }
      catch
      {
        return (object) null;
      }
    }
  }
}
