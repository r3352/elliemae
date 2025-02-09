// Decompiled with JetBrains decompiler
// Type: TreeViewSearchControl.MessageWriterFactory
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchControl
{
  public class MessageWriterFactory
  {
    public static IMessageWriter GetToolTipWriter(
      ToolTip toolTip,
      IWin32Window win32Window,
      Point point,
      int duration)
    {
      return (IMessageWriter) new ToolTipMessageWriter(toolTip, win32Window, point, duration);
    }

    public static IMessageWriter GetLabelWriter(Label label)
    {
      return (IMessageWriter) new LabelMessageWriter(label);
    }

    public static IMessageWriter GetGhostWriter() => (IMessageWriter) new GhostMessageWriter();
  }
}
