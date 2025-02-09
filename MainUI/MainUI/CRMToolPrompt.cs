// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.CRMToolPrompt
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class CRMToolPrompt : Form
  {
    private CRMToolPrompt.CRMOption option = CRMToolPrompt.CRMOption.Later;
    private IContainer components;
    private Label label1;
    private Button btnYes;
    private Button btnNo;
    private CheckBox chkDonotShow;

    public CRMToolPrompt() => this.InitializeComponent();

    public CRMToolPrompt.CRMOption SelectedOption => this.option;

    private void btnYes_Click(object sender, EventArgs e)
    {
      this.option = !this.chkDonotShow.Checked ? CRMToolPrompt.CRMOption.Launch : CRMToolPrompt.CRMOption.DonotShow;
      this.DialogResult = DialogResult.OK;
    }

    private void btnNo_Click(object sender, EventArgs e)
    {
      this.option = !this.chkDonotShow.Checked ? CRMToolPrompt.CRMOption.Later : CRMToolPrompt.CRMOption.DonotShow;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CRMToolPrompt));
      this.label1 = new Label();
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.chkDonotShow = new CheckBox();
      this.SuspendLayout();
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(362, 83);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(219, 126);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.Location = new Point(300, 126);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 2;
      this.btnNo.Text = "No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.chkDonotShow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkDonotShow.AutoSize = true;
      this.chkDonotShow.Location = new Point(16, 103);
      this.chkDonotShow.Name = "chkDonotShow";
      this.chkDonotShow.Size = new Size(182, 17);
      this.chkDonotShow.TabIndex = 3;
      this.chkDonotShow.Text = "Do not show this message again.";
      this.chkDonotShow.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(388, 161);
      this.Controls.Add((Control) this.chkDonotShow);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CRMToolPrompt);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Borrower Contact Synchronization";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public enum CRMOption
    {
      Launch,
      Later,
      DonotShow,
    }
  }
}
