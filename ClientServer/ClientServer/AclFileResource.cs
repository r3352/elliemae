// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AclFileResource
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AclFileResource
  {
    private int fileId = -1;
    private string filePath = string.Empty;
    private AclFileType fileType = AclFileType.LoanProgram;
    private bool isFolder;
    private string owner = string.Empty;

    public AclFileResource()
    {
    }

    public AclFileResource(
      int fileId,
      string filePath,
      AclFileType fileType,
      bool isFolder,
      string owner)
    {
      this.fileId = fileId;
      this.filePath = filePath;
      this.fileType = fileType;
      this.isFolder = isFolder;
      this.owner = owner;
    }

    public string Owner
    {
      get => this.owner;
      set => this.owner = value;
    }

    public bool IsFolder
    {
      get => this.isFolder;
      set => this.isFolder = value;
    }

    public int FileID
    {
      get => this.fileId;
      set => this.fileId = value;
    }

    public string FilePath
    {
      get => this.filePath;
      set => this.filePath = value;
    }

    public string FileName
    {
      get
      {
        string[] strArray = this.FilePath.Split('\\');
        return strArray == null || strArray.Length == 0 ? this.FilePath : strArray[strArray.Length - 1];
      }
    }

    public AclFileType FileType
    {
      get => this.fileType;
      set => this.fileType = value;
    }

    public string FileTypePath
    {
      get
      {
        return (this.fileType > AclFileType.Reports ? string.Concat((object) (int) this.fileType) : ((int) this.fileType).ToString() + " ") + "|" + (this.isFolder ? "1" : "0") + "|" + this.owner + "|" + this.filePath;
      }
    }
  }
}
