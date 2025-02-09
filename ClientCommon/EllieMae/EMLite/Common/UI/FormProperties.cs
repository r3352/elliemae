// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FormProperties
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FormProperties : BinaryConvertible<FormProperties>
  {
    public Size Size;
    public Point Location;
    public FormWindowState WindowState;

    public FormProperties(Form form)
    {
      this.Size = form.Size;
      this.Location = form.Location;
      this.WindowState = form.WindowState;
    }

    public FormProperties(Point location, Size size, FormWindowState state)
    {
      this.Size = size;
      this.Location = location;
      this.WindowState = state;
    }

    public FormProperties(XmlSerializationInfo info)
    {
      this.Location = new Point(info.GetInteger("x"), info.GetInteger("y"));
      this.Size = new Size(info.GetInteger("width"), info.GetInteger("height"));
      this.WindowState = (FormWindowState) info.GetValue("state", typeof (FormWindowState));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("x", (object) this.Location.X);
      info.AddValue("y", (object) this.Location.Y);
      info.AddValue("width", (object) this.Size.Width);
      info.AddValue("height", (object) this.Size.Height);
      info.AddValue("state", (object) this.WindowState);
    }

    public void ApplyToForm(Form form, bool fitToScreen)
    {
      Size size = this.Size;
      Point location = this.Location;
      if (fitToScreen)
      {
        Screen primaryScreen = Screen.PrimaryScreen;
        size.Width = Math.Min(primaryScreen.WorkingArea.Width, this.Size.Width);
        size.Height = Math.Min(primaryScreen.WorkingArea.Height, this.Size.Height);
        location.X = Math.Min(Math.Max(0, this.Location.X), primaryScreen.WorkingArea.Width - size.Width);
        location.Y = Math.Min(Math.Max(0, this.Location.Y), primaryScreen.WorkingArea.Height - size.Height);
      }
      if (this.WindowState == FormWindowState.Maximized)
      {
        form.WindowState = FormWindowState.Maximized;
      }
      else
      {
        form.WindowState = FormWindowState.Normal;
        form.Location = location;
        form.Size = size;
      }
    }
  }
}
