// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateJobInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradeLoanUpdateJobInfo
  {
    public string JobGuid;
    public int TradeID;
    public string TradeName;
    public TradeType TradeType;
    public string CreatedBy;
    public DateTime CreatedDate;
    public DateTime JobStartDate;
    public TradeLoanUpdateQueueStatus JobStatus;
    public string SessionID;
    public int TotalLoans;
    public int LoansCompleted;
    public DateTime LastUpdateDate;
    public DateTime JobEndDate;
    public string Results;
    public string CancelledBy;
    public DateTime CancelledDate;
    public string DeletedBy;
    public DateTime DeletedDate;
    public DateTime SessionLastUpdateDate;
    public TradeLoanUpdateJobInfo.ActionType UpdateActionType;

    public enum ActionType
    {
      Commit,
      Refresh,
      Void,
      ExtendLock,
    }
  }
}
