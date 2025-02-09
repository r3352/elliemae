// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ITemplateEntryList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("8E8CFBC6-F0EF-4cdf-9F30-8F13DF858B55")]
  public interface ITemplateEntryList
  {
    TemplateEntry this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(TemplateEntry value);

    bool Contains(TemplateEntry value);

    int IndexOf(TemplateEntry value);

    void Insert(int index, TemplateEntry value);

    void Remove(TemplateEntry value);

    TemplateEntry[] ToArray();

    IEnumerator GetEnumerator();
  }
}
