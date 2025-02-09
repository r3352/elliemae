// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.Interface.IWebFormBrowser
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.EventObjects;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Elli.Web.Host.Interface
{
  public interface IWebFormBrowser
  {
    event EventHandler<BeforeNavigationEventArgs> BeforeNavigation;

    event PageCompleteEventHandler PageComplete;

    event EventHandler<MouseReleasedEventArgs> MouseReleased;

    void Navigate(string url);

    void Navigate(string url, string postData, Dictionary<string, string> headerCollection);

    string GetBrowserHtml([Optional] object frameName);

    void LoadHtml(string html);

    event EventHandler<TitleChangeEventArgs> BrowserTitleChanged;
  }
}
