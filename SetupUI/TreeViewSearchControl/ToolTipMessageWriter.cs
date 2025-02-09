// Decompiled with JetBrains decompiler
// Type: TreeViewSearchControl.ToolTipMessageWriter
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchControl
{
  public class ToolTipMessageWriter : IMessageWriter
  {
    private ToolTip _tt;
    private IWin32Window _ttControl;
    private Point _point;
    private int _duration;

    public ToolTipMessageWriter(
      ToolTip toolTip,
      IWin32Window win32Window,
      Point point,
      int duration)
    {
      this._tt = toolTip;
      this._ttControl = win32Window;
      this._point = point;
      this._duration = duration;
    }

    public void Write(string message)
    {
      this._tt.Show(message, this._ttControl, this._point, this._duration);
    }
  }
}
