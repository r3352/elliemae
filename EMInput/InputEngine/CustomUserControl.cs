// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CustomUserControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CustomUserControl : UserControl
  {
    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.DisposeCustomControl((Control) this);
      base.Dispose(disposing);
    }

    private void DisposeCustomControl(Control myCtrl)
    {
      if (myCtrl.Controls != null && myCtrl.Controls.Count > 0)
      {
        foreach (Control control in (ArrangedElementCollection) myCtrl.Controls)
          this.DisposeCustomControl(control);
      }
      this.DetachEvents((Component) myCtrl);
      myCtrl.Controls.Clear();
      if (myCtrl == this)
        return;
      myCtrl.Dispose();
    }

    private void DetachEvents(Component obj)
    {
      ((EventHandlerList) obj.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) obj, (object[]) null))?.Dispose();
    }
  }
}
