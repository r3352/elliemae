// Decompiled with JetBrains decompiler
// Type: Jose.DeflateCompression
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.IO;
using System.IO.Compression;

#nullable disable
namespace Jose
{
  public class DeflateCompression : ICompression
  {
    public byte[] Compress(byte[] plainText)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (DeflateStream deflateStream = new DeflateStream((Stream) memoryStream, CompressionMode.Compress))
          deflateStream.Write(plainText, 0, plainText.Length);
        return memoryStream.ToArray();
      }
    }

    public byte[] Decompress(byte[] compressedText)
    {
      using (MemoryStream destination = new MemoryStream())
      {
        using (MemoryStream memoryStream = new MemoryStream(compressedText))
        {
          using (DeflateStream deflateStream = new DeflateStream((Stream) memoryStream, CompressionMode.Decompress))
            deflateStream.CopyTo((Stream) destination);
        }
        return destination.ToArray();
      }
    }
  }
}
