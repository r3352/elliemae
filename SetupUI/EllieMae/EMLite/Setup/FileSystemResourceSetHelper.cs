// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FileSystemResourceSetHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FileSystemResourceSetHelper : IResourceSetHelper
  {
    private Sessions.Session session;

    public FileSystemResourceSetHelper(Sessions.Session session) => this.session = session;

    public void saveResourceSet(
      object resourceSet,
      TreeNodeCollection nodes,
      Dictionary<string, object> updateList)
    {
      FileSystemResourceSet systemResourceSet = (FileSystemResourceSet) resourceSet;
      List<AclFileResource> aclFileResourceList1 = new List<AclFileResource>();
      List<AclFileResource> aclFileResourceList2 = new List<AclFileResource>();
      FileSystemEntry[] update1 = (FileSystemEntry[]) updateList["resetFolders"];
      FileSystemEntry[] update2 = (FileSystemEntry[]) updateList["resetFiles"];
      FileSystemEntry[] update3 = (FileSystemEntry[]) updateList["addFolderList"];
      FileSystemEntry[] update4 = (FileSystemEntry[]) updateList["addInclusiveFolderList"];
      FileSystemEntry[] update5 = (FileSystemEntry[]) updateList["addFileList"];
      if (update1 != null)
      {
        foreach (FileSystemEntry fileSystemEntry in update1)
          aclFileResourceList2.Add(new AclFileResource(-1, fileSystemEntry.ToString(), systemResourceSet.FileType, true, fileSystemEntry.Owner));
      }
      if (update2 != null)
      {
        foreach (FileSystemEntry fileSystemEntry in update2)
          aclFileResourceList1.Add(new AclFileResource(-1, fileSystemEntry.ToString(), systemResourceSet.FileType, false, fileSystemEntry.Owner));
      }
      if (aclFileResourceList2.Count == 0 && aclFileResourceList1.Count == 0)
        return;
      Dictionary<int, AclFileResource> dictionary1 = this.session.AclGroupManager.AddAclFileResources(aclFileResourceList2.ToArray());
      Dictionary<int, AclFileResource> dictionary2 = this.session.AclGroupManager.AddAclFileResources(aclFileResourceList1.ToArray());
      List<FileInGroup> fileInGroupList = new List<FileInGroup>();
      if (update3 != null)
      {
        foreach (FileSystemEntry entry in update3)
          fileInGroupList.Add(new FileInGroup()
          {
            FileID = this.findEntryID(entry, dictionary1),
            IsInclusive = false,
            Access = AclResourceAccess.ReadOnly
          });
      }
      if (update4 != null)
      {
        foreach (FileSystemEntry entry in update4)
          fileInGroupList.Add(new FileInGroup()
          {
            FileID = this.findEntryID(entry, dictionary1),
            IsInclusive = true,
            Access = AclResourceAccess.ReadOnly
          });
      }
      if (update5 != null)
      {
        foreach (FileSystemEntry entry in update5)
          fileInGroupList.Add(new FileInGroup()
          {
            FileID = this.findEntryID(entry, dictionary2),
            IsInclusive = false,
            Access = AclResourceAccess.ReadOnly
          });
      }
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) dictionary1.Keys);
      arrayList.AddRange((ICollection) dictionary2.Keys);
      this.session.AclGroupManager.ResetAclGroupFileRefs(systemResourceSet.GroupID, fileInGroupList.ToArray(), systemResourceSet.FileType, (int[]) arrayList.ToArray(typeof (int)));
    }

    private int findEntryID(FileSystemEntry entry, Dictionary<int, AclFileResource> dictionary)
    {
      foreach (int key in dictionary.Keys)
      {
        AclFileResource aclFileResource = dictionary[key];
        if (aclFileResource.FilePath.EndsWith(entry.Path) && entry.Owner == aclFileResource.Owner)
          return key;
      }
      return -1;
    }
  }
}
