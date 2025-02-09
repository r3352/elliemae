// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Authentication.OAuth2TokenIntrospection
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

#nullable disable
namespace EllieMae.EMLite.Server.Authentication
{
  public class OAuth2TokenIntrospection
  {
    public bool? active { get; set; }

    public string scope { get; set; }

    public string client_id { get; set; }

    public string username { get; set; }

    public string token_type { get; set; }

    public int? exp { get; set; }

    public string sub { get; set; }

    public string bearer_token { get; set; }

    public string encompass_instance_id { get; set; }

    public string user_name { get; set; }

    public string user_key { get; set; }

    public string encompass_user { get; set; }

    public string identity_type { get; set; }

    public string encompass_instance_type { get; set; }

    public string encompass_client_id { get; set; }

    public string realm_name { get; set; }
  }
}
