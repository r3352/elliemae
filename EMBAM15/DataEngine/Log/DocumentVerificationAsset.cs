// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentVerificationAsset
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentVerificationAsset : DocumentVerificationType
  {
    private AssetType assetType;
    private string otherDescription = string.Empty;

    public DocumentVerificationAsset(LoanBorrowerType borrowerType)
      : base(VerificationTimelineType.Asset, borrowerType)
    {
    }

    public DocumentVerificationAsset(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.assetType = Helper.GetAssetType(attributeReader.GetString(nameof (AssetType)));
      this.otherDescription = attributeReader.GetString(nameof (OtherDescription));
    }

    public AssetType AssetType
    {
      set => this.assetType = value;
      get => this.assetType;
    }

    public string OtherDescription
    {
      set
      {
        if (!this.assetType.Equals((object) AssetType.Other))
          throw new Exception("AssetType is not Other");
        this.otherDescription = value;
      }
      get => this.otherDescription;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("AssetType", (object) this.assetType);
      if (!this.assetType.Equals((object) AssetType.Other))
        return;
      attributeWriter.Write("OtherDescription", (object) this.otherDescription);
    }
  }
}
