// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactGroup.ContactGroupInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ContactGroup
{
  [Serializable]
  public class ContactGroupInfo : BusinessInfoBase
  {
    public int GroupId;
    public string UserId;
    public ContactType ContactType;
    public ContactGroupType GroupType;
    public string GroupName;
    public string GroupDesc;
    public DateTime CreationTime;
    [NotUndoable]
    public int[] ContactIds;
    [NotUndoable]
    public BorrowerSummaryInfo[] BorrowerMembers;
    [NotUndoable]
    public BizPartnerSummaryInfo[] PartnerMembers;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;
    [NotUndoable]
    public int[] AddedContactIds;
    [NotUndoable]
    public int[] DeletedContactIds;

    public ContactGroupInfo()
    {
    }

    public ContactGroupInfo(
      int groupId,
      string userId,
      ContactType contactType,
      ContactGroupType groupType,
      string groupName,
      string groupDesc,
      DateTime creationTime,
      int[] contactIds)
    {
      this.GroupId = groupId;
      this.UserId = userId;
      this.ContactType = contactType;
      this.GroupType = groupType;
      this.GroupName = groupName;
      this.GroupDesc = groupDesc;
      this.CreationTime = creationTime;
      this.ContactIds = contactIds;
    }

    public override int GetHashCode() => this.GroupId.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && this.GroupId.Equals(((ContactGroupInfo) obj).GroupId);
    }

    public static bool operator ==(ContactGroupInfo o1, ContactGroupInfo o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(ContactGroupInfo o1, ContactGroupInfo o2) => !(o1 == o2);

    public override string ToString() => this.GroupName;
  }
}
