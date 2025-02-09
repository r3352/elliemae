// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.PaymentReversalLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class PaymentReversalLog : ServicingTransactionBase
  {
    private string paymentGUID = string.Empty;
    private ServicingTransactionTypes reversalType;

    public PaymentReversalLog() => this.TransactionType = ServicingTransactionTypes.PaymentReversal;

    public PaymentReversalLog(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.paymentGUID = attributeReader.GetString(nameof (PaymentGUID));
      this.reversalType = (ServicingTransactionTypes) ServicingEnum.ToEnum(attributeReader.GetString(nameof (ReversalType)), typeof (ServicingTransactionTypes));
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("Type", ServicingTransactionTypes.PaymentReversal.ToString());
      newlog.SetAttribute("PaymentGUID", this.paymentGUID);
      newlog.SetAttribute("ReversalType", this.reversalType.ToString());
    }

    public string PaymentGUID
    {
      get => this.paymentGUID;
      set => this.paymentGUID = value;
    }

    public ServicingTransactionTypes ReversalType
    {
      get => this.reversalType;
      set => this.reversalType = value;
    }
  }
}
