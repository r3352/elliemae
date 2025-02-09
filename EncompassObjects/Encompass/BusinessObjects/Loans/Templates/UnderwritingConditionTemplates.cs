// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplates
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
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" /> objects defined in the system settings.
  /// </summary>
  public class UnderwritingConditionTemplates : 
    SessionBoundObject,
    IUnderwritingConditionTemplates,
    IEnumerable
  {
    private List<UnderwritingConditionTemplate> templates;

    internal UnderwritingConditionTemplates(Session session)
      : base(session)
    {
      this.Refresh();
    }

    /// <summary>
    /// Gets the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" /> objects in the collection.
    /// </summary>
    public int Count => this.templates.Count;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" /> at a specified index in the collection.
    /// </summary>
    /// <param name="index">The index of the requested template.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" /> at the specified index.</returns>
    public UnderwritingConditionTemplate this[int index] => this.templates[index];

    /// <summary>
    /// Retrieves a template from the collection using its unique ID.
    /// </summary>
    /// <param name="templateId">The ID of the template to be retrieved.</param>
    /// <returns>Returns the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplate" /> or, if no template matches
    /// the specified ID, returns <c>null</c>.</returns>
    public UnderwritingConditionTemplate GetTemplateByID(string templateId)
    {
      foreach (UnderwritingConditionTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (UnderwritingConditionTemplate) null;
    }

    /// <summary>Retrieves a template from the collection by title.</summary>
    /// <param name="title">The name of the template to be retrieved.</param>
    /// <returns>Returns the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> or, if no template matches
    /// the specified title, returns <c>null</c>. The compaison for the purposes of string matching
    /// is case insensitive.</returns>
    public UnderwritingConditionTemplate GetTemplateByTitle(string title)
    {
      foreach (UnderwritingConditionTemplate templateByTitle in this)
      {
        if (string.Compare(templateByTitle.Title, title, true) == 0)
          return templateByTitle;
      }
      return (UnderwritingConditionTemplate) null;
    }

    /// <summary>
    /// Refreshes the UnderwritingCondition Template list from the server.
    /// </summary>
    public void Refresh()
    {
      lock (this)
      {
        ConditionTrackingSetup conditionTrackingSetup = this.Session.SessionObjects.ConfigurationManager.GetConditionTrackingSetup(ConditionType.Underwriting);
        this.templates = new List<UnderwritingConditionTemplate>();
        foreach (EllieMae.EMLite.DataEngine.eFolder.UnderwritingConditionTemplate template in (CollectionBase) conditionTrackingSetup)
          this.templates.Add(new UnderwritingConditionTemplate(this.Session, template));
      }
    }

    /// <summary>Provides an enumerator for the collection.</summary>
    public IEnumerator GetEnumerator() => (IEnumerator) this.templates.GetEnumerator();
  }
}
