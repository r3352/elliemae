// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ITradeSynchronizationManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Trading;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ITradeSynchronizationManager
  {
    int Assign(
      TradeInfoObj tradeInfo,
      List<TradeAssignmentItem> items,
      List<string> skipFieldList,
      string siteId,
      BatchJobApplicationChannel applicationChannel,
      string tradeExtensionInfo = null,
      string lockLoanSyncOption = "syncLockToLoan�");

    BatchJobInfo GetJob(int jobId);

    BatchJobSummaryInfo[] GetAllJobStatus();

    bool Cancel(int jobId);

    bool Remove(int jobId);
  }
}
