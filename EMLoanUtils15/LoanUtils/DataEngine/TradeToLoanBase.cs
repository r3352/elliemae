// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.TradeToLoanBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
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
  internal class TradeToLoanBase : ITradeToLoan
  {
    private const string className = "TradeToLoanBase�";
    protected static string sw = Tracing.SwOutsideLoan;

    public TradeToLoanBase(SessionObjects sessionObjects, LoanDataMgr loanDataMgr)
    {
      this.sessionObjects = sessionObjects;
      this.loanDataMgr = loanDataMgr;
    }

    protected SessionObjects sessionObjects { get; set; }

    protected LoanDataMgr loanDataMgr { get; set; }

    public virtual void RemoveFromTrade(TradeInfoObj trade, bool rejected)
    {
      this.RemoveFromTrade(trade, rejected, new List<string>());
    }

    protected void SetCalculator()
    {
      this.loanDataMgr.LoanData.AttachCalculator((ILoanCalculator) new LoanCalculator(this.sessionObjects, this.sessionObjects.LoanManager.GetLoanConfigurationInfo(), this.loanDataMgr.LoanData));
    }

    public virtual void RemoveFromTrade(
      TradeInfoObj trade,
      bool rejected,
      List<string> skipFieldList)
    {
      if (this.loanDataMgr.LoanData.Calculator == null)
        this.SetCalculator();
      if (this.loanDataMgr.LoanData.GetField("2819") != trade.Guid)
        return;
      this.CopyPreviousSnapshot(trade, true, skipFieldList);
      this.loanDataMgr.LoanData.Calculator.FormCalculation("PurchaseAdvice", (string) null, (string) null);
      if (rejected)
        this.loanDataMgr.LoanData.SetField("2031", "Rejected");
      this.ApplyInvestorToLoan(new Investor(), (ContactInformation) null, true, skipFieldList);
      if (trade == null || trade.TradeType != TradeType.MbsPool)
        return;
      MbsPoolInfo mbsPoolInfo = (MbsPoolInfo) trade;
      if (mbsPoolInfo.PoolMortgageType != MbsPoolMortgageType.FannieMae && mbsPoolInfo.PoolMortgageType != MbsPoolMortgageType.FannieMaePE)
        return;
      this.loanDataMgr.LoanData.SetField("ULDD.X33", "N");
    }

    public virtual void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      this.AssignToTrade(trade, skipFieldList, securityPrice, new Dictionary<string, string>(), LoanDataMgr.LockLoanSyncOption.syncLockToLoan);
    }

    public virtual void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      Tracing.Log(TradeToLoanBase.sw, nameof (TradeToLoanBase), TraceLevel.Info, "Assigning loan to trade " + (object) trade.TradeID + "..");
      if (Utils.ParseDate((object) this.loanDataMgr.LoanData.GetField("4120"), DateTime.MinValue) != DateTime.MinValue)
        throw new Exception("The loan has been withdrawn");
      if (this.loanDataMgr.LoanData.GetField("2819") != trade.Guid)
        this.CopyPreviousSnapshot(trade, false, (List<string>) null);
      this.RefreshTradeData(trade, skipFieldList, securityPrice, updateFieldList, syncOption, true);
    }

    public virtual void CopyPreviousSnapshot(
      TradeInfoObj trade,
      bool remove,
      List<string> skipFieldList = null)
    {
      if (trade.TradeType == TradeType.LoanTrade)
      {
        string field = this.loanDataMgr.LoanData.GetField("2819");
        if (!string.IsNullOrEmpty(field) && field != trade.Guid)
        {
          MbsPoolMortgageType poolMortgageType = this.sessionObjects.MbsPoolManager.GetMbsPoolMortgageType(field);
          if (poolMortgageType >= MbsPoolMortgageType.FannieMae)
          {
            this.loanDataMgr.LoanData.SetField("3890", "");
            this.loanDataMgr.LoanData.SetField("4019", "");
            this.ClearUlddFields(poolMortgageType);
          }
        }
      }
      skipFieldList = skipFieldList ?? new List<string>();
      LockRequestLog rec1 = new LockRequestLog(this.loanDataMgr.LoanData.GetLogList());
      DateTime serverTime = this.sessionObjects.Session.ServerTime;
      string str1 = (string) null;
      string str2 = (string) null;
      rec1.Date = serverTime;
      rec1.SetRequestingUser(this.sessionObjects.UserID, this.sessionObjects.UserInfo.FullName);
      rec1.LockRequestStatus = RateLockRequestStatus.RateLocked;
      rec1.DisplayInLog = remove;
      if (!remove)
      {
        this.loanDataMgr.LoanData.SetField("2291", DateTime.Now.ToString("MM/dd/yyyy"));
        this.loanDataMgr.LoanData.SetField("2292", DateTime.Now.ToShortTimeString());
        this.loanDataMgr.LoanData.SetField("2030", this.sessionObjects.UserInfo.FullName);
        this.loanDataMgr.LoanData.SetField("2231", this.loanDataMgr.LoanData.GetField("2160"));
        this.loanDataMgr.LoanData.SetField("2288", this.loanDataMgr.LoanData.GetField("352"));
      }
      else
      {
        this.loanDataMgr.LoanData.SetField("2032", "");
        this.loanDataMgr.LoanData.SetField("2819", "");
      }
      LockRequestLog confirmedLockRequest = this.loanDataMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest();
      Hashtable fields = (Hashtable) null;
      LockRequestLog lockRequestLog = this.loanDataMgr.LoanData.GetLogList().GetCurrentLockOrRequest();
      if (remove && lockRequestLog == null)
      {
        string requestGUID = "";
        if (this.loanDataMgr.LoanData.GetField("LOCKRATE.ISCANCELLED") == "Y")
        {
          rec1.LockRequestStatus = RateLockRequestStatus.Cancelled;
          LockCancellationLog lockCancellation = this.loanDataMgr.LoanData.GetLogList().GetMostRecentLockCancellation();
          if (lockCancellation == null)
          {
            Tracing.Log(TradeToLoanBase.sw, nameof (TradeToLoanBase), TraceLevel.Error, "(CopyPreviousSnapshot) Error getting cancellation log.");
            return;
          }
          requestGUID = lockCancellation.RequestGUID;
        }
        else if (this.loanDataMgr.LoanData.GetField("LOCKRATE.REQUESTSTATUS") == "Denied")
        {
          LockDenialLog denialLockLog = this.loanDataMgr.LoanData.GetLogList().GetDenialLockLog();
          if (denialLockLog == null)
          {
            Tracing.Log(TradeToLoanBase.sw, nameof (TradeToLoanBase), TraceLevel.Error, "(CopyPreviousSnapshot) Error getting denial log.");
            return;
          }
          requestGUID = denialLockLog.RequestGUID;
          Hashtable lockRequestSnapshot = this.loanDataMgr.LoanData.GetLogList().GetLockRequest(requestGUID).GetLockRequestSnapshot();
          LockRequestLog[] allLockRequests = this.loanDataMgr.LoanData.GetLogList().GetAllLockRequests();
          if (allLockRequests != null && allLockRequests.Length >= 2 && allLockRequests[allLockRequests.Length - 1].Guid == requestGUID && this.loanDataMgr.LoanData.GetLogList().GetConfirmLockLogForRequestLog(allLockRequests[allLockRequests.Length - 2].Guid) != null && lockRequestSnapshot[(object) "3839"] != null && !string.IsNullOrEmpty(lockRequestSnapshot[(object) "3839"].ToString()))
            rec1.LockRequestStatus = RateLockRequestStatus.RequestDenied;
          else
            requestGUID = "";
        }
        if (requestGUID != "")
          lockRequestLog = this.loanDataMgr.LoanData.GetLogList().GetLockRequest(requestGUID);
        rec1.AlertLoanOfficer = false;
      }
      if (lockRequestLog != null)
      {
        if (lockRequestLog.LockRequestStatus == RateLockRequestStatus.Requested)
        {
          lockRequestLog.LockRequestStatus = RateLockRequestStatus.OldRequest;
          lockRequestLog = this.loanDataMgr.LoanData.GetLogList().GetCurrentLockRequest() ?? lockRequestLog;
        }
        if (lockRequestLog.LockRequestStatus == RateLockRequestStatus.RateLocked)
          lockRequestLog.LockRequestStatus = RateLockRequestStatus.OldLock;
        else if (lockRequestLog.LockRequestStatus == RateLockRequestStatus.Requested)
          lockRequestLog.LockRequestStatus = RateLockRequestStatus.OldRequest;
        rec1.IsLockExtension = lockRequestLog.IsLockExtension;
        rec1.IsLockCancellation = lockRequestLog.IsLockCancellation;
        rec1.BuySideExpirationDate = lockRequestLog.BuySideExpirationDate;
        if (lockRequestLog.IsLockExtension)
        {
          rec1.BuySideNewLockExtensionDate = lockRequestLog.BuySideNewLockExtensionDate;
          rec1.BuySideNumDayExtended = lockRequestLog.BuySideNumDayExtended;
          rec1.CumulatedDaystoExtend = lockRequestLog.CumulatedDaystoExtend;
        }
        rec1.InvestorName = lockRequestLog.InvestorName;
        fields = lockRequestLog.GetLockRequestSnapshot();
        if (fields != null)
        {
          if (remove)
          {
            this.loanDataMgr.SyncLockRequestSnapshotToLoan(fields, false, false);
          }
          else
          {
            foreach (string buySideField in LockRequestLog.BuySideFields)
              this.loanDataMgr.LoanData.SetField(buySideField, string.Concat(fields[(object) buySideField]));
            foreach (string compSideField in LockRequestLog.CompSideFields)
              this.loanDataMgr.LoanData.SetField(compSideField, string.Concat(fields[(object) compSideField]));
          }
          if (lockRequestLog.IsLockExtension)
          {
            foreach (string lockExtensionField in LockRequestLog.LockExtensionFields)
              this.loanDataMgr.LoanData.SetField(lockExtensionField, string.Concat(fields[(object) lockExtensionField]));
            str2 = (string) fields[(object) "3431"];
          }
          str1 = (string) fields[(object) "3839"];
          Decimal num = Utils.ParseDecimal(fields[(object) "2225"]) + Utils.ParseDecimal(fields[(object) "2227"]) + Utils.ParseDecimal(fields[(object) "2229"]);
          for (int index = 2483; index <= 2515; index += 2)
            num += Utils.ParseDecimal(fields[(object) index.ToString()]);
          this.loanDataMgr.LoanData.SetField("2223", (Utils.ParseDecimal((object) this.loanDataMgr.LoanData.GetField("2231")) - num).ToString());
        }
      }
      if (remove)
      {
        foreach (string sellSideField in LockRequestLog.SellSideFields)
          this.loanDataMgr.LoanData.SetField(sellSideField, "");
      }
      this.loanDataMgr.LoanData.SetField("2592", serverTime.ToString());
      Hashtable hashtable = this.loanDataMgr.LoanData.PrepareLockRequestData();
      if (hashtable.Contains((object) "2592"))
        hashtable[(object) "2592"] = (object) serverTime;
      else
        hashtable.Add((object) "2592", (object) serverTime);
      if (str2 != null)
      {
        if (hashtable.Contains((object) "3431"))
          hashtable[(object) "3431"] = (object) str2;
        else
          hashtable.Add((object) "3431", (object) str2);
      }
      if (remove)
      {
        if (confirmedLockRequest != null)
          hashtable[(object) "3839"] = (object) confirmedLockRequest.Guid;
        else if (str1 != null)
          hashtable[(object) "3839"] = (object) str1;
        hashtable[(object) "3888"] = (object) "";
        hashtable[(object) "4118"] = (object) "";
        if (fields != null)
        {
          foreach (string skipField in skipFieldList)
          {
            switch (skipField)
            {
              case "Commitment Number":
                if (fields[(object) "2286"] != null)
                  hashtable[(object) "2286"] = fields[(object) "2286"];
                if (fields[(object) "996"] != null)
                {
                  hashtable[(object) "996"] = fields[(object) "996"];
                  continue;
                }
                continue;
              case "Investor Information":
                if (fields[(object) "2278"] != null)
                  hashtable[(object) "2278"] = fields[(object) "2278"];
                if (fields[(object) "2279"] != null)
                  hashtable[(object) "2279"] = fields[(object) "2279"];
                if (fields[(object) "2280"] != null)
                  hashtable[(object) "2280"] = fields[(object) "2280"];
                if (fields[(object) "3055"] != null)
                  hashtable[(object) "3055"] = fields[(object) "3055"];
                if (fields[(object) "2281"] != null)
                  hashtable[(object) "2281"] = fields[(object) "2281"];
                if (fields[(object) "2282"] != null)
                  hashtable[(object) "2282"] = fields[(object) "2282"];
                if (fields[(object) "2283"] != null)
                  hashtable[(object) "2283"] = fields[(object) "2283"];
                if (fields[(object) "2284"] != null)
                  hashtable[(object) "2284"] = fields[(object) "2284"];
                if (fields[(object) "2285"] != null)
                  hashtable[(object) "2285"] = fields[(object) "2285"];
                if (fields[(object) "3534"] != null)
                  hashtable[(object) "3534"] = fields[(object) "3534"];
                if (fields[(object) "3535"] != null)
                  hashtable[(object) "3535"] = fields[(object) "3535"];
                if (fields[(object) "3888"] != null)
                  hashtable[(object) "3888"] = fields[(object) "3888"];
                if (fields[(object) "4118"] != null)
                {
                  hashtable[(object) "4118"] = fields[(object) "4118"];
                  continue;
                }
                continue;
              case "Price Adjustments":
                for (int index = 0; index < 20; ++index)
                {
                  int num = 2233 + 2 * index;
                  string key1 = num.ToString();
                  num = 2233 + 2 * index + 1;
                  string key2 = num.ToString();
                  if (fields[(object) key1] != null)
                    hashtable[(object) key1] = fields[(object) key1];
                  if (fields[(object) key2] != null)
                    hashtable[(object) key2] = fields[(object) key2];
                }
                if (fields[(object) "2273"] != null)
                {
                  hashtable[(object) "2273"] = fields[(object) "2273"];
                  continue;
                }
                continue;
              default:
                if (fields[(object) skipField] != null)
                {
                  hashtable[(object) skipField] = fields[(object) skipField];
                  continue;
                }
                continue;
            }
          }
        }
      }
      else if (confirmedLockRequest != null)
        hashtable[(object) "3839"] = (object) confirmedLockRequest.Guid;
      rec1.AddLockRequestSnapshot(hashtable);
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec1);
      if (remove)
        this.loanDataMgr.SyncLockRequestSnapshotToLoan(hashtable, false, false);
      if (!remove)
      {
        this.loanDataMgr.LoanData.SetField("3360", "");
        this.loanDataMgr.LoanData.SetField("3361", "");
        this.loanDataMgr.LoanData.SetField("3362", "");
        this.loanDataMgr.LoanData.SetField("3369", "");
        this.loanDataMgr.LoanData.SetField("2819", trade.Guid);
        if (!(rec1.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked)))
          return;
        LockConfirmLog rec2 = new LockConfirmLog();
        rec2.BuySideExpirationDate = rec1.BuySideExpirationDate;
        rec2.SetConfirmingUser(this.sessionObjects.UserID, this.sessionObjects.UserInfo.FullName);
        rec2.Date = Utils.ParseDate(hashtable[(object) "2592"], serverTime);
        rec2.DateConfirmed = hashtable[(object) "2592"].ToString();
        rec2.RequestGUID = rec1.Guid;
        rec2.SellSideDeliveryDate = rec1.SellSideDeliveryDate;
        rec2.SellSideDeliveredBy = rec1.SellSideDeliveredBy;
        rec2.SellSideExpirationDate = rec1.SellSideExpirationDate;
        rec2.AlertLoanOfficer = true;
        rec2.DisplayInLog = false;
        rec2.CommitmentTermEnabled = Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]);
        rec2.EnableZeroParPricingRetail = Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]);
        rec2.EnableZeroParPricingWholesale = Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]);
        this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec2);
      }
      else
      {
        if (!skipFieldList.Contains("2012"))
          this.loanDataMgr.LoanData.SetField("2012", "");
        if (!skipFieldList.Contains("2013"))
          this.loanDataMgr.LoanData.SetField("2013", "");
        if (!skipFieldList.Contains("2207"))
          this.loanDataMgr.LoanData.SetField("2207", "");
        if (!skipFieldList.Contains("2209"))
          this.loanDataMgr.LoanData.SetField("2209", "");
        if (!skipFieldList.Contains("996") && !skipFieldList.Contains("Commitment Number"))
          this.loanDataMgr.LoanData.SetField("996", "");
        if (rec1.IsLockCancellation)
        {
          LockCancellationLog rec3 = new LockCancellationLog();
          rec3.SetCancellingUser(this.sessionObjects.UserInfo);
          rec3.Date = this.sessionObjects.Session.ServerTime;
          rec3.TimeCancelled = this.sessionObjects.Session.ServerTime.ToLongTimeString();
          rec3.RequestGUID = rec1.Guid;
          rec3.AlertLoanOfficer = true;
          this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec3);
        }
        else if (rec1.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied))
        {
          LockDenialLog rec4 = new LockDenialLog();
          rec4.SetDenyingUser(this.sessionObjects.UserInfo);
          rec4.Date = this.sessionObjects.Session.ServerTime;
          rec4.TimeDenied = this.sessionObjects.Session.ServerTime.ToLongTimeString();
          rec4.RequestGUID = rec1.Guid;
          rec4.AlertLoanOfficer = true;
          rec4.Comments = rec4.Comments;
          this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec4);
        }
        else
        {
          if (!(rec1.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked)))
            return;
          LockConfirmLog rec5 = new LockConfirmLog();
          rec5.BuySideExpirationDate = rec1.BuySideExpirationDate;
          rec5.SetConfirmingUser(this.sessionObjects.UserID, this.sessionObjects.UserInfo.FullName);
          rec5.Date = Utils.ParseDate(hashtable[(object) "2592"], serverTime);
          rec5.DateConfirmed = hashtable[(object) "2592"].ToString();
          rec5.RequestGUID = rec1.Guid;
          rec5.SellSideDeliveryDate = rec1.SellSideDeliveryDate;
          rec5.SellSideDeliveredBy = rec1.SellSideDeliveredBy;
          rec5.SellSideExpirationDate = rec1.SellSideExpirationDate;
          rec5.AlertLoanOfficer = true;
          rec5.DisplayInLog = false;
          rec5.CommitmentTermEnabled = Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]);
          rec5.EnableZeroParPricingRetail = Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]);
          rec5.EnableZeroParPricingWholesale = Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]);
          this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec5);
        }
      }
    }

    public virtual void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price)
    {
      this.ModifyTradeStatus(trade, status, skipFieldList, price, new Dictionary<string, string>());
    }

    public virtual void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList)
    {
      Tracing.Log(TradeToLoanBase.sw, nameof (TradeToLoanBase), TraceLevel.Info, "Setting loan trade status to " + status + "..");
      int num = this.loanDataMgr.LoanData.GetField("2819") != trade.Guid ? 1 : 0;
    }

    public virtual void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price)
    {
    }

    public virtual void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan,
      bool forAssignment = false)
    {
    }

    public virtual void ApplyInvestorToLoan(
      Investor investor,
      ContactInformation assignee,
      bool updateInvestor)
    {
      this.ApplyInvestorToLoan(investor, assignee, updateInvestor, new List<string>());
    }

    public virtual void ApplyInvestorToLoan(
      Investor investor,
      ContactInformation assignee,
      bool updateInvestor,
      List<string> skipFieldList)
    {
      Tracing.Log(TradeToLoanBase.sw, nameof (TradeToLoanBase), TraceLevel.Info, "Applying investor fields to loan object");
      if (updateInvestor)
      {
        this.loanDataMgr.LoanData.SetField("VEND.X263", investor.ContactInformation.EntityName);
        this.loanDataMgr.LoanData.SetField("VEND.X264", investor.ContactInformation.Address.Street1);
        this.loanDataMgr.LoanData.SetField("VEND.X265", investor.ContactInformation.Address.City);
        this.loanDataMgr.LoanData.SetField("VEND.X266", investor.ContactInformation.Address.State);
        this.loanDataMgr.LoanData.SetField("VEND.X267", investor.ContactInformation.Address.Zip);
        this.loanDataMgr.LoanData.SetField("VEND.X271", investor.ContactInformation.ContactName);
        this.loanDataMgr.LoanData.SetField("VEND.X272", investor.ContactInformation.PhoneNumber);
        this.loanDataMgr.LoanData.SetField("VEND.X273", investor.ContactInformation.EmailAddress);
        this.loanDataMgr.LoanData.SetField("VEND.X274", investor.ContactInformation.FaxNumber);
      }
      if (assignee != null && (assignee.EntityName ?? "").Trim() != "")
      {
        this.loanDataMgr.LoanData.SetField("VEND.X369", assignee.EntityName);
        this.loanDataMgr.LoanData.SetField("VEND.X370", assignee.Address.Street1);
        this.loanDataMgr.LoanData.SetField("VEND.X371", assignee.Address.City);
        this.loanDataMgr.LoanData.SetField("VEND.X372", assignee.Address.State);
        this.loanDataMgr.LoanData.SetField("VEND.X373", assignee.Address.Zip);
        this.loanDataMgr.LoanData.SetField("VEND.X374", assignee.ContactName);
        this.loanDataMgr.LoanData.SetField("VEND.X375", assignee.PhoneNumber);
        this.loanDataMgr.LoanData.SetField("VEND.X376", assignee.EmailAddress);
        this.loanDataMgr.LoanData.SetField("VEND.X377", assignee.FaxNumber);
      }
      else
      {
        this.loanDataMgr.LoanData.SetField("VEND.X369", investor.ShippingInformation.EntityName);
        this.loanDataMgr.LoanData.SetField("VEND.X370", investor.ShippingInformation.Address.Street1);
        this.loanDataMgr.LoanData.SetField("VEND.X371", investor.ShippingInformation.Address.City);
        this.loanDataMgr.LoanData.SetField("VEND.X372", investor.ShippingInformation.Address.State);
        this.loanDataMgr.LoanData.SetField("VEND.X373", investor.ShippingInformation.Address.Zip);
        this.loanDataMgr.LoanData.SetField("VEND.X374", investor.ShippingInformation.ContactName);
        this.loanDataMgr.LoanData.SetField("VEND.X375", investor.ShippingInformation.PhoneNumber);
        this.loanDataMgr.LoanData.SetField("VEND.X376", investor.ShippingInformation.EmailAddress);
        this.loanDataMgr.LoanData.SetField("VEND.X377", investor.ShippingInformation.FaxNumber);
      }
      this.loanDataMgr.LoanData.SetField("VEND.X378", investor.CustomerServiceInformation.EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X379", investor.CustomerServiceInformation.Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X380", investor.CustomerServiceInformation.Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X381", investor.CustomerServiceInformation.Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X382", investor.CustomerServiceInformation.Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X383", investor.CustomerServiceInformation.ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X384", investor.CustomerServiceInformation.PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X385", investor.CustomerServiceInformation.EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X386", investor.CustomerServiceInformation.FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X387", investor.TrailingDocumentsInformation.EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X388", investor.TrailingDocumentsInformation.Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X389", investor.TrailingDocumentsInformation.Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X390", investor.TrailingDocumentsInformation.Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X391", investor.TrailingDocumentsInformation.Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X392", investor.TrailingDocumentsInformation.ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X393", investor.TrailingDocumentsInformation.PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X394", investor.TrailingDocumentsInformation.EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X395", investor.TrailingDocumentsInformation.FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X529", investor.GetContactInformation(InvestorContactType.payment).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X530", investor.GetContactInformation(InvestorContactType.payment).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X532", investor.GetContactInformation(InvestorContactType.payment).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X533", investor.GetContactInformation(InvestorContactType.payment).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X534", investor.GetContactInformation(InvestorContactType.payment).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X535", investor.GetContactInformation(InvestorContactType.payment).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X536", investor.GetContactInformation(InvestorContactType.payment).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X537", investor.GetContactInformation(InvestorContactType.payment).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X538", investor.GetContactInformation(InvestorContactType.payment).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X539", investor.GetContactInformation(InvestorContactType.insurance).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X540", investor.GetContactInformation(InvestorContactType.insurance).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X542", investor.GetContactInformation(InvestorContactType.insurance).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X543", investor.GetContactInformation(InvestorContactType.insurance).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X544", investor.GetContactInformation(InvestorContactType.insurance).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X545", investor.GetContactInformation(InvestorContactType.insurance).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X546", investor.GetContactInformation(InvestorContactType.insurance).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X547", investor.GetContactInformation(InvestorContactType.insurance).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X548", investor.GetContactInformation(InvestorContactType.insurance).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X549", investor.GetContactInformation(InvestorContactType.notedelivery).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X550", investor.GetContactInformation(InvestorContactType.notedelivery).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X552", investor.GetContactInformation(InvestorContactType.notedelivery).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X553", investor.GetContactInformation(InvestorContactType.notedelivery).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X554", investor.GetContactInformation(InvestorContactType.notedelivery).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X555", investor.GetContactInformation(InvestorContactType.notedelivery).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X556", investor.GetContactInformation(InvestorContactType.notedelivery).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X557", investor.GetContactInformation(InvestorContactType.notedelivery).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X558", investor.GetContactInformation(InvestorContactType.notedelivery).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X559", investor.GetContactInformation(InvestorContactType.taxnotice).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X560", investor.GetContactInformation(InvestorContactType.taxnotice).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X562", investor.GetContactInformation(InvestorContactType.taxnotice).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X563", investor.GetContactInformation(InvestorContactType.taxnotice).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X564", investor.GetContactInformation(InvestorContactType.taxnotice).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X565", investor.GetContactInformation(InvestorContactType.taxnotice).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X566", investor.GetContactInformation(InvestorContactType.taxnotice).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X567", investor.GetContactInformation(InvestorContactType.taxnotice).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X568", investor.GetContactInformation(InvestorContactType.taxnotice).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X569", investor.GetContactInformation(InvestorContactType.mortgageinsurance).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X570", investor.GetContactInformation(InvestorContactType.mortgageinsurance).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X572", investor.GetContactInformation(InvestorContactType.mortgageinsurance).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X573", investor.GetContactInformation(InvestorContactType.mortgageinsurance).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X574", investor.GetContactInformation(InvestorContactType.mortgageinsurance).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X575", investor.GetContactInformation(InvestorContactType.mortgageinsurance).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X576", investor.GetContactInformation(InvestorContactType.mortgageinsurance).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X577", investor.GetContactInformation(InvestorContactType.mortgageinsurance).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X578", investor.GetContactInformation(InvestorContactType.mortgageinsurance).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X579", investor.GetContactInformation(InvestorContactType.loandelivery).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X580", investor.GetContactInformation(InvestorContactType.loandelivery).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X582", investor.GetContactInformation(InvestorContactType.loandelivery).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X583", investor.GetContactInformation(InvestorContactType.loandelivery).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X584", investor.GetContactInformation(InvestorContactType.loandelivery).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X585", investor.GetContactInformation(InvestorContactType.loandelivery).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X586", investor.GetContactInformation(InvestorContactType.loandelivery).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X587", investor.GetContactInformation(InvestorContactType.loandelivery).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X588", investor.GetContactInformation(InvestorContactType.loandelivery).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X589", investor.GetContactInformation(InvestorContactType.assignment).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X590", investor.GetContactInformation(InvestorContactType.assignment).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X592", investor.GetContactInformation(InvestorContactType.assignment).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X593", investor.GetContactInformation(InvestorContactType.assignment).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X594", investor.GetContactInformation(InvestorContactType.assignment).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X595", investor.GetContactInformation(InvestorContactType.assignment).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X596", investor.GetContactInformation(InvestorContactType.assignment).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X597", investor.GetContactInformation(InvestorContactType.assignment).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X598", investor.GetContactInformation(InvestorContactType.assignment).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X599", investor.GetContactInformation(InvestorContactType.correspondence).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X600", investor.GetContactInformation(InvestorContactType.correspondence).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X602", investor.GetContactInformation(InvestorContactType.correspondence).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X603", investor.GetContactInformation(InvestorContactType.correspondence).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X604", investor.GetContactInformation(InvestorContactType.correspondence).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X605", investor.GetContactInformation(InvestorContactType.correspondence).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X606", investor.GetContactInformation(InvestorContactType.correspondence).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X607", investor.GetContactInformation(InvestorContactType.correspondence).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X608", investor.GetContactInformation(InvestorContactType.correspondence).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X609", investor.GetContactInformation(InvestorContactType.generic1).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X610", investor.GetContactInformation(InvestorContactType.generic1).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X612", investor.GetContactInformation(InvestorContactType.generic1).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X613", investor.GetContactInformation(InvestorContactType.generic1).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X614", investor.GetContactInformation(InvestorContactType.generic1).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X615", investor.GetContactInformation(InvestorContactType.generic1).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X616", investor.GetContactInformation(InvestorContactType.generic1).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X617", investor.GetContactInformation(InvestorContactType.generic1).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X618", investor.GetContactInformation(InvestorContactType.generic1).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X619", investor.GetContactInformation(InvestorContactType.generic2).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X620", investor.GetContactInformation(InvestorContactType.generic2).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X622", investor.GetContactInformation(InvestorContactType.generic2).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X623", investor.GetContactInformation(InvestorContactType.generic2).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X624", investor.GetContactInformation(InvestorContactType.generic2).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X625", investor.GetContactInformation(InvestorContactType.generic2).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X626", investor.GetContactInformation(InvestorContactType.generic2).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X627", investor.GetContactInformation(InvestorContactType.generic2).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X628", investor.GetContactInformation(InvestorContactType.generic2).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X629", investor.GetContactInformation(InvestorContactType.generic3).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X630", investor.GetContactInformation(InvestorContactType.generic3).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X632", investor.GetContactInformation(InvestorContactType.generic3).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X633", investor.GetContactInformation(InvestorContactType.generic3).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X634", investor.GetContactInformation(InvestorContactType.generic3).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X635", investor.GetContactInformation(InvestorContactType.generic3).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X636", investor.GetContactInformation(InvestorContactType.generic3).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X637", investor.GetContactInformation(InvestorContactType.generic3).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X638", investor.GetContactInformation(InvestorContactType.generic3).FaxNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X639", investor.GetContactInformation(InvestorContactType.generic4).EntityName);
      this.loanDataMgr.LoanData.SetField("VEND.X640", investor.GetContactInformation(InvestorContactType.generic4).Address.Street1);
      this.loanDataMgr.LoanData.SetField("VEND.X642", investor.GetContactInformation(InvestorContactType.generic4).Address.City);
      this.loanDataMgr.LoanData.SetField("VEND.X643", investor.GetContactInformation(InvestorContactType.generic4).Address.State);
      this.loanDataMgr.LoanData.SetField("VEND.X644", investor.GetContactInformation(InvestorContactType.generic4).Address.Zip);
      this.loanDataMgr.LoanData.SetField("VEND.X645", investor.GetContactInformation(InvestorContactType.generic4).ContactName);
      this.loanDataMgr.LoanData.SetField("VEND.X646", investor.GetContactInformation(InvestorContactType.generic4).PhoneNumber);
      this.loanDataMgr.LoanData.SetField("VEND.X647", investor.GetContactInformation(InvestorContactType.generic4).EmailAddress);
      this.loanDataMgr.LoanData.SetField("VEND.X648", investor.GetContactInformation(InvestorContactType.generic4).FaxNumber);
      if (skipFieldList.Contains("1397"))
        return;
      FieldDefinition field = EncompassFields.GetField("1397");
      FieldOption fieldOption = field.Options.GetOptionByValue(investor.TypeOfPurchaser) ?? field.Options.GetOptionByText(investor.TypeOfPurchaser);
      if (fieldOption == null)
        return;
      this.loanDataMgr.LoanData.SetField("1397", fieldOption.Value);
    }

    public virtual void CopyTradeDataToSnapshot(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Hashtable newData,
      Dictionary<string, string> updateFieldList)
    {
    }

    public virtual void ExtendLockWithTrade(
      TradeInfoObj trade,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
    }

    public virtual void ClearUlddFields(MbsPoolMortgageType poolMortgageType)
    {
      if (poolMortgageType != MbsPoolMortgageType.FannieMae && poolMortgageType != MbsPoolMortgageType.FannieMaePE)
        return;
      this.loanDataMgr.LoanData.SetField("ULDD.X65", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X72", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X69", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X66", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X156", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X158", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X67", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X115", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X47", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X48", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X40", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X41", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X39", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X45", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X68", "");
      this.loanDataMgr.LoanData.SetField("ULDD.FNM.X70", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X71", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X73", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X74", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X75", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X76", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X77", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X78", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X79", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X80", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X81", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X83", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X84", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X85", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X86", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X87", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X117", "");
      this.loanDataMgr.LoanData.SetField("ULDD.X44", "");
      this.loanDataMgr.LoanData.SetField("997", "");
    }
  }
}
