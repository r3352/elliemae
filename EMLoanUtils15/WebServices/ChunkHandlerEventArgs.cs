// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ChunkHandlerEventArgs
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  public class ChunkHandlerEventArgs
  {
    private long transferredBytes;
    private long totalBytes;
    private int percent;
    public bool Abort;

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
