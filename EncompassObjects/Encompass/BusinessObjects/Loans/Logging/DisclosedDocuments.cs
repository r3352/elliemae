// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedDocument" /> objects in a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure" />.
  /// </summary>
  public class DisclosedDocuments : IDisclosedDocuments, IEnumerable
  {
    private DisclosureBase disclosure;
    private List<DisclosedDocument> disclosedDocuments = new List<DisclosedDocument>();

    internal DisclosedDocuments(DisclosureBase disclosure, DisclosureTrackingFormItem[] forms)
    {
      this.disclosure = disclosure;
      foreach (DisclosureTrackingFormItem form in forms)
        this.disclosedDocuments.Add(new DisclosedDocument(form));
    }

    /// <summary>
    /// Gets the number of DisclosedDocuments in the collection.
    /// </summary>
    public int Count => this.disclosedDocuments.Count;

    /// <summary>Retrieves a comment from the collection by index.</summary>
    /// <param name="index">The index of the comment to retrieve.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedDocument" /> at the specified index.</returns>
    public DisclosedDocument this[int index] => this.disclosedDocuments[index];

    /// <summary>Returns an enumerator for the comments collection.</summary>
    /// <returns>Reurns an IEnumerator implementation for the collection.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) this.disclosedDocuments.GetEnumerator();
  }
}
