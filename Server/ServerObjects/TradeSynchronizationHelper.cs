// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TradeSynchronizationHelper
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.Deferrables;
using EllieMae.EMLite.Server.ServerObjects.Deferrables.BatchJobRuntime.TradeSynchronization;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class TradeSynchronizationHelper
  {
    private static string className = nameof (TradeSynchronizationHelper);

    public static int Assign(
      TradeInfoObj tradeInfo,
      List<TradeAssignmentItem> items,
      List<string> skipFieldList,
      UserInfo userInfo,
      string applicationId,
      string serviceId,
      string instanceId,
      string siteId,
      string eventId,
      BatchJobApplicationChannel applicationChannel,
      string lockLoanSyncOption = "syncLockToLoan�",
      string finalStatus = null,
      string tradeExtensionInfo = null,
      string sessionId = null)
    {
      if (items == null || items.Count == 0)
        throw new Exception("There is no item to assign to trade.");
      DeferrableDataBag<TradeSynchronizationMessage> instance1 = DeferrableDataBag<TradeSynchronizationMessage>.GetInstance();
      try
      {
        instance1.Set("ApplicationId", (object) applicationId).Set("ServiceId", (object) serviceId).Set("InstanceId", (object) instanceId).Set("DataBagKeyValueSessionId", (object) sessionId).Set("SiteId", (object) siteId).Set("EventId", (object) eventId).Set("UserId", (object) userInfo.Userid).Set("TradeInfo", (object) tradeInfo).Set("EntityList", (object) items).Set(nameof (skipFieldList), (object) skipFieldList).Set("ApplicationChannel", (object) applicationChannel).Set("UserInfo", (object) userInfo).Set("DataBagKeyValueDataSyncOption", (object) lockLoanSyncOption);
        if (!string.IsNullOrWhiteSpace(finalStatus))
          instance1.Set("DataBagKeyValueFinalStatus", (object) finalStatus);
        if (!string.IsNullOrWhiteSpace(tradeExtensionInfo))
          instance1.Set("DataBagKeyValueTradeExtensionInfo", (object) tradeExtensionInfo);
        if (!string.IsNullOrWhiteSpace(sessionId))
          instance1.Set("DataBagKeyValueSessionId", (object) sessionId);
        DeferrableMessageDeliveryTaskList instance2 = DeferrableMessageDeliveryTaskList.GetInstance();
        foreach (TradeAssignmentItem tradeAssignmentItem in items)
          instance2.Add(new DeferrableMessageDeliveryTask((IMessage) TradeSynchronizationMessage.CreateBlank(tradeAssignmentItem.EntityId, tradeAssignmentItem.Action.ToString()), "TradeSynchronization"));
        instance2.RegisterTaskHandler<TradeSynchronizationMessage>((IDeferrableMessageTaskHandler) new TradeSynchronizationMessageDeliveryTaskHandler());
        ClientContext current = ClientContext.GetCurrent();
        instance2.RunBatch<TradeSynchronizationMessage>(current, instance1);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(TradeSynchronizationHelper.className, string.Format("Trade Synchronization Error, {0}", (object) ex.Message));
      }
      int num = instance1.Get<int>("BatchJobId");
      return num > 0 ? num : -1;
    }
  }
}
