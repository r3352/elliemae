// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PageImageAnnotations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the collection of PageImageAnnotation objects that are associated with an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" />.
  /// </summary>
  /// <remarks>Attachments represent documents which have been associated to the loan through
  /// the loan's eFolder. If the loan is configure to use images, it can have an PageImageAnnotations collection.</remarks>
  public class PageImageAnnotations : IPageImageAnnotations, IEnumerable
  {
    private PageImage pageImage;
    private List<PageImageAnnotation> annotations = new List<PageImageAnnotation>();

    /// <summary>Creates an instance of the PageImageAnnotations class</summary>
    internal PageImageAnnotations(PageImage pageImage) => this.pageImage = pageImage;

    /// <summary>
    /// Used internally to add existing annotations to the collection.
    /// </summary>
    /// <param name="annotation">The annotation to be added.</param>
    internal void AddToList(PageImageAnnotation annotation) => this.annotations.Add(annotation);

    /// <summary>Adds a new annotation object.</summary>
    /// <param name="annotation">The annotation to be added.</param>
    public int Add(PageImageAnnotation annotation)
    {
      long ticks = DateTime.Now.Ticks;
      PageAnnotation annotation1 = new PageAnnotation(annotation.AddedBy, annotation.Text, annotation.Left, annotation.Top, annotation.Width, annotation.Height);
      int num = this.pageImage.Annotations.Add(annotation1);
      this.annotations.Clear();
      foreach (PageAnnotation annotation2 in annotation1.Page.Annotations)
        this.annotations.Add(new PageImageAnnotation(annotation2));
      RemoteLogger.Write(TraceLevel.Info, "Annotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      return num;
    }

    /// <summary>
    /// Removes an annotation to the ccollection. User should refresh annotations list after calling.
    /// </summary>
    /// <param name="annotation">The annotation to be removed.</param>
    public void Remove(PageImageAnnotation annotation)
    {
      long ticks = DateTime.Now.Ticks;
      this.pageImage.Annotations.Remove(annotation.getPageAnnotation());
      this.annotations.Remove(annotation);
      RemoteLogger.Write(TraceLevel.Info, "Annotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
    }

    /// <summary>
    /// Gets the number of PageImageAnnotations in the collection.
    /// </summary>
    public int Count => this.annotations.Count;

    /// <summary>
    /// Retrieves an AttachmentPageImage from the collection by index.
    /// </summary>
    public PageImageAnnotation this[int index] => this.annotations[index];

    /// <summary>Provides a enumerator for the collection.</summary>
    /// <returns>Returns an IEnumerator for enumerating the collection.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) this.annotations.GetEnumerator();
  }
}
