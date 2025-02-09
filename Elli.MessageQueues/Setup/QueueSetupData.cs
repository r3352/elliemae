// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Setup.QueueSetupData
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Elli.MessageQueues.Setup
{
  [DebuggerStepThrough]
  public class QueueSetupData
  {
    public int MessageTimeToLive { get; set; }

    public bool Durable { get; set; }

    public bool AutoDelete { get; set; }

    public int AutoExpire { get; set; }

    public string DeadLetterExchange { get; set; }

    public string DeadLetterQueue { get; set; }

    public string DeadLetterRoutingKey { get; set; }

    public string DeadLetterRoutingKeyForOriginal { get; set; }

    public int DeadLetterMessageTimeToLive { get; set; }

    public IDictionary<string, object> Arguments { get; private set; }

    public QueueSetupData()
    {
      this.Durable = true;
      this.Arguments = (IDictionary<string, object>) new Dictionary<string, object>();
    }
  }
}
