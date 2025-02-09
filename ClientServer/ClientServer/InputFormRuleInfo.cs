// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.InputFormRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class InputFormRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public string[] Forms;

    public InputFormRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public InputFormRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public InputFormRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      string[] forms)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, forms)
    {
    }

    public InputFormRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      string[] forms)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.Forms = forms;
      this.CommentsTxt = comments;
    }

    public InputFormRuleInfo(DataRow ruleInfo, DataRow[] forms)
      : base(ruleInfo)
    {
      if (forms == null)
        return;
      this.Forms = new string[forms.Length];
      for (int index = 0; index < forms.Length; ++index)
        this.Forms[index] = forms[index]["formID"].ToString();
    }

    public InputFormRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.Forms = info.GetValue<XmlStringList>(nameof (Forms)).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.InputForms;

    public override object Clone()
    {
      return (object) new InputFormRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.Forms);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Forms", (object) new XmlStringList(this.Forms));
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      foreach (KeyValuePair<RelationshipType, string> baseField in this.GetBaseFields())
        yield return baseField;
    }
  }
}
