// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanActionAccessRights
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanActionAccessRights : IXmlSerializable, ISerializable
  {
    public readonly string LoanActionID;
    public readonly Hashtable AccessRights;

    public LoanActionAccessRights(string loanActionID, PersonaLoanActionAccessRight[] rights)
    {
      this.LoanActionID = loanActionID;
      this.AccessRights = new Hashtable();
      if (rights == null)
        return;
      for (int index = 0; index < rights.Length; ++index)
        this.AccessRights.Add((object) rights[index].PersonaID, (object) rights[index].AccessRight);
    }

    private LoanActionAccessRights(string loanActionID, Hashtable AccessRights)
    {
      this.LoanActionID = loanActionID;
      this.AccessRights = AccessRights;
    }

    public LoanActionAccessRights(XmlSerializationInfo info)
    {
      this.LoanActionID = info.GetString(nameof (LoanActionID));
      this.AccessRights = new Hashtable();
      foreach (PersonaLoanActionAccessRight actionAccessRight in (List<PersonaLoanActionAccessRight>) info.GetValue(nameof (AccessRights), typeof (XmlList<PersonaLoanActionAccessRight>)))
        this.AccessRights.Add((object) actionAccessRight.PersonaID, (object) actionAccessRight.AccessRight);
    }

    private LoanActionAccessRights(SerializationInfo info, StreamingContext context)
    {
      this.LoanActionID = info.GetString(nameof (LoanActionID));
      this.AccessRights = new Hashtable();
      string[] strArray = info.GetString("Rights").Split('|');
      for (int index = 1; index < strArray.Length; index += 2)
        this.AccessRights.Add((object) int.Parse(strArray[index - 1]), (object) (BizRule.LoanActionAccessRight) int.Parse(strArray[index]));
    }

    public BizRule.LoanActionAccessRight GetAccessRight(int personaID)
    {
      object accessRight = this.AccessRights[(object) personaID];
      return accessRight == null ? BizRule.LoanActionAccessRight.Enable : (BizRule.LoanActionAccessRight) accessRight;
    }

    public void Combine(int personaID, BizRule.LoanActionAccessRight right)
    {
      object accessRight = this.AccessRights[(object) personaID];
      if (accessRight == null)
      {
        this.AccessRights.Add((object) personaID, (object) right);
      }
      else
      {
        BizRule.LoanActionAccessRight actionAccessRight = (BizRule.LoanActionAccessRight) accessRight;
        if (right >= actionAccessRight)
          return;
        this.AccessRights[(object) personaID] = (object) right;
      }
    }

    public void Combine(Persona persona, BizRule.LoanActionAccessRight right)
    {
      this.Combine(persona.ID, right);
    }

    public void Combine(PersonaLoanActionAccessRight[] rights)
    {
      for (int index = 0; index < rights.Length; ++index)
        this.Combine(rights[index].PersonaID, rights[index].AccessRight);
    }

    public void Combine(Hashtable rights)
    {
      foreach (int key in (IEnumerable) rights.Keys)
        this.Combine(key, (BizRule.LoanActionAccessRight) rights[(object) key]);
    }

    public LoanActionAccessRights Clone()
    {
      string loanActionId = this.LoanActionID;
      Hashtable AccessRights = new Hashtable();
      foreach (DictionaryEntry accessRight in this.AccessRights)
        AccessRights.Add(accessRight.Key, accessRight.Value);
      return new LoanActionAccessRights(loanActionId, AccessRights);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("LoanActionID", (object) this.LoanActionID);
      XmlList<PersonaLoanActionAccessRight> xmlList = new XmlList<PersonaLoanActionAccessRight>();
      foreach (PersonaLoanActionAccessRight actionAccessRight in (IEnumerable) this.AccessRights.Values)
        xmlList.Add(actionAccessRight);
      info.AddValue("AccessRights", (object) xmlList);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("LoanActionID", (object) this.LoanActionID);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (int key in (IEnumerable) this.AccessRights.Keys)
        stringBuilder.Append(key.ToString() + "|" + (object) (int) this.AccessRights[(object) key] + "|");
      info.AddValue("Rights", (object) stringBuilder.ToString());
    }
  }
}
