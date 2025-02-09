// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.ImportDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class ImportDialog : Form
  {
    private FileListBox fileListBox;
    private DirListBox dirListBox;
    private DriveListBox driveListBox;
    private Label label1;
    private Label label2;
    private Label label3;
    private GroupBox groupBox1;
    private Button btnCancel;
    private Button btnOK;
    private CheckBox keepBox;
    private System.ComponentModel.Container components;
    private string sourcePath;
    private string[] sourceFile;
    private bool keepField;

    public ImportDialog()
    {
      this.InitializeComponent();
      this.InitializeDrives();
      this.keepBox.Checked = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    internal string SourcePath => this.sourcePath;

    internal string[] SourceFile => this.sourceFile;

    internal bool KeepField => this.keepField;

    private void InitializeComponent()
    {
      this.fileListBox = new FileListBox();
      this.dirListBox = new DirListBox();
      this.driveListBox = new DriveListBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.groupBox1 = new GroupBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.keepBox = new CheckBox();
      this.SuspendLayout();
      this.fileListBox.FormattingEnabled = true;
      this.fileListBox.Location = new Point(256, 60);
      this.fileListBox.Name = "fileListBox";
      this.fileListBox.Pattern = "*.DOC;*.RTF";
      this.fileListBox.SelectionMode = SelectionMode.MultiExtended;
      this.fileListBox.Size = new Size(234, 264);
      this.fileListBox.TabIndex = 7;
      this.dirListBox.FormattingEnabled = true;
      this.dirListBox.IntegralHeight = false;
      this.dirListBox.Location = new Point(8, 60);
      this.dirListBox.Name = "dirListBox";
      this.dirListBox.Size = new Size(234, 260);
      this.dirListBox.TabIndex = 6;
      this.dirListBox.Change += new EventHandler(this.dirListBox_Change);
      this.driveListBox.FormattingEnabled = true;
      this.driveListBox.Location = new Point(64, 8);
      this.driveListBox.Name = "driveListBox";
      this.driveListBox.Size = new Size(176, 21);
      this.driveListBox.TabIndex = 5;
      this.driveListBox.SelectedIndexChanged += new EventHandler(this.driveListBox_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(45, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Look in:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(39, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Folder:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(260, 40);
      this.label3.Name = "label3";
      this.label3.Size = new Size(31, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Files:";
      this.groupBox1.Location = new Point(10, 366);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(478, 4);
      this.groupBox1.TabIndex = 11;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "groupBox1";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(413, 376);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Location = new Point(332, 376);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 12;
      this.btnOK.Text = "&Import";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.keepBox.Location = new Point(16, 324);
      this.keepBox.Name = "keepBox";
      this.keepBox.Size = new Size(472, 40);
      this.keepBox.TabIndex = 14;
      this.keepBox.Text = "Keep data fields that exist in the custom forms you are importing. Please note that data fields from other applications are not compatible with Encompass.";
      this.keepBox.TextAlign = ContentAlignment.BottomLeft;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(500, 408);
      this.Controls.Add((Control) this.keepBox);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.fileListBox);
      this.Controls.Add((Control) this.dirListBox);
      this.Controls.Add((Control) this.driveListBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Custom Form";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void InitializeDrives()
    {
      this.driveListBox.Drive = SystemSettings.LocalAppDir;
      this.dirListBox.Path = SystemSettings.LocalAppDir;
      this.fileListBox.Path = SystemSettings.LocalAppDir;
    }

    private void driveListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.dirListBox.Path = this.driveListBox.Drive;
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Device " + this.driveListBox.Drive + " unavailable.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        string path = this.dirListBox.Path;
        this.driveListBox.Drive = this.dirListBox.Path.Substring(0, this.dirListBox.Path.IndexOf("\\"));
        this.dirListBox.Path = path;
      }
    }

    private void dirListBox_Change(object sender, EventArgs e)
    {
      this.fileListBox.Path = this.dirListBox.Path;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      ListBox.SelectedObjectCollection selectedItems = this.fileListBox.SelectedItems;
      if (selectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select one custom letter first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.sourceFile = new string[selectedItems.Count];
        for (int index = 0; index < selectedItems.Count; ++index)
          this.sourceFile[index] = selectedItems[index].ToString();
        this.sourcePath = this.fileListBox.Path;
        this.keepField = this.keepBox.Checked;
        this.DialogResult = DialogResult.OK;
      }
    }
  }
}
