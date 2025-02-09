// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class LoanTemplate : Template, ILoanTemplate
  {
    private LoanTemplate template;

    internal LoanTemplate(Session session, TemplateEntry fsEntry, LoanTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    public Loan NewLoan()
    {
      return new Loan(this.Session, LoanDataMgr.NewLoan(this.Session.SessionObjects, this.FileSystemEntry, "", ""));
    }

    public override TemplateType TemplateType => TemplateType.LoanTemplate;

    public override string Description => ((FieldDataTemplate) this.template).Description;

    public Template GetComponent(TemplateType tmplType)
    {
      string path = (string) null;
      switch (tmplType)
      {
        case TemplateType.LoanProgram:
          path = ((FormDataBase) this.template).GetField("PROGRAM");
          break;
        case TemplateType.ClosingCost:
          path = ((FormDataBase) this.template).GetField("COST");
          break;
        case TemplateType.DataTemplate:
          path = ((FormDataBase) this.template).GetField("MISCDATA");
          break;
        case TemplateType.InputFormSet:
          path = ((FormDataBase) this.template).GetField("FORMLIST");
          break;
        case TemplateType.DocumentSet:
          path = ((FormDataBase) this.template).GetField("DOCSET");
          break;
        case TemplateType.TaskSet:
          path = ((FormDataBase) this.template).GetField("TASKSET");
          break;
      }
      return (path ?? "") == "" ? (Template) null : this.Session.Loans.Templates.GetTemplate(tmplType, path);
    }

    internal override object Unwrap() => (object) this.template;
  }
}
