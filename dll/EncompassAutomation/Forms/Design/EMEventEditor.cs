// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EMEventEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class EMEventEditor : UITypeEditor
  {
    private string eventType = string.Empty;

    public void SetEventType(string eventType) => this.eventType = eventType;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    public override object EditValue(
      ITypeDescriptorContext context,
      IServiceProvider provider,
      object value)
    {
      EMEventEditor.ShowEventEditor(context.Instance as Control, this.eventType);
      return (object) null;
    }

    public static void ShowEventEditor(Control control)
    {
      EMEventEditor.ShowEventEditor(control, (string) null);
    }

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
