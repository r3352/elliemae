// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Contacts;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class MilestoneTaskContact : IMilestoneTaskContact
  {
    private MilestoneTask task;
    private MilestoneTaskLog.TaskContact contact;

    internal MilestoneTaskContact(MilestoneTask task, MilestoneTaskLog.TaskContact contact)
    {
      this.task = task;
      this.contact = contact;
    }

    public string Name
    {
      get => this.contact.ContactName;
      set => this.contact.ContactName = value ?? "";
    }

    public BizCategory Category
    {
      get => this.task.Loan.Session.Contacts.BizCategories.GetItemByName(this.contact.ContactRole);
      set
      {
        this.contact.ContactRole = !((EnumItem) value == (EnumItem) null) ? value.Name : throw new ArgumentNullException(nameof (Category), "Category cannot be null");
      }
    }

    public string PhoneNumber
    {
      get => this.contact.ContactPhone;
      set => this.contact.ContactPhone = value ?? "";
    }

    public string Email
    {
      get => this.contact.ContactEmail;
      set => this.contact.ContactEmail = value ?? "";
    }

    public string StreetAddress
    {
      get => this.contact.ContactAddress;
      set => this.contact.ContactAddress = value ?? "";
    }

    public string City
    {
      get => this.contact.ContactCity;
      set => this.contact.ContactCity = value ?? "";
    }

    public string State
    {
      get => this.contact.ContactState;
      set => this.contact.ContactState = value ?? "";
    }

    public string ZipCode
    {
      get => this.contact.ContactZip;
      set => this.contact.ContactZip = value ?? "";
    }

    public BizContact GetLinkedContact()
    {
      return this.contact.ContactID < 0 ? (BizContact) null : (BizContact) this.task.Loan.Session.Contacts.Open(this.contact.ContactID, ContactType.Biz);
    }

    public void LinkContact(BizContact contact)
    {
      this.contact.ContactAddress = contact.BizAddress.Street1;
      this.contact.ContactCity = contact.BizAddress.City;
      this.contact.ContactEmail = contact.PersonalEmail;
      this.contact.ContactID = contact.ID;
      this.contact.ContactName = contact.FullName;
      this.contact.ContactPhone = contact.WorkPhone;
      this.contact.ContactRole = (EnumItem) contact.Category == (EnumItem) null ? "" : contact.Category.Name;
      this.contact.ContactState = contact.BizAddress.State;
      this.contact.ContactZip = contact.BizAddress.City;
    }

    public void UnlinkContact() => this.contact.ContactID = -1;

    internal MilestoneTaskLog.TaskContact Unwrap() => this.contact;
  }
}
