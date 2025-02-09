// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate
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
  /// Represents a saved loan template which can be used for the creation of new loans.
  /// </summary>
  /// <example>
  /// The following code creates a new loan using an existing Loan Template.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessEnums;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Templates;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Fetch the example purchase loan template from the server
  ///       LoanTemplate template = (LoanTemplate) session.Loans.Templates.GetTemplate(TemplateType.LoanTemplate,
  ///          @"public:\Example Puchase Loan Template");
  /// 
  ///       // Create a new loan from the template
  ///       Loan loan = template.NewLoan();
  /// 
  ///       // Set the name and folder
  ///       loan.LoanName = "TemplateLoan";
  ///       loan.LoanFolder = "My Pipeline";
  /// 
  ///       // Commit the loan to save it to the server
  ///       loan.Commit();
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class LoanTemplate : Template, ILoanTemplate
  {
    private EllieMae.EMLite.DataEngine.LoanTemplate template;

    internal LoanTemplate(Session session, TemplateEntry fsEntry, EllieMae.EMLite.DataEngine.LoanTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> object based on this template.
    /// </summary>
    /// <returns>A new, uncommitted <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> object.</returns>
    /// <example>
    /// The following code creates a new loan using an existing Loan Template.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Templates;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch the example purchase loan template from the server
    ///       LoanTemplate template = (LoanTemplate) session.Loans.Templates.GetTemplate(TemplateType.LoanTemplate,
    ///          @"public:\Example Puchase Loan Template");
    /// 
    ///       // Create a new loan from the template
    ///       Loan loan = template.NewLoan();
    /// 
    ///       // Set the name and folder
    ///       loan.LoanName = "TemplateLoan";
    ///       loan.LoanFolder = "My Pipeline";
    /// 
    ///       // Commit the loan to save it to the server
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Loan NewLoan()
    {
      return new Loan(this.Session, LoanDataMgr.NewLoan(this.Session.SessionObjects, this.FileSystemEntry, "", ""));
    }

    /// <summary>
    /// Returns the type of template represented by the object.
    /// </summary>
    public override TemplateType TemplateType => TemplateType.LoanTemplate;

    /// <summary>Gets the description of the template.</summary>
    public override string Description => this.template.Description;

    /// <summary>
    /// Gets one of the component <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.Template" /> objects which make up the Loan Template.
    /// </summary>
    /// <param name="tmplType">The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate.TemplateType" /> of the component to retrieve.</param>
    /// <returns>The specified Template, if one exists. Otherwise, <c>null</c> is returned.</returns>
    public Template GetComponent(TemplateType tmplType)
    {
      string path = (string) null;
      switch (tmplType)
      {
        case TemplateType.LoanProgram:
          path = this.template.GetField("PROGRAM");
          break;
        case TemplateType.ClosingCost:
          path = this.template.GetField("COST");
          break;
        case TemplateType.DataTemplate:
          path = this.template.GetField("MISCDATA");
          break;
        case TemplateType.InputFormSet:
          path = this.template.GetField("FORMLIST");
          break;
        case TemplateType.DocumentSet:
          path = this.template.GetField("DOCSET");
          break;
        case TemplateType.TaskSet:
          path = this.template.GetField("TASKSET");
          break;
      }
      return (path ?? "") == "" ? (Template) null : this.Session.Loans.Templates.GetTemplate(tmplType, path);
    }

    /// <summary>
    /// Unwraps the template object to return the internal data type.
    /// </summary>
    /// <returns></returns>
    /// <exclude />
    internal override object Unwrap() => (object) this.template;
  }
}
