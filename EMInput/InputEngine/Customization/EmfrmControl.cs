// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Customization.EmfrmControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.Forms;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Customization
{
  public class EmfrmControl : ISurfaceElement
  {
    private Control control;

    public EmfrmControl(Control control) => this.control = control;

    public Rectangle Bounds
    {
      get => new Rectangle(this.control.Position, this.control.Size);
      set
      {
        this.control.Size = value.Size;
        this.control.Position = value.Location;
      }
    }
  }
}
