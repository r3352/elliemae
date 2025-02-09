// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ISecurityTradeManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ISecurityTradeManager
  {
    int CreateTrade(SecurityTradeInfo tradeInfo);

    SecurityTradeInfo GetTrade(int tradeId);

    SecurityTradeInfo GetTradeByName(string tradeName);

    SecurityTradeInfo[] GetTradesByName(string tradeName);

    SecurityTradeInfo[] GetTradesByName(string tradeName, bool includeHidden);

    SecurityTradeInfo[] GetActiveTrades();

    Dictionary<int, SecurityTradeInfo> GetTrades(int[] tradeIds);

    void UpdateTrade(SecurityTradeInfo tradeInfo);

    void DeleteTrade(int tradeId);

    SecurityTradeEditorScreenData GetTradeEditorScreenData(int tradeId);

    SecurityTradeHistoryItem[] GetTradeHistory(int tradeId);

    void AddTradeHistoryItem(SecurityTradeHistoryItem item);

    void AssignLoanTradeToTrade(int tradeId, int assigneeTradeId);

    void AssignLoanTradeToTrade(
      int tradeId,
      int assigneeTradeId,
      SecurityLoanTradeStatus status,
      DateTime assignedStatusDate);

    void UnassignLoanTradeToTrade(int tradeId, int assigneeTradeId);

    SecurityTradeAssignment[] GetTradeAssigments(int tradeId);

    ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization);

    void SetTradeStatus(int tradeId, TradeStatus status);

    void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck);

    void UpdateTradeAfterAssignLoanTrade(SecurityTradeInfo tradeInfo);
  }
}
