// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ThinThick.BrowserBase
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.MainUI.ThinThick
{
  public class BrowserBase : UserControl
  {
    private IContainer components;
    private ThinThickBrowser BrowserControl;

    public BrowserBase() => this.InitializeComponent();

    public ToolStripItemCollection MenuItemCollection { get; set; }

    public ThinThickBrowser Browser => this.BrowserControl;

    public void SetMenuState(MenuState[] menuStates, ToolStripItemCollection collection)
    {
      if (collection == null)
        return;
      foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) collection)
      {
        if (!(toolStripItem is ToolStripSeparator))
        {
          foreach (MenuState menuState in menuStates)
          {
            if (toolStripItem.Tag != null && toolStripItem.Tag.Equals((object) menuState.MenuItemTag))
            {
              toolStripItem.Enabled = menuState.Enabled;
              toolStripItem.Visible = menuState.Visible;
            }
          }
          if (((ToolStripDropDownItem) toolStripItem).DropDownItems.Count > 0)
            this.SetMenuState(menuStates, ((ToolStripDropDownItem) toolStripItem).DropDownItems);
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.BrowserControl = new ThinThickBrowser();
      this.SuspendLayout();
      this.BrowserControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.BrowserControl.BackColor = Color.YellowGreen;
      this.BrowserControl.Location = new Point(0, 0);
      this.BrowserControl.Margin = new Padding(0);
      this.BrowserControl.Name = "BrowserControl";
      this.BrowserControl.Size = new Size(0, 0);
      this.BrowserControl.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.BrowserControl);
      this.Name = nameof (BrowserBase);
      this.Size = new Size(0, 0);
      this.ResumeLayout(false);
    }
  }
}
