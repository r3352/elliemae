// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.DownloadResult
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class DownloadResult : IAsyncResult
  {
    private ManualResetEvent e = new ManualResetEvent(false);
    private DownloadStatus downloadStatus;

    public DownloadResult(DownloadStatus status) => this.downloadStatus = status;

    public DownloadStatus DownloadStatus
    {
      get
      {
        lock (this)
          return this.downloadStatus;
      }
      set
      {
        lock (this)
          this.downloadStatus = value;
      }
    }

    public void NotifyComplete(DownloadStatus status)
    {
      lock (this)
      {
        this.downloadStatus = status;
        this.e.Set();
      }
    }

    public object AsyncState => (object) null;

    public bool CompletedSynchronously => false;

    public WaitHandle AsyncWaitHandle => (WaitHandle) this.e;

    public bool IsCompleted
    {
      get
      {
        lock (this)
          return this.downloadStatus == DownloadStatus.Cancelled || this.downloadStatus == DownloadStatus.Complete;
      }
    }
  }
}
