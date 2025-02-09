// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FieldRuleFieldIDAndType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class FieldRuleFieldIDAndType
  {
    public readonly int RuleID;
    public readonly string RuleName;
    private Hashtable fields = new Hashtable();

    public FieldRuleFieldIDAndType(int ruleID, string ruleName)
    {
      this.RuleID = ruleID;
      this.RuleName = ruleName;
    }

    public void AddField(string fieldID, BizRule.FieldRuleType ruleType)
    {
      this.fields.Add((object) fieldID, (object) ruleType);
    }

    public FieldRuleFieldIDAndType.FieldIDRuleTypePair[] GetFields()
    {
      FieldRuleFieldIDAndType.FieldIDRuleTypePair[] fields = new FieldRuleFieldIDAndType.FieldIDRuleTypePair[this.fields.Count];
      int num = 0;
      foreach (string key in (IEnumerable) this.fields.Keys)
        fields[num++] = new FieldRuleFieldIDAndType.FieldIDRuleTypePair(key, (BizRule.FieldRuleType) this.fields[(object) key]);
      return fields;
    }

    public class FieldIDRuleTypePair
    {
      public readonly string FieldID;
      public readonly BizRule.FieldRuleType RuleType;

      public FieldIDRuleTypePair(string fieldID, BizRule.FieldRuleType ruleType)
      {
        this.FieldID = fieldID;
        this.RuleType = ruleType;
      }
    }
  }
}
