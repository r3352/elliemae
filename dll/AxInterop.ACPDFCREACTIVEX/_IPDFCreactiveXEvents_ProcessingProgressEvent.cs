// Decompiled with JetBrains decompiler
// Type: AxACPDFCREACTIVEX._IPDFCreactiveXEvents_ProcessingProgressEvent
// Assembly: AxInterop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50348D5A-A8E2-4894-AD2C-0D88350B72D8
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AxInterop.ACPDFCREACTIVEX.dll

#nullable disable
namespace AxACPDFCREACTIVEX
{
  public class _IPDFCreactiveXEvents_ProcessingProgressEvent
  {
    public int totalSteps;
    public int currentStep;
    public int @continue;

    public _IPDFCreactiveXEvents_ProcessingProgressEvent(
      int totalSteps,
      int currentStep,
      int @continue)
    {
      this.totalSteps = totalSteps;
      this.currentStep = currentStep;
      this.@continue = @continue;
    }
  }
}
