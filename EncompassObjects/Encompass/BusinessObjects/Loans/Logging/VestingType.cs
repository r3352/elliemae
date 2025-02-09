// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.VestingType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>The enumeration of the different Vesting Type</summary>
  [Flags]
  [Guid("A67D0D2A-1A7E-4A58-A1CD-338C807526F6")]
  public enum VestingType
  {
    /// <summary>Vesting Type - Individual</summary>
    Individual = 0,
    /// <summary>Vesting Type - Cosigner</summary>
    Cosigner = 1,
    /// <summary>Vesting Type - TitleOnly</summary>
    TitleOnly = 2,
    /// <summary>Vesting Type - NonTitleSpouse</summary>
    NonTitleSpouse = TitleOnly | Cosigner, // 0x00000003
    /// <summary>Vesting Type - Trustee</summary>
    Trustee = 4,
    /// <summary>Vesting Type - TitleOnlyTrustee</summary>
    TitleOnlyTrustee = Trustee | Cosigner, // 0x00000005
    /// <summary>Vesting Type - SettlorTrustee</summary>
    SettlorTrustee = Trustee | TitleOnly, // 0x00000006
    /// <summary>Vesting Type - Settlor</summary>
    Settlor = SettlorTrustee | Cosigner, // 0x00000007
    /// <summary>Vesting Type - TitleOnlySettlorTrustee</summary>
    TitleOnlySettlorTrustee = 8,
    /// <summary>Vesting Type - Officer</summary>
    Officer = TitleOnlySettlorTrustee | Cosigner, // 0x00000009
    /// <summary>No Vesting Type Selected</summary>
    None = TitleOnlySettlorTrustee | TitleOnly, // 0x0000000A
  }
}
