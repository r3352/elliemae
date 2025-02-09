// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.LoanPurpose
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Defines the loan purposes which can be assigned to a loan.
  /// </summary>
  [Guid("17B92736-7FF8-4c3f-BA90-9AF3BA4C32E4")]
  public enum LoanPurpose
  {
    /// <summary>The loan purpose has not been specified.</summary>
    Unspecified,
    /// <summary>The loan is for the purchase of property.</summary>
    Purchase,
    /// <summary>The loan is a cash-out refinance.</summary>
    CashOutRefi,
    /// <summary>The loan is a no-cash-out refinance.</summary>
    NoCashOutRefi,
    /// <summary>The loan is a construction loan.</summary>
    Construction,
    /// <summary>The loan is a construction-to-permanent loan.</summary>
    ConstructionPerm,
    /// <summary>The loan is another, custom type of loan.</summary>
    Other,
  }
}
