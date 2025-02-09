// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DeleteBorrowersDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DeleteBorrowersDialog : Form
  {
    private RadioButton cobPair;
    private RadioButton pairOpt;
    private DialogButtons dlgButtons;
    private System.ComponentModel.Container components;
    private bool deleteCob;

    public DeleteBorrowersDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cobPair = new RadioButton();
      this.pairOpt = new RadioButton();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.cobPair.Location = new Point(16, 38);
      this.cobPair.Name = "cobPair";
      this.cobPair.Size = new Size(168, 20);
      this.cobPair.TabIndex = 6;
      this.cobPair.Text = "Delete CoBorrower Only";
      this.pairOpt.Checked = true;
      this.pairOpt.Location = new Point(16, 14);
      this.pairOpt.Name = "pairOpt";
      this.pairOpt.Size = new Size(168, 20);
      this.pairOpt.TabIndex = 5;
      this.pairOpt.TabStop = true;
      this.pairOpt.Text = "Delete Borrower Pair";
      this.dlgButtons.ButtonAlignment = HorizontalAlignment.Center;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 71);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(194, 44);
      this.dlgButtons.TabIndex = 7;
      this.dlgButtons.OK += new EventHandler(this.okBtn_Click);
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(194, 115);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.cobPair);
      this.Controls.Add((Control) this.pairOpt);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DeleteBorrowersDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Delete";
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.deleteCob = !this.pairOpt.Checked;
      this.DialogResult = DialogResult.OK;
    }

    internal bool DeleteCob => this.deleteCob;
  }
}
