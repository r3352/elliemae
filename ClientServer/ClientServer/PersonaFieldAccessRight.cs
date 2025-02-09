// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PersonaFieldAccessRight
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
  public class PersonaFieldAccessRight : IXmlSerializable
  {
    public readonly int PersonaID;
    public readonly BizRule.FieldAccessRight AccessRight = BizRule.FieldAccessRight.Edit;

    public PersonaFieldAccessRight(int personaID, BizRule.FieldAccessRight accessRight)
    {
      this.PersonaID = personaID;
      this.AccessRight = accessRight;
    }

    public PersonaFieldAccessRight(Persona persona, BizRule.FieldAccessRight accessRight)
      : this(persona.ID, accessRight)
    {
    }

    public PersonaFieldAccessRight(XmlSerializationInfo info)
    {
      this.PersonaID = info.GetInteger(nameof (PersonaID));
      this.AccessRight = info.GetEnum<BizRule.FieldAccessRight>(nameof (AccessRight));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("PersonaID", (object) this.PersonaID);
      info.AddValue("AccessRight", (object) this.AccessRight);
    }
  }
}
