// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.QueueMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public abstract class QueueMessage
  {
    public const string PayloadVersion2 = "2.0.0�";
    public const string EnvelopeVersion2 = "2.0.0�";

    [IgnoreProperty]
    public string CorrelationId { get; set; }

    [IgnoreProperty]
    public string Type { get; set; }

    public string LoanId { get; set; }

    public string LoanPath { get; set; }

    public string PublishTime { get; set; }

    [IgnoreProperty]
    public string EnvelopeVersion { get; set; }

    [IgnoreProperty]
    public string PayloadVersion { get; set; }

    public Dictionary<string, object> GetPayload()
    {
      return ((IEnumerable<PropertyInfo>) this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.GetCustomAttributes(typeof (IgnorePropertyAttribute), true).Length == 0 && pi.GetValue((object) this, (object[]) null) != null)).ToDictionary<PropertyInfo, string, object>((Func<PropertyInfo, string>) (prop => prop.Name), (Func<PropertyInfo, object>) (prop => prop.GetValue((object) this, (object[]) null)));
    }
  }
}
