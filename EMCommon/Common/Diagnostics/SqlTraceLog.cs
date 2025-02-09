// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.SqlTraceLog
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics
{
  public class SqlTraceLog : Encompass.Diagnostics.Logging.Schema.Log
  {
    public SqlTraceLog()
    {
    }

    protected SqlTraceLog(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public void SetCommandText(string sql) => this.Set<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.Sql, sql);

    public void SetCallStack(StackTrace stackTrace)
    {
      this.Set<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.StackTrace, stackTrace.ToString());
    }

    public override string GetLogMessage()
    {
      double tvalue1;
      bool flag1 = this.TryGet<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, out tvalue1);
      string tvalue2;
      bool flag2 = this.TryGet<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.StackTrace, out tvalue2);
      string str1 = this.Get<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.Sql);
      if (this.Level == Encompass.Diagnostics.Logging.LogLevel.DEBUG || this.Level == Encompass.Diagnostics.Logging.LogLevel.INFO)
      {
        string str2 = "SQL Command Execution: ";
        if (flag1)
          str2 = str2 + "Elapsed Time = " + tvalue1.ToString("0") + " ms" + System.Environment.NewLine;
        string logMessage = str2 + str1;
        if (flag2)
          logMessage = logMessage + System.Environment.NewLine + "======= CallStack: $" + tvalue2;
        return logMessage;
      }
      return string.Format("Long duration SQL query found on thread {0} with elapsed time = {1}s. Command Text follows:", (object) Thread.CurrentThread.GetHashCode(), (object) (tvalue1 / 1000.0).ToString("0.00")) + System.Environment.NewLine + str1 + System.Environment.NewLine + System.Environment.NewLine + "Stack Trace:" + System.Environment.NewLine + tvalue2;
    }
  }
}
