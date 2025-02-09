// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.StringExtensions
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Serialization;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class StringExtensions
  {
    public static StringBuilder ReplaceWordChars(this StringBuilder sb)
    {
      sb.Replace('‘', '\'');
      sb.Replace('’', '\'');
      sb.Replace('‚', '\'');
      sb.Replace('“', '"');
      sb.Replace('”', '"');
      sb.Replace('„', '"');
      sb.Replace("…", "...");
      sb.Replace('–', '-');
      sb.Replace('—', '-');
      sb.Replace('ˆ', '^');
      sb.Replace('‹', '<');
      sb.Replace('›', '>');
      sb.Replace('˜', ' ');
      sb.Replace(' ', ' ');
      return sb;
    }

    public static Stream ToUtf8StreamWithBOM(this string str)
    {
      return (Stream) new EncodedStringStream(Encoding.UTF8, str, true);
    }
  }
}
