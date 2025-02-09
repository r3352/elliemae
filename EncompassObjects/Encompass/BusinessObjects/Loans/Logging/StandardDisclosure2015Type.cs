// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosure2015Type
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Standard Disclosure-2015 type enum.</summary>
  [Flags]
  [Guid("2B860D8A-43FD-4eb3-BD7A-FAB9FF4856C4")]
  public enum StandardDisclosure2015Type
  {
    /// <summary>No standard disclosure</summary>
    None = 0,
    /// <summary>The Provider List disclosure</summary>
    PROVIDERLIST = 1,
    /// <summary>The Safe Harbor disclosure</summary>
    SAFEHARBOR = 2,
    /// <summary>The Safe Harbor and Provider List disclosures</summary>
    SAFEHARBORPROVIDERLIST = SAFEHARBOR | PROVIDERLIST, // 0x00000003
    /// <summary>The Closing Disclosure disclosure</summary>
    CD = 4,
    /// <summary>The Closing Disclosure and Provider List disclosures</summary>
    CDPROVIDERLIST = CD | PROVIDERLIST, // 0x00000005
    /// <summary>The Closing Disclosure and Safe Harbor disclosures</summary>
    CDSAFEHARBOR = CD | SAFEHARBOR, // 0x00000006
    /// <summary>The Closing Disclosure, Provider List and Safe Harbor disclosures</summary>
    CDPROVIDERLISTSAFEHARBOR = CDSAFEHARBOR | PROVIDERLIST, // 0x00000007
    /// <summary>The Loan Estimate disclosure</summary>
    LE = 8,
    /// <summary>The Loan Estimate and Provider List disclosures</summary>
    LEPROVIDERLIST = LE | PROVIDERLIST, // 0x00000009
    /// <summary>The Loan Estimate and Safe Harbor disclosures</summary>
    LESAFEHARBOR = LE | SAFEHARBOR, // 0x0000000A
    /// <summary>The Loan Estimate, Provider List and Safe Harbor Disclosures</summary>
    LEPROVIDERLISTSAFEHARBOR = LESAFEHARBOR | PROVIDERLIST, // 0x0000000B
    /// <summary>The Provider List - No Fee disclosure</summary>
    PROVIDERLISTNOFEE = 16, // 0x00000010
    /// <summary>The Safe Harbor and Provider List disclosures</summary>
    SAFEHARBORPROVIDERLISTNOFEE = PROVIDERLISTNOFEE | SAFEHARBOR, // 0x00000012
    /// <summary>The Closing Disclosure and Provider List - No Fee disclosures</summary>
    CDPROVIDERLISTNOFEE = PROVIDERLISTNOFEE | CD, // 0x00000014
    /// <summary>The Closing Disclosure, Provider List and Safe Harbor disclosures</summary>
    CDPROVIDERLISTNOFEESAFEHARBOR = CDPROVIDERLISTNOFEE | SAFEHARBOR, // 0x00000016
    /// <summary>The Loan Estimate and Provider List - No Fee disclosures</summary>
    LEPROVIDERLISTNOFEE = PROVIDERLISTNOFEE | LE, // 0x00000018
    /// <summary>The Loan Estimate, Provider List - No Fee and Safe Harbor Disclosures</summary>
    LEPROVIDERLISTNOFEESAFEHARBOR = LEPROVIDERLISTNOFEE | SAFEHARBOR, // 0x0000001A
  }
}
