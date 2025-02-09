// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AnimationProgress
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class AnimationProgress : Form
  {
    private IContainer components;
    private PictureBox pictureBox1;

    public AnimationProgress() => this.InitializeComponent();

    private void pictureBox1_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) AnimatedProgessResource.progress;
      this.pictureBox1.Location = new Point(0, 1);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(49, 51);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(49, 51);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pictureBox1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (AnimationProgress);
      this.Text = "Progress";
      this.TransparencyKey = Color.Transparent;
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
