// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.EventObjects.EncCustomPrintHandler
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Handlers;
using DotNetBrowser.Print;
using DotNetBrowser.Print.Events;
using DotNetBrowser.Print.Handlers;
using DotNetBrowser.Print.Settings;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Web.Host.EventObjects
{
  internal class EncCustomPrintHandler
  {
    private static readonly string sw = Tracing.SwThinThick;
    public bool IsPrintJobActive;

    public event EncCustomPrintHandler.EncPrintJobEvent OnPrintJobEvent;

    public Handler<PrintHtmlContentParameters, PrintHtmlContentResponse> PrintHtmlHandler(
      PrintSettings inputPrintSettings)
    {
      return new Handler<PrintHtmlContentParameters, PrintHtmlContentResponse>((Func<PrintHtmlContentParameters, PrintHtmlContentResponse>) (p =>
      {
        IPrintJob<PdfPrinter.IHtmlSettings> printJob = p.Printers.Pdf.PrintJob;
        printJob.PrintCompleted += (EventHandler<PrintCompletedEventArgs<PdfPrinter.IHtmlSettings>>) ((sender, args) => this.OnPrintJobEvent(sender, new PrintJobEventArgs(args.IsCompletedSuccessfully)));
        printJob.Settings.Apply<PdfPrinter.IHtmlSettings>((Action<PdfPrinter.IHtmlSettings>) (s =>
        {
          s.PrintingHeaderFooterEnabled = true;
          s.Orientation = inputPrintSettings.Landscape ? Orientation.Landscape : Orientation.Portrait;
          s.PrintingBackgroundsEnabled = inputPrintSettings.PrintBackgrounds;
          s.PrintingSelectionOnlyEnabled = inputPrintSettings.PrintSelectionOnly;
          s.PrintingHeaderFooterEnabled = inputPrintSettings.DisplayHeaderFooter;
          s.PaperSize = (PaperSize) Enum.Parse(typeof (PaperSize), inputPrintSettings.PaperSize.ToString());
          List<PageRange> pageRanges = this.GetPageRanges(inputPrintSettings.PageRanges);
          if (pageRanges != null && pageRanges.Any<PageRange>())
            s.PageRanges = (IReadOnlyCollection<PageRange>) pageRanges;
          if (string.IsNullOrWhiteSpace(inputPrintSettings.PdfFilePath))
            return;
          s.PdfFilePath = inputPrintSettings.PdfFilePath;
        }));
        return PrintHtmlContentResponse.Print(p.Printers.Pdf);
      }));
    }

    private List<PageRange> GetPageRanges(List<PrintPageRange> encPageRanges)
    {
      List<PageRange> pageRanges = (List<PageRange>) null;
      if (encPageRanges != null && encPageRanges.Any<PrintPageRange>())
      {
        pageRanges = new List<PageRange>();
        foreach (PrintPageRange encPageRange in encPageRanges)
          pageRanges.Add(new PageRange(encPageRange.From, encPageRange.To));
      }
      return pageRanges;
    }

    public delegate void EncPrintJobEvent(object sender, PrintJobEventArgs args);
  }
}
