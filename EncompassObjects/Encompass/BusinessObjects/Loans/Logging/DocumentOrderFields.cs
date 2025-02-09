// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrderFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of field values associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" />.
  /// </summary>
  public class DocumentOrderFields : Fields
  {
    private DocumentOrder order;
    private FieldDescriptors fieldDescriptors;

    internal DocumentOrderFields(DocumentOrder order) => this.order = order;

    /// <summary>
    /// Gets the field descriptors for the fields in the collection.
    /// </summary>
    public FieldDescriptors Descriptors
    {
      get
      {
        this.loadFieldDescriptors();
        return this.fieldDescriptors;
      }
    }

    internal override Field CreateField(string fieldId)
    {
      FieldDescriptor fieldDescriptor = this.order.Loan.Session.Loans.FieldDescriptors[fieldId];
      return fieldDescriptor == null ? (Field) null : (Field) new ReadOnlyField(fieldId, this.order.OrderLog.DocumentFields.GetField(fieldId), fieldDescriptor);
    }

    /// <summary>
    /// Ensures that the list of field descriptors is loaded for this object
    /// </summary>
    private void loadFieldDescriptors()
    {
      if (this.fieldDescriptors != null)
        return;
      this.fieldDescriptors = new FieldDescriptors();
      foreach (string fieldId in this.order.OrderLog.DocumentFields.GetFieldIDs())
      {
        FieldDescriptor fieldDescriptor = this.order.Loan.Session.Loans.FieldDescriptors[fieldId];
        if (fieldDescriptor != null)
          this.fieldDescriptors.Add(fieldDescriptor);
      }
    }
  }
}
