// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FixedRole
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("2D24544A-735E-4cc4-8649-01FC52FFB44B")]
  public enum FixedRole
  {
    None = 0,
    LoanOfficer = 1,
    LoanProcessor = 2,
    LoanCloser = 3,
    Underwriter = 6,
  }
}
