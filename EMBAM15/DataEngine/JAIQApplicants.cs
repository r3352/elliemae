// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.JAIQApplicants
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class JAIQApplicants
  {
    public string id { get; set; }

    public JAIQApplicantName name { get; set; }

    public string taxpayerIdentifier { get; set; }

    public string last4TaxIdentifier
    {
      get
      {
        return this.taxpayerIdentifier.Length >= 4 ? this.taxpayerIdentifier.Substring(this.taxpayerIdentifier.Length - 4) : this.taxpayerIdentifier;
      }
    }
  }
}
