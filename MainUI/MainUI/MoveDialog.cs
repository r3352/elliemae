// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.MoveDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class MoveDialog : Form, IHelp
  {
    private const string className = "MoveDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private ComboBox cboFolder;
    private Label label1;
    private System.ComponentModel.Container components;
    private DialogButtons dlgButtons;

    public MoveDialog(string[] folders, bool restoreLoanFromTrashFolder)
    {
      this.InitializeComponent();
      if (restoreLoanFromTrashFolder)
      {
        this.Text = "Restore Loans";
        this.label1.Text = "Restore selected loans into the following folder:";
      }
      else
      {
        this.Text = "Move Loans";
        this.label1.Text = "Move selected loans into the following folder:";
      }
      this.cboFolder.Items.AddRange((object[]) folders);
      for (int index = 0; index < folders.Length; ++index)
      {
        if (SystemSettings.ArchiveFolder.ToLower() != folders[index].ToLower() && SystemSettings.TrashFolder.ToLower() != folders[index].ToLower())
        {
          this.cboFolder.SelectedIndex = index;
          break;
        }
      }
      if (this.cboFolder.Items.Count <= 0 || this.cboFolder.SelectedIndex >= 0)
        return;
      this.cboFolder.SelectedIndex = 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboFolder = new ComboBox();
      this.label1 = new Label();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.cboFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFolder.Location = new Point(10, 28);
      this.cboFolder.Name = "cboFolder";
      this.cboFolder.Size = new Size(278, 22);
      this.cboFolder.TabIndex = 0;
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(280, 18);
      this.label1.TabIndex = 2;
      this.label1.Text = "Move selected loans to the following folder:";
      this.dlgButtons.DialogResult = DialogResult.OK;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 60);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(301, 44);
      this.dlgButtons.TabIndex = 3;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(301, 104);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboFolder);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MoveDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Move";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
    }

    public string Destination => (string) this.cboFolder.SelectedItem ?? "";

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (MoveDialog));
    }
  }
}
