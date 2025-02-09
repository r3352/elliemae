// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.AppointmentContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  /// <summary>
  /// Provides access to the set of contacts related to an appointment.
  /// </summary>
  /// <example>
  /// The following code display all of the days appointments along with the related
  /// contact information.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Calendar;
  /// using EllieMae.Encompass.BusinessObjects.Contacts;
  /// using EllieMae.Encompass.Query;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Get the current days appointments
  ///       AppointmentList appts = session.Calendar.GetAppointmentsForDate(DateTime.Today);
  /// 
  ///       // Print each one out
  ///       foreach (Appointment appt in appts)
  ///       {
  ///          // Write the start and end times and the appointment's subject
  ///          Console.WriteLine(appt.StartTime + " - " + appt.EndTime + ": " + appt.Subject);
  /// 
  ///          // Now display all the related contacts with their home phone numbers
  ///          for (int i = 0; i < appt.Contacts.Count; i++)
  ///          {
  ///             Contact contact = appt.Contacts[i];
  ///             Console.WriteLine("   " + contact.FirstName + " " + contact.LastName +
  ///                " " + contact.HomePhone);
  ///          }
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class AppointmentContacts : SessionBoundObject, IAppointmentContacts, IEnumerable
  {
    private Appointment appt;
    private ContactList contacts;

    internal AppointmentContacts(Appointment appt)
      : base(appt.Session)
    {
      this.appt = appt;
      if (!appt.IsNew())
        return;
      this.contacts = new ContactList();
    }

    /// <summary>
    /// Gets the number of contacts associated with this appointment.
    /// </summary>
    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.contacts.Count;
      }
    }

    /// <summary>Gets a contact from the collection by index.</summary>
    /// <example>
    /// The following code display all of the days appointments along with the related
    /// contact information.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Get the current days appointments
    ///       AppointmentList appts = session.Calendar.GetAppointmentsForDate(DateTime.Today);
    /// 
    ///       // Print each one out
    ///       foreach (Appointment appt in appts)
    ///       {
    ///          // Write the start and end times and the appointment's subject
    ///          Console.WriteLine(appt.StartTime + " - " + appt.EndTime + ": " + appt.Subject);
    /// 
    ///          // Now display all the related contacts with their home phone numbers
    ///          for (int i = 0; i < appt.Contacts.Count; i++)
    ///          {
    ///             Contact contact = appt.Contacts[i];
    ///             Console.WriteLine("   " + contact.FirstName + " " + contact.LastName +
    ///                " " + contact.HomePhone);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Contact this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.contacts[index];
      }
    }

    /// <summary>Adds a contact to the collection.</summary>
    /// <param name="contact">The contact to be added.</param>
    /// <example>
    /// The following code creates a new appointment to meet with a contact.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create a new appointment in the calendar tha will start 3 hours from now
    ///       Appointment appt = session.Calendar.CreateAppointment(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4));
    /// 
    ///       // Set the appointments properties
    ///       appt.Subject = "Meet with Margaret Taylor re: rate lock";
    ///       appt.Location = "220 W. Grand Ave.";
    /// 
    ///       // Set a reminder for an hour before the appointment
    ///       appt.ReminderEnabled = true;
    ///       appt.ReminderInterval = 60;
    /// 
    ///       // Fetch the contact to attach to this appointment
    ///       StringFieldCriterion fnCri = new StringFieldCriterion();
    ///       fnCri.FieldName = "Contact.FirstName";
    ///       fnCri.Value = "Margaret";
    /// 
    ///       StringFieldCriterion lnCri = new StringFieldCriterion();
    ///       lnCri.FieldName = "Contact.LastName";
    ///       lnCri.Value = "Taylor";
    /// 
    ///       ContactList contacts = session.Contacts.Query(fnCri.And(lnCri), ContactLoanMatchType.None,
    ///          ContactType.Borrower);
    /// 
    ///       // Add the contacts to the appointment
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          appt.Contacts.Add(contacts[i]);
    /// 
    ///       // Save the changes to the appointment
    ///       appt.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Add(Contact contact)
    {
      this.ensureLoaded();
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      if (!contact.Session.Equals((object) this.Session))
        throw new ArgumentException("Specific contact must come from same Session");
      for (int index = 0; index < this.contacts.Count; ++index)
      {
        if (contact.Equals((object) this.contacts[index]))
          throw new ArgumentException("Contact already exists in collection");
      }
      this.contacts.Add(contact);
    }

    /// <summary>Removes a contact from the appointment.</summary>
    /// <param name="contact">The contact to be removed.</param>
    /// <example>
    /// The following code clears the associated contact information for an
    /// appointment.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Loop over the appointments in a calendar, looking for a specific appointment
    ///       AppointmentList appts = session.Calendar.GetAllAppointments();
    /// 
    ///       foreach (Appointment appt in appts)
    ///          if (appt.Subject == "Meeting with Joan")
    ///          {
    ///             for (int i = appt.Contacts.Count - 1; i >=0; i--)
    ///                appt.Contacts.Remove(appt.Contacts[i]);
    /// 
    ///             // Save the changes to the appointment
    ///             appt.Commit();
    ///          }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Remove(Contact contact)
    {
      this.ensureLoaded();
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      this.contacts.Remove(contact);
    }

    /// <summary>
    /// Clears all of the associated contacts for the appointment.
    /// </summary>
    public void Clear() => this.contacts = new ContactList();

    /// <summary>Returns an enumerator for the collection.</summary>
    /// <returns>An IEnumerator object for iterating over the set of contacts.</returns>
    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.contacts.GetEnumerator();
    }

    internal ContactInfo[] GetContactInfoList()
    {
      if (this.contacts == null)
        return (ContactInfo[]) null;
      ContactInfo[] contactInfoList = new ContactInfo[this.contacts.Count];
      for (int index = 0; index < this.contacts.Count; ++index)
      {
        Contact contact = this.contacts[index];
        contactInfoList[index] = new ContactInfo(contact.ID.ToString(), (CategoryType) contact.ContactType);
      }
      return contactInfoList;
    }

    internal void Refresh() => this.contacts = (ContactList) null;

    private void ensureLoaded()
    {
      if (this.contacts != null)
        return;
      this.contacts = new ContactList();
      ContactInfo[] contactInfo = this.Session.Calendar.CalendarManager.GetContactInfo(this.appt.ID.ToString());
      for (int index = 0; index < contactInfo.Length; ++index)
      {
        Contact contact = this.Session.Contacts.Open(int.Parse(contactInfo[index].ContactID), (ContactType) contactInfo[index].ContactType);
        if (contact != null)
          this.contacts.Add(contact);
      }
    }
  }
}
