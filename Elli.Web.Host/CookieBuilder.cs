// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.CookieBuilder
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Cookies;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Web.Host
{
  public class CookieBuilder
  {
    public static string Jsonify(IEnumerable<Cookie> cookies)
    {
      return JsonConvert.SerializeObject((object) cookies);
    }

    public static EncWebBrowserCookie[] Cookify(string cookies)
    {
      return JsonConvert.DeserializeObject<EncWebBrowserCookie[]>(cookies);
    }

    public static IEnumerable<Cookie> BuildAll(string cookies)
    {
      Cookie[] source = (Cookie[]) null;
      EncWebBrowserCookie[] webBrowserCookieArray = CookieBuilder.Cookify(cookies);
      if (webBrowserCookieArray != null && webBrowserCookieArray.Length != 0)
      {
        source = new Cookie[webBrowserCookieArray.Length];
        for (int index = 0; index < webBrowserCookieArray.Length; ++index)
        {
          EncWebBrowserCookie webBrowserCookie = webBrowserCookieArray[index];
          Cookie cookie = new Cookie.Builder(webBrowserCookie.DomainName)
          {
            Name = webBrowserCookie.Name,
            Value = webBrowserCookie.Value,
            Path = webBrowserCookie.Path,
            ExpirationTime = webBrowserCookie.ExpirationTime,
            HttpOnly = webBrowserCookie.IsHttpOnly,
            Secure = webBrowserCookie.IsSecure
          }.Build();
          source[index] = cookie;
        }
      }
      return source == null ? (IEnumerable<Cookie>) null : Enumerable.Cast<Cookie>(source);
    }
  }
}
