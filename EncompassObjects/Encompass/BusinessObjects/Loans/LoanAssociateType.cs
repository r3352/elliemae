// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAssociateType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Enumeration for the different types of Loan Associates which can be assigned to a loan.
  /// </summary>
  public enum LoanAssociateType
  {
    /// <summary>No associate is assigned</summary>
    None,
    /// <summary>A User is assigned as the loan associate</summary>
    User,
    /// <summary>A UserGroup is assigned as the loan associate</summary>
    UserGroup,
  }
}
