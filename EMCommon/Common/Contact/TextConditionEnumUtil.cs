// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.TextConditionEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class TextConditionEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static TextConditionEnumUtil()
    {
      TextConditionEnumUtil._NameToValue.Add((object) "Is (exact)", (object) TextCondition.Is);
      TextConditionEnumUtil._NameToValue.Add((object) "Is Not", (object) TextCondition.IsNot);
      TextConditionEnumUtil._NameToValue.Add((object) "Starts with", (object) TextCondition.StartsWith);
      TextConditionEnumUtil._NameToValue.Add((object) "Contains", (object) TextCondition.Contains);
      TextConditionEnumUtil._ValueToName = new Hashtable();
      TextConditionEnumUtil._ValueToName.Add((object) TextCondition.Is, (object) "Is (exact)");
      TextConditionEnumUtil._ValueToName.Add((object) TextCondition.IsNot, (object) "Is Not");
      TextConditionEnumUtil._ValueToName.Add((object) TextCondition.StartsWith, (object) "Starts with");
      TextConditionEnumUtil._ValueToName.Add((object) TextCondition.Contains, (object) "Contains");
    }

    public static object[] GetDisplayNames()
    {
      TextCondition[] values = (TextCondition[]) Enum.GetValues(typeof (TextCondition));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) TextConditionEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(TextCondition val)
    {
      return (string) TextConditionEnumUtil._ValueToName[(object) val];
    }

    public static TextCondition NameToValue(string name)
    {
      return (TextCondition) TextConditionEnumUtil._NameToValue[(object) name];
    }

    public static bool ContainsName(string name)
    {
      return TextConditionEnumUtil._NameToValue.Contains((object) name);
    }
  }
}
