// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.PropertyUseEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class PropertyUseEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;
    private static Hashtable _ValueToNameInLoan;

    static PropertyUseEnumUtil()
    {
      PropertyUseEnumUtil._NameToValue.Add((object) "", (object) PropertyUse.Blank);
      PropertyUseEnumUtil._NameToValue.Add((object) "Primary", (object) PropertyUse.Primary);
      PropertyUseEnumUtil._NameToValue.Add((object) "Secondary", (object) PropertyUse.Secondary);
      PropertyUseEnumUtil._NameToValue.Add((object) "Investment", (object) PropertyUse.Investment);
      PropertyUseEnumUtil._ValueToName = new Hashtable();
      PropertyUseEnumUtil._ValueToName.Add((object) PropertyUse.Blank, (object) "");
      PropertyUseEnumUtil._ValueToName.Add((object) PropertyUse.Primary, (object) "Primary");
      PropertyUseEnumUtil._ValueToName.Add((object) PropertyUse.Secondary, (object) "Secondary");
      PropertyUseEnumUtil._ValueToName.Add((object) PropertyUse.Investment, (object) "Investment");
      PropertyUseEnumUtil._ValueToNameInLoan = new Hashtable();
      PropertyUseEnumUtil._ValueToNameInLoan.Add((object) PropertyUse.Blank, (object) "");
      PropertyUseEnumUtil._ValueToNameInLoan.Add((object) PropertyUse.Primary, (object) "PrimaryResidence");
      PropertyUseEnumUtil._ValueToNameInLoan.Add((object) PropertyUse.Secondary, (object) "SecondHome");
      PropertyUseEnumUtil._ValueToNameInLoan.Add((object) PropertyUse.Investment, (object) "Investor");
    }

    public static IDictionary ValueToNameMap => (IDictionary) PropertyUseEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      PropertyUse[] values = (PropertyUse[]) Enum.GetValues(typeof (PropertyUse));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) PropertyUseEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(PropertyUse val)
    {
      return (string) PropertyUseEnumUtil._ValueToName[(object) val];
    }

    public static string ValueToNameInLoan(PropertyUse val)
    {
      return (string) PropertyUseEnumUtil._ValueToNameInLoan[(object) val];
    }

    public static PropertyUse NameToValue(string name)
    {
      return PropertyUseEnumUtil._NameToValue.Contains((object) name) ? (PropertyUse) PropertyUseEnumUtil._NameToValue[(object) name] : PropertyUse.Blank;
    }
  }
}
