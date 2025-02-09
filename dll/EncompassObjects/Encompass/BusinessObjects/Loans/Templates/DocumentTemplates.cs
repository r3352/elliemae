// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class DocumentTemplates : SessionBoundObject, IDocumentTemplates, IEnumerable
  {
    private List<DocumentTemplate> templates;

    internal DocumentTemplates(Session session)
      : base(session)
    {
      this.Refresh();
    }

    public int Count => this.templates.Count;

    public DocumentTemplate this[int index] => this.templates[index];

    public DocumentTemplate GetTemplateByID(string templateId)
    {
      foreach (DocumentTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (DocumentTemplate) null;
    }

    public DocumentTemplate GetTemplateByTitle(string docTitle)
    {
      foreach (DocumentTemplate templateByTitle in this)
      {
        if (string.Compare(templateByTitle.Title, docTitle, true) == 0)
          return templateByTitle;
      }
      return (DocumentTemplate) null;
    }

    public void Refresh()
    {
      lock (this)
      {
        DocumentTrackingSetup documentTrackingSetup = this.Session.SessionObjects.ConfigurationManager.GetDocumentTrackingSetup();
        this.templates = new List<DocumentTemplate>();
        foreach (DocumentTemplate docTemplate in documentTrackingSetup)
          this.templates.Add(new DocumentTemplate(docTemplate));
      }
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this.templates.GetEnumerator();
  }
}
