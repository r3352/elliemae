// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.UpdateCorrTradeStatusTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Tasks;
using EllieMae.EMLite.Trading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  internal class UpdateCorrTradeStatusTaskHandler : ITaskHandler
  {
    public const string WEBHOOK_EVENT_XAPI_KEY = "LPivKuC0aK6yJF3SyeExvJWK9i8agljg5s2lfVz10bamqMSsPirutSKzNCLhP0en�";
    public const string WEBHOOK_EVENT_XAPI_KEY_NAME = "x-api-key�";
    public const string WEBHOOK_EVENT_SOURCE = "urn:elli:encompass:�";
    private static readonly string _className = nameof (UpdateCorrTradeStatusTaskHandler);
    private ClientContext _ctx;

    public void ProcessTask(ServerTask taskInfo)
    {
      this._ctx = ClientContext.GetCurrent();
      DateTime now = DateTime.Now;
      this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus Start at: {0}", (object) now.ToString()));
      User latestVersion = UserStore.GetLatestVersion("admin");
      if (!latestVersion.Exists)
      {
        this._ctx.TraceLog.WriteError(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus user dosen't exist"));
      }
      else
      {
        UserInfo userInfo = latestVersion.UserInfo;
        this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus user is : {0}", (object) userInfo.FullName));
        try
        {
          this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus - UpdateCorrespondentTradeStatus at: {0}", (object) DateTime.Now.ToString()));
          List<int> intList = CorrespondentTrades.UpdateCorrespondentTradeStatus();
          this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus - {0} Trades' status have been updated to Settled.", (object) intList.Count<int>()));
          this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus - AddTradeHistory at: {0}", (object) DateTime.Now.ToString()));
          this.AddTradeHistory(intList, userInfo);
        }
        catch (Exception ex)
        {
          this._ctx.TraceLog.WriteError(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus - error: {0} - {1}", (object) ex.Message, (object) ex.StackTrace));
        }
        this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus End at: {0}", (object) DateTime.Now.ToString()));
        this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, string.Format("UpdateCorrTradeStatus Execution Time (minutes): {0}", (object) (DateTime.Now - now).TotalMinutes));
      }
    }

    private void AddTradeHistory(List<int> trades, UserInfo userInfo)
    {
      foreach (int trade1 in trades)
      {
        CorrespondentTradeInfo trade2 = new CorrespondentTradeInfo();
        trade2.TradeID = trade1;
        CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade2, TradeHistoryAction.TradeStatusChanged, TradeStatus.Settled, userInfo));
        bool result = this.PublishWebhookEvent(trade1).Result;
      }
    }

    private async Task<bool> PublishWebhookEvent(int tradeId)
    {
      WebhookEventContract eventContract = new WebhookEventContract();
      eventContract.UserId = "ScheduledJob";
      eventContract.InstanceId = ClientContext.GetCurrent().InstanceName;
      eventContract.EventType = "update";
      eventContract.ResourceId = tradeId.ToString();
      eventContract.AddExpoObject("statusUpdatedTo", (object) "Settled");
      try
      {
        return await this.Publish(eventContract);
      }
      catch (Exception ex)
      {
        this._ctx.TraceLog.WriteException(TraceLevel.Error, UpdateCorrTradeStatusTaskHandler._className, ex);
        return false;
      }
    }

    public async Task<bool> Publish(WebhookEventContract eventContract)
    {
      HttpClient httpClient = new HttpClient();
      bool result = true;
      WebhookEventConsts webhookEventConsts = new WebhookEventConsts(WebhookResource.trade);
      eventContract.ResourceType = webhookEventConsts.EventResourceName;
      eventContract.ResourceRef = webhookEventConsts.ResourceRef + eventContract.ResourceId;
      eventContract.Source = "urn:elli:encompass:" + eventContract.InstanceId;
      string appSetting = ConfigurationManager.AppSettings["x-api-key"];
      string str1 = string.IsNullOrWhiteSpace(appSetting) ? "LPivKuC0aK6yJF3SyeExvJWK9i8agljg5s2lfVz10bamqMSsPirutSKzNCLhP0en" : appSetting;
      using (HttpRequestMessage request = new HttpRequestMessage())
      {
        string str2 = EnConfigurationSettings.AppSettings["oAuth.Url"];
        if (string.IsNullOrEmpty(str2))
          str2 = "https://api.elliemae.com";
        request.Method = new HttpMethod("POST");
        request.RequestUri = new Uri(str2 + webhookEventConsts.EventEndPoint);
        request.Headers.Add("x-api-key", str1);
        request.Content = (HttpContent) new StringContent(JsonConvert.SerializeObject((object) eventContract, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
        }), Encoding.UTF8, "application/json");
        this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, "Calling SendAsync: " + request.RequestUri.ToString() + " Payload: " + request.Content.ToString());
        using (HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(request).ConfigureAwait(false))
        {
          this._ctx.TraceLog.WriteInfo(UpdateCorrTradeStatusTaskHandler._className, "SendAsync Response StatusCode: " + (object) httpResponseMessage.StatusCode);
          if (!httpResponseMessage.IsSuccessStatusCode)
            result = false;
        }
      }
      return result;
    }
  }
}
