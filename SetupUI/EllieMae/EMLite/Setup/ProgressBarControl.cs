// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ProgressBarControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ProgressBarControl : UserControl
  {
    private IContainer components;
    private Label lblProcessText;
    private ProgressBar progressBar;

    public ProgressBarControl() => this.InitializeComponent();

    public void SetProgress(int progress) => this.progressBar.Value = progress;

    public void SetProgressText(string progressText) => this.lblProcessText.Text = progressText;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblProcessText = new Label();
      this.progressBar = new ProgressBar();
      this.SuspendLayout();
      this.lblProcessText.Location = new Point(3, 4);
      this.lblProcessText.Name = "lblProcessText";
      this.lblProcessText.Size = new Size(278, 15);
      this.lblProcessText.TabIndex = 0;
      this.lblProcessText.Text = "Please wait....";
      this.lblProcessText.TextAlign = ContentAlignment.TopCenter;
      this.progressBar.Location = new Point(19, 22);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(243, 23);
      this.progressBar.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.progressBar);
      this.Controls.Add((Control) this.lblProcessText);
      this.Name = nameof (ProgressBarControl);
      this.Size = new Size(284, 60);
      this.ResumeLayout(false);
    }
  }
}
