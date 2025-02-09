// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanFolderInGroup
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanFolderInGroup
  {
    private int groupId = -1;
    private string folderName = string.Empty;
    private string displayName;
    private LoanFolderInfo.LoanFolderType folderType;
    private bool accessible;

    public LoanFolderInGroup()
    {
    }

    public LoanFolderInGroup(string folderName, bool accessible)
      : this(-1, folderName, accessible, LoanFolderInfo.LoanFolderType.Regular)
    {
    }

    public LoanFolderInGroup(int groupId, string folderName, bool accessible)
      : this(groupId, folderName, accessible, LoanFolderInfo.LoanFolderType.Regular)
    {
    }

    public LoanFolderInGroup(
      int groupId,
      string folderName,
      bool accessible,
      LoanFolderInfo.LoanFolderType folderType)
    {
      this.groupId = groupId;
      this.folderName = folderName;
      this.accessible = accessible;
      this.folderType = folderType;
    }

    public override int GetHashCode() => this.folderName.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.folderName, (object) ((LoanFolderInGroup) obj).folderName);
    }

    public static bool operator ==(LoanFolderInGroup o1, LoanFolderInGroup o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(LoanFolderInGroup o1, LoanFolderInGroup o2) => !(o1 == o2);

    public int GroupID
    {
      get => this.groupId;
      set => this.groupId = value;
    }

    public bool Accessible
    {
      get => this.accessible;
      set => this.accessible = value;
    }

    public string FolderName
    {
      get => this.folderName;
      set => this.folderName = value;
    }

    public string DisplayName
    {
      get
      {
        if (this.displayName == null)
          this.displayName = new LoanFolderInfo(this.folderName, this.folderType, true).DisplayName;
        return this.displayName;
      }
    }

    public LoanFolderInfo.LoanFolderType FolderType => this.folderType;
  }
}
