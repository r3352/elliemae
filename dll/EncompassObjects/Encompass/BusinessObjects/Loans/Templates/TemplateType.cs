// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("D77D4EEB-86E8-4ebf-865F-9AE1795CA73C")]
  public enum TemplateType
  {
    LoanProgram = 1,
    ClosingCost = 2,
    DataTemplate = 3,
    InputFormSet = 4,
    DocumentSet = 5,
    LoanTemplate = 6,
    Investor = 14, // 0x0000000E
    TaskSet = 24, // 0x00000018
  }
}
