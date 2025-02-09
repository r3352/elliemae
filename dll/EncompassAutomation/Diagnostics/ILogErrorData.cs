// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.ILogErrorData
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  public interface ILogErrorData
  {
    string Type { get; }

    string Message { get; }

    string StackTrace { get; }

    ILogErrorData Xcause { get; }
  }
}
