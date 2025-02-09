// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.ILog
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  public interface ILog
  {
    string Message { get; }

    DateTime TimeStamp { get; }

    Dictionary<string, object> AdditionalFields { get; }

    ILogErrorData Exception { get; }

    int? ThreadId { get; }

    LogLevel LogLevel { get; }

    string Source { get; }

    string Switch { get; }
  }
}
