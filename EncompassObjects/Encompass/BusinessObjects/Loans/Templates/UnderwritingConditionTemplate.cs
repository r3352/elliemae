// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a configured condition template which can be used to create a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition" />.
  /// </summary>
  public class UnderwritingConditionTemplate : ConditionTemplate, IUnderwritingConditionTemplate
  {
    private EllieMae.EMLite.DataEngine.eFolder.UnderwritingConditionTemplate uwTemplate;
    private Role forRole;

    internal UnderwritingConditionTemplate(Session session, EllieMae.EMLite.DataEngine.eFolder.UnderwritingConditionTemplate template)
      : base(session, (EllieMae.EMLite.DataEngine.eFolder.ConditionTemplate) template)
    {
      this.uwTemplate = template;
    }

    /// <summary>Indicates if the condition is for internal use.</summary>
    public bool ForInternalUse => this.uwTemplate.IsInternal;

    /// <summary>Indicates if the condition is for external use.</summary>
    public bool ForExternalUse => this.uwTemplate.IsExternal;

    /// <summary>Gets the category for the template.</summary>
    public string Category => this.uwTemplate.Category;

    /// <summary>Gets the category for the template.</summary>
    public string PriorTo => this.uwTemplate.PriorTo;

    /// <summary>
    /// Gets the role that the condition is associated with, if any
    /// </summary>
    public Role ForRole
    {
      get
      {
        lock (this)
        {
          if (this.forRole != null)
            return this.forRole;
          if (this.uwTemplate.ForRoleID <= 0)
            return (Role) null;
          this.forRole = this.Session.Loans.Roles.GetRoleByID(this.uwTemplate.ForRoleID);
          return this.forRole;
        }
      }
    }

    /// <summary>
    /// Indicates if the user in the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> specified as the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate.ForRole" /> is
    /// permitted to clear the condition.
    /// </summary>
    public bool AllowToClear => this.uwTemplate.AllowToClear;
  }
}
