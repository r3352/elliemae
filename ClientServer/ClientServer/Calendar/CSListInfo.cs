// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Calendar.CSListInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Calendar
{
  [Serializable]
  public class CSListInfo
  {
    private Hashtable contactList = new Hashtable();

    public CSListInfo(DataRowCollection rows)
    {
      CSMessage.AccessLevel accessLevel = CSMessage.AccessLevel.ReadOnly;
      for (int index = 0; index < rows.Count; ++index)
      {
        object obj1 = rows[index]["UserID"];
        string userID = obj1 != DBNull.Value ? (string) obj1 : throw new Exception("UserID cannot be null");
        object obj2 = rows[index]["OwnerID"];
        string ownerID = obj2 != DBNull.Value ? (string) obj2 : throw new Exception("OwnerID cannot be null");
        object obj3 = rows[index]["OwnerLastName"];
        string ownerLastName = obj3 != DBNull.Value ? (string) obj3 : "";
        object obj4 = rows[index]["OwnerFirstName"];
        string ownerFirstName = obj4 != DBNull.Value ? (string) obj4 : "";
        object obj5 = rows[index]["UserLastName"];
        string userLastName = obj5 != DBNull.Value ? (string) obj5 : "";
        object obj6 = rows[index]["UserFirstName"];
        string userFirstName = obj6 != DBNull.Value ? (string) obj6 : "";
        object obj7 = rows[index]["AccessLevel"];
        if (obj7 != DBNull.Value)
          accessLevel = (CSMessage.AccessLevel) obj7;
        if (!this.contactList.Contains((object) (ownerID + "_" + userID)))
          this.contactList.Add((object) (ownerID + "_" + userID), (object) new CSContactInfo(ownerID, userID, accessLevel, ownerLastName, ownerFirstName, userLastName, userFirstName));
        else
          this.contactList[(object) (ownerID + "_" + userID)] = (object) new CSContactInfo(ownerID, userID, accessLevel, ownerLastName, ownerFirstName, userLastName, userFirstName);
      }
    }

    public int ContactCount => this.contactList.Count;

    public void AddCSContact(string ownerID, string userID, CSMessage.AccessLevel accessLevel)
    {
      if (this.contactList.ContainsKey((object) userID))
        throw new Exception(userID + ": contact already exists");
      this.contactList.Add((object) userID, (object) new CSContactInfo(ownerID, userID, accessLevel, "", "", "", ""));
    }

    public bool ContainsCSContact(string sharerID)
    {
      return this.contactList.ContainsKey((object) sharerID);
    }

    public CSContactInfo GetCSContact(string ownerID)
    {
      return this.contactList.ContainsKey((object) ownerID) ? (CSContactInfo) this.contactList[(object) ownerID] : (CSContactInfo) null;
    }

    public CSContactInfo[] GetCSContactInfoList()
    {
      CSContactInfo[] csContactInfoList = new CSContactInfo[this.contactList.Count];
      this.contactList.Values.CopyTo((Array) csContactInfoList, 0);
      return csContactInfoList;
    }
  }
}
