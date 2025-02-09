// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.ITradeToLoan
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  internal interface ITradeToLoan
  {
    void RemoveFromTrade(TradeInfoObj trade, bool rejected);

    void RemoveFromTrade(TradeInfoObj trade, bool rejected, List<string> skipFieldList);

    void AssignToTrade(TradeInfoObj trade, List<string> skipFieldList, Decimal securityPrice);

    void CopyPreviousSnapshot(TradeInfoObj trade, bool remove, List<string> skipFieldList = null);

    void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price);

    void RefreshTradeData(TradeInfoObj trade, List<string> skipFieldList, Decimal price);

    void ApplyInvestorToLoan(Investor investor, ContactInformation assignee, bool updateInvestor);

    void ApplyInvestorToLoan(
      Investor investor,
      ContactInformation assignee,
      bool updateInvestor,
      List<string> skipFieldList);

    void CopyTradeDataToSnapshot(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Hashtable newData,
      Dictionary<string, string> updateFieldList);

    void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan);

    void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList);

    void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan,
      bool forAssignment = false);

    void ExtendLockWithTrade(
      TradeInfoObj trade,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan);

    void ClearUlddFields(MbsPoolMortgageType poolMortgageType);
  }
}
