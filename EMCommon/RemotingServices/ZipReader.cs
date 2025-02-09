// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ZipReader
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class ZipReader : ISerializable, IDisposable
  {
    private ZipReader.ZipReaderProvider localProvider;
    private ZipReader.ZipReaderProvider remoteProvider;

    public event EventHandler<FileDownloadEventArgs> FileDownloaded;

    public ZipReader(string filepath)
    {
      this.localProvider = new ZipReader.ZipReaderProvider(this, (Stream) ByteStream.FromFile(filepath));
    }

    public void ExtractFiles(string[] entryNameList, string extractPath)
    {
      Queue<string> stringQueue = new Queue<string>((IEnumerable<string>) entryNameList);
      while (stringQueue.Count > 0)
      {
        foreach (BinaryObject binaryObject in this.localProvider == null ? this.remoteProvider.ExtractFiles(stringQueue.ToArray(), (long) BinaryObject.DownloadBlockSize) : this.localProvider.ExtractFiles(stringQueue.ToArray(), 0L))
        {
          string str = stringQueue.Dequeue();
          string path = Path.Combine(extractPath, str);
          binaryObject.Write(path);
          ExtractProgressEventArgs e = new ExtractProgressEventArgs(str, stringQueue.Count, entryNameList.Length);
          this.OnExtractProgress(e);
          if (e.Cancel)
            throw new CanceledOperationException();
        }
      }
    }

    public BinaryObject[] ExtractFiles(string[] entryNameList)
    {
      List<BinaryObject> binaryObjectList = new List<BinaryObject>();
      Queue<string> stringQueue = new Queue<string>((IEnumerable<string>) entryNameList);
      while (stringQueue.Count > 0)
      {
        BinaryObject[] collection = this.localProvider == null ? this.remoteProvider.ExtractFiles(stringQueue.ToArray(), (long) BinaryObject.DownloadBlockSize) : this.localProvider.ExtractFiles(stringQueue.ToArray(), 0L);
        for (int index = 0; index < collection.Length; ++index)
        {
          ExtractProgressEventArgs e = new ExtractProgressEventArgs(stringQueue.Dequeue(), stringQueue.Count, entryNameList.Length);
          this.OnExtractProgress(e);
          if (e.Cancel)
            throw new CanceledOperationException();
        }
        binaryObjectList.AddRange((IEnumerable<BinaryObject>) collection);
      }
      return binaryObjectList.ToArray();
    }

    protected virtual void OnFileDownloaded(string filename, long size, long time)
    {
      if (this.FileDownloaded == null)
        return;
      this.FileDownloaded((object) this, new FileDownloadEventArgs()
      {
        Name = filename,
        Size = size,
        Time = time
      });
    }

    private void unsubscribeFileDownloadListeners()
    {
      if (this.FileDownloaded == null)
        return;
      foreach (Delegate invocation in this.FileDownloaded.GetInvocationList())
        this.FileDownloaded -= invocation as EventHandler<FileDownloadEventArgs>;
    }

    public ZipReader(SerializationInfo info, StreamingContext context)
    {
      this.remoteProvider = (ZipReader.ZipReaderProvider) System.Runtime.Remoting.RemotingServices.Unmarshal((ObjRef) info.GetValue("provider", typeof (ObjRef)) ?? throw new Exception("Invalid serialized state for ZipReader"));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("provider", (object) System.Runtime.Remoting.RemotingServices.Marshal((MarshalByRefObject) this.localProvider));
    }

    public void Dispose()
    {
      if (this.localProvider != null)
      {
        this.localProvider.Dispose();
        this.localProvider = (ZipReader.ZipReaderProvider) null;
      }
      if (this.remoteProvider == null)
        return;
      this.remoteProvider.Disconnect();
      this.remoteProvider = (ZipReader.ZipReaderProvider) null;
    }

    public event ExtractProgressEventHandler ExtractProgress;

    public virtual void OnExtractProgress(ExtractProgressEventArgs e)
    {
      if (this.ExtractProgress == null)
        return;
      this.ExtractProgress((object) this, e);
    }

    private class ZipReaderProvider : MarshalByRefObject
    {
      private Stream stream;
      private ZipFile zipFile;
      private ZipReader zipReader;

      public ZipReaderProvider(ZipReader zipReader, Stream byteStream)
      {
        this.stream = byteStream;
        this.zipFile = new ZipFile(this.stream);
        this.zipReader = zipReader;
      }

      public BinaryObject[] ExtractFiles(string[] entryNameList, long maxChunkSize)
      {
        List<BinaryObject> binaryObjectList = new List<BinaryObject>();
        long num = 0;
        foreach (string entryName in entryNameList)
        {
          Stopwatch stopwatch = new Stopwatch();
          stopwatch.Start();
          ZipEntry entry = this.zipFile.GetEntry(entryName);
          if (entry == null)
            throw new FileNotFoundException(entryName);
          if (num > 0L && maxChunkSize > 0L && num + entry.Size > maxChunkSize)
            return binaryObjectList.ToArray();
          using (Stream inputStream = this.zipFile.GetInputStream(entry))
          {
            BinaryObject binaryObject = BinaryObject.Marshal(new BinaryObject(inputStream, true, entry.Size));
            binaryObjectList.Add(binaryObject);
          }
          num += entry.Size;
          stopwatch.Stop();
          this.zipReader.OnFileDownloaded(entryName, entry.Size, stopwatch.ElapsedMilliseconds);
        }
        return binaryObjectList.ToArray();
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

      public virtual void Dispose()
      {
        if (this.zipFile != null)
        {
          this.zipFile.Close();
          this.zipFile = (ZipFile) null;
        }
        if (this.stream != null)
        {
          this.stream.Dispose();
          this.stream = (Stream) null;
        }
        this.zipReader.unsubscribeFileDownloadListeners();
      }
    }
  }
}
