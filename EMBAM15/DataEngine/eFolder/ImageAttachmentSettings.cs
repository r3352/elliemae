// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.ImageAttachmentSettings
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class ImageAttachmentSettings : IXmlSerializable
  {
    private bool useImageAttachments;
    private ImageConversionType conversionType;
    private int dpiX;
    private int dpiY;
    private bool saveOriginalFormat;

    public ImageAttachmentSettings()
    {
      this.useImageAttachments = false;
      this.conversionType = ImageConversionType.Automatic;
      this.dpiX = 200;
      this.dpiY = 200;
      this.saveOriginalFormat = false;
    }

    public ImageAttachmentSettings(XmlSerializationInfo info)
    {
      this.useImageAttachments = info.GetBoolean(nameof (UseImageAttachments), false);
      this.conversionType = (ImageConversionType) info.GetValue(nameof (ConversionType), typeof (ImageConversionType), (object) ImageConversionType.Automatic);
      this.dpiX = info.GetInteger(nameof (DpiX), 200);
      this.dpiY = info.GetInteger(nameof (DpiY), 200);
      this.saveOriginalFormat = info.GetBoolean(nameof (SaveOriginalFormat), false);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("UseImageAttachments", (object) this.useImageAttachments);
      info.AddValue("ConversionType", (object) this.conversionType);
      info.AddValue("DpiX", (object) this.dpiX);
      info.AddValue("DpiY", (object) this.dpiY);
      info.AddValue("SaveOriginalFormat", (object) this.saveOriginalFormat);
    }

    public bool UseImageAttachments
    {
      get => this.useImageAttachments;
      set => this.useImageAttachments = value;
    }

    public ImageConversionType ConversionType
    {
      get => this.conversionType;
      set => this.conversionType = value;
    }

    public int DpiX
    {
      get => this.dpiX;
      set => this.dpiX = value;
    }

    public int DpiY
    {
      get => this.dpiY;
      set => this.dpiY = value;
    }

    public bool SaveOriginalFormat
    {
      get => this.saveOriginalFormat;
      set => this.saveOriginalFormat = value;
    }
  }
}
