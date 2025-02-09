// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.EncPrintSettings
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public class EncPrintSettings
  {
    public string PrinterName { get; set; }

    public int Copies { get; set; }

    public bool Landscape { get; set; }

    public bool PrintBackgrounds { get; set; }

    public bool PrintSelectionOnly { get; set; }

    public bool DisplayHeaderFooter { get; set; }

    public EncPaperSize PaperSize { get; set; }

    public List<EncPageRange> PageRanges { get; set; }

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
