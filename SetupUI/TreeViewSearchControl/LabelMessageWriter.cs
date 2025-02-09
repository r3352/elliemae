// Decompiled with JetBrains decompiler
// Type: TreeViewSearchControl.LabelMessageWriter
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchControl
{
  public class LabelMessageWriter : IMessageWriter
  {
    private Label _lbl;

    public LabelMessageWriter(Label label) => this._lbl = label;

    public void Write(string message) => this._lbl.Text = message;
  }
}
