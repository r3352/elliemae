// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ICorrespondentMasterManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Trading;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ICorrespondentMasterManager
  {
    CorrespondentMasterInfo GetCorrespondentMaster(int masterId);

    CorrespondentMasterInfo CheckCorrespondentMasterByMasterNumber(string masterNumber);

    int CreateCorrespondentMaster(CorrespondentMasterInfo info);

    void UpdateCorrespondentMaster(CorrespondentMasterInfo info);

    void DeleteCorrespondentMaster(int masterId);

    void SetCorrespondentMasterStatus(int masterId, MasterCommitmentStatus status);

    void SetCorrespondentMasterStatus(int[] masterIds, MasterCommitmentStatus status);

    ICursor OpenCorrespondentMasterCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization);

    CorrespondentMasterHistoryItem[] GetCorrespondentMasterHistory(int correspondentMasterId);

    bool IsAllLoanPurcahsedInMaster(int[] masterIds);

    bool AreTradesInCorrespondentMasters();

    string GetNextAutoCreateMasterName();

    CorrespondentMasterSummaryInfo GetCorrespondentMasterSummary(int id);

    CorrespondentMasterInfo[] GetCorrespondentMastersByTPOID(string tpoId);

    CorrespondentMasterInfo[] GetCorrespondentMastersByOrganizationID(string orgId);

    CorrespondentMasterInfo GetCorrespondentMastersByContractNumber(string masterNumber);

    CorrespondentMasterInfo GetCorrespondentMasterByTradeId(int correspondentMasterTradeId);
  }
}
