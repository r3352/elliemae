// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanPurposeEnums
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  [Flags]
  [Guid("A9BA5485-DAA5-4C9C-A10B-06651B11004A")]
  public enum ExternalLoanPurposeEnums
  {
    None = 0,
    Purchase = 1,
    NoCashOutRefi = 2,
    CashOutRefi = 4,
    Construction = 8,
    ConstructionPerm = 16, // 0x00000010
    Other = 32, // 0x00000020
  }
}
