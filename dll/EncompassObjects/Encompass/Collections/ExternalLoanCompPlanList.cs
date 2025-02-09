// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalLoanCompPlanList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalLoanCompPlanList : ListBase, IExternalLoanCompPlanList
  {
    public ExternalLoanCompPlanList()
      : base(typeof (ExternalLoanCompPlan))
    {
    }

    public ExternalLoanCompPlanList(IList source)
      : base(typeof (ExternalLoanCompPlan), source)
    {
    }

    public ExternalLoanCompPlan this[int index]
    {
      get => (ExternalLoanCompPlan) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalLoanCompPlan value) => this.List.Add((object) value);

    public bool Contains(ExternalLoanCompPlan value) => this.List.Contains((object) value);

    public void Remove(ExternalLoanCompPlan value) => this.List.Remove((object) value);

    public ExternalLoanCompPlan[] ToArray()
    {
      ExternalLoanCompPlan[] array = new ExternalLoanCompPlan[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
