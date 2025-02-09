// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.EventsTab
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.Design;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class EventsTab : PropertyTab
  {
    public override string TabName => "Events";

    public override Bitmap Bitmap
    {
      get
      {
        return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "FLASH.BMP"));
      }
    }

    public override PropertyDescriptorCollection GetProperties(
      object component,
      Attribute[] attributes)
    {
      return this.GetProperties((ITypeDescriptorContext) null, component, attributes);
    }

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
