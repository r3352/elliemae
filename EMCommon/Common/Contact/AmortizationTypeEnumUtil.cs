// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.AmortizationTypeEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class AmortizationTypeEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;
    private static Hashtable _ValueToNameInLoan;
    private static Hashtable _NameInLoanToValue;
    private static Hashtable _ValueToNameInContactSearch;
    private static Hashtable _NameInContactSearchToValue;

    static AmortizationTypeEnumUtil()
    {
      AmortizationTypeEnumUtil._NameToValue.Add((object) "", (object) AmortizationType.Blank);
      AmortizationTypeEnumUtil._NameToValue.Add((object) "Fixed Rate", (object) AmortizationType.FixedRate);
      AmortizationTypeEnumUtil._NameToValue.Add((object) "FixedRate", (object) AmortizationType.FixedRate);
      AmortizationTypeEnumUtil._NameToValue.Add((object) "GPM", (object) AmortizationType.GPM);
      AmortizationTypeEnumUtil._NameToValue.Add((object) "ARM", (object) AmortizationType.ARM);
      AmortizationTypeEnumUtil._NameToValue.Add((object) "Other", (object) AmortizationType.Other);
      AmortizationTypeEnumUtil._ValueToName = new Hashtable();
      AmortizationTypeEnumUtil._ValueToName.Add((object) AmortizationType.Blank, (object) "");
      AmortizationTypeEnumUtil._ValueToName.Add((object) AmortizationType.FixedRate, (object) "Fixed Rate");
      AmortizationTypeEnumUtil._ValueToName.Add((object) AmortizationType.GPM, (object) "GPM");
      AmortizationTypeEnumUtil._ValueToName.Add((object) AmortizationType.ARM, (object) "ARM");
      AmortizationTypeEnumUtil._ValueToName.Add((object) AmortizationType.Other, (object) "Other");
      AmortizationTypeEnumUtil._ValueToNameInLoan = new Hashtable();
      AmortizationTypeEnumUtil._ValueToNameInLoan.Add((object) AmortizationType.Blank, (object) "");
      AmortizationTypeEnumUtil._ValueToNameInLoan.Add((object) AmortizationType.FixedRate, (object) "Fixed");
      AmortizationTypeEnumUtil._ValueToNameInLoan.Add((object) AmortizationType.GPM, (object) "GraduatedPaymentMortgage");
      AmortizationTypeEnumUtil._ValueToNameInLoan.Add((object) AmortizationType.ARM, (object) "AdjustableRate");
      AmortizationTypeEnumUtil._ValueToNameInLoan.Add((object) AmortizationType.Other, (object) "OtherAmortizationType");
      AmortizationTypeEnumUtil._NameInLoanToValue = new Hashtable();
      AmortizationTypeEnumUtil._NameInLoanToValue.Add((object) "", (object) AmortizationType.Blank);
      AmortizationTypeEnumUtil._NameInLoanToValue.Add((object) "Fixed", (object) AmortizationType.FixedRate);
      AmortizationTypeEnumUtil._NameInLoanToValue.Add((object) "GraduatedPaymentMortgage", (object) AmortizationType.GPM);
      AmortizationTypeEnumUtil._NameInLoanToValue.Add((object) "AdjustableRate", (object) AmortizationType.ARM);
      AmortizationTypeEnumUtil._NameInLoanToValue.Add((object) "OtherAmortizationType", (object) AmortizationType.Other);
      AmortizationTypeEnumUtil._ValueToNameInContactSearch = new Hashtable();
      AmortizationTypeEnumUtil._ValueToNameInContactSearch.Add((object) AmortizationType.Blank, (object) "");
      AmortizationTypeEnumUtil._ValueToNameInContactSearch.Add((object) AmortizationType.FixedRate, (object) "Fixed");
      AmortizationTypeEnumUtil._ValueToNameInContactSearch.Add((object) AmortizationType.GPM, (object) "GPM");
      AmortizationTypeEnumUtil._ValueToNameInContactSearch.Add((object) AmortizationType.ARM, (object) "ARM");
      AmortizationTypeEnumUtil._ValueToNameInContactSearch.Add((object) AmortizationType.Other, (object) "Other");
      AmortizationTypeEnumUtil._NameInContactSearchToValue = new Hashtable();
      AmortizationTypeEnumUtil._NameInContactSearchToValue.Add((object) "", (object) AmortizationType.Blank);
      AmortizationTypeEnumUtil._NameInContactSearchToValue.Add((object) "Fixed", (object) AmortizationType.FixedRate);
      AmortizationTypeEnumUtil._NameInContactSearchToValue.Add((object) "GPM", (object) AmortizationType.GPM);
      AmortizationTypeEnumUtil._NameInContactSearchToValue.Add((object) "ARM", (object) AmortizationType.ARM);
      AmortizationTypeEnumUtil._NameInContactSearchToValue.Add((object) "Other", (object) AmortizationType.Other);
    }

    public static IDictionary ValueToNameMap => (IDictionary) AmortizationTypeEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      AmortizationType[] values = (AmortizationType[]) Enum.GetValues(typeof (AmortizationType));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) AmortizationTypeEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static object[] GetDisplayNamesInContactSearch()
    {
      AmortizationType[] values = (AmortizationType[]) Enum.GetValues(typeof (AmortizationType));
      object[] namesInContactSearch = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        namesInContactSearch[index] = (object) AmortizationTypeEnumUtil.ValueToNameInContactSearch(values[index]);
      return namesInContactSearch;
    }

    public static string ValueToName(AmortizationType val)
    {
      return (string) AmortizationTypeEnumUtil._ValueToName[(object) val];
    }

    public static string ValueToNameInLoan(AmortizationType val)
    {
      return (string) AmortizationTypeEnumUtil._ValueToNameInLoan[(object) val];
    }

    public static AmortizationType NameToValue(string name)
    {
      return AmortizationTypeEnumUtil._NameToValue.Contains((object) name) ? (AmortizationType) AmortizationTypeEnumUtil._NameToValue[(object) name] : AmortizationType.Blank;
    }

    public static AmortizationType NameInLoanToValue(string name)
    {
      return AmortizationTypeEnumUtil._NameInLoanToValue.Contains((object) name) ? (AmortizationType) AmortizationTypeEnumUtil._NameInLoanToValue[(object) name] : AmortizationType.Blank;
    }

    public static string ValueToNameInContactSearch(AmortizationType val)
    {
      return (string) AmortizationTypeEnumUtil._ValueToNameInContactSearch[(object) val];
    }

    public static AmortizationType NameInContactSearchToValue(string name)
    {
      return AmortizationTypeEnumUtil._NameInContactSearchToValue.Contains((object) name) ? (AmortizationType) AmortizationTypeEnumUtil._NameInContactSearchToValue[(object) name] : AmortizationType.Blank;
    }
  }
}
