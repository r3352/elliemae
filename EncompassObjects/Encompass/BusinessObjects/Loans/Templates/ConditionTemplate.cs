// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Provides a base class for the condition templates that are defined in Encompass.
  /// </summary>
  public abstract class ConditionTemplate : SessionBoundObject, IConditionTemplate
  {
    private EllieMae.EMLite.DataEngine.eFolder.ConditionTemplate template;
    private ConditionDocuments documents;

    internal ConditionTemplate(Session session, EllieMae.EMLite.DataEngine.eFolder.ConditionTemplate template)
      : base(session)
    {
      this.template = template;
    }

    /// <summary>
    /// Returns the unique identifier for the condition template.
    /// </summary>
    public string ID => this.template.Guid;

    /// <summary>Gets the title of the condition.</summary>
    public string Title => this.template.Name;

    /// <summary>Gets the description of the template</summary>
    public string Description => this.template.Description;

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects associated with the condition.
    /// </summary>
    public ConditionDocuments Documents
    {
      get
      {
        lock (this)
        {
          if (this.documents == null)
            this.loadConditionDocuments();
        }
        return this.documents;
      }
    }

    /// <summary>Provides an equality comparer for two templates.</summary>
    /// <returns>Returns <c>true</c> if the IDs of the two templates are the same, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      return obj is ConditionTemplate conditionTemplate && !(conditionTemplate.GetType() != this.GetType()) && conditionTemplate.ID == this.ID;
    }

    /// <summary>Provides a hash code for the object based on the ID.</summary>
    public override int GetHashCode() => this.ID.GetHashCode();

    /// <summary>Creates a ConditionDocuments object for a template.</summary>
    private void loadConditionDocuments()
    {
      List<DocumentTemplate> documentTemplateList = new List<DocumentTemplate>();
      foreach (string documentId in this.template.GetDocumentIDs())
      {
        DocumentTemplate templateById = this.Session.Loans.Templates.Documents.GetTemplateByID(documentId);
        if (templateById != null)
          documentTemplateList.Add(templateById);
      }
      this.documents = new ConditionDocuments(documentTemplateList.ToArray());
    }
  }
}
