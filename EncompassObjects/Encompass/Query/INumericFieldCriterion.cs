// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.INumericFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>Interface for NumericFieldCriterion class.</summary>
  /// <exclude />
  [Guid("7931D7B5-570B-40b1-B72F-9FB4E02CF90B")]
  public interface INumericFieldCriterion
  {
    QueryCriterion And(QueryCriterion criterion);

    QueryCriterion Or(QueryCriterion criterion);

    string FieldName { get; set; }

    double Value { get; set; }

    OrdinalFieldMatchType MatchType { get; set; }

    QueryCriterion Clone();
  }
}
