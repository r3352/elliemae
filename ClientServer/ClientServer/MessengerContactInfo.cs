// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessengerContactInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MessengerContactInfo : IComparable
  {
    private string contactUserid = "";
    private string firstName = "";
    private string lastName = "";

    public MessengerContactInfo(string contactUserid, string firstName, string lastName)
    {
      this.contactUserid = contactUserid;
      this.firstName = firstName == null ? "" : firstName.Trim();
      this.lastName = lastName == null ? "" : lastName.Trim();
    }

    public int CompareTo(object obj)
    {
      return this.DisplayName.CompareTo(((MessengerContactInfo) obj).DisplayName);
    }

    public string ContactUserid => this.contactUserid;

    public string FirstName => this.firstName;

    public string LastName => this.lastName;

    public string DisplayName
    {
      get
      {
        return this.firstName == "" && this.lastName == "" ? this.contactUserid : (this.firstName + " " + this.lastName).Trim();
      }
    }
  }
}
