// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.NativeAttachment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class NativeAttachment : FileAttachment
  {
    private const string className = "NativeAttachment�";
    private static readonly string sw = Tracing.SwEFolder;
    private int rotation;
    private long fileSize;
    private string rotatedFile;
    private string convertedFile;

    public NativeAttachment(
      string filename,
      string title,
      long fileSize,
      string userId,
      string userName,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList)
      : base(sourceList)
    {
      this.id = filename;
      this.title = title;
      this.fileSize = fileSize;
      this.userId = userId;
      this.userName = userName;
      if (identityList == null)
        return;
      this.Identities.AddRange(identityList);
    }

    public NativeAttachment(string title, long fileSize, FileAttachment existingAttachment)
      : this(existingAttachment.ID, title, fileSize, existingAttachment.UserID, existingAttachment.UserName, (FileAttachment[]) null, (DocumentIdentity[]) null)
    {
      this.date = existingAttachment.date;
      this.documentID = existingAttachment.DocumentID;
      this.isRemoved = existingAttachment.IsRemoved;
      this.isActive = existingAttachment.IsActive;
      this.isNew = existingAttachment.IsNew;
    }

    public NativeAttachment(BackgroundAttachment attachment, long fileSize)
      : base(attachment.Sources)
    {
      this.id = attachment.ID;
      this.title = attachment.Title;
      this.userId = attachment.UserID;
      this.userName = attachment.UserName;
      this.date = attachment.date;
      this.fileSize = fileSize;
      if (attachment.Identities.Count <= 0)
        return;
      this.Identities.AddRange(attachment.Identities.ToArray());
    }

    public NativeAttachment(FileAttachment attachment, long fileSize)
      : base(attachment)
    {
      this.fileSize = fileSize;
    }

    public NativeAttachment(XmlElement elm, bool isRemoved)
      : base(elm, isRemoved)
    {
      AttributeReader attributeReader = new AttributeReader(elm);
      this.id = attributeReader.GetString("Filename");
      this.rotation = attributeReader.GetInteger(nameof (Rotation));
      this.fileSize = attributeReader.GetLong(nameof (FileSize));
      this.ObjectId = attributeReader.GetString(nameof (ObjectId));
      foreach (XmlElement selectNode in elm.SelectNodes("Source"))
        this.sourceList.Add(new AttributeReader(selectNode).GetString("Filename"));
    }

    public override long FileSize => this.fileSize;

    public int Rotation
    {
      get => this.rotation;
      set
      {
        if (this.rotation == value)
          return;
        this.rotation = value;
        this.rotatedFile = (string) null;
        this.MarkAsDirty();
      }
    }

    public string ObjectId { get; set; }

    public SDCDocument CurrentSDCDocument { get; set; }

    public SDCDocument OriginalSDCDocument { get; set; }

    public string GetConvertedFile()
    {
      Tracing.Log(NativeAttachment.sw, TraceLevel.Verbose, nameof (NativeAttachment), "GetConvertedFile: " + this.id);
      return this.convertedFile != null && File.Exists(this.convertedFile) ? this.convertedFile : (string) null;
    }

    public void SetConvertedFile(string filepath)
    {
      Tracing.Log(NativeAttachment.sw, TraceLevel.Verbose, nameof (NativeAttachment), "SaveConvertedFile: " + filepath);
      this.convertedFile = filepath;
    }

    public string GetRotatedFile()
    {
      Tracing.Log(NativeAttachment.sw, TraceLevel.Verbose, nameof (NativeAttachment), "GetRotatedFile: " + this.id);
      return this.rotatedFile != null && File.Exists(this.rotatedFile) ? this.rotatedFile : (string) null;
    }

    public void SetRotatedFile(string filepath)
    {
      Tracing.Log(NativeAttachment.sw, TraceLevel.Verbose, nameof (NativeAttachment), "SaveRotatedFile: " + filepath);
      this.rotatedFile = filepath;
    }

    public override void ToXml(XmlElement elm)
    {
      base.ToXml(elm);
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("Filename", (object) this.id);
      attributeWriter.Write("Rotation", (object) this.rotation);
      attributeWriter.Write("FileSize", (object) this.fileSize);
      if (!string.IsNullOrEmpty(this.ObjectId))
        attributeWriter.Write("ObjectId", (object) this.ObjectId);
      foreach (string source in this.sourceList)
      {
        XmlElement element = elm.OwnerDocument.CreateElement("Source");
        elm.AppendChild((XmlNode) element);
        new AttributeWriter(element).Write("Filename", (object) source);
      }
    }

    public override XmlElement ToXml()
    {
      XmlElement element = new XmlDocument().CreateElement("File");
      this.ToXml(element);
      return element;
    }
  }
}
