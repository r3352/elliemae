// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Serialization.StreamHelper
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.IO;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.Serialization
{
  public static class StreamHelper
  {
    private static readonly RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();

    public static string ToString(this Stream stream, Encoding encoding, bool leaveOpen)
    {
      stream.Seek(0L, SeekOrigin.Begin);
      using (StreamReader streamReader = new StreamReader(stream, encoding, true, 1024, leaveOpen))
        return streamReader.ReadToEnd();
    }

    public static MemoryStream NewMemoryStream() => StreamHelper.manager.GetStream();

    public static MemoryStream NewMemoryStream(byte[] buffer)
    {
      return StreamHelper.manager.GetStream(buffer);
    }
  }
}
