// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FRElement
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FRElement : IXmlSerializable, IFieldRuleDefinition
  {
    private string fieldId;
    private string[] requiredFields;
    private object rule;

    public FRElement(string fieldId, string[] requiredFields, object rule)
    {
      this.fieldId = fieldId;
      this.requiredFields = requiredFields;
      if (this.requiredFields == null)
        this.requiredFields = new string[0];
      this.rule = rule;
    }

    public FRElement(XmlSerializationInfo info)
    {
      this.fieldId = info.GetString(nameof (FieldID));
      this.requiredFields = info.GetValue<XmlStringList>(nameof (RequiredFields)).ToArray();
      BizRule.FieldRuleType fieldRuleType = info.GetEnum<BizRule.FieldRuleType>(nameof (RuleType));
      switch (fieldRuleType)
      {
        case BizRule.FieldRuleType.Range:
          this.rule = (object) info.GetValue<FRRange>(nameof (Rule));
          break;
        case BizRule.FieldRuleType.ListLock:
        case BizRule.FieldRuleType.ListUnlock:
          this.rule = (object) info.GetValue<FRList>(nameof (Rule));
          break;
        case BizRule.FieldRuleType.Code:
          this.rule = (object) info.GetString(nameof (Rule));
          break;
        default:
          throw new Exception("Invalid rule type (" + (object) fieldRuleType + ") specified");
      }
    }

    public string FieldID => this.fieldId;

    public string[] RequiredFields => this.requiredFields;

    public object Rule => this.rule;

    public BizRule.FieldRuleType RuleType
    {
      get
      {
        if (this.rule is FRList)
          return ((FRList) this.rule).RuleType;
        return this.rule is FRRange ? BizRule.FieldRuleType.Range : BizRule.FieldRuleType.Code;
      }
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("FieldID", (object) this.fieldId);
      info.AddValue("RequiredFields", (object) new XmlStringList(this.requiredFields));
      info.AddValue("RuleType", (object) this.RuleType);
      info.AddValue("Rule", this.rule);
    }
  }
}
