// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactQueryItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactQueryItem : IXmlSerializable
  {
    public string FieldDisplayName;
    public string FieldName;
    public string Condition;
    public string Value1;
    public string Value2;
    public string ValueType;
    public string GroupName;

    public ContactQueryItem()
    {
    }

    public ContactQueryItem(XmlSerializationInfo info)
    {
      this.FieldDisplayName = info.GetString(nameof (FieldDisplayName));
      this.FieldName = info.GetString(nameof (FieldName));
      this.Condition = info.GetString(nameof (Condition));
      this.Value1 = info.GetString(nameof (Value1));
      this.Value2 = info.GetString(nameof (Value2));
      this.ValueType = info.GetString(nameof (ValueType));
      this.GroupName = info.GetString(nameof (GroupName));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("FieldDisplayName", (object) this.FieldDisplayName);
      info.AddValue("FieldName", (object) this.FieldName);
      info.AddValue("Condition", (object) this.Condition);
      info.AddValue("Value1", (object) this.Value1);
      info.AddValue("Value2", (object) this.Value2);
      info.AddValue("ValueType", (object) this.ValueType);
      info.AddValue("GroupName", (object) this.GroupName);
    }
  }
}
