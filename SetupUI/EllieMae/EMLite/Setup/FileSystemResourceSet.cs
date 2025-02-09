// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FileSystemResourceSet
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FileSystemResourceSet : IResourceSet
  {
    protected FileSystemEntry[] files;
    protected FileSystemEntry[] folders;
    protected int groupId = -1;
    protected AclFileType fileType = AclFileType.LoanProgram;

    public FileSystemResourceSet(
      int groupId,
      AclFileType fileType,
      AclFileResource[] fileResources)
    {
      this.groupId = groupId;
      this.fileType = fileType;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < fileResources.Length; ++index)
      {
        FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(fileResources[index].FilePath);
        if (fileResources[index].IsFolder)
          arrayList2.Add((object) fileSystemEntry);
        else
          arrayList1.Add((object) fileSystemEntry);
      }
      this.files = (FileSystemEntry[]) arrayList1.ToArray(typeof (FileSystemEntry));
      this.folders = (FileSystemEntry[]) arrayList2.ToArray(typeof (FileSystemEntry));
    }

    public int GroupID
    {
      get => this.groupId;
      set => this.groupId = value;
    }

    public AclFileType FileType
    {
      get => this.fileType;
      set => this.fileType = value;
    }

    public FileSystemEntry[] Files
    {
      get => this.files;
      set => this.files = value;
    }

    public FileSystemEntry[] Folders
    {
      get => this.folders;
      set => this.folders = value;
    }
  }
}
