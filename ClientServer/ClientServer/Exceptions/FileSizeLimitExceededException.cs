// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.FileSizeLimitExceededException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class FileSizeLimitExceededException : SystemException
  {
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("FileName", (object) this.FileName);
      info.AddValue("FilePath", (object) this.FilePath);
      info.AddValue("FileSize", this.FileSize);
      info.AddValue("MaxFileSize", this.ThresholdMaxFileSize);
    }

    protected FileSizeLimitExceededException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.FileName = info.GetString(nameof (FileName));
      this.FilePath = info.GetString(nameof (FilePath));
      this.FileSize = info.GetInt64(nameof (FileSize));
      this.ThresholdMaxFileSize = info.GetInt64("MaxFileSize");
    }

    public FileSizeLimitExceededException(
      string message,
      string fileName,
      string filePath,
      long fileSize)
      : base(message)
    {
      this.FileName = fileName;
      this.FilePath = filePath;
      this.FileSize = fileSize;
    }

    public FileSizeLimitExceededException(
      string message,
      string fileName,
      string filePath,
      long fileSize,
      long thresholdMaxFileSize)
      : this(message, fileName, filePath, fileSize)
    {
      this.ThresholdMaxFileSize = thresholdMaxFileSize;
    }

    public string FileName { get; }

    public string FilePath { get; }

    public long FileSize { get; }

    public long ThresholdMaxFileSize { get; } = -1;
  }
}
