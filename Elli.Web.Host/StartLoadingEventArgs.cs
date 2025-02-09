// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.StartLoadingEventArgs
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using System;

#nullable disable
namespace Elli.Web.Host
{
  public class StartLoadingEventArgs : EventArgs
  {
    private long parentFrameId;
    private bool mainFrame;
    private string validatedURL;
    private bool errorPage;
    private bool isSameDocument;

    public StartLoadingEventArgs(
      bool mainFrame,
      string validatedURL,
      long parentFrameId,
      bool errorPage,
      bool isSameDocument)
    {
      this.mainFrame = mainFrame;
      this.validatedURL = validatedURL;
      this.parentFrameId = parentFrameId;
      this.errorPage = errorPage;
      this.isSameDocument = isSameDocument;
    }

    public StartLoadingEventArgs(bool mainFrame, string validatedURL, bool isSameDocument)
    {
      this.mainFrame = mainFrame;
      this.validatedURL = validatedURL;
      this.isSameDocument = isSameDocument;
    }

    public bool IsMainFrame => this.mainFrame;

    public string ValidatedURL => this.validatedURL;

    public long ParentFrameId => this.parentFrameId;

    public bool IsErrorPage => this.errorPage;

    public bool IsSameDocument => this.isSameDocument;
  }
}
