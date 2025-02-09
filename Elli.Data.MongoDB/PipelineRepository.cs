// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.PipelineRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Mortgage;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class PipelineRepository : IPipelineRepository
  {
    static PipelineRepository() => MongoInitializer.Initialize();

    public IList<PipelineItem> PipelineGetItems(
      string userId,
      IList<PipelineField> fields,
      string loanFolder,
      IList<PipelineSortField> sortFields,
      IList<FilterCriterion> filter,
      int pageIndex,
      int pageSize)
    {
      throw new NotImplementedException();
    }
  }
}
