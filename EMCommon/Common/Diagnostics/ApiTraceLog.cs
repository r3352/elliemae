// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.ApiTraceLog
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics.Logging.Formatters;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics
{
  public class ApiTraceLog : Log
  {
    public ApiTraceLog()
    {
      this.Level = Encompass.Diagnostics.Logging.LogLevel.DEBUG;
      this.Src = "APITRACE";
    }

    protected ApiTraceLog(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public void SetCallerInfo(
      string callerType,
      string developerAppName,
      string callerAssembly,
      string callerEvent)
    {
      this.Set<string>(Log.CommonFields.CallerCategory, callerType);
      this.Set<string>(Log.CommonFields.CallerModuleName, developerAppName);
      this.Set<string>(Log.CommonFields.CallerAssembly, callerAssembly);
      this.Set<string>(Log.CommonFields.CallerEvent, callerEvent);
    }

    private bool TryGetCallerInfo(
      out string callerType,
      out string developerAppName,
      out string callerAssembly)
    {
      bool flag1 = this.TryGet<string>(Log.CommonFields.CallerCategory, out callerType);
      bool flag2 = this.TryGet<string>(Log.CommonFields.CallerModuleName, out developerAppName) | flag1;
      return this.TryGet<string>(Log.CommonFields.CallerAssembly, out callerAssembly) | flag2;
    }

    public void SetSessionInfo(string callerAppName, string userid, string sessionId)
    {
      this.UserId = userid;
      this.Set<string>(Log.CommonFields.CallerAppName, callerAppName);
      this.SessionId = sessionId;
    }

    private bool TryGetSessionInfo(
      out string userID,
      out string callerAppName,
      out string sessionId)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(this.UserId))
      {
        userID = this.UserId;
        flag = true;
      }
      else
        userID = (string) null;
      bool sessionInfo = this.TryGet<string>(Log.CommonFields.CallerAppName, out callerAppName) | flag;
      if (!string.IsNullOrEmpty(this.SessionId))
      {
        sessionId = this.SessionId;
        sessionInfo = true;
      }
      else
        sessionId = (string) null;
      return sessionInfo;
    }

    public override string GetLogMessage()
    {
      string callerType;
      string developerAppName;
      string callerAssembly;
      string str1;
      if (!this.TryGetCallerInfo(out callerType, out developerAppName, out callerAssembly))
        str1 = string.Empty;
      else
        str1 = "/" + callerType + ":" + developerAppName + " From Assembly : ( " + callerAssembly + " )";
      string str2 = str1;
      string str3 = string.Empty;
      double tvalue;
      if (this.TryGet<double>(Log.CommonFields.DurationMS, out tvalue))
        str3 = ",<" + tvalue.ToString("0") + "ms>";
      string userID;
      string callerAppName;
      string sessionId;
      if (!this.TryGetSessionInfo(out userID, out callerAppName, out sessionId))
        return this.Message + str2 + ". " + str3;
      return this.Message + " by " + userID + "/" + callerAppName + str2 + "/" + sessionId + ". " + str3;
    }

    public override DateTime GetLogTime()
    {
      DateTime tvalue;
      return !this.TryGet<DateTime>(Log.CommonFields.StartTime, out tvalue) ? this.Timestamp : tvalue;
    }

    public override string GetLogLevel() => "APITRACE";

    public override string GetSourceName() => string.Empty;

    public override bool SupportsFormatter(LogFormat logFormat)
    {
      return logFormat == LogFormat.PlainText || logFormat == LogFormat.PlainTextWithInstance;
    }
  }
}
