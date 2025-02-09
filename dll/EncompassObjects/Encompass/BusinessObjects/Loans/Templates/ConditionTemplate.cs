// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.Client;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public abstract class ConditionTemplate : SessionBoundObject, IConditionTemplate
  {
    private ConditionTemplate template;
    private ConditionDocuments documents;

    internal ConditionTemplate(Session session, ConditionTemplate template)
      : base(session)
    {
      this.template = template;
    }

    public string ID => this.template.Guid;

    public string Title => this.template.Name;

    public string Description => this.template.Description;

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

    public override bool Equals(object obj)
    {
      return obj is ConditionTemplate conditionTemplate && !(conditionTemplate.GetType() != this.GetType()) && conditionTemplate.ID == this.ID;
    }

    public override int GetHashCode() => this.ID.GetHashCode();

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
