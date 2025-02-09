// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ContactManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Cursors;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class ContactManager : SessionBoundObject, IContactManager
  {
    private const string className = "ContactManager";
    private string[] defaultBorrowerFields;
    private string[] defaultBizPartnerFields;

    public ContactManager()
    {
      this.defaultBorrowerFields = new string[12]
      {
        "Contact.ContactID",
        "Contact.FirstName",
        "Contact.LastName",
        "Contact.HomePhone",
        "Contact.PersonalEmail",
        "Contact.OwnerID",
        "Contact.ContactType",
        "Contact.Status",
        "Contact.NoSpam",
        "Contact.LastModified",
        "Owner.Last_Name",
        "Owner.First_Name"
      };
      this.defaultBizPartnerFields = new string[11]
      {
        "Contact.ContactID",
        "Contact.FirstName",
        "Contact.LastName",
        "Contact.CategoryID",
        "Contact.CompanyName",
        "Contact.WorkPhone",
        "Contact.HomePhone",
        "Contact.MobilePhone",
        "Contact.BizEmail",
        "Contact.IsPublic",
        "Contact.AccessLevel"
      };
    }

    public ContactManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (ContactManager).ToLower());
      return this;
    }

    public virtual int CreateBizPartner(BizPartnerInfo info)
    {
      return this.CreateBizPartner(info, DateTime.Now, ContactSource.NotAvailable);
    }

    public virtual int CreateBizPartner(
      BizPartnerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource)
    {
      this.onApiCalled(nameof (ContactManager), nameof (CreateBizPartner), new object[2]
      {
        (object) info,
        (object) firstContactDate
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("BizPartnerInfo cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        int bizPartner = BizPartnerContact.CreateNew(info, firstContactDate, contactSource);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Created new BizPartner \"" + info.FullName + "\" (" + (object) bizPartner + ")"));
        return bizPartner;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual BizPartnerInfo[] GetBizPartnersByOwner(string ownerId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizPartnersByOwner), new object[1]
      {
        (object) ownerId
      });
      try
      {
        return BizPartnerContact.GetBizPartnersByOwner(ownerId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual void MakeBizPartnersPublic(int[] contactIds)
    {
      this.onApiCalled(nameof (ContactManager), nameof (MakeBizPartnersPublic), new object[1]
      {
        (object) contactIds
      });
      try
      {
        BizPartnerContact.MakeBizPartnersPublic(contactIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BizPartnerInfo GetBizPartner(int contactId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizPartner), new object[1]
      {
        (object) contactId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = ContactStore.GetLatestVersion(contactId, ContactType.BizPartner))
        {
          if (!latestVersion.Exists)
            return (BizPartnerInfo) null;
          BizPartnerInfo contactInfo = ((BizPartnerContact) latestVersion).GetContactInfo();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved BizPartner \"" + contactInfo.FullName + "\" (" + (object) contactId + ")"));
          return contactInfo;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo) null;
      }
    }

    public virtual BizPartnerInfo[] GetBizPartners(int[] contactIds)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizPartners), new object[1]
      {
        (object) contactIds
      });
      try
      {
        return BizPartnerContact.GetBizPartners(contactIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual BizPartnerInfo[] GetBizPartners(string[] contactGuids)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizPartners), (object[]) contactGuids);
      try
      {
        return BizPartnerContact.GetBizPartners(contactGuids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual string GetBizPartnerAdvQueryPath(string userID)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizPartnerAdvQueryPath), new object[1]
      {
        (object) userID
      });
      try
      {
        return BizPartnerQueryStore.getXmlFilePath(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual string GetBorrowerAdvQueryPath(string userID)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrowerAdvQueryPath), new object[1]
      {
        (object) userID
      });
      try
      {
        return BorrowerQueryStore.getXmlFilePath(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual void UpdateBizPartner(BizPartnerInfo info)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateBizPartner), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("BizPartnerInfo cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(info.ContactID, ContactType.BizPartner))
          ((BizPartnerContact) contact).CheckIn(info);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Updated BizPartner \"" + info.FullName + "\" (" + (object) info.ContactID + ")"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBizPartner(int contactId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBizPartner), new object[1]
      {
        (object) contactId
      });
      try
      {
        string str = "";
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, ContactType.BizPartner))
        {
          str = ((BizPartnerContact) contact).GetContactInfo().FullName;
          contact.Delete();
        }
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Deleted BizPartner \"" + str + "\" (" + (object) contactId + ")"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBizPartners(int[] contactIds)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBizPartners), new object[1]
      {
        (object) contactIds
      });
      try
      {
        foreach (int contactId in contactIds)
        {
          using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, ContactType.BizPartner))
          {
            if (contact.Exists)
              contact.Delete();
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BizPartnerInfo[] QueryBizPartnersForUser(
      string userID,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBizPartnersForUser), new object[2]
      {
        (object) userID,
        (object) criteria
      });
      try
      {
        UserInfo user = (UserInfo) null;
        using (User latestVersion = UserStore.GetLatestVersion(userID))
          user = latestVersion.Exists ? latestVersion.UserInfo : throw new ObjectNotFoundException("Invalid user ID '" + userID + "'", ObjectType.User, (object) userID);
        return BizPartnerContact.QueryBizPartners(user, criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual BizPartnerInfo[] QueryBizPartners(QueryCriterion[] criteria)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBizPartners), (object[]) criteria);
      try
      {
        return BizPartnerContact.QueryBizPartners(this.Session.GetUserInfo(), criteria, RelatedLoanMatchType.None, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual BizPartnerInfo[] QueryBizPartners(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBizPartners), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        return BizPartnerContact.QueryBizPartners(this.Session.GetUserInfo(), criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual int[] QueryBizPartnerIds(
      string userId,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBizPartnerIds), new object[3]
      {
        (object) userId,
        (object) criteria,
        (object) matchType
      });
      try
      {
        UserInfo userInfo = (UserInfo) null;
        using (User latestVersion = UserStore.GetLatestVersion(userId))
          userInfo = latestVersion.Exists ? latestVersion.UserInfo : throw new ObjectNotFoundException("Invalid user ID '" + userId + "'", ObjectType.User, (object) userId);
        return BizPartnerContact.QueryBizPartnerIds(userInfo, criteria, matchType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (int[]) null;
      }
    }

    public virtual BizPartnerInfo[] QueryAllBizPartners(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryAllBizPartners), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        return BizPartnerContact.QueryBizPartners((UserInfo) null, criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual int QueryBizPartnerMortgageClause(int categoryID, string mortgageClauseCompany)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBizPartnerMortgageClause), new object[2]
      {
        (object) categoryID,
        (object) mortgageClauseCompany
      });
      try
      {
        return BizPartnerContact.QueryBizPartnerMortgageClause(categoryID, mortgageClauseCompany);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual BizPartnerSummaryInfo[] QueryBizPartnerSummaries(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBizPartnerSummaries), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        return BizPartnerContact.QueryBizPartnerSummaries(this.Session.GetUserInfo(), criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerSummaryInfo[]) null;
      }
    }

    public virtual ICursor OpenBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenBizPartnerCursor), new object[5]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summariesOnly
      });
      try
      {
        if (fields == null)
          fields = this.defaultBizPartnerFields;
        return (ICursor) InterceptionUtils.NewInstance<BizPartnerCursor>().Initialize(this.Session, BizPartnerContact.QueryBizPartnerIds(this.Session.GetUserInfo(), criteria, loanMatchType, sortOrder), fields, summariesOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenAllBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenAllBizPartnerCursor), new object[5]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summariesOnly
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<BizPartnerCursor>().Initialize(this.Session, BizPartnerContact.QueryBizPartnerIds((UserInfo) null, criteria, loanMatchType, sortOrder), fields, summariesOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenPublicBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenPublicBizPartnerCursor), new object[5]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summariesOnly
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<BizPartnerCursor>().Initialize(this.Session, BizPartnerContact.QueryBizPartnerIds(this.Session.GetUserInfo(), criteria, loanMatchType, sortOrder), fields, summariesOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenBizPartnerCursor(
      ContactQueryInfo queryInfo,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenBizPartnerCursor), new object[4]
      {
        (object) queryInfo,
        (object) sortOrder,
        (object) fields,
        (object) summariesOnly
      });
      try
      {
        if (fields == null)
          fields = this.defaultBizPartnerFields;
        return (ICursor) InterceptionUtils.NewInstance<BizPartnerCursor>().Initialize(this.Session, ContactGroupProvider.GetContactQueryMemberIds(UserStore.GetLatestVersion(this.Session.UserID), queryInfo, sortOrder), fields, summariesOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual BorrowerInfo[] GetBorrowersByOwner(string ownerId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrowersByOwner), new object[1]
      {
        (object) ownerId
      });
      try
      {
        return BorrowerContact.GetBorrowersByOwner(ownerId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual int CreateBorrower(BorrowerInfo info)
    {
      return this.CreateBorrower(info, DateTime.Now, ContactSource.NotAvailable);
    }

    public virtual BorrowerInfo CreateBorrowerInfo(
      BorrowerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource)
    {
      this.onApiCalled(nameof (ContactManager), "CreateBorrower", new object[2]
      {
        (object) info,
        (object) firstContactDate
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("Borrower Info cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        info.ContactGuid = new Guid();
        int contactId = BorrowerContact.CreateNew(info, firstContactDate, contactSource);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Created Borrower \"" + info.FullName + "\" (" + (object) contactId + ")"));
        using (EllieMae.EMLite.Server.Contact latestVersion = ContactStore.GetLatestVersion(contactId, ContactType.Borrower))
        {
          if (!latestVersion.Exists)
            return (BorrowerInfo) null;
          BorrowerInfo contactInfo = ((BorrowerContact) latestVersion).GetContactInfo();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved Borrower \"" + info.FullName + "\" (" + (object) contactId + ")"));
          return contactInfo;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo) null;
      }
    }

    public virtual int CreateBorrower(
      BorrowerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource)
    {
      this.onApiCalled(nameof (ContactManager), nameof (CreateBorrower), new object[2]
      {
        (object) info,
        (object) firstContactDate
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("Borrower Info cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        info.ContactGuid = new Guid();
        int borrower = BorrowerContact.CreateNew(info, firstContactDate, contactSource);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Created Borrower \"" + info.FullName + "\" (" + (object) borrower + ")"));
        return borrower;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual BorrowerInfo GetBorrower(int contactId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrower), new object[1]
      {
        (object) contactId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = ContactStore.GetLatestVersion(contactId, ContactType.Borrower))
        {
          if (!latestVersion.Exists)
            return (BorrowerInfo) null;
          BorrowerInfo contactInfo = ((BorrowerContact) latestVersion).GetContactInfo();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved Borrower \"" + contactInfo.FullName + "\" (" + (object) contactId + ")"));
          return contactInfo;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo) null;
      }
    }

    public virtual BorrowerInfo GetBorrower(string contactGuid)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrower), new object[1]
      {
        (object) contactGuid
      });
      try
      {
        int borrowerIdFromGuid = BorrowerContact.GetBorrowerIDFromGuid(contactGuid);
        if (borrowerIdFromGuid < 0)
          return (BorrowerInfo) null;
        using (EllieMae.EMLite.Server.Contact latestVersion = ContactStore.GetLatestVersion(borrowerIdFromGuid, ContactType.Borrower))
        {
          if (!latestVersion.Exists)
            return (BorrowerInfo) null;
          BorrowerInfo contactInfo = ((BorrowerContact) latestVersion).GetContactInfo();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved Borrower \"" + contactInfo.FullName + "\" (" + (object) borrowerIdFromGuid + ")"));
          return contactInfo;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo) null;
      }
    }

    public virtual BizPartnerInfo GetBizPartner(string contactGuid)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizPartner), new object[1]
      {
        (object) contactGuid
      });
      try
      {
        int partnerIdFromGuid = BizPartnerContact.GetBizPartnerIDFromGuid(contactGuid);
        if (partnerIdFromGuid < 0)
          return (BizPartnerInfo) null;
        using (EllieMae.EMLite.Server.Contact latestVersion = ContactStore.GetLatestVersion(partnerIdFromGuid, ContactType.BizPartner))
        {
          if (!latestVersion.Exists)
            return (BizPartnerInfo) null;
          BizPartnerInfo contactInfo = ((BizPartnerContact) latestVersion).GetContactInfo();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved BizPartner \"" + contactInfo.FullName + "\" (" + (object) partnerIdFromGuid + ")"));
          return contactInfo;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo) null;
      }
    }

    public virtual BorrowerInfo[] GetBorrowers(int[] contactIds)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrowers), new object[1]
      {
        (object) contactIds
      });
      try
      {
        return BorrowerContact.GetBorrowers(contactIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual void UpdateBorrower(BorrowerInfo info)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateBorrower), new object[1]
      {
        (object) info
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(info.ContactID, ContactType.Borrower))
          ((BorrowerContact) contact).CheckIn(info);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Updated Borrower \"" + info.FullName + "\" (" + (object) info.ContactID + ")"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBorrower(int contactId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBorrower), new object[1]
      {
        (object) contactId
      });
      try
      {
        string str = "";
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, ContactType.Borrower))
        {
          str = ((BorrowerContact) contact).GetContactInfo().FullName;
          contact.Delete();
        }
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Deleted Borrower \"" + str + "\" (" + (object) contactId + ")"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBorrowers(int[] contactIds)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBorrowers), new object[1]
      {
        (object) contactIds
      });
      try
      {
        foreach (int contactId in contactIds)
        {
          using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, ContactType.Borrower))
          {
            if (contact.Exists)
              contact.Delete();
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BorrowerInfo[] QueryMaxAccessibleBorrowers(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryMaxAccessibleBorrowers), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        UserInfo user = this.Session.GetUserInfo();
        if (user.IsSuperAdministrator())
          user = (UserInfo) null;
        return BorrowerContact.QueryBorrowers(user, criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual BorrowerInfo[] QueryBorrowersForUser(string userID, QueryCriterion[] criteria)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBorrowersForUser), new object[2]
      {
        (object) userID,
        (object) criteria
      });
      try
      {
        UserInfo user = (UserInfo) null;
        using (User latestVersion = UserStore.GetLatestVersion(userID))
          user = latestVersion.Exists ? latestVersion.UserInfo : throw new ObjectNotFoundException("Invalid user ID '" + userID + "'", ObjectType.User, (object) userID);
        return BorrowerContact.QueryBorrowers(user, criteria, RelatedLoanMatchType.None, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual BorrowerInfo[] QueryBorrowers(QueryCriterion[] criteria)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBorrowers), (object[]) criteria);
      try
      {
        return BorrowerContact.QueryBorrowers(this.Session.GetUserInfo(), criteria, RelatedLoanMatchType.None, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual BizPartnerInfo[] QueryMaxAccessibleBizPartners(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryMaxAccessibleBizPartners), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        UserInfo user = this.Session.GetUserInfo();
        if (user.IsSuperAdministrator())
          user = (UserInfo) null;
        return BizPartnerContact.QueryBizPartners(user, criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizPartnerInfo[]) null;
      }
    }

    public virtual ICursor OpenMaxAccessibleBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenMaxAccessibleBizPartnerCursor), new object[5]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summariesOnly
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        if (userInfo.IsSuperAdministrator())
          userInfo = (UserInfo) null;
        return (ICursor) InterceptionUtils.NewInstance<BizPartnerCursor>().Initialize(this.Session, BizPartnerContact.QueryBizPartnerIds(userInfo, criteria, loanMatchType, sortOrder), fields, summariesOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual int[] QueryBorrowerIds(
      string userId,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBorrowerIds), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        UserInfo user = (UserInfo) null;
        using (User latestVersion = UserStore.GetLatestVersion(userId))
          user = latestVersion.Exists ? latestVersion.UserInfo : throw new ObjectNotFoundException("Invalid user ID '" + userId + "'", ObjectType.User, (object) userId);
        return BorrowerContact.QueryBorrowerIds(user, criteria, matchType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (int[]) null;
      }
    }

    public virtual BorrowerInfo[] QueryAllBorrowers(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryAllBorrowers), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        return BorrowerContact.QueryBorrowers((UserInfo) null, criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual BorrowerInfo[] QueryBorrowersConflict(QueryCriterion[] criteria)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBorrowersConflict), (object[]) criteria);
      try
      {
        return BorrowerContact.QueryBorrowersConflict(criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual BorrowerInfo[] QueryBorrowers(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBorrowers), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        return BorrowerContact.QueryBorrowers(this.Session.GetUserInfo(), criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerInfo[]) null;
      }
    }

    public virtual BorrowerSummaryInfo[] QueryBorrowerSummaries(
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryBorrowerSummaries), new object[2]
      {
        (object) criteria,
        (object) matchType
      });
      try
      {
        return BorrowerContact.QueryBorrowerSummaries(this.Session.GetUserInfo(), criteria, matchType, (SortField[]) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerSummaryInfo[]) null;
      }
    }

    public virtual ICursor OpenBorrowerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenBorrowerCursor), new object[5]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summaryOnly
      });
      try
      {
        if (fields == null || fields.Length == 0)
          fields = this.defaultBorrowerFields;
        return (ICursor) InterceptionUtils.NewInstance<BorrowerCursor>().Initialize(this.Session, BorrowerContact.QueryBorrowerIds(this.Session.GetUserInfo(), criteria, loanMatchType, sortOrder), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenBorrowerCursorForUser(
      string userID,
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenBorrowerCursorForUser), new object[6]
      {
        (object) userID,
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summaryOnly
      });
      try
      {
        if (fields == null || fields.Length == 0)
          fields = this.defaultBorrowerFields;
        User latestVersion = UserStore.GetLatestVersion(userID);
        if (latestVersion == null)
          throw new Exception("User '" + userID + "' does not exists");
        return (ICursor) InterceptionUtils.NewInstance<BorrowerCursor>().Initialize(this.Session, BorrowerContact.QueryBorrowerIds(latestVersion.UserInfo, criteria, loanMatchType, sortOrder), fields, summaryOnly, latestVersion.UserInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenBorrowerSummaryCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder)
    {
      this.onApiCalled(nameof (ContactManager), "OpenBorrowerCursor", new object[3]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<BorrowerCursor>().Initialize(this.Session, BorrowerContact.QueryBorrowerIds(this.Session.GetUserInfo(), criteria, loanMatchType, sortOrder), this.defaultBorrowerFields, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenAllBorrowerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenAllBorrowerCursor), new object[5]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summaryOnly
      });
      try
      {
        if (fields == null || fields.Length == 0)
          fields = this.defaultBorrowerFields;
        return (ICursor) InterceptionUtils.NewInstance<BorrowerCursor>().Initialize(this.Session, BorrowerContact.QueryBorrowerIds((UserInfo) null, criteria, loanMatchType, sortOrder), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenBorrowerCursor(
      ContactQueryInfo queryInfo,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenBorrowerCursor), new object[4]
      {
        (object) queryInfo,
        (object) sortOrder,
        (object) fields,
        (object) summaryOnly
      });
      try
      {
        if (fields == null || fields.Length == 0)
          fields = this.defaultBorrowerFields;
        return (ICursor) InterceptionUtils.NewInstance<BorrowerCursor>().Initialize(this.Session, ContactGroupProvider.GetContactQueryMemberIds(UserStore.GetLatestVersion(this.Session.UserID), queryInfo, sortOrder), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenMaxAccessibleBorrowerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (OpenMaxAccessibleBorrowerCursor), new object[5]
      {
        (object) criteria,
        (object) loanMatchType,
        (object) sortOrder,
        (object) fields,
        (object) summaryOnly
      });
      try
      {
        UserInfo user = this.Session.GetUserInfo();
        if (user.IsSuperAdministrator())
          user = (UserInfo) null;
        return (ICursor) InterceptionUtils.NewInstance<BorrowerCursor>().Initialize(this.Session, BorrowerContact.QueryBorrowerIds(user, criteria, loanMatchType, sortOrder), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ContactNote[] GetNotesForContact(int contactId, ContactType contactType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetNotesForContact), new object[2]
      {
        (object) contactId,
        (object) contactType
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          ContactNote[] notes = latestVersion.GetNotes();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved history for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
          return notes;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactNote[]) null;
      }
    }

    public virtual ContactNote GetNoteForContact(
      int contactId,
      ContactType contactType,
      int noteId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetNoteForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) noteId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
          return latestVersion.GetNote(noteId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactNote) null;
      }
    }

    public virtual ContactNote GetContactNote(int noteId, ContactType contactType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetContactNote), new object[2]
      {
        (object) contactType,
        (object) noteId
      });
      try
      {
        return EllieMae.EMLite.Server.Contact.GetContactNote(noteId, contactType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactNote) null;
      }
    }

    public virtual void AddCreditScoresForHistoryItem(
      int contactId,
      int historyId,
      ContactCreditScores[] contactScoresList)
    {
      this.onApiCalled(nameof (ContactManager), "SetCreditScoresForHistoryItem", new object[2]
      {
        (object) contactId,
        (object) historyId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, ContactType.Borrower))
          contact.AddCreditScoresForHistoryItem(historyId, contactScoresList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactCreditScores[] GetCreditScoresForHistoryItem(int contactId, int historyId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetCreditScoresForHistoryItem), new object[2]
      {
        (object) contactId,
        (object) historyId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, ContactType.Borrower))
          return contact.GetCreditScoresForHistoryItem(historyId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return new ContactCreditScores[0];
      }
    }

    public virtual int AddNoteForContact(int contactId, ContactType contactType, ContactNote note)
    {
      this.onApiCalled(nameof (ContactManager), nameof (AddNoteForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) note
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          int num = latestVersion.AddNote(note);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Added note for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
          return num;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateNoteForContact(
      int contactId,
      ContactType contactType,
      ContactNote note)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateNoteForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) note
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          latestVersion.UpdateNote(note);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Updated note " + (object) note.NoteID + " for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteNoteForContact(int contactId, ContactType contactType, int noteId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteNoteForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) noteId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          latestVersion.DeleteNote(noteId);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Updated note " + (object) noteId + " for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactHistoryItem[] GetHistoryForContact(int contactId, ContactType contactType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetHistoryForContact), new object[2]
      {
        (object) contactId,
        (object) contactType
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          ContactHistoryItem[] history = latestVersion.GetHistory();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved history for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
          return history;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactHistoryItem[]) null;
      }
    }

    public virtual ContactHistoryItem[] GetHistoryForContact(
      int contactId,
      ContactType contactType,
      DateTime startDate,
      DateTime endDate)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetHistoryForContact), new object[4]
      {
        (object) contactId,
        (object) contactType,
        (object) startDate,
        (object) endDate
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          ContactHistoryItem[] history = latestVersion.GetHistory(startDate, endDate);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved history for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
          return history;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactHistoryItem[]) null;
      }
    }

    public virtual ContactHistoryItem[] GetHistoryForContact(
      int contactId,
      ContactType contactType,
      string eventType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetHistoryForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) eventType
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          ContactHistoryItem[] history = latestVersion.GetHistory(eventType);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved history for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
          return history;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactHistoryItem[]) null;
      }
    }

    public virtual ContactHistoryItem[] GetHistoryForContact(
      int contactId,
      ContactType contactType,
      string eventType,
      DateTime startDate,
      DateTime endDate)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetHistoryForContact), new object[5]
      {
        (object) contactId,
        (object) contactType,
        (object) eventType,
        (object) startDate,
        (object) endDate
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          ContactHistoryItem[] history = latestVersion.GetHistory(eventType, startDate, endDate);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved history for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
          return history;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactHistoryItem[]) null;
      }
    }

    public virtual ContactHistoryItem GetHistoryItemForContact(
      int contactId,
      ContactType contactType,
      int itemId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetHistoryItemForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) itemId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
          return latestVersion.GetHistoryItem(itemId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactHistoryItem) null;
      }
    }

    public virtual int AddHistoryItemForContact(
      int contactId,
      ContactType contactType,
      ContactHistoryItem item)
    {
      this.onApiCalled(nameof (ContactManager), nameof (AddHistoryItemForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) item
      });
      if (item == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("History item cannot be null", nameof (item), this.Session.SessionInfo));
      try
      {
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, contactType))
        {
          int num = contact.AddHistoryItem(item);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Added history item for " + (object) contact.ContactType + " \"" + contact.FullName + "\" (" + (object) contactId + ")"));
          return num;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void AddHistoryItemForContacts(
      string[] contactGuids,
      ContactType contactType,
      ContactHistoryItem item)
    {
      this.onApiCalled(nameof (ContactManager), nameof (AddHistoryItemForContacts), new object[3]
      {
        (object) contactGuids,
        (object) contactType,
        (object) item
      });
      if (item == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("History item cannot be null", nameof (item), this.Session.SessionInfo));
      try
      {
        foreach (string contactGuid in contactGuids)
        {
          int contactId = contactType != ContactType.Borrower ? BizPartnerContact.GetBizPartnerIDFromGuid(contactGuid) : BorrowerContact.GetBorrowerIDFromGuid(contactGuid);
          using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, contactType))
          {
            contact.AddHistoryItem(item);
            TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Added history item for " + (object) contact.ContactType + " \"" + contact.FullName + "\" (" + (object) contactId + ")"));
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteHistoryItemForContact(
      int contactId,
      ContactType contactType,
      int historyItemId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteHistoryItemForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) historyItemId
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, contactType))
        {
          contact.DeleteHistoryItem(historyItemId);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Deleted history item for " + (object) contact.ContactType + " \"" + contact.FullName + "\" (" + (object) contactId + ")"));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactHistoryNoteInfo GetContactHistoryNote(int noteId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetContactHistoryNote), new object[1]
      {
        (object) noteId
      });
      try
      {
        return EllieMae.EMLite.Server.Contact.GetContactHistoryNote(noteId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactHistoryNoteInfo) null;
      }
    }

    public virtual ContactHistoryNoteInfo[] GetContactHistoryNotes(int[] noteIds)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetContactHistoryNotes), new object[1]
      {
        (object) noteIds
      });
      try
      {
        return EllieMae.EMLite.Server.Contact.GetContactHistoryNotes(noteIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactHistoryNoteInfo[]) null;
      }
    }

    public virtual BizCategory[] GetBizCategories()
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizCategories), Array.Empty<object>());
      try
      {
        return BizPartnerContact.GetBizCategories();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizCategory[]) null;
      }
    }

    public virtual void DeleteBizCategory(BizCategory cat)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBizCategory), new object[1]
      {
        (object) cat
      });
      try
      {
        BizPartnerContact.DeleteBizCategory(cat);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BizCategory AddBizCategory(string categoryName)
    {
      this.onApiCalled(nameof (ContactManager), nameof (AddBizCategory), new object[1]
      {
        (object) categoryName
      });
      if (categoryName == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("Category name cannot be null", nameof (categoryName), this.Session.SessionInfo));
      try
      {
        return BizPartnerContact.AddBizCategory(categoryName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BizCategory) null;
      }
    }

    public virtual void UpdateBizCategory(BizCategory cat)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateBizCategory), new object[1]
      {
        (object) cat
      });
      if (cat == (BizCategory) null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("Category cannot be null", nameof (cat), this.Session.SessionInfo));
      try
      {
        BizPartnerContact.UpdateBizCategory(cat);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int AddContactLoan(ContactLoanInfo info)
    {
      this.onApiCalled(nameof (ContactManager), nameof (AddContactLoan), new object[1]
      {
        (object) info
      });
      try
      {
        return ContactLoans.AddLoan(info);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual ContactLoanInfo GetContactLoan(int loanId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetContactLoan), new object[1]
      {
        (object) loanId
      });
      try
      {
        return ContactLoans.GetLoan(loanId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactLoanInfo) null;
      }
    }

    public virtual ContactLoanInfo[] GetBorrowerLoans(int borrowerId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrowerLoans), new object[1]
      {
        (object) borrowerId
      });
      try
      {
        return ContactLoans.GetBorrowerLoans(borrowerId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactLoanInfo[]) null;
      }
    }

    public virtual ContactLoanInfo GetLastClosedLoanForContact(
      int contactId,
      ContactType contactType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetLastClosedLoanForContact), new object[2]
      {
        (object) contactId,
        (object) contactType
      });
      try
      {
        return ContactLoans.GetLastClosedLoanForContact(contactId, contactType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactLoanInfo) null;
      }
    }

    public virtual ContactCustomFieldInfoCollection GetCustomFieldInfo(ContactType contactType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetCustomFieldInfo), new object[1]
      {
        (object) contactType
      });
      try
      {
        ContactCustomFieldInfoCollection customFieldInfo = EllieMae.EMLite.Server.Contact.GetCustomFieldInfo(contactType);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved custom fields for " + (object) contactType));
        return customFieldInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactCustomFieldInfoCollection) null;
      }
    }

    public virtual void UpdateCustomFieldInfo(
      ContactType contactType,
      ContactCustomFieldInfoCollection customFields)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateCustomFieldInfo), new object[2]
      {
        (object) contactType,
        (object) customFields
      });
      try
      {
        EllieMae.EMLite.Server.Contact.UpdateCustomFieldInfo(contactType, customFields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactCustomField[] GetCustomFieldsForContact(
      int contactId,
      ContactType contactType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetCustomFieldsForContact), new object[2]
      {
        (object) contactId,
        (object) contactType
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact latestVersion = this.getLatestVersion(contactId, contactType))
        {
          ContactCustomField[] customFields = latestVersion.GetCustomFields();
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved custom fields for " + (object) latestVersion.ContactType + " \"" + latestVersion.FullName + "\" (" + (object) contactId + ")"));
          return customFields;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactCustomField[]) null;
      }
    }

    public virtual void UpdateCustomFieldsForContact(
      int contactId,
      ContactType contactType,
      ContactCustomField[] fields)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateCustomFieldsForContact), new object[3]
      {
        (object) contactId,
        (object) contactType,
        (object) fields
      });
      try
      {
        using (EllieMae.EMLite.Server.Contact contact = this.checkOut(contactId, contactType))
        {
          contact.UpdateCustomFields(fields);
          TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Updated custom fields for " + (object) contact.ContactType + " \"" + contact.FullName + "\" (" + (object) contactId + ")"));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CustomFieldsDefinitionInfo[] GetCustomFieldsDefinitions(
      CustomFieldsType customFieldsType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetCustomFieldsDefinitions), new object[1]
      {
        (object) customFieldsType
      });
      try
      {
        return BizPartnerContact.GetCustomFieldsDefinitions(customFieldsType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return new CustomFieldsDefinitionInfo[0];
      }
    }

    public virtual CustomFieldsDefinitionInfo GetCustomFieldsDefinition(
      CustomFieldsType customFieldsType,
      int recordId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetCustomFieldsDefinition), new object[1]
      {
        (object) customFieldsType
      });
      try
      {
        return BizPartnerContact.GetCustomFieldsDefinition(customFieldsType, recordId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (CustomFieldsDefinitionInfo) null;
      }
    }

    public virtual CustomFieldsDefinitionInfo UpdateCustomFieldsDefinition(
      CustomFieldsDefinitionInfo customFieldsDefinitionInfo)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateCustomFieldsDefinition), new object[1]
      {
        (object) customFieldsDefinitionInfo
      });
      try
      {
        CustomFieldsDefinitionInfo fieldsDefinitionInfo = BizPartnerContact.UpdateCustomFieldsDefinition(customFieldsDefinitionInfo);
        if (customFieldsDefinitionInfo.CustomFieldDefinitions != null && customFieldsDefinitionInfo.CustomFieldDefinitions.Length != 0)
        {
          FieldSearchRule fieldSearchRule = new FieldSearchRule(fieldsDefinitionInfo == null ? new CustomFieldsDefinitionInfo() : fieldsDefinitionInfo);
          string identifier = "Category:" + customFieldsDefinitionInfo.CustomFieldDefinitions[0].CategoryId.ToString();
          int parentFsRuleId = FieldSearchDbAccessor.FindParentFSRuleId(0, FieldSearchRuleType.BusinessCustomFields, identifier);
          if (parentFsRuleId != 0)
            fieldSearchRule.ParentFSRuleId = new int?(parentFsRuleId);
          fieldSearchRule.Identifier = identifier;
          if (fieldSearchRule.FieldSearchFields.Count > 0)
            FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule);
          else
            FieldSearchDbAccessor.DeleteFieldSearchInfoWhithDataCheck(0, FieldSearchRuleType.BusinessCustomFields, identifier);
        }
        return fieldsDefinitionInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (CustomFieldsDefinitionInfo) null;
      }
    }

    public virtual CustomFieldsValueInfo GetCustomFieldsValues(int contactId, int categoryId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetCustomFieldsValues), new object[1]
      {
        (object) contactId
      });
      try
      {
        return BizPartnerContact.GetCustomFieldsValues(contactId, categoryId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (CustomFieldsValueInfo) null;
      }
    }

    public virtual CustomFieldsValueInfo UpdateCustomFieldsValues(
      CustomFieldsValueInfo customFieldsValues)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateCustomFieldsValues), new object[1]
      {
        (object) customFieldsValues
      });
      try
      {
        return BizPartnerContact.UpdateCustomFieldsValues(customFieldsValues);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (CustomFieldsValueInfo) null;
      }
    }

    public virtual void DeleteContactCustomFieldValues(ContactType contactType, int[] fieldIds)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteContactCustomFieldValues), Array.Empty<object>());
      try
      {
        EllieMae.EMLite.Server.Contact.DeleteContactCustomFieldValues(contactType, fieldIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CustomFieldsMappingInfo GetCustomFieldsMapping(
      CustomFieldsType customFieldsType,
      int categoryId,
      bool twoWayTransfersOnly)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetCustomFieldsMapping), new object[1]
      {
        (object) customFieldsType
      });
      try
      {
        return EllieMae.EMLite.Server.Contact.GetCustomFieldsMapping(customFieldsType, categoryId, twoWayTransfersOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return new CustomFieldsMappingInfo();
      }
    }

    public virtual void AddBizPartnerQuery(string userid, ContactQuery query)
    {
      this.onApiCalled(nameof (ContactManager), nameof (AddBizPartnerQuery), new object[1]
      {
        (object) userid
      });
      try
      {
        BizPartnerQueryStore.Add(userid, query);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBizPartnerQuery(string userid, ContactQuery query)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBizPartnerQuery), new object[1]
      {
        (object) userid
      });
      try
      {
        BizPartnerQueryStore.Delete(userid, query);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBizPartnerQuery(string userid, ContactQuery query)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateBizPartnerQuery), new object[1]
      {
        (object) userid
      });
      try
      {
        BizPartnerQueryStore.Update(userid, query);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactQueries GetBizPartnerQueries(string userid)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBizPartnerQueries), new object[1]
      {
        (object) userid
      });
      try
      {
        return BizPartnerQueryStore.Get(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactQueries) null;
      }
    }

    public virtual void SetBorrowerQueries(string userid, ContactQueries queries)
    {
      this.onApiCalled(nameof (ContactManager), "SetContactQueries", new object[1]
      {
        (object) userid
      });
      try
      {
        BorrowerQueryStore.Set(userid, queries);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddBorrowerQuery(string userid, ContactQuery query)
    {
      this.onApiCalled(nameof (ContactManager), "AddContactQuery", new object[1]
      {
        (object) userid
      });
      try
      {
        BorrowerQueryStore.Add(userid, query);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactQueries GetBorrowerQueries(string userid)
    {
      this.onApiCalled(nameof (ContactManager), "GetContactQueries", new object[1]
      {
        (object) userid
      });
      try
      {
        return BorrowerQueryStore.Get(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactQueries) null;
      }
    }

    public virtual void DeleteBorrowerQuery(string userid, ContactQuery query)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBorrowerQuery), new object[1]
      {
        (object) userid
      });
      try
      {
        BorrowerQueryStore.Delete(userid, query);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBorrowerQuery(string userid, ContactQuery query)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateBorrowerQuery), new object[1]
      {
        (object) userid
      });
      try
      {
        BorrowerQueryStore.Update(userid, query);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BorrowerStatus GetBorrowerStatus()
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrowerStatus), Array.Empty<object>());
      try
      {
        return ContactAccessor.GetBorrowerContactStatusList();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BorrowerStatus) null;
      }
    }

    public virtual void UpdateBorrowerStatusItem(
      int index,
      BorrowerStatusItem item,
      string oldName)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateBorrowerStatusItem), Array.Empty<object>());
      if (item == (BorrowerStatusItem) null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("BorrowerStatusItem[] cannot be null", nameof (item), this.Session.SessionInfo));
      try
      {
        ContactAccessor.UpdateBorrowerStatusItem(index, item, oldName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RenameStatusInBorrowerTable(string oldStatus, string newStatus)
    {
      this.onApiCalled(nameof (ContactManager), nameof (RenameStatusInBorrowerTable), new object[2]
      {
        (object) oldStatus,
        (object) newStatus
      });
      try
      {
        BorrowerContact.RenameStatus(oldStatus, newStatus);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetBorrowerStatus(BorrowerStatusItem item)
    {
      this.onApiCalled(nameof (ContactManager), nameof (SetBorrowerStatus), new object[1]
      {
        (object) item
      });
      if (item == (BorrowerStatusItem) null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("BorrowerStatusItem cannot be null", nameof (item), this.Session.SessionInfo));
      try
      {
        ContactAccessor.SetBorrowerStatus(new BorrowerStatusItem[1]
        {
          item
        });
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetBorrowerStatus(BorrowerStatusItem[] items)
    {
      this.onApiCalled(nameof (ContactManager), nameof (SetBorrowerStatus), Array.Empty<object>());
      if (items == null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("BorrowerStatusItem[] cannot be null", nameof (items), this.Session.SessionInfo));
      try
      {
        ContactAccessor.SetBorrowerStatus(items);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CreateBorrowerStatus(BorrowerStatusItem item)
    {
      this.onApiCalled(nameof (ContactManager), nameof (CreateBorrowerStatus), new object[1]
      {
        (object) item
      });
      if (item == (BorrowerStatusItem) null)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ServerArgumentException("BorrowerStatusItem cannot be null", nameof (item), this.Session.SessionInfo));
      try
      {
        ContactAccessor.CreateBorrowerStatus(item);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBorrowerOpportunity(Opportunity item)
    {
      this.onApiCalled(nameof (ContactManager), nameof (UpdateBorrowerOpportunity), new object[1]
      {
        (object) item
      });
      try
      {
        BorrowerOpportunity.UpdateBorrowerOpportunity(item);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Updated opportunity for opportunity ID: " + (object) item.OpportunityID));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBorrowerOpportunity(int itemId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteBorrowerOpportunity), new object[1]
      {
        (object) itemId
      });
      try
      {
        BorrowerOpportunity.DeleteBorrowerOpportunity(itemId);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Deleted opportunity ID: " + (object) itemId));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteOpportunityByBorrowerId(int contactId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteOpportunityByBorrowerId), new object[1]
      {
        (object) contactId
      });
      try
      {
        BorrowerOpportunity.DeleteOpportunityByBorrowerId(contactId);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Deleted opportunity of borrower whose ID is: " + (object) contactId));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int CreateBorrowerOpportunity(Opportunity item)
    {
      this.onApiCalled(nameof (ContactManager), nameof (CreateBorrowerOpportunity), new object[1]
      {
        (object) item
      });
      try
      {
        int borrowerOpportunity = BorrowerOpportunity.CreateBorrowerOpportunity(item);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Updated opportunity for opportunity ID: " + (object) item.OpportunityID));
        return borrowerOpportunity;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual Opportunity GetBorrowerOpportunity(int itemId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetBorrowerOpportunity), new object[1]
      {
        (object) itemId
      });
      try
      {
        Opportunity borrowerOpportunity = BorrowerOpportunity.GetBorrowerOpportunity(itemId);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved opportunity for opportunity ID: " + (object) borrowerOpportunity.OpportunityID));
        return borrowerOpportunity;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (Opportunity) null;
      }
    }

    public virtual Opportunity GetOpportunityByBorrowerId(int borrowerId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetOpportunityByBorrowerId), new object[1]
      {
        (object) borrowerId
      });
      try
      {
        Opportunity opportunityByBorrowerId = BorrowerOpportunity.GetOpportunityByBorrowerId(borrowerId);
        TraceLog.WriteInfo(nameof (ContactManager), this.formatMsg("Retrieved opportunity for borrower ID: " + (object) borrowerId));
        return opportunityByBorrowerId;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (Opportunity) null;
      }
    }

    public virtual string[] GetSyncConfigurationNames()
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetSyncConfigurationNames), Array.Empty<object>());
      try
      {
        return SyncConfigurations.GetSyncConfigurationNames(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual BinaryObject GetSyncConfiguration(string name)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetSyncConfiguration), new object[1]
      {
        (object) name
      });
      try
      {
        return BinaryObject.Marshal(SyncConfigurations.GetSyncConfiguration(this.Session.UserID, name));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveSyncConfiguration(string name, BinaryObject o)
    {
      this.onApiCalled(nameof (ContactManager), nameof (SaveSyncConfiguration), new object[2]
      {
        (object) name,
        (object) o
      });
      o.Download();
      try
      {
        SyncConfigurations.SaveSyncConfiguration(this.Session.UserID, name, o);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RenameSyncConfiguration(BinaryObject o, string oldName, string newName)
    {
      this.onApiCalled(nameof (ContactManager), nameof (RenameSyncConfiguration), new object[3]
      {
        (object) o,
        (object) oldName,
        (object) newName
      });
      o.Download();
      try
      {
        SyncConfigurations.RenameSyncConfiguration(this.Session.UserID, o, oldName, newName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteSyncConfiguration(string name)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteSyncConfiguration), new object[1]
      {
        (object) name
      });
      try
      {
        SyncConfigurations.DeleteSyncConfiguration(this.Session.UserID, name);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BinaryObject GetSyncMap(string name)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetSyncMap), new object[1]
      {
        (object) name
      });
      try
      {
        return BinaryObject.Marshal(SyncConfigurations.GetSyncMap(this.Session.UserID, name));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveSyncMap(string name, BinaryObject o)
    {
      this.onApiCalled(nameof (ContactManager), nameof (SaveSyncMap), new object[2]
      {
        (object) name,
        (object) o
      });
      o.Download();
      try
      {
        SyncConfigurations.SaveSyncMap(this.Session.UserID, name, o);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DataTable GetUnSyncBorrowerContacts(string loanFolder)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetUnSyncBorrowerContacts), new object[1]
      {
        (object) loanFolder
      });
      try
      {
        return BorrowerContact.GetUnSyncBorrowerContacts(loanFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (DataTable) null;
      }
    }

    public virtual ContactLoanPair[] GetRelatedLoansForContact(
      int contactId,
      ContactType contactType)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetRelatedLoansForContact), new object[2]
      {
        (object) contactId,
        (object) contactType
      });
      try
      {
        return EllieMae.EMLite.Server.Contact.GetRelatedLoansForContact(contactId, contactType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactLoanPair[]) null;
      }
    }

    public virtual ContactLoanPair[] GetRelatedLoansForContact(string contactGuid)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetRelatedLoansForContact), new object[1]
      {
        (object) contactGuid
      });
      try
      {
        return EllieMae.EMLite.Server.Contact.GetRelatedLoansForContact(contactGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactLoanPair[]) null;
      }
    }

    public virtual ContactLoanPair[] GetRelatedContactsForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetRelatedContactsForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return EllieMae.EMLite.Server.Contact.GetRelatedContactsForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (ContactLoanPair[]) null;
      }
    }

    public virtual TaskInfo GetTask(int taskId)
    {
      this.onApiCalled(nameof (ContactManager), nameof (GetTask), new object[1]
      {
        (object) taskId
      });
      try
      {
        return EllieMae.EMLite.Server.TaskList.GetTask(taskId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (TaskInfo) null;
      }
    }

    public virtual int InsertOrUpdateTask(TaskInfo task)
    {
      this.onApiCalled(nameof (ContactManager), "InsertTask", new object[1]
      {
        (object) task
      });
      try
      {
        return EllieMae.EMLite.Server.TaskList.InsertOrUpdateTask(task);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void DeleteTask(int taskID)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteTask), new object[1]
      {
        (object) taskID
      });
      try
      {
        EllieMae.EMLite.Server.TaskList.DeleteTask(taskID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteTasks(int[] taskIDs)
    {
      this.onApiCalled(nameof (ContactManager), nameof (DeleteTasks), new object[1]
      {
        (object) taskIDs
      });
      try
      {
        EllieMae.EMLite.Server.TaskList.DeleteTasks(taskIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual TaskInfo[] GetAllTasksForUser(string userid)
    {
      this.onApiCalled(nameof (ContactManager), "GetTasks", new object[1]
      {
        (object) userid
      });
      try
      {
        return EllieMae.EMLite.Server.TaskList.GetAllTasksForUser(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (TaskInfo[]) null;
      }
    }

    public virtual TaskInfo[] QueryTasks(QueryCriterion criteria)
    {
      return this.QueryTasks(criteria, false);
    }

    public virtual TaskInfo[] QueryTasks(QueryCriterion criteria, bool fromHomePage)
    {
      this.onApiCalled(nameof (ContactManager), nameof (QueryTasks), new object[2]
      {
        (object) criteria,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.HomePage)
      });
      try
      {
        return EllieMae.EMLite.Server.TaskList.QueryTasks(criteria, fromHomePage);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactManager), ex, this.Session.SessionInfo);
        return (TaskInfo[]) null;
      }
    }

    private EllieMae.EMLite.Server.Contact checkOut(int contactId, ContactType contactType)
    {
      EllieMae.EMLite.Server.Contact contact = ContactStore.CheckOut(contactId, contactType);
      if (!contact.Exists)
      {
        contact.UndoCheckout();
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ObjectNotFoundException("Specified contact cannot be found.", ObjectType.Contact, (object) contactId));
      }
      try
      {
        this.Security.DemandContactRights((IContact) contact);
      }
      catch
      {
        contact.UndoCheckout();
        throw;
      }
      return contact;
    }

    private EllieMae.EMLite.Server.Contact getLatestVersion(int contactId, ContactType contactType)
    {
      EllieMae.EMLite.Server.Contact latestVersion = ContactStore.GetLatestVersion(contactId, contactType);
      if (!latestVersion.Exists)
        Err.Raise(TraceLevel.Warning, nameof (ContactManager), (ServerException) new ObjectNotFoundException("Specified contact cannot be found.", ObjectType.Contact, (object) contactId));
      try
      {
        this.Security.DemandContactRights((IContact) latestVersion);
      }
      catch
      {
        throw;
      }
      return latestVersion;
    }
  }
}
