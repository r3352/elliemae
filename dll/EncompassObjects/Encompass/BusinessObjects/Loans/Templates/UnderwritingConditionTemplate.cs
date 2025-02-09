// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class UnderwritingConditionTemplate : ConditionTemplate, IUnderwritingConditionTemplate
  {
    private UnderwritingConditionTemplate uwTemplate;
    private Role forRole;

    internal UnderwritingConditionTemplate(Session session, UnderwritingConditionTemplate template)
      : base(session, (ConditionTemplate) template)
    {
      this.uwTemplate = template;
    }

    public bool ForInternalUse => this.uwTemplate.IsInternal;

    public bool ForExternalUse => this.uwTemplate.IsExternal;

    public string Category => this.uwTemplate.Category;

    public string PriorTo => this.uwTemplate.PriorTo;

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

    public bool AllowToClear => this.uwTemplate.AllowToClear;
  }
}
