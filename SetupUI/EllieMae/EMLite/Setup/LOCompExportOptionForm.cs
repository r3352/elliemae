// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOCompExportOptionForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LOCompExportOptionForm : Form
  {
    private IContainer components;
    private RadioButton rdoSortByPlan;
    private RadioButton rdoSortByLO;
    private Button btnCancel;
    private Button btnExport;

    public LOCompExportOptionForm() => this.InitializeComponent();

    public bool SortedByPlan => this.rdoSortByPlan.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rdoSortByPlan = new RadioButton();
      this.rdoSortByLO = new RadioButton();
      this.btnCancel = new Button();
      this.btnExport = new Button();
      this.SuspendLayout();
      this.rdoSortByPlan.AutoSize = true;
      this.rdoSortByPlan.Checked = true;
      this.rdoSortByPlan.Location = new Point(28, 13);
      this.rdoSortByPlan.Name = "rdoSortByPlan";
      this.rdoSortByPlan.Size = new Size(142, 17);
      this.rdoSortByPlan.TabIndex = 0;
      this.rdoSortByPlan.TabStop = true;
      this.rdoSortByPlan.Text = "Export list of comp plans ";
      this.rdoSortByPlan.UseVisualStyleBackColor = true;
      this.rdoSortByLO.AutoSize = true;
      this.rdoSortByLO.Location = new Point(28, 37);
      this.rdoSortByLO.Name = "rdoSortByLO";
      this.rdoSortByLO.Size = new Size(203, 17);
      this.rdoSortByLO.TabIndex = 1;
      this.rdoSortByLO.TabStop = true;
      this.rdoSortByLO.Text = "Export comp plans for each originator ";
      this.rdoSortByLO.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(175, 77);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnExport.DialogResult = DialogResult.OK;
      this.btnExport.Location = new Point(94, 77);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(75, 23);
      this.btnExport.TabIndex = 2;
      this.btnExport.Text = "&Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(267, 111);
      this.Controls.Add((Control) this.btnExport);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoSortByLO);
      this.Controls.Add((Control) this.rdoSortByPlan);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LOCompExportOptionForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "LO Compensation Export";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
