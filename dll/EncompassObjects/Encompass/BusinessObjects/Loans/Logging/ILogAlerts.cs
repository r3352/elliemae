// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogAlerts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("4D190FB9-7290-457a-8F84-FFF5921D537D")]
  public interface ILogAlerts
  {
    int Count { get; }

    LogAlert Add(Role roleToAlert, DateTime dueDate);

    LogAlert this[int index] { get; }

    void Remove(LogAlert alertToRemove);

    IEnumerator GetEnumerator();
  }
}
