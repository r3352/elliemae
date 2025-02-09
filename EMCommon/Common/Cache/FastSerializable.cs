// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Cache.FastSerializable
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.RemotingServices;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.Cache
{
  public abstract class FastSerializable : ISerializable
  {
    private readonly string _version;
    private const string NullString = "\0";

    public FastSerializable(string version) => this._version = version;

    public FastSerializable(
      string expectedVersion,
      SerializationInfo info,
      StreamingContext context)
    {
      using (MemoryStream input = new MemoryStream((byte[]) info.GetValue("bytes", typeof (byte[]))))
      {
        using (FastSerializable.InnerBinaryReader br = new FastSerializable.InnerBinaryReader((Stream) input, Encoding.UTF8, true))
        {
          string a = br.ReadString();
          this._version = string.Equals(a, expectedVersion) ? expectedVersion : throw new SerializationException("Version mismatch. Expected " + expectedVersion + ", Actual" + a);
          this.Initialize((BinaryReader) br);
        }
      }
    }

    protected abstract void Initialize(BinaryReader br);

    protected abstract void WriteBytes(BinaryWriter bw);

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      using (ByteStream output = new ByteStream(false))
      {
        using (FastSerializable.InnerBinaryWriter bw = new FastSerializable.InnerBinaryWriter((Stream) output, Encoding.UTF8, true))
        {
          bw.Write(this._version);
          this.WriteBytes((BinaryWriter) bw);
          bw.Flush();
        }
        info.AddValue("bytes", (object) output.ToArray());
      }
    }

    private class InnerBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : BinaryWriter(output, encoding, leaveOpen)
    {
      public override void Write(string value)
      {
        if (value == null)
          value = "\0";
        base.Write(value);
      }
    }

    private class InnerBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : BinaryReader(input, encoding, leaveOpen)
    {
      public override string ReadString()
      {
        string a = base.ReadString();
        return !string.Equals(a, "\0") ? a : (string) null;
      }
    }
  }
}
