// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.ImageMouseEventArgs
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class ImageMouseEventArgs : MouseEventArgs
  {
    private int pdfX;
    private int pdfY;

    public ImageMouseEventArgs(MouseEventArgs e, int pdfX, int pdfY)
      : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
    {
      this.pdfX = pdfX;
      this.pdfY = pdfY;
    }

    public int PdfX => this.pdfX;

    public int PdfY => this.pdfY;
  }
}
