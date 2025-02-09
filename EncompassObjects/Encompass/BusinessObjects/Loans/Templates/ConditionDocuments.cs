// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ConditionDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects associated with a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.ConditionTemplate" />.
  /// </summary>
  public class ConditionDocuments : IConditionDocuments, IEnumerable
  {
    private DocumentTemplate[] docTemplates;

    internal ConditionDocuments(DocumentTemplate[] docTemplates)
    {
      this.docTemplates = docTemplates;
    }

    /// <summary>
    /// Gets the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects in the collection.
    /// </summary>
    public int Count => this.docTemplates.Length;

    /// <summary>
    /// Retrieves a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> from the collection by index.
    /// </summary>
    /// <param name="index">The index of the specified template.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> at the specified index.</returns>
    public DocumentTemplate this[int index] => this.docTemplates[index];

    /// <summary>
    /// Retrieves the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> with the specified ID from the collection.
    /// </summary>
    /// <param name="templateId">The ID of the requested template.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> with the specified ID or, if no template
    /// with the specified ID exists, returns <c>null</c>.</returns>
    public DocumentTemplate GetTemplateByID(string templateId)
    {
      foreach (DocumentTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (DocumentTemplate) null;
    }

    /// <summary>
    /// Determines if a DocumentTemplate is in the collection.
    /// </summary>
    /// <param name="template">The template to check for existence.</param>
    /// <returns>Returns <c>true</c> if the template is in the collection, <c>false</c> otherwise.</returns>
    public bool Contains(DocumentTemplate template)
    {
      if (template == null)
        throw new ArgumentNullException(nameof (template));
      foreach (DocumentTemplate documentTemplate in this)
      {
        if (documentTemplate.Equals((object) template))
          return true;
      }
      return false;
    }

    /// <summary>Provides an enumerator for the collection.</summary>
    public IEnumerator GetEnumerator() => this.docTemplates.GetEnumerator();
  }
}
