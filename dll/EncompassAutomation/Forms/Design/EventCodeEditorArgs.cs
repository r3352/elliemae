// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EventCodeEditorArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class EventCodeEditorArgs : EventArgs
  {
    private Control control;
    private ControlEvent currentEvent;

    public EventCodeEditorArgs(Control control, ControlEvent currentEvent)
    {
      this.control = control;
      this.currentEvent = currentEvent;
    }

    public Control CurrentControl => this.control;

    public ControlEvent CurrentEvent => this.currentEvent;
  }
}
