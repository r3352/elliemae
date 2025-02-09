// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTradeService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Trading;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  [Guid("0A5AF675-A1DB-4EFE-B89E-33F622027576")]
  public class LoanTradeService : SessionBoundObject, ILoanTradeService
  {
    internal LoanTradeService(Session session)
      : base(session)
    {
    }

    public LoanTrade GetLoanTrade(int loanTradeId)
    {
      ICursor icursor = ((ILoanTradeManager) this.Session.GetObject("LoanTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("LoanTradeDetails.TradeId", (Array) new int[1]
        {
          loanTradeId
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return icursor != null && icursor.GetItemCount() > 0 ? new LoanTrade(((LoanTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))[0]) : (LoanTrade) null;
    }

    public LoanTrade GetLoanTrade(string loanTradeNumber)
    {
      ICursor icursor = ((ILoanTradeManager) this.Session.GetObject("LoanTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("LoanTradeDetails.Name", (Array) new string[1]
        {
          loanTradeNumber
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return icursor != null && icursor.GetItemCount() > 0 ? new LoanTrade(((LoanTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))[0]) : (LoanTrade) null;
    }

    public List<TradeLoanAssignment> GetTradeLoanAssignments(int loanTradeId)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ILoanTradeManager iloanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      LoanTradeInfo trade = iloanTradeManager.GetTrade(loanTradeId);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in iloanTradeManager.GetAssignedOrPendingLoans(loanTradeId, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    public List<TradeLoanAssignment> GetTradeLoanAssignments(string loanTradeNumber)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ILoanTradeManager iloanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      LoanTradeInfo trade = iloanTradeManager.GetTrade(loanTradeNumber);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in iloanTradeManager.GetAssignedOrPendingLoans(((TradeBase) trade).TradeID, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    public List<LoanTrade> GetLoanTrades(TradeStatus[] tradeStatus)
    {
      List<LoanTrade> loanTrades = new List<LoanTrade>();
      ILoanTradeManager iloanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<TradeStatus>) tradeStatus).Any<TradeStatus>((Func<TradeStatus, bool>) (a => a == TradeStatus.None)))
      {
        int[] numArray = new int[tradeStatus.Length];
        for (int index = 0; index < tradeStatus.Length; ++index)
          numArray[index] = (int) tradeStatus[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("LoanTradeDetails.Status", (Array) numArray, true);
      }
      ICursor icursor = iloanTradeManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (icursor == null || icursor.GetItemCount() <= 0)
        return loanTrades;
      foreach (LoanTradeViewModel info in (LoanTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
        loanTrades.Add(new LoanTrade(info));
      return loanTrades;
    }

    public Dictionary<string, string> GetAdjustmentTemplateList()
    {
      Dictionary<string, string> adjustmentTemplateList = new Dictionary<string, string>();
      foreach (FileSystemEntry settingsFileEntry in ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 11, true))
        adjustmentTemplateList.Add(string.Concat(settingsFileEntry.Properties[(object) "Guid"]), settingsFileEntry.Name ?? "");
      return adjustmentTemplateList;
    }

    public Dictionary<string, string> GetSRPTemplateList()
    {
      Dictionary<string, string> srpTemplateList = new Dictionary<string, string>();
      foreach (FileSystemEntry settingsFileEntry in ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 12, true))
        srpTemplateList.Add(string.Concat(settingsFileEntry.Properties[(object) "Guid"]), settingsFileEntry.Name ?? "");
      return srpTemplateList;
    }

    public LoanUpdateResults AllocateLoansToLoanTrade(
      List<string> loanNumbers,
      string loanTradeName)
    {
      if (loanNumbers == null || loanNumbers.Count<string>() <= 0)
        return (LoanUpdateResults) null;
      loanNumbers.RemoveAll((Predicate<string>) (l => l == string.Empty));
      LoanUpdateResults loanTrade = new LoanUpdateResults(loanNumbers);
      foreach (string loanNumber in loanNumbers)
      {
        try
        {
          LoanTradeManager.AllocateLoanToLoanTrade(loanNumber, loanTradeName, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
        }
        catch (TradeLoanUpdateException ex)
        {
          loanTrade.LoansWithError.Add(loanNumber, ((Exception) ex).Message);
        }
        catch (Exception ex)
        {
          loanTrade.LoansWithError.Add(loanNumber, ex.Message);
        }
        finally
        {
          loanTrade.ProcessedLoans.Add(loanNumber);
        }
      }
      return loanTrade;
    }

    public LoanUpdateResults AllocateLoansWithTotalPriceToLoanTrade(
      Dictionary<string, Decimal> loans,
      string loanTradeName)
    {
      if (loans == null || loans.Count<KeyValuePair<string, Decimal>>() <= 0)
        return (LoanUpdateResults) null;
      foreach (string key in loans.Keys)
      {
        if (key == string.Empty)
          loans.Remove(key);
      }
      LoanUpdateResults loanTrade = new LoanUpdateResults(loans.Keys.ToList<string>());
      foreach (string key in loans.Keys)
      {
        try
        {
          LoanTradeManager.AllocateLoanToLoanTrade(key, loans[key], loanTradeName, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
        }
        catch (TradeLoanUpdateException ex)
        {
          loanTrade.LoansWithError.Add(key, ((Exception) ex).Message);
        }
        catch (Exception ex)
        {
          loanTrade.LoansWithError.Add(key, ex.Message);
        }
        finally
        {
          loanTrade.ProcessedLoans.Add(key);
        }
      }
      return loanTrade;
    }

    public int CreateLoanTrade(LoanTrade trade)
    {
      ILoanTradeManager iloanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      ISecurityTradeManager isecurityTradeManager = (ISecurityTradeManager) this.Session.GetObject("SecurityTradeManager");
      IMasterContractManager imasterContractManager = (IMasterContractManager) this.Session.GetObject("MasterContractManager");
      string message = this.ValidateLoanTrade(trade);
      if (!string.IsNullOrEmpty(message))
        throw new Exception(message);
      LoanTradeInfo loanTradeInfo = trade.GetLoanTradeInfo();
      if (!string.IsNullOrEmpty(loanTradeInfo.ContractNumber))
        ((TradeInfoObj) loanTradeInfo).ContractID = ((MasterContractSummaryInfo) imasterContractManager.GetContractByContractNumber(loanTradeInfo.ContractNumber)).ContractID;
      this.ApplyInvestorTemplate(loanTradeInfo);
      return iloanTradeManager.CreateTrade(loanTradeInfo);
    }

    public int CreateLoanTrade(
      LoanTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID)
    {
      ILoanTradeManager iloanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      ISecurityTradeManager isecurityTradeManager = (ISecurityTradeManager) this.Session.GetObject("SecurityTradeManager");
      IMasterContractManager imasterContractManager = (IMasterContractManager) this.Session.GetObject("MasterContractManager");
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      string message = this.ValidateLoanTrade(trade);
      if (!string.IsNullOrEmpty(message))
        throw new Exception(message);
      LoanTradeInfo loanTradeInfo = trade.GetLoanTradeInfo();
      if (!string.IsNullOrEmpty(priceAdjustmentTemplateGUID))
      {
        PriceAdjustmentTemplate adjustmentTemplate = PriceAdjustmentTemplate.op_Explicit(iconfigurationManager.GetTemplateSettings((TemplateSettingsType) 11, ((IEnumerable<FileSystemEntry>) iconfigurationManager.GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 11, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == priceAdjustmentTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find Price Adjustment Template for GUID - " + priceAdjustmentTemplateGUID)));
        ((TradeInfoObj) loanTradeInfo).PriceAdjustments = adjustmentTemplate.PriceAdjustments;
      }
      if (!string.IsNullOrEmpty(SRPTemplateGUID))
      {
        SRPTableTemplate srpTableTemplate = SRPTableTemplate.op_Explicit(iconfigurationManager.GetTemplateSettings((TemplateSettingsType) 12, ((IEnumerable<FileSystemEntry>) iconfigurationManager.GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 12, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == SRPTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find SRP Template for GUID - " + priceAdjustmentTemplateGUID)) ?? throw new Exception("Could not find SRP Template for GUID - " + SRPTemplateGUID));
        ((TradeInfoObj) loanTradeInfo).SRPTable = srpTableTemplate.SRPTable;
      }
      this.ApplyInvestorTemplate(loanTradeInfo);
      return iloanTradeManager.CreateTrade(loanTradeInfo);
    }

    public void AssignSecurityTrade(string securityId, DateTime assignedDate, string loanTradeId)
    {
      ILoanTradeManager iloanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      ISecurityTradeManager isecurityTradeManager = (ISecurityTradeManager) this.Session.GetObject("SecurityTradeManager");
      if (assignedDate == DateTime.MinValue)
        throw new Exception("AssignedDate must be set.");
      LoanTradeInfo trade = iloanTradeManager.GetTrade(loanTradeId);
      if (trade == null)
        throw new Exception("Loan Trade not found.");
      SecurityTradeInfo tradeByName = isecurityTradeManager.GetTradeByName(securityId);
      SecurityTradeAssignment securityTradeAssignment = tradeByName != null ? new SecurityTradeAssignment(tradeByName, trade) : throw new Exception("Security Trade not found.");
      if (assignedDate > DateTime.MinValue)
        securityTradeAssignment.AssignedStatusDate = assignedDate;
      if (securityTradeAssignment == null)
        return;
      isecurityTradeManager.AssignLoanTradeToTrade(((TradeBase) tradeByName).TradeID, ((TradeBase) trade).TradeID, (SecurityLoanTradeStatus) 2, securityTradeAssignment.AssignedStatusDate);
    }

    private string ValidateLoanTrade(LoanTrade trade)
    {
      ILoanTradeManager iloanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      IMasterContractManager imasterContractManager = (IMasterContractManager) this.Session.GetObject("MasterContractManager");
      foreach (KeyValuePair<Decimal, Decimal> basePriceItem in trade.BasePriceItems)
      {
        if (basePriceItem.Key > 99999.99M)
          throw new Exception("Rate cannot exceed maximum limit of 99999.99.");
        if (basePriceItem.Value >= 1000M || basePriceItem.Value < 0M)
          throw new Exception("Price must be greater than or equal to 0 and less than 1000.");
      }
      if (string.IsNullOrEmpty(trade.CommitmentNumber))
        return "Trade Id can not be blank.";
      if (iloanTradeManager.GetTradeByName(trade.CommitmentNumber) != null)
        return "Loan Trade with Trade Id " + trade.CommitmentNumber + " already exists.";
      if (trade.IsBulkDelivery && trade.IsWeightedAvgBulkPriceLocked && trade.WeightedAvgBulkPrice == 0M)
        return "WeightedAvgBulkPrice can not be zero if IsBulkDelivery and IsWeightedAvgBulkPriceLocked is set to true.";
      return !string.IsNullOrEmpty(trade.MasterContractNumber) && imasterContractManager.GetContractByContractNumber(trade.MasterContractNumber) == null ? "Master Contract to be associated can not be found." : "";
    }

    private void ApplyInvestorTemplate(LoanTradeInfo lt)
    {
      if (string.IsNullOrEmpty(((TradeInfoObj) lt).InvestorName))
        return;
      InvestorTemplate investorTemplateByName = this.GetInvestorTemplateByName(((TradeInfoObj) lt).InvestorName);
      if (investorTemplateByName == null || investorTemplateByName.CompanyInformation == null)
        return;
      ((TradeInfoObj) lt).Investor.CopyFrom(investorTemplateByName.CompanyInformation);
      if (!(((TradeInfoObj) lt).CommitmentDate != DateTime.MinValue) || !(((TradeInfoObj) lt).InvestorDeliveryDate == DateTime.MinValue) || investorTemplateByName.CompanyInformation.DeliveryTimeFrame <= 0)
        return;
      ((TradeInfoObj) lt).InvestorDeliveryDate = ((TradeInfoObj) lt).CommitmentDate.AddDays((double) investorTemplateByName.CompanyInformation.DeliveryTimeFrame);
    }

    private InvestorTemplate GetInvestorTemplateByName(string name)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      foreach (FileSystemEntry settingsFileEntry in iconfigurationManager.GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 14, true))
      {
        if (settingsFileEntry.Properties[(object) "BulkSale"].ToString() == "Yes" && settingsFileEntry.Name.ToLower() == name.ToLower())
          return InvestorTemplate.op_Explicit(iconfigurationManager.GetTemplateSettings((TemplateSettingsType) 14, settingsFileEntry));
      }
      return (InvestorTemplate) null;
    }
  }
}
