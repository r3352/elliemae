// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EventCodeEditorArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Event arguments class for the Form's EventEditor event.
  /// </summary>
  /// <exclude />
  public class EventCodeEditorArgs : EventArgs
  {
    private Control control;
    private ControlEvent currentEvent;

    /// <summary>Constructor for the EventCodeEditorArgs class.</summary>
    /// <param name="control"></param>
    /// <param name="currentEvent"></param>
    public EventCodeEditorArgs(Control control, ControlEvent currentEvent)
    {
      this.control = control;
      this.currentEvent = currentEvent;
    }

    /// <summary>
    /// Returns the control that should be selected in the event editor.
    /// </summary>
    public Control CurrentControl => this.control;

    /// <summary>
    /// Returns the event that should be selected in the event editor.
    /// </summary>
    public ControlEvent CurrentEvent => this.currentEvent;
  }
}
