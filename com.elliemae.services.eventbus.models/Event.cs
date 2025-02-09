// Decompiled with JetBrains decompiler
// Type: com.elliemae.services.eventbus.models.Event
// Assembly: com.elliemae.services.eventbus.models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 9B148EFB-427E-4DF5-8EA2-5C9491D22624
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\com.elliemae.services.eventbus.models.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace com.elliemae.services.eventbus.models
{
  public class Event
  {
    public string CreatedDate = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");
    public string EnvelopeVersion = "1.0.0";
    public string PayloadVersion = "1.0.0";
    public Dictionary<string, object> Payload;

    public string Id { get; set; }

    public string CorrelationId { get; set; }

    public string Category { get; set; }

    public string Source { get; set; }

    public string Type { get; set; }

    public string Tenant { get; set; }

    public string EntityId { get; set; }

    public static EventBuilder Builder() => new EventBuilder();
  }
}
