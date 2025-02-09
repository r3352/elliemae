// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanMortgages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("437A9C18-C246-4989-9E7E-BED2D6D1F693")]
  public interface ILoanMortgages
  {
    int Count { get; }

    int Add(IntegerList liabilities);

    void RemoveAt(int index);

    void AttachMortgage(int index, IntegerList liabilities);

    IntegerList GetLiabilities(int index);
  }
}
