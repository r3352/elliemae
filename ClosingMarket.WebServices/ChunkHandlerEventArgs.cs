// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.ChunkHandlerEventArgs
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System;

#nullable disable
namespace ClosingMarket.WebServices
{
  public class ChunkHandlerEventArgs
  {
    private long transferredBytes;
    private long totalBytes;
    private int percent;
    public bool Abort = false;

    public long TransferredBytes => this.transferredBytes;

    public long TotalBytes => this.totalBytes;

    public int Percent => this.percent;

    public ChunkHandlerEventArgs(long transferredBytes, long totalBytes)
    {
      this.transferredBytes = transferredBytes;
      this.totalBytes = totalBytes;
      if (totalBytes == 0L)
        return;
      this.percent = Convert.ToInt32(Convert.ToDouble(transferredBytes) / Convert.ToDouble(totalBytes) * 100.0);
    }
  }
}
