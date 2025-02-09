// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PrintDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class PrintDocuments : IPrintocuments
  {
    private ArrayList items = new ArrayList();

    internal PrintDocuments(IEnumerable rawItems)
    {
      foreach (string rawItem in rawItems)
        this.items.Add((object) new PrintDocument(rawItem));
    }

    public int Count => this.items.Count;

    public PrintDocument this[int index] => (PrintDocument) this.items[index];

    public IEnumerator GetEnumerator() => this.items.GetEnumerator();
  }
}
