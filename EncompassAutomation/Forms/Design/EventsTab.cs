// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EventsTab
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.Design;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Represents the Events tab in the Property Grid of the Form Builder.
  /// </summary>
  /// <exclude />
  public class EventsTab : PropertyTab
  {
    /// <summary>Gets the display name of the tab.</summary>
    public override string TabName => "Events";

    /// <summary>
    /// Gets the bitmap image for display in the Property Grid for the tab.
    /// </summary>
    public override Bitmap Bitmap
    {
      get
      {
        return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "FLASH.BMP"));
      }
    }

    /// <summary>Provides the property descriptors for the events tab.</summary>
    /// <param name="component"></param>
    /// <param name="attributes"></param>
    /// <returns></returns>
    public override PropertyDescriptorCollection GetProperties(
      object component,
      Attribute[] attributes)
    {
      return this.GetProperties((ITypeDescriptorContext) null, component, attributes);
    }

    /// <summary>
    /// Provides the event descriptors for a control that supports events.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="component"></param>
    /// <param name="attributes"></param>
    /// <returns></returns>
    public override PropertyDescriptorCollection GetProperties(
      ITypeDescriptorContext context,
      object component,
      Attribute[] attributes)
    {
      ArrayList arrayList = new ArrayList();
      if (component is ISupportsEvents)
      {
        foreach (string supportedEvent in ((ISupportsEvents) component).SupportedEvents)
        {
          CategoryAttribute[] customAttributes = (CategoryAttribute[]) component.GetType().GetEvent(supportedEvent, BindingFlags.Instance | BindingFlags.Public).GetCustomAttributes(typeof (CategoryAttribute), true);
          arrayList.Add((object) new EventPropertyDescriptor(supportedEvent, customAttributes.Length == 0 ? CategoryAttribute.Data : customAttributes[0]));
        }
      }
      return new PropertyDescriptorCollection((PropertyDescriptor[]) arrayList.ToArray(typeof (PropertyDescriptor)));
    }
  }
}
