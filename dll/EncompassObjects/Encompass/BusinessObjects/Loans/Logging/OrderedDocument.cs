// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.OrderedDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class OrderedDocument
  {
    private DocumentOrder order;
    private OrderedDocument orderedDoc;

    internal OrderedDocument(DocumentOrder order, OrderedDocument orderedDoc)
    {
      this.order = order;
      this.orderedDoc = orderedDoc;
    }

    public string ID => this.orderedDoc.Guid;

    public string Title => this.orderedDoc.Title;

    public string DocumentType => this.orderedDoc.DocumentType;

    public string StackingCategory => this.orderedDoc.DocEngineCategory;

    public string SignatureType => this.orderedDoc.SignatureType;

    public string BorrowerPairID => this.orderedDoc.PairID;

    public long Size => this.orderedDoc.Size;

    public string DocumentTemplateGuid => this.orderedDoc.DocumentTemplateGuid;

    public DataObject Retrieve()
    {
      BinaryObject supportingData = this.order.Loan.Unwrap().GetSupportingData(this.orderedDoc.DataKey);
      return supportingData == null ? (DataObject) null : new DataObject(supportingData);
    }
  }
}
