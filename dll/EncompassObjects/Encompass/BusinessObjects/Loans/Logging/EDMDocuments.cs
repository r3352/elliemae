// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class EDMDocuments : IEDMDocuments
  {
    private ArrayList items = new ArrayList();

    internal EDMDocuments(string[] documents)
    {
      foreach (string document in documents)
        this.items.Add((object) new EDMDocument(document));
    }

    public int Count => this.items.Count;

    public EDMDocument this[int index] => (EDMDocument) this.items[index];

    public IEnumerator GetEnumerator() => this.items.GetEnumerator();
  }
}
