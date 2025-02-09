// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.DocumentIdentityCollection
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class DocumentIdentityCollection : IEnumerable
  {
    private FileAttachment attachment;
    private Dictionary<int, DocumentIdentity> identityList = new Dictionary<int, DocumentIdentity>();
    private bool complete;

    internal DocumentIdentityCollection(FileAttachment attachment) => this.attachment = attachment;

    internal DocumentIdentityCollection(FileAttachment attachment, XmlElement elm)
    {
      this.attachment = attachment;
      foreach (XmlElement selectNode in elm.SelectNodes("Identity"))
      {
        DocumentIdentity documentIdentity = new DocumentIdentity(selectNode);
        this.identityList[documentIdentity.PageIndex] = documentIdentity;
      }
    }

    public void Add(DocumentIdentity identity)
    {
      if (this.attachment is ImageAttachment)
      {
        ImageAttachment attachment = (ImageAttachment) this.attachment;
        if (attachment.Pages.Count <= identity.PageIndex)
          return;
        attachment.Pages[identity.PageIndex].Identity = identity;
      }
      else
        this.identityList[identity.PageIndex] = identity;
    }

    public void AddRange(DocumentIdentity[] identities)
    {
      if (this.attachment is ImageAttachment)
      {
        ImageAttachment attachment = (ImageAttachment) this.attachment;
        foreach (DocumentIdentity identity in identities)
        {
          if (attachment.Pages.Count > identity.PageIndex)
            attachment.Pages[identity.PageIndex].Identity = identity;
        }
      }
      else
      {
        foreach (DocumentIdentity identity in identities)
          this.identityList[identity.PageIndex] = identity;
      }
    }

    public void Clear()
    {
      if (this.attachment is ImageAttachment)
      {
        foreach (PageImage page in ((ImageAttachment) this.attachment).Pages)
          page.Identity = (DocumentIdentity) null;
      }
      else
        this.identityList.Clear();
    }

    public int Count => this.ToArray().Length;

    public DocumentIdentity Get(int pageIndex)
    {
      if (this.attachment is ImageAttachment)
      {
        ImageAttachment attachment = (ImageAttachment) this.attachment;
        if (attachment.Pages.Count > pageIndex)
          return attachment.Pages[pageIndex].Identity;
      }
      else if (this.identityList.ContainsKey(pageIndex))
        return this.identityList[pageIndex];
      return (DocumentIdentity) null;
    }

    public IEnumerator GetEnumerator() => this.ToArray().GetEnumerator();

    public bool Complete
    {
      get
      {
        if (this.attachment is NativeAttachment)
          return this.attachment.Identities.Get(0) != null;
        if (!(this.attachment is ImageAttachment))
          return this.complete;
        foreach (PageImage page in ((ImageAttachment) this.attachment).Pages)
        {
          if (page.Identity == null)
            return false;
        }
        return true;
      }
      set => this.complete = value;
    }

    public void Remove(int pageIndex)
    {
      if (this.attachment is ImageAttachment)
      {
        ImageAttachment attachment = (ImageAttachment) this.attachment;
        if (attachment.Pages.Count <= pageIndex)
          return;
        attachment.Pages[pageIndex].Identity = (DocumentIdentity) null;
      }
      else
      {
        if (!this.identityList.ContainsKey(pageIndex))
          return;
        this.identityList.Remove(pageIndex);
      }
    }

    public DocumentIdentity[] ToArray()
    {
      if (!(this.attachment is ImageAttachment))
        return this.identityList.Values.ToArray<DocumentIdentity>();
      List<DocumentIdentity> documentIdentityList = new List<DocumentIdentity>();
      foreach (PageImage page in ((ImageAttachment) this.attachment).Pages)
      {
        DocumentIdentity identity = page.Identity;
        if (identity != null)
          documentIdentityList.Add(identity);
      }
      return documentIdentityList.ToArray();
    }

    public void ToXml(XmlElement elm)
    {
      if (this.attachment is ImageAttachment)
        throw new NotSupportedException();
      foreach (DocumentIdentity documentIdentity in this.identityList.Values)
      {
        XmlElement element = elm.OwnerDocument.CreateElement("Identity");
        elm.AppendChild((XmlNode) element);
        documentIdentity.ToXml(element);
      }
    }
  }
}
