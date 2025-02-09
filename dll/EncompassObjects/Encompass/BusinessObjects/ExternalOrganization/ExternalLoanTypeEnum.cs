// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanTypeEnum
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  [Flags]
  [Guid("83EC5A3A-D0B4-4CDC-B5F0-F3A1289981E7")]
  public enum ExternalLoanTypeEnum
  {
    None = 0,
    Conventional = 1,
    FHA = 2,
    VA = 4,
    USDA = 8,
    HELOC = 16, // 0x00000010
    Other = 32, // 0x00000020
    FirstLien = 64, // 0x00000040
    SecondLien = 128, // 0x00000080
  }
}
