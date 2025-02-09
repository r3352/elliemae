// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogEDMTransactions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("007C9EAA-E663-443c-AE44-FF65A553C216")]
  public interface ILogEDMTransactions
  {
    int Count { get; }

    EDMTransaction this[int index] { get; }

    void Remove(EDMTransaction conversation);

    IEnumerator GetEnumerator();
  }
}
