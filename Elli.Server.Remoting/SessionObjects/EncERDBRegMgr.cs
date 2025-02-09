// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.EncERDBRegMgr
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Belikov.GenuineChannels;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ReportingDbUtils.Interfaces;
using EllieMae.EMLite.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class EncERDBRegMgr : SessionBoundObject, IEncERDBRegMgr
  {
    private const string className = "EncERDBRegMgr";

    public EncERDBRegMgr Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (EncERDBRegMgr).ToLower());
      return this;
    }

    public virtual Dictionary<string, string> CheckConnections(
      string erdbAppServer,
      int erdbAppServerPort)
    {
      this.onApiCalled(nameof (EncERDBRegMgr), nameof (CheckConnections), Array.Empty<object>());
      this.Session.SecurityManager.DemandSuperAdministrator();
      string encompassSystemId = this.Session.Context.EncompassSystemID;
      string clientId = this.Session.Context.ClientID;
      return this.getERDBRegistrationMgr(erdbAppServer, erdbAppServerPort).GetERDBSession(clientId, encompassSystemId).CheckConnections();
    }

    public virtual Dictionary<string, object> GetERDBRegistrationInfo()
    {
      this.onApiCalled(nameof (EncERDBRegMgr), nameof (GetERDBRegistrationInfo), Array.Empty<object>());
      this.Session.SecurityManager.DemandSuperAdministrator();
      return ERDBSession.GetERDBRegistrationInfo(this.Session.Context);
    }

    public virtual string Register(
      string saPwd,
      string erdbAppServer,
      int erdbAppServerPort,
      string erdbServer,
      string erdbName,
      string erdbLogin,
      string erdbPwd,
      string encDataDir,
      string smtpServer,
      int smtpPort,
      string smtpUserName,
      string smtpPwd,
      bool smtpUseSSL,
      string fromEmail,
      string toEmail,
      int emailDeliveryInterval)
    {
      try
      {
        this.onApiCalled(nameof (EncERDBRegMgr), nameof (Register), Array.Empty<object>());
        this.Session.SecurityManager.DemandSuperAdministrator();
        return this.register(saPwd, erdbAppServer, erdbAppServerPort, erdbServer, erdbName, erdbLogin, erdbPwd, encDataDir, smtpServer, smtpPort, smtpUserName, smtpPwd, smtpUseSSL, fromEmail, toEmail, emailDeliveryInterval);
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    private string register(
      string saPwd,
      string erdbAppServer,
      int erdbAppServerPort,
      string erdbServer,
      string erdbName,
      string erdbLogin,
      string erdbPwd,
      string encDataDir,
      string smtpServer,
      int smtpPort,
      string smtpUserName,
      string smtpPwd,
      bool smtpUseSSL,
      string fromEmail,
      string toEmail,
      int emailDeliveryInterval)
    {
      string encompassSystemId = this.Session.Context.EncompassSystemID;
      string clientId = this.Session.Context.ClientID;
      try
      {
        string encDbConnString = EnConfigurationSettings.GlobalSettings.DatabaseConnectionString;
        int startIndex = encDbConnString.ToLower().IndexOf("(local)");
        if (startIndex > 0)
        {
          string oldValue = encDbConnString.Substring(startIndex, "(local)".Length);
          try
          {
            encDbConnString = encDbConnString.Replace(oldValue, Dns.GetHostEntry("127.0.0.1").HostName);
          }
          catch
          {
            encDbConnString = encDbConnString.Replace(oldValue, Dns.GetHostName());
          }
        }
        string str = this.getERDBRegistrationMgr(erdbAppServer, erdbAppServerPort).Register(clientId, encompassSystemId, saPwd, erdbServer, erdbName, erdbLogin, erdbPwd, encDataDir, encDbConnString, this.Session.GetSessionStartupInfo().ServiceUrls?.ERDBWebServiceUrl);
        if (str != null)
          return str;
        ERDBSession.UpdateERDBRegistrationInfo(this.Session.Context, clientId, erdbAppServer, erdbAppServerPort, encDataDir, erdbServer, erdbName, erdbLogin, erdbPwd);
        this.UpdateNotificationSettings(smtpServer, smtpPort, smtpUserName, smtpPwd, smtpUseSSL, fromEmail, toEmail, emailDeliveryInterval);
      }
      catch (GenuineExceptions.DestinationIsUnreachable ex)
      {
        this.Session.Context.ResetRemotingInterfaces();
        return ex.Message + "\r\n\r\nPlease make sure that the ERDB Application Server is up and running.";
      }
      catch (GenuineExceptions.CanNotConnectToRemoteHost ex)
      {
        this.Session.Context.ResetRemotingInterfaces();
        return ex.Message + "\r\n\r\nPlease make sure that the ERDB Application Server is up and running.";
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
      return (string) null;
    }

    private IERDBRegistrationMgr getERDBRegistrationMgr(string erdbServer, int erdbServerPort)
    {
      return (IERDBRegistrationMgr) Activator.GetObject(typeof (IERDBRegistrationMgr), "gtcp://" + erdbServer + ":" + (object) erdbServerPort + "/RegistrationManager.rem", (object) null);
    }

    public virtual void UpdateNotificationSettings(
      string smtpServer,
      int smtpServerPort,
      string smtpUserName,
      string smtpPassword,
      bool smtpUseSSL,
      string fromEmail,
      string toEmail,
      int emailDeliveryInterval)
    {
      this.onApiCalled(nameof (EncERDBRegMgr), nameof (UpdateNotificationSettings), Array.Empty<object>());
      this.Session.SecurityManager.DemandSuperAdministrator();
      this.Session.Context.Settings.SetServerSettings((IDictionary) new Dictionary<string, string>()
      {
        {
          ERDBSession.failureNotification + ".SMTPServer",
          (smtpServer ?? "").Trim()
        },
        {
          ERDBSession.failureNotification + ".SMTPPort",
          string.Concat((object) smtpServerPort)
        },
        {
          ERDBSession.failureNotification + ".SMTPUserName",
          (smtpUserName ?? "").Trim()
        },
        {
          ERDBSession.failureNotification + ".SMTPPassword",
          XT.ESB64((smtpPassword ?? "").Trim(), KB.KB64)
        },
        {
          ERDBSession.failureNotification + ".SMTPUseSSL",
          smtpUseSSL ? "True" : "False"
        },
        {
          ERDBSession.failureNotification + ".FromEmail",
          (fromEmail ?? "").Trim()
        },
        {
          ERDBSession.failureNotification + ".Email",
          (toEmail ?? "").Trim()
        },
        {
          ERDBSession.failureNotification + ".EmailDeliveryInterval",
          string.Concat((object) emailDeliveryInterval)
        }
      });
    }
  }
}
