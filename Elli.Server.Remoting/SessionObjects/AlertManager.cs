// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.AlertManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class AlertManager : SessionBoundObject, IAlertManager
  {
    private const string className = "AlertManager";

    public AlertManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (AlertManager).ToLower());
      return this;
    }

    public virtual AlertConfig[] GetAlertConfigList()
    {
      this.onApiCalled(nameof (AlertManager), nameof (GetAlertConfigList), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return AlertConfigAccessor.GetAlertConfigList();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AlertManager), ex, this.Session.SessionInfo);
        return (AlertConfig[]) null;
      }
    }

    public virtual AlertConfig GetAlertConfig(int alertId)
    {
      this.onApiCalled(nameof (AlertManager), nameof (GetAlertConfig), new object[2]
      {
        (object) alertId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return AlertConfigAccessor.GetAlertConfig(alertId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AlertManager), ex, this.Session.SessionInfo);
        return (AlertConfig) null;
      }
    }

    public virtual AlertConfig GetAlertConfigByName(string alertName)
    {
      this.onApiCalled(nameof (AlertManager), nameof (GetAlertConfigByName), new object[2]
      {
        (object) alertName,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return AlertConfigAccessor.GetAlertConfigByName(alertName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AlertManager), ex, this.Session.SessionInfo);
        return (AlertConfig) null;
      }
    }

    public virtual int UpdateAlertConfig(AlertConfig alertConfig)
    {
      this.onApiCalled(nameof (AlertManager), nameof (UpdateAlertConfig), new object[1]
      {
        (object) alertConfig
      });
      try
      {
        int id = AlertConfigAccessor.UpdateAlertConfig(alertConfig);
        FieldSearchDbAccessor.UpdateFieldSearchInfo(new FieldSearchRule(alertConfig, id));
        return id;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AlertManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void DeleteAlertConfig(AlertConfig alertConfig)
    {
      this.onApiCalled(nameof (AlertManager), nameof (DeleteAlertConfig), new object[1]
      {
        (object) alertConfig.AlertID
      });
      try
      {
        PipelineInfo.Alert[] loanAlertsByAlertId = AlertConfigAccessor.GetLoanAlertsByAlertId(alertConfig.AlertID);
        Dictionary<string, Milestone> dictionary = WorkflowBpmDbAccessor.GetMilestones(true).ToDictionary<Milestone, string>((Func<Milestone, string>) (m => m.MilestoneID));
        User latestVersion = UserStore.GetLatestVersion(this.Session.UserID);
        AlertConfigAccessor.DeleteAlertConfig(alertConfig.AlertID);
        FieldSearchDbAccessor.DeleteFieldSearchInfo(alertConfig.AlertID, FieldSearchRuleType.Alerts);
        foreach (PipelineInfo.Alert alert in loanAlertsByAlertId)
        {
          AlertChange alertChange = AlertChange.GetAlertChange(alert, alertConfig, EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts.AlertStatus.Cleared, dictionary, latestVersion.UserInfo.userName);
          Loan.PublishLoanAlertChangeKafkaEvent(alert.LoanGuid, latestVersion.UserInfo.Userid, DateTime.Now, new List<AlertChange>()
          {
            alertChange
          });
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AlertManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool AddMsToMsFinishedAlertList(string milestoneID)
    {
      this.onApiCalled(nameof (AlertManager), nameof (AddMsToMsFinishedAlertList), new object[1]
      {
        (object) milestoneID
      });
      try
      {
        return AlertConfigAccessor.AddMsToMsFinishedAlertList(milestoneID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AlertManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool DeleteMsFromMsFinishedAlertList(string milestoneID)
    {
      this.onApiCalled(nameof (AlertManager), nameof (DeleteMsFromMsFinishedAlertList), new object[1]
      {
        (object) milestoneID
      });
      try
      {
        return AlertConfigAccessor.DeleteMsFromMsFinishedAlertList(milestoneID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AlertManager), ex, this.Session.SessionInfo);
        return false;
      }
    }
  }
}
