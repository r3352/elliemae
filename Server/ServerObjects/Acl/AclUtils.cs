// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.AclUtils
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class AclUtils
  {
    public static readonly string[] ImportFolderNames = new string[5]
    {
      "Calyx Point",
      "Fannie Mae 3.x",
      "Contour - TLH 5.3",
      "Genesis",
      "ULAD"
    };

    private AclUtils()
    {
    }

    public static int[] GetPersonaIDs(Persona[] personas)
    {
      int[] personaIds = new int[personas.Length];
      for (int index = 0; index < personas.Length; ++index)
        personaIds[index] = personas[index].ID;
      return personaIds;
    }

    public static int[] GetAclGroupIDs(AclGroup[] groups)
    {
      int[] aclGroupIds = new int[groups.Length];
      for (int index = 0; index < groups.Length; ++index)
        aclGroupIds[index] = groups[index].ID;
      return aclGroupIds;
    }

    public static int[] GetEnumValueArray(Array items)
    {
      int[] enumValueArray = new int[items.Length];
      for (int index = 0; index < enumValueArray.Length; ++index)
        enumValueArray[index] = Convert.ToInt32(items.GetValue(index));
      return enumValueArray;
    }
  }
}
