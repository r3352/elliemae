// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.BackgroundAttachment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class BackgroundAttachment : FileAttachment
  {
    private string extension;
    private bool documentConversionOn;
    private ImageConversionType conversionType = ImageConversionType.None;
    private bool keepNativeCopy;
    private int dpiX;
    private int dpiY;
    private List<DocumentIdentity> identityList = new List<DocumentIdentity>();

    public BackgroundAttachment(
      string title,
      string fileExtension,
      string userId,
      string userName,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList)
      : base(sourceList)
    {
      this.id = "Attachment-" + Guid.NewGuid().ToString() + fileExtension;
      this.extension = fileExtension;
      this.title = title;
      this.userId = userId;
      this.userName = userName;
      this.documentConversionOn = false;
      if (identityList == null)
        return;
      this.Identities.AddRange(identityList);
    }

    public BackgroundAttachment(
      string title,
      string fileExtension,
      string userId,
      string userName,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList,
      ImageConversionType conversionType,
      bool keepNativeCopy,
      int dpiX,
      int dpiY)
      : base(sourceList)
    {
      this.id = Guid.NewGuid().ToString();
      this.extension = fileExtension;
      this.title = title;
      this.userId = userId;
      this.userName = userName;
      this.documentConversionOn = true;
      this.conversionType = conversionType;
      this.keepNativeCopy = keepNativeCopy;
      this.dpiX = dpiX;
      this.dpiY = dpiY;
      if (identityList == null)
        return;
      this.Identities.AddRange(identityList);
    }

    public BackgroundAttachment(FileAttachment attachment)
      : base(attachment)
    {
    }

    public BackgroundAttachment(XmlElement elm, bool isRemoved)
      : base(elm, isRemoved)
    {
      AttributeReader attributeReader = new AttributeReader(elm);
      this.id = attributeReader.GetString("ID");
      this.extension = attributeReader.GetString(nameof (Extension));
      this.documentConversionOn = attributeReader.GetBoolean(nameof (DocumentConversionOn));
      this.keepNativeCopy = attributeReader.GetBoolean(nameof (KeepNativeCopy));
      this.conversionType = (ImageConversionType) Enum.Parse(typeof (ImageConversionType), attributeReader.GetString(nameof (ConversionType)));
      this.dpiX = attributeReader.GetInteger(nameof (DpiX));
      this.dpiY = attributeReader.GetInteger(nameof (DpiY));
      foreach (XmlElement selectNode in elm.SelectNodes("Source"))
        this.sourceList.Add(new AttributeReader(selectNode).GetString("ID"));
      this.identities = new DocumentIdentityCollection((FileAttachment) this, elm);
    }

    public BackgroundAttachment(
      string id,
      string title,
      string fileExtension,
      string userId,
      string userName,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList)
      : this(title, fileExtension, userId, userName, sourceList, identityList)
    {
      if (string.IsNullOrEmpty(id))
        return;
      this.id = id;
    }

    public BackgroundAttachment(
      string id,
      string title,
      string fileExtension,
      string userId,
      string userName,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList,
      ImageConversionType conversionType,
      bool keepNativeCopy,
      int dpiX,
      int dpiY)
      : this(title, fileExtension, userId, userName, sourceList, identityList, conversionType, keepNativeCopy, dpiX, dpiY)
    {
      if (string.IsNullOrEmpty(id))
        return;
      this.id = id;
    }

    public bool DocumentConversionOn => this.documentConversionOn;

    public bool KeepNativeCopy => this.keepNativeCopy;

    public ImageConversionType ConversionType => this.conversionType;

    public override long FileSize => 0;

    public string Extension => this.extension;

    public int DpiX => this.dpiX;

    public int DpiY => this.dpiY;

    public override void ToXml(XmlElement elm)
    {
      base.ToXml(elm);
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("ID", (object) this.id);
      attributeWriter.Write("Extension", (object) this.extension);
      attributeWriter.Write("DocumentConversionOn", (object) this.documentConversionOn);
      attributeWriter.Write("KeepNativeCopy", (object) this.keepNativeCopy);
      attributeWriter.Write("ConversionType", (object) this.conversionType.ToString());
      attributeWriter.Write("DpiX", (object) this.dpiX);
      attributeWriter.Write("DpiY", (object) this.dpiY);
      foreach (string source in this.sourceList)
      {
        XmlElement element = elm.OwnerDocument.CreateElement("Source");
        elm.AppendChild((XmlNode) element);
        new AttributeWriter(element).Write("ID", (object) source);
      }
      this.Identities.ToXml(elm);
    }

    public override XmlElement ToXml()
    {
      XmlElement element = new XmlDocument().CreateElement("Background");
      this.ToXml(element);
      return element;
    }
  }
}
