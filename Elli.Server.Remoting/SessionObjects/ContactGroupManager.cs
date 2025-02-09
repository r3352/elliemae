// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ContactGroupManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.Server;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class ContactGroupManager : SessionBoundObject, IContactGroup
  {
    private const string className = "ContactGroupManager";

    public ContactGroupManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (ContactGroupManager).ToLower());
      return this;
    }

    public virtual ContactGroupInfo[] GetContactGroupsForUser(
      ContactGroupCollectionCriteria criteria)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetContactGroupsForUser), new object[1]
      {
        (object) criteria.UserId
      });
      try
      {
        return ContactGroupProvider.GetContactGroupsForUser(this.Session.UserID, criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactGroupInfo[]) null;
      }
    }

    public virtual ContactGroupInfo[] GetContactGroupsForContact(
      ContactType contactType,
      int contactId)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetContactGroupsForContact), new object[1]
      {
        (object) contactId
      });
      try
      {
        return ContactGroupProvider.GetContactGroupsForContact(this.Session.UserID, contactType, contactId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactGroupInfo[]) null;
      }
    }

    public virtual ContactGroupInfo GetContactGroup(int groupId)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetContactGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        return ContactGroupProvider.GetContactGroup(groupId, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactGroupInfo) null;
      }
    }

    public virtual int[] GetContactQueryMemberIds(
      ContactQueryInfo queryInfo,
      SortField[] sortFields)
    {
      this.onApiCalled(nameof (ContactGroupManager), "GetContactGroup", new object[1]
      {
        (object) queryInfo.QueryName
      });
      try
      {
        return ContactGroupProvider.GetContactQueryMemberIds(UserStore.GetLatestVersion(this.Session.UserID), queryInfo, sortFields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (int[]) null;
      }
    }

    public virtual void AddContactsToGroup(
      int[] newContactIds,
      int groupId,
      ContactType contactType)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (AddContactsToGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        ContactGroupProvider.AddContactsToGroup(newContactIds, groupId, contactType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactGroupInfo SaveContactGroup(ContactGroupInfo groupInfo)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (SaveContactGroup), new object[1]
      {
        (object) groupInfo.GroupId
      });
      try
      {
        bool flag = false;
        string groupName = groupInfo.GroupName;
        if (groupInfo.ContactType == ContactType.PublicBiz && groupInfo.GroupId == 0)
          flag = true;
        ContactGroupInfo contactGroupInfo = ContactGroupProvider.SaveContactGroup(groupInfo, this.Session.UserID);
        if (groupInfo.ContactType == ContactType.PublicBiz)
        {
          if (flag)
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new BizContactGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessContactGroupCreated, DateTime.Now, contactGroupInfo.GroupId, groupName));
          else
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new BizContactGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessContactGroupModified, DateTime.Now, contactGroupInfo.GroupId, groupName));
        }
        return contactGroupInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactGroupInfo) null;
      }
    }

    public virtual void DeleteContactGroup(int groupId)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (DeleteContactGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        ContactGroupInfo contactGroup = ContactGroupProvider.GetContactGroup(groupId, this.Session.UserID);
        string groupName = contactGroup.GroupName;
        ContactGroupProvider.DeleteContactGroup(groupId);
        if (contactGroup.ContactType != ContactType.PublicBiz)
          return;
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new BizContactGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessContactGroupDeleted, DateTime.Now, groupId, groupName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BorrowerSummaryInfo[] GetMembersForBorrowerGroup(int groupId)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetMembersForBorrowerGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        return ContactGroupProvider.GetMembersForBorrowerGroup(groupId, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (BorrowerSummaryInfo[]) null;
      }
    }

    public virtual BizPartnerSummaryInfo[] GetMembersForPartnerGroup(int groupId)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetMembersForPartnerGroup), new object[1]
      {
        (object) groupId
      });
      try
      {
        return ContactGroupProvider.GetMembersForPartnerGroup(groupId, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (BizPartnerSummaryInfo[]) null;
      }
    }

    public virtual ContactQueryInfo[] GetContactQueries(ContactQueryCollectionCriteria criteria)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetContactQueries), new object[1]
      {
        (object) criteria.UserId
      });
      try
      {
        return ContactGroupProvider.GetContactQueries(criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactQueryInfo[]) null;
      }
    }

    public virtual ContactQueryInfo GetContactQuery(int queryId)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetContactQuery), new object[1]
      {
        (object) queryId
      });
      try
      {
        return ContactGroupProvider.GetContactQuery(queryId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactQueryInfo) null;
      }
    }

    public virtual ContactQueryInfo SaveContactQuery(ContactQueryInfo queryInfo)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (SaveContactQuery), new object[1]
      {
        (object) queryInfo.QueryId
      });
      try
      {
        return ContactGroupProvider.SaveContactQuery(queryInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactQueryInfo) null;
      }
    }

    public virtual void DeleteContactQuery(int queryId)
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (DeleteContactQuery), new object[1]
      {
        (object) queryId
      });
      try
      {
        this.DeleteContactQuery(queryId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactGroupInfo[] GetPublicBizContactGroups()
    {
      this.onApiCalled(nameof (ContactGroupManager), nameof (GetPublicBizContactGroups), Array.Empty<object>());
      try
      {
        return ContactGroupProvider.GetPublicBizContactGroups();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactGroupManager), ex, this.Session.SessionInfo);
        return (ContactGroupInfo[]) null;
      }
    }
  }
}
