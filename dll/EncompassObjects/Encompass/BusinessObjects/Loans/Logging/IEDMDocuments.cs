// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IEDMDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("25F83120-8DE0-49e5-B86A-76263E22963E")]
  public interface IEDMDocuments
  {
    int Count { get; }

    EDMDocument this[int index] { get; }

    IEnumerator GetEnumerator();
  }
}
