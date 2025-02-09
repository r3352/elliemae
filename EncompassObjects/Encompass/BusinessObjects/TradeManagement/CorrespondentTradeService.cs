// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTradeService
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
using EllieMae.Encompass.BusinessObjects.Settings;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>CorrespondentTradeService Class</summary>
  [Guid("2D55838C-BB75-4351-BC75-A3A53293515E")]
  public class CorrespondentTradeService : SessionBoundObject, ICorrespondentTradeService
  {
    internal CorrespondentTradeService(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> by trade id
    /// </summary>
    /// <param name="correspondentTradeId">Correspondent Trade Id</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> when passing in the identifier of correspondent trade
    /// </remarks>
    public CorrespondentTrade GetCorrespondentTrade(int correspondentTradeId)
    {
      CorrespondentTradeInfo trade = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetTrade(correspondentTradeId);
      return trade != null ? new CorrespondentTrade(trade) : (CorrespondentTrade) null;
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" />  by correspondent commitment number
    /// </summary>
    /// <param name="correspondentTradeNumber">Correspondent Commitment Number</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /></returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> when passing in the correspondent trade number
    /// </remarks>
    public CorrespondentTrade GetCorrespondentTrade(string correspondentTradeNumber)
    {
      CorrespondentTradeInfo trade = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetTrade(correspondentTradeNumber);
      return trade != null ? new CorrespondentTrade(trade) : (CorrespondentTrade) null;
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> associated with the TPO
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> with some basic trade information when passing in the identifier of TPO company
    /// </remarks>
    public List<CorrespondentTrade> GetCorrespondentTradesByTpoId(int Id)
    {
      List<CorrespondentTrade> correspondentTradesByTpoId = new List<CorrespondentTrade>();
      ICursor cursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.ExternalOriginatorManagementID", (Array) new int[1]
        {
          Id
        }, true)
      }, (SortField[]) null, false);
      if (cursor != null && cursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
          correspondentTradesByTpoId.Add(new CorrespondentTrade(info));
      }
      return correspondentTradesByTpoId;
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> associated with the TPO
    /// </summary>
    /// <param name="tpoId">TPO external Id</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> with some basic trade information when passing in the TPO external id
    /// </remarks>
    public List<CorrespondentTrade> GetCorrespondentTradesByTpoId(string tpoId)
    {
      List<CorrespondentTrade> correspondentTradesByTpoId = new List<CorrespondentTrade>();
      ICursor cursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.ExternalID", (Array) new string[1]
        {
          tpoId
        }, true)
      }, (SortField[]) null, false);
      if (cursor != null && cursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
          correspondentTradesByTpoId.Add(new CorrespondentTrade(info));
      }
      return correspondentTradesByTpoId;
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> associated with the TPO
    /// </summary>
    /// <param name="orgId">TPO Organization Id</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> with some basic trade information when passing in the TPO organization id
    /// </remarks>
    public List<CorrespondentTrade> GetCorrespondentTradesByOrgId(string orgId)
    {
      List<CorrespondentTrade> correspondentTradesByOrgId = new List<CorrespondentTrade>();
      ICursor cursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.OrganizationID", (Array) new string[1]
        {
          orgId
        }, true)
      }, (SortField[]) null, false);
      if (cursor != null && cursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
          correspondentTradesByOrgId.Add(new CorrespondentTrade(info));
      }
      return correspondentTradesByOrgId;
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> associated with the correspondent master
    /// </summary>
    /// <param name="correspondentMasterNumber">Correspondent Master Number</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> with some basic trade information when passing in the correspondent master number
    /// </remarks>
    public List<CorrespondentTrade> GetCorrespondentTradesByMasterNumber(
      string correspondentMasterNumber)
    {
      List<CorrespondentTrade> tradesByMasterNumber = new List<CorrespondentTrade>();
      ICursor cursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.ContractNumber", (Array) new string[1]
        {
          correspondentMasterNumber
        }, true)
      }, (SortField[]) null, false);
      if (cursor != null && cursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
          tradesByMasterNumber.Add(new CorrespondentTrade(info));
      }
      return tradesByMasterNumber;
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /> by trade status
    /// </summary>
    /// <param name="tradeStatus"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeStatus" />List</param>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" /></returns>
    /// <remarks>
    /// Example 1: Get all active loan trades
    ///                  TradeStatus[] statusList = new TradeStatus[4] { TradeStatus.Open, TradeStatus.Committed, TradeStatus.Shipped and TradeStatus.Purchased };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.CorrespondentTradeService.GetCorrespondentTrades(statusList);
    /// Example 2: Get all archived loan trades
    ///                  TradeStatus[] statusList = new TradeStatus[1] { TradeStatus.Archived };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ArchivedLoanTrades = session.CorrespondentTradeService.GetCorrespondentTrades(statusList);
    /// Example 3: Get all loan trades
    ///                  TradeStatus[] statusList = new TradeStatus[1] { TradeStatus.None };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var AllLoanTrades = session.CorrespondentTradeService.GetCorrespondentTrades(statusList);
    /// </remarks>
    public List<CorrespondentTrade> GetCorrespondentTrades(TradeStatus[] tradeStatus)
    {
      List<CorrespondentTrade> correspondentTrades = new List<CorrespondentTrade>();
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<TradeStatus>) tradeStatus).Any<TradeStatus>((Func<TradeStatus, bool>) (a => a == TradeStatus.None)))
        queryCriterion = (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.Status", (Array) Array.ConvertAll<TradeStatus, int>(tradeStatus, (Converter<TradeStatus, int>) (value => (int) value)), true);
      ICursor cursor = correspondentTradeManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, false);
      if (cursor != null && cursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
          correspondentTrades.Add(new CorrespondentTrade(info));
      }
      return correspondentTrades;
    }

    /// <summary>
    /// Get Assigned/Pending Loans for the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" />
    /// </summary>
    /// <param name="correspondentTradeId">Trade Id</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /> with some basic loan information
    /// and loan assignment information when passing in the identifier of correspondent trade
    /// </remarks>
    public List<TradeLoanAssignment> GetTradeLoanAssignments(int correspondentTradeId)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = correspondentTradeManager.GetTrade(correspondentTradeId);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in correspondentTradeManager.GetAssignedOrPendingLoans(correspondentTradeId, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    /// <summary>
    /// Get Assigned/Pending Loans for the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade" />
    /// </summary>
    /// <param name="correspondentTradeNumber">Commitment Number</param>
    /// <returns>List of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /></returns>
    /// <remarks>Return a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /> with some basic loan information
    /// and loan assignment information when passing in the identifier of correspondent trade
    /// </remarks>
    public List<TradeLoanAssignment> GetTradeLoanAssignments(string correspondentTradeNumber)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = correspondentTradeManager.GetTrade(correspondentTradeNumber);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in correspondentTradeManager.GetAssignedOrPendingLoans(trade.TradeID, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    /// <summary>
    /// Returns Dictionary of eligible Correspondent Master(s) for Correspondent Trade.
    /// </summary>
    /// <param name="correspondentTradeId">Correspondent Trade Number</param>
    /// <remarks>Returns Dictionary{tradeId, commitmentNumber} of eligible Correspondent Master(s) for Correspondent Trade.</remarks>
    public Dictionary<int, string> GetEligibleCorrespondentMastersByTradeId(int correspondentTradeId)
    {
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      return configurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTMASTER") == "False" && configurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTTRADE") == "False" ? new Dictionary<int, string>() : correspondentTradeManager.GetEligibleCorrespondentMastersByTradeId(correspondentTradeId);
    }

    /// <summary>
    /// Get Effective Correspondent Trades Based on loan information
    /// </summary>
    /// <param name="externalOrgId">External Organization ID</param>
    /// <param name="deliveryType">Delivery Type</param>
    /// <param name="loanAmount">Loan Amount</param>
    /// <returns>A list of Correspondent Trade information including Commitment Number and Correspondent Trade Identifier</returns>
    /// <remarks>Return a list of Correspondent Trades based on Delivery Type, Loan Amount and External Organization which the loan belongs to
    /// </remarks>
    public Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanInfo(
      string externalOrgId,
      string deliveryType,
      double loanAmount)
    {
      return ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetEligibleCorrespondentTradeByLoanInfo(externalOrgId, deliveryType, loanAmount);
    }

    /// <summary>
    /// Get Effective Correspondent Trades Based on loan number
    /// </summary>
    /// <param name="deliveryType">Delivery Type</param>
    /// <param name="loanNumber">Loan Number</param>
    /// <returns>A list of Correspondent Trade information including Commitment Number and Correspondent Trade Identifier</returns>
    /// <remarks>Return a list of Correspondent Trades based on Delivery Type, and Loan Number
    /// </remarks>
    public Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanNumber(
      string deliveryType,
      string loanNumber)
    {
      return ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetEligibleCorrespondentTradeByLoanNumber(deliveryType, loanNumber);
    }

    /// <summary>
    /// Get a list of possible eligible Correspondent Trades based on Loan Number, Delivery Type and Correspondent Master.
    /// </summary>
    /// <param name="deliveryType">Delivery Type</param>
    /// <param name="loanNumber">Loan Number</param>
    /// <param name="correspondentMasterNumber">Correspondent Master Number</param>
    /// <returns>Dictionary of Correspondent Trades</returns>
    /// <remarks>Return a list of Correspondent Trades based on Delivery Type, Loan Number and Correspondent Master Number
    /// </remarks>
    public Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanNumber(
      string deliveryType,
      string loanNumber,
      string correspondentMasterNumber)
    {
      return ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetEligibleCorrespondentTradeByLoanNumber(deliveryType, loanNumber, correspondentMasterNumber);
    }

    /// <summary>Allocate a loan to correspondent trade</summary>
    /// <param name="deliveryType">Delivery Type</param>
    /// <param name="loanNumber">Loan Number</param>
    /// <param name="correspondentTradeId">Correspondent Trade Identifier</param>
    /// <remarks>Validate the loan and the correspondent trade for loan allocation. If the loan and the trade are both eligible, process loan allocation by adding the loan to the correspondent trade and update the loan with the correspondent trade information.
    /// </remarks>
    public void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      int correspondentTradeId)
    {
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, correspondentTradeId, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
    }

    /// <summary>Allocate a loan to correspondent trade</summary>
    /// <param name="deliveryType">Delivery Type</param>
    /// <param name="loanNumber">Loan Number</param>
    /// <param name="correspondentTradeName">Correspondent Trade Commitment Number</param>
    /// <remarks>Validate the loan and the correspondent trade for loan allocation. If the loan and the trade are both eligible, process loan allocation by adding the loan to the correspondent trade and update the loan with the correspondent trade information.
    /// </remarks>
    public void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      string correspondentTradeName)
    {
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, correspondentTradeName, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
    }

    /// <summary>Allocate a loan to correspondent trade</summary>
    /// <param name="deliveryType">Delivery Type</param>
    /// <param name="loanNumbers">a list of Loan Numbers</param>
    /// <param name="correspondentTradeName">Correspondent Trade Commitment Number</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanUpdateResults" /></returns>
    /// <remarks>Validate a list of loans and the correspondent trade for loan allocation. If the loan and the trade are both eligible, process loan allocation by adding the loan to the correspondent trade and update the loan with the correspondent trade information.
    /// </remarks>
    public LoanUpdateResults AllocateLoansToCorrespondentTrade(
      string deliveryType,
      List<string> loanNumbers,
      string correspondentTradeName)
    {
      if (loanNumbers == null || loanNumbers.Count<string>() <= 0)
        return (LoanUpdateResults) null;
      loanNumbers.RemoveAll((Predicate<string>) (l => l == string.Empty));
      LoanUpdateResults correspondentTrade = new LoanUpdateResults(loanNumbers);
      foreach (string loanNumber in loanNumbers)
      {
        try
        {
          CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, correspondentTradeName, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
        }
        catch (TradeLoanUpdateException ex)
        {
          correspondentTrade.LoansWithError.Add(loanNumber, ex.Message);
        }
        catch (Exception ex)
        {
          correspondentTrade.LoansWithError.Add(loanNumber, ex.Message);
        }
        finally
        {
          correspondentTrade.ProcessedLoans.Add(loanNumber);
        }
      }
      return correspondentTrade;
    }

    /// <summary>Allocate a loan to correspondent trade</summary>
    /// <param name="deliveryType">Delivery Type</param>
    /// <param name="loans">a list of Loan Numbers with assigned Total Price</param>
    /// <param name="correspondentTradeName">Correspondent Trade Commitment Number</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanUpdateResults" /></returns>
    /// <remarks>Validate a list of loans and the correspondent trade for loan allocation. If the loan and the trade are both eligible, process loan allocation by adding the loan to the correspondent trade and update the loan with the correspondent trade information.
    /// </remarks>
    public LoanUpdateResults AllocateLoansWithTotalPriceToCorrespondentTrade(
      string deliveryType,
      Dictionary<string, Decimal> loans,
      string correspondentTradeName)
    {
      if (loans == null || loans.Count<KeyValuePair<string, Decimal>>() <= 0)
        return (LoanUpdateResults) null;
      foreach (string key in loans.Keys)
      {
        if (key == string.Empty)
          loans.Remove(key);
      }
      LoanUpdateResults correspondentTrade = new LoanUpdateResults(loans.Keys.ToList<string>());
      foreach (string key in loans.Keys)
      {
        try
        {
          CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, key, loans[key], correspondentTradeName, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
        }
        catch (TradeLoanUpdateException ex)
        {
          correspondentTrade.LoansWithError.Add(key, ex.Message);
        }
        catch (Exception ex)
        {
          correspondentTrade.LoansWithError.Add(key, ex.Message);
        }
        finally
        {
          correspondentTrade.ProcessedLoans.Add(key);
        }
      }
      return correspondentTrade;
    }

    /// <summary>Create a Correspondent Trade</summary>
    /// <param name="trade">Correspondent Trade object</param>
    /// <param name="priceAdjustmentTemplateGUID">Price adjustment template GUID</param>
    /// <param name="SRPTemplateGUID">SRP template GUID</param>
    /// <returns><see cref="T:System.Int32" /></returns>
    /// <remarks>Creates Correspondent Trade with price adjustment template and SRP template GUIDs. This will also do validations before creating it.
    /// </remarks>
    public int CreateCorrespondentTrade(
      CorrespondentTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID)
    {
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      Hashtable companySettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySettings("TRADE");
      CorrespondentTradeInfo tradeInfo = trade.GetTradeInfo();
      this.RequiredFieldValidations(trade, companySettings, false, tradeInfo);
      this.RunBusinessRules(tradeInfo, priceAdjustmentTemplateGUID, SRPTemplateGUID, false);
      if (!tradeInfo.IsToleranceLocked)
        this.CalculateTolerance(tradeInfo);
      int trade1 = correspondentTradeManager.CreateTrade(tradeInfo);
      trade.GetTradeInfo().TradeID = trade1;
      correspondentTradeManager.PublishKafkaEvent("create", trade1, (Hashtable) null);
      return trade1;
    }

    /// <summary>Update Correspondent Trade</summary>
    /// <param name="trade">Correspondent Trade object</param>
    /// <param name="priceAdjustmentTemplateGUID">Price adjustment template GUID</param>
    /// <param name="SRPTemplateGUID">SRP template GUID</param>
    /// <returns><see cref="T:System.Int32" />Returns Id of the Trade that was updated</returns>
    /// <remarks>Update Correspondent Trade with price adjustment template and SRP template GUIDs.
    /// If price adjustment template and SRP template GUIDs are not provided it will not be replaced with corresponding Template.
    /// This will also do validations before update trade.
    /// </remarks>
    public int UpdateCorrespondentTrade(
      CorrespondentTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID)
    {
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      Hashtable companySettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySettings("TRADE");
      CorrespondentTradeInfo tradeInfo = trade.GetTradeInfo();
      this.RequiredFieldValidations(trade, companySettings, true, tradeInfo);
      this.RunBusinessRules(tradeInfo, priceAdjustmentTemplateGUID, SRPTemplateGUID, true);
      if (!tradeInfo.IsToleranceLocked)
        this.CalculateTolerance(tradeInfo);
      correspondentTradeManager.UpdateTrade(tradeInfo, true);
      correspondentTradeManager.PublishKafkaEvent("update", tradeInfo.TradeID, (Hashtable) null);
      return tradeInfo.TradeID;
    }

    private void CalculateTolerance(CorrespondentTradeInfo trade)
    {
      ExternalOriginatorManagementData originatorManagementData = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrganizationsWithoutExtension(this.Session.UserID, (string) null).Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => !x.DisabledLogin && x.ExternalID == trade.TPOID)).FirstOrDefault<ExternalOriginatorManagementData>();
      if (trade.CommitmentType == EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.Mandatory)
      {
        Decimal val1 = Convert.ToDecimal((object) originatorManagementData.MandatoryTolerancePct, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num1 = Convert.ToDecimal((object) trade.TradeAmount, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num2 = Convert.ToDecimal((object) originatorManagementData.MandatoryToleranceAmt, (IFormatProvider) CultureInfo.InvariantCulture);
        if (originatorManagementData == null || !originatorManagementData.CommitmentMandatory)
          return;
        if (originatorManagementData.MandatoryTolerencePolicy == ExternalOriginatorCommitmentTolerancePolicy.FlatTolerance)
        {
          trade.Tolerance = val1;
        }
        else
        {
          if (originatorManagementData.MandatoryTolerencePolicy != ExternalOriginatorCommitmentTolerancePolicy.ConditionalTolerance || !(num1 > 0M))
            return;
          Decimal val2 = Math.Round(num2 / num1 * 100M, 9);
          trade.Tolerance = Math.Min(val1, val2);
        }
      }
      else
      {
        if (trade.CommitmentType != EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.BestEfforts)
          return;
        Decimal val1 = Convert.ToDecimal((object) originatorManagementData.BestEffortTolerancePct, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num3 = Convert.ToDecimal((object) trade.TradeAmount, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num4 = Convert.ToDecimal((object) originatorManagementData.BestEffortToleranceAmt, (IFormatProvider) CultureInfo.InvariantCulture);
        if (originatorManagementData == null || !originatorManagementData.CommitmentUseBestEffort)
          return;
        if (originatorManagementData.BestEffortTolerencePolicy == ExternalOriginatorCommitmentTolerancePolicy.FlatTolerance)
        {
          trade.Tolerance = val1;
        }
        else
        {
          if (originatorManagementData.BestEffortTolerencePolicy != ExternalOriginatorCommitmentTolerancePolicy.ConditionalTolerance || !(num3 > 0M))
            return;
          Decimal val2 = Math.Round(num4 / num3 * 100M, 9);
          trade.Tolerance = Math.Min(val1, val2);
        }
      }
    }

    /// <summary>Update Correspondent Trade Loan</summary>
    /// <param name="tradeId">Correspondent Trade Id</param>
    /// <param name="loanNumber">Assigned Loan Number</param>
    /// <returns><see cref="T:System.Boolean" />Returns true if succeeded, It will also throw Exception if updating fails.</returns>
    /// <remarks>Update one loan in Correspondent Trade which is already in Assinged status
    /// </remarks>
    public bool UpdateLoanInCorrespondentTrade(int tradeId, string loanNumber)
    {
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySettings("TRADE");
      CorrespondentTradeInfo trade = correspondentTradeManager.GetTrade(tradeId);
      if (trade == null)
        throw new Exception("Correspondent Trade with Id " + (object) tradeId + " does not exist.");
      PipelineInfo[] assignedOrPendingLoans = correspondentTradeManager.GetAssignedOrPendingLoans(tradeId, TradeLoanAssignment.GetFieldList().ToArray(), false);
      if (((IEnumerable<PipelineInfo>) assignedOrPendingLoans).Where<PipelineInfo>((Func<PipelineInfo, bool>) (x => x.LoanNumber == loanNumber)).Count<PipelineInfo>() == 0)
        throw new Exception("Loan Name with name " + loanNumber + " does not exist or is not assigned to trade with Trade Id -" + (object) tradeId);
      PipelineInfo[] array = ((IEnumerable<PipelineInfo>) assignedOrPendingLoans).Where<PipelineInfo>((Func<PipelineInfo, bool>) (x => x.LoanNumber == loanNumber)).ToArray<PipelineInfo>();
      CorrespondentTradeLoanAssignmentManager assignmentManager = new CorrespondentTradeLoanAssignmentManager(this.Session.SessionObjects, tradeId, array);
      UpdateTradeToLoanProcess tradeToLoanProcess = new UpdateTradeToLoanProcess();
      string message = "";
      foreach (CorrespondentTradeLoanAssignment assignedPendingLoan in assignmentManager.GetAllAssignedPendingLoans())
      {
        try
        {
          tradeToLoanProcess.CommitCorrespondentTradeAssignment(this.Session.SessionObjects, assignedPendingLoan, trade, true, new List<string>(), 0M);
        }
        catch (TradeLoanUpdateException ex)
        {
          if (ex.Message != null)
            message = message + "Error in updating loan " + assignedPendingLoan.PipelineInfo.LoanNumber + " due to - " + ex.Error.Message + "\n";
        }
        catch (Exception ex)
        {
          if (ex.Message != null)
            message = message + "Error in updating loan " + assignedPendingLoan.PipelineInfo.LoanNumber + " due to - " + ex.Message + "\n";
        }
      }
      if (!string.IsNullOrEmpty(message))
        throw new Exception(message);
      return true;
    }

    /// <summary>Validates required fields</summary>
    /// <param name="trade"></param>
    /// <param name="settings"></param>
    /// <param name="updateTrade"></param>
    /// <param name="tradeInfo"></param>
    private void RequiredFieldValidations(
      CorrespondentTrade trade,
      Hashtable settings,
      bool updateTrade,
      CorrespondentTradeInfo tradeInfo)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (settings[(object) "ENABLECORRESPONDENTTRADE"].ToString().ToLower() != "true")
        throw new Exception("Correspondent Trade is not enabled in Encompass");
      if (updateTrade)
      {
        if (string.IsNullOrEmpty(trade.CommitmentNumber))
          throw new Exception("Commitment Number can not be blank.");
      }
      else
      {
        if (settings[(object) "EnableAutoCommitmentNumber"].ToString().ToLower() == "true" && !string.IsNullOrEmpty(trade.CommitmentNumber))
          throw new Exception("When Auto numbering of Correspondent Trade is enabled Commitment Number cannot be set.");
        if (settings[(object) "EnableAutoCommitmentNumber"].ToString().ToLower() != "true" && string.IsNullOrEmpty(trade.CommitmentNumber))
          throw new Exception("Commitment Number can not be blank.");
      }
      if (trade.CommitmentType == CorrespondentTradeCommitmentType.None)
        throw new Exception("Commitment Type cannot be blank");
      if (trade.DeliveryType == CorrespondentMasterDeliveryType.None)
        throw new Exception("Delivery Type cannot be blank");
      if (trade.CommitmentDate == DateTime.MinValue)
        throw new Exception("Commitment Date field cannot be blank");
      if (trade.ExpirationDate == DateTime.MinValue)
        throw new Exception("Expiration Date field cannot be blank");
      if (trade.DeliveryExpirationDate == DateTime.MinValue)
        throw new Exception("Delivery Expiration Date field cannot be blank");
      if (trade.CommitmentDate > trade.ExpirationDate)
        throw new Exception("The Commitment Date must be before the Expiration Date");
      if (trade.TradeAmount == 0M)
        throw new Exception("Trade Amount field cannot be blank");
      if (tradeInfo != null && tradeInfo.CorrespondentTradePairOffs != null)
      {
        foreach (CorrespondentTradePairOff correspondentTradePairOff in tradeInfo.CorrespondentTradePairOffs)
        {
          if (trade.TradeAmount < correspondentTradePairOff.TradeAmount)
            throw new Exception("The trade amount of the pair-off cannot be greater than the open amount");
        }
      }
      if (!trade.MinNoteRateRange.HasValue || !trade.MaxNoteRateRange.HasValue)
        return;
      Decimal? minNoteRateRange = trade.MinNoteRateRange;
      Decimal? maxNoteRateRange = trade.MaxNoteRateRange;
      if (minNoteRateRange.GetValueOrDefault() > maxNoteRateRange.GetValueOrDefault() & minNoteRateRange.HasValue & maxNoteRateRange.HasValue)
        throw new Exception("Minimum term must be less than or equal to the Maximum");
    }

    private void RunBusinessRules(
      CorrespondentTradeInfo tradeInfo,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID,
      bool updateTrade)
    {
      ICorrespondentMasterManager correspondentMasterManager = (ICorrespondentMasterManager) this.Session.GetObject("CorrespondentMasterManager");
      ICorrespondentTradeManager ctmgr = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      Decimal num1 = 0M;
      Hashtable companySettings = configurationManager.GetCompanySettings("TRADE");
      ExternalOriginatorManagementData tpoSettings = configurationManager.GetExternalOrganizationsWithoutExtension(this.Session.UserID, (string) null).Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => !x.DisabledLogin && x.ExternalID == tradeInfo.TPOID)).FirstOrDefault<ExternalOriginatorManagementData>();
      tradeInfo.TPOID = tpoSettings != null ? tpoSettings.ExternalID : throw new Exception("No TPO found for TPO ID - " + tradeInfo.TPOID);
      tradeInfo.OrganizationID = tpoSettings.OrgID;
      tradeInfo.CompanyName = tpoSettings.OrganizationName;
      tradeInfo.ExternalOriginatorManagementID = tpoSettings.oid;
      if (tpoSettings.CommitmentMandatory && !tpoSettings.CommitmentUseBestEffort && tradeInfo.CommitmentType != EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.Mandatory)
        throw new Exception("Only Mandatory commitment type can be selected based on TPO Commitment settings for TPO ID - " + tpoSettings.ExternalID);
      if (!tpoSettings.CommitmentMandatory && tpoSettings.CommitmentUseBestEffort && tradeInfo.CommitmentType != EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.BestEfforts)
        throw new Exception("Only Best Efforts commitment type can be selected based on TPO Commitment settings for TPO ID - " + tpoSettings.ExternalID);
      if (tradeInfo.CommitmentType == EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.BestEfforts && tpoSettings.CommitmentUseBestEffort && tradeInfo.DeliveryType != EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.IndividualBestEfforts)
        throw new Exception("Only Best Efforts delivery type is available to set based on TPO Settings.");
      if (tradeInfo.CommitmentType == EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.Mandatory)
      {
        string message = "";
        switch (tradeInfo.DeliveryType)
        {
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.AOT:
            if (!tpoSettings.IsCommitmentDeliveryAOT)
            {
              message = "AOT Delivery type is not selectable based on TPO Settings.";
              break;
            }
            break;
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.Forwards:
            if (!tpoSettings.IsCommitmentDeliveryForward)
            {
              message = "Forwards Delivery type is not selectable based on TPO Settings.";
              break;
            }
            break;
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.IndividualBestEfforts:
            message = "Individual Best Effort Delivery type is not valid Delivery Type for Mandatory commitment type";
            break;
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.IndividualMandatory:
            if (!tpoSettings.IsCommitmentDeliveryIndividual)
            {
              message = "Individual Mandatory Delivery type is not selectable based on TPO Settings.";
              break;
            }
            break;
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.LiveTrade:
            if (!tpoSettings.IsCommitmentDeliveryLiveTrade)
            {
              message = "Direct Trade Delivery type is not selectable based on TPO Settings.";
              break;
            }
            break;
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.Bulk:
            if (!tpoSettings.IsCommitmentDeliveryBulk)
            {
              message = "Bulk Delivery type is not selectable based on TPO Settings.";
              break;
            }
            break;
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.BulkAOT:
            if (!tpoSettings.IsCommitmentDeliveryBulkAOT)
            {
              message = "Bulk AOT Delivery type is not selectable based on TPO Settings.";
              break;
            }
            break;
          case EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.CoIssue:
            if (!tpoSettings.IsCommitmentDeliveryCoIssue)
            {
              message = "Co-Issue Delivery type is not selectable based on TPO Settings.";
              break;
            }
            break;
        }
        if (!string.IsNullOrEmpty(message))
          throw new Exception(message);
      }
      foreach (TradePricingItem simplePricingItem in tradeInfo.Pricing.SimplePricingItems)
      {
        if (simplePricingItem.Rate > 99999.99M)
          throw new Exception("Rate cannot exceed maximum limit of 99999.99.");
        if (simplePricingItem.Price >= 1000M || simplePricingItem.Price < 0M)
          throw new Exception("Price must be greater than or equal to 0 and less than 1000.");
      }
      if (!string.IsNullOrEmpty(tradeInfo.CorrespondentMasterCommitmentNumber) && configurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTMASTER") == "True")
      {
        CorrespondentMasterInfo[] correspondentMasterInfos = ctmgr.GetCorrespondentMasterInfos(tradeInfo);
        if (((IEnumerable<CorrespondentMasterInfo>) correspondentMasterInfos).Select<CorrespondentMasterInfo, bool>((Func<CorrespondentMasterInfo, bool>) (x => x.Name == tradeInfo.CorrespondentMasterCommitmentNumber)).Count<bool>() == 0)
          throw new Exception("The specified master commitment cannot be found. It may have been deleted by another user.");
        CorrespondentMasterInfo masterInfo = ((IEnumerable<CorrespondentMasterInfo>) correspondentMasterInfos).Where<CorrespondentMasterInfo>((Func<CorrespondentMasterInfo, bool>) (x => x.Name == tradeInfo.CorrespondentMasterCommitmentNumber)).First<CorrespondentMasterInfo>();
        if (masterInfo.DeliveryInfos.Where<MasterCommitmentDeliveryInfo>((Func<MasterCommitmentDeliveryInfo, bool>) (x => x.Type == tradeInfo.DeliveryType)).Count<MasterCommitmentDeliveryInfo>() == 0)
          throw new Exception("For selected Correspondent Master " + masterInfo.Name + ", Delivery Type " + tradeInfo.DeliveryType.ToDescription() + " is not configured.");
        if (tradeInfo.TradeAmount > this.GetAvailableAmount(masterInfo, 0M, tpoSettings, ctmgr))
          throw new Exception("The Correspondent Trade Amount should be equal to or less than the Available amount of the Master Commitment.");
        tradeInfo.CorrespondentMasterID = correspondentMasterInfos[0].ID;
      }
      if (updateTrade)
      {
        CorrespondentTradeInfo trade = ctmgr.GetTrade(tradeInfo.TradeID);
        num1 = trade.TradeAmount;
        if (trade == null)
          throw new Exception("Correspondent Trade with Id " + (object) tradeInfo.TradeID + " does not exist.");
        if (trade.Status == EllieMae.EMLite.Trading.TradeStatus.Archived || trade.Status == EllieMae.EMLite.Trading.TradeStatus.Pending)
          throw new Exception("Corespondent Trade can not be updated when its in Archived or Pending status");
        if (tradeInfo.TradeID == trade.TradeID && tradeInfo.Name != trade.Name && ctmgr.CheckExistingTradeByName(tradeInfo.Name))
          throw new Exception("A correspondent trade with the commitment # '" + tradeInfo.Name + "' already exists. You must enter a unique commitment # for this correspondent trade.");
        if (tradeInfo.TradeID == trade.TradeID && tradeInfo.Name != trade.Name && ctmgr.CheckTradeByName(tradeInfo.Name))
          throw new Exception("A correspondent trade with the commitment # '" + tradeInfo.Name + "' was previously deleted. You must enter a unique commitment # for this correspondent trade.");
        if (trade.Name != tradeInfo.Name)
          tradeInfo.OverrideTradeName = true;
        if (string.IsNullOrEmpty(tradeInfo.Name))
          throw new Exception("Commitment Number can not be blank.");
        if (trade.CommitmentType != tradeInfo.CommitmentType)
          throw new Exception("Commitment Type can not be changed for existing trade.");
        if (trade.TPOID != tradeInfo.TPOID)
          throw new Exception("TPO Id can not be changed for existing trade.");
      }
      else if (companySettings[(object) "ENABLEAUTOCOMMITMENTNUMBER"].ToString().ToLower() == "true")
      {
        string nextAutoNumber = ctmgr.GenerateNextAutoNumber();
        if (nextAutoNumber.Length > 18 || string.IsNullOrEmpty(nextAutoNumber))
          throw new Exception("Correspondent Trade cannot be saved because the maximum number of commitment numbers has been reached. Please go to settings and adjust the starting number");
        tradeInfo.Name = "use_autonumber_reserved";
      }
      else
      {
        if (ctmgr.CheckTradeByName(tradeInfo.Name))
          throw new Exception("A correspondent trade with the commitment # '" + tradeInfo.Name + "' was previously deleted. You must enter a unique commitment # for this correspondent trade.");
        if (ctmgr.CheckExistingTradeByName(tradeInfo.Name))
          throw new Exception("A correspondent trade with the commitment # '" + tradeInfo.Name + "' already exists. You must enter a unique commitment # for this correspondent trade.");
      }
      tradeInfo = this.PopulateEPPSProgramByID(tradeInfo);
      this.ValidateFromPPE(tradeInfo, companySettings);
      if ((tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.IndividualMandatory || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.IndividualBestEfforts) && (!string.IsNullOrEmpty(priceAdjustmentTemplateGUID) || !string.IsNullOrEmpty(SRPTemplateGUID) || tradeInfo.Pricing.SimplePricingItems.Count > 0))
        throw new Exception("When Delivery type is Individual Delivery Type pricing value cannot be set");
      if (!string.IsNullOrEmpty(priceAdjustmentTemplateGUID))
      {
        PriceAdjustmentTemplate templateSettings = (PriceAdjustmentTemplate) configurationManager.GetTemplateSettings(TemplateSettingsType.TradePriceAdjustment, ((IEnumerable<FileSystemEntry>) configurationManager.GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.TradePriceAdjustment, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == priceAdjustmentTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find Price Adjustment Template for GUID - " + priceAdjustmentTemplateGUID));
        tradeInfo.PriceAdjustments = templateSettings.PriceAdjustments;
      }
      if (!string.IsNullOrEmpty(SRPTemplateGUID))
      {
        SRPTableTemplate srpTableTemplate = (SRPTableTemplate) (configurationManager.GetTemplateSettings(TemplateSettingsType.SRPTable, ((IEnumerable<FileSystemEntry>) configurationManager.GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.SRPTable, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == SRPTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find SRP Template for GUID - " + priceAdjustmentTemplateGUID)) ?? throw new Exception("Could not find SRP Template for GUID - " + SRPTemplateGUID));
        tradeInfo.SRPTable = srpTableTemplate.SRPTable;
      }
      if (updateTrade && tradeInfo.MaxAmount < this.GetAssignedLoanAmount(tradeInfo.TradeID, ctmgr))
        throw new Exception("The assigned amount cannot be more than the maximum amount for the correspondent trade.");
      EllieMae.EMLite.Trading.MasterCommitmentType commitmentType = tradeInfo.CommitmentType == EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.BestEfforts ? EllieMae.EMLite.Trading.MasterCommitmentType.BestEfforts : EllieMae.EMLite.Trading.MasterCommitmentType.Mandatory;
      if ((!tpoSettings.CommitmentUseBestEffort || tradeInfo.CommitmentType != EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.BestEfforts) && tpoSettings.CommitmentTradePolicy == ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation && tradeInfo.TradeAmount - num1 > ctmgr.CalculateTPOAvailableAmount(commitmentType, tpoSettings))
        throw new Exception("Trade Amount exceeds available Commitment Authority. Trade cannot be saved.");
      CorrespondentTradeInfo correspondentTradeInfo1 = tradeInfo;
      if ((correspondentTradeInfo1 != null ? (correspondentTradeInfo1.IsToleranceLocked ? 1 : 0) : 0) != 0)
      {
        CorrespondentTradeInfo correspondentTradeInfo2 = tradeInfo;
        int num2;
        if (correspondentTradeInfo2 == null)
        {
          num2 = 1;
        }
        else
        {
          Decimal tolerance = correspondentTradeInfo2.Tolerance;
          num2 = 0;
        }
        if (num2 != 0)
          throw new Exception("Tolerance field cannot be blank.");
        CorrespondentTradeInfo correspondentTradeInfo3 = tradeInfo;
        if ((correspondentTradeInfo3 != null ? (correspondentTradeInfo3.Tolerance == 0M ? 1 : 0) : 0) != 0)
          throw new Exception("Tolerance field cannot be zero.");
        Decimal? tolerance1 = tradeInfo?.Tolerance;
        Decimal num3 = (Decimal) 100;
        if (!(tolerance1.GetValueOrDefault() > num3 & tolerance1.HasValue))
        {
          CorrespondentTradeInfo correspondentTradeInfo4 = tradeInfo;
          if ((correspondentTradeInfo4 != null ? (correspondentTradeInfo4.Tolerance < 0M ? 1 : 0) : 0) == 0)
            return;
        }
        throw new Exception("The tolerance cannot be negative and cannot exceed 100 percent.");
      }
      CorrespondentTradeInfo correspondentTradeInfo5 = tradeInfo;
      if ((correspondentTradeInfo5 != null ? (correspondentTradeInfo5.Tolerance != 0M ? 1 : 0) : 1) != 0)
        throw new Exception("Tolerance violates commitment settings.");
      if (tradeInfo.CommitmentType == EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.Mandatory && tpoSettings.MandatoryTolerancePct == 0M || tradeInfo.CommitmentType == EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType.BestEfforts && tpoSettings.BestEffortTolerancePct == 0M)
        throw new Exception("Tolerance is not configured under commitment settings.");
    }

    private void ValidateFromPPE(CorrespondentTradeInfo tradeInfo, Hashtable settings)
    {
      bool flag1 = settings[(object) "EPPSLOANPROGELIPRICING"].ToString().ToLower() == "true";
      bool flag2 = false;
      bool flag3 = tradeInfo.EPPSLoanProgramsFilter.Count > 0;
      if (this.Session.SessionObjects.StartupInfo.ProductPricingPartner != null && this.Session.SessionObjects.StartupInfo.ProductPricingPartner.IsEPPS)
        flag2 = flag3;
      if (flag1 & flag2 && (tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.AOT || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.LiveTrade || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.Forwards || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.BulkAOT || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.CoIssue))
        return;
      if (tradeInfo.AdjustmentsfromPPE)
        throw new Exception("Adjustments from PPE is only allowed for AOT, Direct Trade and Forwards with Pricing Engine ICE PPE and ICE PPE Loan Program Eligibility/Pricing turned ON.");
      if (tradeInfo.SRPfromPPE)
        throw new Exception("SRP from PPE is only allowed for AOT, Direct Trade and Forwards with Pricing Engine ICE PPE and ICE PPE Loan Program Eligibility/Pricing turned ON.");
    }

    private Decimal GetAvailableAmount(
      CorrespondentMasterInfo masterInfo,
      Decimal commitmentAmount,
      ExternalOriginatorManagementData tpoSettings,
      ICorrespondentTradeManager ctmgr)
    {
      return tpoSettings == null ? commitmentAmount : masterInfo.CommitmentAmount + CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(commitmentAmount, tpoSettings.CommitmentUseBestEffortLimited, ctmgr.GetTradeInfosByMasterId(masterInfo.ID));
    }

    /// <summary>Service to remove loan(s) from correspondent trade</summary>
    /// <param name="correspondentTradeName"></param>
    /// <param name="loanNumbers"></param>
    /// <returns></returns>
    public LoanUpdateResults RemoveLoansFromCorrespondentTrade(
      string correspondentTradeName,
      List<string> loanNumbers)
    {
      LoanUpdateResults loanUpdateResults1 = new LoanUpdateResults(loanNumbers);
      Dictionary<string, string> dictionary = CorrespondentTradeManager.RemoveLoansFromCorrespondentTrade(correspondentTradeName.Trim(), loanNumbers.Select<string, string>((Func<string, string>) (s => s.Trim())).ToList<string>(), this.Session.SessionObjects);
      string str = (string) null;
      if (dictionary.Count == 1 && dictionary.TryGetValue("Trade Error", out str))
      {
        loanUpdateResults1.TradeError = dictionary["Trade Error"];
        return loanUpdateResults1;
      }
      LoanUpdateResults loanUpdateResults2 = new LoanUpdateResults(loanNumbers);
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        loanUpdateResults2.ProcessedLoans.Add(keyValuePair.Key);
        if (!keyValuePair.Value.Equals("Processed", StringComparison.OrdinalIgnoreCase))
          loanUpdateResults2.LoansWithError.Add(keyValuePair.Key, keyValuePair.Value);
      }
      return loanUpdateResults2;
    }

    private Decimal GetAssignedLoanAmount(int tradeId, ICorrespondentTradeManager ctmgr)
    {
      CorrespondentTradeEditorScreenData editorScreenData = ctmgr.GetTradeEditorScreenData(tradeId, new List<string>()
      {
        "Loan.TotalLoanAmount"
      }.ToArray(), false);
      CorrespondentTradeLoanAssignmentManager assignmentManager = new CorrespondentTradeLoanAssignmentManager(this.Session.SessionObjects, tradeId, editorScreenData.AssignedLoans);
      List<LoanToTradeAssignmentBase> tradeAssignmentBaseList = new List<LoanToTradeAssignmentBase>();
      Decimal assignedLoanAmount = 0M;
      if (assignmentManager != null)
      {
        foreach (LoanToTradeAssignmentBase assignedPendingLoan in assignmentManager.GetAllAssignedPendingLoans())
        {
          Decimal num = Utils.ParseDecimal(assignedPendingLoan.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"]);
          assignedLoanAmount += num;
        }
      }
      return assignedLoanAmount;
    }

    /// <summary>Service to get Correspondent Trade EPPS Loan Programs</summary>
    /// <param name="correspondentTradeName"></param>
    /// <returns></returns>
    public List<EppsLoanProgram> GetCorrespondentTradeEppsLoanPrograms(string correspondentTradeName)
    {
      List<EppsLoanProgram> eppsLoanPrograms = new List<EppsLoanProgram>();
      ICorrespondentTradeManager mngr = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo correspondentTradeInfo = this.ValidateEppsLoanProgram(mngr, mngr.GetTrade(correspondentTradeName) ?? throw new Exception("Correspondent Trade does not exist."));
      if (correspondentTradeInfo != null)
      {
        foreach (EPPSLoanProgramFilter loanProgramFilter in (IEnumerable<EPPSLoanProgramFilter>) correspondentTradeInfo.EPPSLoanProgramsFilter.OrderBy<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (r => r.ProgramName)))
          eppsLoanPrograms.Add(new EppsLoanProgram(loanProgramFilter.ProgramId, loanProgramFilter.ProgramName));
      }
      return eppsLoanPrograms;
    }

    /// <summary>
    /// Service to add EPPS Loan Program to Corresonpdent Trade
    /// </summary>
    /// <param name="correspondentTradeName"></param>
    /// <param name="programIds"></param>
    /// <returns></returns>
    public bool AddCorrespondentTradeEppsLoanPrograms(
      string correspondentTradeName,
      List<string> programIds)
    {
      ICorrespondentTradeManager mngr = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = mngr.GetTrade(correspondentTradeName);
      if (trade == null)
        throw new Exception("Correspondent Trade does not exist.");
      ProductPricingSetting productPricingPartner = this.Session.SessionObjects.StartupInfo.ProductPricingPartner;
      if (productPricingPartner == null || !productPricingPartner.IsEPPS)
        throw new Exception("Access to the ICE PPE Loan Programs table is only allowed if ICE PPE is the selected Product and Pricing supplier.");
      CorrespondentTradeInfo correspondentTrade = this.ValidateEppsLoanProgram(mngr, trade);
      if (correspondentTrade == null)
        return false;
      List<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram> eppsLoanProgram1 = new SettingsService(this.Session).GetEppsLoanProgram();
      List<string> list1 = correspondentTrade.EPPSLoanProgramsFilter.Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (e => e.ProgramId)).ToList<string>();
      foreach (string programId1 in programIds)
      {
        string programId = programId1;
        EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram eppsLoanProgram2 = eppsLoanProgram1.Where<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram>((Func<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram, bool>) (e => e.ProgramId == programId)).FirstOrDefault<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram>();
        if (eppsLoanProgram2 != null && !list1.Contains(programId))
          correspondentTrade.EPPSLoanProgramsFilter.Add(new EPPSLoanProgramFilter(programId, eppsLoanProgram2.ProgramName));
      }
      mngr.UpdateTrade(correspondentTrade, true);
      List<string> list2 = eppsLoanProgram1.Select<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram, string>((Func<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram, string>) (s => s.ProgramId)).ToList<string>();
      List<string> list3 = programIds.Except<string>((IEnumerable<string>) list2).ToList<string>();
      if (eppsLoanProgram1 == null || eppsLoanProgram1.Count == 0 || list3.Count > 0)
        throw new Exception(string.Format("ICE PPE Loan Program ID {0} did not match table. Entry not added.", (object) string.Join(",", list3.ToArray())));
      return true;
    }

    /// <summary>
    /// Service to add EPPS Loan Program from Corresonpdent Trade
    /// </summary>
    /// <param name="correspondentTradeName"></param>
    /// <param name="programIds"></param>
    /// <returns></returns>
    public bool DeleteCorrespondentTradeEppsLoanPrograms(
      string correspondentTradeName,
      List<string> programIds)
    {
      ICorrespondentTradeManager mngr = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo correspondentTrade = this.ValidateEppsLoanProgram(mngr, mngr.GetTrade(correspondentTradeName) ?? throw new Exception("Correspondent Trade does not exist."));
      if (correspondentTrade == null)
        return false;
      List<string> list1 = correspondentTrade.EPPSLoanProgramsFilter.Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (e => e.ProgramId)).ToList<string>();
      List<string> list2 = programIds.Except<string>((IEnumerable<string>) list1).ToList<string>();
      EPPSLoanProgramFilters loanProgramFilters = new EPPSLoanProgramFilters();
      foreach (EPPSLoanProgramFilter loanProgramFilter in correspondentTrade.EPPSLoanProgramsFilter)
      {
        if (!programIds.Contains(loanProgramFilter.ProgramId))
          loanProgramFilters.Add(new EPPSLoanProgramFilter(loanProgramFilter.ProgramId, loanProgramFilter.ProgramName));
      }
      correspondentTrade.EPPSLoanProgramsFilter = loanProgramFilters;
      mngr.UpdateTrade(correspondentTrade, true);
      if (list2.Count > 0)
        throw new Exception(string.Format("ICE PPE Loan Program ID {0} did not match table. Entry not deleted.", (object) string.Join(",", list2.ToArray())));
      return true;
    }

    private CorrespondentTradeInfo ValidateEppsLoanProgram(
      ICorrespondentTradeManager mngr,
      CorrespondentTradeInfo tradeInfo)
    {
      if (!this.Session.GetCurrentUser().HasAccessTo(Feature.TabTrades))
        throw new Exception("User does not have access right to Correspondent Trades.");
      string a = this.Session.SessionObjects.ConfigurationManager.GetCompanySettings("TRADE")[(object) "EnableCorrespondentTrade"].ToString();
      if (!(a != string.Empty) || !string.Equals(a, "true", StringComparison.CurrentCultureIgnoreCase))
        throw new Exception("Correspondent Trade is not available.");
      return tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.AOT || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.Forwards || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.LiveTrade || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.BulkAOT || tradeInfo.DeliveryType == EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType.CoIssue ? tradeInfo : throw new Exception(string.Format("Correspondent trade Delivery Type of {0} does not support the use of the ICE PPE Loan Programs table.", (object) tradeInfo.DeliveryType.ToDescription()));
    }

    /// <summary>
    /// Populates the EPPS Program details in the Correspondent Trade Info object
    /// </summary>
    /// <param name="tradeInfo"></param>
    /// <returns></returns>
    public CorrespondentTradeInfo PopulateEPPSProgramByID(CorrespondentTradeInfo tradeInfo)
    {
      SettingsService settingsService = new SettingsService(this.Session);
      List<EPPSLoanProgram> programsSettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetEPPSLoanProgramsSettings();
      tradeInfo.EPPSLoanProgramsFilter.Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (e => e.ProgramId)).ToList<string>();
      string[] array = tradeInfo.EPPSLoanProgramsFilter.Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (x => x.ProgramId)).ToArray<string>();
      tradeInfo.EPPSLoanProgramsFilter.Clear();
      foreach (string str in array)
      {
        string id = str;
        EPPSLoanProgram eppsLoanProgram = programsSettings.Where<EPPSLoanProgram>((Func<EPPSLoanProgram, bool>) (e => e.ProgramID == id)).FirstOrDefault<EPPSLoanProgram>();
        if (eppsLoanProgram != null && !tradeInfo.EPPSLoanProgramsFilter.Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (x => x.ProgramId.ToLower())).ToList<string>().Contains(id))
          tradeInfo.EPPSLoanProgramsFilter.Add(new EPPSLoanProgramFilter(eppsLoanProgram.ProgramID, eppsLoanProgram.ProgramName));
      }
      return tradeInfo;
    }
  }
}
