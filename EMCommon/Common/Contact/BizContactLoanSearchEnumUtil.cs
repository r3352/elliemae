// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.BizContactLoanSearchEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class BizContactLoanSearchEnumUtil
  {
    private static Hashtable _NameToValue = new Hashtable();
    private static Hashtable _ValueToName;

    static BizContactLoanSearchEnumUtil()
    {
      BizContactLoanSearchEnumUtil._NameToValue.Add((object) "", (object) BizContactLoanSearchEnum.Blank);
      BizContactLoanSearchEnumUtil._NameToValue.Add((object) "Any Closed Loan", (object) BizContactLoanSearchEnum.AnyClosed);
      BizContactLoanSearchEnumUtil._NameToValue.Add((object) "Last Closed Loan", (object) BizContactLoanSearchEnum.LastClosed);
      BizContactLoanSearchEnumUtil._ValueToName = new Hashtable();
      BizContactLoanSearchEnumUtil._ValueToName.Add((object) BizContactLoanSearchEnum.Blank, (object) "");
      BizContactLoanSearchEnumUtil._ValueToName.Add((object) BizContactLoanSearchEnum.AnyClosed, (object) "Any Closed Loan");
      BizContactLoanSearchEnumUtil._ValueToName.Add((object) BizContactLoanSearchEnum.LastClosed, (object) "Last Closed Loan");
    }

    public static IDictionary ValueToNameMap
    {
      get => (IDictionary) BizContactLoanSearchEnumUtil._ValueToName;
    }

    public static object[] GetDisplayNames()
    {
      BizContactLoanSearchEnum[] values = (BizContactLoanSearchEnum[]) Enum.GetValues(typeof (BizContactLoanSearchEnum));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) BizContactLoanSearchEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(BizContactLoanSearchEnum val)
    {
      return (string) BizContactLoanSearchEnumUtil._ValueToName[(object) val];
    }

    public static BizContactLoanSearchEnum NameToValue(string name)
    {
      return BizContactLoanSearchEnumUtil._NameToValue.Contains((object) name) ? (BizContactLoanSearchEnum) BizContactLoanSearchEnumUtil._NameToValue[(object) name] : BizContactLoanSearchEnum.Blank;
    }
  }
}
