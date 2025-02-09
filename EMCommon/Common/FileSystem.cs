// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.FileSystem
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class FileSystem
  {
    public const string IllegalChars = "\\/:*?\"<>|";
    public const string IllegalSubobjectChars = "/:*?\"<>|";

    public static bool IsTemplatePublic(string str)
    {
      string str1 = string.Empty;
      if (str != null && str != string.Empty)
      {
        int length = str.ToLower().IndexOf("\\");
        str1 = length <= -1 ? "PUBLIC:" : str.Substring(0, length).ToUpper().Trim();
      }
      return !(str1 == "PRIVATE:") && !(str1 == "PERSONAL:");
    }

    public static string GetFolderLocation(string str)
    {
      if (str == string.Empty)
        return string.Empty;
      string folderLocation = "\\";
      if (str != null && str != string.Empty)
      {
        int startIndex = str.ToLower().IndexOf("\\");
        if (startIndex > -1)
        {
          str = str.Substring(startIndex);
          int num = str.LastIndexOf("\\");
          if (num > -1)
            folderLocation = str.Substring(0, num + 1);
        }
      }
      return folderLocation;
    }

    public static string GetFileLocation(string str)
    {
      if (str == string.Empty)
        return string.Empty;
      string fileLocation = string.Empty;
      if (str != null && str != string.Empty)
      {
        int startIndex = str.ToLower().IndexOf("\\");
        if (startIndex > -1)
          fileLocation = str.Substring(startIndex);
      }
      return fileLocation;
    }

    public static string GetFileName(string str)
    {
      if ((str ?? "") == string.Empty)
        return string.Empty;
      int num = str.LastIndexOf("\\");
      if (num >= 0 && num < str.Length - 1)
        return str.Substring(num + 1);
      return num >= 0 ? "" : str;
    }

    public static bool IsValidFilename(string fileName)
    {
      fileName = (fileName ?? "").Trim();
      if (fileName.Length == 0)
        return false;
      for (int index = 0; index < fileName.Length; ++index)
      {
        if (fileName[index] == '\\')
          return false;
      }
      return true;
    }

    public static bool IsValidName(string name)
    {
      return FileSystem.IsValidFilename(name) && FileSystem.IsValidSubobjectName(name, true);
    }

    public static bool IsValidSubobjectName(string name, bool encode = false)
    {
      switch (name)
      {
        case null:
          return false;
        case "":
          return false;
        default:
          if (encode)
            name = FileSystem.EncodeFilename(name, false);
          byte[] bytes = Encoding.ASCII.GetBytes(name);
          for (int index = 0; index < bytes.Length; ++index)
          {
            if (encode && bytes[index] == (byte) 63 || bytes[index] < (byte) 32 || bytes[index] > (byte) 126)
              return false;
          }
          return "/:*?\"<>|".IndexOfAny(name.ToCharArray()) < 0 && !name.StartsWith(".\\") && !name.StartsWith("..\\") && name.IndexOf("\\.\\") <= 0 && name.IndexOf("\\..\\") <= 0;
      }
    }

    public static string RemoveFileExtension(string file, string ext)
    {
      if (file == string.Empty || file == null || ext == string.Empty || ext == null || ext.Length >= file.Length || file.Length <= ext.Length || !(file.Substring(file.Length - ext.Length).ToUpper() == ext.ToUpper()))
        return file;
      file = file.Substring(0, file.Length - ext.Length);
      return file;
    }

    public static string EncodeFilename(string name, bool allowDirSeperator)
    {
      string str1 = "";
      string str2 = allowDirSeperator ? "/:*?\"<>|" : "\\/:*?\"<>|";
      for (int index = 0; index < name.Length; ++index)
      {
        char ch = name[index];
        byte num;
        if (ch == '%')
        {
          string str3 = str1;
          num = (byte) 37;
          string str4 = num.ToString("X2");
          str1 = str3 + "%" + str4;
        }
        else if (str2.IndexOf(ch) >= 0)
        {
          string str5 = str1;
          num = (byte) ch;
          string str6 = num.ToString("X2");
          str1 = str5 + "%" + str6;
        }
        else
          str1 += ch.ToString();
      }
      return str1;
    }

    public static string DecodeFilename(string name)
    {
      try
      {
        string str = "";
        for (int index = 0; index < name.Length; ++index)
        {
          char ch = name[index];
          if (ch == '%')
          {
            if (index < name.Length - 2)
            {
              try
              {
                str += ((char) byte.Parse(name.Substring(index + 1, 2), NumberStyles.HexNumber)).ToString();
                index += 2;
                continue;
              }
              catch
              {
                str += ch.ToString();
                continue;
              }
            }
          }
          str += ch.ToString();
        }
        return str;
      }
      catch (Exception ex)
      {
        throw new ArgumentException("Invalid encoded file name: " + name);
      }
    }

    public static string Combine(string path, string relativePath)
    {
      if (relativePath.StartsWith("\\"))
        throw new ArgumentException("Relative Path argument cannot start with a directory separator (\\)");
      return path.EndsWith("\\") ? path + relativePath : path + "\\" + relativePath;
    }

    [CLSCompliant(false)]
    public static string GetFilename(string path)
    {
      for (int index = path.Length - 1; index >= 0; --index)
      {
        if (path[index] == '\\')
          return index == path.Length - 1 ? "" : path.Substring(index + 1);
      }
      return path;
    }

    public static string GetFilenameWithoutExtension(string path)
    {
      string filename = FileSystem.GetFilename(path);
      for (int index = filename.Length - 1; index >= 0; --index)
      {
        if (filename[index] == '.')
          return index == 0 ? "" : filename.Substring(0, index);
      }
      return filename;
    }

    public static string StripIllegalCharacters(string name)
    {
      for (int index = 0; index < "\\/:*?\"<>|".Length; ++index)
        name = name.Replace("\\/:*?\"<>|"[index].ToString(), "");
      return name;
    }
  }
}
