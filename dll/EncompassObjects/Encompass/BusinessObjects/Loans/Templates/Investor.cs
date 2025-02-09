// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.Investor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class Investor : Template, IInvestor
  {
    private InvestorTemplate template;

    internal Investor(Session session, TemplateEntry fsEntry, InvestorTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    public string InvestorName => this.template.CompanyInformation.Name;

    public override TemplateType TemplateType => TemplateType.Investor;

    public override string Description => this.template.Description;

    internal override object Unwrap() => (object) this.template;
  }
}
