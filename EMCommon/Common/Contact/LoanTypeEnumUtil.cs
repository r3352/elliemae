// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.LoanTypeEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class LoanTypeEnumUtil
  {
    private static Hashtable _NameToValue = new Hashtable();
    private static Hashtable _ValueToName;
    private static Hashtable _ValueToNameInLoan;
    private static Hashtable _NameInLoanToValue;
    private static Hashtable _ValueToNameInContactSearch;
    private static Hashtable _NameInContactSearchToValue;

    static LoanTypeEnumUtil()
    {
      LoanTypeEnumUtil._NameToValue.Add((object) "", (object) LoanTypeEnum.Blank);
      LoanTypeEnumUtil._NameToValue.Add((object) "Conventional", (object) LoanTypeEnum.Conventional);
      LoanTypeEnumUtil._NameToValue.Add((object) "USDA-RHS", (object) LoanTypeEnum.USDA);
      LoanTypeEnumUtil._NameToValue.Add((object) "USDA", (object) LoanTypeEnum.USDA);
      LoanTypeEnumUtil._NameToValue.Add((object) "FHA", (object) LoanTypeEnum.FHA);
      LoanTypeEnumUtil._NameToValue.Add((object) "VA", (object) LoanTypeEnum.VA);
      LoanTypeEnumUtil._NameToValue.Add((object) "HELOC", (object) LoanTypeEnum.HELOC);
      LoanTypeEnumUtil._NameToValue.Add((object) "Other", (object) LoanTypeEnum.Other);
      LoanTypeEnumUtil._ValueToName = new Hashtable();
      LoanTypeEnumUtil._ValueToName.Add((object) LoanTypeEnum.Blank, (object) "");
      LoanTypeEnumUtil._ValueToName.Add((object) LoanTypeEnum.Conventional, (object) "Conventional");
      LoanTypeEnumUtil._ValueToName.Add((object) LoanTypeEnum.USDA, (object) "USDA-RHS");
      LoanTypeEnumUtil._ValueToName.Add((object) LoanTypeEnum.FHA, (object) "FHA");
      LoanTypeEnumUtil._ValueToName.Add((object) LoanTypeEnum.VA, (object) "VA");
      LoanTypeEnumUtil._ValueToName.Add((object) LoanTypeEnum.HELOC, (object) "HELOC");
      LoanTypeEnumUtil._ValueToName.Add((object) LoanTypeEnum.Other, (object) "Other");
      LoanTypeEnumUtil._ValueToNameInLoan = new Hashtable();
      LoanTypeEnumUtil._ValueToNameInLoan.Add((object) LoanTypeEnum.Blank, (object) "");
      LoanTypeEnumUtil._ValueToNameInLoan.Add((object) LoanTypeEnum.Conventional, (object) "Conventional");
      LoanTypeEnumUtil._ValueToNameInLoan.Add((object) LoanTypeEnum.USDA, (object) "FarmersHomeAdministration");
      LoanTypeEnumUtil._ValueToNameInLoan.Add((object) LoanTypeEnum.FHA, (object) "FHA");
      LoanTypeEnumUtil._ValueToNameInLoan.Add((object) LoanTypeEnum.VA, (object) "VA");
      LoanTypeEnumUtil._ValueToNameInLoan.Add((object) LoanTypeEnum.HELOC, (object) "HELOC");
      LoanTypeEnumUtil._ValueToNameInLoan.Add((object) LoanTypeEnum.Other, (object) "Other");
      LoanTypeEnumUtil._NameInLoanToValue = new Hashtable();
      LoanTypeEnumUtil._NameInLoanToValue.Add((object) "", (object) LoanTypeEnum.Blank);
      LoanTypeEnumUtil._NameInLoanToValue.Add((object) "Conventional", (object) LoanTypeEnum.Conventional);
      LoanTypeEnumUtil._NameInLoanToValue.Add((object) "FarmersHomeAdministration", (object) LoanTypeEnum.USDA);
      LoanTypeEnumUtil._NameInLoanToValue.Add((object) "FHA", (object) LoanTypeEnum.FHA);
      LoanTypeEnumUtil._NameInLoanToValue.Add((object) "VA", (object) LoanTypeEnum.VA);
      LoanTypeEnumUtil._NameInLoanToValue.Add((object) "HELOC", (object) LoanTypeEnum.HELOC);
      LoanTypeEnumUtil._NameInLoanToValue.Add((object) "Other", (object) LoanTypeEnum.Other);
      LoanTypeEnumUtil._ValueToNameInContactSearch = new Hashtable();
      LoanTypeEnumUtil._ValueToNameInContactSearch.Add((object) LoanTypeEnum.Blank, (object) "");
      LoanTypeEnumUtil._ValueToNameInContactSearch.Add((object) LoanTypeEnum.Conventional, (object) "Conventional");
      LoanTypeEnumUtil._ValueToNameInContactSearch.Add((object) LoanTypeEnum.USDA, (object) "USDA-RHS");
      LoanTypeEnumUtil._ValueToNameInContactSearch.Add((object) LoanTypeEnum.FHA, (object) "FHA");
      LoanTypeEnumUtil._ValueToNameInContactSearch.Add((object) LoanTypeEnum.VA, (object) "VA");
      LoanTypeEnumUtil._ValueToNameInContactSearch.Add((object) LoanTypeEnum.HELOC, (object) "HELOC");
      LoanTypeEnumUtil._ValueToNameInContactSearch.Add((object) LoanTypeEnum.Other, (object) "Other");
      LoanTypeEnumUtil._NameInContactSearchToValue = new Hashtable();
      LoanTypeEnumUtil._NameInContactSearchToValue.Add((object) "", (object) LoanTypeEnum.Blank);
      LoanTypeEnumUtil._NameInContactSearchToValue.Add((object) "Conventional", (object) LoanTypeEnum.Conventional);
      LoanTypeEnumUtil._NameInContactSearchToValue.Add((object) "USDA-RHS", (object) LoanTypeEnum.USDA);
      LoanTypeEnumUtil._NameInContactSearchToValue.Add((object) "FHA", (object) LoanTypeEnum.FHA);
      LoanTypeEnumUtil._NameInContactSearchToValue.Add((object) "VA", (object) LoanTypeEnum.VA);
      LoanTypeEnumUtil._NameInContactSearchToValue.Add((object) "HELOC", (object) LoanTypeEnum.HELOC);
      LoanTypeEnumUtil._NameInContactSearchToValue.Add((object) "Other", (object) LoanTypeEnum.Other);
    }

    public static IDictionary ValueToNameMap => (IDictionary) LoanTypeEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      LoanTypeEnum[] values = (LoanTypeEnum[]) Enum.GetValues(typeof (LoanTypeEnum));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) LoanTypeEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static object[] GetDisplayNamesInContactSearch()
    {
      LoanTypeEnum[] values = (LoanTypeEnum[]) Enum.GetValues(typeof (LoanTypeEnum));
      object[] namesInContactSearch = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        namesInContactSearch[index] = (object) LoanTypeEnumUtil.ValueToNameInContactSearch(values[index]);
      return namesInContactSearch;
    }

    public static string ValueToName(LoanTypeEnum val)
    {
      return (string) LoanTypeEnumUtil._ValueToName[(object) val];
    }

    public static string ValueToNameInLoan(LoanTypeEnum val)
    {
      return (string) LoanTypeEnumUtil._ValueToNameInLoan[(object) val];
    }

    public static LoanTypeEnum NameToValue(string name)
    {
      return LoanTypeEnumUtil._NameToValue.Contains((object) name) ? (LoanTypeEnum) LoanTypeEnumUtil._NameToValue[(object) name] : LoanTypeEnum.Blank;
    }

    public static LoanTypeEnum NameInLoanToValue(string name)
    {
      return LoanTypeEnumUtil._NameInLoanToValue.Contains((object) name) ? (LoanTypeEnum) LoanTypeEnumUtil._NameInLoanToValue[(object) name] : LoanTypeEnum.Blank;
    }

    public static string ValueToNameInContactSearch(LoanTypeEnum val)
    {
      return (string) LoanTypeEnumUtil._ValueToNameInContactSearch[(object) val];
    }

    public static LoanTypeEnum NameInContactSearchToValue(string name)
    {
      return LoanTypeEnumUtil._NameInContactSearchToValue.Contains((object) name) ? (LoanTypeEnum) LoanTypeEnumUtil._NameInContactSearchToValue[(object) name] : LoanTypeEnum.Blank;
    }
  }
}
