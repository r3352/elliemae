// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ILoanTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("6D646A05-5F09-42c1-BA87-93861EC53F5F")]
  public interface ILoanTemplate
  {
    string Name { get; }

    string Path { get; }

    Loan NewLoan();

    TemplateType TemplateType { get; }

    string Description { get; }

    Template GetComponent(TemplateType tmplType);
  }
}
