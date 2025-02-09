// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanURLAOtherLiabilities
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("EBD82728-95D6-460F-ADF1-F677A32D02E8")]
  public interface ILoanURLAOtherLiabilities
  {
    int Count { get; }

    int Add();

    void RemoveAt(int index);
  }
}
