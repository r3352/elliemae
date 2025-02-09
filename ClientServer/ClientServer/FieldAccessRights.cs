// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FieldAccessRights
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
  public class FieldAccessRights : IXmlSerializable, ISerializable
  {
    public readonly string FieldID;
    public readonly Hashtable AccessRights;

    public FieldAccessRights(string fieldID, PersonaFieldAccessRight[] rights)
    {
      this.FieldID = fieldID;
      this.AccessRights = new Hashtable();
      if (rights == null)
        return;
      for (int index = 0; index < rights.Length; ++index)
        this.AccessRights.Add((object) rights[index].PersonaID, (object) rights[index].AccessRight);
    }

    private FieldAccessRights(string fieldID, Hashtable AccessRights)
    {
      this.FieldID = fieldID;
      this.AccessRights = AccessRights;
    }

    public FieldAccessRights(XmlSerializationInfo info)
    {
      this.FieldID = info.GetString(nameof (FieldID));
      this.AccessRights = new Hashtable();
      foreach (PersonaFieldAccessRight fieldAccessRight in (List<PersonaFieldAccessRight>) info.GetValue(nameof (AccessRights), typeof (XmlList<PersonaFieldAccessRight>)))
        this.AccessRights.Add((object) fieldAccessRight.PersonaID, (object) fieldAccessRight.AccessRight);
    }

    private FieldAccessRights(SerializationInfo info, StreamingContext context)
    {
      this.FieldID = info.GetString(nameof (FieldID));
      this.AccessRights = new Hashtable();
      string[] strArray = info.GetString("Rights").Split('|');
      for (int index = 1; index < strArray.Length; index += 2)
        this.AccessRights.Add((object) int.Parse(strArray[index - 1]), (object) (BizRule.FieldAccessRight) int.Parse(strArray[index]));
    }

    public BizRule.FieldAccessRight GetAccessRight(int personaID)
    {
      object accessRight = this.AccessRights[(object) personaID];
      return accessRight == null ? BizRule.FieldAccessRight.DoesNotApply : (BizRule.FieldAccessRight) accessRight;
    }

    public void Combine(int personaID, BizRule.FieldAccessRight right)
    {
      object accessRight = this.AccessRights[(object) personaID];
      if (accessRight == null)
      {
        this.AccessRights.Add((object) personaID, (object) right);
      }
      else
      {
        BizRule.FieldAccessRight fieldAccessRight = (BizRule.FieldAccessRight) accessRight;
        if (right >= fieldAccessRight)
          return;
        this.AccessRights[(object) personaID] = (object) right;
      }
    }

    public void Combine(Persona persona, BizRule.FieldAccessRight right)
    {
      this.Combine(persona.ID, right);
    }

    public void Combine(PersonaFieldAccessRight[] rights)
    {
      for (int index = 0; index < rights.Length; ++index)
        this.Combine(rights[index].PersonaID, rights[index].AccessRight);
    }

    public void Combine(Hashtable rights)
    {
      foreach (int key in (IEnumerable) rights.Keys)
        this.Combine(key, (BizRule.FieldAccessRight) rights[(object) key]);
    }

    public FieldAccessRights Clone()
    {
      string fieldId = this.FieldID;
      Hashtable AccessRights = new Hashtable();
      foreach (DictionaryEntry accessRight in this.AccessRights)
        AccessRights.Add(accessRight.Key, accessRight.Value);
      return new FieldAccessRights(fieldId, AccessRights);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("FieldID", (object) this.FieldID);
      XmlList<PersonaFieldAccessRight> xmlList = new XmlList<PersonaFieldAccessRight>();
      foreach (int key in (IEnumerable) this.AccessRights.Keys)
      {
        BizRule.FieldAccessRight accessRight = (BizRule.FieldAccessRight) this.AccessRights[(object) key];
        xmlList.Add(new PersonaFieldAccessRight(key, accessRight));
      }
      info.AddValue("AccessRights", (object) xmlList);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("FieldID", (object) this.FieldID);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (int key in (IEnumerable) this.AccessRights.Keys)
      {
        if ((BizRule.FieldAccessRight) this.AccessRights[(object) key] != BizRule.FieldAccessRight.DoesNotApply)
          stringBuilder.Append(key.ToString() + "|" + (object) (int) this.AccessRights[(object) key] + "|");
      }
      info.AddValue("Rights", (object) stringBuilder.ToString());
    }
  }
}
