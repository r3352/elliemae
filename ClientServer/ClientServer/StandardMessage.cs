// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.StandardMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using Newtonsoft.Json;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class StandardMessage
  {
    private string _instanceId;

    [JsonIgnore]
    public DateTime CreateAt { get; set; }

    [JsonIgnore]
    public string EntityId { get; set; }

    [JsonIgnore]
    public string InstanceId
    {
      get => (this._instanceId ?? string.Empty).Trim();
      set => this._instanceId = value;
    }

    [JsonIgnore]
    public string ServiceId { get; set; }

    [JsonIgnore]
    public string SiteId { get; set; }

    [JsonIgnore]
    public string Source { get; set; }

    [JsonIgnore]
    public string UserId { get; set; }

    [JsonIgnore]
    public string Tenant { get; set; }

    [CLSCompliant(false)]
    [JsonIgnore]
    public Enums.Category Category { get; set; }
  }
}
