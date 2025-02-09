// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DTErrorLogger
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Globalization;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class DTErrorLogger
  {
    public static void WriteLog(
      string userId,
      DateTime dateTime,
      string source,
      string borrowerPair,
      string DisclosureType)
    {
      try
      {
        DTErrorLogHelper dtErrorLogHelper = new DTErrorLogHelper();
        dtErrorLogHelper.WriteLine(userId, dateTime, source, borrowerPair, DisclosureType);
        dtErrorLogHelper.Close();
      }
      catch
      {
      }
    }

    public static string ReadLog()
    {
      try
      {
        DTErrorLogHelper dtErrorLogHelper = new DTErrorLogHelper();
        string str = dtErrorLogHelper.ReadLog();
        dtErrorLogHelper.Close();
        return str;
      }
      catch
      {
        return "";
      }
    }

    public static void Rename()
    {
      DTErrorLogHelper dtErrorLogHelper = new DTErrorLogHelper();
      string logPath = dtErrorLogHelper.LogPath;
      string destFileName = logPath.Substring(0, logPath.Length - 4) + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm", (IFormatProvider) CultureInfo.InvariantCulture) + ".log";
      if (File.Exists(logPath))
        File.Move(logPath, destFileName);
      dtErrorLogHelper.Close();
    }
  }
}
