// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.Contact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [ComSourceInterfaces(typeof (IPersistentObjectEvents))]
  public abstract class Contact : SessionBoundObject, IContact
  {
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    protected internal IContactManager mngr;
    protected internal ContactNotes notes;
    protected internal ContactEvents events;
    protected internal ContactCustomFields customFields;

    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    protected internal Contact(Session session)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Contact), "Committed");
      this.mngr = session.Contacts.ContactManager;
    }

    public abstract int ID { get; }

    internal abstract string Guid { get; }

    public abstract string LastName { get; set; }

    public abstract string Salutation { get; set; }

    public abstract string FirstName { get; set; }

    public string FullName => Utils.JoinName(this.FirstName, this.LastName);

    public abstract string JobTitle { get; set; }

    public abstract string WorkPhone { get; set; }

    public abstract string HomePhone { get; set; }

    public abstract string MobilePhone { get; set; }

    public abstract string FaxNumber { get; set; }

    public abstract string CustField1 { get; set; }

    public abstract string CustField2 { get; set; }

    public abstract string CustField3 { get; set; }

    public abstract string CustField4 { get; set; }

    public abstract string PersonalEmail { get; set; }

    public abstract string BizEmail { get; set; }

    public abstract string BizWebUrl { get; set; }

    public abstract Address BizAddress { get; }

    public abstract bool NoSpam { get; set; }

    public abstract DateTime LastModified { get; }

    public abstract User Owner { get; set; }

    public abstract ContactAccessLevel AccessLevel { get; set; }

    public abstract ContactType Type { get; }

    public abstract void Commit();

    public abstract void Delete();

    public abstract void Refresh();

    public bool IsNew => this.ID < 0;

    public ContactNotes Notes
    {
      get
      {
        this.ensureExists();
        if (this.notes == null)
          this.notes = new ContactNotes(this);
        return this.notes;
      }
    }

    public ContactEvents Events
    {
      get
      {
        this.ensureExists();
        if (this.events == null)
          this.events = new ContactEvents(this);
        return this.events;
      }
    }

    public ContactCustomFields CustomFields
    {
      get
      {
        if (this.customFields == null)
          this.customFields = new ContactCustomFields(this);
        return this.customFields;
      }
    }

    public LoanContactRelationshipList GetLoanRelationships()
    {
      ContactLoanPair[] relatedLoansForContact = this.Session.SessionObjects.ContactManager.GetRelatedLoansForContact(this.ID, this.ContactType);
      LoanContactRelationshipList loanRelationships = new LoanContactRelationshipList();
      foreach (ContactLoanPair contactLoanPair in relatedLoansForContact)
        loanRelationships.Add(new LoanContactRelationship(this.Session, contactLoanPair.LoanGuid, contactLoanPair.ContactID, (ContactType) contactLoanPair.ContactType, (LoanContactRelationshipType) contactLoanPair.RoleType, contactLoanPair.BorrowerPair));
      return loanRelationships;
    }

    public override string ToString() => this.FirstName + " " + this.LastName;

    public override int GetHashCode() => this.ID;

    public override bool Equals(object obj)
    {
      if (object.Equals((object) (obj as Contact), (object) null))
        return false;
      Contact contact = (Contact) obj;
      if (contact.Session != this.Session)
        return false;
      if (contact.ID < 0 || this.ID < 0)
        return this == obj;
      return contact.ContactType == this.ContactType && contact.ID == this.ID;
    }

    internal abstract ContactType ContactType { get; }

    protected internal void ensureExists()
    {
      if (this.IsNew)
        throw new InvalidOperationException("This operation is not valid until object is commited");
    }

    protected internal void OnCommitted()
    {
      this.committed((object) this, new PersistentObjectEventArgs(this.ID.ToString()));
    }
  }
}
