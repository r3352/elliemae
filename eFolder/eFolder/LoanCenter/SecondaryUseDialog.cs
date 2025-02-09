// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.SecondaryUseDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class SecondaryUseDialog : Form
  {
    private IContainer components;
    private Label infoLbl;
    private Button noBtn;
    private Button yesBtn;
    private LinkLabel infoLnk;

    public SecondaryUseDialog() => this.InitializeComponent();

    private void infoLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Session.MainScreen.OpenURL("http://help.icemortgagetechnology.com/encompass/secondary_use.htm", "Secondary Use", 500, 500);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SecondaryUseDialog));
      this.infoLbl = new Label();
      this.noBtn = new Button();
      this.yesBtn = new Button();
      this.infoLnk = new LinkLabel();
      this.SuspendLayout();
      this.infoLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.infoLbl.Location = new Point(12, 12);
      this.infoLbl.Name = "infoLbl";
      this.infoLbl.Size = new Size(395, 60);
      this.infoLbl.TabIndex = 0;
      this.infoLbl.Text = componentResourceManager.GetString("infoLbl.Text");
      this.noBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.noBtn.DialogResult = DialogResult.No;
      this.noBtn.Location = new Point(324, 86);
      this.noBtn.Name = "noBtn";
      this.noBtn.Size = new Size(75, 22);
      this.noBtn.TabIndex = 1;
      this.noBtn.Text = "No";
      this.yesBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.yesBtn.DialogResult = DialogResult.Yes;
      this.yesBtn.Location = new Point(243, 86);
      this.yesBtn.Name = "yesBtn";
      this.yesBtn.Size = new Size(75, 22);
      this.yesBtn.TabIndex = 2;
      this.yesBtn.Text = "Yes";
      this.infoLnk.AutoSize = true;
      this.infoLnk.Location = new Point(12, 88);
      this.infoLnk.Name = "infoLnk";
      this.infoLnk.Size = new Size(52, 14);
      this.infoLnk.TabIndex = 3;
      this.infoLnk.TabStop = true;
      this.infoLnk.Text = "More Info";
      this.infoLnk.LinkClicked += new LinkLabelLinkClickedEventHandler(this.infoLnk_LinkClicked);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.noBtn;
      this.ClientSize = new Size(412, 118);
      this.Controls.Add((Control) this.infoLnk);
      this.Controls.Add((Control) this.noBtn);
      this.Controls.Add((Control) this.yesBtn);
      this.Controls.Add((Control) this.infoLbl);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SecondaryUseDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Secondary Use";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
