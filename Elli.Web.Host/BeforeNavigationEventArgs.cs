// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.BeforeNavigationEventArgs
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Navigation.Handlers;
using System;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host
{
  public class BeforeNavigationEventArgs : EventArgs
  {
    private bool hasUserGesture;
    private bool isExternalProtocol;
    private bool isMainFrame;
    private bool isPost;
    private bool isRedirect;
    private string url;
    private bool isCancelled;

    public bool HasUserGesture => this.hasUserGesture;

    public bool IsExternalProtocol => this.isExternalProtocol;

    public bool IsMainFrame => this.isMainFrame;

    public bool IsPost => this.isPost;

    public bool IsRedirect => this.isRedirect;

    public string Url => this.url;

    public bool Cancel
    {
      get => this.isCancelled;
      set => this.isCancelled = value;
    }

    public static BeforeNavigationEventArgs GetArgs(StartNavigationParameters p)
    {
      return new BeforeNavigationEventArgs()
      {
        hasUserGesture = p.HasUserGesture,
        isExternalProtocol = p.IsExternalProtocol,
        isMainFrame = p.IsMainFrame,
        isPost = p.IsPost,
        isRedirect = p.IsRedirect,
        url = p.Url,
        isCancelled = false
      };
    }

    public static BeforeNavigationEventArgs GetArgs(WebBrowserNavigatingEventArgs e)
    {
      return new BeforeNavigationEventArgs()
      {
        isMainFrame = true,
        url = e.Url.ToString(),
        isCancelled = e.Cancel
      };
    }
  }
}
