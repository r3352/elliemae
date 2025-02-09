// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Customization.SurfaceLayout
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Customization
{
  public abstract class SurfaceLayout
  {
    private IControlSurface surface;

    public SurfaceLayout(IControlSurface surface) => this.surface = surface;

    public IControlSurface Surface => this.surface;

    public abstract Size TileSize { get; }

    public abstract IControlSurface CreateTile();

    public virtual void FinalizeLayout()
    {
    }
  }
}
