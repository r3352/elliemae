// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMFieldRule
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMFieldRule
  {
    public const string DDMFieldRuleID = "ruleID�";
    public const string RULE_NAME = "ruleName�";
    public const string FIELDS = "fields�";
    public const string DDMFeeRuleStatus = "status�";
    public const string INIT_LE_SENT = "initLESent�";
    public const string CONDITION = "condition�";
    public const string CONDITION_STATE = "conditionState�";
    public const string LAST_MOD_TIME = "LastModTime�";
    public const string LAST_MOD_BY_FULLNAME = "lastModifiedByFullName�";
    public const string LAST_MOD_BY_USERID = "lastModByUserID�";
    public const string CREATE_DT = "createDT�";
    public const string ADVCODECONDXML = "advCodeCondXML�";

    public int RuleID { get; set; }

    public string RuleName { get; set; }

    [CLSCompliant(false)]
    public string Fields { get; set; }

    public bool InitLESent { get; set; }

    public string ConditionState { get; set; }

    [CLSCompliant(false)]
    public bool Condition { get; set; }

    public ruleStatus Status { get; set; }

    public string UpdateDt { get; set; }

    public string LastModByUserID { get; set; }

    public string LastModByFullName { get; set; }

    public string CreateDt { get; set; }

    public string AdvCodeConditionXML { get; set; }

    public List<DDMFieldRuleScenario> Scenarios { get; set; }

    public DDMFieldRule(int ruleID) => this.RuleID = ruleID;

    public DDMFieldRule(
      string rulename,
      string fields,
      bool initLEsent,
      ruleStatus status,
      string conditionState,
      bool condition,
      string userID = null,
      string userFullName = null,
      string createDt = null,
      string updateDt = null,
      string advCodeCondXML = "�")
    {
      this.RuleName = rulename;
      this.Fields = fields;
      this.InitLESent = initLEsent;
      this.ConditionState = conditionState;
      this.Condition = condition;
      this.Status = status;
      this.LastModByUserID = userID;
      this.LastModByFullName = userFullName;
      this.UpdateDt = updateDt;
      this.CreateDt = createDt;
      this.AdvCodeConditionXML = advCodeCondXML;
      this.Scenarios = new List<DDMFieldRuleScenario>();
    }

    public DDMFieldRule(DataRow row)
    {
      this.RuleID = (int) row["ruleID"];
      this.RuleName = row["ruleName"] == DBNull.Value ? (string) null : row["ruleName"].ToString();
      if (row["status"] != null)
        this.Status = (ruleStatus) Convert.ToInt16(row["status"]);
      this.Fields = (string) row["fields"];
      this.ConditionState = (string) row["conditionState"];
      this.UpdateDt = row["LastModTime"] == null ? (string) null : Convert.ToDateTime(row["LastModTime"]).ToString();
      this.LastModByUserID = (string) row["lastModByUserID"];
      this.LastModByFullName = (string) row["lastModifiedByFullName"];
      this.CreateDt = row["createDT"] == null ? (string) null : Convert.ToDateTime(row["createDT"]).ToString();
      this.AdvCodeConditionXML = row["advCodeCondXML"] == DBNull.Value ? (string) null : (string) row["advCodeCondXML"];
      if (row["initLESent"] != null)
        this.InitLESent = Convert.ToBoolean(row["initLESent"]);
      if (row["condition"] == null)
        return;
      this.Condition = Convert.ToBoolean(row["condition"]);
    }

    public object Clone()
    {
      return (object) new DDMFieldRule("Copy of " + this.RuleName, this.Fields, this.InitLESent, ruleStatus.Inactive, this.ConditionState, this.Condition, advCodeCondXML: "");
    }

    public DDMFieldRule shallowClone() => (DDMFieldRule) this.MemberwiseClone();
  }
}
