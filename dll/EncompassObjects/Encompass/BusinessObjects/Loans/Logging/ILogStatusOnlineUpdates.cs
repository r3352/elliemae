// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogStatusOnlineUpdates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("9D1AC62D-1FD8-4b27-A595-282F0B332F42")]
  public interface ILogStatusOnlineUpdates
  {
    int Count { get; }

    StatusOnlineUpdate this[int index] { get; }

    void Remove(StatusOnlineUpdate conversation);

    IEnumerator GetEnumerator();
  }
}
