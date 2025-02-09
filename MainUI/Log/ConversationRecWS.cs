// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ConversationRecWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ConversationRecWS : UserControl, IOnlineHelpTarget
  {
    private System.ComponentModel.Container components = new System.ComponentModel.Container();
    private ConversationLog con;

    public ConversationRecWS(ConversationLog con)
    {
      Cursor.Current = Cursors.Arrow;
      this.con = con;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      ConversationDialog conversationDialog = new ConversationDialog(con, (GVItem) null);
      this.Controls.Add((Control) conversationDialog);
      this.components.Add((IComponent) conversationDialog);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string GetHelpTargetName() => "ConversationWS";

    private void InitializeComponent()
    {
      this.Name = nameof (ConversationRecWS);
      this.Size = new Size(520, 428);
    }
  }
}
