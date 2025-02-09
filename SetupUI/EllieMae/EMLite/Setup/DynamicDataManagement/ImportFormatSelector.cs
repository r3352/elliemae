// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportFormatSelector
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ImportFormatSelector : Form
  {
    private IContainer components;
    private GroupBox gbxSelectImportSource;
    private Button btnContinue;
    private Button btnCancel;
    private RadioButton rbtnImportFromDataTemplate;
    private RadioButton rbtnImportFromCsv;

    public ImportSource ImportSource { get; private set; }

    public ImportFormatSelector() => this.InitializeComponent();

    private void btnContinue_Click(object sender, EventArgs e)
    {
      this.ImportSource = this.GetImportSource();
      if (this.ImportSource == ImportSource.None)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the import source type.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private ImportSource GetImportSource()
    {
      if (this.rbtnImportFromCsv.Checked)
        return ImportSource.Csv;
      return this.rbtnImportFromDataTemplate.Checked ? ImportSource.DataTemplate : ImportSource.None;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportFormatSelector));
      this.gbxSelectImportSource = new GroupBox();
      this.rbtnImportFromDataTemplate = new RadioButton();
      this.rbtnImportFromCsv = new RadioButton();
      this.btnContinue = new Button();
      this.btnCancel = new Button();
      this.gbxSelectImportSource.SuspendLayout();
      this.SuspendLayout();
      this.gbxSelectImportSource.Controls.Add((Control) this.rbtnImportFromDataTemplate);
      this.gbxSelectImportSource.Controls.Add((Control) this.rbtnImportFromCsv);
      this.gbxSelectImportSource.Location = new Point(12, 12);
      this.gbxSelectImportSource.Name = "gbxSelectImportSource";
      this.gbxSelectImportSource.Size = new Size(272, 72);
      this.gbxSelectImportSource.TabIndex = 0;
      this.gbxSelectImportSource.TabStop = false;
      this.gbxSelectImportSource.Text = "Select Import Source";
      this.rbtnImportFromDataTemplate.AutoSize = true;
      this.rbtnImportFromDataTemplate.Location = new Point(7, 44);
      this.rbtnImportFromDataTemplate.Name = "rbtnImportFromDataTemplate";
      this.rbtnImportFromDataTemplate.Size = new Size(144, 17);
      this.rbtnImportFromDataTemplate.TabIndex = 1;
      this.rbtnImportFromDataTemplate.TabStop = true;
      this.rbtnImportFromDataTemplate.Text = "Import from data template";
      this.rbtnImportFromDataTemplate.UseVisualStyleBackColor = true;
      this.rbtnImportFromCsv.AutoSize = true;
      this.rbtnImportFromCsv.Location = new Point(7, 20);
      this.rbtnImportFromCsv.Name = "rbtnImportFromCsv";
      this.rbtnImportFromCsv.Size = new Size(116, 17);
      this.rbtnImportFromCsv.TabIndex = 0;
      this.rbtnImportFromCsv.TabStop = true;
      this.rbtnImportFromCsv.Text = "Import from .csv file";
      this.rbtnImportFromCsv.UseVisualStyleBackColor = true;
      this.btnContinue.Location = new Point(128, 90);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 23);
      this.btnContinue.TabIndex = 1;
      this.btnContinue.Text = "Continue";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.btnCancel.Location = new Point(209, 90);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(296, 120);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.gbxSelectImportSource);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportFormatSelector);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Import";
      this.gbxSelectImportSource.ResumeLayout(false);
      this.gbxSelectImportSource.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
