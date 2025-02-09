// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.UserDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class UserDataSource : RemotableDataSource, IUserDataSource
  {
    private UserInfo user;
    private LoanData loan;

    public UserDataSource(UserInfo user, LoanData loan)
      : base(true)
    {
      this.user = user;
      this.loan = loan;
    }

    public UserInfo UserInfo => this.user;

    public override object Clone() => (object) new UserDataSource(this.user, this.loan);

    public string ID => this.user.Userid;

    public string FirstName => this.user.FirstName;

    public string LastName => this.user.LastName;

    public string FullName => this.user.FullName;

    public bool HasPersona(string personaName)
    {
      foreach (Persona userPersona in this.user.UserPersonas)
      {
        if (string.Compare(userPersona.Name, personaName, true) == 0)
          return true;
      }
      return false;
    }

    public bool IsInRole(string roleNameOrAbbr)
    {
      RoleInfo roleInfo = RoleInfo.Null;
      foreach (RoleInfo allRole in this.loan.Settings.AllRoles)
      {
        if (string.Compare(allRole.RoleAbbr, roleNameOrAbbr, true) == 0 || string.Compare(allRole.RoleName, roleNameOrAbbr, true) == 0)
        {
          roleInfo = allRole;
          break;
        }
      }
      return roleInfo != RoleInfo.Null && this.loan.AccessRules.IsUserInEffectiveRole(roleInfo.RoleID);
    }
  }
}
