// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ThinThick.TradingScreens.SecurityTradeEditor
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.MainUI.ThinThick.TradingScreens.Interfaces;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI.ThinThick.TradingScreens
{
  public class SecurityTradeEditor : ITradingScreen
  {
    public ToolStripItemCollection ToolStripItemCollection
    {
      get => MainForm.Instance.TradingMenu_SecurityTradeEditor;
    }

    public string MenuName => "Trades - SecurityTradeEditor";
  }
}
