// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FieldRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FieldRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public Hashtable RequiredFields;
    public Hashtable FieldRules;

    public FieldRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public FieldRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public FieldRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      Hashtable requiredFields,
      Hashtable fieldRules)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, requiredFields, fieldRules)
    {
    }

    public FieldRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      Hashtable requiredFields,
      Hashtable fieldRules)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.RequiredFields = requiredFields;
      this.FieldRules = fieldRules;
    }

    public FieldRuleInfo(DataRow dataRow, Hashtable requiredFields, Hashtable fieldRules)
      : base(dataRow)
    {
      this.RequiredFields = requiredFields;
      this.FieldRules = fieldRules;
    }

    public FieldRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      XmlList<FRElement> xmlList = info.GetValue<XmlList<FRElement>>(nameof (FieldRules));
      this.FieldRules = new Hashtable();
      this.RequiredFields = new Hashtable();
      foreach (FRElement frElement in (List<FRElement>) xmlList)
      {
        this.RequiredFields[(object) frElement.FieldID] = (object) frElement.RequiredFields;
        this.FieldRules[(object) frElement.FieldID] = frElement.Rule;
      }
    }

    public override BizRuleType RuleType => BizRuleType.FieldRules;

    public override object Clone()
    {
      return (object) new FieldRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.RequiredFields, this.FieldRules);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      XmlList<FRElement> xmlList = new XmlList<FRElement>();
      foreach (string key in (IEnumerable) this.FieldRules.Keys)
        xmlList.Add(new FRElement(key, (string[]) this.RequiredFields[(object) key], this.FieldRules[(object) key]));
      info.AddValue("FieldRules", (object) xmlList);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      FieldRuleInfo fieldRuleInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in fieldRuleInfo.GetBaseFields())
        yield return baseField;
      if (fieldRuleInfo.FieldRules != null && fieldRuleInfo.FieldRules.Count > 0)
      {
        foreach (object key in (IEnumerable) fieldRuleInfo.FieldRules.Keys)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, key.ToString());
      }
      if (fieldRuleInfo.RequiredFields != null && fieldRuleInfo.RequiredFields.Count > 0)
      {
        foreach (object key in (IEnumerable) fieldRuleInfo.RequiredFields.Keys)
        {
          if (fieldRuleInfo.RequiredFields[key] != null && fieldRuleInfo.RequiredFields[key].GetType() == typeof (string[]))
          {
            string[] strArray = (string[]) fieldRuleInfo.RequiredFields[key];
            for (int index = 0; index < strArray.Length; ++index)
              yield return new KeyValuePair<RelationshipType, string>(RelationshipType.IsPrerequesiteOf, strArray[index]);
            strArray = (string[]) null;
          }
        }
      }
    }
  }
}
