// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.EmailNotificationClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.eFolder.EmailNotificationController;
using EllieMae.EMLite.eFolder.WcfExtensions;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class EmailNotificationClient
  {
    private const string className = "NotifyUsersDialog";
    private static readonly string sw = Tracing.SwEFolder;

    public EmailSetting[] EmailGet(Guid[] loanIds)
    {
      EmailNotificationControllerClient notificationClient = this.getEmailNotificationClient();
      EmailGetRequest request = new EmailGetRequest();
      request.Security = this.getSecurityHeader();
      request.LoanIds = loanIds;
      Tracing.Log(EmailNotificationClient.sw, TraceLevel.Verbose, "NotifyUsersDialog", "Calling EmailGet of EmailNotificationService");
      EmailGetResponse emailGetResponse = notificationClient.EmailGet(request);
      Tracing.Log(EmailNotificationClient.sw, TraceLevel.Verbose, "NotifyUsersDialog", "Response from  EmailGet of EmailNotificationService: " + (object) ((IEnumerable<EmailSetting>) emailGetResponse.EmailSettings).Count<EmailSetting>());
      return emailGetResponse.EmailSettings;
    }

    public int ActiveEmailCount(Guid[] loanIds)
    {
      int num = 0;
      foreach (EmailSetting emailSetting in this.EmailGet(loanIds))
      {
        DateTime dateTime = emailSetting.ValidTill;
        if (!(dateTime.Date == DateTime.MinValue))
        {
          DateTime validTill = emailSetting.ValidTill;
          dateTime = DateTime.Now;
          DateTime date = dateTime.Date;
          if (!(validTill >= date))
            continue;
        }
        ++num;
      }
      return num;
    }

    public FailedEmailSetting[] EmailSave(EmailSetting[] emailSettings)
    {
      EmailNotificationControllerClient notificationClient = this.getEmailNotificationClient();
      EmailSaveRequest request = new EmailSaveRequest();
      request.Security = this.getSecurityHeader();
      request.EmailSettings = emailSettings;
      Tracing.Log(EmailNotificationClient.sw, TraceLevel.Verbose, "NotifyUsersDialog", "Calling EmailSave of EmailNotificationService");
      EmailSaveResponse emailSaveResponse = notificationClient.EmailSave(request);
      Tracing.Log(EmailNotificationClient.sw, TraceLevel.Verbose, "NotifyUsersDialog", "EmailSave response: " + emailSaveResponse.Success.ToString());
      return emailSaveResponse.FailedEmailSettings;
    }

    private EmailNotificationControllerClient getEmailNotificationClient()
    {
      Tracing.Log(EmailNotificationClient.sw, TraceLevel.Verbose, "NotifyUsersDialog", "Initializing Service Client and Endpoint");
      BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
      basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
      string str = Session.SessionObjects?.StartupInfo?.ServiceUrls?.EmailNotificationUrl;
      if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
        str = "https://loancenter.elliemae.com/EmailNotification/EmailNotificationController.svc";
      EndpointAddress remoteAddress = new EndpointAddress(str);
      EmailNotificationControllerClient notificationClient = new EmailNotificationControllerClient((Binding) basicHttpBinding, remoteAddress);
      notificationClient.ChannelFactory.Endpoint.Behaviors.Add((IEndpointBehavior) new SsoTokenEndpointBehavior());
      return notificationClient;
    }

    private Security getSecurityHeader()
    {
      return new Security()
      {
        ClientId = Session.CompanyInfo.ClientID,
        UserId = Session.UserID,
        Password = Session.Password
      };
    }
  }
}
