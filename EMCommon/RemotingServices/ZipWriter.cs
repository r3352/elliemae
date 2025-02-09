// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ZipWriter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class ZipWriter : ISerializable, IDisposable
  {
    private ZipWriter.ZipWriterProvider localProvider;
    private ZipWriter.ZipWriterProvider remoteProvider;
    private long size;

    public event ZipWriter.FileUploadEventHandler FileUploaded;

    public ZipWriter(string filepath, int compressionLevel)
    {
      this.localProvider = new ZipWriter.ZipWriterProvider(this, filepath, compressionLevel);
    }

    public ZipWriter(string filepath, int compressionLevel, string password)
    {
      this.localProvider = new ZipWriter.ZipWriterProvider(this, filepath, compressionLevel, password);
    }

    public ZipWriter(SerializationInfo info, StreamingContext context)
    {
      this.remoteProvider = (ZipWriter.ZipWriterProvider) System.Runtime.Remoting.RemotingServices.Unmarshal((ObjRef) info.GetValue("provider", typeof (ObjRef)) ?? throw new Exception("Invalid serialized state for ZipWriter"));
    }

    public string Filepath
    {
      get
      {
        return this.localProvider != null ? this.localProvider.Filepath : this.remoteProvider.Filepath;
      }
    }

    public long Size => this.size;

    public void AddFile(string filepath)
    {
      FileInfo fileInfo = new FileInfo(filepath);
      using (BinaryObject data = new BinaryObject(filepath, false))
      {
        if (this.localProvider != null)
          this.size = this.localProvider.AddFile(fileInfo.Name, data);
        else
          this.size = this.remoteProvider.AddFile(fileInfo.Name, data);
      }
    }

    protected virtual void OnFileUploaded(string filename, long size, long time)
    {
      if (this.FileUploaded == null)
        return;
      this.FileUploaded((object) this, new FileUploadEventArgs()
      {
        Name = filename,
        Size = size,
        Time = time
      });
    }

    private void unsubscribeFileUploadedListeners()
    {
      if (this.FileUploaded == null)
        return;
      foreach (Delegate invocation in this.FileUploaded.GetInvocationList())
        this.FileUploaded -= invocation as ZipWriter.FileUploadEventHandler;
    }

    public void CreateZip()
    {
      if (this.localProvider != null)
        this.size = this.localProvider.CreateZip();
      else
        this.size = this.remoteProvider.CreateZip();
    }

    public void CreateZip(string[] fileList)
    {
      string[] entryNameList = new string[fileList.Length];
      BinaryObject[] dataList = new BinaryObject[fileList.Length];
      for (int index = 0; index < fileList.Length; ++index)
      {
        FileInfo fileInfo = new FileInfo(fileList[index]);
        entryNameList[index] = fileInfo.Name;
        dataList[index] = new BinaryObject(fileList[index]);
      }
      if (this.localProvider != null)
        this.size = this.localProvider.CreateZip(entryNameList, dataList);
      else
        this.size = this.remoteProvider.CreateZip(entryNameList, dataList);
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
        this.localProvider = (ZipWriter.ZipWriterProvider) null;
      }
      if (this.remoteProvider == null)
        return;
      this.remoteProvider.Disconnect();
      this.remoteProvider = (ZipWriter.ZipWriterProvider) null;
    }

    public delegate void FileUploadEventHandler(object source, FileUploadEventArgs args);

    private class ZipWriterProvider : MarshalByRefObject
    {
      private string filepath;
      private FileStream stream;
      private ZipOutputStream zipStream;
      private ZipWriter zipWriter;

      public ZipWriterProvider(ZipWriter zipwriter, string filepath, int compressionLevel)
      {
        this.filepath = filepath;
        this.stream = File.Create(this.filepath);
        this.zipStream = new ZipOutputStream((Stream) this.stream);
        this.zipStream.IsStreamOwner = false;
        this.zipStream.SetLevel(compressionLevel);
        this.zipWriter = zipwriter;
      }

      public ZipWriterProvider(
        ZipWriter zipwriter,
        string filepath,
        int compressionLevel,
        string password)
      {
        this.filepath = filepath;
        this.stream = File.Create(this.filepath);
        this.zipStream = new ZipOutputStream((Stream) this.stream);
        this.zipStream.IsStreamOwner = false;
        this.zipStream.SetLevel(compressionLevel);
        this.zipStream.Password = password;
        this.zipWriter = zipwriter;
      }

      public string Filepath => this.filepath;

      public long AddFile(string entryName, BinaryObject data)
      {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        CompressionMethod compressionMethod = CompressionMethod.Deflated;
        if (this.zipStream.GetLevel() == 0)
          compressionMethod = CompressionMethod.Stored;
        ZipEntry entry = new ZipEntry(entryName);
        entry.Size = data.Length;
        entry.CompressionMethod = compressionMethod;
        this.zipStream.PutNextEntry(entry);
        data.AsStream().CopyTo((Stream) this.zipStream);
        this.zipStream.CloseEntry();
        data.DisposeDeserialized();
        stopwatch.Stop();
        this.zipWriter.OnFileUploaded(entryName, entry.Size, stopwatch.ElapsedMilliseconds);
        return this.stream.Length;
      }

      public long CreateZip()
      {
        this.zipStream.Finish();
        long length = this.stream.Length;
        this.stream.Close();
        return length;
      }

      public long CreateZip(string[] entryNameList, BinaryObject[] dataList)
      {
        for (int index = 0; index < entryNameList.Length; ++index)
          this.AddFile(entryNameList[index], dataList[index]);
        return this.CreateZip();
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
        if (this.zipStream != null)
        {
          this.zipStream.Dispose();
          this.zipStream = (ZipOutputStream) null;
        }
        if (this.stream != null)
        {
          this.stream.Dispose();
          this.stream = (FileStream) null;
        }
        this.zipWriter.unsubscribeFileUploadedListeners();
      }
    }
  }
}
