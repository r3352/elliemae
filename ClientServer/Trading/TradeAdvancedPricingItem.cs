// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAdvancedPricingItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradeAdvancedPricingItem : TradeEntity, IXmlSerializable
  {
    private Decimal noteRate;
    private Decimal buyUp;
    private Decimal buyDown;
    private Decimal gnmaIIExcess;
    private Decimal mandAdj;
    private Decimal servicingRetained;
    private string gseContractNumber = "";
    private string productName = "";
    private Decimal servicingFee;
    private Decimal guarantyFee;

    public TradeAdvancedPricingItem()
    {
    }

    public TradeAdvancedPricingItem(
      Decimal noteRate,
      Decimal buyUp,
      Decimal buyDown,
      Decimal gnmaIIExcess,
      Decimal mandAdj,
      Decimal servicingRetained,
      string gseCommitmentNumber,
      string productName)
    {
      this.noteRate = noteRate;
      this.buyUp = buyUp;
      this.buyDown = buyDown;
      this.gnmaIIExcess = gnmaIIExcess;
      this.mandAdj = mandAdj;
      this.servicingRetained = servicingRetained;
      this.gseContractNumber = gseCommitmentNumber;
      this.productName = productName;
    }

    public TradeAdvancedPricingItem(TradeAdvancedPricingItem source)
    {
      this.noteRate = source.noteRate;
      this.buyUp = source.buyUp;
      this.buyDown = source.buyDown;
      this.gnmaIIExcess = source.gnmaIIExcess;
      this.mandAdj = source.mandAdj;
      this.servicingRetained = source.servicingRetained;
      this.gseContractNumber = source.gseContractNumber;
      this.productName = source.productName;
      this.servicingFee = source.servicingFee;
      this.guarantyFee = source.guarantyFee;
    }

    public TradeAdvancedPricingItem(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.noteRate = info.GetDecimal(nameof (NoteRate));
      this.buyUp = info.GetDecimal(nameof (BuyUp));
      this.buyDown = info.GetDecimal(nameof (BuyDown));
      this.gnmaIIExcess = info.GetDecimal(nameof (GNMAIIExcess));
      this.mandAdj = info.GetDecimal(nameof (MandAdj));
      try
      {
        this.servicingRetained = info.GetDecimal(nameof (ServicingRetained));
      }
      catch
      {
        this.servicingRetained = 0M;
      }
      this.gseContractNumber = info.GetString("GSECommitmentNumber", "");
      this.productName = info.GetString(nameof (ProductName), "");
      try
      {
        this.guarantyFee = info.GetDecimal(nameof (GuarantyFee));
      }
      catch
      {
        this.guarantyFee = 0M;
      }
      try
      {
        this.servicingFee = info.GetDecimal(nameof (ServicingFee));
      }
      catch
      {
        this.servicingFee = 0M;
      }
    }

    public Decimal NoteRate
    {
      get => this.noteRate;
      set => this.noteRate = value;
    }

    public Decimal BuyUp
    {
      get => this.buyUp;
      set => this.buyUp = value;
    }

    public Decimal BuyDown
    {
      get => this.buyDown;
      set => this.buyDown = value;
    }

    public Decimal GNMAIIExcess
    {
      get => this.gnmaIIExcess;
      set => this.gnmaIIExcess = value;
    }

    public Decimal MandAdj
    {
      get => this.mandAdj;
      set => this.mandAdj = value;
    }

    public Decimal ServicingRetained
    {
      get => this.servicingRetained;
      set => this.servicingRetained = value;
    }

    public string GSEContractNumber
    {
      get => this.gseContractNumber;
      set => this.gseContractNumber = value;
    }

    public string ProductName
    {
      get => this.productName;
      set => this.productName = value;
    }

    public Decimal TotalPrice => this.buyUp + this.buyDown + this.gnmaIIExcess + this.mandAdj;

    public Decimal ServicingFee
    {
      get => this.servicingFee;
      set => this.servicingFee = value;
    }

    public Decimal GuarantyFee
    {
      get => this.guarantyFee;
      set => this.guarantyFee = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("NoteRate", (object) this.noteRate);
      info.AddValue("BuyUp", (object) this.buyUp);
      info.AddValue("BuyDown", (object) this.buyDown);
      info.AddValue("GNMAIIExcess", (object) this.gnmaIIExcess);
      info.AddValue("MandAdj", (object) this.mandAdj);
      info.AddValue("ServicingRetained", (object) this.servicingRetained);
      info.AddValue("GSECommitmentNumber", (object) this.gseContractNumber);
      info.AddValue("ProductName", (object) this.productName);
      info.AddValue("GuarantyFee", (object) this.guarantyFee);
      info.AddValue("ServicingFee", (object) this.servicingFee);
    }
  }
}
