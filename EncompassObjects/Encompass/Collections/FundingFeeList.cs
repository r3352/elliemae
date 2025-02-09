// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.FundingFeeList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>
  /// Represents a list of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FundingFee">FundingFee</see>
  /// objects.
  /// </summary>
  public class FundingFeeList : ListBase, IFundingFeeList
  {
    /// <summary>Constructs a new, empty list.</summary>
    internal FundingFeeList()
      : base(typeof (FundingFee))
    {
    }

    /// <summary>
    /// Constructs a list initialized from the specified source.
    /// </summary>
    /// <param name="source">The list of items copied into the new object.</param>
    internal FundingFeeList(IList source)
      : base(typeof (FundingFee), source)
    {
    }

    /// <summary>
    /// Provides access to an item from the list using its index within the list.
    /// </summary>
    public FundingFee this[int index]
    {
      get => (FundingFee) this.List[index];
      set => this.List[index] = (object) value;
    }

    /// <summary>Determines if the list contains the specified item.</summary>
    /// <param name="value">The item to search for in the list.</param>
    /// <returns>Returns a boolean indication if the specified item is in the list.</returns>
    public bool Contains(FundingFee value) => this.List.Contains((object) value);

    /// <summary>Returns the index of the specified item.</summary>
    /// <param name="value">The value to search for in the list.</param>
    /// <returns>The index of the specified item, or -1 if not found.</returns>
    public int IndexOf(FundingFee value) => this.List.IndexOf((object) value);

    /// <summary>Converts the list to an Array.</summary>
    /// <returns>An array containing the items from the list.</returns>
    public FundingFee[] ToArray()
    {
      FundingFee[] array = new FundingFee[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
