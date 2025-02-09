// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.WebhookEventConsts
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class WebhookEventConsts
  {
    public const string WEBHOOK_EVENT_XAPI_KEY_NAME = "x-api-key�";
    public const string WEBHOOK_EVENT_SOURCE = "urn:elli:encompass:�";
    private WebhookResource _eventResource;

    public WebhookEventConsts(WebhookResource eventResource) => this._eventResource = eventResource;

    public string EventResourceName => this._eventResource.ToString();

    public string EventEndPoint
    {
      get
      {
        switch (this._eventResource)
        {
          case WebhookResource.trade:
            return "/platform/v1/events/trade/";
          case WebhookResource.externalOrganization:
            return "/platform/v1/events/externalorganization/";
          default:
            return string.Empty;
        }
      }
    }

    public string ResourceRef
    {
      get
      {
        switch (this._eventResource)
        {
          case WebhookResource.trade:
            return "secondary/v1/trades/correspondent/";
          case WebhookResource.externalOrganization:
            return "encompass/v3/externalOrganizations/tpos/";
          default:
            return string.Empty;
        }
      }
    }
  }
}
