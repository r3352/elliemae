// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanProgram
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a saved Loan Program which can be applied to a loan.
  /// </summary>
  public class LoanProgram : Template, ILoanProgram
  {
    private EllieMae.EMLite.DataEngine.LoanProgram template;
    private TemplateFields fields;

    internal LoanProgram(Session session, TemplateEntry fsEntry, EllieMae.EMLite.DataEngine.LoanProgram template)
      : base(session, fsEntry)
    {
      this.template = template;
      this.fields = new TemplateFields((Template) this, (FormDataBase) template);
    }

    /// <summary>
    /// Returns the type of template represented by the object.
    /// </summary>
    public override TemplateType TemplateType => TemplateType.LoanProgram;

    /// <summary>Gets the description of the template.</summary>
    public override string Description => this.template.Description;

    /// <summary>
    /// Gets the collection of fields associated with the template.
    /// </summary>
    public TemplateFields Fields => this.fields;

    /// <summary>
    /// Unwraps the template object to return the internal data type.
    /// </summary>
    internal override object Unwrap() => (object) this.template;
  }
}
