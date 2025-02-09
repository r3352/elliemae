// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.BorContactLoanSearchEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class BorContactLoanSearchEnumUtil
  {
    private static Hashtable _NameToValue = new Hashtable();
    private static Hashtable _ValueToName;

    static BorContactLoanSearchEnumUtil()
    {
      BorContactLoanSearchEnumUtil._NameToValue.Add((object) "", (object) BorContactLoanSearchEnum.Blank);
      BorContactLoanSearchEnumUtil._NameToValue.Add((object) "Any Originated Loan", (object) BorContactLoanSearchEnum.AnyOriginated);
      BorContactLoanSearchEnumUtil._NameToValue.Add((object) "Last Originated Loan", (object) BorContactLoanSearchEnum.LastOriginated);
      BorContactLoanSearchEnumUtil._NameToValue.Add((object) "Any Closed Loan", (object) BorContactLoanSearchEnum.AnyCompleted);
      BorContactLoanSearchEnumUtil._NameToValue.Add((object) "Last Closed Loan", (object) BorContactLoanSearchEnum.LastCompleted);
      BorContactLoanSearchEnumUtil._NameToValue.Add((object) "Any Completed Loan", (object) BorContactLoanSearchEnum.AnyCompleted);
      BorContactLoanSearchEnumUtil._NameToValue.Add((object) "Last Completed Loan", (object) BorContactLoanSearchEnum.LastCompleted);
      BorContactLoanSearchEnumUtil._ValueToName = new Hashtable();
      BorContactLoanSearchEnumUtil._ValueToName.Add((object) BorContactLoanSearchEnum.Blank, (object) "");
      BorContactLoanSearchEnumUtil._ValueToName.Add((object) BorContactLoanSearchEnum.AnyOriginated, (object) "Any Originated Loan");
      BorContactLoanSearchEnumUtil._ValueToName.Add((object) BorContactLoanSearchEnum.LastOriginated, (object) "Last Originated Loan");
      BorContactLoanSearchEnumUtil._ValueToName.Add((object) BorContactLoanSearchEnum.AnyCompleted, (object) "Any Completed Loan");
      BorContactLoanSearchEnumUtil._ValueToName.Add((object) BorContactLoanSearchEnum.LastCompleted, (object) "Last Completed Loan");
    }

    public static IDictionary ValueToNameMap
    {
      get => (IDictionary) BorContactLoanSearchEnumUtil._ValueToName;
    }

    public static object[] GetDisplayNames()
    {
      BorContactLoanSearchEnum[] values = (BorContactLoanSearchEnum[]) Enum.GetValues(typeof (BorContactLoanSearchEnum));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) BorContactLoanSearchEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(BorContactLoanSearchEnum val)
    {
      return (string) BorContactLoanSearchEnumUtil._ValueToName[(object) val];
    }

    public static BorContactLoanSearchEnum NameToValue(string name)
    {
      return BorContactLoanSearchEnumUtil._NameToValue.Contains((object) name) ? (BorContactLoanSearchEnum) BorContactLoanSearchEnumUtil._NameToValue[(object) name] : BorContactLoanSearchEnum.Blank;
    }
  }
}
