// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EventPropertyDescriptor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class EventPropertyDescriptor : PropertyDescriptor
  {
    private EMEventEditor eventEditor;
    private string eventType = string.Empty;

    public EventPropertyDescriptor(string eventString, CategoryAttribute category)
      : base(eventString, new Attribute[2]
      {
        (Attribute) category,
        (Attribute) RefreshPropertiesAttribute.All
      })
    {
      this.eventType = eventString;
    }

    public override Type PropertyType => typeof (ControlEvent);

    public override Type ComponentType => typeof (Control);

    public override bool IsReadOnly => false;

    public override object GetValue(object component)
    {
      Control control = component as Control;
      return (object) control.Form.ControlEvents.GetEvent(control.ControlID, this.eventType);
    }

    public override void SetValue(object component, object value)
    {
      ControlEvents controlEvents = (component as Control).Form.ControlEvents;
    }

    public override void ResetValue(object o)
    {
    }

    public override bool CanResetValue(object o) => false;

    public override bool ShouldSerializeValue(object o) => false;

    public override object GetEditor(Type editorBaseType)
    {
      if (!(editorBaseType == typeof (UITypeEditor)))
        return base.GetEditor(editorBaseType);
      if (this.eventEditor == null)
        this.eventEditor = new EMEventEditor();
      this.eventEditor.SetEventType(this.eventType);
      return (object) this.eventEditor;
    }
  }
}
