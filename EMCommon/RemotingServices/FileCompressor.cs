// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.FileCompressor
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class FileCompressor : MarshalByRefObject
  {
    private static FileCompressor instance;

    public static FileCompressor Instance
    {
      get
      {
        if (FileCompressor.instance == null)
          FileCompressor.instance = new FileCompressor();
        return FileCompressor.instance;
      }
    }

    private string getRelativePath(string rootDir, string srcDir)
    {
      int num = srcDir.IndexOf(rootDir);
      if (num < 0)
        return srcDir;
      string str = srcDir.Substring(num + rootDir.Length, srcDir.Length - rootDir.Length);
      return str.StartsWith("\\") ? str.Substring(1, str.Length - 1) : str;
    }

    private void zipFile(ZipEntry entry, string file, ZipOutputStream zos)
    {
      byte[] buffer = (byte[]) null;
      using (FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        buffer = new byte[fileStream.Length];
        fileStream.Read(buffer, 0, buffer.Length);
      }
      entry.Size = (long) buffer.Length;
      entry.DateTime = File.GetLastWriteTime(file);
      Crc32 crc32 = new Crc32();
      crc32.Reset();
      crc32.Update(buffer);
      entry.Crc = crc32.Value;
      zos.PutNextEntry(entry);
      zos.Write(buffer, 0, buffer.Length);
    }

    public byte[] Zip(ByteStream inputStream)
    {
      using (ByteStream byteStream = new ByteStream(false))
      {
        using (GZipStream destination = new GZipStream((Stream) byteStream, CompressionLevel.Fastest, true))
        {
          inputStream.Position = 0L;
          inputStream.CopyTo((Stream) destination);
        }
        return byteStream.ToArray();
      }
    }

    public ByteStream Unzip(byte[] bytes)
    {
      ByteStream destination = new ByteStream(false);
      using (GZipStream gzipStream = new GZipStream((Stream) new MemoryStream(bytes), CompressionMode.Decompress, true))
        gzipStream.CopyTo((Stream) destination);
      destination.Position = 0L;
      return destination;
    }

    public byte[] ZipBuffer(byte[] buffer)
    {
      using (MemoryStream baseOutputStream = new MemoryStream())
      {
        using (ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) baseOutputStream))
        {
          zipOutputStream.PutNextEntry(new ZipEntry("embedded_stream"));
          zipOutputStream.Write(buffer, 0, buffer.Length);
          zipOutputStream.Finish();
          return baseOutputStream.ToArray();
        }
      }
    }

    public byte[] UnzipBuffer(byte[] buffer)
    {
      using (ZipInputStream zipInputStream = new ZipInputStream((Stream) new MemoryStream(buffer)))
      {
        byte[] buffer1 = new byte[zipInputStream.GetNextEntry().Size];
        int offset = 0;
        int num;
        while (offset < buffer1.Length && (num = zipInputStream.Read(buffer1, offset, buffer1.Length - offset)) > 0)
          offset += num;
        zipInputStream.Close();
        return buffer1;
      }
    }

    public void ZipFile(string srcPath, string destPath)
    {
      using (ZipOutputStream zos = new ZipOutputStream((Stream) File.Create(destPath)))
      {
        zos.SetLevel(6);
        this.zipFile(new ZipEntry(new FileInfo(srcPath).Name), srcPath, zos);
        zos.Finish();
        zos.Close();
      }
    }

    private void zipSingleDirectory(string rootDir, string srcDir, ZipOutputStream zos)
    {
      bool flag = Path.IsPathRooted(srcDir);
      foreach (string file in Directory.GetFiles(srcDir))
        this.zipFile(!flag ? new ZipEntry(file) : new ZipEntry(this.getRelativePath(rootDir, file)), file, zos);
      foreach (string directory in Directory.GetDirectories(srcDir))
        this.zipSingleDirectory(rootDir, directory, zos);
    }

    public void ZipDirectory(string srcDir, string destPath)
    {
      Directory.GetFiles(srcDir);
      using (ZipOutputStream zos = new ZipOutputStream((Stream) File.Create(destPath)))
      {
        zos.SetLevel(6);
        this.zipSingleDirectory(srcDir, srcDir, zos);
        zos.Finish();
        zos.Close();
      }
    }

    public void Unzip(string srcPath, string destDir)
    {
      if (!File.Exists(srcPath))
        return;
      using (ZipInputStream zipInputStream = new ZipInputStream((Stream) File.OpenRead(srcPath)))
      {
label_17:
        ZipEntry nextEntry;
        while ((nextEntry = zipInputStream.GetNextEntry()) != null)
        {
          string path = destDir + "\\" + Path.GetDirectoryName(nextEntry.Name);
          string fileName = Path.GetFileName(nextEntry.Name);
          if (path.Length > 0 && !Directory.Exists(path))
            Directory.CreateDirectory(path);
          if (fileName != string.Empty)
          {
            using (FileStream fileStream = File.Create(Path.Combine(destDir, nextEntry.Name)))
            {
              byte[] buffer = new byte[2048];
              while (true)
              {
                int count;
                try
                {
                  count = zipInputStream.Read(buffer, 0, buffer.Length);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                  count = 0;
                }
                if (count > 0)
                  fileStream.Write(buffer, 0, count);
                else
                  goto label_17;
              }
            }
          }
        }
        zipInputStream.Close();
      }
    }

    public byte[] ZipString(string text, Encoding encoding)
    {
      return this.ZipBuffer(encoding.GetBytes(text));
    }
  }
}
