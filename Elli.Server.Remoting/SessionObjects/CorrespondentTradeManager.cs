// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.CorrespondentTradeManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Cursors;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using EllieMae.EMLite.Trading;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class CorrespondentTradeManager : TradeManagerBase, ICorrespondentTradeManager
  {
    private const string className = "CorrespondentTradeManager";

    public CorrespondentTradeManager Initialize(ISession session)
    {
      this.Initialize(session, nameof (CorrespondentTradeManager).ToLower(), TradeType.CorrespondentTrade);
      return this;
    }

    public virtual List<CorrespondentTradeInfo> GetAllTrades()
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetAllTrades), Array.Empty<object>());
      try
      {
        return new List<CorrespondentTradeInfo>((IEnumerable<CorrespondentTradeInfo>) CorrespondentTrades.GetAllTrades());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (List<CorrespondentTradeInfo>) null;
      }
    }

    public virtual CorrespondentTradeInfo GetTrade(int tradeId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return CorrespondentTrades.GetTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeInfo) null;
      }
    }

    public virtual CorrespondentMasterInfo[] GetCorrespondentMasterInfos(
      CorrespondentTradeInfo tradeInfo)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetCorrespondentMasterInfos), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        return CorrespondentTrades.GetActiveMasters(tradeInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterInfo[]) null;
      }
    }

    public virtual CorrespondentTradeEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeEditorScreenData), new object[2]
      {
        (object) tradeId,
        (object) assignedLoanFields
      });
      try
      {
        CorrespondentTradeEditorScreenData editorScreenData = new CorrespondentTradeEditorScreenData();
        if (tradeId > 0)
        {
          editorScreenData.TradeHistory = CorrespondentTrades.GetTradeHistory(tradeId);
          editorScreenData.AssignedLoans = CorrespondentTrades.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, assignedLoanFields, isExternalOrganization);
          editorScreenData.AssignedContract = CorrespondentTrades.GetMasterForTrade(tradeId);
        }
        return editorScreenData;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeEditorScreenData) null;
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      CorrespondentTradeLoanStatus status)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (CommitPendingTradeStatus), new object[3]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (CorrespondentTradeManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == CorrespondentTradeLoanStatus.None)
        Err.Raise(nameof (CorrespondentTradeManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        CorrespondentTrades.CommitPendingTradeStatus(tradeId, loanGuid, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      CorrespondentTradeLoanStatus status,
      bool rejected)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (CommitPendingTradeStatus), new object[4]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status,
        (object) rejected
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (CorrespondentTradeManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == CorrespondentTradeLoanStatus.None)
        Err.Raise(nameof (CorrespondentTradeManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        CorrespondentTrades.CommitPendingTradeStatus(tradeId, loanGuid, status, rejected);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int CreateTrade(CorrespondentTradeInfo tradeInfo)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (CreateTrade), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        return CorrespondentTrades.CreateTrade(tradeInfo, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateTrade(CorrespondentTradeInfo tradeInfo, bool checkStatus)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (UpdateTrade), new object[2]
      {
        (object) tradeInfo,
        (object) checkStatus
      });
      try
      {
        CorrespondentTrades.UpdateTrade(tradeInfo, this.Session.GetUserInfo(), false, checkStatus);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void PublishKafkaEvent(string eventType, int tradeId, Hashtable eventPayload)
    {
      int serverMode = (int) EncompassServer.ServerMode;
      WebhookEventContract webhookEvent = new WebhookEventContract();
      webhookEvent.UserId = this.Session.SessionInfo?.UserID;
      webhookEvent.InstanceId = this.Session.SessionInfo?.Server?.InstanceName ?? "LOCALHOST";
      webhookEvent.EventType = eventType;
      webhookEvent.ResourceId = tradeId.ToString();
      try
      {
        WebhookEventConsts webhookEventConsts = new WebhookEventConsts(WebhookResource.trade);
        webhookEvent.ResourceType = webhookEventConsts.EventResourceName;
        webhookEvent.ResourceRef = webhookEventConsts.ResourceRef + webhookEvent.ResourceId;
        webhookEvent.Source = "urn:elli:encompass:" + webhookEvent.InstanceId;
        string clientId = Company.GetCompanyInfo().ClientID;
        TradeChangeEvent tradeChangeEvent = new TradeChangeEvent(webhookEvent.InstanceId, Convert.ToString(tradeId), webhookEvent.UserId, true);
        switch (eventType)
        {
          case "publish":
            CorrespondentTradeInfo trade = this.GetTrade(tradeId);
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
              DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
            };
            webhookEvent.AddExpoObject("lastPublishedDateTime", (object) JsonConvert.SerializeObject((object) trade.LastPublishedDateTime, settings));
            break;
          case "create":
            webhookEvent.payload = (object) new ExpandoObject();
            break;
          case "update":
            if (eventPayload != null)
            {
              IEnumerator enumerator = eventPayload.Keys.GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  string current = (string) enumerator.Current;
                  webhookEvent.AddExpoObject(current, eventPayload[(object) current]);
                }
                break;
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
            }
            else
              break;
          case "loanAssignmentComplete":
            if (eventPayload != null)
            {
              IEnumerator enumerator = eventPayload.Keys.GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  string current = (string) enumerator.Current;
                  webhookEvent.AddExpoObject("loanId", (object) current);
                  webhookEvent.AddExpoObject("failedReason", (object) eventPayload[(object) current].ToString());
                }
                break;
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
            }
            else
              break;
        }
        tradeChangeEvent.AddKafkaMessage(Convert.ToString(tradeId), webhookEvent.InstanceId, clientId, webhookEvent.UserId, eventType, webhookEvent);
        if (!tradeChangeEvent.QueueMessages.Any<QueueMessage>())
          return;
        MessageQueueEventService queueEventService = new MessageQueueEventService();
        IMessageQueueProcessor messageQueueProcessor = (IMessageQueueProcessor) new KafkaTradeProcessor();
        TradeChangeEvent queueEvent = tradeChangeEvent;
        IMessageQueueProcessor processor = messageQueueProcessor;
        queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
      }
      catch (Exception ex)
      {
        Err.Reraise(string.Format("PublishKafkaEvent failed for event {0} on trade {1}. ", (object) eventType, (object) tradeId), ex, this.Session.SessionInfo);
      }
    }

    public virtual void PublishTrade(CorrespondentTradeInfo tradeInfo, bool checkStatus)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), "UpdateTrade", new object[2]
      {
        (object) tradeInfo,
        (object) checkStatus
      });
      try
      {
        CorrespondentTrades.PublishTrade(tradeInfo, this.Session.GetUserInfo(), false, checkStatus);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor GetEligibleLoanCursor(
      int tradeId,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new int[1]
      {
        tradeId
      }, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      int[] tradeIds,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetEligibleLoanCursor), new object[4]
      {
        (object) tradeIds,
        (object) dataToInclude,
        (object) fields,
        (object) sortOrder
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<TradeLoanCursor>().Initialize(this.Session, tradeIds, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor GetEligibleLoanCursor(
      CorrespondentTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new CorrespondentTradeInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      CorrespondentTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new CorrespondentTradeInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, guidsToOmit, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      CorrespondentTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(trades, fields, dataToInclude, sortOrder, (string[]) null, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      CorrespondentTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetEligibleLoanCursor), new object[5]
      {
        (object) trades,
        (object) fields,
        (object) dataToInclude,
        (object) sortOrder,
        (object) guidsToOmit
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<CorrespondentTradeLoanCursor>().Initialize(this.Session, trades, fields, dataToInclude, sortOrder, guidsToOmit, isExternalOrganization, filterOption);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void AddTradeHistoryItem(CorrespondentTradeHistoryItem item)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (AddTradeHistoryItem), new object[1]
      {
        (object) item
      });
      try
      {
        CorrespondentTrades.AddTradeHistoryItem(item);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (OpenTradeCursor), new object[2]
      {
        (object) criteria,
        (object) sortOrder
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<CorrespondentTradeCursor>().Initialize(this.Session, CorrespondentTrades.QueryTradeIds(this.Session.GetUserInfo(), criteria, sortOrder, isExternalOrganization));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void DeleteTrade(int tradeId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (DeleteTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        CorrespondentTrades.DeleteTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CorrespondentTradeViewModel GetTradeViewForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeViewForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return CorrespondentTrades.GetTradeViewForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeViewModel) null;
      }
    }

    public virtual CorrespondentTradeInfo GetTradeForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return CorrespondentTrades.GetTradeForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeInfo) null;
      }
    }

    public virtual CorrespondentTradeInfo GetTradeForRejectedLoan(string loanGuid)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeForRejectedLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return CorrespondentTrades.GetTradeForRejectedLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeInfo) null;
      }
    }

    public override void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (SetTradeStatus), new object[4]
      {
        (object) tradeIds,
        (object) status,
        (object) action,
        (object) needPendingCheck
      });
      try
      {
        CorrespondentTrades.SetTradeStatus(tradeIds, status, action, this.Session.GetUserInfo(), needPendingCheck);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateTradeStatus(int tradeId, TradeStatus status, bool needPendingCheck)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (UpdateTradeStatus), new object[3]
      {
        (object) tradeId,
        (object) status,
        (object) needPendingCheck
      });
      try
      {
        CorrespondentTrades.UpdateTradeStatus(tradeId, status, this.Session.GetUserInfo(), needPendingCheck);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CorrespondentTradeViewModel[] GetActiveTradeView()
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetActiveTradeView), Array.Empty<object>());
      try
      {
        return CorrespondentTrades.GetActiveTradeView();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeViewModel[]) null;
      }
    }

    public virtual CorrespondentTradeViewModel[] GetTradeViewsByMasterId(int masterId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeViewsByMasterId), new object[1]
      {
        (object) masterId
      });
      try
      {
        return CorrespondentTrades.GetTradeViewsByMasterId(masterId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeViewModel[]) null;
      }
    }

    public virtual List<CorrespondentTradeInfo> GetTradeInfosByMasterId(int masterId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeInfosByMasterId), new object[1]
      {
        (object) masterId
      });
      try
      {
        return ((IEnumerable<CorrespondentTradeInfo>) CorrespondentTrades.GetTradeInfosByMasterId(masterId)).ToList<CorrespondentTradeInfo>();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (List<CorrespondentTradeInfo>) null;
      }
    }

    public virtual List<CorrespondentTradeInfo> GetTradeInfosByExternalOrgId(int externalOrgId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeInfosByExternalOrgId), new object[1]
      {
        (object) externalOrgId
      });
      try
      {
        return ((IEnumerable<CorrespondentTradeInfo>) CorrespondentTrades.GetTradeInfosByExternalOrgId(externalOrgId)).ToList<CorrespondentTradeInfo>();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (List<CorrespondentTradeInfo>) null;
      }
    }

    public virtual Dictionary<CorrespondentMasterDeliveryType, Decimal> GetOutStandingCommitments(
      int externalOrgId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetOutStandingCommitments), new object[1]
      {
        (object) externalOrgId
      });
      try
      {
        Dictionary<CorrespondentMasterDeliveryType, Decimal> dictionary = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
        foreach (CorrespondentMasterDeliveryType key in Enum.GetValues(typeof (CorrespondentMasterDeliveryType)).Cast<CorrespondentMasterDeliveryType>())
          dictionary.Add(key, 0M);
        List<CorrespondentTradeInfo> infosByExternalOrgId = this.GetTradeInfosByExternalOrgId(externalOrgId);
        return infosByExternalOrgId != null ? CorrespondentTradeCalculation.CalculateOutStandingCommitments(infosByExternalOrgId, true, true) : dictionary;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (Dictionary<CorrespondentMasterDeliveryType, Decimal>) null;
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      CorrespondentTradeLoanStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (SetPendingTradeStatuses), new object[5]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) isExternalOrganization,
        (object) removePendingLoan
      });
      try
      {
        CorrespondentTrades.SetPendingTradeStatus(tradeId, loanGuids, statuses, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      CorrespondentTradeLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (SetPendingTradeStatuses), new object[6]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) comments,
        (object) isExternalOrganization,
        (object) removePendingLoan
      });
      try
      {
        CorrespondentTrades.SetPendingTradeStatus(tradeId, loanGuids, statuses, comments, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      CorrespondentTradeLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan,
      Decimal[] totalPrices,
      bool forceUpdateAllLoans)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (SetPendingTradeStatuses), new object[8]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) comments,
        (object) isExternalOrganization,
        (object) removePendingLoan,
        (object) totalPrices,
        (object) forceUpdateAllLoans
      });
      try
      {
        CorrespondentTrades.SetPendingTradeStatus(tradeId, loanGuids, statuses, comments, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan, totalPrices, forceUpdateAllLoans);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetAssignedOrPendingLoans), new object[2]
      {
        (object) tradeId,
        (object) fields
      });
      try
      {
        return CorrespondentTrades.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, fields, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    public virtual CorrespondentTradeInfo GetTrade(string tradeName)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTrade), new object[1]
      {
        (object) tradeName
      });
      try
      {
        return CorrespondentTrades.GetTrade(tradeName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeInfo) null;
      }
    }

    public virtual CorrespondentTradeHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeHistoryForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return CorrespondentTrades.GetTradeHistoryForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeHistoryItem[]) null;
      }
    }

    public virtual CorrespondentTradeHistoryItem[] GetTradeHistoryForTrade(int tradeId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeHistoryForTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return CorrespondentTrades.GetTradeHistory(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (CorrespondentTradeHistoryItem[]) null;
      }
    }

    public virtual bool CheckTradeByName(string name)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (CheckTradeByName), new object[1]
      {
        (object) name
      });
      try
      {
        return CorrespondentTrades.CheckTradeByName(name);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CheckExistingTradeByName(string name)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (CheckExistingTradeByName), new object[1]
      {
        (object) name
      });
      try
      {
        return CorrespondentTrades.CheckExistingTradeByName(name);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual string GetNextAutoCreateTradeName(string name, string loanGUID)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetNextAutoCreateTradeName), new object[1]
      {
        (object) name
      });
      try
      {
        return CorrespondentTrades.GetNextAutoCreateTradeName(name, loanGUID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return "";
      }
    }

    public virtual string GenerateNextAutoNumber()
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), "GetNextAutoCreateTradeName", Array.Empty<object>());
      try
      {
        return CorrespondentTrades.GenerateNextAutoNumber();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return "";
      }
    }

    public virtual Dictionary<int, string> GetEligibleCorrespondentMastersByTradeId(int tradeId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetEligibleCorrespondentMastersByTradeId), Array.Empty<object>());
      try
      {
        return CorrespondentTrades.GetCorrespondentMastersByCorrespndentTradeId(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return new Dictionary<int, string>();
      }
    }

    public virtual Dictionary<int, string> GetEligibleCorrespondentTradeByLoanInfo(
      string externalId,
      string deliveryType,
      double loanAmount)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetEligibleCorrespondentTradeByLoanInfo), Array.Empty<object>());
      try
      {
        IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
        CorrespondentMasterDeliveryType deliveryTypeEnum = TradeUtils.GetDeliveryTypeEnum(deliveryType);
        return string.Equals(configurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTTRADE"), "False", StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(externalId) || deliveryTypeEnum == CorrespondentMasterDeliveryType.None || deliveryTypeEnum == CorrespondentMasterDeliveryType.IndividualBestEfforts || deliveryTypeEnum == CorrespondentMasterDeliveryType.IndividualMandatory ? new Dictionary<int, string>() : CorrespondentTrades.GetEligibleCorrespondentTradeByLoanInfo(externalId, deliveryTypeEnum, loanAmount);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (Dictionary<int, string>) null;
      }
    }

    public virtual Dictionary<int, string> GetEligibleCorrespondentTradeByLoanNumber(
      string deliveryType,
      string loanNumber)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetEligibleCorrespondentTradeByLoanNumber), Array.Empty<object>());
      try
      {
        IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
        CorrespondentMasterDeliveryType deliveryTypeEnum = TradeUtils.GetDeliveryTypeEnum(deliveryType);
        return string.Equals(configurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTTRADE"), "False", StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(loanNumber) || deliveryTypeEnum == CorrespondentMasterDeliveryType.None || deliveryTypeEnum == CorrespondentMasterDeliveryType.IndividualBestEfforts || deliveryTypeEnum == CorrespondentMasterDeliveryType.IndividualMandatory ? new Dictionary<int, string>() : CorrespondentTrades.GetEligibleCorrespondentTradeByLoanNumber(deliveryTypeEnum, loanNumber);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return (Dictionary<int, string>) null;
      }
    }

    public virtual Dictionary<int, string> GetEligibleCorrespondentTradeByLoanNumber(
      string deliveryType,
      string loanNumber,
      string correspondentMasterNumber)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetEligibleCorrespondentTradeByLoanNumber), Array.Empty<object>());
      try
      {
        IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
        CorrespondentMasterDeliveryType deliveryTypeEnum = TradeUtils.GetDeliveryTypeEnum(deliveryType);
        return string.Equals(configurationManager.GetCompanySetting("TRADE", "ENABLECORRESPONDENTMASTER"), "False", StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(loanNumber) || deliveryTypeEnum == CorrespondentMasterDeliveryType.None || deliveryTypeEnum == CorrespondentMasterDeliveryType.IndividualBestEfforts || deliveryTypeEnum == CorrespondentMasterDeliveryType.IndividualMandatory ? new Dictionary<int, string>() : CorrespondentTrades.GetEligibleCorrespondentTradeByLoanNumber(deliveryType, loanNumber, correspondentMasterNumber);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return new Dictionary<int, string>();
      }
    }

    public virtual Decimal CalculateTPOAvailableAmount(
      MasterCommitmentType commitmentType,
      ExternalOriginatorManagementData tpoSettings)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (CalculateTPOAvailableAmount), Array.Empty<object>());
      try
      {
        Dictionary<CorrespondentMasterDeliveryType, Decimal> dictionary1 = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
        Dictionary<CorrespondentMasterDeliveryType, Decimal> dictionary2 = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
        Decimal num1 = 0M;
        Decimal num2 = 0M;
        Dictionary<CorrespondentMasterDeliveryType, Decimal> standingCommitments = this.GetOutStandingCommitments(tpoSettings.oid);
        Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommitments = ExternalOrgManagementAccessor.GetNonAllocatedOutstandingCommitments(tpoSettings.ExternalID);
        if (commitmentType == MasterCommitmentType.BestEfforts && tpoSettings.CommitmentUseBestEffortLimited)
        {
          num1 = tpoSettings.MaxCommitmentAuthority;
          if (standingCommitments.ContainsKey(CorrespondentMasterDeliveryType.IndividualBestEfforts))
            num2 += standingCommitments[CorrespondentMasterDeliveryType.IndividualBestEfforts];
          if (outstandingCommitments.ContainsKey(CorrespondentMasterDeliveryType.IndividualBestEfforts))
            num2 += outstandingCommitments[CorrespondentMasterDeliveryType.IndividualBestEfforts];
        }
        else if (commitmentType == MasterCommitmentType.Mandatory || commitmentType == MasterCommitmentType.BothMandatoryAndBestEfforts)
        {
          num1 = tpoSettings.MaxCommitmentAmount;
          foreach (CorrespondentMasterDeliveryType key in Enum.GetValues(typeof (CorrespondentMasterDeliveryType)))
          {
            if (key != CorrespondentMasterDeliveryType.IndividualBestEfforts && (key != CorrespondentMasterDeliveryType.IndividualMandatory || tpoSettings.IsCommitmentDeliveryIndividual) && (key != CorrespondentMasterDeliveryType.AOT || tpoSettings.IsCommitmentDeliveryAOT) && (key != CorrespondentMasterDeliveryType.BulkAOT || tpoSettings.IsCommitmentDeliveryBulkAOT) && (key != CorrespondentMasterDeliveryType.Bulk || tpoSettings.IsCommitmentDeliveryBulk) && (key != CorrespondentMasterDeliveryType.CoIssue || tpoSettings.IsCommitmentDeliveryCoIssue) && (key != CorrespondentMasterDeliveryType.Forwards || tpoSettings.IsCommitmentDeliveryForward) && (key != CorrespondentMasterDeliveryType.LiveTrade || tpoSettings.IsCommitmentDeliveryLiveTrade))
            {
              if (standingCommitments.ContainsKey(key))
                num2 += standingCommitments[key];
              if (outstandingCommitments.ContainsKey(key))
                num2 += outstandingCommitments[key];
            }
          }
        }
        return num1 - num2;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return 0M;
      }
    }

    public virtual void VoidAssigedPendingLoanAssignment(int tradeId, string[] loanGuids)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (VoidAssigedPendingLoanAssignment), new object[2]
      {
        (object) tradeId,
        (object) loanGuids
      });
      try
      {
        CorrespondentTrades.VoidAssigedPendingLoanAssignment(tradeId, loanGuids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool IsTradeAssignmentUpdateCompleted(
      int tradeId,
      string loanGuid,
      CorrespondentTradeLoanStatus status)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (IsTradeAssignmentUpdateCompleted), Array.Empty<object>());
      try
      {
        return CorrespondentTrades.IsTradeAssignmentUpdateCompleted(tradeId, loanGuid, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void UpdateAssignmentsWithTradeExtension(
      int tradeId,
      string[] loanGuids,
      string tradeExtensionInfo)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (UpdateAssignmentsWithTradeExtension), Array.Empty<object>());
      try
      {
        CorrespondentTrades.UpdateAssignmentsWithTradeExtension(tradeId, loanGuids, tradeExtensionInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual TradeStatus GetTradeStatus(int tradeId)
    {
      this.onApiCalled(nameof (CorrespondentTradeManager), nameof (GetTradeStatus), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return CorrespondentTrades.GetTradeStatus(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentTradeManager), ex, this.Session.SessionInfo);
        return TradeStatus.None;
      }
    }
  }
}
