// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PrintDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of documents printed in n
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PrintEvent" />.
  /// </summary>
  public class PrintDocuments : IPrintocuments
  {
    private ArrayList items = new ArrayList();

    internal PrintDocuments(IEnumerable rawItems)
    {
      foreach (string rawItem in rawItems)
        this.items.Add((object) new PrintDocument(rawItem));
    }

    /// <summary>Gets the number of items in the collection.</summary>
    public int Count => this.items.Count;

    /// <summary>
    /// Gets an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PrintDocument" /> from the collection by index.
    /// </summary>
    public PrintDocument this[int index] => (PrintDocument) this.items[index];

    /// <summary>Gets an enumerator for the collection.</summary>
    /// <returns>An object that implements IEnumerator for iterating over the collection.</returns>
    public IEnumerator GetEnumerator() => this.items.GetEnumerator();
  }
}
