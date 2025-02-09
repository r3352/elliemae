// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeHistoryAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public enum TradeHistoryAction
  {
    None = 0,
    TradeCreated = 1,
    TradeLocked = 2,
    TradeUnlocked = 3,
    TradeArchived = 4,
    TradeActivated = 5,
    TradeStatusChanged = 6,
    UnlockPendingTrade = 7,
    TradeVoided = 8,
    LoanAssigned = 10, // 0x0000000A
    LoanRemoved = 11, // 0x0000000B
    LoanStatusChanged = 12, // 0x0000000C
    LoanRejected = 13, // 0x0000000D
    ContractAssigned = 20, // 0x00000014
    ContractUnassigned = 21, // 0x00000015
    AssigneeAssigned = 30, // 0x0000001E
    AssigneeUnassigned = 31, // 0x0000001F
    AssigneeChanged = 32, // 0x00000020
    AssignedAmountChanged = 33, // 0x00000021
    GSEAssigneeAssigned = 34, // 0x00000022
    GSEAssigneeUnassigned = 35, // 0x00000023
    LoanUpdateErrors = 40, // 0x00000028
    LoanUpdateCancelled = 41, // 0x00000029
    LoanUpdatePending = 42, // 0x0000002A
    LoanUpdateCompleted = 43, // 0x0000002B
    LoanUpdateCleared = 44, // 0x0000002C
    TradePublished = 45, // 0x0000002D
    TradeUpdated = 46, // 0x0000002E
    TradeCommitted = 47, // 0x0000002F
    TradeDelivered = 48, // 0x00000030
    TradeSetteled = 49, // 0x00000031
    PairOffCreated = 50, // 0x00000032
    PairOffUpdated = 51, // 0x00000033
    PairOffReversed = 52, // 0x00000034
    PairOffDeleted = 53, // 0x00000035
    PairOffNotChanged = 54, // 0x00000036
    StatusChangedManually = 55, // 0x00000037
  }
}
