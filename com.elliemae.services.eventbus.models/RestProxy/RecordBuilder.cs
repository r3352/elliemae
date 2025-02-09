// Decompiled with JetBrains decompiler
// Type: com.elliemae.services.eventbus.models.RestProxy.RecordBuilder
// Assembly: com.elliemae.services.eventbus.models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 9B148EFB-427E-4DF5-8EA2-5C9491D22624
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\com.elliemae.services.eventbus.models.dll

#nullable disable
namespace com.elliemae.services.eventbus.models.RestProxy
{
  public class RecordBuilder
  {
    public string Key;
    public Event Value;

    public Record Build()
    {
      return new Record()
      {
        Key = this.Key,
        Value = this.Value
      };
    }

    public override string ToString()
    {
      return "Record.RecordBuilder(key=" + this.Key + ", value=" + (object) this.Value + ")";
    }
  }
}
