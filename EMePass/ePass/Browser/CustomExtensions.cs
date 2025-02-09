// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.CustomExtensions
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using Elli.Web.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public static class CustomExtensions
  {
    public static PrintSettings CustomPrintSettingsMapper<T>(
      this EncPrintSettings fromEncPrintSettings)
    {
      if (fromEncPrintSettings == null)
        return (PrintSettings) null;
      PrintSettings printSettings1 = new PrintSettings();
      printSettings1.PrinterName = fromEncPrintSettings.PrinterName;
      printSettings1.Copies = fromEncPrintSettings.Copies;
      printSettings1.Landscape = fromEncPrintSettings.Landscape;
      printSettings1.PrintBackgrounds = fromEncPrintSettings.PrintBackgrounds;
      printSettings1.PrintSelectionOnly = fromEncPrintSettings.PrintSelectionOnly;
      printSettings1.DisplayHeaderFooter = fromEncPrintSettings.DisplayHeaderFooter;
      printSettings1.PrintToPDF = fromEncPrintSettings.PrintToPDF;
      printSettings1.PdfFileName = fromEncPrintSettings.PdfFileName;
      printSettings1.PdfDirLocation = fromEncPrintSettings.PdfDirLocation;
      printSettings1.PaperSize = (PrintPaperSize) Enum.Parse(typeof (PrintPaperSize), fromEncPrintSettings.PaperSize.ToString());
      if (fromEncPrintSettings?.PageRanges != null && fromEncPrintSettings.PageRanges.Any<EncPageRange>())
      {
        PrintSettings printSettings2 = printSettings1;
        IEnumerable<PrintPageRange> source = fromEncPrintSettings.PageRanges.Select<EncPageRange, PrintPageRange>((Func<EncPageRange, PrintPageRange>) (item => new PrintPageRange(item.From, item.To)));
        List<PrintPageRange> list = source != null ? source.ToList<PrintPageRange>() : (List<PrintPageRange>) null;
        printSettings2.PageRanges = list;
      }
      return printSettings1;
    }

    public static ModuleParameters CustomModuleParametersMapper(
      this EncModuleParameters fromEncModuleParameter)
    {
      return fromEncModuleParameter == null ? (ModuleParameters) null : new ModuleParameters(fromEncModuleParameter.User != null ? new ModuleUser(fromEncModuleParameter.User.ID, fromEncModuleParameter.User.FirstName, fromEncModuleParameter.User.LastName, fromEncModuleParameter.User.Email) : (ModuleUser) null, fromEncModuleParameter.Parameters);
    }

    public static T CustomObjectMapper<T>(this object fromObject)
    {
      return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(fromObject));
    }
  }
}
