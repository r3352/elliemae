// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.SortCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Represents a field in the sort order of a returned set of data.
  /// </summary>
  public class SortCriterion : ISortCriterion
  {
    private string fieldName = "";
    private SortOrder sortOrder;
    private DataConversion conversion;

    /// <summary>
    /// Default constructor for an empty SortCriterion object.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.Query.SortCriterion.FieldName" /> property must be set in order for
    /// this criterion object to be valid.</remarks>
    public SortCriterion()
    {
    }

    /// <summary>
    /// Constructor for a SortCriterion using a specified field.
    /// </summary>
    /// <param name="fieldName">The field on which to sort the records.</param>
    /// <remarks>By default, the sort order created is in ascending order.</remarks>
    public SortCriterion(string fieldName) => this.FieldName = fieldName;

    /// <summary>
    /// Constructor for a SortCriterion where both the field name and order are specified.
    /// </summary>
    /// <param name="fieldName">The field on which to sort the records.</param>
    /// <param name="sortOrder">The order in which to sort the records, either
    /// <see cref="F:EllieMae.Encompass.Query.SortOrder.Ascending" /> or
    /// <see cref="F:EllieMae.Encompass.Query.SortOrder.Descending" />.</param>
    public SortCriterion(string fieldName, SortOrder sortOrder)
    {
      this.FieldName = fieldName;
      this.SortOrder = sortOrder;
    }

    /// <summary>
    /// Constructor for a SortCriterion where both the field name and order are specified.
    /// </summary>
    /// <param name="fieldName">The field on which to sort the records.</param>
    /// <param name="sortOrder">The order in which to sort the records, either
    /// <see cref="F:EllieMae.Encompass.Query.SortOrder.Ascending" /> or
    /// <see cref="F:EllieMae.Encompass.Query.SortOrder.Descending" />.</param>
    /// <param name="conversion">The conversion, if any, which should be applied
    /// to the data field in order to modify the behavior of the sort.</param>
    public SortCriterion(string fieldName, SortOrder sortOrder, DataConversion conversion)
    {
      this.FieldName = fieldName;
      this.SortOrder = sortOrder;
      this.Conversion = conversion;
    }

    /// <summary>
    /// Gets or sets the canonical name of the field on which to sort.
    /// </summary>
    public string FieldName
    {
      get => this.fieldName;
      set => this.fieldName = value ?? "";
    }

    /// <summary>Gets or sets the ordering used for this field.</summary>
    public SortOrder SortOrder
    {
      get => this.sortOrder;
      set
      {
        switch (value)
        {
          case SortOrder.Ascending:
          case SortOrder.Descending:
            this.sortOrder = value;
            break;
          default:
            throw new ArgumentException("Invalid sort order specified");
        }
      }
    }

    /// <summary>
    /// Gets the data conversion to be applied to the field values to perform the sort.
    /// </summary>
    /// <remarks>
    /// Setting the conversion method for a sort criterion can be useful when you
    /// wish to compare the values in a format which is not its default. For example,
    /// if you wish to sort a numeric field alphabetically using its string representation.
    /// </remarks>
    public DataConversion Conversion
    {
      get => this.conversion;
      set
      {
        this.conversion = Enum.GetName(typeof (DataConversion), (object) value) != null ? value : throw new ArgumentException("Invalid conversion specified");
      }
    }

    /// <summary>
    /// This method is meant solely for internal use by Encompass.
    /// </summary>
    /// <returns></returns>
    public SortField Unwrap()
    {
      if (this.fieldName == "")
        throw new InvalidOperationException("The SortCriterion does not contain a valid field name");
      return new SortField(this.fieldName, (FieldSortOrder) this.sortOrder, (EllieMae.EMLite.ClientServer.Query.DataConversion) this.conversion);
    }
  }
}
