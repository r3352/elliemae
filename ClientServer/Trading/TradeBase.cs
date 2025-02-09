// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeBase
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public abstract class TradeBase
  {
    private TradeType tradeType;

    public TradeBase()
    {
    }

    public TradeBase(TradeType tradeType) => this.tradeType = tradeType;

    public TradeType TradeType
    {
      get => this.tradeType;
      set
      {
        this.tradeType = value != TradeType.None ? value : throw new InvalidOperationException("Invalid trade type.");
      }
    }

    public abstract int TradeID { get; set; }

    public abstract string Guid { get; set; }

    public abstract string Name { get; set; }
  }
}
