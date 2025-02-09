// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.LoanPurchaseLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class LoanPurchaseLog : ServicingTransactionBase
  {
    private DateTime _purchaseAdviceDate = DateTime.MinValue;
    private string _investor = string.Empty;
    private string _investorLoanNumber = string.Empty;
    private DateTime _firstPaymenttoInvestor = DateTime.MinValue;
    private double _purchaseAmount;
    private DateTime _createdTime = DateTime.MinValue;

    public LoanPurchaseLog() => this.TransactionType = ServicingTransactionTypes.PurchaseAdvice;

    public LoanPurchaseLog(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this._purchaseAdviceDate = attributeReader.GetDate(nameof (PurchaseAdviceDate));
      this._investor = attributeReader.GetString(nameof (Investor));
      this._investorLoanNumber = attributeReader.GetString(nameof (InvestorLoanNumber));
      this._firstPaymenttoInvestor = attributeReader.GetDate(nameof (FirstPaymenttoInvestor));
      this._purchaseAmount = attributeReader.GetDouble(nameof (PurchaseAmount), 0.0);
      this._createdTime = attributeReader.GetDate(nameof (CreatedTime));
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("Type", ServicingTransactionTypes.PurchaseAdvice.ToString());
      newlog.SetAttribute("PurchaseAdviceDate", this._purchaseAdviceDate.ToString("MM/dd/yyyy"));
      newlog.SetAttribute("Investor", this._investor);
      newlog.SetAttribute("InvestorLoanNumber", this._investorLoanNumber);
      newlog.SetAttribute("FirstPaymenttoInvestor", this._firstPaymenttoInvestor.ToString("MM/dd/yyyy"));
      newlog.SetAttribute("PurchaseAmount", this._purchaseAmount.ToString("N2"));
      newlog.SetAttribute("CreatedTime", this._createdTime.ToString("MM/dd/yyyy HH:mm:ss"));
    }

    public DateTime PurchaseAdviceDate
    {
      get => this._purchaseAdviceDate;
      set => this._purchaseAdviceDate = value;
    }

    public string Investor
    {
      get => this._investor;
      set => this._investor = value;
    }

    public string InvestorLoanNumber
    {
      get => this._investorLoanNumber;
      set => this._investorLoanNumber = value;
    }

    public DateTime FirstPaymenttoInvestor
    {
      get => this._firstPaymenttoInvestor;
      set => this._firstPaymenttoInvestor = value;
    }

    public double PurchaseAmount
    {
      get => this._purchaseAmount;
      set => this._purchaseAmount = value;
    }

    public DateTime CreatedTime
    {
      get => this._createdTime;
      set => this._createdTime = value;
    }
  }
}
