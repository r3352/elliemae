// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Customization.ControlWriter
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Customization
{
  public class ControlWriter
  {
    private SurfaceLayout layout;

    public ControlWriter(SurfaceLayout layout) => this.layout = layout;

    public SurfaceLayout Layout => this.layout;

    public void AddField(FieldDescriptor fieldDesc)
    {
      IControlSurface tile = this.layout.CreateTile();
      Rectangle bounds1 = tile.Bounds;
      Rectangle bounds2 = new Rectangle(bounds1.Location, new Size(bounds1.Width / 2, bounds1.Height));
      tile.CreateLabel(bounds2, fieldDesc.Description);
      Rectangle bounds3 = new Rectangle(bounds2.Right, bounds1.Top, bounds1.Width / 2, bounds1.Height);
      tile.CreateField(bounds3, fieldDesc);
    }
  }
}
