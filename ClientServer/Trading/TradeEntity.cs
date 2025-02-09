// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeEntity
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public abstract class TradeEntity
  {
    private Guid _id;

    public virtual Guid Id
    {
      get => this._id;
      set => this._id = value;
    }

    public TradeEntity()
    {
      if (!(this._id == Guid.Empty))
        return;
      this._id = Guid.NewGuid();
    }

    public void ReadId(XmlSerializationInfo info)
    {
      try
      {
        Guid guid = info.GetValue<Guid>("id");
        this.Id = guid == Guid.Empty ? Guid.NewGuid() : guid;
      }
      catch
      {
      }
    }

    public void WriteId(XmlSerializationInfo info) => info.AddValue("id", (object) this.Id);
  }
}
