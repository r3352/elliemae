// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.MessageScreen
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Web.Host.BrowserControls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class MessageScreen : Form
  {
    private string scAppCmdArgs;
    private IContainer components;
    private Panel panel1;
    private EncWebFormBrowserControl webBrowser1;
    private Button btnCancel;
    private Button btnOK;

    public MessageScreen(string url, string scAppCmdArgs)
    {
      this.scAppCmdArgs = scAppCmdArgs;
      this.InitializeComponent();
      this.webBrowser1.Navigate(url);
    }

    private void MessageScreen_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.webBrowser1.Dispose();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnOK_Click(object sender, EventArgs e)
    {
      new Process()
      {
        StartInfo = {
          FileName = Path.Combine(Application.StartupPath, "ScApp\\ScApp.exe"),
          Arguments = this.scAppCmdArgs
        }
      }.Start();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MessageScreen));
      this.panel1 = new Panel();
      this.webBrowser1 = BrowserFactory.GetWebBrowserInstance();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.webBrowser1);
      this.panel1.Location = new Point(-2, -1);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(680, 437);
      this.panel1.TabIndex = 0;
      this.webBrowser1.Dock = DockStyle.Fill;
      this.webBrowser1.Location = new Point(0, 0);
      this.webBrowser1.MinimumSize = new Size(20, 20);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.Size = new Size(680, 437);
      this.webBrowser1.TabIndex = 0;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(589, 443);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(508, 443);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(676, 471);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.panel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MessageScreen);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Message";
      this.FormClosing += new FormClosingEventHandler(this.MessageScreen_FormClosing);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
