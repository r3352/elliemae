// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.QueryEngine.QueryEngineParameters
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.PipelineEngine;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.QueryEngine
{
  public sealed class QueryEngineParameters
  {
    public UserInfo User { get; set; }

    public string[] LoanFolders { get; set; }

    public string[] GuidList { get; set; }

    public string[] Fields { get; set; }

    public QueryCriterion Filter { get; set; }

    public SortField[] SortFields { get; set; }

    public PipelinePagination PaginationInfo { get; set; }

    public int? MaxCount { get; set; }

    public bool ExcludeArchivedLoans { get; set; }
  }
}
