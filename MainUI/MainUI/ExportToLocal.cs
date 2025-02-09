// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ExportToLocal
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class ExportToLocal : Form
  {
    public string folder;
    public string fileName;
    private IContainer components;
    private TextBox txtFileName;
    private Label label2;
    private StandardIconButton btnFolder;
    private TextBox txtFolder;
    private Label label1;
    private DialogButtons dialogButtons1;

    public ExportToLocal() => this.InitializeComponent();

    private void btnFolder_Click(object sender, EventArgs e)
    {
      using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
      {
        if (this.txtFolder.Text.Trim() != string.Empty && Directory.Exists(this.txtFolder.Text.Trim()))
          folderBrowserDialog.SelectedPath = this.txtFolder.Text;
        folderBrowserDialog.Description = "Please select a target folder:";
        folderBrowserDialog.ShowNewFolderButton = false;
        if (folderBrowserDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          if (string.Compare(this.txtFolder.Text, folderBrowserDialog.SelectedPath, true) != 0)
            this.txtFolder.Text = folderBrowserDialog.SelectedPath;
        }
      }
      this.folder = this.txtFolder.Text;
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      this.folder = this.txtFolder.Text;
      this.fileName = this.txtFileName.Text;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExportToLocal));
      this.txtFileName = new TextBox();
      this.label2 = new Label();
      this.btnFolder = new StandardIconButton();
      this.txtFolder = new TextBox();
      this.label1 = new Label();
      this.dialogButtons1 = new DialogButtons();
      ((ISupportInitialize) this.btnFolder).BeginInit();
      this.SuspendLayout();
      this.txtFileName.Location = new Point(67, 29);
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.Size = new Size(293, 20);
      this.txtFileName.TabIndex = 11;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "File Name";
      this.btnFolder.BackColor = Color.Transparent;
      this.btnFolder.Location = new Point(365, 10);
      this.btnFolder.MouseDownImage = (Image) null;
      this.btnFolder.Name = "btnFolder";
      this.btnFolder.Size = new Size(16, 16);
      this.btnFolder.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFolder.TabIndex = 9;
      this.btnFolder.TabStop = false;
      this.btnFolder.Click += new EventHandler(this.btnFolder_Click);
      this.txtFolder.Location = new Point(67, 7);
      this.txtFolder.Name = "txtFolder";
      this.txtFolder.ReadOnly = true;
      this.txtFolder.Size = new Size(293, 20);
      this.txtFolder.TabIndex = 8;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Export to";
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 66);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(407, 44);
      this.dialogButtons1.TabIndex = 6;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(407, 110);
      this.Controls.Add((Control) this.txtFileName);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnFolder);
      this.Controls.Add((Control) this.txtFolder);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ExportToLocal);
      this.Text = "Export";
      ((ISupportInitialize) this.btnFolder).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
