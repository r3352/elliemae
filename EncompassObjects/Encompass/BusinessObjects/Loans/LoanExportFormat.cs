// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanExportFormat
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Enumerates the supported export formats for a loan file.
  /// </summary>
  [Guid("38799532-82D0-34DE-BDDC-2D75AAB98D44")]
  public enum LoanExportFormat
  {
    /// <summary>Fannie Mae verion 3.0 format.</summary>
    FNMA30,
    /// <summary>Fannie Mae verion 3.2 format.</summary>
    FNMA32,
    /// <summary>MISMO Closing v2.3.1 format.</summary>
    CLOSING231,
    /// <summary>CountryWide LEDA export format.</summary>
    CWLEDA,
    /// <summary>MISMO Closing v2.4 format.</summary>
    CLOSING24,
    /// <summary>MISMO Closing v2.6 format.</summary>
    CLOSING26,
    /// <summary>MISMO AUS v2.3.1 format.</summary>
    AUS231,
    /// <summary>MISMO AUS v2.4 format.</summary>
    AUS24,
    /// <exclude>Encompass Data Replication Services format.</exclude>
    EDRS,
    /// <summary> ULDD Loan Deliver.</summary>
    LOANDELIVERY,
  }
}
