// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactLoan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Interface for ContactLoan class.</summary>
  /// <exclude />
  [Guid("FDAD7C6D-AAAD-4ab1-B038-764FE805901D")]
  public interface IContactLoan
  {
    int ID { get; }

    int BorrowerID { get; }

    string LoanStatus { get; }

    Decimal AppraisedValue { get; }

    Decimal LoanAmount { get; }

    Decimal InterestRate { get; }

    int Term { get; }

    string Purpose { get; }

    Decimal DownPayment { get; }

    Decimal LTV { get; }

    string Amortization { get; }

    DateTime DateCompleted { get; }

    string LoanType { get; }

    string LienPosition { get; }
  }
}
