// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.PageAnnotation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class PageAnnotation
  {
    private PageImage page;
    private DateTime date;
    private string addedBy;
    private string text;
    private int left;
    private int top;
    private int width;
    private int height;
    private PageAnnotationVisibilityType visibility;

    public PageAnnotation(string addedBy, string text, int left, int top, int width, int height)
      : this(addedBy, text, left, top, width, height, PageAnnotationVisibilityType.Public)
    {
    }

    public PageAnnotation(
      string addedBy,
      string text,
      int left,
      int top,
      int width,
      int height,
      PageAnnotationVisibilityType visibility)
    {
      this.date = DateTime.Now;
      this.addedBy = addedBy;
      this.text = text;
      this.left = left;
      this.top = top;
      this.width = width;
      this.height = height;
      this.visibility = visibility;
    }

    internal PageAnnotation(PageImage page, XmlElement elm)
    {
      this.page = page;
      AttributeReader attributeReader = new AttributeReader(elm);
      this.date = attributeReader.GetDate(nameof (Date));
      this.addedBy = attributeReader.GetString(nameof (AddedBy));
      this.text = attributeReader.GetString(nameof (Text));
      this.left = attributeReader.GetInteger(nameof (Left));
      this.top = attributeReader.GetInteger(nameof (Top));
      this.width = attributeReader.GetInteger(nameof (Width), 20);
      this.height = attributeReader.GetInteger(nameof (Height), 20);
      this.visibility = (PageAnnotationVisibilityType) Enum.ToObject(typeof (PageAnnotationVisibilityType), attributeReader.GetInteger(nameof (Visibility)));
    }

    public PageImage Page => this.page;

    public DateTime Date => this.date;

    public string AddedBy => this.addedBy;

    public string Text => this.text;

    public int Left
    {
      get => this.left;
      set
      {
        this.left = value;
        this.page.Attachment.MarkAsDirty();
      }
    }

    public int Top
    {
      get => this.top;
      set
      {
        this.top = value;
        this.page.Attachment.MarkAsDirty();
      }
    }

    public int Width => this.width;

    public int Height => this.height;

    public PageAnnotationVisibilityType Visibility
    {
      get => this.visibility;
      set
      {
        this.visibility = value;
        this.page.Attachment.MarkAsDirty();
      }
    }

    internal void SetPage(PageImage page) => this.page = page;

    internal void Rotate()
    {
      int top = this.top;
      this.top = this.left;
      this.left = this.page.Width - (top + this.height);
      int width = this.width;
      this.width = this.height;
      this.height = width;
    }

    public void ToXml(XmlElement elm)
    {
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("Date", (object) this.date);
      attributeWriter.Write("AddedBy", (object) this.addedBy);
      attributeWriter.Write("Text", (object) this.text);
      attributeWriter.Write("Left", (object) this.left);
      attributeWriter.Write("Top", (object) this.top);
      attributeWriter.Write("Width", (object) this.width);
      attributeWriter.Write("Height", (object) this.height);
      attributeWriter.Write("Visibility", (object) (int) this.Visibility);
    }
  }
}
