// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MasterCommitmentDeliveryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MasterCommitmentDeliveryInfo
  {
    public int ID { get; set; }

    public CorrespondentMasterDeliveryType Type { get; set; }

    public Decimal Tolerance { get; set; }

    public DateTime EffectiveDateTime { get; set; }

    public DateTime ExpireDateTime { get; set; }

    public int DeliveryDays { get; set; }

    public MasterCommitmentDeliveryInfo()
    {
    }

    public MasterCommitmentDeliveryInfo(
      int id,
      CorrespondentMasterDeliveryType type,
      Decimal tolerance,
      DateTime effectiveDateTime,
      DateTime expireDateTime,
      int deliveryDays)
    {
      this.ID = id;
      this.Type = type;
      this.EffectiveDateTime = effectiveDateTime;
      this.ExpireDateTime = this.ExpireDateTime;
      this.Tolerance = tolerance;
      this.DeliveryDays = deliveryDays;
    }
  }
}
