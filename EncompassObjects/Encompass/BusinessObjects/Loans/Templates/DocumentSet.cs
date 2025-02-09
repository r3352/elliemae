// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentSet
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a saved Document Set template which can be applied to a loan.
  /// </summary>
  public class DocumentSet : Template, IDocumentSet
  {
    private DocumentSetTemplate template;

    internal DocumentSet(Session session, TemplateEntry fsEntry, DocumentSetTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    /// <summary>
    /// Returns the type of template represented by the object.
    /// </summary>
    public override TemplateType TemplateType => TemplateType.DocumentSet;

    /// <summary>Gets the description of the template.</summary>
    public override string Description => this.template.Description;

    /// <summary>Retrieves all documents for the specified milestone.</summary>
    /// <param name="ms">The <see cref="T:EllieMae.EMLite.Common.Milestone" /></param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.DocumentTemplateList" /> containing the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects which are assigned to the specified milestone.</returns>
    public DocumentTemplateList GetDocumentsForMilestone(Milestone ms)
    {
      DocumentTemplateList documentsForMilestone = new DocumentTemplateList();
      foreach (string docTitle in this.template.GetDocumentsByMilestone(ms.InternalName))
      {
        DocumentTemplate templateByTitle = this.Session.Loans.Templates.Documents.GetTemplateByTitle(docTitle);
        if (templateByTitle != null)
          documentsForMilestone.Add(templateByTitle);
      }
      return documentsForMilestone;
    }

    /// <summary>
    /// Retrieves all documents which are part of the Document Set.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.DocumentTemplateList" /> containing the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects which are part of the document set.</returns>
    public DocumentTemplateList GetAllDocuments()
    {
      DocumentTemplateList allDocuments = new DocumentTemplateList();
      foreach (ArrayList arrayList in (IEnumerable) this.template.DocList.Values)
      {
        foreach (string docTitle in arrayList)
        {
          DocumentTemplate templateByTitle = this.Session.Loans.Templates.Documents.GetTemplateByTitle(docTitle);
          if (templateByTitle != null)
            allDocuments.Add(templateByTitle);
        }
      }
      return allDocuments;
    }

    /// <summary>
    /// Unwraps the template object to return the internal data type.
    /// </summary>
    internal override object Unwrap() => (object) this.template;
  }
}
