// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FileInGroup
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FileInGroup
  {
    private int fileId;
    private bool isInclusive;
    private AclResourceAccess access;

    public FileInGroup()
    {
    }

    public FileInGroup(int fileId, bool isInclusive, AclResourceAccess access)
    {
      this.fileId = fileId;
      this.isInclusive = isInclusive;
      this.access = access;
    }

    public override int GetHashCode() => this.fileId.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.fileId, (object) ((FileInGroup) obj).fileId);
    }

    public static bool operator ==(FileInGroup o1, FileInGroup o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(FileInGroup o1, FileInGroup o2) => !(o1 == o2);

    public int FileID
    {
      get => this.fileId;
      set => this.fileId = value;
    }

    public bool IsInclusive
    {
      get => this.isInclusive;
      set => this.isInclusive = value;
    }

    public AclResourceAccess Access
    {
      get => this.access;
      set => this.access = value;
    }
  }
}
