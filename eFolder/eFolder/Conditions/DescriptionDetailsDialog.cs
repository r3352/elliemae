// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.DescriptionDetailsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host.BrowserControls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class DescriptionDetailsDialog : Form
  {
    private IContainer components;
    private Button btnClose;
    private BorderPanel pnlBrowser;
    private EncWebFormBrowserControl encFormBrowser;

    public DescriptionDetailsDialog(string details)
    {
      this.InitializeComponent();
      this.loadDetails(details);
    }

    private void loadDetails(string details)
    {
      string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("htm");
      using (StreamWriter text = File.CreateText(nameWithExtension))
        text.Write(details);
      this.encFormBrowser.Navigate(nameWithExtension);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnClose = new Button();
      this.pnlBrowser = new BorderPanel();
      this.encFormBrowser = BrowserFactory.GetWebBrowserInstance();
      this.pnlBrowser.SuspendLayout();
      this.SuspendLayout();
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(454, 228);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.pnlBrowser.Controls.Add((Control) this.encFormBrowser);
      this.pnlBrowser.Location = new Point(12, 12);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(516, 208);
      this.pnlBrowser.TabIndex = 0;
      this.encFormBrowser.Dock = DockStyle.Fill;
      this.encFormBrowser.Location = new Point(1, 1);
      this.encFormBrowser.MinimumSize = new Size(20, 20);
      this.encFormBrowser.Name = "webBrowser";
      this.encFormBrowser.Size = new Size(514, 206);
      this.encFormBrowser.TabIndex = 1;
      this.AcceptButton = (IButtonControl) this.btnClose;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(538, 260);
      this.Controls.Add((Control) this.pnlBrowser);
      this.Controls.Add((Control) this.btnClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DescriptionDetailsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Description Details";
      this.pnlBrowser.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
