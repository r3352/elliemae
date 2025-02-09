// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BinaryObject
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class BinaryObject : ISerializable, IDisposable, IBinaryObject, IWhiteListRemoteMethodParam
  {
    public static readonly int DownloadBlockSize = 400000;
    public static readonly int DownloadBufferCount = 100;
    public static readonly int MaxPooledBlockSize = 4000000;
    public static readonly BufferPool DownloadBlockPool = (BufferPool) null;
    private Stream localData;
    private bool disposeAfterSerialization;
    private bool isDeserializedObject;
    private BinaryObject.DataProvider remoteProvider;
    private long sizeWhenDisposed = -1;

    public event DownloadProgressEventHandler DownloadProgress;

    public event DownloadProgressEventHandler UploadProgress;

    static BinaryObject()
    {
      int result1;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BinaryObjectBlockSize"]), out result1))
        BinaryObject.DownloadBlockSize = result1;
      int result2;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BinaryObjectBufferCount"]), out result2))
        BinaryObject.DownloadBufferCount = result2;
      int result3;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BinaryObjectMaxPooledBlockSize"]), out result3))
        BinaryObject.MaxPooledBlockSize = result3;
      if (BinaryObject.DownloadBlockSize > BinaryObject.MaxPooledBlockSize)
        return;
      BinaryObject.DownloadBlockPool = new BufferPool("BinaryObjectDownloadPool", BinaryObject.DownloadBlockSize, 0, BinaryObject.DownloadBufferCount);
    }

    public BinaryObject(byte[] data)
    {
      this.localData = data != null ? (Stream) new MemoryStream(data) : throw new ArgumentNullException(nameof (data));
    }

    public BinaryObject(string path, bool readIntoMemory)
    {
      if (readIntoMemory)
        this.localData = (Stream) ByteStream.FromFile(path);
      else
        this.localData = (Stream) new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public BinaryObject(string path)
      : this(path, true)
    {
    }

    public BinaryObject(string data, Encoding encoding)
    {
      this.localData = (Stream) new MemoryStream(encoding.GetBytes(data));
    }

    public BinaryObject(byte[] bytesData, bool readIntoMemory)
      : this((Stream) new MemoryStream(bytesData), readIntoMemory)
    {
    }

    public BinaryObject(Stream inputStream, bool readIntoMemory)
    {
      if (readIntoMemory || !inputStream.CanSeek)
        this.localData = (Stream) ByteStream.FromStream(inputStream, inputStream.CanSeek);
      else
        this.localData = inputStream;
    }

    public BinaryObject(Stream inputStream, bool readIntoMemory, long fileSize)
    {
      if (readIntoMemory || !inputStream.CanSeek)
        this.localData = (Stream) ByteStream.FromStream(inputStream, inputStream.CanSeek, fileSize);
      else
        this.localData = inputStream;
    }

    public BinaryObject(Stream inputStream)
      : this(inputStream, true)
    {
    }

    public BinaryObject(IXmlSerializable serializableObject)
    {
      if (serializableObject == null)
        throw new ArgumentNullException(nameof (serializableObject));
      this.localData = (Stream) new ByteStream(false);
      new XmlSerializer().Serialize(this.localData, (object) serializableObject);
    }

    public BinaryObject(SerializationInfo info, StreamingContext context)
    {
      long num = (long) info.GetValue("size", typeof (long));
      if (num > 0L)
      {
        byte[] buffer = (byte[]) info.GetValue("data", typeof (byte[]));
        if (buffer != null)
          this.localData = (Stream) new MemoryStream(buffer);
      }
      ObjRef objectRef = (ObjRef) info.GetValue("provider", typeof (ObjRef));
      if (objectRef != null)
        this.remoteProvider = (BinaryObject.DataProvider) System.Runtime.Remoting.RemotingServices.Unmarshal(objectRef);
      this.isDeserializedObject = true;
      if (this.localData != null || this.remoteProvider != null)
        return;
      if (num != 0L)
        throw new Exception("Invalid serialized state for BinaryObject");
      this.localData = (Stream) new ByteStream(false);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      this.Download();
      lock (this)
      {
        if (this.localData.Length <= (long) BinaryObject.DownloadBlockSize)
        {
          info.AddValue("data", this.localData.Length == 0L ? (object) (byte[]) null : (object) this.GetBytes());
          info.AddValue("size", this.localData.Length);
          info.AddValue("provider", (object) null);
          if (!this.disposeAfterSerialization)
            return;
          this.Dispose();
        }
        else
        {
          BinaryObject.DataProvider dataProvider = new BinaryObject.DataProvider(this.disposeAfterSerialization ? this.localData : (Stream) ByteStream.FromStream(this.localData, true));
          dataProvider.UploadProgress += new DownloadProgressEventHandler(this.onUploadProgress);
          info.AddValue("data", (object) null);
          info.AddValue("size", this.localData.Length);
          info.AddValue("provider", (object) System.Runtime.Remoting.RemotingServices.Marshal((MarshalByRefObject) dataProvider));
        }
      }
    }

    public virtual void Dispose()
    {
      if (this.localData != null)
      {
        this.sizeWhenDisposed = this.localData.Length;
        this.localData.Dispose();
        this.localData = (Stream) null;
      }
      if (this.remoteProvider == null)
        return;
      this.sizeWhenDisposed = this.remoteProvider.Length;
      this.remoteProvider.Dispose();
      this.remoteProvider = (BinaryObject.DataProvider) null;
    }

    public void DisposeDeserialized()
    {
      if (!this.isDeserializedObject)
        return;
      this.Dispose();
    }

    public bool DisposeAfterSerialization
    {
      get => this.disposeAfterSerialization;
      set => this.disposeAfterSerialization = value;
    }

    public bool IsDisposed
    {
      get
      {
        lock (this)
          return this.localData == null && this.remoteProvider == null;
      }
    }

    public Stream AsStream()
    {
      this.Download();
      this.localData.Position = 0L;
      return this.localData;
    }

    public Stream OpenStream()
    {
      if (this.localData == null)
        this.Download();
      return (Stream) ByteStream.FromStream(this.localData, true);
    }

    public BinaryObject Clone()
    {
      this.Download();
      return new BinaryObject(this.localData, true);
    }

    public byte[] GetBytes()
    {
      this.Download();
      lock (this)
      {
        if (this.localData is ByteStream)
          return ((ByteStream) this.localData).ToArray();
        if (this.localData is MemoryStream)
          return ((MemoryStream) this.localData).ToArray();
        byte[] buffer = new byte[this.localData.Length];
        this.localData.Position = 0L;
        this.localData.Read(buffer, 0, buffer.Length);
        return buffer;
      }
    }

    public long Length
    {
      get
      {
        lock (this)
        {
          if (this.IsDisposed)
            return this.sizeWhenDisposed;
          return this.localData != null ? this.localData.Length : this.remoteProvider.Length;
        }
      }
    }

    public void Write(string path)
    {
      this.Download();
      try
      {
        this.localData.Position = 0L;
        using (FileStream destination = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
          this.localData.CopyTo((Stream) destination);
      }
      catch (Exception ex)
      {
        throw new LocalFileIOException(".  It is currently opened by another process.", ex.InnerException);
      }
    }

    public void Write(Stream stream)
    {
      this.Download();
      this.localData.Position = 0L;
      this.localData.CopyTo(stream);
      stream.Flush();
    }

    public BinaryObject Append(BinaryObject o)
    {
      this.Download();
      o.Download();
      ByteStream inputStream = new ByteStream(this.Length + o.Length);
      this.localData.Position = 0L;
      inputStream.CopyFrom(this.localData);
      o.localData.Position = 0L;
      inputStream.CopyFrom(o.localData);
      inputStream.Position = 0L;
      return new BinaryObject((Stream) inputStream, false);
    }

    public string ToString(Encoding encoding) => this.AsStream().ToString(encoding, true);

    public override string ToString() => this.ToString(Encoding.Default);

    public T ToObject<T>()
    {
      this.localData.Seek(0L, SeekOrigin.Begin);
      return new XmlSerializer().Deserialize<T>(this.localData);
    }

    public BinaryObject Zip()
    {
      return new BinaryObject(FileCompressor.Instance.ZipBuffer(this.GetBytes()));
    }

    public BinaryObject Unzip()
    {
      return new BinaryObject(FileCompressor.Instance.UnzipBuffer(this.GetBytes()));
    }

    public bool RequiresDownload
    {
      get
      {
        lock (this)
          return this.remoteProvider != null;
      }
    }

    public bool RequiresTransferChunking => this.Length > (long) BinaryObject.DownloadBlockSize;

    public void Download()
    {
      if (this.remoteProvider == null)
        return;
      lock (this)
      {
        if (this.remoteProvider == null)
          return;
        int length = (int) this.remoteProvider.Length;
        ByteStream byteStream = new ByteStream((long) length);
        this.raiseDownloadProgressEvent(0, length);
        this.remoteProvider.Position = 0L;
        byte[] buffer;
        while ((buffer = this.remoteProvider.Read(BinaryObject.DownloadBlockSize)) != null)
        {
          byteStream.Write(buffer, 0, buffer.Length);
          if (!this.raiseDownloadProgressEvent((int) byteStream.Length, length))
          {
            this.remoteProvider.Reset();
            throw new CanceledOperationException();
          }
        }
        this.localData = (Stream) byteStream;
        this.localData.Position = 0L;
        this.remoteProvider.NotifyTransferComplete();
        this.remoteProvider = (BinaryObject.DataProvider) null;
      }
    }

    public static BinaryObject Marshal(BinaryObject o)
    {
      if (o != null)
        o.DisposeAfterSerialization = true;
      return o;
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

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) new JObject()
      {
        {
          "byteSize",
          (JToken) this.Length
        }
      });
    }

    private class DataProvider : MarshalByRefObject
    {
      private Stream innerStream;
      private byte[] reusesableBuffer;

      public event DownloadProgressEventHandler UploadProgress;

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
        if (count1 != BinaryObject.DownloadBlockSize || BinaryObject.DownloadBlockPool == null)
        {
          byte[] buffer = new byte[count1];
          this.innerStream.Read(buffer, 0, count1);
          return buffer;
        }
        if (this.reusesableBuffer == null)
          this.reusesableBuffer = BinaryObject.DownloadBlockPool.TakeBuffer();
        this.innerStream.Read(this.reusesableBuffer, 0, count1);
        return this.reusesableBuffer;
      }

      public virtual byte[] ReadAll()
      {
        this.Position = 0L;
        return this.Read((int) this.Length) ?? new byte[0];
      }

      public void Reset() => this.innerStream.Position = 0L;

      public virtual void Dispose()
      {
        if (this.innerStream != null)
        {
          this.innerStream.Dispose();
          this.innerStream = (Stream) null;
        }
        if (this.reusesableBuffer != null)
        {
          BinaryObject.DownloadBlockPool.ReturnBuffer(this.reusesableBuffer);
          this.reusesableBuffer = (byte[]) null;
        }
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
          System.Runtime.Remoting.RemotingServices.Disconnect((MarshalByRefObject) this);
        }
        catch
        {
        }
        this.Dispose();
      }

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
