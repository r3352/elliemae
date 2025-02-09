// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CompanySetupAddDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CompanySetupAddDialog : Form
  {
    private IContainer components;
    private ComboBox cmbImport;
    private Button btnOk;
    private Button btnCancel;

    public CompanySetupAddDialog(bool forLender)
    {
      this.InitializeComponent();
      if (forLender)
        this.cmbImport.Items.AddRange((object[]) new string[2]
        {
          "Import from Business Contacts",
          "Manually Add"
        });
      else
        this.cmbImport.Items.AddRange((object[]) new string[3]
        {
          "Import from TPO WebCenter",
          "Import from Business Contacts",
          "Manually Add"
        });
      this.cmbImport.SelectedIndex = 0;
    }

    public string ImportType => this.cmbImport.SelectedItem.ToString().ToLower();

    private void btnOk_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CompanySetupAddDialog));
      this.cmbImport = new ComboBox();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.cmbImport.BackColor = SystemColors.Window;
      this.cmbImport.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbImport.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.cmbImport, "cmbImport");
      this.cmbImport.Name = "cmbImport";
      componentResourceManager.ApplyResources((object) this.btnOk, "btnOk");
      this.btnOk.Name = "btnOk";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOk;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.cmbImport);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (CompanySetupAddDialog);
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
    }
  }
}
