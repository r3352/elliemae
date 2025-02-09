// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.AEExternalAccessibleEntity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class AEExternalAccessibleEntity : IAEExternalAccessibleEntity
  {
    private List<string> accessibleCompanies;
    private Dictionary<string, string[]> accessibleBranchies;
    private List<string> accessibleContacts;

    internal AEExternalAccessibleEntity(
      List<string> accessibleCompanies,
      Dictionary<string, string[]> accessibleBranchies,
      List<string> accessibleContacts)
    {
      this.accessibleCompanies = accessibleCompanies;
      this.accessibleBranchies = accessibleBranchies;
      this.accessibleContacts = accessibleContacts;
    }

    public List<string> AccessibleCompanies => this.accessibleCompanies;

    public Dictionary<string, string[]> AccessibleBranchies => this.accessibleBranchies;

    public List<string> AccessibleContacts => this.accessibleContacts;
  }
}
