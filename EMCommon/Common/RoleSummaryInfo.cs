// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.RoleSummaryInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class RoleSummaryInfo : IComparable
  {
    public readonly int RoleID;
    public readonly string RoleName = "";
    public readonly string RoleAbbr = "";
    public bool Protected;
    public readonly DateTime LastModTime = DateTime.MinValue;

    public RoleSummaryInfo(
      int roleID,
      string roleName,
      string abbreviation,
      bool protect,
      DateTime lastModTime)
    {
      if (roleName != null)
        roleName = roleName.Trim();
      if ((roleName ?? "") == "")
        throw new Exception("Role Name cannot be blank.");
      if ((abbreviation ?? "") == "")
        throw new Exception("Role abbreviation cannot be null.");
      this.RoleID = roleID;
      this.RoleName = roleName;
      this.RoleAbbr = abbreviation ?? "";
      this.Protected = protect;
      this.LastModTime = lastModTime;
    }

    public RoleSummaryInfo(string roleName, string abbreviation, bool protect)
      : this(0, roleName, abbreviation, protect, DateTime.MaxValue)
    {
    }

    public RoleSummaryInfo(int roleID, string roleName, string abbreviation, bool protect)
      : this(roleID, roleName, abbreviation, protect, DateTime.MaxValue)
    {
    }

    public override string ToString() => this.RoleName;

    public int ID => this.RoleID;

    public string Name => this.RoleName;

    public int CompareTo(object obj)
    {
      return !(obj is RoleSummaryInfo roleSummaryInfo) ? 1 : string.Compare(this.RoleName, roleSummaryInfo.RoleName, true);
    }
  }
}
