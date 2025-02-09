// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EMEventEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>Provides a UITypeEditor for control events.</summary>
  /// <remarks>This class and its properties and methods are meant for use within the
  /// Encompass Form Builder class only</remarks>
  /// <exclude />
  public class EMEventEditor : UITypeEditor
  {
    private string eventType = string.Empty;

    /// <summary>Sets the name of the event being edited.</summary>
    /// <param name="eventType">The event name.</param>
    public void SetEventType(string eventType) => this.eventType = eventType;

    /// <summary>
    /// Sets the editor style to be a modal dialog (the event editor).
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    /// <summary>
    /// Displays the event editor for a particular control's event.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="provider"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override object EditValue(
      ITypeDescriptorContext context,
      IServiceProvider provider,
      object value)
    {
      EMEventEditor.ShowEventEditor(context.Instance as Control, this.eventType);
      return (object) null;
    }

    /// <summary>Static method for opening the event editor.</summary>
    /// <param name="control"></param>
    public static void ShowEventEditor(Control control)
    {
      EMEventEditor.ShowEventEditor(control, (string) null);
    }

    /// <summary>
    /// Displays the event editor for a specific control event.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="eventType"></param>
    public static void ShowEventEditor(Control control, string eventType)
    {
      Form form = control.Form;
      ControlEvents controlEvents = form.ControlEvents;
      ControlEvent currentEvent = (ControlEvent) null;
      if (eventType != null)
        currentEvent = controlEvents.GetEvent(control.ControlID, eventType) ?? controlEvents.CreateNew(control.ControlID, eventType);
      EventCodeEditorArgs e = new EventCodeEditorArgs(control, currentEvent);
      form.OnEventEditor(e);
    }
  }
}
