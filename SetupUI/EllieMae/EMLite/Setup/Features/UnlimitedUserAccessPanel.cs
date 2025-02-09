// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.UnlimitedUserAccessPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.WebServices;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class UnlimitedUserAccessPanel : UserControl
  {
    private Label label2;
    private System.ComponentModel.Container components;
    private PictureBox pictureBox1;
    private Label label1;
    private ModuleLicense license;

    public UnlimitedUserAccessPanel(EncompassModule module, ModuleLicense license)
    {
      this.InitializeComponent();
      this.license = license;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (UnlimitedUserAccessPanel));
      this.label2 = new Label();
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(70, 51);
      this.label2.Name = "label2";
      this.label2.Size = new Size(336, 74);
      this.label2.TabIndex = 10;
      this.label2.Text = "Your license grants all users unlimited access to this add-on.";
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(18, 20);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 11;
      this.pictureBox1.TabStop = false;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(70, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(336, 28);
      this.label1.TabIndex = 12;
      this.label1.Text = "Add-On Enabled";
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label2);
      this.Name = nameof (UnlimitedUserAccessPanel);
      this.Size = new Size(438, 380);
      this.ResumeLayout(false);
    }
  }
}
