// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanTypeEnum
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Defines the possible loan type for an external organization
  /// </summary>
  [Flags]
  [Guid("83EC5A3A-D0B4-4CDC-B5F0-F3A1289981E7")]
  public enum ExternalLoanTypeEnum
  {
    /// <summary>No External LoanType Selected</summary>
    None = 0,
    /// <summary>Conventional Loan Type</summary>
    Conventional = 1,
    /// <summary>FHA Loan Type</summary>
    FHA = 2,
    /// <summary>VA Loan Type</summary>
    VA = 4,
    /// <summary>USDA Loan Type</summary>
    USDA = 8,
    /// <summary>HELOC Loan Type</summary>
    HELOC = 16, // 0x00000010
    /// <summary>Other Loan Type</summary>
    Other = 32, // 0x00000020
    /// <summary>FirstLien Loan Type</summary>
    FirstLien = 64, // 0x00000040
    /// <summary>SecondLien Loan Type</summary>
    SecondLien = 128, // 0x00000080
  }
}
