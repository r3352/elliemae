// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> objects which are assigned
  /// to a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" />.
  /// </summary>
  public class MilestoneTaskContacts : IMilestoneTaskContacts, IEnumerable
  {
    private MilestoneTask task;
    private MilestoneTaskLog logItem;
    private Dictionary<MilestoneTaskLog.TaskContact, MilestoneTaskContact> cachedContacts = new Dictionary<MilestoneTaskLog.TaskContact, MilestoneTaskContact>();

    internal MilestoneTaskContacts(MilestoneTask task)
    {
      this.task = task;
      this.logItem = (MilestoneTaskLog) task.Unwrap();
    }

    /// <summary>Gets the number of contacts in the collection.</summary>
    public int Count => this.logItem.ContactCount;

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> from the collection.
    /// </summary>
    /// <param name="index">The index of the desired contact.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> at the specified index.</returns>
    public MilestoneTaskContact this[int index]
    {
      get
      {
        MilestoneTaskLog.TaskContact taskContactAt = this.logItem.GetTaskContactAt(index);
        if (taskContactAt == null)
          return (MilestoneTaskContact) null;
        if (!this.cachedContacts.ContainsKey(taskContactAt))
          this.cachedContacts[taskContactAt] = new MilestoneTaskContact(this.task, taskContactAt);
        return this.cachedContacts[taskContactAt];
      }
    }

    /// <summary>Adds a new contact to the collection.</summary>
    /// <param name="contactName">The contact's full name.</param>
    /// <param name="contactEmail">The contact's email address.</param>
    /// <param name="contactPhone">The contact's phone number.</param>
    /// <returns>Returns the new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> object.</returns>
    /// <remarks>You must provide a name and either an email address or phone number or this method
    /// will raise an exception.</remarks>
    public MilestoneTaskContact Add(string contactName, string contactEmail, string contactPhone)
    {
      if ((contactName ?? "") == "")
        throw new ArgumentException("A valid name must be specified", nameof (contactName));
      if ((contactEmail ?? "") == "" && (contactPhone ?? "") == "")
        throw new ArgumentException("A valid email address or phone number must be specified");
      MilestoneTaskLog.TaskContact taskContact = new MilestoneTaskLog.TaskContact(-1, contactName, "", contactPhone, contactEmail, "", "", "", "", "");
      this.logItem.AddContact(taskContact);
      MilestoneTaskContact milestoneTaskContact = new MilestoneTaskContact(this.task, taskContact);
      this.cachedContacts[taskContact] = milestoneTaskContact;
      return milestoneTaskContact;
    }

    /// <summary>
    /// Create a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> and links it to an existing <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" />.
    /// </summary>
    /// <param name="contactToLink">The <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" /> to which to link the Task contact.</param>
    /// <returns>Returns the new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> object.</returns>
    public MilestoneTaskContact Add(BizContact contactToLink)
    {
      if (contactToLink == null)
        throw new ArgumentNullException(nameof (contactToLink));
      MilestoneTaskLog.TaskContact taskContact = new MilestoneTaskLog.TaskContact(contactToLink.ID, contactToLink.FullName, "", "", "", "", "", "", "", "");
      this.logItem.AddContact(taskContact);
      MilestoneTaskContact milestoneTaskContact = new MilestoneTaskContact(this.task, taskContact);
      milestoneTaskContact.LinkContact(contactToLink);
      this.cachedContacts[taskContact] = milestoneTaskContact;
      return milestoneTaskContact;
    }

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> from the collection.
    /// </summary>
    /// <param name="taskContact">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContact" /> to remove.</param>
    public void Remove(MilestoneTaskContact taskContact)
    {
      this.logItem.RemoveContact(taskContact.Unwrap());
    }

    /// <summary>Clears all contacts from the collection.</summary>
    public void Clear() => this.logItem.ClearContactList();

    /// <summary>Provides a enumerator for the collection of contacts.</summary>
    /// <returns>Returns an <see cref="T:System.Collections.IEnumerator" /> implementation for the collection.</returns>
    public IEnumerator GetEnumerator()
    {
      List<MilestoneTaskContact> milestoneTaskContactList = new List<MilestoneTaskContact>();
      for (int index = 0; index < this.Count; ++index)
        milestoneTaskContactList.Add(this[index]);
      return (IEnumerator) milestoneTaskContactList.GetEnumerator();
    }
  }
}
