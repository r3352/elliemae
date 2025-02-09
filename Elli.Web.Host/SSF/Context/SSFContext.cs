// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Context.SSFContext
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.SSF.Bridges;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.SSF.Context
{
  public class SSFContext
  {
    private const string className = "SSFContext";
    private static readonly string sw = Tracing.SwThinThick;
    private static object LockObject = new object();
    private readonly LoanDataMgr _loanDataMgr;
    private readonly string _hostUrl;
    private readonly SSFGuest _guestInfo;
    private ApplicationBridge _application;
    private AuthBridge _auth;
    private LoanBridge _loan;
    private ModuleBridge _module;
    private ServiceBridge _service;

    private SSFContext(LoanDataMgr loanDataMgr, string hostUrl, SSFGuest guestInfo)
    {
      this._loanDataMgr = loanDataMgr;
      this._hostUrl = hostUrl;
      this._guestInfo = guestInfo;
    }

    public static SSFContext Create(SSFHostType hostType, string guestUrl)
    {
      return SSFContext.Create((LoanDataMgr) null, hostType, guestUrl);
    }

    public static SSFContext Create(string hostUrl, string guestUrl)
    {
      return SSFContext.Create((LoanDataMgr) null, hostUrl, guestUrl);
    }

    public static SSFContext Create(LoanDataMgr loanDataMgr, SSFHostType hostType, string guestUrl)
    {
      string hostUrl = SSFContext.GetHostUrl(hostType);
      return SSFContext.Create(loanDataMgr, hostUrl, guestUrl);
    }

    public static SSFContext Create(LoanDataMgr loanDataMgr, string hostUrl, string guestUrl)
    {
      SSFContext ssfContext = (SSFContext) null;
      try
      {
        Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Creating SSFGuestStore");
        SSFGuestRegistry ssfGuestRegistry = new SSFGuestRegistry();
        Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Calling SSFGuestStore.GetGuestInfo: " + guestUrl);
        string guestUrl1 = guestUrl;
        Task<SSFGuest> guest = ssfGuestRegistry.GetGuest(guestUrl1);
        Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Waiting for GetGuestInfo Task");
        Task.WaitAll((Task) guest);
        Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Checking GetGuestInfo Result");
        if (guest.Result == null)
          throw new Exception("Guest Not Registered: " + guestUrl);
        Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Creating SSFContext: " + guest.Result.title);
        ssfContext = new SSFContext(loanDataMgr, hostUrl, guest.Result);
      }
      catch (Exception ex)
      {
        Tracing.Log(SSFContext.sw, TraceLevel.Error, nameof (SSFContext), "Failed to create SSFContext: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred trying to create the SSFContext: \n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return ssfContext;
    }

    public static SSFContext Create(
      LoanDataMgr loanDataMgr,
      SSFHostType hostType,
      SSFGuest guestInfo)
    {
      string hostUrl = SSFContext.GetHostUrl(hostType);
      return SSFContext.Create(loanDataMgr, hostUrl, guestInfo);
    }

    public static SSFContext Create(LoanDataMgr loanDataMgr, string hostUrl, SSFGuest guestInfo)
    {
      Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Creating SSFContext: " + guestInfo?.title);
      return new SSFContext(loanDataMgr, hostUrl, guestInfo);
    }

    internal LoanDataMgr loanDataMgr => this._loanDataMgr;

    public string hostUrl => this._hostUrl;

    public string guestUrl => this._guestInfo.uri;

    internal string guestScope => this._guestInfo.scope;

    public Dictionary<string, object> parameters { get; set; }

    public Action unloadHandler { get; set; }

    internal AccessToken getAccessToken(bool noCache)
    {
      ICache simpleCache = CacheManager.GetSimpleCache("AccessTokenCache");
      string key = string.Format("{0}_{1}_{2}", (object) this._guestInfo.clientId, (object) this._guestInfo.uri, (object) this._guestInfo.scope);
      if (!noCache)
      {
        AccessToken accessToken = (AccessToken) simpleCache.Get(key);
        if (accessToken != null)
        {
          Tracing.Log(SSFContext.sw, TraceLevel.Info, nameof (SSFContext), string.Format("Using Cached AccessToken. Key:{0} Token:{1}", (object) key, (object) accessToken.TypeAndToken));
          return accessToken;
        }
      }
      Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Calling User.GetAccessTokenForGuest");
      AccessToken accessTokenForGuest = Session.User.GetAccessTokenForGuest(this._guestInfo.clientId, this._guestInfo.scope, this._guestInfo.uri);
      Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), string.Format("Caching AccessToken. Key:{0} Token:{1}", (object) key, (object) accessTokenForGuest.TypeAndToken));
      simpleCache.Put(key, new CacheItem((object) accessTokenForGuest, new CacheItemRetentionPolicy(new TimeSpan(0, 15, 0), new TimeSpan(2, 0, 0))));
      return accessTokenForGuest;
    }

    public ApplicationBridge application
    {
      get
      {
        if (this._application == null)
          this._application = new ApplicationBridge(this);
        return this._application;
      }
    }

    public AuthBridge auth
    {
      get
      {
        if (this._auth == null)
          this._auth = new AuthBridge(this);
        return this._auth;
      }
    }

    public LoanBridge loan
    {
      get
      {
        if (this._loan == null)
          this._loan = new LoanBridge(this);
        return this._loan;
      }
    }

    public ModuleBridge module
    {
      get
      {
        if (this._module == null)
          this._module = new ModuleBridge(this);
        return this._module;
      }
    }

    public ServiceBridge service
    {
      get
      {
        if (this._service == null)
          this._service = new ServiceBridge(this);
        return this._service;
      }
    }

    private static string GetHostUrl(SSFHostType hostType)
    {
      string hostUrl = (string) null;
      if (hostType == SSFHostType.Network)
        hostUrl = Session.StartupInfo?.ServiceUrls?.SSFHostUrl + "SSFHost.html";
      return hostUrl;
    }

    internal IRaiseEventHandler RaiseEventHandler { get; set; }

    internal virtual bool RaiseEvent(SSFEventArgs e)
    {
      if (this.RaiseEventHandler == null)
        return false;
      Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Calling RaiseEventHandler.RaiseEvent: " + e.ToString());
      return this.RaiseEventHandler.RaiseEvent(e);
    }

    internal virtual bool RaiseEvent<T>(SSFEventArgs<T> e, int millisecondsTimeout = 2000)
    {
      if (this.RaiseEventHandler == null)
        return false;
      Tracing.Log(SSFContext.sw, TraceLevel.Verbose, nameof (SSFContext), "Calling RaiseEventHandler.RaiseEvent: " + e.ToString());
      return this.RaiseEventHandler.RaiseEvent<T>(e, millisecondsTimeout);
    }

    private void subscribeLoanEvents()
    {
      this._loanDataMgr.LoanData.FieldChanged += new FieldChangedEventHandler(this.LoanData_FieldChanged);
    }

    private void LoanData_FieldChanged(object source, FieldChangedEventArgs e)
    {
      this.RaiseEvent(new SSFEventArgs("loan", "change"));
    }
  }
}
