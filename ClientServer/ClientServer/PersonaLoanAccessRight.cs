// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PersonaLoanAccessRight
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PersonaLoanAccessRight : IXmlSerializable
  {
    public readonly int PersonaID;
    public readonly int AccessRight = 16777215;
    public readonly string[] editableFields;

    public PersonaLoanAccessRight(int personaID, int accessRight, string[] editableFields)
    {
      this.PersonaID = personaID;
      this.AccessRight = accessRight;
      this.editableFields = editableFields;
    }

    public PersonaLoanAccessRight(Persona persona, int accessRight, string[] editableFields)
      : this(persona.ID, accessRight, editableFields)
    {
    }

    public PersonaLoanAccessRight(XmlSerializationInfo info)
    {
      this.PersonaID = info.GetInteger(nameof (PersonaID));
      this.AccessRight = info.GetInteger(nameof (AccessRight));
      XmlStringList xmlStringList = info.GetValue<XmlStringList>("EditableFields", (XmlStringList) null);
      this.editableFields = xmlStringList == null ? (string[]) null : xmlStringList.ToArray();
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("PersonaID", (object) this.PersonaID);
      info.AddValue("AccessRight", (object) this.AccessRight);
      if (this.editableFields == null)
        return;
      info.AddValue("EditableFields", (object) new XmlStringList(this.editableFields));
    }
  }
}
