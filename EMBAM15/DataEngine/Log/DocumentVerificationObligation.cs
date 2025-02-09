// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentVerificationObligation
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentVerificationObligation : DocumentVerificationType
  {
    private ObligationType obligationType;
    private string otherDescription = string.Empty;
    private string mortageLateCount = string.Empty;
    private Decimal amount;

    public DocumentVerificationObligation(LoanBorrowerType borrowerType)
      : base(VerificationTimelineType.Obligation, borrowerType)
    {
    }

    public DocumentVerificationObligation(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.obligationType = Helper.GetObligationType(attributeReader.GetString(nameof (ObligationType)));
      this.otherDescription = attributeReader.GetString(nameof (OtherDescription));
      this.mortageLateCount = attributeReader.GetString(nameof (MortageLateCount));
      this.amount = attributeReader.GetDecimal(nameof (Amount), 0M);
    }

    public ObligationType ObligationType
    {
      set => this.obligationType = value;
      get => this.obligationType;
    }

    public string MortageLateCount
    {
      set
      {
        if (!this.obligationType.Equals((object) ObligationType.MortgageLate))
          throw new Exception("Obligation Type is not MortgageLate");
        this.mortageLateCount = value;
      }
      get => this.mortageLateCount;
    }

    public string OtherDescription
    {
      set
      {
        if (!this.obligationType.Equals((object) ObligationType.OtherMonthlyObligation) && !this.obligationType.Equals((object) ObligationType.OtherCreditHistory))
          throw new Exception("Obligation Type is not Other");
        this.otherDescription = value;
      }
      get => this.otherDescription;
    }

    public Decimal Amount
    {
      set
      {
        if (!this.obligationType.Equals((object) ObligationType.HELOC) && !this.obligationType.Equals((object) ObligationType.SecondLien))
          throw new Exception("Obligation Type is not HELOC or SecondLien");
        this.amount = value;
      }
      get => this.amount;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("ObligationType", (object) this.obligationType);
      if (this.obligationType.Equals((object) ObligationType.OtherMonthlyObligation) || this.obligationType.Equals((object) ObligationType.OtherCreditHistory))
        attributeWriter.Write("OtherDescription", (object) this.otherDescription);
      if (this.obligationType.Equals((object) ObligationType.MortgageLate))
        attributeWriter.Write("MortageLateCount", (object) this.mortageLateCount);
      if (!this.obligationType.Equals((object) ObligationType.HELOC) && !this.obligationType.Equals((object) ObligationType.SecondLien))
        return;
      attributeWriter.Write("Amount", (object) this.amount);
    }
  }
}
