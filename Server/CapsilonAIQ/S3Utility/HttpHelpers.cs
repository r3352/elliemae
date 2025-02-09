// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CapsilonAIQ.S3Utility.HttpHelpers
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.CapsilonAIQ.S3Utility
{
  public static class HttpHelpers
  {
    public static string ReadResponseBody(HttpWebResponse response)
    {
      if (response == null)
        throw new ArgumentNullException(nameof (response), "Value cannot be null");
      string str = string.Empty;
      using (Stream responseStream = response.GetResponseStream())
      {
        if (responseStream != null)
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
            str = streamReader.ReadToEnd();
        }
      }
      return str;
    }

    public static string UrlEncode(string data, bool isPath = false)
    {
      string source = HttpHelpers.ValidUrlSpecialCharacters(isPath);
      StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
      foreach (char ch in Encoding.UTF8.GetBytes(data))
      {
        if (ch >= 'A' && ch <= 'Z')
          stringBuilder.Append(ch);
        else if (ch >= 'a' && ch <= 'z')
          stringBuilder.Append(ch);
        else if (ch >= '0' && ch <= '9')
          stringBuilder.Append(ch);
        else if (source.Contains<char>(ch))
          stringBuilder.Append(ch);
        else
          stringBuilder.Append("%").Append(string.Format("{0:X2}", (object) (int) ch));
      }
      return stringBuilder.ToString();
    }

    public static string ValidUrlSpecialCharacters(bool isPath)
    {
      byte[] bytes;
      if (isPath)
        bytes = new byte[6]
        {
          (byte) 45,
          (byte) 46,
          (byte) 47,
          (byte) 58,
          (byte) 95,
          (byte) 126
        };
      else
        bytes = new byte[4]
        {
          (byte) 45,
          (byte) 46,
          (byte) 95,
          (byte) 126
        };
      return Encoding.ASCII.GetString(bytes);
    }
  }
}
