// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.LoginManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Interception;
using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.MFAAuthentication;
using EllieMae.EMLite.Server.ServerObjects;
using Encompass.Diagnostics.Logging.Schema;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;

#nullable disable
namespace Elli.Server.Remoting
{
  public class LoginManager : RemotedObject, ILoginManager
  {
    protected static string sw = Tracing.SwOutsideLoan;
    private const string className = "LoginManager";
    private static ConcurrentDictionary<string, bool> userResets = new ConcurrentDictionary<string, bool>();
    public static readonly LoginManager RemotingInstance = InterceptionUtils.NewInstance<LoginManager>();

    static LoginManager()
    {
      Err instance = Err.Instance;
    }

    public ISession Login(LoginParameters loginParams, IServerCallback client)
    {
      if (loginParams == null)
        Err.Raise(TraceLevel.Warning, nameof (LoginManager), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Login parameters must not be null"));
      if (client == null)
        Err.Raise(TraceLevel.Warning, nameof (LoginManager), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Callback reference must not be null"));
      using (ClientContext.Open(loginParams.Server.InstanceName).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        return this.LoginInternal(loginParams, client);
    }

    protected virtual ISession LoginInternal(LoginParameters loginParams, IServerCallback client)
    {
      ClientContext current1 = ClientContext.GetCurrent();
      PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Server.Login", "Server Side Login", 94, nameof (LoginInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\LoginManager.cs");
      int num1 = 0;
      int num2 = 0;
      IPAddress clientAddress = (IPAddress) null;
      string clientIpAddress;
      if (!loginParams.OfflineMode)
      {
        clientAddress = ConnectionManager.GetCurrentClientIPAddress();
        clientIpAddress = (clientAddress ?? IPAddress.None).ToString();
      }
      else
        clientIpAddress = "Local";
      bool flag = this.IsValidTokenFlow((IClientContext) current1, loginParams.Server.InstanceName, loginParams.AppName, loginParams.UserID, clientIpAddress);
      RemoteCallTracker current2 = RemoteCallTracker.GetCurrent();
      if (current2 != null)
        current2.AddDecorator((IRemoteCallLogDecorator) new LoginManager.LoginCallProcessingInfo()
        {
          IsTokenLoginFlow = flag,
          ClientIpAddress = clientIpAddress
        });
      TraceLog.WriteApi(nameof (LoginManager), "Login", (object) loginParams.UserID, (object) loginParams.Hostname, (object) loginParams.AppName, (object) clientIpAddress, (object) ("IsTokenLogin: " + flag.ToString()));
      if (flag)
      {
        if (!string.IsNullOrWhiteSpace(loginParams.AuthCode))
          return this.LoginMFA(loginParams, client, current1);
        Err.Raise(TraceLevel.Error, nameof (LoginManager), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Login - AccessToken is Null or Empty in TokenOnlyFlow"));
      }
      try
      {
        LoginResponse loginResponse1 = (LoginResponse) LoginReturnCode.USERID_NOT_FOUND;
        if (current1.Settings.Disabled)
          Err.Raise(nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) new SecurityException("ClientContext '" + loginParams.Server.InstanceName + "' is marked as disabled"));
        loginParams.Server.InstanceName = current1.InstanceName;
        try
        {
          num2 = current1.Cache.GetMaxConcurrentLogins((IClientContext) current1);
        }
        catch
        {
        }
        if (num2 > 0)
        {
          num1 = current1.Cache.IncrementNumConcurrentLogins();
          if (num1 > num2)
          {
            LoginException ex = new LoginException(loginParams.Clone(false), clientAddress, LoginReturnCode.SERVER_BUSY);
            EncompassServer.RaiseEvent((ServerEvent) new ExceptionEvent((Exception) ex));
            Err.Raise(TraceLevel.Info, nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) ex);
          }
        }
        if (loginParams.UserID != "(trusted)")
          LicenseManager.ValidateServerLicense(current1);
        LoginResponse loginResponse2;
        if (loginParams.UserID == "(trusted)")
          loginResponse2 = (LoginResponse) this.validateTrustedUserLogin((object) client);
        else if (!loginParams.IsTrustedServiceApp && !loginParams.OfflineMode && !this.validateIP(current1, loginParams.UserID, clientAddress))
        {
          loginResponse2 = (LoginResponse) LoginReturnCode.IP_Blocked;
          try
          {
            Tracing.Log(LoginManager.sw, TraceLevel.Warning, nameof (LoginManager), "User " + loginParams.UserID + " logged in from " + clientAddress.ToString() + " was blocked by IP restriction.");
          }
          catch
          {
          }
        }
        else
          loginResponse2 = !loginParams.OfflineMode || !(loginParams.AppName == "Encompass") || !current1.AllowConcurrentEditing ? (!loginParams.OfflineMode || !(loginParams.AppName == "AdminTools") || !(loginParams.UserID == "admin") || !(loginParams.Password == EnConfigurationSettings.GlobalSettings.DatabasePassword) ? (!loginParams.IsTrustedServiceApp ? this.validateLogin(loginParams.UserID, loginParams.Password, loginParams.ClientDisplayVersion, false) : this.validateLogin(loginParams.UserID, loginParams.Password, (string) null, true)) : (LoginResponse) LoginReturnCode.SUCCESS) : (LoginResponse) LoginReturnCode.Concurrent_Editing_Offline_Not_Allowed;
        if (!this.areLoginsEnabled(current1) && loginParams.UserID != "admin" && loginParams.UserID != "(trusted)")
          Err.Raise(TraceLevel.Info, nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) new LoginException(loginParams.Clone(false), clientAddress, LoginReturnCode.LOGIN_DISABLED));
        if (loginResponse2.ReturnCode != LoginReturnCode.SUCCESS)
        {
          if (clientAddress != null)
            this.insertFailedLoginRecord(loginResponse2.ReturnCode, loginParams.UserID, clientAddress.ToString());
          else
            this.insertFailedLoginRecord(loginResponse2.ReturnCode, loginParams.UserID, "localhost");
          LoginException ex = loginResponse2.ReturnCode == LoginReturnCode.USER_LOCKED ? (string.IsNullOrEmpty(loginResponse2.AdditionalInfo) ? new LoginException(loginParams.Clone(false), clientAddress, loginResponse2.ReturnCode) : new LoginException(loginParams.Clone(false), clientAddress, loginResponse2.ReturnCode, loginResponse2.AdditionalInfo)) : new LoginException(loginParams.Clone(false), clientAddress, loginResponse2.ReturnCode);
          EncompassServer.RaiseEvent((ServerEvent) new ExceptionEvent((Exception) ex));
          Err.Raise(TraceLevel.Info, nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) ex);
        }
        performanceMeter.AddCheckpoint("Server.Login - Login Check", 215, nameof (LoginInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\LoginManager.cs");
        Session innerSession = new Session(loginParams.Clone(false), current1, client);
        if (!string.IsNullOrWhiteSpace(loginParams.PrevSessionID))
        {
          Loan.ReplaceSessionID(loginParams.PrevSessionID, innerSession.SessionID);
          this.terminatePreviousSession(current1, loginParams.PrevSessionID);
        }
        performanceMeter.AddCheckpoint("Server.Login - Create Login Sessoin", 225, nameof (LoginInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\LoginManager.cs");
        performanceMeter.Stop();
        RemoteCallTracker.GetCurrent()?.SetSession((ISession) innerSession);
        return (ISession) InterceptionUtils.NewInstance<SessionProxy>().Initialize(innerSession);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoginManager), ex);
        return (ISession) null;
      }
      finally
      {
        if (num1 > 0)
          current1.Cache.DecrementNumConcurrentLogins();
      }
    }

    private bool IsValidTokenFlow(
      IClientContext context,
      string instance,
      string appName,
      string userId,
      string clientIpAddress)
    {
      if (string.IsNullOrWhiteSpace(instance) || userId == "(trusted)" || appName == "trustedEnc" || string.IsNullOrEmpty(clientIpAddress) || clientIpAddress == "Local")
        return false;
      string lowerCaseAppName = appName.ToLower();
      string serverSetting = (string) ClientContext.GetCurrent().Settings.GetServerSetting("LOGIN.ExclusionListPassPwdToAuthCode", false);
      List<string> stringList;
      if (!string.IsNullOrWhiteSpace(serverSetting))
        stringList = ((IEnumerable<string>) serverSetting.ToLower().Split(',')).ToList<string>();
      else
        stringList = (List<string>) null;
      List<string> source = stringList;
      return source == null ? this.IsSmartClientTokenLoginOnlyEnabled(context, instance) && !lowerCaseAppName.StartsWith("api.") : this.IsSmartClientTokenLoginOnlyEnabled(context, instance) && !lowerCaseAppName.StartsWith("api.") && !source.Any<string>((Func<string, bool>) (app => lowerCaseAppName.StartsWith(app)));
    }

    private ISession LoginMFA(
      LoginParameters loginParams,
      IServerCallback client,
      ClientContext context)
    {
      TraceLog.WriteApi(nameof (LoginManager), nameof (LoginMFA), (object) loginParams.UserID, (object) loginParams.Hostname, (object) loginParams.AppName);
      string empty = string.Empty;
      if (loginParams == null)
        Err.Raise(TraceLevel.Warning, nameof (LoginManager), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Login parameters must not be null"));
      if (client == null)
        Err.Raise(TraceLevel.Warning, nameof (LoginManager), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Callback reference must not be null"));
      PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Server.Login", "Server Side Login", 282, nameof (LoginMFA), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\LoginManager.cs");
      Session innerSession;
      try
      {
        if (!loginParams.OfflineMode)
          (ConnectionManager.GetCurrentClientIPAddress() ?? IPAddress.None).ToString();
        IntrospectionDetails tokenIntrospection = IntrospectionAccessor.GetSessionIDFromAccessTokenIntrospection(new OAuth2(EnConfigurationSettings.AppSettings["oAuth.Url"]).GetAccessTokenFromAuthCode(loginParams.AuthCode, "https://encompass.elliemae.com/homepage/atest.asp").access_token);
        if (string.IsNullOrWhiteSpace(tokenIntrospection.SessionID))
          Err.Raise(TraceLevel.Error, nameof (LoginManager), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Login - introspecting API returns empty SessionID."));
        this.UpdateUserLoginInfo(tokenIntrospection.UserName, loginParams.ClientDisplayVersion);
        string sessionId = tokenIntrospection.SessionID;
        innerSession = new Session(loginParams.Clone(false, tokenIntrospection.UserName), context, client, sessionId);
        TraceLog.WriteApi((ISession) null, nameof (LoginManager), nameof (LoginMFA), (object) ("SSO sessionId : " + innerSession.SessionID), (object) ("UserId :" + tokenIntrospection.UserName), (object) client);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoginManager.sw, TraceLevel.Error, nameof (LoginManager), "Login Error introspecting authcode to accessToken '" + loginParams.UserID + "' Exception: " + ex.Message);
        Err.Reraise(nameof (LoginManager), ex);
        return (ISession) null;
      }
      performanceMeter.AddCheckpoint("Server.Login - Create Login session", 319, nameof (LoginMFA), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\LoginManager.cs");
      performanceMeter.Stop();
      RemoteCallTracker.GetCurrent()?.SetSession((ISession) innerSession);
      return (ISession) InterceptionUtils.NewInstance<SessionProxy>().Initialize(innerSession);
    }

    private void terminatePreviousSession(ClientContext context, string prevSessionID)
    {
      try
      {
        context.Sessions.GetSession(prevSessionID)?.Abandon();
      }
      catch (Exception ex)
      {
        try
        {
          Tracing.Log(LoginManager.sw, TraceLevel.Error, nameof (LoginManager), "Error terminating previous session '" + prevSessionID + "': " + ex.Message);
        }
        catch
        {
        }
      }
    }

    private bool validateIP(ClientContext context, string userid, IPAddress clientAddress)
    {
      Tracing.Log(LoginManager.sw, TraceLevel.Info, nameof (LoginManager), "User " + userid + " logged in from " + clientAddress.ToString());
      if (!context.IPRestrictionSetting)
        return true;
      bool flag = false;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass" + (string.IsNullOrEmpty(context.InstanceName) ? "" : "$" + context.InstanceName)))
      {
        if (registryKey != null)
          flag = string.Concat(registryKey.GetValue("IPRestrictionTest")).Trim() == "1";
      }
      if (!flag && clientAddress != null && clientAddress.ToString() == "127.0.0.1")
        return true;
      IPRange[] allowedIpRanges = ClientIPManager.GetAllowedIPRanges((IClientContext) context);
      return !IPRange.ExistsUserConfig(userid, allowedIpRanges) || IPRange.IsInRanges(userid, clientAddress, allowedIpRanges);
    }

    private void insertFailedLoginRecord(
      LoginReturnCode code,
      string targetUserID,
      string ipAddress)
    {
      ActionType actionType = ActionType.FailedLoginLoginDisabled;
      switch (code)
      {
        case LoginReturnCode.USERID_NOT_FOUND:
          actionType = ActionType.FailedLoginUserNotFound;
          break;
        case LoginReturnCode.PASSWORD_MISMATCH:
          actionType = ActionType.FailedLoginPasswordMismatch;
          break;
        case LoginReturnCode.USER_DISABLED:
          actionType = ActionType.FailedLoginUserDisabled;
          break;
        case LoginReturnCode.LOGIN_DISABLED:
          actionType = ActionType.FailedLoginLoginDisabled;
          break;
        case LoginReturnCode.USER_LOCKED:
          actionType = ActionType.FailedLoginUserLocked;
          break;
        case LoginReturnCode.PERSONA_NOT_FOUND:
          actionType = ActionType.FailedLoginPersonaNotFound;
          break;
        case LoginReturnCode.IP_Blocked:
          actionType = ActionType.IPBlocked;
          break;
        case LoginReturnCode.SERVER_BUSY:
          actionType = ActionType.FailedLoginServerBusy;
          break;
      }
      SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new FailedUserLoginAuditRecord(targetUserID, actionType, ipAddress, DateTime.Now));
    }

    public virtual SecurityContextOptions GetClientSecurityOptions()
    {
      TraceLog.WriteApi((ISession) null, nameof (LoginManager), nameof (GetClientSecurityOptions));
      try
      {
        return ConnectionManager.SecurityContext;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoginManager), ex);
        return (SecurityContextOptions) null;
      }
    }

    public virtual IManagementSession Manage(LoginParameters loginParams, IServerCallback client)
    {
      TraceLog.WriteApi((ISession) null, nameof (LoginManager), nameof (Manage), (object) loginParams, (object) "IServerCallback");
      try
      {
        if (EllieMae.EMLite.Server.ServerGlobals.RMIPassword == "")
          Err.Raise(nameof (LoginManager), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("The Remote Management Interface is not enabled for this server"));
        if (loginParams.UserID.ToLower() != "(rmi)" || loginParams.Password != EllieMae.EMLite.Server.ServerGlobals.RMIPassword)
          Err.Raise(TraceLevel.Info, nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) new LoginException(loginParams.Clone(false), ConnectionManager.GetCurrentClientIPAddress(), LoginReturnCode.PASSWORD_MISMATCH));
        return (IManagementSession) new ManagementSession(loginParams, client);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoginManager), ex);
        return (IManagementSession) null;
      }
    }

    public ITitleFeesAccessor GetTitleFeesAccessor(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials,
      IServerCallback client)
    {
      TraceLog.WriteApi((ISession) null, nameof (LoginManager), nameof (GetTitleFeesAccessor), (object) instanceName, (object) orderUID, (object) loanGUID, (object) credentials, (object) "IServerCallback");
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          if (!TitleFeeCredentialsAccessor.ValidateCredentials(orderUID, loanGUID, credentials))
            Err.Raise(TraceLevel.Info, nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) new LoginException(new LoginParameters(orderUID, credentials, new EllieMae.EMLite.ClientServer.ServerIdentity(instanceName), (string) null, "", false).Clone(false), ConnectionManager.GetCurrentClientIPAddress(), LoginReturnCode.PASSWORD_MISMATCH));
        }
        return (ITitleFeesAccessor) new TitleFeesAccessor(instanceName, orderUID, loanGUID, credentials, client);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoginManager), ex);
        return (ITitleFeesAccessor) null;
      }
    }

    public IData4CloAccessor GetData4CloAccessor(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials,
      IServerCallback client)
    {
      TraceLog.WriteApi((ISession) null, nameof (LoginManager), nameof (GetData4CloAccessor), (object) instanceName, (object) orderUID, (object) loanGUID, (object) credentials, (object) "IServerCallback");
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          if (!TitleFeeCredentialsAccessor.ValidateCredentials(orderUID, loanGUID, credentials))
            Err.Raise(TraceLevel.Info, nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) new LoginException(new LoginParameters(orderUID, credentials, new EllieMae.EMLite.ClientServer.ServerIdentity(instanceName), (string) null, "", false).Clone(false), ConnectionManager.GetCurrentClientIPAddress(), LoginReturnCode.PASSWORD_MISMATCH));
        }
        return (IData4CloAccessor) new Data4CloAccessor(instanceName, orderUID, loanGUID, credentials, client);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoginManager), ex);
        return (IData4CloAccessor) null;
      }
    }

    public DateTime ServerTime() => DateTime.Now;

    public object Echo(string connectionId, object obj)
    {
      TraceLog.WriteApi((ISession) null, nameof (LoginManager), nameof (Echo), (object) connectionId, obj);
      ConnectionManager.UpdateLastEchoTimeForConnection(connectionId);
      TraceLog.WriteApi((ISession) null, nameof (LoginManager), "Echo response sent from server", (object) connectionId, (object) this.ServerTime());
      return obj;
    }

    private bool validateS2SLogin(string connString)
    {
      SqlConnection sqlConnection = (SqlConnection) null;
      try
      {
        sqlConnection = new SqlConnection(connString);
        sqlConnection.Open();
      }
      catch
      {
        return false;
      }
      finally
      {
        sqlConnection?.Close();
      }
      return true;
    }

    private LoginResponse validateLogin(
      string userId,
      string password,
      string encompassVersion,
      bool s2s)
    {
      using (User latestVersion = UserStore.GetLatestVersion(userId))
      {
        if (!latestVersion.Exists)
          return (LoginResponse) LoginReturnCode.USERID_NOT_FOUND;
        if (latestVersion.UserInfo.ApiUser)
          return (LoginResponse) LoginReturnCode.API_USER_RESTRICTED;
        if (latestVersion.UserInfo.SSOOnly)
          return (LoginResponse) LoginReturnCode.SSO_USER_PASSWORD_NOT_ALLOWED;
        if (userId == "admin")
        {
          bool admin = this.applyLockOutSettingToAdmin();
          if (admin && latestVersion.UserInfo.Locked && this.stillLockedOut(latestVersion))
            return new LoginResponse()
            {
              ReturnCode = LoginReturnCode.USER_LOCKED,
              AdditionalInfo = this.getAdditionalInfo(latestVersion)
            };
          if (s2s)
          {
            if (!this.IsValidConnection(password))
              return (LoginResponse) LoginReturnCode.PASSWORD_MISMATCH;
          }
          else if (!latestVersion.ComparePassword(password))
          {
            if (!latestVersion.UserInfo.Locked & admin)
            {
              using (User user = UserStore.CheckOut(userId))
                LoginManager.incrementFailedLoginAttempts(user);
            }
            return (LoginResponse) LoginReturnCode.PASSWORD_MISMATCH;
          }
        }
        else
        {
          if (latestVersion.UserInfo.Status != UserInfo.UserStatusEnum.Enabled)
            return (LoginResponse) LoginReturnCode.USER_DISABLED;
          if (latestVersion.UserInfo.Locked && !latestVersion.UserInfo.LastLockedOutDateTime.HasValue)
            return (LoginResponse) LoginReturnCode.USER_LOGIN_DISABLED;
          if (latestVersion.UserInfo.Locked && this.stillLockedOut(latestVersion))
            return new LoginResponse()
            {
              ReturnCode = LoginReturnCode.USER_LOCKED,
              AdditionalInfo = this.getAdditionalInfo(latestVersion)
            };
          if (s2s)
          {
            if (!this.IsValidConnection(password))
              return (LoginResponse) LoginReturnCode.PASSWORD_MISMATCH;
          }
          else if (!latestVersion.ComparePassword(password))
          {
            if (!latestVersion.UserInfo.Locked)
            {
              using (User user = UserStore.CheckOut(userId))
                LoginManager.incrementFailedLoginAttempts(user);
            }
            return (LoginResponse) LoginReturnCode.PASSWORD_MISMATCH;
          }
          if (latestVersion.UserInfo.UserPersonas == null || latestVersion.UserInfo.UserPersonas.Length == 0)
            return (LoginResponse) LoginReturnCode.PERSONA_NOT_FOUND;
          if (latestVersion.UserInfo.UserType.ToLower().Equals("external"))
            return (LoginResponse) LoginReturnCode.TPO_LOGIN_RESTRICTED;
        }
      }
      this.UpdateUserLoginInfo(userId, encompassVersion);
      return (LoginResponse) LoginReturnCode.SUCCESS;
    }

    private void UpdateUserLoginInfo(string userId, string encompassVersion)
    {
      BackgroundActionRunner.EnqueueAction(string.Format("UpdateLoginInfo-{0}", (object) userId), (IClientContext) ClientContext.GetCurrent(), true, (Action<IClientContext>) (context =>
      {
        try
        {
          IDisposable hzcLockObj;
          if (!context.Cache.TryGetLock(userId + "_reset-login", EllieMae.EMLite.ClientServer.LockType.ReaderWriter, 0, out hzcLockObj, true) || hzcLockObj == null)
            return;
          using (hzcLockObj)
          {
            using (User user = UserStore.CheckOut(userId))
            {
              user.FailedLoginAttempts = 0;
              user.UserInfo.LastLogin = DateTime.Now;
              user.UserInfo.LastLockedOutDateTime = new DateTime?();
              if ((encompassVersion ?? "") != "")
                user.UserInfo.EncompassVersion = encompassVersion;
              user.ResetLoginInfo();
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(LoginManager.sw, TraceLevel.Error, nameof (LoginManager), "Error while UpdateUserLoginInfo() for userId :: '" + userId + "::Exception::" + ex.Message);
        }
      }));
    }

    private bool applyLockOutSettingToAdmin()
    {
      return (EnableDisableSetting) ClientContext.GetCurrent().Settings.GetServerSetting("Password.PasswordFailureLockoutForAdmin") == EnableDisableSetting.Enabled;
    }

    private bool IsMFALoginEnabled()
    {
      return (EnableDisableSetting) ClientContext.GetCurrent().Settings.GetServerSetting("Password.MFALogin") == EnableDisableSetting.Enabled;
    }

    private bool IsSSOLoginEnabled()
    {
      return (EnableDisableSetting) ClientContext.GetCurrent().Settings.GetServerSetting("Password.SSOLogin") == EnableDisableSetting.Enabled;
    }

    private bool IsSmartClientTokenLoginOnlyEnabled(IClientContext context, string instance)
    {
      string tokenLoginOnly = this.getTokenLoginOnly(context, instance);
      return !string.IsNullOrWhiteSpace(tokenLoginOnly) && tokenLoginOnly != "0";
    }

    private string getTokenLoginOnly(IClientContext context, string instance)
    {
      string name = string.Format("TokenLoginOnly_{0}", (object) instance);
      string attribute = (string) context.Cache.Get(name);
      if (string.IsNullOrWhiteSpace(attribute))
      {
        try
        {
          attribute = SmartClientUtils.GetAttribute(instance, "EncompassBE", "AppLauncher.exe", "TokenLoginOnly");
          context.Cache.Put(name, (object) attribute);
        }
        catch (Exception ex)
        {
          try
          {
            Tracing.Log(LoginManager.sw, TraceLevel.Warning, nameof (LoginManager), "Error retrieving SmartClientUtils.GetAttribute for TokenLoginOnly '" + ex.Message);
          }
          catch
          {
          }
        }
      }
      return attribute;
    }

    private string getAdditionalInfo(User user)
    {
      if ((EnableDisableSetting) ClientContext.GetCurrent().Settings.GetServerSetting("Password.PasswordFailureLockout") == EnableDisableSetting.Disabled)
        return string.Empty;
      TimeSpan timeSpan = this.getTimeSpanForLockoutSetting() - (DateTime.Now - user.UserInfo.LastLockedOutDateTime.Value);
      return timeSpan.Minutes.ToString() + " minutes and " + timeSpan.Seconds.ToString() + " seconds";
    }

    private bool stillLockedOut(User user)
    {
      return (EnableDisableSetting) ClientContext.GetCurrent().Settings.GetServerSetting("Password.PasswordFailureLockout") == EnableDisableSetting.Disabled || user.UserInfo.LastLockedOutDateTime.HasValue && DateTime.Now <= user.UserInfo.LastLockedOutDateTime.Value.Add(this.getTimeSpanForLockoutSetting());
    }

    private TimeSpan getTimeSpanForLockoutSetting()
    {
      return new TimeSpan(0, (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.PasswordFailureLockoutPeriod"), 0);
    }

    private LoginReturnCode validateTrustedUserLogin(object remoteObject)
    {
      if (System.Runtime.Remoting.RemotingServices.IsTransparentProxy(remoteObject))
        Err.Raise(TraceLevel.Warning, nameof (LoginManager), (EllieMae.EMLite.ClientServer.Exceptions.ServerException) new SecurityException("Local connection attempted by remote client", (SessionInfo) null));
      return LoginReturnCode.SUCCESS;
    }

    public static void IncrementFailedLoginAttempts(User user)
    {
      LoginManager.incrementFailedLoginAttempts(user);
    }

    private static void incrementFailedLoginAttempts(User user)
    {
      ++user.FailedLoginAttempts;
      int serverSetting1 = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.MaxLoginFailures");
      if (serverSetting1 > 0 && user.FailedLoginAttempts >= serverSetting1)
      {
        user.FailedLoginAttempts = 0;
        user.UserInfo.Locked = true;
        int serverSetting2 = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.PasswordFailureLockout");
        user.UserInfo.LastLockedOutDateTime = new DateTime?(DateTime.Now);
      }
      user.CheckIn(true, "<system>", true);
    }

    private bool areLoginsEnabled(ClientContext context)
    {
      return (bool) context.Settings.GetServerSetting("Login.Enabled");
    }

    private bool IsValidConnection(string connectionString)
    {
      SqlConnectionStringBuilder connectionStringBuilder1 = new SqlConnectionStringBuilder(ClientContext.GetCurrent().Settings.ConnectionString);
      SqlConnectionStringBuilder connectionStringBuilder2 = new SqlConnectionStringBuilder(connectionString);
      return string.Equals(connectionStringBuilder1.DataSource, connectionStringBuilder2.DataSource, StringComparison.OrdinalIgnoreCase) && string.Equals(connectionStringBuilder1.InitialCatalog, connectionStringBuilder2.InitialCatalog, StringComparison.OrdinalIgnoreCase) && EllieMae.EMLite.DataAccess.DbAccessManager.ValidateConnection(connectionString);
    }

    public override sealed object InitializeLifetimeService() => base.InitializeLifetimeService();

    public override sealed ObjRef CreateObjRef(Type requestedType)
    {
      return base.CreateObjRef(requestedType);
    }

    public static bool ExistsConcurrentUserLogin(string userID, string appName, string sessionID = "")
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM [Sessions] WHERE [UserID] = {0} AND [AppName] = {1} AND [SessionID] != {2}", (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(userID), (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(appName), (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(sessionID)));
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0;
    }

    private class LoginCallProcessingInfo : IRemoteCallLogDecorator
    {
      public bool IsTokenLoginFlow { get; set; }

      public string ClientIpAddress { get; set; }

      public void Decorate(Encompass.Diagnostics.Logging.Schema.Log log)
      {
        log.Set<bool>(LogFields.Field<bool>("tokenLoginFlow"), this.IsTokenLoginFlow);
        log.Set<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.ClientIpAddress, this.ClientIpAddress);
      }
    }
  }
}
