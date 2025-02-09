// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanAccessRights
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanAccessRights
  {
    private Hashtable accessRights;
    private Hashtable editableFields;

    public LoanAccessRights(PersonaLoanAccessRight[] rights)
    {
      this.accessRights = new Hashtable();
      this.editableFields = new Hashtable();
      if (rights == null)
        return;
      for (int index = 0; index < rights.Length; ++index)
      {
        this.accessRights.Add((object) rights[index].PersonaID, (object) rights[index].AccessRight);
        this.editableFields.Add((object) rights[index].PersonaID, (object) rights[index].editableFields);
      }
    }

    public int GetAccessRight(int personaID)
    {
      object accessRight = this.accessRights[(object) personaID];
      return accessRight == null ? 16384 : (int) accessRight;
    }

    public string[] GetEditableFields(int personaID)
    {
      return (string[]) this.editableFields[(object) personaID] ?? (string[]) null;
    }

    public int Count => this.accessRights == null ? 0 : this.accessRights.Count;
  }
}
