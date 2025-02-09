// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMDataTable
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMDataTable : BizRuleInfo, IFieldSearchable
  {
    public const string FLD_DT_ID = "dataTableID�";
    public const string FLD_DT_NAME = "dataTableName�";
    public const string FLD_DT_DESC = "dataTableDesc�";
    public const string FLD_DT_TYPE = "dataTableType�";
    public const string FLD_LAST_MOD_DT = "lastModTime�";
    public const string FLD_LAST_MOD_USERID = "lastModByUserID�";
    public const string FLD_LAST_MOD_USER_FN = "lastModifiedByFullName�";
    public const string FLD_FIELD_ID_LIST = "fieldIdList�";
    public const string FLD_OUTPUT_ID_LIST = "outputIdList�";
    private const int DT_CRITERIA_CALCULATION = 17;
    private const int DT_CRITERIA_IGNOREVALUEINLOANFILE = 24;
    private const int DT_CRITERIA_NONE = -1;

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string DataTableType { get; set; }

    public string LastModDt { get; set; }

    public string LastModByUserID { get; set; }

    public string LastModByFullName { get; set; }

    public string FieldIdList { get; set; }

    public string OutputIdList { get; set; }

    public Dictionary<int, List<DDMDataTableFieldValue>> FieldValues { get; set; }

    public string OriginalName { get; set; }

    [Obsolete]
    public DDMDataTable(
      int id,
      string name,
      string description,
      string lastModDt,
      string lastModByUid,
      string lastModByUfn,
      string dataTableType,
      string fieldIdList)
      : base(id, name, BizRule.Condition.Null, (string) null, (string) null, (string) null, (string) null)
    {
      this.Id = id;
      this.Name = name;
      this.Description = description;
      this.LastModDt = lastModDt;
      this.LastModByUserID = lastModByUid;
      this.LastModByFullName = lastModByUfn;
      this.DataTableType = dataTableType;
      this.FieldIdList = fieldIdList;
    }

    public DDMDataTable(
      int id,
      string name,
      string description,
      string lastModDt,
      string lastModByUid,
      string lastModByUfn,
      string dataTableType,
      string fieldIdList,
      string outputIdList)
      : base(id, name, BizRule.Condition.Null, (string) null, (string) null, (string) null, (string) null)
    {
      this.Id = id;
      this.Name = name;
      this.Description = description;
      this.LastModDt = lastModDt;
      this.LastModByUserID = lastModByUid;
      this.LastModByFullName = lastModByUfn;
      this.DataTableType = dataTableType;
      this.FieldIdList = fieldIdList;
      this.OutputIdList = outputIdList;
    }

    public DDMDataTable(
      int id,
      string name,
      string description,
      string lastModDt,
      string lastModByUid,
      string lastModByUfn,
      string dataTableType,
      string fieldIdList,
      string outputIdList,
      Dictionary<int, List<DDMDataTableFieldValue>> fieldValues)
      : base(id, name, BizRule.Condition.Null, (string) null, (string) null, (string) null, (string) null)
    {
      this.Id = id;
      this.Name = name;
      this.Description = description;
      this.LastModDt = lastModDt;
      this.LastModByUserID = lastModByUid;
      this.LastModByFullName = lastModByUfn;
      this.DataTableType = dataTableType;
      this.FieldIdList = fieldIdList;
      this.OutputIdList = outputIdList;
      this.FieldValues = fieldValues;
    }

    public DDMDataTable(DataRow row)
      : base((int) row["dataTableID"], (string) row["dataTableName"], BizRule.Condition.Null, (string) null, (string) null, (string) null, (string) null)
    {
      this.Id = (int) row["dataTableID"];
      this.Name = (string) row["dataTableName"];
      this.Description = (string) row["dataTableDesc"];
      this.LastModDt = Convert.ToDateTime(row["lastModTime"]).ToString();
      this.LastModByUserID = (string) row["lastModByUserID"];
      this.LastModByFullName = (string) row["lastModifiedByFullName"];
      this.DataTableType = (string) row["dataTableType"];
      this.FieldIdList = (string) row["fieldIdList"];
      this.OutputIdList = (string) row["outputIdList"];
    }

    public override object Clone()
    {
      return (object) new DDMDataTable(-1, "Copy of " + this.Name, this.Description, DateTime.Now.ToString(), this.LastModByUserID, this.LastModByFullName, this.DataTableType, this.FieldIdList, this.OutputIdList, this.FieldValues);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      List<string> uniqueFieldIds = new List<string>();
      if (!string.IsNullOrWhiteSpace(this.FieldIdList))
      {
        string[] strArray1 = this.FieldIdList.Split('|');
        int fieldIndex = 0;
        bool seenOutputCol = false;
        string[] strArray;
        int index;
        if (this.FieldValues != null && this.FieldValues.Count > 0)
        {
          strArray = strArray1;
          for (index = 0; index < strArray.Length; ++index)
          {
            string fieldId = strArray[index];
            if (!seenOutputCol)
            {
              List<DDMDataTableFieldValue> dataTableFieldValueList = new List<DDMDataTableFieldValue>();
              foreach (KeyValuePair<int, List<DDMDataTableFieldValue>> fieldValue in this.FieldValues)
              {
                if (fieldValue.Value[fieldIndex].IsOutput)
                {
                  seenOutputCol = true;
                  break;
                }
                if (fieldValue.Value[fieldIndex].Criteria != 24 && fieldValue.Value[fieldIndex].Criteria != -1 && !uniqueFieldIds.Contains(fieldId))
                {
                  uniqueFieldIds.Add(fieldId);
                  yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, fieldId);
                }
              }
              ++fieldIndex;
              fieldId = (string) null;
            }
            else
              break;
          }
          strArray = (string[]) null;
        }
        if (this.FieldValues != null && this.FieldValues.Count > 0)
        {
          foreach (KeyValuePair<int, List<DDMDataTableFieldValue>> fieldValue in this.FieldValues)
          {
            if (fieldValue.Value[fieldValue.Value.Count - 1].Criteria == 17)
            {
              strArray = FieldReplacementRegex.ParseDependentFields(fieldValue.Value[fieldValue.Value.Count - 1].Values);
              for (index = 0; index < strArray.Length; ++index)
              {
                string str = strArray[index];
                if (!uniqueFieldIds.Contains(str))
                {
                  uniqueFieldIds.Add(str);
                  yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, str);
                }
              }
              strArray = (string[]) null;
            }
          }
        }
      }
    }

    public override BizRuleType RuleType => BizRuleType.DDMDataTables;
  }
}
