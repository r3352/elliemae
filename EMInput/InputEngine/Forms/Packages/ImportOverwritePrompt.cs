// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ImportOverwritePrompt
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Packages;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class ImportOverwritePrompt : Form
  {
    private Label lblPrompt;
    private Button btnYes;
    private Button btnNo;
    private Button btnCancel;
    private CheckBox chkAlways;
    private PictureBox pictureBox1;
    private System.ComponentModel.Container components;

    public ImportOverwritePrompt(ExportPackageObjectType objectType, string objectName)
    {
      this.InitializeComponent();
      string str1;
      string str2;
      switch (objectType)
      {
        case ExportPackageObjectType.InputForm:
          str1 = "Forms";
          str2 = "The custom form '" + objectName + "' already exists. Overwrite the existing form with the version in the package?";
          break;
        case ExportPackageObjectType.CodebaseAssembly:
          str1 = "Assemblies";
          str2 = "The form assembly '" + objectName + "' already exists and has a more recent file version than the one in the package. Overwrite the assembly with the version in the package?";
          break;
        case ExportPackageObjectType.LoanCustomField:
          str1 = "Fields";
          str2 = "The Custom Field '" + objectName + "' already exists. Overwrite the field with the information in the package?";
          break;
        case ExportPackageObjectType.Plugin:
          str1 = "Plugins";
          str2 = "The plugin '" + objectName + "' already exists. Overwrite the plugin with the version in the package?";
          break;
        case ExportPackageObjectType.CustomData:
          str1 = "Custom Data Objects";
          str2 = "The custom data object '" + objectName + "' already exists. Overwrite the custom data object with the one in the package?";
          break;
        default:
          throw new ArgumentException("Invalid ExportObjectType specified (" + (object) objectType + ")");
      }
      this.lblPrompt.Text = str2;
      this.chkAlways.Text = "Apply this decision to all " + str1;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportOverwritePrompt));
      this.lblPrompt = new Label();
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.btnCancel = new Button();
      this.chkAlways = new CheckBox();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblPrompt.Location = new Point(54, 10);
      this.lblPrompt.Name = "lblPrompt";
      this.lblPrompt.Size = new Size(316, 48);
      this.lblPrompt.TabIndex = 0;
      this.lblPrompt.Text = "(Prompt goes here)";
      this.btnYes.DialogResult = DialogResult.Yes;
      this.btnYes.Location = new Point(73, 102);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "&Yes";
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(155, 102);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 2;
      this.btnNo.Text = "&No";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(237, 102);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.chkAlways.Location = new Point(54, 64);
      this.chkAlways.Name = "chkAlways";
      this.chkAlways.Size = new Size(288, 20);
      this.chkAlways.TabIndex = 4;
      this.chkAlways.Text = "Apply this decision to all items";
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(10, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 5;
      this.pictureBox1.TabStop = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(384, 142);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.chkAlways);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.lblPrompt);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportOverwritePrompt);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Overwrite Data";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public bool ApplyToAll => this.chkAlways.Checked;

    public PackageImportConflictOption ShowOverwritePrompt(IWin32Window parent)
    {
      switch (this.ShowDialog(parent))
      {
        case DialogResult.Cancel:
          return PackageImportConflictOption.Abort;
        case DialogResult.Yes:
          return PackageImportConflictOption.Overwrite;
        default:
          return PackageImportConflictOption.Skip;
      }
    }
  }
}
