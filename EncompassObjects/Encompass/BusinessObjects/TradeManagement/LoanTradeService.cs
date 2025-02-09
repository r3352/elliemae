// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTradeService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

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
  /// <summary>Represents loan trade service.</summary>
  [Guid("0A5AF675-A1DB-4EFE-B89E-33F622027576")]
  public class LoanTradeService : SessionBoundObject, ILoanTradeService
  {
    internal LoanTradeService(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> by trade id
    /// </summary>
    /// <param name="loanTradeId">Loan Trade Id</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> when passing in the identifier of loan trade
    /// </remarks>
    public LoanTrade GetLoanTrade(int loanTradeId)
    {
      ICursor cursor = ((ILoanTradeManager) this.Session.GetObject("LoanTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("LoanTradeDetails.TradeId", (Array) new int[1]
        {
          loanTradeId
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return cursor != null && cursor.GetItemCount() > 0 ? new LoanTrade(((LoanTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))[0]) : (LoanTrade) null;
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" />  by loan trade commitment number
    /// </summary>
    /// <param name="loanTradeNumber">Loan Trade Commitment Number</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /></returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> when passing in the loan trade number
    /// </remarks>
    public LoanTrade GetLoanTrade(string loanTradeNumber)
    {
      ICursor cursor = ((ILoanTradeManager) this.Session.GetObject("LoanTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("LoanTradeDetails.Name", (Array) new string[1]
        {
          loanTradeNumber
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return cursor != null && cursor.GetItemCount() > 0 ? new LoanTrade(((LoanTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))[0]) : (LoanTrade) null;
    }

    /// <summary>
    /// Get Assigned/Pending Loans for the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" />
    /// </summary>
    /// <param name="loanTradeId">Trade Id</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /> with some basic loan information
    /// and loan assignment information when passing in the identifier of loan trade
    /// </remarks>
    public List<TradeLoanAssignment> GetTradeLoanAssignments(int loanTradeId)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ILoanTradeManager loanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      LoanTradeInfo trade = loanTradeManager.GetTrade(loanTradeId);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in loanTradeManager.GetAssignedOrPendingLoans(loanTradeId, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    /// <summary>
    /// Get Assigned/Pending Loans for the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" />
    /// </summary>
    /// <param name="loanTradeNumber">Commitment Number</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /> with some basic loan information
    /// and loan assignment information when passing in the identifier of loan trade
    /// </remarks>
    public List<TradeLoanAssignment> GetTradeLoanAssignments(string loanTradeNumber)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ILoanTradeManager loanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      LoanTradeInfo trade = loanTradeManager.GetTrade(loanTradeNumber);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in loanTradeManager.GetAssignedOrPendingLoans(trade.TradeID, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> by trade status
    /// </summary>
    /// <param name="tradeStatus"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeStatus" />List</param>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /></returns>
    /// <remarks>
    /// Example 1: Get all active loan trades
    ///                  TradeStatus[] statusList = new TradeStatus[4] { TradeStatus.Open, TradeStatus.Committed, TradeStatus.Shipped and TradeStatus.Purchased };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.LoanTradeService.GetLoanTrades(statusList);
    /// Example 2: Get all archived loan trades
    ///                  TradeStatus[] statusList = new TradeStatus[1] { TradeStatus.Archived };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ArchivedLoanTrades = session.LoanTradeService.GetLoanTrades(statusList);
    /// Example 3: Get all loan trades
    ///                  TradeStatus[] statusList = new TradeStatus[1] { TradeStatus.None };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var AllLoanTrades = session.LoanTradeService.GetLoanTrades(statusList);
    /// </remarks>
    public List<LoanTrade> GetLoanTrades(TradeStatus[] tradeStatus)
    {
      List<LoanTrade> loanTrades = new List<LoanTrade>();
      ILoanTradeManager loanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<TradeStatus>) tradeStatus).Any<TradeStatus>((Func<TradeStatus, bool>) (a => a == TradeStatus.None)))
      {
        int[] valueList = new int[tradeStatus.Length];
        for (int index = 0; index < tradeStatus.Length; ++index)
          valueList[index] = (int) tradeStatus[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("LoanTradeDetails.Status", (Array) valueList, true);
      }
      ICursor cursor = loanTradeManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (cursor == null || cursor.GetItemCount() <= 0)
        return loanTrades;
      foreach (LoanTradeViewModel info in (LoanTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
        loanTrades.Add(new LoanTrade(info));
      return loanTrades;
    }

    /// <summary>
    /// Return a list of Price Adjustment template names with their GUIDS. Template names can be accessed with key which is a GUID.
    /// </summary>
    public Dictionary<string, string> GetAdjustmentTemplateList()
    {
      Dictionary<string, string> adjustmentTemplateList = new Dictionary<string, string>();
      foreach (FileSystemEntry settingsFileEntry in ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.TradePriceAdjustment, true))
        adjustmentTemplateList.Add(string.Concat(settingsFileEntry.Properties[(object) "Guid"]), settingsFileEntry.Name ?? "");
      return adjustmentTemplateList;
    }

    /// <summary>
    /// Return a list of SRP template names with their GUIDS. Template names can be accessed with key which is a GUID.
    /// </summary>
    public Dictionary<string, string> GetSRPTemplateList()
    {
      Dictionary<string, string> srpTemplateList = new Dictionary<string, string>();
      foreach (FileSystemEntry settingsFileEntry in ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.SRPTable, true))
        srpTemplateList.Add(string.Concat(settingsFileEntry.Properties[(object) "Guid"]), settingsFileEntry.Name ?? "");
      return srpTemplateList;
    }

    /// <summary>Allocate a loan to loan trade</summary>
    /// <param name="loanNumbers">a list of Loan Numbers</param>
    /// <param name="loanTradeName">Loan Trade Commitment Number</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanUpdateResults" /></returns>
    /// <remarks>Validate a list of loans and the loan trade for loan allocation. If the loan and the trade are both eligible, process loan allocation by adding the loan to the loan trade and update the loan with the loan trade information.
    /// </remarks>
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
          loanTrade.LoansWithError.Add(loanNumber, ex.Message);
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

    /// <summary>Allocate a loan to loan trade</summary>
    /// <param name="loans">a list of Loan Numbers with assigned Total Price</param>
    /// <param name="loanTradeName">Loan Trade Commitment Number</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanUpdateResults" /></returns>
    /// <remarks>Validate a list of loans and the loan trade for loan allocation. If the loan and the trade are both eligible, process loan allocation by adding the loan to the loan trade and update the loan with the loan trade information.
    /// </remarks>
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
          loanTrade.LoansWithError.Add(key, ex.Message);
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

    /// <summary>Create a loan trade</summary>
    /// <param name="trade"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /></param>
    /// <returns>Id of the trade created.</returns>
    /// <remarks>Validates <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> object, throws exception if failed and creates Loan Trade.
    /// </remarks>
    public int CreateLoanTrade(LoanTrade trade)
    {
      ILoanTradeManager loanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      ISecurityTradeManager securityTradeManager = (ISecurityTradeManager) this.Session.GetObject("SecurityTradeManager");
      IMasterContractManager masterContractManager = (IMasterContractManager) this.Session.GetObject("MasterContractManager");
      string message = this.ValidateLoanTrade(trade);
      if (!string.IsNullOrEmpty(message))
        throw new Exception(message);
      LoanTradeInfo loanTradeInfo = trade.GetLoanTradeInfo();
      if (!string.IsNullOrEmpty(loanTradeInfo.ContractNumber))
        loanTradeInfo.ContractID = masterContractManager.GetContractByContractNumber(loanTradeInfo.ContractNumber).ContractID;
      this.ApplyInvestorTemplate(loanTradeInfo);
      return loanTradeManager.CreateTrade(loanTradeInfo);
    }

    /// <summary>Create a loan trade</summary>
    /// <param name="trade"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /></param>
    /// <param name="priceAdjustmentTemplateGUID">Price adjustment template GUID</param>
    /// <param name="SRPTemplateGUID">SRP template GUID</param>
    /// <returns>Id of the trade created.</returns>
    /// <remarks>Validates <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> object, throws exception if failed to create Loan Trade.
    /// </remarks>
    public int CreateLoanTrade(
      LoanTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID)
    {
      ILoanTradeManager loanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      ISecurityTradeManager securityTradeManager = (ISecurityTradeManager) this.Session.GetObject("SecurityTradeManager");
      IMasterContractManager masterContractManager = (IMasterContractManager) this.Session.GetObject("MasterContractManager");
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      string message = this.ValidateLoanTrade(trade);
      if (!string.IsNullOrEmpty(message))
        throw new Exception(message);
      LoanTradeInfo loanTradeInfo = trade.GetLoanTradeInfo();
      if (!string.IsNullOrEmpty(priceAdjustmentTemplateGUID))
      {
        PriceAdjustmentTemplate templateSettings = (PriceAdjustmentTemplate) configurationManager.GetTemplateSettings(TemplateSettingsType.TradePriceAdjustment, ((IEnumerable<FileSystemEntry>) configurationManager.GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.TradePriceAdjustment, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == priceAdjustmentTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find Price Adjustment Template for GUID - " + priceAdjustmentTemplateGUID));
        loanTradeInfo.PriceAdjustments = templateSettings.PriceAdjustments;
      }
      if (!string.IsNullOrEmpty(SRPTemplateGUID))
      {
        SRPTableTemplate srpTableTemplate = (SRPTableTemplate) (configurationManager.GetTemplateSettings(TemplateSettingsType.SRPTable, ((IEnumerable<FileSystemEntry>) configurationManager.GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.SRPTable, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == SRPTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find SRP Template for GUID - " + priceAdjustmentTemplateGUID)) ?? throw new Exception("Could not find SRP Template for GUID - " + SRPTemplateGUID));
        loanTradeInfo.SRPTable = srpTableTemplate.SRPTable;
      }
      this.ApplyInvestorTemplate(loanTradeInfo);
      return loanTradeManager.CreateTrade(loanTradeInfo);
    }

    /// <summary>Assignes Security Trade to Loan Trade</summary>
    /// <param name="securityId">Security Id/Name of Security Trade</param>
    /// <param name="assignedDate">Assign Date</param>
    /// <param name="loanTradeId">Trade Id of Loan Trade</param>
    /// <remarks>Associates Security Trade to Loan Trade and records Assign Date</remarks>
    public void AssignSecurityTrade(string securityId, DateTime assignedDate, string loanTradeId)
    {
      ILoanTradeManager loanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      ISecurityTradeManager securityTradeManager = (ISecurityTradeManager) this.Session.GetObject("SecurityTradeManager");
      if (assignedDate == DateTime.MinValue)
        throw new Exception("AssignedDate must be set.");
      LoanTradeInfo trade = loanTradeManager.GetTrade(loanTradeId);
      if (trade == null)
        throw new Exception("Loan Trade not found.");
      SecurityTradeInfo tradeByName = securityTradeManager.GetTradeByName(securityId);
      SecurityTradeAssignment securityTradeAssignment = tradeByName != null ? new SecurityTradeAssignment(tradeByName, trade) : throw new Exception("Security Trade not found.");
      if (assignedDate > DateTime.MinValue)
        securityTradeAssignment.AssignedStatusDate = assignedDate;
      if (securityTradeAssignment == null)
        return;
      securityTradeManager.AssignLoanTradeToTrade(tradeByName.TradeID, trade.TradeID, SecurityLoanTradeStatus.Assigned, securityTradeAssignment.AssignedStatusDate);
    }

    private string ValidateLoanTrade(LoanTrade trade)
    {
      ILoanTradeManager loanTradeManager = (ILoanTradeManager) this.Session.GetObject("LoanTradeManager");
      IMasterContractManager masterContractManager = (IMasterContractManager) this.Session.GetObject("MasterContractManager");
      foreach (KeyValuePair<Decimal, Decimal> basePriceItem in trade.BasePriceItems)
      {
        if (basePriceItem.Key > 99999.99M)
          throw new Exception("Rate cannot exceed maximum limit of 99999.99.");
        if (basePriceItem.Value >= 1000M || basePriceItem.Value < 0M)
          throw new Exception("Price must be greater than or equal to 0 and less than 1000.");
      }
      if (string.IsNullOrEmpty(trade.CommitmentNumber))
        return "Trade Id can not be blank.";
      if (loanTradeManager.GetTradeByName(trade.CommitmentNumber) != null)
        return "Loan Trade with Trade Id " + trade.CommitmentNumber + " already exists.";
      if (trade.IsBulkDelivery && trade.IsWeightedAvgBulkPriceLocked && trade.WeightedAvgBulkPrice == 0M)
        return "WeightedAvgBulkPrice can not be zero if IsBulkDelivery and IsWeightedAvgBulkPriceLocked is set to true.";
      return !string.IsNullOrEmpty(trade.MasterContractNumber) && masterContractManager.GetContractByContractNumber(trade.MasterContractNumber) == null ? "Master Contract to be associated can not be found." : "";
    }

    private void ApplyInvestorTemplate(LoanTradeInfo lt)
    {
      if (string.IsNullOrEmpty(lt.InvestorName))
        return;
      InvestorTemplate investorTemplateByName = this.GetInvestorTemplateByName(lt.InvestorName);
      if (investorTemplateByName == null || investorTemplateByName.CompanyInformation == null)
        return;
      lt.Investor.CopyFrom(investorTemplateByName.CompanyInformation);
      if (!(lt.CommitmentDate != DateTime.MinValue) || !(lt.InvestorDeliveryDate == DateTime.MinValue) || investorTemplateByName.CompanyInformation.DeliveryTimeFrame <= 0)
        return;
      lt.InvestorDeliveryDate = lt.CommitmentDate.AddDays((double) investorTemplateByName.CompanyInformation.DeliveryTimeFrame);
    }

    private InvestorTemplate GetInvestorTemplateByName(string name)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      foreach (FileSystemEntry settingsFileEntry in configurationManager.GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.Investor, true))
      {
        if (settingsFileEntry.Properties[(object) "BulkSale"].ToString() == "Yes" && settingsFileEntry.Name.ToLower() == name.ToLower())
          return (InvestorTemplate) configurationManager.GetTemplateSettings(TemplateSettingsType.Investor, settingsFileEntry);
      }
      return (InvestorTemplate) null;
    }
  }
}
