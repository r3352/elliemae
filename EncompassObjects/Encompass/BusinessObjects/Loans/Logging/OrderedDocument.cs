// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.OrderedDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single document that is part of a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" />.
  /// </summary>
  public class OrderedDocument
  {
    private DocumentOrder order;
    private EllieMae.EMLite.DataEngine.Log.OrderedDocument orderedDoc;

    internal OrderedDocument(DocumentOrder order, EllieMae.EMLite.DataEngine.Log.OrderedDocument orderedDoc)
    {
      this.order = order;
      this.orderedDoc = orderedDoc;
    }

    /// <summary>Returns a unique identifier for the document</summary>
    public string ID => this.orderedDoc.Guid;

    /// <summary>Gets the name of the document</summary>
    public string Title => this.orderedDoc.Title;

    /// <summary>
    /// Gets the document type, such as "Closing Document", "Standard Form", etc.
    /// </summary>
    public string DocumentType => this.orderedDoc.DocumentType;

    /// <summary>
    /// Gets the category used to stack this document when a Stacking Order is applied.
    /// </summary>
    /// <remarks>If the document does nto belong to a stacking order category, this
    /// property will return an empty string.</remarks>
    public string StackingCategory => this.orderedDoc.DocEngineCategory;

    /// <summary>Gets the signature type for the document</summary>
    public string SignatureType => this.orderedDoc.SignatureType;

    /// <summary>Gets the Borrower Pair identifier for this document</summary>
    public string BorrowerPairID => this.orderedDoc.PairID;

    /// <summary>Gets the size of the document, in bytes</summary>
    public long Size => this.orderedDoc.Size;

    /// <summary>
    /// Gets the Document Template Guid from which the document derived if the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.OrderedDocument.DocumentType" /> is either "Standard Form" or "Custom Form".
    /// </summary>
    public string DocumentTemplateGuid => this.orderedDoc.DocumentTemplateGuid;

    /// <summary>Retrieves the document from the Encompass Server.</summary>
    /// <returns>If the document has been purged or cannot be located, this method will return
    /// <c>null</c>.</returns>
    public DataObject Retrieve()
    {
      BinaryObject supportingData = this.order.Loan.Unwrap().GetSupportingData(this.orderedDoc.DataKey);
      return supportingData == null ? (DataObject) null : new DataObject(supportingData);
    }
  }
}
