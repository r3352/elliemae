// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.OtherTransactionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class OtherTransactionLog : ServicingTransactionBase
  {
    private string institutionName = string.Empty;
    private string institutionRouting = string.Empty;
    private string accountNumber = string.Empty;
    private string reference = string.Empty;
    private string comments = string.Empty;

    public OtherTransactionLog() => this.TransactionType = ServicingTransactionTypes.Other;

    public OtherTransactionLog(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.institutionName = attributeReader.GetString(nameof (InstitutionName));
      this.institutionRouting = attributeReader.GetString(nameof (InstitutionRouting));
      this.accountNumber = attributeReader.GetString(nameof (AccountNumber));
      this.reference = attributeReader.GetString(nameof (Reference));
      this.comments = attributeReader.GetString(nameof (Comments));
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("Type", ServicingTransactionTypes.Other.ToString());
      newlog.SetAttribute("InstitutionName", this.institutionName);
      newlog.SetAttribute("InstitutionRouting", this.institutionRouting);
      newlog.SetAttribute("AccountNumber", this.accountNumber);
      newlog.SetAttribute("Reference", this.reference);
      newlog.SetAttribute("Comments", this.comments);
    }

    public string InstitutionName
    {
      get => this.institutionName;
      set => this.institutionName = value;
    }

    public string InstitutionRouting
    {
      get => this.institutionRouting;
      set => this.institutionRouting = value;
    }

    public string AccountNumber
    {
      get => this.accountNumber;
      set => this.accountNumber = value;
    }

    public string Reference
    {
      get => this.reference;
      set => this.reference = value;
    }

    public string Comments
    {
      get => this.comments;
      set => this.comments = value;
    }
  }
}
