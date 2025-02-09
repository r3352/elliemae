// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMFeeRule
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
  public class DDMFeeRule
  {
    public const string DDMFeeRuleID = "ruleID�";
    public const string RULE_NAME = "ruleName�";
    public const string FEE_LINE = "feeLine�";
    public const string FEE_TYPE = "feeType�";
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

    public string FeeLine { get; set; }

    public type FeeType { get; set; }

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

    public List<DDMFeeRuleScenario> Scenarios { get; set; }

    public DDMFeeRule(int ruleID) => this.RuleID = ruleID;

    public DDMFeeRule(
      string rulename,
      string feelinenumber,
      bool initLEsent,
      ruleStatus status,
      string conditionState,
      bool condition,
      string userID,
      string userFullName,
      string createDt,
      string updateDt,
      string advCodeCondXML)
    {
      this.RuleName = rulename;
      this.FeeLine = feelinenumber;
      this.InitLESent = initLEsent;
      this.ConditionState = conditionState;
      this.Condition = condition;
      this.Status = status;
      this.LastModByUserID = userID;
      this.LastModByFullName = userFullName;
      this.UpdateDt = updateDt;
      this.CreateDt = createDt;
      this.AdvCodeConditionXML = advCodeCondXML;
      this.Scenarios = new List<DDMFeeRuleScenario>();
    }

    public DDMFeeRule(DataRow row)
    {
      this.RuleID = (int) row["ruleID"];
      this.RuleName = row["ruleName"] == DBNull.Value ? (string) null : row["ruleName"].ToString();
      if (row["status"] != DBNull.Value)
        this.Status = (ruleStatus) Convert.ToInt16(row["status"]);
      if (row["feeType"] != DBNull.Value)
        this.FeeType = (type) Convert.ToInt16(row["feeType"]);
      this.FeeLine = row["feeLine"] == DBNull.Value ? (string) null : (string) row["feeLine"];
      this.ConditionState = row["conditionState"] == DBNull.Value ? (string) null : (string) row["conditionState"];
      this.UpdateDt = row["LastModTime"] == DBNull.Value ? (string) null : Convert.ToDateTime(row["LastModTime"]).ToString();
      this.LastModByUserID = row["lastModByUserID"] == DBNull.Value ? (string) null : (string) row["lastModByUserID"];
      this.LastModByFullName = row["lastModifiedByFullName"] == DBNull.Value ? (string) null : (string) row["lastModifiedByFullName"];
      this.CreateDt = row["createDT"] == DBNull.Value ? (string) null : Convert.ToDateTime(row["createDT"]).ToString();
      this.AdvCodeConditionXML = row["advCodeCondXML"] == DBNull.Value ? (string) null : (string) row["advCodeCondXML"];
      if (row["initLESent"] != DBNull.Value)
        this.InitLESent = Convert.ToBoolean(row["initLESent"]);
      if (row["condition"] == DBNull.Value)
        return;
      this.Condition = Convert.ToBoolean(row["condition"]);
    }

    public DDMFeeRule shallowClone() => (DDMFeeRule) this.MemberwiseClone();
  }
}
