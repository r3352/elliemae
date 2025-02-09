// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.Contacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ContactUI;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class Contacts : SessionBoundObject, IContacts
  {
    private BizCategories bizCategories;
    private Hashtable customFields = new Hashtable();

    internal Contacts(Session session)
      : base(session)
    {
    }

    public Contact Open(int contactId, ContactType type)
    {
      ContactList contactList = this.Query((QueryCriterion) new NumericFieldCriterion()
      {
        FieldName = "Contact.ContactID",
        Value = (double) contactId,
        MatchType = OrdinalFieldMatchType.Equals
      }, ContactLoanMatchType.None, type);
      return contactList.Count > 0 ? contactList[0] : (Contact) null;
    }

    public Contact CreateNew(ContactType type)
    {
      if (type == ContactType.Biz)
        return (Contact) new BizContact(this.Session);
      if (type == ContactType.Borrower)
        return (Contact) new BorrowerContact(this.Session);
      throw new ArgumentException("Invalid contact type specified");
    }

    public void Delete(int contactId, ContactType type) => this.Open(contactId, type)?.Delete();

    public ContactList Query(
      QueryCriterion criterion,
      ContactLoanMatchType loanMatchType,
      ContactType type)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (type == ContactType.Biz)
        return BizContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBizPartners(new QueryCriterion[1]
        {
          criterion.Unwrap()
        }, (RelatedLoanMatchType) loanMatchType));
      if (type != ContactType.Borrower)
        throw new ArgumentException("Invalid contact type specified");
      return BorrowerContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBorrowers(new QueryCriterion[1]
      {
        criterion.Unwrap()
      }, (RelatedLoanMatchType) loanMatchType));
    }

    public ContactCursor QueryCursor(
      QueryCriterion criterion,
      ContactLoanMatchType loanMatchType,
      SortCriterionList sortCriteria,
      ContactType type)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (sortCriteria == null)
        sortCriteria = new SortCriterionList();
      if (type == ContactType.Biz)
        return new ContactCursor(this.Session, this.ContactManager.OpenBizPartnerCursor(new QueryCriterion[1]
        {
          criterion.Unwrap()
        }, (RelatedLoanMatchType) loanMatchType, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Biz);
      if (type != ContactType.Borrower)
        throw new ArgumentException("Invalid contact type specified");
      return new ContactCursor(this.Session, this.ContactManager.OpenBorrowerCursor(new QueryCriterion[1]
      {
        criterion.Unwrap()
      }, (RelatedLoanMatchType) loanMatchType, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Borrower);
    }

    public ContactList GetAll(ContactType type)
    {
      if (type == ContactType.Biz)
        return BizContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBizPartners(new QueryCriterion[0], (RelatedLoanMatchType) 0));
      if (type == ContactType.Borrower)
        return BorrowerContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBorrowers(new QueryCriterion[0], (RelatedLoanMatchType) 0));
      throw new ArgumentException("Invalid contact type specified");
    }

    public ContactCursor OpenCursor(SortCriterionList sortCriteria, ContactType type)
    {
      if (sortCriteria == null)
        sortCriteria = new SortCriterionList();
      if (type == ContactType.Biz)
        return new ContactCursor(this.Session, this.ContactManager.OpenMaxAccessibleBizPartnerCursor(new QueryCriterion[0], (RelatedLoanMatchType) 0, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Biz);
      if (type == ContactType.Borrower)
        return new ContactCursor(this.Session, this.ContactManager.OpenMaxAccessibleBorrowerCursor(new QueryCriterion[0], (RelatedLoanMatchType) 0, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Borrower);
      throw new ArgumentException("Invalid contact type specified");
    }

    public BizCategories BizCategories
    {
      get
      {
        if (this.bizCategories == null)
          this.bizCategories = new BizCategories(this.ContactManager);
        return this.bizCategories;
      }
    }

    internal IContactManager ContactManager
    {
      get => (IContactManager) this.Session.GetObject(nameof (ContactManager));
    }

    internal ContactCustomFieldInfoCollection GetCustomFieldDefinitions(ContactType contactType)
    {
      lock (this)
      {
        ContactCustomFieldInfoCollection customField = (ContactCustomFieldInfoCollection) this.customFields[(object) contactType];
        if (customField != null)
          return customField;
        ContactCustomFieldInfoCollection customFieldInfo = this.Session.Contacts.ContactManager.GetCustomFieldInfo(contactType);
        this.customFields.Add((object) contactType, (object) customFieldInfo);
        return customFieldInfo;
      }
    }
  }
}
