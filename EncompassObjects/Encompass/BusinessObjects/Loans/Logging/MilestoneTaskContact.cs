// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Contacts;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a contact associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" />.
  /// </summary>
  /// <remarks>
  /// When a contact is associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" />, the contact's name and address information
  /// can be directly entered in the object or the contact can be linked to a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" />,
  /// in which case the information is taken from the BizContact record. However, once the link is established,
  /// you can override the values in this MilestoneTaskContact so it will differ from the values in
  /// the linked Contact.
  /// </remarks>
  public class MilestoneTaskContact : IMilestoneTaskContact
  {
    private MilestoneTask task;
    private MilestoneTaskLog.TaskContact contact;

    internal MilestoneTaskContact(MilestoneTask task, MilestoneTaskLog.TaskContact contact)
    {
      this.task = task;
      this.contact = contact;
    }

    /// <summary>Gets or sets the name of the contact.</summary>
    public string Name
    {
      get => this.contact.ContactName;
      set => this.contact.ContactName = value ?? "";
    }

    /// <summary>Gets or sets the category of the contact.</summary>
    /// <summary>
    /// Gets or sets the <see cref="T:EllieMae.Encompass.BusinessEnums.BizCategory">BizCategory</see> associated with this contact.
    /// </summary>
    public BizCategory Category
    {
      get => this.task.Loan.Session.Contacts.BizCategories.GetItemByName(this.contact.ContactRole);
      set
      {
        this.contact.ContactRole = !((EnumItem) value == (EnumItem) null) ? value.Name : throw new ArgumentNullException(nameof (Category), "Category cannot be null");
      }
    }

    /// <summary>Gets or sets the contact's phone number.</summary>
    public string PhoneNumber
    {
      get => this.contact.ContactPhone;
      set => this.contact.ContactPhone = value ?? "";
    }

    /// <summary>Gets or sets the contact's email address.</summary>
    public string Email
    {
      get => this.contact.ContactEmail;
      set => this.contact.ContactEmail = value ?? "";
    }

    /// <summary>Gets or sets the contact's street address.</summary>
    public string StreetAddress
    {
      get => this.contact.ContactAddress;
      set => this.contact.ContactAddress = value ?? "";
    }

    /// <summary>
    /// Gets or sets the city portion of the contact's address.
    /// </summary>
    public string City
    {
      get => this.contact.ContactCity;
      set => this.contact.ContactCity = value ?? "";
    }

    /// <summary>
    /// Gets or sets the state portion of the contact's address.
    /// </summary>
    public string State
    {
      get => this.contact.ContactState;
      set => this.contact.ContactState = value ?? "";
    }

    /// <summary>
    /// Gets or sets the ZIP Code portion of the contact's address.
    /// </summary>
    public string ZipCode
    {
      get => this.contact.ContactZip;
      set => this.contact.ContactZip = value ?? "";
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" /> which is linked to this Task Contact.
    /// </summary>
    /// <returns>Returns <c>null</c> if there is no linked contact, otherwise it returns
    /// the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" /> object for the linked contact.</returns>
    public BizContact GetLinkedContact()
    {
      return this.contact.ContactID < 0 ? (BizContact) null : (BizContact) this.task.Loan.Session.Contacts.Open(this.contact.ContactID, ContactType.Biz);
    }

    /// <summary>
    /// Links the task contact to the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" />.
    /// </summary>
    /// <param name="contact"></param>
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

    /// <summary>Unlinks the task from the currently linked contact.</summary>
    public void UnlinkContact() => this.contact.ContactID = -1;

    /// <summary>
    /// Returns the underlying MilestoneTaskLog.TaskContact object.
    /// </summary>
    internal MilestoneTaskLog.TaskContact Unwrap() => this.contact;
  }
}
