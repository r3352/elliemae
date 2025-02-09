// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.ContactQuery
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public class ContactQuery : BusinessBase, IDisposable
  {
    private ContactQueryInfo queryInfo;
    [NotUndoable]
    private SessionObjects sessionObjects;

    public int QueryId => this.queryInfo.QueryId;

    public string UserId => this.queryInfo.UserId;

    public ContactType ContactType
    {
      get => this.queryInfo.ContactType;
      set => this.queryInfo.ContactType = value;
    }

    public ContactQueryType QueryType
    {
      get => this.queryInfo.QueryType;
      set
      {
        if (this.queryInfo.QueryType == value)
          return;
        this.queryInfo.QueryType = value;
        this.MarkDirty();
      }
    }

    public string QueryName
    {
      get => this.queryInfo.QueryName;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.queryInfo.QueryName != value))
          return;
        this.queryInfo.QueryName = value;
        this.BrokenRules.Assert("QueryNameRequired", "Query Name is a required field", this.queryInfo.QueryName.Length < 1);
        this.BrokenRules.Assert("QueryNameLength", "Query Name exceeds 64 characters", this.queryInfo.QueryName.Length > 64);
        this.MarkDirty();
      }
    }

    public string QueryDesc
    {
      get => this.queryInfo.QueryDesc;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.queryInfo.QueryDesc != value))
          return;
        this.queryInfo.QueryDesc = value;
        this.BrokenRules.Assert("ContactQueryDescriptionLength", "Contact Query Description exceeds 250 characters", this.queryInfo.QueryDesc.Length > 250);
        this.MarkDirty();
      }
    }

    public string XmlQueryString
    {
      get => this.queryInfo.XmlQueryString;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.queryInfo.XmlQueryString != value))
          return;
        this.queryInfo.XmlQueryString = value;
        this.BrokenRules.Assert("XmlQueryStringRequired", "You must create a query", this.queryInfo.XmlQueryString.Length < 1);
        this.BrokenRules.Assert("XmlQueryStringLength", "XmlQueryString exceeds 6000 characters", this.queryInfo.XmlQueryString.Length > 6000);
        this.MarkDirty();
      }
    }

    public bool PrimaryOnly
    {
      get => this.queryInfo.PrimaryOnly;
      set
      {
        if (this.queryInfo.PrimaryOnly == value)
          return;
        this.queryInfo.PrimaryOnly = value;
        this.MarkDirty();
      }
    }

    internal ContactQueryInfo GetInfo()
    {
      this.queryInfo.IsNew = this.IsNew;
      this.queryInfo.IsDirty = this.IsDirty;
      this.queryInfo.IsDeleted = this.IsDeleted;
      return this.queryInfo;
    }

    public override string ToString() => string.Format("ContactQuery[{0}]", (object) this.QueryId);

    public bool Equals(ContactQuery contactQuery) => this.QueryId.Equals(contactQuery.QueryId);

    public new static bool Equals(object objA, object objB)
    {
      return objA is ContactQuery && objB is ContactQuery && ((ContactQuery) objA).Equals((ContactQuery) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is ContactQuery && this.Equals((ContactQuery) obj);
    }

    public override int GetHashCode()
    {
      return string.Format("{0}", (object) this.queryInfo.QueryId).GetHashCode();
    }

    public override BusinessBase Save()
    {
      if (this.IsDeleted)
      {
        if (!this.IsNew)
          this.sessionObjects.ContactGroupManager.DeleteContactQuery(this.QueryId);
        this.MarkNew();
      }
      else if (this.IsDirty)
      {
        this.queryInfo.IsNew = this.IsNew;
        this.queryInfo.IsDirty = this.IsDirty;
        this.queryInfo.IsDeleted = this.IsDeleted;
        this.queryInfo = this.sessionObjects.ContactGroupManager.SaveContactQuery(this.queryInfo);
        this.MarkOld();
      }
      return (BusinessBase) this;
    }

    public static ContactQuery NewContactQuery(
      ContactType contactType,
      ContactQueryType queryType,
      SessionObjects sessionObjects)
    {
      return new ContactQuery(contactType, queryType, sessionObjects);
    }

    public static ContactQuery NewContactQuery(ContactQueryInfo queryInfo)
    {
      return new ContactQuery(queryInfo);
    }

    public static ContactQuery NewContactQuery(
      CampaignTemplate.ContactQueryTemplate queryTemplate,
      SessionObjects sessionObjects)
    {
      return new ContactQuery(queryTemplate, sessionObjects);
    }

    public static ContactQuery GetContactQuery(int queryId, SessionObjects sessionObjects)
    {
      return new ContactQuery(sessionObjects.ContactGroupManager.GetContactQuery(queryId));
    }

    public static void DeleteContactQuery(int queryId, SessionObjects sessionObjects)
    {
      sessionObjects.ContactGroupManager.DeleteContactQuery(queryId);
    }

    private ContactQuery(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.queryInfo = new ContactQueryInfo(0, this.sessionObjects.UserID, ContactType.Borrower, (ContactQueryType) 0, string.Empty, string.Empty, string.Empty, false);
      this.BrokenRules.Assert("QueryNameRequired", "Query Name is a required field", this.queryInfo.QueryName.Length < 1);
      this.BrokenRules.Assert("XmlQueryStringRequired", "You must create a query", this.queryInfo.XmlQueryString.Length < 1);
    }

    private ContactQuery(
      ContactType contactType,
      ContactQueryType queryType,
      SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.queryInfo = new ContactQueryInfo(0, this.sessionObjects.UserID, contactType, queryType, string.Empty, string.Empty, string.Empty, false);
      this.BrokenRules.Assert("QueryNameRequired", "Query Name is a required field", this.queryInfo.QueryName.Length < 1);
      this.BrokenRules.Assert("XmlQueryStringRequired", "You must create a query", this.queryInfo.XmlQueryString.Length < 1);
    }

    private ContactQuery(ContactQueryInfo queryInfo)
    {
      this.MarkOld();
      this.queryInfo = queryInfo;
    }

    private ContactQuery(
      CampaignTemplate.ContactQueryTemplate queryTemplate,
      SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.queryInfo = new ContactQueryInfo(0, this.sessionObjects.UserID, (ContactType) new ContactTypeNameProvider().GetValue(queryTemplate.GetField(nameof (ContactType))), (ContactQueryType) Enum.Parse(typeof (ContactQueryType), queryTemplate.GetField(nameof (QueryType))), Guid.NewGuid().ToString(), queryTemplate.GetField(nameof (QueryDesc)), queryTemplate.GetField(nameof (XmlQueryString)), bool.Parse(queryTemplate.GetField(nameof (PrimaryOnly))));
      this.BrokenRules.Assert("QueryNameRequired", "Query Name is a required field", this.queryInfo.QueryName.Length < 1);
      this.BrokenRules.Assert("XmlQueryStringRequired", "You must create a query", this.queryInfo.XmlQueryString.Length < 1);
    }

    public void Dispose()
    {
    }

    [Serializable]
    private class Criteria
    {
      public int QueryId;

      public Criteria(int queryId) => this.QueryId = queryId;
    }
  }
}
