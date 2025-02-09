// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.ConfirmDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class ConfirmDialog : Form
  {
    private Label label1;
    private Button yesButton;
    private Button noButton;
    private CheckBox applyAllCheckBox;
    private System.ComponentModel.Container components;
    private bool applyToAll;

    public ConfirmDialog(string title, string msg, bool showApplyToAllCheckBox)
    {
      this.InitializeComponent();
      this.Text = title;
      this.label1.Text = msg;
      this.applyAllCheckBox.Visible = showApplyToAllCheckBox;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public bool ApplyToAll => this.applyToAll;

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.yesButton = new Button();
      this.noButton = new Button();
      this.applyAllCheckBox = new CheckBox();
      this.SuspendLayout();
      this.label1.Location = new Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(316, 72);
      this.label1.TabIndex = 0;
      this.yesButton.DialogResult = DialogResult.Yes;
      this.yesButton.Location = new Point(172, 96);
      this.yesButton.Name = "yesButton";
      this.yesButton.TabIndex = 2;
      this.yesButton.Text = "&Yes";
      this.noButton.DialogResult = DialogResult.No;
      this.noButton.Location = new Point(252, 96);
      this.noButton.Name = "noButton";
      this.noButton.TabIndex = 1;
      this.noButton.Text = "&No";
      this.applyAllCheckBox.Location = new Point(12, 96);
      this.applyAllCheckBox.Name = "applyAllCheckBox";
      this.applyAllCheckBox.Size = new Size(156, 24);
      this.applyAllCheckBox.TabIndex = 3;
      this.applyAllCheckBox.Text = "Apply to all items";
      this.applyAllCheckBox.CheckedChanged += new EventHandler(this.applyAllCheckBox_CheckedChanged);
      this.AcceptButton = (IButtonControl) this.noButton;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.noButton;
      this.ClientSize = new Size(338, 128);
      this.Controls.Add((Control) this.applyAllCheckBox);
      this.Controls.Add((Control) this.noButton);
      this.Controls.Add((Control) this.yesButton);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConfirmDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Delete";
      this.ResumeLayout(false);
    }

    private void applyAllCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      this.applyToAll = this.applyAllCheckBox.Checked;
    }
  }
}
