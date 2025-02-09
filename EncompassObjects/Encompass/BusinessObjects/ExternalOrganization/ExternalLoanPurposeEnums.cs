// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanPurposeEnums
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
  /// Defines the possible loan purpose for an external organization
  /// </summary>
  [Flags]
  [Guid("A9BA5485-DAA5-4C9C-A10B-06651B11004A")]
  public enum ExternalLoanPurposeEnums
  {
    /// <summary>No External Loan Purpose Selected</summary>
    None = 0,
    /// <summary>Purchase Loan</summary>
    Purchase = 1,
    /// <summary>NoCashOutRefi Loan</summary>
    NoCashOutRefi = 2,
    /// <summary>CashOutRefi Loan</summary>
    CashOutRefi = 4,
    /// <summary>Construction Loan</summary>
    Construction = 8,
    /// <summary>ConstructionPerm Loan</summary>
    ConstructionPerm = 16, // 0x00000010
    /// <summary>Other Loan</summary>
    Other = 32, // 0x00000020
  }
}
