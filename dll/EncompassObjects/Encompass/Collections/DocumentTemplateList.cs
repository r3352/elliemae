// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.DocumentTemplateList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class DocumentTemplateList : ListBase, IDocumentTemplateList
  {
    public DocumentTemplateList()
      : base(typeof (DocumentTemplate))
    {
    }

    public DocumentTemplateList(IList source)
      : base(typeof (DocumentTemplate), source)
    {
    }

    public DocumentTemplate this[int index]
    {
      get => (DocumentTemplate) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(DocumentTemplate value) => this.List.Add((object) value);

    public bool Contains(DocumentTemplate value) => this.List.Contains((object) value);

    public int IndexOf(DocumentTemplate value) => this.List.IndexOf((object) value);

    public void Insert(int index, DocumentTemplate value)
    {
      this.List.Insert(index, (object) value);
    }

    public void Remove(DocumentTemplate value) => this.List.Remove((object) value);

    public DocumentTemplate[] ToArray()
    {
      DocumentTemplate[] array = new DocumentTemplate[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
