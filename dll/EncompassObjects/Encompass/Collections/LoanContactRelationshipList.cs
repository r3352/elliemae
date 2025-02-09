// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.LoanContactRelationshipList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class LoanContactRelationshipList : ListBase, ILoanContactRelationshipList
  {
    public LoanContactRelationshipList()
      : base(typeof (LoanContactRelationship))
    {
    }

    public LoanContactRelationshipList(IList source)
      : base(typeof (LoanContactRelationship), source)
    {
    }

    public LoanContactRelationship this[int index]
    {
      get => (LoanContactRelationship) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(LoanContactRelationship value) => this.List.Add((object) value);

    public bool Contains(LoanContactRelationship value) => this.List.Contains((object) value);

    public int IndexOf(LoanContactRelationship value) => this.List.IndexOf((object) value);

    public void Insert(int index, LoanContactRelationship value)
    {
      this.List.Insert(index, (object) value);
    }

    public void Remove(LoanContactRelationship value) => this.List.Remove((object) value);

    public LoanContactRelationship[] ToArray()
    {
      LoanContactRelationship[] array = new LoanContactRelationship[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
