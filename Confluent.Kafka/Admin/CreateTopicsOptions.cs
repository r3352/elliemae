// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.CreateTopicsOptions
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>Options for the CreateTopics method.</summary>
  public class CreateTopicsOptions
  {
    /// <summary>
    ///     If true, the request should be validated on the broker only
    ///     without creating the topic.
    /// 
    ///     Default: false
    /// </summary>
    public bool ValidateOnly { get; set; }

    /// <summary>
    ///     The overall request timeout, including broker lookup, request
    ///     transmission, operation time on broker, and response. If set
    ///     to null, the default request timeout for the AdminClient will
    ///     be used.
    /// 
    ///     Default: null
    /// </summary>
    public TimeSpan? RequestTimeout { get; set; }

    /// <summary>
    ///     The broker's operation timeout - the maximum time to wait for
    ///     CreateTopics before returning a result to the application.
    ///     If set to null, will return immediately upon triggering topic
    ///     creation.
    /// 
    ///     Default: null
    /// </summary>
    public TimeSpan? OperationTimeout { get; set; }
  }
}
