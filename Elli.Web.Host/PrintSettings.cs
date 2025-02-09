// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.PrintSettings
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Elli.Web.Host
{
  public class PrintSettings
  {
    public string PrinterName { get; set; }

    public int Copies { get; set; }

    public bool Landscape { get; set; }

    public bool PrintBackgrounds { get; set; }

    public bool PrintSelectionOnly { get; set; }

    public bool DisplayHeaderFooter { get; set; }

    public PrintPaperSize PaperSize { get; set; }

    public List<PrintPageRange> PageRanges { get; set; }

    public bool PrintToPDF { get; set; }

    public string PdfFileName { get; set; }

    public string PdfDirLocation { get; set; }

    public string PdfFilePath
    {
      get
      {
        return !string.IsNullOrEmpty(this.PdfDirLocation) && !string.IsNullOrEmpty(this.PdfFileName) ? Path.Combine(this.PdfDirLocation, this.PdfFileName) : (string) null;
      }
    }

    public IList<string> SettingValidationErrors { get; private set; }

    public bool IsSettingsValid()
    {
      this.SettingValidationErrors = (IList<string>) new List<string>();
      bool flag = true;
      if (this.PrintToPDF)
      {
        if (string.IsNullOrWhiteSpace(this.PdfFilePath))
        {
          flag = false;
          this.SettingValidationErrors.Add("Pdf file path is mandatory when PrintToPdf is true.");
        }
        else if (!Directory.Exists(this.PdfDirLocation))
        {
          flag = false;
          this.SettingValidationErrors.Add("Directory given to save pdf is invalid or does not exists");
        }
        else if (string.IsNullOrWhiteSpace(this.PdfFileName))
        {
          flag = false;
          this.SettingValidationErrors.Add("Filename to save pdf is mandatory");
        }
        else if (this.PdfFilePath.IndexOfAny(Path.GetInvalidPathChars()) > -1)
        {
          flag = false;
          this.SettingValidationErrors.Add("Pdf file path has invalid characters.");
        }
      }
      return flag;
    }
  }
}
