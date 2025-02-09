// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.AlterConfigsOptions
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>Options for the AlterConfigs method.</summary>
  public class AlterConfigsOptions
  {
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
    ///     If true, the request should be validated only without altering
    ///     the configs.
    /// 
    ///     Default: false
    /// </summary>
    public bool ValidateOnly { get; set; }
  }
}
