// Decompiled with JetBrains decompiler
// Type: Jose.Compact
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.Text;

#nullable disable
namespace Jose
{
  public class Compact
  {
    public static string Serialize(params byte[][] parts)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte[] part in parts)
        stringBuilder.Append(Base64Url.Encode(part)).Append(".");
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    public static byte[][] Parse(string token)
    {
      string[] strArray = token.Split('.');
      byte[][] numArray = new byte[strArray.Length][];
      for (int index = 0; index < strArray.Length; ++index)
        numArray[index] = Base64Url.Decode(strArray[index]);
      return numArray;
    }
  }
}
