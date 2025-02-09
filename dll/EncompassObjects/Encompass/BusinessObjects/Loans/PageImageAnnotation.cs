// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PageImageAnnotation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.eFolder;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class PageImageAnnotation : IPageImageAnnotation
  {
    private PageAnnotation annotation;
    private DateTime date;
    private string addedBy;
    private string text;
    private int left;
    private int top;
    private int width;
    private int height;

    public PageImageAnnotation(
      string addedBy,
      string text,
      int left,
      int top,
      int width,
      int height)
    {
      this.date = DateTime.Now;
      this.addedBy = addedBy;
      this.text = text;
      this.left = left;
      this.top = top;
      this.width = width;
      this.height = height;
    }

    internal PageImageAnnotation(PageAnnotation pageAnnotation)
    {
      this.annotation = pageAnnotation;
      this.date = pageAnnotation.Date;
      this.addedBy = pageAnnotation.AddedBy;
      this.text = pageAnnotation.Text;
      this.left = pageAnnotation.Left;
      this.top = pageAnnotation.Top;
      this.width = pageAnnotation.Width;
      this.height = pageAnnotation.Height;
    }

    internal PageAnnotation getPageAnnotation() => this.annotation;

    public DateTime Date => this.date;

    public string AddedBy => this.addedBy;

    public string Text => this.text;

    public int Left => this.left;

    public int Top => this.top;

    public int Width => this.width;

    public int Height => this.height;
  }
}
