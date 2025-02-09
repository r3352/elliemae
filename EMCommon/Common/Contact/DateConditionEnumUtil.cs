// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.DateConditionEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class DateConditionEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static DateConditionEnumUtil()
    {
      DateConditionEnumUtil._NameToValue.Add((object) "Is", (object) DateCondition.Is);
      DateConditionEnumUtil._NameToValue.Add((object) "Before", (object) DateCondition.Before);
      DateConditionEnumUtil._NameToValue.Add((object) "After", (object) DateCondition.After);
      DateConditionEnumUtil._NameToValue.Add((object) "Between", (object) DateCondition.Between);
      DateConditionEnumUtil._ValueToName = new Hashtable();
      DateConditionEnumUtil._ValueToName.Add((object) DateCondition.Is, (object) "Is");
      DateConditionEnumUtil._ValueToName.Add((object) DateCondition.Before, (object) "Before");
      DateConditionEnumUtil._ValueToName.Add((object) DateCondition.After, (object) "After");
      DateConditionEnumUtil._ValueToName.Add((object) DateCondition.Between, (object) "Between");
    }

    public static object[] GetDisplayNames()
    {
      DateCondition[] values = (DateCondition[]) Enum.GetValues(typeof (DateCondition));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) DateConditionEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(DateCondition val)
    {
      return (string) DateConditionEnumUtil._ValueToName[(object) val];
    }

    public static DateCondition NameToValue(string name)
    {
      return (DateCondition) DateConditionEnumUtil._NameToValue[(object) name];
    }

    public static bool ContainsName(string name)
    {
      return DateConditionEnumUtil._NameToValue.Contains((object) name);
    }
  }
}
