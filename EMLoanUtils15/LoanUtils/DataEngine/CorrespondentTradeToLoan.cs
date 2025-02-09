// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.CorrespondentTradeToLoan
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientServer.Trading;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  internal class CorrespondentTradeToLoan(SessionObjects sessionObjects, LoanDataMgr loanDataMgr) : 
    CorrespondentTradeToLoanBase(sessionObjects, loanDataMgr)
  {
    private const string className = "CorrespondentTradeToLoan�";
    private const string EppsPricingNotFound = "ICE PPE pricing for tradeId: {0} and LoanId: {1} not found.�";
    private const string EppsLoanNotEligible = "ICE PPE pricing for tradeId: {0} and LoanId: {1} is not eligible�";
    private const string EppsSrpPriceNotFound = "ICE PPE SRP pricing for tradeId: {0} and LoanId: {1} not found.�";
    private const string EppsAdjustmetPriceNotFound = "ICE PPE LLPA pricing for tradeId: {0} and LoanId: {1} not found.�";

    public override void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price)
    {
      this.ModifyTradeStatus(trade, status, skipFieldList, price, new Dictionary<string, string>());
    }

    public override void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList)
    {
      Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoan), TraceLevel.Info, "Setting Correspondent trade status to " + status + "..");
      if (this.loanDataMgr.LoanData.GetField("3914") != trade.Guid)
        return;
      if ((CorrespondentTradeLoanStatus) status == CorrespondentTradeLoanStatus.Shipped)
      {
        if (Utils.ParseDate((object) this.loanDataMgr.LoanData.GetField("2014")) == DateTime.MinValue)
          this.loanDataMgr.LoanData.SetField("2014", DateTime.Now.ToString("MM/dd/yyyy"));
      }
      else if ((CorrespondentTradeLoanStatus) status == CorrespondentTradeLoanStatus.Purchased)
      {
        this.loanDataMgr.LoanData.SetField("2014", "");
        if (Utils.ParseDate((object) this.loanDataMgr.LoanData.GetField("2370")) == DateTime.MinValue)
          this.loanDataMgr.LoanData.SetField("2370", DateTime.Now.ToString("MM/dd/yyyy"));
      }
      this.RefreshTradeData(trade, skipFieldList, price, updateFieldList, LoanDataMgr.LockLoanSyncOption.syncLockToLoan, false);
    }

    public override void CopyTradeDataToSnapshot(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Hashtable newData,
      Dictionary<string, string> updateFieldList = null)
    {
      CorrespondentTradeInfo correspondentTradeInfo = (CorrespondentTradeInfo) trade;
      bool flag1 = correspondentTradeInfo.IsForIndividualLoan();
      Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoan), TraceLevel.Info, "Refreshing trade data from trade " + (object) correspondentTradeInfo.TradeID + "..");
      LockUtils lockUtils = new LockUtils(this.sessionObjects);
      if (this.loanDataMgr.LoanData.GetField("3914") != trade.Guid)
        return;
      this.loanDataMgr.LoanData.SetField("3915", trade.Name);
      Decimal origValue1 = new Decimal(0.0);
      Decimal num1 = Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("3"));
      if (correspondentTradeInfo.DeliveryType == CorrespondentMasterDeliveryType.Bulk || correspondentTradeInfo.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT)
      {
        if (updateFieldList.ContainsKey("TotalPrice"))
          origValue1 = Utils.ParseDecimal((object) updateFieldList["TotalPrice"]);
      }
      else
        origValue1 = correspondentTradeInfo.GetBasePriceForNoteRate(num1, price);
      if (!flag1)
      {
        Decimal loanAmount = Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("2"));
        bool impoundsWaived = this.loanDataMgr.LoanData.GetField("2293") == "Waived";
        string field = this.loanDataMgr.LoanData.GetField("14");
        Decimal num2 = 0M;
        int num3 = 0;
        TradeLoanEligibilityPricingInfo pricingInfo = (TradeLoanEligibilityPricingInfo) null;
        string guid = this.loanDataMgr.LoanData.GUID;
        int tradeId = correspondentTradeInfo.TradeID;
        bool flag2 = false;
        if (updateFieldList.ContainsKey("EPPSLoanProgramName") && !string.IsNullOrEmpty(updateFieldList["EPPSLoanProgramName"]))
        {
          newData[(object) "2866"] = (object) updateFieldList["EPPSLoanProgramName"];
          flag2 = true;
        }
        if (this.loanDataMgr.IsFromPlatform)
        {
          pricingInfo = this.GetPricingInfoFromEpps(tradeId, guid);
          if (pricingInfo != null)
          {
            if (pricingInfo.IsEligible)
            {
              if ((correspondentTradeInfo.EPPSLoanProgramsFilter == null || correspondentTradeInfo.EPPSLoanProgramsFilter.Count <= 0 ? (EPPSLoanProgramFilters) null : correspondentTradeInfo.EPPSLoanProgramsFilter) != null)
              {
                if (!flag2)
                {
                  try
                  {
                    EPPSLoanProgramFilter loanProgramFilter = correspondentTradeInfo.EPPSLoanProgramsFilter.First<EPPSLoanProgramFilter>((Func<EPPSLoanProgramFilter, bool>) (x => x.ProgramId.Equals(pricingInfo.ProgramId.ToString())));
                    if (loanProgramFilter != null)
                    {
                      if (!string.IsNullOrEmpty(loanProgramFilter.ProgramName))
                        newData[(object) "2866"] = (object) loanProgramFilter.ProgramName;
                    }
                  }
                  catch (Exception ex)
                  {
                  }
                }
              }
            }
            else if (!pricingInfo.IsEligible && (correspondentTradeInfo.AdjustmentsfromPPE || correspondentTradeInfo.SRPfromPPE))
              this.ThrowEppsPricingException("ICE PPE pricing for tradeId: {0} and LoanId: {1} is not eligible", (object) tradeId, (object) guid);
          }
        }
        if (correspondentTradeInfo.AdjustmentsfromPPE && this.loanDataMgr.IsFromPlatform)
        {
          List<TradeLoanEligibilityPricingItem> pricingItemsFromEpps = this.GetPricingItemsFromEpps(pricingInfo, TradeLoanEligibilityType.Adjustment);
          if (pricingItemsFromEpps == null || pricingItemsFromEpps.Count == 0)
            this.ThrowEppsPricingException("ICE PPE LLPA pricing for tradeId: {0} and LoanId: {1} not found.", (object) tradeId, (object) guid);
          foreach (TradeLoanEligibilityPricingItem eligibilityPricingItem in pricingItemsFromEpps)
          {
            newData[(object) (2162 + 2 * num3).ToString()] = (object) eligibilityPricingItem.Description;
            newData[(object) (2162 + 2 * num3 + 1).ToString()] = (object) eligibilityPricingItem.Price.ToString("0.000");
            num2 += eligibilityPricingItem.Rate;
            ++num3;
          }
        }
        else if (!correspondentTradeInfo.AdjustmentsfromPPE)
        {
          for (int index = 0; index < trade.PriceAdjustments.Count && num3 < 20; ++index)
          {
            TradePriceAdjustment priceAdjustment = trade.PriceAdjustments[index];
            if (priceAdjustment.CriterionList.CreateEvaluator().Evaluate(this.loanDataMgr.LoanData, FilterEvaluationOption.None))
            {
              newData[(object) (2162 + 2 * num3).ToString()] = (object) priceAdjustment.CriterionList.ToString();
              newData[(object) (2162 + 2 * num3 + 1).ToString()] = (object) priceAdjustment.PriceAdjustment.ToString("0.000");
              num2 += priceAdjustment.PriceAdjustment;
              ++num3;
            }
          }
        }
        else
          num2 = Utils.ParseDecimal(newData[(object) "2202"], 0M);
        int num4;
        if (!correspondentTradeInfo.AdjustmentsfromPPE)
        {
          for (; num3 < 20; ++num3)
          {
            Hashtable hashtable1 = newData;
            num4 = 2162 + 2 * num3;
            string key1 = num4.ToString();
            hashtable1[(object) key1] = (object) "";
            Hashtable hashtable2 = newData;
            num4 = 2162 + 2 * num3 + 1;
            string key2 = num4.ToString();
            hashtable2[(object) key2] = (object) "";
          }
        }
        Decimal origValue2;
        if (correspondentTradeInfo.SRPfromPPE && this.loanDataMgr.IsFromPlatform)
        {
          List<TradeLoanEligibilityPricingItem> pricingItemsFromEpps = this.GetPricingItemsFromEpps(pricingInfo, TradeLoanEligibilityType.SRP);
          if (pricingItemsFromEpps == null || pricingItemsFromEpps.Count == 0)
            this.ThrowEppsPricingException("ICE PPE SRP pricing for tradeId: {0} and LoanId: {1} not found.", (object) tradeId, (object) guid);
          origValue2 = pricingItemsFromEpps.Select<TradeLoanEligibilityPricingItem, Decimal>((Func<TradeLoanEligibilityPricingItem, Decimal>) (p => p.Price)).Sum();
        }
        else
          origValue2 = correspondentTradeInfo.SRPfromPPE ? Utils.ParseDecimal(newData[(object) "2205"], 0M) : trade.SRPTable.GetAdjustmentForLoan(loanAmount, impoundsWaived, field);
        newData[(object) "2205"] = (object) lockUtils.FormatSecondaryDecimalDigits("2205", origValue2);
        this.SetDateTimeField(newData, "2149", correspondentTradeInfo.CommitmentDate);
        this.SetDateTimeField(newData, "2151", correspondentTradeInfo.ExpirationDate);
        TimeSpan timeSpan;
        if (!string.IsNullOrEmpty(newData[(object) "2149"] as string) && !string.IsNullOrEmpty(newData[(object) "2151"] as string))
        {
          timeSpan = correspondentTradeInfo.ExpirationDate - correspondentTradeInfo.CommitmentDate;
          int totalDays = (int) timeSpan.TotalDays;
          newData[(object) "2150"] = (object) (totalDays > 0 ? totalDays : 0);
        }
        newData[(object) "2152"] = (object) lockUtils.FormatSecondaryDecimalDigits("2152", num1);
        newData[(object) "2160"] = (object) lockUtils.FormatSecondaryDecimalDigits("2160", num1);
        newData[(object) "2161"] = (object) lockUtils.FormatSecondaryDecimalDigits("2161", origValue1);
        if (!skipFieldList.Contains("3420"))
          newData[(object) "3420"] = (object) lockUtils.FormatSecondaryDecimalDigits("3420", origValue1);
        newData[(object) "2202"] = (object) num2.ToString("0.000");
        newData[(object) "2203"] = (object) lockUtils.FormatSecondaryDecimalDigits("2203", origValue1 + num2);
        newData[(object) "2205"] = (object) origValue2.ToString("0.000");
        newData[(object) "2218"] = (object) (origValue1 + num2 + origValue2);
        this.SetDateTimeField(newData, "2089", correspondentTradeInfo.CommitmentDate);
        this.SetDateTimeField(newData, "2091", correspondentTradeInfo.ExpirationDate);
        if (!string.IsNullOrEmpty(newData[(object) "2089"] as string) && !string.IsNullOrEmpty(newData[(object) "2091"] as string))
        {
          timeSpan = correspondentTradeInfo.ExpirationDate - correspondentTradeInfo.CommitmentDate;
          int totalDays = (int) timeSpan.TotalDays;
          newData[(object) "2090"] = (object) (totalDays > 0 ? totalDays : 0);
        }
        newData[(object) "2092"] = (object) lockUtils.FormatSecondaryDecimalDigits("2092", num1);
        newData[(object) "2100"] = (object) lockUtils.FormatSecondaryDecimalDigits("2100", num1);
        newData[(object) "2101"] = (object) lockUtils.FormatSecondaryDecimalDigits("2101", origValue1);
        int num5 = 0;
        for (int index = 0; index < trade.PriceAdjustments.Count && num5 < 20; ++index)
        {
          TradePriceAdjustment priceAdjustment = trade.PriceAdjustments[index];
          if (priceAdjustment.CriterionList.CreateEvaluator().Evaluate(this.loanDataMgr.LoanData, FilterEvaluationOption.None))
          {
            Hashtable hashtable3 = newData;
            num4 = 2102 + 2 * num5;
            string key3 = num4.ToString();
            string str1 = priceAdjustment.CriterionList.ToString();
            hashtable3[(object) key3] = (object) str1;
            Hashtable hashtable4 = newData;
            num4 = 2102 + 2 * num5 + 1;
            string key4 = num4.ToString();
            string str2 = priceAdjustment.PriceAdjustment.ToString("0.000");
            hashtable4[(object) key4] = (object) str2;
            ++num5;
          }
        }
        for (; num5 < 20; ++num5)
        {
          Hashtable hashtable5 = newData;
          num4 = 2102 + 2 * num5;
          string key5 = num4.ToString();
          hashtable5[(object) key5] = (object) "";
          Hashtable hashtable6 = newData;
          num4 = 2102 + 2 * num5 + 1;
          string key6 = num4.ToString();
          hashtable6[(object) key6] = (object) "";
        }
        newData[(object) "2143"] = (object) lockUtils.FormatSecondaryDecimalDigits("2143", origValue1 + num2);
      }
      newData[(object) "3902"] = (object) correspondentTradeInfo.Name;
      newData[(object) "3903"] = (object) correspondentTradeInfo.CorrespondentMasterCommitmentNumber;
      this.SetDateTimeField(newData, "3909", correspondentTradeInfo.CommitmentDate);
      this.SetDateTimeField(newData, "3912", correspondentTradeInfo.ExpirationDate);
      newData[(object) "3910"] = (object) correspondentTradeInfo.CommitmentType.ToDescription();
      newData[(object) "3911"] = (object) correspondentTradeInfo.DeliveryType.ToDescription();
      this.SetDateTimeField(newData, "3913", correspondentTradeInfo.DeliveryExpirationDate);
      this.loanDataMgr.LoanData.SetField("4105", correspondentTradeInfo.CommitmentType.ToDescription());
    }

    private TradeLoanEligibilityPricingInfo GetPricingInfoFromEpps(int tradeId, string loanId)
    {
      return ((ITradeLoanEligibilityManager) this.sessionObjects.Session.GetObject("TradeLoanEligibilityManager")).GeTradeLoanEligibilityByTradeIdAndLoanId(loanId, tradeId);
    }

    private List<TradeLoanEligibilityPricingItem> GetPricingItemsFromEpps(
      TradeLoanEligibilityPricingInfo pricingInfo,
      TradeLoanEligibilityType pricingType)
    {
      return pricingInfo == null || pricingInfo.TradeLoanEligibilityPricingItems == null || pricingInfo.TradeLoanEligibilityPricingItems.Count == 0 ? (List<TradeLoanEligibilityPricingItem>) null : pricingInfo.TradeLoanEligibilityPricingItems.Where<TradeLoanEligibilityPricingItem>((Func<TradeLoanEligibilityPricingItem, bool>) (p => p.Type == pricingType)).ToList<TradeLoanEligibilityPricingItem>();
    }

    private void ThrowEppsPricingException(string errorMessage, params object[] errorArgs)
    {
      throw new Exception(string.Format(errorMessage, errorArgs));
    }
  }
}
