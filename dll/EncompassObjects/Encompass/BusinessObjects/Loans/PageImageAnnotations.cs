// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PageImageAnnotations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class PageImageAnnotations : IPageImageAnnotations, IEnumerable
  {
    private PageImage pageImage;
    private List<PageImageAnnotation> annotations = new List<PageImageAnnotation>();

    internal PageImageAnnotations(PageImage pageImage) => this.pageImage = pageImage;

    internal void AddToList(PageImageAnnotation annotation) => this.annotations.Add(annotation);

    public int Add(PageImageAnnotation annotation)
    {
      long ticks = DateTime.Now.Ticks;
      PageAnnotation pageAnnotation = new PageAnnotation(annotation.AddedBy, annotation.Text, annotation.Left, annotation.Top, annotation.Width, annotation.Height);
      int num = this.pageImage.Annotations.Add(pageAnnotation);
      this.annotations.Clear();
      foreach (PageAnnotation annotation1 in pageAnnotation.Page.Annotations)
        this.annotations.Add(new PageImageAnnotation(annotation1));
      RemoteLogger.Write(TraceLevel.Info, "Annotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      return num;
    }

    public void Remove(PageImageAnnotation annotation)
    {
      long ticks = DateTime.Now.Ticks;
      this.pageImage.Annotations.Remove(annotation.getPageAnnotation());
      this.annotations.Remove(annotation);
      RemoteLogger.Write(TraceLevel.Info, "Annotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
    }

    public int Count => this.annotations.Count;

    public PageImageAnnotation this[int index] => this.annotations[index];

    public IEnumerator GetEnumerator() => (IEnumerator) this.annotations.GetEnumerator();
  }
}
