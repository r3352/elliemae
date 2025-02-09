// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Printing.ImagePrintOptions
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

#nullable disable
namespace EllieMae.EMLite.eFolder.Printing
{
  public class ImagePrintOptions
  {
    private string[] imageFiles;
    private string currentPrinter;
    private short copies;
    private bool allPages;
    private int currentPage;
    private string pageRange = string.Empty;
    private string paperSize = string.Empty;
    private bool printToFit;
    private bool actualSize;
    private bool shrinkOversized;
    private bool duplexVertical;
    private bool duplexHorizontal;

    public string[] ImageFiles
    {
      get => this.imageFiles;
      set => this.imageFiles = value;
    }

    public string Printer
    {
      get => this.currentPrinter;
      set => this.currentPrinter = value;
    }

    public short Copies
    {
      get => this.copies;
      set => this.copies = value;
    }

    public bool AllPages
    {
      get => this.allPages;
      set => this.allPages = value;
    }

    public int CurrentPage
    {
      get => this.currentPage;
      set => this.currentPage = value;
    }

    public string PageRange
    {
      get => this.pageRange;
      set => this.pageRange = value;
    }

    public string PaperSize
    {
      get => this.paperSize;
      set => this.paperSize = value;
    }

    public bool PrintToFit
    {
      get => this.printToFit;
      set => this.printToFit = value;
    }

    public bool ActualSize
    {
      get => this.actualSize;
      set => this.actualSize = value;
    }

    public bool ShrinkOversized
    {
      get => this.shrinkOversized;
      set => this.shrinkOversized = value;
    }

    public bool DuplexVertical
    {
      get => this.duplexVertical;
      set => this.duplexVertical = value;
    }

    public bool DuplexHorizontal
    {
      get => this.duplexHorizontal;
      set => this.duplexHorizontal = value;
    }
  }
}
