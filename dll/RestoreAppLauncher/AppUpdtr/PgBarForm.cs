// Decompiled with JetBrains decompiler
// Type: AppUpdtr.PgBarForm
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace AppUpdtr
{
  public class PgBarForm : Form
  {
    private IContainer components;
    private ProgressBar progressBar1;
    private Label lblMsg;

    public PgBarForm(string title, string filename)
    {
      this.InitializeComponent();
      this.Text = title;
      this.lblMsg.Text = "Updating " + filename + ". This may take a couple of minutes. Please wait...";
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

    public int Step
    {
      get => this.progressBar1.Step;
      set => this.progressBar1.Step = value;
    }

    public void PerformStep()
    {
      this.progressBar1.PerformStep();
      Application.DoEvents();
    }

    public void ShowProgressBar()
    {
      this.Show();
      Application.DoEvents();
    }

    public void CloseProgressBar() => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PgBarForm));
      this.progressBar1 = new ProgressBar();
      this.lblMsg = new Label();
      this.SuspendLayout();
      this.progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar1.Location = new Point(0, 26);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(447, 23);
      this.progressBar1.TabIndex = 0;
      this.lblMsg.AutoSize = true;
      this.lblMsg.Location = new Point(9, 7);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new Size(282, 13);
      this.lblMsg.TabIndex = 1;
      this.lblMsg.Text = "Updating file. This may take up to 2 minutes. Please wait...";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(447, 50);
      this.Controls.Add((Control) this.lblMsg);
      this.Controls.Add((Control) this.progressBar1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (PgBarForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "ProgressBarForm";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
