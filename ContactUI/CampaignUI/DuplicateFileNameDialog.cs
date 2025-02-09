// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.DuplicateFileNameDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class DuplicateFileNameDialog : Form
  {
    private string duplicateFileName = string.Empty;
    private TextBox txtFileName;
    private Label lblAlreadyExists;
    private Label lblFileName;
    private Button btnOverWrite;
    private Button btnCancel;
    private Button btnRename;
    private PictureBox picWarning;
    private Label lblAction;
    private System.ComponentModel.Container components;

    public string FileName
    {
      get => this.txtFileName.Text;
      set => this.txtFileName.Text = value;
    }

    public DuplicateFileNameDialog(string duplicateFileName)
    {
      this.duplicateFileName = duplicateFileName.Trim();
      this.InitializeComponent();
      this.lblAlreadyExists.Text = string.Format("A file with the name '{0}' already exists.", (object) this.duplicateFileName);
      this.txtFileName.Text = this.duplicateFileName;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void txtFileName_TextChanged(object sender, EventArgs e)
    {
      this.btnRename.Enabled = string.Empty != this.txtFileName.Text && this.duplicateFileName != this.txtFileName.Text.Trim();
    }

    private void btnOverWrite_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Yes;
      this.Close();
    }

    private void btnRename_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.No;
      this.Close();
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DuplicateFileNameDialog));
      this.lblAlreadyExists = new Label();
      this.lblFileName = new Label();
      this.txtFileName = new TextBox();
      this.btnOverWrite = new Button();
      this.btnCancel = new Button();
      this.btnRename = new Button();
      this.picWarning = new PictureBox();
      this.lblAction = new Label();
      ((ISupportInitialize) this.picWarning).BeginInit();
      this.SuspendLayout();
      this.lblAlreadyExists.Location = new Point(82, 16);
      this.lblAlreadyExists.Name = "lblAlreadyExists";
      this.lblAlreadyExists.Size = new Size(295, 23);
      this.lblAlreadyExists.TabIndex = 0;
      this.lblAlreadyExists.Text = "A file with the name 'filename' already exists.";
      this.lblAlreadyExists.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFileName.AutoSize = true;
      this.lblFileName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblFileName.Location = new Point(16, 72);
      this.lblFileName.Name = "lblFileName";
      this.lblFileName.Size = new Size(61, 13);
      this.lblFileName.TabIndex = 1;
      this.lblFileName.Text = "New name:";
      this.lblFileName.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFileName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtFileName.Location = new Point(82, 68);
      this.txtFileName.MaxLength = 64;
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.Size = new Size(447, 20);
      this.txtFileName.TabIndex = 2;
      this.txtFileName.TextChanged += new EventHandler(this.txtFileName_TextChanged);
      this.btnOverWrite.Location = new Point(164, 111);
      this.btnOverWrite.Name = "btnOverWrite";
      this.btnOverWrite.Size = new Size(75, 23);
      this.btnOverWrite.TabIndex = 3;
      this.btnOverWrite.Text = "&Overwrite";
      this.btnOverWrite.Click += new EventHandler(this.btnOverWrite_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(326, 111);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnRename.Enabled = false;
      this.btnRename.Location = new Point(245, 111);
      this.btnRename.Name = "btnRename";
      this.btnRename.Size = new Size(75, 23);
      this.btnRename.TabIndex = 4;
      this.btnRename.Text = "&Rename";
      this.btnRename.UseVisualStyleBackColor = true;
      this.btnRename.Click += new EventHandler(this.btnRename_Click);
      this.picWarning.Image = (Image) componentResourceManager.GetObject("picWarning.Image");
      this.picWarning.Location = new Point(16, 16);
      this.picWarning.Name = "picWarning";
      this.picWarning.Size = new Size(32, 32);
      this.picWarning.SizeMode = PictureBoxSizeMode.StretchImage;
      this.picWarning.TabIndex = 18;
      this.picWarning.TabStop = false;
      this.lblAction.AutoSize = true;
      this.lblAction.Location = new Point(82, 43);
      this.lblAction.Name = "lblAction";
      this.lblAction.Size = new Size(447, 13);
      this.lblAction.TabIndex = 19;
      this.lblAction.Text = "Would you like to Overwrite the existing file, Rename the current file, or Cancel the operation?";
      this.lblAction.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(565, 148);
      this.Controls.Add((Control) this.lblAction);
      this.Controls.Add((Control) this.picWarning);
      this.Controls.Add((Control) this.btnRename);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOverWrite);
      this.Controls.Add((Control) this.lblFileName);
      this.Controls.Add((Control) this.txtFileName);
      this.Controls.Add((Control) this.lblAlreadyExists);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DuplicateFileNameDialog);
      this.ShowInTaskbar = false;
      this.Text = "Encompass";
      ((ISupportInitialize) this.picWarning).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
