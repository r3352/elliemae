// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.TPONotificationQueue
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public static class TPONotificationQueue
  {
    private const string className = "TPONotificationQueue";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static string clientID = (string) null;
    private static Thread processingThread = (Thread) null;
    private static Queue<string> loanQueue = new Queue<string>();

    public static void ClearMessages(string loanGuid, bool async)
    {
      TPONotificationQueue.initializeQueue();
      if (async)
      {
        TPONotificationQueue.enqueueLoan(loanGuid);
      }
      else
      {
        TPONotificationQueue.clearWebCenterNotifications(loanGuid);
        EPassMessages.SyncReadMessages(false);
      }
    }

    private static void initializeQueue()
    {
      lock (typeof (TPONotificationQueue))
      {
        if (TPONotificationQueue.clientID == null)
          TPONotificationQueue.clientID = Session.CompanyInfo.ClientID;
        if (TPONotificationQueue.processingThread != null)
          return;
        TPONotificationQueue.processingThread = new Thread(new ThreadStart(TPONotificationQueue.processLoanQueue));
        TPONotificationQueue.processingThread.IsBackground = true;
        TPONotificationQueue.processingThread.Priority = ThreadPriority.BelowNormal;
        TPONotificationQueue.processingThread.Start();
      }
    }

    private static void enqueueLoan(string loanGuid)
    {
      lock (TPONotificationQueue.loanQueue)
      {
        if (TPONotificationQueue.loanQueue.Contains(loanGuid))
          return;
        TPONotificationQueue.loanQueue.Enqueue(loanGuid);
        Monitor.Pulse((object) TPONotificationQueue.loanQueue);
      }
    }

    private static void processLoanQueue()
    {
      try
      {
        Tracing.Log(TPONotificationQueue.sw, nameof (TPONotificationQueue), TraceLevel.Info, "Started TPO notification thread.");
        while (true)
        {
          bool flag;
          do
          {
            string loanGuid = (string) null;
            lock (TPONotificationQueue.loanQueue)
            {
              while (TPONotificationQueue.loanQueue.Count == 0)
                Monitor.Wait((object) TPONotificationQueue.loanQueue);
              loanGuid = TPONotificationQueue.loanQueue.Dequeue();
            }
            TPONotificationQueue.clearWebCenterNotifications(loanGuid);
            flag = false;
            lock (TPONotificationQueue.loanQueue)
            {
              if (TPONotificationQueue.loanQueue.Count == 0)
                flag = true;
            }
          }
          while (!flag);
          EPassMessages.SyncReadMessages(false);
        }
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        Tracing.Log(TPONotificationQueue.sw, nameof (TPONotificationQueue), TraceLevel.Error, "TPO Notifications processing thread terminated due to exception: " + (object) ex);
      }
    }

    private static void clearWebCenterNotifications(string loanGuid)
    {
      try
      {
        string messageXml = TPONotificationQueue.createMessageXml(loanGuid);
        using (EMSiteWebService emSiteWebService = new EMSiteWebService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CenterwiseServicesUrl))
        {
          emSiteWebService.Timeout = 10000;
          emSiteWebService.ClearTPONotifications(messageXml);
        }
        Tracing.Log(TPONotificationQueue.sw, nameof (TPONotificationQueue), TraceLevel.Info, "Cleared TPO Notifications for loan '" + loanGuid + "'");
      }
      catch (Exception ex)
      {
        Tracing.Log(TPONotificationQueue.sw, nameof (TPONotificationQueue), TraceLevel.Warning, "Failed to clear TPO Notifications for loan '" + loanGuid + "': " + (object) ex);
      }
    }

    private static string createMessageXml(string loanGuid)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<Message />");
      ElementWriter elementWriter = new ElementWriter(xmlDocument.DocumentElement);
      elementWriter.Append("ClientID", TPONotificationQueue.clientID);
      elementWriter.Append("LoanGuid", loanGuid);
      return xmlDocument.OuterXml;
    }

    public static bool IsTPOWebCenterLoan(LoanData loan)
    {
      return loan.GetSimpleField("2024") == "WebCenter-TPO";
    }
  }
}
