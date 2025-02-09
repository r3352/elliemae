// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.ConsumerConnect.SiteAccessor
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.ConsumerConnect
{
  public class SiteAccessor
  {
    private const string Scope = "cc�";
    private static readonly string _sw = Tracing.SwCommon;
    private string _idpServer;

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public CMSPaginatedResponse GetCCSites(SessionObjects session, string query = "�", int startOffset = 0)
    {
      if (session == null)
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: session is null" + Environment.NewLine);
        return (CMSPaginatedResponse) null;
      }
      if (string.IsNullOrEmpty(session.SessionID))
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: sessionId is empty." + Environment.NewLine);
        return (CMSPaginatedResponse) null;
      }
      string serverInstanceName = session.StartupInfo.ServerInstanceName;
      if (string.IsNullOrEmpty(serverInstanceName))
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: instanceId is empty" + Environment.NewLine);
        return (CMSPaginatedResponse) null;
      }
      if (string.IsNullOrEmpty(session.UserID))
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: User information is empty." + Environment.NewLine);
        return (CMSPaginatedResponse) null;
      }
      try
      {
        bool flag = true;
        string str1 = session.CcAuthToken;
        this._idpServer = session.IdpServer;
        if (string.IsNullOrEmpty(str1))
        {
          OAuth2.AuthToken accessToken = new OAuth2(session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(session)).GetAccessToken(serverInstanceName, session.SessionID, "cc");
          str1 = accessToken.TypeAndToken;
          this._idpServer = "https://" + accessToken.host_name;
          flag = false;
        }
        if (string.IsNullOrEmpty(str1))
        {
          Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: Authentication information is not available to get Site Id." + Environment.NewLine);
          return (CMSPaginatedResponse) null;
        }
        RestClient client = new RestClient(this._idpServer);
        IRestResponse response = this.GetResponse(client, str1, startOffset, query);
        if (response.StatusCode != HttpStatusCode.OK)
        {
          if (flag)
          {
            OAuth2.AuthToken accessToken = new OAuth2(session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(session)).GetAccessToken(serverInstanceName, session.SessionID, "cc");
            str1 = accessToken.TypeAndToken;
            this._idpServer = "https://" + accessToken.host_name;
            flag = false;
            if (string.IsNullOrEmpty(str1))
            {
              Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: Authentication information is not available to get Site Id." + Environment.NewLine);
              return (CMSPaginatedResponse) null;
            }
            response = this.GetResponse(client, str1, startOffset, query);
          }
          if (response.StatusCode != HttpStatusCode.OK)
          {
            Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: The request to " + this._idpServer + "/content/v1/sites?status=published&tags=CC API failed. The status code is " + response.StatusCode.ToString() + Environment.NewLine);
            return (CMSPaginatedResponse) null;
          }
        }
        if (!flag && !string.IsNullOrEmpty(str1))
        {
          session.SetCcAuthToken(str1);
          session.SetIdpServer(this._idpServer);
        }
        int totalSiteCount = int.Parse(response.Headers.ToList<Parameter>().Find((Predicate<Parameter>) (x => x.Name == "X-Total-Count")).Value.ToString());
        Site[] source = JsonConvert.DeserializeObject<Site[]>(response.Content);
        if (source != null && ((IEnumerable<Site>) source).Any<Site>())
          return new CMSPaginatedResponse(((IEnumerable<Site>) source).ToList<Site>(), totalSiteCount);
        string str2 = "CCSiteId: " + this._idpServer + "/content/v1/sites?status=published&tags=CC (Token:" + str1 + ") did not return any site.";
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Info, str2 + Environment.NewLine);
        return (CMSPaginatedResponse) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: " + ex.ToString() + Environment.NewLine);
        return (CMSPaginatedResponse) null;
      }
    }

    private IRestResponse GetResponse(
      RestClient client,
      string authString,
      int start,
      string query = "�")
    {
      if (query.Length > 0)
        query = "&domain=~*" + query.Trim() + "*";
      RestRequest request = new RestRequest("/content/v1/sites?status=published&tags=CC" + query + "&start=" + (object) start + "&limit=100&sort=createdAt", Method.GET);
      request.AddHeader("authorization", authString);
      return client.Execute((IRestRequest) request);
    }

    private IRestResponse GetResponseForSiteId(RestClient client, string authString, string siteId)
    {
      RestRequest request = new RestRequest("/content/v1/sites/" + siteId + "?status=published", Method.GET);
      request.AddHeader("authorization", authString);
      return client.Execute((IRestRequest) request);
    }

    public Site GetSiteForSiteID(SessionObjects session, string siteId)
    {
      if (session == null)
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: session is null" + Environment.NewLine);
        return (Site) null;
      }
      if (string.IsNullOrEmpty(session.SessionID))
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: sessionId is empty." + Environment.NewLine);
        return (Site) null;
      }
      string serverInstanceName = session.StartupInfo.ServerInstanceName;
      if (string.IsNullOrEmpty(serverInstanceName))
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: instanceId is empty" + Environment.NewLine);
        return (Site) null;
      }
      if (string.IsNullOrEmpty(session.UserID))
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: User information is empty." + Environment.NewLine);
        return (Site) null;
      }
      try
      {
        bool flag = true;
        string str1 = session.CcAuthToken;
        this._idpServer = session.IdpServer;
        if (string.IsNullOrEmpty(str1))
        {
          OAuth2.AuthToken accessToken = new OAuth2(session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(session)).GetAccessToken(serverInstanceName, session.SessionID, "cc");
          str1 = accessToken.TypeAndToken;
          this._idpServer = "https://" + accessToken.host_name;
          flag = false;
        }
        if (string.IsNullOrEmpty(str1))
        {
          Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: Authentication information is not available to get Site Id." + Environment.NewLine);
          return (Site) null;
        }
        RestClient client = new RestClient(this._idpServer);
        IRestResponse responseForSiteId = this.GetResponseForSiteId(client, str1, siteId);
        if (responseForSiteId.StatusCode != HttpStatusCode.OK)
        {
          if (flag)
          {
            OAuth2.AuthToken accessToken = new OAuth2(session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(session)).GetAccessToken(serverInstanceName, session.SessionID, "cc");
            str1 = accessToken.TypeAndToken;
            this._idpServer = "https://" + accessToken.host_name;
            flag = false;
            if (string.IsNullOrEmpty(str1))
            {
              Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: Authentication information is not available to get Site Id." + Environment.NewLine);
              return (Site) null;
            }
            responseForSiteId = this.GetResponseForSiteId(client, str1, siteId);
          }
          if (responseForSiteId.StatusCode != HttpStatusCode.OK)
          {
            Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: The request to " + this._idpServer + "/content/v1/sites?status=published&tags=CC API failed. The status code is " + responseForSiteId.StatusCode.ToString() + Environment.NewLine);
            return (Site) null;
          }
        }
        if (!flag && !string.IsNullOrEmpty(str1))
        {
          session.SetCcAuthToken(str1);
          session.SetIdpServer(this._idpServer);
        }
        Site siteForSiteId = JsonConvert.DeserializeObject<Site>(responseForSiteId.Content);
        if (siteForSiteId != null)
          return siteForSiteId;
        string str2 = "CCSiteId: " + this._idpServer + "/content/v1/sites?status=published&tags=CC (Token:" + str1 + ") did not return any site.";
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Info, str2 + Environment.NewLine);
        return (Site) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(SiteAccessor._sw, this.GetType().ToString(), TraceLevel.Error, "CCSiteId: " + ex.ToString() + Environment.NewLine);
        return (Site) null;
      }
    }
  }
}
