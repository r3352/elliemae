// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.PairOff
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class PairOff : TradeEntity, IXmlSerializable
  {
    private int index = -1;
    private DateTime date = DateTime.MinValue;
    private Decimal undeliveredAmt;
    private Decimal fee;
    private bool locked;

    public PairOff()
    {
    }

    public PairOff(int index, DateTime date, Decimal undeliveredAmt)
    {
      this.index = index;
      this.date = date;
      this.undeliveredAmt = undeliveredAmt;
    }

    public PairOff(PairOff source)
    {
      this.index = source.Index;
      this.date = source.date;
      this.undeliveredAmt = source.undeliveredAmt;
    }

    public PairOff(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.index = info.GetInteger(nameof (index));
      this.date = info.GetDateTime(nameof (date));
      this.undeliveredAmt = info.GetDecimal("amt");
      this.fee = info.GetDecimal(nameof (fee));
      this.locked = info.GetBoolean(nameof (locked));
    }

    public int Index => this.index;

    public DateTime Date
    {
      get => this.date;
      set => this.date = value;
    }

    public Decimal UndeliveredAmount
    {
      get => this.undeliveredAmt;
      set => this.undeliveredAmt = value;
    }

    public Decimal Fee
    {
      get => !this.locked ? 0M : this.fee;
      set => this.fee = value;
    }

    public bool Locked
    {
      get => this.locked;
      set => this.locked = value;
    }

    internal void SetIndex(int index) => this.index = index;

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("index", (object) this.index);
      info.AddValue("date", (object) this.date);
      info.AddValue("amt", (object) this.undeliveredAmt);
      info.AddValue("fee", (object) this.Fee);
      info.AddValue("locked", (object) this.locked);
    }
  }
}
