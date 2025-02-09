// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTradeService
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
  [Guid("2D55838C-BB75-4351-BC75-A3A53293515E")]
  public class CorrespondentTradeService : SessionBoundObject, ICorrespondentTradeService
  {
    internal CorrespondentTradeService(Session session)
      : base(session)
    {
    }

    public CorrespondentTrade GetCorrespondentTrade(int correspondentTradeId)
    {
      CorrespondentTradeInfo trade = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetTrade(correspondentTradeId);
      return trade != null ? new CorrespondentTrade(trade) : (CorrespondentTrade) null;
    }

    public CorrespondentTrade GetCorrespondentTrade(string correspondentTradeNumber)
    {
      CorrespondentTradeInfo trade = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetTrade(correspondentTradeNumber);
      return trade != null ? new CorrespondentTrade(trade) : (CorrespondentTrade) null;
    }

    public List<CorrespondentTrade> GetCorrespondentTradesByTpoId(int Id)
    {
      List<CorrespondentTrade> correspondentTradesByTpoId = new List<CorrespondentTrade>();
      ICursor icursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.ExternalOriginatorManagementID", (Array) new int[1]
        {
          Id
        }, true)
      }, (SortField[]) null, false);
      if (icursor != null && icursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
          correspondentTradesByTpoId.Add(new CorrespondentTrade(info));
      }
      return correspondentTradesByTpoId;
    }

    public List<CorrespondentTrade> GetCorrespondentTradesByTpoId(string tpoId)
    {
      List<CorrespondentTrade> correspondentTradesByTpoId = new List<CorrespondentTrade>();
      ICursor icursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.ExternalID", (Array) new string[1]
        {
          tpoId
        }, true)
      }, (SortField[]) null, false);
      if (icursor != null && icursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
          correspondentTradesByTpoId.Add(new CorrespondentTrade(info));
      }
      return correspondentTradesByTpoId;
    }

    public List<CorrespondentTrade> GetCorrespondentTradesByOrgId(string orgId)
    {
      List<CorrespondentTrade> correspondentTradesByOrgId = new List<CorrespondentTrade>();
      ICursor icursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.OrganizationID", (Array) new string[1]
        {
          orgId
        }, true)
      }, (SortField[]) null, false);
      if (icursor != null && icursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
          correspondentTradesByOrgId.Add(new CorrespondentTrade(info));
      }
      return correspondentTradesByOrgId;
    }

    public List<CorrespondentTrade> GetCorrespondentTradesByMasterNumber(
      string correspondentMasterNumber)
    {
      List<CorrespondentTrade> tradesByMasterNumber = new List<CorrespondentTrade>();
      ICursor icursor = ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.ContractNumber", (Array) new string[1]
        {
          correspondentMasterNumber
        }, true)
      }, (SortField[]) null, false);
      if (icursor != null && icursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
          tradesByMasterNumber.Add(new CorrespondentTrade(info));
      }
      return tradesByMasterNumber;
    }

    public List<CorrespondentTrade> GetCorrespondentTrades(TradeStatus[] tradeStatus)
    {
      List<CorrespondentTrade> correspondentTrades = new List<CorrespondentTrade>();
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<TradeStatus>) tradeStatus).Any<TradeStatus>((Func<TradeStatus, bool>) (a => a == TradeStatus.None)))
        queryCriterion = (QueryCriterion) new ListValueCriterion("CorrespondentTradeDetails.Status", (Array) Array.ConvertAll<TradeStatus, int>(tradeStatus, (Converter<TradeStatus, int>) (value => (int) value)), true);
      ICursor icursor = icorrespondentTradeManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, false);
      if (icursor != null && icursor.GetItemCount() > 0)
      {
        foreach (CorrespondentTradeViewModel info in (CorrespondentTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
          correspondentTrades.Add(new CorrespondentTrade(info));
      }
      return correspondentTrades;
    }

    public List<TradeLoanAssignment> GetTradeLoanAssignments(int correspondentTradeId)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = icorrespondentTradeManager.GetTrade(correspondentTradeId);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in icorrespondentTradeManager.GetAssignedOrPendingLoans(correspondentTradeId, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    public List<TradeLoanAssignment> GetTradeLoanAssignments(string correspondentTradeNumber)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo trade = icorrespondentTradeManager.GetTrade(correspondentTradeNumber);
      if (trade == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in icorrespondentTradeManager.GetAssignedOrPendingLoans(((TradeBase) trade).TradeID, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(trade, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    public Dictionary<int, string> GetEligibleCorrespondentMastersByTradeId(int correspondentTradeId)
    {
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      return iconfigurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTMASTER") == "False" && iconfigurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTTRADE") == "False" ? new Dictionary<int, string>() : icorrespondentTradeManager.GetEligibleCorrespondentMastersByTradeId(correspondentTradeId);
    }

    public Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanInfo(
      string externalOrgId,
      string deliveryType,
      double loanAmount)
    {
      return ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetEligibleCorrespondentTradeByLoanInfo(externalOrgId, deliveryType, loanAmount);
    }

    public Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanNumber(
      string deliveryType,
      string loanNumber)
    {
      return ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetEligibleCorrespondentTradeByLoanNumber(deliveryType, loanNumber);
    }

    public Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanNumber(
      string deliveryType,
      string loanNumber,
      string correspondentMasterNumber)
    {
      return ((ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager")).GetEligibleCorrespondentTradeByLoanNumber(deliveryType, loanNumber, correspondentMasterNumber);
    }

    public void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      int correspondentTradeId)
    {
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, correspondentTradeId, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
    }

    public void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      string correspondentTradeName)
    {
      CorrespondentTradeManager.AllocateLoanToCorrespondentTrade(deliveryType, loanNumber, correspondentTradeName, this.Session.SessionObjects, TradeLoanAssignment.GetFieldList().ToArray());
    }

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
          correspondentTrade.LoansWithError.Add(loanNumber, ((Exception) ex).Message);
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
          correspondentTrade.LoansWithError.Add(key, ((Exception) ex).Message);
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

    public int CreateCorrespondentTrade(
      CorrespondentTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID)
    {
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      Hashtable companySettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySettings("TRADE");
      CorrespondentTradeInfo tradeInfo = trade.GetTradeInfo();
      this.RequiredFieldValidations(trade, companySettings, false, tradeInfo);
      this.RunBusinessRules(tradeInfo, priceAdjustmentTemplateGUID, SRPTemplateGUID, false);
      if (!tradeInfo.IsToleranceLocked)
        this.CalculateTolerance(tradeInfo);
      int trade1 = icorrespondentTradeManager.CreateTrade(tradeInfo);
      ((TradeBase) trade.GetTradeInfo()).TradeID = trade1;
      icorrespondentTradeManager.PublishKafkaEvent("create", trade1, (Hashtable) null);
      return trade1;
    }

    public int UpdateCorrespondentTrade(
      CorrespondentTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID)
    {
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      Hashtable companySettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySettings("TRADE");
      CorrespondentTradeInfo tradeInfo = trade.GetTradeInfo();
      this.RequiredFieldValidations(trade, companySettings, true, tradeInfo);
      this.RunBusinessRules(tradeInfo, priceAdjustmentTemplateGUID, SRPTemplateGUID, true);
      if (!tradeInfo.IsToleranceLocked)
        this.CalculateTolerance(tradeInfo);
      icorrespondentTradeManager.UpdateTrade(tradeInfo, true);
      icorrespondentTradeManager.PublishKafkaEvent("update", ((TradeBase) tradeInfo).TradeID, (Hashtable) null);
      return ((TradeBase) tradeInfo).TradeID;
    }

    private void CalculateTolerance(CorrespondentTradeInfo trade)
    {
      ExternalOriginatorManagementData originatorManagementData = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrganizationsWithoutExtension(this.Session.UserID, (string) null).Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => !x.DisabledLogin && x.ExternalID == trade.TPOID)).FirstOrDefault<ExternalOriginatorManagementData>();
      if (trade.CommitmentType == 2)
      {
        Decimal val1 = Convert.ToDecimal((object) originatorManagementData.MandatoryTolerancePct, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num1 = Convert.ToDecimal((object) ((TradeInfoObj) trade).TradeAmount, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num2 = Convert.ToDecimal((object) originatorManagementData.MandatoryToleranceAmt, (IFormatProvider) CultureInfo.InvariantCulture);
        if (originatorManagementData == null || !originatorManagementData.CommitmentMandatory)
          return;
        if (originatorManagementData.MandatoryTolerencePolicy == 1)
        {
          ((TradeInfoObj) trade).Tolerance = val1;
        }
        else
        {
          if (originatorManagementData.MandatoryTolerencePolicy != 2 || !(num1 > 0M))
            return;
          Decimal val2 = Math.Round(num2 / num1 * 100M, 9);
          ((TradeInfoObj) trade).Tolerance = Math.Min(val1, val2);
        }
      }
      else
      {
        if (trade.CommitmentType != 1)
          return;
        Decimal val1 = Convert.ToDecimal((object) originatorManagementData.BestEffortTolerancePct, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num3 = Convert.ToDecimal((object) ((TradeInfoObj) trade).TradeAmount, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num4 = Convert.ToDecimal((object) originatorManagementData.BestEffortToleranceAmt, (IFormatProvider) CultureInfo.InvariantCulture);
        if (originatorManagementData == null || !originatorManagementData.CommitmentUseBestEffort)
          return;
        if (originatorManagementData.BestEffortTolerencePolicy == 1)
        {
          ((TradeInfoObj) trade).Tolerance = val1;
        }
        else
        {
          if (originatorManagementData.BestEffortTolerencePolicy != 2 || !(num3 > 0M))
            return;
          Decimal val2 = Math.Round(num4 / num3 * 100M, 9);
          ((TradeInfoObj) trade).Tolerance = Math.Min(val1, val2);
        }
      }
    }

    public bool UpdateLoanInCorrespondentTrade(int tradeId, string loanNumber)
    {
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySettings("TRADE");
      CorrespondentTradeInfo trade = icorrespondentTradeManager.GetTrade(tradeId);
      if (trade == null)
        throw new Exception("Correspondent Trade with Id " + (object) tradeId + " does not exist.");
      PipelineInfo[] assignedOrPendingLoans = icorrespondentTradeManager.GetAssignedOrPendingLoans(tradeId, TradeLoanAssignment.GetFieldList().ToArray(), false);
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
          tradeToLoanProcess.CommitCorrespondentTradeAssignment(this.Session.SessionObjects, assignedPendingLoan, trade, true, new List<string>(), 0M, (LoanDataMgr.LockLoanSyncOption) 1);
        }
        catch (TradeLoanUpdateException ex)
        {
          if (((Exception) ex).Message != null)
            message = message + "Error in updating loan " + ((LoanToTradeAssignmentBase) assignedPendingLoan).PipelineInfo.LoanNumber + " due to - " + ex.Error.Message + "\n";
        }
        catch (Exception ex)
        {
          if (ex.Message != null)
            message = message + "Error in updating loan " + ((LoanToTradeAssignmentBase) assignedPendingLoan).PipelineInfo.LoanNumber + " due to - " + ex.Message + "\n";
        }
      }
      if (!string.IsNullOrEmpty(message))
        throw new Exception(message);
      return true;
    }

    private void RequiredFieldValidations(
      CorrespondentTrade trade,
      Hashtable settings,
      bool updateTrade,
      CorrespondentTradeInfo tradeInfo)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
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
          if (trade.TradeAmount < ((CommonPairOff) correspondentTradePairOff).TradeAmount)
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
      ICorrespondentMasterManager icorrespondentMasterManager = (ICorrespondentMasterManager) this.Session.GetObject("CorrespondentMasterManager");
      ICorrespondentTradeManager ctmgr = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      Decimal num1 = 0M;
      Hashtable companySettings = iconfigurationManager.GetCompanySettings("TRADE");
      ExternalOriginatorManagementData tpoSettings = iconfigurationManager.GetExternalOrganizationsWithoutExtension(this.Session.UserID, (string) null).Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => !x.DisabledLogin && x.ExternalID == tradeInfo.TPOID)).FirstOrDefault<ExternalOriginatorManagementData>();
      tradeInfo.TPOID = tpoSettings != null ? tpoSettings.ExternalID : throw new Exception("No TPO found for TPO ID - " + tradeInfo.TPOID);
      tradeInfo.OrganizationID = tpoSettings.OrgID;
      tradeInfo.CompanyName = tpoSettings.OrganizationName;
      tradeInfo.ExternalOriginatorManagementID = tpoSettings.oid;
      if (tpoSettings.CommitmentMandatory && !tpoSettings.CommitmentUseBestEffort && tradeInfo.CommitmentType != 2)
        throw new Exception("Only Mandatory commitment type can be selected based on TPO Commitment settings for TPO ID - " + tpoSettings.ExternalID);
      if (!tpoSettings.CommitmentMandatory && tpoSettings.CommitmentUseBestEffort && tradeInfo.CommitmentType != 1)
        throw new Exception("Only Best Efforts commitment type can be selected based on TPO Commitment settings for TPO ID - " + tpoSettings.ExternalID);
      if (tradeInfo.CommitmentType == 1 && tpoSettings.CommitmentUseBestEffort && tradeInfo.DeliveryType != 15)
        throw new Exception("Only Best Efforts delivery type is available to set based on TPO Settings.");
      if (tradeInfo.CommitmentType == 2)
      {
        string message = "";
        CorrespondentMasterDeliveryType deliveryType = tradeInfo.DeliveryType;
        if (deliveryType <= 20)
        {
          if (deliveryType <= 10)
          {
            if (deliveryType != 5)
            {
              if (deliveryType == 10 && !tpoSettings.IsCommitmentDeliveryForward)
                message = "Forwards Delivery type is not selectable based on TPO Settings.";
            }
            else if (!tpoSettings.IsCommitmentDeliveryAOT)
              message = "AOT Delivery type is not selectable based on TPO Settings.";
          }
          else if (deliveryType != 15)
          {
            if (deliveryType == 20 && !tpoSettings.IsCommitmentDeliveryIndividual)
              message = "Individual Mandatory Delivery type is not selectable based on TPO Settings.";
          }
          else
            message = "Individual Best Effort Delivery type is not valid Delivery Type for Mandatory commitment type";
        }
        else if (deliveryType <= 30)
        {
          if (deliveryType != 25)
          {
            if (deliveryType == 30 && !tpoSettings.IsCommitmentDeliveryBulk)
              message = "Bulk Delivery type is not selectable based on TPO Settings.";
          }
          else if (!tpoSettings.IsCommitmentDeliveryLiveTrade)
            message = "Direct Trade Delivery type is not selectable based on TPO Settings.";
        }
        else if (deliveryType != 35)
        {
          if (deliveryType == 40 && !tpoSettings.IsCommitmentDeliveryCoIssue)
            message = "Co-Issue Delivery type is not selectable based on TPO Settings.";
        }
        else if (!tpoSettings.IsCommitmentDeliveryBulkAOT)
          message = "Bulk AOT Delivery type is not selectable based on TPO Settings.";
        if (!string.IsNullOrEmpty(message))
          throw new Exception(message);
      }
      foreach (TradePricingItem simplePricingItem in ((TradeInfoObj) tradeInfo).Pricing.SimplePricingItems)
      {
        if (simplePricingItem.Rate > 99999.99M)
          throw new Exception("Rate cannot exceed maximum limit of 99999.99.");
        if (simplePricingItem.Price >= 1000M || simplePricingItem.Price < 0M)
          throw new Exception("Price must be greater than or equal to 0 and less than 1000.");
      }
      if (!string.IsNullOrEmpty(tradeInfo.CorrespondentMasterCommitmentNumber) && iconfigurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTMASTER") == "True")
      {
        CorrespondentMasterInfo[] correspondentMasterInfos = ctmgr.GetCorrespondentMasterInfos(tradeInfo);
        if (((IEnumerable<CorrespondentMasterInfo>) correspondentMasterInfos).Select<CorrespondentMasterInfo, bool>((Func<CorrespondentMasterInfo, bool>) (x => ((TradeBase) x).Name == tradeInfo.CorrespondentMasterCommitmentNumber)).Count<bool>() == 0)
          throw new Exception("The specified master commitment cannot be found. It may have been deleted by another user.");
        CorrespondentMasterInfo masterInfo = ((IEnumerable<CorrespondentMasterInfo>) correspondentMasterInfos).Where<CorrespondentMasterInfo>((Func<CorrespondentMasterInfo, bool>) (x => ((TradeBase) x).Name == tradeInfo.CorrespondentMasterCommitmentNumber)).First<CorrespondentMasterInfo>();
        if (((MasterCommitmentBase) masterInfo).DeliveryInfos.Where<MasterCommitmentDeliveryInfo>((Func<MasterCommitmentDeliveryInfo, bool>) (x => x.Type == tradeInfo.DeliveryType)).Count<MasterCommitmentDeliveryInfo>() == 0)
          throw new Exception("For selected Correspondent Master " + ((TradeBase) masterInfo).Name + ", Delivery Type " + ((Enum) (object) tradeInfo.DeliveryType).ToDescription() + " is not configured.");
        if (((TradeInfoObj) tradeInfo).TradeAmount > this.GetAvailableAmount(masterInfo, 0M, tpoSettings, ctmgr))
          throw new Exception("The Correspondent Trade Amount should be equal to or less than the Available amount of the Master Commitment.");
        tradeInfo.CorrespondentMasterID = ((MasterCommitmentBase) correspondentMasterInfos[0]).ID;
      }
      if (updateTrade)
      {
        CorrespondentTradeInfo trade = ctmgr.GetTrade(((TradeBase) tradeInfo).TradeID);
        num1 = ((TradeInfoObj) trade).TradeAmount;
        if (trade == null)
          throw new Exception("Correspondent Trade with Id " + (object) ((TradeBase) tradeInfo).TradeID + " does not exist.");
        if (((TradeInfoObj) trade).Status == 5 || ((TradeInfoObj) trade).Status == 6)
          throw new Exception("Corespondent Trade can not be updated when its in Archived or Pending status");
        if (((TradeBase) tradeInfo).TradeID == ((TradeBase) trade).TradeID && ((TradeBase) tradeInfo).Name != ((TradeBase) trade).Name && ctmgr.CheckExistingTradeByName(((TradeBase) tradeInfo).Name))
          throw new Exception("A correspondent trade with the commitment # '" + ((TradeBase) tradeInfo).Name + "' already exists. You must enter a unique commitment # for this correspondent trade.");
        if (((TradeBase) tradeInfo).TradeID == ((TradeBase) trade).TradeID && ((TradeBase) tradeInfo).Name != ((TradeBase) trade).Name && ctmgr.CheckTradeByName(((TradeBase) tradeInfo).Name))
          throw new Exception("A correspondent trade with the commitment # '" + ((TradeBase) tradeInfo).Name + "' was previously deleted. You must enter a unique commitment # for this correspondent trade.");
        if (((TradeBase) trade).Name != ((TradeBase) tradeInfo).Name)
          tradeInfo.OverrideTradeName = true;
        if (string.IsNullOrEmpty(((TradeBase) tradeInfo).Name))
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
        ((TradeBase) tradeInfo).Name = "use_autonumber_reserved";
      }
      else
      {
        if (ctmgr.CheckTradeByName(((TradeBase) tradeInfo).Name))
          throw new Exception("A correspondent trade with the commitment # '" + ((TradeBase) tradeInfo).Name + "' was previously deleted. You must enter a unique commitment # for this correspondent trade.");
        if (ctmgr.CheckExistingTradeByName(((TradeBase) tradeInfo).Name))
          throw new Exception("A correspondent trade with the commitment # '" + ((TradeBase) tradeInfo).Name + "' already exists. You must enter a unique commitment # for this correspondent trade.");
      }
      tradeInfo = this.PopulateEPPSProgramByID(tradeInfo);
      this.ValidateFromPPE(tradeInfo, companySettings);
      if ((tradeInfo.DeliveryType == 20 || tradeInfo.DeliveryType == 15) && (!string.IsNullOrEmpty(priceAdjustmentTemplateGUID) || !string.IsNullOrEmpty(SRPTemplateGUID) || ((TradeInfoObj) tradeInfo).Pricing.SimplePricingItems.Count > 0))
        throw new Exception("When Delivery type is Individual Delivery Type pricing value cannot be set");
      if (!string.IsNullOrEmpty(priceAdjustmentTemplateGUID))
      {
        PriceAdjustmentTemplate adjustmentTemplate = PriceAdjustmentTemplate.op_Explicit(iconfigurationManager.GetTemplateSettings((TemplateSettingsType) 11, ((IEnumerable<FileSystemEntry>) iconfigurationManager.GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 11, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == priceAdjustmentTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find Price Adjustment Template for GUID - " + priceAdjustmentTemplateGUID)));
        ((TradeInfoObj) tradeInfo).PriceAdjustments = adjustmentTemplate.PriceAdjustments;
      }
      if (!string.IsNullOrEmpty(SRPTemplateGUID))
      {
        SRPTableTemplate srpTableTemplate = SRPTableTemplate.op_Explicit(iconfigurationManager.GetTemplateSettings((TemplateSettingsType) 12, ((IEnumerable<FileSystemEntry>) iconfigurationManager.GetAllPublicTemplateSettingsFileEntries((TemplateSettingsType) 12, true)).Where<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Properties[(object) "Guid"].ToString() == SRPTemplateGUID.ToLower())).FirstOrDefault<FileSystemEntry>() ?? throw new Exception("Could not find SRP Template for GUID - " + priceAdjustmentTemplateGUID)) ?? throw new Exception("Could not find SRP Template for GUID - " + SRPTemplateGUID));
        ((TradeInfoObj) tradeInfo).SRPTable = srpTableTemplate.SRPTable;
      }
      if (updateTrade && ((TradeInfoObj) tradeInfo).MaxAmount < this.GetAssignedLoanAmount(((TradeBase) tradeInfo).TradeID, ctmgr))
        throw new Exception("The assigned amount cannot be more than the maximum amount for the correspondent trade.");
      MasterCommitmentType masterCommitmentType = tradeInfo.CommitmentType == 1 ? (MasterCommitmentType) 1 : (MasterCommitmentType) 2;
      if ((!tpoSettings.CommitmentUseBestEffort || tradeInfo.CommitmentType != 1) && tpoSettings.CommitmentTradePolicy == 1 && ((TradeInfoObj) tradeInfo).TradeAmount - num1 > ctmgr.CalculateTPOAvailableAmount(masterCommitmentType, tpoSettings))
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
          Decimal tolerance = ((TradeInfoObj) correspondentTradeInfo2).Tolerance;
          num2 = 0;
        }
        if (num2 != 0)
          throw new Exception("Tolerance field cannot be blank.");
        CorrespondentTradeInfo correspondentTradeInfo3 = tradeInfo;
        if ((correspondentTradeInfo3 != null ? (((TradeInfoObj) correspondentTradeInfo3).Tolerance == 0M ? 1 : 0) : 0) != 0)
          throw new Exception("Tolerance field cannot be zero.");
        Decimal? tolerance1 = ((TradeInfoObj) tradeInfo)?.Tolerance;
        Decimal num3 = (Decimal) 100;
        if (!(tolerance1.GetValueOrDefault() > num3 & tolerance1.HasValue))
        {
          CorrespondentTradeInfo correspondentTradeInfo4 = tradeInfo;
          if ((correspondentTradeInfo4 != null ? (((TradeInfoObj) correspondentTradeInfo4).Tolerance < 0M ? 1 : 0) : 0) == 0)
            return;
        }
        throw new Exception("The tolerance cannot be negative and cannot exceed 100 percent.");
      }
      CorrespondentTradeInfo correspondentTradeInfo5 = tradeInfo;
      if ((correspondentTradeInfo5 != null ? (((TradeInfoObj) correspondentTradeInfo5).Tolerance != 0M ? 1 : 0) : 1) != 0)
        throw new Exception("Tolerance violates commitment settings.");
      if (tradeInfo.CommitmentType == 2 && tpoSettings.MandatoryTolerancePct == 0M || tradeInfo.CommitmentType == 1 && tpoSettings.BestEffortTolerancePct == 0M)
        throw new Exception("Tolerance is not configured under commitment settings.");
    }

    private void ValidateFromPPE(CorrespondentTradeInfo tradeInfo, Hashtable settings)
    {
      bool flag1 = settings[(object) "EPPSLOANPROGELIPRICING"].ToString().ToLower() == "true";
      bool flag2 = false;
      bool flag3 = ((TradeInfoObj) tradeInfo).EPPSLoanProgramsFilter.Count > 0;
      if (this.Session.SessionObjects.StartupInfo.ProductPricingPartner != null && this.Session.SessionObjects.StartupInfo.ProductPricingPartner.IsEPPS)
        flag2 = flag3;
      if (flag1 & flag2 && (tradeInfo.DeliveryType == 5 || tradeInfo.DeliveryType == 25 || tradeInfo.DeliveryType == 10 || tradeInfo.DeliveryType == 35 || tradeInfo.DeliveryType == 40))
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
      return tpoSettings == null ? commitmentAmount : ((MasterCommitmentBase) masterInfo).CommitmentAmount + CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(commitmentAmount, tpoSettings.CommitmentUseBestEffortLimited, ctmgr.GetTradeInfosByMasterId(((MasterCommitmentBase) masterInfo).ID));
    }

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
          Decimal num = Utils.ParseDecimal(assignedPendingLoan.PipelineInfo.Info[(object) "Loan.TotalLoanAmount"], false, 0M);
          assignedLoanAmount += num;
        }
      }
      return assignedLoanAmount;
    }

    public List<EppsLoanProgram> GetCorrespondentTradeEppsLoanPrograms(string correspondentTradeName)
    {
      List<EppsLoanProgram> eppsLoanPrograms = new List<EppsLoanProgram>();
      ICorrespondentTradeManager mngr = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo correspondentTradeInfo = this.ValidateEppsLoanProgram(mngr, mngr.GetTrade(correspondentTradeName) ?? throw new Exception("Correspondent Trade does not exist."));
      if (correspondentTradeInfo != null)
      {
        foreach (EPPSLoanProgramFilter loanProgramFilter in (IEnumerable<EPPSLoanProgramFilter>) ((IEnumerable<EPPSLoanProgramFilter>) ((TradeInfoObj) correspondentTradeInfo).EPPSLoanProgramsFilter).OrderBy<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (r => r.ProgramName)))
          eppsLoanPrograms.Add(new EppsLoanProgram(loanProgramFilter.ProgramId, loanProgramFilter.ProgramName));
      }
      return eppsLoanPrograms;
    }

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
      CorrespondentTradeInfo correspondentTradeInfo = this.ValidateEppsLoanProgram(mngr, trade);
      if (correspondentTradeInfo == null)
        return false;
      List<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram> eppsLoanProgram1 = new SettingsService(this.Session).GetEppsLoanProgram();
      List<string> list1 = ((IEnumerable<EPPSLoanProgramFilter>) ((TradeInfoObj) correspondentTradeInfo).EPPSLoanProgramsFilter).Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (e => e.ProgramId)).ToList<string>();
      foreach (string programId1 in programIds)
      {
        string programId = programId1;
        EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram eppsLoanProgram2 = eppsLoanProgram1.Where<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram>((Func<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram, bool>) (e => e.ProgramId == programId)).FirstOrDefault<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram>();
        if (eppsLoanProgram2 != null && !list1.Contains(programId))
          ((TradeInfoObj) correspondentTradeInfo).EPPSLoanProgramsFilter.Add(new EPPSLoanProgramFilter(programId, eppsLoanProgram2.ProgramName));
      }
      mngr.UpdateTrade(correspondentTradeInfo, true);
      List<string> list2 = eppsLoanProgram1.Select<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram, string>((Func<EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram, string>) (s => s.ProgramId)).ToList<string>();
      List<string> list3 = programIds.Except<string>((IEnumerable<string>) list2).ToList<string>();
      if (eppsLoanProgram1 == null || eppsLoanProgram1.Count == 0 || list3.Count > 0)
        throw new Exception(string.Format("ICE PPE Loan Program ID {0} did not match table. Entry not added.", (object) string.Join(",", list3.ToArray())));
      return true;
    }

    public bool DeleteCorrespondentTradeEppsLoanPrograms(
      string correspondentTradeName,
      List<string> programIds)
    {
      ICorrespondentTradeManager mngr = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      CorrespondentTradeInfo correspondentTradeInfo = this.ValidateEppsLoanProgram(mngr, mngr.GetTrade(correspondentTradeName) ?? throw new Exception("Correspondent Trade does not exist."));
      if (correspondentTradeInfo == null)
        return false;
      List<string> list1 = ((IEnumerable<EPPSLoanProgramFilter>) ((TradeInfoObj) correspondentTradeInfo).EPPSLoanProgramsFilter).Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (e => e.ProgramId)).ToList<string>();
      List<string> list2 = programIds.Except<string>((IEnumerable<string>) list1).ToList<string>();
      EPPSLoanProgramFilters loanProgramFilters = new EPPSLoanProgramFilters();
      foreach (EPPSLoanProgramFilter loanProgramFilter in ((TradeInfoObj) correspondentTradeInfo).EPPSLoanProgramsFilter)
      {
        if (!programIds.Contains(loanProgramFilter.ProgramId))
          loanProgramFilters.Add(new EPPSLoanProgramFilter(loanProgramFilter.ProgramId, loanProgramFilter.ProgramName));
      }
      ((TradeInfoObj) correspondentTradeInfo).EPPSLoanProgramsFilter = loanProgramFilters;
      mngr.UpdateTrade(correspondentTradeInfo, true);
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
      return tradeInfo.DeliveryType == 5 || tradeInfo.DeliveryType == 10 || tradeInfo.DeliveryType == 25 || tradeInfo.DeliveryType == 35 || tradeInfo.DeliveryType == 40 ? tradeInfo : throw new Exception(string.Format("Correspondent trade Delivery Type of {0} does not support the use of the ICE PPE Loan Programs table.", (object) ((Enum) (object) tradeInfo.DeliveryType).ToDescription()));
    }

    public CorrespondentTradeInfo PopulateEPPSProgramByID(CorrespondentTradeInfo tradeInfo)
    {
      SettingsService settingsService = new SettingsService(this.Session);
      List<EPPSLoanProgram> programsSettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetEPPSLoanProgramsSettings();
      ((IEnumerable<EPPSLoanProgramFilter>) ((TradeInfoObj) tradeInfo).EPPSLoanProgramsFilter).Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (e => e.ProgramId)).ToList<string>();
      string[] array = ((IEnumerable<EPPSLoanProgramFilter>) ((TradeInfoObj) tradeInfo).EPPSLoanProgramsFilter).Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (x => x.ProgramId)).ToArray<string>();
      ((TradeInfoObj) tradeInfo).EPPSLoanProgramsFilter.Clear();
      foreach (string str in array)
      {
        string id = str;
        EPPSLoanProgram eppsLoanProgram = programsSettings.Where<EPPSLoanProgram>((Func<EPPSLoanProgram, bool>) (e => e.ProgramID == id)).FirstOrDefault<EPPSLoanProgram>();
        if (eppsLoanProgram != null && !((IEnumerable<EPPSLoanProgramFilter>) ((TradeInfoObj) tradeInfo).EPPSLoanProgramsFilter).Select<EPPSLoanProgramFilter, string>((Func<EPPSLoanProgramFilter, string>) (x => x.ProgramId.ToLower())).ToList<string>().Contains(id))
          ((TradeInfoObj) tradeInfo).EPPSLoanProgramsFilter.Add(new EPPSLoanProgramFilter(eppsLoanProgram.ProgramID, eppsLoanProgram.ProgramName));
      }
      return tradeInfo;
    }
  }
}
