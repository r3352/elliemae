// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Schema.LogErrorData
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Schema
{
  [Serializable]
  public class LogErrorData
  {
    public string Type { get; set; }

    public string Message { get; set; }

    public string StackTrace { get; set; }

    public LogErrorData Xcause { get; set; }

    [JsonIgnore]
    public Exception Exception { get; }

    public LogErrorData()
    {
    }

    public LogErrorData(Exception ex)
    {
      this.Exception = ex;
      this.Type = ex.GetType().FullName;
      this.Message = ex.Message;
      this.StackTrace = ex.StackTrace;
      if (ex.InnerException == null)
        return;
      this.Xcause = new LogErrorData(ex.InnerException);
    }

    public override string ToString() => this.GetFullStackTrace();
  }
}
