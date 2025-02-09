// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DirectApiCallRestProxy
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using Elli.DirectoryServices.Contracts.Dto;
using RestApiProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

#nullable disable
namespace EllieMae.EMLite
{
  internal class DirectApiCallRestProxy : ProxyBase, IRestApiProxy
  {
    private readonly IDirectoryService _directoryService;

    internal DirectApiCallRestProxy(string sessionId, string userName, string mediaContentType)
      : base(sessionId, mediaContentType)
    {
      this._directoryService = (IDirectoryService) new DirectoryServiceClient();
      this.UserName = userName;
    }

    public HttpClient GetHttpClient()
    {
      string restApiBaseUri = this.GetRestApiBaseUri();
      string accessToken = this.GetAccessToken();
      HttpClient httpClient = this.BaseObject(restApiBaseUri);
      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("EMAuth", accessToken);
      return httpClient;
    }

    internal override string GetRestApiBaseUri()
    {
      if (this.InstanceId.ToUpper() == "LOCALHOST")
        return "http://localhost";
      try
      {
        DirectoryEntryDto[] entriesInInstance = this._directoryService.GetEntriesInInstance(this.InstanceId);
        if (!((IEnumerable<DirectoryEntryDto>) entriesInInstance).Any<DirectoryEntryDto>())
          return (string) null;
        string configKey = this.GetConfigValue<string>("restApiBaseUriKey").ToUpper();
        return ((IEnumerable<DirectoryEntryDto>) entriesInInstance).FirstOrDefault<DirectoryEntryDto>((Func<DirectoryEntryDto, bool>) (item => item.Name.ToUpper() == configKey))?.Value.ToString();
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Either directory service end-point or restApiBaseUriKey key is not defined in client application's configuration", ex.InnerException);
      }
    }
  }
}
