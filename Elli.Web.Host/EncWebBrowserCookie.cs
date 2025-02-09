// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.EncWebBrowserCookie
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Cookies;
using System;

#nullable disable
namespace Elli.Web.Host
{
  public class EncWebBrowserCookie
  {
    public DateTime CreationTime { get; set; }

    public string DomainName { get; set; }

    public DateTime? ExpirationTime { get; set; }

    public bool IsHttpOnly { get; set; }

    public bool IsSecure { get; set; }

    public bool IsSession { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public SameSite SameSite { get; set; }

    public string Value { get; set; }
  }
}
