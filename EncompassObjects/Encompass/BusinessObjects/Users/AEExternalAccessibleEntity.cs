// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.AEExternalAccessibleEntity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Represents a collection of objects accessbile by an AE
  /// </summary>
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

    /// <summary>Returns a list of accessible company TPO ID</summary>
    public List<string> AccessibleCompanies => this.accessibleCompanies;

    /// <summary>
    /// Returns a dictionary object containing a list of accessible branches grouped by company.  The key of the dictionary is the TPO ID of the companies.  The value
    /// of a corresponding key is the accessible branches within that company.
    /// </summary>
    public Dictionary<string, string[]> AccessibleBranchies => this.accessibleBranchies;

    /// <summary>Gets a list of accessible contacts' contact ID</summary>
    public List<string> AccessibleContacts => this.accessibleContacts;
  }
}
