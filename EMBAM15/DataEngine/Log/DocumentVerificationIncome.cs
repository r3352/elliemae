// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentVerificationIncome
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentVerificationIncome : DocumentVerificationType
  {
    private IncomeType incomeType;
    private int taxReturnYear;
    private string otherDescription = string.Empty;

    public DocumentVerificationIncome(LoanBorrowerType borrowerType)
      : base(VerificationTimelineType.Income, borrowerType)
    {
    }

    public DocumentVerificationIncome(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.incomeType = Helper.GetIncomeType(attributeReader.GetString(nameof (IncomeType)));
      this.taxReturnYear = attributeReader.GetInteger(nameof (TaxReturnYear));
      this.otherDescription = attributeReader.GetString(nameof (OtherDescription));
    }

    public IncomeType IncomeType
    {
      set => this.incomeType = value;
      get => this.incomeType;
    }

    public int TaxReturnYear
    {
      set
      {
        if (!this.incomeType.Equals((object) IncomeType.TaxReturn))
          throw new Exception("IncomeType is not TaxReturn");
        this.taxReturnYear = value;
      }
      get => this.taxReturnYear;
    }

    public string OtherDescription
    {
      set
      {
        if (!this.incomeType.Equals((object) IncomeType.OtherEmployment) && !this.incomeType.Equals((object) IncomeType.OtherNonEmployment))
          throw new Exception("IncomeType is not Other");
        this.otherDescription = value;
      }
      get => this.otherDescription;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("IncomeType", (object) this.incomeType);
      if (this.incomeType.Equals((object) IncomeType.TaxReturn))
        attributeWriter.Write("TaxReturnYear", (object) this.taxReturnYear);
      if (!this.incomeType.Equals((object) IncomeType.OtherEmployment) && !this.incomeType.Equals((object) IncomeType.OtherNonEmployment))
        return;
      attributeWriter.Write("OtherDescription", (object) this.otherDescription);
    }
  }
}
