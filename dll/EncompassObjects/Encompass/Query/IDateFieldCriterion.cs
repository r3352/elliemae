// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.IDateFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  [Guid("C3DC77E0-DDAE-464c-8D2F-F7DA2E2879B0")]
  public interface IDateFieldCriterion
  {
    QueryCriterion And(QueryCriterion criterion);

    QueryCriterion Or(QueryCriterion criterion);

    string FieldName { get; set; }

    DateTime Value { get; set; }

    OrdinalFieldMatchType MatchType { get; set; }

    DateFieldMatchPrecision Precision { get; set; }

    QueryCriterion Clone();

    void SetEmptyDateValue();

    void SetNonEmptyDateValue();
  }
}
