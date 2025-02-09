// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.IBinaryObject
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.IO;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public interface IBinaryObject
  {
    bool DisposeAfterSerialization { get; set; }

    bool IsDisposed { get; }

    long Length { get; }

    bool RequiresDownload { get; }

    bool RequiresTransferChunking { get; }

    event DownloadProgressEventHandler DownloadProgress;

    event DownloadProgressEventHandler UploadProgress;

    BinaryObject Append(BinaryObject o);

    Stream AsStream();

    BinaryObject Clone();

    void Dispose();

    void DisposeDeserialized();

    void Download();

    byte[] GetBytes();

    void GetObjectData(SerializationInfo info, StreamingContext context);

    Stream OpenStream();

    T ToObject<T>();

    string ToString(Encoding encoding);

    BinaryObject Unzip();

    void Write(Stream stream);

    void Write(string path);

    BinaryObject Zip();
  }
}
