// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.IStringFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  [Guid("81F19380-9523-467e-AC04-86F1621BA320")]
  public interface IStringFieldCriterion
  {
    QueryCriterion And(QueryCriterion criterion);

    QueryCriterion Or(QueryCriterion criterion);

    string FieldName { get; set; }

    string Value { get; set; }

    StringFieldMatchType MatchType { get; set; }

    bool Include { get; set; }

    QueryCriterion Clone();
  }
}
