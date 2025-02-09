// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ITradeBatchService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.TradeManagement;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects
{
  public interface ITradeBatchService
  {
    int CreateLoanUpdateJob(int tradeId, TradeType tradeType, bool allOrPending);

    int CreateLoanUpdateJob(int tradeId, TradeType tradeType, List<string> loanNumbers);

    TradeQueueJob GetJobStatus(int jobId);

    TradeQueueJobSummary[] GetAllJobStatus();

    bool CancelJob(int jobId);

    bool RemoveJob(int jobId);
  }
}
