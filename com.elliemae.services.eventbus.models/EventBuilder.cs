// Decompiled with JetBrains decompiler
// Type: com.elliemae.services.eventbus.models.EventBuilder
// Assembly: com.elliemae.services.eventbus.models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 9B148EFB-427E-4DF5-8EA2-5C9491D22624
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\com.elliemae.services.eventbus.models.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace com.elliemae.services.eventbus.models
{
  public class EventBuilder
  {
    private bool isCreatedDateSet;
    private DateTime createdDate;
    private bool isEnvelopeVersionSet;
    private string envelopeVersion;
    private bool isPayloadVersionSet;
    private string payloadVersion;

    public string Id { get; set; }

    public string CorrelationId { get; set; }

    public string Category { get; set; }

    public string Source { get; set; }

    public string Type { get; set; }

    public string Tenant { get; set; }

    public string EntityId { get; set; }

    public DateTime CreatedDate
    {
      get => this.createdDate;
      set
      {
        this.createdDate = value;
        this.isCreatedDateSet = true;
      }
    }

    public string EnvelopeVersion
    {
      get => this.envelopeVersion;
      set
      {
        this.envelopeVersion = value;
        this.isEnvelopeVersionSet = true;
      }
    }

    public string PayloadVersion
    {
      get => this.payloadVersion;
      set
      {
        this.payloadVersion = value;
        this.isPayloadVersionSet = true;
      }
    }

    public Dictionary<string, object> Payload { get; set; }

    public Event Build()
    {
      DateTime dateTime = this.createdDate;
      if (!this.isCreatedDateSet)
        dateTime = DateTime.UtcNow;
      string str1 = this.envelopeVersion;
      if (!this.isEnvelopeVersionSet)
        str1 = "1.0.0";
      string str2 = this.payloadVersion;
      if (!this.isPayloadVersionSet)
        str2 = "1.0.0";
      return new Event()
      {
        Id = this.Id,
        CorrelationId = this.CorrelationId,
        Category = this.Category,
        Source = this.Source,
        Type = this.Type,
        Tenant = this.Tenant,
        EntityId = this.EntityId,
        CreatedDate = dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"),
        EnvelopeVersion = str1,
        PayloadVersion = str2,
        Payload = this.Payload
      };
    }

    public string Tostring()
    {
      return "EventBuilder(id=" + this.Id + ", correlationId=" + this.CorrelationId + ", category=" + this.Category + ", source=" + this.Source + ", type=" + this.Type + ", tenant=" + this.Tenant + ", entityId=" + this.EntityId + ", createdDate=" + (object) this.createdDate + ", envelopeVersion=" + this.envelopeVersion + ", payloadVersion=" + this.payloadVersion + ", payload=" + (object) this.Payload + ")";
    }
  }
}
