// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.EncBeforeNavigationEventArgs
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public class EncBeforeNavigationEventArgs : EventArgs
  {
    private bool hasUserGesture;
    private bool isExternalProtocol;
    private bool isMainFrame;
    private bool isPost;
    private bool isRedirect;
    private string url;
    private bool isCancelled;

    public EncBeforeNavigationEventArgs(
      bool hasUserGesture,
      bool isExternalProtocol,
      bool isMainFrame,
      bool isPost,
      bool isRedirect,
      string url,
      bool isCancelled)
    {
      this.hasUserGesture = hasUserGesture;
      this.isExternalProtocol = isExternalProtocol;
      this.isMainFrame = isMainFrame;
      this.isPost = isPost;
      this.isRedirect = isRedirect;
      this.url = url;
      this.isCancelled = isCancelled;
    }

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
  }
}
