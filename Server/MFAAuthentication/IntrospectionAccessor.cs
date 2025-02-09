// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.MFAAuthentication.IntrospectionAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.MFAAuthentication
{
  public class IntrospectionAccessor
  {
    private const string className = "IntrospectionAccessor�";
    private static readonly string sw = Tracing.SwCommon;

    public static IntrospectionDetails GetSessionIDFromAccessTokenIntrospection(string accessToken)
    {
      string requestUriString = EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token/introspection";
      IntrospectionDetails tokenIntrospection = new IntrospectionDetails();
      try
      {
        string appSetting1 = EnConfigurationSettings.AppSettings["oAuth.ClientId"];
        string appSetting2 = EnConfigurationSettings.AppSettings["oAuth.ClientSecret"];
        WebRequest webRequest = WebRequest.Create(requestUriString);
        webRequest.Method = "POST";
        webRequest.ContentType = "application/x-www-form-urlencoded";
        byte[] bytes = Encoding.UTF8.GetBytes("token=" + accessToken + "&client_id=" + appSetting1 + "&client_secret=" + appSetting2);
        webRequest.ContentLength = (long) bytes.Length;
        using (Stream requestStream = webRequest.GetRequestStream())
        {
          requestStream.Write(bytes, 0, bytes.Length);
          requestStream.Close();
        }
        string end;
        using (StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        Introspection introspection = JsonConvert.DeserializeObject<Introspection>(end);
        if (!string.IsNullOrWhiteSpace(introspection.bearer_token))
        {
          string[] source = introspection.bearer_token.Split('_');
          if (((IEnumerable<string>) source).Count<string>() == 2)
          {
            tokenIntrospection.SessionID = source[1];
          }
          else
          {
            Exception exception = new Exception(string.Format("IntrospectionAccessor bearer token is not in correct format. bearer_token : {0}", (object) introspection.bearer_token));
          }
        }
        else
        {
          Exception exception1 = new Exception(string.Format("IntrospectionAccessor bearer token returned as null"));
        }
        tokenIntrospection.UserName = introspection.user_name;
      }
      catch (Exception ex)
      {
        Tracing.Log(IntrospectionAccessor.sw, nameof (IntrospectionAccessor), TraceLevel.Error, "Login Exception in GetSessionIDFromAccessTokenIntrospection()" + ex.ToString());
        return (IntrospectionDetails) null;
      }
      return tokenIntrospection;
    }
  }
}
