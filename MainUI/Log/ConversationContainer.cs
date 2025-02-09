// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ConversationContainer
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ConversationContainer : Form
  {
    private bool existingLog;
    private ConversationLog log;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Panel panelTop;

    public ConversationContainer(ConversationLog log, bool existingLog)
    {
      this.log = log;
      this.existingLog = existingLog;
      this.InitializeComponent();
      this.panelTop.Controls.Add((Control) new ConversationRecWS(log));
      if (!this.existingLog)
        return;
      this.dialogButtons1.OKButton.Text = "&Close";
      this.dialogButtons1.CancelButton.Visible = false;
      this.dialogButtons1.OKButton.Left = this.dialogButtons1.CancelButton.Left;
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (!this.existingLog)
        Session.LoanData.GetLogList().AddRecord((LogRecordBase) this.log);
      Session.Application.GetService<ILoanEditor>().RefreshContents();
      this.DialogResult = DialogResult.OK;
    }

    private void ConversationContainer_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.panelTop = new Panel();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 506);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(785, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(785, 506);
      this.panelTop.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(785, 550);
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConversationContainer);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Conversation Log";
      this.KeyPress += new KeyPressEventHandler(this.ConversationContainer_KeyPress);
      this.ResumeLayout(false);
    }
  }
}
