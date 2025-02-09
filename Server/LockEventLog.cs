// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LockEventLog
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using Encompass.Diagnostics.Logging.Schema;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LockEventLog : Log
  {
    public LockEventLog()
    {
    }

    protected LockEventLog(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public override string GetLogLevel() => "LOCKRPT";

    public override string GetLogMessage()
    {
      LockStatus lockStatus = this.Get<LockStatus>(LockEventLog.Fields.Status);
      switch (lockStatus)
      {
        case LockStatus.Released:
        case LockStatus.ReleaseErrored:
        case LockStatus.EvictionDetected:
          string str1 = this.Get<string>(LockEventLog.Fields.Key);
          LockType lockType = this.Get<LockType>(LockEventLog.Fields.LockType);
          long num1 = this.Get<long>(LockEventLog.Fields.WaitTime);
          long num2 = this.Get<long>(LockEventLog.Fields.ProcessingTime);
          long num3 = this.Get<long>(LockEventLog.Fields.ReleaseTime);
          long num4 = num3 + num2;
          long num5 = this.Get<long>(LockEventLog.Fields.TotalTime);
          int[] values = this.Get<int[]>(LockEventLog.Fields.ThreadsBlocked);
          int length = values != null ? values.Length : 0;
          string str2 = values == null ? string.Empty : string.Join<int>(":", (IEnumerable<int>) values);
          int num6 = this.Get<int>(LockEventLog.Fields.NestLevel);
          string str3 = "Completed";
          switch (lockStatus)
          {
            case LockStatus.ReleaseErrored:
              str3 = "Release Errored";
              break;
            case LockStatus.EvictionDetected:
              str3 = "Eviction Detected";
              break;
          }
          return string.Format("{0} {1} lock on key '{2}': WaitTime = {3}ms, ProcessingTime = {4}ms, ReleaseTime = {5}ms, TotalTimeInLock = {6}ms, TotalTime = {7}ms, #ThreadsBlocked = {8}, ThreadBlocked = {9}, NestLevel = {10}", (object) str3, (object) lockType, (object) str1, (object) num1, (object) num2, (object) num3, (object) num4, (object) num5, (object) length, (object) str2, (object) num6);
        default:
          return base.GetLogMessage();
      }
    }

    public class Fields
    {
      public static readonly LogFieldName<string> Key = LogFields.Field<string>("lockId");
      public static readonly LogFieldName<LockType> LockType = LogFields.Field<LockType>("lockType");
      public static readonly LogFieldName<LockStatus> Status = LogFields.Field<LockStatus>("lockStatus");
      public static readonly LogFieldName<long> WaitTime = LogFields.Field<long>("lockWaitTimeMS");
      public static readonly LogFieldName<long> ProcessingTime = LogFields.Field<long>("lockProcessingTimeMS");
      public static readonly LogFieldName<long> ReleaseTime = LogFields.Field<long>("lockReleaseTimeMS");
      public static readonly LogFieldName<long> TotalTime = LogFields.Field<long>("lockTotalTimeMS");
      public static readonly LogFieldName<int[]> ThreadsBlocked = LogFields.Field<int[]>("lockThreadsBlocked");
      public static readonly LogFieldName<int> NestLevel = LogFields.Field<int>("lockNestLevel");
    }
  }
}
