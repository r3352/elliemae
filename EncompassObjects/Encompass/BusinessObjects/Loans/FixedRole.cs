// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FixedRole
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents one of the fixed roles within Encompass.</summary>
  /// <remarks>Encompass defines several fixed roles which are used to determine certain behaviors
  /// within the loan process. In the Broker Edition, these fixed roles map to the corresponding
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> objects of the same name. In the Banker Edition, the administrator
  /// can set their own mapping of the fixed roles to their customized roles.</remarks>
  [Guid("2D24544A-735E-4cc4-8649-01FC52FFB44B")]
  public enum FixedRole
  {
    /// <summary>None</summary>
    None = 0,
    /// <summary>LoanOfficer</summary>
    LoanOfficer = 1,
    /// <summary>LoanProcessor</summary>
    LoanProcessor = 2,
    /// <summary>LoanCloser</summary>
    LoanCloser = 3,
    /// <summary>Underwriter</summary>
    Underwriter = 6,
  }
}
