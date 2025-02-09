// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ProductPricing.ProgressDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ProductPricing
{
  public class ProgressDialog : Form
  {
    private IContainer components;
    private ProgressBar progressBar;
    private Label lblPercentage;

    public ProgressDialog(int maximum)
    {
      this.InitializeComponent();
      this.progressBar.Maximum = maximum;
    }

    public void UpdateProgress(int progress)
    {
      int percentage = progress * 100 / this.progressBar.Maximum;
      if (this.progressBar.InvokeRequired)
      {
        this.progressBar.BeginInvoke((Delegate) (() =>
        {
          this.progressBar.Value = progress;
          this.lblPercentage.Text = percentage.ToString() + "%";
        }));
      }
      else
      {
        this.progressBar.Value = progress;
        this.lblPercentage.Text = percentage.ToString() + "%";
      }
    }

    public void SetIndeterminate(bool isIndeterminate)
    {
      if (this.progressBar.InvokeRequired)
        this.progressBar.BeginInvoke((Delegate) (() =>
        {
          if (isIndeterminate)
            this.progressBar.Style = ProgressBarStyle.Marquee;
          else
            this.progressBar.Style = ProgressBarStyle.Blocks;
        }));
      else if (isIndeterminate)
        this.progressBar.Style = ProgressBarStyle.Marquee;
      else
        this.progressBar.Style = ProgressBarStyle.Blocks;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.progressBar = new ProgressBar();
      this.lblPercentage = new Label();
      this.SuspendLayout();
      this.progressBar.Location = new Point(12, 11);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(287, 35);
      this.progressBar.TabIndex = 2;
      this.lblPercentage.AutoSize = true;
      this.lblPercentage.Location = new Point(264, 54);
      this.lblPercentage.Name = "lblPercentage";
      this.lblPercentage.Size = new Size(33, 13);
      this.lblPercentage.TabIndex = 3;
      this.lblPercentage.Text = "100%";
      this.lblPercentage.TextAlign = ContentAlignment.MiddleRight;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(311, 80);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblPercentage);
      this.Controls.Add((Control) this.progressBar);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ProgressDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Exporting Users";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
