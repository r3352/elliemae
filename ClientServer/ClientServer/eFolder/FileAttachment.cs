// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.FileAttachment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public abstract class FileAttachment
  {
    private const string className = "FileAttachment�";
    private static readonly string sw = Tracing.SwEFolder;
    protected string id;
    protected string title;
    protected string userId;
    protected string userName;
    internal string date = DateTime.UtcNow.ToString("u");
    protected string documentID;
    protected bool isActive = true;
    protected List<string> sourceList = new List<string>();
    protected DocumentIdentityCollection identities;
    protected bool isRemoved;
    protected bool isNew;
    protected bool isDirty;
    protected AiqMetadata aiqMetadata;
    [NonSerialized]
    private IFileHistoryMonitor monitor;

    public FileAttachment(FileAttachment[] sourceList)
    {
      if (sourceList != null)
      {
        foreach (FileAttachment source in sourceList)
        {
          this.sourceList.Add(source.ID);
          this.aiqMetadata = source.aiqMetadata;
        }
      }
      this.isNew = true;
    }

    public FileAttachment(string[] sourceList)
    {
      if (sourceList != null)
      {
        foreach (string source in sourceList)
          this.sourceList.Add(source);
      }
      this.isNew = true;
    }

    public FileAttachment(FileAttachment attachment)
      : this(attachment.Sources)
    {
      this.id = attachment.ID;
      this.userId = attachment.UserID;
      this.userName = attachment.UserName;
      this.date = attachment.date;
      this.title = attachment.Title;
      this.isRemoved = attachment.IsRemoved;
      this.isActive = attachment.IsActive;
      this.documentID = attachment.documentID;
      this.isNew = attachment.IsNew;
      this.aiqMetadata = attachment.aiqMetadata;
    }

    public FileAttachment(XmlElement elm, bool isRemoved)
    {
      AttributeReader attributeReader1 = new AttributeReader(elm);
      this.title = attributeReader1.GetString(nameof (Title));
      this.userId = attributeReader1.GetString(nameof (UserID));
      this.userName = attributeReader1.GetString(nameof (UserName));
      this.date = attributeReader1.GetString(nameof (Date));
      this.documentID = attributeReader1.GetString("DocID", (string) null);
      this.isActive = attributeReader1.GetBoolean(nameof (IsActive), true);
      this.isRemoved = isRemoved;
      if (this.title == string.Empty)
        this.title = "Untitled";
      if (!elm.HasChildNodes)
        return;
      XmlElement e = (XmlElement) elm.SelectSingleNode("Aiq");
      if (e == null)
        return;
      AttributeReader attributeReader2 = new AttributeReader(e);
      this.aiqMetadata = new AiqMetadata();
      this.aiqMetadata.Id = attributeReader2.GetString("Id");
      this.aiqMetadata.Type = attributeReader2.GetString("Type");
      this.aiqMetadata.Status = attributeReader2.GetString("Status");
      this.aiqMetadata.FriendlyId = attributeReader2.GetString("FriendlyId");
      this.aiqMetadata.Comments = attributeReader2.GetString("Comments");
      if (e.HasAttribute("LastUpdated"))
        this.aiqMetadata.LastUpdated = new DateTime?(attributeReader2.GetDate("LastUpdated"));
      if (!e.HasChildNodes)
        return;
      XmlNode xmlNode = e.SelectSingleNode("Tags");
      if (xmlNode == null || !xmlNode.HasChildNodes)
        return;
      this.aiqMetadata.Tags = new string[xmlNode.ChildNodes.Count];
      for (int i = 0; i < xmlNode.ChildNodes.Count; ++i)
        this.aiqMetadata.Tags[i] = xmlNode.ChildNodes[i].Attributes["tag"].Value;
    }

    [CLSCompliant(false)]
    public string ID => this.id;

    [CLSCompliant(false)]
    public string Title
    {
      get => this.title;
      set
      {
        if (!(this.title != value))
          return;
        this.title = value;
        this.MarkAsDirty();
      }
    }

    public virtual long FileSize { get; set; }

    [CLSCompliant(false)]
    public string UserID => this.userId;

    [CLSCompliant(false)]
    public string UserName => this.userName;

    [CLSCompliant(false)]
    public DateTime Date => AttributeReader.ParseDateTime(this.date);

    [CLSCompliant(false)]
    public string DocumentID => this.documentID;

    [CLSCompliant(false)]
    public bool IsActive
    {
      get => this.isActive;
      set => this.isActive = value;
    }

    [CLSCompliant(false)]
    public bool IsDirty => this.isDirty || this.isNew;

    [CLSCompliant(false)]
    public bool IsNew => this.isNew;

    public string[] Sources => this.sourceList.ToArray();

    [CLSCompliant(false)]
    public DocumentIdentityCollection Identities
    {
      get
      {
        if (this.identities == null)
          this.identities = new DocumentIdentityCollection(this);
        return this.identities;
      }
    }

    [CLSCompliant(false)]
    public bool IsRemoved
    {
      get => this.isRemoved;
      set
      {
        if (this.isRemoved == value)
          return;
        this.isRemoved = value;
        this.MarkAsDirty();
      }
    }

    public AttachmentType AttachmentType
    {
      get
      {
        switch (this)
        {
          case BackgroundAttachment _:
            return AttachmentType.Background;
          case ImageAttachment _:
            return AttachmentType.Image;
          case CloudAttachment _:
            return AttachmentType.Cloud;
          default:
            return AttachmentType.Native;
        }
      }
    }

    public AiqMetadata AiqMetadata
    {
      get => this.aiqMetadata;
      set
      {
        if (this.aiqMetadata == value)
          return;
        this.aiqMetadata = value;
        this.MarkAsDirty();
      }
    }

    public void AttachLoanHistoryMonitor(IFileHistoryMonitor monitor) => this.monitor = monitor;

    protected internal void TrackChange(string details)
    {
      if (this.monitor != null)
        this.monitor.TrackChange(this, details);
      this.MarkAsDirty();
    }

    protected internal void TrackChange(string details, FileAttachment linkedFile)
    {
      if (this.monitor != null)
        this.monitor.TrackChange(this, details, linkedFile);
      this.MarkAsDirty();
    }

    protected internal void TrackChange(PageImage page, string details)
    {
      if (this.monitor != null)
        this.monitor.TrackChange(page, details, this);
      this.MarkAsDirty();
    }

    public virtual void MarkAsClean()
    {
      this.isDirty = false;
      this.isNew = false;
    }

    public virtual void MarkAsDirty()
    {
      this.isDirty = true;
      this.OnFileAttachmentChanged(EventArgs.Empty);
    }

    public virtual void ToXml(XmlElement elm)
    {
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("Title", (object) this.title);
      attributeWriter.Write("UserID", (object) this.userId);
      attributeWriter.Write("UserName", (object) this.userName);
      attributeWriter.Write("Date", (object) this.date);
      attributeWriter.Write("DocID", (object) this.documentID);
      attributeWriter.Write("IsActive", (object) this.isActive);
      XmlDocument ownerDocument = elm.OwnerDocument;
      if (this.aiqMetadata == null)
        return;
      XmlElement xmlElement1 = (XmlElement) elm.AppendChild((XmlNode) ownerDocument.CreateElement("Aiq"));
      if (!string.IsNullOrWhiteSpace(this.aiqMetadata.Id))
        xmlElement1.SetAttribute("Id", this.aiqMetadata.Id);
      if (!string.IsNullOrWhiteSpace(this.aiqMetadata.Type))
        xmlElement1.SetAttribute("Type", this.aiqMetadata.Type);
      if (!string.IsNullOrWhiteSpace(this.aiqMetadata.Status))
        xmlElement1.SetAttribute("Status", this.aiqMetadata.Status);
      if (!string.IsNullOrWhiteSpace(this.aiqMetadata.FriendlyId))
        xmlElement1.SetAttribute("FriendlyId", this.aiqMetadata.FriendlyId);
      if (!string.IsNullOrWhiteSpace(this.aiqMetadata.Comments))
        xmlElement1.SetAttribute("Comments", this.aiqMetadata.Comments);
      string[] tags = this.aiqMetadata.Tags;
      if ((tags != null ? (((IEnumerable<string>) tags).Count<string>() > 0 ? 1 : 0) : 0) != 0)
      {
        XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) ownerDocument.CreateElement("Tags"));
        foreach (string tag in this.aiqMetadata.Tags)
          ((XmlElement) xmlElement2.AppendChild((XmlNode) ownerDocument.CreateElement("Entry"))).SetAttribute("tag", tag);
      }
      if (!this.aiqMetadata.LastUpdated.HasValue)
        return;
      xmlElement1.SetAttribute("LastUpdated", Convert.ToString((object) this.aiqMetadata.LastUpdated, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public abstract XmlElement ToXml();

    [field: NonSerialized]
    public event EventHandler FileAttachmentChanged;

    public virtual void OnFileAttachmentChanged(EventArgs e)
    {
      if (this.FileAttachmentChanged == null)
        return;
      this.FileAttachmentChanged((object) this, e);
    }
  }
}
