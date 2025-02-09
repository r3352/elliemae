// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalLoanCompHistoryList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalLoanCompHistoryList : ListBase, IExternalLoanCompHistoryList
  {
    public ExternalLoanCompHistoryList()
      : base(typeof (ExternalLoanCompHistory))
    {
    }

    public ExternalLoanCompHistoryList(IList source)
      : base(typeof (ExternalLoanCompHistory), source)
    {
    }

    public ExternalLoanCompHistory this[int index]
    {
      get => (ExternalLoanCompHistory) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalLoanCompHistory value) => this.List.Add((object) value);

    public bool Contains(ExternalLoanCompHistory value) => this.List.Contains((object) value);

    public void Remove(ExternalLoanCompHistory value) => this.List.Remove((object) value);

    public ExternalLoanCompHistory[] ToArray()
    {
      ExternalLoanCompHistory[] array = new ExternalLoanCompHistory[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
