// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IContactGroup
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ContactUI;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IContactGroup
  {
    ContactGroupInfo[] GetContactGroupsForUser(ContactGroupCollectionCriteria criteria);

    ContactGroupInfo[] GetContactGroupsForContact(ContactType contactType, int contactId);

    ContactGroupInfo GetContactGroup(int groupId);

    ContactGroupInfo SaveContactGroup(ContactGroupInfo groupInfo);

    void AddContactsToGroup(int[] newContactIds, int groupId, ContactType contactType);

    void DeleteContactGroup(int groupId);

    BorrowerSummaryInfo[] GetMembersForBorrowerGroup(int groupId);

    BizPartnerSummaryInfo[] GetMembersForPartnerGroup(int groupId);

    int[] GetContactQueryMemberIds(ContactQueryInfo queryInfo, SortField[] sortFields);

    ContactQueryInfo[] GetContactQueries(ContactQueryCollectionCriteria criteria);

    ContactQueryInfo GetContactQuery(int queryId);

    ContactQueryInfo SaveContactQuery(ContactQueryInfo queryInfo);

    void DeleteContactQuery(int queryId);

    ContactGroupInfo[] GetPublicBizContactGroups();
  }
}
