// Decompiled with JetBrains decompiler
// Type: com.elliemae.services.eventbus.models.Enums
// Assembly: com.elliemae.services.eventbus.models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 9B148EFB-427E-4DF5-8EA2-5C9491D22624
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\com.elliemae.services.eventbus.models.dll

using System.ComponentModel;

#nullable disable
namespace com.elliemae.services.eventbus.models
{
  public static class Enums
  {
    public enum Category
    {
      [Description("task")] TASK,
      [Description("event")] EVENT,
    }

    public enum Source
    {
      [Description("urn:elli:service:svc-orchestration")] URN_ELLI_SERVICE_SVC_ORCHESTRATION,
      [Description("urn:elli:service:epc")] URN_ELLI_SERVICE_EPC,
      [Description("urn:elli:service:ebs")] URN_ELLI_SERVICE_EBS,
      [Description("urn:elli:service:sdk")] URN_ELLI_SERVICE_SDK,
      [Description("urn:elli:service:encompass")] URN_ELLI_SERVICE_ENCOMPASS,
    }

    public enum Type
    {
      [Description("Order.Failed")] ORDER_FAILED,
      [Description("Condition.ClosingDate.Violation")] CONDITION_CLOSING_DATE_VIOLATION,
      [Description("Condition.Escrow.Disbursement.Due")] CONDITION_ESCROW_DISBURSEMENT_DUE,
      [Description("Condition.Mailing.Due")] CONDITION_MAILING_DUE,
      [Description("Condition.Preliminary.Expected")] CONDITION_PRELIMINARY_EXPECTED,
      [Description("Condition.Underwriting.Expected")] CONDITION_UNDERWRITING_EXPECTED,
      [Description("Consent.Accepted")] CONSENT_ACCEPTED,
      [Description("Consent.Declined")] CONSENT_DECLINED,
      [Description("Consent.NotReceived")] CONSENT_NOT_RECEIVED,
      [Description("Document.EstimateToExpire")] DOCUMENT_ESTIMATE_TO_EXPIRE,
      [Description("Document.Expected")] DOCUMENT_EXPECTED,
      [Description("Document.Expired")] DOCUMENT_EXPIRED,
      [Description("Document.FaxReceived")] DOCUMENT_FAX_RECEIVED,
      [Description("Document.GFE.Expires")] DOCUMENT_GFE_EXPIRES,
      [Description("Document.HUD-1.Violated")] DOCUMENT_HUD_1_VIOLATED,
      [Description("Document.InitialDisclosureSend")] DOCUMENT_INITIAL_DISCLOSURE_SEND,
      [Description("Document.NotViewed")] DOCUMENT_NOT_VIEWED,
      [Description("Document.Redisclose.CD")] DOCUMENT_REDISCLOSE_CD,
      [Description("Document.Redisclose.GFE")] DOCUMENT_REDISCLOSE_GFE,
      [Description("Document.Redisclose.LE.APR.Change")] DOCUMENT_REDISCLOSE_LE_APR_CHANGE,
      [Description("Document.Redisclose.LE.Changed")] DOCUMENT_REDISCLOSE_LE_CHANGED,
      [Description("Document.Redisclose.LE.RateLocked")] DOCUMENT_REDISCLOSE_LE_RATE_LOCKED,
      [Description("Document.Signing.Completed.By.All")] DOCUMENT_SIGNING_COMPLETED_BY_ALL,
      [Description("Document.SinginCompleted")] DOCUMENT_SINGIN_COMPLETED,
      [Description("Document.Uploaded")] DOCUMENT_UPLOADED,
      [Description("Document.Viewed")] DOCUMENT_VIEWED,
      [Description("RateLock.Request.Confirmed")] RATE_LOCK_REQUEST_CONFIRMED,
      [Description("RateLock.Requested")] RATE_LOCK_REQUESTED,
      [Description("Loan.Created")] LOAN_CREATED,
      [Description("Loan.Submitted")] LOAN_SUBMITTED,
      [Description("Loan.UserAssigned")] LOAN_USER_ASSIGNED,
      [Description("DeferredLoan.EmailTrigger")] DEFERRED_LOAN_EMAIL_TRIGGER,
      [Description("DeferredLoan.StatusOnline")] DEFERRED_LOAN_STATUS_ONLINE,
      [Description("DeferredLoan.ReportingDatabase")] DEFERRED_LOAN_REPORTING_DATABASE,
      [Description("DeferredLoan.AuditTrail")] DEFERRED_LOAN_AUDIT_TRAIL,
      [Description("DeferredLoan.MilestoneEmailNotification")] DEFERRED_LOAN_MILESTONE_EMAIL_NOTIFICATION,
      [Description("DeferredLoan.AlertNotification")] DEFERRED_LOAN_ALERT_NOTIFICATION,
      [Description("DeferredLoan.ServiceOrder")] DEFERRED_LOAN_SERVICE_ORDER,
      [Description("Loan.Trade.Sync")] BATCH_JOB_LOAN_TRADE_SYNC,
      [Description("Loan.Event.Webhooks")] LOAN_EVENT_WEBHOOKS,
    }
  }
}
