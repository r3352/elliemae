// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.Investor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a saved Task Set template which can be applied to a loan.
  /// </summary>
  public class Investor : Template, IInvestor
  {
    private InvestorTemplate template;

    internal Investor(Session session, TemplateEntry fsEntry, InvestorTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    /// <summary>
    /// Returns the type of template represented by the object.
    /// </summary>
    public string InvestorName => this.template.CompanyInformation.Name;

    /// <summary>
    /// Returns the type of template represented by the object.
    /// </summary>
    public override TemplateType TemplateType => TemplateType.Investor;

    /// <summary>Gets the description of the template.</summary>
    public override string Description => this.template.Description;

    /// <summary>
    /// Unwraps the template object to return the internal data type.
    /// </summary>
    internal override object Unwrap() => (object) this.template;
  }
}
