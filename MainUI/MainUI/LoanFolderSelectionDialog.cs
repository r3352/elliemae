// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoanFolderSelectionDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class LoanFolderSelectionDialog : Form
  {
    private IContainer components;
    private Label label1;
    private ComboBox cmbFolders;
    private DialogButtons dlgButtons;

    public LoanFolderSelectionDialog(string[] folders, string defaultFolder)
    {
      this.InitializeComponent();
      this.cmbFolders.Items.Clear();
      this.cmbFolders.Items.AddRange((object[]) folders);
      if ((defaultFolder ?? "") != "")
        ClientCommonUtils.PopulateDropdown(this.cmbFolders, (object) defaultFolder, false);
      if (this.cmbFolders.SelectedIndex >= 0)
        return;
      this.cmbFolders.SelectedIndex = 0;
    }

    public string SelectedFolder => this.cmbFolders.SelectedItem.ToString();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.cmbFolders = new ComboBox();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(294, 18);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select the folder for the new loan.";
      this.cmbFolders.FormattingEnabled = true;
      this.cmbFolders.Location = new Point(8, 26);
      this.cmbFolders.Name = "cmbFolders";
      this.cmbFolders.Size = new Size(294, 22);
      this.cmbFolders.TabIndex = 1;
      this.dlgButtons.DialogResult = DialogResult.OK;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 55);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(311, 44);
      this.dlgButtons.TabIndex = 2;
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(311, 99);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.cmbFolders);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanFolderSelectionDialog);
      this.Text = "New Loan";
      this.ResumeLayout(false);
    }
  }
}
