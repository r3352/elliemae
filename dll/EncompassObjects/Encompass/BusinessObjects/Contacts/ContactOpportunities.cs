// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunities
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactOpportunities : SessionBoundObject, IContactOpportunities, IEnumerable
  {
    private IContactManager mngr;
    private Contact contact;
    private ContactOpportunityList opps;

    internal ContactOpportunities(Contact contact)
      : base(contact.Session)
    {
      this.mngr = this.Session.Contacts.ContactManager;
      this.contact = contact;
    }

    public ContactOpportunity this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.opps[index];
      }
    }

    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.opps.Count;
      }
    }

    public ContactOpportunity Add()
    {
      this.ensureLoaded();
      if (this.opps.Count >= 1)
        throw new InvalidOperationException("The current Encompass version supports at most one Opportunity per contact.");
      ContactOpportunity contactOpportunity = new ContactOpportunity(this.contact, this.Session.Contacts.ContactManager.GetBorrowerOpportunity(this.Session.Contacts.ContactManager.CreateBorrowerOpportunity(new Opportunity()
      {
        ContactID = this.contact.ID
      })));
      this.opps.Add(contactOpportunity);
      return contactOpportunity;
    }

    public void Remove(ContactOpportunity opp)
    {
      this.ensureLoaded();
      if (!this.opps.Contains(opp))
        throw new ArgumentException("Specified Opportunity not found", nameof (opp));
      this.Session.Contacts.ContactManager.DeleteBorrowerOpportunity(opp.ID);
      this.opps.Remove(opp);
    }

    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.opps.GetEnumerator();
    }

    public void Refresh() => this.opps = (ContactOpportunityList) null;

    private void ensureLoaded()
    {
      if (this.opps != null)
        return;
      this.opps = new ContactOpportunityList();
      Opportunity opportunityByBorrowerId = this.mngr.GetOpportunityByBorrowerId(this.contact.ID);
      if (opportunityByBorrowerId == null)
        return;
      this.opps.Add(new ContactOpportunity(this.contact, opportunityByBorrowerId));
    }
  }
}
