// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.eFolder;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class DocumentTemplate : IDocumentTemplate
  {
    private DocumentTemplate docTemplate;

    internal DocumentTemplate(DocumentTemplate docTemplate) => this.docTemplate = docTemplate;

    public string ID => this.docTemplate.Guid;

    public string Title => this.docTemplate.Name;

    public bool IncludeInEDisclosurePackage => this.docTemplate.OpeningDocument;

    public bool IncludeInClosingPackage => this.docTemplate.ClosingDocument;

    public int DaysToReceive => this.docTemplate.DaysTillDue;

    public int DaysToExpire => this.docTemplate.DaysTillExpire;

    public string Source => this.docTemplate.Source;

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

    public override bool Equals(object obj)
    {
      return obj is DocumentTemplate documentTemplate && documentTemplate.ID == this.ID;
    }

    public override int GetHashCode() => this.ID.GetHashCode();
  }
}
