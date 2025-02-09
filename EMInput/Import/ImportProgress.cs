// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportProgress
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ImportProgress : Form
  {
    private ProgressBar progressBar;
    public Button cancelBtn;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    public Label label1;
    public Label statusLbl;
    public Label progressLbl;
    public Label fileLbl;
    private System.ComponentModel.Container components;

    public ImportProgress(int fileCount)
    {
      this.InitializeComponent();
      this.Cursor = Cursors.WaitCursor;
      this.progressBar.Step = 1;
      this.progressBar.Maximum = fileCount;
    }

    internal string CurrentFile
    {
      set
      {
        this.statusLbl.Text = value;
        this.progressBar.PerformStep();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.progressBar = new ProgressBar();
      this.cancelBtn = new Button();
      this.statusLbl = new Label();
      this.groupBox1 = new GroupBox();
      this.fileLbl = new Label();
      this.groupBox2 = new GroupBox();
      this.progressLbl = new Label();
      this.label1 = new Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.progressBar.Location = new Point(16, 64);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(360, 20);
      this.progressBar.TabIndex = 6;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(334, 264);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Cancel";
      this.statusLbl.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.statusLbl.ForeColor = Color.Blue;
      this.statusLbl.Location = new Point(104, 24);
      this.statusLbl.Name = "statusLbl";
      this.statusLbl.Size = new Size(288, 56);
      this.statusLbl.TabIndex = 10;
      this.statusLbl.TextAlign = ContentAlignment.MiddleCenter;
      this.groupBox1.Controls.Add((Control) this.fileLbl);
      this.groupBox1.Controls.Add((Control) this.statusLbl);
      this.groupBox1.Location = new Point(8, 160);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(400, 96);
      this.groupBox1.TabIndex = 11;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Status Indicator";
      this.fileLbl.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.fileLbl.Location = new Point(16, 40);
      this.fileLbl.Name = "fileLbl";
      this.fileLbl.Size = new Size(88, 20);
      this.fileLbl.TabIndex = 11;
      this.fileLbl.Text = "Importing File:";
      this.groupBox2.Controls.Add((Control) this.progressLbl);
      this.groupBox2.Controls.Add((Control) this.label1);
      this.groupBox2.Controls.Add((Control) this.progressBar);
      this.groupBox2.Location = new Point(8, 16);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(400, 120);
      this.groupBox2.TabIndex = 12;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Progress Indicator";
      this.progressLbl.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.progressLbl.ForeColor = Color.Tomato;
      this.progressLbl.Location = new Point(164, 30);
      this.progressLbl.Name = "progressLbl";
      this.progressLbl.Size = new Size(184, 21);
      this.progressLbl.TabIndex = 7;
      this.label1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(28, 30);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 20);
      this.label1.TabIndex = 6;
      this.label1.Text = "Importing Files From:";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(416, 293);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportProgress);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Progress and Status Indicator";
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
