// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MiCenterInfoForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.HelpAPI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MiCenterInfoForm : UserControl, IOnlineHelpTarget
  {
    private const string ClassName = "MiCenterInfoForm";
    private IContainer components;
    private Label InfoLabel;

    public MiCenterInfoForm(string labelToDisplay)
    {
      this.InitializeComponent();
      this.InfoLabel.Text = labelToDisplay;
    }

    public string GetHelpTargetName() => nameof (MiCenterInfoForm);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.InfoLabel = new Label();
      this.SuspendLayout();
      this.InfoLabel.Dock = DockStyle.Fill;
      this.InfoLabel.AutoSize = false;
      this.InfoLabel.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.InfoLabel.Location = new Point(0, 0);
      this.InfoLabel.Name = "InfoLabel";
      this.InfoLabel.TabIndex = 0;
      this.InfoLabel.Text = "MI Center";
      this.InfoLabel.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.InfoLabel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (MiCenterInfoForm);
      this.AutoSize = false;
      this.Dock = DockStyle.Fill;
      this.ResumeLayout(false);
    }
  }
}
