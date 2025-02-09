// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.ILog
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  /// <summary>
  /// interface for <see cref="T:EllieMae.Encompass.Diagnostics.Log" /> class
  /// </summary>
  public interface ILog
  {
    /// <summary>Describes the event being logged.</summary>
    string Message { get; }

    /// <summary>Date-timestamp at which the log was generated</summary>
    DateTime TimeStamp { get; }

    /// <summary>Additional log fields.</summary>
    Dictionary<string, object> AdditionalFields { get; }

    /// <summary>
    /// If this logs is for an Exception this field will be populated.
    /// </summary>
    ILogErrorData Exception { get; }

    /// <summary>
    /// Managed Thread Id of the thread that generated the log
    /// </summary>
    int? ThreadId { get; }

    /// <summary>LogLevel for the log</summary>
    LogLevel LogLevel { get; }

    /// <summary>
    /// Source of the log. Generally the class or method name where the log is originated.
    /// </summary>
    string Source { get; }

    /// <summary>
    /// Logger switch using wich log was written. A logger switch corresponds to a functional area for which granularity of logs can be configured.
    /// </summary>
    string Switch { get; }
  }
}
