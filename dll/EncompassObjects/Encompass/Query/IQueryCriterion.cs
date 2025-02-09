// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.IQueryCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  [Guid("1F823875-81E2-4de2-A6BA-9B7BF7B661FF")]
  public interface IQueryCriterion
  {
    QueryCriterion And(QueryCriterion criterion);

    QueryCriterion Or(QueryCriterion criterion);

    QueryCriterion Clone();
  }
}
