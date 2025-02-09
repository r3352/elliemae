// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessengerGroupInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MessengerGroupInfo : IComparable
  {
    private string groupName = "";
    private Hashtable contacts = new Hashtable();

    public MessengerGroupInfo(string groupName) => this.groupName = groupName;

    public MessengerGroupInfo(
      string groupName,
      string contactUserid,
      string firstName,
      string lastName)
    {
      this.groupName = groupName;
      this.AddContact(contactUserid, firstName, lastName);
    }

    public int CompareTo(object obj)
    {
      return this.groupName.CompareTo(((MessengerGroupInfo) obj).GroupName);
    }

    public string GroupName => this.groupName;

    public void AddContact(string contactUserid, string firstName, string lastName)
    {
      if (this.contacts.ContainsKey((object) contactUserid))
        throw new Exception(contactUserid + ": contact already exists");
      this.contacts.Add((object) contactUserid, (object) new MessengerContactInfo(contactUserid, firstName, lastName));
    }

    public bool ContainsContact(string contactUserid)
    {
      return this.contacts.ContainsKey((object) contactUserid);
    }

    public MessengerContactInfo GetMessengerContact(string contactUserid)
    {
      return (MessengerContactInfo) this.contacts[(object) contactUserid];
    }

    public MessengerContactInfo[] GetMessengerContacts()
    {
      MessengerContactInfo[] messengerContacts = new MessengerContactInfo[this.contacts.Count];
      this.contacts.Values.CopyTo((Array) messengerContacts, 0);
      return messengerContacts;
    }
  }
}
