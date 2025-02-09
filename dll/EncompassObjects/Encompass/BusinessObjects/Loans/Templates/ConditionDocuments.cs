// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ConditionDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class ConditionDocuments : IConditionDocuments, IEnumerable
  {
    private DocumentTemplate[] docTemplates;

    internal ConditionDocuments(DocumentTemplate[] docTemplates)
    {
      this.docTemplates = docTemplates;
    }

    public int Count => this.docTemplates.Length;

    public DocumentTemplate this[int index] => this.docTemplates[index];

    public DocumentTemplate GetTemplateByID(string templateId)
    {
      foreach (DocumentTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (DocumentTemplate) null;
    }

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

    public IEnumerator GetEnumerator() => this.docTemplates.GetEnumerator();
  }
}
