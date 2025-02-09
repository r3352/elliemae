// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.EncFinishLoadingEventArgs
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public class EncFinishLoadingEventArgs : EventArgs
  {
    private long frameId;
    private bool mainFrame;
    private object frameName;
    private string validatedURL;

    [Obsolete]
    public EncFinishLoadingEventArgs(long frameId, bool mainFrame, string validatedURL)
    {
      this.frameId = frameId;
      this.mainFrame = mainFrame;
      this.validatedURL = validatedURL;
    }

    public EncFinishLoadingEventArgs(object frameName, bool mainFrame, string validatedURL)
    {
      this.frameName = frameName;
      this.mainFrame = mainFrame;
      this.validatedURL = validatedURL;
    }

    public long FrameId => this.frameId;

    public object FrameName => this.frameName;

    public bool IsMainFrame => this.mainFrame;

    public string ValidatedURL => this.validatedURL;
  }
}
