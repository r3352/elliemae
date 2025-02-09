// Decompiled with JetBrains decompiler
// Type: RestApiProxy.ProxyBase
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

#nullable disable
namespace RestApiProxy
{
  internal abstract class ProxyBase
  {
    private readonly string _sessionId;
    private readonly string _mediaContentType;

    protected ProxyBase(string sessionId, string mediaContentType)
    {
      this._sessionId = sessionId;
      this._mediaContentType = mediaContentType;
    }

    public string UserName { get; set; }

    public virtual HttpClient BaseObject(string baseUri)
    {
      HttpClient httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri(baseUri);
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(this._mediaContentType));
      return httpClient;
    }

    internal abstract string GetRestApiBaseUri();

    internal virtual string GetAccessToken()
    {
      return Convert.ToBase64String(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject((object) new JsonWebToken()
      {
        elli_inst = this.InstanceId,
        elli_uid = this.UserName,
        session = this.SessionId
      })));
    }

    public virtual string SessionId
    {
      get
      {
        if (string.IsNullOrEmpty(this._sessionId))
          return (string) null;
        string[] strArray = this._sessionId.Split('_');
        Guid result;
        return strArray.Length != 2 ? (!Guid.TryParse(strArray[0], out result) ? (string) null : strArray[0]) : (!Guid.TryParse(strArray[1], out result) ? (string) null : strArray[1]);
      }
    }

    public virtual string InstanceId
    {
      get
      {
        if (string.IsNullOrEmpty(this._sessionId))
          return (string) null;
        string[] strArray = this._sessionId.Split('_');
        return !Guid.TryParse(strArray[0], out Guid _) ? strArray[0] : (string) null;
      }
    }

    public string MediaContentType => this._mediaContentType;

    public virtual T GetConfigValue<T>(string key)
    {
      if (string.IsNullOrEmpty(key))
        throw new ConfigurationErrorsException(string.Format("Configuration key {0} can not be null or empty", (object) key));
      try
      {
        return (T) Convert.ChangeType((object) ConfigurationManager.AppSettings[key], typeof (T));
      }
      catch (Exception ex)
      {
        throw new ConfigurationErrorsException(string.Format("Configuration key {0} is not found in RestApiProxy configuration file.", (object) key));
      }
    }
  }
}
