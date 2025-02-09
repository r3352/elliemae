// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IMasterContractManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Trading;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IMasterContractManager
  {
    MasterContractInfo GetContract(int contractId);

    MasterContractInfo GetContractByContractNumber(string contractNumber);

    MasterContractSummaryInfo[] GetAllContracts(bool includeTradeData);

    MasterContractSummaryInfo[] GetActiveContracts(bool includeTradeData);

    MasterContractSummaryInfo[] GetArchivedContracts(bool includeTradeData);

    int CreateContract(MasterContractInfo contract);

    void UpdateContract(MasterContractInfo contract);

    void DeleteContract(int contractId);

    void SetContractStatus(int contractId, MasterContractStatus status);

    void SetContractStatus(int[] contractIds, MasterContractStatus status);

    ICursor OpenMasterContractCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization);
  }
}
