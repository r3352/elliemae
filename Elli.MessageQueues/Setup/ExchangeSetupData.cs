// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Setup.ExchangeSetupData
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Elli.MessageQueues.Setup
{
  [DebuggerStepThrough]
  public class ExchangeSetupData
  {
    public string ExchangeType { get; set; }

    public bool Durable { get; set; }

    public bool AutoDelete { get; set; }

    public IDictionary<string, object> Arguments { get; private set; }

    public ExchangeSetupData()
    {
      this.Durable = true;
      this.ExchangeType = "direct";
      this.Arguments = (IDictionary<string, object>) new Dictionary<string, object>();
    }
  }
}
