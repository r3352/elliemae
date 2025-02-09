// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.LogErrorData
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.PII;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  public class LogErrorData : ILogErrorData
  {
    internal LogErrorData(LogErrorData logErrorData, bool maskSQL)
    {
      this.Type = logErrorData.Type;
      this.Message = MaskUtilities.MaskPII(logErrorData.Message, maskSQL);
      this.StackTrace = MaskUtilities.MaskPII(logErrorData.StackTrace, maskSQL);
      if (logErrorData.Xcause == null)
        return;
      this.Xcause = (ILogErrorData) new LogErrorData(logErrorData.Xcause, maskSQL);
    }

    public string Type { get; }

    public string Message { get; }

    public string StackTrace { get; }

    public ILogErrorData Xcause { get; }
  }
}
