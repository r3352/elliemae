// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.CloudAttachment
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
  public class CloudAttachment : FileAttachment
  {
    private string objectId;
    private long fileSize;

    public CloudAttachment(
      string objectId,
      string title,
      long fileSize,
      string userId,
      string userName)
      : this(objectId, objectId, title, fileSize, userId, userName)
    {
    }

    public CloudAttachment(
      string id,
      string objectId,
      string title,
      long fileSize,
      string userId,
      string userName)
      : base((FileAttachment[]) null)
    {
      this.id = id;
      this.objectId = objectId;
      this.title = title;
      this.fileSize = fileSize;
      this.userId = userId;
      this.userName = userName;
    }

    public CloudAttachment(
      string objectId,
      string title,
      long fileSize,
      FileAttachment existingAttachment)
      : this(existingAttachment.ID, objectId, title, fileSize, existingAttachment.UserID, existingAttachment.UserName)
    {
      this.date = existingAttachment.date;
      this.documentID = existingAttachment.DocumentID;
      this.isRemoved = existingAttachment.IsRemoved;
      this.isNew = existingAttachment.IsNew;
    }

    public CloudAttachment(FileAttachment attachment, string objectId, long fileSize)
      : base(attachment)
    {
      this.objectId = objectId;
      this.fileSize = fileSize;
    }

    public CloudAttachment(XmlElement elm, bool isRemoved)
      : base(elm, isRemoved)
    {
      AttributeReader attributeReader = new AttributeReader(elm);
      this.id = attributeReader.GetString("ID");
      this.objectId = attributeReader.GetString(nameof (ObjectId), this.id);
      this.fileSize = attributeReader.GetLong(nameof (FileSize));
      foreach (XmlElement selectNode in elm.SelectNodes("Source"))
        this.sourceList.Add(new AttributeReader(selectNode).GetString("ID"));
    }

    public string ObjectId
    {
      get => this.objectId;
      set
      {
        if (!(this.objectId != value))
          return;
        this.objectId = value;
        this.MarkAsDirty();
      }
    }

    public override long FileSize
    {
      get => this.fileSize;
      set
      {
        if (this.fileSize == value)
          return;
        this.fileSize = value;
        this.MarkAsDirty();
      }
    }

    public override void ToXml(XmlElement elm)
    {
      base.ToXml(elm);
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("ID", (object) this.id);
      attributeWriter.Write("ObjectId", (object) this.objectId);
      attributeWriter.Write("FileSize", (object) this.fileSize);
      foreach (string source in this.sourceList)
      {
        XmlElement element = elm.OwnerDocument.CreateElement("Source");
        elm.AppendChild((XmlNode) element);
        new AttributeWriter(element).Write("ID", (object) source);
      }
    }

    public override XmlElement ToXml()
    {
      XmlElement element = new XmlDocument().CreateElement("Cloud");
      this.ToXml(element);
      return element;
    }
  }
}
