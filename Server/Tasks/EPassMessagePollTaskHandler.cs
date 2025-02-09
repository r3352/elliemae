// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.EPassMessagePollTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  internal class EPassMessagePollTaskHandler : ITaskHandler
  {
    private const string className = "EPassMessagePollTaskHandler�";
    private static TimeSpan requestTimeout = TimeSpan.FromSeconds(20.0);

    public void ProcessTask(ServerTask taskInfo)
    {
      TraceLog.WriteInfo(nameof (EPassMessagePollTaskHandler), "Preparing request for ePASS data servers.");
      Thread.Sleep(Math.Abs(ClientContext.GetCurrent().ClientID.GetHashCode() % 30) * 1000);
      EPassMessageServerResponse resp = this.queryEPassMessageServer();
      if (resp == null)
        return;
      this.postMessages(resp.GetMessages());
      this.clearReadMessages(resp.GetReadMessageIDs());
      this.deleteExpiredMessages(resp.GetFirstMessageDate());
      this.saveNextRequestParameters(resp);
    }

    private void saveNextRequestParameters(EPassMessageServerResponse resp)
    {
      try
      {
        Company.SetCompanySetting("EPASSMSGS", "LastMsgID", resp.GetLastMessageID());
        Company.SetCompanySetting("EPASSMSGS", "LastReadMsgDate", resp.GetLastReadMessageDate());
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (EPassMessagePollTaskHandler), "Error reading response parameters from ePASS Message Server: " + (object) ex);
      }
    }

    private void postMessages(EPassMessageInfo[] messages)
    {
      List<EPassMessageInfo> epassMessageInfoList = new List<EPassMessageInfo>();
      foreach (EPassMessageInfo message1 in messages)
      {
        if (message1.LoanGuid != "" && !LoanStore.GetLatestVersion(message1.LoanGuid).Exists)
        {
          TraceLog.WriteWarning(nameof (EPassMessagePollTaskHandler), "Message received for unknown loan Guid '" + message1.LoanGuid + "'. Skipping message.");
        }
        else
        {
          bool message2 = EPassMessages.CreateMessage(message1);
          TimeSpan timeSpan = DateTime.Now - EllieMae.EMLite.Common.TimeZoneInfo.Convert(EPassUtils.TimeZoneInfo, EllieMae.EMLite.Common.TimeZoneInfo.CurrentTimeZone, message1.Timestamp);
          if (message2 && timeSpan.TotalHours < 3.0)
          {
            this.createUserNotification(message1);
            epassMessageInfoList.Add(message1);
          }
        }
      }
    }

    [PgReady]
    private void clearReadMessages(string[] messageIds)
    {
      foreach (string messageId in messageIds)
        EPassMessages.DeleteMessage(messageId);
    }

    private void deleteExpiredMessages(DateTime firstMsgDate)
    {
      if (firstMsgDate == DateTime.MinValue)
        return;
      EPassMessages.DeleteMessagesBefore(firstMsgDate);
    }

    private void createUserNotification(EPassMessageInfo msg)
    {
      if ((msg.UserID ?? "") != "")
      {
        UserNotifications.Send((UserNotification) new EPassMessageNotification(msg.UserID, msg));
      }
      else
      {
        if (!((msg.LoanGuid ?? "") != ""))
          return;
        this.sendNotificationsToLoanAssociates(msg);
      }
    }

    private void sendNotificationsToLoanAssociates(EPassMessageInfo msg)
    {
      LoanAssociateInfo[] loanAssociateInfoArray = (LoanAssociateInfo[]) null;
      using (Loan latestVersion = LoanStore.GetLatestVersion(msg.LoanGuid))
      {
        if (!latestVersion.Exists)
          return;
        loanAssociateInfoArray = latestVersion.GetLoanAssociates(false, true);
      }
      foreach (LoanAssociateInfo loanAssociateInfo in loanAssociateInfoArray)
      {
        if ((loanAssociateInfo.AssociateUserID ?? "") != "")
          UserNotifications.Send((UserNotification) new EPassMessageNotification(loanAssociateInfo.AssociateUserID, msg));
      }
    }

    private EPassMessageServerResponse queryEPassMessageServer()
    {
      CompanyInfo companyInfo = Company.GetCompanyInfo();
      string companySetting1 = Company.GetCompanySetting("EPASSMSGS", "LastMsgID");
      string companySetting2 = Company.GetCompanySetting("EPASSMSGS", "LastReadMsgDate");
      try
      {
        return EPassMessageServerResponse.QueryServer(companyInfo.ClientID, companySetting1, companySetting2, false, EPassMessagePollTaskHandler.requestTimeout, EnConfigurationSettings.AppSettings["ePassGetMessageAlertsUrl"]);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (EPassMessagePollTaskHandler), "Error polling ePASS Message Server: " + (object) ex);
        throw;
      }
    }
  }
}
