// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PersonaLoanActionAccessRight
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
  public class PersonaLoanActionAccessRight : IXmlSerializable
  {
    public readonly int PersonaID;
    public readonly BizRule.LoanActionAccessRight AccessRight = BizRule.LoanActionAccessRight.Enable;

    public PersonaLoanActionAccessRight(int personaID, BizRule.LoanActionAccessRight accessRight)
    {
      this.PersonaID = personaID;
      this.AccessRight = accessRight;
    }

    public PersonaLoanActionAccessRight(Persona persona, BizRule.LoanActionAccessRight accessRight)
      : this(persona.ID, accessRight)
    {
    }

    public PersonaLoanActionAccessRight(XmlSerializationInfo info)
    {
      this.PersonaID = info.GetInteger(nameof (PersonaID));
      this.AccessRight = info.GetEnum<BizRule.LoanActionAccessRight>(nameof (AccessRight));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("PersonaID", (object) this.PersonaID);
      info.AddValue("AccessRight", (object) this.AccessRight);
    }
  }
}
