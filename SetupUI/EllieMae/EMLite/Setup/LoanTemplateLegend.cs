// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanTemplateLegend
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanTemplateLegend : UserControl
  {
    private IContainer components;
    private Label label3;
    private Label label2;
    private PictureBox picBoxThisGroupAndBelow;
    private PictureBox picBoxThisUser;

    public LoanTemplateLegend() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label3 = new Label();
      this.label2 = new Label();
      this.picBoxThisGroupAndBelow = new PictureBox();
      this.picBoxThisUser = new PictureBox();
      ((ISupportInitialize) this.picBoxThisGroupAndBelow).BeginInit();
      ((ISupportInitialize) this.picBoxThisUser).BeginInit();
      this.SuspendLayout();
      this.label3.AutoSize = true;
      this.label3.Location = new Point(29, 28);
      this.label3.Name = "label3";
      this.label3.Size = new Size(155, 13);
      this.label3.TabIndex = 11;
      this.label3.Text = "The group can access this item";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(29, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(261, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "The group can access all items in this level and below";
      this.picBoxThisGroupAndBelow.Image = (Image) Resources.members_this_group_and_below;
      this.picBoxThisGroupAndBelow.Location = new Point(7, 5);
      this.picBoxThisGroupAndBelow.Name = "picBoxThisGroupAndBelow";
      this.picBoxThisGroupAndBelow.Size = new Size(16, 16);
      this.picBoxThisGroupAndBelow.TabIndex = 7;
      this.picBoxThisGroupAndBelow.TabStop = false;
      this.picBoxThisUser.Image = (Image) Resources.member_group;
      this.picBoxThisUser.Location = new Point(7, 25);
      this.picBoxThisUser.Name = "picBoxThisUser";
      this.picBoxThisUser.Size = new Size(16, 16);
      this.picBoxThisUser.TabIndex = 6;
      this.picBoxThisUser.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.picBoxThisGroupAndBelow);
      this.Controls.Add((Control) this.picBoxThisUser);
      this.Name = nameof (LoanTemplateLegend);
      this.Size = new Size(360, 49);
      ((ISupportInitialize) this.picBoxThisGroupAndBelow).EndInit();
      ((ISupportInitialize) this.picBoxThisUser).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
