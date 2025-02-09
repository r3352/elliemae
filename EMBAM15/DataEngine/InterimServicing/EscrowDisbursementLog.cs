// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.EscrowDisbursementLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class EscrowDisbursementLog : ServicingTransactionBase
  {
    private int disbursementNo;
    private DateTime disbursementDueDate = DateTime.MinValue;
    private ServicingDisbursementTypes disbursementType;
    private string institutionName = string.Empty;
    private string comments = string.Empty;

    public EscrowDisbursementLog()
    {
    }

    public EscrowDisbursementLog(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.disbursementDueDate = attributeReader.GetDate(nameof (DisbursementDueDate));
      this.disbursementNo = attributeReader.GetInteger("DisbursementNumber");
      this.disbursementType = ServicingEnum.DisbursementTypesToEnum(attributeReader.GetString(nameof (DisbursementType)));
      this.institutionName = attributeReader.GetString(nameof (InstitutionName));
      this.comments = attributeReader.GetString(nameof (Comments));
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("Type", ServicingTransactionTypes.EscrowDisbursement.ToString());
      newlog.SetAttribute("DisbursementNumber", this.disbursementNo.ToString());
      newlog.SetAttribute("DisbursementDueDate", this.disbursementDueDate.ToString("MM/dd/yyyy"));
      newlog.SetAttribute("DisbursementType", ServicingEnum.DisbursementTypesToUI(this.disbursementType));
      newlog.SetAttribute("InstitutionName", this.institutionName);
      newlog.SetAttribute("Comments", this.comments);
    }

    public int DisbursementNo
    {
      get => this.disbursementNo;
      set => this.disbursementNo = value;
    }

    public DateTime DisbursementDueDate
    {
      get => this.disbursementDueDate;
      set => this.disbursementDueDate = value;
    }

    public ServicingDisbursementTypes DisbursementType
    {
      get => this.disbursementType;
      set => this.disbursementType = value;
    }

    public string InstitutionName
    {
      get => this.institutionName;
      set => this.institutionName = value;
    }

    public string Comments
    {
      get => this.comments;
      set => this.comments = value;
    }
  }
}
