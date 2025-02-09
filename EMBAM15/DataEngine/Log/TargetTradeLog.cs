// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.TargetTradeLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class TargetTradeLog : LogRecordBase
  {
    public static readonly string XmlType = "TargetTrade";
    private string _commitmentId;
    private Decimal _basePrice;
    private Decimal _SRPPaidOut;
    private PriceAdjustmentList _priceAdjustments = new PriceAdjustmentList();

    public TargetTradeLog(
      DateTime creationDate,
      string tradeId,
      params PriceAdjustmentLogRecord[] priceAdjusments)
      : base(creationDate, "Loan targeted for trade: " + tradeId)
    {
      this._commitmentId = tradeId;
      if (priceAdjusments.Length > 20)
        throw new ArgumentOutOfRangeException(nameof (priceAdjusments), "Cannot specify more than 20 price adjustments");
      int idx;
      for (idx = 0; idx < priceAdjusments.Length; ++idx)
        this._priceAdjustments[idx] = PriceAdjustmentLogRecord.Copy(priceAdjusments[idx]);
      for (; idx < 20; ++idx)
        this._priceAdjustments[idx] = (PriceAdjustmentLogRecord) null;
    }

    public TargetTradeLog()
      : base(DateTime.Now, "Loan targeted for trade")
    {
      this._commitmentId = (string) null;
      this.date = DateTime.Now;
      for (int idx = 0; idx < 20; ++idx)
        this._priceAdjustments[idx] = (PriceAdjustmentLogRecord) null;
    }

    public TargetTradeLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.Date = attributeReader.GetDate("Date");
      this._commitmentId = attributeReader.GetString("CommitmentID");
      this._basePrice = attributeReader.GetDecimal(nameof (BasePrice), 0M);
      this.SRPPaidOut = attributeReader.GetDecimal(nameof (SRPPaidOut), 0M);
      XmlElement e1 = (XmlElement) e.SelectSingleNode("PriceAdjustments");
      if (e1 != null)
        this._priceAdjustments = new PriceAdjustmentList(e1);
      this.MarkAsClean();
    }

    public string CommitmentNum
    {
      get => this._commitmentId;
      set => this._commitmentId = value;
    }

    public Decimal BasePrice
    {
      get => this._basePrice;
      set => this._basePrice = value;
    }

    public Decimal SRPPaidOut
    {
      get => this._SRPPaidOut;
      set => this._SRPPaidOut = value;
    }

    public PriceAdjustmentLogRecord this[int idx]
    {
      get
      {
        return idx >= 1 && idx <= 20 ? this._priceAdjustments[idx - 1] : throw new ArgumentOutOfRangeException(nameof (idx), "Idx must be betwwen 1 and 20 inclusive");
      }
      set
      {
        if (idx < 1 || idx > 20)
          throw new ArgumentOutOfRangeException(nameof (idx), "Idx must be betwwen 1 and 20 inclusive");
        this._priceAdjustments[idx - 1] = value;
      }
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) TargetTradeLog.XmlType);
      attributeWriter.Write("Date", (object) this.Date);
      attributeWriter.Write("BasePrice", (object) this._basePrice);
      attributeWriter.Write("SRPPaidOut", (object) this._SRPPaidOut);
      attributeWriter.Write("CommitmentID", (object) this._commitmentId);
      this._priceAdjustments.ToXml((XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement("PriceAdjustments")));
    }
  }
}
