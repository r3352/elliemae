// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MasterCommitmentBase
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public abstract class MasterCommitmentBase : TradeBase, ITradeInfoObject, IPropertyDictionary
  {
    private readonly bool _isCorrespondent;
    private TradeCalculation _calculation;
    private int tradeID = -1;
    private string guid = string.Empty;
    private string name = string.Empty;

    public bool IsCorrespondent => this._isCorrespondent;

    public int ID { get; set; }

    public Decimal CommitmentAmount { get; set; }

    public abstract MasterCommitmentStatus Status { get; set; }

    public List<MasterCommitmentDeliveryInfo> DeliveryInfos { get; set; }

    public int TradeCount { get; set; }

    public Decimal AssignedAmount { get; set; }

    public Decimal TotalProfit { get; set; }

    public Decimal CompletionPercent { get; set; }

    public override int TradeID
    {
      get => this.tradeID;
      set => this.tradeID = value;
    }

    public override string Guid
    {
      get => string.IsNullOrEmpty(this.guid) ? System.Guid.NewGuid().ToString() : this.guid;
      set => this.guid = value;
    }

    public override string Name
    {
      get => this.name;
      set => this.name = value;
    }

    protected MasterCommitmentBase(bool isCorrespondent)
    {
      this._isCorrespondent = isCorrespondent;
      this.DeliveryInfos = new List<MasterCommitmentDeliveryInfo>();
      this.ID = -1;
    }

    public virtual TradeCalculation Calculation
    {
      get
      {
        return this._calculation ?? (this._calculation = new TradeCalculation((ITradeInfoObject) this));
      }
    }

    public object this[string propertyName]
    {
      get => this.GetType().GetProperty(propertyName).GetValue((object) this, (object[]) null);
      set
      {
        this.GetType().GetProperty(propertyName).SetValue((object) this, value, (object[]) null);
      }
    }

    public override string ToString() => this.Name;
  }
}
