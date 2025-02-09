// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.PipelineEngine.PipelineParameters
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.PipelineEngine
{
  public sealed class PipelineParameters
  {
    public UserInfo User { get; set; }

    public LoanInfo.Right AccessRights { get; set; }

    public string[] LoanFolders { get; set; }

    public string IdentitySelectionQuery { get; set; }

    public string[] GuidList { get; set; }

    public string[] Fields { get; set; }

    public PipelineData DataToInclude { get; set; }

    public QueryCriterion Filter { get; set; }

    public SortField[] SortFields { get; set; }

    public ICriterionTranslator FieldTranslator { get; set; }

    public bool IsExternalOrganization { get; set; }

    public int SqlRead { get; set; }

    public TradeType TradeType { get; set; }

    public int? MaxCount { get; set; }

    public bool ApplyUserAccessFiltering { get; set; }

    public bool UseGetLoansForMyLoans { get; set; }

    public PipelinePagination PaginationInfo { get; set; }

    public bool IsGlobalSearch { get; set; }

    public CalculateTotalCountEnum CalculateCountOnly { get; set; }

    public bool excludeArchivedLoans { get; set; }

    public void DedupGuidList()
    {
      if (this.GuidList == null || this.GuidList.Length <= 1)
        return;
      this.GuidList = new HashSet<string>((IEnumerable<string>) this.GuidList).ToArray<string>();
    }
  }
}
