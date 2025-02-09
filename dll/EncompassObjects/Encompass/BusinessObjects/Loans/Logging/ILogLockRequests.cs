// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogLockRequests
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("A1E35EEA-D97D-4bc8-B90D-3657A926C483")]
  public interface ILogLockRequests
  {
    int Count { get; }

    LockRequest this[int index] { get; }

    LockRequest Add(User requestingUser);

    LockRequest GetCurrent();

    IEnumerator GetEnumerator();
  }
}
