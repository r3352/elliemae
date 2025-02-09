// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.DeferrableDynamicProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public class DeferrableDynamicProcessor : IDeferrableProcessor
  {
    private readonly string _key;

    public DeferrableDynamicProcessor(string key) => this._key = key;

    public Action ProcessorHandler { get; set; }

    public void Execute()
    {
      if (this.ProcessorHandler == null)
        return;
      this.ProcessorHandler();
    }

    public string GetKey() => this._key;

    public DeferrableType GetDeferrableType() => DeferrableType.RealTime;
  }
}
