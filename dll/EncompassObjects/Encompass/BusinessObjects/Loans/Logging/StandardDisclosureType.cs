// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosureType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Flags]
  [Guid("2B860D8A-43FD-4eb3-BD7A-FAB9FF4856C4")]
  public enum StandardDisclosureType
  {
    None = 0,
    GFE = 1,
    TIL = 2,
    GFETIL = TIL | GFE, // 0x00000003
    SAFEHARBOR = 4,
    SAFEHARBORGFE = SAFEHARBOR | GFE, // 0x00000005
    SAFEHARBORTIL = SAFEHARBOR | TIL, // 0x00000006
    SAFEHARBORGFETIL = SAFEHARBORTIL | GFE, // 0x00000007
  }
}
