// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MiCenterService
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MiCenterService : IOnlineHelpTarget
  {
    private const string className = "MiCenterService";
    private static string sw = Tracing.SwOutsideLoan;

    public string GetHelpTargetName() => nameof (MiCenterService);

    public string LaunchMiCenter(
      LoanDataMgr loanDataMgr,
      MiCenterUiMetaInfo miCenterInfo,
      int width,
      int height)
    {
      try
      {
        string uriString = Session.SessionObjects?.StartupInfo?.ServiceUrls?.MiCenterUrl;
        if (string.IsNullOrWhiteSpace(uriString) || !Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
          uriString = "https://www.encompassloconnect.com/mi-center/";
        string str1 = string.IsNullOrWhiteSpace(miCenterInfo?.UiUrl?.AbsoluteUri) ? uriString : miCenterInfo.UiUrl.AbsoluteUri;
        Tracing.Log(MiCenterService.sw, TraceLevel.Info, nameof (MiCenterService), "Launch MI Center: " + str1);
        SSFGuest guestInfo = new SSFGuest()
        {
          uri = str1,
          scope = "mic",
          clientId = "m71h0hve"
        };
        SSFContext context = SSFContext.Create(loanDataMgr, SSFHostType.Network, guestInfo);
        if (context == null)
        {
          Tracing.Log(MiCenterService.sw, TraceLevel.Error, nameof (MiCenterService), "No SSF Context created.");
          return "Error happened when launching MI Center.";
        }
        string str2 = new Bam().GetAccessToken("sc").Replace("Bearer ", "");
        context.parameters = new Dictionary<string, object>()
        {
          {
            "hostname",
            (object) "smartclient"
          },
          {
            "instanceId",
            (object) Session.ServerIdentity.InstanceName
          },
          {
            "loanGuid",
            (object) loanDataMgr.LoanData.GUID
          },
          {
            "errorMessages",
            (object) new List<string>()
          },
          {
            "token",
            (object) str2
          }
        };
        using (SSFDialog ssfDialog = new SSFDialog(context))
        {
          ssfDialog.Text = "MI Center";
          ssfDialog.Height = height;
          ssfDialog.Width = width;
          ssfDialog.ShowDialog((IWin32Window) Session.MainForm);
        }
        return string.Empty;
      }
      catch (Exception ex)
      {
        Tracing.Log(MiCenterService.sw, TraceLevel.Error, nameof (MiCenterService), string.Format("Error in LaunchMiCenter. Exception: {0}", (object) ex));
        return "Error happened when launching MI Center.";
      }
    }

    public MiCenterUiMetaInfo GetMiCenterUiMetaInfo()
    {
      try
      {
        string oapiGatewayBaseUri = Session.StartupInfo.OAPIGatewayBaseUri;
        if (string.IsNullOrEmpty(oapiGatewayBaseUri))
          return (MiCenterUiMetaInfo) null;
        HttpWebRequest httpWebRequest = this.GetHttpWebRequest(oapiGatewayBaseUri + "/tql/mi/v1/ui/metainfo");
        httpWebRequest.Method = "GET";
        string accessToken = new Bam().GetAccessToken("sc");
        httpWebRequest.Headers.Add("Authorization", accessToken);
        httpWebRequest.ContentType = "application/json";
        string end;
        using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        return JsonConvert.DeserializeObject<MiCenterUiMetaInfo>(end);
      }
      catch (Exception ex)
      {
        Tracing.Log(MiCenterService.sw, TraceLevel.Error, nameof (MiCenterService), string.Format("Error in GetMiCenterUiMetaInfo. Exception: {0}", (object) ex));
        return (MiCenterUiMetaInfo) null;
      }
    }

    private HttpWebRequest GetHttpWebRequest(string apiUrl)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(apiUrl);
      httpWebRequest.ServicePoint.Expect100Continue = false;
      return httpWebRequest;
    }
  }
}
