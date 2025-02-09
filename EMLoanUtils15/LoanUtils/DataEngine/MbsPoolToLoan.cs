// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.MbsPoolToLoan
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
  internal class MbsPoolToLoan(SessionObjects sessionObjects, LoanDataMgr loanDataMgr) : 
    TradeToLoanBase(sessionObjects, loanDataMgr)
  {
    private const string className = "MbsPoolToLoan�";

    public override void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList)
    {
      Tracing.Log(TradeToLoanBase.sw, nameof (MbsPoolToLoan), TraceLevel.Info, "Setting MBS Pool status to " + status + "..");
      if (this.loanDataMgr.LoanData.GetField("2819") != trade.Guid)
        return;
      if ((MbsPoolLoanStatus) status == MbsPoolLoanStatus.Shipped)
      {
        this.loanDataMgr.LoanData.SetField("2031", MbsPoolLoanStatus.Shipped.ToString());
        if (trade.ShipmentDate == DateTime.MinValue)
          this.loanDataMgr.LoanData.SetField("2014", DateTime.Now.ToString("MM/dd/yyyy"));
        else
          this.loanDataMgr.LoanData.SetField("2014", trade.ShipmentDate.ToString("MM/dd/yyyy"));
        skipFieldList.Add("2014");
      }
      else if ((MbsPoolLoanStatus) status == MbsPoolLoanStatus.Purchased)
      {
        this.loanDataMgr.LoanData.SetField("2031", MbsPoolLoanStatus.Purchased.ToString());
        this.loanDataMgr.LoanData.SetField("2014", "");
        if (trade.PurchaseDate == DateTime.MinValue)
          this.loanDataMgr.LoanData.SetField("2370", DateTime.Now.ToString("MM/dd/yyyy"));
        else
          this.loanDataMgr.LoanData.SetField("2370", trade.PurchaseDate.ToString("MM/dd/yyyy"));
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
      MbsPoolInfo mbsPoolInfo = (MbsPoolInfo) trade;
      Tracing.Log(TradeToLoanBase.sw, nameof (MbsPoolToLoan), TraceLevel.Info, "Refreshing trade data from trade " + (object) mbsPoolInfo.TradeID + "..");
      LockUtils lockUtils = new LockUtils(this.sessionObjects);
      if (this.loanDataMgr.LoanData.GetField("2819") != trade.Guid)
        return;
      this.loanDataMgr.LoanData.SetField("2032", trade.Name);
      LockRequestLog currentLockOrRequest = this.loanDataMgr.LoanData.GetLogList().GetCurrentLockOrRequest();
      if (currentLockOrRequest == null)
      {
        Tracing.Log(TradeToLoanBase.sw, nameof (MbsPoolToLoan), TraceLevel.Error, "No current lock request found for loan " + this.loanDataMgr.LoanData.GUID + " in RefreshTradeData");
      }
      else
      {
        Hashtable lockRequestSnapshot = currentLockOrRequest.GetLockRequestSnapshot();
        if (lockRequestSnapshot == null)
        {
          Tracing.Log(TradeToLoanBase.sw, nameof (MbsPoolToLoan), TraceLevel.Error, "No lock request snapshot found for loan " + this.loanDataMgr.LoanData.GUID + " in RefreshTradeData");
        }
        else
        {
          Decimal num1 = new Decimal(0.0);
          Decimal noteRate = Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("3"));
          Decimal origValue1 = !skipFieldList.Contains("2232") ? mbsPoolInfo.GetBasePriceForNoteRate(noteRate, price, updateFieldList.ContainsKey("CPA") ? Utils.ParseDecimal((object) updateFieldList["CPA"]) : 0M, updateFieldList.ContainsKey("4093") ? updateFieldList["4093"] : (string) null, updateFieldList.ContainsKey("4094") ? updateFieldList["4094"] : (string) null) : Utils.ParseDecimal(lockRequestSnapshot[(object) "2232"]);
          Decimal num2 = Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("2"));
          bool flag = this.loanDataMgr.LoanData.GetField("2293") == "Waived";
          this.loanDataMgr.LoanData.GetField("14");
          Decimal num3 = 0M;
          int num4 = 0;
          for (int index = 0; index < trade.PriceAdjustments.Count && num4 < 20; ++index)
          {
            TradePriceAdjustment priceAdjustment = trade.PriceAdjustments[index];
            if (!skipFieldList.Contains("Price Adjustments") && priceAdjustment.CriterionList.CreateEvaluator().Evaluate(this.loanDataMgr.LoanData, FilterEvaluationOption.None))
            {
              lockRequestSnapshot[(object) (2233 + 2 * num4).ToString()] = (object) priceAdjustment.CriterionList.ToString();
              lockRequestSnapshot[(object) (2233 + 2 * num4 + 1).ToString()] = (object) priceAdjustment.PriceAdjustment.ToString("0.000");
              num3 += priceAdjustment.PriceAdjustment;
              ++num4;
            }
          }
          if (!skipFieldList.Contains("Price Adjustments"))
          {
            for (; num4 < 20; ++num4)
            {
              lockRequestSnapshot[(object) (2233 + 2 * num4).ToString()] = (object) "";
              lockRequestSnapshot[(object) (2233 + 2 * num4 + 1).ToString()] = (object) "";
            }
          }
          else
          {
            for (int index = 0; index < 20; ++index)
              num3 += Utils.ParseDecimal(lockRequestSnapshot[(object) (2233 + 2 * index + 1).ToString()], 0M);
          }
          if (!skipFieldList.Contains("2232"))
            lockRequestSnapshot[(object) "2232"] = (object) lockUtils.FormatSecondaryDecimalDigits("2232", origValue1);
          if (!skipFieldList.Contains("2273"))
            lockRequestSnapshot[(object) "2273"] = (object) num3.ToString("0.000");
          lockRequestSnapshot[(object) "2274"] = (object) lockUtils.FormatSecondaryDecimalDigits("2274", origValue1 + num3);
          Decimal origValue2 = origValue1 + num3;
          Decimal num5 = Utils.ParseDecimal((object) string.Concat(lockRequestSnapshot[(object) "2218"]));
          Decimal origValue3 = origValue2 - num5;
          Decimal origValue4 = Utils.ParseDecimal(lockRequestSnapshot[(object) "3835"]) - origValue2;
          lockRequestSnapshot[(object) "2295"] = (object) lockUtils.FormatSecondaryDecimalDigits("2295", origValue2);
          lockRequestSnapshot[(object) "2296"] = (object) lockUtils.FormatSecondaryDecimalDigits("2296", origValue3);
          lockRequestSnapshot[(object) "2028"] = (object) lockUtils.FormatSecondaryDecimalDigits("2028", origValue3 * num2 / 100M);
          lockRequestSnapshot[(object) "3836"] = (object) lockUtils.FormatSecondaryDecimalDigits("3836", origValue4);
          lockRequestSnapshot[(object) "3837"] = (object) lockUtils.FormatSecondaryDecimalDigits("3837", origValue4 * num2 / 100M);
          if (!skipFieldList.Contains("2841"))
            lockRequestSnapshot[(object) "2841"] = (object) mbsPoolInfo.ContractNumber;
          if (!skipFieldList.Contains("2297"))
          {
            if (mbsPoolInfo.InvestorDeliveryDate != DateTime.MinValue)
              lockRequestSnapshot[(object) "2297"] = (object) mbsPoolInfo.InvestorDeliveryDate.ToString("MM/dd/yyyy");
            else
              lockRequestSnapshot[(object) "2297"] = (object) "";
            currentLockOrRequest.SellSideDeliveryDate = Utils.ParseDate((object) lockRequestSnapshot[(object) "2297"].ToString());
          }
          if (!skipFieldList.Contains("2206"))
          {
            if (mbsPoolInfo.TargetDeliveryDate != DateTime.MinValue)
              lockRequestSnapshot[(object) "2206"] = (object) mbsPoolInfo.TargetDeliveryDate.ToString("MM/dd/yyyy");
            else
              lockRequestSnapshot[(object) "2206"] = (object) "";
          }
          if (!skipFieldList.Contains("4019"))
            lockRequestSnapshot[(object) "4019"] = (object) mbsPoolInfo.Name;
          if (!skipFieldList.Contains("3890"))
            lockRequestSnapshot[(object) "3890"] = (object) mbsPoolInfo.PoolNumber;
          if (!skipFieldList.Contains("4093") && updateFieldList.ContainsKey("4093"))
            lockRequestSnapshot[(object) "4093"] = (object) updateFieldList["4093"];
          if (!skipFieldList.Contains("4094") && updateFieldList.ContainsKey("4094"))
            lockRequestSnapshot[(object) "4094"] = (object) updateFieldList["4094"];
          if (!skipFieldList.Contains("4016"))
          {
            if (mbsPoolInfo.CommitmentDate != DateTime.MinValue)
              lockRequestSnapshot[(object) "4016"] = (object) mbsPoolInfo.CommitmentDate.ToString("MM/dd/yyyy");
            else
              lockRequestSnapshot[(object) "4016"] = (object) "";
          }
          string val1 = "";
          string val2 = "";
          string val3 = "";
          Decimal num6;
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
            switch (mbsPoolInfo.ServicingType)
            {
              case ServicingType.ServicingReleased:
                lockRequestSnapshot[(object) "3534"] = (object) "Service Released";
                lockRequestSnapshot[(object) "4118"] = (object) "";
                break;
              case ServicingType.ServicingRetained:
                lockRequestSnapshot[(object) "3534"] = (object) "Service Retained";
                lockRequestSnapshot[(object) "4118"] = (object) mbsPoolInfo.GetMSRValueForNoteRate(Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("3")));
                break;
              default:
                lockRequestSnapshot[(object) "3534"] = (object) "";
                lockRequestSnapshot[(object) "4118"] = (object) "";
                break;
            }
            lockRequestSnapshot[(object) "3535"] = (object) mbsPoolInfo.Servicer;
            lockRequestSnapshot[(object) "3889"] = (object) (val1 = this.GetGuaranteeFeeValue(mbsPoolInfo, updateFieldList));
            val2 = mbsPoolInfo.FixedServicingFeePercent == 0M ? "" : lockUtils.FormatSecondaryDecimalDigits("3888", mbsPoolInfo.FixedServicingFeePercent);
            lockRequestSnapshot[(object) "3888"] = (object) val2;
            if (mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMaePE || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMae || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FreddieMac)
            {
              Tuple<Decimal, Decimal> guarantyFeeForNoteRate = mbsPoolInfo.GetServicingGuarantyFeeForNoteRate(noteRate, mbsPoolInfo, updateFieldList.ContainsKey("4093") ? updateFieldList["4093"] : "", updateFieldList.ContainsKey("4094") ? updateFieldList["4094"] : "");
              val3 = mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMae || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FreddieMac ? (mbsPoolInfo.BaseGuarantyFee == 0M ? "" : lockUtils.FormatSecondaryDecimalDigits("4182", mbsPoolInfo.BaseGuarantyFee)) : val1;
              if (guarantyFeeForNoteRate != null)
              {
                val2 = guarantyFeeForNoteRate.Item1.ToString();
                num6 = guarantyFeeForNoteRate.Item2;
                val1 = num6.ToString();
              }
              lockRequestSnapshot[(object) "3889"] = (object) val1;
              lockRequestSnapshot[(object) "3888"] = (object) val2;
              lockRequestSnapshot[(object) "4182"] = (object) val3;
            }
          }
          if (!forAssignment && currentLockOrRequest.LockRequestStatus == RateLockRequestStatus.RateLocked)
          {
            this.CreateSnapshotForRateLocked(currentLockOrRequest, lockRequestSnapshot);
          }
          else
          {
            currentLockOrRequest.AddLockRequestSnapshot(lockRequestSnapshot);
            this.loanDataMgr.SyncSellComparisonToLoan(lockRequestSnapshot);
          }
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
            if (mbsPoolInfo.ShipmentDate != DateTime.MinValue)
              this.loanDataMgr.LoanData.SetField("2014", mbsPoolInfo.ShipmentDate.ToString("MM/dd/yyyy"));
            else
              this.loanDataMgr.LoanData.SetField("2014", "");
          }
          if (!skipFieldList.Contains("2370"))
          {
            if (mbsPoolInfo.PurchaseDate != DateTime.MinValue)
              this.loanDataMgr.LoanData.SetField("2370", mbsPoolInfo.PurchaseDate.ToString("MM/dd/yyyy"));
            else
              this.loanDataMgr.LoanData.SetField("2370", "");
          }
          if (mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMae || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
          {
            if (!skipFieldList.Contains("3890"))
              this.loanDataMgr.LoanData.SetField("ULDD.X65", mbsPoolInfo.PoolNumber);
            this.loanDataMgr.LoanData.SetField("ULDD.X72", mbsPoolInfo.SuffixID);
            string val4;
            switch (mbsPoolInfo.MortgageType)
            {
              case "Conventional":
                val4 = "Conventional";
                break;
              case "FHA":
                val4 = "FHA";
                break;
              case "USDA Rural Housing":
                val4 = "USDARuralHousing";
                break;
              case "VA":
                val4 = "VA";
                break;
              default:
                val4 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X69", val4);
            string val5;
            switch (mbsPoolInfo.AmortizationType)
            {
              case "Adjustable Rate":
                val5 = "AdjustableRate";
                break;
              case "Fixed":
                val5 = "Fixed";
                break;
              case "Graduated Payment ARM":
                val5 = "GraduatedPaymentARM";
                break;
              case "Graduated Payment Mortgage":
                val5 = "GraduatedPaymentMortgage";
                break;
              case "Growing Equity Mortgage":
                val5 = "GrowingEquityMortgage";
                break;
              default:
                val5 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X66", val5);
            this.loanDataMgr.LoanData.SetField("ULDD.X156", mbsPoolInfo.DocCustodianID);
            this.loanDataMgr.LoanData.SetField("ULDD.X158", mbsPoolInfo.ServicerID);
            this.loanDataMgr.LoanData.SetField("ULDD.X67", mbsPoolInfo.InvestorProductPlanID);
            this.loanDataMgr.LoanData.SetField("ULDD.X115", mbsPoolInfo.InvestorFeatureID);
            string val6;
            switch (mbsPoolInfo.LoanDefaultLossParty)
            {
              case "Investor":
                val6 = "Investor";
                break;
              case "Lender":
                val6 = "Lender";
                break;
              case "Shared":
                val6 = "Shared";
                break;
              default:
                val6 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X47", val6);
            string val7;
            switch (mbsPoolInfo.ReoMarketingParty)
            {
              case "Investor":
                val7 = "Investor";
                break;
              case "Lender":
                val7 = "Lender";
                break;
              default:
                val7 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X48", val7);
            LoanData loanData1 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.BaseGuarantyFee;
            string val8 = num6.ToString();
            loanData1.SetField("ULDD.X40", val8);
            LoanData loanData2 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.GFeeAfterAltPaymentMethod;
            string val9 = num6.ToString();
            loanData2.SetField("ULDD.X41", val9);
            if (!skipFieldList.Contains("Investor Information"))
              this.loanDataMgr.LoanData.SetField("ULDD.X39", val1);
            this.loanDataMgr.LoanData.SetField("ULDD.X45", mbsPoolInfo.InvestorRemittanceDay.ToString());
            if (mbsPoolInfo.IssueDate != DateTime.MinValue)
              this.loanDataMgr.LoanData.SetField("ULDD.X68", mbsPoolInfo.IssueDate.ToString("yyyy-MM-dd"));
            this.loanDataMgr.LoanData.SetField("ULDD.FNM.X70", Convert.ToInt32(mbsPoolInfo.OwnershipPercent).ToString());
            string val10;
            switch (mbsPoolInfo.StructureType)
            {
              case "Investor Defined Multiple Lender":
                val10 = "InvestorDefinedMultipleLender";
                break;
              case "Lender Initiated Multiple Lender":
                val10 = "LenderInitiatedMultipleLender";
                break;
              case "Single Lender":
                val10 = "SingleLender";
                break;
              default:
                val10 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X71", val10);
            string val11;
            switch (mbsPoolInfo.AccrualRateStructureType)
            {
              case "Stated Structure":
                val11 = "StatedStructure";
                break;
              case "Weighted Average Structure":
                val11 = "WeightedAverageStructure";
                break;
              default:
                val11 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X73", val11);
            LoanData loanData3 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.SecurityIssueDateIntRate;
            string val12 = num6.ToString();
            loanData3.SetField("ULDD.X74", val12);
            LoanData loanData4 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.MinAccuralRate;
            string val13 = num6.ToString();
            loanData4.SetField("ULDD.X75", val13);
            LoanData loanData5 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.MaxAccuralRate;
            string val14 = num6.ToString();
            loanData5.SetField("ULDD.X76", val14);
            LoanData loanData6 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.MarginRate;
            string val15 = num6.ToString();
            loanData6.SetField("ULDD.X77", val15);
            string val16;
            switch (mbsPoolInfo.IntRateRoundingType)
            {
              case "Down":
                val16 = "Down";
                break;
              case "Nearest":
                val16 = "Nearest";
                break;
              case "No Rounding":
                val16 = "NoRounding";
                break;
              case "Up":
                val16 = "Up";
                break;
              default:
                val16 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X78", val16);
            LoanData loanData7 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.IntRateRoundingPercent;
            string val17 = num6.ToString();
            loanData7.SetField("ULDD.X79", val17);
            this.loanDataMgr.LoanData.SetField("ULDD.X80", mbsPoolInfo.IsInterestOnly ? "Y" : "N");
            this.loanDataMgr.LoanData.SetField("ULDD.X81", mbsPoolInfo.IntPaymentAdjIndexLeadDays.ToString());
            this.loanDataMgr.LoanData.SetField("ULDD.X83", mbsPoolInfo.IsAssumability ? "Y" : "N");
            this.loanDataMgr.LoanData.SetField("ULDD.X84", mbsPoolInfo.IsBalloon ? "Y" : "N");
            if (!skipFieldList.Contains("Investor Information"))
            {
              this.loanDataMgr.LoanData.SetField("ULDD.X85", val2);
              if (mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMae || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
                this.loanDataMgr.LoanData.SetField("ULDD.X40", val3);
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X86", mbsPoolInfo.ScheduledRemittancePaymentDay.ToString());
            if (mbsPoolInfo.SecurityTradeBookEntryDate != DateTime.MinValue)
              this.loanDataMgr.LoanData.SetField("ULDD.X87", mbsPoolInfo.SecurityTradeBookEntryDate.ToString("yyyy-MM-dd"));
            this.loanDataMgr.LoanData.SetField("ULDD.X117", mbsPoolInfo.PayeeCode);
            string val18;
            switch (mbsPoolInfo.InvestorRemittanceType)
            {
              case "Actual Interest Actual Principal":
                val18 = "ActualInterestActualPrincipal";
                break;
              case "Scheduled Interest Actual Principal":
                val18 = "ScheduledInterestActualPrincipal";
                break;
              case "Scheduled Interest Scheduled Principal":
                val18 = "ScheduledInterestScheduledPrincipal";
                break;
              default:
                val18 = "";
                break;
            }
            this.loanDataMgr.LoanData.SetField("ULDD.X44", val18);
          }
          if (mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.GinnieMae)
          {
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.IssuerId", mbsPoolInfo.IssuerNum);
            this.loanDataMgr.LoanData.SetField("ULDD.X72", mbsPoolInfo.GinniePoolType);
            this.loanDataMgr.LoanData.SetField("ULDD.X68", mbsPoolInfo.IssueDate == DateTime.MinValue ? "" : mbsPoolInfo.IssueDate.ToString("yyyy-MM-dd"));
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHBnkAccntPrpsTrnstIdntfr", mbsPoolInfo.MasterTnIABA);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolMaturityDate", mbsPoolInfo.MaturityDate == DateTime.MinValue ? "" : mbsPoolInfo.MaturityDate.ToString("yyyy-MM-dd"));
            LoanData loanData8 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.MbsMargin;
            string val19 = num6.ToString();
            loanData8.SetField("ULDD.X77", val19);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.BondFinPoolIndic", mbsPoolInfo.IsBondFinancePool ? "Y" : "N");
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHBnkAccntIdentfr", mbsPoolInfo.MasterTnIAcctNum);
            LoanData loanData9 = this.loanDataMgr.LoanData;
            num6 = mbsPoolInfo.GuaranteeFee;
            string val20 = num6.ToString();
            loanData9.SetField("ULDD.GNM.GRNTYPrcnt", val20);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHBnkAccntPrpsTyp", mbsPoolInfo.ACHBankAccountPurposeType);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHABARtngAndTrnstNmbr", mbsPoolInfo.ACHABARoutingAndTransitNum);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHABARoutingAndTransitIdentifier", mbsPoolInfo.ACHABARoutingAndTransitId);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHInstttnTlgrphcAbbrName", mbsPoolInfo.ACHInsitutionTelegraphicName);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHRcvrSbccntName", mbsPoolInfo.ACHReceiverSubaccountName);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.ACHBnkAccntDscr", mbsPoolInfo.ACHBankAccountDescription);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.IndxType", mbsPoolInfo.GinniePoolIndexType);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolIssuerTransferee", mbsPoolInfo.PoolIssuerTransferee);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolCertPaymtDate", mbsPoolInfo.PoolCertificatePaymentDate == DateTime.MinValue ? "" : mbsPoolInfo.PoolCertificatePaymentDate.ToString("yyyy-MM-dd"));
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.BondFinProgType", mbsPoolInfo.BondFinanceProgramType.Replace(" ", ""));
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.BondFinProgName", mbsPoolInfo.BondFinanceProgramName);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolClassType", mbsPoolInfo.GinniePoolClassType);
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolConcurTrnsfrIndic", mbsPoolInfo.GinniePoolConcurrentTransferIndicator ? "Y" : "N");
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolCurLoanCount", mbsPoolInfo.PoolCurrentLoanCount.ToString());
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolCurPrinBalAmt", mbsPoolInfo.PoolCurrentPrincipalBalAmt.ToString());
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolingMethodType", mbsPoolInfo.PoolingMethodType.Replace(" ", ""));
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolIntAdjEffDate", mbsPoolInfo.PoolInterestAdjustmentEffectiveDate == DateTime.MinValue ? "" : mbsPoolInfo.PoolInterestAdjustmentEffectiveDate.ToString("yyyy-MM-dd"));
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.PoolMaturityPeriodCount", mbsPoolInfo.PoolMaturityPeriodCount.ToString());
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.DocSubmissionIndic", mbsPoolInfo.DocSubmissionIndicator ? "Y" : "N");
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.DocReqIndic", mbsPoolInfo.DocReqIndicator ? "Y" : "N");
            this.loanDataMgr.LoanData.SetField("ULDD.GNM.SecurityOrigSubscrAmt", mbsPoolInfo.SecurityOrigSubscriptionAmt.ToString());
            this.loanDataMgr.LoanData.SetField("ULDD.X65", mbsPoolInfo.Name.ToString());
          }
          if (mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMae || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FreddieMac || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
            this.loanDataMgr.LoanData.SetField("997", mbsPoolInfo.SellerId);
          if (mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMae || mbsPoolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
            this.loanDataMgr.LoanData.SetField("ULDD.X33", "Y");
          this.loanDataMgr.LoanData.Calculator.FormCalculation("PurchaseAdvice", (string) null, (string) null);
          this.ApplyInvestorToLoan(trade.Investor, trade.Assignee, true, skipFieldList);
          Tracing.Log(TradeToLoanBase.sw, nameof (MbsPoolToLoan), TraceLevel.Info, "Trade data refresh completed");
        }
      }
    }

    private void CreateSnapshotForRateLocked(
      LockRequestLog oldLockRequestLog,
      Hashtable snapshotData)
    {
      LockRequestLog lockRequestLogToCopy = new LockRequestLog(this.loanDataMgr.LoanData.GetLogList());
      oldLockRequestLog.Copy(ref lockRequestLogToCopy);
      DateTime serverTime = this.sessionObjects.Session.ServerTime;
      lockRequestLogToCopy.DisplayInLog = false;
      lockRequestLogToCopy.LockRequestStatus = RateLockRequestStatus.RateLocked;
      lockRequestLogToCopy.SetRequestingUser(this.sessionObjects.UserID, this.sessionObjects.UserInfo.FullName);
      lockRequestLogToCopy.Date = serverTime;
      lockRequestLogToCopy.AddLockRequestSnapshot(snapshotData);
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) lockRequestLogToCopy);
      this.loanDataMgr.SyncSellComparisonToLoan(snapshotData);
      this.loanDataMgr.updateOldLockStatus(lockRequestLogToCopy, (List<string>) null);
      LockConfirmLog recentConfirmedLock = this.loanDataMgr.LoanData.GetLogList().GetMostRecentConfirmedLock();
      if (recentConfirmedLock == null || !oldLockRequestLog.Guid.Equals(recentConfirmedLock.RequestGUID))
        return;
      LockConfirmLog lockConfirmLog = this.loanDataMgr.CreateLockConfirmLog(lockRequestLogToCopy, snapshotData, this.sessionObjects.UserInfo);
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) lockConfirmLog);
    }

    private string GetGuaranteeFeeValue(
      MbsPoolInfo mbsPool,
      Dictionary<string, string> updateFieldList)
    {
      LockUtils lockUtils = new LockUtils(this.sessionObjects);
      return !updateFieldList.ContainsKey("3889") ? (mbsPool.GuaranteeFee == 0M ? "" : lockUtils.FormatSecondaryDecimalDigits("3889", mbsPool.GuaranteeFee)) : (Utils.ToDouble(updateFieldList["3889"]) == 0.0 ? "" : lockUtils.FormatSecondaryDecimalDigits("3889", updateFieldList["3889"]));
    }

    public override void RemoveFromTrade(
      TradeInfoObj trade,
      bool rejected,
      List<string> skipFieldList)
    {
      base.RemoveFromTrade(trade, rejected, skipFieldList);
      this.ClearUlddFields(((MbsPoolInfo) trade).PoolMortgageType);
    }
  }
}
