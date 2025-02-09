// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.PrincipalDisbursementLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class PrincipalDisbursementLog : ServicingTransactionBase
  {
    private DateTime disbursementDate = DateTime.MinValue;
    private string institutionName = string.Empty;
    private string comments = string.Empty;

    public PrincipalDisbursementLog()
    {
      this.TransactionType = ServicingTransactionTypes.PrincipalDisbursement;
    }

    public PrincipalDisbursementLog(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.disbursementDate = attributeReader.GetDate(nameof (DisbursementDate));
      this.institutionName = attributeReader.GetString(nameof (InstitutionName));
      this.comments = attributeReader.GetString(nameof (Comments));
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("Type", ServicingTransactionTypes.PrincipalDisbursement.ToString());
      newlog.SetAttribute("DisbursementDate", this.disbursementDate.ToString("MM/dd/yyyy"));
      newlog.SetAttribute("InstitutionName", this.institutionName);
      newlog.SetAttribute("Comments", this.comments);
    }

    public DateTime DisbursementDate
    {
      get => this.disbursementDate;
      set => this.disbursementDate = value;
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
