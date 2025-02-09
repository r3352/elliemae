// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Provides access to the loan templates accessible in the Encompass system
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
  public class Templates : SessionBoundObject, ITemplates
  {
    private DocumentTemplates documentTemplates;
    private UnderwritingConditionTemplates uwConditionTemplates;
    private PostClosingConditionTemplates pcConditionTemplates;
    private TaskTemplates taskTemplates;

    internal Templates(Session session)
      : base(session)
    {
    }

    /// <summary>Retrieves a loan template from the Encompass Server.</summary>
    /// <param name="templateType">The type of template to be retrieved.</param>
    /// <param name="path">The path of the template.</param>
    /// <returns>Returns the specified template or <c>null</c>
    /// if no template with the specified path is found.</returns>
    /// <remarks>
    /// <p>Every template in Encompass has a well-defined <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.Template.Path" /> that
    /// can be used to retrieve the template from the Encompass Server. The path is made up
    /// or three parts:
    /// <list type="bulleted">
    /// <item><term>Domain</term>, which can be either "public" or "personal" and identifies the template
    /// as being a publicly-accessible template available to all users or a private template
    /// available on to it owner.</item>
    /// <item><term>Folders</term>, which is the delimited folder hierarchy in which the template
    /// resides. The elements of the path are separated using the backslash (\) character.</item>
    /// <item><term>Name</term>, which is the name of the template and can include any printable
    /// character except the path delimiter (\).</item>
    /// </list>
    /// A full path for a template would then have the format:
    /// <code>&lt;domain&gt;:\&lt;folder&gt;\&lt;name&gt;</code>
    /// For example, a publicly-accessible template named "MyTemplate" which resides in the
    /// folders "LenderA\ARMs" would be "public:\LenderA\ARMs\MyTemplate".</p>
    /// <p>If a personal template path is specified, the template's owner is assumed to be the
    /// currently logged in user. Thus, you should not pass in a user-qualified path for this
    /// parameter. To retrieve a personal template for a user other than the currently logged in
    /// user, use the overload of GetTemplate that allows a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry" /> to
    /// be specified.</p>
    /// </remarks>
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
    public Template GetTemplate(TemplateType templateType, string path)
    {
      return this.GetTemplate(templateType, TemplateEntry.Parse(path, this.Session.UserID));
    }

    /// <summary>Retrieves a Template from the server.</summary>
    /// <param name="templateType">The type of template to be retrieved.</param>
    /// <param name="entry">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry" /> of the template to be retrieved.</param>
    /// <returns>Returns the specified template, if found, or <c>null</c> if the template is
    /// not found.</returns>
    public Template GetTemplate(TemplateType templateType, TemplateEntry entry)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      FileSystemEntry fileEntry = entry.Unwrap();
      BinaryObject templateSettings = configurationManager.GetTemplateSettings((TemplateSettingsType) templateType, fileEntry);
      if (templateSettings == null)
        return (Template) null;
      switch (templateType)
      {
        case TemplateType.LoanProgram:
          return (Template) new LoanProgram(this.Session, entry, (EllieMae.EMLite.DataEngine.LoanProgram) templateSettings);
        case TemplateType.ClosingCost:
          return (Template) new ClosingCost(this.Session, entry, (EllieMae.EMLite.DataEngine.ClosingCost) templateSettings);
        case TemplateType.DataTemplate:
          return (Template) new DataTemplate(this.Session, entry, (EllieMae.EMLite.DataEngine.DataTemplate) templateSettings);
        case TemplateType.InputFormSet:
          return (Template) new InputFormSet(this.Session, entry, (FormTemplate) templateSettings);
        case TemplateType.DocumentSet:
          return (Template) new DocumentSet(this.Session, entry, (DocumentSetTemplate) templateSettings);
        case TemplateType.LoanTemplate:
          return (Template) new LoanTemplate(this.Session, entry, (EllieMae.EMLite.DataEngine.LoanTemplate) templateSettings);
        case TemplateType.Investor:
          return (Template) new Investor(this.Session, entry, (InvestorTemplate) templateSettings);
        case TemplateType.TaskSet:
          return (Template) new TaskSet(this.Session, entry, (TaskSetTemplate) templateSettings);
        default:
          throw new ArgumentException("Unsupported template type specified");
      }
    }

    /// <summary>
    /// Gets the contents of a folder in the template repository.
    /// </summary>
    /// <param name="templateType">The type of template for which the directory contents are to be retrieved.
    /// </param>
    /// <param name="folderEntry">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry" /> that represents the folder for
    /// which the request is being made.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.TemplateEntryList" /> containing the entries within the
    /// specified folder. Each entry represents either a subfolder of the specified <c>folderEntry</c>
    /// or a template that resides in the folder. The method does not recurse into subfolders of the specified
    /// folder, thus if you wish to retrieve all templates you will need to call this method recursively. Such
    /// recursive calls may perform poorly on systems with large numbers of templates.
    /// </returns>
    /// <example>
    ///       The following code demonstrates how to recursively descend the template
    ///       file system hierarchy to generate a list of all available loan templates.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
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
    ///       // Call the method which will descend the hierarchy, starting with the Public root
    ///       // folder for the templates.
    ///       printTemplateHierarchy(session, TemplateEntry.PublicRoot);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///     // Allows for recursively traversing the template hierarchy
    ///     private static void printTemplateHierarchy(Session session, TemplateEntry parent)
    ///     {
    ///       // Retrieve the contents of the specified parent folder
    ///       TemplateEntryList templateEntries = session.Loans.Templates.GetTemplateFolderContents(TemplateType.LoanTemplate, parent);
    /// 
    ///       // Iterate over each of the TemplateEntry records, each of which represents either
    ///       // a Template or a subfolder of the parent folder.
    ///       foreach (TemplateEntry e in templateEntries)
    ///       {
    ///         printTemplateEntry(e);
    /// 
    ///         // If the entry represents a subfolder, recurse into that folder
    ///         if (e.EntryType == TemplateEntryType.Folder)
    ///           printTemplateHierarchy(session, e);
    ///       }
    ///     }
    /// 
    ///     // Prints the details of a single TemplateEntry object
    ///     private static void printTemplateEntry(TemplateEntry e)
    ///     {
    ///       Console.WriteLine("-> " + e.Name);
    ///       Console.WriteLine("   Type = " + e.EntryType);
    ///       Console.WriteLine("   IsPublic = " + e.IsPublic);
    ///       Console.WriteLine("   LastModified = " + e.LastModified);
    ///       Console.WriteLine("   Owner = " + e.Owner);
    ///       Console.WriteLine("   ParentFolder = " + e.ParentFolder);
    ///       Console.WriteLine("   Path = " + e.Path);
    ///       Console.WriteLine("   RepositoryPath = " + e.DomainPath);
    /// 
    ///       foreach (string name in e.Properties.GetPropertyNames())
    ///         Console.WriteLine("   Properties[\"" + name + "\"] = " + e.Properties[name]);
    /// 
    ///       Console.WriteLine();
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public TemplateEntryList GetTemplateFolderContents(
      TemplateType templateType,
      TemplateEntry folderEntry)
    {
      if (folderEntry == null)
        throw new ArgumentNullException(nameof (folderEntry));
      if (folderEntry.EntryType != TemplateEntryType.Folder)
        throw new ArgumentException("The specified folderEntry represents a template instead of a folder.");
      FileSystemEntry[] templateDirEntries = this.Session.SessionObjects.ConfigurationManager.GetFilteredTemplateDirEntries((TemplateSettingsType) templateType, folderEntry.Unwrap());
      TemplateEntryList templateFolderContents = new TemplateEntryList();
      foreach (FileSystemEntry fsEntry in templateDirEntries)
        templateFolderContents.Add(new TemplateEntry(fsEntry));
      return templateFolderContents;
    }

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects defined in the Encompass system.
    /// </summary>
    /// <example>
    ///       The following code adds a new Document Tracking record to a loan using
    ///       a DocumentTemplate object.
    ///       <code>
    ///         <![CDATA[
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
    ///       // Create a new loan and populate some default fields
    ///       Loan loan = session.Loans.CreateNew();
    ///       loan.LoanFolder = "My Pipeline";
    ///       loan.Fields["36"].Value = "Mike";
    ///       loan.Fields["37"].Value = "Smith";
    ///       loan.Fields["1109"].Value = 200000;
    ///       loan.Fields["4"].Value = 360;
    ///       loan.Fields["3"].Value = 5.75;
    /// 
    ///       // Retrieve the Credit Report document template
    ///       DocumentTemplate tmpl = session.Loans.Templates.Documents.GetTemplateByTitle("Credit Report");
    /// 
    ///       // Create a TrackedDocument in the loan using the template
    ///       loan.Log.TrackedDocuments.AddFromTemplate(tmpl, session.Loans.Milestones.Processing.Name);
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
    ///       </code>
    ///     </example>
    public DocumentTemplates Documents
    {
      get
      {
        lock (this)
        {
          if (this.documentTemplates == null)
            this.documentTemplates = new DocumentTemplates(this.Session);
        }
        return this.documentTemplates;
      }
    }

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate" /> objects defined in the Encompass system.
    /// </summary>
    public TaskTemplates Tasks
    {
      get
      {
        lock (this)
        {
          if (this.taskTemplates == null)
            this.taskTemplates = new TaskTemplates(this.Session);
        }
        return this.taskTemplates;
      }
    }

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" /> objects defined in the Encompass system.
    /// </summary>
    public UnderwritingConditionTemplates UnderwritingConditions
    {
      get
      {
        lock (this)
        {
          if (this.uwConditionTemplates == null)
            this.uwConditionTemplates = new UnderwritingConditionTemplates(this.Session);
        }
        return this.uwConditionTemplates;
      }
    }

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects defined in the Encompass system.
    /// </summary>
    public PostClosingConditionTemplates PostClosingConditions
    {
      get
      {
        lock (this)
        {
          if (this.pcConditionTemplates == null)
            this.pcConditionTemplates = new PostClosingConditionTemplates(this.Session);
        }
        return this.pcConditionTemplates;
      }
    }
  }
}
