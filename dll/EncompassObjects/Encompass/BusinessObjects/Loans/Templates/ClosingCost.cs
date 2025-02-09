// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ClosingCost
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class ClosingCost : Template, IClosingCost
  {
    private ClosingCost template;
    private TemplateFields fields;

    internal ClosingCost(Session session, TemplateEntry fsEntry, ClosingCost template)
      : base(session, fsEntry)
    {
      this.template = template;
      this.fields = new TemplateFields((Template) this, (FormDataBase) template);
    }

    public override TemplateType TemplateType => TemplateType.ClosingCost;

    public override string Description => ((FieldDataTemplate) this.template).Description;

    public TemplateFields Fields => this.fields;

    internal override object Unwrap() => (object) this.template;

    public bool IsLocked(string id)
    {
      return this.template != null && ((FormDataBase) this.template).IsLocked(id);
    }
  }
}
