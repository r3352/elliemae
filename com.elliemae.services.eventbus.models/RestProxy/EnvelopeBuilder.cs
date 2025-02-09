// Decompiled with JetBrains decompiler
// Type: com.elliemae.services.eventbus.models.RestProxy.EnvelopeBuilder
// Assembly: com.elliemae.services.eventbus.models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 9B148EFB-427E-4DF5-8EA2-5C9491D22624
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\com.elliemae.services.eventbus.models.dll

using System.Collections.Generic;

#nullable disable
namespace com.elliemae.services.eventbus.models.RestProxy
{
  public class EnvelopeBuilder
  {
    public List<Record> Records { get; set; }

    public Envelope Build()
    {
      return new Envelope() { Records = this.Records };
    }

    public override string ToString()
    {
      return "Envelope.EnvelopeBuilder(records=" + (object) this.Records + ")";
    }
  }
}
