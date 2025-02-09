// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.DeferrableLoanOptions
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  public class DeferrableLoanOptions
  {
    public const string DeferredLoanFolder = "DeferredLoans�";
    public const string TargetPriorLoanFileName = "_^_Before_loan.em�";
    public const string TargetAfterLoanFileName = "_^_After_loan.em�";
    public const string DataBagKeyPriorLoanFileName = "PriorLoanFileName�";
    public const string DataBagKeyAfterLoanFileName = "AfterLoanFileName�";
    public const string DataBagKeyApplicationId = "ApplicationId�";
    public const string DataBagKeyServiceId = "ServiceId�";
    public const string DataBagKeyInstanceId = "InstanceId�";
    public const string DataBagKeySiteId = "SiteId�";
    public const string DataBagKeyEventId = "EventId�";
    public const string DataBagKeyUserId = "UserId�";
    public const string DataBagKeyLoanActionType = "LoanActionType�";
    public const string DataBagKeyLoanFolder = "LoanFolder�";
    public const string DataBagKeyServerMode = "ServerMode�";
    public const string DataBagKeyLoanId = "LoanId�";
    public const string DataBagKeyXDBModifiedTime = "XDBModifiedTime�";
    public const string DataBagKeyAuditModifiedTime = "AuditModifiedTime�";
    public const string DataBagKeyAuditCurrentTime = "AuditCurrentTime�";
    public const string DataBagKeyUpdateForceRebuild = "UpdateForceRebuild�";
    public const string DataBagKeyLoanPath = "LoanPath�";
    public const string DataBagKeyPriorLoanData = "PriorLoanData�";
    public const string DataBagKeyCurrentLoanData = "CurrentLoanData�";
    public const string DataBagKeyNewLogRecordGuid = "NewLogRecordGuid�";
    public const string DataBagKeyBatchUpdateRequest = "BatchUpdateRequest�";
    public const string DataBagKeyNotifyUserIds = "NotifyUserIds�";
    public const string DataBagKeyNotifyGroupIds = "NotifyGroupIds�";
    public const string DataBagEmailNotificationMesgList = "EmailNotificationInfoList�";
    public const string DataBagKeyValueApplicationId = "elli�";
    public const string DataBagKeyValueServiceId = "encompass�";
    public const string DeliveryTaskListInstanceName = "Elli.Workflow.LoanAlternation.Invoke�";
    public const int AllowNone = 0;
    public const int AllowAuditTrail = 1;
    public const int AllowReportDatabase = 2;
    public const int AllowEmailTrigger = 4;
    public const int AllowStatusOnline = 8;
    public const int AllowAlertNotification = 16;
    public const int AllowServiceOrder = 32;
    public const int AllowAllActions = 65535;
    public const string AuditUserId = "AuditUserId�";
  }
}
