// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.AsyncProcessMessage
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  [Serializable]
  public class AsyncProcessMessage(
    string applicationId,
    string serviceId,
    string instanceId,
    string siteId,
    string eventId,
    string userId,
    string auditUserId = null) : StandardMessage(applicationId, serviceId, instanceId, siteId, eventId, userId, auditUserId), ICloneable
  {
    public MessageActionType ActionType { get; set; }

    public EncompassServerMode ServerMode { get; set; }

    public bool IsRelayed { get; set; }

    public string LoanId { get; set; }

    public string LoanFolder { get; set; }

    public string LoanPath { get; set; }

    public string PriorLoanFileName { get; set; }

    public string AfterLoanFileName { get; set; }

    public Elli.ElliEnum.LoanActionType? LoanActionType { get; set; }

    public DateTime PublishTime { get; set; }

    public DateTime AuditModifiedTime { get; set; }

    public DateTime AuditCurrentTime { get; set; }

    public DateTime XDBModifiedTime { get; set; }

    public bool UpdateForceRebuild { get; set; }

    public string NotifyUserIds { get; set; }

    public string NotifyGroupIds { get; set; }

    public string NewLogRecordGuid { get; set; }

    public string DeferredActions { get; set; }

    public object Clone()
    {
      return (object) new AsyncProcessMessage(this.ApplicationId, this.ServiceId, this.InstanceId, this.SiteId, this.EventId, this.UserId, this.AuditUserId)
      {
        ActionType = this.ActionType,
        ServerMode = this.ServerMode,
        IsRelayed = this.IsRelayed,
        LoanId = this.LoanId,
        LoanFolder = this.LoanFolder,
        LoanActionType = this.LoanActionType,
        LoanPath = this.LoanPath,
        PriorLoanFileName = this.PriorLoanFileName,
        AfterLoanFileName = this.AfterLoanFileName,
        PublishTime = this.PublishTime,
        AuditModifiedTime = this.AuditModifiedTime,
        AuditCurrentTime = this.AuditCurrentTime,
        XDBModifiedTime = this.XDBModifiedTime,
        UpdateForceRebuild = this.UpdateForceRebuild,
        NewLogRecordGuid = this.NewLogRecordGuid,
        DeferredActions = this.DeferredActions
      };
    }

    public static AsyncProcessMessage CreateBlank(MessageActionType actionType)
    {
      return new AsyncProcessMessage("", "", "", "", "", "")
      {
        ActionType = actionType
      };
    }
  }
}
