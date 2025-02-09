// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.LoanTradeToLoan
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  internal class LoanTradeToLoan(SessionObjects sessionObjects, LoanDataMgr loanDataMgr) : 
    TradeToLoanBase(sessionObjects, loanDataMgr)
  {
    private const string className = "LoanTradeToLoan�";

    public override void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList)
    {
      Tracing.Log(TradeToLoanBase.sw, nameof (LoanTradeToLoan), TraceLevel.Info, "Setting loan trade status to " + status + "..");
      if (this.loanDataMgr.LoanData.GetField("2819") != trade.Guid)
        return;
      if ((LoanTradeStatus) status == LoanTradeStatus.Shipped)
      {
        this.loanDataMgr.LoanData.SetField("2031", LoanTradeStatus.Shipped.ToString());
        DateTime shipmentDate = trade.ShipmentDate;
        if (shipmentDate == DateTime.MinValue)
          this.loanDataMgr.LoanData.SetField("2014", DateTime.Now.ToString("MM/dd/yyyy"));
        else
          this.loanDataMgr.LoanData.SetField("2014", shipmentDate.ToString("MM/dd/yyyy"));
        skipFieldList.Add("2014");
      }
      else if ((LoanTradeStatus) status == LoanTradeStatus.Purchased)
      {
        this.loanDataMgr.LoanData.SetField("2031", LoanTradeStatus.Purchased.ToString());
        this.loanDataMgr.LoanData.SetField("2014", "");
        DateTime purchaseDate = trade.PurchaseDate;
        if (purchaseDate == DateTime.MinValue)
          this.loanDataMgr.LoanData.SetField("2370", DateTime.Now.ToString("MM/dd/yyyy"));
        else
          this.loanDataMgr.LoanData.SetField("2370", purchaseDate.ToString("MM/dd/yyyy"));
        skipFieldList.Add("2370");
        skipFieldList.Add("2014");
      }
      this.RefreshTradeData(trade, skipFieldList, price, updateFieldList, LoanDataMgr.LockLoanSyncOption.syncLockToLoan, false);
    }

    public override void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price)
    {
      this.ModifyTradeStatus(trade, status, skipFieldList, price, new Dictionary<string, string>());
    }

    public override void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price)
    {
      this.RefreshTradeData(trade, skipFieldList, price, new Dictionary<string, string>(), LoanDataMgr.LockLoanSyncOption.syncLockToLoan, false);
    }

    public override void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan,
      bool forAssignment = false)
    {
      if (this.loanDataMgr.LoanData.Calculator == null)
        this.SetCalculator();
      LoanTradeInfo loanTradeInfo = (LoanTradeInfo) trade;
      Tracing.Log(TradeToLoanBase.sw, nameof (LoanTradeToLoan), TraceLevel.Info, "Refreshing trade data from trade " + (object) loanTradeInfo.TradeID + "..");
      LockUtils lockUtils = new LockUtils(this.sessionObjects);
      if (this.loanDataMgr.LoanData.GetField("2819") != trade.Guid)
        return;
      this.loanDataMgr.LoanData.SetField("2032", trade.Name);
      LockRequestLog currentLockOrRequest = this.loanDataMgr.LoanData.GetLogList().GetCurrentLockOrRequest();
      if (currentLockOrRequest == null)
      {
        Tracing.Log(TradeToLoanBase.sw, nameof (LoanTradeToLoan), TraceLevel.Error, "No current lock request found for loan " + this.loanDataMgr.LoanData.GUID + " in RefreshTradeData");
      }
      else
      {
        Hashtable lockRequestSnapshot = currentLockOrRequest.GetLockRequestSnapshot();
        if (lockRequestSnapshot == null)
        {
          Tracing.Log(TradeToLoanBase.sw, nameof (LoanTradeToLoan), TraceLevel.Error, "No lock request snapshot found for loan " + this.loanDataMgr.LoanData.GUID + " in RefreshTradeData");
        }
        else
        {
          Decimal origValue1 = new Decimal(0.0);
          if (skipFieldList.Contains("2232"))
            origValue1 = Utils.ParseDecimal(lockRequestSnapshot[(object) "2232"]);
          else if (loanTradeInfo.IsBulkDelivery)
          {
            if (updateFieldList.ContainsKey("TotalPrice"))
              origValue1 = Utils.ParseDecimal((object) updateFieldList["TotalPrice"]);
          }
          else
          {
            Decimal noteRate = Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("3"));
            origValue1 = loanTradeInfo.GetBasePriceForNoteRate(noteRate, price);
          }
          Decimal loanAmount = Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("2"));
          bool impoundsWaived = this.loanDataMgr.LoanData.GetField("2293") == "Waived";
          string field = this.loanDataMgr.LoanData.GetField("14");
          Decimal num1 = 0M;
          int num2 = 0;
          for (int index = 0; index < trade.PriceAdjustments.Count && num2 < 20; ++index)
          {
            TradePriceAdjustment priceAdjustment = trade.PriceAdjustments[index];
            if (!skipFieldList.Contains("Price Adjustments") && priceAdjustment.CriterionList.CreateEvaluator().Evaluate(this.loanDataMgr.LoanData, FilterEvaluationOption.None))
            {
              lockRequestSnapshot[(object) (2233 + 2 * num2).ToString()] = (object) priceAdjustment.CriterionList.ToString();
              lockRequestSnapshot[(object) (2233 + 2 * num2 + 1).ToString()] = (object) priceAdjustment.PriceAdjustment.ToString("0.000");
              num1 += priceAdjustment.PriceAdjustment;
              ++num2;
            }
          }
          if (!skipFieldList.Contains("Price Adjustments"))
          {
            for (; num2 < 20; ++num2)
            {
              lockRequestSnapshot[(object) (2233 + 2 * num2).ToString()] = (object) "";
              lockRequestSnapshot[(object) (2233 + 2 * num2 + 1).ToString()] = (object) "";
            }
          }
          else
          {
            for (int index = 0; index < 20; ++index)
              num1 += Utils.ParseDecimal(lockRequestSnapshot[(object) (2233 + 2 * index + 1).ToString()], 0M);
            for (int index = 0; index < 10; ++index)
              num1 += Utils.ParseDecimal(lockRequestSnapshot[(object) (3495 + 2 * index).ToString()], 0M);
          }
          if (!skipFieldList.Contains("2232"))
            lockRequestSnapshot[(object) "2232"] = (object) lockUtils.FormatSecondaryDecimalDigits("2232", origValue1);
          if (!skipFieldList.Contains("2273"))
            lockRequestSnapshot[(object) "2273"] = (object) num1.ToString("0.000");
          lockRequestSnapshot[(object) "2274"] = (object) lockUtils.FormatSecondaryDecimalDigits("2274", origValue1 + num1);
          Decimal origValue2 = trade.SRPTable.GetAdjustmentForLoan(loanAmount, impoundsWaived, field);
          if (!skipFieldList.Contains("2276"))
            lockRequestSnapshot[(object) "2276"] = (object) lockUtils.FormatSecondaryDecimalDigits("2276", origValue2);
          else
            origValue2 = !lockRequestSnapshot.Contains((object) "2276") ? new Decimal(0.0) : Utils.ParseDecimal(lockRequestSnapshot[(object) "2276"]);
          Decimal origValue3 = loanTradeInfo.IsBulkDelivery ? origValue1 + (skipFieldList.Contains("2276") ? origValue2 : 0M) : origValue1 + num1 + origValue2;
          Decimal num3 = Utils.ParseDecimal((object) string.Concat(lockRequestSnapshot[(object) "2218"]));
          Decimal origValue4 = origValue3 - num3;
          Decimal origValue5 = Utils.ParseDecimal(lockRequestSnapshot[(object) "3835"]) - origValue3;
          lockRequestSnapshot[(object) "2295"] = (object) lockUtils.FormatSecondaryDecimalDigits("2295", origValue3);
          lockRequestSnapshot[(object) "2296"] = (object) lockUtils.FormatSecondaryDecimalDigits("2296", origValue4);
          lockRequestSnapshot[(object) "2028"] = (object) lockUtils.FormatSecondaryDecimalDigits("2028", origValue4 * loanAmount / 100M);
          lockRequestSnapshot[(object) "3836"] = (object) lockUtils.FormatSecondaryDecimalDigits("3836", origValue5);
          lockRequestSnapshot[(object) "3837"] = (object) lockUtils.FormatSecondaryDecimalDigits("3837", origValue5 * loanAmount / 100M);
          if (!skipFieldList.Contains("Commitment Number"))
          {
            lockRequestSnapshot[(object) "2286"] = (object) loanTradeInfo.InvestorCommitmentNumber;
            this.loanDataMgr.LoanData.SetField("996", string.Concat(lockRequestSnapshot[(object) "2286"]));
          }
          if (!skipFieldList.Contains("2841"))
            lockRequestSnapshot[(object) "2841"] = (object) loanTradeInfo.ContractNumber;
          if (!skipFieldList.Contains("2842"))
            lockRequestSnapshot[(object) "2842"] = (object) loanTradeInfo.InvestorTradeNumber;
          if (!skipFieldList.Contains("2297"))
          {
            if (loanTradeInfo.InvestorDeliveryDate != DateTime.MinValue)
              lockRequestSnapshot[(object) "2297"] = (object) loanTradeInfo.InvestorDeliveryDate.ToString("MM/dd/yyyy");
            else
              lockRequestSnapshot[(object) "2297"] = (object) "";
            currentLockOrRequest.SellSideDeliveryDate = Utils.ParseDate((object) lockRequestSnapshot[(object) "2297"].ToString());
          }
          if (!skipFieldList.Contains("2206"))
          {
            if (loanTradeInfo.TargetDeliveryDate != DateTime.MinValue)
              lockRequestSnapshot[(object) "2206"] = (object) loanTradeInfo.TargetDeliveryDate.ToString("MM/dd/yyyy");
            else
              lockRequestSnapshot[(object) "2206"] = (object) "";
          }
          if (!skipFieldList.Contains("4016"))
          {
            if (loanTradeInfo.CommitmentDate != DateTime.MinValue)
              lockRequestSnapshot[(object) "4016"] = (object) loanTradeInfo.CommitmentDate.ToString("MM/dd/yyyy");
            else
              lockRequestSnapshot[(object) "4016"] = (object) "";
          }
          if (!skipFieldList.Contains("Investor Information"))
          {
            if (trade.Investor.Name != "")
              lockRequestSnapshot[(object) "2278"] = (object) trade.Investor.Name;
            else
              lockRequestSnapshot[(object) "2278"] = (object) trade.Investor.ContactInformation.EntityName;
            currentLockOrRequest.InvestorName = lockRequestSnapshot[(object) "2278"].ToString();
            lockRequestSnapshot[(object) "2279"] = (object) trade.Investor.ContactInformation.ContactName;
            lockRequestSnapshot[(object) "2280"] = (object) trade.Investor.ContactInformation.PhoneNumber;
            lockRequestSnapshot[(object) "3055"] = (object) trade.Investor.ContactInformation.EmailAddress;
            lockRequestSnapshot[(object) "2281"] = (object) trade.Investor.ContactInformation.Address.Street1;
            lockRequestSnapshot[(object) "2282"] = (object) trade.Investor.ContactInformation.Address.City;
            lockRequestSnapshot[(object) "2283"] = (object) trade.Investor.ContactInformation.Address.State;
            lockRequestSnapshot[(object) "2284"] = (object) trade.Investor.ContactInformation.Address.Zip;
            lockRequestSnapshot[(object) "2285"] = (object) trade.Investor.ContactInformation.WebSite;
            switch (loanTradeInfo.ServicingType)
            {
              case ServicingType.ServicingReleased:
                lockRequestSnapshot[(object) "3534"] = (object) "Service Released";
                lockRequestSnapshot[(object) "4118"] = (object) "";
                break;
              case ServicingType.ServicingRetained:
                lockRequestSnapshot[(object) "3534"] = (object) "Service Retained";
                Decimal noteRate = Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("3"));
                lockRequestSnapshot[(object) "3888"] = (object) loanTradeInfo.GetServiceForNoteRate(noteRate);
                this.loanDataMgr.LoanData.SetField("ULDD.X85", lockRequestSnapshot[(object) "3888"].ToString());
                lockRequestSnapshot[(object) "4118"] = (object) loanTradeInfo.GetMSRValueForNoteRate(noteRate);
                break;
              default:
                lockRequestSnapshot[(object) "3534"] = (object) "";
                lockRequestSnapshot[(object) "4118"] = (object) "";
                break;
            }
            lockRequestSnapshot[(object) "3535"] = (object) loanTradeInfo.Servicer;
          }
          currentLockOrRequest.AddLockRequestSnapshot(lockRequestSnapshot);
          if (!skipFieldList.Contains("2012"))
            this.loanDataMgr.LoanData.SetField("2012", string.Concat(lockRequestSnapshot[(object) "2297"]));
          if (!skipFieldList.Contains("2013"))
            this.loanDataMgr.LoanData.SetField("2013", string.Concat(lockRequestSnapshot[(object) "2206"]));
          if (!skipFieldList.Contains("2207"))
            this.loanDataMgr.LoanData.SetField("2207", string.Concat(lockRequestSnapshot[(object) "2274"]));
          if (!skipFieldList.Contains("2209"))
            this.loanDataMgr.LoanData.SetField("2209", string.Concat(lockRequestSnapshot[(object) "2276"]));
          if (!skipFieldList.Contains("2014"))
          {
            if (loanTradeInfo.ShipmentDate != DateTime.MinValue)
              this.loanDataMgr.LoanData.SetField("2014", loanTradeInfo.ShipmentDate.ToString("MM/dd/yyyy"));
            else
              this.loanDataMgr.LoanData.SetField("2014", "");
          }
          if (!skipFieldList.Contains("2370"))
          {
            if (loanTradeInfo.PurchaseDate != DateTime.MinValue)
              this.loanDataMgr.LoanData.SetField("2370", loanTradeInfo.PurchaseDate.ToString("MM/dd/yyyy"));
            else
              this.loanDataMgr.LoanData.SetField("2370", "");
          }
          this.loanDataMgr.LoanData.Calculator.FormCalculation("PurchaseAdvice", (string) null, (string) null);
          this.ApplyInvestorToLoan(trade.Investor, trade.Assignee, true, skipFieldList);
          this.loanDataMgr.SyncLockRequestSnapshotToLoan(lockRequestSnapshot, false, false);
          Tracing.Log(TradeToLoanBase.sw, nameof (LoanTradeToLoan), TraceLevel.Info, "Trade data refresh completed");
        }
      }
    }
  }
}
