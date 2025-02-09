// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.FileAttachmentReference
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class FileAttachmentReference
  {
    private string attachmentID;
    private DocumentLog doc;
    private bool isActive;

    public FileAttachmentReference(string attachmentID)
      : this(attachmentID, true)
    {
    }

    public FileAttachmentReference(string attachmentID, bool isActive)
    {
      this.attachmentID = attachmentID;
      this.isActive = isActive;
    }

    public FileAttachmentReference(DocumentLog doc, XmlElement e)
    {
      this.doc = doc;
      AttributeReader attributeReader = new AttributeReader(e);
      this.attachmentID = attributeReader.GetString(nameof (AttachmentID));
      this.isActive = attributeReader.GetBoolean(nameof (IsActive));
    }

    public string AttachmentID => this.attachmentID;

    public DocumentLog Document => this.doc;

    public bool IsActive => this.isActive;

    public void MarkAsActive(string userID)
    {
      if (this.isActive)
        return;
      this.isActive = true;
      this.doc.TrackChange("File Current Version checked", this);
      if (this.doc.Received)
        return;
      this.doc.MarkAsReceived(DateTime.Now, userID);
    }

    public void UnmarkAsActive()
    {
      if (!this.isActive)
        return;
      this.isActive = false;
      this.doc.TrackChange("File Current Version unchecked", this);
      if (!this.doc.Received || this.doc.Files.HasActiveFile)
        return;
      this.doc.UnmarkAsReceived();
    }

    internal void SetLinkedDocument(DocumentLog doc) => this.doc = doc;

    public void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("AttachmentID", (object) this.attachmentID);
      attributeWriter.Write("IsActive", (object) this.isActive);
    }
  }
}
