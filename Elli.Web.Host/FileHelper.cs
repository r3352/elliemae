// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.FileHelper
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using System.IO;

#nullable disable
namespace Elli.Web.Host
{
  public class FileHelper
  {
    private static readonly object fileLocker = new object();

    public static string Read(string path, string fileName)
    {
      lock (FileHelper.fileLocker)
      {
        string str = string.Empty;
        path = Path.Combine(path, fileName);
        if (File.Exists(path))
        {
          using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            str = new StreamReader((Stream) fileStream).ReadToEnd();
        }
        return str;
      }
    }

    public static byte[] ReadBytes(string path, string fileName)
    {
      lock (FileHelper.fileLocker)
      {
        byte[] buffer = (byte[]) null;
        path = Path.Combine(path, fileName);
        if (File.Exists(path))
        {
          using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
          {
            if (fileStream.Length > 0L)
            {
              buffer = new byte[fileStream.Length];
              fileStream.Read(buffer, 0, buffer.Length);
            }
          }
        }
        return buffer;
      }
    }

    public static bool Write(string path, string fileName, string content, bool createDirectory)
    {
      lock (FileHelper.fileLocker)
      {
        bool flag = false;
        if (!DirectoryHelper.IsCreated(path, createDirectory))
          return flag;
        path = Path.Combine(path, fileName);
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        {
          StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
          streamWriter.Write(content);
          streamWriter.Flush();
          flag = true;
        }
        return flag;
      }
    }

    public static bool WriteBytes(
      string path,
      string fileName,
      byte[] content,
      bool createDirectory)
    {
      lock (FileHelper.fileLocker)
      {
        bool flag = false;
        if (!DirectoryHelper.IsCreated(path, createDirectory))
          return flag;
        path = Path.Combine(path, fileName);
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        {
          fileStream.Write(content, 0, content.Length);
          flag = true;
        }
        return flag;
      }
    }
  }
}
