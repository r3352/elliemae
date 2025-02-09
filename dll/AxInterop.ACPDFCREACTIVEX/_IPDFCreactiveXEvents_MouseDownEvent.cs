// Decompiled with JetBrains decompiler
// Type: AxACPDFCREACTIVEX._IPDFCreactiveXEvents_MouseDownEvent
// Assembly: AxInterop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50348D5A-A8E2-4894-AD2C-0D88350B72D8
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AxInterop.ACPDFCREACTIVEX.dll

using ACPDFCREACTIVEX;

#nullable disable
namespace AxACPDFCREACTIVEX
{
  public class _IPDFCreactiveXEvents_MouseDownEvent
  {
    public acObject pObject;
    public int xPos;
    public int yPos;
    public int @continue;

    public _IPDFCreactiveXEvents_MouseDownEvent(
      acObject pObject,
      int xPos,
      int yPos,
      int @continue)
    {
      this.pObject = pObject;
      this.xPos = xPos;
      this.yPos = yPos;
      this.@continue = @continue;
    }
  }
}
