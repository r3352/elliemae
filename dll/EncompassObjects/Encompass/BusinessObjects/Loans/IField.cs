// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("FB1F8EBF-F958-4b5e-BB30-0C322104CDEC")]
  public interface IField
  {
    string ID { get; }

    LoanFieldFormat Format { get; }

    string UnformattedValue { get; }

    string Value { get; set; }

    FieldDescriptor Descriptor { get; }

    bool IsEmpty();

    string FormattedValue { get; }

    int ToInt();

    [return: MarshalAs(UnmanagedType.Currency)]
    Decimal ToDecimal();

    DateTime ToDate();
  }
}
