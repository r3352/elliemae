// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentSet
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class DocumentSet : Template, IDocumentSet
  {
    private DocumentSetTemplate template;

    internal DocumentSet(Session session, TemplateEntry fsEntry, DocumentSetTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    public override TemplateType TemplateType => TemplateType.DocumentSet;

    public override string Description => this.template.Description;

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

    internal override object Unwrap() => (object) this.template;
  }
}
