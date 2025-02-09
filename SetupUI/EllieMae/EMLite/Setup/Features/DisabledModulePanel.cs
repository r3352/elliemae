// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.DisabledModulePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class DisabledModulePanel : UserControl
  {
    private Label label3;
    private PictureBox pictureBox1;
    private Label label1;
    private System.ComponentModel.Container components;

    public DisabledModulePanel() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (DisabledModulePanel));
      this.label3 = new Label();
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(72, 20);
      this.label3.Name = "label3";
      this.label3.Size = new Size(394, 24);
      this.label3.TabIndex = 12;
      this.label3.Text = "Add-On Disabled";
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(18, 20);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 11;
      this.pictureBox1.TabStop = false;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(72, 50);
      this.label1.Name = "label1";
      this.label1.Size = new Size(394, 54);
      this.label1.TabIndex = 9;
      this.label1.Text = "This add-on has been disabled. To restore access to this add-on, contact an Ellie Mae account representative at 888-955-9100.";
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (DisabledModulePanel);
      this.Size = new Size(524, 410);
      this.ResumeLayout(false);
    }
  }
}
