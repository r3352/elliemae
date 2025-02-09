// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IAuditTrailEntryList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("FC679D62-D42E-4211-BEB0-9DB44EB952F6")]
  public interface IAuditTrailEntryList
  {
    AuditTrailEntry this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(AuditTrailEntry value);

    bool Contains(AuditTrailEntry value);

    int IndexOf(AuditTrailEntry value);

    void Insert(int index, AuditTrailEntry value);

    void Remove(AuditTrailEntry value);

    AuditTrailEntry[] ToArray();

    IEnumerator GetEnumerator();
  }
}
