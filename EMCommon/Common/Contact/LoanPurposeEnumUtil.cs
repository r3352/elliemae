// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.LoanPurposeEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class LoanPurposeEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;
    private static Hashtable _ValueToNameInLoan;
    private static Hashtable _NameInLoanToValue;
    private static Hashtable _ValueToNameInContactSearch;
    private static Hashtable _NameInContactSearchToValue;

    static LoanPurposeEnumUtil()
    {
      LoanPurposeEnumUtil._NameToValue.Add((object) "", (object) LoanPurpose.Blank);
      LoanPurposeEnumUtil._NameToValue.Add((object) "Purchase", (object) LoanPurpose.Purchase);
      LoanPurposeEnumUtil._NameToValue.Add((object) "Cash-Out Refi", (object) LoanPurpose.CashOutRefi);
      LoanPurposeEnumUtil._NameToValue.Add((object) "CashOutRefi", (object) LoanPurpose.CashOutRefi);
      LoanPurposeEnumUtil._NameToValue.Add((object) "Cash Out Refi", (object) LoanPurpose.CashOutRefi);
      LoanPurposeEnumUtil._NameToValue.Add((object) "Construction", (object) LoanPurpose.Construction);
      LoanPurposeEnumUtil._NameToValue.Add((object) "Construction - Perm", (object) LoanPurpose.ConstructionPerm);
      LoanPurposeEnumUtil._NameToValue.Add((object) "ConstructionPerm", (object) LoanPurpose.ConstructionPerm);
      LoanPurposeEnumUtil._NameToValue.Add((object) "Construction - Permanent", (object) LoanPurpose.ConstructionPerm);
      LoanPurposeEnumUtil._NameToValue.Add((object) "No Cash-Out Refi", (object) LoanPurpose.NoCashOutRefi);
      LoanPurposeEnumUtil._NameToValue.Add((object) "NoCashOutRefi", (object) LoanPurpose.NoCashOutRefi);
      LoanPurposeEnumUtil._NameToValue.Add((object) "No Cash Out Refi", (object) LoanPurpose.NoCashOutRefi);
      LoanPurposeEnumUtil._NameToValue.Add((object) "Other", (object) LoanPurpose.Other);
      LoanPurposeEnumUtil._ValueToName = new Hashtable();
      LoanPurposeEnumUtil._ValueToName.Add((object) LoanPurpose.Blank, (object) "");
      LoanPurposeEnumUtil._ValueToName.Add((object) LoanPurpose.CashOutRefi, (object) "Cash-Out Refi");
      LoanPurposeEnumUtil._ValueToName.Add((object) LoanPurpose.Construction, (object) "Construction");
      LoanPurposeEnumUtil._ValueToName.Add((object) LoanPurpose.ConstructionPerm, (object) "Construction - Perm");
      LoanPurposeEnumUtil._ValueToName.Add((object) LoanPurpose.NoCashOutRefi, (object) "No Cash-Out Refi");
      LoanPurposeEnumUtil._ValueToName.Add((object) LoanPurpose.Other, (object) "Other");
      LoanPurposeEnumUtil._ValueToName.Add((object) LoanPurpose.Purchase, (object) "Purchase");
      LoanPurposeEnumUtil._ValueToNameInLoan = new Hashtable();
      LoanPurposeEnumUtil._ValueToNameInLoan.Add((object) LoanPurpose.Blank, (object) "");
      LoanPurposeEnumUtil._ValueToNameInLoan.Add((object) LoanPurpose.CashOutRefi, (object) "Cash-Out Refinance");
      LoanPurposeEnumUtil._ValueToNameInLoan.Add((object) LoanPurpose.Construction, (object) "ConstructionOnly");
      LoanPurposeEnumUtil._ValueToNameInLoan.Add((object) LoanPurpose.ConstructionPerm, (object) "ConstructionToPermanent");
      LoanPurposeEnumUtil._ValueToNameInLoan.Add((object) LoanPurpose.NoCashOutRefi, (object) "NoCash-Out Refinance");
      LoanPurposeEnumUtil._ValueToNameInLoan.Add((object) LoanPurpose.Other, (object) "Other");
      LoanPurposeEnumUtil._ValueToNameInLoan.Add((object) LoanPurpose.Purchase, (object) "Purchase");
      LoanPurposeEnumUtil._NameInLoanToValue = new Hashtable();
      LoanPurposeEnumUtil._NameInLoanToValue.Add((object) "", (object) LoanPurpose.Blank);
      LoanPurposeEnumUtil._NameInLoanToValue.Add((object) "Cash-Out Refinance", (object) LoanPurpose.CashOutRefi);
      LoanPurposeEnumUtil._NameInLoanToValue.Add((object) "ConstructionOnly", (object) LoanPurpose.Construction);
      LoanPurposeEnumUtil._NameInLoanToValue.Add((object) "ConstructionToPermanent", (object) LoanPurpose.ConstructionPerm);
      LoanPurposeEnumUtil._NameInLoanToValue.Add((object) "NoCash-Out Refinance", (object) LoanPurpose.NoCashOutRefi);
      LoanPurposeEnumUtil._NameInLoanToValue.Add((object) "Other", (object) LoanPurpose.Other);
      LoanPurposeEnumUtil._NameInLoanToValue.Add((object) "Purchase", (object) LoanPurpose.Purchase);
      LoanPurposeEnumUtil._ValueToNameInContactSearch = new Hashtable();
      LoanPurposeEnumUtil._ValueToNameInContactSearch.Add((object) LoanPurpose.Blank, (object) "");
      LoanPurposeEnumUtil._ValueToNameInContactSearch.Add((object) LoanPurpose.CashOutRefi, (object) "Cash Out Refi");
      LoanPurposeEnumUtil._ValueToNameInContactSearch.Add((object) LoanPurpose.Construction, (object) "Construction");
      LoanPurposeEnumUtil._ValueToNameInContactSearch.Add((object) LoanPurpose.ConstructionPerm, (object) "Construction - Permanent");
      LoanPurposeEnumUtil._ValueToNameInContactSearch.Add((object) LoanPurpose.NoCashOutRefi, (object) "No Cash Out Refi");
      LoanPurposeEnumUtil._ValueToNameInContactSearch.Add((object) LoanPurpose.Other, (object) "Other");
      LoanPurposeEnumUtil._ValueToNameInContactSearch.Add((object) LoanPurpose.Purchase, (object) "Purchase");
      LoanPurposeEnumUtil._NameInContactSearchToValue = new Hashtable();
      LoanPurposeEnumUtil._NameInContactSearchToValue.Add((object) "", (object) LoanPurpose.Blank);
      LoanPurposeEnumUtil._NameInContactSearchToValue.Add((object) "Cash Out Refi", (object) LoanPurpose.CashOutRefi);
      LoanPurposeEnumUtil._NameInContactSearchToValue.Add((object) "Construction", (object) LoanPurpose.Construction);
      LoanPurposeEnumUtil._NameInContactSearchToValue.Add((object) "Construction - Permanent", (object) LoanPurpose.ConstructionPerm);
      LoanPurposeEnumUtil._NameInContactSearchToValue.Add((object) "No Cash Out Refi", (object) LoanPurpose.NoCashOutRefi);
      LoanPurposeEnumUtil._NameInContactSearchToValue.Add((object) "Other", (object) LoanPurpose.Other);
      LoanPurposeEnumUtil._NameInContactSearchToValue.Add((object) "Purchase", (object) LoanPurpose.Purchase);
    }

    public static IDictionary ValueToNameMap => (IDictionary) LoanPurposeEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      LoanPurpose[] values = (LoanPurpose[]) Enum.GetValues(typeof (LoanPurpose));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) LoanPurposeEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static object[] GetDisplayNamesInContactSearch()
    {
      LoanPurpose[] values = (LoanPurpose[]) Enum.GetValues(typeof (LoanPurpose));
      object[] namesInContactSearch = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        namesInContactSearch[index] = (object) LoanPurposeEnumUtil.ValueToNameInContactSearch(values[index]);
      return namesInContactSearch;
    }

    public static string ValueToName(LoanPurpose val)
    {
      return (string) LoanPurposeEnumUtil._ValueToName[(object) val];
    }

    public static string ValueToNameInLoan(LoanPurpose val)
    {
      return (string) LoanPurposeEnumUtil._ValueToNameInLoan[(object) val];
    }

    public static LoanPurpose NameToValue(string name)
    {
      return LoanPurposeEnumUtil._NameToValue.Contains((object) name) ? (LoanPurpose) LoanPurposeEnumUtil._NameToValue[(object) name] : LoanPurpose.Blank;
    }

    public static LoanPurpose NameInLoanToValue(string name)
    {
      return LoanPurposeEnumUtil._NameInLoanToValue.Contains((object) name) ? (LoanPurpose) LoanPurposeEnumUtil._NameInLoanToValue[(object) name] : LoanPurpose.Blank;
    }

    public static string ValueToNameInContactSearch(LoanPurpose val)
    {
      return (string) LoanPurposeEnumUtil._ValueToNameInContactSearch[(object) val];
    }

    public static LoanPurpose NameInContactSearchToValue(string name)
    {
      return LoanPurposeEnumUtil._NameInContactSearchToValue.Contains((object) name) ? (LoanPurpose) LoanPurposeEnumUtil._NameInContactSearchToValue[(object) name] : LoanPurpose.Blank;
    }
  }
}
