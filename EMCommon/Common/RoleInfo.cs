// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.RoleInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [CLSCompliant(true)]
  [Serializable]
  public class RoleInfo : RoleSummaryInfo
  {
    public static readonly RoleInfo FileStarter = new RoleInfo(0, "File Starter", "FS", false, new int[0], new int[0], DateTime.MaxValue);
    public static readonly RoleInfo Others = new RoleInfo(-1, nameof (Others), nameof (Others), false, new int[0], new int[0], DateTime.MaxValue);
    public static readonly RoleInfo All = new RoleInfo(-2, nameof (All), nameof (All), false, new int[0], new int[0], DateTime.MaxValue);
    public static readonly RoleInfo Null = new RoleInfo(-100, nameof (Null), nameof (Null), false, new int[0], new int[0], DateTime.MaxValue);
    public readonly int[] PersonaIDs;
    public readonly int[] UserGroupIDs;

    public RoleInfo(
      int roleID,
      string roleName,
      string abbreviation,
      bool protect,
      int[] personaIDs,
      int[] groupIDs,
      DateTime lastModTime)
      : base(roleID, roleName, abbreviation, protect, lastModTime)
    {
      if (personaIDs == null)
        throw new ArgumentNullException(nameof (personaIDs));
      if (groupIDs == null)
        throw new ArgumentNullException(nameof (groupIDs));
      this.PersonaIDs = personaIDs;
      this.UserGroupIDs = groupIDs;
    }

    public RoleInfo(
      string roleName,
      string abbreviation,
      bool protect,
      int[] personaIDs,
      int[] groupIDs)
      : base(roleName, abbreviation, protect)
    {
      if (personaIDs == null)
        throw new ArgumentNullException(nameof (personaIDs));
      if (groupIDs == null)
        throw new ArgumentNullException(nameof (groupIDs));
      this.PersonaIDs = personaIDs;
      this.UserGroupIDs = groupIDs;
    }

    public RoleInfo(
      int roleID,
      string roleName,
      string abbreviation,
      bool protect,
      int[] personaIDs,
      int[] groupIDs)
      : base(roleID, roleName, abbreviation, protect)
    {
      if (personaIDs == null)
        throw new ArgumentNullException(nameof (personaIDs));
      if (groupIDs == null)
        throw new ArgumentNullException(nameof (groupIDs));
      this.PersonaIDs = personaIDs;
      this.UserGroupIDs = groupIDs;
    }

    public bool ContainsPersona(int personaId)
    {
      for (int index = 0; index < this.PersonaIDs.Length; ++index)
      {
        if (this.PersonaIDs[index] == personaId)
          return true;
      }
      return false;
    }

    public bool ContainsUserGroup(int groupID)
    {
      for (int index = 0; index < this.UserGroupIDs.Length; ++index)
      {
        if (this.UserGroupIDs[index] == groupID)
          return true;
      }
      return false;
    }
  }
}
