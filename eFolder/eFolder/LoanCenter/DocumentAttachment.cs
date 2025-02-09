// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.DocumentAttachment
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine.Log;
using iTextSharp.text.pdf;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class DocumentAttachment : LoanCenterAttachment
  {
    private DocumentLog doc;
    private string signatureType;
    private string[] signatureFields;
    private ForBorrowerType intendedFor;
    private int pageCount;
    private PdfReader pdfReader;

    public DocumentAttachment(string filepath, DocumentLog doc)
      : base(filepath, doc.Title)
    {
      this.doc = doc;
      this.id = doc.Guid;
      this.pageCount = 0;
    }

    public DocumentLog Document => this.doc;

    public string SignatureType
    {
      get => this.signatureType;
      set => this.signatureType = value;
    }

    public string[] SignatureFields
    {
      get => this.signatureFields;
      set => this.signatureFields = value;
    }

    public ForBorrowerType IntendedFor
    {
      get => this.intendedFor;
      set => this.intendedFor = value;
    }

    public int PageCount
    {
      get => this.pageCount;
      set => this.pageCount = value;
    }

    public PdfReader PdfReader
    {
      get => this.pdfReader;
      set => this.pdfReader = value;
    }
  }
}
