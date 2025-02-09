// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.AuditTrailEntryList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class AuditTrailEntryList : ListBase, IAuditTrailEntryList
  {
    public AuditTrailEntryList()
      : base(typeof (AuditTrailEntry))
    {
    }

    public AuditTrailEntryList(IList source)
      : base(typeof (AuditTrailEntry), source)
    {
    }

    public AuditTrailEntry this[int index]
    {
      get => (AuditTrailEntry) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(AuditTrailEntry value) => this.List.Add((object) value);

    public bool Contains(AuditTrailEntry value) => this.List.Contains((object) value);

    public int IndexOf(AuditTrailEntry value) => this.List.IndexOf((object) value);

    public void Insert(int index, AuditTrailEntry value) => this.List.Insert(index, (object) value);

    public void Remove(AuditTrailEntry value) => this.List.Remove((object) value);

    public AuditTrailEntry[] ToArray()
    {
      AuditTrailEntry[] array = new AuditTrailEntry[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
