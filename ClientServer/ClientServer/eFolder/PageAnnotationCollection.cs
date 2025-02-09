// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.PageAnnotationCollection
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class PageAnnotationCollection : CollectionBase
  {
    private PageImage page;

    internal PageAnnotationCollection(PageImage page) => this.page = page;

    internal PageAnnotationCollection(PageImage page, XmlElement elm)
    {
      this.page = page;
      foreach (XmlElement selectNode in elm.SelectNodes("ANNOTATION"))
        this.List.Add((object) new PageAnnotation(this.page, selectNode));
    }

    public int Add(PageAnnotation annotation)
    {
      int num = 0;
      lock (this.List.SyncRoot)
      {
        if (this.List.Contains((object) annotation))
          return this.List.IndexOf((object) annotation);
        if (annotation.Page != null)
          annotation.Page.Annotations.Remove(annotation);
        num = this.List.Add((object) annotation);
        annotation.SetPage(this.page);
      }
      this.page.Attachment.TrackChange(this.page, "Notes added \"" + annotation.Text + "\"");
      return num;
    }

    public new void Clear()
    {
      lock (this.List.SyncRoot)
        this.List.Clear();
    }

    public bool Contains(PageAnnotation annotation)
    {
      lock (this.List.SyncRoot)
        return this.List.Contains((object) annotation);
    }

    public new int Count
    {
      get
      {
        lock (this.List.SyncRoot)
          return this.List.Count;
      }
    }

    public new IEnumerator GetEnumerator()
    {
      PageAnnotation[] pageAnnotationArray = (PageAnnotation[]) null;
      lock (this.List.SyncRoot)
      {
        pageAnnotationArray = new PageAnnotation[this.List.Count];
        this.List.CopyTo((Array) pageAnnotationArray, 0);
      }
      return pageAnnotationArray.GetEnumerator();
    }

    public PageAnnotation this[int index]
    {
      get
      {
        lock (this.List.SyncRoot)
          return (PageAnnotation) this.List[index];
      }
    }

    public void Remove(PageAnnotation annotation)
    {
      lock (this.List.SyncRoot)
      {
        if (!this.List.Contains((object) annotation))
          return;
        this.List.Remove((object) annotation);
        annotation.SetPage((PageImage) null);
      }
      this.page.Attachment.TrackChange(this.page, "Notes deleted \"" + annotation.Text + "\"");
    }

    public new void RemoveAt(int index) => this.Remove(this[index]);

    public void ToXml(XmlElement elm)
    {
      foreach (PageAnnotation pageAnnotation in (IEnumerable) this.List)
      {
        XmlElement element = elm.OwnerDocument.CreateElement("ANNOTATION");
        elm.AppendChild((XmlNode) element);
        pageAnnotation.ToXml(element);
      }
    }
  }
}
