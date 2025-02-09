// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FolderSettingsDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FolderSettingsDialog : Form
  {
    private string[] folders;
    private bool originalFolderType;
    private IContainer components;
    private TextBox txtBoxFolderName;
    private Label label1;
    private Button btnOK;
    private Button btnCancel;
    private CheckBox chkBoxArchiveFolder;
    private CheckBox chkDupLoan;

    public FolderSettingsDialog(
      string[] folders,
      string folderName,
      bool isArchiveFolder,
      bool dupLoanCheck)
    {
      this.InitializeComponent();
      this.folders = folders;
      this.txtBoxFolderName.Text = (folderName ?? "").Trim();
      this.chkDupLoan.Checked = dupLoanCheck;
      this.originalFolderType = isArchiveFolder;
      if (this.folders != null)
        return;
      this.Text = "Edit Loan Folder";
      this.txtBoxFolderName.ReadOnly = true;
      this.chkBoxArchiveFolder.Checked = isArchiveFolder;
      if (this.txtBoxFolderName.Text == SystemSettings.TrashFolder)
      {
        this.chkBoxArchiveFolder.Checked = false;
        this.chkBoxArchiveFolder.Enabled = false;
        this.chkDupLoan.Checked = false;
        this.chkDupLoan.Enabled = false;
      }
      else
      {
        if (!(this.txtBoxFolderName.Text == SystemSettings.ArchiveFolder))
          return;
        this.chkBoxArchiveFolder.Checked = true;
        this.chkBoxArchiveFolder.Enabled = false;
      }
    }

    public string FolderName => this.txtBoxFolderName.Text.Trim();

    public bool IsArchiveFolder => this.chkBoxArchiveFolder.Checked;

    public bool IncludeInDuplicateLoanCheck => this.chkDupLoan.Checked;

    public bool IsFolderTypeChanged => this.originalFolderType != this.chkBoxArchiveFolder.Checked;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.folders == null)
        return;
      if (this.FolderName == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Folder name cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      else if (this.FolderName.IndexOfAny(SystemUtil.InvalidFilenameCharArray) >= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A folder name cannot contain any of the following characters: \\ / : * ? \" < > |", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      else if (this.FolderName.Length > 80)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The length of folder name cannot exceed 80 characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        foreach (string folder in this.folders)
        {
          if (string.Compare(folder, this.FolderName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A folder with the name you specified already exists. Please specify a different folder name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.DialogResult = DialogResult.None;
            break;
          }
        }
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
      this.txtBoxFolderName = new TextBox();
      this.label1 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.chkBoxArchiveFolder = new CheckBox();
      this.chkDupLoan = new CheckBox();
      this.SuspendLayout();
      this.txtBoxFolderName.Location = new Point(86, 10);
      this.txtBoxFolderName.MaxLength = 80;
      this.txtBoxFolderName.Name = "txtBoxFolderName";
      this.txtBoxFolderName.Size = new Size(217, 20);
      this.txtBoxFolderName.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(67, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Folder Name";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(147, 78);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "Save";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(228, 78);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.chkBoxArchiveFolder.AutoSize = true;
      this.chkBoxArchiveFolder.Location = new Point(86, 38);
      this.chkBoxArchiveFolder.Name = "chkBoxArchiveFolder";
      this.chkBoxArchiveFolder.Size = new Size(94, 17);
      this.chkBoxArchiveFolder.TabIndex = 4;
      this.chkBoxArchiveFolder.Text = "Archive Folder";
      this.chkBoxArchiveFolder.UseVisualStyleBackColor = true;
      this.chkDupLoan.AutoSize = true;
      this.chkDupLoan.Location = new Point(86, 55);
      this.chkDupLoan.Name = "chkDupLoan";
      this.chkDupLoan.Size = new Size(181, 17);
      this.chkDupLoan.TabIndex = 5;
      this.chkDupLoan.Text = "Include in Duplicate Loan Check";
      this.chkDupLoan.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(316, 111);
      this.Controls.Add((Control) this.chkDupLoan);
      this.Controls.Add((Control) this.chkBoxArchiveFolder);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtBoxFolderName);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FolderSettingsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create Loan Folder";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
