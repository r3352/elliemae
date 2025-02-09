// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single document included in a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure" />.
  /// </summary>
  public class DisclosedDocument : IDisclosedDocument
  {
    private DisclosureTrackingFormItem formItem;

    internal DisclosedDocument(DisclosureTrackingFormItem formItem) => this.formItem = formItem;

    /// <summary>Gets the title of the document.</summary>
    public string Title => this.formItem.FormName;

    /// <summary>Gets the document type of the document.</summary>
    public DisclosedDocumentType DocumentType
    {
      get => (DisclosedDocumentType) this.formItem.OutputFormType;
    }
  }
}
