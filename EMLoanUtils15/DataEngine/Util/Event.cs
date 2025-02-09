// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Util.Event
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Util
{
  public class Event
  {
    [JsonProperty("eventType")]
    public string EventType { get; set; }

    [JsonProperty("ipAddress")]
    public string IPAddress { get; set; }

    [JsonProperty("date")]
    public string date { get; set; }

    public string Date => Utils.ConvertToPST(this.date);

    [JsonProperty("data")]
    public string Data { get; set; }
  }
}
