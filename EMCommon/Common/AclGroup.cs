// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.AclGroup
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class AclGroup : IComparable
  {
    public static int NoUserGroupId = -1;
    private int groupID;
    private string groupName;
    private bool viewSubordContacts;
    private int displayOrder;
    private AclResourceAccess contactAccess;
    private DateTime? createdDate;
    private string createdBy = string.Empty;
    private DateTime? lastModifiedDate;
    private string lastModifiedBy = string.Empty;

    public AclGroup()
    {
      this.groupID = -1;
      this.groupName = string.Empty;
      this.viewSubordContacts = false;
      this.displayOrder = 0;
      this.contactAccess = AclResourceAccess.ReadOnly;
      this.createdDate = new DateTime?();
      this.createdBy = string.Empty;
      this.lastModifiedDate = new DateTime?();
      this.lastModifiedBy = string.Empty;
    }

    public AclGroup(
      int groupID,
      string groupName,
      bool viewSubordContacts,
      AclResourceAccess contactAccess,
      int displayOrder,
      DateTime? createdDate,
      string createdBy,
      DateTime? lastModifiedDate,
      string lastModifiedBy)
    {
      this.groupID = groupID;
      this.groupName = groupName;
      this.viewSubordContacts = viewSubordContacts;
      this.displayOrder = displayOrder;
      this.contactAccess = contactAccess;
      this.createdDate = createdDate;
      this.createdBy = createdBy;
      this.lastModifiedDate = lastModifiedDate;
      this.lastModifiedBy = lastModifiedBy;
    }

    public int ID => this.groupID;

    public string Name
    {
      get => this.groupName;
      set => this.groupName = value;
    }

    public bool ViewSubordinatesContacts
    {
      get => this.viewSubordContacts;
      set => this.viewSubordContacts = value;
    }

    public AclResourceAccess ContactAccess
    {
      get => this.contactAccess;
      set => this.contactAccess = value;
    }

    public int DisplayOrder
    {
      get => this.displayOrder;
      set => this.displayOrder = value;
    }

    public DateTime? CreatedDate
    {
      get => this.createdDate;
      set => this.createdDate = value;
    }

    public string CreatedBy
    {
      get => this.createdBy;
      set => this.createdBy = value;
    }

    public DateTime? LastModifiedDate
    {
      get => this.lastModifiedDate;
      set => this.lastModifiedDate = value;
    }

    public string LastModifiedBy
    {
      get => this.lastModifiedBy;
      set => this.lastModifiedBy = value;
    }

    public override int GetHashCode() => this.groupID.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.groupID, (object) ((AclGroup) obj).groupID);
    }

    public static bool operator ==(AclGroup o1, AclGroup o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(AclGroup o1, AclGroup o2) => !(o1 == o2);

    public override string ToString() => this.groupName;

    public int CompareTo(object obj)
    {
      AclGroup aclGroup = (AclGroup) obj;
      if (this.groupName == "All Users")
        return -1;
      return aclGroup.groupName == "All Users" ? 1 : string.Compare(this.groupName, aclGroup.groupName);
    }
  }
}
