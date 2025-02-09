// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessengerListInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MessengerListInfo
  {
    private Hashtable groups = new Hashtable();

    public MessengerListInfo(DataRowCollection rows)
    {
      for (int index = 0; index < rows.Count; ++index)
      {
        object obj1 = rows[index]["groupName"];
        string str = obj1 != DBNull.Value ? (string) obj1 : throw new Exception("gourpName cannot be null");
        object obj2 = rows[index]["contactUserid"];
        string contactUserid = obj2 != DBNull.Value ? (string) obj2 : (string) null;
        object obj3 = rows[index]["firstName"];
        string firstName = obj3 != DBNull.Value ? (string) obj3 : (string) null;
        object obj4 = rows[index]["lastName"];
        string lastName = obj4 != DBNull.Value ? (string) obj4 : (string) null;
        if (this.groups.ContainsKey((object) str))
          ((MessengerGroupInfo) this.groups[(object) str]).AddContact(contactUserid, firstName, lastName);
        else if (contactUserid != null)
          this.groups.Add((object) str, (object) new MessengerGroupInfo(str, contactUserid, firstName, lastName));
        else
          this.groups.Add((object) str, (object) new MessengerGroupInfo(str));
      }
    }

    public int GroupCount => this.groups.Count;

    public MessengerGroupInfo GetMessengerGroup(string groupName)
    {
      return (MessengerGroupInfo) this.groups[(object) groupName];
    }

    public MessengerGroupInfo[] GetMessengerGroups()
    {
      MessengerGroupInfo[] messengerGroups = new MessengerGroupInfo[this.groups.Count];
      this.groups.Values.CopyTo((Array) messengerGroups, 0);
      return messengerGroups;
    }
  }
}
