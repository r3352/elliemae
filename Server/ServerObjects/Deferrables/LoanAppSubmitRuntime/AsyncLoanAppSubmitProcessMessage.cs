// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanAppSubmitRuntime.AsyncLoanAppSubmitProcessMessage
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanAppSubmitRuntime
{
  [Serializable]
  public class AsyncLoanAppSubmitProcessMessage(
    string applicationId,
    string serviceId,
    string instanceId,
    string siteId,
    string eventId,
    string userId,
    string auditUserId = null) : StandardMessage(applicationId, serviceId, instanceId, siteId, eventId, userId, auditUserId), ICloneable
  {
    public AsyncLoanAppSubmitProcessMessage.MessageActionType ActionType { get; set; }

    public string IdentityClaim { get; set; }

    public string LoanFolder { get; set; }

    public string LoanId { get; set; }

    public DateTime PublishTime { get; set; }

    public string RealmClaim { get; set; }

    public string SubjectClaim { get; set; }

    public string UserIdClaim { get; set; }

    public static AsyncLoanAppSubmitProcessMessage CreateBlank()
    {
      return new AsyncLoanAppSubmitProcessMessage("", "", "", "", "", "")
      {
        ActionType = AsyncLoanAppSubmitProcessMessage.MessageActionType.LoanApplicationSubmit
      };
    }

    public object Clone() => throw new NotImplementedException();

    public enum MessageActionType
    {
      LoanApplicationSubmit,
    }
  }
}
