// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DownloadProgressEventArgs
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class DownloadProgressEventArgs : EventArgs
  {
    private long bytesDownloaded;
    private long totalSize;
    private bool cancel;

    internal DownloadProgressEventArgs(long bytesDownloaded, long totalSize)
    {
      this.bytesDownloaded = bytesDownloaded;
      this.totalSize = totalSize;
    }

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
