// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalLoanCompPlanList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>ExternalLoanCompPlanList Class</summary>
  public class ExternalLoanCompPlanList : ListBase, IExternalLoanCompPlanList
  {
    /// <summary>Constructs a new, empty list.</summary>
    public ExternalLoanCompPlanList()
      : base(typeof (ExternalLoanCompPlan))
    {
    }

    /// <summary>
    /// Constructs a list initialized from the specified source.
    /// </summary>
    /// <param name="source">The list of items copied into the new object.</param>
    public ExternalLoanCompPlanList(IList source)
      : base(typeof (ExternalLoanCompPlan), source)
    {
    }

    /// <summary>
    /// Provides access to an item from the list using its index within the list.
    /// </summary>
    public ExternalLoanCompPlan this[int index]
    {
      get => (ExternalLoanCompPlan) this.List[index];
      set => this.List[index] = (object) value;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="value">The item to be added.</param>
    public void Add(ExternalLoanCompPlan value) => this.List.Add((object) value);

    /// <summary>Determines if the list contains the specified item.</summary>
    /// <param name="value">The item to search for in the list.</param>
    /// <returns>Returns a boolean indication if the specified item is in the list.</returns>
    public bool Contains(ExternalLoanCompPlan value) => this.List.Contains((object) value);

    /// <summary>Removes an item from the list.</summary>
    /// <param name="value">The item to be removed from the list.</param>
    public void Remove(ExternalLoanCompPlan value) => this.List.Remove((object) value);

    /// <summary>Converts the list to an Array.</summary>
    /// <returns>An array containing the items from the list.</returns>
    public ExternalLoanCompPlan[] ToArray()
    {
      ExternalLoanCompPlan[] array = new ExternalLoanCompPlan[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
