// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ImportPackageSelectorPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.Packages;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class ImportPackageSelectorPanel : WizardItemWithHeader
  {
    private Label label1;
    private TextBox txtPackagePath;
    private Button btnBrowse;
    private OpenFileDialog ofdBrowse;
    private Panel panel2;
    private IContainer components;

    public ImportPackageSelectorPanel() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtPackagePath = new TextBox();
      this.btnBrowse = new Button();
      this.ofdBrowse = new OpenFileDialog();
      this.panel2 = new Panel();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.label1.Location = new Point(38, 72);
      this.label1.Name = "label1";
      this.label1.Size = new Size(418, 18);
      this.label1.TabIndex = 10;
      this.label1.Text = "Select the Form Package to be imported:";
      this.txtPackagePath.Location = new Point(40, 92);
      this.txtPackagePath.Name = "txtPackagePath";
      this.txtPackagePath.ReadOnly = true;
      this.txtPackagePath.Size = new Size(318, 20);
      this.txtPackagePath.TabIndex = 11;
      this.txtPackagePath.Text = "";
      this.btnBrowse.BackColor = SystemColors.Control;
      this.btnBrowse.Location = new Point(359, 91);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(67, 20);
      this.btnBrowse.TabIndex = 12;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.ofdBrowse.Filter = "Encompass Package Files (*.empkg)|*.empkg|All Files (*.*)|*.*";
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.btnBrowse);
      this.panel2.Controls.Add((Control) this.txtPackagePath);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 13;
      this.Controls.Add((Control) this.panel2);
      this.Header = "Import Package - Select Package";
      this.Name = nameof (ImportPackageSelectorPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override bool NextEnabled => this.txtPackagePath.Text != "";

    public override WizardItem Next()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        PackageImportWizard.CurrentPackage = new ExportPackage(this.txtPackagePath.Text);
        return (WizardItem) new ImportSummaryPanel((WizardItem) this);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error opening package: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (WizardItem) null;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      if (this.ofdBrowse.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
        return;
      this.txtPackagePath.Text = this.ofdBrowse.FileName;
      this.OnControlsChange();
    }
  }
}
