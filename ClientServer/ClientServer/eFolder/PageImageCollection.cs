// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.PageImageCollection
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class PageImageCollection : CollectionBase
  {
    private ImageAttachment attachment;

    internal PageImageCollection(ImageAttachment attachment, PageImage[] pageList)
    {
      this.attachment = attachment;
      this.AddRange(pageList);
    }

    internal PageImageCollection(ImageAttachment attachment, XmlElement elm)
    {
      this.attachment = attachment;
      foreach (XmlElement selectNode in elm.SelectNodes("PAGE"))
        this.List.Add((object) new PageImage(attachment, selectNode));
    }

    public void AddRange(PageImage[] pageList) => this.AddRange(pageList, -1);

    public void AddRange(PageImage[] pageList, int insertPosition)
    {
      Dictionary<ImageAttachment, int> dictionary = new Dictionary<ImageAttachment, int>();
      lock (this.List.SyncRoot)
      {
        foreach (PageImage page in pageList)
        {
          if (!this.List.Contains((object) page))
          {
            if (page.Attachment != null)
            {
              if (dictionary.ContainsKey(page.Attachment))
                dictionary[page.Attachment]++;
              else
                dictionary.Add(page.Attachment, 1);
              page.Attachment.Pages.Remove(page);
            }
            if (insertPosition == -1)
              this.List.Add((object) page);
            else
              this.List.Insert(insertPosition, (object) page);
            page.SetAttachment(this.attachment);
          }
        }
      }
      foreach (KeyValuePair<ImageAttachment, int> keyValuePair in dictionary)
      {
        keyValuePair.Key.TrackChange("Split " + (object) keyValuePair.Value + " pages to ", (FileAttachment) this.attachment);
        this.attachment.TrackChange("Merged " + (object) keyValuePair.Value + " pages from ", (FileAttachment) keyValuePair.Key);
      }
    }

    public new void Clear()
    {
      lock (this.List.SyncRoot)
        this.List.Clear();
    }

    public bool Contains(PageImage page)
    {
      lock (this.List.SyncRoot)
        return this.List.Contains((object) page);
    }

    public new int Count
    {
      get
      {
        lock (this.List.SyncRoot)
          return this.List.Count;
      }
    }

    public int IndexOf(PageImage page)
    {
      lock (this.List.SyncRoot)
        return this.List.IndexOf((object) page);
    }

    public void Insert(int index, PageImage page)
    {
      lock (this.List.SyncRoot)
      {
        if (!this.List.Contains((object) page))
        {
          this.AddRange(new PageImage[1]{ page }, index);
          return;
        }
        int index1 = this.List.IndexOf((object) page);
        if (index == index1)
          return;
        if (index1 < index)
        {
          this.List.Insert(index, (object) page);
          this.List.RemoveAt(index1);
        }
        else if (index1 > index)
        {
          this.List.RemoveAt(index1);
          this.List.Insert(index, (object) page);
        }
      }
      this.attachment.MarkAsDirty();
    }

    public new IEnumerator GetEnumerator()
    {
      PageImage[] pageImageArray = (PageImage[]) null;
      lock (this.List.SyncRoot)
      {
        pageImageArray = new PageImage[this.List.Count];
        this.List.CopyTo((Array) pageImageArray, 0);
      }
      return pageImageArray.GetEnumerator();
    }

    public PageImage this[int index]
    {
      get
      {
        lock (this.List.SyncRoot)
          return (PageImage) this.List[index];
      }
    }

    public void RemoveRange(PageImage[] pageList)
    {
      int num = 0;
      lock (this.List.SyncRoot)
      {
        foreach (PageImage page in pageList)
        {
          if (this.List.Contains((object) page))
          {
            this.List.Remove((object) page);
            page.SetAttachment((ImageAttachment) null);
            ++num;
          }
        }
      }
      if (num <= 0)
        return;
      this.attachment.TrackChange("Deleted " + num.ToString() + " pages");
    }

    public void Remove(PageImage page)
    {
      lock (this.List.SyncRoot)
      {
        if (!this.List.Contains((object) page))
          return;
        this.List.Remove((object) page);
        page.SetAttachment((ImageAttachment) null);
      }
      this.attachment.MarkAsDirty();
    }

    public new void RemoveAt(int index) => this.Remove(this[index]);

    public PageImage[] ToArray()
    {
      PageImage[] array = (PageImage[]) null;
      lock (this.List.SyncRoot)
      {
        array = new PageImage[this.List.Count];
        this.List.CopyTo((Array) array, 0);
      }
      return array;
    }

    public void ToXml(XmlElement elm)
    {
      foreach (PageImage pageImage in (IEnumerable) this.List)
      {
        XmlElement element = elm.OwnerDocument.CreateElement("PAGE");
        elm.AppendChild((XmlNode) element);
        pageImage.ToXml(element);
      }
    }
  }
}
