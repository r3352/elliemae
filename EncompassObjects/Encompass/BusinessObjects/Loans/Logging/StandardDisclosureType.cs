// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosureType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Provides a bitmask to indicate one or more standard disclosures.
  /// </summary>
  [Flags]
  [Guid("2B860D8A-43FD-4eb3-BD7A-FAB9FF4856C4")]
  public enum StandardDisclosureType
  {
    /// <summary>No standard disclosure</summary>
    None = 0,
    /// <summary>The Good Faith Estimate disclosure</summary>
    GFE = 1,
    /// <summary>The Turth in Lending disclosure</summary>
    TIL = 2,
    /// <summary>Both the GFE and the TIL disclosures</summary>
    GFETIL = TIL | GFE, // 0x00000003
    /// <summary>The Safe Harbor disclosure</summary>
    SAFEHARBOR = 4,
    /// <summary>The Safe Harbor AND GFE disclosures</summary>
    SAFEHARBORGFE = SAFEHARBOR | GFE, // 0x00000005
    /// <summary>The Safe Harbor and TIL disclosures</summary>
    SAFEHARBORTIL = SAFEHARBOR | TIL, // 0x00000006
    /// <summary>The Safe Harbor, GFE, and TIL disclosures</summary>
    SAFEHARBORGFETIL = SAFEHARBORTIL | GFE, // 0x00000007
  }
}
