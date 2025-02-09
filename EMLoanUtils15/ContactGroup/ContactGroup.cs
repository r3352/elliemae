// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.ContactGroup
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public class ContactGroup : BusinessBase, IDisposable
  {
    private ContactGroupInfo groupInfo;
    private ContactGroupMemberCollection groupMembers;
    [NotUndoable]
    private SessionObjects sessionObjects;

    public int GroupId => this.groupInfo.GroupId;

    public string UserId => this.groupInfo.UserId;

    public ContactType ContactType => this.groupInfo.ContactType;

    public ContactGroupType GroupType => this.groupInfo.GroupType;

    public string GroupName
    {
      get => this.groupInfo.GroupName;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.groupInfo.GroupName != value))
          return;
        this.groupInfo.GroupName = value;
        this.BrokenRules.Assert("GroupNameRequired", "Group Name is a required field", this.groupInfo.GroupName.Length < 1);
        this.BrokenRules.Assert("GroupNameLength", "Group Name exceeds 64 characters", this.groupInfo.GroupName.Length > 64);
        this.BrokenRules.Assert("ReservedGroupName", string.Format("{0} is a reserved group name", (object) this.groupInfo.GroupName), this.groupInfo.GroupName.ToLower() == "all contacts" || this.groupInfo.GroupName.ToLower() == "my contacts");
        this.MarkDirty();
      }
    }

    public string GroupDesc
    {
      get => this.groupInfo.GroupDesc;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.groupInfo.GroupDesc != value))
          return;
        this.groupInfo.GroupDesc = value;
        this.BrokenRules.Assert("ContactGroupDescriptionLength", "Contact Group Description exceeds 250 characters", this.groupInfo.GroupDesc.Length > 250);
        this.MarkDirty();
      }
    }

    public DateTime CreationTime => this.groupInfo.CreationTime;

    public ContactGroupMemberCollection GroupMembers
    {
      get
      {
        if (this.groupMembers == null)
          this.getMembers();
        return this.groupMembers;
      }
    }

    internal ContactGroupInfo GetInfo()
    {
      this.groupInfo.IsNew = this.IsNew;
      this.groupInfo.IsDirty = this.IsDirty;
      this.groupInfo.IsDeleted = this.IsDeleted;
      this.groupInfo.ContactIds = (int[]) null;
      this.groupInfo.AddedContactIds = new int[this.groupMembers.AddedCount];
      this.groupInfo.DeletedContactIds = new int[this.groupMembers.DeletedCount];
      int num1 = 0;
      foreach (ContactGroupMember groupMember in (CollectionBase) this.groupMembers)
      {
        if (groupMember.IsNew)
          this.groupInfo.AddedContactIds[num1++] = groupMember.ContactId;
      }
      int num2 = 0;
      foreach (ContactGroupMember deleted in (CollectionBase) this.groupMembers.deletedList)
        this.groupInfo.DeletedContactIds[num2++] = deleted.ContactId;
      return this.groupInfo;
    }

    internal void SetInfo(ContactGroupInfo groupInfo)
    {
      if (!((ContactGroupInfo) null != groupInfo))
        return;
      this.groupInfo = groupInfo;
      this.populateMemberCollection();
    }

    private int GroupMembersCount => this.groupMembers == null ? 0 : this.groupMembers.Count;

    private void getMembers()
    {
      if (this.ContactType == ContactType.Borrower)
        this.groupInfo.BorrowerMembers = this.sessionObjects.ContactGroupManager.GetMembersForBorrowerGroup(this.GroupId);
      else
        this.groupInfo.PartnerMembers = this.sessionObjects.ContactGroupManager.GetMembersForPartnerGroup(this.GroupId);
      this.populateMemberCollection();
    }

    private void populateMemberCollection()
    {
      this.groupMembers = (ContactGroupMemberCollection) null;
      if (this.ContactType == ContactType.Borrower)
      {
        if (this.groupInfo.BorrowerMembers == null && ContactGroupType.QueryGroup != this.GroupType)
          return;
        if (this.groupInfo.BorrowerMembers == null)
        {
          this.groupMembers = ContactGroupMemberCollection.NewContactGroupMemberCollection();
        }
        else
        {
          this.groupMembers = ContactGroupMemberCollection.NewContactGroupMemberCollection(this.groupInfo.BorrowerMembers.Length);
          foreach (BorrowerSummaryInfo borrowerMember in this.groupInfo.BorrowerMembers)
            this.groupMembers.Add((ContactGroupMember) BorrowerGroupMember.NewBorrowerGroupMember(this.GroupId, borrowerMember));
        }
      }
      else
      {
        if (this.groupInfo.PartnerMembers == null && ContactGroupType.QueryGroup != this.GroupType)
          return;
        if (this.groupInfo.PartnerMembers == null)
        {
          this.groupMembers = ContactGroupMemberCollection.NewContactGroupMemberCollection();
        }
        else
        {
          this.groupMembers = ContactGroupMemberCollection.NewContactGroupMemberCollection(this.groupInfo.PartnerMembers.Length);
          foreach (BizPartnerSummaryInfo partnerMember in this.groupInfo.PartnerMembers)
            this.groupMembers.Add((ContactGroupMember) PartnerGroupMember.NewPartnerGroupMember(this.GroupId, partnerMember));
        }
      }
    }

    public override string ToString() => string.Format("ContactGroup[{0}]", (object) this.GroupId);

    public bool Equals(EllieMae.EMLite.ContactGroup.ContactGroup contactGroup)
    {
      return this.GroupId.Equals(contactGroup.GroupId);
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is EllieMae.EMLite.ContactGroup.ContactGroup && objB is EllieMae.EMLite.ContactGroup.ContactGroup && ((EllieMae.EMLite.ContactGroup.ContactGroup) objA).Equals((EllieMae.EMLite.ContactGroup.ContactGroup) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is EllieMae.EMLite.ContactGroup.ContactGroup && this.Equals((EllieMae.EMLite.ContactGroup.ContactGroup) obj);
    }

    public override int GetHashCode()
    {
      return string.Format("{0}", (object) this.groupInfo.GroupId).GetHashCode();
    }

    public override bool IsValid
    {
      get
      {
        if (!base.IsValid)
          return false;
        return this.groupMembers == null || this.groupMembers.IsValid;
      }
    }

    public override bool IsDirty
    {
      get
      {
        if (base.IsDirty)
          return true;
        return this.groupMembers != null && this.groupMembers.IsDirty;
      }
    }

    public override BusinessBase Save()
    {
      if (this.IsDeleted)
      {
        if (!this.IsNew)
          this.sessionObjects.ContactGroupManager.DeleteContactGroup(this.GroupId);
        this.MarkNew();
      }
      else if (this.IsDirty)
      {
        this.groupInfo.IsNew = this.IsNew;
        this.groupInfo.IsDirty = this.IsDirty;
        this.groupInfo.IsDeleted = this.IsDeleted;
        this.groupInfo.AddedContactIds = new int[this.groupMembers.AddedCount];
        int num1 = 0;
        foreach (ContactGroupMember groupMember in (CollectionBase) this.groupMembers)
        {
          if (groupMember.IsNew)
            this.groupInfo.AddedContactIds[num1++] = groupMember.ContactId;
        }
        this.groupInfo.DeletedContactIds = new int[this.groupMembers.deletedList.Count];
        int num2 = 0;
        foreach (ContactGroupMember deleted in (CollectionBase) this.groupMembers.deletedList)
          this.groupInfo.DeletedContactIds[num2++] = deleted.ContactId;
        this.SetInfo(this.sessionObjects.ContactGroupManager.SaveContactGroup(this.groupInfo));
        this.MarkOld();
      }
      return (BusinessBase) this;
    }

    public static EllieMae.EMLite.ContactGroup.ContactGroup NewContactGroup(
      ContactType contactType,
      ContactGroupType groupType,
      SessionObjects sessionObjects)
    {
      return new EllieMae.EMLite.ContactGroup.ContactGroup(contactType, groupType, sessionObjects);
    }

    public static EllieMae.EMLite.ContactGroup.ContactGroup NewContactGroup(
      ContactGroupInfo groupInfo,
      SessionObjects sessionObjects)
    {
      return new EllieMae.EMLite.ContactGroup.ContactGroup(groupInfo, sessionObjects);
    }

    public static EllieMae.EMLite.ContactGroup.ContactGroup GetContactGroup(
      int groupId,
      SessionObjects sessionObjects)
    {
      ContactGroupInfo contactGroup = sessionObjects.ContactGroupManager.GetContactGroup(groupId);
      return (ContactGroupInfo) null == contactGroup ? (EllieMae.EMLite.ContactGroup.ContactGroup) null : new EllieMae.EMLite.ContactGroup.ContactGroup(contactGroup, sessionObjects);
    }

    public static void DeleteContactGroup(int groupId, SessionObjects sessionObjects)
    {
      sessionObjects.ContactGroupManager.DeleteContactGroup(groupId);
    }

    private ContactGroup(
      ContactType contactType,
      ContactGroupType groupType,
      SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.groupInfo = new ContactGroupInfo(0, this.sessionObjects.UserID, contactType, groupType, string.Empty, string.Empty, DateTime.MinValue, (int[]) null);
      this.groupMembers = ContactGroupMemberCollection.NewContactGroupMemberCollection();
      this.BrokenRules.Assert("GroupNameRequired", "Group Name is a required field", this.groupInfo.GroupName.Length < 1);
    }

    private ContactGroup(ContactGroupInfo groupInfo, SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkOld();
      this.groupInfo = groupInfo;
      this.populateMemberCollection();
    }

    public void Dispose()
    {
    }

    [Serializable]
    private class Criteria
    {
      public int GroupId;

      public Criteria(int groupId) => this.GroupId = groupId;
    }
  }
}
