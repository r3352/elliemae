// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.ProgressBarForm
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class ProgressBarForm : Form, IProgressBar
  {
    private IContainer components;
    private ProgressBar progressBar1;

    public ProgressBarForm(string title)
    {
      this.InitializeComponent();
      this.Text = title;
    }

    public string Title
    {
      get => this.Text;
      set => this.Text = value;
    }

    public int Minimum
    {
      get => this.progressBar1.Minimum;
      set => this.progressBar1.Minimum = value;
    }

    public int Maximum
    {
      get => this.progressBar1.Maximum;
      set => this.progressBar1.Maximum = value;
    }

    public int Value
    {
      get => this.progressBar1.Value;
      set
      {
        this.progressBar1.Value = value;
        Application.DoEvents();
      }
    }

    public void PerformStep()
    {
      this.progressBar1.PerformStep();
      Application.DoEvents();
    }

    public void ShowProgressBar() => this.Show();

    public void CloseProgressBar() => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProgressBarForm));
      this.progressBar1 = new ProgressBar();
      this.SuspendLayout();
      this.progressBar1.Location = new Point(0, 2);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(447, 23);
      this.progressBar1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(447, 26);
      this.Controls.Add((Control) this.progressBar1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (ProgressBarForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (ProgressBarForm);
      this.ResumeLayout(false);
    }
  }
}
