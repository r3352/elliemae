// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.XML
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class XML
  {
    private XML()
    {
    }

    public static string Encode(object value)
    {
      return value is string ? XML.EncodeString((string) value) : value.ToString();
    }

    public static string EncodeString(string value)
    {
      return "\"" + value.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;") + "\"";
    }

    public static string EncaseXpathString(string input)
    {
      if (!input.Contains("\""))
        return string.Format("\"{0}\"", (object) input);
      if (!input.Contains("'"))
        return string.Format("'{0}'", (object) input);
      StringBuilder stringBuilder = new StringBuilder("concat(");
      int startIndex = 0;
      for (int index = input.IndexOf("\""); index != -1; index = input.IndexOf("\"", startIndex))
      {
        if (startIndex != 0)
          stringBuilder.Append(",");
        stringBuilder.AppendFormat("\"{0}\",'\"'", (object) input.Substring(startIndex, index - startIndex));
        int num;
        startIndex = num = index + 1;
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }
  }
}
