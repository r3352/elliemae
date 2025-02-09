// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.FileCompressor
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class FileCompressor
  {
    private FileCompressor()
    {
    }

    private static void zipFile(ZipEntry entry, string file, ZipOutputStream zos)
    {
      byte[] buffer = File.ReadAllBytes(file);
      entry.Size = (long) buffer.Length;
      entry.DateTime = File.GetLastWriteTime(file);
      Crc32 crc32 = new Crc32();
      crc32.Reset();
      crc32.Update(buffer);
      entry.Crc = crc32.Value;
      zos.PutNextEntry(entry);
      ((Stream) zos).Write(buffer, 0, buffer.Length);
    }

    public static void ZipFiles(string srcDir, string[] files, string destZipFilePath)
    {
      using (ZipOutputStream zos = new ZipOutputStream((Stream) File.Create(destZipFilePath)))
      {
        zos.SetLevel(6);
        foreach (string file in files)
          FileCompressor.zipFile(new ZipEntry(file), Path.Combine(srcDir, file), zos);
        ((DeflaterOutputStream) zos).Finish();
        ((Stream) zos).Close();
      }
    }

    public static void ZipFile(string srcDir, string srcFileName, string destPath)
    {
      FileCompressor.ZipFiles(srcDir, new string[1]
      {
        srcFileName
      }, destPath);
    }

    public static byte[] ZipBuffer(byte[] buffer)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) memoryStream))
        {
          zipOutputStream.PutNextEntry(new ZipEntry("embedded_stream"));
          ((Stream) zipOutputStream).Write(buffer, 0, buffer.Length);
          ((DeflaterOutputStream) zipOutputStream).Finish();
          return memoryStream.ToArray();
        }
      }
    }

    public static byte[] UnzipBuffer(byte[] buffer)
    {
      using (ZipInputStream zipInputStream = new ZipInputStream((Stream) new MemoryStream(buffer)))
      {
        byte[] buffer1 = new byte[zipInputStream.GetNextEntry().Size];
        int offset = 0;
        int num;
        while (offset < buffer1.Length && (num = ((Stream) zipInputStream).Read(buffer1, offset, buffer1.Length - offset)) > 0)
          offset += num;
        ((Stream) zipInputStream).Close();
        return buffer1;
      }
    }

    public static bool Unzip(string srcPath, string destDir)
    {
      return FileCompressor.Unzip(srcPath, destDir, (IProgressBar) null);
    }

    public static bool Unzip(string srcPath, string destDir, IProgressBar progBar)
    {
      bool flag = true;
      if (!File.Exists(srcPath))
        return false;
      ZipInputStream zipInputStream = (ZipInputStream) null;
      try
      {
        progBar?.ShowProgressBar();
        zipInputStream = new ZipInputStream((Stream) File.OpenRead(srcPath));
        ZipEntry nextEntry;
        while ((nextEntry = zipInputStream.GetNextEntry()) != null)
        {
          string str = Path.Combine(destDir, Path.GetDirectoryName(nextEntry.Name));
          string fileName = Path.GetFileName(nextEntry.Name);
          if (str.Length > 0 && !Directory.Exists(str))
            Directory.CreateDirectory(str);
          if (fileName != string.Empty)
          {
            using (FileStream fileStream = File.Create(Path.Combine(str, fileName)))
            {
              if (progBar != null)
              {
                progBar.Title = "Unzipping " + nextEntry.Name;
                progBar.Minimum = 0;
                progBar.Maximum = (int) nextEntry.Size;
                progBar.Value = 0;
              }
              byte[] buffer = new byte[2048];
              int num = 0;
              int count;
              while ((count = ((Stream) zipInputStream).Read(buffer, 0, buffer.Length)) > 0)
              {
                fileStream.Write(buffer, 0, count);
                num += count;
                if (progBar != null)
                  progBar.Value = num;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        flag = false;
      }
      finally
      {
        ((Stream) zipInputStream)?.Close();
        progBar?.CloseProgressBar();
      }
      return flag;
    }
  }
}
