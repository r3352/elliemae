// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.FileAttachmentReferenceCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class FileAttachmentReferenceCollection : CollectionBase
  {
    private DocumentLog doc;
    private bool isMigrated;

    public FileAttachmentReferenceCollection(DocumentLog doc)
    {
      this.doc = doc;
      this.isMigrated = true;
    }

    public FileAttachmentReferenceCollection(
      DocumentLog doc,
      XmlElement parentElement,
      string groupName)
    {
      this.doc = doc;
      XmlElement e = (XmlElement) parentElement.SelectSingleNode(groupName);
      if (e == null)
        return;
      this.isMigrated = new AttributeReader(e).GetBoolean(nameof (IsMigrated), true);
      foreach (XmlElement selectNode in e.SelectNodes("Reference"))
        this.List.Add((object) new FileAttachmentReference(doc, selectNode));
    }

    public FileAttachmentReference this[int index]
    {
      get
      {
        lock (this.List.SyncRoot)
          return (FileAttachmentReference) this.List[index];
      }
    }

    public FileAttachmentReference this[string attachmentID]
    {
      get
      {
        lock (this.List.SyncRoot)
        {
          foreach (FileAttachmentReference attachmentReference in (IEnumerable) this.List)
          {
            if (attachmentReference.AttachmentID == attachmentID)
              return attachmentReference;
          }
          return (FileAttachmentReference) null;
        }
      }
    }

    public FileAttachmentReference Add(string attachmentID, string userID)
    {
      return this.Add(attachmentID, userID, true);
    }

    public FileAttachmentReference Add(string attachmentID, string userID, bool isActive)
    {
      FileAttachmentReference attachmentReference = this[attachmentID];
      if (attachmentReference != null)
        return attachmentReference;
      FileAttachmentReference fileRef = new FileAttachmentReference(attachmentID, isActive);
      lock (this.List.SyncRoot)
        this.List.Add((object) fileRef);
      fileRef.SetLinkedDocument(this.doc);
      this.doc.MarkLastAttachment();
      this.doc.TrackChange("File attached", fileRef);
      if (isActive && !this.doc.Received)
        this.doc.MarkAsReceived(DateTime.Now, userID);
      return fileRef;
    }

    public FileAttachmentReference Add(
      string attachmentID,
      string userID,
      bool isActive,
      DateTime FileUploadDate)
    {
      FileAttachmentReference attachmentReference = this[attachmentID];
      if (attachmentReference != null)
        return attachmentReference;
      FileAttachmentReference fileRef = new FileAttachmentReference(attachmentID, isActive);
      lock (this.List.SyncRoot)
        this.List.Add((object) fileRef);
      fileRef.SetLinkedDocument(this.doc);
      this.doc.MarkLastAttachment();
      this.doc.TrackChange("File attached", fileRef);
      if (isActive && !this.doc.Received)
        this.doc.MarkAsReceived(FileUploadDate, userID);
      return fileRef;
    }

    public bool Contains(string attachmentID) => this.Contains(attachmentID, true);

    public bool Contains(string attachmentID, bool checkActive)
    {
      lock (this.List.SyncRoot)
      {
        foreach (FileAttachmentReference attachmentReference in (IEnumerable) this.List)
        {
          if ((!checkActive || attachmentReference.IsActive) && attachmentReference.AttachmentID == attachmentID)
            return true;
        }
        return false;
      }
    }

    public void Import(string attachmentID, bool isActive)
    {
      if (this.Contains(attachmentID, false))
        return;
      FileAttachmentReference attachmentReference = new FileAttachmentReference(attachmentID, isActive);
      lock (this.List.SyncRoot)
        this.List.Add((object) attachmentReference);
      attachmentReference.SetLinkedDocument(this.doc);
      this.doc.MarkAsDirty();
    }

    public int IndexOf(string attachmentID)
    {
      lock (this.List.SyncRoot)
      {
        for (int index = 0; index < this.List.Count; ++index)
        {
          if (((FileAttachmentReference) this.List[index]).AttachmentID == attachmentID)
            return index;
        }
        return -1;
      }
    }

    public void Insert(int index, string attachmentID, string userID, bool isActive)
    {
      if (this.Contains(attachmentID, false))
        return;
      FileAttachmentReference attachmentReference = new FileAttachmentReference(attachmentID, isActive);
      lock (this.List.SyncRoot)
        this.List.Insert(index, (object) attachmentReference);
      attachmentReference.SetLinkedDocument(this.doc);
      this.doc.MarkLastAttachment();
      this.doc.MarkLastUpdated();
      if (!attachmentReference.IsActive || this.doc.Received)
        return;
      this.doc.MarkAsReceived(DateTime.Now, userID);
    }

    public void Remove(string attachmentID)
    {
      FileAttachmentReference fileRef = this[attachmentID];
      if (fileRef == null)
        return;
      lock (this.List.SyncRoot)
        this.List.Remove((object) fileRef);
      fileRef.SetLinkedDocument((DocumentLog) null);
      this.doc.TrackChange("File removed", fileRef);
      if (!this.doc.Received || this.HasActiveFile)
        return;
      this.doc.UnmarkAsReceived();
    }

    public void Swap(string attachmentID1, string attachmentID2)
    {
      FileAttachmentReference attachmentReference1 = this[attachmentID1];
      if (attachmentReference1 == null)
        return;
      FileAttachmentReference attachmentReference2 = this[attachmentID2];
      if (attachmentReference2 == null)
        return;
      lock (this.List.SyncRoot)
      {
        int index1 = this.List.IndexOf((object) attachmentReference1);
        int index2 = this.List.IndexOf((object) attachmentReference2);
        this.List[index1] = (object) attachmentReference2;
        this.List[index2] = (object) attachmentReference1;
      }
      this.doc.MarkLastUpdated();
    }

    public bool HasActiveFile
    {
      get
      {
        lock (this.List.SyncRoot)
        {
          foreach (FileAttachmentReference attachmentReference in (IEnumerable) this.List)
          {
            if (attachmentReference.IsActive)
              return true;
          }
          return false;
        }
      }
    }

    public bool IsMigrated
    {
      get => this.isMigrated;
      set => this.isMigrated = value;
    }

    public FileAttachmentReference[] ToArray()
    {
      lock (this.List.SyncRoot)
      {
        FileAttachmentReference[] array = new FileAttachmentReference[this.List.Count];
        this.List.CopyTo((Array) array, 0);
        return array;
      }
    }

    public void ToXml(XmlElement e, string groupName)
    {
      lock (this.List.SyncRoot)
      {
        if (this.List.Count == 0)
          return;
        XmlElement element1 = e.OwnerDocument.CreateElement(groupName);
        new AttributeWriter(element1).Write("IsMigrated", (object) this.isMigrated);
        e.AppendChild((XmlNode) element1);
        foreach (FileAttachmentReference attachmentReference in (IEnumerable) this.List)
        {
          XmlElement element2 = e.OwnerDocument.CreateElement("Reference");
          element1.AppendChild((XmlNode) element2);
          attachmentReference.ToXml(element2);
        }
      }
    }
  }
}
