// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.ContactGroupMemberCursorCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public class ContactGroupMemberCursorCollection : ICursorCollectionBase
  {
    [NotUndoable]
    [NonSerialized]
    private ContactType contactType;
    private int groupId;
    private ContactQuery contactQuery;
    private QueryCriterion[] queryCriteria = new QueryCriterion[0];
    private RelatedLoanMatchType matchType;
    private SortField[] sortFields;
    [NotUndoable]
    private SessionObjects sessionObjects;

    public ContactType ContactType => this.contactType;

    public int GroupId => this.groupId;

    public ContactGroupMember this[int index] => (ContactGroupMember) this.List[index];

    public ContactGroupMember Find(int contactId)
    {
      foreach (object obj in (IEnumerable) this.List)
      {
        if (contactId == ((ContactGroupMember) obj).ContactId)
          return (ContactGroupMember) obj;
      }
      return (ContactGroupMember) null;
    }

    public void AddQueryCriteria(QueryCriterion[] queryCriteria)
    {
      if (queryCriteria == null)
        return;
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) this.queryCriteria);
      arrayList.AddRange((ICollection) queryCriteria);
      this.queryCriteria = (QueryCriterion[]) arrayList.ToArray(typeof (QueryCriterion));
    }

    public void SetSortFields(SortField[] sortFields) => this.sortFields = sortFields;

    protected void Add(ContactGroupMember contactGroupMember)
    {
      this.locked = false;
      this.List.Add((object) contactGroupMember);
      this.locked = true;
    }

    private bool isOwnerOnly()
    {
      foreach (QueryCriterion queryCriterion in this.queryCriteria)
      {
        if (queryCriterion is StringValueCriterion && "Contact.OwnerID" == ((FieldValueCriterion) queryCriterion).FieldName && this.sessionObjects.UserID == ((StringValueCriterion) queryCriterion).Value && ((StringValueCriterion) queryCriterion).MatchType == StringMatchType.Exact)
          return true;
      }
      return false;
    }

    public bool Contains(ContactGroupMember contactGroupMember)
    {
      foreach (ContactGroupMember contactGroupMember1 in (IEnumerable) this.List)
      {
        if (contactGroupMember1.Equals(contactGroupMember))
          return true;
      }
      return false;
    }

    protected override void OpenCursor()
    {
      if (this.iCursor != null)
        return;
      if (this.contactType == ContactType.Borrower)
      {
        if (this.contactQuery == null)
        {
          this.iCursor = this.sessionObjects.ContactManager.OpenBorrowerCursor(this.queryCriteria, this.matchType, this.sortFields, (string[]) null, true);
        }
        else
        {
          ContactQueryInfo info = this.contactQuery.GetInfo();
          info.OwnerOnly = this.isOwnerOnly();
          this.iCursor = this.sessionObjects.ContactManager.OpenBorrowerCursor(info, this.sortFields, (string[]) null, true);
        }
      }
      else if (this.contactQuery == null)
        this.iCursor = this.sessionObjects.ContactManager.OpenBizPartnerCursor(this.queryCriteria, this.matchType, this.sortFields, (string[]) null, true);
      else
        this.iCursor = this.sessionObjects.ContactManager.OpenBizPartnerCursor(this.contactQuery.GetInfo(), this.sortFields, (string[]) null, true);
    }

    protected override void SyncCursor(object[] items, int count)
    {
    }

    public static ContactGroupMemberCursorCollection NewContactGroupMemberCursorCollection(
      ContactType contactType,
      int groupId,
      SessionObjects sessionObjects)
    {
      return new ContactGroupMemberCursorCollection(contactType, groupId, sessionObjects);
    }

    public static ContactGroupMemberCursorCollection NewContactGroupMemberCursorCollection(
      ContactQuery contactQuery,
      SessionObjects sessionObjects)
    {
      return new ContactGroupMemberCursorCollection(contactQuery, sessionObjects);
    }

    private ContactGroupMemberCursorCollection(
      ContactType contactType,
      int groupId,
      SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.contactType = contactType;
      this.groupId = groupId;
      if (PsuedoGroupId.MyContacts == groupId)
      {
        this.queryCriteria = new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("Contact.OwnerID", this.sessionObjects.UserID, StringMatchType.Exact)
        };
      }
      else
      {
        if (PsuedoGroupId.AllContacts == groupId)
          return;
        this.queryCriteria = new QueryCriterion[1]
        {
          (QueryCriterion) new OrdinalValueCriterion("ContactGroup.GroupID", (object) groupId, OrdinalMatchType.Equals)
        };
      }
    }

    private ContactGroupMemberCursorCollection(
      ContactQuery contactQuery,
      SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.contactQuery = contactQuery;
      this.contactType = contactQuery.ContactType;
      this.groupId = 0;
    }
  }
}
