// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.ExportToLocalDialog
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class ExportToLocalDialog : Form
  {
    private string savedPath = string.Empty;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Label label1;
    private TextBox txtFolder;
    private StandardIconButton btnFolder;
    private TextBox txtFileName;
    private Label label2;

    public ExportToLocalDialog(string dialogTitle, string defaultFileName)
    {
      this.InitializeComponent();
      if (!string.IsNullOrEmpty(dialogTitle))
        this.Text = dialogTitle;
      this.savedPath = Path.GetTempPath() + "ExportToLocal" + new FileInfo(defaultFileName).Name.Replace(new FileInfo(defaultFileName).Extension, string.Empty) + ".txt";
      if (File.Exists(this.savedPath))
      {
        try
        {
          using (StreamReader streamReader = new StreamReader(this.savedPath, Encoding.ASCII))
            this.txtFolder.Text = streamReader.ReadToEnd();
        }
        catch (Exception ex)
        {
          this.txtFolder.Text = string.Empty;
        }
      }
      this.txtFileName.Text = defaultFileName;
    }

    private void btnFolder_Click(object sender, EventArgs e)
    {
      using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
      {
        if (this.txtFolder.Text.Trim() != string.Empty && Directory.Exists(this.txtFolder.Text.Trim()))
          folderBrowserDialog.SelectedPath = this.txtFolder.Text;
        folderBrowserDialog.Description = "Please select a target folder:";
        folderBrowserDialog.ShowNewFolderButton = false;
        if (folderBrowserDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || string.Compare(this.txtFolder.Text, folderBrowserDialog.SelectedPath, true) == 0)
          return;
        this.txtFolder.Text = folderBrowserDialog.SelectedPath;
        try
        {
          using (StreamWriter streamWriter = new StreamWriter(this.savedPath, false, Encoding.ASCII))
            streamWriter.Write(this.txtFolder.Text);
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.txtFolder.Text.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a target folder.");
      }
      else if (!Directory.Exists(this.txtFolder.Text.Trim()))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Specified folder does not exist or is not accessible.");
      }
      else if (this.txtFileName.Text.Trim() == "")
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Please enter a file name.");
      }
      else
      {
        if (File.Exists(this.txtFolder.Text.Trim() + "\\" + this.txtFileName.Text.Trim()) && Utils.Dialog((IWin32Window) this, "The file " + this.txtFileName.Text + " currently exists. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.DialogResult = DialogResult.OK;
      }
    }

    public string SelectedFolder => this.txtFolder.Text.Trim();

    public string SelectedFileName => this.txtFileName.Text.Trim();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.label1 = new Label();
      this.txtFolder = new TextBox();
      this.btnFolder = new StandardIconButton();
      this.txtFileName = new TextBox();
      this.label2 = new Label();
      ((ISupportInitialize) this.btnFolder).BeginInit();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 70);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(393, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Export to";
      this.txtFolder.Location = new Point(67, 13);
      this.txtFolder.Name = "txtFolder";
      this.txtFolder.Size = new Size(293, 20);
      this.txtFolder.TabIndex = 2;
      this.btnFolder.BackColor = Color.Transparent;
      this.btnFolder.Location = new Point(365, 16);
      this.btnFolder.MouseDownImage = (Image) null;
      this.btnFolder.Name = "btnFolder";
      this.btnFolder.Size = new Size(16, 16);
      this.btnFolder.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFolder.TabIndex = 3;
      this.btnFolder.TabStop = false;
      this.btnFolder.Click += new EventHandler(this.btnFolder_Click);
      this.txtFileName.Location = new Point(67, 35);
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.Size = new Size(293, 20);
      this.txtFileName.TabIndex = 5;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "File Name";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(393, 114);
      this.Controls.Add((Control) this.txtFileName);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnFolder);
      this.Controls.Add((Control) this.txtFolder);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ExportToLocalDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Export";
      ((ISupportInitialize) this.btnFolder).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
