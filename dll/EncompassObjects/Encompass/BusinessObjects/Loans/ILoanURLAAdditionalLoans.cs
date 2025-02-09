// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanURLAAdditionalLoans
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("E56114E1-6419-49A6-BFBB-E781623AFD60")]
  public interface ILoanURLAAdditionalLoans
  {
    int Count { get; }

    int Add();

    void RemoveAt(int index);
  }
}
