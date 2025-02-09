// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ServicingTransactionList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ServicingTransactionList : ListBase, IServicingTransactionList
  {
    public ServicingTransactionList()
      : base(typeof (ServicingTransaction))
    {
    }

    public ServicingTransactionList(IList source)
      : base(typeof (ServicingTransaction), source)
    {
    }

    public ServicingTransaction this[int index]
    {
      get => (ServicingTransaction) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ServicingTransaction value) => this.List.Add((object) value);

    public bool Contains(ServicingTransaction value) => this.List.Contains((object) value);

    public int IndexOf(ServicingTransaction value) => this.List.IndexOf((object) value);

    public void Insert(int index, ServicingTransaction value)
    {
      this.List.Insert(index, (object) value);
    }

    public void Remove(ServicingTransaction value) => this.List.Remove((object) value);

    public ServicingTransaction[] ToArray()
    {
      ServicingTransaction[] array = new ServicingTransaction[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
