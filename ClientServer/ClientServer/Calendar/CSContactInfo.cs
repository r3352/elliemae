// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Calendar.CSContactInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Calendar
{
  [Serializable]
  public class CSContactInfo : IComparable
  {
    private string userID;
    private string ownerID;
    private string ownerLastName;
    private string ownerFirstName;
    private string userLastName;
    private string userFirstName;
    private CSMessage.AccessLevel accessLevel;

    public CSContactInfo(
      string ownerID,
      string userID,
      CSMessage.AccessLevel accessLevel,
      string ownerLastName,
      string ownerFirstName,
      string userLastName,
      string userFirstName)
    {
      this.ownerID = ownerID;
      this.userID = userID;
      this.ownerFirstName = ownerFirstName;
      this.ownerLastName = ownerLastName;
      this.userFirstName = userFirstName;
      this.userLastName = userLastName;
      this.accessLevel = accessLevel;
    }

    public int CompareTo(object obj) => this.OwnerID.CompareTo(((CSContactInfo) obj).OwnerID);

    public string UserID => this.userID;

    public string OwnerID => this.ownerID;

    public string OwnerLastName => this.ownerLastName;

    public string OwnerFirstName => this.ownerFirstName;

    public string UserLastName => this.userLastName;

    public string UserFirstName => this.userFirstName;

    public CSMessage.AccessLevel AccessLevel => this.accessLevel;
  }
}
