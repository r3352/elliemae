// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.NoInetAccessPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class NoInetAccessPanel : UserControl
  {
    private Label label1;
    private Button btnRetry;
    private PictureBox pictureBox1;
    private Label label3;
    private System.ComponentModel.Container components;

    public event EventHandler RetryAccess;

    public NoInetAccessPanel() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (NoInetAccessPanel));
      this.label1 = new Label();
      this.btnRetry = new Button();
      this.pictureBox1 = new PictureBox();
      this.label3 = new Label();
      this.SuspendLayout();
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(70, 47);
      this.label1.Name = "label1";
      this.label1.Size = new Size(340, 49);
      this.label1.TabIndex = 0;
      this.label1.Text = "An Internet connection is required to view or modify current license settings.";
      this.btnRetry.Location = new Point(70, 100);
      this.btnRetry.Name = "btnRetry";
      this.btnRetry.TabIndex = 2;
      this.btnRetry.Text = "Try Again";
      this.btnRetry.Click += new EventHandler(this.btnRetry_Click);
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(18, 20);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 3;
      this.pictureBox1.TabStop = false;
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(70, 20);
      this.label3.Name = "label3";
      this.label3.Size = new Size(340, 24);
      this.label3.TabIndex = 4;
      this.label3.Text = "Internet Access Required";
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.btnRetry);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (NoInetAccessPanel);
      this.Size = new Size(436, 360);
      this.ResumeLayout(false);
    }

    private void btnRetry_Click(object sender, EventArgs e)
    {
      if (this.RetryAccess == null)
        return;
      this.RetryAccess((object) this, EventArgs.Empty);
    }
  }
}
