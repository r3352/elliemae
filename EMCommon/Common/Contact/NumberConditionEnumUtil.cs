// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.NumberConditionEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class NumberConditionEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static NumberConditionEnumUtil()
    {
      NumberConditionEnumUtil._NameToValue.Add((object) "Is", (object) NumberCondition.Is);
      NumberConditionEnumUtil._NameToValue.Add((object) "Greater Than", (object) NumberCondition.GreaterThan);
      NumberConditionEnumUtil._NameToValue.Add((object) "Less Than", (object) NumberCondition.LessThan);
      NumberConditionEnumUtil._NameToValue.Add((object) "Between", (object) NumberCondition.Between);
      NumberConditionEnumUtil._ValueToName = new Hashtable();
      NumberConditionEnumUtil._ValueToName.Add((object) NumberCondition.Is, (object) "Is");
      NumberConditionEnumUtil._ValueToName.Add((object) NumberCondition.GreaterThan, (object) "Greater Than");
      NumberConditionEnumUtil._ValueToName.Add((object) NumberCondition.LessThan, (object) "Less Than");
      NumberConditionEnumUtil._ValueToName.Add((object) NumberCondition.Between, (object) "Between");
    }

    public static object[] GetDisplayNames()
    {
      NumberCondition[] values = (NumberCondition[]) Enum.GetValues(typeof (NumberCondition));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) NumberConditionEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(NumberCondition val)
    {
      return (string) NumberConditionEnumUtil._ValueToName[(object) val];
    }

    public static NumberCondition NameToValue(string name)
    {
      return (NumberCondition) NumberConditionEnumUtil._NameToValue[(object) name];
    }

    public static bool ContainsName(string name)
    {
      return NumberConditionEnumUtil._NameToValue.Contains((object) name);
    }
  }
}
