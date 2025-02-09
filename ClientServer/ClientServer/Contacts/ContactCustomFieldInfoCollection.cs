// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactCustomFieldInfoCollection
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactCustomFieldInfoCollection : IXmlSerializable, IFieldSearchable
  {
    public ContactCustomFieldInfo[] Items = new ContactCustomFieldInfo[0];
    public string Page1Name = "";
    public string Page2Name = "";
    public string Page3Name = "";
    public string Page4Name = "";
    public string Page5Name = "";

    public ContactCustomFieldInfoCollection()
    {
    }

    public ContactCustomFieldInfoCollection(XmlSerializationInfo info)
    {
      this.Items = (ContactCustomFieldInfo[]) info.GetValue(nameof (Items), typeof (ContactCustomFieldInfo[]));
      this.Page1Name = info.GetString(nameof (Page1Name));
      this.Page2Name = info.GetString(nameof (Page2Name));
      this.Page3Name = info.GetString(nameof (Page3Name));
      this.Page4Name = info.GetString(nameof (Page4Name));
      this.Page5Name = info.GetString(nameof (Page5Name));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Items", (object) this.Items);
      info.AddValue("Page1Name", (object) this.Page1Name);
      info.AddValue("Page2Name", (object) this.Page2Name);
      info.AddValue("Page3Name", (object) this.Page3Name);
      info.AddValue("Page4Name", (object) this.Page4Name);
      info.AddValue("Page5Name", (object) this.Page5Name);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      ContactCustomFieldInfo[] contactCustomFieldInfoArray = this.Items;
      for (int index = 0; index < contactCustomFieldInfoArray.Length; ++index)
      {
        ContactCustomFieldInfo fieldInfo = contactCustomFieldInfoArray[index];
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ContactCustomFields, fieldInfo.Label);
        if (!string.IsNullOrEmpty(fieldInfo.LoanFieldId))
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.AffectsValueOf, fieldInfo.LoanFieldId);
        fieldInfo = (ContactCustomFieldInfo) null;
      }
      contactCustomFieldInfoArray = (ContactCustomFieldInfo[]) null;
    }
  }
}
