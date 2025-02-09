// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.LoanReportDataList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Reporting;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>
  /// Represents a list of <see cref="T:EllieMae.Encompass.Reporting.LoanReportData">LoanReportData</see>
  /// objects.
  /// </summary>
  public class LoanReportDataList : ListBase, ILoanReportDataList
  {
    /// <summary>Constructs a new, empty list.</summary>
    public LoanReportDataList()
      : base(typeof (LoanReportData))
    {
    }

    /// <summary>
    /// Constructs a list initialized from the specified source.
    /// </summary>
    /// <param name="source">The list of items copied into the new object.</param>
    public LoanReportDataList(IList source)
      : base(typeof (LoanReportData), source)
    {
    }

    /// <summary>
    /// Provides access to an item from the list using its index within the list.
    /// </summary>
    public LoanReportData this[int index]
    {
      get => (LoanReportData) this.List[index];
      set => this.List[index] = (object) value;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="value">The item to be added.</param>
    public void Add(LoanReportData value) => this.List.Add((object) value);

    /// <summary>Adds multiple items to the list.</summary>
    /// <param name="values"></param>
    public void AddRange(ICollection values)
    {
      foreach (LoanReportData loanReportData in (IEnumerable) values)
        this.Add(loanReportData);
    }

    /// <summary>Determines if the list contains the specified item.</summary>
    /// <param name="value">The item to search for in the list.</param>
    /// <returns>Returns a boolean indication if the specified item is in the list.</returns>
    public bool Contains(LoanReportData value) => this.List.Contains((object) value);

    /// <summary>Returns the index of the specified item.</summary>
    /// <param name="value">The value to search for in the list.</param>
    /// <returns>The index of the specified item, or -1 if not found.</returns>
    public int IndexOf(LoanReportData value) => this.List.IndexOf((object) value);

    /// <summary>
    /// Inserts a new item into the list at the specified index.
    /// </summary>
    /// <param name="index">The index at which the item will be inserted.</param>
    /// <param name="value">The item to be inserted.</param>
    public void Insert(int index, LoanReportData value) => this.List.Insert(index, (object) value);

    /// <summary>Removes an item from the list.</summary>
    /// <param name="value">The item to be removed from the list.</param>
    public void Remove(LoanReportData value) => this.List.Remove((object) value);

    /// <summary>Converts the list to an Array.</summary>
    /// <returns>An array containing the items from the list.</returns>
    public LoanReportData[] ToArray()
    {
      LoanReportData[] array = new LoanReportData[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
