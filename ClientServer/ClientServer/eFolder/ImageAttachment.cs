// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.ImageAttachment
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
  public class ImageAttachment : FileAttachment
  {
    private PageImageCollection pages;

    public ImageAttachment(
      string title,
      string userId,
      string userName,
      PageImage[] pageList,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList)
      : base(sourceList)
    {
      this.id = Guid.NewGuid().ToString();
      this.title = title;
      this.userId = userId;
      this.userName = userName;
      this.pages = new PageImageCollection(this, pageList);
      if (identityList == null)
        return;
      this.Identities.AddRange(identityList);
    }

    public ImageAttachment(string title, FileAttachment existingAttachment, PageImage[] pageList)
      : this(title, existingAttachment.UserID, existingAttachment.UserName, pageList, (FileAttachment[]) null, (DocumentIdentity[]) null)
    {
      this.id = existingAttachment.ID;
      this.date = existingAttachment.date;
      this.documentID = existingAttachment.DocumentID;
      this.isRemoved = existingAttachment.IsRemoved;
      this.isNew = existingAttachment.IsNew;
    }

    public ImageAttachment(BackgroundAttachment attachment, PageImage[] pageList)
      : base(attachment.Sources)
    {
      this.id = attachment.ID;
      this.title = attachment.Title;
      this.userId = attachment.UserID;
      this.userName = attachment.UserName;
      this.date = attachment.date;
      this.pages = new PageImageCollection(this, pageList);
      if (attachment.Identities.Count <= 0)
        return;
      this.Identities.AddRange(attachment.Identities.ToArray());
    }

    public ImageAttachment(FileAttachment attachment, PageImage[] pageList)
      : base(attachment)
    {
      this.pages = new PageImageCollection(this, pageList);
      if (attachment.Identities.Count <= 0)
        return;
      this.Identities.AddRange(attachment.Identities.ToArray());
    }

    public ImageAttachment(XmlElement elm, bool isRemoved)
      : base(elm, isRemoved)
    {
      this.id = new AttributeReader(elm).GetString("ID");
      this.pages = new PageImageCollection(this, (XmlElement) elm.SelectSingleNode("PAGES"));
      foreach (XmlElement selectNode in elm.SelectNodes("Source"))
        this.sourceList.Add(new AttributeReader(selectNode).GetString("ID"));
    }

    public ImageAttachment(
      string id,
      string title,
      string userId,
      string userName,
      PageImage[] pageList,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList)
      : this(title, userId, userName, pageList, sourceList, identityList)
    {
      if (string.IsNullOrEmpty(id))
        return;
      this.id = id;
    }

    public override long FileSize
    {
      get
      {
        long fileSize = 0;
        foreach (PageImage page in this.pages)
          fileSize += page.FileSize;
        return fileSize;
      }
    }

    public PageImageCollection Pages => this.pages;

    public override void ToXml(XmlElement elm)
    {
      base.ToXml(elm);
      new AttributeWriter(elm).Write("ID", (object) this.id);
      XmlElement element1 = elm.OwnerDocument.CreateElement("PAGES");
      elm.AppendChild((XmlNode) element1);
      this.pages.ToXml(element1);
      foreach (string source in this.sourceList)
      {
        XmlElement element2 = elm.OwnerDocument.CreateElement("Source");
        elm.AppendChild((XmlNode) element2);
        new AttributeWriter(element2).Write("ID", (object) source);
      }
    }

    public override XmlElement ToXml()
    {
      XmlElement element = new XmlDocument().CreateElement("Image");
      this.ToXml(element);
      return element;
    }
  }
}
