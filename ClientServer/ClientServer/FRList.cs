// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FRList
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FRList : IFieldRuleDefinition, IXmlSerializable
  {
    public readonly string[] List;
    public readonly bool IsLock;

    public FRList(string[] list, bool isLock)
    {
      this.List = list;
      this.IsLock = isLock;
    }

    public FRList(string[] list, BizRule.FieldRuleType fieldRuleType)
    {
      if (fieldRuleType != BizRule.FieldRuleType.ListLock && fieldRuleType != BizRule.FieldRuleType.ListUnlock)
        throw new Exception("Invalid field rule type " + (object) fieldRuleType);
      this.IsLock = fieldRuleType == BizRule.FieldRuleType.ListLock;
      this.List = list;
    }

    public FRList(XmlSerializationInfo info)
    {
      this.List = info.GetValue<XmlStringList>(nameof (List)).ToArray();
      this.IsLock = info.GetBoolean(nameof (IsLock));
    }

    public BizRule.FieldRuleType RuleType
    {
      get => !this.IsLock ? BizRule.FieldRuleType.ListUnlock : BizRule.FieldRuleType.ListLock;
    }

    public override string ToString() => string.Join("\n", this.List);

    public static FRList Parse(string text, BizRule.FieldRuleType fieldRuleType)
    {
      string[] list = text.Split('\n');
      for (int index = 0; index < list.Length; ++index)
        list[index] = list[index].Trim();
      return new FRList(list, fieldRuleType);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("List", (object) new XmlStringList(this.List));
      info.AddValue("IsLock", (object) this.IsLock);
    }
  }
}
