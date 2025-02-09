// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoginParameters
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.VersionInterface15;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoginParameters : IWhiteListRemoteMethodParam
  {
    private string userId;
    private string password;
    private int pwdLength;
    private ServerIdentity serverId;
    private JedVersion clientVersion;
    private string hostname = "";
    private string winUsername = "";
    private OperatingSystem osVersion;
    private string osPlatform = "";
    private ulong totalMemory;
    private string processorId = "";
    private string processorSpeed = "";
    private DateTime currentTime = DateTime.MinValue;
    private string appName = "";
    private bool licenseRequired = true;
    private string clientEnvironment;
    private string connectionId = "";
    private string clientDisplayVersion = "";
    private bool offlineMode;
    private string prevSessionID;
    private string authCode;

    public LoginParameters(
      string userId,
      string password,
      ServerIdentity serverId,
      string connectionId,
      string appName)
      : this(userId, password, serverId, connectionId, appName, true, false, (string) null)
    {
    }

    public LoginParameters(
      string userId,
      string password,
      ServerIdentity serverId,
      string connectionId,
      string appName,
      bool licenseRequired)
      : this(userId, password, serverId, connectionId, appName, licenseRequired, false, (string) null)
    {
    }

    public LoginParameters(
      string userId,
      string password,
      ServerIdentity serverId,
      string connectionId,
      string appName,
      bool licenseRequired,
      string prevSessionID)
      : this(userId, password, serverId, connectionId, appName, licenseRequired, false, prevSessionID)
    {
    }

    public LoginParameters(
      string userId,
      string password,
      ServerIdentity serverId,
      string connectionId,
      string appName,
      bool licenseRequired,
      bool offlineMode)
      : this(userId, password, serverId, connectionId, appName, licenseRequired, offlineMode, (string) null)
    {
    }

    public LoginParameters(
      string userId,
      string password,
      ServerIdentity serverId,
      string connectionId,
      string appName,
      bool licenseRequired,
      string prevSessionID,
      string authCode)
      : this(userId, password, serverId, connectionId, appName, licenseRequired, prevSessionID)
    {
      this.authCode = authCode;
    }

    public LoginParameters(
      string userId,
      string password,
      ServerIdentity serverId,
      string connectionId,
      string appName,
      bool licenseRequired,
      bool offlineMode,
      string prevSessionID)
    {
      this.userId = (userId ?? "").ToLower().Trim();
      this.password = password;
      this.clientVersion = VersionInformation.CurrentVersion.Version;
      this.connectionId = connectionId;
      this.serverId = serverId;
      this.hostname = Dns.GetHostName();
      this.osVersion = Environment.OSVersion;
      this.osPlatform = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") ?? "";
      this.winUsername = Environment.UserName;
      this.currentTime = DateTime.Now;
      this.appName = appName;
      this.licenseRequired = licenseRequired;
      this.clientEnvironment = ClientInfoS.RuntimeEnvAsString;
      this.clientDisplayVersion = VersionInformation.CurrentVersion.DisplayVersion;
      this.totalMemory = SystemUtil.GetMemoryInformation().TotalPhysicalMemory;
      SystemUtil.ProcessorInfo processorInformation = SystemUtil.GetProcessorInformation();
      this.processorId = processorInformation.ProcessorName;
      this.processorSpeed = processorInformation.ProcessorSpeed;
      this.offlineMode = offlineMode;
      this.prevSessionID = prevSessionID;
    }

    public LoginParameters(LoginParameters source, bool copyPasswordInfo)
    {
      this.userId = source.userId;
      if (!copyPasswordInfo)
        this.pwdLength = (source.Password ?? "").Length;
      this.password = copyPasswordInfo ? source.password : "";
      this.serverId = source.serverId;
      this.connectionId = source.connectionId;
      this.clientVersion = source.clientVersion;
      this.hostname = source.hostname;
      this.osVersion = source.osVersion;
      this.osPlatform = source.osPlatform;
      this.winUsername = source.winUsername;
      this.totalMemory = source.totalMemory;
      this.processorId = source.processorId;
      this.processorSpeed = source.processorSpeed;
      this.currentTime = source.currentTime;
      this.licenseRequired = source.licenseRequired;
      this.appName = source.appName;
      this.clientEnvironment = source.ClientEnvironment;
      this.clientDisplayVersion = source.clientDisplayVersion;
      this.offlineMode = source.offlineMode;
      this.prevSessionID = source.prevSessionID;
    }

    public string UserID => this.userId;

    public string AuthCode => this.authCode;

    public string Password => this.password;

    public int PwdLength
    {
      get => !string.IsNullOrEmpty(this.password) ? this.password.Length : this.pwdLength;
    }

    public JedVersion ClientVersion => this.clientVersion;

    public string ClientDisplayVersion => this.clientDisplayVersion;

    public ServerIdentity Server => this.serverId;

    public string ConnectionID => this.connectionId;

    public string Hostname => this.hostname;

    public OperatingSystem OSVersion => this.osVersion;

    public string OSPlatform => this.osPlatform;

    public string WindowUsername => this.winUsername;

    public string ProcessorID => this.processorId;

    public string ProcessorSpeed => this.processorSpeed;

    [CLSCompliant(false)]
    public ulong TotalMemory => this.totalMemory;

    public DateTime ClientTimestamp => this.currentTime;

    public string AppName => this.appName;

    public bool LicenseRequired => this.licenseRequired;

    public string ClientEnvironment => this.clientEnvironment;

    public bool OfflineMode => this.offlineMode;

    public string PrevSessionID => this.prevSessionID;

    public bool IsTrustedServiceApp
    {
      get => this.AppName == "<EncompassServer>" || this.AppName == "trustedEnc";
    }

    public LoginParameters Clone(bool copyPasswordInfo)
    {
      return new LoginParameters(this, copyPasswordInfo);
    }

    public LoginParameters Clone(bool copyPasswordInfo, string username)
    {
      this.userId = username;
      return new LoginParameters(this, copyPasswordInfo);
    }

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) new JObject()
      {
        {
          "userId",
          (JToken) this.UserID
        },
        {
          "hostname",
          (JToken) this.Hostname
        },
        {
          "appName",
          (JToken) this.AppName
        },
        {
          "hasAuthCode",
          (JToken) !string.IsNullOrEmpty(this.AuthCode)
        },
        {
          "hasPassword",
          (JToken) !string.IsNullOrEmpty(this.Password)
        }
      });
    }
  }
}
