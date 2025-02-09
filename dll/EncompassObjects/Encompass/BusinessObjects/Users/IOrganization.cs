// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IOrganization
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("B61B4EF7-F381-4bdb-97B6-A8CE872DE657")]
  public interface IOrganization
  {
    int ID { get; }

    string Name { get; set; }

    string OrgCode { get; set; }

    string CompanyName { get; set; }

    Address CompanyAddress { get; }

    string CompanyPhone { get; set; }

    string CompanyFax { get; set; }

    string Description { get; set; }

    void Commit();

    void Delete();

    void Refresh();

    bool IsNew { get; }

    bool IsTopMostOrganization { get; }

    Organization GetParentOrganization();

    OrganizationList GetSuborganizations();

    Organization CreateSuborganization(string name);

    UserList GetUsers();

    User CreateUser(string loginId, string password, PersonaList userPersona);

    string ToString();

    UserGroupList GetUserGroups();

    bool SSO_UseParentInfo { get; set; }

    bool SSO_LoginAccess { get; set; }
  }
}
