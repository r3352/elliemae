// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.MOTD.MessageScreen
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.MOTD
{
  public class MessageScreen : Form
  {
    private string scAppCmdArgs;
    private string scAppExe = Path.Combine(Application.StartupPath, "ScApp\\ScApp.exe");
    private IContainer components;
    private Panel panel1;
    private WebBrowser webBrowser1;
    private CheckBox chkBoxDontShow;
    private Button btnOK;
    private Button btnRun;

    public MessageScreen(string url, bool scAppEnabled = false, string scAppCmdArgs = null)
    {
      this.InitializeComponent();
      this.btnRun.Visible = scAppEnabled && File.Exists(this.scAppExe);
      this.scAppCmdArgs = scAppCmdArgs;
      this.webBrowser1.Navigate(url);
    }

    public bool DontShowChecked => this.chkBoxDontShow.Checked;

    public bool DontShowEnabled
    {
      get => this.chkBoxDontShow.Enabled;
      set => this.chkBoxDontShow.Enabled = value;
    }

    private void MessageScreen_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.webBrowser1.Dispose();
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    private void btnRun_Click(object sender, EventArgs e)
    {
      new Process()
      {
        StartInfo = {
          FileName = this.scAppExe,
          Arguments = this.scAppCmdArgs
        }
      }.Start();
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
      this.webBrowser1 = new WebBrowser();
      this.chkBoxDontShow = new CheckBox();
      this.btnOK = new Button();
      this.btnRun = new Button();
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
      this.chkBoxDontShow.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.chkBoxDontShow.AutoSize = true;
      this.chkBoxDontShow.Location = new Point(401, 447);
      this.chkBoxDontShow.Name = "chkBoxDontShow";
      this.chkBoxDontShow.Size = new Size(179, 17);
      this.chkBoxDontShow.TabIndex = 1;
      this.chkBoxDontShow.Text = "Do not show this message again";
      this.chkBoxDontShow.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(587, 443);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnRun.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnRun.Location = new Point(12, 443);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new Size(75, 23);
      this.btnRun.TabIndex = 3;
      this.btnRun.Text = "Run";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Visible = false;
      this.btnRun.Click += new EventHandler(this.btnRun_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(676, 471);
      this.Controls.Add((Control) this.btnRun);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.chkBoxDontShow);
      this.Controls.Add((Control) this.panel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MessageScreen);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Message";
      this.FormClosing += new FormClosingEventHandler(this.MessageScreen_FormClosing);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
