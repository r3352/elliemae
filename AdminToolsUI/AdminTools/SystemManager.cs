// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.SystemManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Setup;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class SystemManager : Form
  {
    private Button closeBtn;
    private CurrentLoginsUserControl currentLoginsUserControl1;
    private System.ComponentModel.Container components;

    public event EventHandler SessionTerminatedEvent;

    public SystemManager()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SystemManager));
      this.closeBtn = new Button();
      this.currentLoginsUserControl1 = new CurrentLoginsUserControl();
      this.SuspendLayout();
      this.closeBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.closeBtn.Location = new Point(667, 491);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(85, 23);
      this.closeBtn.TabIndex = 1;
      this.closeBtn.Text = "Close";
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.currentLoginsUserControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.currentLoginsUserControl1.Location = new Point(0, 0);
      this.currentLoginsUserControl1.Name = "currentLoginsUserControl1";
      this.currentLoginsUserControl1.Size = new Size(761, 485);
      this.currentLoginsUserControl1.TabIndex = 2;
      this.currentLoginsUserControl1.SessionTerminated += new EventHandler(this.onSessionTerminated);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(760, 518);
      this.Controls.Add((Control) this.closeBtn);
      this.Controls.Add((Control) this.currentLoginsUserControl1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SystemManager);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Online User Manager";
      this.ResumeLayout(false);
    }

    private void closeBtn_Click(object sender, EventArgs e) => this.Close();

    public void onSessionTerminated(object sender, EventArgs e)
    {
      try
      {
        if (this.SessionTerminatedEvent == null)
          return;
        this.SessionTerminatedEvent(sender, e);
      }
      catch
      {
      }
    }
  }
}
