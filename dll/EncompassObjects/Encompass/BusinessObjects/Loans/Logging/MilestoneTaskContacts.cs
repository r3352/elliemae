// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTaskContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public int Count => this.logItem.ContactCount;

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

    public void Remove(MilestoneTaskContact taskContact)
    {
      this.logItem.RemoveContact(taskContact.Unwrap());
    }

    public void Clear() => this.logItem.ClearContactList();

    public IEnumerator GetEnumerator()
    {
      List<MilestoneTaskContact> milestoneTaskContactList = new List<MilestoneTaskContact>();
      for (int index = 0; index < this.Count; ++index)
        milestoneTaskContactList.Add(this[index]);
      return (IEnumerator) milestoneTaskContactList.GetEnumerator();
    }
  }
}
