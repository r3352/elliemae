// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.LienEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class LienEnumUtil
  {
    private static Hashtable _NameToValue = new Hashtable();
    private static Hashtable _ValueToName;
    private static Hashtable _ValueToNameInLoan;
    private static Hashtable _NameInLoanToValue;

    static LienEnumUtil()
    {
      LienEnumUtil._NameToValue.Add((object) "", (object) LienEnum.Blank);
      LienEnumUtil._NameToValue.Add((object) "First", (object) LienEnum.First);
      LienEnumUtil._NameToValue.Add((object) "Second", (object) LienEnum.Second);
      LienEnumUtil._ValueToName = new Hashtable();
      LienEnumUtil._ValueToName.Add((object) LienEnum.Blank, (object) "");
      LienEnumUtil._ValueToName.Add((object) LienEnum.First, (object) "First");
      LienEnumUtil._ValueToName.Add((object) LienEnum.Second, (object) "Second");
      LienEnumUtil._ValueToNameInLoan = new Hashtable();
      LienEnumUtil._ValueToNameInLoan.Add((object) LienEnum.Blank, (object) "");
      LienEnumUtil._ValueToNameInLoan.Add((object) LienEnum.First, (object) "FirstLien");
      LienEnumUtil._ValueToNameInLoan.Add((object) LienEnum.Second, (object) "SecondLien");
      LienEnumUtil._NameInLoanToValue = new Hashtable();
      LienEnumUtil._NameInLoanToValue.Add((object) "", (object) LienEnum.Blank);
      LienEnumUtil._NameInLoanToValue.Add((object) "FirstLien", (object) LienEnum.First);
      LienEnumUtil._NameInLoanToValue.Add((object) "SecondLien", (object) LienEnum.Second);
    }

    public static IDictionary ValueToNameMap => (IDictionary) LienEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      LienEnum[] values = (LienEnum[]) Enum.GetValues(typeof (LienEnum));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) LienEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(LienEnum val)
    {
      return (string) LienEnumUtil._ValueToName[(object) val];
    }

    public static string ValueToNameInLoan(LienEnum val)
    {
      return (string) LienEnumUtil._ValueToNameInLoan[(object) val];
    }

    public static LienEnum NameToValue(string name)
    {
      return LienEnumUtil._NameToValue.Contains((object) name) ? (LienEnum) LienEnumUtil._NameToValue[(object) name] : LienEnum.Blank;
    }

    public static LienEnum NameInLoanToValue(string name)
    {
      return LienEnumUtil._NameInLoanToValue.Contains((object) name) ? (LienEnum) LienEnumUtil._NameInLoanToValue[(object) name] : LienEnum.Blank;
    }
  }
}
