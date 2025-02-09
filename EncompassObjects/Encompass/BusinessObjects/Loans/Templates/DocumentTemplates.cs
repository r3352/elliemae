// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects defined in the system settings.
  /// </summary>
  public class DocumentTemplates : SessionBoundObject, IDocumentTemplates, IEnumerable
  {
    private List<DocumentTemplate> templates;

    internal DocumentTemplates(Session session)
      : base(session)
    {
      this.Refresh();
    }

    /// <summary>
    /// Gets the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects in the collection.
    /// </summary>
    public int Count => this.templates.Count;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> at a specified index in the collection.
    /// </summary>
    /// <param name="index">The index of the requested template.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> at the specified index.</returns>
    public DocumentTemplate this[int index] => this.templates[index];

    /// <summary>
    /// Retrieves a template from the collection using its unique ID.
    /// </summary>
    /// <param name="templateId">The ID of the template to be retrieved.</param>
    /// <returns>Returns the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> or, if no template matches
    /// the specified ID, returns <c>null</c>.</returns>
    public DocumentTemplate GetTemplateByID(string templateId)
    {
      foreach (DocumentTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (DocumentTemplate) null;
    }

    /// <summary>Retrieves a template from the collection by title.</summary>
    /// <param name="docTitle">The title of the template to be retrieved.</param>
    /// <returns>Returns the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> or, if no template matches
    /// the specified title, returns <c>null</c>. The compaison for the purposes of string matching
    /// is case insensitive.</returns>
    public DocumentTemplate GetTemplateByTitle(string docTitle)
    {
      foreach (DocumentTemplate templateByTitle in this)
      {
        if (string.Compare(templateByTitle.Title, docTitle, true) == 0)
          return templateByTitle;
      }
      return (DocumentTemplate) null;
    }

    /// <summary>Refreshes the Document Template list from the server.</summary>
    public void Refresh()
    {
      lock (this)
      {
        DocumentTrackingSetup documentTrackingSetup = this.Session.SessionObjects.ConfigurationManager.GetDocumentTrackingSetup();
        this.templates = new List<DocumentTemplate>();
        foreach (EllieMae.EMLite.DataEngine.eFolder.DocumentTemplate docTemplate in documentTrackingSetup)
          this.templates.Add(new DocumentTemplate(docTemplate));
      }
    }

    /// <summary>Provides an enumerator for the collection.</summary>
    public IEnumerator GetEnumerator() => (IEnumerator) this.templates.GetEnumerator();
  }
}
