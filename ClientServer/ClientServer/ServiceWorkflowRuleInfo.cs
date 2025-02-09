// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServiceWorkflowRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ServiceWorkflowRuleInfo : BizRuleInfo
  {
    public ServiceWorkflowAction[] Actions;
    public string Description;
    public WorkflowServiceType ServiceType;

    public string LoanFieldId { get; set; }

    public DateTime EffectiveDate { get; set; }

    public OrderType OrderType { get; set; }

    public ServiceWorkflowRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public ServiceWorkflowRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName, BizRule.Condition.Null, "1,2,3,4", "", "", (string) null)
    {
    }

    public ServiceWorkflowRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      ServiceWorkflowAction[] actions)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, actions)
    {
    }

    public ServiceWorkflowRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      ServiceWorkflowAction[] actions)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
      this.Actions = actions;
    }

    public ServiceWorkflowRuleInfo(DataRow dataRow, ServiceWorkflowAction[] actions)
      : base(dataRow)
    {
      this.Description = dataRow["description"] != DBNull.Value ? (string) dataRow["description"] : "";
      this.ServiceType = dataRow[nameof (ServiceType)] != DBNull.Value ? (WorkflowServiceType) dataRow[nameof (ServiceType)] : WorkflowServiceType.Service;
      this.LoanFieldId = dataRow[nameof (LoanFieldId)] != DBNull.Value ? (string) dataRow[nameof (LoanFieldId)] : "";
      this.EffectiveDate = dataRow[nameof (EffectiveDate)] != DBNull.Value ? (DateTime) dataRow[nameof (EffectiveDate)] : DateTime.MinValue;
      this.OrderType = dataRow[nameof (OrderType)] != DBNull.Value ? (OrderType) (short) dataRow[nameof (OrderType)] : OrderType.Manual;
      this.Actions = actions;
    }

    public ServiceWorkflowRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.Actions = info.GetValue<XmlList<ServiceWorkflowAction>>(nameof (Actions)).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.ServiceWorkflowRules;

    public override object Clone()
    {
      return (object) new ServiceWorkflowRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.Actions);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Actions", (object) new XmlList<ServiceWorkflowAction>((IEnumerable<ServiceWorkflowAction>) this.Actions));
    }
  }
}
