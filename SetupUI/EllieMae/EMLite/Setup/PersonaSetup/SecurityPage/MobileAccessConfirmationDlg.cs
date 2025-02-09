// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.MobileAccessConfirmationDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.BrowserControls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class MobileAccessConfirmationDlg : Form
  {
    private IContainer components;
    private Panel panel1;
    private CheckBox checkBox1;
    private Button btnOK;
    private Button btnCancel;
    private EncWebFormBrowserControl webBrowser1;

    public MobileAccessConfirmationDlg() => this.InitializeComponent();

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.checkBox1.Checked;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.checkBox1 = new CheckBox();
      this.webBrowser1 = BrowserFactory.GetWebBrowserInstance();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 228);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(565, 40);
      this.panel1.TabIndex = 1;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(396, 9);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(477, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new Point(12, 210);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(202, 17);
      this.checkBox1.TabIndex = 2;
      this.checkBox1.Text = "I understand and accept these terms.";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.webBrowser1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.webBrowser1.Location = new Point(13, 13);
      this.webBrowser1.MinimumSize = new Size(20, 20);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.Size = new Size(540, 191);
      this.webBrowser1.TabIndex = 3;
      this.webBrowser1.Navigate("https://download.elliemae.com/encompass/Static/Persona/Mobile Access Confirmation.html");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(565, 268);
      this.Controls.Add((Control) this.webBrowser1);
      this.Controls.Add((Control) this.checkBox1);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (MobileAccessConfirmationDlg);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Mobile Access Confirmation";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
