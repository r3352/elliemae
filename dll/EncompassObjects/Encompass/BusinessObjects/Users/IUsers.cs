// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IUsers
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("DED67F4B-09A3-4b16-85C4-3DFF859429EA")]
  public interface IUsers
  {
    UserList GetAllUsers();

    UserList GetUsersWithPersona(Persona persona, bool exactMatch);

    User GetUser(string loginId);

    UserGroups Groups { get; }

    Personas Personas { get; }

    ExternalUser GetExternalUserByEmailandSiteID(string loginEmail, string SiteID);

    ExternalUser GetExternalUserByExternalID(string externalUserID);

    ExternalUser GetExternalUserByContactID(string contactID);

    ExternalUser ValidateExternalUserBySiteID(string loginEmail, string password, string siteID);

    ArrayList GetTPOWCAEView(User aeUser, int urlID);
  }
}
