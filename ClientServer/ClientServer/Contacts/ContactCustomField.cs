// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactCustomField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactCustomField
  {
    private int fieldId = -1;
    private int contactId;
    private string ownerId;
    private string fieldValue;

    public ContactCustomField(int contactId, int fieldId, string ownerId, string fieldValue)
    {
      this.contactId = contactId;
      this.fieldId = fieldId;
      this.ownerId = ownerId;
      this.fieldValue = fieldValue;
    }

    public ContactCustomField()
      : this(-1, -1, "", "")
    {
    }

    public int ContactID
    {
      get => this.contactId;
      set => this.contactId = value;
    }

    public int FieldID
    {
      get => this.fieldId;
      set => this.fieldId = value;
    }

    public string OwnerID
    {
      get => this.ownerId;
      set => this.ownerId = value;
    }

    public string FieldValue
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }
  }
}
