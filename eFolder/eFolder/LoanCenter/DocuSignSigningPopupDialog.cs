// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.DocuSignSigningPopupDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class DocuSignSigningPopupDialog : Form
  {
    private IContainer components;
    private WebBrowser webBrowser;

    public DocuSignSigningPopupDialog()
    {
      this.InitializeComponent();
      this.setWindowSize();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      if (this.Owner != null)
        this.Owner.Enabled = true;
      base.OnFormClosing(e);
    }

    public WebBrowser Browser => this.webBrowser;

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.9);
        this.Height = Convert.ToInt32((double) form.Height * 0.9);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.9);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.9);
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
      this.webBrowser = new WebBrowser();
      this.SuspendLayout();
      this.webBrowser.Dock = DockStyle.Fill;
      this.webBrowser.Location = new Point(0, 0);
      this.webBrowser.MinimumSize = new Size(20, 20);
      this.webBrowser.Name = "webBrowser";
      this.webBrowser.Size = new Size(284, 262);
      this.webBrowser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(284, 262);
      this.Controls.Add((Control) this.webBrowser);
      this.Name = "DocuSignSigningPopup";
      this.Text = "eSign Documents";
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.ShowInTaskbar = false;
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.ResumeLayout(false);
    }
  }
}
