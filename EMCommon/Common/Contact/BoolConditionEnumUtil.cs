// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.BoolConditionEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class BoolConditionEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static BoolConditionEnumUtil()
    {
      BoolConditionEnumUtil._NameToValue.Add((object) "Is", (object) BoolCondition.Is);
      BoolConditionEnumUtil._NameToValue.Add((object) "Is not", (object) BoolCondition.IsNot);
      BoolConditionEnumUtil._ValueToName = new Hashtable();
      BoolConditionEnumUtil._ValueToName.Add((object) BoolCondition.Is, (object) "Is");
      BoolConditionEnumUtil._ValueToName.Add((object) BoolCondition.IsNot, (object) "Is not");
    }

    public static object[] GetDisplayNames()
    {
      BoolCondition[] values = (BoolCondition[]) Enum.GetValues(typeof (BoolCondition));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) BoolConditionEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(BoolCondition val)
    {
      return (string) BoolConditionEnumUtil._ValueToName[(object) val];
    }

    public static BoolCondition NameToValue(string name)
    {
      return (BoolCondition) BoolConditionEnumUtil._NameToValue[(object) name];
    }

    public static bool ContainsName(string name)
    {
      return BoolConditionEnumUtil._NameToValue.Contains((object) name);
    }
  }
}
