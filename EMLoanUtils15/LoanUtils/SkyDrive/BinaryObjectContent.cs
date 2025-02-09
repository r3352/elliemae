// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.SkyDrive.BinaryObjectContent
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.SkyDrive
{
  public class BinaryObjectContent : HttpContent
  {
    private const int _DefaultCopyBufferSize = 81920;
    private readonly int _BufferSize;
    private readonly long _TotalBytes;
    private readonly long _SentOffset;
    private Stream _Stream;

    public event DownloadProgressEventHandler UploadProgress;

    public BinaryObjectContent(BinaryObject data)
      : this(data, 81920)
    {
    }

    public BinaryObjectContent(BinaryObject data, int bufferSize)
      : this(data, bufferSize, 0L, 0L)
    {
    }

    public BinaryObjectContent(
      BinaryObject data,
      int bufferSize,
      long sentOffset,
      long totalBytes)
    {
      this._Stream = data.OpenStream();
      this._BufferSize = bufferSize;
      this._SentOffset = sentOffset;
      this._TotalBytes = totalBytes;
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
      return Task.Run((Func<Task>) (async () =>
      {
        using (this._Stream)
        {
          byte[] buffer = new byte[this._BufferSize];
          int bytesRead = 0;
          long totalBytes = this._TotalBytes == 0L ? this._Stream.Length : this._TotalBytes;
          long bytesSent = this._SentOffset;
          do
          {
            bytesRead = await this._Stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
              await stream.WriteAsync(buffer, 0, bytesRead);
              bytesSent += Convert.ToInt64(bytesRead);
              if (this.UploadProgress != null)
              {
                DownloadProgressEventArgs e = new DownloadProgressEventArgs(bytesSent, totalBytes);
                this.UploadProgress((object) this, e);
                if (e.Cancel)
                  throw new CanceledOperationException();
              }
            }
          }
          while (bytesRead > 0);
          buffer = (byte[]) null;
        }
      }));
    }

    protected override bool TryComputeLength(out long length)
    {
      length = this._Stream.Length;
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this._Stream.Dispose();
      base.Dispose(disposing);
    }
  }
}
