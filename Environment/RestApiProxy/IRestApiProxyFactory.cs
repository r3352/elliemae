// Decompiled with JetBrains decompiler
// Type: RestApiProxy.IRestApiProxyFactory
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using System.Net.Http;

#nullable disable
namespace RestApiProxy
{
  public interface IRestApiProxyFactory
  {
    HttpClient CreateGatewayApiHttpClient(string sessionId, string mediaContentType);
  }
}
