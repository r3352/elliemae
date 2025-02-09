// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DownloadProgressEventArgs
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DownloadProgressEventArgs : EventArgs
  {
    private long bytesDownloaded;
    private long totalSize;
    private bool cancel;

    public DownloadProgressEventArgs(long bytesDownloaded, long totalSize)
    {
      this.bytesDownloaded = bytesDownloaded;
      this.totalSize = totalSize;
    }

    public long BytesDownloaded => this.bytesDownloaded;

    public long TotalSize => this.totalSize;

    public float PercentComplete
    {
      get => Convert.ToSingle(this.bytesDownloaded) / Convert.ToSingle(this.totalSize);
    }

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
