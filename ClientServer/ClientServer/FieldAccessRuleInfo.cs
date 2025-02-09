// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FieldAccessRuleInfo
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
  public class FieldAccessRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public EllieMae.EMLite.ClientServer.FieldAccessRights[] FieldAccessRights;

    public FieldAccessRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public FieldAccessRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public FieldAccessRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      EllieMae.EMLite.ClientServer.FieldAccessRights[] fieldAccessRights)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, fieldAccessRights)
    {
    }

    public FieldAccessRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      EllieMae.EMLite.ClientServer.FieldAccessRights[] fieldAccessRights)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.FieldAccessRights = fieldAccessRights;
    }

    public FieldAccessRuleInfo(DataRow dataRow, EllieMae.EMLite.ClientServer.FieldAccessRights[] fieldAccessRights)
      : base(dataRow)
    {
      this.FieldAccessRights = fieldAccessRights;
    }

    public FieldAccessRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.FieldAccessRights = ((List<EllieMae.EMLite.ClientServer.FieldAccessRights>) info.GetValue(nameof (FieldAccessRights), typeof (XmlList<EllieMae.EMLite.ClientServer.FieldAccessRights>))).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.FieldAccess;

    public override object Clone()
    {
      return (object) new FieldAccessRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.FieldAccessRights);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      XmlList<EllieMae.EMLite.ClientServer.FieldAccessRights> xmlList = new XmlList<EllieMae.EMLite.ClientServer.FieldAccessRights>();
      if (this.FieldAccessRights != null)
        xmlList.AddRange((IEnumerable<EllieMae.EMLite.ClientServer.FieldAccessRights>) this.FieldAccessRights);
      info.AddValue("FieldAccessRights", (object) xmlList);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      FieldAccessRuleInfo fieldAccessRuleInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in fieldAccessRuleInfo.GetBaseFields())
        yield return baseField;
      EllieMae.EMLite.ClientServer.FieldAccessRights[] fieldAccessRightsArray = fieldAccessRuleInfo.FieldAccessRights;
      for (int index = 0; index < fieldAccessRightsArray.Length; ++index)
      {
        EllieMae.EMLite.ClientServer.FieldAccessRights fieldAccessRights = fieldAccessRightsArray[index];
        if (fieldAccessRights != null && fieldAccessRights.AccessRights != null && fieldAccessRights.AccessRights.Keys.Count > 0)
        {
          bool flag = true;
          foreach (int key in (IEnumerable) fieldAccessRights.AccessRights.Keys)
          {
            if (BizRule.FieldAccessRight.DoesNotApply != (BizRule.FieldAccessRight) fieldAccessRights.AccessRights[(object) key])
            {
              flag = false;
              break;
            }
          }
          if (!flag)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, fieldAccessRights.FieldID);
        }
      }
      fieldAccessRightsArray = (EllieMae.EMLite.ClientServer.FieldAccessRights[]) null;
    }
  }
}
