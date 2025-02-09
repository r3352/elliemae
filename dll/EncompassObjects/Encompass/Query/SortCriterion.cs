// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.SortCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.Encompass.Query
{
  public class SortCriterion : ISortCriterion
  {
    private string fieldName = "";
    private SortOrder sortOrder;
    private DataConversion conversion;

    public SortCriterion()
    {
    }

    public SortCriterion(string fieldName) => this.FieldName = fieldName;

    public SortCriterion(string fieldName, SortOrder sortOrder)
    {
      this.FieldName = fieldName;
      this.SortOrder = sortOrder;
    }

    public SortCriterion(string fieldName, SortOrder sortOrder, DataConversion conversion)
    {
      this.FieldName = fieldName;
      this.SortOrder = sortOrder;
      this.Conversion = conversion;
    }

    public string FieldName
    {
      get => this.fieldName;
      set => this.fieldName = value ?? "";
    }

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

    public DataConversion Conversion
    {
      get => this.conversion;
      set
      {
        this.conversion = Enum.GetName(typeof (DataConversion), (object) value) != null ? value : throw new ArgumentException("Invalid conversion specified");
      }
    }

    public SortField Unwrap()
    {
      if (this.fieldName == "")
        throw new InvalidOperationException("The SortCriterion does not contain a valid field name");
      return new SortField(this.fieldName, (FieldSortOrder) this.sortOrder, (DataConversion) this.conversion);
    }
  }
}
