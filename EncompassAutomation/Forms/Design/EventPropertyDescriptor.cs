// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EventPropertyDescriptor
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
  /// <summary>
  /// Provides a property descriptor for use with Form control events.
  /// </summary>
  /// <exclude />
  public class EventPropertyDescriptor : PropertyDescriptor
  {
    private EMEventEditor eventEditor;
    private string eventType = string.Empty;

    /// <summary>Constructor of the EventPropertyDescriptor class.</summary>
    /// <param name="eventString"></param>
    /// <param name="category"></param>
    public EventPropertyDescriptor(string eventString, CategoryAttribute category)
      : base(eventString, new Attribute[2]
      {
        (Attribute) category,
        (Attribute) RefreshPropertiesAttribute.All
      })
    {
      this.eventType = eventString;
    }

    /// <summary>Returns the type of the property values.</summary>
    public override Type PropertyType => typeof (ControlEvent);

    /// <summary>
    /// The type of component the framework expects for this property.
    /// </summary>
    public override Type ComponentType => typeof (Control);

    /// <summary>Indicates that the property is read/write.</summary>
    public override bool IsReadOnly => false;

    /// <summary>Returns the ControlEvent for the descriptor.</summary>
    /// <param name="component"></param>
    /// <returns></returns>
    public override object GetValue(object component)
    {
      Control control = component as Control;
      return (object) control.Form.ControlEvents.GetEvent(control.ControlID, this.eventType);
    }

    /// <summary>Sets the control event for the descriptor.</summary>
    /// <param name="component"></param>
    /// <param name="value"></param>
    public override void SetValue(object component, object value)
    {
      ControlEvents controlEvents = (component as Control).Form.ControlEvents;
    }

    /// <summary>Abstract member override</summary>
    /// <param name="o"></param>
    public override void ResetValue(object o)
    {
    }

    /// <summary>This is not a resettable property.</summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public override bool CanResetValue(object o) => false;

    /// <summary>This property doesn't participate in code generation.</summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public override bool ShouldSerializeValue(object o) => false;

    /// <summary>
    /// Provides the editor for the property, which is the <see cref="T:EllieMae.Encompass.Forms.Design.EMEventEditor" />.
    /// </summary>
    /// <param name="editorBaseType"></param>
    /// <returns></returns>
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
