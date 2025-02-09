// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.EncStartLoadingEventArgs
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public class EncStartLoadingEventArgs : EventArgs
  {
    private long parentFrameId;
    private bool mainFrame;
    private string validatedURL;
    private bool errorPage;
    private bool isSameDocument;

    public EncStartLoadingEventArgs(
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

    public EncStartLoadingEventArgs(bool mainFrame, string validatedURL, bool isSameDocument)
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
