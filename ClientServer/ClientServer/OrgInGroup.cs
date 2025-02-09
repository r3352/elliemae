// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.OrgInGroup
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class OrgInGroup
  {
    private int orgId;
    private bool isInclusive;
    private string orgName = "";
    private string type = "";

    public OrgInGroup()
    {
    }

    public OrgInGroup(int orgId, bool isInclusive, string orgName)
    {
      this.orgId = orgId;
      this.isInclusive = isInclusive;
      this.orgName = orgName;
    }

    public override int GetHashCode() => this.orgId.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && (object) (obj as OrgInGroup) != null && object.Equals((object) this.orgId, (object) ((OrgInGroup) obj).orgId);
    }

    public static bool operator ==(OrgInGroup o1, OrgInGroup o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(OrgInGroup o1, OrgInGroup o2) => !(o1 == o2);

    public int OrgID
    {
      get => this.orgId;
      set => this.orgId = value;
    }

    public bool IsInclusive
    {
      get => this.isInclusive;
      set => this.isInclusive = value;
    }

    public string OrgName
    {
      get => this.orgName;
      set => this.orgName = value;
    }

    public string Type
    {
      get => this.type;
      set => this.type = value;
    }
  }
}
