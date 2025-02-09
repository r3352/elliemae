// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public int Count => this.disclosedDocuments.Count;

    public DisclosedDocument this[int index] => this.disclosedDocuments[index];

    public IEnumerator GetEnumerator() => (IEnumerator) this.disclosedDocuments.GetEnumerator();
  }
}
