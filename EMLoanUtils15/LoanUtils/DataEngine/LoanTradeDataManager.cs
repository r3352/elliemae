// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.LoanTradeDataManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  public class LoanTradeDataManager
  {
    public LoanTradeDataManager(SessionObjects sessionObjects, LoanDataMgr loanDataMgr)
    {
      this.sessionObjects = sessionObjects;
      this.loanDataMgr = loanDataMgr;
    }

    private SessionObjects sessionObjects { get; set; }

    private LoanDataMgr loanDataMgr { get; set; }

    private ITradeToLoan createProvider(TradeInfoObj trade)
    {
      ITradeToLoan provider;
      switch (trade)
      {
        case LoanTradeInfo _:
          provider = (ITradeToLoan) new LoanTradeToLoan(this.sessionObjects, this.loanDataMgr);
          break;
        case MbsPoolInfo _:
          provider = (ITradeToLoan) new MbsPoolToLoan(this.sessionObjects, this.loanDataMgr);
          break;
        case CorrespondentTradeInfo _:
          provider = (ITradeToLoan) new CorrespondentTradeToLoan(this.sessionObjects, this.loanDataMgr);
          break;
        default:
          provider = (ITradeToLoan) new TradeToLoanBase(this.sessionObjects, this.loanDataMgr);
          break;
      }
      return provider;
    }

    public void RemoveFromTrade(TradeInfoObj trade, bool rejected)
    {
      this.createProvider(trade).RemoveFromTrade(trade, rejected);
    }

    public void RemoveFromTrade(TradeInfoObj trade, bool rejected, List<string> skipFieldList)
    {
      this.createProvider(trade).RemoveFromTrade(trade, rejected, skipFieldList);
    }

    public void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      this.createProvider(trade).AssignToTrade(trade, skipFieldList, securityPrice);
    }

    public void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      this.createProvider(trade).AssignToTrade(trade, skipFieldList, securityPrice, updateFieldList, syncOption);
    }

    public void CopyPreviousSnapshot(TradeInfoObj trade, bool remove)
    {
      this.createProvider(trade).CopyPreviousSnapshot(trade, remove);
    }

    public void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      this.createProvider(trade).ModifyTradeStatus(trade, status, skipFieldList, securityPrice);
    }

    public void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList)
    {
      this.createProvider(trade).ModifyTradeStatus(trade, status, skipFieldList, securityPrice, updateFieldList);
    }

    public void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      this.createProvider(trade).RefreshTradeData(trade, skipFieldList, securityPrice);
    }

    public void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      this.createProvider(trade).RefreshTradeData(trade, skipFieldList, securityPrice, updateFieldList, syncOption);
    }

    public void ApplyInvestorToLoan(
      Investor investor,
      ContactInformation assignee,
      bool updateInvestor)
    {
      this.createProvider((TradeInfoObj) null).ApplyInvestorToLoan(investor, assignee, updateInvestor);
    }

    public void ApplyInvestorToLoan(
      Investor investor,
      ContactInformation assignee,
      bool updateInvestor,
      List<string> skipFieldList)
    {
      this.createProvider((TradeInfoObj) null).ApplyInvestorToLoan(investor, assignee, updateInvestor, skipFieldList);
    }

    public void ExtendLockWithTrade(
      TradeInfoObj trade,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      this.createProvider(trade).ExtendLockWithTrade(trade, updateFieldList, syncOption);
    }
  }
}
