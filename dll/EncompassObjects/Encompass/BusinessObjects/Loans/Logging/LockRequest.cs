// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LockRequest : LogEntry, ILockRequest
  {
    private LockRequestLog logItem;
    private LockConfirmation confirmation;
    private LockDenial denial;
    private LockRequestFields fields;

    internal LockRequest(Loan loan, LockRequestLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    public override LogEntryType EntryType => LogEntryType.LockRequest;

    public DateTime Date => this.logItem.DateTimeRequested;

    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    public object ExpirationDate
    {
      get => this.IsExtension ? this.Fields["3364"].Value : this.Fields["2151"].Value;
    }

    public object ExtensionExpirationDate
    {
      get
      {
        return !(this.logItem.BuySideNewLockExtensionDate == DateTime.MinValue) ? (object) this.logItem.BuySideNewLockExtensionDate : (object) null;
      }
    }

    public object BuySideExpirationDate
    {
      get
      {
        return !(this.logItem.BuySideExpirationDate == DateTime.MinValue) ? (object) this.logItem.BuySideExpirationDate : (object) null;
      }
      set
      {
        this.logItem.BuySideExpirationDate = value == null ? DateTime.MinValue : Convert.ToDateTime(value);
      }
    }

    public bool IsExtension => this.logItem.IsLockExtension;

    public bool IsRelock => this.logItem.IsRelock;

    public int BuySideExtensionDays => this.logItem.BuySideNumDayExtended;

    public string RequestedBy => this.logItem.RequestedBy ?? "";

    public LockRequestLog LockRequestLog => this.logItem;

    public LockRequestStatus Status
    {
      get
      {
        if (this.logItem.RequestedStatus == "")
          return LockRequestStatus.Pending;
        if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString((RateLockRequestStatus) 6), true) == 0)
          return LockRequestStatus.Denied;
        if (this.SellSideExpirationDate != null && Convert.ToDateTime(this.SellSideExpirationDate) < DateTime.Today)
        {
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString((RateLockRequestStatus) 1), true) == 0)
            return LockRequestStatus.RateExpired;
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString((RateLockRequestStatus) 4), true) == 0)
            return LockRequestStatus.LockExpired;
        }
        if (this.ExpirationDate != null && Convert.ToDateTime(this.ExpirationDate) < DateTime.Today)
        {
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString((RateLockRequestStatus) 1), true) == 0)
            return LockRequestStatus.RateExpired;
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString((RateLockRequestStatus) 4), true) == 0)
            return LockRequestStatus.LockExpired;
        }
        return (LockRequestStatus) RateLockEnum.GetRateLockEnum(this.logItem.RequestedStatus);
      }
    }

    public object SellSideDeliveryDate
    {
      get
      {
        return !(this.logItem.SellSideDeliveryDate == DateTime.MinValue) ? (object) this.logItem.SellSideDeliveryDate : (object) null;
      }
      set
      {
        this.logItem.SellSideDeliveryDate = value == null ? DateTime.MinValue : Convert.ToDateTime(value);
      }
    }

    public object SellSideExpirationDate
    {
      get
      {
        return !(this.logItem.SellSideExpirationDate == DateTime.MinValue) ? (object) this.logItem.SellSideExpirationDate : (object) null;
      }
      set
      {
        this.logItem.SellSideExpirationDate = value == null ? DateTime.MinValue : Convert.ToDateTime(value);
      }
    }

    public LockConfirmation Confirmation
    {
      get
      {
        if (this.Status == LockRequestStatus.Pending)
          return (LockConfirmation) null;
        if (this.confirmation == null)
        {
          foreach (LockConfirmation lockConfirmation in (LoanLogEntryCollection) this.Loan.Log.LockConfirmations)
          {
            if (lockConfirmation.LockRequest == this && (this.confirmation == null || lockConfirmation.Date > this.confirmation.Date))
              this.confirmation = lockConfirmation;
          }
        }
        return this.confirmation;
      }
    }

    public LockDenial Denial
    {
      get
      {
        if (this.Status != LockRequestStatus.Denied)
          return (LockDenial) null;
        if (this.denial == null)
        {
          foreach (LockDenial lockDenial in (LoanLogEntryCollection) this.Loan.Log.LockDenials)
          {
            if (lockDenial.LockRequest == this)
            {
              this.denial = lockDenial;
              break;
            }
          }
        }
        return this.denial;
      }
    }

    public LockRequestFields Fields
    {
      get
      {
        if (this.fields == null)
          this.fields = new LockRequestFields(this, this.logItem.GetLockRequestSnapshot());
        return this.fields;
      }
    }

    public bool IsActive()
    {
      return this.Status == LockRequestStatus.Pending || this.Status == LockRequestStatus.RateLocked;
    }

    public void Lock()
    {
      this.EnsureValid();
      if (this.Status == LockRequestStatus.RateLocked)
        return;
      if (this.logItem.LockRequestStatus != 1)
        throw new InvalidOperationException("The current rate lock request is not active");
      this.Fields.CommitChanges();
      this.Loan.Unwrap().LockRateRequest(this.logItem, this.Fields.FieldTable, (UserInfo) null, false, false, true, (LoanDataMgr.LockLoanSyncOption) 1, true, true, false, (List<string>) null, true);
    }

    public LockConfirmation Confirm() => this.Confirm((User) null);

    public LockConfirmation Confirm(User confirmingUser)
    {
      this.EnsureValid();
      if (confirmingUser == null)
        confirmingUser = this.Loan.Session.GetCurrentUser();
      this.Fields.CommitChanges();
      this.Loan.Unwrap().LockRateRequest(this.logItem, this.Fields.FieldTable, confirmingUser.Unwrap(), true, false, true, (LoanDataMgr.LockLoanSyncOption) 1, true, true, false, (List<string>) null, true);
      this.confirmation = (LockConfirmation) null;
      return this.Confirmation;
    }

    public LockDenial Deny() => this.Deny((User) null);

    public LockDenial Deny(User denyingUser)
    {
      this.EnsureValid();
      if (this.Status == LockRequestStatus.Denied)
        throw new InvalidOperationException("This request has already been denied");
      if (!this.IsActive())
        throw new InvalidOperationException("The current rate lock request is not active");
      if (denyingUser == null)
        denyingUser = this.Loan.Session.GetCurrentUser();
      this.Fields.CommitChanges();
      this.Loan.Unwrap().DenyRateRequest(this.logItem, this.Fields.FieldTable, denyingUser.Unwrap(), "");
      return this.Denial;
    }

    public LockRequest Extend(int daysToExtend, Decimal priceAdjustment, string comments)
    {
      return this.Extend((User) null, daysToExtend, priceAdjustment, comments);
    }

    public LockRequest Extend(
      User extendingUser,
      int daysToExtend,
      Decimal priceAdjustment,
      string comments)
    {
      LockUtils.ValidateLockExtension(this.Loan.Session.SessionObjects, this.Loan.Unwrap(), daysToExtend, priceAdjustment);
      LockExtensionUtils lockExtensionUtils = new LockExtensionUtils(this.Loan.Session.SessionObjects, this.Loan.LoanData);
      DateTime date = this.Fields["2220"].ToDate();
      if (date == DateTime.MinValue)
        date = this.Fields["2149"].ToDate();
      if (date == DateTime.MinValue)
        date = this.Fields["2089"].ToDate();
      if (date == DateTime.MinValue)
        date = this.Fields["3664"].ToDate();
      DateTime dateTime = this.Fields["3358"].ToDate();
      if (dateTime == DateTime.MinValue)
        dateTime = this.Fields["3369"].ToDate();
      if (dateTime == DateTime.MinValue)
        dateTime = Convert.ToDateTime(this.ExpirationDate);
      if (!lockExtensionUtils.IsLockExtensionEnabled())
        throw new Exception("Lock Extensions are not permitted by policy.");
      int currentExtNumber = this.Loan.LoanData.GetLogList().GetCurrentExtNumber();
      if (lockExtensionUtils.IsCompanyControlled)
      {
        if (daysToExtend == 0)
          throw new Exception("Days to Extend cannot be 0");
        if (!lockExtensionUtils.HasPriceAdjustment(daysToExtend) || lockExtensionUtils.GetPriceAdjustment(daysToExtend) != priceAdjustment)
          throw new Exception("Days to Extend and Price Adjustment does not match company lock extension policies");
      }
      else if (lockExtensionUtils.IsCompanyControlledOccur)
      {
        if (daysToExtend == 0)
        {
          daysToExtend = lockExtensionUtils.GetExtensionDays(currentExtNumber + 1);
          if (daysToExtend == 0)
            throw new Exception("This loan has reached the maximum number of times allowed to be extended.");
        }
        if (priceAdjustment == 0M)
          priceAdjustment = lockExtensionUtils.GetPriceAdjustmentOccur(currentExtNumber + 1);
        if (lockExtensionUtils.GetExtensionDays(currentExtNumber + 1) != daysToExtend || lockExtensionUtils.GetPriceAdjustmentOccur(currentExtNumber + 1) != priceAdjustment)
          throw new Exception("Days to Extend and Price Adjustment does not match company lock extension policies");
      }
      else if (daysToExtend == 0)
        throw new Exception("Days to Extend cannot be 0");
      if (extendingUser == null)
        extendingUser = this.Loan.Session.GetCurrentUser();
      if (!lockExtensionUtils.IsCompanyControlledOccur && lockExtensionUtils.IfExceedNumExtensionsLimit(currentExtNumber + 1))
        throw new Exception("This loan has reached the maximum number of times allowed to be extended.");
      if (!lockExtensionUtils.IsCompanyControlledOccur && lockExtensionUtils.MaxDaysToExtend(dateTime, date) < daysToExtend)
        throw new Exception("This request exceeds the maximum number of days allowed for an extension.");
      DateTime extensionExpirationDate = lockExtensionUtils.GetExtensionExpirationDate(Convert.ToDateTime(this.ExpirationDate), daysToExtend);
      int days = (Convert.ToDateTime(this.ExpirationDate) - date).Days;
      if (lockExtensionUtils.IfExceedsCapDays(date, extensionExpirationDate))
        throw new Exception("This loan has reached the maximum number of total lock days");
      if (priceAdjustment == 0M && lockExtensionUtils.IsCompanyControlled)
        priceAdjustment = lockExtensionUtils.GetPriceAdjustment(Convert.ToDateTime(this.ExpirationDate), daysToExtend);
      return (LockRequest) this.Loan.Log.Find((LogRecordBase) this.Loan.Unwrap().CreateExtendedRateLockRequest(extendingUser.Unwrap(), daysToExtend, extensionExpirationDate, priceAdjustment, comments), false);
    }
  }
}
