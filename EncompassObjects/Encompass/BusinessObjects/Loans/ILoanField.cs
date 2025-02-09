// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for LoanField class.</summary>
  /// <exclude />
  [Guid("FABDFBD5-2F20-47a4-8BF6-341714FC158B")]
  public interface ILoanField
  {
    string ID { get; }

    LoanFieldFormat Format { get; }

    string UnformattedValue { get; }

    string Value { get; set; }

    string OriginalValue { get; }

    bool Locked { get; set; }

    string GetValueForBorrowerPair(BorrowerPair pair);

    void SetValueForBorrowerPair(BorrowerPair pair, string value);

    FieldDescriptor Descriptor { get; }

    bool IsEmpty();

    string FormattedValue { get; }

    int ToInt();

    [return: MarshalAs(UnmanagedType.Currency)]
    Decimal ToDecimal();

    DateTime ToDate();
  }
}
