// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.EventObjects.NavigationCompletedEventArgs
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using System;

#nullable disable
namespace Elli.Web.Host.EventObjects
{
  public class NavigationCompletedEventArgs : EventArgs
  {
    private string errorCode;
    private bool hasCommited;
    private bool isErrorPage;
    private bool isSameDocument;
    private string url;
    private bool wasServerRedirect;
    private string frameName;

    public NavigationCompletedEventArgs(
      string frameName,
      string errorCode,
      bool hasCommited,
      bool isErrorPage,
      bool isSameDocument,
      string url,
      bool wasServerRedirect)
    {
      this.frameName = frameName;
      this.errorCode = errorCode;
      this.hasCommited = hasCommited;
      this.isErrorPage = isErrorPage;
      this.isSameDocument = isSameDocument;
      this.url = url;
      this.wasServerRedirect = wasServerRedirect;
    }

    public string FrameName => this.frameName;

    public string ErrorCode => this.errorCode;

    public bool HasCommitted => this.hasCommited;

    public bool IsErrorPage => this.isErrorPage;

    public bool IsSameDocument => this.isSameDocument;

    public string Url => this.url;

    public bool WasServerRedirect => this.wasServerRedirect;
  }
}
