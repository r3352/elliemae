// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanImportFormat
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Enumerates the supported import formats for a loan file.
  /// </summary>
  [Guid("2E91D8F1-DF95-328E-8D7C-CBAF3F919C8B")]
  public enum LoanImportFormat
  {
    /// <summary>Fannie Mae verion 3.0 or 3.2 format.</summary>
    FNMA3X = 1,
    /// <summary>Fannie Mae verion MISMO 3.4 format.</summary>
    FNMA34 = 2,
    /// <summary>iLAD format</summary>
    ILAD = 3,
  }
}
