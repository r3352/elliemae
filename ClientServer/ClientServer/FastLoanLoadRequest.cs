// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FastLoanLoadRequest
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.DataEngine;
using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FastLoanLoadRequest : IWhiteListRemoteMethodParam, IRemoteCallLogDecorator
  {
    public string Guid { get; set; }

    public bool ShouldLock { get; set; }

    public LoanConfigurationParameters ConfigParams { get; set; }

    public LockInfo.ExclusiveLock ExclusiveLock { get; set; }

    public void Decorate(Log log)
    {
      if (string.IsNullOrEmpty(this.Guid))
        return;
      log.Set<string>(Log.CommonFields.LoanId, this.Guid);
    }

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) new JObject()
      {
        {
          "id",
          (JToken) this.Guid
        }
      });
    }
  }
}
