// Decompiled with JetBrains decompiler
// Type: Elli.Service.IPipelineService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain.Mortgage;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public interface IPipelineService
  {
    IList<PipelineItem> PipelineGetItems(
      IList<PipelineField> fields,
      string loanFolder,
      IList<PipelineSortField> sortFields,
      IList<FilterCriterion> filter,
      int maxResults,
      int pageIndex,
      int pageSize);
  }
}
