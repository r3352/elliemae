// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintFormRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PrintFormRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public PrintRequiredFieldsInfo[] FormRules;

    public PrintFormRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public PrintFormRuleInfo(string ruleName)
      : base(ruleName)
    {
    }

    public PrintFormRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      PrintRequiredFieldsInfo[] formRules)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, formRules)
    {
    }

    public PrintFormRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      PrintRequiredFieldsInfo[] formRules)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.FormRules = formRules;
    }

    public PrintFormRuleInfo(DataRow dataRow, PrintRequiredFieldsInfo[] formRules)
      : base(dataRow)
    {
      this.FormRules = formRules;
    }

    public PrintFormRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.FormRules = info.GetValue<XmlList<PrintRequiredFieldsInfo>>(nameof (FormRules)).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.PrintForms;

    public override object Clone()
    {
      return (object) new PrintFormRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.FormRules);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("FormRules", (object) new XmlList<PrintRequiredFieldsInfo>((IEnumerable<PrintRequiredFieldsInfo>) this.FormRules));
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      PrintFormRuleInfo printFormRuleInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in printFormRuleInfo.GetBaseFields())
        yield return baseField;
      foreach (PrintRequiredFieldsInfo requiredFieldsInfo in ((IEnumerable<PrintRequiredFieldsInfo>) printFormRuleInfo.FormRules).Where<PrintRequiredFieldsInfo>((System.Func<PrintRequiredFieldsInfo, bool>) (printRequiredFieldsInfo => printRequiredFieldsInfo != null)))
      {
        PrintRequiredFieldsInfo printRequiredFieldsInfo = requiredFieldsInfo;
        string[] strArray;
        int index;
        if (printRequiredFieldsInfo.FieldIDs != null && printRequiredFieldsInfo.FieldIDs.Length != 0)
        {
          strArray = printRequiredFieldsInfo.FieldIDs;
          for (index = 0; index < strArray.Length; ++index)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, strArray[index]);
          strArray = (string[]) null;
        }
        if (!string.IsNullOrEmpty(printRequiredFieldsInfo.AdvancedCoding))
        {
          strArray = FieldReplacementRegex.ParseDependentFields(printRequiredFieldsInfo.AdvancedCoding);
          for (index = 0; index < strArray.Length; ++index)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, strArray[index]);
          strArray = (string[]) null;
          printRequiredFieldsInfo = (PrintRequiredFieldsInfo) null;
        }
      }
    }
  }
}
