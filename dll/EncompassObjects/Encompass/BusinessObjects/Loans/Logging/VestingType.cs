// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.VestingType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Flags]
  [Guid("A67D0D2A-1A7E-4A58-A1CD-338C807526F6")]
  public enum VestingType
  {
    Individual = 0,
    Cosigner = 1,
    TitleOnly = 2,
    NonTitleSpouse = TitleOnly | Cosigner, // 0x00000003
    Trustee = 4,
    TitleOnlyTrustee = Trustee | Cosigner, // 0x00000005
    SettlorTrustee = Trustee | TitleOnly, // 0x00000006
    Settlor = SettlorTrustee | Cosigner, // 0x00000007
    TitleOnlySettlorTrustee = 8,
    Officer = TitleOnlySettlorTrustee | Cosigner, // 0x00000009
    None = TitleOnlySettlorTrustee | TitleOnly, // 0x0000000A
  }
}
