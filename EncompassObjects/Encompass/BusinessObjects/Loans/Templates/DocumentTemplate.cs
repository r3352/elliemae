// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a configured document template which can be used to create a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" />.
  /// </summary>
  public class DocumentTemplate : IDocumentTemplate
  {
    private EllieMae.EMLite.DataEngine.eFolder.DocumentTemplate docTemplate;

    internal DocumentTemplate(EllieMae.EMLite.DataEngine.eFolder.DocumentTemplate docTemplate)
    {
      this.docTemplate = docTemplate;
    }

    /// <summary>
    /// Returns the unique identifier for the document template.
    /// </summary>
    public string ID => this.docTemplate.Guid;

    /// <summary>Gets the title of the document.</summary>
    public string Title => this.docTemplate.Name;

    /// <summary>
    /// Indicates if the document is to be included in the eDisclosure package.
    /// </summary>
    public bool IncludeInEDisclosurePackage => this.docTemplate.OpeningDocument;

    /// <summary>
    /// Indicates if the document is to be included in the closing package.
    /// </summary>
    public bool IncludeInClosingPackage => this.docTemplate.ClosingDocument;

    /// <summary>
    /// Gets the number of days from the date ordered until the document should be received.
    /// </summary>
    public int DaysToReceive => this.docTemplate.DaysTillDue;

    /// <summary>
    /// Gets the number of days from the receipt date until the document expires.
    /// </summary>
    public int DaysToExpire => this.docTemplate.DaysTillExpire;

    /// <summary>
    /// Gets the source of the document template if the document comes from a standard form.
    /// </summary>
    public string Source => this.docTemplate.Source;

    /// <summary>
    /// Gets the type of document represented by the template.
    /// </summary>
    public DocumentTemplateType Type
    {
      get
      {
        switch (this.docTemplate.SourceType)
        {
          case "Standard Form":
            return DocumentTemplateType.StandardForm;
          case "Custom Form":
            return DocumentTemplateType.CustomForm;
          case "Borrower Specific Custom Form":
            return DocumentTemplateType.CustomForm;
          case "Needed":
            return DocumentTemplateType.Needed;
          default:
            return !(this.docTemplate.SourceType == "") ? DocumentTemplateType.Other : DocumentTemplateType.None;
        }
      }
    }

    /// <summary>Provides an equality comparer for two templates.</summary>
    /// <returns>Returns <c>true</c> if the IDs of the two templates are the same, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      return obj is DocumentTemplate documentTemplate && documentTemplate.ID == this.ID;
    }

    /// <summary>Provides a hash code for the object based on the ID.</summary>
    public override int GetHashCode() => this.ID.GetHashCode();
  }
}
