// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.BinaryObject
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  [Serializable]
  public class BinaryObject : ISerializable
  {
    private static int DownloadBlockSize = 400000;
    private const string className = "BinaryObject";
    private byte[] localData;
    private BinaryObject.DataProvider localProvider;
    private BinaryObject.DataProvider remoteProvider;

    public event DownloadProgressEventHandler DownloadProgress;

    public event DownloadProgressEventHandler UploadProgress;

    static BinaryObject()
    {
      int num = -1;
      if (num <= 0)
        return;
      BinaryObject.DownloadBlockSize = num;
    }

    public BinaryObject(string path)
    {
      using (FileStream dataStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        this.localData = new BinaryObject.DataProvider((Stream) dataStream).ReadAll();
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      lock (this)
      {
        if (this.remoteProvider != null)
          this.Download();
        if (this.localProvider != null)
        {
          BinaryObject.DataProvider dataProvider = (BinaryObject.DataProvider) this.localProvider.Clone();
          dataProvider.UploadProgress += new DownloadProgressEventHandler(this.onUploadProgress);
          info.AddValue("data", (object) null);
          info.AddValue("size", this.localProvider.Length);
          info.AddValue("provider", (object) RemotingServices.Marshal((MarshalByRefObject) dataProvider));
        }
        else if (this.localData.Length <= BinaryObject.DownloadBlockSize)
        {
          info.AddValue("data", this.localData.Length == 0 ? (object) (byte[]) null : (object) this.localData);
          info.AddValue("size", this.localData.Length);
          info.AddValue("provider", (object) null);
        }
        else
        {
          BinaryObject.DataProvider dataProvider = new BinaryObject.DataProvider(this.localData);
          dataProvider.UploadProgress += new DownloadProgressEventHandler(this.onUploadProgress);
          info.AddValue("data", (object) null);
          info.AddValue("size", this.localData.Length);
          info.AddValue("provider", (object) RemotingServices.Marshal((MarshalByRefObject) dataProvider));
        }
      }
    }

    public byte[] Data
    {
      get
      {
        lock (this)
        {
          if (this.remoteProvider != null)
            this.Download();
          else if (this.localData == null)
            this.cacheLocalData();
          return this.localData;
        }
      }
    }

    public string ToString(Encoding encoding) => encoding.GetString(this.Data);

    public override string ToString() => this.ToString(Encoding.Default);

    public void Download()
    {
      lock (this)
      {
        if (this.remoteProvider == null)
          return;
        byte[] dst = new byte[this.remoteProvider.Length];
        int num = 0;
        this.raiseDownloadProgressEvent(0, dst.Length);
        this.remoteProvider.Position = 0L;
        byte[] src;
        while ((src = this.remoteProvider.Read(BinaryObject.DownloadBlockSize)) != null)
        {
          int length = src.Length;
          Buffer.BlockCopy((Array) src, 0, (Array) dst, num, length);
          num += length;
          if (!this.raiseDownloadProgressEvent(num, dst.Length))
          {
            this.remoteProvider.Reset();
            throw new CanceledOperationException();
          }
        }
        this.localData = dst;
        this.remoteProvider.NotifyTransferComplete();
        this.remoteProvider = (BinaryObject.DataProvider) null;
      }
    }

    private void cacheLocalData()
    {
      this.localData = this.localProvider.ReadAll();
      this.localProvider.Dispose();
      this.localProvider = (BinaryObject.DataProvider) null;
    }

    private bool raiseDownloadProgressEvent(int bytesDownloaded, int totalBytes)
    {
      if (this.DownloadProgress == null)
        return true;
      DownloadProgressEventArgs e = new DownloadProgressEventArgs((long) bytesDownloaded, (long) totalBytes);
      this.DownloadProgress((object) this, e);
      return !e.Cancel;
    }

    private void onUploadProgress(object source, DownloadProgressEventArgs e)
    {
      if (this.UploadProgress == null)
        return;
      this.UploadProgress((object) this, e);
    }

    private class DataProvider : MarshalByRefObject, ICloneable
    {
      private Stream innerStream;

      public event DownloadProgressEventHandler UploadProgress;

      public DataProvider(byte[] data) => this.innerStream = (Stream) new MemoryStream(data, false);

      public DataProvider(Stream dataStream) => this.innerStream = dataStream;

      public long Length => this.innerStream.Length;

      public long Position
      {
        get => this.innerStream.Position;
        set => this.innerStream.Position = value;
      }

      public byte[] Read(int count)
      {
        if (!this.raiseUploadProgressEvent((int) this.innerStream.Position, (int) this.innerStream.Length))
        {
          this.clearEventListeners();
          this.Reset();
          throw new CanceledOperationException();
        }
        int count1 = (int) Math.Min((long) count, this.innerStream.Length - this.innerStream.Position);
        if (count1 == 0)
          return (byte[]) null;
        byte[] buffer = new byte[count1];
        this.innerStream.Read(buffer, 0, count1);
        return buffer;
      }

      public virtual byte[] ReadAll()
      {
        this.Position = 0L;
        return this.Read((int) this.Length) ?? new byte[0];
      }

      public void Close()
      {
        try
        {
          if (this.innerStream == null)
            return;
          this.innerStream.Close();
          this.innerStream = (Stream) null;
        }
        catch
        {
        }
      }

      public void Reset() => this.innerStream.Position = 0L;

      public virtual void Dispose()
      {
        this.innerStream = (Stream) null;
        this.clearEventListeners();
      }

      private void clearEventListeners()
      {
        try
        {
          if (this.UploadProgress == null)
            return;
          foreach (DownloadProgressEventHandler invocation in this.UploadProgress.GetInvocationList())
            this.UploadProgress -= invocation;
        }
        catch
        {
        }
      }

      public void NotifyTransferComplete()
      {
        this.raiseUploadProgressEvent((int) this.innerStream.Length, (int) this.innerStream.Length);
        this.Disconnect();
      }

      public virtual void Disconnect()
      {
        try
        {
          RemotingServices.Disconnect((MarshalByRefObject) this);
        }
        catch
        {
        }
        this.Dispose();
      }

      public virtual object Clone() => (object) new BinaryObject.DataProvider(this.innerStream);

      private bool raiseUploadProgressEvent(int bytesSent, int bytesTotal)
      {
        try
        {
          if (this.UploadProgress != null)
          {
            DownloadProgressEventArgs e = new DownloadProgressEventArgs((long) bytesSent, (long) bytesTotal);
            this.UploadProgress((object) this, e);
            return !e.Cancel;
          }
        }
        catch
        {
        }
        return true;
      }
    }
  }
}
